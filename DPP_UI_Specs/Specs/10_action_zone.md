# DPP UI Spec — 10: Exploded Action Zone (live 3D panel)

> **Living spec** — updated every iteration. Current version: **v4.6.2 (2026-07-20), device-validated.**
> Shared standards (colors, hover, hit areas, on-plane rule): see `00_design_standards.md` §2/§4.
> History of rejected designs at the bottom — do not re-propose them as-is.

---

## 1. Purpose & concept

A **transparent action zone** floating beside the instruction panel where the worker freely
inspects and **manually disassembles** a live 3D clone of the VCU: rotate it, zoom it, and pull
components out along their **real extraction axes**, with the physical dependency order enforced.
No navy surface, no frame — the user sees only the model and its floating controls. The zone is
the "Explore it" layer of the three-layer learning model (spec 00 §9).

## 2. Canvas & spawn

| Property | Value |
|---|---|
| Canvas | World-space, **340 × 300**, scale 0.001, **fully transparent** |
| Spawn | On every activation: at the **main panel's right edge** (centre-to-centre +0.55 m along the panel's right axis), same rotation. User repositions freely afterwards via the grab circle. |
| Model | Runtime clone (`VCU_ZoneModel`) under `ModelAnchor` (z −70, scale ×1000, fit 0.9). Original stays on the `DPPPreview` layer for the RenderTexture loops; user only ever sees the clone. |
| Initial view | **Yaw 180°** — front face (connectors) toward the user. Restored on every zone entry and by the gesture reset. |
| Real-life colors | housing_bottom brown `#8a5a3b` · housing_upper yellow `#f2c11e` · connectors + PCB green `#2e7d4f` (spec 00 §2) — matches the printed prototype. |

## 3. Floating controls (all orbit + billboard with the model)

### 3.1 Grab circle (move the panel)
Pinned just below the model's **front-bottom** (AABB support function, gapFront 0.015 m,
gapBelow 0.035 m). Black disc 15 px + grey dot 5 px (whitens on hover) inside an invisible
**52 px hit circle**. Pinch-grab and drag moves the whole zone; billboards while dragging.

### 3.2 Gesture status column
Own nested world-space canvas (on-plane rule) pinned to the model's front side edge,
vertically centred, offsets derived from the live AABB (2× zoom pushes it outward, never overlaps).
Backplate `#2e5aa0` @ alpha 160/255, rounded; elements navy `#0a1f44`. Top → bottom:

| Element | Behaviour |
|---|---|
| **[?]** | 52 px hit / 30 px visual, hover ring. Opens the gesture-guide modal (§5). |
| **L / R hand lights** | Solid green `#27c46c` = that hand is pinching · dim ring = open. Live always. |
| **YAW** | Current model yaw (°). Tinted `#4da3ff` while in the rotation band. |
| **DIST** | Live hand separation (m); "—" when not tracking two pinches. Tinted in zoom band. |
| **ZOOM** | Current zoom (×). Tinted in zoom band. |
| **[+]** | Toggles the part list (§6.2); glyph rotates 45° to an × while open. |
| **[⟲ recycle icon]** | **Regroup**: every displaced part cascades home (§6.5). |

### 3.3 Gesture-guide modal
Own nested canvas, 300 × 250 navy `#0d1526`, opens centred on the zone. Four rows: Move the
panel / Rotate / Zoom / Hand lights. **× close** (hover ring) + **standard grabber bar** below —
the modal is draggable anywhere in AR space. **Modal state:** while open, the model, grab circle
and column HIDE and all gestures pause; closing (or re-entering the screen) restores everything.

## 4. Whole-model gestures (two-hand, zero on-screen chrome)

Both hands pinching; the line between the two RayPose points drives everything.
Bands are **exclusive**, split by hand separation:

| Band | Separation | Behaviour |
|---|---|---|
| **Rotation** | 5 – 25 cm | Twist like a flat steering wheel → yaw only, 1:1 (`gain`). Zoom frozen. |
| **Zoom** | > 25 cm | Rotation stops. Separation is an **absolute dial**: 25 cm = 1× (default fit) → 55 cm = 2× (max). Close toward 25 cm to zoom out. Model **glides** to the dialed size (`zoomResponse` 5); ~1 cm hysteresis at the border. |

Guards: min separation 5 cm · panel-drag wins · part sessions (§6) and the guide modal block
gestures · per-frame spike filter (12°). Technical: `PXR_Hand.transform` is static — positions
come from the **RayPose child**. Manipulation applies to the **anchor**, never the clone.

## 5. HUD feedback rules

The active band tints its rows `#4da3ff` (YAW alone in rotation band; DIST + ZOOM together in
zoom band). Chips always live, even while gestures are blocked. No other permanent chrome.

## 6. Component drag (mechanism #4)

All parts move **only along their real extraction axis**, clamped to their real travel, screws
spinning with travel. Released parts **park in place** (no snap-back).

### 6.1 Method 1 — direct
Ray over a part **brightens it** (hover cue; small-part-wins picking makes screws hittable over
the lid). Pinch = select: part isolates — **all other parts drop to 50 % opacity** — and while
pinching, hand motion slides the part along its axis (ray↔axis closest-point; no precision
aiming needed once grabbed). Release parks it and restores opacity.

### 6.2 Method 2 — the "+" part list
Rows fan to the user's **right** of the "+" as a **masked 3-row window** (RectMask2D), rows in
disassembly order: Lid screws · Upper housing · Connector screws · Connectors · Board screws ·
PCB · Chip 1–6. States: white = available · grey `#5d7396` = locked · blue `#2e5aa0` + white
outline = held · outline alone = hovered.

- **Scroll:** pinch anywhere inside the window and drag vertically (threshold 20 px turns the
  pinch into a scroll and cancels any accidental hold).
- **Select & drag (two-handed):** pinch-**hold** a name → part isolates in the model. The **other
  hand** pinches **anywhere** — zero aiming — and pulls; the part follows the ray's motion along
  its axis. Release the name → session ends, part stays parked, **list stays open**.

### 6.3 Dependency physics (both directions)
- **Unlock at 50 %:** a part unlocks when every prerequisite has ≥ 50 % of its travel
  (`dependencyUnlockFraction`). Dependency graph: lid ← lid screws; connectors ← connector
  screws; PCB ← lid + board screws; chips ← PCB.
- **Reverse floor:** while any dependent is displaced, its prerequisite **cannot return below
  the 50 % threshold** (you can't screw the lid screws back while the lid floats).
- Locked part touched (either method): red flash + shake, nothing moves; its list row is grey.

### 6.4 Gesture arbitration
Any part session (hover-drag, row hold, scroll) **blocks twist/zoom** (they share two-pinch
postures). Panel grab always wins over everything.

### 6.5 Regroup
The recycle-icon button reassembles every displaced part as a **staggered cascade in reverse
dependency order** (chips → PCB → board screws → connectors → connector screws → lid → lid
screws; 0.1 s stagger, ~0.45 s per part) — the only order the reverse floor permits. Ends any
active session first.

## 7. Implementation map

| Piece | File |
|---|---|
| Zone lifecycle, clone, spawn-at-right, followers (handle + column), modal-state hide | `Scripts/DDP/UI/ExplodedZoneInteraction.cs` |
| Two-hand twist/zoom bands + HUD state + blocking flags | `Scripts/DDP/UI/TwoHandTwistRotate.cs` |
| HUD binding, chips, help modal open/close | `Scripts/DDP/UI/ZoneGestureHUD.cs` |
| Part drag (both methods), list, scroll, regroup | `Scripts/DDP/UI/ZonePartInteraction.cs` |
| Constrained-body engine: bodies, axes, travels, dependencies (both directions), isolation, hover, locked feedback, reassembly | `Scripts/DDP/ConstrainedTeardownModel.cs` |
| Ghost/fade material factory (shared with step-focus ghosting) | `Scripts/DDP/DisassemblyAnimator.cs` (`CreateFadeMaterial`) |
| Builder: canvas, anchor (180°), handle, column, list, modal, wiring | `Editor/DPPUIBuilder.StepFlow.cs` (`BuildExplodedCanvas`, `BuildGestureColumn`, `BuildZoneHelpModal`) |
| Real-life colors utility | `Editor/DPPUIBuilder.Colors.cs` (menu `DPP → Apply Real-Life Colors`) |

Rebuild rule: **Build Phase 4, then always Phase 5** (re-wires the summary hand-off).
Device build caveat: ghost/fade shader (URP Lit or Standard) must be in Always Included Shaders.

## 8. History — tested and REJECTED (do not re-propose as-is)

v2.x control set (2026-07-16→19): free two-finger arcball · curved per-axis arc bars + knobs
(canvas-fixed and model-glued variants) · zoom slider · '+' part-list panel with 3D axis-arrow
drag handle · hover teal wash. Root causes: pointer precision vs small targets, controls-vs-model
visual overlap, too many mechanisms at once. The v4 gesture family succeeds because whole-model
manipulation needs **no on-screen chrome** and part selection needs **no precision after grab**.
Backplate color history: `#d3d9e0` (camouflaged the white reticle) → `#333d4d` (off-brand) →
`#2e5aa0` @160 (final). Initial yaw 270° corrected to 180°.

## 9. Open items

- Field-tune during user studies: `dependencyUnlockFraction`, scroll threshold, pick depth window.
- Possible later: per-frame axis reprojection if drag direction feels inverted after large yaw.

*Last updated: 2026-07-20 · v4.6.2 · Status: device-validated, study-ready*
