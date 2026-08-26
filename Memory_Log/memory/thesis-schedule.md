---
name: thesis-schedule
description: "Live thesis state, refreshed at the close of Session 43 (2026-08-26). CHAPTER 4 IS CLOSED AND PASTED. CHAPTERS 5 AND 6 BOTH DRAFTED END TO END, neither pasted. Only the Abstract, Foreword, Acknowledgements and the take-home messages remain. Submission Monday 31 August."
type: project
---

# 🟢 WHERE THINGS STAND, close of Session 43 (2026-08-26)

- **CHAPTER 4 IS CLOSED AND IN THE DOCUMENT.** Verified from the .docx on 2026-08-26. 4.1 including
  the ReCiPe block, 4.2 (740 words, Figures 26 to 38) and 4.3 (1,830 words, Tables 19 to 22, Figures
  39 to 44) are all pasted. **The paste queue is closed.**
- **CHAPTER 5 IS DRAFTED END TO END.** Opener plus 5.1, 5.2 and 5.3, all signed off in session. Paste
  source: `Memory_Log/memory/chapter_5_discussion_FULL.md`. See [[ch5_discussion_progress]].
- **CHAPTER 6 IS DRAFTED.** 6.1 Answering the research questions and 6.2 Outlook. Paste source
  `Memory_Log/memory/chapter_6_conclusion_draft.md`. See [[ch6_conclusion_progress]].
- **NEXT SESSION: Abstract, Foreword, Acknowledgements**, plus the take-home messages closing
  Chapter 6, which are his own.
- 🔴 **CITATIONS ARE REQUIRED IN EVERY CHAPTER, Conclusion included.** SRH template, verbatim:
  *"it is outmost imperative that you cite every sentence which you didn't author"*, and the list of
  what must be cited names legal statutes and regulations explicitly.
- ✅ **VERIFIED REGULATION DATES**, Regulation (EU) 2026/1738 p. 29: repeals from 1 Sep 2028,
  Articles 11 and 29 from 1 Sep 2029, Article 33 from 1 Jan 2030, Article 13 from 1 Sep 2032.
  ⚠ No source on disk for a DPP date outside the vehicle regulation.

**Live numbering, read from the document 2026-08-26:** Tables run to 22, Figures to 44 in the core
text. Appendices run to IX. `LIST OF GENERATIVE AI TOOL USAGES` and `AFFIDAVIT` sections exist.

**Chapter 4's live heading names:** 4.1 Life Cycle Assessment Results, 4.2 AR DPP Prototype,
4.3 Voluntary participants tests, with 4.3.1 Participants, 4.3.2 Task completion time and errors,
4.3.3 Perceived usability and interview responses.

# 🔴 THE PLAN, submission Monday 31 August

| day | task | notes |
|---|---|---|
| Tue 25 Aug | Results | ✅ 4.2 and 4.3 closed and pasted the same night |
| **Wed 26 Aug** | 🟢 **Discussion DRAFTED** (Task Tracker 12:00 to 19:00), 🔴 Conclusion and Abstract (19:00 to 02:30) | |
| Thu 27 to Sun 30 Aug | compile-day backlog, affidavit denoting | four Task Tracker rows, one per day |
| **Mon 31 Aug** | 📦 **Submission**, 12:00 to 14:00 Berlin | hard deadline 2 Sep 16:00 |
| Sat 19 Sep | 🎓 Defence | target before 15 Sep |

**Do not re-litigate the schedule.** Record the position; he decides what gives.

# 🔴 THE THREE MOST AT-RISK ITEMS

1. 🔴🔴 **THE AFFIDAVIT DENOTING.** Claude's prose is pasted verbatim across all of Chapter 4, and the
   whole of Chapter 5 is Claude prose too. SRH requires AI phrasing **denoted in the core text**.
   `ai_use_log.md` was last modified **2026-08-23**, so Sessions 41, 42 and 43 have no entry. This is
   the single item that can fail an otherwise finished thesis.
2. 🔴 **TWO DOCUMENT DEFECTS BLOCK PASTING CHAPTER 5.** Section 1.3 says the teardown model
   "reproduces the geometry" (5.2 contradicts it) and section 4.1.6 blames the units for the ReCiPe
   ranking mismatch (the cause is the normalization references). See
   `thesis_review_backlog_2026-08-26.md`, items A1 and A2.
3. 🔴 **THE CONDITION-ORDER CONTRADICTION.** The 2026-07-21 study spec sets *"odd participant IDs
   start 2D-first, even AR-first."* The real order breaks it for **P03** and **P04**. If that rule is
   stated in the .docx it contradicts Table 21.

# ✅ CHAPTERS

| chapter | state |
|---|---|
| 1 Introduction | CLOSED. 🔴 live defect: "reproduces the geometry" |
| 2 Literature Review | CLOSED 2026-08-17 |
| 3 Methodology | DRAFTED. 🔴 three edits owed to the impact assessment subsection, three provenance edits, two "replica" swaps |
| 4 Results | 🟢 **CLOSED AND PASTED end to end** |
| **5 Discussion of Findings** | 🟢 **DRAFTED end to end, Session 43.** Not yet pasted. SRH boilerplate still live under the heading |
| **6 Conclusion** | 🟢 **DRAFTED, two subsections.** Not pasted. Take-home messages owed, his own |
| Abstract, Foreword, Acknowledgements | 🔴 **NEXT SESSION** |

# ✅ THE SRH CHAPTER CONTRACTS, read from the template 2026-08-26

**DISCUSSION OF FINDINGS**, five instructions: three-sentence summary of findings, what the data mean
with reference to the research objectives, methodological limitations, comparison against the
literature review, transferability. ⚠ Thiago overrode the three-sentence limit deliberately.

**CONCLUSION**, four instructions: the author's own thoughts and conclusions, integrating
introduction, literature review, research design and findings; the possible impact for industry,
society and the scientific community, plus **whether the strengths and weaknesses of the
methodological approach showed up in the findings**; new questions as potential for further research;
a closing section of **clear take-home messages** with a comment on the relevance of the findings.

# 🔴 COMPILE-DAY BACKLOG

The full list, fifty-odd items in five groups, is in **`thesis_review_backlog_2026-08-26.md`**, built
by reading the whole thesis PDF. Group A holds the three alarming items, B the gaps, C grammar and
typography, D what is still owed from earlier sessions, E what was verified correct.

# ✅ THE Sc4 BAND CONTRADICTION, resolved by relocation

**3.3.3 currently ends:** *"Its functional yield is unsourced, so that scenario may be reported only
as a band and never as a single value."*
**Replace with:** *"Its functional yield is unsourced, so the deterministic result for that scenario
is reported together with its simulated interval, and that interval is given with the uncertainty
results."*

# ✅ THE 4.3 ARTEFACT CHAIN

`ARDPP_study_data.xlsx` (raw only) to `ARDPP_study_analysis.ipynb` (20 cells) to
`ARDPP_study_results.xlsx` (nine derived sheets) plus `ARDPP_figure_audit.xlsx` (21 checks). All four
are in `Memory_Log/memory/`. The participant-level cross-check derived from them is in
[[ch5_discussion_progress]].

# Google Calendar

⚠ Zero events for six consecutive wake-ups. **The Notion Task Tracker is the schedule of record.**
Not re-tested on 2026-08-26; do not investigate.

# Notion session log

Latest row: **Session 43 (2026-08-26)**, written at the close of this session. Next is **44**. Still UNLOGGED: the
2026-08-02/03 backfill.

# Diagram channel

**Miro** for diagrams, **LCA_explorer** for LCA data charts, **`ARDPP_study_analysis.ipynb`** for the
user-study charts. Board "Master Thesis" (`uXjVGpjOAoU`).

# Run-order trap (durable)

Teardown builders (menu 05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only) and
layer-hide. Never run while the stage clone exists. Rig rebuild is always 09, 10, 11, 12, 13, 14,
Verify, save.

Related: [[ch6_conclusion_progress]], [[ch5_discussion_progress]], [[ch4_findings_progress]], [[study_results_verified]],
[[recipe_cross_check_verified]], [[lca_results_verified_ch4]], [[study_design_verified]],
[[research_questions_final]], [[voice_and_verification_rules]], [[session_logging_routine]],
[[ch3_methodology_progress]], [[teardown_model_as_built]]
