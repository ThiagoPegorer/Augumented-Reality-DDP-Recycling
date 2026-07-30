"""
ReCiPe 2016 endpoint — area-of-protection damage totals (the replacement for the single score).

WHY THE SINGLE SCORE WAS DROPPED
--------------------------------
openLCA's 'ReCiPe 2016 Endpoint (H)' NW sets normalise EACH endpoint category by its own
midpoint-derived reference and then apply the area-of-protection weight. Verified numerically
on 2026-07-28: endpoint points ÷ weight reproduces the MIDPOINT person-equivalents almost
exactly (ratios 0.995–1.001 for freshwater ecotoxicity, marine ecotoxicity, human carcinogenic
toxicity, global warming and mineral resource scarcity). In other words the endpoint "single
score" is weighted midpoint normalisation wearing a damage-unit costume: it adds no endpoint
information and inherits ReCiPe normalisation's toxicity dominance in full. Symptom: resources
end up at 0.13 % of that score, against 44.1 % under the old ReCiPe 2008 (H,A) single score.

Proper endpoint aggregation (sum damages within an area of protection, then normalise by the
per-capita AoP reference) is not what that NW set does, and the AoP references are not in the
database. So this study reports the endpoint the way it needs no value judgements at all:
the three AREA-OF-PROTECTION DAMAGE TOTALS in their native units. They are additive by
construction — that is the entire purpose of endpoint modelling — so no factor is required
and nothing is assumed.

  human health      = Σ categories in DALY
  ecosystem quality = Σ categories in species.yr
  resources         = Σ categories in USD2013

Reported alongside the 18 characterised midpoint categories, this gives the supervisor-requested
ReCiPe cross-check with zero normalisation and zero weighting. Prioritisation of categories
stays with EF 3.1, whose weights are the EU policy frame the DPP regulation itself sits in.

OFFLINE — reads impact_ReCiPe_end.csv, writes impact_ReCiPe_end_aop.csv. No openLCA, no IPC.
Run:  py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\recipe_aop_totals.py"
"""
import csv, pathlib, traceback

HERE = pathlib.Path(__file__).resolve().parent.parent.parent / "Outputs" / pathlib.Path(__file__).resolve().parent.name
HERE.mkdir(parents=True, exist_ok=True)
LOG = HERE / "recipe_aop_totals.txt"; _l = []
def log(m):
    print(m); _l.append(str(m))
    try: LOG.write_text("\n".join(_l), encoding="utf-8")
    except OSError: pass

AOP = {"DALY": "human health", "species.yr": "ecosystem quality", "USD2013": "resources"}
ORDER = ["human health", "ecosystem quality", "resources"]
COLS = ["sc1", "sc2_gross", "sc2_saving", "sc2_net",
        "sc3_gross", "sc3_saving", "sc3_net",
        "sc4_gross", "sc4_saving", "sc4_net"]

def main():
    src = HERE / "impact_ReCiPe_end.csv"
    if not src.exists():
        log(f"!! {src.name} not found — run recipe2016_rerun.py first."); return
    rows = list(csv.DictReader(src.open(encoding="utf-8")))
    units = sorted({r["unit"].strip() for r in rows})
    log(f"Read {len(rows)} endpoint categories; units present: {units}")
    unknown = [u for u in units if u not in AOP]
    if unknown:
        log(f"!! unit(s) not mapped to an area of protection: {unknown}")
        log("   These would be silently dropped, so ABORTING rather than reporting a partial")
        log("   total. If this is the ReCiPe 2008 endpoint (unit 'points'), the file has not")
        log("   been regenerated on ReCiPe 2016 yet.")
        return

    agg, members = {}, {}
    for r in rows:
        a = AOP[r["unit"].strip()]
        members.setdefault(a, []).append(r["category"])
        for c in COLS:
            agg.setdefault(a, {}).setdefault(c, 0.0)
            agg[a][c] += float(r[c])

    unit_of = {"human health": "DALY", "ecosystem quality": "species.yr", "resources": "USD2013"}
    with (HERE / "impact_ReCiPe_end_aop.csv").open("w", newline="", encoding="utf-8") as f:
        w = csv.writer(f)
        w.writerow(["area_of_protection", "n_categories"] + COLS + ["unit"])
        for a in ORDER:
            if a not in agg: continue
            w.writerow([a, len(members[a])] + [agg[a][c] for c in COLS] + [unit_of[a]])
    log(f"-> impact_ReCiPe_end_aop.csv")

    log("\nArea-of-protection damage totals (per 1 VCU, cradle-to-grave, characterised only):")
    for a in ORDER:
        if a not in agg: continue
        v = agg[a]; s1 = v["sc1"]
        log(f"  {a:<18} ({len(members[a])} categories)  Sc1 {s1:.5g} {unit_of[a]}")
        log(f"    net: " + " / ".join(f"{v[c]:.5g}" for c in ("sc2_net", "sc3_net", "sc4_net"))
            + "   (" + " / ".join(f"{100*(v[c]-s1)/s1:+.1f} %" for c in ("sc2_net", "sc3_net", "sc4_net")) + ")")
    log("\nNo normalisation, no weighting, no single score — by design (see the docstring).")
    log("Prioritisation of impact categories remains with EF 3.1 (impact_screening.csv).")

if __name__ == "__main__":
    try: main()
    except Exception: log("FATAL:\n" + traceback.format_exc())
    finally:
        try: LOG.write_text("\n".join(_l), encoding="utf-8")
        except OSError: pass
        print(f"\nLog: {LOG}")
