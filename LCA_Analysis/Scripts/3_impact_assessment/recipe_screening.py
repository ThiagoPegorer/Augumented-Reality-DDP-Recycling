"""
ReCiPe screening tables — the ReCiPe counterparts of impact_screening.csv.

WHY THIS IS NOT A COPY OF THE EF SCRIPT
---------------------------------------
EF 3.1 ships normalisation AND weighting factors, so `impact_screening.csv` ranks
categories by a weighted person-equivalent and applies the PEF >=80 % rule.
ReCiPe 2016 is different in BOTH of its levels:

* ENDPOINT (H,A): the results are ALREADY normalised and weighted — that is what the
  unit "points" means. Verified numerically: the 17 leaf categories sum to the
  `total - total` single score to 1e-13. So the endpoint screening needs NO new data
  and NO NW set: the share of each category in the single score IS the screening.
  This part therefore runs OFFLINE from impact_ReCiPe_end.csv (no openLCA needed).

* MIDPOINT (H): ReCiPe 2016 defines normalisation factors (world 2010, per person)
  but NO official midpoint weighting factors — weighting only enters at the endpoint.
  So a midpoint screening can be NORMALISED ONLY. The PEF ">=80 % of the weighted
  footprint" rule cannot be transplanted verbatim; the cumulative column here ranks
  by person-equivalents and must be labelled as such in the thesis.
  This part needs the openLCA DB (IPC on 8080) to read the pack's NW set. If the
  ei-ReCiPe Midpoint pack carries no normalisation set, the script says so and writes
  nothing rather than inventing factors (framework §0: no guessing).

OUTPUT (Outputs\3_impact_assessment\)
  impact_screening_ReCiPe_end.csv   always (offline)
  impact_screening_ReCiPe_mid.csv   only if the DB provides normalisation factors
  recipe_screening_log.txt          what was found, what was skipped, and why

Column schema deliberately MATCHES impact_screening.csv so the notebook's existing
Pareto-chart cell works unchanged (only the filename and the title change):
  category, weighted_pe, share_pct, cum_pct, in_reporting_set, goal_pinned, basis
`weighted_pe` holds points (endpoint) or person-equivalents (midpoint); the `basis`
column names which, so a reader of the CSV alone cannot confuse them.

Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\recipe_screening.py"
      (openLCA + IPC only needed for the midpoint half; endpoint half always runs)
"""
import csv, pathlib, re, traceback

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
TXT = HERE / "recipe_screening_log.txt"; _l = []
def log(m):
    print(m); _l.append(str(m))
    try: TXT.write_text("\n".join(_l), encoding="utf-8")
    except OSError: pass

BASELINE = "sc1"          # PEF practice: screen on the reference footprint
THRESHOLD = 80.0

# goal-pinned categories, mirroring the EF screening's climate + minerals pins
PIN_END = ("metal depletion", "climate change")
PIN_MID = ("metal depletion", "climate change")

def write_screen(path, rows, basis, pins):
    """rows: list of (category, value). Ranked, cumulated, PEF-style set marked."""
    rows = sorted(rows, key=lambda r: -abs(r[1]))
    tot = sum(abs(v) for _, v in rows) or 1.0
    out, cum, crossed = [], 0.0, False
    for cat, val in rows:
        share = 100.0 * abs(val) / tot
        cum += share
        in_set = not crossed              # include the category that CROSSES the threshold
        if cum >= THRESHOLD: crossed = True
        pin = any(p in cat.lower() for p in pins)
        out.append((cat, val, share, cum, in_set, pin, basis))
        log(f"    {cat:<52} {share:5.1f}%  cum {cum:5.1f}%"
            f"{'  << reporting set' if in_set else ''}{'  [PINNED]' if pin else ''}")
    with path.open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["category", "weighted_pe", "share_pct", "cum_pct",
                    "in_reporting_set", "goal_pinned", "basis"])
        w.writerows(out)
    log(f"  -> {path.name} ({len(out)} categories)")
    return out

# ---------------------------------------------------------------- ENDPOINT (offline)
def endpoint():
    src = HERE / "impact_ReCiPe_end.csv"
    if not src.exists():
        log(f"!! {src.name} not found - endpoint screening skipped."); return
    rows = list(csv.DictReader(src.open(encoding="utf-8")))
    leaves = [r for r in rows if not r["category"].strip().endswith("- total")]
    grand = [r for r in rows if r["category"].strip() == "total - total"]
    if not grand:
        log("!! no 'total - total' row - endpoint screening skipped."); return
    g = float(grand[0][BASELINE])
    s = sum(float(r[BASELINE]) for r in leaves)
    log(f"\n=== ReCiPe ENDPOINT (H,A) — already normalised + weighted ===")
    log(f"  consistency check: leaves sum {s:.6f} vs single score {g:.6f} "
        f"(diff {s-g:.2e}) -> {'OK, points are additive' if abs(s-g) < 1e-6*max(1,abs(g)) else '** MISMATCH, investigate **'}")
    log(f"  screening = share of the {g:.3f}-point single score, {BASELINE} basis:")
    write_screen(HERE / "impact_screening_ReCiPe_end.csv",
                 [(r["category"], float(r[BASELINE])) for r in leaves],
                 "points (already normalised+weighted by the method)", PIN_END)

# ---------------------------------------------------------------- MIDPOINT (needs IPC)
def midpoint():
    log(f"\n=== ReCiPe MIDPOINT (H) — normalisation only (ReCiPe defines no midpoint weights) ===")
    src = HERE / "impact_ReCiPe_mid.csv"
    if not src.exists():
        log(f"!! {src.name} not found - midpoint screening skipped."); return
    mid = {r["category"]: float(r[BASELINE]) for r in csv.DictReader(src.open(encoding="utf-8"))}

    try:
        import olca_ipc as ipc, olca_schema as o
    except ImportError:
        log("!! olca_ipc not importable - midpoint screening skipped."); return
    try:
        client = ipc.Client(8080)
        ds = client.get_descriptors(o.ImpactMethod)
    except Exception as ex:
        log(f"!! cannot reach the IPC server on 8080 ({ex}).")
        log("   Start openLCA + the IPC server and re-run; the endpoint CSV above is already written.")
        return

    norm = lambda s: re.sub(r"\s+", " ", (s or "")).strip().lower()
    target = "ei - recipe midpoint (h)"
    hit = [d for d in ds if norm(d.name) == target] or \
          [d for d in ds if "recipe" in norm(d.name) and "midpoint" in norm(d.name) and "(h)" in norm(d.name)]
    if not hit:
        log("!! ei-ReCiPe Midpoint (H) not found. Methods containing 'recipe':")
        for d in ds:
            if "recipe" in norm(d.name): log(f"    - {d.name}")
        return
    m = hit[0]
    log(f"  method: '{m.name}'")

    full = client.get(o.ImpactMethod, uid=m.id)
    nwsets = full.nw_sets or []
    if not nwsets:
        log("  !! this pack carries NO normalisation/weighting set.")
        log("     NOT inventing factors (framework §0). Options, in order of preference:")
        log("       (a) import the openLCA-pack 'ReCiPe 2016 Midpoint (H)' method, which ships")
        log("           the World (2010) H normalisation set, and re-run this script against it;")
        log("       (b) enter the published ReCiPe 2016 World-2010 normalisation factors by hand")
        log("           as a [L] literature input, documented in the framework;")
        log("       (c) skip the midpoint screening and use the ENDPOINT single-score shares as")
        log("           the weighted view — defensible, since ReCiPe's own weighting lives there.")
        return

    log(f"  NW sets found: {[n.name for n in nwsets]}")
    nw = nwsets[0]
    for cand in nwsets:                       # prefer a World/H set if several exist
        nm = norm(cand.name)
        if "world" in nm or "(h)" in nm or " h" in nm: nw = cand; break
    nwfull = client.get(o.NwSet, uid=nw.id) if not getattr(nw, "factors", None) else nw
    factors = getattr(nwfull, "factors", None) or []
    if not factors:
        log(f"  !! NW set '{nw.name}' exposes no factors via IPC - skipped."); return

    has_w = any(getattr(f, "weighting_factor", None) for f in factors)
    log(f"  using NW set '{nw.name}' — {len(factors)} factors, "
        f"weighting factors {'PRESENT' if has_w else 'ABSENT (normalisation only, as expected for ReCiPe)'}")

    vals, missing = [], []
    for fct in factors:
        cname = getattr(fct.impact_category, "name", "?")
        nf = getattr(fct, "normalisation_factor", None)
        if cname not in mid: continue
        if not nf: missing.append(cname); continue
        wf = getattr(fct, "weighting_factor", None) or 1.0     # 1.0 = no weighting applied
        vals.append((cname, mid[cname] / nf * wf))
    uncovered = [c for c in mid if c not in {v[0] for v in vals}]
    if missing:   log(f"  categories with no normalisation factor (excluded): {missing}")
    if uncovered: log(f"  categories in the CSV not covered by the NW set (excluded): {uncovered}")
    if not vals:
        log("  !! no category could be normalised - skipped."); return

    basis = ("person-equivalents, normalised only (ReCiPe defines no midpoint weighting)"
             if not has_w else f"normalised + weighted with NW set '{nw.name}'")
    log(f"  ranking by {basis}, {BASELINE} basis:")
    write_screen(HERE / "impact_screening_ReCiPe_mid.csv", vals, basis, PIN_MID)
    if not has_w:
        log("  NOTE for the thesis: this is a NORMALISED ranking, not a weighted one. The")
        log("       PEF >=80 % rule presumes weighting, so cite the cumulative column as")
        log("       'cumulative share of normalised impact', never as 'weighted footprint'.")

def main():
    log("ReCiPe screening tables — counterparts of impact_screening.csv")
    log(f"Baseline: {BASELINE} (PEF practice: screen on the reference footprint)\n")
    endpoint()
    midpoint()
    log("\nDONE.")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        try: TXT.write_text("\n".join(_l), encoding="utf-8")
        except OSError: pass
        print(f"\nLog: {TXT}")
