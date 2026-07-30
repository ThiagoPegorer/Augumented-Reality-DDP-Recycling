"""
Repair the kWh-unit gotcha IN PLACE (no rebuild, links untouched).
new_input() defaulted electricity exchanges to the Energy property's reference unit (MJ)
instead of kWh. This script finds the affected exchanges, swaps the unit Ref to the
provider's own kWh unit, and reports before/after. Amount values stay as specified
(they were always meant as kWh). Idempotent - no-op when units are already kWh.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\1_stage_builds\fix_energy_units.py"
Afterwards: F5 in openLCA, verify the three exchanges show kWh, then RE-RUN
scenarios_calc.py - previous results underestimated S2/S4 electricity by 3.6x.
"""
import pathlib
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "fix_energy_units_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

# (process name, flow-name prefix, expected amount in kWh)
TARGETS = [
    ("VCU S2 Hardware Assembly",           "electricity, medium voltage", 0.47),
    ("VCU S4 Use Phase",                   "electricity, low voltage",    66.2),
    ("VCU S5 EoL Sc3 (guided disassembly)","electricity, low voltage",    0.01),
]

client = ipc.Client(8080)

def provider_kwh_unit(provider_id):
    p = client.get(o.Process, uid=provider_id)
    if p is None: return None
    for e in (p.exchanges or []):
        if getattr(e, "is_quantitative_reference", False):
            return e.unit
    return None

def main():
    log("kWh unit repair - in place")
    for proc_name, flow_prefix, expected in TARGETS:
        proc = client.get(o.Process, name=proc_name)
        if proc is None:
            log(f"!! '{proc_name}' not found - SKIPPED"); continue
        fixed = any_found = False
        for e in (proc.exchanges or []):
            if not e.is_input or e.flow is None: continue
            if not (e.flow.name or "").lower().startswith(flow_prefix): continue
            any_found = True
            un = e.unit.name if getattr(e, "unit", None) else "?"
            if un.lower() == "kwh":
                log(f"  OK {proc_name}: '{e.flow.name}' already kWh ({e.amount}) - no-op")
                continue
            if abs(e.amount - expected) > 1e-9:
                log(f"  ?? {proc_name}: amount {e.amount} != expected {expected} - REVIEW, "
                    "fixing unit anyway")
            prov = getattr(e, "default_provider", None)
            uref = provider_kwh_unit(prov.id) if prov else None
            if uref is None or (uref.name or "").lower() != "kwh":
                log(f"  !! {proc_name}: could not obtain kWh unit from provider - NOT fixed")
                continue
            log(f"  FIX {proc_name}: '{e.flow.name}' {e.amount} {un} -> {e.amount} {uref.name}")
            e.unit = uref
            fixed = True
        if any_found and fixed:
            client.put(proc); log(f"  -> saved '{proc_name}'")
        elif not any_found:
            log(f"  !! {proc_name}: no matching electricity input found - REVIEW")
    log("\nDONE. F5 in openLCA; verify units; RE-RUN scenarios_calc.py (old results invalid).")

if __name__ == "__main__":
    try: main()
    finally:
        LOG.write_text("\n".join(_l), encoding="utf-8")
        print(f"Log: {LOG}")
