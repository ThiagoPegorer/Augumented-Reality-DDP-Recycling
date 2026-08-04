# DPP UI Spec — Screen 4: Disassembly Step 1 (Open the housing)

> Source of truth for disassembly **Step 1** AND the **template for steps 2–5**
> (`05_…`–`08_…` specify only their per-step content against this template).
> Reads with `00_design_standards.md`. Defines the two-canvas step pattern.

> **Revision 2026-07-10 (v3, approved & Editor-tested).** Approved mocks:
> `drafts/04_step1_v3.svg` + `drafts/04_cancel_modal.svg`. Changes vs v2:
> 1. **Header cleaned** — eyebrow (`DISASSEMBLY · MS 50.4`) and `Step n of 5`
>    REMOVED; the progress rail's `n/5` label carries that info. Home button stays.
> 2. **Task rows unboxed** — the card boxes read as buttons. Each action is now a
>    plain row whose icon circle is a **clickable STATUS BUTTON** (task-completion
>    gating, §4a).
> 3. **Confirm gating** — `Confirm & next` starts LOCKED and unlocks only when
>    both tasks are marked done.
> 4. **How-to went live and frameless** — the dark container box, `HOW TO · this
>    step` eyebrow, `static/loop` badge and caption are all REMOVED. The current
>    step's motion plays on the real `VCU_assembly` in a RenderTexture floating
>    directly on the navy panel (§5), with **step-focus ghosting** (§5.2).
> 5. **Back = abort** — Back opens a **cancel modal** from any step (§7a);
>    per-step back-navigation was removed deliberately (the physical teardown is
>    one-way and task state resets per step anyway).
> 6. Exploded-view canvas: **unchanged in v3** (still the static render, §6) —
>    its live-3D/orbit/zoom upgrade is the next work block.

---

## 1. Purpose & context

Step 1 of the guided disassembly of the VCU model: **open the housing**.
Pure access — nothing recovered yet.

- **Reached from:** Disassembly intro (03) → `Start disassembly`. On entry the layout becomes the two-canvas pair.
- **Exit:** `Confirm & next` (unlocked) → Step 2. `‹ Back` → cancel modal → main page (Yes) or stay (No).
- **User:** non-expert worker, gloved, PICO passthrough AR.

---

## 2. The two-canvas pattern (applies to all steps)

| Canvas | Size | Role |
|---|---|---|
| **Instruction canvas** | 640 × 430 | Read + Watch + Confirm: progress, title, task rows, live how-to, nav. The `StepFlow` screen on the main DPPPanelCanvas. |
| **Exploded-view canvas** | 268 × 430 | Explore: the teardown visual. Separate world-space canvas (`ExplodedCanvas`), own grabber bar, ~0.514 m right of the main panel, independently movable. |

Content-linked, never position-linked. The exploded canvas is active **only while the
step flow is active**.

> **Router rule (v3, load-bearing):** `ScreenRouter.Show()` deactivates all
> non-target screens FIRST, then activates the target. Outgoing screens release
> the shared preview camera / reset the model in `OnDisable`; activating the new
> screen first breaks that handover (the intro's animation died after Back).

---

## 3. Instruction canvas layout (640 × 430, panel-local coords = SVG − 20)

| Element | x | y | w | h | Notes |
|---|---|---|---|---|---|
| Progress track | 22 | 26 | 8 | 378 | radius 4, `scroll/track`; vertical, top→bottom |
| Progress fill | 22 | 26 | 8 | 378·(n/5) | radius 4, `teal/light`; grows per step |
| Progress label | 26 (center) | 416 | — | — | `n/5`, 11 px `teal/muted` |
| Home button | cx 70 | cy 46 | r 18 | — | `card/blue` + outline, white house → Main Page |
| Title | 52 | 88 | 320 | 32 | step title, 25 px bold white |
| Task row 1 | 52 | 142 | 320 | 56 | unboxed (§4) |
| Task row 2 | 52 | 210 | 320 | 56 | unboxed (§4) |
| Task hint | 52 | 278 | 460 | 16 | `Tap the icon when a task is done — both green unlock the next step`, 11 px `text/tip` |
| How-to preview | center 468, 205 | — | 242 | 225 | frameless RawImage (§5) — same slot as the intro preview |
| Back button | 52 | 350 | 150 | 52 | radius 13, secondary — opens cancel modal (§7a) |
| Confirm & next | 214 | 350 | 398 | 52 | radius 13 — LOCKED grey ↔ unlocked teal (§4a) |
| Cancel modal | overlay | — | — | — | §7a; last sibling, draws on top |
| Grabber bar | centered | below panel | 200 | 22 | per `00` §5 |

---

## 4. Task rows (unboxed, v3)

**Anatomy:** status button (36 px circle, row-local center 18, 18) + title 14.5 bold
white at x 50 + subtitle 12 at x 50/y 22. No fill, no stroke — plain on the panel.

**Gold value accent** (high-value recovery actions, steps 2 & 4) lives on the
**subtitle text only** (`#f0c879`) — the icon circle is a status control now.

### 4a. Task-completion gating

| State | Circle | Content |
|---|---|---|
| Pending | fill `#e24b4a` (red) | white **✗** (two 20 × 3 capsule bars, ±45°) |
| Done | fill `teal/accent` `#1d9e75` | white **✓** (two capsule bars, ±45°) |

**v3.2 (Thiago, 2026-08-01) — the circle is now a pure binary status light.**
Pending used to show the *action's own* glyph (cross / up / pins / usb / lever /
board / magnify / chip / recycle / label). A glyph on red asks "what is this icon
telling me?"; an ✗ on red says "not done yet" and nothing else. Both marks are drawn
from the same 3 px capsule so the pair matches in stroke weight rather than reading
as glyph-vs-mark. The task's identity is carried by the title + subtitle beside it.

**Consequences:** `iconKeys`/`iconSprites`, `LookupIcon()` (view) and
`WireIconLookup()` (builder) are **deleted**; `StepAction.icon` stays in the payload
and the schema but **has no reader** — kept because removing it would churn the
backend model for nothing. The ten action-glyph sprites are still generated by
`DPPSpriteFactory`, now unreferenced by the step flow.

- Tapping toggles pending ↔ done (toggle-back allowed — glove mis-taps happen).
- **Both done → `Confirm & next` unlocks:** fill `teal/accent`, white bold label,
  white chevron, hover on. Locked: fill `#1a2740`, text/chevron `#5d7396`,
  `interactable = false`, hover off.
- Task state **resets on every step entry** (also when re-entering the flow).
- `Confirm()` also guards in code (no-op unless both done).

## 5. How-to preview — live, frameless (v3)

The current step's motion plays on the real model, on repeat.

### 5.1 Implementation

- **`StepHowToLoop.cs`** (on the `HowToPreview` GO): loop = `ResetInstant` →
  `ApplyStepInstant(1 … n−1)` (parts from earlier steps are already out) → beat
  0.4 s → `RunStep(n)` → hold 1.0 s → repeat. `SetStep(n)` is called by
  `StepFlowController` on every refresh.
- Films `VCU_assembly` via the **shared `TeardownPreviewCamera`** (same one the
  intro loop uses — never active simultaneously, see §2 router rule) into a
  runtime RenderTexture (2× slot, ARGB32, MSAA 4, transparent bg) on a RawImage.
- Framing identical to the intro loop (trimetric from the auto-detected connector
  axis; yaw 0, frameFactor 1.5, frameHeightBias 0.06, FOV 30).
- No container, no eyebrow, no badge, no caption (all removed in v3).

### 5.2 Step-focus ghosting (part highlighting — spec 04 §11.3 realized)

While a step loops, parts NOT relevant to it swap to transparent **ghost
materials** (`fadedAlpha` default **0.1**, slider on `DisassemblyAnimator`).
Relevant (solid) sets:

| Step | Solid parts |
|---|---|
| 1 | lid + 4 lid screws |
| 2 | 3 connectors + 6 connector screws |
| 3 | PCB + 4 board screws + 6 chips (they ride the board) |
| 4 | 6 chips |
| 5 | **bottom shell only** (lid is already off & sorted) |

Ghost materials are created at runtime (URP Lit transparent, fallback built-in
Standard Fade; `_BaseColor`/`_BaseMap` copied) and cached per source material —
`fadedAlpha` changes need a Play restart. `ClearFocus()` restores originals on
leaving the flow. ⚠️ **Device builds:** the ghost shader must be in *Always
Included Shaders* or ghosts render magenta (check at the PICO test). The
AR-space model ghosts too during the flow — accepted as a feature (it highlights
what to touch on the real view).

---

## 6. Exploded-view canvas (268 × 430) — still v1 static (next work block)

Unchanged from v2: fill `navy/canvas-3d` `#081830`, `EXPLORE · 3D model` eyebrow,
`static preview` badge, static teardown render, notes, own grabber bar. Planned
upgrade: live 3D + orbit/zoom + Explode/Reassemble buttons; badge → `interactive`.

---

## 7. Interaction states (v3)

| Element | Resting | Hover | Action |
|---|---|---|---|
| Home | `card/blue`+outline | white outline | → Main Page (hides exploded canvas) |
| Status button | red / green fill | white outline | toggles task done state |
| Confirm & next (locked) | grey `#1a2740` | — (hover off) | no-op |
| Confirm & next (unlocked) | `teal/accent` | white outline | next step; step 5 → completion summary |
| Back | secondary | white outline | opens cancel modal |
| Both canvases | — | — | move via their grabber bars |

### 7a. Cancel modal (v3)

Back (any step) → modal over the instruction canvas:

| Element | Value |
|---|---|
| Dim overlay | full-panel, black 55 %, **raycast target** (blocks the UI behind) |
| Card | 400 × 170, radius 20, fill `#0d2a57`, stroke `#2e5aa0` |
| Title | `Want to cancel this disassembly?`, 17 px bold white, centered |
| Yes button | 150 × 46, **green** `teal/accent` → hide modal + `ShowMainPage()` (abandons the run; next entry restarts at step 1 with a fresh stopwatch) |
| No button | 150 × 46, **red** `#e24b4a` → hide modal, keep working |

Modal is hidden on flow entry. The how-to loop keeps playing dimmed behind it.
(Convention note: green-for-abort inverts the usual destructive-action-red; kept
deliberately — green = "proceed" is this app's consistent language.)

---

## 8. Data bindings (backend v0.4/v0.5 — unchanged)

| UI element | Source | Fallback |
|---|---|---|
| Progress | `disassembly.steps[]` length + current index | `n/5` |
| Title | `steps[i].title` | demo |
| Task rows | `steps[i].actions[]` (`title`, `subtitle`, `value`) | demo 2 |
| Subtitle accent | `actions[].value` → gold text | `text/secondary` |
| How-to loop | step index → `StepHowToLoop.SetStep(n)` | wiring missing → RawImage stays hidden |

⚠ **`actions[].icon` is no longer read** (v3.2, §4a): the keyword→sprite mapping
`cross, up, pins, usb, lever, board, magnify, chip, recycle, label` is gone from the
view and the builder. The field survives in the payload and schema, unused.

Task done-state is **pure UI state** — nothing persists to the backend (the
completion report at 09 §7 is unchanged).

---

## 9. Per-step reward (kept)

On `Confirm & next`: progress bar fills one increment (animated 0.25 s). The
richer reward (part greys out on the exploded canvas) lands with the live
exploded canvas.

---

## 10. Behaviour notes

- Entering the flow always starts at step 1; task states and the cancel modal reset on entry.
- Leaving via Home or modal-Yes hides the exploded canvas (router).
- Steps 2–5 reuse this template — only content changes (see `05_…`–`08_…`).
- No safety banner (no hazards on this product).

---

## 11. Future implementation — remaining items

1. **Interactive exploded canvas:** live model render + pinch-drag rotate +
   zoom + Explode/Reassemble buttons; badge → `interactive`. (NEXT)
2. **Per-step reward, full version:** completed part greys on the exploded canvas.
3. Per-task GIF capture for the thesis (Unity Recorder + `PlayTask`).

Realized since v2: how-to animation (§5, live loop), part highlighting (§5.2,
step-focus ghosting), state-aware previews (`ApplyStepInstant`).

---

## 12. Approved SVG mockups (v3)

> `drafts/04_step1_v3.svg` (instruction canvas — shows done + pending + locked
> states at once) and `drafts/04_cancel_modal.svg` (cancel modal).
> v2 drafts (`04_v2_step1_instruction.svg`, `04_v2_exploded_canvas_static.svg`)
> remain valid ONLY for the exploded canvas.
> The device drawings in drafts are vector stand-ins — Unity renders the real
> `VCU_assembly` RenderTexture.

---

> **Revision 2026-08-01 (v3.2).** Status circle → **binary ✗ / ✓** (§4a): the pending
> state shows a universal red ✗ instead of the action's own glyph. Per-action glyph
> lookup deleted from view + builder; `actions[].icon` retained in the payload but
> unread (§8). Applies to **all five steps** — one template, one change.

*Last updated: 2026-08-01 · Status: v3.2 coded, awaiting device check · Prev: 03 intro · Next: 05 Step 2 (content only) · Next work block: step text-consistency pass*
