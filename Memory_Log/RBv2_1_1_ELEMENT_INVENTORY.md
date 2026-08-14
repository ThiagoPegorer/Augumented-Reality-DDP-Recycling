# RBv2.1.1 — Element Inventory (frozen study build)

**Purpose.** One place that answers *"what actually exists in the prototype?"* so that any thesis
chapter — Methodology above all — can be written without re-deriving it, and so that a new Cowork
chat can be brought up to speed in one read.

**Status of the build:** RBv2.1.1 was **officially frozen by Thiago on 2026-08-10**. It is the exact
build the lab study measures (Jin 2026-08-15 15:30 · Neighboors 2026-08-17 ~19:45). Only device-pass
bug fixes are permitted, each producing a new version number.

**Provenance rule.** Every statement below is traced to a project-memory file, named in the
`source:` line of its section. Nothing here is inferred. Where a fact is uncertain or was never
verified on device, it is marked **[unverified]**. Numbers that feed the thesis carry their own
source file.

**This document is a SNAPSHOT, not the source of truth.** Project memory is authoritative. See
`README.md` in this folder for the sync rule.

Last rebuilt: 2026-08-14.

---

## 1. What the thing is

*source: `prototype_concept.md`, `rbv2-1-1-handoff.md`, `thesis_identity.md`*

**ReBuilt v2.1.1** — an AR Digital Product Passport for a vehicle control unit, delivered on a
head-mounted display, that guides a non-expert dismantler through a five-step teardown while
exposing the unit's passport data.

Three layers make up the prototype:

| Layer | What it is |
|---|---|
| Physical | A 3D-printed polymer replica of a VCU, printed at home on a Bambu Lab P2S in PETG |
| AR | Unity 6.0 LTS application on a PICO 4 Ultra in colour passthrough (MR), entered by QR scan |
| Data | A FastAPI backend serving a CIRPASS-aligned DPP JSON payload per product ID |

**Control condition for the user study:** a conventional 2D paper-style manual
(`Docs/VCU_2D_manual_v1.pdf`).

**Two disclosures that are now written into Chapter 1** *(source: Notion Session 30, 2026-08-13)*:

1. Participants dismantle a **3D-printed replica**, not a real VCU.
2. The reference device is a **Bosch Motorsport MS 50.4** because no series-production VCU
   datasheet could be obtained.

---

## 2. Locked technology decisions

*source: `prototype_concept.md`, `pico_xr_loader_choice.md`, `unity_state.md`*

| Layer | Decision |
|---|---|
| Engine | Unity 6.0 LTS (6000.0.x) |
| Headset | PICO 4 Ultra (personal device since 2026-07-16; earlier validation was on a university PICO 4) |
| XR loader | **PICO native `PXR_Loader`, NOT Unity OpenXR.** Scripts gated `#if !PICO_OPENXR_SDK` |
| SDK | PICO Unity Integration SDK 3.4 |
| Passthrough | `EnablePassthrough.cs` on Main Camera; `PXR_Manager.EnableVideoSeeThrough` is a **property** in SDK 3.4, not a method |
| QR | ZXing.Net (Vuforia dropped) |
| QR payload | custom URI scheme `dpp:<product_id>` |
| AR anchor | one-shot world anchor on QR detection (not continuous — the QR gets occluded during teardown) |
| JSON | Newtonsoft.Json (Unity's `JsonUtility` lacks nullable support) |
| Animation | DOTween |
| Backend | FastAPI (Python), JSON files on disk, no database |
| CAD → Unity | NX 2206 → STEP (AP242) → glTF via FreeCAD → Unity via **glTFast** (`com.unity.cloud.gltfast`) |

**Consequence worth naming in Methodology:** because the project runs on the PICO native loader, the
XRI Hands prefab's OpenXR `pinch_ext` bindings do not fire. Hand input had to be built as a custom
bridge (§7).

---

## 3. The physical artifact

*source: `cad_prototype_scope.md`*

A **generic VCU device inspired by** the Bosch MS 50.4 — explicitly **not a replica** of it, and
deliberately not matched to the datasheet envelope (no access to the real unit). As-built record:
`AR_DPP/CAD_Specs/UNITY FILES/VCU_AsBuilt_Design_Record.md` v3.0.

| Part | Geometry |
|---|---|
| `housing_bottom` | 200 × 150 × 45 mm, wall 5, R10 corners; 14 insert holes Ø4.0 + 0.5×45° chamfer; 3× Ø24 connector bores; 6 connector screw holes THRU |
| `housing_upper` (lid) | 200 × 150 × 15, R10/R5; 4× Ø4.2 clearance + counterbores; locating lip 189 × 139 (0.5 mm/side) |
| `connector` ×3 | length 43 mm; body Ø23.4 → bore Ø24.5; flange 2× Ø4.0 at ±20; collar Ø26 |
| `pcb` | 170 × 120 × 2, R5, 4× Ø4.2 mounts, chip pockets 0.5 mm clearance |
| chips ×6 | comp1 70×40 · comp2 50×30 · comp3 60×40 · comp4 40×10 ×3 |
| Fasteners | 14 M3 heat-set inserts + 14 M3 socket-cap bolts; **Allen key 2.5 mm** |

**Print process:** PETG on Bambu Lab P2S, 0.4 nozzle, 0.2 layer, 15 % infill with 4 wall loops, no
supports, brim on the two large flats. Heat-set inserts at 250–260 °C, rested not pressed.
`housing_bottom` printed in **4 h 27 m actual** against a ~12 h estimate.

**Insert-hole diameter was determined experimentally, not assumed:** coupon v1 at Ø3.8 was too tight
(insert set crooked under heavy pressure); coupon v2 at Ø4.0 passed. This is a defensible
methodology detail.

**Real-life colours applied in Unity to match the printed part:** `housing_bottom` #8a5a3b brown ·
`housing_upper` #f2c11e Bambu yellow · connectors + pcb #2e7d4f green. Connector colour-coding
(LIFE/SENS-A/SENS-B red/yellow/blue) is **physical tape only — the digital R/Y/B is dead.**

---

## 4. The reference device (what the passport data describes)

*source: `vcu_bosch_ms504.md`*

**Bosch Motorsport VCU MS 50.4**, data sheet 245099915, order no. F02U.V02.965-02.

- 166 × 121 × 41 mm, ≤ 660 g, IP67, operating −20 to 80 °C
- 3 motorsport connectors, 198 pins total: LIFE (red) AS018-35PN · SENS-A (yellow) AS018-35PA ·
  SENS-B (blue) AS018-35PB. Accessory: opening tool for shell size 18
- Supply 5–18 V; 20 analog channels; 8+4 digital PWM inputs; 4 thermocouple
- 2× 667 MHz dual-core processors (one vehicle control, one logging); internal logger
  1,500 channels, 2× 4 GB partitions
- 3× Ethernet 100 Mbit, 4 CAN, 1 LIN, 1 USB, 1 RS232
- Maintenance interval 220 h or 2 years

**No battery** — the unit is externally powered; the "battery voltage" line measures external supply.
**No heat sink** — the IP67 aluminium case is the thermal mass. **No documented hazard**, which is
why the build carries no safety-banner step.

⚠ The datasheet contains **no teardown and no BOM**, so the internal component breakdown is a
representative model. This is a stated limitation, not an oversight.

**The five-step teardown plan is datasheet-grounded:**

1. Open the aluminium housing (access)
2. Remove the 3 connectors + USB (access + value; gold pins)
3. Lift out the main PCB (structural; carries 2 processors, 4 GB memory, yaw-rate/IMU sensor)
4. Recover high-value silicon: 2 processors + 4 GB memory — separate, do not shred
5. Sort the aluminium housing to the aluminium fraction

On-board sensors are **not** separate steps; they are grouped with the PCB and called out.

---

## 5. Screen inventory — the frozen RBv2.1.1 journey

*source: `rbv2-1-1-handoff.md`, `device-round1-fixes.md`, `rb2_1_dpp_page.md`, `rebuilt_v2_user_journey.md`*

```
LAUNCH → Welcome → QR scan → (scan OK?) → Stakeholder decision → Passport (Super Panel)
                                                                        ↓ gated CTA
                                                            Guided disassembly mode
                                                                        ↓
                                                            Summary → send report → Quit / Close app
```

### 5.1 Welcome

Entry and universal return target. `Close app` (destructive red) calls the real
`Application.Quit`. This is the **only** place the app actually quits — elsewhere "Quit" ends the
*session*, not the process, so a kiosk loop does not end the study for everyone queued behind the
current participant.

### 5.2 QR scan

`QRScanController` state machine, ZXing. Entry is **QR-only** — the user-facing demo fallback was
deleted; `scanOnStart` survives as an operator kill-switch. `waitForWelcome` is set by the Welcome
builder, which is why the QR builder must run before the Welcome builder.

⚠ **Network constraint that affects study logistics:** university Wi-Fi uses AP isolation and blocks
PICO↔PC. **A personal hotspot is required for lab studies.** `DPPClient.baseUrl` must match the
active network's PC IP.

### 5.3 Stakeholder decision

Two role cards. Sets `ScreenRouter.Mode`, which drives the role matrix in §5.4. `Quit` (red) returns
to Welcome.

### 5.4 Passport face — the "Super Panel"

**Four data tabs in a 2 × 2 grid** (290 × 118 each), plus a red sequential certificates tab:

| # | Tab | Content |
|---|---|---|
| 1 | Product specifications | Mechanical + electrical + compliance; Component ID sub-tab |
| 2 | Usage history | Three lenses deriving SOH 48 % and reuse fraction 0.767 (§9.2) |
| 3 | Environmental impact | Four sub-tabs, real per-stage LCA (§9.1) |
| 4 | Certificates & safety | Red, sequential; promoted from a modal to a full screen |

**Role matrix** (`ScreenRouter.Mode`):

| | Product user | Recycler |
|---|---|---|
| Header arrow | shown → stakeholder | hidden |
| Left button | `Quit` (RED) → Welcome | `Back` (grey) → stakeholder |
| Primary | `Next` | `Next` |
| Tab 4 | shown | shown |

Both roles read `Next` on page primaries — the product user's earlier `Scan next product` wrongly
restarted the scan and was corrected in a device round.

**The rail CTA is gated:** it turns green only when all four tabs have been visited. The recycler's
CTA enters guided disassembly; the product user gets Back-to-stakeholders instead.

**Bottom-bar geometry is standardised across all four data tabs** (Back cx 69 / w 90 · Next cx 321 /
w 150 · cy 402 · 11 pt) after Thiago noticed buttons jumping between tabs.

**Compliance badge:** header-right, 200 × 30, reads `CE · REACH · WEEE 5 · IP67`. Red here means
*regulatory or safety marking* — outline and glyph only, never fill.

### 5.5 Guided disassembly mode

*spec `DPP_UI_Specs/RB2_1_1/10_disassembly_mode.md` v1.2*

The CTA **swaps the rig's rail and data in place** — it is a mode of the Super Panel, not a separate
screen set.

- **Rail:** 7 entries (Intro · steps 1–5 · Summary), sequential unlock, back-surfing allowed,
  `Quit` on step 1 (modal → Intro, run and timer reset). The Summary locks backward navigation.
- **Data pane:** briefing page → per-step task pages (the whole ROW toggles; the status circle is
  named `CircleFill`) → summary table (reuses `CompletionSummaryView` + report send).
- **Timer is invisible until the Summary.**
- **Post-report modal:** `Quit` (grey) → Welcome, with the rig hidden explicitly ·
  `Close app` (solid red) → `WelcomeController.CloseApp`.

**Step-5 texts are deliberately material-neutral:** "Separate the bottom shell" / "Remove the QR code
sticker". The instructions address the PETG object in the participant's hands; the aluminium truth
stays in the passport data, which describes the Bosch unit. ⚠ The Intro list still says
"(HPDC aluminium)" — left by decision, one-string change if participants trip on it.

---

## 6. The 3D model and its exploded behaviours

*source: `unity_state.md`, `exploded_zone.md`, `ui_phase3_disassembly_intro.md`, `ui_phase4_step_flow.md`, `device-round1-fixes.md`*

### 6.1 The model asset

`VCU_assembly.gltf` (+ `.bin`), imported via glTFast, in `MainScene` as GameObject **`VCU_assembly`**.
Real scale ~0.2 m, glTF **Y-up**, 7 materials/colours preserved.

Child part names, used by every script that touches the model:

```
housing_bottom · housing_upper
connector / connector001 / connector002
pcb
component1..4 + component001 / component002      (6 chips)
screws_housing*   (4 lid screws)
screws_connector* (6)
PCB_screw*        (4)
```

### 6.2 `DisassemblyAnimator.cs`

Attached to `VCU_assembly`. Auto-finds parts by name prefix. Five-step / two-task teardown matching
the backend payload. API: `PlayTask(step,task)` · `PlayStep(n)` · `RunStep(n)` ·
`ApplyStepInstant(n)` · `PlayFullTeardown()` · `Reassemble()` · `ResetInstant()` ·
`SetStepFocus(step)` / `ClearFocus()` · `SnapModelHome()`.

**Tuned travel distances (m):** lidScrewRise 0.20 · lidRise 0.14 · connectorScrewDist 0.11 ·
connectorDist 0.09 · pcbScrewRise 0.08 · pcbRise 0.06 · chipRise 0.035 · shellDrop 0.08.

Three debugging findings worth keeping, because they explain why the motions look the way they do:

1. **Connector bore axis is auto-detected from geometry** — the three connectors form a row, the bore
   is horizontal and perpendicular to that row, signed away from the device centre, snapped to the
   dominant axis. The earlier offset-from-centre heuristic returned the *row* direction and the
   connectors slid sideways.
2. **Screw spin could not use `DORotate` with an arbitrary axis** — `WorldAxisAdd`/`LocalAxisAdd`
   take Euler triples, and glTF screw pivots are not on the centreline, so screws orbited or flipped.
   Fixed with one `DOTween.To` per screw, spinning about the bore axis *through the mesh centre*,
   with the pivot offset compensated per frame.
3. **Step 3 lifts the PCB with the chips riding on it**; step 4 pops the chips from that raised
   height; step 5 drops the shell straight down.

### 6.3 ⚠ There are FIVE distinct "exploded" behaviours — do not conflate them

This is the single most confusable part of the prototype.

| # | Name | Where | Behaviour | In RBv2.1.1? |
|---|---|---|---|---|
| A | **Exploded action zone** (v4.5.3) | own 268 × 430 canvas | Two-hand free manipulation: rotation band 5–25 cm, zoom band > 25 cm (absolute dial, 25 cm → 1×, 0.55 → 2×), gesture HUD column, draggable help modal | **Removed from the steps** in RBv2.0 (`ScreenRouter.zoneFollowsExploration`) |
| B | **Teardown preview loop** | Disassembly intro panel | `TeardownPreviewLoop.cs` — explode → hold 1.5 s → reassemble → hold, filmed by `TeardownPreviewCamera` to a RenderTexture on a RawImage | RBv2.0-era intro |
| C | **Per-step how-to loop** | inside the instruction canvas | `StepHowToLoop.cs` — reset → `ApplyStepInstant(1..n-1)` → beat 0.4 s → `RunStep(n)` → hold 1 s → repeat, frameless on navy | RBv2.0-era steps |
| D | **LINKED exploded showcase** | passport model panel | Permanently exploded display model, yaw 25°/205°, idle spin 30°/s, select-only ghost at alpha 0.30, orange/green padlock | ✅ **frozen build** |
| E | **Per-step entry explode** | guided mode | On step open, that step's parts EXPLODE OUT and stay out for the whole step; they vanish on the *jump* to the next step. Ticking a task never touches the model | ✅ **frozen build** |

**The passport model panel has two scenarios**, toggled by padlock:

- **LINKED** — exploded showcase, idle spin, ghosting on select, model picks drive the data panel
- **FREE** — reassembled, upright (yaw 0°/180°), **real size 1:1**, 2× zoom available

**Model picks are inert outside Product specs and Usage** — a pick only does something where the
data panel has an answer for it.

**During guided-mode steps there is no spin and no padlock.** Removed parts are hidden, the rest are
ghosted.

⚠ **Timing note that belongs in Results:** each step's recorded split includes revisit time **and**
that step's ~2.5 s entry animation.

---

## 7. Interaction inventory

*source: `pinch_gesture_implementation.md`, `gesture_ux_polish.md`, `exploded_zone.md`, `device-round1-fixes.md`*

### 7.1 Hand input

PICO Building Block "PICO Hand Tracking" supplies `HandLeft`/`HandRight` prefabs (`PXR_Hand`, mesh,
`RayPose`, `DefaultRay`) — the same components PICO Home uses, so the ray *looks* native.

`PicoHandUIBridge.cs` reads `PXR_Hand.Pinch` and `RayPose.forward` each frame and, on the rising edge
of a pinch, fires `pointerClickHandler` through the canvas hit test. It also raises `OnPinch3D` for
3D collider hits.

**Two bugs had to be fixed before it worked at all, both instructive:**

1. Projecting the ray's far endpoint through `Camera.main` to get a screen pixel put the point far
   outside the frustum, so `RaycastAll` returned nothing. **Fix:** intersect the ray with each
   world-space canvas *plane* directly and test each graphic's rect. No camera involved.
2. A decorative full-panel `Background` image with `raycastTarget = true` was winning the topmost
   hit and swallowing every click. **Fix:** skip graphics whose hierarchy contains no
   `IPointerClickHandler`.

⚠ **THE PARALLAX / ON-PLANE LAW (hit at least three times).** The bridge resolves a click against a
**canvas plane**, so *any* overlay sharing a plane with live controls will double-resolve and the
element behind can win. Travelling or floating UI must be its **own nested world-space Canvas with
its own GraphicRaycaster**, and tiny floaters need ≥ 50 px invisible hit areas. This is what caused
the certificates modal's Close button to fire "Continue to disassembly".

### 7.2 Reticles and panel handling

Per-hand pointer dot, always visible when the ray is valid: idle (faint) → hover (white) → pinch
(gold flash, 0.18 s). Panels carry a **grabber bar** (~200 × 22 pill, docked ~12 px below the panel)
that the user pinch-drags to move the whole canvas; `PanelGrabHandle.FaceCamera()` keeps the panel
billboarded so it does not go edge-on when dragged aside. `RecenterInFrontOfUser()` runs once at
startup, placing the panel 0.7 m ahead and 0.1 m below eye level.

### 7.3 Two-hand model manipulation (behaviour A)

Both hands pinching: **5–25 cm separation = rotation band** (twist → yaw, zoom frozen);
**> 25 cm = zoom band**, where separation is an absolute dial. 1 cm hysteresis, glide response 5,
minimum hand separation 0.05.

⚠ `PXR_Hand.transform` is **static** — use the `RayPose` child.

### 7.4 Rejected interaction designs — do not re-propose

Free arcball · arc knobs (both variants) · zoom slider · `+` part list · axis arrows · hover
brighten. Root causes: pointer precision against small targets, controls overlapping the model, too
many mechanisms at once. **v2.x died on trying to pick 5 mm screws by ray.** The v4.x design wins
because manipulation needs no on-screen chrome and no aim precision.

**This is a genuine finding, not a build note** — it belongs in Methodology as design rationale.

---

## 8. Data layer

*source: `backend_state.md`, `dpp_data_model_cirpass.md`, `dpp_payload_v07_bom_reconciliation.md`, `usage_history_data.md`*

### 8.1 Backend

FastAPI. Endpoints: `GET /` health · `GET /dpp` list product IDs · `GET /dpp/{product_id}` ·
report POST. Validation by Pydantic models in `models.py`.

**Source of truth is `backend/models.py`.** `schema/dpp_schema.json` is **generated** — run
`python export_schema.py` after any model change. `Assets/Scripts/DDP/DPPModels.cs` is a hand-kept
C# mirror; field parity has been checked and passes.

⚠ **Two recurring gotchas.** `response_model=DPP` silently *strips* fields a stale running Pydantic
model does not know about — **always fully restart uvicorn after `models.py` changes.** And the
backend caches: if step-5 texts do not appear, restart FastAPI.

**Payload version at freeze: v0.19**, written to both copies (XR and root backend).

### 8.2 Schema shape

Grounded in **CIRPASS D2.2 Table 6** (generic electronics information requirements, 22 attributes:
13 mandatory by legislation, 9 used by DPP initiatives). Full transcription and prototype coverage:
`DPP_UI_Specs/13b_information_model.md`.

Blocks: `dpp_meta` · `identity` · `components[]` (with `material_breakdown`) · `documents[]` ·
`substances_of_concern[]` · `service` · `usage_history` · `unit_use_phase` · `repair_history` ·
`indicators` · `certifications[]` · `environmental` · `disassembly` · `end_of_life`.

**Honest-labelling vocabularies — the UI switches on these:**
`basis`: `declared | datasheet | measured | assumed | modelled | not_provided`
`status`: `available | not_provided | not_applicable`

**The reframing worth citing:** Table 6 items #1 and #2 come from the Energy Labelling Regulation
(EU) 2017/1369, which covers labelled product groups only. An automotive control unit is not one, so
the correct passport value is **`not_applicable` with a reason**, not an empty mandatory field.
Remaining genuine gaps: #12 use/repair information and #16 instructions for safe use.

**The core design argument:** Table 6 #5 (location of dangerous substances) and #6 (substances of
concern: name, *location within the product*, concentration) cannot be satisfied by a flat table.
The high-value passport data is **spatial and per-component**. That is the strongest argument for AR
over a paper WEEE sheet.

⚠ **And the prototype cannot currently demonstrate it.** `substances_of_concern` is `[]` and
`end_of_life.substances_basis` is `not_provided`, deliberately — populating it without a source
would be invented data in a study instrument. **Consequence: the per-part hazard highlighting, the
single most defensible AR-over-paper capability, has nothing to render.** Similarly
`disassembly.safety_warnings` is `[]`. Both are Thiago's fabrication calls, both still open.

### 8.3 Component set

BOM_v4 has 17 components; the payload keeps **11 IDs** because `Step.component_ids`, the
completion-summary mass table and the recovery report all reference them.

| id | g | step | id | g | step |
|---|---|---|---|---|---|
| housing | 344.0 | 5 | passives | 47.5 | 3 |
| connectors | 150.1 | 2 | actives | 23.7 | 4 |
| pcb_substrate | 49.0 | 3 | pcb_copper | 14.0 | 3 |
| fasteners | 12.0 | 1 | tim | 8.0 | 3 |
| misc | 5.0 | 5 | solder | 3.9 | 3 |
| coating | 3.0 | 3 | **total** | **660.2** | |

⚠ **The prototype was showing wrong precious-metal figures until 2026-07-30.** Corrected against
`VCU_BOM_v4.xlsx`:

| Metal | was shown | corrected | error |
|---|---|---|---|
| Gold | 62.7 mg | **91.7 mg** | 1.46× too low |
| Silver | 250.8 mg | **59.3 mg** | 4.2× too high |
| Palladium | 27.6 mg | **5.5 mg** | 5.0× too high |
| Tantalum | 82.5 mg | **900 mg** | 10.9× too low |
| Nickel | 2475 mg | **500 mg** | 5.0× too high |

**Any screenshot or result taken before 2026-07-30 carries the old figures.**

⚠ **Two known data disputes to resolve before writing Results:** the "14 screws vs ~12 in the BOM"
reconciliation (the fasteners row was renamed `14 × M3`; the count a dismantler physically verifies
won, mass untouched), and tin at 3.9 vs 4.6 g.

---

## 9. Derived figures the prototype displays

### 9.1 Environmental impact

*source: `dpp_payload_v07_bom_reconciliation.md`, `lca_findings_for_writing.md`, `lca_scope_verified.md`*

openLCA, EF 3.1. Headline `environmental.co2_footprint_kg` = **73.4326 kg CO₂-eq** (Sc1 baseline).

| Category | share | Sc1 baseline | Sc2 | Sc3 | Sc4 |
|---|---|---|---|---|---|
| Resource use, minerals and metals | 72.45 % | 0.0187391 kg Sb-eq | −10.0 % | −32.2 % | −47.3 % |
| Climate change | 6.67 % | 73.4326 kg CO₂-eq | −6.0 % | −11.1 % | −21.0 % |
| Eutrophication, freshwater | 6.58 % | 0.11592 kg P-eq | −5.9 % | −16.5 % | −25.7 % |

Scenario labels: Sc2 "Recycling as usual" · Sc3 "Guided dismantling" · Sc4 "Dismantling + reuse".

**Per-stage numbers are real** — `stage_contributions.py` computed S1–S5 by cumulative differencing
against the live openLCA database, with zero drift on 25 checks against the frozen thesis CSVs.
Climate: 38.49 S1 + 34.30 S4 of 73.43. Minerals: 97.8 % S1.

🚨 **Standing constraint from `lca_scope_verified.md`: Sc3 and Sc4 are literature-parameterised, not
measurements from the prototype.** The framework says so itself. Do not let a passport screen or a
thesis sentence imply otherwise.

🚨 **Every LCA headline number is currently marked `[M]` under the zero-trust rule and must be traced
to its named CSV row before the Findings chapter is written.**

### 9.2 Usage history

*source: `usage_history_data.md`*

⚠ **The Usage tab is about the UNIT, not the car.** A car-centric design (distance, energy, a DACH
driving map) was built and then deleted, because a movement profile is personal data a recycler has
no basis to see.

**The result that matters for the thesis:** the mass-weighted reuse fraction is **0.767**
(506.1 g of 660.2 g). Spec 14 §4 declares Sc4's functional reuse yield as `[A]` assumed at 0.5–0.9,
"not quantified in literature". **0.767 sits inside that band — the passport evidences the
assumption the LCA could not source.** This belongs in Results and Discussion.

**Two calculations Thiago will be asked to defend:**

- **Thermal fatigue via Coffin-Manson + Miner**, not a fake cycle budget:
  `N_f(ΔT) = A · ΔT^-n`, `A = 15,000 · 40² = 24,000,000`, `n = 2.0` → `D = 0.5161`, so 48 % remains.
  `n = 2.0` is the low end of the 2–3 range for SnAgCu, so the estimate is conservative.
  ⚠ **Still needs a citation** (IPC-9701 or a reliability text). Do not put the equation in the
  thesis without one.
- **SOH = min(mechanisms), never a weighted mean** — `min(flash 55 %, fatigue 48 %) = 48 %`,
  limiting mechanism thermal fatigue. A part fails at its weakest mechanism, not the mean of them.

---

## 10. Study instrumentation — what the build actually records

*source: `ui_phase5_completion_summary.md`, `rebuilt_v2_scope.md`, `device-round1-fixes.md`*

- **Stopwatch starts at "Start disassembly."** The tutorial and the whole passport phase sit
  *outside* the timer. There is no `prep_time_s`.
- `StepFlowController` records a split at every Confirm (`_stepSplits`), shipped in the recovery
  report as `step_times_s: List[int]`. **This is the user-test dataset.**
- Per-step masses, data-driven by summing components by `disassembly_step`:
  **20 / 58 / 185 / 20 / 378 g = 660**.
- The report is sent by `Send dismantling report`; confirmation reads `Dismantling report sent`.
  `DPPClient.timeoutSeconds = 10` — without it, an unreachable backend froze the screen permanently.

**Not instrumented:** error counts and interaction failures are observational, not logged by the
build. **[unverified — no memory file records an error-logging mechanism.]**

---

## 11. Scope boundaries — what is deliberately NOT in the build

*source: `prototype_concept.md`, `rebuilt_v2_scope.md`, `dpp_data_model_cirpass.md`, `dpp_ux_flow.md`*

These are decisions, not gaps, and each one should appear in Limitations or Future Work:

1. **No Stage 3.** The thesis works only with the 3D-printed model. Real-VCU field work with
   recycling centres is post-thesis. Scenarios needing field data are **modelled, not measured**.
2. **No AI chatbot.** The GPT-4o assistant layer was cut from thesis scope on 2026-07-15.
3. **No computer vision.** Highlighting happens on the CAD model, not on the physical object in
   passthrough. Real-world component recognition is the stated dream goal and future work.
4. **No per-part hazard highlighting rendered**, because `substances_of_concern` is empty by choice
   (§8.2). The capability exists in the schema; the data does not.
5. **No safety-banner step** — the MS 50.4 has no documented hazard and participants handle an
   unpowered polymer object.
6. **The exploded action zone was removed from the disassembly steps** (behaviour A in §6.3). Spec 00
   §9's three-layer learning model therefore loses layer 3 *during the task*; layer 2 remains.
   **This is a named cost, already flagged for Limitations.**
7. **Tutorial scope is pinch + pinch-drag only.** Model-manipulation gestures are deliberately not
   taught there — the zone's `?` modal is the only place they are taught.
8. **Entry is QR-only.** This removed spec 11 §5's single-point-of-failure fallback.
9. **No re-entry to the tutorial** once "FIRST TIME?" is answered NO.

---

## 12. Version lineage

*source: `rebuilt_v2_scope.md`, `rb2_1_scope.md`, `rbv2_menu_and_cleanup.md`, `rbv2-1-1-handoff.md`*

| Version | What it was | Who saw it |
|---|---|---|
| RBv1.0 | Two top-level tabs (Informations / Disassembly), main page, accordion info tab | early tests |
| **RBv2.0** | Origin: AR-supervisor feedback from **Ms. Elle Langer, 2026-07-29**, refined into Miro user-journey diagram v4. Tab bar removed, single linear path, Welcome + QR + first-run, DPP Canva + model exploration + gate | **P02 (Waldek, Jul 31)** and **P03 (Domenik, Aug 1)** |
| RB2.1 | Driven by two P02/P03 findings: *"too much information — what do I do with it?"* and *"I didn't perceive there was a 3D model there."* DPP Canva and model exploration **merge into one panel screen**; model is free-floating and persistent; 2 × 2 tab grid | — |
| **RBv2.1.1** | Component ID on EF 3.1 impact, CAD↔passport map, Usage & service, Environmental impact with real per-stage LCA, gated rail, guided disassembly mode. **FROZEN 2026-08-10** | Jin 08-15, Neighboors 08-17 |

**Versioning ruling (Thiago, 2026-08-04): a new prototype version per participant test is
intentional.** Cross-participant comparability is not to be raised again.

**Unity menu at freeze:** one tab `RBv2_1_1`, items 01–14 + Tools + Legacy, ordered so that
top-to-bottom is the safe full rebuild. Canonical map: `DPP_UI_Specs/RB2_1_1/09_build_menu.md`.

🚨 **Run-order trap.** Routine chain is strictly ascending: **09 → 10 → 11 → 12 → 13 → 14 → Verify →
SAVE.** Teardown builders (05/06) bind `FindFirstObjectByType<DisassemblyAnimator>` (active-only) and
layer-hide — **never run them while the stage clone exists.** If they must run: delete
`DppSuperPanel` first, then rebuild 10→14 ascending.

---

## 13. Open items at freeze

*source: `rbv2-1-1-handoff.md`, `device-round1-fixes.md`, `writing_phase_setup.md`*

1. **Pre-study freeze drill** — verify the flashed build contains the vanish-on-jump fix and the
   step-5 texts. If not, one final build, and record its version number in the study protocol.
2. **Round-3 device build** pending; checklist is spec 10 §7.
3. Commit round 1+2 files (controller ×2, animator, `SuperPanelView`, builder Disassembly, spec 10
   v1.2) — **[unverified: may have been done since 2026-08-10].**
4. Notion session-log backfill for 2026-08-02/03.
5. Delete `_to_delete_outside_assets/retirement_20260809/`.
6. Every LCA headline number traced to its named CSV row before Findings is written.

---

## 14. Standing rules that survive the freeze

*source: `rbv2-1-1-handoff.md`, `git_workflow.md`, `working_agreements.md`, `github_repo_setup.md`*

| Rule | Why |
|---|---|
| **Git is Thiago's.** Give commands, never run them | Bridge staging broke twice; `git status` times out over the device bridge and a killed one leaves a stale `.git/index.lock` |
| **Never create `_to_delete/` inside `Assets/`** | Unity compiles it → 139 errors |
| **Never rename runtime MonoBehaviour `.cs` files outside Unity** | Scene components bind by `.meta` GUID |
| **Never `git add .`** — stage your own paths explicitly | Re-opening the project re-touches vendor SDK files with CRLF↔LF churn |
| Bosch PDFs and participant data never enter the repo | |
| Mock → approval → code for new screens; corrections go straight to code | |
| After any session break, verify staged files against round history | Trap 7: a stale builder copy silently shipped and resurrected an old design |
| **Never invent a number.** `VCU_BOM_v4.xlsx` + `LCA_framework_v4.md` are materials truth; `LCA_Analysis/Outputs/` CSVs are impact truth | |
