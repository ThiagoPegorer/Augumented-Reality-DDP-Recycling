"""
S4 builder v4 - 'VCU S4 Use Phase' per LCA_framework_v4.md §3 Stage-4 spec.
Own-draw basis: 9 W [L, Bosch MS 5.0 family manual] x 5,625 h [L, MiD 2017 40 km/h]
/ 0.765 grid->12V [L+A] = 66.2 kWh DE low-voltage electricity. (48 W rating was a
phantom - removed 2026-07-25, see framework header correction.)
Idempotent. Logs to ..\Outputs\s4_build_log.txt. Prereqs: DB open, IPC on 8080, S3 v4 built.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\1_stage_builds\s4_build.py"
"""
import pathlib, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s4_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROC = "VCU S4 Use Phase"; REF = "VCU used, 15 y (S4)"; REFKG = 0.66  # renamed 2026-07-25
OLD_PROCS = [PROC, "VCU S4 Use phase"]     # v3 name (should already be gone via v3_cleanup)
S3_PROC = "VCU S3 Distribution"; S3_FLOW = "VCU delivered (S3)"; S3_AMT = 0.66

# 9 W [L] x 5,625 h [L] / 0.765 [L+A]  (framework §3/S4; MC range 54-189 kWh)
ELEC_KWH = 66.2
ELEC = "market for electricity, low voltage"

DESC = ("S4 per LCA_framework_v4 §3: VCU own electricity consumption only, BEV in Germany, "
 "15 y x 15,000 km/yr. Own draw 9 W [L] (Bosch MS 5.0 manual 'approx. 9 W at 14 V', declared "
 "family proxy; range 9-20 W [A]) x 5,625 h (225,000 km / 40 km/h [L] MiD 2017) / 0.765 "
 "grid->12V (charging 0.85 [L] Apostolaki-Iosifidou 2017 x DC/DC 0.90 [A]) = 66.2 kWh as DE "
 "low-voltage electricity (AC home/wallbox charging dominant [A]). Replaces v3's 216 kWh built "
 "on the phantom 48 W rating (datasheet correction 2026-07-25). External sensor/actuator loads "
 "are outside this FU (supply-rail capacity != own draw). Sleep draw (~1.5-2 kWh/15 y) and "
 "carried-mass energy excluded (declared). Identical across all EoL scenarios.")

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
    """Exact-name matches; location must match as whole name or >=4-char substring
    (short codes like 'de' would otherwise match 'Bangladesh')."""
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
    log(f"S4 v4 build - '{PROC}'")
    for d in descriptors():
        if d.name in OLD_PROCS:
            try: client.delete(d); log(f"Deleted prior process '{d.name}'.")
            except Exception as ex: log(f"(delete {d.name}: {ex})")
    for d in client.get_descriptors(o.Flow):
        if d.name == REF:
            try: client.delete(d)
            except Exception: pass

    mass = client.get(o.FlowProperty, name="Mass")
    proc = o.new_process(PROC); proc.description = DESC
    rf = o.new_product(REF, mass); client.put(rf)
    out = o.new_output(proc, rf, REFKG); out.is_quantitative_reference = True

    # (1) chain from S3 v4
    s3p = client.get(o.Process, name=S3_PROC); s3f = client.get(o.Flow, name=S3_FLOW)
    if s3p is None or s3f is None:
        log("!! S3 v4 process/flow not found - run s3_build.py first. ABORTING."); return
    e = o.new_input(proc, s3f, S3_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s3p.id, name=s3p.name)
    log(f"  OK chain: {S3_AMT} kg <- {S3_PROC}")

    # (2) use-phase electricity, DE low voltage
    b, matched = find_exact_by_location(ELEC, ["germany"])
    if b:
        p, f, loc, un, uref = b
        if un.replace("^", "").lower() != "kwh":
            log(f"  !! electricity: unit is '{un}', expected kWh - NOT linked, review.")
        else:
            if not matched: log("  ?? electricity: no Germany match, linked fallback - REVIEW")
            e = o.new_input(proc, f, ELEC_KWH)
            e.unit = uref  # explicit kWh (unit gotcha fix 2026-07-25)
            e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
            log(f"  OK use-phase electricity: {ELEC_KWH} {un} -> {p.name} [{loc}]")
    else:
        log(f"  !! '{ELEC}' not found - SKIPPED, review.")

    client.put(proc)
    log(f"\nDONE - '{PROC}' built (chain S3 + 66.2 kWh DE low-voltage). F5 in openLCA.")
    log("CHECKPOINT: confirm Germany location + low-voltage choice (home AC charging [A]).")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
