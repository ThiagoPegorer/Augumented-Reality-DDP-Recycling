---
name: thesis-schedule
description: "Live thesis state at the close of Session 44 (2026-08-26). FIRST FULL VERSION OF THE THESIS IS WRITTEN. Every chapter and all three front-matter pages are drafted. Chapters 5, 6 and the whole front matter are NOT pasted. Thursday 27 to Sunday 30 August is the revision period. Submission Monday 31 August."
type: project
---

# 🟢 WHERE THINGS STAND, close of Session 44 (2026-08-26)

**His words: "the first version of the thesis is done."** Nothing is left to draft. Everything left
is pasting, revising and the affidavit.

| chapter | drafted | pasted |
|---|---|---|
| 1 Introduction | ✅ | ✅ 🔴 live defect: "reproduces the geometry" |
| 2 Literature Review | ✅ | ✅ |
| 3 Methodology | ✅ | ✅ 🔴 three impact-assessment edits, three provenance edits, two "replica" swaps owed |
| 4 Results | ✅ | ✅ end to end |
| 5 Discussion | ✅ | ❌ **NOT PASTED.** Boilerplate still live under the heading |
| 6 Conclusion | ✅ | ❌ **NOT PASTED.** Take-home messages still owed, HIS OWN |
| Abstract | ✅ 300 words | ❌ boilerplate live at paragraphs 16 to 23 |
| Foreword | ✅ corrected | ❌ his uncorrected version is live |
| Acknowledgements | ✅ corrected | ❌ one unfinished line is live |

Paste sources: `chapter_5_discussion_FULL.md`, `chapter_6_conclusion_draft.md`,
`abstract_FINAL_2026-08-26.md`, `front_matter_2026-08-26.md`. All in `Memory_Log/memory/`.
See [[front_matter_progress]], [[ch5_discussion_progress]], [[ch6_conclusion_progress]].

# 🔴 THE PLAN

| day | task |
|---|---|
| Thu 27 to Sun 30 Aug | 🔴 **REVISION PERIOD.** Paste, the compile-day backlog, the affidavit |
| **Mon 31 Aug** | 📦 **Submission**, 12:00 to 14:00 Berlin. Hard deadline 2 Sep 16:00 |
| Sat 19 Sep | 🎓 Defence |

Task Tracker carries four "Compile all chapters & thesis formatting" rows, 27 to 30 August, and the
submission row on 31 August. "Write Discussion" and "Write Conclusion & Abstract" are still open
rows although the work is done; he may want to close them.

# 🔴 THE FIVE MOST AT-RISK ITEMS

1. 🔴🔴 **THE AFFIDAVIT.** Chapters 4, 5, 6 and the Abstract's last two paragraphs are Claude prose,
   which SRH requires denoted in the core text. **Sessions 41, 42 and 43 still have no `ai_use_log`
   entry**, and their prompts exist only in the Cowork transcripts. A **GAP NOTICE** naming what
   those sessions produced was inserted in the log on 2026-08-26. Session 44 is fully logged.
   **Only he can recover those three prompt sets.**
2. 🔴 **NOTHING FROM SESSION 43 OR 44 IS IN THE DOCUMENT.** Two chapters and three front-matter
   pages are sitting in markdown files.
3. 🔴 **TWO DOCUMENT DEFECTS BLOCK PASTING CHAPTER 5.** Section 1.3 says the teardown model
   "reproduces the geometry"; section 4.1.6 blames the units for the ReCiPe ranking mismatch. Items
   A1 and A2 of `thesis_review_backlog_2026-08-26.md`.
4. 🔴 **THE TITLE PAGE HAS NO SUPERVISOR NAMES** and no date. See [[front_matter_progress]].
5. 🔴 **THE CONDITION-ORDER CONTRADICTION.** The 2026-07-21 spec says odd IDs start 2D-first; the
   real order breaks it for P03 and P04.

# 🔴 COMPILE-DAY BACKLOG

Fifty-odd items in five groups: `Memory_Log/memory/thesis_review_backlog_2026-08-26.md`, built by
reading the whole thesis PDF. Group A the three alarming items, B the gaps, C grammar and
typography, D what is owed from earlier sessions, E what was verified correct.

# ✅ THE SRH CHAPTER CONTRACTS

**DISCUSSION:** three-sentence summary (overridden deliberately) · what the data mean against the
objectives · methodological limitations · comparison with the literature review · transferability.

**CONCLUSION:** the author's own thoughts integrating all prior chapters · impact for industry,
society and the scientific community, plus whether the method's strengths and weaknesses showed up ·
new questions as further research ⚠ **currently unmet, he deleted the four research gaps** · a
closing section of clear take-home messages.

**ABSTRACT, FOREWORD, ACKNOWLEDGEMENTS:** see [[front_matter_progress]].

🔴 **CITATIONS ARE REQUIRED IN EVERY CHAPTER, Conclusion included.** Template, verbatim: *"Throughout
your thesis, it is outmost imperative that you cite every sentence which you didn't author!"* The
list of what must be cited names **legal statutes and regulations** explicitly.

# ✅ THE Sc4 BAND CONTRADICTION, resolved by relocation

**3.3.3 currently ends:** *"Its functional yield is unsourced, so that scenario may be reported only
as a band and never as a single value."*
**Replace with:** *"Its functional yield is unsourced, so the deterministic result for that scenario
is reported together with its simulated interval, and that interval is given with the uncertainty
results."*

# ✅ VERIFIED REGULATION DATES, Regulation (EU) 2026/1738, p. 29

Repeals 2000/53/EC and 2005/64/EC from **1 Sep 2028** · Articles 11 and 29 from **1 Sep 2029** ·
Article 33 from **1 Jan 2030** · Article 13 from **1 Sep 2032**.
⚠ **No source on disk for a DPP date covering product groups outside the vehicle regulation.**

# Google Calendar

⚠ Zero events for seven consecutive wake-ups. **The Notion Task Tracker is the schedule of record.**

# Notion session log

Latest row: **Session 44 (2026-08-26)**. Next is **45**. Sessions 43 and 44 both fell on 26 August.
Still UNLOGGED: the 2026-08-02/03 backfill.

# Diagram channel

**Miro** for diagrams, **LCA_explorer** for LCA data charts, **`ARDPP_study_analysis.ipynb`** for the
user-study charts. Board "Master Thesis" (`uXjVGpjOAoU`).

# Run-order trap (durable)

Teardown builders (menu 05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only) and
layer-hide. Never run while the stage clone exists. Rig rebuild is always 09, 10, 11, 12, 13, 14,
Verify, save.

Related: [[front_matter_progress]], [[ch6_conclusion_progress]], [[ch5_discussion_progress]],
[[ch4_findings_progress]], [[study_results_verified]], [[recipe_cross_check_verified]],
[[lca_results_verified_ch4]], [[study_design_verified]], [[research_questions_final]],
[[voice_and_verification_rules]], [[session_logging_routine]], [[ch3_methodology_progress]],
[[teardown_model_as_built]]
