---
name: study-design-verified
description: "[P] THE verified user-study method, from AR_DPP/Docs read 2026-08-22. Section 3.5 is written and closed on this material. The condition order was corrected THREE TIMES in one session; the notebook table below is authoritative. The study is TWO BLOCKS: P02-P05 on RBv1.0 with manual v1, and five sessions on 2026-08-23 on RBv2.1.1 with manual v2. Carries the order imbalance, the instrument asymmetry, the three appendices and the C7 warning."
type: project
---

# 🔴 3.5 WAS NEVER BLOCKED, and the block cost five days

Memory carried 3.5 as blocked since 2026-08-17 on "the manual-condition timings, the study design, the
interface feedback and the participant backgrounds". **Three of those four are RESULTS.** The design
had been on disk since 2026-07-21. **Check what a block actually is before recording it.**

**Sources, all in `AR_DPP/Docs/`:** `STUDY_questionnaire_v1.md` (the whole design) ·
`build_study_form.gs` (the script that BUILT the Google Form, better than the live page) ·
`AR_DPP_Participant_Consent_Form.docx` · `VCU_2D_manual_v1.pdf` (condition A as used in block 1) ·
`TLCB_PICO4_Offsite_Request.docx`. Responses live in Google Forms, not the repository.
⚠ The `/edit` Google Forms URL cannot be opened without his login. Use the `.gs`.

# 🔴🔴 THE ORDER. Corrected three times on 2026-08-22. This table is the notebook record.

| Participant | Order actually run | Registered design said | Match |
|---|---|---|---|
| P01 | **AR only. No manual condition at all** | 2D first | n/a |
| P02 | 2D, then AR | AR first | ✗ inverted |
| P03 | AR, then 2D | 2D first | ✗ inverted |
| P04 | 2D, then AR | AR first | ✗ inverted |
| P05 | 2D, then AR | 2D first | ✓ |

**Three of four met the 2D manual first, so the second-pass advantage falls on the AR condition for
three of four.** That direction works against the thesis and hits the comprehension claim hardest,
because the comparison items ask about identification, confidence, mistakes, mental effort and
readiness, all of which a repeat pass inflates.

⚠ **The registered design specified alternation and it was not carried out.** Report as a deviation.
🔴 **NOT covered by his version ruling, and it currently appears in neither 3.5 nor the Discussion
draft. It has to land somewhere.**

**P01 is a pilot on two independent grounds:** AR condition only, so it can enter no comparison; and
it was run with a member of the supervisory team.
🔴 **Never write in the thesis that P01 was a supervisor.** The consent form promises identification
by code only and P01 appears elsewhere as a code. Write "a pilot session with a member of the
supervisory team", carry no code for it, and give the AR-only reason, which does the work alone.

**Analyzed sample from block 1: four (P02 to P05).**

# 🔴 TWO BLOCKS. Thiago ruled 2026-08-22; do not re-argue.

| | Block 1, July | Block 2, 2026-08-23 |
|---|---|---|
| Participants | P02 to P05, plus the P01 pilot | five more |
| AR build | **RBv1.0** | **RBv2.1.1** |
| Manual | **v1**, 2026-07-21 | **v2**, built 2026-08-22 |
| Order | 3 of 4 manual-first | recommendation given twice, unanswered: run all five **AR first** |

**Nothing pools across blocks without saying so.** The appendix carries **manual v2** on his explicit
instruction, so the appendix document is not what block 1 read.

⚠ `08_disassembly_step_texts.md` carries a **no-mid-study-changes rule** he wrote himself. Block 2
breaks it. Recorded; the decision is his.

# ✅ MANUAL v2, built and delivered 2026-08-22

Step titles were ALREADY identical between v1 and RBv2.1.1. What changed is content:

| | v1 (block 1 read this) | v2 |
|---|---|---|
| Step 4 | **3 processors and 3 memory chips**, six items | **Four IC groups**, pop the packages out |
| Step 5 | Housing into the **metal fraction**, aluminium 363 g | **Material-neutral**: separate the bottom shell, remove the QR sticker |
| Step 2 | Connectors LIFE, SENS-A, SENS-B | Connectors **AS018-35**, reuse after contact test |
| Tables | Unit overview + components and materials | 🔴 **BOTH REMOVED on his instruction**, 2026-08-22 |

🔴 **Step 5 went material-neutral because the aluminium wording confused people:** the object in hand
is visibly PETG. `08_disassembly_step_texts.md` ruling 3, dated 2026-08-10, **after block 1 ran**. So
block 1's manual still contained the defect the AR build later removed. ⚠ The file says "participants"
without saying which. **Do not assert it. Ask him.**

**Why the tables went, in his words:** *"Those tables are not precise and they dont represent what is
neither the Bosch and neither the 3D printed prototype."* Correct, and it removed both flagged numbers
at once: the 660 g (the Bosch data sheet CEILING, printed next to the printed artifact's dimensions)
and the fasteners "Steel 15 g" against the BOM's 12.0 g.

**What v2 keeps that the tables used to carry:** the tool and screw layout, and the sentence naming
the gold-plated pins and silicon as the highest-value fractions.

# 🔴 C7 MUST BE REPORTED BY BLOCK, NEVER POOLED

Comparison item C7 asks which version taught more about **what the components are made of AND their
value**. Block 1 read a manual with a full composition table; block 2 will not.
- **Value and recovery half: comparable**, because the high-value sentence survives in both.
- **Composition half: not comparable.**

# ✅ THE DESIGN

- **Within-subjects**, both conditions per participant (except the P01 pilot).
- Condition A = conventional 2D manual. Condition B = AR-DPP. The questionnaire header names
  **RBv1.0** itself, independently confirming [[study_build_version_finding]].
- **45 to 60 minutes**, one session per participant.
- ⚠ **PICO 4** on the signed consent form; project memory says **PICO 4 Ultra**. A signed form cannot
  be retro-edited. Decide before Chapter 4 names a model.
- **No tutorial before the AR condition**, deliberately, to observe how quickly an untrained person can
  operate it. AR usability answers therefore describe **first contact**.
- **Eligibility open.** *"no restrict background was necessary to be elegible."* ⚠ Open eligibility is
  not no background data: P3, P4 and P5 collect it.

# ✅ THE TASK, identical in both conditions

Five steps on the printed teardown artifact, **Allen key hex 2.5 mm**, about 5 minutes, **14 screws**
(4 lid, 6 connector, 4 board).

# ✅ MEASURES. 🔴 He ruled: comprehension PRIMARY, time SECONDARY.

| Measure | Instrument | When | Role |
|---|---|---|---|
| Direct comparison, 10 items (C1-C10), 1 = manual … 5 = AR, 3 = no difference | Questionnaire p.4 | After both | **Primary** |
| Open questions O1-O7 (O1, O2 required) | Questionnaire p.5 | After both | **Primary** |
| Closing interview | **Written notes. No audio recorded** | End of session | **Primary** |
| Usability scale, 10 items, scored 0-100 | Questionnaire p.2 and p.3 | After EACH condition | Secondary |
| Per-step time, AR | The application's own report `.json` | Automatic | Context |
| Per-step time, manual | **Experimenter stopwatch, one lap per step** | During | Context |
| Errors | Experimenter, written in the notebook | During both | Context |
| Age band · field or profession · prior headset 1-5 · prior disassembly 1-5 | Questionnaire p.1 | Before | Sample |

**His framing:** the study is not about which method is faster. On this prototype the times look
almost the same. What matters is which medium leaves a person understanding the device rather than
merely executing the motions.

🔴 **The usability scale needs a real citation and so does its 0-100 scoring rule and benchmark. The
one gap in 3.5 with NO flag-free alternative** — it is a published instrument. Also confirm
reproduction rights before the ten items sit in Appendix VII.

**The instrument asymmetry.** AR times are machine-written; manual times are hand-lapped by an
experimenter who is not blind and built the AR system. A hand lap arrives late, never early, and the
two conditions do not share a definition of when a step ended. The comprehension-primary ruling
demotes this from a threat to a disclosure, but 3.5 still carries it.

⚠ **Open question about the form:** its pages put the manual's usability scale BEFORE the AR one,
because it was written assuming manual-first. P03 ran AR-first. Ask which page was filled when.

# ✅ ETHICS AND DATA PROTECTION

Voluntary; withdrawal any time **before anonymisation**. Codes only, no names or emails in the
research data. The signed form is the sole person-to-code link, stored separately. HMD discomfort
disclosed; the participant may pause, remove the headset or stop at any moment. Audio recording was a
separate opt-in and **none was made**. GDPR named. Supervisors on the form: **Ms. Elle Langer (XR)**
and **Prof. Dr. Saman Ghobadian (Sustainability / LCA)**. Student ID 100003505.

🔴 **A consent form does not guarantee anonymity, and he wrote that it does.** It is the one document
that links a person to a code, which is why it is stored separately, and while signed forms exist the
data is pseudonymized, not anonymous. Corrected wording supplied and accepted.

🔴 **Check the notebook.** Stopwatch laps, errors and interview notes are all on paper. If any page
carries a name beside data, that page is the person-to-code link, not the consent form.

# ✅ THE THREE APPENDICES, numbered by Thiago 2026-08-22

- **Appendix V** — the 2D disassembly manual, v2, tables removed. Delivered as `.docx`.
- **Appendix VI** — the participant consent form. Already on disk.
- **Appendix VII** — the user study questionnaire. Delivered as `.docx`, built from `build_study_form.gs`.

# ⚠ STILL OPEN

1. Whether block 2 runs AR-first (recommended twice, unanswered).
2. Which usability page P03 filled when.
3. The usability scale citation, its benchmark citation, and reproduction rights.
4. PICO 4 versus PICO 4 Ultra.
5. Where the counterbalancing deviation is written.
6. Whether the metal-stream confusion came from study participants or his own device rounds.
7. The final analyzed sample size, currently a `[FILL]` in 3.5.

Related: [[study_build_version_finding]], [[ch3_methodology_progress]], [[rbv2_1_1_ar_system_verified]],
[[thesis-schedule]], [[voice_and_verification_rules]]
