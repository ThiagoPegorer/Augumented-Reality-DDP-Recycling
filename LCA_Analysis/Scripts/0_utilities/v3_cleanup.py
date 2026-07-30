"""
v3 cleanup - removes leftover v3 model objects in dependency order
(product systems -> processes -> flows), then lists the surviving VCU objects.
Fixes the openLCA 'process is null' error: v3 'VCU S4 Use phase' still references
the deleted v3 S3 process/flow, so the GUI hits dangling pointers.

DOES NOT TOUCH the live v4 objects (S1 Materials & Construction, S2 Hardware
Assembly, S3 Distribution + their flows) - only v3-exclusive names are deleted.

Idempotent. Logs to ..\Outputs\v3_cleanup_log.txt. Prereqs: DB open, IPC on 8080.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\0_utilities\v3_cleanup.py"
Afterwards: F5 in openLCA (close any open v3 editor tabs; restart openLCA if the
null error lingers in an open tab).
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "v3_cleanup_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

V3_PROCESSES = [               # downstream first
    "VCU S5 EoL Sc1 (no recycling)",
    "VCU S4 Use phase",
    "VCU S1 Raw materials",
]
V3_FLOWS = [
    # 2026-07-25: cleanup COMPLETED; S4/S5 flow names removed from this list because the
    # v4 builds now REUSE those names - re-running with them here would delete live v4 flows.
    "VCU raw materials (S1)",
]
V4_KEEP = {                    # guard list - NEVER deleted, verified at the end
    "VCU S1 Materials & Construction", "VCU S2 Hardware Assembly", "VCU S3 Distribution",
    "VCU materials (S1)", "VCU assembled (S2)", "VCU delivered (S3)",
}

client = ipc.Client(8080)

def main():
    log("v3 cleanup - product systems -> processes -> flows")

    # (1) product systems: this DB should contain only our own; delete VCU/Sc ones, list others
    try: systems = client.get_descriptors(o.ProductSystem)
    except Exception as ex: systems = []; log(f"(product system listing failed: {ex})")
    if not systems: log("  no product systems found.")
    for d in systems:
        n = (d.name or "")
        if any(k in n.lower() for k in ("vcu", "sc1", "sc2", "sc3", "sc4")):
            try: client.delete(d); log(f"  DEL product system: {n}")
            except Exception as ex: log(f"  !! product system '{n}': {ex}")
        else:
            log(f"  ?? product system left untouched (review): {n}")

    # (2) v3 processes
    for d in client.get_descriptors(o.Process):
        if d.name in V3_PROCESSES:
            if d.name in V4_KEEP: log(f"  !! guard hit, skipping: {d.name}"); continue
            try: client.delete(d); log(f"  DEL process: {d.name}")
            except Exception as ex: log(f"  !! process '{d.name}': {ex}")

    # (3) v3 flows
    for d in client.get_descriptors(o.Flow):
        if d.name in V3_FLOWS:
            if d.name in V4_KEEP: log(f"  !! guard hit, skipping: {d.name}"); continue
            try: client.delete(d); log(f"  DEL flow: {d.name}")
            except Exception as ex: log(f"  !! flow '{d.name}': {ex}")

    # (4) survivors - should be EXACTLY the six v4 names
    log("\nSurviving VCU objects (expect exactly the 6 v4 names):")
    for d in client.get_descriptors(o.Process):
        if "vcu" in (d.name or "").lower():
            mark = "OK v4" if d.name in V4_KEEP else "?? UNEXPECTED - review"
            log(f"  process: {d.name}   [{mark}]")
    for d in client.get_descriptors(o.Flow):
        if "vcu" in (d.name or "").lower():
            mark = "OK v4" if d.name in V4_KEEP else "?? UNEXPECTED - review"
            log(f"  flow:    {d.name}   [{mark}]")

    log("\nDONE. F5 in openLCA; close/reopen any editor tab that still shows the null error.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
