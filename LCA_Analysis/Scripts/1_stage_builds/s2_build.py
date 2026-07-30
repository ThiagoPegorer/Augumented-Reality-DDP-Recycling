"""
S2 builder v4 - 'VCU S2 Hardware Assembly' per LCA_framework_v4.md §3 Stage-2 spec
(chains S1 v4 + die-casting energy [Dalquist & Gutowski 2004 Table 3] + SMT mounting dataset).
Idempotent. Logs to ..\Outputs\s2_build_log.txt. Prereqs: DB open, IPC on 8080, S1 v4 built+passed.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\1_stage_builds\s2_build.py"
"""
import pathlib, re, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "s2_build_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

PROC = "VCU S2 Hardware Assembly"; REF = "VCU assembled (S2)"; REFKG = 0.66
OLD_PROCS = [PROC, "VCU S2 Manufacturing"]        # also removes the v3 process (lost-wax proxy)
S1_PROC = "VCU S1 Materials & Construction"; S1_FLOW = "VCU materials (S1)"; S1_AMT = 0.66

# Die-casting, housing 0.344 kg cast [BOM #1]; D&G 2004 Table 3 [L] (framework §3/S2):
# 3.0 MJ/kg natural gas (melt/hold) + 4.9 MJ/kg electricity  ->  per VCU:
GAS_MJ   = 1.03   # via HEAT dataset (combustion included; final energy at gate)
ELEC_KWH = 0.47   # DE grid, medium voltage
# SMT placement + reflow: ecoinvent mounting dataset, per m2 of mounted board.
# 150x105 mm = 0.0158 m2 single-side basis. CHECKPOINT: if dataset doc counts per mounted
# SIDE, change to 0.0315 (double-sided population [A]) and re-run (idempotent).
SMT_M2 = 0.0158

DESC = ("S2 per LCA_framework_v4 §3: consumes S1 v4 output; adds (a) die-casting energy for the "
 "0.344 kg housing - Dalquist & Gutowski 2004 Table 3 [L]: 1.03 MJ as 'market for heat, district "
 "or industrial, natural gas' (heat dataset => combustion emissions included) + 0.47 kWh DE "
 "electricity; NOT the 14.9 MJ/kg 'incl. loss' row (grid losses live in the background DB); "
 "(b) SMT placement + reflow via 'market for mounting, surface mount technology, Pb-free solder' "
 f"at {SMT_M2} m2 (150x105 mm board, single-side basis pending dataset-doc check). "
 "Declared overlap: mounting dataset contains solder paste; S1 carries 3.2 g solder explicitly "
 "(~0.5% device mass, conservative, S1(c) pattern) - quantified in build log. "
 "Bare-PCB fab energy remains a declared gap (S1 note d). Software excluded (framework §3/S2).")

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
    """All descriptors whose name matches exactly; fetch until location matches a term.
    A term matches only as the WHOLE location name (case-insensitive) or as a whole
    substring of >=4 chars - short codes like 'de' would otherwise match 'Bangladesh'."""
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

def link(proc, b, amt, label, expect_unit=None):
    p, f, loc, un, uref = b
    if expect_unit and un.replace("^", "").lower() not in (expect_unit, expect_unit.lower()):
        log(f"  !! {label}: unit is '{un}', expected {expect_unit} - NOT linked, review.")
        return False
    e = o.new_input(proc, f, amt)
    e.unit = uref  # explicit unit from provider ref (kWh gotcha fix 2026-07-25)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=p.id, name=p.name)
    log(f"  OK {label}: {amt} {un} -> {p.name} [{loc}]")
    return True

def log_solder_content(market_bundle):
    """Checkpoint aid: find the underlying mounting ACTIVITY and print its solder/flux/
    electricity exchanges scaled to SMT_M2, so the S1-solder overlap is quantified."""
    pat = re.compile(r"solder|flux|electricit", re.I)
    mname = market_bundle[0].name or ""
    base = mname.split("|")[0].replace("market for", "").strip().lower()
    log(f"  -- overlap check (exchanges x {SMT_M2} m2):")
    shown = 0
    for d in descriptors():
        n = (d.name or "").lower()
        if base in n and "market for" not in n:
            p = client.get(o.Process, uid=d.id)
            for e in (p.exchanges or []):
                if e.is_input and e.flow and pat.search(e.flow.name or ""):
                    un = e.unit.name if getattr(e, "unit", None) else "?"
                    log(f"       {e.flow.name}: {e.amount} {un}/m2 -> {e.amount*SMT_M2:.6g} {un}")
                    shown += 1
            break
    if not shown: log("       (no matching activity/exchanges found - inspect in openLCA)")

def main():
    log(f"S2 v4 build - '{PROC}'")
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
    ok, planned = 0, 4

    # (1) chain from S1 v4
    s1p = client.get(o.Process, name=S1_PROC); s1f = client.get(o.Flow, name=S1_FLOW)
    if s1p is None or s1f is None:
        log("!! S1 v4 process/flow not found - run s1_build.py first. ABORTING."); return
    e = o.new_input(proc, s1f, S1_AMT)
    e.default_provider = o.Ref(ref_type=o.RefType.Process, id=s1p.id, name=s1p.name)
    ok += 1; log(f"  OK chain: {S1_AMT} kg <- {S1_PROC}")

    # (2) die-cast melt/hold heat (gas), Europe without Switzerland preferred
    b, matched = find_exact_by_location("market for heat, district or industrial, natural gas",
                                        ["europe"])
    if b:
        if not matched: log("  ?? heat: no Europe location match, linked fallback - REVIEW")
        if link(proc, b, GAS_MJ, "die-cast heat (natural gas) [D&G 2004]", expect_unit="mj"): ok += 1
    else:
        log("  !! heat dataset not found - SKIPPED")

    # (3) die-cast electricity, DE
    b, matched = find_exact_by_location("market for electricity, medium voltage",
                                        ["germany"])
    if b:
        if not matched: log("  ?? electricity: no DE match, linked fallback - REVIEW")
        if link(proc, b, ELEC_KWH, "die-cast electricity DE [D&G 2004]", expect_unit="kwh"): ok += 1
    else:
        log("  !! electricity dataset not found - SKIPPED")

    # (4) SMT placement + reflow (mounting dataset) - runtime resolve, review at checkpoint
    cands = find_substring(["mounting, surface mount technology",
                            "surface mount technology",
                            "mounting, smt"])
    if not cands:
        log("  !! SMT mounting: NO candidates ('mounting, surface mount technology') - SKIPPED.")
        log("     -> dataset may be absent from 3.8 APOS; STOP and discuss fallback (framework §3/S2).")
    else:
        log("  ?? SMT mounting: runtime-resolved, top candidates:")
        for c in cands[:6]: log(f"       - {c.name} [{c.id}]")
        pick = next((c for c in cands
                     if "market for" in (c.name or "").lower()
                     and "pb-free" in (c.name or "").lower()), cands[0])
        b = bundle(pick.id)
        if b and link(proc, b, SMT_M2, "SMT mounting (paste+placement+reflow)", expect_unit="m2"):
            ok += 1; log("     -> linked; CONFIRM name/unit + m2-per-side question at checkpoint")
            log_solder_content(b)

    client.put(proc)
    log(f"\nDONE - {ok}/{planned} inputs linked (+S1 chain counted). F5 in openLCA.")
    log("CHECKPOINT: review '??' lines, dataset doc (m2 per side?), and overlap figures above.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: LOG.write_text("\n".join(_l), encoding="utf-8"); print(f"Log: {LOG}")
