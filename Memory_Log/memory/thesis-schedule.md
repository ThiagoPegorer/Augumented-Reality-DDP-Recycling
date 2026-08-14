---
name: thesis-schedule
description: Live thesis schedule (refreshed 2026-08-14 from the Notion Task Tracker). Chapter 1 CLOSED 2026-08-13; Literature Review is 2 days late; Methodology now collides with study day 1. RBv2.1.1 frozen 2026-08-10. Includes the run-order trap.
type: project
---
# Milestone: RBv2.1.1 OFFICIALLY FROZEN 2026-08-10 (Thiago's declaration, Session 27)

Prototype work is OVER except device-pass bug fixes (new version number each; last build >= 1 h
before a participant; no changes to step content/order/difficulty). See [[rbv2-1-1-handoff]]
§THE FREEZE. From here, EVERY session should default to WRITING unless a study session or a
device bug says otherwise.

# Where the writing actually is (refreshed 2026-08-14)

**✅ Chapter 1 is CLOSED (Session 30, 2026-08-13).** Sections 1.1, 1.1.1, 1.2 Objectives,
1.3 Scope & Limitations and 1.4 Research Questions are written. 1.5 Structure/Overview was skipped
as optional. Details in [[introduction_progress]] and [[research_questions_final]].

Three things changed in Chapter 1 that propagate forward:
1. **Participants dismantle a 3D-printed replica, not a real VCU** — disclosed and scoped, not apologised for.
2. **The reference device is a Bosch Motorsport MS 50.4** because no series-production VCU datasheet could be obtained.
3. **The research questions were rebuilt** around a main question naming the recycler and the end-of-life routes; the registered set asked no environmental question. **Hypotheses were dropped** — the SRH template never asks for them.

**⬜ Literature Review is TWO DAYS LATE.** The Task Tracker carries two open rows: one starting
2026-08-12 and one starting 2026-08-14, both `Not Started`. Nothing of it is written.

# Thesis schedule (Notion Start Dates are the trusted field; re-queried 2026-08-14)

- ~~08-10 Introduction~~ → written 08-11 and 08-13. **DONE.**
- **08-12 + 08-14** — Literature Review (two rows, both still Not Started). **THIS IS THE SLIP.**
- **08-15 08:30** — Methodology. ⚠ Same day as data collection (12:30–15:30) + lab study **Jin**
  (15:30–17:30) + Results (18:45–02:30). Four things on one day.
- **08-16 11:30** — Results. **08-17 06:15** — Results again + **08-17 12:15** Discussion +
  lab study **Neighboors** (~19:45).
- **08-18 / 08-19** — Conclusion & Abstract. **08-20 / 08-21** — Compile & formatting.
- **08-25** — submission target. **Sep 2, 16:00** — hard deadline.

**Most at-risk item (unchanged in kind, worse in degree):** Methodology is now scheduled on study
day 1, and Discussion on study day 2. Interpretation would be written hours after the data lands,
with no buffer. Methodology is also the chapter that OWNS the prototype description, so it is the
one chapter that could have been written any time in the last two weeks and was not.

**Second risk:** the Findings chapter is blocked until every LCA headline number is traced to its
named CSV row. All are `[M]` under the zero-trust rule. That verification pass must NOT land on
08-15/16 when study data also arrives.

**Note on the tracker:** the separate "Write Prototype Development chapter" row that
[[writing_phase_setup]] warned about no longer appears in the open task set as of 2026-08-14.
Either it was renamed or closed. The prototype belongs to Methodology; there is no such chapter.

# Notion session log status

Latest row: **Session 31 — 2026-08-14** (memory system rebuilt; OPEN row, Part 2 pending — see
[[session_logging_routine]]).
Session 30 — 2026-08-13 (Chapter 1 closed). Session 29 — 2026-08-11 (first thesis words).
UNLOGGED: only the 2026-08-02/03 backfill.

# Run-order trap (durable — do not lose)

Teardown builders (menu 05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only)
and layer-hide — **never run while the stage clone exists**. If they must: delete
`DppSuperPanel` first, then rebuild 10→14 ascending. Rig rebuild is always the chain
09 → 10 → 11 → 12 → 13 → 14 → Verify → save (10 destroys ProductSpecsPage, the guided-mode
rail/pages/controller and the stage clone).
