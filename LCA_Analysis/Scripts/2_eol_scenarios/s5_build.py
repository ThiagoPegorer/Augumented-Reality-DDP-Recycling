"""
S5 builder v4 - Scenario 1 (no recycling): 'VCU S5 EoL Sc1 (no recycling)'.
Chains from S4 v4; unit leaves as two WASTE OUTPUTS (waste-flow convention: treatment
processes take the waste as their reference input, so we OUTPUT the waste flow and set
the treatment as its default provider - validated v3 pattern, UUIDs unchanged).
Sc2-Sc4 are built later from this template once recovery rates are sourced.
Idempotent. Logs to ..\Outputs\s5_build_log.txt. Prereqs: DB open, IPC on 8080, S4 v4 built.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\s5_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s5_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROC = "VCU S5 EoL Sc1 (no recycling)"; REF = "VCU life cycle, Sc1 (no recycling)"; REFKG = 0.66
S4_PROC = "VCU S4 Use Phase"; S4_FLOW = "VCU used, 15 y (S4)"; S4_AMT = 0.66  # renamed 2026-07-25

# Sc1 split RULED 2026-07-27: BOM-derived (§0.1 source of truth; consistent with Sc2-Sc4
# combustible accounting). Combustible = polymers 60 g + silicone 12.5 g (BOM v4.1 Table 2).
WASTE = [
    ("Inert 0.5875 kg -> sanitary landfill", "1b875518-1445-3702-a24b-04f7616461e7", 0.5875),
    ("Combustible 0.0725 kg -> waste plastic market (CH proxy)",
     "d4694e95-cc65-3c51-a0bc-befd9e404ae5", 0.0725),
]

DESC = ("S5 Scenario 1 (no recycling) - cradle-to-grave reference process, v4 chain. "
 "Chains from S4 v4 (66.2 kWh use phase). Disposal split per framework §3/S5 Sc1 (kept as "
 "BOM-derived, ruled 2026-07-27): inert 0.5875 kg to sanitary landfill [Europe w/o CH], "
 "combustible 0.0725 kg (polymers 60 g + silicone 12.5 g, BOM v4.1) to waste plastic market "
 "[CH proxy, flagged]. CAVEAT (declared, framework §0.3): APOS waste "
 "markets bundle the regional treatment mix incl. some recycling, so 'no recycling' is "
 "approximate; APOS-everywhere consistency accepted with supervisor 2026-07-02.")

client = ipc.Client(8080)

def bundle(uid):
    p = client.get(o.Process, uid=uid)
    if p is None: return None
    ref = None
    for e in (p.exchanges or []):
        if getattr(e, "is_quantitative_reference", False): ref = e; break
    if ref is None or ref.flow is None: return None
    f = client.get(o.Flow, uid=ref.flow.id)
    loc = p.location.name if p.location else "-"
    un = ref.unit.name if getattr(ref, "unit", None) else "?"
    return (p, f, loc, un)

def main():
    log(f"S5 v4 build - '{PROC}' (Sc1)")
    for d in client.get_descriptors(o.Process):
        if d.name == PROC:
            try: client.delete(d); log(f"Deleted prior '{PROC}'.")
            except Exception as ex: log(f"(delete: {ex})")
    for d in client.get_descriptors(o.Flow):
        if d.name == REF:
            try: client.delete(d)
            except Exception: pass

    mass = client.get(o.FlowProperty, name="Mass")
    proc = o.new_process(PROC); proc.description = DESC
    rf = o.new_product(REF, mass); client.put(rf)
    out = o.new_output(proc, rf, REFKG); out.is_quantitative_reference = True

    # (1) chain from S4 v4
    s4p = client.get(o.Process, name=S4_PROC); s4f = client.get(o.Flow, name=S4_FLOW)
    if s4p is None or s4f is None:
        log("!! S4 v4 process/flow not found - run s4_build.py first. ABORTING."); return
    e = o.new_input(proc, s4f, S4_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s4p.id, name=s4p.name)
    log(f"  OK chain: {S4_AMT} kg <- {S4_PROC}")

    # (2) waste outputs (treatment = default provider)
    total = 0.0
    for label, pid, amt in WASTE:
        b = bundle(pid)
        if not b:
            log(f"  !! {label}: provider {pid} not found - SKIPPED"); continue
        p, wf, loc, un = b
        e = o.new_output(proc, wf, amt)          # waste-flow convention: OUTPUT on our side
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        total += amt
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")

    log(f"  -- waste mass closure: {total:.3f} kg vs device 0.66 kg "
        f"({'OK' if abs(total-0.66) < 1e-9 else '!! MISMATCH - review split'})")

    client.put(proc)
    log(f"\nDONE - '{PROC}' built (chain S4 + 2 waste outputs). F5 in openLCA.")
    log("This process is the cradle-to-grave calculation target for the Sc1 product system.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
