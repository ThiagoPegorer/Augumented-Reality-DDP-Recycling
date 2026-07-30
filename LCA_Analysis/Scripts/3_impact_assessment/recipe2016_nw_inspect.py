"""
ReCiPe 2016 normalisation/weighting — dump the factors, then rebuild the endpoint single score.

WHY THIS EXISTS
---------------
The 2026-07-28 re-run produced a screening in which **Mineral resource scarcity = 0.0 %** and
the three toxicity categories take ~90 % of the normalised profile. That is either (a) genuine
ReCiPe behaviour — its World-2010 normalisation reference for mineral resources is enormous
while its ecotoxicity references are tiny — or (b) a factor-matching bug in our code. A number
that decides the thesis narrative may not rest on "probably (a)". So this script PRINTS EVERY
FACTOR it uses, next to the raw result, so the arithmetic can be checked by hand against the
published ReCiPe 2016 report.

It also fixes two things the re-run left open:
  1. NW-set choice was arbitrary. The endpoint pack offers 'World (2010) H/H' and
     'World (2010) H/A'; the re-run silently took the first. H/A (Hierarchist perspective,
     Average weighting) is the direct successor of the old ecoinvent '(H,A)' endpoint, so it is
     the continuity-preserving choice. BOTH are computed here and reported side by side.
  2. The single score disappeared. ReCiPe 2016 Endpoint reports DALY / species.yr / USD2013,
     not points, so 'impact_ReCiPe_end.csv' has no 'total - total' row — the old
     11.25 -> 8.20 points narrative has no 2016 equivalent until the NW set is applied.
     This script applies it and writes the points table, incl. the three area-of-protection
     totals and the single score.

NO RECALCULATION. It reads the CSVs the re-run already wrote plus the NW factors from the
database, so it takes seconds, not minutes. IPC on 8080 required (factors live in the DB).

OUTPUT (Outputs\3_impact_assessment\)
  recipe2016_nw_factors.txt          every normalisation + weighting factor, per method
  impact_ReCiPe_end_points.csv       endpoint in points: per category, per AoP, single score
                                     (H/A basis; H/H reported in the log for comparison)
  impact_screening_ReCiPe_end.csv    rebuilt on the chosen NW set, with AoP grouping

Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\recipe2016_nw_inspect.py"
"""
import csv, pathlib, re, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "recipe2016_nw_factors.txt"; _l = []
def log(m):
    print(m); _l.append(str(m))
    try: TXT.write_text("\n".join(_l), encoding="utf-8")
    except OSError: pass

SCEN_COLS = ["sc1", "sc2_net", "sc3_net", "sc4_net"]
PREFERRED_END_NW = "world (2010) h/a"       # successor of the old ecoinvent (H,A)
AOP_BY_UNIT = {"DALY": "human health", "species.yr": "ecosystem quality",
               "USD2013": "resources"}

client = ipc.Client(8080)
def _n(s): return re.sub(r"\s+", " ", (s or "")).strip().lower()

def resolve_method(target):
    ds = client.get_descriptors(o.ImpactMethod)
    hit = [d for d in ds if _n(d.name) == _n(target)] or [d for d in ds if _n(target) in _n(d.name)]
    if hit: hit.sort(key=lambda d: len(d.name or "")); return hit[0]
    return None

def nw_factors(nwref):
    f = getattr(nwref, "factors", None)
    if f: return f
    try: return getattr(client.get(o.NwSet, uid=nwref.id), "factors", None) or []
    except Exception: return []

def read_csv(name):
    p = HERE / name
    if not p.exists(): log(f"!! {name} not found."); return None
    return list(csv.DictReader(p.open(encoding="utf-8")))

def dump(method_name, rows, valcol="sc1"):
    """Print every factor beside the raw value and the resulting person-equivalent."""
    m = resolve_method(method_name)
    if m is None: log(f"!! method '{method_name}' not found."); return None, {}
    full = client.get(o.ImpactMethod, uid=m.id)
    raw = {r["category"]: (float(r[valcol]), r["unit"]) for r in rows} if rows else {}
    sets = {}
    log(f"\n{'='*78}\n{m.name}\n{'='*78}")
    for nwref in (full.nw_sets or []):
        fl = nw_factors(nwref)
        log(f"\n  NW set '{nwref.name}'  ({len(fl)} factors)")
        log(f"    {'category':<46}{'raw (Sc1)':>13} {'norm.factor':>13} {'weight':>8} {'-> PE·w':>12}")
        table = {}
        for fct in sorted(fl, key=lambda x: _n(getattr(x.impact_category, 'name', ''))):
            cname = getattr(fct.impact_category, "name", "?")
            nf = getattr(fct, "normalisation_factor", None)
            wf = getattr(fct, "weighting_factor", None)
            rv, un = raw.get(cname, (None, ""))
            pe = (rv / nf * (wf or 1.0)) if (rv is not None and nf) else None
            table[cname] = (nf, wf)
            log(f"    {cname:<46}"
                f"{('%.4g' % rv) if rv is not None else '—':>13} "
                f"{('%.4g' % nf) if nf else '—':>13} "
                f"{('%.4g' % wf) if wf else '—':>8} "
                f"{('%.4g' % pe) if pe is not None else '—':>12}   {un}")
        sets[nwref.name] = table
    return m, sets

def main():
    log("ReCiPe 2016 NW-factor audit — verify before any chart is built.\n")
    log("Read the 'norm.factor' column against the published ReCiPe 2016 World-2010 table.")
    log("If Mineral resource scarcity carries a normalisation factor of order 1e5 kg Cu-eq")
    log("while the ecotoxicity categories carry order 1e1-1e2 kg 1,4-DCB, then the 0.0 %")
    log("share is REAL ReCiPe behaviour (a property of the normalisation references), not a")
    log("bug in our code — and the conclusion is that ReCiPe normalisation must not be used")
    log("to prioritise categories in this study.\n")

    mid_rows = read_csv("impact_ReCiPe_mid.csv")
    end_rows = read_csv("impact_ReCiPe_end.csv")
    dump("ReCiPe 2016 Midpoint (H)", mid_rows)
    _, end_sets = dump("ReCiPe 2016 Endpoint (H)", end_rows)
    if not end_rows or not end_sets:
        log("\n!! endpoint data or NW sets missing - single score not rebuilt."); return

    # ---- rebuild the endpoint single score in points, for every scenario column ----
    chosen = None
    for name in end_sets:
        if _n(name) == PREFERRED_END_NW: chosen = name; break
    if chosen is None:
        chosen = sorted(end_sets)[0]
        log(f"\n!! '{PREFERRED_END_NW}' not present; falling back to '{chosen}'.")
    log(f"\n{'='*78}\nENDPOINT SINGLE SCORE — NW set '{chosen}' (H/A = successor of the old (H,A))\n{'='*78}")

    for setname, table in end_sets.items():
        tot = {}
        for r in end_rows:
            nf, wf = table.get(r["category"], (None, None))
            if not nf: continue
            for col in SCEN_COLS:
                tot[col] = tot.get(col, 0.0) + float(r[col]) / nf * (wf or 1.0)
        mark = "  <-- used" if setname == chosen else "  (comparison only)"
        log(f"  {setname:<22} single score  " +
            " / ".join(f"{tot.get(c, 0):.4g}" for c in SCEN_COLS) + f" pt{mark}")
        if setname == chosen and tot.get("sc1"):
            s1 = tot["sc1"]
            log(f"    reductions vs Sc1: " +
                " / ".join(f"{100*(tot[c]-s1)/s1:+.1f} %" for c in SCEN_COLS[1:]))

    table = end_sets[chosen]
    out, aop = [], {}
    for r in end_rows:
        nf, wf = table.get(r["category"], (None, None))
        if not nf:
            log(f"    (no factor, excluded from points: {r['category']})"); continue
        pts = {c: float(r[c]) / nf * (wf or 1.0) for c in SCEN_COLS}
        a = AOP_BY_UNIT.get(r["unit"].strip(), "unassigned")
        for c in SCEN_COLS: aop.setdefault(a, {}).setdefault(c, 0.0); aop[a][c] += pts[c]
        out.append([r["category"], a, r["unit"]] + [pts[c] for c in SCEN_COLS])
    grand = {c: sum(v[c] for v in aop.values()) for c in SCEN_COLS}

    with (HERE / "impact_ReCiPe_end_points.csv").open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "area_of_protection", "source_unit",
                    "sc1_pt", "sc2_net_pt", "sc3_net_pt", "sc4_net_pt", "nw_set"])
        for row in sorted(out, key=lambda x: (x[1], -abs(x[3]))):
            w.writerow(row + [chosen])
        for a, v in sorted(aop.items()):
            w.writerow([f"{a} - total", a, "points"] + [v[c] for c in SCEN_COLS] + [chosen])
        w.writerow(["total - total", "all", "points"] + [grand[c] for c in SCEN_COLS] + [chosen])
    log(f"\n  -> impact_ReCiPe_end_points.csv (single score {grand['sc1']:.4g} -> "
        + " / ".join(f"{grand[c]:.4g}" for c in SCEN_COLS[1:]) + " pt)")
    log("  area-of-protection split (Sc1): " +
        " · ".join(f"{a} {v['sc1']:.4g}" for a, v in sorted(aop.items())))

    # ---- endpoint screening rebuilt on the chosen set, WITH the AoP column ----
    pairs = sorted(((r[0], r[1], r[3]) for r in out), key=lambda x: -abs(x[2]))
    tot = sum(abs(p[2]) for p in pairs) or 1.0
    cum, crossed, srows = 0.0, False, []
    log("\n  screening on the single score (Sc1 basis):")
    for cat, a, val in pairs:
        share = 100.0 * abs(val) / tot; cum += share
        in_set = not crossed
        if cum >= 80.0: crossed = True
        pin = any(p in cat.lower() for p in ("mineral resource", "global warming"))
        srows.append((cat, val, share, cum, in_set, pin, f"points, NW set '{chosen}'", a))
        log(f"    {cat:<46} {share:5.1f}%  cum {cum:5.1f}%"
            f"{'  << reporting set' if in_set else ''}{'  [PINNED]' if pin else ''}")
    with (HERE / "impact_screening_ReCiPe_end.csv").open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "weighted_pe", "share_pct", "cum_pct",
                    "in_reporting_set", "goal_pinned", "basis", "area_of_protection"])
        w.writerows(srows)
    log("  -> impact_screening_ReCiPe_end.csv (rebuilt)")
    log("\nDONE. Send recipe2016_nw_factors.txt.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        try: TXT.write_text("\n".join(_l), encoding="utf-8")
        except OSError: pass
        print(f"\nLog: {TXT}")
