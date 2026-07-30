"""
Re-run the ReCiPe cross-check on ReCiPe 2016 (openLCA LCIA pack) and build its screenings.

WHY
---
The 'ei - ReCiPe Midpoint (H)' / 'ei - ReCiPe Endpoint (H,A)' packs shipped with ecoinvent are
ReCiPe **2008**, not 2016 — proven by their own category names and units (metal depletion in
kg Fe-eq, ALOP/ULOP land occupation split, HTPinf lumped human toxicity, WDP water depletion,
and the 2008-style '(H,A)' perspective+weighting-set notation). They also carry NO
normalisation set, so no midpoint screening is possible from them.

This script switches the ReCiPe cross-check to the openLCA pack's **ReCiPe 2016 Midpoint (H)**
and **ReCiPe 2016 Endpoint (H)**, which are the current generation and ship normalisation /
weighting sets.

WHAT IT WRITES  (Outputs\3_impact_assessment\ — canonical names are OVERWRITTEN in place, so
no superseded twin is left behind; the stale-file trap of 2026-07-27 must not be repeated)
  impact_ReCiPe_mid.csv             2016 midpoint, gross | saving | net per scenario
  impact_ReCiPe_end.csv             2016 endpoint, same layout
  impact_screening_ReCiPe_mid.csv   midpoint screening (normalised; weighted only if the pack
                                    supplies weighting factors — ReCiPe usually does not at
                                    midpoint, in which case the ranking is normalisation-only)
  impact_screening_ReCiPe_end.csv   endpoint screening (share of the single score)
  recipe2016_rerun.txt              full diagnostic log

The script is DIAGNOSTIC-FIRST: before any screening it prints each method's category list,
units and NW sets, so the log alone proves which method produced which number. Where the
structure is ambiguous it logs and SKIPS rather than guessing (framework §0).

Runtime: 14 calculations (7 product systems × 2 methods) — expect ~10–25 min. READS ONLY from
the database; nothing is written back to openLCA.

Prereqs: openLCA open on the AR_DPP database + IPC server running on 8080.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\recipe2016_rerun.py"
"""
import csv, pathlib, re, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "recipe2016_rerun.txt"; _l = []
def log(m):
    print(m); _l.append(str(m))
    try: TXT.write_text("\n".join(_l), encoding="utf-8")
    except OSError: pass

SYSTEMS = [
    ("Sc1",  "VCU S5 EoL Sc1 (no recycling)"),
    ("Sc2g", "VCU S5 EoL Sc2 (bulk recycling)"),
    ("Sc2s", "VCU S5 EoL Sc2 credits (avoided virgin)"),
    ("Sc3g", "VCU S5 EoL Sc3 (guided disassembly)"),
    ("Sc3s", "VCU S5 EoL Sc3 credits (avoided virgin)"),
    ("Sc4g", "VCU S5 EoL Sc4 (disassembly + reuse)"),
    ("Sc4s", "VCU S5 EoL Sc4 credits (avoided virgin + components)"),
]
METHODS = [   # (key, exact target name, results csv, screening csv)
    ("RMid2016", "ReCiPe 2016 Midpoint (H)", "impact_ReCiPe_mid.csv", "impact_screening_ReCiPe_mid.csv"),
    ("REnd2016", "ReCiPe 2016 Endpoint (H)", "impact_ReCiPe_end.csv", "impact_screening_ReCiPe_end.csv"),
]
BASELINE = "Sc1"
THRESHOLD = 80.0
PINS = ("mineral resource", "global warming", "climate change", "metal depletion")

client = ipc.Client(8080)
def _n(s): return re.sub(r"\s+", " ", (s or "")).strip().lower()

def resolve_method(target):
    ds = client.get_descriptors(o.ImpactMethod)
    exact = [d for d in ds if _n(d.name) == _n(target)]
    if exact: return exact[0]
    subs = [d for d in ds if _n(target) in _n(d.name)]
    if subs:
        subs.sort(key=lambda d: len(d.name or "")); return subs[0]
    log(f"  !! '{target}' not found. Methods containing 'recipe':")
    for d in ds:
        if "recipe" in _n(d.name): log(f"       - {d.name}")
    return None

def resolve_system(name):
    for d in client.get_descriptors(o.ProductSystem):
        if (d.name or "").split("|")[0].strip() == name: return d
    return None

def calc(sys_ref, m_ref):
    setup = o.CalculationSetup(
        target=o.Ref(ref_type=o.RefType.ProductSystem, id=sys_ref.id, name=sys_ref.name),
        impact_method=o.Ref(ref_type=o.RefType.ImpactMethod, id=m_ref.id, name=m_ref.name))
    r = client.calculate(setup); r.wait_until_ready()
    vals, units = {}, {}
    for i in r.get_total_impacts():
        c = i.impact_category
        n = getattr(c, "name", "?")
        vals[n] = i.amount
        units[n] = getattr(c, "ref_unit", None) or getattr(c, "refUnit", "") or ""
    r.dispose()
    return vals, units

def describe_method(m):
    """Log the method's structure BEFORE trusting any screening built on it."""
    full = client.get(o.ImpactMethod, uid=m.id)
    nws = full.nw_sets or []
    log(f"  NW sets: {[n.name for n in nws] if nws else 'NONE'}")
    chosen, factors, has_w = None, [], False
    for cand in nws:
        f = getattr(cand, "factors", None)
        if not f:
            try: f = getattr(client.get(o.NwSet, uid=cand.id), "factors", None) or []
            except Exception: f = []
        w = any(getattr(x, "weighting_factor", None) for x in f)
        log(f"    - '{cand.name}': {len(f)} factors, weighting {'PRESENT' if w else 'absent'}")
        if f and chosen is None:
            chosen, factors, has_w = cand, f, w
    return chosen, factors, has_w

def write_results(csvname, R, units):
    cats = sorted(set().union(*[set(v) for v in R.values()]), key=str.lower)
    rows = []
    for c in cats:
        row = [c, R[BASELINE].get(c, 0.0)]
        for sc in ("Sc2", "Sc3", "Sc4"):
            g = R[sc + "g"].get(c, 0.0); s = R[sc + "s"].get(c, 0.0)
            row += [g, s, g - s]
        row.append(units.get(c, ""))
        rows.append(row)
    with (HERE / csvname).open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "sc1", "sc2_gross", "sc2_saving", "sc2_net",
                    "sc3_gross", "sc3_saving", "sc3_net",
                    "sc4_gross", "sc4_saving", "sc4_net", "unit"])
        w.writerows(rows)
    log(f"  -> {csvname} ({len(rows)} categories)")
    return rows

def write_screening(csvname, pairs, basis):
    pairs = sorted(pairs, key=lambda r: -abs(r[1]))
    tot = sum(abs(v) for _, v in pairs) or 1.0
    out, cum, crossed = [], 0.0, False
    for cat, val in pairs:
        share = 100.0 * abs(val) / tot
        cum += share
        in_set = not crossed
        if cum >= THRESHOLD: crossed = True
        pin = any(p in cat.lower() for p in PINS)
        out.append((cat, val, share, cum, in_set, pin, basis))
        log(f"    {cat:<52} {share:5.1f}%  cum {cum:5.1f}%"
            f"{'  << reporting set' if in_set else ''}{'  [PINNED]' if pin else ''}")
    with (HERE / csvname).open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "weighted_pe", "share_pct", "cum_pct",
                    "in_reporting_set", "goal_pinned", "basis"])
        w.writerows(out)
    log(f"  -> {csvname} ({len(out)} categories)")

def screen(mkey, csvname, R, units, nwfactors, has_w):
    base = R[BASELINE]
    leaves = {c: v for c, v in base.items() if not _n(c).endswith("total") and _n(c) != "total"}
    dropped = [c for c in base if c not in leaves]
    if dropped: log(f"  aggregate/total rows excluded from the shares: {dropped}")
    ulist = sorted({units.get(c, "") for c in leaves})
    log(f"  distinct units across leaf categories: {ulist}")

    if len(ulist) == 1:
        # one common unit (points) -> the method already normalised+weighted; shares are valid
        log(f"  single common unit '{ulist[0]}' -> categories are directly additive; "
            f"screening = share of the aggregated score.")
        write_screening(csvname, list(leaves.items()),
                        f"{ulist[0]} (aggregated directly; method-supplied weighting)")
        return

    # mixed units -> normalisation (and weighting, if supplied) is required
    if not nwfactors:
        log("  !! categories are in MIXED units and the method carries no usable NW set.")
        log("     Screening SKIPPED (summing DALY + species·yr + USD would be meaningless).")
        log("     The results CSV is still valid and complete.")
        return
    vals, missing = [], []
    for fct in nwfactors:
        cname = getattr(fct.impact_category, "name", "?")
        if cname not in leaves: continue
        nf = getattr(fct, "normalisation_factor", None)
        if not nf: missing.append(cname); continue
        wf = getattr(fct, "weighting_factor", None) or 1.0
        vals.append((cname, leaves[cname] / nf * wf))
    uncovered = [c for c in leaves if c not in {v[0] for v in vals}]
    if missing:   log(f"  no normalisation factor (excluded): {missing}")
    if uncovered: log(f"  not covered by the NW set (excluded): {uncovered}")
    if not vals:
        log("  !! nothing could be normalised - screening skipped."); return
    basis = ("normalised + weighted (person-equivalents × weighting)" if has_w
             else "person-equivalents, NORMALISED ONLY (no weighting factors in the set)")
    log(f"  ranking by {basis}:")
    write_screening(csvname, vals, basis)
    if not has_w:
        log("  NOTE: normalised, not weighted -> cite the cumulative column as 'cumulative")
        log("        share of normalised impact', never as 'weighted footprint'.")

def main():
    log("ReCiPe 2016 re-run — replacing the ecoinvent ReCiPe 2008 cross-check")
    log("Canonical CSVs are overwritten in place (no superseded twins).\n")

    systems = {}
    for k, name in SYSTEMS:
        d = resolve_system(name)
        if d is None: log(f"!! product system '{name}' not found - ABORT"); return
        systems[k] = d
    log(f"All {len(systems)} product systems resolved.")

    for mkey, target, rescsv, scrcsv in METHODS:
      try:
        log(f"\n=== {mkey}: looking for '{target}' ===")
        m = resolve_method(target)
        if m is None:
            log(f"  SKIPPED — canonical {rescsv} left untouched (still the ReCiPe 2008 data).")
            continue
        log(f"  method: '{m.name}'")
        nwset, factors, has_w = describe_method(m)

        R, units = {}, {}
        for k, _ in SYSTEMS:
            log(f"  calculating {k} ...")
            R[k], u = calc(systems[k], m); units.update(u)

        rows = write_results(rescsv, R, units)

        # Everything below is reporting, not computation. A formatting slip here must NEVER
        # discard finished calculations again (it did on the 2026-07-28 run), so each block
        # is fault-isolated.
        try:
            log(f"  {BASELINE} values (sanity check against the old 2008 numbers):")
            # row layout: 0 cat | 1 sc1 | 2,3,4 sc2 g/s/net | 5,6,7 sc3 | 8,9,10 sc4 | 11 unit
            for row in rows:
                c, s1 = row[0], float(row[1])
                n2, n3, n4, un = float(row[4]), float(row[7]), float(row[10]), row[11]
                if s1 == 0: continue
                log(f"    {c:<50} {s1:.4g} -> net {n2:.4g} / {n3:.4g} / {n4:.4g} {un}"
                    f"   (Sc4 {100*(n4-s1)/abs(s1):+.1f} %)")
        except Exception as ex:
            log(f"  (sanity-check printout failed: {ex} — results CSV is unaffected)")

        try:
            log(f"  screening ({BASELINE} basis):")
            screen(mkey, scrcsv, R, units, factors, has_w)
        except Exception as ex:
            log(f"  !! screening failed: {ex}")
            log(f"     {rescsv} is written and valid; only {scrcsv} is missing.")
      except Exception:
        # one method failing must not cost the other its 7 calculations
        log(f"  !! {mkey} aborted:\n{traceback.format_exc()}")
        log(f"     continuing with the next method.")

    log("\nDONE. Send recipe2016_rerun.txt + the four CSVs.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        try: TXT.write_text("\n".join(_l), encoding="utf-8")
        except OSError: pass
        print(f"\nLog: {TXT}")
