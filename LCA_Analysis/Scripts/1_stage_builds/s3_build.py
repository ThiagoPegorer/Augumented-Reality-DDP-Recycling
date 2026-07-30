"""
S3 builder v4 - 'VCU S3 Distribution' per LCA_framework_v4.md §3 Stage-3 spec.
Outbound finished-unit leg ONLY (v3 sea + inbound legs removed - embedded in S1 markets).
Idempotent. Logs to ..\Outputs\s3_build_log.txt. Prereqs: DB open, IPC on 8080, S2 v4 built.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\1_stage_builds\s3_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s3_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROC = "VCU S3 Distribution"; REF = "VCU delivered (S3)"; REFKG = 0.66
S2_PROC = "VCU S2 Hardware Assembly"; S2_FLOW = "VCU assembled (S2)"; S2_AMT = 0.66

# Abstatt -> Wolfsburg ~490 km [A, 450-550] x 0.66 kg = 0.323 tkm (framework §3/S3)
TKM = 0.323
TRUCK = "market for transport, freight, lorry >32 metric ton, EURO6"
TRUCK_ALT = "market for transport, freight, lorry 16-32 metric ton, EURO6"

DESC = ("S3 per LCA_framework_v4 §3: outbound distribution of the finished VCU, Bosch Abstatt -> "
 "VW Wolfsburg (working example), ~490 km [A, 450-550] x 0.66 kg = 0.323 tkm as 'market for "
 "transport, freight, lorry >32 metric ton, EURO6' (RER) - long-haul semitrailer duty [A]; "
 "16-32 t class = alternate candidate in build log. v3 sea leg (Asia->DE) and inbound road leg "
 "REMOVED: inbound logistics are embedded in S1 market datasets (anti-double-count convention). "
 "Packaging excluded (<1% carrier mass, §6 cut-off). Identical across all EoL scenarios.")

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

def descriptors():
    if not hasattr(descriptors, "_c"): descriptors._c = client.get_descriptors(o.Process)
    return descriptors._c

def find_exact_by_location(name, loc_terms):
    """Exact-name matches; fetch until location matches a term (whole name, or >=4-char
    substring - short codes like 'de' would otherwise match 'Bangladesh')."""
    fallback = None
    for d in descriptors():
        base = (d.name or "").split("|")[0].strip().lower()
        if base != name.lower(): continue
        b = bundle(d.id)
        if not b: continue
        if fallback is None: fallback = b
        loc = b[2].lower().strip()
        for t in loc_terms:
            t = t.lower()
            if loc == t or (len(t) >= 4 and t in loc): return b, True
    return fallback, False

def main():
    log(f"S3 v4 build - '{PROC}'")
    for d in descriptors():
        if d.name == PROC:
            try: client.delete(d); log(f"Deleted prior process '{PROC}' (v3 or earlier run).")
            except Exception as ex: log(f"(delete: {ex})")
    for d in client.get_descriptors(o.Flow):
        if d.name == REF:
            try: client.delete(d)
            except Exception: pass

    mass = client.get(o.FlowProperty, name="Mass")
    proc = o.new_process(PROC); proc.description = DESC
    rf = o.new_product(REF, mass); client.put(rf)
    out = o.new_output(proc, rf, REFKG); out.is_quantitative_reference = True

    # (1) chain from S2 v4
    s2p = client.get(o.Process, name=S2_PROC); s2f = client.get(o.Flow, name=S2_FLOW)
    if s2p is None or s2f is None:
        log("!! S2 v4 process/flow not found - run s2_build.py first. ABORTING."); return
    e = o.new_input(proc, s2f, S2_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s2p.id, name=s2p.name)
    log(f"  OK chain: {S2_AMT} kg <- {S2_PROC}")

    # (2) outbound truck leg, RER preferred
    b, matched = find_exact_by_location(TRUCK, ["europe", "rer"])
    if b:
        p, f, loc, un, uref = b
        if "km" not in un.lower():
            log(f"  !! truck: unit is '{un}', expected t*km - NOT linked, review.")
        else:
            if not matched: log("  ?? truck: no Europe/RER location match, linked fallback - REVIEW")
            e = o.new_input(proc, f, TKM)
            e.unit = uref  # explicit t*km (unit gotcha fix 2026-07-25)
            e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
            log(f"  OK truck Abstatt->Wolfsburg (~490 km): {TKM} {un} -> {p.name} [{loc}]")
    else:
        log(f"  !! '{TRUCK}' not found - SKIPPED, review candidates manually.")

    # checkpoint aid: alternate class candidate
    alt, alt_matched = find_exact_by_location(TRUCK_ALT, ["europe", "rer"])
    if alt:
        log(f"  -- alternate class candidate (not linked): {alt[0].name} [{alt[2]}] id={alt[0].id}")

    client.put(proc)
    log(f"\nDONE - '{PROC}' built (chain S2 + 1 outbound leg). F5 in openLCA.")
    log("CHECKPOINT: confirm truck class (>32 t vs 16-32 t) and RER location.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
