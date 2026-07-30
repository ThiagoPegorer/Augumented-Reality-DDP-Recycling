# User Study Questionnaire — v1 DRAFT (for Google Forms)

> Study: Condition A = **Conventional 2D manual** · Condition B = **AR-DPP (RBv1.0)**
> Within-subjects: every participant does BOTH conditions, **order alternated per participant**
> (odd IDs: 2D first · even IDs: AR first). Task metrics (completion time, errors) are captured
> separately by the experimenter; per-step AR times arrive automatically via the dismantling report.
> Form structure below maps 1:1 to Google Forms sections. Estimated fill time: ~8 minutes.

---

## Section 1 — Participant (before the tasks)

| # | Question | Type |
|---|---|---|
| P1 | Participant ID (given by the experimenter) | Short answer |
| P2 | Age group | Multiple choice: 18–24 · 25–34 · 35–44 · 45+ |
| P3 | Field of study / profession | Short answer |
| P4 | Prior experience with VR/AR headsets | Linear 1–5 (None → Very experienced) |
| P5 | Prior experience disassembling electronic devices | Linear 1–5 (None → Very experienced) |

## Section 2 — SUS: Conventional 2D manual *(fill immediately after Condition A)*

Standard System Usability Scale — 10 statements, 1 = Strongly disagree … 5 = Strongly agree.
"The system" = **the 2D manual you just used**.

1. I think that I would like to use this manual frequently for tasks like this.
2. I found the manual unnecessarily complex.
3. I thought the manual was easy to use.
4. I think that I would need the support of a technical person to be able to use this manual.
5. I found the various parts of the manual well integrated.
6. I thought there was too much inconsistency in this manual.
7. I would imagine that most people would learn to use this manual very quickly.
8. I found the manual very cumbersome to use.
9. I felt very confident using the manual.
10. I needed to learn a lot of things before I could get going with this manual.

## Section 3 — SUS: AR-DPP application *(fill immediately after Condition B)*

Same 10 statements, "the system" = **the AR application you just used** (word each item with
"the AR application" in place of "the manual").

> Scoring (analysis, not in the form): odd items score (answer−1), even items (5−answer);
> sum × 2.5 → 0–100 per participant per condition. Benchmark: 68 = average.

## Section 4 — Direct comparison *(after BOTH conditions)*

Instruction shown in the form: *"For each aspect, choose which version worked better for you.
1 = definitely the **conventional 2D manual** · 3 = no difference · 5 = definitely the **AR 3D
model**. A 2 or 4 means a slight preference."* (Google Forms: linear scale 1–5, left label
"Conventional 2D manual", right label "AR 3D model".)

| # | Aspect (form wording) |
|---|---|
| C1 | Which version let you work with more **agility** (faster, smoother progress)? |
| C2 | Which version made the instructions **easier to understand**? |
| C3 | Which version made it easier to **identify the correct component** at each step? |
| C4 | Which version gave you more **confidence** that you were doing the step correctly? |
| C5 | Which version helped you better **avoid mistakes** (wrong part, wrong order)? |
| C6 | Which version required less **mental effort** to follow? |
| C7 | Which version taught you more about **what the components are made of and their value** (materials, recovery)? |
| C8 | With which version would you feel more prepared to **disassemble a similar device again without help**? |
| C9 | Which version was more **engaging** to use? |
| C10 | **Overall**, which version would you choose for this kind of task? |

## Section 5 — Open questions *(after both conditions)*

| # | Question |
|---|---|
| O1 | What did you find most helpful in the AR application? |
| O2 | What was most difficult or frustrating in the AR application? |
| O3 | Was there a moment where the 2D manual worked better for you than the AR version? Describe it. |
| O4 | How intuitive were the hand gestures (rotate, zoom, moving parts)? What would you change? |
| O5 | Did the material/recovery information (what parts are worth recovering, and why) influence how you did the task? How? |
| O6 | If this tool existed at a real recycling workstation, would you want to use it? Why / why not? |
| O7 | Anything else you want to tell us? |

---

## Google Forms build notes

- **One form, six PAGES** (Forms sections = pages), in order: Participant → SUS 2D → SUS AR →
  Comparison → Open questions → **Experimenter: report upload**. Sections 2 and 3 are filled at
  different MOMENTS — the experimenter tells the participant when to advance.
- **Section 6 (new, 2026-07-21):** file-upload question where the EXPERIMENTER attaches the
  participant's dismantling report `.json` — one response = questionnaire + behavioural data,
  fully self-documenting. ⚠ Constraints: (a) a file-upload question forces Google sign-in for
  the whole form (fine on the lab laptop with Thiago's account); (b) the Forms API cannot create
  upload questions — the build script leaves a placeholder, added manually once (+ Add question
  → File upload → 1 file, 10 MB).
- Build automation: `Docs\build_study_form.gs` — paste into script.google.com, run `buildForm`
  once, links printed in the log. All SUS + comparison items required; open questions optional
  except O1/O2. No names/emails (participant ID only) — GDPR-simple.

## Iteration log
- **2026-07-21 (a)** — v1 draft: demographics + SUS×2 + 10-item comparative scale + 7 open questions.
- **2026-07-21 (b)** — v1.1 APPROVED: each section its own page; section 6 added (experimenter
  uploads the participant's dismantling-report .json). Apps Script builder generated.
