# SESSION RESUME — read me first in a fresh chat
_Updated: 2026-07-28, end of Session 19. **The openLCA numerical work is COMPLETE**, incl.
the ReCiPe 2008→2016 migration. Next session switches workstream to **AR / ReBuilt v2.0**;
this document remains the LCA handoff for the writing and figure work._

## Status: DONE
- openLCA v4 model: S1–S4 + four EoL scenarios, all literature-anchored or declared [A].
- Deterministic impact suite: EF 3.1 + **ReCiPe 2016** Midpoint (H) + Endpoint (H) (openLCA
  pack — the ecoinvent `ei - ReCiPe …` packs were ReCiPe **2008**; migrated 2026-07-28) +
  EF normalization/weighting screening. `Outputs\3_impact_assessment\`.
- Monte Carlo: n = 1000 per system, all 7 systems, EF 3.1, GUI route.
  `Outputs\4_monte_carlo\`.
- Everything documented in `Docs\LCA_framework_v4.md` (§4 concepts, §5 all result files).
- Thesis-ready conclusions + defense arguments: project memory [[lca-findings-for-writing]].

Remaining (non-blocking): VCU_BOM_v4.xlsx regeneration; optional GUI figures (contribution
tree + Sankey, Sc1 + Sc3, EF 3.1 — pictures of the same numbers, GUI-only).

---

# VISUALIZATION BRIEF (read before plotting anything)

## Which file for which chart
| Chart | Source | Columns |
|---|---|---|
| Scenario comparison (THE headline) | `3_impact_assessment\impact_EF31.csv` | `sc1`, `sc2_net`, `sc3_net`, `sc4_net` |
| Gross vs credit decomposition | same | `*_gross`, `*_saving` (net = gross − saving) |
| Category prioritization / why minerals | `impact_screening.csv` | `share_pct`, `cum_pct`, `in_80pct_set` |
| Damage view (3 AoP totals) | `impact_ReCiPe_end_aop.csv` | `sc1`, `sc*_net` per area of protection |
| Method cross-check | `impact_ReCiPe_mid.csv` | mineral resource scarcity (Cu-eq) vs EF minerals (Sb-eq) |
| Normalisation disjointness (methodology) | `impact_screening.csv` + `impact_screening_ReCiPe_mid.csv` | `share_pct` — see rule 5 |
| Uncertainty bands | `4_monte_carlo\mc_summary.csv` | rows `Sc2s/Sc3s/Sc4s` = the CREDITS |
| Raw distributions (histograms, violins) | `4_monte_carlo\mc_raw_<key>.csv` | 1000 rows × 25 categories |

## FIVE RULES — violating these produces a wrong chart (rules 4–5 below the chart ideas)
1. **Never plot `mc_net.csv` means as a scenario comparison.** Each system was simulated
   independently, so gross systems that are deterministically identical (73.479 / 73.409 /
   73.410 kg CO₂-eq) drew MC means 2.54 kg apart by sampling luck. Result: the true Sc2→Sc3
   climate gap of 3.81 kg shows as only 1.24 kg. It understates the thesis result by two
   thirds. `mc_net.csv` = within-scenario spread only.
2. **Never mix deterministic and MC numbers on the same axis.** Deterministic = mode/central
   (Sc1 climate 73.4); MC mean = 94.4 because the use-phase triangle (54–66.2–189 kWh) is
   right-skewed. Scenario bars → deterministic. Error bars → credit distributions. Label which
   is which in every caption.
3. **Do not chart MC bands for the quarantined categories** (ecoinvent pedigree artefacts, not
   model results): water use (CV 3484 %), human toxicity cancer/non-cancer (1100–1850 %, sign
   flips), ionising radiation (113 %), land use (57 %). Deterministic values for these are fine.

## The uncertainty story to visualize (the strong result)
Absolute footprints carry ~±20 %, but the scenario-differentiating CREDITS are tight and their
p5–p95 bands never touch in minerals, climate and fossils:

| Saving (n=1000) | Sc2 | Sc3 | Sc4 | CV |
|---|---|---|---|---|
| Climate [kg CO₂-eq] | 4.65 ± 0.17 | 8.42 ± 0.26 | 15.85 ± 0.99 | 3–6 % |
| Minerals [kg Sb-eq] | 0.00187 ± 0.00001 | 0.00595 ± 0.00010 | 0.00890 ± 0.00043 | 0.7–4.8 % |
| Fossils [MJ] | 61.7 ± 2.6 | 113.4 ± 4.2 | 212.2 ± 13.6 | 3.7–6.4 % |

Sc4 p5 sits 1.6× / 1.4× / 1.6× above Sc3 p95. Caption line: *"absolute values carry ±20 %,
the scenario ranking does not."*
**Honest exception to show, not hide:** in freshwater eutrophication the Sc3/Sc4 credit bands
overlap slightly (Sc3 p95 0.0243 vs Sc4 p5 0.0229 kg P-eq); means still ordered.

## Headline numbers (deterministic, for labels)
- EF 3.1 climate: 73.4 → 69.1 / 65.2 / 58.0 kg CO₂-eq (−6.0 / −11.1 / −21.0 %)
- EF 3.1 minerals & metals: 0.0187 → 0.0169 / 0.0127 / 0.0099 kg Sb-eq (−10.0 / −32.2 / −47.3 %)
- **ReCiPe 2016 endpoint = three AoP damage totals** (the single score is DEAD — the old
  11.25 → 8.20 pt figure must not be quoted anywhere): human health 3.318e-4 DALY →
  −9.4 / −21.1 / −32.5 % · ecosystem quality 4.657e-7 species·yr → −7.4 / −15.9 / −25.8 % ·
  resources 3.714 USD2013 → −9.1 / −17.2 / −29.2 %
- ReCiPe 2016 midpoint: global warming −21.1 % (EF climate −21.0 %), mineral resource
  scarcity −38.9 %; monotonic in all 18 categories
- Screening: minerals & metals = 72.5 % of weighted footprint, climate 6.7 %, freshwater
  eutrophication 6.6 % (reporting set = those three, 85.7 % cum)
- Ordering Sc1 > Sc2 > Sc3 > Sc4 monotonic in every category of every method.

## Chart ideas that carry the thesis argument (not just the data)
- **Screening Pareto** (minerals 72.5 % dominating) → justifies the DPP as a critical-raw-
  materials instrument with the EU's own weighting arithmetic.
- **Stacked gross/credit bars** → shows EoL comparison is a *credits* game (gross sides are
  nearly identical; Sc1 ≈ Sc2 gross).
- **9 g of reused components = 1.4 % of mass but ~45 % of Sc4's climate saving** → the
  burden-vs-mass contrast; the single strongest DPP argument in the whole LCA.
- **Two-panel climate vs minerals** → reuse pays in carbon, dismantling-for-smelting pays in
  minerals; the two interventions answer different questions.

## RULE 5 (added 2026-07-28) — ReCiPe normalisation is NOT a prioritisation tool
ReCiPe's World-2010 references put **mineral resource scarcity at 0.0 %** and toxicity at ~90 %
(the VCU's 2.2 kg Cu-eq against a 120 000 kg Cu-eq per-capita reference). Factors verified
against the published table — this is real ReCiPe behaviour, not an error. Its endpoint NW set
is midpoint normalisation in disguise (points ÷ weight = midpoint person-equivalents), so the
single score was dropped. Prioritisation stays with EF 3.1. `impact_screening_ReCiPe_mid.csv`
is EVIDENCE for framework §4.2.1 (figure R3), never a prioritisation chart. Three screening
charts → three different axis labels: EF "share of weighted footprint", ReCiPe midpoint
"share of normalised impact", ReCiPe endpoint — no screening chart at all.

## ReCiPe chart set — already built and tested
`LCA_Notebook\recipe_notebook_cells.py` holds four ready-to-paste cells in the notebook's own
style, executed end-to-end against the real CSVs: **R1** method-agreement dumbbell (mean
disagreement 3.2 pp over 15 paired indicators), **R2** AoP damage panels, **R3** the
disjointness figure (minerals 1st at 72.5 % under EF vs 18th of 18 at 0.000 % under ReCiPe),
**R4** optional 18-category heatmap, plus table **T1**. Do NOT mirror the EF chapter with
ReCiPe scenario bars — ReCiPe answers robustness, not prioritisation.

---

## Key docs & folder layout
- `Docs\LCA_framework_v4.md` — THE methodology doc (single source of truth).
- `Docs\BOM_v4.md` (v4.1, 660 g closed) — BOM source of truth.
- Scripts\ and Outputs\ mirrored subfolders: `0_utilities`, `1_stage_builds`,
  `2_eol_scenarios`, `3_impact_assessment`, `4_monte_carlo`.
- `LCA_Notebook\LCA_explorer.ipynb` — figures; `recipe_notebook_cells.py` — ReCiPe cells.
- Notion Session Log current through Session 19 (2026-07-28). One duplicate row still
  numbered 18 needs a manual delete (content already merged into the surviving row).

## Standing session protocol
"Hi Claude, wake up, new session" → Notion sync FIRST (Session Log `bf1b6c9d-d84e-429a-b977-6c4ae2d1bd11`,
Task Tracker `3e923b6b-f16c-498c-9e02-3e24acf48db9`), then short status, then ask what to work
on. "End of the session" → real-work report + ONE Session Log row. NEVER auto-create Task
Tracker rows unless explicitly asked.

## Deadlines
Thesis complete Aug 7 · submit Aug 18. Calendar ahead: ReBuilt v2.0 execution Jul 28–29,
Introduction writing Jul 30–Aug 1, controlled lab studies Jul 31–Aug 1, Literature Review
Aug 2–3.
