---
name: thesis-schedule
description: Live thesis state, refreshed at the close of Session 39 (2026-08-22). CHAPTER 3 IS DRAFTED END TO END. 3.4 and 3.5 closed today, and 3.6 was deleted from Methodology because the SRH template puts methodological limitations in the Discussion. Next is the Results chapter. Five more participants run on 23 August on RBv2.1.1, which is also the first Results day.
type: project
---

# 🟢 WHERE THINGS STAND, close of Session 39

**Chapter 3 is drafted end to end.** Session 39 closed 3.4 (intro, 3.4.1, 3.4.2, 3.4.3) and 3.5
(purpose and conditions, the manual, the questionnaire), built five Miro figures, delivered three
appendix documents, and removed 3.6 from the chapter entirely.

**3.5 was never actually blocked.** The block recorded since 2026-08-17 listed "the manual-condition
timings, the study design, the interface feedback and the participant backgrounds". Three of those
four are RESULTS. The design had been on disk in `AR_DPP/Docs/` since 2026-07-21. That was a category
error costing five days of a section sitting marked unwritable. **Check what a block actually is
before recording it.**

# 🔴 THE PLAN

| day | task | notes |
|---|---|---|
| **Wed 19 Aug** | Write Methodology | ✅ 3.1, 3.2, 3.2.1 |
| **Thu 20 Aug** | Methodology 3.2.2, 3.2.3 | ✅ closed |
| **Fri 21 Aug** | Methodology 3.3 | ✅ closed in full |
| **Sat 22 Aug** | Methodology 3.4, 3.5, 3.6 | ✅ **3.4 and 3.5 closed. 3.6 moved to the Discussion** |
| **Sun 23 Aug** | Write Results chapter | 🔴 **also carries five more participant sessions** |
| **Mon 24 Aug** | Write Results chapter | |
| **Tue 25 Aug** | Discussion + Conclusion + Abstract | 🔴 **now also carries the limitations section moved out of 3.6** |
| **Wed 26 Aug** | Compile all chapters and formatting | the only day for the backlog |
| **Thu 27 Aug** | 📦 Final submission | hard deadline 2 Sep 16:00 |
| **Sat 19 Sep** | 🎓 Defence | target before 15 Sep |

**Do not re-litigate the schedule.** Record the position; he decides what gives.

# 🔴 TOMORROW, 23 August: five more participants, and it is also Results day one

- **Build: RBv2.1.1**, not the RBv1.0 that P02 to P05 used. Thiago's explicit decision.
- **Manual: v2**, tables removed, step names matched to RBv2.1.1. Delivered today.
- So the study is **two blocks** and nothing pools across them without saying so. See
  [[study_design_verified]].
- **Standing recommendation, given twice, not yet answered: run all five AR-first.** The first block
  ran three of four manual-first, so the second-attempt advantage currently helps the AR condition.
  Five AR-first sessions take the split from 3:1 to 3:6 and tilt the residue conservatively.

# Chapters

| chapter | state |
|---|---|
| 1 Introduction | CLOSED. ⚠ **Live defect: "The teardown model reproduces the geometry"** (it does not; 200x150x60 against 166x121x41) |
| 2 Literature Review | CLOSED 2026-08-17 |
| 3 Methodology | ✅ **DRAFTED END TO END.** opener ⚠ three defects · 3.1 to 3.5 closed · 3.6 deleted. See [[ch3_methodology_progress]] |
| 4 Findings | **NEXT.** ⚠ Its headline table is built on "net", which the supervisor abolished. Re-verify monotonicity on the SAVING column |
| 5 Discussion | 🔴 **Now also owns the limitations section**, drafted today. Plus the six defect items and the disjoint-reporting-set finding |
| 6 Conclusion | Template asks for a brief strengths-and-weaknesses-against-findings summary |
| Appendix I to IV | NX drawings · CIRPASS Table 6 · bill of materials · LCA inventory |
| **Appendix V** | **2D manual v2. Delivered 2026-08-22** |
| **Appendix VI** | **Participant consent form.** On disk at `AR_DPP/Docs/` |
| **Appendix VII** | **User study questionnaire. Delivered 2026-08-22** |

⚠ **LIST OF EQUATIONS must be KEPT.** 3.3.2 carries Equation 1.

# Google Calendar

⚠ The connector returned zero events on 08-17, 08-18 and 08-19 and was not re-tested since. **The
Notion Task Tracker is the schedule of record.** Re-test once each wake-up, do not investigate.

**Notion query traps.** One row per working day per task, so query ALL statuses. Column names in SQL
are `date:<Prop>:start`, never `"Date"`.

# Fixed dates

**27 August** submission · **2 September 16:00** hard deadline · defence target before **15 September**.

# 🔴 THE COMPILE-DAY BACKLOG

The full list lives in [[ch3_methodology_progress]] under OPEN ITEMS, twenty entries. The ones that
change text rather than files:

1. The Methodology opener: false sentence, "four stages" collision, missing verb.
2. Chapter 1: "reproduces the geometry".
3. Delete "named as RBv2.1.1" from the 3.5 opening sentence, which contradicts 3.4 three pages away.
4. The usability scale citation and its benchmark. **The one gap with no flag-free alternative.**
5. Where the counterbalancing deviation lives. Currently nowhere.
6. What Table 4 is.
7. Delete the SRH template boilerplate still live in the .docx.
8. The 3.3.3 ReCiPe 2010 reword; the five 3.3 citations still open.

# Notion session log

Latest row: **Session 39 (2026-08-22)**. The next session is **40**. Still UNLOGGED: the 2026-08-02/03
backfill.

# Diagram channel

**Miro is the standard channel.** Board "Master Thesis" (`uXjVGpjOAoU`). One frame per figure.
⚠ Tell him to zoom in before exporting; Miro exports at the current zoom.
⚠ Delete superseded frames: two old Figure 12s, and four landscape Figures 14 to 17.
⚠ Captions on Figures 8 to 12 must say **Appendix IV**, not III.

# Run-order trap (durable)

Teardown builders (menu 05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only) and
layer-hide. Never run while the stage clone exists. Rig rebuild is always 09 → 10 → 11 → 12 → 13 → 14
→ Verify → save.

Related: [[ch3_methodology_progress]], [[study_design_verified]], [[rbv2_1_1_ar_system_verified]],
[[lca_methodology_3_3]], [[study_build_version_finding]], [[voice_and_verification_rules]],
[[bosch_sources_verified]], [[session_logging_routine]]
