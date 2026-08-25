# 4.3 Test Participants Results — skeleton

Scaffold, not prose. Order as you set it: participants, then time, then scores, then the interview.

Figures are produced by `ARDPP_study_analysis.ipynb` from `ARDPP_study_data.xlsx`.

---

## Block A — the participants

**Establishes:** who the sample is, so every number after it can be read against a person.
**Rests on:** the `participants` sheet, nine rows, all self-reported on the background page.
**Figures:** three, one per variable, each captioned on its own —
`fig_participants_age` (pie), `fig_participants_headset_experience` (bars),
`fig_participants_disassembly_experience` (bars). The two bar figures share one axis limit, so their
bar lengths compare directly across the two figures.
**Table:** the nine participants with age group, profession, and the two experience ratings as
numbers, the endpoints named once beneath the table.

- n = 9. Age: two in 18 to 24, four in 25 to 34, three in 45 and over.
- Nine different professions. Range them in a sentence rather than charting them: electrician,
  electrical engineer, economist, marketing analyst, marketing and social media, strategic design,
  innovation manager, student, retired.
- Headset experience, 1 to 5: four at 1 (never used), two at 2, one at 3, one at 4, one at 5
  (expert).
- Disassembly experience, 1 to 5: five at 1 (never used), one at 2, none at 3, two at 4, one at 5
  (expert).
- ⚠ **This block is the only evidence in the thesis for Chapter 1's "mixed background" claim.**
  Say the range plainly and the claim is supported.
- `[FILL: which condition each participant performed first.]`
- **Hands to:** the task itself.

⚠ The background items are self-rated 1 to 5. Only the endpoints are confirmed, 1 never used and
5 expert, and the three middle points are blank in the `experience_scale` sheet because the form
carried no wording for them. The charts label the endpoints and keep the numbers. Type wording into
any middle point and every chart and table switches to words with no code change.

---

## Block B — task completion time

**Establishes:** what the two conditions cost in time, per participant.
**Rests on:** `completion_times`, per-step seconds, summed in the notebook.
**Figure:** `fig_completion_times` — one row per participant, both conditions, signed difference.
**Table:** manual, Augmented Reality, difference, plus the summary row block.

Points, in order:

1. Seventeen runs, nine with the headset and eight with the manual. P01 performed the headset
   condition only. Every run reached the fifth step.
2. The per-participant result: slower with the headset for five of eight, faster for three,
   differences from 26 s faster to 25 s slower.
3. The group figures: manual 312.5 s mean and 300.0 s median; headset 318.2 s mean and 320.0 s
   median.
4. **The point the figure actually makes:** the condition moves a time by 15 to 26 s while the
   participants differ from one another by up to 274 s. Same person, same rank, either tool.
   `[OPTIONAL: r = 0.992 across the eight. Derived, n = 8.]`
5. One sentence on the per-step pattern: the first two steps carry most of the time in both
   conditions.
6. ⚠ **Say once that the application's own elapsed value and the sum of its step values differ by up
   to 2 s in five of nine runs**, and that the sum is used throughout so both conditions are measured
   the same way. The notebook prints this.
7. Errors: none recorded in any of the seventeen runs, in either condition. State the property of the
   object beside it, no causal word joining them.
   🔴 **Your observation that the prototype was too simple for a mistake to be possible is the
   explanation, and it belongs in the Discussion.** It is the same sentence that explains Block D.
- **Hands to:** what participants thought of the two tools.

---

## Block C — perceived usability

**Establishes:** how the two conditions were rated, per participant.
**Rests on:** `usability_items`, scored in the notebook from the polarity column.
**Figure:** `fig_usability_scores` — same grammar as the time figure, so the two read together.
**Table:** the nine scores per condition with the difference.

Points, in order:

1. Higher for the application in seven of the nine.
2. Manual mean 79.4, median 85.0. Application mean 83.9, median 87.5.
3. **The mean and the median disagree, and say why in the same sentence:** the mean difference is
   +4.4 and the median +12.5, because one participant's −55.0 pulls the mean.
4. P02 holds that −55.0, scoring the manual 97.5 and the application 42.5. P02 also reports the
   highest disassembly experience and among the lowest headset experience.
5. The comparative items, answered 1 to 5 where 1 is the two-dimensional manual and 5 is the
   Augmented Reality model. All ten return a median of 5, 78 of the 90 responses are 5, five of the
   nine answered 5 on all ten, and two items are answered identically by all nine, on what the
   components are made of and on engagement. Band counts per item: seven to nine toward the model,
   at most one at the midpoint, at most two toward the manual.
6. The agility item against the measured times: five of the eight who performed both took longer
   with the application; three of those five answered the agility item toward the model and two
   toward the manual. Counts only, nothing joined.
- **Table, not chart, for the comparative items.** Nine responses do not carry a distribution.
- **Hands to:** what participants said in their own session.

---

## Block D — the interview

**Establishes:** what participants reported, in themes and in individual observations.
**Rests on:** `open_answers`, the experimenter's written record of the closing interview.
**Form:** counts where several recorded the same thing, participants named where they recorded
something of their own, **no quotation marks anywhere.**

Opens with one sentence of provenance: the responses are the experimenter's written record of the
closing interview and are reported as summary notes.

Then, in this order:

1. **Most helpful.** Four named the identification of tasks and components through the animations.
   P01 named the interface design, P09 the component-level detail, P03 the value as a way to learn
   the sequence before handling the real component, P04 the understanding of components and their
   function with no background in electronics. P02 recorded that the value lies in devices where a
   mistake can injure the operator.
2. **Most difficult.** Six named passthrough sharpness reducing accuracy on the fasteners. Three
   named the gestures as the initial obstacle. P03 also recorded early eye strain. P01 recorded none.
3. **The gestures.** Three found them immediately intuitive, three needed a demonstration first,
   P08 struggled at the first attempt. P02 recorded that it became easier once it was clear the
   interface responds to a pinch rather than a touch, and asked for a sound on button presses and on
   incorrect actions. **Add your clause: that request fed the later development of the application.**
4. **The material and recovery information.** 🔴 **Eight of nine recorded that it did not help them
   perform the task, and seven of those recorded that it gave them knowledge beyond conventional
   specifications.** P01 recorded that it did influence the task.
   🔴 **Your reading, that this follows from the prototype being too simple, is the Discussion's
   sentence, not this one.** Here: the count and P02's and P04's wordings, nothing joined.
5. **Use at a real workstation.** All nine would use it. Eight named training as the use; P02 named
   complex or dangerous devices. Seven named headset comfort over a long session as the constraint,
   P02 putting it at about fifteen minutes and locating it in the hardware rather than the
   application. Nothing further was added by anyone.
- **Hands to:** the Discussion.

---

## What this section must also carry

The application writes `co2_avoided_kg` into every session report and the value is the modelled
15.4315 kg CO2 eq from the passport record. No real unit was dismantled. One sentence, anywhere in
the section.

---

## What goes to the Discussion, not here

Three things, all of them yours and all of them interpretation:

1. **The simplicity of the prototype**, which explains both the absent errors and the eight of nine
   who found the recovery information unhelpful for the task. One cause, two findings.
2. **Training rather than throughput.** Eight of nine name training, all nine would use it, and the
   times show no consistent gain. Three measures pointing the same way.
3. **The hardware ceiling.** Passthrough sharpness at six of nine and comfort at seven of nine are a
   limit on the device, not on the design.

---

```
=== Check before using ===

Blocked until you supply it:
  - The condition order per participant, for Block A.

Confirmed 2026-08-25 and recorded in the workbook:
  - Background items: 1 never used, 5 expert, 2 to 4 unlabelled on the form.
  - Questionnaire items: 1 disagree, 5 agree, reported as the numbers 1 to 5.
  - Comparative items: 1 the two-dimensional manual, 5 the Augmented Reality model.

Verified first-hand:
  - Every count and statistic above regenerated by the notebook from the workbook on this run.

Decisions recorded, not reopened:
  - One sample of nine, no grouping.
  - The open answers are reported as your record, in counts and named observations, never in quotes.
```
