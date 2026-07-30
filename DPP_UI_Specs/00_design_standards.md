# DPP UI Spec — 00: Design Standards (shared)

> Global standards referenced by every per-screen spec (01–…). When a value here
> conflicts with an older per-screen spec, **this file wins** and the screen spec
> should be updated to match.

---

## 1. Canvas / panel sizes (STANDARDIZED)

| Panel | Size (viewBox units) | Used by |
|---|---|---|
| **Instruction / content panel** | **640 × 430** | Main page (01), Information tab (02), Disassembly intro (03), every Disassembly step (04+), Completion summary (09) |
| **Exploded ACTION ZONE** | **340 × 300 · fully transparent background** | Disassembly steps — floats independently at ~+0.55 m beside the instruction panel |

- All primary panels share the **same 640 × 430 footprint** so the experience feels consistent and panels are interchangeable in space.
- **Action zone (v4, 2026-07-19/20):** the old 268 × 430 navy "exploded-view canvas" is replaced by a **transparent action zone** — the user sees only the live 3D model, the circular grab handle, and the gesture status column (§5). The zone delimits the interaction space without drawing a surface.
- **Spatial independence:** panels are *content-linked* but *independently positioned* — each has its own transform and its own grab handle, so the worker moves each one separately.

> History: 01–03 were originally drawn at varying heights (370 / 460 / 510), re-fitted to 430. The exploded canvas history: 268×430 navy (v1) → 340×300 transparent zone (v3 reset) → + gesture column/HUD (v4.3–4.5).

---

## 2. Color tokens (single source of truth)

Product UI uses fixed brand hexes (not theme-adaptive), matching the Canva "DPP Framework" navy style.

| Token | Hex | Usage |
|---|---|---|
| `navy/panel` | `#0a1f44` | Main panel surface |
| `row/fill` | `#0e2950` | Cards, accordion rows, sub-action cards |
| `card/blue` | `#13366b` | Disassembly choice card, icon circles, control buttons |
| `row/stroke` | `#21407a` | Card / row borders |
| `tab/active-fill` | `#0d2a57` | Active tab |
| `tab/active-stroke` | `#2e5aa0` | Active tab outline, home button outline |
| `tab/inactive-fill` | `#324a6d` | Inactive tab |
| `tab/inactive-text` | `#c2cee0` | Inactive tab label |
| `teal/accent` | `#1d9e75` | Primary CTA, recycling icon, LCA accent, progress fill base |
| `teal/light` | `#5dcaa5` | Highlights, progress fill, scrollbar thumb, step-icon strokes |
| `teal/text` | `#9fe1cb` | Text on teal surfaces, confirmation messages |
| `teal/muted` | `#7fb89e` | Provisional / progress labels |
| `safety/stroke` | `#e24b4a` | Safety red, task icon "not done" state |
| `gold/highlight` | `#f0c879` | "Longest step" tag + time on the summary table |
| `text/on-navy` | `#ffffff` | Primary text on navy |
| `text/secondary` | `#9fb3d1` | Subtitles on navy |
| `text/label` | `#8ba3c4` | Field labels |
| `text/caption` | `#7f9bc4` | Captions / section eyebrows |
| `text/tip` | `#6f86a8` | Tip lines, hints, disabled, footnotes |
| `scroll/track` | `#16335f` | Scrollbar + progress-bar track |
| `grabber/fill` | `#0a0e16` | Grab handles (bar + circle) |
| `grabber/stroke` | `#2a3344` | Grabber bar border |
| `grabber/grip` | `#6b7686` | Grip indicator (bar line / circle inner dot) |
| `zone/pill` | `#0a1f44` (= `navy/panel`) | HUD pill, "?" button fill, hand-chip ring interior |
| `zone/backplate` | `#2e5aa0` (= `tab/active-stroke`) | Gesture-column backing plate. History: `#d3d9e0` camouflaged the white reticle; `#333d4d` was off-brand grey; settled on brand blue 2026-07-20 |
| `hud/active` | `#4da3ff` | Gesture HUD active-band tint (YAW / DIST / ZOOM) |
| `hud/value` | `#dbe4f0` | HUD values resting state |
| `hud/caption` | `#5d7396` | HUD captions, open-hand ring, locked/secondary chrome |
| `hand/pinch` | `#27c46c` | Hand-light chip when that hand is pinching; task icon "done" state |
| `modal/panel` | `#0d1526` stroke `#1a2740` | Zone gesture-guide modal, cancel modal |
| **Model — real-life colors (2026-07-20, matches the printed prototype)** | | |
| `model/housing-bottom` | `#8a5a3b` | PETG brown — printed part 1 |
| `model/housing-upper` | `#f2c11e` | PETG yellow (Bambu Lab approx.) — printed part 2 |
| `model/connector+pcb` | `#2e7d4f` | Green — 3 connectors + PCB board |
| `model/ghost` | 10 % opacity of the part's real color | Step-focus ghosting (non-relevant parts) |

> Retired: `grey/card` `#e9edf3`, `grey/card-stroke`, `navy/canvas-3d` `#081830` (zone is transparent now), old `model/structural` grey. Connector LIFE/SENS-A/SENS-B color coding is done with **physical tape on the printed unit**, not digitally.

---

## 3. Typography

- **Family:** SF Pro — `SF Pro Display` (≥18 px), `SF Pro Text` (<18 px). Unity: SF Pro TTF → TextMeshPro. Fallback: `-apple-system, Helvetica, Arial, sans-serif`.
- **Case:** sentence case everywhere. **Weights:** 400 regular, 700 bold only.
- **Glyph rule:** stick to the SF Pro atlas — no exotic glyphs (✓, ⟳, …). Use text captions (e.g. `YAW`, `DIST`, `ZOOM`) or drawn shapes instead.

| Role | Size | Weight |
|---|---|---|
| Hero / total time | 27–32 | Bold |
| Screen title | 25–27 | Bold |
| Section / card title | 15–16 | Bold |
| Tab label | 15 | Bold active / Regular inactive |
| Body / subtitle | 13–14 | Regular |
| Field label | 13 | Regular |
| Caption / eyebrow | 11–12.5 | Regular |
| HUD caption | 7.5–8 | Bold (all-caps) |
| HUD value | 11 | Bold |

---

## 4. Global interaction rules

**Hover (unchanged):** the white / bright highlight outline appears ONLY on hover (when the worker's pinch-ray passes over an interactive element). Resting state = the element's normal fill. Nothing sits permanently highlighted. Selection = pinch (PICO native hand tracking) routed via `PicoHandUIBridge` → `Button.onClick`. **This applies to EVERY clickable element, including the zone "?" button and the gesture-guide modal's × close.**

**Hand-ray hit areas (2026-07-20):** any element clicked by hand ray gets an **invisible hit area of ≥ ~50 px** with a small visual inside (grab-handle pattern: 52 px hit circle, 15–30 px visual). Visual size is a design choice; hit size is an accessibility requirement — 26 px visuals with 26 px hit areas are unhittable on device.

**On-plane rule (2026-07-20):** `PicoHandUIBridge` computes clicks by intersecting the ray with each **canvas plane**. Clickable UI must therefore sit **ON its canvas plane (z = 0)** — off-plane elements suffer parallax displacement (you aim at the element, the click evaluates centimeters away). **Corollary: any UI group that moves or floats independently (the gesture column, the draggable "?" modal) must be its OWN nested world-space Canvas + GraphicRaycaster** — then the bridge raycasts that group's own (billboarded) plane and clicks stay accurate wherever it goes. Oversized hit areas are the fallback only for tiny floating elements (the grab circle, which does its own plane test).

**Modal state (zone):** a 3D mesh always wins the depth test against world-space UI, so a modal can never reliably render "in front of" the model. When the gesture-guide modal opens: the model, grab handle and column **hide**, gestures pause (`TwoHandTwistRotate.Paused`), and the modal sits on the canvas plane. Closing (or re-entering the screen) restores everything.

---

## 5. Recurring components

- **Header / tab bar:** circular home button (`card/blue` + `tab/active-stroke`, white house icon) at left; two tab pills (`Informations` / `Disassembly`). Active = `tab/active-fill` + outline + white bold. Home → Main Page (with cancel modal inside the step flow).
- **Vertical progress bar (step screens):** thin fill on the far-left edge, top→bottom, track `scroll/track`, fill `teal/light`; `n/N` label at the bottom.
- **Primary CTA:** `teal/accent` button, white bold label, `›` chevron; e.g. `Confirm & next`, `Start disassembly`, `Send dismantling report`.
- **Secondary button:** `Back`, fill `#1a2740`, stroke `tab/inactive-fill`, text `text/secondary`.
- **Task status buttons (step screens):** red (`safety/stroke`) → green (`hand/pinch`) toggle icons; `Confirm & next` stays locked (grey `#1a2740` / `#5d7396`) until both tasks are green.
- **Cancel modal (step flow):** "Want to cancel this disassembly?" — Yes (green) / No (red); Back-to-home aborts the run.
- **Scroll region (content panels):** fixed header + scrollable body (`RectMask2D`), teal thumb on `scroll/track`.
- **Grab handles (reposition), two variants:**
  - **Standard panels — grabber bar:** dark pill (~200×22, `grabber/fill` + `grabber/stroke`, grip line `grabber/grip`) docked below the panel's bottom edge. Pinch-grab and drag moves the whole panel (visionOS-style); billboard-while-dragging; startup recenter-in-front-of-user.
  - **Action zone — grab circle (v3.1):** black disc (15 px) + inner grey dot (5 px, whitens on hover) inside an invisible 52 px hit circle, **pinned below the model's front face** — follows the live model AABB, billboards, and orbits so it is always on the user's side. Zone keeps its spot beside the main panel (no startup recenter).
- **Gesture status column (zone, v4.3–4.5):** vertical stack pinned to the model's **front-left** edge (AABB-derived offset, orbits + billboards). On a `zone/backplate` rounded plate: **[?] help button** (52 px hit, 30 px visual) on top, then the dark pill with **L/R hand lights** (`hand/pinch` solid when pinching / `hud/caption` ring when open), **YAW**, **DIST**, **ZOOM** rows (caption + value). The active band tints `hud/active`: YAW in the rotation band, DIST+ZOOM in the zoom band.
- **Gesture-guide modal (zone):** `modal/panel` 300×250, title "How to control the model", four rows (Move the panel / Rotate / Zoom / Hand lights), × close with hover ring. Opens from "?"; obeys the modal-state rule (§4).

---

## 6. Exploded action zone — interaction standard (v4, 2026-07-20)

All model manipulation is **two-hand gestures, zero on-screen controls** (arc knobs, sliders and part lists were tested and rejected — precision + overlap failures):

| Mechanism | Gesture |
|---|---|
| **Move panel** | Pinch the grab circle, drag (billboards while dragging) |
| **Rotate (yaw only)** | BOTH hands pinching, **5–25 cm apart**: twist like a flat steering wheel — heading change of the hand-to-hand line = model yaw, 1:1. Zoom frozen. |
| **Zoom** | BOTH hands pinching, **beyond 25 cm**: separation is an absolute dial — 25 cm = 1× (default fit), 55 cm = 2× (max). Close toward 25 cm to zoom out. Rotation stops. Model glides to the dialed size (no snaps); ~1 cm hysteresis at the band border. |
| **Guards** | min separation 5 cm; panel-drag wins over gestures; gestures pause while the guide modal is open; per-frame spike filter. |

- **Initial view:** model starts (and resets on every zone entry) at **yaw 180°** — front face toward the user (glTF default showed the back).
- Technical: gestures read the **`RayPose` child** of each `PXR_Hand` (the hand root transform is static). Manipulation applies to the **model anchor**, never the clone, so the constrained-body engine's axes stay intact.
- Pending mechanism: **component-move along dismantling axes** (constrained-body engine is built and dormant on the runtime clone).

---

## 7. Product reference

The DPP data models a **generic 5-step VCU** (inspired by the Bosch Motorsport MS 50.4, but generic as-built): printed housing bottom (brown) + upper (yellow), main PCB (green) carrying 3 processors + 3 chips, 3 green connectors (LIFE / SENS-A / SENS-B marked with colored tape on the physical unit), 14 screws. Tools: Allen key (hex 2.5 mm) · Est. time 5 min · Scope 5 steps. The AR model uses the **real-life color tokens (§2)** so the virtual and printed units read as the same object during the user studies.

---

## 8. Disassembly flow (5 steps + summary)

1. Open the housing — *access*
2. Remove the connectors — *access + value (gold plating)*
3. Lift out the main PCB — *structural*
4. Recover the silicon — *value*
5. Sort the housing — *material recovery*
6. Completion summary — *per-step table (time split + recovered mass + material grams), longest-step gold tag, assumed-splits footnote, `Send dismantling report` → `Done`*

Per-step timing (`step_times_s`) is recorded at each Confirm and shipped in the dismantling report — the user-study dataset.

---

## 9. Per-step three-layer learning model

Each step presents the same component knowledge three ways:

1. **Read it** — task rows in the instruction panel with red→green status buttons (gating the CTA).
2. **Watch it** — a **frameless, looping, state-aware how-to animation** (RenderTexture, no box, no surrounding text): plays *this step's* action; parts not relevant to the step are **ghosted at ~10 % opacity** (step-focus ghosting).
3. **Explore it** — the exploded action zone: free yaw + zoom via the two-hand gestures (§6), live gesture HUD, "?" guide.

---

*Last updated: 2026-07-20 · Status: standard · Referenced by 01–09 · Changes: action zone v4 (transparent, gestures, HUD column, modal-state), real-life model colors, hand-ray hit-area + on-plane rules, dismantling report labels, generic product reference.*
