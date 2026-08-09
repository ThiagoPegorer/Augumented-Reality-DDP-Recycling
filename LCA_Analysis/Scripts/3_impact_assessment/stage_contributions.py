"""
Per-stage contributions (04d "Per stage" tab) - computes the S1..S4 direct contributions
plus S5 (Sc1 baseline) for ALL EF 3.1 categories by CUMULATIVE DIFFERENCING:
the stages chain linearly (S5<-S4<-S3<-S2<-S1, each pulling 0.66 kg from its predecessor),
so a product system built from the S_n process totals the CUMULATIVE impact through stage n,
and stage n's own contribution is total(S_n) - total(S_n-1). This reproduces the GUI
contribution tree at stage level with only the IPC calls this repo has already proven
(ps_build.py creation pattern + impact_runs.py calculation pattern).

Steps: resolve the 4 stage processes by prefix (no guessed names - aborts listing candidates
if ambiguous) -> create product systems (idempotent) -> 5 calculations (S1..S4 + the existing
Sc1 system) -> drift-check Sc1 against impact_EF31.csv -> write
Outputs\3_impact_assessment\impact_stage_contributions.csv -> inject the three reporting-set
categories into environmental.lifecycle_stages of BOTH payload copies (explicit paths).

Prereqs: openLCA open on the v4 database, IPC server on 8080. Runtime ~10 min (5 calcs).
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\stage_contributions.py"

GUI fallback if IPC misbehaves: create the four stage product systems by hand (right-click
process -> Create product system -> prefer default providers, unit processes), calculate each
under EF 3.1, and read the totals; the differencing arithmetic below still applies.
"""
import csv, datetime, json, pathlib, re, traceback
import olca_ipc as ipc, olca_schema as o

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "stage_contributions_log.txt"; _l = []
def log(m): print(m); _l.append(str(m))

# ---- explicit paths only (standing rule: never glob) ----
EF31_CSV = HERE / "impact_EF31.csv"
OUT_CSV  = HERE / "impact_stage_contributions.csv"
PAYLOADS = [  # live + mirror; BOTH must be written (standing payload rule)
    pathlib.Path(r"C:\Claude\Projects\AR_DPP\XR\AR_DPP_VCU\backend\data\vcu_001.json"),
    pathlib.Path(r"C:\Claude\Projects\AR_DPP\backend\data\vcu_001.json"),
]

STAGE_PREFIXES = ["VCU S1", "VCU S2", "VCU S3", "VCU S4"]   # resolved live, never guessed
SC1_SYSTEM = "VCU S5 EoL Sc1 (no recycling)"                # exists since ps_build.py
METHOD_TERMS = ["EF 3.1 Method (adapted)", "EF 3.1"]
DRIFT_WARN_PCT = 0.5

# payload injection targets: EF category name -> also mirrored into co2_kg?
REPORTING_SET = {
    "Resource use minerals and metals": False,
    "Climate change": True,
    "Eutrophication freshwater": False,
}

client = ipc.Client(8080)

def _norm(s): return re.sub(r"\s+", " ", (s or "")).strip().lower()

def resolve_method(terms):
    ds = client.get_descriptors(o.ImpactMethod)
    for t in terms:
        exact = [d for d in ds if _norm(d.name) == _norm(t)]
        if exact: return exact[0]
    for t in terms:
        subs = [d for d in ds if _norm(t) in _norm(d.name)]
        if subs:
            subs.sort(key=lambda d: len(d.name or ""))
            return subs[0]
    return None

def resolve_stage_process(prefix):
    """Exactly-one prefix match or abort - build-spec names are 'VCU S1 Materials &
    Construction' etc., but S2's exact name was never pinned in the framework doc,
    so all four are resolved live (section 0.2 discipline)."""
    hits = [d for d in client.get_descriptors(o.Process)
            if _norm(d.name).startswith(_norm(prefix))]
    if len(hits) == 1: return hits[0]
    log(f"!! process prefix '{prefix}' matched {len(hits)} processes:")
    for d in hits: log(f"     - {d.name}")
    return None

def resolve_system(name):
    for d in client.get_descriptors(o.ProductSystem):
        if (d.name or "").split("|")[0].strip() == name: return d
    return None

def create_system(proc_ref):
    """ps_build.py pattern - multiple signatures, prefer default providers."""
    proc = client.get(o.Process, uid=proc_ref.id)
    config = o.LinkingConfig(prefer_unit_processes=True,
                             provider_linking=o.ProviderLinking.PREFER_DEFAULTS)
    err = None
    for call in (
        lambda: client.create_product_system(proc, config),
        lambda: client.create_product_system(
            o.Ref(ref_type=o.RefType.Process, id=proc.id, name=proc.name), config),
        lambda: client.create_product_system(proc.id, config),
    ):
        try: return call()
        except TypeError as ex: err = ex; continue
        except Exception as ex: err = ex; break
    log(f"!! could not create product system for '{proc.name}' via IPC ({err})")
    return None

def calc(sys_ref, m_ref):
    setup = o.CalculationSetup(
        target=o.Ref(ref_type=o.RefType.ProductSystem, id=sys_ref.id, name=sys_ref.name),
        impact_method=o.Ref(ref_type=o.RefType.ImpactMethod, id=m_ref.id, name=m_ref.name))
    result = client.calculate(setup)
    result.wait_until_ready()
    vals, units = {}, {}
    for i in result.get_total_impacts():
        cat = i.impact_category
        n = getattr(cat, "name", "?")
        vals[n] = i.amount
        units[n] = getattr(cat, "ref_unit", None) or getattr(cat, "refUnit", "") or ""
    result.dispose()
    return vals, units

def read_ef31_sc1():
    ref = {}
    if not EF31_CSV.exists():
        log(f"!! {EF31_CSV} missing - drift check SKIPPED"); return ref
    with EF31_CSV.open(encoding="utf-8") as f:
        for row in csv.DictReader(f):
            try: ref[row["category"]] = float(row["sc1"])
            except (KeyError, ValueError): pass
    return ref

def inject_payloads(stage_rows, units):
    """Fill environmental.lifecycle_stages[*].impacts for the reporting set; flip basis
    to 'modelled'. Requires the v0.19 skeleton (5 stage objects) to be present."""
    today = datetime.date.today().isoformat()
    for path in PAYLOADS:
        if not path.exists():
            log(f"!! payload missing: {path} - SKIPPED (fix the checkout, then re-run)")
            continue
        data = json.loads(path.read_text(encoding="utf-8"))
        stages = (data.get("environmental") or {}).get("lifecycle_stages") or []
        if len(stages) != 5:
            log(f"!! {path.name}: expected the v0.19 five-stage skeleton, found "
                f"{len(stages)} entries - payload NOT touched.")
            continue
        for s in stages:
            sid = s.get("id")             # "S1".."S5"
            if sid not in stage_rows: continue
            for imp in s.get("impacts") or []:
                cat = imp.get("category")
                if cat not in REPORTING_SET: continue
                imp["value"] = stage_rows[sid][cat]
                imp["basis"] = "modelled"
                if REPORTING_SET[cat]: s["co2_kg"] = round(stage_rows[sid][cat], 4)
        env = data["environmental"]
        env["lifecycle_stages_basis"] = "modelled"
        env["lifecycle_stages_note"] = (
            f"S1-S4 direct contributions + S5 (Sc1 baseline) computed {today} by "
            "stage_contributions.py: cumulative differencing of the chained stage product "
            "systems, EF 3.1, same model as impact_EF31.csv. S5 varies by scenario - "
            "see impact_recovery.")
        if "dpp_meta" in data and data["dpp_meta"]:
            data["dpp_meta"]["last_updated"] = today
        path.write_text(json.dumps(data, indent=2, ensure_ascii=False) + "\n",
                        encoding="utf-8")
        log(f"  -> injected into {path}")

def main():
    m = resolve_method(METHOD_TERMS)
    if m is None: log("!! EF 3.1 method not found - ABORT"); return
    log(f"Method: '{m.name}'")

    procs = {}
    for p in STAGE_PREFIXES:
        d = resolve_stage_process(p)
        if d is None: log("ABORT - pin the ambiguous name and re-run."); return
        procs[p] = d
        log(f"  {p} -> '{d.name}'")

    sc1 = resolve_system(SC1_SYSTEM)
    if sc1 is None:
        log(f"!! product system '{SC1_SYSTEM}' not found - run ps_build.py first. ABORT")
        return

    # idempotence: drop prior same-named stage systems, then create fresh
    names = {(d.name or "") for d in procs.values()}
    for d in client.get_descriptors(o.ProductSystem):
        if (d.name or "").split("|")[0].strip() in names:
            try: client.delete(d); log(f"Deleted prior product system '{d.name}'.")
            except Exception as ex: log(f"(delete {d.name}: {ex})")
    systems = {}
    for p in STAGE_PREFIXES:
        ref = create_system(procs[p])
        if ref is None: log("ABORT - see GUI fallback in the docstring."); return
        systems[p] = resolve_system(procs[p].name) or ref
        log(f"  product system OK for {p}")

    # 5 calculations: cumulative S1..S4 + Sc1 full total
    cum, units = {}, {}
    for p in STAGE_PREFIXES:
        log(f"calculating cumulative {p} ...")
        cum[p], u = calc(systems[p], m); units.update(u)
    log("calculating Sc1 (full cradle-to-grave) ...")
    total, u = calc(sc1, m); units.update(u)

    # drift check vs the frozen thesis CSV
    ref = read_ef31_sc1()
    drift_hits = 0
    for cat, v in ref.items():
        if cat in total and abs(v) > 1e-30:
            d = 100.0 * abs(total[cat] - v) / abs(v)
            if d > DRIFT_WARN_PCT:
                log(f"!! DRIFT {d:.2f} % in '{cat}' vs impact_EF31.csv - the DB changed "
                    "since 2026-07-27. STOP and report before trusting any number.")
                drift_hits += 1
    log(f"Drift check: {drift_hits} categories over {DRIFT_WARN_PCT} % "
        f"({len(ref)} compared).")

    # differencing: S1 = cum(S1); Sn = cum(Sn) - cum(Sn-1); S5 = total - cum(S4)
    cats = sorted(set().union(*[set(v) for v in cum.values()], set(total)), key=str.lower)
    per_stage = {}   # sid -> {cat: value}
    keys = STAGE_PREFIXES
    for i, p in enumerate(keys):
        sid = f"S{i+1}"
        prev = cum[keys[i-1]] if i > 0 else None
        per_stage[sid] = {c: cum[p].get(c, 0.0) - (prev.get(c, 0.0) if prev else 0.0)
                          for c in cats}
    per_stage["S5"] = {c: total.get(c, 0.0) - cum[keys[-1]].get(c, 0.0) for c in cats}

    # negative-stage sanity: a materially negative difference means the chain is not
    # linear (double-count or missing link) - report, never silently clamp
    for sid, row in per_stage.items():
        for c, v in row.items():
            base = abs(total.get(c, 0.0)) or 1e-30
            if v < 0 and abs(v) / base > 0.001:
                log(f"!! NEGATIVE stage value {sid} '{c}' = {v:.4g} "
                    "(>0.1 % of total) - chain not linear? Investigate before use.")

    with OUT_CSV.open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "s1", "s2", "s3", "s4", "s5_sc1",
                    "total_sc1", "ef31_csv_sc1", "unit"])
        for c in cats:
            w.writerow([c] + [per_stage[s].get(c, 0.0) for s in
                              ("S1", "S2", "S3", "S4", "S5")]
                       + [total.get(c, 0.0), ref.get(c, ""), units.get(c, "")])
    log(f"-> {OUT_CSV} ({len(cats)} categories)")

    for c in REPORTING_SET:
        parts = " | ".join(f"{s} {per_stage[s].get(c, 0.0):.4g}"
                           for s in ("S1", "S2", "S3", "S4", "S5"))
        log(f"  {c:<40} {parts} {units.get(c, '')}")

    if drift_hits == 0:
        inject_payloads(per_stage, units)
        log("DONE - CSV written, both payloads injected. Send stage_contributions_log.txt.")
    else:
        log("DONE WITH DRIFT - CSV written; payloads NOT touched. Send the log.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally: TXT.write_text("\n".join(_l), encoding="utf-8"); print(f"\nSummary: {TXT}")
