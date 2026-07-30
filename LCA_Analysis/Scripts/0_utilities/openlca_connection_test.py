"""
openLCA IPC connection test.

RUN THIS ON THE WINDOWS MACHINE, after:
  1) opening the ecoinvent database in openLCA (name shown bold),
  2) starting the IPC server:  Tools > Developer tools > IPC Server > port 8080 > Start
     (leave the server dialog OPEN while scripts run),
  3) installing the client:     py -m pip install -U olca-ipc

Usage:
  py "C:\\Claude\\Projects\\AR_DPP\\CO2_analysis\\openlca_connection_test.py"

It prints a report AND writes it to ipc_test_output.txt next to this file,
so it can be reviewed afterwards.
"""

import sys
import datetime
import pathlib

_OUTDIR = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
_OUTDIR.mkdir(parents=True, exist_ok=True)
OUT = _OUTDIR / "ipc_test_output.txt"
lines = []


def log(msg: str) -> None:
    print(msg)
    lines.append(str(msg))


def save_and_exit(code: int) -> None:
    OUT.write_text("\n".join(lines), encoding="utf-8")
    print(f"\nReport written to: {OUT}")
    sys.exit(code)


log(f"openLCA IPC connection test - {datetime.datetime.now():%Y-%m-%d %H:%M:%S}")

# --- 0. import the client (verified package names) ---
try:
    import olca_ipc as ipc
    import olca_schema as o
except Exception as e:
    log(f"IMPORT FAILED: {e}")
    log("Fix: py -m pip install -U olca-ipc   (needs Python >= 3.11)")
    save_and_exit(1)

# --- 1. connect (verified: ipc.Client(8080)) ---
try:
    client = ipc.Client(8080)
    log("Client created on port 8080.")
except Exception as e:
    log(f"CLIENT FAILED: {e}")
    save_and_exit(1)

# --- 2. prove a read works (verified: client.get(Type, name=...)) ---
try:
    mass = client.get(o.FlowProperty, name="Mass")
    if mass is None:
        log("READ returned None - is a database open and the IPC server started?")
        save_and_exit(1)
    log(f"Read OK - 'Mass' flow property id: {mass.id}")
except Exception as e:
    log(f"READ FAILED (database open? IPC server started?): {e}")
    save_and_exit(1)

# --- 3. list impact methods, flag EF (extra check; wrapped so it can't fail the test) ---
try:
    methods = client.get_descriptors(o.ImpactMethod)
    log(f"Impact methods in database: {len(methods)}")
    ef = [m.name for m in methods if "EF" in (m.name or "")]
    if ef:
        log("EF methods present:")
        for name in ef:
            log(f"   - {name}")
    else:
        log("No 'EF' method found yet - import EF 3.1 before the build step.")
except Exception as e:
    log(f"(impact-method listing skipped: {e})")

# --- 4. count processes, to confirm ecoinvent background is loaded ---
try:
    procs = client.get_descriptors(o.Process)
    log(f"Processes in database: {len(procs)}")
except Exception as e:
    log(f"(process count skipped: {e})")

log("DONE - if you see method and process counts above, the IPC pipeline is ready.")
save_and_exit(0)
