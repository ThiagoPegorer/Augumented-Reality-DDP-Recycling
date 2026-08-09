# DPP UI Spec — 05: The Digital Model Panel (the stage)

> **Living spec** — RBv2.1.1. Current version: **v1.2 (2026-08-08) — 04e round 2 pick-routing
> change on top of v1.1's two-scenario routine.** What is described here is what is in the
> build, not what is planned.
> **v1.2 delta:** a pinch on a body acts ONLY on Product specs (opens Component ID) and on
> Usage & service (opens the part's usage record). On every other tab the pick is INERT — the
> old fall-through jumped the user to Product specs from wherever they were, which broke the
> walkthrough position (`ModelLinkController.HandlePick`; `04e_rail_gate.md` v1.1).
> Shared standards: `RB2_1/00_design_standards_rbv2.md` (§4 hover, §4.2 hit areas, §8.1 CAD↔passport
> map). Parent layout: `RB2_1/04_DPP_page.md` (the super panel). Gesture family source:
> `RB2_0/10_action_zone.md` v4.6.2. Approved interaction mock: `drafts/04_stage_gestures_v1.svg`
> (decisions 1–3 agreed 2026-08-08; the round-4 feedback then moved the lock control and removed
> the help button — this file records the as-built result).

---

## 1. Purpose & concept

The stage is the middle canvas of the super panel — **the model permanently between the
navigation and the data**, so there is no state in which the passport is visible and the model is
not (the v1 lesson: P02 and P03 both reported not perceiving the 3D model when it lived beside
the passport).

From RBv2.1.1 the stage model is not an illustration. It is a **selection surface wired two-way
to the passport**: opening a component in the data canvas highlights its bodies on the model, and
pinching a body on the model opens that component's page. Both directions drive one piece of
state, so they cannot disagree. The model is deliberately recoloured to match the printed
prototype on the participant's desk — model and object must read as one thing.

## 2. Geometry

| Property | Value |
|---|---|
| Stage canvas | 400 × 430 units @ 0.001, yaw 0° (deliberately flat — world-pose-preserving re-parents depend on it) |
| Surface | **None at all** (round 8): the `StageFrame` tint and the FREE-state ghost outline are both gone — through passthrough they read as a green panel, not as chrome. The model IS the stage; re-locking is carried by the padlock alone |
| Rig | 0.75 m from the user, eye height 1.1176 m (matches the XR rig's camera offset) |
| `ModelHome` | local (0, −10, −40) canvas units — slightly toward the user, biased LOW |
| Home rotation | **pitch 25°, yaw 205°** — the isometric presentation (round 3): seen slightly from above and off-axis so depth reads; yaw 180° would be the flat connector-face elevation. Tune in play mode on `ModelHome`, then feed the numbers back to the builder |
| Model | Runtime clone `StageModel` of `VCU_assembly`, under `ModelPivot` under the home. Keeps its `DisassemblyAnimator`; colliders enabled; Default layer |

**The pose lives on the HOME, never on the pivot or the clone** — re-lock snaps the pivot's local
rotation to identity, so anything baked lower is silently undone on every re-link.

## 3. Scale — zoom 1.00× is REAL SIZE (round 4)

The closed model's longest side is fitted once to **`realWorldSpan` = 0.200 m** — the physical
printed mock (200 × 150 × 60 mm). Zoom 1.00× therefore means 1:1 with the unit on the desk.

The fit (`ModelLinkController.EnsureFitted`, once per session): reset closed → measure → scale →
open instantly → measure → **re-centre the OPEN pose on the home** (the teardown grows mostly
upward — lid and screws rise, only the bottom shell drops — so without this the exploded model
rides the frame's top edge) → reset. The console logs closed and open spans.

**History, do not resurrect:** the fit used to target the OPEN envelope (0.26 m → ÷1.2 → ÷1.5)
to guarantee the exploded pose stayed inside the stage. That guarantee made the closed model read
as a toy and is **gone by decision** — the exploded pose of a real-size model may approach the
stage frame, and that is accepted.

## 4. States — the round-5 two-scenario routine (Thiago, 2026-08-08)

### Scenario 1 — LINKED (the default; ORANGE padlock `#f28c28`)

* Every DPP-page entry **starts at the home pose (25°/205°)** — round 7: the spin used to
  accumulate on the pivot and survive leaving the passport, so re-entries started at a random yaw.
* Entry plays the **disassembly intro's own teardown** once (`DisassemblyAnimator.PlayFullTeardown`
  — never a bespoke explode); the model stays open, and **then begins the idle showcase spin**:
  30°/s about its own middle axis at the iso tilt (a 12 s loop, gated on `OpenDone` — the spin
  never runs during the animation).
* **No gestures.** Twist/zoom are disabled while LINKED; the only interactions are the two
  selection directions:
  * **Canvas → model:** opening a component keeps its bodies at true materials and turns
    everything else into a **transparent ghost** (α 0.30 fade-material swap — round 6; the darker
    dim read as shadow, not de-emphasis).
  * **Model → canvas:** pinching a body switches the rail to Product specs and opens that page.
    `fasteners` never selects.
* The gesture column is **collapsed to the single padlock** (orange on the dark disc).
* ⚠ Known tension, accepted by ruling: spin + pinch-selection coexist (v1.0 removed the spin
  precisely because a drifting body is harder to hit). At 30°/s picking is slow-moving; if study
  pilots show missed pinches, the fallback is pausing the spin while the ray hovers the model.

### Scenario 2 — FREE (GREEN padlock; a SEQUENCE, not a switch)

Tapping the padlock runs, in order:

1. Lock turns **green** instantly; the link is cut both ways; the spin stops.
2. The model **eases upright** — 25°/205° → **0°/180°** (round 7: square to the user, no iso
   offset; 0.35 s) — while the parts **REASSEMBLE** to their original places: the user floats the
   **closed, real-size unit**, as if holding the physical one. Uprightness stays ENFORCED every
   frame (§6).
3. The column **extends** (70 → 180, 0.2 s), revealing hand lights + YAW / DIST / ZOOM.
4. Only then: the teal **grab bar appears** (drag anywhere in AR space), the column starts
   **following the model**, and the twist/zoom gestures **enable**.

* Re-link reverses it at once (no sequence): gestures off, column collapses, grab bar hides,
  snap home over 0.28 s, **instant re-explode** (no second teardown), spin resumes, re-select
  whatever the data canvas shows. Leaving the passport force-relocks.

## 5. Whole-model gestures (from spec 10 v4.6.2)

Both hands pinching; the horizontal line between the RayPose points drives everything. Bands are
exclusive, split by hand separation, ~1 cm hysteresis, 12° per-frame spike filter:

| Band | Separation | Behaviour |
|---|---|---|
| **Rotation** | 5 – 25 cm | Twist like a wheel → world yaw, 1:1. Zoom frozen |
| **Zoom** | > 25 cm | Absolute dial: 25 cm = 1.00× → 55 cm = **2× cap** (round 7 — back to the zone's dial; the 1.5× stage-containment cap died with LINKED gestures). Glides, never snaps |

* ⚠ **Zoom is applied RELATIVELY** (`scale ×= zoomNew/zoomOld`), never as an absolute base×zoom.
  The pivot reparents between a 0.001-scale canvas and a world-scale free root; an absolute write
  stomps Unity's reparent compensation by ~1000× — the round-4 "model fills the room" bug.
  **Never write an absolute scale to a transform that reparents.**
* Arbitration: the rig grabber and the free-model grabber always win (`extraBlockingHandles` —
  the free bar lives on a sibling root the child-search cannot see). Two hands pinching
  suppresses body-picks so starting a twist cannot yank the data canvas to another page. Known
  gap: the *first* pinch of a two-hand gesture can still land as a pick — device-tune item.
* Round 5: gestures are live in **FREE only** — the component is disabled while LINKED
  (`resetOnEnable` off; the view owns the freed pose and the resets).

## 6. The gesture column

44 × 180 at stage-local centre (370, 218), backplate `#2e5aa0` @ α160. Top to bottom:

| Element | Behaviour |
|---|---|
| **Padlock toggle** | THE LINKED/FREE control (round 4 — it replaced the [?] help button; the round-3 rail tile lasted one build). 44 hit / 30 visual, dark-blue disc. Round 6: **Thiago's own padlock artwork** — `ic_lock_linked` (orange, closed) / `ic_lock_free` (green, open), in `Assets/Textures/Icons`; the glyph is NEVER tinted, only the word beneath takes the state colour. Wired to `SuperPanelView.ToggleLock` |
| **L / R hand lights** | Solid green `#27c46c` = pinching · dark disc = open. Always live |
| **YAW / DIST / ZOOM** | Live readouts; the active band's rows tint `#4da3ff` (YAW in rotation; DIST+ZOOM in zoom) |

Round 5: while LINKED the column is **collapsed to the padlock alone** (backplate 70 tall,
top-anchored); the free sequence grows it downward to 180 and reveals the rows (`hudExtras`).

Not ported from the zone, by decision (mock, agreed): **no [+] part list** (the parts list IS the
Component ID page), **no ⟲ regroup** (nothing parks on the stage), and — round 4 — **no [?]** and
no gesture-guide modal.

**While FREE the column follows the model** (`StageGestureHudFollower`, the zone's §3.2 follower
reduced): every LateUpdate it pins beside the model's live AABB — offset by the bounds' extent
along the viewer's right + 55 mm, so zoom pushes it outward and it never overlaps the mesh — and
yaw-billboards to the user (never pitches). On re-link it parks back at its stage pose.

The column carries its **own nested Canvas + GraphicRaycaster** — spec 10 §3.2's *on-plane rule*,
re-learned round 6: the hand-ray bridge resolves pointer hits against canvas PLANES, so a child
graphic floated off its parent's plane looked right and was untouchable. With its own canvas the
column's plane travels with it; it still hides with the rig like everything else.

Round 6 also made FREE's uprightness **enforced, not just eased**: the free grab bar's billboard
re-orients the free root toward the eye and was re-tilting the model to ~25° the moment it
activated. While FREE, the view flattens the pivot's pitch and roll every frame, keeping only the
user's yaw. "No inclination" is a contract, not a transition.

Round 7: **the drag bar follows too** — the same follower component in *below/always* mode,
yaw-billboarded square to the user; it used to keep whatever pose the free root had, which left
it lateral after a few moves.

Round 8 anchoring corrections, both from device:

* **The column pins to the model's FRONT SIDE EDGE** (round 9 — zone §3.2 verbatim, after the
  round-8 model-frame anchor orbited with the model's yaw and ended up behind the unit at 269°):
  the same camera-facing plane as the drag bar, at the viewer's right of the live AABB, offsets
  derived from the bounds so zoom pushes it outward and the gap to the model stays constant.
* **The drag bar pins to the FRONT-BOTTOM** (zone §3.1): camera-facing front face + a **capped
  drop** (`maxDrop` 0.14 m) — at 2× zoom a strictly-below bar fell out of the user's view.
* The lock column rides higher on the stage (centre y 218 → 178).

## 7. Colours & highlight

* Real-life colours (`RBv2_1/Tools/Apply real-life colors`): **both housing shells brown
  `#8a5a3b`** (the physical printed lid is brown — decided 2026-08-08, superseding the yellow-upper
  mapping), PCB + connectors green `#2e7d4f`, chips keep their CAD group colours (00 §8.1: gold /
  blue / red / brown), screws untouched. The tool walks **all** `DisassemblyAnimator`s and clones
  materials **per colour** (the two shells share glTF `mat_0`).
* Dim technique: MPB write of base × 0.35 on **all three property names** — `_BaseColor`, `_Color`
  **and `baseColorFactor`** (glTFast's name; writing only the URP names silently no-ops — the
  round-3 invisible-dim bug). No alpha, no transparent queue, no PICO sorting risk. The selected
  group is written back at its stored colour, so a body can never keep a stale dim.

## 8. Implementation map

| Piece | File |
|---|---|
| Model↔data link, fit (real size), open-pose re-centre, dim, pick | `Scripts/DDP/UI/ModelLinkController.cs` (on `ModelPivot`) |
| Twist/zoom bands, relative zoom, `ResetPose`, blocking handles | `Scripts/DDP/UI/TwoHandTwistRotate.cs` (SHARED with the zone — additive changes only) |
| HUD value binding, hand lights, band tints | `Scripts/DDP/UI/ZoneGestureHUD.cs` (shared; help refs null on the stage) |
| Column follower (FREE) | `Scripts/DDP/UI/StageGestureHudFollower.cs` |
| LINKED/FREE state, snap home, tab logic, `Paint` (padlock colours) | `Scripts/DDP/UI/SuperPanelView.cs` |
| Teardown (THE animation — never bespoke) | `Scripts/DDP/DisassemblyAnimator.cs` |
| Builder: rig, clone, home pose, gesture wiring, column | `Editor/DPPUIBuilder.SuperPanel.cs` (`RBv2_1_1/1`, `/2`, `SpGestureHud`, `SpBuildStageModel`) |
| Recolour tool | `Editor/DPPUIBuilder.Colors.cs` |

**Rebuild chain — always:** `RBv2_1_1/1 → RBv2_1/9 → RBv2_1_1/2 → RBv2_1/Tools/Verify wiring →
SAVE`. `/1` destroys the ProductSpecsPage living in the rig's data canvas, which is why `/9`
must follow it. Never re-run `RBv2_0/4` or `/5` while the stage clone exists (their active-only
animator Find can bind — or layer-hide — the stage model).

⚠ `/2` **merges** into `tabPages`, never overwrites (round 6): the version that replaced the
whole array wiped the training page out of ShowPage's management — it stayed permanently active
under the other pages, and its invisible "Continue to disassembly" hit root over parts-list row 7
sent the penultimate component's pinch to the disassembly intro. `00` §4.3's invisible-hit-area
lesson, third sighting.

## 9. History — decided and rejected (do not re-propose as-is)

* Idle yaw (locked "living illustration" spin) — removed when the model became a selection surface.
* Lock button on the stage (bottom centre → upper right) → rail "Model link" tile (one build) →
  **padlock in the gesture column** (round 4, final for now).
* [?] help button + gesture-guide modal — built round 3, removed round 4.
* Open-envelope fit chain 0.26 → ÷1.2 → ÷1.5 — replaced by real-size closed fit.
* Zone grab **circle** — not ported; the FREE grab keeps the proven teal bar.
* Yellow upper housing — the print's lid is brown; mapping corrected.
* Part axis-drag on the stage — rejected (mock decision 1): pinch already means *select* here;
  dragging parts out belongs to the action zone / training disassembly.
* Gestures while LINKED — built round 3, **reversed round 5**: LINKED is a select-only showcase;
  the gesture kit belongs to FREE.
* Grey LINKED padlock — round 4, replaced by orange round 5 (grey read as disabled).
* No idle yaw — the v1.0 rule, **reversed by ruling round 5**: the showcase spin is back, gated
  to start after the teardown. See §4's known-tension note before re-litigating either way.

## 10. Open items

1. ~~Iso angles~~ — **approved as final** (25°/205°, round 5).
2. 1.5× zoom overlap: confirmed on device round 4 — Thiago has a design idea pending; the
   FREE-only gesture change may already retire the LINKED-state overlap. Re-check on device.
3. Follower feel: 55 mm gap, yaw-only billboard — first guesses, one-line tunes.
4. First-pinch pick race at two-hand gesture start (§5) — FREE only now.
5. HUD 6.5 pt captions at 0.75 m — legibility unconfirmed.
6. `SpLockButton` (retired) — delete in the next retirement pass.
7. Training-disassembly tab will get changes EXCLUSIVE to that tab (Thiago, 2026-08-08) — this
   spec governs the stage everywhere else; expect a §-level carve-out when 04b lands.
8. Spin vs pinch-selection (§4 scenario 1) — watch miss rates in the study pilot.

*Last updated: 2026-08-08 · v1.1 · Status: as-built, round-5 routine implemented, awaiting device test*
