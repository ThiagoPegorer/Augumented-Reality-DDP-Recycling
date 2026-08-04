# DPP UI Spec — Screen 3: Disassembly Intro

> Page-level design specification for the AR Digital Product Passport (VCU).
> Source of truth for the Unity implementation of the Disassembly **intro / briefing** screen.
> Shares the global palette, typography and hover rule from `00_design_standards.md` / `01_main_page.md`.
> NOTE: this is the **pre-flight briefing** only. The per-step disassembly screens (the
> two-canvas instruction + exploded-view pair) are documented separately in `04_…` onward.

> **Revision 2026-07-10 (v3, approved & Editor-tested):** unboxed job overview,
> "Dismantling" part list, and a **live 3D teardown loop** replacing the static PNG.
> Approved mock: `drafts/03_intro_v3.svg`. Implemented in `DPPUIBuilder.Intro.cs`
> (rebuild-safe: only DisassemblyIntro + TeardownPreviewCamera are recreated).
> The v2 split layout (2×2 stat cards, static AI render) is superseded — kept in
> git history only.

---

## 1. Purpose & context

The Disassembly Intro is the **pre-flight briefing** shown when the worker enters the Disassembly tab, *before* the guided step-by-step flow begins.

- **User:** non-expert WEEE dismantling worker (gloved, time-pressured, PICO passthrough AR).
- **Reached from:** Main Page → `Disassembly` card; or the tab bar → `Disassembly`.
- **Exit:** `Start disassembly` → first disassembly step (Screen 04), two-canvas pair.
- **Design principle:** it briefs, it does not instruct. Global facts only (tool, time, scope, what gets separated) plus a **moving preview** of what's inside.
- **v3 rationale:** the v2 stat *cards* read as buttons (worker feedback) — replaced by plain text rows; the "Recover" prize card was dropped with them (the value story lives in the gold accents of steps 2 & 4). The static illustrative render is replaced by the real CAD model animating, which needs no "illustrative" disclaimer.

---

## 2. Layout — split briefing (canvas 680 × 470 reference, panel 640 × 430)

Panel-local coordinates in parentheses (spec x/y − 20).

| Region | x | y | w | h | Notes |
|---|---|---|---|---|---|
| Panel | 20 | 20 | 640 | 430 | Main navy surface, r22 |
| Header zone | 44 | 36 | 592 | 60 | identical to 02 v3 §3.1, Disassembly active |
| Separator | 44→636 | 96 | — | — | 1 px `#1a335f` |
| "Job overview" eyebrow | 44 | ~126 bl | — | — | 12.5 px `#7f9bc4` (local TL 24, 96) |
| Row — Tools | 44 / 132 | ~156 bl | — | 20 | label / value (local TL 24, 124) |
| Row — Est. time | 44 / 132 | ~184 bl | — | 20 | (local TL 24, 152) |
| Row — Scope | 44 / 132 | ~212 bl | — | 20 | (local TL 24, 180) |
| Divider | 44→320 | 232 | 276 | 1.5 | `#1a335f` (local TL 24, 210) |
| "Dismantling" eyebrow | 44 | ~258 bl | — | — | 12.5 px `#7f9bc4` (local TL 24, 226) |
| Dismantling list | 44 / 188 | 268.. | 2×140 | 3×22 | TWO columns of three rows (local x 24 / 168, y 248 + i·22) |
| Teardown preview (RawImage) | ~350..636 | ~106..346 | 242 | 225 | live RenderTexture, centered at local (468, 205) |
| Preview caption | 488 c | 366 bl | — | — | `Teardown preview`, 11 px `#6f86a8` |
| Start button | 44 | 388 | 592 | 48 | teal primary, unchanged from v2 |
| Grabber bar | centered | below panel | 200 | 22 | per `00` §5 |

---

## 3. Header / tab bar

Identical to the Information tab header (02 v3 §3.1) — home button cx 62 / cy 64 r 20,
tab pills y 46 h 38 at x 150 / 342, separator y 96 — except **Disassembly is the active
tab**. Built by the shared `MakeTabHeader` helper.

| Element | Hover | Action |
|---|---|---|
| Home button | white outline | → Main Page |
| Informations tab | white outline | → Information tab |
| Disassembly tab | white outline | stays (current) |

---

## 4. Safety banner (conditional — NOT shown for this product)

Unchanged from v2 and still not built (no hazards on the VCU model).
`DisassemblyIntroView` logs a warning if the payload ever flags hazards.
Original banner spec: rect x 44 y 106 w 592 h 44 r12, fill `#3a1d22`, stroke `#e24b4a`,
red `!` circle, text `#f3b0b0` 13.5 bold; stats+preview shift down ~56 when shown.

---

## 5. Teardown preview — LIVE 3D loop (v3)

The worker **sees the real device explode and reassemble** before starting.

### 5.1 Implementation

- **`TeardownPreviewLoop.cs`** (component on the DisassemblyIntro screen root):
  while the screen is active it drives `DisassemblyAnimator` (on `VCU_assembly`)
  in an endless cycle — **full teardown → hold 1.5 s → Reassemble → hold 1.5 s**.
  `OnDisable` stops the loop and calls `ResetInstant()`, so the step flow always
  starts from the assembled pose.
- **`TeardownPreviewCamera`** (root-level GameObject, Camera disabled at rest):
  films the model into a runtime **RenderTexture** (2× the slot size, ARGB32,
  MSAA 4, transparent background) shown by the **RawImage** in the old PNG slot.
  Layout, slot position and caption position are unchanged from v2.
- **Auto-framing** (recomputed every time the screen enables): trimetric view
  built from the animator's auto-detected connector bore axis —
  `dir = yaw(yawOffsetDeg) · (front·0.9 + side·0.55) + up·0.55`, aimed at the
  assembled bounds centre **+ frameHeightBias** so the rising lid/screws keep
  headroom and the empty space below the model is used.
- **Tuned values (Editor-tested 2026-07-10):** `yawOffsetDeg 0`, `frameFactor 1.5`,
  `frameHeightBias 0.06`, `holdExploded/holdAssembled 1.5 s`, `fieldOfView 30`.
- Culling note: the camera sees all layers; if scene props ever appear behind the
  model, move `VCU_assembly` to a dedicated layer and set the camera's culling mask.
- Legacy asset `Assets/Textures/Intro/vcu_teardown.png` stays on disk (unused here;
  the Phase 4 builder still loads it for its static preview until Phase 4 goes live-3D).

### 5.2 Motion set (DisassemblyAnimator v2, shared with the step flow)

- **Connector bore axis auto-detected** from geometry: the 3 connectors form a row;
  bore = horizontal ⊥ row, signed away from the device centre, snapped to the
  dominant local axis. (The v1 offset-from-centre heuristic could return the row
  direction — connectors then slid sideways.)
- **Screw spin is pivot-independent:** screws rotate about the bore axis **through
  their own mesh centre** (glTF pivots are not on the centerline — spinning about
  the pivot made screws orbit/flip). Spin + eased back-out run in ONE tween.
- Step 3 lifts the **PCB with the chips riding on it**; step 4 pops the chips from
  the raised board; step 5 drops the bottom shell straight **down**.
- **Tuned distances (m):** lidScrewRise 0.20 · lidRise 0.14 · connectorScrewDist 0.11 ·
  connectorDist 0.09 · pcbScrewRise 0.08 · pcbRise 0.06 · chipRise 0.035 · shellDrop 0.08.

---

## 6. Job overview + Dismantling (left half, v3 — unboxed)

No cards, no strokes — plain text on the panel (cards read as buttons).

**Job overview rows** — label 12.5 px `#8ba3c4` at x44; value **14 px bold white** at x132:

| Row | Value (vcu_001) | Data source |
|---|---|---|
| Tools | `Allen key (hex 2.5 mm)` | `disassembly.tools[]` joined with " · " |
| Est. time | `~5 min` | `disassembly.estimated_time_min` |
| Scope | `5 steps` | `disassembly.total_steps` (no parts sub-line) |

**Dismantling list** — teal 6 px dot bullets, labels **same style as the values**
(14 px bold white). TWO columns of three rows (index 0–2 col 1, 3–5 col 2):

| Column 1 | Column 2 |
|---|---|
| VCU case | 3 chips |
| PCB board | 3 connectors |
| 3 processors | 14 screws |

Bound to **`disassembly.parts[]`** (backend, display strings) — the **physical part
groups**, deliberately distinct from the material-based `components[]` (12 entries).
Rows beyond `parts.length` are hidden; extras beyond the built 6 log a warning.

---

## 7. Start button

Unchanged from v2: x 44, y 388, w 592, h 48, r 14, fill `#1d9e75`,
label `Start disassembly` white 16 bold + white `›` chevron, white hover outline.
Action → Screen 04 step flow (two-canvas split).

---

## 8. Transition to the step flow (architecture note, unchanged)

Pressing `Start disassembly` triggers the two-canvas spatial split (instruction canvas +
268 × 430 exploded-view canvas). With the live loop, the intro preview now literally
shows the same model that the step flow animates — `TeardownPreviewLoop.OnDisable`
resets it to assembled before the handover.

---

## 9. Color tokens (v3 delta)

| Token | Hex | Usage |
|---|---|---|
| `overview/label` | `#8ba3c4` | Row labels (Tools / Est. time / Scope) |
| `overview/value` | `#ffffff` (bold 14) | Row values AND dismantling labels |
| `list/dot` | `#1d9e75` | Dismantling bullets |
| `divider` | `#1a335f` | Line between overview and dismantling |
| `cta/fill` | `#1d9e75` | Start button |
| `caption` | `#6f86a8` | Preview caption |

Retired in v3: `stat/fill` `#0e2950`, `stat/recover-fill` `#0e2335`, recover accent set.

---

## 10. Data bindings (backend v0.5+)

| UI element | Source (DPP payload) | Fallback |
|---|---|---|
| Safety warning (log only) | `end_of_life.contains_battery`, `hazardous_warnings`, `components[].hazardous` | — |
| Tools row | `disassembly.tools[]` joined " · " | baked demo text |
| Est. time row | `disassembly.estimated_time_min` → `~{n} min` | baked |
| Scope row | `disassembly.total_steps` → `{n} steps` | baked |
| Dismantling list | `disassembly.parts[]` (display strings) | baked; missing entries hide rows |
| Teardown preview | live render of `VCU_assembly` via `TeardownPreviewLoop` | RawImage stays disabled if wiring missing |

> **Schema additions (2026-07-10):** `disassembly.parts: string[]` in `models.py` +
> `DPPModels.cs`. Backend content: `tools = ["Allen key (hex 2.5 mm)"]` (2.5 mm
> confirmed — M3 socket-cap), `estimated_time_min = 5`, all `steps[].tool` updated,
> step 4 subtitle "3 processors + 3 chips in the board pockets".
> `schema/dpp_schema.json` not yet updated (known drift).

---

## 11. Behaviour notes

- Briefing only — no step instructions here.
- Preview is non-interactive; it becomes interactive only after Start, on its own canvas.
- Camera re-frames on screen enable — after tuning Inspector values in Play mode,
  re-enter the tab (Informations → Disassembly) to apply.
- One primary action (Start); home + tab bar are the only other navigation.
- Grabber bar below the panel (`00` §5).

## 12. Open items / future

- [ ] Move `VCU_assembly` to a dedicated layer for the preview camera if passthrough props ever show up behind it.
- [ ] Update `schema/dpp_schema.json` to v0.5 (parts[]).
- [ ] Consider re-introducing a compact value cue ("what's worth recovering") if supervisors miss the Recover card.
- [ ] Loading state between tab entry and render.

---

## 13. Approved SVG mockup (v3.1, 2026-07-10)

> Source file: `DPP_UI_Specs/drafts/03_intro_v3.svg`. The device drawing in the right
> half is a vector stand-in — Unity renders the real `VCU_assembly` RenderTexture.

*(see draft file; embedded copy omitted — the live RT preview cannot be represented faithfully in SVG)*

---

*Last updated: 2026-07-10 · Status: v3 approved & Editor-tested · Prev: 02 Information tab · Next: 04 Disassembly step (two-canvas pair)*
