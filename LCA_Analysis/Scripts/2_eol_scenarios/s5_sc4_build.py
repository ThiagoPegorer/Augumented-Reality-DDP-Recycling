"""
S5 Sc4 builder v4 - builds BOTH Sc4 processes per LCA_framework_v4.md §3/S5 Sc4 spec
(EXPLORATORY UPPER-BOUND scenario - functional yield is [A], wide MC bands):
  A) 'VCU S5 EoL Sc4 (disassembly + reuse)'                 - gross side, chains S4 v4
  B) 'VCU S5 EoL Sc4 credits (avoided virgin + components)' - saving side
Reuse chain: harvest 0.94 [L Park removal] x functional 0.70 [A 0.5-0.9] x substitution
0.80 [A 0.5-1.0] = 52.6% of eligible 17 g => 9.0 g reused (IC 4.2 g + transistor 4.7 g).
Idempotent. Logs to ..\Outputs\s5_sc4_build_log.txt. Prereqs: DB open, IPC 8080, S4 v4 built.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\s5_sc4_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s5_sc4_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROC_G = "VCU S5 EoL Sc4 (disassembly + reuse)"
REF_G = "VCU life cycle, Sc4 (disassembly + reuse)"; REFKG = 0.66
S4_PROC = "VCU S4 Use Phase"; S4_FLOW = "VCU used, 15 y (S4)"; S4_AMT = 0.66

FACILITY = ("Dismantling facility share [C1 intensity]",
            "58807b1b-e827-3079-8669-b30f89939974", 1.06e-8)
DISMANTLE_KWH = 0.02   # [A, 0.005-0.05] tools + AR headset + functional-testing bench
SMELTER = ("Board minus reused components -> Cu smelter [SE]",
           "01e00045-86d0-373b-890b-da147da7ab66", 0.132)   # 0.141 - 0.009 reused
COMBUSTIBLE = ("Residual combustible -> waste plastic market",
               "d4694e95-cc65-3c51-a0bc-befd9e404ae5", 0.0325)
INERT = ("Residual inert (ON-SITE sorting losses only) -> sanitary landfill",
         "1b875518-1445-3702-a24b-04f7616461e7", 0.0111)

PROC_C = "VCU S5 EoL Sc4 credits (avoided virgin + components)"
REF_C = "VCU Sc4 recovered materials & components"; REF_C_KG = 0.4141  # Bigum-refined
CREDITS = [  # (label, uid, kg)
    ("NEW Avoided IC production (4.2 g reused processors+flash)",
     "92416fbe-e108-3676-86f9-94a87b83d2a5", 0.0042),
    ("NEW Avoided transistor production (4.7 g reused power stages)",
     "ba609a3d-8496-3fb2-bed7-599f9a3637bf", 0.0047),
    ("Avoided Al primary ingot (339.9 g [L Lee x L Bigum])",
     "abb8f45f-8076-3728-a4c0-83ad3d103177", 0.3399),
    ("Avoided Cu cathode (53.3 g = 56.8 - 3.5 in reused parts)",
                                           "b103dac8-20d8-38dd-bde3-f275b3487bab", 0.0533),
    ("Avoided chromium steel (11.8 g [L])","5d5262a4-7975-31b7-a6c6-f35d9add4901", 0.0118),
    ("Avoided gold (84.5 mg = 86.6 - haircut)", "b9a4f149-3f53-3445-a446-8ee24da098ab", 8.5e-5),
    ("Avoided silver (51.1 mg = 54.9 - haircut)", "8434ffb4-c639-3af6-91ba-a239c053753b", 5.1e-5),
    ("Avoided palladium (4.8 mg = 5.2 - haircut)","174e04de-106d-3946-8b17-886fb2da6dee", 4.8e-6),
]

DESC_G = ("S5 Scenario 4 (full disassembly + component reuse) - GROSS side. EXPLORATORY "
 "UPPER-BOUND: functional yield is [A], present with wide MC bands only. As Sc3 plus "
 "component-level removal and functional testing (electricity 0.02 kWh [A 0.005-0.05]); "
 "reused 9.0 g leave as components (harvest 0.94 [L Park REMOVAL rate, Zhao 2023 p.207; "
 "upper 1.00 L Marconi damage-free p.208] x functional 0.70 [A - NOT quantified in "
 "literature, Zhao Table 5 criteria only; bathtub + Conti&Orcioni qualitative; DPP "
 "traceability argument] x substitution 0.80 [A]). Failed harvested parts return to smelter "
 "feed 0.132 kg [SE]. Small SMDs excluded (39.73% removal, Chen 2013 in Zhao p.207). "
 "Bigum ledger: 0.4749 scrap-out + 0.0111 sorting losses + 0.132 smelter + 0.0325 "
 "combustible + 0.009 reused = 0.660.")

DESC_C = ("S5 Scenario 4 - SAVING side. NEW component credits: avoided IC 4.2 g + avoided "
 "transistor 4.7 g (S1 datasets, credited ONCE - their contained metals removed from material "
 "credits: Cu -3.5 g, board-PM proportional haircut 6.5% [A] -> Au 84.5 mg, Ag 51.1 mg, "
 "Pd 4.8 mg; base rates BIGUM-REFINED as Sc3). Al 339.9 g and steel 11.8 g as Sc3. "
 "Calculated separately: total = SAVING; "
 "net = gross - saving. Regulatory anchors: WEEELABEX >10 cm2 board separation; EN 50625-1 "
 "reuse-preparation mass counts toward recycling rate.")

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
    return (p, f, loc, un, ref.unit)

def wipe(proc_name, flow_name):
    for d in client.get_descriptors(o.Process):
        if d.name == proc_name:
            try: client.delete(d); log(f"Deleted prior '{proc_name}'.")
            except Exception as ex: log(f"(delete: {ex})")
    for d in client.get_descriptors(o.Flow):
        if d.name == flow_name:
            try: client.delete(d)
            except Exception: pass

def main():
    log("S5 Sc4 v4 build - gross + credits (exploratory upper-bound)")
    wipe(PROC_G, REF_G); wipe(PROC_C, REF_C)
    mass = client.get(o.FlowProperty, name="Mass")

    # ---------- Process A: gross ----------
    proc = o.new_process(PROC_G); proc.description = DESC_G
    rf = o.new_product(REF_G, mass); client.put(rf)
    out = o.new_output(proc, rf, REFKG); out.is_quantitative_reference = True

    s4p = client.get(o.Process, name=S4_PROC); s4f = client.get(o.Flow, name=S4_FLOW)
    if s4p is None or s4f is None:
        log("!! S4 v4 not found (check flow rename) - ABORTING."); return
    e = o.new_input(proc, s4f, S4_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s4p.id, name=s4p.name)
    log(f"  OK chain: {S4_AMT} kg <- {S4_PROC}")

    b = bundle(FACILITY[1])
    if b:
        p, f, loc, un, uref = b
        e = o.new_input(proc, f, FACILITY[2]); e.unit = uref
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        log(f"  OK {FACILITY[0]}: {FACILITY[2]:g} {un} -> {p.name} [{loc}]")
    else:
        log(f"  !! {FACILITY[0]}: not found - SKIPPED, review")

    elec_linked = False
    for d in client.get_descriptors(o.Process):
        base = (d.name or "").split("|")[0].strip().lower()
        if base == "market for electricity, low voltage":
            bb = bundle(d.id)
            if bb and "germany" in bb[2].lower():
                p, f, loc, un, uref = bb
                e = o.new_input(proc, f, DISMANTLE_KWH); e.unit = uref
                e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
                log(f"  OK dismantle+test electricity [A]: {DISMANTLE_KWH} {un} -> {p.name} [{loc}]")
                elec_linked = True; break
    if not elec_linked: log("  !! electricity: no Germany low-voltage match - SKIPPED, review")

    for label, pid, amt in (SMELTER, COMBUSTIBLE, INERT):
        b = bundle(pid)
        if not b: log(f"  !! {label}: provider {pid} not found - SKIPPED"); continue
        p, wf, loc, un, uref = b
        e = o.new_output(proc, wf, amt); e.unit = uref
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
    client.put(proc)
    closure = 0.4749 + 0.0111 + 0.132 + 0.0325 + 0.009
    log(f"  -- mass ledger (Bigum): 0.4749 scrap-out + 0.0111 sorting losses + 0.132 smelter "
        f"+ 0.0325 combustible + 0.009 REUSED = {closure:.4f} kg vs 0.66 "
        f"({'OK' if abs(closure-0.66) < 2e-3 else '!! MISMATCH'})")

    # ---------- Process B: credits ----------
    procC = o.new_process(PROC_C); procC.description = DESC_C
    rfc = o.new_product(REF_C, mass); client.put(rfc)
    outc = o.new_output(procC, rfc, REF_C_KG); outc.is_quantitative_reference = True
    ok, tot = 0, 0.0
    for label, uid, amt in CREDITS:
        b = bundle(uid)
        if not b: log(f"  !! {label}: provider {uid} not found - SKIPPED"); continue
        p, f, loc, un, uref = b
        e = o.new_input(procC, f, amt); e.unit = uref
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        ok += 1; tot += amt
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
    client.put(procC)
    log(f"  -- credits: {ok}/{len(CREDITS)} linked, total {tot*1000:.1f} g vs ref {REF_C_KG*1000:.1f} g")

    log(f"\nDONE - '{PROC_G}' + '{PROC_C}' built. F5 in openLCA.")
    log("CHECKPOINT: confirm the two NEW component credits (IC, transistor) + haircut amounts.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
