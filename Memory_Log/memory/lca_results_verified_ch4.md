---
name: lca-results-verified-ch4
description: "[P] The re-verifications required before Chapter 4 could be written, computed 2026-08-23 (Session 40) directly from the Outputs CSVs. Carries the CORRECTED reading of the supervisor's 'net' ruling (the term was abolished, the idea was not, and the balance line is permitted), the monotonicity result in both its forms, the EF screening reporting set, the ReCiPe cross-check divergence on minerals, and the deterministic-versus-Monte-Carlo defect."
type: project
---

# 🔴 0. THE "NET" RULING, CORRECTED. Read this before touching any results chart.

**Thiago, 2026-08-23:** *"he abolished the term, not the idea, the idea is to do a net, but doing a
true net it looks like that you are removing the emission of something that already happen the
emission, what is not very accurate with the reality. Using the balance line, you show explicit the
gross value and add in the negative axis how much of emission you can create in credit."*

**So the ruling is presentational, not methodological.**

- ❌ Wrong reading, carried in [[lca_methodology_3_3]] until today: "net is gone, the quantity has no
  name, a balance line reintroduces the abolished quantity."
- ✅ Correct reading: **a single netted number is banned because it reads as if an emission that
  already occurred had been removed. The same arithmetic is fine when it is inspectable**, that is,
  gross plotted above the axis, credit plotted below, and a **balance line** drawn across them.

**The balance line IS permitted.** The supervisor's own reference chart carries one. The question
recorded as "raised and unanswered" on 2026-08-21 is now ANSWERED.

**Three conditions:**

1. **The word is "balance".** Never "net". `LCA_framework_v4.md`'s FINAL RESULTS table headers
   ("Sc2 net / Sc3 net / Sc4 net") must be restated to it.
2. 🔴 **The caption must state that the credit sits OUTSIDE the system boundary.** The avoided
   burden leaves the boundary and does not return to this unit, per the Figure 7 design ruling in
   [[lca_methodology_3_3]]. The balance line is a **comparison device between scenarios**, not this
   unit's footprint.
3. **One clause on the APOS partial double count**, which now sits inside the line.

✅ **NO RECOMPUTATION.** Gross minus avoided reproduces the existing `*_net` columns exactly,
difference 0.00e+00 in every row of `impact_EF31.csv` and `impact_ReCiPe_mid.csv`.

# ✅ 1. MONOTONICITY. It holds in BOTH forms.

Computed 2026-08-23 from `Outputs/3_impact_assessment/impact_EF31.csv` and `impact_ReCiPe_mid.csv`.

| Stated on | EF 3.1 | ReCiPe 2016 Midpoint |
|---|---|---|
| **Balance**, Sc1 > Sc2 > Sc3 > Sc4 | **25 of 25 rows** | **18 of 18** |
| **Avoided impact**, Sc2 < Sc3 < Sc4 | **25 of 25 rows** | **18 of 18** |

⚠ **EF 3.1 has 16 categories, not 25.** The 25 rows include 9 sub-indicators: climate change split
into biogenic, fossil and land use; freshwater ecotoxicity and both human toxicity families split
into organics and inorganics. **The screening runs on the 16.** Say which is meant.

Open item 6 of [[lca_methodology_3_3]] is CLOSED.

# 🔴 2. THE STRONGER CLAIM DOES NOT HOLD. Do not write it.

Tested whether the three avoided-impact DISTRIBUTIONS separate (Sc2s p95 < Sc3s p5 and
Sc3s p95 < Sc4s p5) from `Outputs/4_monte_carlo/mc_summary.csv`, n = 1,000 per system.

**Fully disjoint in only 10 of 25 rows.** Fifteen overlap: freshwater ecotoxicity, both human
toxicity families, land use, water use, ozone depletion, particulate matter, ionising radiation.

🔴 **In five categories the avoided impact is NEGATIVE at the 5th percentile**, so some parameter
draws make the recovery scenario cost more than it avoids: human toxicity cancer (Sc3 −4.10e−07,
Sc4 −5.07e−07 CTUh), human toxicity non-cancer (Sc3 −4.96e−05, Sc4 −6.23e−05 CTUh), land use
(Sc3 −69.31, Sc4 −90.10 pt), water use (Sc3 −561.79, Sc4 −681.78 m³ world eq). **None is in the
reporting set.** It is a burden-shifting result.

# 🔴 3. LIVE DEFECT: THE DETERMINISTIC VALUE IS NOT THE CENTRAL ESTIMATE

| Sc1 | Deterministic (EF 3.1) | MC p5 | MC p50 | MC p95 | MC mean |
|---|---|---|---|---|---|
| **Climate change** | **73.4326 kg CO2 eq** | 70.02 | **91.33** | 128.10 | 94.38 |
| **Resource use, fossils** | **1019.35 MJ** | 969.79 | **1272.81** | 1804.99 | 1320.97 |
| Resource use, minerals and metals | 0.0187391 kg Sb eq | 0.017533 | 0.0188976 | 0.0206581 | 0.0189501 |
| Eutrophication, freshwater | 0.115920 kg P eq | 0.078244 | 0.129911 | 0.277854 | — |

**73.4326 sits near the 6th percentile of its own simulation**, roughly 20 % below the median. The
thesis reports it and the prototype displays a figure from the same run. **Chapter 4 must report
both and name which is which.** The explanation goes to the Discussion.

**[A] Likely driver, NOT verified:** the use-phase electricity. Stage 4 is 46.71 % of Sc1's climate
total; Equation 1 gives 66.2 kWh as the mode against a declared triangular range of 54 to 189 kWh,
which is right-skewed. Scaling stage 4 to ≈100 kWh closes the 17.9 kg gap almost exactly, and
minerals escapes because only 1.71 % of it is use-phase. **Confirm before writing it.**

# ✅ 4. MONTE CARLO BAND WIDTHS, (p95 − p5) / p50

| | Sc1 gross | Sc2 gross | Sc3 gross | Sc4 gross | Sc2 credit | Sc3 credit | Sc4 credit |
|---|---|---|---|---|---|---|---|
| Minerals and metals | 16.54 % | 17.05 % | 17.73 % | 17.30 % | 2.22 % | 5.15 % | **15.60 %** |
| Climate change | 63.59 % | 65.96 % | 65.72 % | 62.23 % | 11.97 % | 10.41 % | **20.07 %** |
| Eutrophication, freshwater | 153.65 % | 160.90 % | 154.91 % | 154.40 % | 57.02 % | 50.68 % | **57.57 %** |

⚠ **"Sc4 has the widest band" is strong in minerals and climate and a TIE in freshwater
eutrophication** (57.57 against 57.02, half a point). State it per category.
The cause is in 3.3.3 already: Sc4's functional yield is declared over 0.50 to 0.90 and is unsourced.

# ✅ 5. THE EF 3.1 REPORTING SET, from `impact_screening.csv`

**Resource use, minerals and metals 72.45 %** (cum 72.45, goal-pinned) · **Climate change 6.67 %**
(cum **79.12 %**, goal-pinned) · **Eutrophication, freshwater 6.58 %** (cum **85.70 %**).

⚠ **Climate alone reaches 79.12 %, which is BELOW the threshold.** The third category enters under
the PEF crossing-category rule. Write the mechanism, not "the top three exceed 80 %".
✅ **Both goal-pinned categories fall inside on their own merit.**
Next below the line: fossils 4.25 %, acidification 2.09 %, freshwater ecotoxicity 1.60 %.

# ✅ 6. HEADLINE NUMBERS, EF 3.1, deterministic

| Category | Sc1 | Sc2 avoided | Sc3 avoided | Sc4 avoided |
|---|---|---|---|---|
| Minerals and metals (kg Sb eq) | 0.0187391 | 0.00187177 (9.99 %) | 0.00602588 (32.16 %) | 0.00887018 (**47.34 %**) |
| Climate change (kg CO2 eq) | 73.4326 | 4.41898 (6.02 %) | 8.16273 (11.12 %) | 15.4315 (**21.01 %**) |
| Eutrophication, freshwater (kg P eq) | 0.11592 | 0.00689894 (5.95 %) | 0.0191428 (16.51 %) | 0.0297611 (25.67 %) |

**Balance against Sc1:** climate −5.95 / −11.15 / −21.04 %; minerals −9.99 / −32.15 / −47.33 %;
freshwater eutrophication −5.94 / −16.51 / −25.66 %.
**Balance absolute:** climate 73.4326 → 69.0605 → 65.2466 → 57.9788.

**Against Sc2 as multipliers:** minerals 3.22x and 4.74x · climate 1.85x and 3.49x · freshwater
eutrophication 2.77x and 4.31x.

**Gross at 7 significant figures** (the first precision separating all four in every panel; minerals
needs 7, freshwater 6, climate 5):
minerals 0.01873912 / 0.01873964 / 0.01873949 / 0.01873952 ·
climate 73.43259 / 73.47943 / 73.40933 / 73.41032 ·
freshwater 0.1159196 / 0.1159353 / 0.1159295 / 0.1159373.

⚠ **Gross is NOT invariant in the two human toxicity categories**, where the spread is 5.59 % and
3.89 % and **Sc3 gross exceeds Sc4 gross**. It is under 0.100 % in the other fourteen.

**Stage shares of Sc1 (4 dp so nothing reads as zero):**
minerals S1 97.8432, S2 0.4450, S3 0.0003, S4 1.7114, S5 0.0002 ·
climate S1 52.4121, S2 0.6004, S3 0.0380, S4 46.7091, S5 0.2404 ·
freshwater eutrophication S1 53.9425, S2 0.4906, S3 0.0016, S4 45.5635, S5 0.0019.

**Stage 5 per scenario, DERIVED** as gross minus the sum of stages 1 to 4. The same subtraction
reproduces the printed `s5_sc1` exactly (0.00e+00) in all three categories, but the premise that
stages 1 to 4 are identical across scenarios is **NOT independently verified**:
minerals 0.0002 / 0.0029 / 0.0021 / 0.0023 % of each scenario's gross ·
climate 0.2404 / 0.3040 / 0.2088 / 0.2101 % · freshwater 0.0019 / 0.0154 / 0.0104 / 0.0171 %.
⚠ The absolute stage 5 burdens differ BETWEEN scenarios by 46 % in climate and a factor of eighteen
in minerals. "The scenarios' end-of-life processes are almost the same" is FALSE; "stage 5 is
negligible against the total" is TRUE.

# 🔴 7. THE ReCiPe CROSS-CHECK DIVERGES ON MINERALS

`LCA_framework_v4.md` line 709 labels `impact_screening_ReCiPe_mid.csv` **"Not a prioritisation
table, evidence for §4.2.1"**, and line 646 fixes ReCiPe's role as **characterisation robustness, at
characterised level only.**

Avoided impact as a share of each method's own Sc1 burden, delta as EF minus ReCiPe:

| Pair | Sc2 | Sc3 | Sc4 |
|---|---|---|---|
| EF *Climate change* vs ReCiPe *Global warming* | +0.01 pp | 0.00 pp | −0.04 pp |
| EF *minerals and metals* vs ReCiPe *mineral resource scarcity* | −1.15 pp | +4.67 pp | **+8.40 pp** |
| EF *fossils* vs ReCiPe *fossil resource scarcity* | −0.27 pp | −0.61 pp | −1.14 pp |
| EF *Eutrophication, freshwater* vs ReCiPe *Freshwater eutrophication* | 0.00 pp | 0.00 pp | 0.00 pp |

**Climate and freshwater eutrophication agree to two decimals. Minerals does not**, and minerals is
the category carrying 72.45 % of the weighted footprint. **The ordering holds under both methods;
the magnitude does not.**

🔴 **Only 4 of the 15 category pairs share a unit**, and the pairs that agree are exactly the pairs
that share one. Minerals is kg Sb eq against kg Cu eq, two different scarcity models. **The pair
table predicts which comparisons will agree**, which is why it is worth printing.

# 📁 FILES

All results under `AR_DPP/LCA_Analysis/Outputs/`. The six-block plan for section 4.1 is at
`MASTER THESIS/MAIN PAPER/ch4_1_lca_results_scaffold.md`; the revised section at
`section_4_1_revised.md`.
⚠ CSV headers read `Resource use fossils` and `Resource use minerals and metals`, without the comma
the framework text uses.

# ✅ CITATIONS TRACED FIRST-HAND 2026-08-23

- **Andreasi Bassi et al. (2023), EUR 31414 EN, printed p. 5** for the sixteen midpoint categories
  and for the definitions of normalisation and weighting.
  ⚠ It does **NOT** contain the ≥80 % rule; all 57 pages searched.
  ⚠ Its printed p. 3 carries the typo "The Environmental Product Environmental Footprint (PEF)".
  **PEF is Product Environmental Footprint**; OEF is Organisation Environmental Footprint.
- **Commission Recommendation (EU) 2021/2279 of 15 December 2021, Annex I, section 6.3.1**,
  "Procedure to identify the most relevant impact categories", **OJ page L 471/223**, OJ L 471 of
  30 December 2021. ⚠ Section, heading and threshold confirmed from two independent reads; **the
  exact sentence wording is NOT**. Paraphrase and cite, do not quote. ⚠ **Not yet on disk.**
- 🔴 **Attribute ONLY the 80 % rule to the Recommendation.** The crossing-category inclusion and the
  goal-pinned categories are this study's own additions.

Related: [[ch4_findings_progress]], [[lca_methodology_3_3]], [[results_chapter_start_here]],
[[thesis-schedule]]
