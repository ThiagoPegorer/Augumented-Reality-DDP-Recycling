---
name: study-results-verified
description: "[P] THE file for section 4.3. Every verified number of the nine-participant user study, the three confirmed response scales, the condition order, the reproducible artefact chain (raw workbook to notebook to audit pack), and three open discrepancies against the 2026-07-21 study spec. Built and verified 2026-08-25, Session 42."
type: project
---

# ⭐ THE ARTEFACT CHAIN. Everything in 4.3 comes out of these four files.

All in `MASTER THESIS/MAIN PAPER/`.

| file | role |
|---|---|
| `ARDPP_study_data.xlsx` | **RAW ONLY.** Ten sheets. No derived value lives here. |
| `ARDPP_study_analysis.ipynb` | 20 cells. Derives every number, draws every figure, runs the audit. |
| `ARDPP_study_results.xlsx` | nine sheets of derived tables, written by the notebook |
| `ARDPP_figure_audit.xlsx` | eight sheets, one per figure, each reconciled against the raw sheets |

🔴 **Architecture rule, in the README of the raw workbook:** *"This workbook holds RAW data only.
Every derived value is computed in the analysis notebook, not here."* Do not break it.

⚠ **Notes must sit in column B, never column A.** The notebook's loader filters rows by regex on the
key column, so a note in column A is read as a participant identifier and the integrity assert fires.

⚠ `nbformat.v4.read` does not exist. Use `nbformat.read`.

# ✅ THE THREE RESPONSE SCALES, confirmed by Thiago 2026-08-25

| scale | 1 | 5 | reported as |
|---|---|---|---|
| background experience | never used | expert | **numbers 1 to 5, endpoints named only** |
| ten questionnaire items | disagree | agree | numbers 1 to 5 |
| ten comparative items | the two-dimensional manual | the Augmented Reality model | numbers 1 to 5 |

🔴 **The three middle points of the background scale carried NO wording on the form.** They are blank
in the `experience_scale` sheet on purpose. An earlier session invented "tried once or twice",
"occasional use", "frequent use" as placeholders; those were **removed**, because printing them
states an instrument that was never run. If wording is ever found, typing it into the sheet switches
every chart and table to words with no code change.

✅ The comparative anchors are corroborated by the **Session 12 Notion page, 2026-07-21**:
*"10-item comparative scale (1 = conventional 2D manual · 5 = AR 3D model)"*. Primary, dated, and
independent of Thiago's recollection.

# ✅ PARTICIPANTS, n = 9, P01 to P09

Age 18 to 24 ×2 · 25 to 34 ×4 · 45+ ×3. Nine different fields: electrician, electrical engineer,
economist, marketing analyst, marketing and social media, strategic design, innovation manager,
student, retired.

Headset experience 1 to 5: **4, 2, 1, 1, 1**. Disassembly experience: **5, 1, 0, 2, 1**.

**This block is the only evidence in the thesis for Chapter 1's "mixed background" claim.**

# ✅ CONDITION ORDER, supplied 2026-08-25

P01 AR only · P02 AR→2D · P03 AR→2D · P04 2D→AR · P05 2D→AR · P06 AR→2D · P07 2D→AR ·
P08 AR→2D · P09 2D→AR.

Among the eight who performed both: **four started with each condition.**

🔴🔴 **DISCREPANCY AGAINST THE PRE-REGISTERED RULE.** The 2026-07-21 study spec says
*"odd participant IDs start 2D-first, even AR-first."* The actual order breaks it for **P03 (odd,
started AR)** and **P04 (even, started 2D)**, and P01 ran no manual condition at all. Six of nine
follow the rule. **If Methodology states the odd/even rule, it contradicts the data.** Check the
.docx before submission.

# ✅ TASK COMPLETION TIME

**Definition: the sum of the five per-step values, in BOTH conditions.** Never the application's own
`elapsed_s` for one side and step sums for the other. Mixing them is what produced the stale
median difference of +15.5 in an earlier draft.

| P | manual | AR | difference |
|---|---|---|---|
| P01 | — | 418 | — |
| P02 | 440 | 460 | +20 |
| P03 | 299 | 324 | +25 |
| P04 | 357 | 376 | +19 |
| P05 | 301 | 316 | +15 |
| P06 | 201 | 186 | −15 |
| P07 | 261 | 235 | −26 |
| P08 | 427 | 450 | +23 |
| P09 | 214 | 199 | −15 |

Manual mean **312.5** median **300.0** · AR mean **318.2** median **320.0** · difference mean **+5.8**
median **+17.0** · AR slower for **5 of 8** · all nine AR runs mean 329.3 median 324.0 ·
**r = 0.992** between conditions, n = 8.

Spread: manual 201 to 440 (**239 s**), AR 186 to 460 (**274 s**), while **no participant's two times
differ by more than 26 s.** This is the strongest sentence in 4.3 and it is pure arithmetic.

Per-step: the five steps rank identically in both conditions; steps 1 and 2 together are **60.4 %**
of the mean total in the manual and **60.7 %** in AR; step 5 shortest in both.

**Errors: none, in any of the seventeen runs.**

## ✅ THE ORDER EFFECT, new 2026-08-25

**Five of the eight recorded a shorter time in the SECOND condition they performed, whichever it
was** (P02, P03, P07, P08, P09). Of the four who began with AR, the AR run was the slower for
**three**. Of the four who began with the manual, for **two**.

🔴 [Likely] a practice effect contributes to AR appearing slower. Four per side is a direction, not a
measurement. **The counts are in 4.3 with nothing joined; the reading belongs in the Discussion.**

## ✅ THE ELAPSED-VERSUS-STEP-SUM GAP, explained 2026-08-25

Four of nine AR runs agree exactly. Five differ by 1 or 2 s, **mixed sign** (P02 −2, P05 −2, P03 +1,
P06 +1, P09 +2). Every value the application writes is a whole second.

[Likely] each step duration and the session elapsed are rounded independently; five roundings drift
up to ±2.5 s and the largest observed gap is 2.0. **The mixed sign rules out the elapsed value
containing time outside the steps**, which would be systematically positive. Not read from the
application source.

**It changes nothing.** No participant changes sign under either definition; AR slower for 5 of 8
either way; AR mean identical at 318.2. Only the median difference moves, +17.0 against +15.5.

# ✅ PERCEIVED USABILITY, ten items, 0 to 100

| P | manual | AR | difference |
|---|---|---|---|
| P01 | 85.0 | 100.0 | +15.0 |
| P02 | 97.5 | 42.5 | **−55.0** |
| P03 | 92.5 | 80.0 | −12.5 |
| P04 | 70.0 | 85.0 | +15.0 |
| P05 | 65.0 | 87.5 | +22.5 |
| P06 | 92.5 | 97.5 | +5.0 |
| P07 | 85.0 | 97.5 | +12.5 |
| P08 | 32.5 | 65.0 | +32.5 |
| P09 | 95.0 | 100.0 | +5.0 |

Manual mean **79.4** median **85.0** · AR mean **83.9** median **87.5** · AR higher for **7 of 9** ·
mean difference **+4.4**, median **+12.5**. The two differ in size because P02's −55.0 enters the
mean and not the median.

## ✅ THE SCORING IS BALANCED, proved 2026-08-25

Polarity alternates: q1 q3 q5 q7 q9 positive (`value − 1`), q2 q4 q6 q8 q10 negative (`5 − value`).
Both halves run 0 to 4.

🔴 **The proof:** answering the **same value to all ten items scores exactly 50.0** for 1, 2, 3, 4 and
5. Best possible answer 100, worst 0. **No participant straight-lined** any of the eighteen rows.

P02 worked through: manual 39 points × 2.5 = 97.5 · AR 17 points × 2.5 = 42.5. He agreed he would
need technical support (q4 = 5 → 0 points) and that it was difficult to manage (q8 = 4 → 1 point),
and disagreed that others would learn it quickly (q7 = 1 → 0 points). **His 42.5 is earned in both
directions, not a scoring artefact.**

# ✅ THE COMPARATIVE ITEMS

All ten return a median of **5**. **78 of the 90 responses are 5** (six 1s, four 3s, two 4s). **Five
of nine answered 5 on all ten** (P01, P06, P07, P08, P09). c7 (materials and value) and c9
(engagement) are unanimous. Band counts per item: 7 to 9 toward the model, at most 1 at the
midpoint, at most 2 toward the manual. **P02 is the only participant to answer below 3, on five of
the ten.**

🔴 **The ceiling is real and must be owned in the Discussion.** [Likely] the ten items separated P02
and nobody else.

⚠ **The midpoint is never called "neither".** Only 1 and 5 are documented. Responses are reported in
three bands so nothing is claimed about what 3 meant.

## ✅ PERCEIVED AGAINST MEASURED AGILITY, new 2026-08-25

c1 asks which version allowed more agility; the timed runs measure the same thing. Of the eight who
performed both, **five took longer with AR**. **Three of those five (P03, P04, P08) answered the
agility item toward the AR model**; two (P02, P05) toward the manual. Counts only in 4.3.

# ✅ THE INTERVIEW

Provenance, confirmed 2026-08-25: the six open questions were put in a **closing interview** and
**Thiago wrote each answer into the form himself.**

🔴🔴 **NO QUOTATION MARKS, ANYWHERE, EVER.** Body, appendix, Discussion. The wording is his, not the
participants'. Report shared themes as counts and individual observations by named participant.

Counts: most helpful through the animations 4 of 9 · passthrough sharpness as the main difficulty
**6 of 9** · gestures as the initial obstacle 3 · immediately intuitive 3 · learnable after a
demonstration 3 · material and recovery information did not help the task **8 of 9** · of those,
gave knowledge beyond conventional specifications 7 · would use it at a real workstation **9 of 9** ·
training as the use 8 · headset comfort as the constraint 7 · nothing further 9 of 9.

**P02 carries the most content and appears in four of the five paragraphs.** That is a fair
reflection of the record.

# 🔴 THREE OPEN DISCREPANCIES AGAINST THE 2026-07-21 STUDY SPEC

1. **Condition order breaks the odd/even rule for P03 and P04.** See above.
2. **The spec says SEVEN open questions; the export has SIX.** The one apparently dropped is the
   *"deliberate pro-2D probe"*. If Methodology states seven, correct it, and note that the probe
   designed to counter pro-AR bias is the one missing.
3. **The spec names the instrument as SUS** (*"SUS for the 2D manual → SUS for the AR app"*).
   Thiago's ruling 2026-08-25 is to **leave it unnamed** in the thesis, reporting a ten-item
   questionnaire and a 0 to 100 score with no benchmark comparison. That is internally consistent
   and owes no citation. ⚠ **But the Methodology and the appendix must not name it either**, or the
   citation becomes owed again.

# ✅ THE FIGURES, six, all audited

`fig_participants_age` · `fig_participants_headset_experience` ·
`fig_participants_disassembly_experience` · `fig_completion_times` · `fig_usability_scores` ·
`fig_comparative_items`. Each `.png` at 300 dpi and `.svg`.

**The participants figure was split into three on his instruction**, one per background variable.
The two bar figures share one axis limit so they compare across figures.

**The comparative figure is a response matrix**, 10 items × 9 participants, every cell coloured by
its value with the number printed. **Nothing is averaged**, so it cannot imply a distribution nine
responses do not carry. A bar chart of means or per-item distributions was argued against and is
not used. P02 reads as a vertical stripe.

Okabe-Ito throughout: manual **#E69F00**, Augmented Reality **#0072B2**, midpoint grey.

# ✅ THE AUDIT PACK, `cell_figure_audit.py`, 21 checks

Every figure paired with its exact table, each **recomputed from the raw columns by a second route**
and compared. The cell ends `assert not failed`, so a data correction that breaks a downstream value
halts the notebook rather than writing an audit file that disagrees with the data.

Sheets: `fig1_age` · `fig2_headset_experience` · `fig3_disassembly_experience` ·
`fig4_completion_times` · `fig5_usability_scores` · **`fig5b_scoring_key`** ·
**`fig5c_worked_example`** · `fig6_comparative_items`.

🔴 **Why 5b and 5c exist:** the original check "scores reproduce from the raw items" reran the same
rule and would have passed even with a wrong polarity column. The added checks test the inversion
itself. **A reconciliation that reruns the same code proves nothing.**

`fig6_comparative_items` carries the full item wording and all nine responses, so it can serve
directly as the appendix table.

# ⚠ NOT REPORTED, deliberately

**Nothing from the session-report fields except the step count.** `n_components_recovered` and
`co2_avoided_kg` differ between participants and would expose the build split. Thiago's standing
instruction: **do not make that difference explicit anywhere.** He also ruled 2026-08-25 to ignore
`co2_avoided_kg` entirely.

⚠ The questionnaire export is titled **"Guided Disassembly RBv1.0"** and contains all nine
participants. **Rename before it goes into an appendix.**

⚠ No report file was written after 12:46 on 2026-08-25 despite the application showing *"Report was
successfully sent"* in the 14:33 screenshot. Unresolved.

Related: [[study_design_verified]], [[ch4_findings_progress]], [[thesis-schedule]],
[[voice_and_verification_rules]], [[study_build_version_finding]]
