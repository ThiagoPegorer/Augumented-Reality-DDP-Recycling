"""
Sc1 split ruling (2026-07-27): adopt the BOM-derived disposal split, IN PLACE.
  inert -> sanitary landfill:      0.528  -> 0.5875 kg
  combustible -> waste plastic:    0.132  -> 0.0725 kg  (polymers 60 g + silicone 12.5 g, BOM v4.1)
Closure stays exact: 0.5875 + 0.0725 = 0.660. Edits amounts without rebuilding the process,
so the Sc1 product system stays valid. Idempotent.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\fix_sc1_split.py"
Afterwards: RE-RUN impact_runs.py and impact_recipe_rerun.py (all Sc1 columns, percentages
and the screening change slightly - old CSVs are superseded).
"""
import olca_ipc as ipc, olca_schema as o

FIXES = [  # (flow-name prefix, old, new)
    ("inert waste",           0.528, 0.5875),
    ("waste plastic, mixture", 0.132, 0.0725),
]

client = ipc.Client(8080)
proc = client.get(o.Process, name="VCU S5 EoL Sc1 (no recycling)")
if proc is None:
    print("!! Sc1 process not found")
else:
    changed = False
    for prefix, old, new in FIXES:
        hit = None
        for e in (proc.exchanges or []):
            if (not e.is_input) and e.flow and (e.flow.name or "").lower().startswith(prefix):
                hit = e; break
        if hit is None:
            print(f"!! no output exchange starting '{prefix}' - REVIEW"); continue
        if abs(hit.amount - new) < 1e-12:
            print(f"OK '{hit.flow.name}': already {new} - no-op"); continue
        if abs(hit.amount - old) > 1e-9:
            print(f"?? '{hit.flow.name}': amount {hit.amount} != expected {old} - fixing anyway")
        print(f"FIX '{hit.flow.name}': {hit.amount} -> {new} kg")
        hit.amount = new; changed = True
    if changed:
        client.put(proc)
        print("saved. Closure: 0.5875 + 0.0725 = 0.660 kg = device mass. BOM-consistent.")
