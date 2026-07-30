"""
Sc1 vs Sc2 comparison - EF 3.1, cradle-to-grave. READS ONLY (no DB writes).
Three calculation targets (each pulls the full S1->S5 chain via default providers):
  1) 'VCU S5 EoL Sc1 (no recycling)'            -> Sc1 total
  2) 'VCU S5 EoL Sc2 (bulk recycling)'          -> Sc2 GROSS
  3) 'VCU S5 EoL Sc2 credits (avoided virgin)'  -> Sc2 SAVING
Reported per EF 3.1 category: Sc1 | Sc2 gross | saving | Sc2 net (gross - saving) | delta vs Sc1.
Writes Outputs\sc1_sc2_results.txt + sc1_sc2_results.csv.
Prereqs: DB open, IPC on 8080, S1-S5(Sc1,Sc2) built. Runtime: 3 full calcs - be patient.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\sc1_sc2_calc.py"
"""
import csv, pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "sc1_sc2_results.txt"; CSV = HERE / "sc1_sc2_results.csv"; _l = []
def log(m): print(m); _l.append(str(m))

METHOD = "EF 3.1 Method (adapted)"
TARGETS = [
    ("Sc1",     "VCU S5 EoL Sc1 (no recycling)"),
    ("Sc2g",    "VCU S5 EoL Sc2 (bulk recycling)"),
    ("Sc2save", "VCU S5 EoL Sc2 credits (avoided virgin)"),
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
    vals = {}
    units = {}
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

    results, units = {}, {}
    for key, name in TARGETS:
        r = calc(name, method)
        if r is None: return
        results[key], u = r
        units.update(u)

    cats = sorted(set(results["Sc1"]) | set(results["Sc2g"]) | set(results["Sc2save"]),
                  key=str.lower)
    log(f"\n=== Sc1 vs Sc2, EF 3.1, 1 VCU cradle-to-grave ({len(cats)} categories) ===\n")
    header = f"{'Category':<52} {'Sc1':>12} {'Sc2 gross':>12} {'saving':>12} {'Sc2 net':>12} {'net-Sc1':>12}  unit"
    log(header); log("-" * len(header))
    rows = []
    for c in cats:
        s1 = results["Sc1"].get(c, 0.0)
        g = results["Sc2g"].get(c, 0.0)
        sv = results["Sc2save"].get(c, 0.0)
        net = g - sv
        d = net - s1
        un = units.get(c, "")
        log(f"{c:<52} {s1:>12.4g} {g:>12.4g} {sv:>12.4g} {net:>12.4g} {d:>12.4g}  {un}")
        rows.append((c, s1, g, sv, net, d, un))

    log("\nSpotlight (thesis-relevant):")
    for key in SPOTLIGHT:
        for c, s1, g, sv, net, d, un in rows:
            if key in c.lower() and "(" not in c:
                pct = (100.0 * (s1 - net) / s1) if s1 else 0.0
                log(f"  * {c}: Sc1 {s1:.4g} -> Sc2 net {net:.4g} {un}  ({pct:+.1f}% vs Sc1)")

    with CSV.open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "sc1", "sc2_gross", "sc2_saving", "sc2_net", "net_minus_sc1", "unit"])
        w.writerows(rows)

    log("\nReading guide (APOS in practice):")
    log("- 'Sc2 gross' includes shredding + residues on TOP of the shared S1-S4 chain.")
    log("- 'saving' is the credits process alone (avoided virgin production).")
    log("- APOS background: some datasets already allocate recycled content upstream, so the")
    log("  explicit credit carries a mild double-count risk - declared in framework §4.")
    log("- Sanity checks: saving should NOT exceed gross-of-S1-materials for any category;")
    log("  Sc2 net should sit BETWEEN Sc1 and (Sc1 - full-credit) expectations. Flag anomalies.")
    log("\nDONE.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        TXT.write_text("\n".join(_l), encoding="utf-8")
        print(f"\nResults written to: {TXT}\nCSV: {CSV}")
