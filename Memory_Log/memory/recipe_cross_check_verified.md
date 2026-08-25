---
name: recipe-cross-check-verified
description: "[P] The whole ReCiPe 2016 Midpoint cross-check as computed and settled 2026-08-24 (Session 41). The ratio basis that replaced the Sc1-relative one, the full 15-pair mapping and how it was rebuilt, the normalisation-reference mechanism behind the 0.0005 percent ranking, the independence limit on freshwater, and the two Methodology edits the block requires."
type: project
---

# 🔴 THE BASIS CHANGED. Never use Sc1-relative percentages for this block again.

**Thiago, 2026-08-24:** he rejected the LCA_explorer charts *"because there I am using the 'net'
method, where show the performance between the Sc2, Sc3, Sc4 with Sc1."*

The cross-check is now built on **ratios against Sc2, taken inside each method**. Ratios are
dimensionless, so the kg Sb eq against kg Cu eq problem disappears, and Sc2 is already the reference
for the scenario comparison in 4.1, so no new percentage basis enters the chapter.

⚠ [[lca_results_verified_ch4]] §6 still carries the old Sc1-relative pp table. **It is not wrong,
it is no longer the reported basis.** Do not paste it into the thesis.

# ✅ THE RATIOS, verified from impact_EF31.csv and impact_ReCiPe_mid.csv

| pair | method | Sc3 / Sc2 | Sc4 / Sc2 |
|---|---|---|---|
| minerals / mineral resource scarcity | EF 3.1 | 3.219 | 4.739 |
| minerals / mineral resource scarcity | ReCiPe | **2.467** | **3.494** |
| climate change / global warming | EF 3.1 | 1.847 | 3.492 |
| climate change / global warming | ReCiPe | 1.850 | 3.503 |
| eutrophication, freshwater / freshwater eutrophication | EF 3.1 | 2.775 | 4.314 |
| eutrophication, freshwater / freshwater eutrophication | ReCiPe | 2.775 | 4.314 |

Differences: climate 0.003 and 0.011 · freshwater 0.000 and 0.000 · **minerals 0.752 and 1.245**.
EF 3.1 returns the larger minerals multiple at both steps.

**Ordering: avoided impact increases Sc2 < Sc3 < Sc4 in 25 of 25 EF rows and 18 of 18 ReCiPe
categories.** Asserted in the notebook, not trusted.

# 🔴 THE INDEPENDENCE LIMIT. It must be disclosed and Thiago agreed to keep it.

At characterised level the two methods return **almost the same freshwater eutrophication result**:
Sc2 credit 0.00689894 kg P eq under EF 3.1 against 0.00689905 kg P eq under ReCiPe, agreeing to the
fifth significant figure (0.0016 % apart). **That agreement is arithmetic, not corroboration.**

Climate is a real test: 4.41898 against 4.47268 kg CO2 eq, 1.2152 % apart at level, with ratios
still agreeing to 0.011. **The block therefore tests two categories, not three.**

He cut the two number-heavy paragraphs (the level comparison and the gross spread) and kept the
caveat as a clause. The gross-spread paragraph is gone for good.

# ✅ THE FULL 15-PAIR MAPPING, rebuilt 2026-08-24

⚠ **Thiago's own PAIRS list lives in his LCA_explorer, not on disk.** `nb/cell_recipe_pairs.py`
references it without defining it. The mapping was rebuilt from the two category lists and
reproduces all four constraints his own cell states. The notebook `assert`s each one.

15 pairs · **4 share a unit** (climate/global warming, eutrophication freshwater/freshwater
eutrophication, eutrophication marine/marine eutrophication, ozone depletion/stratospheric ozone
depletion) · EF's **Eutrophication terrestrial** has no counterpart · ReCiPe's **marine ecotoxicity,
terrestrial ecotoxicity and ozone formation for terrestrial ecosystems** stay unpaired.

# 🔴 A MEMORY CLAIM THAT IS FALSE. Do not write it.

Session 40 recorded *"the pairs that agree are exactly the pairs that share a unit."*
**Across all fifteen pairs that is false.** Water use and water consumption differ in unit and agree
to 0.000. Ozone depletion shares its unit and differs by 0.241.
**It holds only inside the three reported categories**, and the thesis states it only there.

Sc4 multiple divergence, all 15 pairs, largest first: human toxicity non-cancer **1.554** ·
minerals **1.245** · freshwater ecotoxicity 0.639 · particulate matter 0.382 · ozone depletion 0.241
· photochemical ozone formation 0.238 · marine eutrophication 0.172 · human toxicity cancer 0.138 ·
acidification 0.079 · land use 0.055 · fossils 0.032 · ionising radiation 0.020 · climate 0.011 ·
water use 0.000 · freshwater eutrophication 0.000.

🔴 **Human toxicity non-cancer diverges MORE than minerals.** It is outside the reporting set, so it
is not in Findings. **It is point 5 of the Discussion scaffold** and naming it there closes the one
place a reader could call the reporting selective.

# ✅ THE RANKING, and the mechanism that must accompany it

Thiago decided to publish the ReCiPe ranking as the second result of the block, over the earlier
recommendation. **His reason reframed it and the reframing is right:** the ranking is evidence that
category prioritisation is method-dependent, not an argument against his own EF selection.

ReCiPe reporting set: freshwater ecotoxicity 37.77 % · marine ecotoxicity 29.03 % · human
carcinogenic toxicity 24.52 %, closing at **91.33 % cumulative**.
The three EF counterparts: freshwater eutrophication **rank 4** at 4.8311 % · global warming
**rank 11** at 0.2522 % · mineral resource scarcity **rank 18 of 18** at 0.0005 %.
**The two reporting sets share no category at all.**

🔴 **The mechanism must be stated as data or the chart misleads.** Normalisation references, derived
as raw Sc1 / normalised person-equivalents and cross-checked against `recipe2016_nw_factors.txt`:

| category | normalisation reference |
|---|---|
| Mineral resource scarcity | **1.201 x 10^5 kg Cu eq** |
| Global warming | 7,990 kg CO2 eq |
| Freshwater ecotoxicity | 25.17 kg 1,4-DCB |
| Freshwater eutrophication | 0.6499 kg P eq |

Sc1's characterised mineral result is **2.203 kg Cu eq**, which is not small. The reference is large.
His own audit file already concludes *"ReCiPe normalisation must not be used to prioritise categories
in this study."*

⚠ **The unit does NOT explain the ranking mismatch.** Global warming shares kg CO2 eq with its
counterpart and still moves from EF rank 2 to ReCiPe rank 11. Thiago drafted a sentence saying the
mismatch is "because the units are not the same"; it is falsified by two of his own three pairs and
it carries the banned word. Corrected in the draft.

# 🔴 TWO METHODOLOGY EDITS THE BLOCK REQUIRES

1. *"reports one of them at characterized level only"* → the normalised ranking breaks that sentence.
2. Three sentences appended to the ReCiPe paragraph, ending on **"and does not use it to select
   categories"**. That clause is what turns the ranking into evidence.

Both are drafted in `MAIN PAPER/recipe_block_draft.md` Part 2, with the Sc4 band edit as the third.

# 📁 ARTEFACTS

`MAIN PAPER/recipe_block_draft.md` (Findings prose, Methodology edits, AI-use log entry) ·
`MAIN PAPER/why_it_mismatches_placement.md` (the three-way split and the Discussion scaffold) ·
`recipe_cross_check.ipynb`, 15 cells, executed clean, writes `recipe_cross_check.xlsx`.

⚠ `recipe_screening_log.txt` still contains a **superseded ReCiPe 2008 endpoint screening in which
metal depletion ranks FIRST at 22.1 %**, printed directly above the rebuilt ReCiPe 2016 endpoint
screening where mineral resource scarcity is 0.0 %. Two opposite answers in one file. Clean it.
⚠ The log says it wrote `impact_screening_ReCiPe_end.csv`. **That file is not on disk.**

Related: [[lca_results_verified_ch4]], [[ch4_findings_progress]], [[lca_methodology_3_3]]
