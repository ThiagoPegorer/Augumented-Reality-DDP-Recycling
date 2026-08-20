---
name: thesis-schedule
description: Live thesis state, refreshed at the close of Session 37 (2026-08-20). Chapters 1 and 2 CLOSED; Methodology opener, 3.1, 3.2, 3.2.1, 3.2.2 and 3.2.3 CLOSED. 3.3, 3.4, 3.5 and 3.6 all sit on ONE day, 21 August, and 3.5 is still blocked on study data that is not in the repository.
type: project
---

# 🔴 SCHEDULE REALITY CHECK, close of 2026-08-20

Thiago has put 3.3, 3.4, 3.5 and 3.6 on tomorrow. By the chapter's own estimates that is roughly
twenty-five paragraphs in one day, and 3.5 CANNOT BE WRITTEN AT ALL until the manual-condition timings
and the study design exist somewhere he can read them. See study_build_version_finding.md.

The realistic outcome of 21 August is 3.3, 3.4 and 3.6 written, and a hole where 3.5 goes. THE SAME
MISSING DATA ALSO BLOCKS THE RESULTS CHAPTER scheduled for 22 August. One evening of transcription
unblocks both. Nothing else on the plan does.

DO NOT RE-LITIGATE THE SCHEDULE WITH THIAGO. Record the position; he decides what gives.

# 🔴 THE PLAN, from his own tracker read 2026-08-20

| day | task | notes |
|---|---|---|
| Wed 19 Aug | Write Methodology | ✅ Done. 3.1, 3.2, 3.2.1 |
| Thu 20 Aug | Methodology 3.2.2, 3.2.3 and 3.3 | ⚠ PARTIAL. 3.2.2 and 3.2.3 closed; 3.3 not started |
| Fri 21 Aug | Methodology 3.4, 3.5 and 3.6 | now also carries 3.3. 3.5 blocked |
| Sat 22 Aug | Write Results chapter | blocked on the same study data |
| Sun 23 Aug | Write Discussion chapter | |
| Mon 24 Aug | Write Conclusion and Abstract | |
| Tue 25 Aug | Compile all chapters and formatting | the only day left for the whole backlog |
| Wed 26 Aug | 📦 Final submission, USB drive to Exam Office | hard deadline 2 Sep 16:00 |
| Sat 19 Sep | 🎓 Defence and grading | target before 15 Sep |

There is NO dedicated code-review day any more, and Foreword is not a tracked task.

# Chapters

| chapter | state |
|---|---|
| 1 Introduction | CLOSED 2026-08-13. ⚠ Live defect: "The replica reproduces the geometry" |
| 2 Literature Review | CLOSED 2026-08-17 |
| 3 Methodology | opener ⚠ (one FALSE sentence), 3.1 ✅, 3.2 ✅, 3.2.1 ✅, 3.2.2 ✅, 3.2.3 ✅. 3.3 next |
| 4 Findings | Not started. Blocked with 3.5 on the missing study record |
| 5 Discussion | Not started |
| 6 Conclusion | Not started |
| Appendix I | The eight NX drawings. ⚠ Renumber figures to I.1 ... |
| Appendix II | CIRPASS Table 6 reproduced as a true copy. Permission verified. Caption as Figure II.1 |

# Google Calendar

⚠ The connector disconnected and reconnected during Session 37 and was NOT re-tested. It returned zero
events on 08-17, 08-18 and 08-19. THE NOTION TASK TRACKER IS THE SCHEDULE OF RECORD. Re-test once each
wake-up and do not investigate further.

NOTION QUERY TRAPS. The tracker carries one row per working day per task, so query ALL statuses. Rows
are added and removed between sessions, so re-query rather than trusting a cached table. Column names in
SQL are date:<Prop>:start, never "Date".

# Fixed dates

26 August delivery to the University - 2 September 16:00 hard deadline - defence target before 15 Sep.

# 🔴 THE BACKLOG, now 14 items, with only 25 August to hold them

1. Fix the Methodology opener's false sentence about the frozen version. One sentence, highest value.
2. Fix Chapter 1: "The replica reproduces the geometry."
3. Heading typo: "The Product and it Digital Product Passport" -> its.
4. Copy-edit the closing sentence of 3.2.1; check for any surviving "three red bars are three sensors".
5. Delete AR_DPP/backend/data/vcu_001.json and its .bak_before_rename.
6. Delete the hardlinked Operation-Manual_MS50.4P.pdf and its (1) duplicate, rename _FIXED back; delete
   one of the two data sheet duplicates.
7. Two connector drawings side by side: decide which is current.
8. Verify the Appendix I bottom-housing image shows diameter 4.
9. Renumber Appendix figures; bump SHEET REV on the regenerated NX sheets.
10. Audit the CIRPASS year letters across the document (three deliverables share 2024).
11. Add the three Bosch reference entries. See bosch_sources_verified.md.
12. Rebuild _index/lit_index.jsonl, stale since 15 August. Also: dpp_eol.pdf and
    DPP_vcu_connecting_producers_recyclers.pdf are the same paper; "Driving sustainable circular economy
    in electronics.pdf" is still the one-page abstract.
13. American spelling pass across the .docx.
14. Number inconsistencies for the compile: BOM cited as 2026-07-25 while its header says v4.1 on
    2026-07-24; type_number is the data sheet's order number and differs from the declaration's type;
    BOM says ~12 fasteners against 14; 245099915 appears in the payload and in the BOM header.

Carried: Adisorn is p. 9 not p. 10; Chen et al. pages are pre-print pages.
✅ Closed this session: the five CIRPASS in-text conversions (cancelled by the group-author scheme) and
the Jensen page, both fixed by Thiago.

# Notion session log

Latest row: Session 37 (2026-08-20). The next session is 38. Still UNLOGGED: the 2026-08-02/03 backfill.

# Run-order trap (durable, do not lose)

Teardown builders (menu 05/06) bind FindFirstObjectByType<DisassemblyAnimator> (active-only) and
layer-hide. Never run while the stage clone exists. If they must: delete DppSuperPanel first, then
rebuild 10->14 ascending. Rig rebuild is always 09 -> 10 -> 11 -> 12 -> 13 -> 14 -> Verify -> save.

# Diagram channel

MIRO IS NOW THE STANDARD CHANNEL for thesis diagrams. Board "Master Thesis" (uXjVGpjOAoU). Figures 5 and
6 live in the frame "Methodology 3.2.3 - Figures 5 and 6". Thiago exports PNG from there.
⚠ Tell him to zoom in before exporting; Miro exports at the current zoom.

Related: [[ch3_methodology_progress]], [[study_build_version_finding]], [[bosch_sources_verified]],
[[methodology_start_here]], [[ch1_verbatim_facts]], [[table6_coverage_map]],
[[modelled_unit_composition]], [[teardown_model_as_built]], [[voice_and_verification_rules]],
[[rbv2-1-1-handoff]]
