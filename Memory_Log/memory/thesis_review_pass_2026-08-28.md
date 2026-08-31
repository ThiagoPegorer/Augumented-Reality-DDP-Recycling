---
name: thesis-review-pass-2026-08-28
description: "The final revision review of the full thesis, Session 45 (2026-08-28). The agreed four-pass structure and fixed output schema, where the 204 findings live, the six that matter most, and the five find-and-replace traps. READ THIS BEFORE ANY FURTHER REVIEW WORK."
type: project
---

# THE AGREED REVIEW STRUCTURE, his decision 2026-08-28

He asked for a task-based routine and rejected the first two output formats as inconsistent.
**One schema for every finding, forever:** `ID | p. | Live text | Change to | Why`.
Live text verbatim and ctrl-F-able. Change to is paste-ready. Why is eight words.

**Severity is fixed and applies INSIDE every pass**, and output is grouped by pass then by
severity, never in document order. His words: *"dividing the outputs not in document order but in
task pass order"*.
**A** wrong or self-contradictory · **B** an examiner marks it · **C** cosmetic.

**His five tasks map onto four passes.** He chose meaning-first over his own 1-to-5 order, and
"everything at once" over chapter-by-chapter delivery.

| pass | his tasks | scope |
|---|---|---|
| P1 MEANING | 3 terminology · 4 contradictions · 5 data | whole document, one shot |
| P2 MECHANICAL | 1, the global half | find-and-replace |
| P3 SENTENCE | 1 + 2 merged, the local half | chapter by chapter |
| P4 FRONT AND BACK MATTER | all five, plus citations | last |

**Why not his order:** tasks 1 and 2 cannot be separated in one reading, and they edit the
sentences that 3, 4 and 5 rewrite. Task 4 cannot run chapter by chapter at all.

# WHERE THE 204 FINDINGS ARE

`Memory_Log/memory/`: **`REVIEW_INDEX_2026-08-28.md`** first, then
`review_P1_meaning_2026-08-28.md` (28) · `review_P2_mechanical_2026-08-28.md` (48) ·
`review_P3_sentence_2026-08-28.md` (99) · `review_P4_frontmatter_2026-08-28.md` (29).
The routine itself is `review_routine_2026-08-28.md`. The Chapter 4 data working is
`review_ch4_LCA_dataverify_2026-08-28.md`. Two files are prefixed `SUPERSEDED_`.

68 sev A, 103 sev B, 33 sev C.

# THE SIX THAT MATTER MOST

1. **The Abstract's gross-burden caveat is GONE AGAIN, third time.** Verification rule E held.
2. **"prototype" names two different objects**, ReBuilt and the 3D printed model. Pages 42, 43, 44,
   45, 53, 61, 62, 69 plus Figures 3 and 4 and the Table 2 column head. 61 occurrences total.
3. **Six CIRPASS in-text citations point at reference entries that do not exist.** The body cites
   "CIRPASS, 2024a" and "CIRPASS, 2024b"; the list has CIRPASS (D2.1), (D2.2), (D2.3), (D5.1).
4. **Backlog A2 still live.** 4.1.6 blames the units for the ReCiPe ranking mismatch.
5. **Table 4 sources the use-phase power draw to the "MS 5.0 manual".** The unit is the MS 50.4.
6. **The table of contents is stale**, disagreeing with three body headings. Refresh LAST.

# VERIFIED SOUND. Never re-derive.

- **Chapter 4.1: ZERO data errors**, every number recomputed from the CSVs.
- **Chapter 4.3: ZERO data errors** except a 60.4 against 60.48 rounding. **All seventeen per-step
  rows of Table 22 sum correctly.**
- Figures 1 to 86, Tables 1 to 37, Equation 1, Appendices I to IX: complete and in order.
- The four em dashes are all inside published titles. Correct.
- **The title page now has both supervisor names and a date.**
- **Backlog A1, B3, B4, D2 and D4 are CLOSED.** B3 in particular: the Methodology never states the
  odd/even rule, so there is no contradiction with Table 21.
- The Abstract runs **308 words against a 300 limit.**

# FIVE FIND-AND-REPLACE TRAPS

1. `route` to `scenario`: 4 of 12 change. The copper route, the 490 km transport route, the generic
   recovery route in Chapter 3, "ecoinvent routes" as a verb and two interface strings stay.
2. `aluminium` to `aluminum`: 1 of 17. Sixteen are ecoinvent dataset names.
3. `standardised`: 2 of 3. The third is inside a quoted regulation.
4. `anonymised` in Appendix VI: none. It is the signed consent form.
5. `prototype` to `teardown model`: 8 pages of 61 occurrences.

# TWO NEW FACTS ABOUT THE SOURCE FILES

- `Outputs/2_eol_scenarios/scenarios_results.csv` **disagrees with** `impact_EF31.csv` (climate Sc1
  73.5718 against 73.4326). **The thesis uses impact_EF31.csv, which is the verified set.** Mark the
  other superseded before a future check opens the wrong file.
- The 54 to 189 kWh triangular range for use-phase electricity is in `LCA_framework_v4.md` line 656.

Related: [[thesis-schedule]], [[voice_and_verification_rules]], [[lca_results_verified_ch4]],
[[study_results_verified]]
