# RB2.1 — Routine (user journey) · transcription of `Routine_RB2_1.pdf`

> **Living spec — parent document for RB2.1.** A faithful reading of the routine diagram plus
> the decisions Thiago has given verbally against it. Node-level design comes later, in the
> per-screen specs that will sit beside this file.
> Source: `Routine_RB2_1.pdf` (rev 2, 2026-08-04). Baseline: `../RB2_0/` (frozen).
> §6 = deltas vs RB2.0 · §7 = the timing problem that has no answer yet · §8 = open reads.

---

## 1. Node vocabulary (the legend)

| Shape | Colour | Meaning |
|---|---|---|
| Ellipse | red | **Action** — the app starts or stops doing something |
| Rectangle | blue | **Checkpoint** — a screen the user lands on |
| Diamond | green | **Decision tree** — evaluated by the app, no user input |
| Rounded rect | orange | **Feature** — content or capability hanging off a checkpoint |
| Ellipse | gold | **User question (Yes/No)** — a button interaction the user answers |
| Rounded rect | purple | **Tutorial** — new in RB2.1, see §5 |
| Dashed box | — | **Phase routine** — a named group of the above |

**Three routines, one of them drawn twice:** OPEN APP · **DPP (× 2, one per stakeholder)** ·
DISASSEMBLY.

## 2. The premise — one app, two stakeholders

Thiago, 2026-08-04: *"My application can be used for two different stakeholders: the ones that
just want to read the DPP and the ones that are recyclers."*

| | **DPP USER** | **RECYCLER** |
|---|---|---|
| Reaches | DPP Canva only | DPP Canva, then **mandatorily** the disassembly routine |
| Sees | all mandatory + good-practice DPP data, information and features | the same |
| Leaves by | `QUIT BUTTON` → `"Welcome to ReBuilt"` home screen | `QUIT BUTTON` → home · or completes the teardown |
| Can disassemble | **no** | yes |

The fork is a genuine product decision, not a routing convenience: the passport is the whole
product for one audience and the entry hall for the other. The recycler **cannot skip the
passport** — reading precedes dismantling by design.

## 3. OPEN APP routine

```
LAUNCH APP ─▶ OPEN APP ──CONTINUE BUTTON──▶ SCAN QR CODE ─▶ ◆ QR SCAN SUCCESSFUL?
                 │                               ▲                 │           │
      CLOSE APP BUTTON                           │                NO          YES
                 ▼                          YES  │                 ▼           ▼
             CLOSE APP          ⬬ CONTINUE APP? ◀─┴───────── ERROR MESSAGE  STAKEHOLDER
                 ▲                     │                                     DIRECTION
                 └────────────────────NO                                         │
                                                                    ┌────────────┴────────────┐
                                                                DPP USER                 RECYCLER
```

- **OPEN APP** carries the feature **`"Welcome to ReBuit"`** — ⚠ typo in the source; the product
  is **ReBuilt**. Correct on the diagram.
- **OPEN APP** carries a Tutorial node.
- The scan-failure loop is real: `ERROR MESSAGE → CONTINUE APP?` → **Yes** back to `SCAN QR CODE`,
  **No** closes the app. Retries are unlimited.
- `QR SCAN SUCCESSFUL?` is green — the app decides silently. `CONTINUE APP?` and
  `STAKEHOLDER DIRECTION` are gold — the user answers with buttons.

## 4. DPP routine — drawn twice, one per stakeholder

Both branches open a **DPP CANVA** checkpoint carrying the same three features, each with its
own Tutorial:

| Feature (as written on the diagram) | Maps to RB2.0 |
|---|---|
| `PRODUCT INFO (mandatory DPP info - check the legislation)` | DPP Canva tiles (`13`, `13b`–`13e`) |
| `Life Cycle Assessment (information coming from OpenLCA)` | Composition & impact (`14`) |
| `Digital Model exploration panel` | Exploded action zone (`10`) |

| Edge | DPP USER | RECYCLER |
|---|---|---|
| `QUIT BUTTON` → home | ✔ | ✔ |
| `CONTINUE TO DISASSEMBLY BUTTON` | **absent** | ✔ → disassembly intro |
| `BACK BUTTON` received from the intro | — | ✔ |

## 5. The tutorial layer — eight nodes

| # | Attached to | Routine |
|---|---|---|
| 1 | OPEN APP | Open App |
| 2–4 | DPP USER · Product info · LCA · Digital model panel | DPP (user) |
| 5–7 | RECYCLER · LCA · Product info · Digital model panel | DPP (recycler) |
| 8 | OPEN DISASSEMBLY INTRO PAGE | Disassembly |

**Shape, per Thiago:** *"in every page, create a pop up that will give a tutorial for each
page."* A **per-page pop-up**, met in context — not one linear onboarding flow.

⚠ **Supersedes `../VERSIONS.md` RBv2.0 block 4**, which describes a single gated sequence
(*pinch a button → pinch-drag a canvas onto a marked AR location → `FINISH TUTORIAL?`*). That
design front-loads one lesson before the product; this one distributes eight across it. The
registry must be corrected so both do not read as planned.

Trigger, dismissal, repeat behaviour, per-page content and whether they share one component are
**deliberately unspecified here** — they come with the per-node work.

## 6. DISASSEMBLY routine (expanded in rev 2)

```
        ┌──────────────────────────────────────────┐
        ▼                                          │
OPEN DISASSEMBLY INTRO PAGE (+ Tutorial)           │
        │            ▲                             │
        ▼            └── BACK BUTTON               │
DISASSEMBLY STEPS                                  │
        ▼                                          │
     STEP 1 ─────QUIT DISASSEMBLY BUTTON───────────┘
        │  ▲
        ▼  └─ BACK BUTTON
     STEP 2
        │  ▲
        ▼  └─ BACK BUTTON
     STEP 3
        │  ▲
        ▼  └─ BACK BUTTON
     STEP 4
        │  ▲
        ▼  └─ BACK BUTTON
     STEP 5
        │  FINISH DISASSEMBLY BUTTON
        ▼
     SUMMARY
        ▼
     SEND REPORT
        ▼
⬬ END OF PROCESS, WANT TO SCAN A NEW QR CODE?
```

### 6.1 Back behaviour — Thiago's ruling, 2026-08-04

> *"the cancel modal I will add it ONLY in the first step, cancelling the disassembly, after the
> 1st step, the other steps will have a back button that will allow the user come back to the
> previous step and the steps MUST BE SAVED, so if I come back, the green status must be green."*

| Step | Back button does | Label on the diagram |
|---|---|---|
| **1** | opens the **cancel modal** → confirm → disassembly intro | `QUIT DISASSEMBLY BUTTON` |
| **2–5** | returns to step **n−1**, no modal | `BACK BUTTON` (× 4) |

**Task state persists per step.** Returning to a completed step shows its circles **still
green**; the participant does not re-tick work already done.

### 6.2 What this reverses in RB2.0

Two deliberate RB2.0 decisions are being undone, both recorded with reasons in
`../RB2_0/04_disassembly_step1.md`:

| RB2.0 | Reason it was built that way | RB2.1 |
|---|---|---|
| Back = **abort from any step** (cancel modal everywhere); per-step back-navigation removed | *"the physical teardown is one-way"* — you cannot un-remove a screw, so a back edge described a fiction | Back = previous step, modal only on step 1 |
| Task state **resets on every step entry** (`_done[0] = _done[1] = false` in `Refresh()`) | there was no way to return to a step, so nothing needed saving | State saved per step, restored on return |

The one-way argument survives physically but loses to a stronger one: **participants re-read.**
A step they have finished is still a reference they may want to look at, and forcing them to
re-tick two circles to get back to where they were is a usability tax on a correct instinct.
That is the trade Thiago made.

### 6.3 `SUMMARY` — resolved

Rev 2 adds a **`SUMMARY` checkpoint between STEP 5 and SEND REPORT**, closing the open question
from rev 1. It is RB2.0's completion summary (`../RB2_0/09_completion_summary.md`) with its
timing table, unchanged in position. `STEP 5 → SUMMARY` is labelled **`FINISH DISASSEMBLY
BUTTON`** (RB2.0's button currently reads `Finish & see summary` — align the wording).

## 7. ⚠ The timing problem — needs a decision before any code

Saving step state collides with how the stopwatch works, and the collision lands in the thesis
data, not just the UI.

RB2.0 records timing in `StepFlowController` as an **append-only list**: every `Confirm()`
appends one split (`_stepSplits.Add(...)`), and the array is handed to the summary and the
report as `step_times_s`. It is append-only because, before RB2.1, **a step could only ever be
confirmed once**.

With back-navigation, a participant who goes 3 → 2 → 3 confirms step 3 twice. Consequences:

- `step_times_s` gets **six or more entries for five steps**
- the summary's per-step table and its gold **"longest step"** tag read the wrong rows
- the sent report — the study's primary timing artefact — is silently wrong

### 7.1 The rule — **ACCUMULATE ALL VISITS** (Thiago, 2026-08-04)

> **`step_times_s[n]` = the total seconds spent on step n, summed across every visit to it.**

A participant who spends 40 s on step 3, goes back to step 2, then returns for 15 s more has
`step_times_s[2] = 55`.

**Why this one and not first-wins or last-wins:** it is the only rule under which the five
per-step times still **sum exactly to the total elapsed time**. The completion summary shows
both on the same screen; under first-wins or last-wins the table would visibly fail to
reconcile with its own total, and that is the kind of arithmetic a supervisor checks. It is also
the honest reading of the phrase "time spent on step 3".

### 7.2 Implementation consequences

| RB2.0 | RB2.1 |
|---|---|
| `_stepSplits` — append-only `List<int>`, one `Add` per `Confirm()` | **fixed-length `int[TotalSteps]`**, indexed by step, `+=` on every departure from a step |
| clock restarts only on `Confirm()` | clock banked on **every** departure — Confirm, Back, and the step-1 quit — then restarted on arrival |
| `_done` is one `bool[2]` cleared in `Refresh()` | **`bool[TotalSteps][2]`**, written on toggle, restored on arrival, cleared only in `OnEnable()` |

⚠ **The invariant to assert:** `Σ step_times_s == total elapsed`. Any drift means a departure
path forgot to bank its time — the failure is silent otherwise, which is exactly how RB2.0's
`operating_hours` went stale for two weeks.

### 7.3 Free metric worth capturing

Accumulating means the code already knows how many times each step was entered. **Record it**
(`step_visits[n]`, or just a total back-press count). "Participants went back to a step N times"
is a usability finding about the instructions, obtainable at zero extra cost, and it is the
direct evidence for whether the back button that motivated this whole change was needed. Not
required for the report to work — flagged so the chance is not thrown away.

## 8. Still unresolved on the drawing

1. **`END OF PROCESS … ?` → NO** joins the long left-hand return line; cannot tell whether it
   terminates at **CLOSE APP** or **OPEN APP**. Semantically "no, I don't want another unit"
   reads as CLOSE APP.
2. **`QUIT BUTTON` target** on both DPP branches — Thiago says *"back to the home screen
   'Welcome to ReBuilt'"* (= **OPEN APP**), which the drawing supports; recorded as settled but
   the shared line makes it worth a glance.
3. **`YES`** on the same question appears to return to **OPEN APP** rather than straight to
   `SCAN QR CODE`, so the user re-passes the welcome screen. Deliberate or artefact?
4. **`DISASSEMBLY STEPS → BACK BUTTON → intro`** is drawn *in addition to* step 1's
   `QUIT DISASSEMBLY BUTTON` to the same place. Redundant depiction of one behaviour, or two
   distinct edges?
5. **Tile order on the two DPP Canvas** differs between branches on the drawing (left starts
   PRODUCT INFO, right starts LCA). Drafting order, or a spec?
6. ~~Does the DPP USER branch ship in RB2.1?~~ **Resolved 2026-08-04: yes, built in full.**
   ⚠ **Accepted risk to carry into Limitations:** the study's participants are all recyclers, so
   this branch reaches the thesis **without a single participant run on it**. Mitigation, at
   minimum: Thiago walks the DPP USER path end-to-end on the device himself before the next
   session, and the thesis states plainly that the stakeholder split was implemented but only
   the recycler branch was evaluated. Claiming it was "tested" because it was built is the
   failure mode to avoid.

---

## 9. Change log

- **2026-08-04 (rev 1)** — first transcription. Flagged: stakeholder fork absent from RB2.0,
  eight tutorial nodes, missing completion summary, Back-vs-cancel-modal contradiction.
- **2026-08-04 (rev 2)** — diagram expanded: five STEP nodes, `SUMMARY` restored,
  `FINISH DISASSEMBLY BUTTON`, `QUIT DISASSEMBLY BUTTON` on step 1, four inter-step
  `BACK BUTTON` edges. Thiago ruled on back behaviour and state persistence (§6.1). Summary
  question closed (§6.3). New blocker raised: the timing model (§7).
- **2026-08-04 (rev 2, decisions)** — timing rule settled: **accumulate all visits**, with the
  `Σ splits == total` invariant (§7.1–7.2). DPP USER branch **ships in RB2.1 in full**, with the
  untested-branch risk logged for Limitations (§8.6).
- **2026-08-04 (build plan)** — build order set to **journey order** (§10), overriding the
  risk-first proposal; per-canvas spec-before-build process recorded (§11); P03 feedback folded
  into the canvases rather than tracked separately, with the recording requirement flagged.

## 10. Build order — **journey order** (Thiago, 2026-08-04)

> *"I want to implement the features in logic order, so first the Open app routine, then the DPP
> routine and then the Disassembly Routine. In this way we can test in a line logical path the
> changes."*

| # | Block | Screens | Specs in RB2_1 |
|---|---|---|---|
| **1** | **OPEN APP routine** | Open App / Welcome · Scan QR · scan-fail loop · **Stakeholder direction (new)** · **first Tutorial pop-up** | `12` rev · `11` rev · **`15` new** · **`16` new** |
| **2** | **DPP routine** (both branches) | DPP Canva × 2 · 6 tutorial pop-ups · quit + continue routing | `13`, `13b`–`13e`, `14` rev · `15` |
| **3** | **DISASSEMBLY routine** | intro (+ tutorial) · steps 1–5 · summary · report | `03`, `04`–`08`, `09` rev |

Rationale accepted: each routine is a **complete, linearly testable path**, so a device session
exercises exactly what was just built instead of jumping between unrelated screens.

⚠ **The cost of this order, stated once so it is on the record:** the timing model and step-state
persistence (§7) — the only change in RB2.1 that touches a **variable the thesis reports** — now
lands **last**. If the version runs out of runway, the unfinished block is the one holding the
study's primary data artefact. Mitigation: treat block 3 as non-droppable, and if schedule
pressure appears, cut tutorial content (block 2's pop-ups) rather than anything in block 3.

⚠ **Block 1 is larger than it looks.** The Open App routine contains the *first* tutorial node,
which forces the **shared pop-up component** to be designed then and there (`16`). Blocks 2–3
reuse it and only author content.

## 11. Working process per canvas

Thiago, 2026-08-04: *"while we will building the canva, we will follow that process of create the
.md for each canva (if it doesn't exist) or change the .md that exists for each canva to fit to
the new version."*

For every screen touched, in order:

1. **Spec first** — create the RB2_1 file, or copy the RB2_0 one into RB2_1 and revise it. Per
   `INDEX.md` §1 the RB2_1 copy is **self-contained** and opens with a `Changed from RB2_0` block.
2. **Flip its row in `INDEX.md` §2** from `inherited` to `revised`, and log the change in §3.
3. **Then build**, then device-check.

### Domenik (P03) feedback — how it is being handled

Thiago, 2026-08-04: *"The feedback of Domenik will be implemented already in this new version…
Some of his feedbacks are UI feedbacks."* So P03's findings are **not** a separate work item;
they are folded into whichever canvas they belong to as that canvas is rebuilt.

⚠ **This only works if each item is written down when its canvas is specified.** A feedback item
that is fixed but never recorded cannot be cited in the thesis as evidence that testing changed
the design — which is the whole argument for having run the session. **`INDEX.md` §3 is the
place**: one row per item, naming P03 as origin and the spec that answers it. Nothing has been
entered yet.

*Created 2026-08-04 · Source: `Routine_RB2_1.pdf` rev 2 · Baseline: `../RB2_0/` · Registry: `../VERSIONS.md`*
