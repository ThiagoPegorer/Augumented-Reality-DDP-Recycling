# Session 42 — 2026-08-25 · CHAPTER 4 CLOSED

Sections 4.2 and 4.3 both written today. Nine participants, a reproducible data chain, and an
audit pack. Submission moved to **Monday 31 August**: the SRH examination office is shut this week.

## What was built

| file | what it is |
|---|---|
| `ARDPP_study_data.xlsx` | RAW data only, ten sheets. No derived value lives here. |
| `ARDPP_study_analysis.ipynb` | 20 cells. Derives every number, draws six figures, runs 21 audit checks. |
| `ARDPP_study_results.xlsx` | nine sheets of derived tables |
| `ARDPP_figure_audit.xlsx` | eight sheets, one per figure, each reconciled against the raw |
| `section_4_2_draft.md` | 4.2, drafted from 44 headset screenshots |
| `section_4_3_full_draft.md` | 4.3, three subsections, 1,562 words |
| `cell_figure_audit.py` | the audit cell, pasteable |
| six figures | `.png` at 300 dpi and `.svg` |

## The four things that were blocked and are now closed

1. **The comparative-scale anchors.** 1 is the two-dimensional manual, 5 is the Augmented Reality
   model. Corroborated by the Session 12 Notion page dated 2026-07-21.
2. **The condition order.** P01 AR only · P02 P03 P06 P08 AR first · P04 P05 P07 P09 manual first.
   Four and four among the eight who ran both.
3. **The background scale labels.** Endpoints only, 1 never used and 5 expert. Three middle labels
   invented in an earlier session were removed: the form carried no wording for them.
4. **The instrument naming.** Left unnamed. No citation owed, no benchmark sentence.

## Findings that were not there this morning

- **The order effect.** Five of the eight were faster in whichever condition they performed SECOND.
  The headset was the slower run for three of the four who ran it first, against two of the four who
  ran it second. A practice effect plausibly contributes to the headset looking slower.
- **Perceived against measured agility.** Five of eight took longer with the headset; three of those
  five still answered the agility item toward the headset.
- **The comparative ceiling.** 78 of 90 responses are 5, and five of nine answered 5 on all ten
  items. Those ten items separated P02 and nobody else.
- **The scoring is provably balanced.** Answering the same value to all ten questionnaire items
  scores exactly 50.0, whichever value it is. No participant straight-lined any of the eighteen rows.
- **The elapsed-versus-step-sum gap explained.** Mixed sign, maximum 2 s, consistent with five
  independent roundings. It changes no conclusion.

## Three discrepancies against the 2026-07-21 study spec, all open

1. The spec sets *"odd participant IDs start 2D-first, even AR-first."* **P03 and P04 break it.**
   If Methodology states that rule it contradicts the data.
2. The spec says **seven** open questions. The export has **six**. The one apparently dropped is the
   *deliberate pro-2D probe*.
3. The spec names the instrument as SUS. The thesis now leaves it unnamed, which is only consistent
   if Methodology and the appendix do not name it either.

## Corrections to my own work, recorded

- **A reconciliation that reruns the same code proves nothing.** The usability audit passed while a
  wrong polarity column would also have passed. Fixed by testing the inversion itself.
- **Never invent scale labels.** Blank is honest.
- **Completion time is the sum of the five step values in both conditions.** Mixing definitions
  produced a stale median difference of +15.5 in the superseded draft.

## Tomorrow

Discussion in the morning, Conclusion in the afternoon, then Abstract, Foreword and
Acknowledgements. Chapter 4 needs pasting, not writing.
