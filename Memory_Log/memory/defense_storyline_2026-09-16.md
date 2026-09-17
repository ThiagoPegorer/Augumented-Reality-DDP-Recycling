---
name: defense-storyline
description: "[P] The defense presentation spine, built 2026-09-16 (Session 48). Parameters, the one-line argument, the slide-by-slide time budget for 20 minutes with a live PICO demo, the five findings to front-run, the demo fallback, and the open flags to close before the defense."
type: project
---

# PARAMETERS

| item | value |
|---|---|
| Date | **NOT CONFIRMED.** Likely 28 September. Supervisors proposed 25 or 28 September |
| Presentation | **20 minutes, hard** |
| Questions | 10 minutes, both supervisors |
| Grading | 30 minutes, supervisors alone, candidate out of the room |
| Prototype | **LIVE PICO demo**, his decision 2026-09-16 |
| Examiners | Saman Ghobadian (first supervisor), Elle Langer (professorial supervisor) |
| Build shown | RBv2.1.1 |

Deck is built against the earlier of the two proposed dates, so a 25 September confirmation costs nothing.

# THE ONE LINE THE WHOLE TALK DEFENDS

> The passport works as an interface, and the environmental case does not sit where the audience
> expects it. Participants preferred the Augmented Reality version and would use it at a real
> workstation, while the environmental data did not help them perform the task, and dismantling does
> not reduce the unit's burden at all. What rises is the primary production it avoids.

Every slide either supports that sentence or is cut. The talk does not narrate the thesis chapter by
chapter, because the examiners have read it. It argues.

# THE FIVE FINDINGS TO FRONT-RUN

These go against the result he hoped for. The rule: **he states each one himself, on a slide, before
an examiner can raise it in the ten minutes.** A finding an examiner digs out reads as a defect. The
same finding volunteered reads as command of the data.

| # | The finding | The counterweight, in the same breath |
|---|---|---|
| 1 | **The headset was slower.** AR slower for 5 of 8 who ran both. Manual median 300.0 s, AR median 320.0 s | No participant's two times differ by more than 26 s, while the spread across participants is 239 s (manual) and 274 s (AR). Between-person variation dwarfs the condition. r = 0.992 between conditions |
| 2 | **A practice effect plausibly contributes.** 5 of 8 were faster in whichever condition they ran second | Stated as a direction, never a measurement. Four per side is not a test |
| 3 | **8 of 9 said the material and recovery information did not help the task** | 7 of those 8 said it gave knowledge beyond conventional specifications. 9 of 9 would use it at a real workstation. 8 of 9 named training as the use |
| 4 | **The comparative items hit a ceiling.** 78 of 90 responses are the top point. P02 alone answers below the midpoint, on 5 of 10 items | Owned as a limitation of a 10-item scale on 9 people, not explained away. The usability instrument separated the same participant and is provably balanced: answering the same value to all ten items scores exactly 50.0 |
| 5 | **Dismantling does not decrease the unit's environmental impact.** The gross burden is identical across all four scenarios | The avoided impact rises monotonically, Sc2 < Sc3 < Sc4, in 25 of 25 EF 3.1 rows and 18 of 18 ReCiPe midpoint rows. That ordering is the robust result of the whole LCA |

# THE SLIDE SPINE, 20 MINUTES

Total 19:40 against a 20:00 slot. The demo is the only block that can overrun, and it is capped.

| # | Slide | Time | Carries |
|---|---|---|---|
| 0 | Title | 0:20 | Title, name, both supervisors, date. Nothing spoken beyond one sentence |
| 1 | A Vehicle Control Unit at end of life | 1:10 | The physical object, where it goes today, and what is not known about it when it arrives |
| 2 | The question | 1:10 | Main question verbatim, then the four sub-questions. **The examiners grade against these four.** Say plainly that the talk answers them in order |
| 3 | How it was done | 1:20 | ONE diagram, the whole method chain: the record feeds the application, the life cycle model feeds the record, nine participants test the application against a 2D manual. The point of the slide is that the LCA is passport payload, not a second thesis |
| 4 | SQ1, what the record carries | 1:00 | The two kinds of content, component and environmental. Coverage against the CIRPASS requirement set |
| 5 | SQ2, the interface | 0:40 | Four tabs, the gate, the exploded view, the hand gestures. **Set the demo up here, then stop talking** |
| D | **LIVE DEMO** | **3:30 HARD CAP** | See the demo protocol below |
| 6 | SQ4, the environmental result | 1:30 | Gross identical across the four scenarios, credit rising. The balance chart, gross above the axis and credit below. **The word is balance, never net.** Caption states the credit sits outside the system boundary |
| 7 | SQ4, how far the result can be trusted | 1:10 | Monte Carlo band widths. The ranking is robust, the absolute values are not. Name the one overlap: Sc3 and Sc4 avoided intervals overlap in freshwater eutrophication. Volunteering this is worth more than hiding it |
| 8 | SQ3, time and errors | 1:20 | The nine completion times, both conditions. Finding 1 and finding 2, spoken. Zero errors in 17 runs |
| 9 | SQ3, usability and preference | 1:20 | Usability 0 to 100, AR higher for 7 of 9. The comparative response matrix, 10 items by 9 participants, nothing averaged. Finding 4, spoken |
| 10 | SQ3, what they said afterwards | 1:00 | Closing interview counts. Finding 3, spoken, with its three counterweights. **No quotation marks anywhere. The wording is his, not the participants'** |
| 11 | What it means | 1:30 | The interface carried the task; the record did not change how the task was performed. The use the participants themselves named is training. A passport that shows a single deterministic number to a non-specialist is a design problem, and that is a recommendation, not a finding |
| 12 | Limitations, owned | 1:10 | n = 9, one device, one session block. The printed object is **not a replica**, it is a generic device inspired by the datasheet, 200 x 150 x 60 mm against the reference unit's 166 x 121 x 41. The ceiling effect. The dismantling electricity assumption behind the largest freshwater contributor in Sc3 and Sc4 |
| 13 | Contribution and outlook | 1:10 | What exists that did not exist before: a working AR passport, a life cycle model of the unit, and a measured comparison. Then the regulation timeline, the AI layer reading the passport, and **VERA Arm in one line as a direction** |
| 14 | Closing | 0:20 | One sentence. His own words |

**Backup slides, after the closing, unnumbered and never shown unless asked:** per-step times ·
the condition order table · the usability scoring key and the worked example · the ReCiPe cross-check
on ratios against Sc2 · the Monte Carlo table · stage contributions · the payload schema ·
the three discrepancies against the study spec.

A backup slide answering a question is the strongest thing that can happen in the ten minutes.

# THE DEMO PROTOCOL

He chose a live demo. In a 20-minute slot it is the only block that can destroy the talk, so it is
run as a timed block with a hard exit.

1. **Headset already on, running, and cast to the projector BEFORE slide 0.** No boot, no pairing, no
   Wi-Fi handshake in front of the examiners.
2. **He wears it. Not an examiner.** Offer the headset to the examiners in the ten minutes, never
   inside the twenty.
3. **One scripted path, rehearsed to the second:** scan or anchor, open the passport, one tab, the
   exploded view, one disassembly step. Nothing else. No improvisation.
4. **3:30 hard cap.** A phone timer in his eyeline.
5. **The fallback is a recorded video on the next slide, already loaded.** If anything is not working
   by 40 seconds in, he says one sentence and plays the video. Rehearse saying that sentence.
6. **His own participants named passthrough sharpness as the main difficulty, 6 of 9.** Room lighting
   is a real variable. Ask about the room.

# WHAT MUST NOT BE SAID, IN THE ROOM

1. That dismantling decreases the environmental impact. The gross burden is identical.
2. That the tool increases recycling rates. Write the obstacle, never the gain. This verb has been
   caught nine times, and a tenth time in the LinkedIn caption.
3. That the printed model is a replica. It is not.
4. "Route" for the four modelled constructs. The word is **scenario**. The exception is the main
   research question, which says end-of-life routes deliberately and means real pathways.
5. Per-part hazard highlighting. It was not implemented.
6. The two-build difference, explicit anywhere.
7. Any quotation marks around a participant answer.
8. "Net". The word is balance.

# KNOWN DEFECTS IN THE SUBMITTED PDF, in case an examiner opens one

Do not raise these unprompted. Know the answer if asked.

- **4.1.6 states a wrong cause** for the ReCiPe ranking mismatch. It is the normalization references,
  not the units. Correct answer ready: the references differ by about four orders of magnitude
  between mineral scarcity and freshwater ecotoxicity, and the subsection prints both two paragraphs
  later.
- **"Prototype" names two objects**, the application and the printed model, on pages 42 to 69.
- **The Acknowledgements call Elle Langer "Ms." where the title page reads "Prof."**
- **LCA_explorer.ipynb on the USB contains 36 uses of the abolished word "net"** across 16 cells.
  Deferred deliberately, a working file, not a reported result.
- **The deterministic climate value sits near the 6th percentile of its own Monte Carlo
  distribution**, 73.4326 against a median of 91.33. This one is disclosed in the uncertainty block.
  The honest line: the reported figure is the deterministic result, the distribution is wider and
  higher, and the ranking rather than the level is what the thesis claims.

# OPEN FLAGS TO CLOSE BEFORE THE DEFENSE

1. 🔴 **The defense date is not confirmed.** No task exists for chasing it.
2. 🔴 **The four research questions were rewritten on 2026-08-13 and there is still no written
   supervisor agreement.** All four narrow rather than expand the scope. The record says the
   agreement should exist before the defense, and the defense is the room where they are graded.
3. **VERA Arm.** If no preliminary simulation exists, the outlook uses a generated video. It must be
   labelled on the slide as an illustration of an intended process, not as a result. An examiner who
   mistakes imagined work for completed work in a defense is a real risk, and the label removes it.
4. **The demo room.** Lighting, projector, casting, Wi-Fi, and whether the printed model can be on
   the table.

Related: [[research_questions_final]], [[study_results_verified]], [[lca_results_verified_ch4]],
[[recipe_cross_check_verified]], [[ch6_conclusion_progress]], [[ch5_discussion_progress]],
[[voice_and_verification_rules]], [[rebuilt_public_comms]]
