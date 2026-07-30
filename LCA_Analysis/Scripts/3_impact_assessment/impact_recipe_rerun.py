"""
Rerun (step 3b) - the TWO ecoinvent-pack ReCiPe methods only, per Thiago's selection:
  'ei - ReCiPe Midpoint (H)'  and  'ei - ReCiPe Endpoint (H,A)'
(the earlier RMid run used openLCA's own ReCiPe pack twin - superseded).
Also REGENERATES impact_screening.csv with the corrected PEF rule: the reporting set is the
smallest set reaching AT LEAST 80 % cumulative (the crossing category is INCLUDED).
READS ONLY. 14 calculations (~10-25 min).
Outputs: impact_ReCiPe_mid.csv (overwritten), impact_ReCiPe_end.csv, impact_screening.csv
(overwritten), impact_recipe_rerun.txt.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\impact_recipe_rerun.py"
"""
import csv, pathlib, re, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "impact_recipe_rerun.txt"; _l = []
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
METHODS = [
    ("RMid", "ei - recipe midpoint (h)",   "impact_ReCiPe_mid.csv"),
    ("REnd", "ei - recipe endpoint (h,a)", "impact_ReCiPe_end.csv"),
]
PINNED = ("climate change", "minerals and metals")

client = ipc.Client(8080)

def norm(s): return re.sub(r"\s+", " ", (s or "")).strip().lower()

def resolve_method(target):
    ds = client.get_descriptors(o.ImpactMethod)
    exact = [d for d in ds if norm(d.name) == target]
    if exact: return exact[0]
    # tolerate 'ReCiPe 2016' vs 'ReCiPe' naming inside the ei pack
    loose = [d for d in ds if norm(d.name).startswith("ei -") and
             target.replace("ei - ", "").replace("recipe", "") in
             norm(d.name).replace("recipe 2016", "").replace("recipe", "")]
    if len(loose) == 1: return loose[0]
    cands = [d for d in ds if "ei -" in norm(d.name) and "recipe" in norm(d.name)]
    log(f"  !! no exact match for '{target}'; ei-ReCiPe candidates:")
    for d in cands: log(f"       - {d.name}")
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

    for mkey, target, csvname in METHODS:
        m = resolve_method(target)
        if m is None: log(f"!! {mkey}: unresolved - SKIPPED"); continue
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
            if mkey == "REnd" or any(k in c.lower() for k in
                                     ("resource", "global warming", "mineral")):
                log(f"    {c:<52} Sc1 {s1:.4g} | net {n2:.4g} / {n3:.4g} / {n4:.4g} {un}")

    # ---- corrected EF screening (recomputed from the existing EF CSV; no recalculation) ----
    try:
        ef = {}
        with (HERE / "impact_EF31.csv").open(encoding="utf-8") as f:
            for r in csv.DictReader(f):
                ef[r["category"]] = float(r["sc1"])
        mEF = None
        for d in client.get_descriptors(o.ImpactMethod):
            if norm(d.name) == "ef 3.1 method (adapted)": mEF = d; break
        full = client.get(o.ImpactMethod, uid=mEF.id)
        nw = (full.nw_sets or [None])[0]
        nwfull = client.get(o.NwSet, uid=nw.id) if not hasattr(nw, "factors") else nw
        factors = getattr(nwfull, "factors", None) or []
        weighted = []
        for fct in factors:
            cname = getattr(fct.impact_category, "name", "?")
            nf = getattr(fct, "normalisation_factor", None)
            wf = getattr(fct, "weighting_factor", None) or 1.0
            if not nf or cname not in ef: continue
            weighted.append((cname, ef[cname] / nf * wf))
        tot = sum(v for _, v in weighted) or 1.0
        weighted.sort(key=lambda x: -abs(x[1]))
        log("\nCorrected EF screening (smallest set reaching >=80 %; crossing category INCLUDED):")
        cum = 0.0; crossed = False; srows = []
        for cname, val in weighted:
            share = 100.0 * abs(val) / tot
            cum += share
            in_set = not crossed          # include every category until the threshold is crossed
            if cum >= 80.0: crossed = True
            pin = any(p in cname.lower() for p in PINNED)
            log(f"    {cname:<48} {share:5.1f}%  cum {cum:5.1f}%"
                f"{'  << reporting set' if in_set else ''}{'  [PINNED]' if pin else ''}")
            srows.append((cname, val, share, cum, in_set, pin))
        with (HERE / "impact_screening.csv").open("w", newline="", encoding="utf-8") as f:
            w = csv.writer(f)
            w.writerow(["category", "weighted_pe", "share_pct", "cum_pct",
                        "in_reporting_set", "goal_pinned"])
            w.writerows(srows)
        log("  -> impact_screening.csv regenerated (corrected rule)")
    except Exception as ex:
        log(f"!! screening regeneration failed ({ex}) - EF totals unaffected")

    log("\nDONE.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: TXT.write_text("\n".join(_l), encoding="utf-8"); print(f"\nSummary: {TXT}")
