"""
In-place fix: Sc3 gold credit 8.7e-5 -> 8.66e-5 kg (86.6 mg, Bigum-refined precise value).
The exchange amount is edited without rebuilding the process, so the Sc3-credits product
system stays valid. Idempotent.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\fix_sc3_gold.py"
"""
import olca_ipc as ipc, olca_schema as o

client = ipc.Client(8080)
proc = client.get(o.Process, name="VCU S5 EoL Sc3 credits (avoided virgin)")
if proc is None:
    print("!! process not found")
else:
    done = False
    for e in (proc.exchanges or []):
        if e.is_input and e.flow and (e.flow.name or "").lower().startswith("gold"):
            if abs(e.amount - 8.66e-5) < 1e-12:
                print("already 8.66e-5 - no-op")
            else:
                print(f"FIX gold: {e.amount} -> 8.66e-05 kg")
                e.amount = 8.66e-5
                client.put(proc)
                print("saved.")
            done = True
            break
    if not done: print("!! gold exchange not found - review")
