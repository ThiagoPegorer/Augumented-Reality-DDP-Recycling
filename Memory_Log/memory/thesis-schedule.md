---
name: thesis-schedule
description: Live thesis state, refreshed at the close of Session 40 (2026-08-23). Section 4.1 LCA Results is written through the Monte Carlo block; only the ReCiPe cross-check remains. 4.2 and 4.3 have not been started, and 4.3's input data is still not machine-readable. Four days to submission.
type: project
---

# 🟢 WHERE THINGS STAND, close of Session 40

**Section 4.1 is written from the chapter opener through the Monte Carlo block.** Nine paragraphs
accepted, Tables 7, 8 and 9 and Figures 18, 19 and 20 placed, Figure 21 (the balance chart) built as
a tested notebook cell, Figure 22 (Monte Carlo) supplied by him, one appendix table delivered.
The section was also rewritten end to end for continuity, since it read as disconnected blocks.

**Only the ReCiPe cross-check is left in 4.1**, and the decision on it is open. See
[[ch4_findings_progress]].

# 🔴 THE PLAN, four days left

| day | task | notes |
|---|---|---|
| **Sun 23 Aug** | Results chapter | ✅ **4.1 written through Monte Carlo.** ⚠ five participant sessions were scheduled for today and he **never mentioned running them** |
| **Mon 24 Aug** | Results chapter | 🔴 **ReCiPe block, then ALL of 4.2 and ALL of 4.3.** This is the heaviest day of the week |
| **Tue 25 Aug** | Discussion + Conclusion + Abstract | 🔴 also carries the limitations section moved out of 3.6 |
| **Wed 26 Aug** | Compile and formatting | the only day for the backlog |
| **Thu 27 Aug** | 📦 Final submission | hard deadline 2 Sep 16:00 |
| **Sat 19 Sep** | 🎓 Defence | target before 15 Sep |

**Do not re-litigate the schedule.** Record the position; he decides what gives.

# 🔴🔴 THE SINGLE MOST AT-RISK ITEM

**Section 4.3 has no machine-readable input and one day to be written.** The manual-condition times,
the error counts and the interview notes exist only in his paper notebook. The Google Forms responses
have never been exported. Claude cannot write one sentence of 4.3 until both land on disk.

⚠ **Whether the five block-2 participant sessions ran on 23 August is UNCONFIRMED.** He decided that
morning to run all five **AR-first** (taking the manual-first split from 3:1 to 3:6), and then spent
the day writing. **Ask first thing next session.** If they did not run, the two-block design and every
comparison in 4.3 change shape.

# ✅ CHAPTERS

| chapter | state |
|---|---|
| 1 Introduction | CLOSED. ⚠ live defect: "The teardown model reproduces the geometry" (200x150x60 against 166x121x41) |
| 2 Literature Review | CLOSED 2026-08-17 |
| 3 Methodology | DRAFTED END TO END. 🔴 **3.3.3 says Sc4 "may be reported only as a band and never as a single value" and 4.1 currently reports it as single values.** See [[ch4_findings_progress]] |
| **4 Results** | 🔴 **4.1 nearly closed. 4.2 and 4.3 NOT STARTED** |
| 5 Discussion | owns the limitations section, the six defect items, and now three ReCiPe explanations if the ranking is published |
| 6 Conclusion | brief strengths-and-weaknesses-against-findings summary |
| Appendix I to VII | NX drawings · CIRPASS Table 6 · BOM · LCA inventory · manual v2 · consent form · questionnaire |
| **Appendix VIII** | **NEW. Monte Carlo percentile summary**, delivered as .xlsx |

# 🔴 COMPILE-DAY BACKLOG, the items that change text

1. **Sc4 band clauses** in 4.1, or 3.3.3 and 4.1 contradict each other.
2. **"route" to "scenario"**, 25 paragraphs. List at `MAIN PAPER/route_to_scenario_checklist.md`.
   ⚠ **Never find-and-replace.** Three break, one of them the main research question.
3. **Table 8 caption** does not say gross. **Table 9 caption** still says "route" and owes its
   derivation note. **Table 8 needs seven significant figures** to match Figure 21.
4. **LIST OF TABLES and LIST OF FIGURES are stale.** Refresh fields.
5. ⚠ **The 3.5 measures table has no caption in the body.** Captions jump Table 6 to Table 7.
6. The Methodology opener: false sentence, "four stages" collision, missing verb.
7. Chapter 1: "reproduces the geometry".
8. Delete "named as RBv2.1.1" from 3.5's opening sentence.
9. Delete the SRH template boilerplate still live under FINDINGS, DISCUSSION and CONCLUSION.
10. Where the counterbalancing deviation lives. Currently nowhere.
11. **The usability scale citation and its benchmark.** Still the one gap with no flag-free alternative.
12. Email Saman Ghobadian for written agreement on the 2026-08-13 research-question changes.

# ✅ CITATIONS CLOSED THIS SESSION

- **Andreasi Bassi et al. (2023), EUR 31414 EN, printed p. 5** for the sixteen categories and the
  definitions of normalisation and weighting. Verified first-hand.
- **Commission Recommendation (EU) 2021/2279, Annex I, 6.3.1**, OJ page L 471/223, for the ≥80 %
  most relevant categories rule. ⚠ **Not yet on disk.** Download it into LITERATURE.

# Google Calendar

⚠ Returned zero events again on 2026-08-23, the fourth consecutive wake-up. **The Notion Task
Tracker is the schedule of record.** Re-test once each wake-up, do not investigate.

# Notion session log

Latest row: **Session 40 (2026-08-23)**. The next session is **41**. Still UNLOGGED: the
2026-08-02/03 backfill.

# Diagram channel

**Miro** for diagrams, **the LCA_explorer notebook** for data charts. Board "Master Thesis"
(`uXjVGpjOAoU`). ⚠ Delete superseded frames: two old Figure 12s, four landscape Figures 14 to 17.
⚠ Captions on Figures 8 to 12 must say Appendix IV.

# Run-order trap (durable)

Teardown builders (menu 05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only) and
layer-hide. Never run while the stage clone exists. Rig rebuild is always 09 → 10 → 11 → 12 → 13 → 14
→ Verify → save.

Related: [[ch4_findings_progress]], [[lca_results_verified_ch4]], [[results_chapter_start_here]],
[[study_design_verified]], [[research_questions_final]], [[voice_and_verification_rules]],
[[lca_methodology_3_3]], [[session_logging_routine]]
