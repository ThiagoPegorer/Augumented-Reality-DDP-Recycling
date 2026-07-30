"""
Monte Carlo runner (step 4) - v3. The openLCA IPC session has an empirical budget of
~300-350 draws (leak in the server layer; simulator recycling does NOT clear it, only an
openLCA restart does). v3 therefore STOPS THE WHOLE RUN as soon as two stalls occur close
together (< 50 good draws apart) instead of limping at ~3 min/draw. On stop: restart
openLCA + IPC server and re-run the same command - it resumes from what is on disk.
Changes vs v1:
  * every HTTP call to the IPC server now has a 120 s timeout (no infinite hangs),
  * a stall watchdog: if one iteration takes > 180 s, the simulator is disposed and
    recreated and the iteration is retried (3 consecutive failures = system aborted),
  * the simulator is disposed + recreated every RESTART_EVERY = 50 draws, so
    server-side memory cannot accumulate across hundreds of iterations (the likely
    cause of the stalls). Draws are independent -> statistically irrelevant.
  * torn last rows in mc_raw_*.csv (from killed runs) are detected and truncated.

BEFORE running: restart openLCA AND the IPC server once (fresh JVM). If stalls
persist, raise openLCA's memory: File > Preferences > Configuration > maximum
memory -> 8192 MB or more, restart openLCA.

Usage (resumable - tops existing raw files up to N):
    py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\4_monte_carlo\mc_run.py" 1000
    py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\4_monte_carlo\mc_run.py" 1000 Sc3g   (one system)

Outputs (Outputs\): mc_raw_<key>.csv, mc_summary.csv, mc_net.csv, mc_run_log.txt.
READS ONLY - never writes to the database.
"""
import csv, math, pathlib, re, sys, time, traceback
import requests as _rq
import olca_ipc as ipc, olca_schema as o
import olca_ipc.ipc as _ipc_mod

# ---- give every IPC HTTP call a timeout (v1 could hang forever on a wedged server) ----
class _TimeoutRequests:
    def __getattr__(self, n): return getattr(_rq, n)
    def post(self, *a, **k):
        k.setdefault("timeout", 120)
        return _rq.post(*a, **k)
_ipc_mod.requests = _TimeoutRequests()

STALL_TIMEOUT = 180     # s allowed per single iteration before we recover
RESTART_EVERY = 50      # dispose + recreate the simulator every N draws
MAX_FAILS     = 3       # consecutive recoveries before aborting a system
MIN_GAP       = 50      # <50 good draws between two stalls = session degraded -> STOP RUN

class SessionDegraded(Exception): pass
_gdraw = 0              # successful draws across all systems this run
_last_stall = None      # _gdraw value at the previous stall

def register_stall():
    """Two stalls close together mean the server session is exhausted: recreating the
    simulator no longer helps and each draw costs ~3 min. Stop the whole run instead."""
    global _last_stall
    if _last_stall is not None and _gdraw - _last_stall < MIN_GAP:
        raise SessionDegraded(
            f"two stalls within {_gdraw - _last_stall} draws - openLCA session exhausted")
    _last_stall = _gdraw

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "mc_run_log.txt"; _l = []
def log(m):
    print(m); _l.append(str(m))
    try: TXT.write_text("\n".join(_l), encoding="utf-8")
    except OSError: pass

SYSTEMS = [
    ("Sc1",  "VCU S5 EoL Sc1 (no recycling)"),
    ("Sc2g", "VCU S5 EoL Sc2 (bulk recycling)"),
    ("Sc2s", "VCU S5 EoL Sc2 credits (avoided virgin)"),
    ("Sc3g", "VCU S5 EoL Sc3 (guided disassembly)"),
    ("Sc3s", "VCU S5 EoL Sc3 credits (avoided virgin)"),
    ("Sc4g", "VCU S5 EoL Sc4 (disassembly + reuse)"),
    ("Sc4s", "VCU S5 EoL Sc4 credits (avoided virgin + components)"),
]
NETS = [("Sc2", "Sc2g", "Sc2s"), ("Sc3", "Sc3g", "Sc3s"), ("Sc4", "Sc4g", "Sc4s")]
METHOD_TERMS = ["EF 3.1 Method (adapted)", "EF 3.1"]

client = ipc.Client(8080)

def _norm(s): return re.sub(r"\s+", " ", (s or "")).strip().lower()

def resolve_method():
    ds = client.get_descriptors(o.ImpactMethod)
    for t in METHOD_TERMS:
        hit = [d for d in ds if _norm(t) == _norm(d.name)] or \
              [d for d in ds if _norm(t) in _norm(d.name)]
        if hit:
            hit.sort(key=lambda d: len(d.name or ""))
            return hit[0]
    return None

def resolve_system(name):
    for d in client.get_descriptors(o.ProductSystem):
        if (d.name or "").split("|")[0].strip() == name: return d
    return None

def read_impacts(result):
    vals, units = {}, {}
    for i in result.get_total_impacts():
        cat = i.impact_category
        n = getattr(cat, "name", "?")
        vals[n] = i.amount
        units[n] = getattr(cat, "ref_unit", None) or getattr(cat, "refUnit", "") or ""
    return vals, units

def wait_ready(result, timeout_s):
    """Poll the result state; return the state when ready, None on stall/timeout."""
    t0 = time.time()
    while True:
        try:
            st = result.get_state()
        except Exception:
            return None                       # HTTP timeout / connection error = stall
        if getattr(st, "error", None): return st
        if getattr(st, "is_ready", False): return st
        if time.time() - t0 > timeout_s: return None
        time.sleep(0.5)

def dispose_quiet(result):
    if result is None: return
    try: result.dispose()
    except Exception: pass

def percentile(sorted_xs, q):
    if not sorted_xs: return float("nan")
    k = (len(sorted_xs) - 1) * q / 100.0
    f, c = int(math.floor(k)), int(math.ceil(k))
    if f == c: return sorted_xs[f]
    return sorted_xs[f] + (sorted_xs[c] - sorted_xs[f]) * (k - f)

def stats(xs):
    n = len(xs)
    if n == 0: return None
    mean = sum(xs) / n
    sd = math.sqrt(sum((x - mean) ** 2 for x in xs) / (n - 1)) if n > 1 else 0.0
    s = sorted(xs)
    return [n, mean, sd] + [percentile(s, q) for q in (5, 25, 50, 75, 95)]

def existing_rows(path):
    """Read a previous partial run; tolerate + truncate a torn final row."""
    if not path.exists(): return [], []
    with path.open(newline="", encoding="utf-8") as f:
        r = list(csv.reader(f))
    if len(r) < 1: return [], []
    header = r[0][1:]
    rows, torn = [], False
    for row in r[1:]:
        if not row: continue
        try:
            vals = [float(v) for v in row[1:]]
            if len(vals) != len(header): raise ValueError
            rows.append(vals)
        except ValueError:
            torn = True; break               # keep everything before the torn row
    if torn:
        with path.open("w", newline="", encoding="utf-8") as f:
            w = csv.writer(f)
            w.writerow(["iteration"] + header)
            for i, v in enumerate(rows, 1): w.writerow([i] + v)
        log(f"    (repaired torn row in {path.name}; {len(rows)} clean draws kept)")
    return header, rows

def simulate_system(skey, sys_ref, m_ref, n_target):
    raw_path = HERE / f"mc_raw_{skey}.csv"
    cats, rows = existing_rows(raw_path)
    done = len(rows)
    if done >= n_target:
        log(f"  {skey}: {done} draws on disk (>= {n_target}) - skipped.")
        return cats, rows, {}
    log(f"  {skey}: {done} on disk, running {n_target - done} more ...")

    setup = o.CalculationSetup(
        target=o.Ref(ref_type=o.RefType.ProductSystem, id=sys_ref.id, name=sys_ref.name),
        impact_method=o.Ref(ref_type=o.RefType.ImpactMethod, id=m_ref.id, name=m_ref.name))

    units = {}
    result = None
    sim_draws = 0            # draws taken from the current simulator
    fails = 0                # consecutive failures
    prev_vec = rows[-1] if rows else None
    t0 = time.time(); done0 = done
    f = raw_path.open("a", newline="", encoding="utf-8")
    w = csv.writer(f)
    it = done + 1
    try:
        while it <= n_target:
            try:
                if result is None:                        # (re)create simulator
                    result = client.simulate(setup)       # schedules draw #1 itself
                    sim_draws = 0
                else:
                    st = result.simulate_next()
                    if getattr(st, "error", None):
                        raise RuntimeError(f"simulate_next: {st.error}")
                st = wait_ready(result, STALL_TIMEOUT)
                if st is None:
                    raise RuntimeError(f"iteration stalled (> {STALL_TIMEOUT} s)")
                if getattr(st, "error", None):
                    raise RuntimeError(f"server error: {st.error}")
                vals, units = read_impacts(result)
            except SessionDegraded:
                dispose_quiet(result); result = None
                raise
            except Exception as ex:
                fails += 1
                log(f"    !! {skey} iter {it}: {ex} - recovering "
                    f"({fails}/{MAX_FAILS}): disposing + recreating simulator ...")
                dispose_quiet(result); result = None
                register_stall()                           # raises SessionDegraded if too soon
                if fails >= MAX_FAILS:
                    raise SessionDegraded(f"{MAX_FAILS} consecutive failures in {skey}")
                time.sleep(5)
                continue                                   # retry the same iteration

            vec = [vals.get(c, 0.0) for c in cats] if cats else None
            if not cats:
                cats = sorted(vals, key=str.lower)
                w.writerow(["iteration"] + cats)
                vec = [vals.get(c, 0.0) for c in cats]
            if prev_vec is not None and vec == prev_vec:
                log(f"    !! {skey} iter {it}: duplicate draw - recreating simulator.")
                dispose_quiet(result); result = None
                fails += 1
                if fails >= MAX_FAILS:
                    log(f"    !! {skey}: repeated duplicates - ABORTING this system.")
                    break
                continue

            w.writerow([it] + vec); f.flush()
            rows.append(vec); prev_vec = vec
            global _gdraw; _gdraw += 1
            fails = 0; sim_draws += 1; it += 1

            if sim_draws >= RESTART_EVERY:                 # preventive server-side cleanup
                dispose_quiet(result); result = None

            n_done = it - 1 - done0
            if n_done % 10 == 0 or n_done == 1:
                per = (time.time() - t0) / n_done
                eta = per * (n_target - it + 1) / 60.0
                log(f"    {skey} iter {it-1}/{n_target}  ({per:.1f} s/iter, ~{eta:.0f} min left)")
    finally:
        dispose_quiet(result)
        f.close()
    return cats, rows, units

def main():
    n_target = int(sys.argv[1]) if len(sys.argv) > 1 else 20
    only = sys.argv[2] if len(sys.argv) > 2 else None
    log(f"Monte Carlo run (hardened v2) - target {n_target} draws/system"
        + (f", system {only} only" if only else ", all 7 systems")
        + f". Simulator recycled every {RESTART_EVERY} draws.")

    m = resolve_method()
    if m is None: log("!! EF 3.1 method not found - ABORT"); return
    log(f"Method: '{m.name}'")

    # progress status first: what is already on disk
    for skey, _ in SYSTEMS:
        _, r0 = existing_rows(HERE / f"mc_raw_{skey}.csv")
        log(f"  status {skey}: {len(r0)}/{n_target} draws on disk")

    data = {}
    degraded = False
    for skey, name in SYSTEMS:
        if only and skey != only: continue
        d = resolve_system(name)
        if d is None: log(f"!! product system '{name}' not found - ABORT"); return
        try:
            data[skey] = simulate_system(skey, d, m, n_target)
        except SessionDegraded as ex:
            degraded = True
            log(f"\n!! SESSION EXHAUSTED ({ex}).")
            log("   This openLCA session cannot produce more draws (known ~300-350 draw")
            log("   budget per session; simulator recycling does not clear it).")
            log("   -> Close this window, RESTART openLCA AND the IPC server, then re-run")
            log("      the SAME command. Everything on disk is kept; the run resumes.")
            break

    log("\nWriting mc_summary.csv ...")
    with (HERE / "mc_summary.csv").open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["system", "category", "unit", "n", "mean", "sd",
                    "p5", "p25", "p50", "p75", "p95"])
        for skey, _ in SYSTEMS:
            cats, rows, units = data.get(skey) or (*existing_rows(HERE / f"mc_raw_{skey}.csv"), {})
            for j, c in enumerate(cats):
                st = stats([r[j] for r in rows])
                if st: w.writerow([skey, c, units.get(c, "")] + st)

    log("Writing mc_net.csv (net = gross - saving, iteration-paired, "
        "independent-sampling approximation) ...")
    with (HERE / "mc_net.csv").open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["scenario", "category", "n_pairs", "mean", "sd",
                    "p5", "p25", "p50", "p75", "p95"])
        for scen, gk, sk in NETS:
            gc, gr, _ = data.get(gk) or (*existing_rows(HERE / f"mc_raw_{gk}.csv"), {})
            sc_, sr, _ = data.get(sk) or (*existing_rows(HERE / f"mc_raw_{sk}.csv"), {})
            if not gr or not sr:
                log(f"  {scen}: gross or saving raw file missing - skipped."); continue
            npairs = min(len(gr), len(sr))
            for j, c in enumerate(gc):
                if c not in sc_: continue
                js = sc_.index(c)
                st = stats([gr[i][j] - sr[i][js] for i in range(npairs)])
                if st: w.writerow([scen, c] + st)

    log("\nDONE. Send mc_run_log.txt + mc_summary.csv + mc_net.csv.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        try: TXT.write_text("\n".join(_l), encoding="utf-8")
        except OSError: pass
        print(f"\nLog: {TXT}")
