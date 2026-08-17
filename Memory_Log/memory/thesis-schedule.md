---
name: thesis-schedule
description: Live thesis state, rebuilt 2026-08-17 (Session 34) from the Notion Task Tracker and Thiago's own confirmation. Chapters 1 and 2 CLOSED, Methodology opener and 3.1 CLOSED. ALL PARTICIPANT STUDY SESSIONS ARE COMPLETE. Submission target 25 August, hard deadline 2 September 16:00.
type: project
---

# Where the thesis actually is (end of 2026-08-17)

# THE STUDIES ARE DONE

Thiago confirmed on 2026-08-17: "All the studies is already done." This overrides the forward-looking
wording in rbv2-1-1-handoff section 4, which still reads as though the sessions were upcoming.

STILL UNRECORDED ANYWHERE, and section 3.5 needs all of it:
- how many participants took part, and their group assignment
- within or between subjects
- what was captured (step times, errors, questionnaire, which instrument)
- where the raw files live on disk
- whether any session deviated from the protocol, and the build version flashed
ASK HIM. Do not infer.
Note: Chapter 1 describes participants as "of mixed background: people experienced in electronics, people
experienced in Augmented Reality, and people meeting both the device and the technology for the first
time." See ch1_verbatim_facts.

# Chapters

| chapter | state |
|---|---|
| 1 Introduction | CLOSED 2026-08-13, corrected 08-16 against Regulation (EU) 2026/1738 |
| 2 Literature Review | CLOSED 2026-08-17. Jensen et al. (2023) added to block 2.4 on 08-17, plus six typo fixes |
| 3 Methodology | opener CLOSED, 3.1 CLOSED, 3.2 is next. See ch3_methodology_progress |
| 4 Findings | Not started. Study data exists; the LCA number trace is still owed |
| 5 Discussion | Not started |
| 6 Conclusion | Not started |

# The real interim dates, from the Notion Task Tracker

Tracker times are UTC; Berlin is UTC+2. THE TRACKER CARRIES ONE ROW PER WORKING DAY PER TASK, so a task
appears several times. Read the whole set before judging a status, and query ALL statuses, not just
"not Done" - filtering hides the rows that prove what already happened.

| date (UTC) | task | status |
|---|---|---|
| 08-11 to 08-13 | Write Introduction (4 rows) | Done on three |
| 08-12 to 08-17 | Write Literature Review chapters (5 rows) | Done on four |
| 08-17 13:00 | Write Methodology | Not Started (worked on this session) |
| 08-19 07:15 | Write Methodology (second working day) | Not Started |
| 08-19 19:15, 08-20 07:15, 08-21 07:15 | Write Results chapter | Not Started |
| 08-22 06:30 | Write Conclusion & Abstract | Not Started |
| 08-23 07:00 | Write Discussion chapter | Not Started |
| 08-24 07:00 | Compile all chapters & thesis formatting | Not Started |
| 08-26 10:00 | Final submission, USB drive to Exam Office | Not Started |
| 09-19 | Defence and grading (target before 15 Sep) | Not Started |

ONE ORDERING DEFECT, flagged 2026-08-17 and not yet resolved: Conclusion & Abstract (08-22) is scheduled
BEFORE Discussion (08-23). The Conclusion integrates the Discussion, so as ordered the integration is
written before the interpretation.

# Google Calendar

Standing instruction from Thiago (2026-08-17): check the Calendar as well as Notion at every wake-up and
write what it says into memory. THE CONNECTOR CURRENTLY RETURNS ZERO EVENTS on both real calendars
(thiagomp4903@gmail.com and the read-only import "My Schedule"), with and without a time range. Metadata
comes back fine. Until that is fixed the Notion Task Tracker is the schedule of record. Re-test each
wake-up.

# Fixed dates

25 August submission target | 26 August 10:00 tracker submission slot | 2 September 16:00 hard deadline |
defence target before 15 September.

# Corpus defects still open

1. CIRPASS D2.2: a working copy CIRPASS_D2.2_DPP_UseCases_Report_v2.0_FIXED.pdf was made 2026-08-17 and
   IS readable. The ORIGINAL is still hardlinked. Thiago must delete it by hand and rename the copy back.
2. _index/lit_index.jsonl stale since eighteen AR PDFs and Regulation (EU) 2026/1738 landed on 08-16.
3. dpp_eol.pdf and DPP_vcu_connecting_producers_recyclers.pdf are the same paper. Delete one.
4. "Driving sustainable circular economy in electronics.pdf" is still the one-page abstract.
5. Adisorn page error (p. 9, not p. 10) in text already pasted; Chen et al. pages in 2.5 are pre-print.
6. The American spelling pass across the .docx.
7. Five CIRPASS in-text citations to convert to the hybrid author scheme.

# Notion session log

Latest row before this session: Session 33 (2026-08-17, Chapter 2 closed). This session is 34.
Query live before writing a new row; other Cowork projects increment the same counter.
Still UNLOGGED: the 2026-08-02/03 backfill.

# Run-order trap (durable, do not lose)

Teardown builders (menu 05/06) bind FindFirstObjectByType<DisassemblyAnimator> (active-only) and
layer-hide. Never run while the stage clone exists. If they must: delete DppSuperPanel first, then
rebuild 10-14 ascending. Rig rebuild is always 09 -> 10 -> 11 -> 12 -> 13 -> 14 -> Verify -> save.

Related: ch3_methodology_progress, methodology_start_here, ch1_verbatim_facts, rbv2-1-1-handoff,
voice_and_verification_rules, lca_findings_for_writing
