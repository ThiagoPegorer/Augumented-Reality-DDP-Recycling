# RBv2.1 — 00: Design Standards (UI + UX)

> **The single source of truth for RBv2.1.** Every screen spec (`01`–…) resolves its sizes,
> colours, type and interactions against this file. Where this file and a screen spec disagree,
> **this file wins** and the screen spec is corrected.
>
> **Lineage.** Sections 1–5 carry forward from `../RB2_0/00_design_standards.md` (dated
> 2026-07-20) — those values were device-tested through two participant sessions and are not
> being re-litigated. **§6 (gestures) and §7 (sound) are new**: RB2.0 had gestures scattered
> across screen specs and no audio documentation at all, because the audio system did not exist
> until 2026-08-01.
>
> **Numbering restart.** RB2.1 renumbers from `00` and is a **self-contained set** — `../RB2_0/`
> is legacy and expected to be deleted once RB2.1 is complete (Thiago, 2026-08-04). Nothing here
> may say "see RB2_0 for the rest."

---

## 1. Canvas and panel dimensions

| Panel | Size (viewBox units) | Used by |
|---|---|---|
| **Instruction / content panel** | **640 × 430** | Every screen. No exceptions. |
| **Exploded ACTION ZONE** | **340 × 300**, fully transparent | Digital model exploration, floats ~+0.55 m beside the panel |

- **World scale:** `0.001` — a 640 × 430 canvas is 0.64 × 0.43 m in the room.
- **One footprint, always.** RB2.0 learned this the hard way: `01`–`03` were drawn at 370 / 460 /
  510 and re-fitted to 430, and the first-run prompt shipped as a 440 × 210 card before being
  rebuilt at full size. A one-off panel size always ends up looking like a mistake next to the
  others. **A new screen uses 640 × 430 or it argues its case here first.**
- **Spatial independence:** panels are content-linked but independently positioned — each has its
  own transform and its own grab handle, so the user moves each one separately.
- **Default placement:** 0.6 m in front, eye height 1.1176 m, yaw-only facing (upright, never
  pitched). Panels that open mid-session (prompts, modals) **recenter in front of the user** on
  open, because the user has been moving their hands and may have drifted.

### 1.1 Standard content geometry (panel-local)

| Region | Coordinates |
|---|---|
| Header title | x 76, baseline ~36 · back circle at x 24 |
| Header rule | y 76, x 24 → 616 (1 px, `#1a335f`) |
| Content band | **y 88 → 418** — 12 px below the rule, 12 px above the panel edge, **mirror-equal** |
| Side margins | 24 px left and right |
| Two-column split | left 24 → 314 · right 326 → 616 (290 wide each, 12 px gutter) |
| Button row | cy 376 |

**Mirror the margins.** Unequal top/bottom padding was the single most repeated audit finding in
RB2.0's detail pages.

## 2. Colour tokens

Fixed brand hexes, not theme-adaptive.

| Token | Hex | Usage |
|---|---|---|
| `navy/panel` | `#0a1f44` | Main panel surface |
| `row/fill` | `#0e2950` | Cards, rows |
| `card/blue` | `#13366b` | Icon circles, control buttons |
| `row/stroke` | `#21407a` | Card / row borders |
| `tab/active-fill` | `#0d2a57` | Active surfaces |
| `tab/active-stroke` | `#2e5aa0` | Active outline, home button outline, zone backplate |
| `tab/inactive-fill` | `#324a6d` | Inactive surfaces, secondary button stroke |
| `teal/accent` | `#1d9e75` | **Primary CTA**, task done state, progress fill base |
| `teal/light` | `#5dcaa5` | Highlights, progress fill, scrollbar thumb |
| `teal/text` | `#9fe1cb` | Text on teal, confirmations |
| `teal/muted` | `#7fb89e` | Provisional / progress labels |
| `safety/stroke` | `#e24b4a` | **Red.** Task not-done state · end-of-life bar. See §2.1 |
| `gold/highlight` | `#f0c879` | High-value accents (longest step, value-bearing subtitles) |
| `text/on-navy` | `#ffffff` | Primary text |
| `text/secondary` | `#9fb3d1` | Subtitles |
| `text/label` | `#8ba3c4` | Field labels |
| `text/caption` | `#7f9bc4` | Captions, eyebrows |
| `text/tip` | `#6f86a8` | Hints, disabled, footnotes |
| `scroll/track` | `#16335f` | Scrollbar + progress track, row hairlines |
| `grabber/fill` | `#0a0e16` | Grab handles |
| `grabber/stroke` | `#2a3344` | Grabber bar border |
| `grabber/grip` | `#6b7686` | Grip indicator |
| `hud/active` | `#4da3ff` | Gesture HUD active band |
| `hud/value` | `#dbe4f0` | HUD values at rest |
| `hud/caption` | `#5d7396` | HUD captions, **locked/disabled chrome** |
| `hand/pinch` | `#27c46c` | Hand-light chip while pinching |
| `modal/panel` | `#0d1526` stroke `#1a2740` | Modals |

**Model — real-life colours** (match the printed prototype so virtual and physical read as one object):

| Token | Hex | Part |
|---|---|---|
| `model/housing-bottom` | `#8a5a3b` | PETG brown |
| `model/housing-upper` | `#f2c11e` | PETG yellow |
| `model/connector+pcb` | `#2e7d4f` | Green — 3 connectors + PCB |
| `model/ghost` | 10 % opacity of the part's real colour | Step-focus ghosting |

### 2.1 Red is a reserved signal — **four** sanctioned meanings

`safety/stroke` `#e24b4a` carries **state, consequence or regulation — never decoration**:

| # | Meaning | Where |
|---|---|---|
| 1 | **"this task is not done"** | step task circles (red ✗) |
| 2 | **"this unit has consumed its design life"** | usage-profile bar |
| 3 | **"this action ends the session"** | `Close app` (Thiago, 2026-08-04) |
| 4 | **"this is a regulatory or safety marking"** | compliance badge + its modal chips (Thiago, 2026-08-04) |

**Rule for meaning 3:** red marks the **exit**, and only the exit. It sits on the **secondary
button slot** (180 × 52 at cx 114) — never on a primary CTA, never on a button that continues the
journey. One red button per screen, maximum.

**Red button treatment:** solid fill `#e24b4a`, white bold label, no chevron (a chevron means
"forward"). Same geometry as the secondary pill it replaces, so no hit target moves.

**Rule for meaning 4 — added 2026-08-04 with the compliance badge (`04` §6).** Regulatory red is
**outline and glyph only, never fill.** A solid red surface means "consequence" (meaning 3) and the
two must stay visually separable at a glance:

| | Meaning 3 — consequence | Meaning 4 — regulation |
|---|---|---|
| Treatment | solid `#e24b4a` fill, white label | `#e24b4a` 1.4 px stroke + shield glyph, `text/on-navy` label on `row/fill` |
| Shape | 180 × 52 pill in the button row | 200 × 30 badge in the header band |
| Position | bottom-left button slot | header, right of the title |

Three constraints on meaning 4, all mandatory:

1. **Red never carries the message alone.** The badge always spells out what it marks
   (`CE · REACH · WEEE 5 · IP67`), so a colour-blind user loses nothing — the same mitigation
   already required for red beside green below.
2. **It is informational, not an alert.** The badge says "this product carries these markings",
   not "something is wrong". Red inside the modal is narrower still: a chip is red **only when the
   marking itself reports something adverse** — currently `REACH`, because 2 SVHCs are declared
   above 0.1 % w/w. `CE`, `WEEE 5` and `IP67` stay `teal/light`.
3. **One regulatory badge per screen**, in the header. It is never repeated inside the content band.

⚠ **Red beside green.** `Close app` red now sits next to a `teal/accent` green CTA on both the
Welcome page and the scan-error panel. Roughly 8 % of men have a red-green colour deficiency, for
whom that pair is the hardest possible discrimination. **Mitigated, and the mitigation is
mandatory, not optional:** the two buttons must always differ in **label, position and size** as
well as hue — never rely on colour alone to distinguish them. Both current uses satisfy this
(180 px grey-slot left vs 388 px primary right).

⚠ **Open concern carried from RB2.0:** the pending task circle shows a red ✗ *before the
participant has done anything*, which can read as "error" rather than "to do". Never
participant-tested. Watch it in the next think-aloud.

## 3. Typography

- **Family:** SF Pro — `SF Pro Display` (≥18 px), `SF Pro Text` (<18 px), via TextMeshPro SDF.
- **Case:** sentence case. **Weights:** 400 and 700 only.

| Role | Size | Weight |
|---|---|---|
| Hero / total time | 27–32 | Bold |
| Screen title | 25–27 | Bold |
| Detail-page title | 19 | Bold |
| Section / card title | 15–16 | Bold |
| Body / subtitle | 13–14 | Regular |
| Field label | 13 | Regular |
| Caption / eyebrow | 11–12.5 | Regular |
| HUD caption | 7.5–8 | Bold, all-caps |

### 3.1 Glyph rule — the atlas does not contain what you think

The SF Pro SDF atlas in this project is **missing** these, and a missing glyph renders as a
visible box on device:

| Missing | | Present |
|---|---|---|
| `≤` U+2264 · `→` U+2192 · `Ω` U+03A9 · `−` U+2212 (true minus) | | `°` · `×` · `±` · `µ` · en dash · em dash · middot `·` |

**Shipped bugs caused by this rule being broken: 2.** Write `max` instead of `≤`, use an en dash
for ranges instead of `→`, use the ASCII hyphen for minus. Marks like ✓ and ✗ are **drawn from
capsule bars, never typed** (§5).

## 4. Interaction rules (non-negotiable)

**Hover — the element RISES. RBv2.1.1 retires the white outline.** Thiago, 2026-08-06: *"the
white border is a bit old school and not a modern method… create a highlight that gives the idea
the button is jumping a bit."* A ring is a flat-screen convention; in a headset the surface has
real depth, so hover is a physical response, not a decoration:

| | while hovered | why |
|---|---|---|
| Position | **−6 units along the canvas normal** (toward the user) | the actual elevation cue |
| Scale | **× 1.03** | supports the rise; alone it reads as a zoom |
| Shadow | drops **3 units** further, alpha **+0.12** | what sells "off the surface" — the rise is invisible without it |
| Fill | **+0.09** on each RGB channel | as if catching more light |
| Ease | ease-out over **0.09 s** | a snap reads as a state change, a rise as a response |

All four live in `HoverHighlight`; **no builder wires them.** The ring survives behind
`useOutline = false` so it can be switched back on for a participant comparison.

*Corollary that governs layout:* **chrome = touchable.** A card border around static content
reads as a button — RB2.0 user feedback, and the reason the detail pages carry chrome on scroll
windows only. **RBv2.1.1 adds a second corollary: elevation = touchable.** Nothing that cannot be
pressed gets a shadow.

### 4.1 The elevation kit — every clickable surface, no exceptions

Three pieces, always in this child order:

| Child | Geometry | Colour |
|---|---|---|
| **`Shadow`** | visual box **+3**, offset **+3 down**, FIRST child | black **α 0.32** |
| *the element* | its own stroke / fill / icon / label | per §5 |
| **`Gloss`** | see below — after the fill, before the label | white **α 0.10** / **0.05** |

**The sheen does NOT scale with the element**, and that is the point:

| element height | gloss | alpha | position |
|---|---|---|---|
| ≤ 40 (buttons, pills, rows) | **40 %** of the height | 0.10 | centred on the upper third |
| > 40 (tabs, cards, entries) | **18 %, capped at 12** | 0.05 | pinned 3 under the top edge |

40 % of a 34-unit pill is light falling across a curved top. 40 % of a 170-unit role card is a
68-unit grey slab — a big flat card has no curve for light to fall on, only a lit top EDGE.
Device test 2026-08-06: *"the glow in the tabs might be too much… the small buttons are okay."*

Built by `AddShadow()` and `AddGloss()`. ⚠ **The names `Shadow` and `Fill` are load-bearing** —
`HoverHighlight` resolves them from the children by name, so a hand-rolled button renders fine and
silently does not respond to hover.

Two rules the pieces themselves encode:

- **The shadow is sized from the VISUAL box, never the hit box.** A shadow around an invisible
  50-unit hit area is a grey halo in mid-air.
- **The gloss is inset 12 units.** A highlight that reaches the edge reads as a second border
  rather than as light.

### 4.2 Corners — `Pill` is not a capsule

⚠ `DPPSpriteFactory.Pill` is a **400 × 44 rounded rect with a ZERO 9-slice border**, authored to
be *stretched* at 200 × 22 for the grabber bar. Drawn at any other aspect its corners stretch into
ellipses — at 110 × 34 the radius becomes 6 units wide by 17 tall, which reads as an angular
corner. This was the "buttons are a bit angulate" defect of 2026-08-06.

**Use `Capsule(img, visualHeight)`**: it swaps in the 9-sliced r22 sprite and sets
`pixelsPerUnitMultiplier = 24 / (h / 2)`, landing the corner radius exactly on half the height at
any size, from one sprite.

**Hit areas ≥ 50 px.** Any hand-ray target gets an invisible hit area of ≥ ~50 px with a smaller
visual inside (pattern: 52 px hit circle, 15–40 px visual). Visual size is a design choice; hit
size is an accessibility requirement. 26 px visuals with 26 px hit areas proved unhittable.

**On-plane rule.** `PicoHandUIBridge` computes clicks by intersecting the ray with each **canvas
plane**, so clickable UI must sit **on its canvas plane (z = 0)** — off-plane elements suffer
parallax (you aim at the element, the click lands centimetres away). **Corollary: any UI group
that moves independently must be its own nested world-space Canvas + GraphicRaycaster.**

**Modal depth.** A 3D mesh always wins the depth test against world-space UI, so a modal can
never reliably draw "in front of" the model. When a modal opens over the zone: the model, grab
handle and gesture column **hide**, gestures pause, the modal sits on the canvas plane. Closing
restores everything.

### 4.3 A selector never relabels itself

**A tab, pill or segmented control shows the same label in every state it can reach.** It names a
DESTINATION, not the current contents. The moment it renames itself to whatever is on screen it
stops being a landmark and becomes a readout — and on a hand-tracked panel, a control whose label
changes under the hand reads as the control having MOVED.

This was the "the pill says `Upper housing shell (HPDC)`" defect of 2026-08-06 in 04c: the second
sub-tab took the open component's name three levels deep, which was defensible on paper (the
selector keeps naming what is below it) and wrong on the headset.

**Where the varying text goes instead:** the caption line, which is already right-aligned on the
rule under the pills and blank by default. Position, mass, `n of 8` — all fine there. If the
caption cannot carry it, the state needs a title, not a mutating tab.

**Corollary — hide the button, don't blank it.** When an action does not exist in a state, disable
the whole GameObject. A label set to `""` leaves a ≥ 50-unit invisible hit area (§4.2) that eats
pinches aimed at whatever is behind it, and that failure is silent.

### 4.4 Never move a RectTransform that HoverHighlight lifts

`HoverHighlight.OnEnable` calls `Apply()`, and `Apply()` writes `lift.localPosition = _restPos` —
the resting pose captured the **first** time that object was enabled. So any layout code that
repositions the same transform is silently undone the next time its parent state is activated.

This cost a device round in 04c: the `i` was positioned per component by `LayoutDetail`, then
reset to its build-time pose every time the Detail state came back on, and it appeared beside an
arbitrary row.

**Two ways out, pick one per element:**

1. **Keep it fixed** and let something else absorb the variation. (04c: the `i` moved to the
   button line, which never moves.)
2. **Give `HoverHighlight.lift` a CHILD to raise.** The outer transform is then free to be moved
   by layout, and only the inner visual rises. Wire `shadow` and `fill` explicitly when doing
   this — the by-name lookup only searches direct children.

The capture is deliberate and must not be made lazy: capturing on first hover would record an
already-raised pose if the pointer were over the element on the frame it was enabled, and the
button would never come back down.

## 5. Recurring components

- **Primary CTA** — `teal/accent` pill, height 52, white bold 16, `›` chevron drawn as two
  capsule bars (no glyph). **Locked state:** fill `#1a2740`, text and chevron `hud/caption`,
  `interactable = false`, hover disabled.
- **Secondary button** — height 52, fill `#1a2740` inside a `tab/inactive-fill` stroke, label
  `text/secondary`, no chevron.
- **Destructive button** — the secondary slot in solid `safety/stroke` `#e24b4a`, white bold
  label, no chevron. Session-ending actions only (§2.1).
- **Small button row (RBv2.1.1, the 420 data canvas and the stakeholder screen)** — left slot
  **110 × 34**, primary **150 × 34**, both drawn inside a **50-unit hit root** so §4's hit-area
  rule still holds while the button looks half the size. The primary is still on the right.
  ⚠ This deliberately breaks "keep these coordinates across screens" below: the bar a user
  repeats four times (one per DPP tab) beats the bar they see once.
- **Standard button row (RB2.0 screens)** — left slot 180 × 52 at cx 114, primary 388 × 52 at cx 422, both cy 376.
  **Keep these coordinates across screens** so hit targets do not move under the user.
  **The primary is always on the right.** A screen that puts the go-forward action on the left
  teaches the opposite reflex to every other screen — this was wrong on the RB2.0 scan-error
  panel and is corrected in `02`.
- **Back** — circular button at the header's left (x 24), `←` sprite. Every Back moves exactly
  **one step**; an edge that leaves the session says **Quit**, never Back.
- **Grabber bar** — dark pill ~200 × 22 (`grabber/fill` + `grabber/stroke`, grip `grabber/grip`)
  docked below the panel's bottom edge. Pinch-drag repositions the whole panel; billboards while
  dragging; keeps its rotation on release.
- **Scroll window** — `RectMask2D` viewport + taller content + `PinchScrollArea`. Chrome allowed
  here because it *is* touchable. A partially-clipped last row is the scroll affordance.
- **✓ and ✗ marks** — both drawn from **3 px capsule bars** so the pair matches in weight:
  ✓ = two bars (9 and 16 long) at ±45°; ✗ = two bars (20 long) at ±45°. Never typed (§3.1).
- **Status circle** — 36 px disc, binary: `safety/stroke` + ✗ = pending, `teal/accent` + ✓ = done.
- **Modal** — `modal/panel` fill and stroke, own canvas, `sortingOrder 10`, recenters on open.

---

# UX

## 6. Gestures

All manipulation is **hand tracking, no controllers, no on-screen sliders or knobs** (arc knobs
and sliders were built, tested and rejected in RB2.0 for precision and overlap failures).

Technical basis for every gesture below: read the **`RayPose` child** of each `PXR_Hand` — the
hand root transform is static and will silently give wrong results. Pinch state is `hand.Pinch`,
valid only when `hand.Computed`.

| # | Gesture | Input | Result |
|---|---|---|---|
| 1 | **Point** | ray over an element | hover outline appears |
| 2 | **Pinch** | one hand, pinch while pointing | click / select |
| 3 | **Pinch-drag a panel** | pinch the grabber bar, move | panel follows at grab distance, billboards |
| 4 | **Pinch-drag a list** | pinch inside a scroll window, move vertically | list scrolls; a threshold (~12 px) protects taps |
| 5 | **Pinch-drag a part** | pinch a model part, move | part slides along its real extraction axis only |
| 6 | **Two-hand twist** | both pinching, **5–25 cm apart**, rotate the hand-to-hand line | model yaw, 1:1. Zoom frozen. |
| 7 | **Two-hand spread** | both pinching, **beyond 25 cm** | absolute zoom dial: 25 cm = 1×, 55 cm = 2×. Rotation frozen. |

**Guards:** minimum separation 5 cm · panel drag beats model gestures · gestures pause while a
modal is open · per-frame spike filter · ~1 cm hysteresis at the 25 cm band border.

**Initial model view:** yaw 180° on every zone entry — the glTF default faces away from the user.

**Aim-free drag (5).** Part dragging uses the closest-point parameter between the hand ray and
the part's axis, so only the ray's **motion along the axis** matters, not what it points at. The
user does not have to keep aiming at a part that is moving away from them.

## 7. Sound

Added 2026-08-01 from P02's feedback: *"I missed sounds when clicking and interacting with the AR
application."* Three sounds, one rule each. All are **2D** (`spatialBlend = 0`) — they are
interface feedback, not objects in the room.

Files live in `Assets/Audio/UI/`. All synthesised, 48 kHz mono 16-bit.

### 7.1 Button click — "Buttons sound"

| | |
|---|---|
| File | `ui_click.wav` — 70 ms, noise tick + two-tone decaying body |
| Fires on | **every** `Button.onClick` in the scene |
| Volume | 0.9 |
| Component | `UIClickAudio` — one scene object sweeps all Buttons at `Start` |

Deliberately global: a button that is silent when its neighbours click reads as broken. Adding a
new button requires no audio wiring.

### 7.2 Grab — per-hand water drop

| | |
|---|---|
| Files | `pinch_right.wav` (rising bubble chirp, 680 Hz) · `pinch_left.wav` (410 Hz) |
| Fires on | **taking hold of something** — a part, a panel bar, a scroll list, a scrollbar |
| Pan | right hand +0.6 · left hand −0.6 |
| Volume | 0.5 |

**The rule that matters: an air pinch is silent.** The sound is the audible partner of the L/R
hand indicators, so it must mean "you grabbed *that*", not "your fingers touched". Achieving this
is why the audio component does **not** poll the hands — the interaction scripts call
`HandPinchAudio.ObjectGrabbed(rightHand)` when they actually take hold. Higher pitch right,
lower left, so the two hands are distinguishable without looking.

### 7.3 Drag — wind with directional EQ

| | |
|---|---|
| Files | `drag_loop_right.wav` · `drag_loop_left.wav` — 2.0 s seam-crossfaded filtered-noise loops |
| Fires on | every frame an object is **actually moving** under a drag |
| Driven by | `HandPinchAudio.DragTick(rightHand, worldDelta)` — the **object's** motion, not the hand's |

The loop is equalised continuously against the drag direction, projected onto the head-camera
plane so "up" means up on the user's screen:

| Direction | Low-pass | High-pass | Other |
|---|---|---|---|
| **Up** | opens to 7500 Hz | rises to 420 Hz | bright, airy |
| **Down** | closes to 380 Hz | drops to 30 Hz | dark rumble |
| **Left / right** | — | — | stereo pan sweep + pitch bend ±0.10 |
| **Speed** | — | — | drives volume (max 0.4 at 0.35 m/s) |

Cutoffs interpolate in the **log domain** (perceptual), smoothed over 0.10 s to prevent zipper
noise. Fade in/out 0.12 s; the loop stops **0.15 s after ticks stop arriving** — there is
deliberately **no release call**, so a dropped pinch can never leave the wind running.

**Two consequences that are features, not bugs:**

- **An object held still is silent.** Volume follows speed, so holding without moving makes no sound.
- **A part pinned at a dependency limit goes quiet** even while the hand keeps pulling — because
  the audio follows the part, not the hand. That is free haptic-like feedback for "this will not
  move yet."

### 7.4 Rules for adding sound in RB2.1

1. **Never add a looping sound tied to a gesture rather than to an outcome.** The first drag
   implementation looped on any held pinch and was rejected on feel within one test.
2. **Silence is a valid signal.** Every sound above has a defined silence that means something.
3. **No sound for state the user cannot cause.** Audio marks user actions, not app events.
4. Anything new gets a row in this section before it gets a `.wav`.

---

## 8. Product reference

A **generic 5-step VCU** (inspired by the Bosch Motorsport MS 50.4, generic as-built): printed
housing bottom (brown) + upper (yellow), main PCB (green) with 3 processors + 3 chips, 3 green
connectors (LIFE / SENS-A / SENS-B, marked with coloured tape on the physical unit), 14 screws.
Tools: Allen key (hex 2.5 mm) · ~5 min · 5 steps.

Prototype outer size **200 × 150 × 60 mm**; the datasheet product is **166 × 121 × 41 mm**. These
are different objects and the passport must not confuse them.

### 8.1 CAD ↔ passport map — the one table three specs depend on

`VCU_assembly.gltf` carries **41 nodes, 26 of them with meshes**. The passport carries **8 parts
and 7 board materials**. They do not correspond, and pretending otherwise is how the model link,
the training teardown and the step nomenclature all end up disagreeing with each other.

The map lives in the **payload** (`components[].mesh_nodes`, schema v0.17), not in code — the BOM
is the source of truth and a lookup table compiled into the app would be a second one.

| passport id | display name | glTF nodes | CAD colour |
|---|---|---|---|
| `housing_upper` | Upper housing shell (HPDC) | `housing_upper` | `#989898` grey |
| `housing_bottom` | Bottom housing shell (HPDC) | `housing_bottom` | `#989898` grey |
| `pcb` | Bare PCB, 4-layer FR-4 | `pcb` | `#00592C` green |
| `connectors` | Connectors 3× AS018-35 | `connector`, `connector001`, `connector002` | `#363636` near-black |
| `ic_1` | Processors 2× FCBGA + flash 2× 4 GB | `component1` | **`#E0C14A` gold** |
| `ic_2` | Regulators + analog front-end | `component3` | **`#003C99` blue** |
| `ic_3` | Power stages 6× (DPAK) | `component2` | **`#CB3636` red** |
| `ic_4` | Comm transceivers + MEMS sensors | `component4`, `component001`, `component002` | **`#8F5110` brown** |
| `fasteners` | Fasteners (~12 × M3) | 14 screw nodes (4 housing · 6 connector · 4 PCB) | `#989898` grey |

Confirmed by Thiago 2026-08-07 from the CAD colours; the four IC assignments were proposed from
body size, count and board position and accepted. **All 26 mesh-bearing nodes are claimed exactly
once**, checked in both directions.

> ⚠ **`ic_3` ↔ `ic_4` corrected on device, 2026-08-09 (payload v0.18).** The 2026-08-07 table had
> the red `component4` family on `ic_3` and `component2` on `ic_4`. Round-7 device testing showed
> the LAST Component-ID row (ic_4) highlighting the RED bodies — the two assignments were crossed.
> `mesh_nodes` swapped in `vcu_001.json` (both copies); the highlight is the arbiter, not the
> proposal. The colour column above now records what the glTF actually renders.

**Three consequences that are easy to get wrong:**

1. **One passport row can be several bodies.** Selecting `connectors` must highlight all three;
   selecting `ic_4` must highlight all three of its bodies. A single-node highlight is a bug.
2. **One body can stand for several real devices.** `component1` is *one* gold body representing
   2 × FCBGA + 2 × 4 GB flash. The mock CAD is a stand-in for the Bosch unit (§8), and the
   passport describes the Bosch unit.
3. **The screws belong to the TEARDOWN, not to Component ID.** `fasteners` is a
   `board_material`, and Component ID lists parts only — so a screw has no passport page to open.
   In the model link they should be non-selectable; in `04b` they are the whole point.

⚠ **Two open discrepancies, both harmless to the UI and both citable in the thesis:**

* **Screw count.** §8 and the CAD both say **14**; the BOM row says **~12 × M3**, 12.000 g.
* **Colour.** §8 describes the *printed* prototype (bottom brown, upper yellow, connectors green);
  the *CAD* has both housings grey and the connectors near-black. Participants see the printed
  object and the on-screen model at the same time — check on device that this does not read as a
  different product.

## 9. Spec numbering (RB2.1)

| # | Screen | Routine |
|---|---|---|
| `00` | **this file** | — |
| `01` | Welcome page | Open App |
| `02` | Scan QR + scan-failure loop | Open App |
| `03` | Stakeholder direction | Open App |
| `04` | DPP Canva (+ detail pages) | DPP |
| `05` | Digital model exploration | DPP |
| `06` | Disassembly intro | Disassembly |
| `07` | Disassembly steps 1–5 | Disassembly |
| `08` | Summary + report | Disassembly |
| `09` | Tutorial pop-up (**last** — Thiago, 2026-08-04) | all |

Numbers are reserved now so cross-references written today do not need rewriting later. A spec
that does not exist yet is still citable by number.

*Created 2026-08-04 · Supersedes `../RB2_0/00_design_standards.md` · Status: living*
