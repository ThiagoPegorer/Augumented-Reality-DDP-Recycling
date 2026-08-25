# Chapter 4, section 4.1 LCA Results: block plan and verified data pack

Built 2026-08-23 (Session 40). Every number below was read directly from the Outputs CSVs in this
session and is traced to its file. **This is a scaffold, not prose.** Nothing here is written to be
pasted into the thesis.

Structure confirmed by the author: **4.1 LCA Results · 4.2 AR DPP Results · 4.3 AR DPP Test results.**
Reporting sequence inside 4.1 confirmed by the author and matched against `LCA_framework_v4.md`.

---

## 0. THREE CORRECTIONS TO THE PLAN, before anything is drafted

### 0.1 ReCiPe is a characterisation cross-check ONLY. It is not a second screening.

`LCA_framework_v4.md` line 709 labels `impact_screening_ReCiPe_mid.csv` **"Not a prioritisation
table, evidence for §4.2.1"**, and instructs that its cumulative column be cited as *cumulative
share of normalised impact*, **never as "weighted footprint"**. Line 646 closes: *"ReCiPe's role is
characterisation robustness, at characterised level only."*

**Consequence for step (e):** the cross-check compares **characterised results and the scenario
ordering**. It must not compare reporting sets as if both were valid prioritisations, because
section 3.3.3 already argues that ReCiPe normalisation is unusable for this product.

The disjointness of the two reporting sets is still reportable, but as a **stated consequence of
the method choice**, which the framework flags at line 642 as *"the strongest available evidence
that normalisation and weighting are the most value-laden layer in LCA."* That sentence carries
interpretation, so **it belongs in the Discussion, not in Findings.**

### 0.2 The cumulative is 85.70 %, and the third category is in because of the crossing rule

Climate change alone reaches **79.12 %**, which is **below** the PEF threshold. Freshwater
eutrophication is included because the rule takes the smallest set reaching ≥80 % **with the
crossing category included**. Write the mechanism, not just "the top three exceed 80 %".

### 0.3 EF 3.1 has 16 categories, not 25

`impact_EF31.csv` carries 25 rows because 9 are sub-indicators: climate change split into biogenic,
fossil and land use; freshwater ecotoxicity and both human toxicity families split into organics
and inorganics. **The screening runs on the 16 top-level categories.** Any count in the prose must
say which of the two is meant.

---

## 1. THE BLOCK PLAN. Six blocks, in the author's order.

| Block | What it establishes | Visual | Source file |
|---|---|---|---|
| **4.1 (a)** | Which categories the chapter reports, and why those | one table, one bar chart | `impact_screening.csv` |
| **4.1 (b)** | The gross burden of each route in the three reported categories | grouped bar, 3 panels | `impact_EF31.csv` |
| **4.1 (c)** | The avoided impact of each route in the same three | grouped bar, 3 panels | `impact_EF31.csv` |
| **4.1 (d)** | Gross above the axis, avoided below, all 16 categories | the signed-axis chart | `impact_EF31.csv` |
| **4.1 (e)** | Whether the ordering survives a second characterisation method | table of deltas | `impact_ReCiPe_mid.csv` |
| **4.1 (f)** | What the parameter ranges do to the result | box plots | `mc_summary.csv` |

**Join into 4.2:** block (c) produces the avoided-impact numbers that the passport displays.
**Join out of 3.3:** the four scenarios and the functional unit are already defined; do not redefine.

---

## 2. BLOCK (a). Normalisation and weighting screening, EF 3.1

**What it must establish:** that the reduction from 16 categories to 3 is a rule applied, not a
choice made.

**5W.** *What:* the PEF most-relevant-categories screening. *Why here:* it selects the reporting
set before any result is shown. *How:* rank the weighted person-equivalent contributions on the
Sc1 basis, take the smallest set reaching ≥80 % cumulative, include the crossing category, add the
goal-pinned categories. *Who ruled it:* the PEF method itself. *When:* on the Sc1 baseline.

**[TABLE n: EF 3.1 normalisation and weighting screening on the Sc1 baseline | source: author, from `impact_screening.csv`]**

| Category | Share of weighted footprint | Cumulative | In reporting set | Goal-pinned |
|---|---|---|---|---|
| Resource use, minerals and metals | **72.45 %** | 72.45 % | yes | yes |
| Climate change | 6.67 % | **79.12 %** | yes | yes |
| Eutrophication, freshwater | 6.58 % | **85.70 %** | yes | no |
| Resource use, fossils | 4.25 % | 89.96 % | no | no |
| Acidification | 2.09 % | 92.04 % | no | no |
| Ecotoxicity, freshwater | 1.60 % | 93.64 % | no | no |
| Particulate matter | 1.42 % | 95.06 % | no | no |
| Human toxicity, non-cancer | 1.15 % | 96.22 % | no | no |
| Photochemical ozone formation (human health) | 0.96 % | 97.17 % | no | no |
| Water use | 0.93 % | 98.10 % | no | no |
| Eutrophication, terrestrial | 0.62 % | 98.72 % | no | no |
| Eutrophication, marine | 0.45 % | 99.17 % | no | no |
| Ionising radiation (human health) | 0.44 % | 99.61 % | no | no |
| Human toxicity, cancer | 0.24 % | 99.86 % | no | no |
| Land use | 0.12 % | 99.98 % | no | no |
| Ozone depletion | 0.02 % | 100.00 % | no | no |

⚠ **Both goal-pinned categories fall inside the set on their own merit.** Say so. It removes the
objection that the pinning drove the selection.

⚠ The prose must carry the method in the sentence: this ranking is **EF 3.1 normalisation and
weighting**, and it exists only because EF supplies those factors.

### ✅ THE CITATION IS FOUND, 2026-08-23. Two sources, one job each.

**1. The sixteen categories, and what normalisation and weighting are.**
**Andreasi Bassi et al. (2023), EUR 31414 EN, printed p. 5.** Verified first-hand this session from
`LITERATURE/JRC130796_01.pdf`, PDF page 9. Verbatim:

> "The inputs and outputs from the life cycle inventory are aggregated in 16 midpoint characterised
> impact categories. These impact categories are then normalised (i.e., the results are divided by
> the overall inventory of a reference unit, e.g., the entire world, to convert the characterised
> impact categories in relative shares of the impacts of the analysed system) and weighted (i.e.,
> each impact category is multiplied by a weighting factor to reflect their perceived relative
> importance)."

⚠ **This report does NOT contain the ≥80 % rule.** All 57 pages were searched. Its uses of "most
relevant impact categories" are a descriptive label for the top five contributors in its own
comparison tables, and the three "80%" hits are figure axis labels. **Do not cite it for the rule.**
⚠ Its own sentence on printed p. 3 reads "The Environmental Product Environmental Footprint (PEF)",
which is a typo in a published JRC document. **Never quote that phrase.**

**2. The ≥80 % most relevant categories rule.**
**Commission Recommendation (EU) 2021/2279 of 15 December 2021**, on the use of the Environmental
Footprint methods to measure and communicate the life cycle environmental performance of products
and organisations. **Annex I, section 6.3.1, "Procedure to identify the most relevant impact
categories", at OJ page L 471/223.** Published OJ L 471, 30 December 2021.
`[FILL: OJ page range, read it off the document's last page]`

⚠ **The section number, the heading and the 80 % threshold are confirmed from two independent
reads. The exact sentence wording is NOT.** It came through a summarising layer, and an earlier read
of the same document returned a different answer. **Paraphrase and cite; do not quote it** until the
sentence is read in the PDF itself.

🔴 **ATTRIBUTE ONLY THE 80 % RULE TO THE RECOMMENDATION.** The framework's own words are "corrected
PEF rule, smallest set reaching ≥80 % cumulative, **crossing category included**", and the
goal-pinned categories are this study's addition. Writing the pinning as part of the PEF rule would
cite a source for something it does not say, which is the defect verification rule C exists to stop.
State the pinning as the study's own decision.

✅ Independently confirmed: the PEF ranking is performed on **normalised and weighted** results, not
on characterised ones. That is what makes the screening table's basis correct as drafted.

**Action:** download the Recommendation into `LITERATURE/` so the source sits on disk like every
other one. A copy is served at `afec.es/documentos/english/recommendation-2021-2279.pdf`; the
authoritative copy is the EUR-Lex ELI record.

---

## 3. BLOCK (b). Gross burden per route

**What it must establish:** that the route barely changes the burden. That is what makes block (c)
the whole story.

**Verified, EF 3.1, deterministic** (from `impact_EF31.csv`):

| Category | Sc1 | Sc2 gross | Sc3 gross | Sc4 gross |
|---|---|---|---|---|
| Resource use, minerals and metals (kg Sb eq) | 0.0187391 | 0.0187396 | 0.0187395 | 0.0187395 |
| Climate change (kg CO2 eq) | 73.4326 | 73.4794 | 73.4093 | 73.4103 |
| Eutrophication, freshwater (kg P eq) | 0.11592 | 0.115935 | 0.115929 | 0.115937 |

**The number that carries the block:** across all four routes the gross burden varies by **under
0.1 % in 20 of the 25 result rows.**

⚠ **It is NOT flat everywhere, and the exception must be stated.** In the five human toxicity rows
the spread runs **2.5 % to 5.7 %**, and **Sc3 gross exceeds Sc4 gross** in every one of them. None
of those five is in the reporting set, so this belongs in one sentence plus the full table, not in
the chart.

**Stage contributions to Sc1** (from `impact_stage_contributions.csv`; the file's own
`ef31_csv_sc1` column reconciles with `total_sc1` to the digit in every category):

| Category | S1 materials | S2 assembly | S3 distribution | S4 use | S5 end of life |
|---|---|---|---|---|---|
| Minerals and metals | **97.84 %** | 0.44 % | 0.00 % | 1.71 % | 0.00 % |
| Climate change | 52.41 % | 0.60 % | 0.04 % | **46.71 %** | 0.24 % |
| Eutrophication, freshwater | 53.94 % | 0.49 % | 0.00 % | 45.56 % | 0.00 % |

**The contrast is the finding:** the mineral burden is almost entirely in materials, while climate
is split roughly evenly between materials and use. Report both shares. Do not explain them here.

---

## 4. BLOCK (c). Avoided impact per route

**Verified, EF 3.1, deterministic**, avoided impact and its share of the Sc1 burden:

| Category | Sc2 avoided | % of Sc1 | Sc3 avoided | % of Sc1 | Sc4 avoided | % of Sc1 |
|---|---|---|---|---|---|---|
| Minerals and metals (kg Sb eq) | 0.00187177 | **9.99 %** | 0.00602588 | **32.16 %** | 0.00887018 | **47.34 %** |
| Climate change (kg CO2 eq) | 4.41898 | **6.02 %** | 8.16273 | **11.12 %** | 15.4315 | **21.01 %** |
| Eutrophication, freshwater (kg P eq) | 0.00689894 | 5.95 % | 0.0191428 | 16.51 % | 0.0297611 | 25.67 % |

Relative to Sc2, Sc4 avoids **4.74x** as much in minerals, **3.49x** in climate and **4.31x** in
freshwater eutrophication.

✅ **These three climate percentages are identical to the framework's old net-based −6.0, −11.1 and
−21.0.** The supervisor's ruling changes the word, not the numbers. Worth one sentence, because a
reader holding an older draft will check.

### 🔴 The robustness claim, RE-VERIFIED on the saving column this session

Ordering **Sc2 avoided < Sc3 avoided < Sc4 avoided** holds in:

- **all 16 EF 3.1 categories, and in all 9 sub-indicators, so 25 of 25 result rows**
- **all 18 ReCiPe 2016 Midpoint categories**

The claim previously rested on the abolished net quantity. **It now rests on the avoided-impact
column and it survives.** State it as a claim about deterministic characterised results.

⚠ **Do NOT extend it to the distributions.** At Monte Carlo p5/p95 the three avoided-impact
distributions are fully separated in only **10 of 25** rows. That belongs in block (f).

---

## 5. BLOCK (d). The signed-axis chart, all 16 categories

**What it must establish:** the reporting convention the supervisor required, applied once to the
whole category set, so the three reported categories are visibly not cherry-picked.

**Chart spec.** One horizontal axis at zero. Gross burden plotted above, avoided impact plotted
below, per scenario. **Never a combined bar.** Each category normalised to its own Sc1 gross so 16
different units can share one panel; label the axis as a share of the Sc1 burden and print the
absolute Sc1 value against each category name.

### ✅ THE BALANCE LINE IS PERMITTED. Ruling clarified by the author, 2026-08-23.

**The supervisor abolished the TERM "net", not the IDEA.** His objection is presentational: a single
netted number reads as if an emission that already occurred had been removed, which the physical
system does not do. Plotting gross explicitly above the axis and the credit explicitly below, with a
**balance line** drawn across them, keeps the same arithmetic **inspectable** instead of hidden
inside one figure. That is why his own reference chart carries a GHG balance line.

**So block (d) is unblocked, and the chart carries the line.**

**Three conditions on it.**

1. **The word is "balance", never "net".** Define it once on first use: gross burden minus avoided
   impact, plotted as a line, not as a bar. Then hold it everywhere, including the framework's FINAL
   RESULTS table headers, which still read "Sc2 net / Sc3 net / Sc4 net" and must be restated.
2. 🔴 **The caption must say the credit sits outside the system boundary.** `lca_methodology_3_3`
   records the design ruling that the avoided burden LEAVES the boundary and does not return to this
   unit. The balance line is therefore a **comparison device between routes**, not this unit's
   footprint. Without that clause the chart asserts something the model does not.
3. **One clause on the APOS partial double count**, which now sits inside the line rather than
   beside it. The agreed three-sentence wording is in [[ch3_methodology_progress]].

### The balance values, verified. NO RECOMPUTATION NEEDED.

Gross minus avoided reproduces the existing `*_net` columns **exactly**, difference 0.00e+00 in every
row. Only the vocabulary and the plot change, exactly as the framework predicted.

| Category | Sc1 | Sc2 balance | Sc3 balance | Sc4 balance |
|---|---|---|---|---|
| Minerals and metals (kg Sb eq) | 0.0187391 | 0.0168679 (**−9.99 %**) | 0.0127136 (**−32.15 %**) | 0.00986934 (**−47.33 %**) |
| Climate change (kg CO2 eq) | 73.4326 | 69.0605 (**−5.95 %**) | 65.2466 (**−11.15 %**) | 57.9788 (**−21.04 %**) |
| Eutrophication, freshwater (kg P eq) | 0.11592 | 0.109036 (−5.94 %) | 0.0967866 (−16.51 %) | 0.0861762 (−25.66 %) |

⚠ **Correction to an earlier statement in this session.** The framework's headline percentages
(−6.0, −11.1, −21.0 climate; −10.0, −32.2, −47.3 minerals) are **balance-against-Sc1** figures, not
avoided-share figures. The two are close but not identical, because Sc2 to Sc4 gross differs slightly
from Sc1: climate gives 6.02 / 11.12 / 21.01 as an avoided share against 5.95 / 11.15 / 21.04 as a
balance change. **They round the same at one decimal. Pick one basis, say which it is, and do not mix
them within a table.**

### The robustness claim now has TWO forms and both hold

| Stated on | EF 3.1 | ReCiPe 2016 Midpoint |
|---|---|---|
| **Balance**, Sc1 > Sc2 > Sc3 > Sc4 | **25 of 25 rows** | **18 of 18** |
| **Avoided impact**, Sc2 < Sc3 < Sc4 | **25 of 25 rows** | **18 of 18** |

Write the balance form as the headline, since that is what the chart draws, and the avoided form as
its companion. **Neither extends to the distributions.** See block (f).

---

## 6. BLOCK (e). ReCiPe 2016 Midpoint cross-check, characterised only

**What it must establish:** that the scenario ordering is not an artifact of EF 3.1's
characterisation factors.

**Verified, avoided impact as a share of that method's own Sc1 burden:**

| Pair | Sc2 | Sc3 | Sc4 |
|---|---|---|---|
| EF *Climate change* | 6.02 % | 11.12 % | 21.01 % |
| ReCiPe *Global warming* | 6.01 % | 11.12 % | 21.06 % |
| **difference** | **+0.01 pp** | **0.00 pp** | **−0.04 pp** |
| EF *Resource use, minerals and metals* | 9.99 % | 32.16 % | 47.34 % |
| ReCiPe *Mineral resource scarcity* | 11.14 % | 27.49 % | 38.93 % |
| **difference** | **−1.15 pp** | **+4.67 pp** | **+8.40 pp** |
| EF *Resource use, fossils* | 5.76 % | 10.77 % | 20.25 % |
| ReCiPe *Fossil resource scarcity* | 6.03 % | 11.38 % | 21.40 % |
| **difference** | **−0.27 pp** | **−0.61 pp** | **−1.14 pp** |

Absolute Sc1 values differ by construction because the units differ: climate 73.4326 kg CO2 eq
against 74.3996 kg CO2 eq, minerals 0.0187391 kg Sb eq against 2.20278 kg Cu eq, fossils 1019.35 MJ
against 18.9675 kg oil eq.

🔴 **The honest reading, and it must not be smoothed.** The climate cross-check agrees to within
0.05 percentage points, which is as close as this comparison can get. **The mineral cross-check
does not:** at Sc4 the two methods differ by 8.40 percentage points on the category that carries
72.45 % of the weighted footprint. The **ordering** holds under both. The **magnitude** does not.
Report both facts in Findings. The reason goes to the Discussion.

⚠ Every one of these numbers carries its method in the sentence. Two methods appear on one page.

---

## 7. BLOCK (f). Monte Carlo, box plots, last

n = 1,000 per system, seven systems: Sc1, and gross and saving separately for Sc2, Sc3 and Sc4.

**Climate change, kg CO2 eq** (from `mc_summary.csv`):

| System | p5 | p50 | p95 | mean | sd |
|---|---|---|---|---|---|
| Sc1 | 70.02 | 91.33 | 128.10 | 94.38 | 18.38 |
| Sc2 gross | 68.98 | 88.20 | 127.15 | 92.01 | 17.65 |
| Sc3 gross | 69.40 | 91.28 | 129.39 | 94.55 | 18.86 |
| Sc4 gross | 69.94 | 91.14 | 126.66 | 93.49 | 17.61 |
| **Sc2 avoided** | 4.395 | 4.626 | 4.949 | 4.648 | 0.172 |
| **Sc3 avoided** | 8.013 | 8.415 | 8.889 | 8.424 | 0.263 |
| **Sc4 avoided** | 14.323 | 15.827 | 17.500 | 15.846 | 0.985 |

**Relative spread (p95 − p5) / p50:** gross systems **62 % to 66 %**; avoided systems **10 % to
20 %**. The avoided term is three to six times better constrained than the gross term.

**Two findings this block owns, both reportable without comment:**

1. **Separation is partial.** The three avoided distributions are fully disjoint at p5/p95 in
   **10 of 25 rows**. Fifteen overlap, including both human toxicity families, freshwater
   ecotoxicity, land use, water use, ozone depletion, particulate matter and ionising radiation.
2. **Five categories go negative at p5.** Some parameter draws make the recovery route cost more
   than it avoids: human toxicity cancer (Sc3 −4.10e−07, Sc4 −5.07e−07 CTUh), human toxicity
   non-cancer (Sc3 −4.96e−05, Sc4 −6.23e−05 CTUh), land use (Sc3 −69.31, Sc4 −90.10 pt) and water
   use (Sc3 −561.79, Sc4 −681.78 m3 world eq). **None is in the reporting set.** Report it in the
   full table with one sentence in the text.

### 🔴 THE DEFECT THIS BLOCK MUST DISCLOSE

| Sc1 | Deterministic | MC p5 | MC p50 | MC p95 |
|---|---|---|---|---|
| Climate change | **73.4326** | 70.02 | **91.33** | 128.10 |
| Resource use, fossils | **1019.35** | 969.79 | **1272.81** | 1804.99 |
| Resource use, minerals and metals | 0.0187391 | 0.017533 | 0.0188976 | 0.0206581 |

**The deterministic climate result sits near the 6th percentile of its own simulation, roughly 24 %
below the median.** Fossils behaves the same way. Minerals is centred and unaffected.

This is not optional. Section 4.1 reports 73.4326 as the footprint, and the prototype displays a
figure derived from the same run. Findings states the two values and names which is which. **The
explanation goes to the Discussion.**

**[A] Not verified, do not write it as fact.** The likely driver is the use-phase electricity.
Stage 4 is 46.71 % of Sc1's climate total, Equation 1 gives 66.2 kWh as the mode, and the declared
triangular range is 54 to 189 kWh, which is right-skewed. Scaling stage 4 to about 100 kWh closes
the 17.9 kg gap almost exactly, and minerals escapes because only 1.71 % of it is use-phase.
**Confirm against the simulation inputs before this reaches the Discussion.**

---

## 8. NUMBERING WARNING

Chapter 3 closes at **Table 7** and **Figure 17**. Every table and figure added to Chapter 4 pushes
every appendix table and figure number up again. Word renumbers automatically from the captions;
**anything typed by hand into running prose will break.** Use `[TABLE n]` and `[FIGURE n]`
placeholders throughout the draft and let Word resolve them at compile.

---

## === Check before using ===

**Assumptions I made:** none in the numbers. Every value is read from the named CSV.

**Needs a real citation:** ✅ **CLOSED 2026-08-23.** Block (a) now cites Andreasi Bassi et al.
(2023, p. 5) for the sixteen categories and for normalisation and weighting, and Commission
Recommendation (EU) 2021/2279, Annex I, 6.3.1, for the ≥80 % rule. Two residual jobs: read the
6.3.1 sentence in the PDF before quoting it, and fill the OJ page range.

**Needs a decision from you:**
1. ✅ **Balance line: ANSWERED 2026-08-23 and permitted.** See block (d). The open part is the word:
   confirm **"balance"** as the term that replaces "net" everywhere, including the framework's FINAL
   RESULTS table headers.
2. **Does the mineral 8.40 pp divergence get its own table row, or a sentence?** It is the most
   awkward number in the section and the most defensible one to have found yourself.
3. **Which basis carries the headline percentages, balance-against-Sc1 or avoided-share?** They
   differ in the second decimal. One basis per table.

**Possible contradiction, resolved:** the framework specifies EF normalisation and weighting as the
screening basis (line 55) while section 3.3.3 rejects ReCiPe normalisation and weighting. Those are
not in conflict. The rejection is method-specific and reasoned, and line 646 keeps prioritisation
with EF because its weights are the European Commission's. **Section 4.1 must not blur the two.**
