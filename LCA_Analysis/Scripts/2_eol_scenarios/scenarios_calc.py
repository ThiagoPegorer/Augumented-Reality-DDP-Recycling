"""
Scenario comparison Sc1 / Sc2 / Sc3 - EF 3.1, cradle-to-grave. READS ONLY.
Supersedes sc1_sc2_calc.py (adds Sc3; fixes the spotlight percentage wording).
Five targets: Sc1 | Sc2 gross | Sc2 credits | Sc3 gross | Sc3 credits.
Writes Outputs\scenarios_results.txt + scenarios_results.csv.
Prereqs: DB open, IPC 8080, Sc1-Sc3 built. Runtime: 5 full calcs - be patient.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\scenarios_calc.py"
"""
import csv, pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "scenarios_results.txt"; CSV = HERE / "scenarios_results.csv"; _l = []
def log(m): print(m); _l.append(str(m))

METHOD = "EF 3.1 Method (adapted)"
TARGETS = [
    ("Sc1",     "VCU S5 EoL Sc1 (no recycling)"),
    ("Sc2g",    "VCU S5 EoL Sc2 (bulk recycling)"),
    ("Sc2s",    "VCU S5 EoL Sc2 credits (avoided virgin)"),
    ("Sc3g",    "VCU S5 EoL Sc3 (guided disassembly)"),
    ("Sc3s",    "VCU S5 EoL Sc3 credits (avoided virgin)"),
    ("Sc4g",    "VCU S5 EoL Sc4 (disassembly + reuse)"),
    ("Sc4s",    "VCU S5 EoL Sc4 credits (avoided virgin + components)"),
]
SPOTLIGHT = ("climate change", "minerals and metals", "resource use, fossils")

client = ipc.Client(8080)

def calc(proc_name, method):
    proc = client.get(o.Process, name=proc_name)
    if proc is None:
        log(f"ERROR: process '{proc_name}' not found."); return None
    setup = o.CalculationSetup(
        target=o.Ref(ref_type=o.RefType.Process, id=proc.id, name=proc.name),
        impact_method=o.Ref(ref_type=o.RefType.ImpactMethod, id=method.id, name=method.name),
    )
    log(f"Calculating: {proc_name} ...")
    result = client.calculate(setup)
    result.wait_until_ready()
    vals, units = {}, {}
    for i in result.get_total_impacts():
        cat = i.impact_category
        name = getattr(cat, "name", "?")
        vals[name] = i.amount
        units[name] = getattr(cat, "ref_unit", None) or getattr(cat, "refUnit", "") or ""
    result.dispose()
    return vals, units

def main():
    method = client.get(o.ImpactMethod, name=METHOD)
    if method is None:
        log(f"ERROR: impact method '{METHOD}' not found."); return

    R, units = {}, {}
    for key, name in TARGETS:
        r = calc(name, method)
        if r is None: return
        R[key], u = r
        units.update(u)

    cats = sorted(set().union(*[set(v) for v in R.values()]), key=str.lower)
    log(f"\n=== Sc1-Sc4, EF 3.1, 1 VCU cradle-to-grave ({len(cats)} categories) ===\n")
    hdr = (f"{'Category':<44} {'Sc1':>10} {'Sc2 net':>10} {'Sc3 net':>10} {'Sc4 net':>10} "
           f"{'Sc2 sv':>10} {'Sc3 sv':>10} {'Sc4 sv':>10}  unit")
    log(hdr); log("-" * len(hdr))
    rows = []
    for c in cats:
        s1 = R["Sc1"].get(c, 0.0)
        nets, svs, gs = {}, {}, {}
        for k in ("Sc2", "Sc3", "Sc4"):
            gs[k] = R[k + "g"].get(c, 0.0)
            svs[k] = R[k + "s"].get(c, 0.0)
            nets[k] = gs[k] - svs[k]
        un = units.get(c, "")
        log(f"{c:<44} {s1:>10.4g} {nets['Sc2']:>10.4g} {nets['Sc3']:>10.4g} "
            f"{nets['Sc4']:>10.4g} {svs['Sc2']:>10.4g} {svs['Sc3']:>10.4g} "
            f"{svs['Sc4']:>10.4g}  {un}")
        rows.append((c, s1, gs["Sc2"], svs["Sc2"], nets["Sc2"], gs["Sc3"], svs["Sc3"],
                     nets["Sc3"], gs["Sc4"], svs["Sc4"], nets["Sc4"], un))

    log("\nSpotlight (reductions RELATIVE TO Sc1; bigger = better):")
    for key in SPOTLIGHT:
        for (c, s1, g2, sv2, s2n, g3, sv3, s3n, g4, sv4, s4n, un) in rows:
            if key in c.lower() and "(" not in c:
                r = {k: (100.0 * (s1 - n) / s1 if s1 else 0.0)
                     for k, n in (("Sc2", s2n), ("Sc3", s3n), ("Sc4", s4n))}
                log(f"  * {c} [{un}]: Sc1 {s1:.4g}")
                log(f"      Sc2 net {s2n:.4g} (-{r['Sc2']:.1f}%) | Sc3 net {s3n:.4g} "
                    f"(-{r['Sc3']:.1f}%) | Sc4 net {s4n:.4g} (-{r['Sc4']:.1f}%)")
                log(f"      saving ratios: Sc3/Sc2 = {(sv3/sv2 if sv2 else 0):.2f}x | "
                    f"Sc4/Sc3 = {(sv4/sv3 if sv3 else 0):.2f}x")

    with CSV.open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "sc1", "sc2_gross", "sc2_saving", "sc2_net",
                    "sc3_gross", "sc3_saving", "sc3_net",
                    "sc4_gross", "sc4_saving", "sc4_net", "unit"])
        w.writerows(rows)

    log("\nSanity checks to eyeball:")
    log("- Sc3 saving > Sc2 saving in EVERY category; Sc4 saving >= Sc3 saving in every")
    log("  category (component credits add on top; PM haircut is smaller than IC credit).")
    log("- If Sc4 saving < Sc3 saving anywhere, the haircut/credit balance needs review.")
    log("- Sc4 is EXPLORATORY (functional yield [A]) - report with wide MC bands only.")
    log("\nDONE.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        TXT.write_text("\n".join(_l), encoding="utf-8")
        print(f"\nResults: {TXT}\nCSV: {CSV}")
