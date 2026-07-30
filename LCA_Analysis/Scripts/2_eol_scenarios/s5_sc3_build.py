"""
S5 Sc3 builder v4 - builds BOTH Sc3 processes per LCA_framework_v4.md §3/S5 Sc3 spec:
  A) 'VCU S5 EoL Sc3 (guided disassembly)'       - gross side, chains S4 v4
  B) 'VCU S5 EoL Sc3 credits (avoided virgin)'   - saving side
Dismantling ~0 burden [A]; intact board 0.141 kg -> Cu smelter SE [01e00045]; credits at
Lee 2012 x Hagelueken 2006 rates (framework §3/S5 Sc3 tables). S1-validated credit UUIDs.
Idempotent. Logs to ..\Outputs\s5_sc3_build_log.txt. Prereqs: DB open, IPC 8080, S4 v4 built.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\s5_sc3_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s5_sc3_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROC_G = "VCU S5 EoL Sc3 (guided disassembly)"
REF_G = "VCU life cycle, Sc3 (guided disassembly)"; REFKG = 0.66
S4_PROC = "VCU S4 Use Phase"; S4_FLOW = "VCU used, 15 y (S4)"; S4_AMT = 0.66

SMELTER = ("Intact board+coating -> Cu smelter [SE, Boliden-type]",
           "01e00045-86d0-373b-890b-da147da7ab66", 0.141)
# Manual dismantling proxy (2026-07-25, replaces the ~0 declaration):
FACILITY = ("Dismantling facility share [C1 intensity: 1.6e-8/kg x 0.66]",
            "58807b1b-e827-3079-8669-b30f89939974", 1.06e-8)   # units (Item)
DISMANTLE_KWH = 0.01   # [A, 0.002-0.03] powered tools + AR headset, DE low voltage
COMBUSTIBLE = ("Residual combustible (insert+TIM+labels+fluorosilicone) -> waste plastic market",
               "d4694e95-cc65-3c51-a0bc-befd9e404ae5", 0.0325)
INERT = ("Residual inert (ON-SITE sorting losses only; remelt losses live inside [L] yields) -> sanitary landfill",
         "1b875518-1445-3702-a24b-04f7616461e7", 0.0111)

PROC_C = "VCU S5 EoL Sc3 credits (avoided virgin)"
REF_C = "VCU Sc3 recovered materials"; REF_C_KG = 0.4086  # Bigum-refined 2026-07-26
CREDITS = [  # (label, uid, kg) - rates per framework §3/S5 Sc3 table
    ("Avoided Al primary ingot (339.9 g, 98%[L Lee] x 79%[L Bigum])",
     "abb8f45f-8076-3728-a4c0-83ad3d103177", 0.3399),
    ("Avoided Cu cathode (56.8 g: contacts 31.3 + board 25.5, x95%[L])",
                                               "b103dac8-20d8-38dd-bde3-f275b3487bab", 0.0568),
    ("Avoided chromium steel (11.8 g, 98%[L Lee] x 100%[L Bigum])",
                                               "5d5262a4-7975-31b7-a6c6-f35d9add4901", 0.0118),
    ("Avoided gold (86.6 mg, 96x98%[L])",      "b9a4f149-3f53-3445-a446-8ee24da098ab", 8.66e-5),
    ("Avoided silver (54.9 mg, 96x97%[L])",    "8434ffb4-c639-3af6-91ba-a239c053753b", 5.5e-5),
    ("Avoided palladium (5.2 mg, 96x98%[L])",  "174e04de-106d-3946-8b17-886fb2da6dee", 5.2e-6),
]

DESC_G = ("S5 Scenario 3 (AR/DPP-guided disassembly) - GROSS side. Manual dismantling ~0 burden "
 "[A] (corroborated by ecoinvent laptop manual-dismantling structure: facility-only, no energy). "
 "Clean fractions bypass shredding entirely; intact board+coating 0.141 kg fed to 'treatment of "
 "electronics scrap, metals recovery in copper smelter' [SE] - European integrated route, "
 "burden ~0.046 kWh + 0.375 kg quicklime per kg feed, slag inside dataset. DECLARED DEVIATION: "
 "ecoinvent's used-PWB market would shred even dismantled boards; Sc3 bypasses per industrial "
 "practice at integrated smelters (Hagelueken 2006). Residues: combustible 32.5 g, inert 11.1 g "
 "(ON-SITE sorting losses only; remelt losses inside [L] yields - Bigum ledger 2026-07-26). "
 "Scrap-out 474.9 g leaves burden-free (credit convention §4). "
 "Downstream technology identical to Sc2 - difference isolated at dismantling/separation.")

DESC_C = ("S5 Scenario 3 - SAVING side (avoided virgin, §4). BIGUM-REFINED net rates: Al 77% "
 "(98 [L Lee 2012] x 79 [L Bigum/EASEWASTE]), Cu ~89% (x95 [L]), steel 98% x 100% [L] of "
 "fasteners (board Fe uncredited), Au 94% (96 [L Lee] x 98 [L Bigum/Huisman]), Ag ~93%, "
 "Pd ~94%. Ta 0 (no at-scale route, "
 "Cui & Zhang 2008 - Sc3 enables, does not credit). Sn 0 central (0-50% [A] MC). "
 "Contrast vs Sc2: Au 25.1% -> 94.1%. Calculated separately: total = SAVING; net = gross - saving.")

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
    log("S5 Sc3 v4 build - gross + credits")
    wipe(PROC_G, REF_G); wipe(PROC_C, REF_C)
    mass = client.get(o.FlowProperty, name="Mass")

    # ---------- Process A: gross ----------
    proc = o.new_process(PROC_G); proc.description = DESC_G
    rf = o.new_product(REF_G, mass); client.put(rf)
    out = o.new_output(proc, rf, REFKG); out.is_quantitative_reference = True

    s4p = client.get(o.Process, name=S4_PROC); s4f = client.get(o.Flow, name=S4_FLOW)
    if s4p is None or s4f is None:
        log("!! S4 v4 not found (check flow rename ran) - ABORTING."); return
    e = o.new_input(proc, s4f, S4_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s4p.id, name=s4p.name)
    log(f"  OK chain: {S4_AMT} kg <- {S4_PROC}")

    # manual dismantling proxy: facility share (INPUT, infrastructure) + tool/headset electricity
    b = bundle(FACILITY[1])
    if b:
        p, f, loc, un, uref = b
        e = o.new_input(proc, f, FACILITY[2])
        e.unit = uref
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        log(f"  OK {FACILITY[0]}: {FACILITY[2]:g} {un} -> {p.name} [{loc}]")
    else:
        log(f"  !! {FACILITY[0]}: provider not found - SKIPPED, review")
    elec_linked = False
    for d in client.get_descriptors(o.Process):
        base = (d.name or "").split("|")[0].strip().lower()
        if base == "market for electricity, low voltage":
            bb = bundle(d.id)
            if bb and "germany" in bb[2].lower():
                p, f, loc, un, uref = bb
                e = o.new_input(proc, f, DISMANTLE_KWH)
                e.unit = uref  # explicit kWh (unit gotcha fix 2026-07-25)
                e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
                log(f"  OK dismantling electricity (tools+AR headset [A]): {DISMANTLE_KWH} {un} "
                    f"-> {p.name} [{loc}]")
                elec_linked = True
                break
    if not elec_linked:
        log("  !! dismantling electricity: no Germany low-voltage match - SKIPPED, review")

    for label, pid, amt in (SMELTER, COMBUSTIBLE, INERT):
        b = bundle(pid)
        if not b: log(f"  !! {label}: provider {pid} not found - SKIPPED"); continue
        p, wf, loc, un, uref = b
        e = o.new_output(proc, wf, amt)          # waste-output convention
        e.unit = uref
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
    client.put(proc)
    closure = 0.4749 + 0.0111 + 0.141 + 0.0325  # scrap-out + on-site sorting losses + smelter + combustible
    log(f"  -- mass ledger (Bigum 2026-07-26): 0.4749 scrap-out + 0.0111 sorting losses(inert) "
        f"+ 0.141 smelter + 0.0325 combustible = {closure:.4f} kg vs 0.66 kg "
        f"({'OK' if abs(closure - 0.66) < 2e-3 else '!! MISMATCH - review'})")

    # ---------- Process B: credits ----------
    procC = o.new_process(PROC_C); procC.description = DESC_C
    rfc = o.new_product(REF_C, mass); client.put(rfc)
    outc = o.new_output(procC, rfc, REF_C_KG); outc.is_quantitative_reference = True
    ok, tot = 0, 0.0
    for label, uid, amt in CREDITS:
        b = bundle(uid)
        if not b: log(f"  !! {label}: provider {uid} not found - SKIPPED"); continue
        p, f, loc, un, uref = b
        e = o.new_input(procC, f, amt)
        e.unit = uref
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        ok += 1; tot += amt
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
    client.put(procC)
    log(f"  -- credits: {ok}/{len(CREDITS)} linked, total {tot*1000:.1f} g vs ref {REF_C_KG*1000:.1f} g")

    log(f"\nDONE - '{PROC_G}' + '{PROC_C}' built. F5 in openLCA.")
    log("CHECKPOINT: confirm smelter link [SE] + amounts vs framework §3/S5 Sc3 tables.")
    log("Reminder: run the patched s5_sc2_build.py to add Sc2's 0.038 kg smelter feed (asymmetry fix).")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
