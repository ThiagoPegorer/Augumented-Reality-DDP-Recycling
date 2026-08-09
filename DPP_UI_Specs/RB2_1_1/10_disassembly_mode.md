# 10 — Guided disassembly mode of the Super Panel (RBv2.1.1)

**Status:** v1.2 · 2026-08-10 · mock `drafts/10_v1_disassembly_mode.svg` approved 2026-08-09 ·
**rounds 1–2 device feedback applied (§9) — round-3 build pending.**
Builder: `RBv2_1_1/14` · Controller: `DisassemblyModeController.cs`.

## 1. Concept

One rig, two faces. The rail-gate CTA no longer routes to the RB2_0 flat screens — it swaps the
SAME Super Panel's rail + data content in place. The model never leaves the screen.

| Zone | Passport face | Disassembly face |
|---|---|---|
| RAIL 220 | 4 tabs + gated CTA | eyebrow + 7 entries 184×44 @ 52 pitch: Intro · steps 1–5 · Summary |
| STAGE | LINKED showcase | Intro/Summary: exploded + spin. Steps: **current physical state**, animated (§3) |
| DATA 420 | tab pages | `DisIntroPage` / `DisStepPage` (one page, rebound per step) / `DisSummaryPage` |

## 2. Rulings (mock notes 1–8, amended by device round 1)

1. **One rig** — geometry, yaw, grab bar, bottom-bar standard untouched (Back cx 69/w 90 · Next cx 321/w 150 · cy 402 · 11 pt).
2. **Sequential unlock**; visited steps revisitable via Back or rail; locked = dim + refuses click.
3. **Quit**: step 1's Back reads "Quit" → confirm modal → Intro, run + timer cancelled. Intro's Back leaves the mode to the passport.
4. **Round 2 — the animation LEADS, the participant follows:**
   - **Start** → the pivot eases home while the parts **REASSEMBLE** from the exploded showcase (the model becomes the closed unit on the desk), then step 1 opens.
   - **Opening a step** plays that step's removal immediately (`RunStep` — never bespoke): the parts to work on **explode out** and stay out, highlighted, **for the whole step** (round 3: hiding them at the second tick yanked the reference away mid-work).
   - **Jumping to the next step** is what makes them vanish — `SetGuidedStepState(n+1)` hides the removals of every earlier step. Ticking never touches the model.
   - **Next** is instant — no animation at completion (round 1 animated here; it read as the app lagging behind finished work).
5. **Timer invisible**: runs from Start, splits per frontier completion; total + splits only on Summary.
6. **LINKED only during steps**: gesture column (padlock incl.) hidden in steps, back on Intro/Summary; step entry force-relocks.
7. **No drag-out**: the stage is guidance; picks inert for the whole mode (`ModelLinkController.SetGuided`).
8. **Summary is final**: rail locks backward. Send report → post-report modal (round 2): **"Quit" grey → Welcome page** (the rig is hidden explicitly — `ShowWelcome` only hides the flat canvas) · **"Close app" solid red → quits via `WelcomeController.CloseApp`** (00 §2.1 meaning 3, the one sanctioned red; pair differs in label, position and size — colour-blind mitigation).

## 3. Model state machine (stage clone's animator — additive API)

| View | Hidden | Pose / motion | Focus |
|---|---|---|---|
| Intro / Summary | nothing | full open (`ApplyOpenInstant`) + spin | none |
| Start pressed | — | pivot `SnapModelHome` + `Reassemble()` (~1.3 s) | none |
| Step n (frontier) | removals of 1..n−1 | before-state, then **entry explode** `RunStep(n)`; parts stay out until the jump to n+1 | step n parts |
| Step n (revisited) | removals of 1..**n** | after-state, no animation (step 3: board+chips risen) | **cleared** — highlighting hidden bodies would ghost the whole visible model |
| Step 4 | + PCB screws | board + chips risen (`ApplyStepInstant(3)`) | chips |

`SetGuidedStepState(n, completed)` = ResetInstant → unhide all → hide cumulative → pose →
focus/clear. Ghost alpha 0.30 on the stage clone (set by /14). Entry-explode coroutine is
controller-hosted and stopped on every navigation; `SetGuidedStepState` kills the tweens.

## 4. Deviations from the approved mock (deliberate)

- Intro's dismantling list is ONE column of 7 (labels too long for two columns at 420).
- Locked rail entries dim with no padlock glyph (consistent with the passport rail's language).
- Task rows are boxed AND the WHOLE ROW toggles (chrome = touchable; 372×64 beats a 36-unit
  circle for gloved hands). Circle named `CircleFill` so the row's HoverHighlight can never
  repaint it (trap 1 avoided structurally).

## 5. Files

| File | Change |
|---|---|
| `Scripts/DDP/UI/DisassemblyModeController.cs` | mode state machine · round 2: `StartRunRoutine` (reassemble entry), entry-explode per step, vanish-on-tick, instant Next, `OnSummaryQuit` / `OnSummaryCloseApp` (round-1 modal targets kept as fallbacks) |
| `Scripts/DDP/DisassemblyAnimator.cs` | `RemovedByStep` / `SetGuidedStepState(step, completed)` / `SetStepPartsHidden` / `ClearGuidedState` / `ApplyOpenInstant` |
| `Scripts/DDP/UI/ModelLinkController.cs` | `SetGuided` — picks refused, ghost/tint writer stands down |
| `Scripts/DDP/UI/SuperPanelView.cs` | guided chrome/spin/column API + `SnapModelHome` + CTA routing + OnEnable restore |
| `Scripts/DDP/UI/CompletionSummaryView.cs` | `public bool Sent` |
| `Editor/DPPUIBuilder.Disassembly.cs` | `RBv2_1_1/14` — rail group + 3 pages + modals; round 2: post-report modal = Quit (grey) + Close app (red) |
| `Editor/DPPUIBuilder.Verify.cs` | DisassemblyModeController map + SuperPanelView.disassembly wire |

## 6. Editor chain

- Routine chain: `09 → 10 → 11 → 12 → 13 → 14 → Tools/Verify wiring → SAVE`. /14 ALWAYS after a
  /10 re-run (Verify catches the dangling stageAnimator).
- **Round 2 minimum: run 14 → Verify → SAVE** (script changes + the rebuilt modal; nothing
  before 14 changed).
- Menu 05/06/07 untouched — rollback route (clear `SuperPanelView.disassembly`).

## 7. Device checklist — round 2

1. Intro → Start: model eases to home yaw WHILE reassembling (no freeze at a random angle, no
   snap-cut), then step 1's lid screws + lid explode out highlighted.
2. Ticking tasks never moves the model — lid + screws stay exploded out through the whole step.
3. Next (both ticked) → instant step 2: lid + screws vanish as the state swaps; connectors explode out on entry.
4. Back-surf 2→1: completed step shows after-state (parts gone, no ghost-wash, no replay), tasks
   read-only green; forward again → no re-animation.
5. Step 3 completed then revisited: board + chips risen and visible, true colours.
6. Quit on step 1 → modal → Quit → Intro exploded + spinning; Start again = fresh timer AND the
   reassemble transition plays again.
7. Step 5: shell stays visible throughout; Next → Summary exploded + spin.
8. Send report → modal shows **Quit / Close app**: Quit → Welcome page with NO rig floating
   behind it; scan from Welcome → passport arrives in PASSPORT face. Close app → app exits.
9. Step-4 subtitle wrap · bottom bars still (cy 402) · rail ticks/locks correct at every phase.

## 8. Study-instrument note (thesis)

Splits include revisit time (Back-surf). Round 2 moves the removal animation BEFORE the tasks:
the split now also contains the ~2.5 s entry animation of the step itself — identical for every
participant, so comparisons stay clean; one sentence in Methodology.

## 9. Iteration log

- **2026-08-09** — v1 coded from the approved mock (8 notes), 0 device rounds.
- **2026-08-10 — round 1 device feedback (Thiago), 3 corrections:** ① Start froze the showcase
  at a random yaw and snap-cut to assembled → reassemble transition + pivot snap-home.
  ② Removal animated at Next, AFTER the work was done → entry explode on step open; parts vanish
  at the second tick; Next instant. ③ Post-report modal offered passport/scan → the run is
  terminal: Quit (grey → Welcome) / Close app (red → quit). Round-1 modal methods kept as
  stale-wiring fallbacks.
- **2026-08-10 — round 2 (Thiago), 1 correction:** vanish-at-second-tick (round 1's ②) removed —
  the parts now stay exploded for the whole step and disappear on the JUMP to the next step
  (`SetGuidedStepState` already hides earlier removals; ticking no longer touches the model).
  `DisassemblyAnimator.SetStepPartsHidden` stays in the API, currently uncalled — delete in a
  retirement pass if round 3 confirms.
