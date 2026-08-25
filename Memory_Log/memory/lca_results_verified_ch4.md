---
name: lca-results-verified-ch4
description: "[P] Every LCA number Chapter 4 uses, verified directly from the Outputs CSVs. Opened 2026-08-23 (Session 40), extended 2026-08-24 (Session 41) with the stage 1 to 4 shares, the stage 5 process contributions per scenario, the corrected Monte Carlo band ranges and the Sc3/Sc4 freshwater overlap. Carries the corrected reading of the 'net' ruling and the deterministic-versus-median defect."
type: project
---

# 🔴 0. THE "NET" RULING, CORRECTED. Read this before touching any results chart.

**Thiago, 2026-08-23:** *"he abolished the term, not the idea… Using the balance line, you show
explicit the gross value and add in the negative axis how much of emission you can create in credit."*

The ruling is **presentational, not methodological**. A single netted number is banned because it
reads as if an emission that already occurred had been removed. The same arithmetic is fine when it
is inspectable: gross above the axis, credit below, **balance line** across them.

**Three conditions:** the word is **"balance"**, never "net" · the caption must state that the credit
sits **outside the system boundary** · one clause on the APOS partial double count.
✅ **No recomputation.** Gross minus avoided reproduces the old `*_net` columns exactly, 0.00e+00.

# ✅ 1. MONOTONICITY. It holds in BOTH forms.

| Stated on | EF 3.1 | ReCiPe 2016 Midpoint |
|---|---|---|
| **Balance**, Sc1 > Sc2 > Sc3 > Sc4 | **25 of 25 rows** | **18 of 18** |
| **Avoided impact**, Sc2 < Sc3 < Sc4 | **25 of 25 rows** | **18 of 18** |

⚠ **EF 3.1 has 16 categories, not 25.** The 25 rows include 9 sub-indicators. The screening runs on
the 16. Say which is meant.

# 🔴 2. THE STRONGER CLAIM DOES NOT HOLD. Do not write it.

Distribution-level separation of the three avoided systems is **fully disjoint in only 10 of 25
rows**. In five categories the avoided impact is **negative at the 5th percentile**: human toxicity
cancer, human toxicity non-cancer, land use, water use. **None is in the reporting set.**

🔴 **Refined 2026-08-24, inside the reporting set:** the Sc3 and Sc4 avoided intervals are separated
in minerals (Sc3 p95 0.006091 against Sc4 p5 0.008271) and in climate (8.89 against 14.32), and
**OVERLAP in freshwater eutrophication** (Sc3 reaches 0.024275 kg P eq, Sc4 begins at 0.022911).
Sc2 to Sc3 is separated in all three. The four gross distributions overlap across their full range.
**This is now stated in 4.1.**

# 🔴 3. LIVE DEFECT: THE DETERMINISTIC GROSS VALUE IS NOT THE CENTRAL ESTIMATE

| Sc1 | Deterministic (EF 3.1) | MC p5 | MC p50 | MC p95 |
|---|---|---|---|---|
| **Climate change** | **73.4326 kg CO2 eq** | 70.02 | **91.33** | 128.10 |
| Eutrophication, freshwater | 0.115920 kg P eq | 0.0782 | **0.129911** | 0.2779 |
| Resource use, minerals and metals | 0.0187391 kg Sb eq | 0.017533 | 0.0188976 | 0.0206581 |

Ratios deterministic/median: climate **0.804**, freshwater 0.892, minerals 0.992.

🔴 **The claim must say "gross".** It is FALSE for the credits: Sc4 minerals deterministic 0.008870
against a median of 0.008870 (ratio 1.000), climate 0.975, freshwater 1.020.

# ✅ 4. THE EF 3.1 REPORTING SET, from `impact_screening.csv`

**Minerals and metals 72.45 %** (goal-pinned) · **Climate change 6.67 %** (cum **79.12 %**,
goal-pinned) · **Eutrophication, freshwater 6.58 %** (cum **85.70 %**).
⚠ Climate alone reaches 79.12 %, **below** the threshold. The third enters under the PEF
crossing-category rule. Write the mechanism, not "the top three exceed 80 %".

# ✅ 5. HEADLINE NUMBERS, EF 3.1, deterministic

| Category | Sc1 | Sc2 avoided | Sc3 avoided | Sc4 avoided |
|---|---|---|---|---|
| Minerals and metals (kg Sb eq) | 0.0187391 | 0.00187177 | 0.00602588 | 0.00887018 |
| Climate change (kg CO2 eq) | 73.4326 | 4.41898 | 8.16273 | 15.4315 |
| Eutrophication, freshwater (kg P eq) | 0.115920 | 0.00689894 | 0.0191428 | 0.0297611 |

**Gross at seven significant figures**, which is where the four scenarios separate:
minerals 0.01873912 / 0.01873964 / 0.01873949 / 0.01873952 · climate 73.43259 / 73.47943 /
73.40933 / 73.41032 · freshwater 0.1159196 / 0.1159353 / 0.1159295 / 0.1159373.

⚠ **Three percentage bases are live.** Avoided against Sc1 gross · avoided against Sc2 as multipliers
· balance against Sc1. **Basis 3 is deliberately not reported.**

# ✅ 5b. STAGE CONTRIBUTIONS, verified 2026-08-24 from `impact_stage_contributions.csv`

Shares of the **stage 1 to 4 subtotal** (a fourth basis, reconciled in the text):

| stage | minerals | climate | freshwater |
|---|---|---|---|
| S1 Materials and construction | 97.8433 % | 52.5384 % | 53.9435 % |
| S2 Hardware assembly | 0.4450 % | 0.6018 % | 0.4906 % |
| S3 Distribution | **0.0003 %** | 0.0381 % | **0.0016 %** |
| S4 Use phase | 1.7114 % | 46.8216 % | 45.5643 % |

Stages 1 to 4 as a share of the **Sc1 gross**: 99.9998 % · **99.7596 %** · 99.9981 %.
⚠ S2 and S3 are below the angular width a pie can render. The values are printed under each chart.

**Stage 5 as a share of each scenario's gross:** climate 0.2404 / 0.3040 / 0.2088 / 0.2101 % ·
minerals at most 0.0029 % · freshwater at most 0.0171 %.

# ✅ 5c. STAGE 5 PROCESS CONTRIBUTIONS, verified 2026-08-24

Shares of each scenario's own **stage 5** result. Sc1 holds 2 processes, Sc2 4, Sc3 and Sc4 5.

| process | Sc1 | Sc2 | Sc3 | Sc4 |
|---|---|---|---|---|
| *minerals* WEEE shredding | n/a | 86.69 | n/a | n/a |
| *minerals* copper smelter | n/a | 10.51 | 53.27 | 45.90 |
| *minerals* manual treatment | n/a | n/a | 33.44 | 30.77 |
| *minerals* dismantling electricity | n/a | n/a | 12.06 | 22.20 |
| *climate* waste plastic market | 96.55 | 76.83 | 49.85 | 49.53 |
| *freshwater* dismantling electricity | n/a | n/a | **66.25** | **80.32** |
| *freshwater* WEEE shredding | n/a | 91.58 | n/a | n/a |
| *freshwater* sanitary landfill | 82.44 | 2.60 | 0.28 | 0.17 |

Stage 5 totals: minerals 2.9974e-08 / 5.48533e-07 / 4.01573e-07 / 4.36362e-07 · climate 0.17652 /
0.223367 / 0.153265 / 0.154252 · freshwater 2.16613e-06 / 1.78596e-05 / 1.20436e-05 / 1.98662e-05.

🔴 **Dismantling electricity is the largest freshwater contributor in Sc3 and Sc4, and it rests on
an `[A]` assumption of 0.01 and 0.02 kWh.** Named as an assumption in the text.
🔴 **Sc1's two climate values were TRANSPOSED in the openLCA export.** Corrected: landfill
0.00609064, waste plastic 0.170429. The sum is unchanged, which is why the total check passed.

# ✅ 5d. MONTE CARLO BAND WIDTHS, (p95 − p5)/p50, verified 2026-08-24

| system | minerals | climate | freshwater |
|---|---|---|---|
| gross, range across the four | **16.54 to 17.73 %** | **62.23 to 65.96 %** | **153.65 to 160.90 %** |
| Sc2 avoided | 2.22 % | 11.97 % | 57.02 % |
| Sc3 avoided | 5.15 % | 10.41 % | 50.68 % |
| Sc4 avoided | 15.60 % | 20.07 % | 57.57 % |

⚠ **Two of his written ranges were rounded to integers in a way that raised the floor above the
lowest value in the set** (he had "17 to 18" and "154 to 161"). Corrected in the text to one decimal.
**Sc4 p5 to p95 credit:** 0.008271 to 0.009654 kg Sb eq · 14.32 to 17.50 kg CO2 eq · 0.022911 to
0.039715 kg P eq.

# 🔴 6. THE ReCiPe CROSS-CHECK. Basis superseded 2026-08-24.

⚠ **The Sc1-relative pp table that stood here is NO LONGER THE REPORTED BASIS.** Thiago rejected
Sc1-relative comparison on 2026-08-24. The block is now built on **ratios against Sc2**.
**See [[recipe_cross_check_verified]] for the whole block**, including the 15-pair mapping, the
normalisation-reference mechanism, the independence limit on freshwater, and the false memory claim
about units predicting agreement.

Kept for reference, avoided impact as a share of each method's own Sc1 burden: climate agrees to
within 0.05 pp; minerals diverges **8.40 pp at Sc4**.
Absolute Sc1 differs in unit by construction: climate 73.4326 kg CO2 eq against 74.3996;
minerals 0.0187391 kg Sb eq against 2.20278 kg Cu eq.

# 📁 FILES

All results under `AR_DPP/LCA_Analysis/Outputs/`.
⚠ CSV headers read `Resource use fossils` and `Resource use minerals and metals`, without the comma
the framework text uses.
⚠ `Outputs/2_eol_scenarios/contribuition_tree.xlsx` is **EMPTY**.
⚠ `s5_build_log.txt` is **stale for Sc1**: it prints 0.528/0.132 where the database carries
0.5875/0.0725.

Related: [[recipe_cross_check_verified]], [[ch4_findings_progress]], [[lca_methodology_3_3]],
[[results_chapter_start_here]], [[thesis-schedule]]
