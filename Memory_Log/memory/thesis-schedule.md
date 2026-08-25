---
name: thesis-schedule
description: "Live thesis state, refreshed at the close of Session 42 (2026-08-25). CHAPTER 4 IS CLOSED end to end. Submission moved to Monday 31 August because the examination office is shut this week. Discussion, Conclusion, Abstract, Foreword and Acknowledgements are all that remain to write, plus the compile-day backlog and the paste queue."
type: project
---

# 🟢 WHERE THINGS STAND, close of Session 42

**CHAPTER 4 IS CLOSED.** 4.1 written end to end, 4.2 drafted from the headset screenshots, 4.3
drafted end to end in three subsections from a fully audited data chain. His words:
*"Results is done and close for now."*

⚠ **Closed does NOT mean pasted.** The ReCiPe block, all of 4.2 and all of 4.3 are still in
markdown files, not in the .docx. See [[ch4_findings_progress]].

# 🔴 THE PLAN, submission Monday 31 August

**The date moved from Thursday 27 to Monday 31** because the SRH examination office is closed this
week. Confirmed by Thiago 2026-08-25.

| day | task | notes |
|---|---|---|
| Sun 23 Aug | Results | ✅ 4.1 through Monte Carlo |
| Mon 24 Aug | Results | ✅ stage tables, stage pies, uncertainty, ReCiPe block drafted |
| Tue 25 Aug | Results | ✅ **4.2 AND 4.3 BOTH CLOSED.** Nine participants run, data consolidated, notebook and audit pack built |
| **Wed 26 Aug** | 🔴 **Discussion (morning) + Conclusion (afternoon)**, then Abstract, Foreword, Acknowledgements | his plan, stated at the close of Session 42 |
| Thu 27 to Sun 30 Aug | paste queue, compile-day backlog, affidavit denoting | four days of slack, the first he has had |
| **Mon 31 Aug** | 📦 **Submission** | hard deadline 2 Sep 16:00 |
| Sat 19 Sep | 🎓 Defence | target before 15 Sep |

**Do not re-litigate the schedule.** Record the position; he decides what gives.

# 🔴 THE THREE MOST AT-RISK ITEMS

1. 🔴🔴 **THE AFFIDAVIT DENOTING.** Claude's prose is now pasted verbatim across 4.1, and 4.2 and 4.3
   were delivered as prose too. SRH requires AI phrasing **denoted in the core text**, not merely
   logged. Neither the Session 41 nor the Session 42 AI-use log entry is in `ai_use_log.md`. This is
   the single item that can fail an otherwise finished thesis.
2. 🔴 **THE PASTE QUEUE.** Nothing from Sessions 41 or 42 is in the document. That is one ReCiPe
   block, three Methodology edits, three provenance edits, all of 4.2, all of 4.3, six figures and
   five tables.
3. 🔴 **A METHODOLOGY / RESULTS CONTRADICTION ON THE CONDITION ORDER.** The 2026-07-21 study spec
   sets *"odd participant IDs start 2D-first, even AR-first."* The real order breaks it for **P03**
   and **P04**. If that rule is stated in the .docx it contradicts the data. **Check before
   submission.** See [[study_results_verified]].

# ✅ CHAPTERS

| chapter | state |
|---|---|
| 1 Introduction | CLOSED. ⚠ live defect: "The teardown model reproduces the geometry" (200x150x60 against 166x121x41) |
| 2 Literature Review | CLOSED 2026-08-17 |
| 3 Methodology | DRAFTED. 🔴 **three edits owed to the impact assessment subsection** plus **three open-answer provenance edits** |
| **4 Results** | 🟢 **CLOSED end to end. Paste queue outstanding.** |
| **5 Discussion** | 🔴 **NOT STARTED. Wednesday morning.** Six inherited arguments listed in [[ch4_findings_progress]] |
| **6 Conclusion** | 🔴 **NOT STARTED. Wednesday afternoon.** |
| Abstract, Foreword, Acknowledgements | 🔴 NOT STARTED |
| Appendix I to VIII | VIII is the Monte Carlo percentile summary. 4.3 adds item-level responses and the interview record |

# ✅ THE Sc4 BAND CONTRADICTION, resolved by relocation

**3.3.3 currently ends:** *"Its functional yield is unsourced, so that scenario may be reported only
as a band and never as a single value."*
**Replace with:** *"Its functional yield is unsourced, so the deterministic result for that scenario
is reported together with its simulated interval, and that interval is given with the uncertainty
results."*

The closing paragraph of the uncertainty subsection carries the band: 0.008271 to 0.009654 kg Sb eq ·
14.32 to 17.50 kg CO2 eq · 0.022911 to 0.039715 kg P eq.

# 🔴 COMPILE-DAY BACKLOG, the items that change text

1. **Paste the ReCiPe block**, the three Methodology edits and the three provenance edits.
2. **Paste 4.2 and 4.3** with their six figures and five tables.
3. **Denote retained AI phrasing across 4.1, 4.2 and 4.3**, and paste both AI-use log entries.
4. **Decimal separators**, document-wide.
5. **"route" to "scenario"**, 25 paragraphs. `route_to_scenario_checklist.md`.
   ⚠ Never find-and-replace. Three break, one of them the main research question.
6. **Table 8 to seven significant figures.**
7. `%` against "percent" spelled out.
8. Subsection heading capitalisation; 4.1.1's en dash.
9. **LIST OF TABLES and LIST OF FIGURES.** Refresh fields after every insert.
10. Stage 5 tables published twice. He kept both; consider moving 11 to 14 to an appendix.
11. The Methodology opener: false sentence, "four stages" collision, missing verb.
12. Chapter 1: "reproduces the geometry". Delete "named as RBv2.1.1" from 3.5.
13. Delete the SRH boilerplate still live under DISCUSSION and CONCLUSION.
14. 🔴 **Check the condition-order rule in Methodology against the real order.** New, see above.
15. 🔴 **The study spec says SEVEN open questions; the export has SIX.** The missing one is the
    deliberate pro-2D probe. Correct Methodology if it states seven.
16. ⚠ **The questionnaire export is titled "Guided Disassembly RBv1.0".** Rename before the appendix.
17. ⚠ **Do not name the instrument anywhere**, Methodology and appendix included. He ruled it stays
    unnamed, which is only consistent if nothing else names it.
18. Email Saman Ghobadian for written agreement on the 2026-08-13 research-question changes.
19. Download Recommendation (EU) 2021/2279 into LITERATURE.
20. `LCA_explorer.ipynb`: fix cell 10's title, **delete cells 14, 32, 35, 36**.
21. Clean `recipe_screening_log.txt`: superseded ReCiPe 2008 endpoint screening inside it.
22. `facts_register.md` §5 is stale: it says Huijbregts et al. (2017) is missing. **It is cited.**

# ✅ THE 4.3 ARTEFACT CHAIN

`ARDPP_study_data.xlsx` (raw only) → `ARDPP_study_analysis.ipynb` (20 cells, derives everything) →
`ARDPP_study_results.xlsx` (nine derived sheets) + `ARDPP_figure_audit.xlsx` (eight sheets, 21
reconciliation checks). Six figures at 300 dpi plus SVG. Full record in [[study_results_verified]].

# Google Calendar

⚠ Zero events again, sixth consecutive wake-up. **The Notion Task Tracker is the schedule of
record.** Re-test once per wake-up, do not investigate.

# Notion session log

Latest row: **Session 42 (2026-08-25)**. Next is **43**. Still UNLOGGED: the 2026-08-02/03 backfill.

# Diagram channel

**Miro** for diagrams, **LCA_explorer** for LCA data charts, **`ARDPP_study_analysis.ipynb`** for the
user-study charts. Board "Master Thesis" (`uXjVGpjOAoU`).
⚠ Delete superseded frames: two old Figure 12s, four landscape Figures 14 to 17.

# Run-order trap (durable)

Teardown builders (menu 05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only) and
layer-hide. Never run while the stage clone exists. Rig rebuild is always 09 → 10 → 11 → 12 → 13 → 14
→ Verify → save.

Related: [[ch4_findings_progress]], [[study_results_verified]], [[recipe_cross_check_verified]],
[[lca_results_verified_ch4]], [[study_design_verified]], [[research_questions_final]],
[[voice_and_verification_rules]], [[session_logging_routine]]
