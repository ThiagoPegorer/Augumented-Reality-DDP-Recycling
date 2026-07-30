"""
Impact runs (step 3) - calculates ALL SEVEN product systems under THREE methods
(EF 3.1 adapted · ReCiPe 2016 Midpoint (H) · ReCiPe 2016 Endpoint (H,A)), assembles the
gross | saving | net tables, and performs the EF normalization+weighting screening
(PEF >=80 % cumulative rule + goal-pinned categories) on the Sc1 baseline.
READS ONLY. Runtime: 21 calculations - expect 15-40 min; leave it running.
Outputs: Outputs\impact_EF31.csv, impact_ReCiPe_mid.csv, impact_ReCiPe_end.csv,
         impact_screening.csv, impact_runs.txt (summary).
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\impact_runs.py"
"""
import csv, pathlib, re, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "impact_runs.txt"; _l = []
def log(m): print(m); _l.append(str(m))

SYSTEMS = [
    ("Sc1",  "VCU S5 EoL Sc1 (no recycling)"),
    ("Sc2g", "VCU S5 EoL Sc2 (bulk recycling)"),
    ("Sc2s", "VCU S5 EoL Sc2 credits (avoided virgin)"),
    ("Sc3g", "VCU S5 EoL Sc3 (guided disassembly)"),
    ("Sc3s", "VCU S5 EoL Sc3 credits (avoided virgin)"),
    ("Sc4g", "VCU S5 EoL Sc4 (disassembly + reuse)"),
    ("Sc4s", "VCU S5 EoL Sc4 credits (avoided virgin + components)"),
]
METHODS = [  # (key, search substrings in priority order, csv filename)
    ("EF31", ["EF 3.1 Method (adapted)", "EF 3.1"], "impact_EF31.csv"),
    ("RMid", ["ei - ReCiPe Midpoint (H)"], "impact_ReCiPe_mid.csv"),   # ecoinvent LCIA pack (user-selected)
    ("REnd", ["ei - ReCiPe Endpoint (H,A)"], "impact_ReCiPe_end.csv"), # ecoinvent LCIA pack (user-selected)
]
PINNED = ("climate change", "minerals and metals")   # goal-pinned EF categories

client = ipc.Client(8080)

def _norm(s):  # ei-pack names carry double spaces ('ei -  ReCiPe ...') -> normalize
    return re.sub(r"\s+", " ", (s or "")).strip().lower()

def resolve_method(terms):
    ds = client.get_descriptors(o.ImpactMethod)
    for t in terms:
        exact = [d for d in ds if _norm(d.name) == _norm(t)]
        if exact: return exact[0]
    for t in terms:
        subs = [d for d in ds if _norm(t) in _norm(d.name)]
        if subs:
            subs.sort(key=lambda d: len(d.name or ""))
            return subs[0]
    return None

def resolve_system(name):
    for d in client.get_descriptors(o.ProductSystem):
        if (d.name or "").split("|")[0].strip() == name: return d
    return None

def calc(sys_ref, m_ref):
    setup = o.CalculationSetup(
        target=o.Ref(ref_type=o.RefType.ProductSystem, id=sys_ref.id, name=sys_ref.name),
        impact_method=o.Ref(ref_type=o.RefType.ImpactMethod, id=m_ref.id, name=m_ref.name))
    result = client.calculate(setup)
    result.wait_until_ready()
    vals, units = {}, {}
    for i in result.get_total_impacts():
        cat = i.impact_category
        n = getattr(cat, "name", "?")
        vals[n] = i.amount
        units[n] = getattr(cat, "ref_unit", None) or getattr(cat, "refUnit", "") or ""
    result.dispose()
    return vals, units

def main():
    systems = {}
    for key, name in SYSTEMS:
        d = resolve_system(name)
        if d is None: log(f"!! product system '{name}' not found - ABORT"); return
        systems[key] = d
    log("All 7 product systems resolved.")

    for mkey, terms, csvname in METHODS:
        m = resolve_method(terms)
        if m is None:
            log(f"!! method not found for {terms} - SKIPPED. Check LCIA pack names:")
            for d in client.get_descriptors(o.ImpactMethod)[:40]: log(f"    - {d.name}")
            continue
        log(f"\n=== {mkey}: '{m.name}' ===")
        R, units = {}, {}
        for skey, _ in SYSTEMS:
            log(f"  calculating {skey} ...")
            R[skey], u = calc(systems[skey], m); units.update(u)
        cats = sorted(set().union(*[set(v) for v in R.values()]), key=str.lower)
        rows = []
        for c in cats:
            s1 = R["Sc1"].get(c, 0.0)
            row = [c, s1]
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
        for c, s1, g2, v2, n2, g3, v3, n3, g4, v4, n4, un in rows:
            low = c.lower()
            if any(k in low for k in ("climate change", "minerals and metals",
                                      "human health", "ecosystem", "resource")) and "(" not in c:
                log(f"    {c:<48} Sc1 {s1:.4g} | net {n2:.4g} / {n3:.4g} / {n4:.4g} {un}")

        # ---- EF screening on the Sc1 baseline ----
        if mkey == "EF31":
            try:
                full = client.get(o.ImpactMethod, uid=m.id)
                nw = None
                for cand in (full.nw_sets or []):
                    nw = cand
                    if "EF" in (cand.name or "") or "3.1" in (cand.name or ""): break
                if nw is None: raise RuntimeError("no NW set on the EF method")
                nwfull = client.get(o.NwSet, uid=nw.id) if not hasattr(nw, "factors") else nw
                factors = getattr(nwfull, "factors", None) or []
                if not factors: raise RuntimeError(f"NW set '{nw.name}' has no factors via IPC")
                weighted = []
                for fct in factors:
                    cname = getattr(fct.impact_category, "name", "?")
                    nf = getattr(fct, "normalisation_factor", None)
                    wf = getattr(fct, "weighting_factor", None) or 1.0
                    if not nf: continue
                    val = R["Sc1"].get(cname, 0.0) / nf * wf
                    weighted.append((cname, val))
                tot = sum(v for _, v in weighted) or 1.0
                weighted.sort(key=lambda x: -abs(x[1]))
                log(f"\n  EF screening (NW set '{nw.name}', Sc1 baseline, PEF >=80% rule):")
                cum = 0.0; crossed = False
                srows = []
                for cname, val in weighted:
                    share = 100.0 * abs(val) / tot
                    cum += share
                    in_set = not crossed        # PEF rule: include the category that CROSSES 80%
                    if cum >= 80.0: crossed = True
                    pin = " [PINNED]" if any(p in cname.lower() for p in PINNED) else ""
                    log(f"    {cname:<48} {share:5.1f}%  cum {cum:5.1f}%"
                        f"{'  << reporting set' if in_set else ''}{pin}")
                    srows.append((cname, val, share, cum, in_set,
                                  any(p in cname.lower() for p in PINNED)))
                with (HERE / "impact_screening.csv").open("w", newline="", encoding="utf-8") as f:
                    w = csv.writer(f)
                    w.writerow(["category", "weighted_pe", "share_pct", "cum_pct",
                                "in_80pct_set", "goal_pinned"])
                    w.writerows(srows)
                log("  -> impact_screening.csv (reporting set = >=80% cumulative + pinned; all 16 still shown)")
            except Exception as ex:
                log(f"  !! screening via IPC failed ({ex}).")
                log("     If 'Permission denied': close Excel / any viewer holding Outputs\\*.csv and re-run.")
                log("     GUI fallback: open the EF 3.1 result in openLCA -> tab 'Impact analysis'")
                log("     -> enable normalization & weighting -> export; the >=80% ranking can")
                log("     then be read directly. Raw totals in impact_EF31.csv are unaffected.")

    log("\nDONE - three methods across seven systems. Send impact_runs.txt + CSVs.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: TXT.write_text("\n".join(_l), encoding="utf-8"); print(f"\nSummary: {TXT}")
