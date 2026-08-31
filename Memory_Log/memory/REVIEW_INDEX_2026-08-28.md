# THESIS REVIEW, INDEX
Built 2026-08-28 against `Master_Thesis_ThiagoPegorer_100003505_to_review_version.pdf`, 164 pages
read end to end. Routine: `review_routine_2026-08-28.md`.

## THE FOUR PASSES, apply in this order

| file | pass | your tasks | A | B | C | total |
|---|---|---|---|---|---|---|
| `review_P1_meaning_2026-08-28.md` | **P1 MEANING** | 3 terminology · 4 contradictions · 5 data | 6 | 18 | 4 | **28** |
| `review_P2_mechanical_2026-08-28.md` | **P2 MECHANICAL** | 1, the global half | 4 | 35 | 9 | **48** |
| `review_P3_sentence_2026-08-28.md` | **P3 SENTENCE** | 1 + 2, the local half | 45 | 36 | 18 | **99** |
| `review_P4_frontmatter_2026-08-28.md` | **P4 FRONT AND BACK MATTER** | all five, plus citations | 13 | 14 | 2 | **29** |
| | | | **68** | **103** | **33** | **204** |

Severity is fixed and means the same thing in every pass:
**A** wrong or self-contradictory, must fix · **B** an examiner marks it, should fix ·
**C** cosmetic, fix if time.

Every finding uses one schema: `ID | p. | Live text | Change to | Why`. Live text is verbatim and
ctrl-F-able. Change to is paste-ready.

## ORDER OF WORK, and two hard sequencing rules

1. **P1 first.** These edits rewrite whole sentences, and P3 findings sit on some of the same
   sentences.
2. **P2 second**, as find-and-replace operations. ⚠ **Except P2-A01**, the field refresh, which is
   the LAST action before the PDF export.
3. **P3 third**, chapter by chapter.
4. **P4 last.** The Abstract restates every claim in the body and can only be checked once the body
   is settled.

## THE SIX THINGS THAT MATTER MOST

1. **P4-A03 · the Abstract's gross-burden caveat is gone again.** Third time. Without it the most-read
   page reads as a claim your Table 8 rules out.
2. **P1-A01 · "prototype" names two different objects**, ReBuilt and the 3D printed model. Eight pages,
   two figure captions and one table column.
3. **P4-A08 · six CIRPASS citations point at reference entries that do not exist.**
4. **P1-A05 · 4.1.6 still states the wrong cause** for the ReCiPe ranking mismatch. Backlog A2, open
   since 26 August.
5. **P3-A25 · Table 4 sources the use-phase power draw to the "MS 5.0 manual".** The unit is the MS 50.4.
6. **P2-A01 · the table of contents is stale** and disagrees with three body headings.

## WHAT THE REVIEW FOUND SOUND. Do not spend time re-checking.

- **Every number in Chapter 4.1**, recomputed from the CSVs. Zero errors across Tables 7, 8 and 9,
  the 4.1.4 avoided values, ratios and balances, all thirteen Monte Carlo spreads and every interval
  endpoint, and all fifteen ReCiPe claims.
- **Every number in Chapter 4.3**, including all seventeen per-step rows of Table 22 summing to their
  totals. One exception, P1-B15, a rounding at 60.4 against 60.48.
- **Figures 1 to 86 and Tables 1 to 37**, complete, in order, no duplicates, every front-list entry
  matched in the body.
- **The four em dashes** are all inside published titles in the reference list.
- **The title page** now carries both supervisor names and a date.
- Backlog items **A1, B3, B4, D2 and D4 are closed.**

## FIVE PLACES WHERE A FIND-AND-REPLACE WOULD DAMAGE THE THESIS

1. `route` to `scenario`: four of the twelve change, eight are correct.
2. `aluminium` to `aluminum`: one of seventeen changes, sixteen are ecoinvent dataset names.
3. `standardised` to `standardized`: two of three, the third is inside a quoted regulation.
4. `anonymised` in Appendix VI: none. It is a signed consent form.
5. `prototype` to `teardown model`: eight pages of sixty-one occurrences.

Related: [[review_routine_2026-08-28]], [[thesis-schedule]], [[voice_and_verification_rules]]
