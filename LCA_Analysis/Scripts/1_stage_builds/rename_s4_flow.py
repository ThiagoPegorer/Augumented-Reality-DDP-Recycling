"""
Rename the S4 output flow: 'VCU end-of-life-ready (S4)' -> 'VCU used, 15 y (S4)'.
In-place rename (openLCA links by ID, not name) - no rebuild, no recalculation needed.
Also safe to re-run (no-op if already renamed).
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\1_stage_builds\rename_s4_flow.py"
Afterwards: F5 in openLCA (close any editor tab showing the old name).
"""
import olca_ipc as ipc, olca_schema as o

OLD = "VCU end-of-life-ready (S4)"
NEW = "VCU used, 15 y (S4)"

client = ipc.Client(8080)
flow = client.get(o.Flow, name=OLD)
if flow is None:
    check = client.get(o.Flow, name=NEW)
    print(f"'{OLD}' not found - " +
          (f"already renamed to '{NEW}'. Nothing to do." if check else
           "NEITHER name found. Investigate."))
else:
    flow.name = NEW
    client.put(flow)
    print(f"Renamed flow: '{OLD}' -> '{NEW}' (id {flow.id}). F5 in openLCA.")
