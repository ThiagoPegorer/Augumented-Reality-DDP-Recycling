"""
S5 Sc2 builder v4 - builds BOTH Sc2 processes per LCA_framework_v4.md §3/S5 Sc2 spec:
  A) 'VCU S5 EoL Sc2 (bulk recycling)'      - gross side (shred + residues), chains S4 v4
  B) 'VCU S5 EoL Sc2 credits (avoided virgin)' - saving side (avoided virgin markets)
Net recovery per Chancerel et al. 2009 stream-splits x Hagelueken 2006 smelter yield
(parameter table in framework §3/S5). Credits use S1-validated provider UUIDs.
Idempotent. Logs to ..\Outputs\s5_sc2_build_log.txt. Prereqs: DB open, IPC 8080, S4 v4 built.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\2_eol_scenarios\s5_sc2_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s5_sc2_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

# --- Process A: gross ---
PROC_G = "VCU S5 EoL Sc2 (bulk recycling)"
REF_G = "VCU life cycle, Sc2 (bulk recycling)"; REFKG = 0.66
S4_PROC = "VCU S4 Use Phase"; S4_FLOW = "VCU used, 15 y (S4)"; S4_AMT = 0.66  # renamed 2026-07-25
SHRED_TERMS = ["waste electric and electronic equipment, shredding",
               "electronic equipment, shredding",
               "waste electrical and electronic equipment"]
COMBUSTIBLE = ("Residual combustible -> waste plastic market",
               "d4694e95-cc65-3c51-a0bc-befd9e404ae5", 0.073)
INERT = ("Residual inert -> sanitary landfill",
         "1b875518-1445-3702-a24b-04f7616461e7", 0.1526)   # Bigum ledger 2026-07-26: misrouted mass leaves WITH scrap streams
# Asymmetry fix 2026-07-25 (framework §3/S5): Sc2 also carries smelter burden for what it
# actually sends - Cu-stream fragments = recovered smelter-route mass / 0.95 yield [A].
SMELTER = ("Cu-stream fragments -> Cu smelter [SE, asymmetry fix]",
           "01e00045-86d0-373b-890b-da147da7ab66", 0.038)

# --- Process B: credits (avoided virgin; S1-validated UUIDs) ---
PROC_C = "VCU S5 EoL Sc2 credits (avoided virgin)"
REF_C = "VCU Sc2 recovered materials"; REF_C_KG = 0.3527  # Bigum-refined
CREDITS = [  # (label, uid, kg)  net = stream-split [L Chancerel] x yield [L Hagelueken / A]
    ("Avoided Al primary ingot (300.3 g, 86%x79% [L Bigum T8])",
     "abb8f45f-8076-3728-a4c0-83ad3d103177", 0.3003),
    ("Avoided Cu cathode (36.1 g, 60x95% [L][L])", "b103dac8-20d8-38dd-bde3-f275b3487bab", 0.0361),
    ("Avoided chromium steel (16.3 g, 96x100% [L Bigum])", "5d5262a4-7975-31b7-a6c6-f35d9add4901", 0.0163),
    ("Avoided gold (23.1 mg, 25.6x98% [L])", "b9a4f149-3f53-3445-a446-8ee24da098ab", 2.31e-5),
    ("Avoided silver (6.6 mg, 11.5x97% [L])", "8434ffb4-c639-3af6-91ba-a239c053753b", 6.6e-6),
    ("Avoided palladium (1.38 mg, 25.6x98% [L])", "174e04de-106d-3946-8b17-886fb2da6dee", 1.4e-6),
]

DESC_G = ("S5 Scenario 2 (current practice, bulk route) - GROSS side. Device enters shredder "
 "WITHOUT manual/precise dismantling; mechanical macro-separation only (Chancerel et al. 2009 "
 "stream-splits: Au/Pd 25.6%, Ag 11.5%, Cu 60% reach recoverable fractions; corroborated by "
 "Marra et al. 2018). Chains S4 v4; whole 0.66 kg through WEEE shredding treatment; residues: "
 "combustible 0.073 kg -> waste plastic market, inert 0.1526 kg -> sanitary landfill (Bigum "
 "ledger 2026-07-26: misrouted mass leaves WITH scrap streams). Savings live in the separate "
 "credits process - report gross | saving | net (§4). "
 "Smelter own burden approximated inside treatment datasets (declared). APOS caveat declared.")

DESC_C = ("S5 Scenario 2 - SAVING side (avoided virgin production, framework §4). Inputs = "
 "avoided virgin markets at net-recovered masses (BIGUM-REFINED 2026-07-26, Table 8 p.11): "
 "Al 300.3 g (86% x 79% [L]), Cu 36.1 g (60% [L] x 95% [L]), steel 16.3 g (96% x 100% [L]), "
 "Au 23.1 mg (25.6% x 98% [L]), Ag 6.6 mg (11.5% x 97% [L]), Pd 1.38 mg (25.6% x 98% [L]). "
 "Ta: NO credit (lost to slag, Cui & Zhang "
 "2008). Sn: no central credit (0-50% [A] MC only). Calculated separately: total impact of this "
 "process = the SAVING; net = gross - saving in the results script. Never netted silently.")

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

def descriptors():
    if not hasattr(descriptors, "_c"): descriptors._c = client.get_descriptors(o.Process)
    return descriptors._c

def find_substring(terms):
    hits, seen = [], set()
    for t in terms:
        tl = t.lower()
        for d in descriptors():
            n = (d.name or "").lower()
            if tl in n and d.id not in seen:
                seen.add(d.id); hits.append(d)
    hits.sort(key=lambda d: (0 if "market for" in (d.name or "").lower() else 1, len(d.name or "")))
    return hits

def wipe(proc_name, flow_name):
    for d in descriptors():
        if d.name == proc_name:
            try: client.delete(d); log(f"Deleted prior '{proc_name}'.")
            except Exception as ex: log(f"(delete: {ex})")
    for d in client.get_descriptors(o.Flow):
        if d.name == flow_name:
            try: client.delete(d)
            except Exception: pass

def main():
    log("S5 Sc2 v4 build - gross + credits")
    wipe(PROC_G, REF_G); wipe(PROC_C, REF_C)
    mass = client.get(o.FlowProperty, name="Mass")

    # ---------- Process A: gross ----------
    proc = o.new_process(PROC_G); proc.description = DESC_G
    rf = o.new_product(REF_G, mass); client.put(rf)
    out = o.new_output(proc, rf, REFKG); out.is_quantitative_reference = True

    s4p = client.get(o.Process, name=S4_PROC); s4f = client.get(o.Flow, name=S4_FLOW)
    if s4p is None or s4f is None:
        log("!! S4 v4 not found - run s4_build.py first. ABORTING."); return
    e = o.new_input(proc, s4f, S4_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s4p.id, name=s4p.name)
    log(f"  OK chain: {S4_AMT} kg <- {S4_PROC}")

    # shredding treatment - runtime resolve, waste-output convention
    cands = find_substring(SHRED_TERMS)
    if not cands:
        log("  !! WEEE shredding: NO candidates - SKIPPED. STOP and discuss dataset choice.")
    else:
        log("  ?? WEEE shredding: runtime-resolved, top candidates:")
        for c in cands[:8]: log(f"       - {c.name} [{c.id}]")
        pick = next((c for c in cands if "shredding" in (c.name or "").lower()), cands[0])
        b = bundle(pick.id)
        if b:
            p, wf, loc, un = b
            e = o.new_output(proc, wf, 0.66)   # waste-output convention
            e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
            log(f"  OK shredding: 0.66 {un} -> {p.name} [{loc}]")
            log("     -> linked; CONFIRM choice + UUID at checkpoint")
        else:
            log(f"  !! shredding provider {pick.id} unusable - SKIPPED")

    for label, pid, amt in (COMBUSTIBLE, INERT, SMELTER):
        b = bundle(pid)
        if not b: log(f"  !! {label}: provider {pid} not found - SKIPPED"); continue
        p, wf, loc, un = b
        e = o.new_output(proc, wf, amt)
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")

    client.put(proc)
    log(f"  -- mass ledger (Bigum 2026-07-26): 0.660 = 0.3964 scrap-out + 0.038 smelter feed "
        f"+ 0.073 combustible + 0.1526 inert "
        f"({'OK' if abs(0.3964+0.038+0.073+0.1526-0.660) < 1e-3 else '!! MISMATCH'})")

    # ---------- Process B: credits ----------
    procC = o.new_process(PROC_C); procC.description = DESC_C
    rfc = o.new_product(REF_C, mass); client.put(rfc)
    outc = o.new_output(procC, rfc, REF_C_KG); outc.is_quantitative_reference = True
    ok, tot = 0, 0.0
    for label, uid, amt in CREDITS:
        b = bundle(uid)
        if not b: log(f"  !! {label}: provider {uid} not found - SKIPPED"); continue
        p, f, loc, un = b
        e = o.new_input(procC, f, amt)
        e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
        ok += 1; tot += amt
        log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
    client.put(procC)
    log(f"  -- credits: {ok}/{len(CREDITS)} linked, total {tot*1000:.1f} g vs ref {REF_C_KG*1000} g")

    log(f"\nDONE - '{PROC_G}' + '{PROC_C}' built. F5 in openLCA.")
    log("CHECKPOINT: confirm shredding dataset (?? candidates above) + residual split [A].")
    log("Calc plan: gross = target A | saving = target B | net = A - B (results script).")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
