---
name: ch4-findings-progress
description: "[P] Live record of Chapter 4 Findings, opened 2026-08-23 (Session 40). Section 4.1's structure and running order as the author fixed it, the accepted prose, the verified table and figure numbering, the author's rulings including two taken against advice, and the METHODOLOGY CONTRADICTION on how Sc4 may be reported."
type: project
---

# ✅ STRUCTURE, fixed by the author 2026-08-23

**4.1 LCA Results · 4.2 AR DPP Results · 4.3 AR DPP Test results.**
**4.1 has NO subsections.** He asked for one continuous section and rejected splitting it.

**Running order inside 4.1, as of the close of Session 40:**

1. Section opener, seven product systems, EF 3.1 primary and ReCiPe as cross-check
2. Screening: sixteen categories, normalisation and weighting, the PEF rule
3. The ranking is performed on Sc1, the baseline
4. **Table 7** the screening · **Figure 18** the threshold chart · the three categories, 85.70 %
5. **Figure 19** and **Table 8** gross per scenario · the gross barely changes
6. Stages 1 to 4 identical, the difference is stage 5 · **Table 9** stage 5 shares
7. **Figure 20** avoided impact · the avoided values · the Sc2 comparison
8. **Figure 21** gross above and avoided below with the balance line · the balance falls
9. **Figure 22** Monte Carlo box plots · the bands · the deterministic-versus-median disclosure
10. 🔴 **ReCiPe cross-check LAST**, moved there by him on 2026-08-23

⚠ **ReCiPe after Monte Carlo creates a reading problem:** the cross-check runs on characterised
deterministic values and lands right after the paragraph saying those values are off centre.
**Fix agreed as the cheaper one: the ReCiPe block opens by naming its level**, which 3.3.3 requires
anyway. Do not reorder.

# 🔴 THE ReCiPe BLOCK, the open decision for the next session

He proposed: rank all 16 ReCiPe categories, take the top set to 85 % cumulative, compare those with
EF 3.1. **Computed 2026-08-23. The proposal has a good half and a costly half.**

✅ **THE GOOD HALF, take it.** The ordering holds inside ReCiPe's OWN top set, which is a stronger
robustness argument than comparing only the EF-selected categories:

| ReCiPe top category | share of its profile | Sc2 / Sc3 / Sc4 avoided, % of own Sc1 |
|---|---|---|
| Freshwater ecotoxicity | 37.77 % | 10.64 / 31.21 / 45.06 |
| Marine ecotoxicity | 29.03 % | 10.49 / 30.90 / 44.66 |
| Human carcinogenic toxicity | 24.52 % | 14.78 / 20.90 / 26.79 |

🔴 **THE COSTLY HALF, publishing the ranking. Three prices:**

1. **Marine ecotoxicity has NO EF counterpart.** 29.03 % of ReCiPe's profile, unpaired in EF 3.1.
2. **Human carcinogenic toxicity diverges by −11.28 pp at Sc4** (ReCiPe 26.79 % vs EF 38.07 %),
   which is LARGER than the minerals divergence already in the block.
3. **The ranking exposes mineral scarcity at 0.0005 %**, contradicting the EF selection that block
   (a) spends three paragraphs justifying, in a chapter that cannot explain it.

**Claude's recommendation: keep the robustness test, drop the ranking table.** Three sentences carry
the whole argument. Recommendation given, decision NOT yet taken. **Ask him.**

# ✅ THE EF vs ReCiPe CROSS-CHECK, verified

Avoided impact as a share of each method's own Sc1 gross. Delta is ReCiPe minus EF.

| category | Sc2 | Sc3 | Sc4 |
|---|---|---|---|
| Climate change (kg CO2 eq both) | −0.01 pp | 0.00 pp | +0.04 pp |
| Resource use, minerals and metals (kg Sb eq vs kg Cu eq) | +1.15 pp | −4.67 pp | **−8.40 pp** |
| Eutrophication, freshwater (kg P eq both) | 0.00 pp | 0.00 pp | 0.00 pp |

Mean absolute disagreement 1.59 pp, maximum 8.40 pp. Ordering Sc2 < Sc3 < Sc4 holds under both.

🔴 **THE PAIR TABLE PREDICTS THE DISAGREEMENT.** Only **4 of 15 pairs share a unit**. The two
reporting categories that agree share theirs; minerals does not, kg Sb eq against kg Cu eq, two
different scarcity models. That is why the pairing table earns a place in the thesis.
⚠ EF's "Eutrophication terrestrial" has no ReCiPe counterpart. Three ReCiPe categories are unpaired:
marine ecotoxicity, terrestrial ecotoxicity, ozone formation for terrestrial ecosystems.

# 🔴🔴 A METHODOLOGY CONTRADICTION, OPEN AND UNRESOLVED

**Section 3.3.3 says, verbatim from the .docx:** *"For the fourth scenario it is not optional. Its
functional yield is unsourced, so that scenario may be reported only as a band and never as a single
value."*

**Section 4.1 currently reports Sc4 as single values throughout**, and Figures 20 and 21 plot it as
single bars. **Two chapters contradict each other.**

**The fix, three clauses in the avoided-impact paragraph:**

| Sc4 avoided | point value | simulated p5 to p95 |
|---|---|---|
| Resource use, minerals and metals | 0.008870 kg Sb eq | 0.008271 to 0.009654 |
| Climate change | 15.43 kg CO2 eq | 14.32 to 17.50 |
| Eutrophication, freshwater | 0.029761 kg P eq | 0.022911 to 0.039715 |

**Do NOT soften the 3.3.3 sentence.** It is a self-imposed rigor commitment and retreating from it
under pressure reads worse than the edit.

# 🔴 NUMBERING, read from the .docx. Read it again, never assert from memory.

Chapter 3's body closes at **Table 6** and **Figure 17**. Chapter 4 so far: Tables **7, 8, 9**,
Figures **18, 19, 20**, plus **Figure 21** (balance) and **Figure 22** (Monte Carlo) to come.
Appendix II CIRPASS is now Table 10 and shifts again with every body table added.
**Appendix VIII** is new, the Monte Carlo percentile summary.
⚠ **LIST OF TABLES and LIST OF FIGURES go stale on every insert. Refresh fields before reading them.**
⚠ **The 3.5 measures table has no caption anywhere in the body.** Captions jump Table 6 to Table 7.
Check section 3.5.

# ⚠ THREE PERCENTAGE BASES ARE LIVE IN 4.1. Never mix two in one table.

1. **Avoided as a share of Sc1 gross** — 9.99 / 32.16 / 47.34 minerals · 6.02 / 11.12 / 21.01 climate
2. **Avoided against Sc2**, expressed as multipliers — 3.22x / 4.74x minerals · 1.85x / 3.49x climate
3. **Balance against Sc1** — −9.99 / −32.15 / −47.33 minerals · −5.95 / −11.15 / −21.04 climate

🔴 **Basis 3 is DELIBERATELY NOT REPORTED.** It differs from basis 1 only in the second decimal, and
printing both reads as a typo. The balance is reported in absolute values only.

# ✅ WHAT IS WRITTEN AND ACCEPTED

Section 4.1 is drafted through the Monte Carlo block. Only the ReCiPe block remains. The full
annotated revision, paragraph by paragraph with the reason for each change, is at
`MASTER THESIS/MAIN PAPER/section_4_1_revised.md`.

**Data workbooks delivered, all in MAIN PAPER:** `EF31_normalization_weighting_screening.xlsx` ·
`block_b_gross_burden_and_stages.xlsx` · `block_b_body_tables.xlsx` ·
`block_c_avoided_impact.xlsx` · `appendix_monte_carlo_summary.xlsx`.
**Notebook cell** for Figure 21 at `AR_DPP/LCA_Analysis/LCA_Notebook/cell_balance_chart.py`.
**Route cleanup list** at `MAIN PAPER/route_to_scenario_checklist.md`.

# 🔴 TWO DECISIONS TAKEN AGAINST ADVICE. Recorded, not to be reopened.

1. **The chapter opener omits that the two study blocks used two different prototype builds.**
   Closed with *"I will keep like this, dont argue, I know the consequences."* **4.3 must therefore
   carry the two-build fact itself**, or the separate reporting of the blocks has no stated reason.
2. **The sixteen-category ordering statement was cut from 4.1.** His reasoning: the framework and the
   explorer are a study draft, the thesis reports only what survives his own filter. Consequence to
   accept: the ordering claim is stated for three categories only, and the framework's own line
   *"All 16 remain shown"* no longer describes the thesis.

# ⚠ TWO RECURRING FAILURE MODES

**He drops details when pasting.** The ISO sentence, the red-beside-green flag, both Andreasi Bassi
page numbers, and the ordering paragraph. **Hand him citations inline in the sentence, never as a
note beside it.**

**Excel mangles decimals on paste.** His first Table 7 carried `72.452.384` for `72.45`; the decimal
point became a thousands separator, corrupting 8 of 16 share values and all 16 cumulative values.
**Deliver every table as .xlsx, never as pasted text.**

# ⚠ NOTEBOOK ITEMS STILL OPEN

`LCA_explorer.ipynb` cell 10's title still reads "saving impact per scenario, before recovery
credits", which regenerates the wrong title on the next run. Cell 14 is built on "net" in three
places and contains an em dash. Cell 32's `red_pct` computes the balance change against Sc1, which
is percentage basis 3 and is not reported. **Delete cells 14, 32, 35 and 36** once the new cells are in.
Cell 10's comment claims 8 s.f. separates all four scenarios; that was derived on the saving
columns. **On the gross columns it is 7.**
⚠ `Outputs/2_eol_scenarios/contribuition_tree.xlsx` is **empty**, one sheet, dimensions A1:A1.

Related: [[lca_results_verified_ch4]], [[results_chapter_start_here]], [[research_questions_final]],
[[voice_and_verification_rules]], [[thesis-schedule]], [[lca_methodology_3_3]]
