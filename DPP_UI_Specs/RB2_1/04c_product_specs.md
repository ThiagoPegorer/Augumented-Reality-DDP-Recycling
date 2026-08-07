# DPP UI Spec — 04c: Product specifications tab (super panel, RB2.1)

> **Living spec** — ReBuilt v2.1. Status: **SPECCED 2026-08-06, not yet coded.**
> Parent: `04_DPP_page.md` (the 980 × 430 super panel). Siblings: `04a_use_phase.md`,
> `04b` (training disassembly, to write), `04d` (environmental impact, to write).
> Mocks: `drafts/04c_v1_product_specs.svg` (first read of the Miro routine),
> `drafts/04c_v2_product_specs.svg` (real NX sheets introduced),
> **`drafts/04c_v3_product_specs.svg` (approved 2026-08-06)**.
> Data: `VCU_BOM_v4.xlsx` → `By_Component`; narrative in `LCA_Analysis/Docs/BOM_v4.md`.

---

## 1. What this tab is, and what it is not

Product specifications is the **first tab** in the rail and, for the Recycler, the **entry
screen** of the walkthrough (`04` §6). It answers two questions and nothing else:

1. *What is this thing?* → the **Identity** sub-tab.
2. *What is it made of, and where is each material?* → the **Mechanical & electrical** sub-tab.

It is **not** the impact page (that is 04d) and **not** the dismantling instruction (04b). Where
a component's disassembly step is shown here it is a cross-reference, not an instruction.

### 1.1 The two bodies — resolved 2026-08-06

The project holds two different physical objects and RB2.0 mixed them:

| | Bosch MS 50.4 (the product) | NX demonstrator (the AR mock) |
|---|---|---|
| Size | **166 × 121 × 41 mm** `[D]` sheet 245099915 | 200 × 150 × 60 mm |
| Mass | **660 g** `[D]` | ≈1.15 kg if cast at drawn wall thickness |
| Role | **everything the passport declares** | the 3D model in the stage, and the source of the 2D sheets |

**Ruling (Thiago, 2026-08-06):** *"the passport will describe the Bosch product using the
prototype as mock of the CAD and real physical Bosch VCU."* So:

- every declared value — size, mass, materials, LCA — is the **Bosch product**;
- the NX sheets contribute **geometry and a shaded view only**; their titles, title blocks and
  sheet labels are stripped and never shown;
- `specifications.size_mm` moves **200 × 150 × 60 → 166 × 121 × 41** (payload change, §6).

⚠ This retires the RB2.0 defect where the demonstrator's measured size sat in the product block
next to the product's modelled mass — a mix `00` §8 forbids.

## 2. Source of truth

**`VCU_BOM_v4.xlsx` / sheet `By_Component`.** Not the .md, not the payload, not the CAD.
The tab reads masses and materials from the BOM and nothing else invents chemistry.

Four BOM internal disagreements, recorded so they are not silently absorbed:

| | `BOM_v4.md` | `VCU_BOM_v4.xlsx` `By_Component` | used here |
|---|---|---|---|
| Processors 2× FCBGA | epoxy 3.65 · Cu 1.85 · Si 0.90 · Sn 0.60 | epoxy 3.50 · Cu 1.70 · Si 0.90 · Sn 0.60 · **other 0.30** | **xlsx** |
| Passives/misc on-board | 44.9 g | **44.9965 g** | **xlsx** |
| **Tin (solder), device total** | Table 2: **3.9 g** — "Sn 3.2 (joints) + 0.7 g in package balls #5/6 → board Sn total 3.9" | `Solder joints` **3.90** *and* FCBGA 0.60 + flash 0.10 → **4.60 g** | **xlsx, 4.60 g** |
| Device total | "closes at 660 g ✔" | **660.1565 g** | **660.1565 g** |

The xlsx wins because it is machine-readable and is what the payload binds to.

⚠ **The Tin row is not a rounding difference — it is a 0.7 g double-count.** The .md's Table 1
row 14 is `3.2 g` of joints and says the package balls are counted inside rows 5/6, giving a
device total of 3.9 g. The xlsx's `Solder joints` row carries `3.90` — the *total* — while rows
5 and 6 still carry their own 0.70 g, so the device total reaches 4.60 g. One of the two is
wrong. **Decide which before the Results chapter**; 0.7 g of Sn is small in mass but tin has a
non-trivial characterisation factor and it appears in the recovery figures.

**Second action:** state 660.1565 g (or 660.16), or move the excess into the
`Labels/adhesives/misc` balancing item and make the .md follow. Do not keep a stated closure
check that does not hold.

## 3. The part list — 8 parts + 7 board materials

RB2.0 showed 11 flat components. RB2.1 splits them into **what a recycler can pick up (parts,
each with an NX drawing)** and **what is only a material on the board (no drawing)**. The split
is a refinement of the 11, not a new inventory: every mass below traces to a BOM row.

### 3.1 Parts (drawing available)

| # | Part | Mass g | Material breakdown (g) | BOM rows | Step |
|---|---|---|---|---|---|
| 1 | Upper housing | **108.4757** | Aluminium 108.4757 | 1 (area share) | 5 |
| 2 | Bottom housing | **235.5243** | Aluminium 235.5243 | 1 (area share) | 5 |
| 3 | Bare PCB, 4-layer | **63.00** | glass fibre 29.0 · polymers 20.0 · Cu 14.0 | 3 | 3 |
| 4 | Connector AS018-35 (× 3) | **150.06** | Al 95.0 · Cu 35.0 · polymers 15.0 · silicone 4.5 · Ni 0.5 · **Au 0.06** | 4 | 2 |
| 5 | Processors 2x FCBGA + flash 2x 4 GB | **8.00** | polymers 4.00 · Cu 1.95 · Si 1.05 · Sn 0.70 · other 0.30 | 5 + 6 | 4 |
| 6 | Regulators + analog front-end | **4.20** | polymers 2.20 · Cu 1.60 · Si 0.40 | 9 + 11 | 4 |
| 7 | Power stages 6x (DPAK) | **9.00** | Cu 4.50 · polymers 3.60 · Si 0.90 | 8 | 4 |
| 8 | Comm transceivers + MEMS sensors | **2.50** | polymers 0.85 · Cu 0.70 · ceramics 0.60 · Si 0.35 | 10 + 7 | 4 |
| | **subtotal** | **580.7600** | | | |

Housing masses carry four decimals in the payload because the area split is exact; the UI
rounds to `108.5 g` / `235.5 g`. Do not round in the payload — the closure check depends on it.

### 3.2 Board materials (no drawing — correctly so)

| Item | Mass g | Material breakdown (g) | BOM row |
|---|---|---|---|
| Passives, MLCC, R, L, misc on-board | **44.9965** | ceramics 25.0 · mixed terminations 10.0 · Fe 5.0 · Al 3.3 · polymers 1.6 · **Au 0.0317 · Ag 0.0593 · Pd 0.0055** | 13 |
| Ta capacitors | **2.50** | polymers 1.60 · **Ta 0.90** | 12 |
| Solder joints, SAC305 | **3.90** | Sn 3.90 | 14 |
| Thermal interface material | **8.00** | silicone 8.00 | 15 |
| Conformal coating | **3.00** | polymers 3.00 | 16 |
| Labels, adhesives, misc. | **5.00** | polymers 5.00 | 17 |
| Fasteners (~12 × M3) | **12.00** | steel/Fe 12.00 | 2 |
| | **79.3965** | | |

**Closure: 580.7600 + 79.3965 = 660.1565 g** — identical to the xlsx `DEVICE TOTAL`
(660.156499). The regrouping adds and removes nothing.

**Cross-check against `By_Material`.** The split must not move any material total. All sixteen
verified to 4 decimals at payload write, asserted in the generator:

| | payload | xlsx | | payload | xlsx |
|---|---|---|---|---|---|
| Aluminium | 442.3000 | 442.3000 ✔ | Mixed terminations | 10.0000 | 10.0000 ✔ |
| Copper | 57.7500 | 57.7500 ✔ | Tin (solder) | 4.6000 | 4.6000 ✔ |
| Polymers/epoxy | 56.8500 | 56.8500 ✔ | Silicon | 2.7000 | 2.7000 ✔ |
| Glass fibre | 29.0000 | 29.0000 ✔ | Tantalum | 0.9000 | 0.9000 ✔ |
| Ceramics | 25.6000 | 25.6000 ✔ | Nickel | 0.5000 | 0.5000 ✔ |
| Steel/Fe | 17.0000 | 17.0000 ✔ | Other | 0.3000 | 0.3000 ✔ |
| Silicone | 12.5000 | 12.5000 ✔ | **Gold** | **0.0917** | 0.0917 ✔ |
| | | | Silver / Palladium | 0.0593 / 0.0055 | ✔ / ✔ |

**Gold total 91.7 mg** — 60 mg connector plating + 31.7 mg on-board. This is the number the
Recycler is looking for and it is now traceable to two named sources, not one rounded chip.

### 3.3 The two allocation rules

Both are derivations, not assumptions. They are the only places the 17-row BOM had to be
reshaped to reach the 15-row passport structure.

**(a) Housing → two shells, by surface area.** BOM row 1 derives 344 g from *637 cm² of shell*
× 1.8–2.2 mm × 2.70 g/cm³ `[A] geometry`. For the 166 × 121 × 41 body that 637 cm² decomposes
exactly:

```
lid    16.6 × 12.1                       = 200.86 cm²
tray   16.6 × 12.1 + 2(16.6+12.1) × 4.1  = 436.20 cm²
                                    sum  = 637.06 cm²   ← the BOM's own 637 cm²
```

so the mass split is the BOM's own geometry basis re-read, not a new number:
`108.5 g` / `235.5 g` (200.86 / 436.20 × 344).

**(b) Actives → the four CAD blocks, by footprint.** BOM rows 5–11 are seven entries totalling
23.70 g; the demonstrator carries four IC blocks. Approved by Thiago 2026-08-06:

| CAD block | Displayed name | Footprint | BOM rows | Mass |
|---|---|---|---|---|
| `ic_1` | **Processors 2x FCBGA + flash 2x 4 GB** | 70 × 40 × 5 | 5 + 6 | 8.00 |
| `ic_2` | **Regulators + analog front-end** | 50 × 30 × 5 | 9 + 11 | 4.20 |
| `ic_3` | **Power stages 6x (DPAK)** | 60 × 40 × 5 | 8 | 9.00 |
| `ic_4` | **Comm transceivers + MEMS sensors** | 40 × 10 × 5 | 10 + 7 | 2.50 |
| | | | | **23.70** ✔ |

⚠ **RENAMED 2026-08-06, and this closes open item 2.** They were `Processor 1…4` — the CAD
sheet titles — on the instruction to keep the existing names. On device that produced four
identical-looking rows saying nothing, and the thesis would still have had to explain that a VCU
does not contain four processors. Thiago: *"rename the components to fit the same name of the LCA,
not be just generic Processor 1."*

The names are now the **BOM_v4 row names**, so a reader can move between the passport, the BOM and
the LCA without a lookup table. `ic_1` taking the FCBGAs remains the one non-arbitrary assignment —
FCBGA is the largest package class on the board, so it belongs on the largest footprint. The other
three are a modelling choice and are still declared as such. `represents` continues to name the
underlying BOM entries on the detail page.

## 4. Layout

> ⚠ **§4.1–4.5 are the PRE-BUILD draft.** They were written before the tab was coded and three
> device tests have overtaken them: there is no page title, no SPECS chip row, no board-material
> rows, no iso inset and no zoom controls. **§4.6 is what is on the headset.** Read 4.1–4.5 for
> the reasoning; build from 4.6.

Super panel geometry, toe-in and placement are inherited from `04` §2 unchanged. This spec
governs the **420 × 430 data canvas** only; the 220 rail and the 340 stage behave as `04` §3.1
and §3.2 describe.

### 4.1 Sub-tab row

Two pills at panel-local (24, 56), 26 high: **`Identity`** 90 wide · **`Mechanical & electrical`**
190 wide, 10 apart. Active = `#0d2a57` fill, `#2e5aa0` stroke, white bold; inactive = `ROW` fill,
`STROKE` stroke, `text/secondary`. Rule at y 92, 372 × 1, `RULE`.
Content band **y 104 → 352**. Button row at y 362 (130 + 230, `04` §3.3) — never overlapped.

### 4.2 Identity sub-tab

Seven label/value rows at y 108 + i·26: key 8.5 pt `teal/light` left, value 11.5 pt bold white
right-aligned, 1 px `#12294e` hairline under each.

`MANUFACTURER` · `NAME` · `TYPE` · `SERIAL` · `PRODUCED` · `ORIGIN` · `CATEGORY`

Then **`SPECS · data sheet 245099915`** at y 300 (8.5 pt `teal/light`) and a chip row at y 308:
`166 × 121 × 41 mm` · `660 g` · `IP67` · `5-18 V` · `198 pins`.
Caption at y 348, 9 pt `text/tip`: `3 × AS018-35 connectors · 2 × 667 MHz dual-core · 8 GB flash`.

**SPECS lives inside Identity** — read off the Miro routine, not a separate sub-tab.

### 4.3 Mechanical & electrical sub-tab

One `PinchScrollArea` (the family scroll component, 13c §3.1) over the y 104–352 band, 372 wide.
Two sections, oldest family conventions kept: chrome only where touchable.

- Section head `PARTS · NX drawing available` — 8.5 pt `teal/light`.
- **8 part rows**, 21 high, 23 pitch, `ROW` fill, rx 7. Left: a 13 × 10 drawing glyph in
  `teal/light`. Name 10.5 pt bold white at x+30. Mass 9.5 pt `text/secondary` right-aligned at
  x+272. Material summary 8.5 pt `text/tip` right-aligned at x+364.
- Section head `BOARD MATERIALS · not discrete parts, no drawing` — 8.5 pt `text/tip`.
- **7 material rows**, same metrics at **opacity 0.55**, no glyph, name left / mass right.

Total 15 rows + 2 heads = 361 px of content in a 246 px window; ~11 rows visible, the clipped
12th is the scroll affordance (family rule). A 3 × 118 `INACT` scrollbar hint sits at x+374.

**Only the 8 part rows are hit targets.** Board-material rows are inert — there is nothing
behind them, and per the collected feedback (13c §3.1) inert chrome that looks tappable is a
defect. Their reduced opacity is the affordance signal.

### 4.4 Component detail (a part row was tapped)

Panel title becomes the part name; caption `n of 8 parts · <mass>`; back button reads
**`All parts`**; the sub-tab row stays visible with `Mechanical & electrical` active.

| Region | Geometry (panel-local) | Content |
|---|---|---|
| Drawing card | (24, 104) 372 × 122, rx 10, `#07142c` on `STROKE` | dimensioned NX views, contain-fit into 268 × 106 at x 24 |
| Iso inset | (296, 112) 94 × 106, rx 8, `#0b1e3d` | the shaded NX view, original colour, contain-fit |
| Caption | (34, 120) 8.5 pt `teal/light` | `NX sheet · <key dimensions>` — never the sheet title |
| Enlarge chip | (200, 204) 88 × 16 | `tap to enlarge` |
| Data rows | y 238 + i·20, 372 × 17, rx 6, `ROW` | one row per material, then `Disassembly step` |

Material rows read `<Material> · <role>` left in `text/secondary`, `<mass> g` right in bold
white. **Gold renders in `accent/gold`** — it is the one value a recycler is looking for, and it
is the only colour exception on this page. Where a part represents several BOM entries, an
`LCA entries` row in `teal/light` names them (§3.3b).

### 4.5 Drawing enlarged

The tap target from §4.4. Full data canvas: drawing card (24, 104) 372 × 248, contain-fit with
14 pad. Sub-tab row is **suppressed** (the drawing owns the panel). Back reads `Back to data`.
Two 28 px circular controls at (338, 330) and (372, 330): `−` and `+`, zoom only, no pan in v1.

**Why this state exists:** at 0.75 m a 372-unit-wide preview cannot carry `Ø23,4` legibly. Either
the drawing gets the panel or the drawing is decoration. Verify on device (open item 1).

### 4.6 As built — RBv2.1.1, after the 2026-08-06 device tests

Four states in the 420 × 430 data canvas. Geometry constants live in
`DPPUIBuilder.ProductSpecs.cs` (`Ps*`); this table is the intent, that file is the truth.

| State | Header | Content band (76 → 360) | Bottom bar |
|---|---|---|---|
| **Product ID** | pills `Product ID` / `Component ID`, caption blank | 7 key/value rows, 34 pitch, value auto-shrinks 12.5 → 8 pt | `Back` · `Next` |
| **Component ID** | same, second pill active | 8 part rows, 348 × 30, 35 pitch, in a `PinchScrollArea` | `Back` · tab-primary |
| **Detail** | same, **no caption** | **1 × 2 grid, proportional** — drawing card 372 × *h*, then one merged chart column (see 4.6.1) | `All parts` · **`i`**, no Next |
| **Drawing** | same, caption **blank** | drawing card 372 × 284, contain-fit, 14 pad, **nothing else** | `Back to data` **alone** |

#### 4.6.1 The proportional split

    lower   = 14 (heads) + rows × 20
    drawing = 284 − 10 − lower,   clamped to [114, 240]

Unclamped the two close **exactly** on the band bottom, which is why none of the constants is
round. 1 material → 240 of drawing · 3 → 200 · 4 → 180 · 5 → 160 · 6 → 140. The clamp bites past
7 materials and `LayoutDetail` **warns** rather than sliding the table under the button row.

⚠ **Nothing is reserved under the last row.** The first cut kept a 26-unit strip there for the
`i`; on device that read as empty navy on all eight components. The `i` moved to the button line
and every part got those 26 units back as drawing.

Implementation: the drawing card is one `RectTransform` whose *height* the view sets; its stroke,
fill, the drawing and the `View` pill are all anchor-stretched or corner-anchored inside it, so
one assignment moves everything. The lower block's `y` is the only other thing that moves.

#### 4.6.2 One chart column, two axes

| Column | x (content-local) | Content |
|---|---|---|
| MATERIAL | 0 → 96 | `Short(material)`, white at ≥ 1 % impact, `text/secondary` below |
| MASS | → 104, right | mg under a gram, else 1 dp |
| chart | 112, track 152 | **impact** bar 7 high at y 2 (log, 1e-6 %…100 %) · **rate** bar 5 high at y 11 (linear) |
| IMP % | → 316, right | `#2eb086`, gold rows in `accent/gold` bold |
| REC % | → 372, right | `#1f77b4`; `text/tip` at 0 %; `heat` `?` when null |

**The column heads are the legend** — each in its bar's colour, repeated by the % column beneath.
No key row.

⚠ **The two axes are not comparable and the panel must keep saying so.** Three decade ticks sit at
25/50/75 % of the track, drawn **over** the impact bar (dark where covered, light where not) — the
only mark on the row distinguishing a log length from a linear one. On the connector, impact 98.5 %
and rate 94 % end four units apart; that is a coincidence of two scales. Remove the ticks or the
modal and the chart lies quietly.

#### 4.6.3 The `i`, and the button line in this state

**`All parts` · … · `i`. No Next.** Detail and Drawing are leaves of the drill, and the only move
that makes sense from a leaf is back up it — Next there offered to leave the whole tab from its
deepest screen. Product ID and the list keep theirs, so the tab still has a way forward.

The `i` takes the slot Next vacated: a 22-unit dot in a 44 × 50 root at the far right of the
button line, same hit height as every other button on that line. It replaced the 372-wide footnote
that stated both formulas — true, unreadable at 0.75 m, permanently on screen. It opens a
344 × 214 modal on the page's own canvas (so `00` §4.2's modal-depth rule does not apply); the
scrim, a **grey `Back`** and **any state change** all close it. The modal is also where
`Drawing dimensions in mm` lives.

⚠ **Why it is built on the state root and not inside the lower block.** It was inside the block at
first, and `LayoutDetail` moved it per component — but `HoverHighlight.OnEnable` calls `Apply()`,
which writes `lift.localPosition = _restPos`, the pose captured the **first** time that object was
enabled. Activating the Detail root therefore undid the move every single time, and the `i` landed
beside an arbitrary row. **Rule: a RectTransform that HoverHighlight lifts must not also be moved
at runtime** — either give `HoverHighlight.lift` a child to raise, or keep the element fixed. This
one is fixed.

#### 4.6.3b No text in this state

Only the drawing and the table. The `Sc4 reuse eligible` badge is gone from the panel; the flag
and its note stay in the payload and in the LCA, and **04d** declares the reuse set. `View` also
shrank to 46 × 26 at 10.5 pt — **inside an unchanged 50-unit hit root** (`00` §4.2: shrink what is
drawn, never what can be pressed).

#### 4.6.4 Rules fixed by the earlier passes

Four rules this revision fixed, all from the same device test:

1. **The second pill never relabels itself.** It reads `Component ID` in all four states. It used
   to take the open component's name; on device that read as the tab moving under the hand. See
   `00` §4.3 — this is now an app-wide rule, not a decision local to this tab.
2. **The enlarged drawing carries only the drawing.** No component name inside the card, no
   `scale 1:1 · mm` caption. The name is redundant (the user tapped it out of a list one screen
   ago) and the scale claim was false — the drawing is contain-fit to the card, not to size.
3. **No `Next` in the enlarged drawing.** The whole button hides, not just its label, so its
   ~50-unit hit area stops swallowing pinches in the lower right. From the deepest screen in the
   tab the only offered move is back.
4. **No page title, no SPECS chips, no board-material rows** (carried from the earlier passes).

#### 4.6.5 Two things the chart cannot fix

1. **On 5 of the 8 parts one material carries 100.0 % of the impact** — both housings (aluminium
   alone) and the three IC groups (copper alone; polymers, silicon and ceramics have no EF 3.1
   factor at all). A full-width 100 % bar there is trivially true and reads as a finding. The
   chart earns its keep on the **connector** (gold: 0.04 % of mass, 98.5 % of impact) and **ic_1**
   (tin 80.9 %). That is the thesis argument, visible on 2 of 8 screens. **Open item.**
2. ⚠ Board materials — 79.4 g, **all** the tantalum and most of the on-board precious metals — are
   still undeclared anywhere in the UI. They are in the payload and in the LCA. **04d owns this.**

#### 4.6.6 Payload v0.16 — why `—` and `<0.01 %` are different answers

v0.15 stored `impact_share_pct` rounded to 4 dp. Connector aluminium **has** an EF 3.1 factor and a
real 3.28e-6 % share, but it stored as `0.0` — indistinguishable from polymers, which have **no**
factor — and the panel rendered both as `—`. v0.16 stores the share to 6 significant figures, and
`ShareLabel` decides *characterised vs not* from `impact_kg_sb_eq`, never from the share, so the
distinction survives an old payload too.

  * `—` = not characterised (no factor)
  * `<0.01 %` = characterised and negligible

## 5. Roles

Inherited from `04` §5. Differences that belong to this tab:

| | Product user | Recycler |
|---|---|---|
| Entry | tab chosen from the rail | **lands here** — first screen of the walkthrough |
| Other rail tabs | all four lit | later tabs at 0.38 opacity until visited |
| CTA | `Scan next product` | **`Next`** → Usage & service |
| Component detail | full access, all 8 parts | full access, all 8 parts |
| Emphasis | identity, compliance chips | mass, material, disassembly step, precious metals |

The Recycler cannot reach `Continue to disassembly` from here — that CTA exists only on the
Training disassembly tab (`04` §6).

## 6. Payload changes

Schema **0.13 → 0.14**.

1. `specifications.size_mm`: `"200 x 150 x 60"` → **`"166 x 121 x 41"`** (§1.1).
2. `components[]` 11 rows → **15 rows**, each gaining `group: "part" | "board_material"`:
   - `housing` (344) → `housing_upper` (108.5) + `housing_bottom` (235.5)
   - `pcb_substrate` (49) + `pcb_copper` (14) → `pcb` (63.0)
   - `actives` (23.7) → `ic_1` (8.0) + `ic_2` (4.2) + `ic_3` (9.0) + `ic_4` (2.5)
   - `passives` (47.5) → `passives` (45.0) + `ta_caps` (2.5)
   - `connectors`, `solder`, `tim`, `coating`, `misc`, `fasteners` — unchanged
3. New per-component fields: `drawing_id` (null for board materials), `bom_rows[]`,
   `represents` (free text, the `LCA entries` line).
4. `material_breakdown[]` restated from `By_Component` for every one of the 15 rows.

`disassembly_step` values are preserved through the split — no step changes.

### 6.1 Retired ids and the references that pointed at them

Four ids disappear: `housing`, `pcb_substrate`, `pcb_copper`, `actives`. Three blocks referenced
them by id and would have dangled silently — a null lookup, not an error. All remapped and
re-verified (0 dangling references across the payload):

| Block | Was | Now |
|---|---|---|
| `disassembly.steps[3].component_ids` | `pcb_substrate, pcb_copper, solder, passives, tim, coating` | `pcb, solder, passives, **ta_caps**, tim, coating` |
| `disassembly.steps[4].component_ids` | `actives` | `ic_1, ic_2, ic_3, ic_4` |
| `disassembly.steps[5].component_ids` | `housing, misc` | `housing_upper, housing_bottom, misc` |
| `repair_history.events[12]` | `actives` | `ic_1, ic_2, ic_3, ic_4` |
| `unit_use_phase.health.reuse_assessment` | 11 rows | **15 rows**, one per component |

`ta_caps` is added to step 3 because it was split out of `passives`, which was already there —
omitting it would have quietly dropped 2.5 g of tantalum out of the guided teardown.

Each reuse verdict inherited by a split row carries `verdict_inherited_from` naming its source,
so a reader can see that `ic_1`'s verdict is `actives`' verdict and not an independent judgement.

**Closure preserved:** verdict mass shares recompute to reuse 53.93 % · reuse-after-test 22.73 % ·
material recovery 22.13 % · consumable 1.21 %, and `reuse_fraction_by_mass` recomputes to 0.7666
against the stated 0.767. The regrouping moved no mass between verdicts.

## 7. Assets

16 PNGs, generated from the eight NX PDFs (`CAD_Specs/CAD/DWG/`), delivered as
`vcu_dwg_assets.zip`, destined for `Assets/Resources/dwg/`:

| Key | Part | `_dwg` (views + dimensions) | `_iso` (shaded) |
|---|---|---|---|
| `upper_housing` | Upper housing | ✔ | ✔ brown |
| `bottom_housing` | Bottom housing | ✔ | ✔ brown |
| `pcb` | Bare PCB | ✔ | ✔ green |
| `connector` | Connector AS018-35 | ✔ | ✔ grey (lifted) |
| `ic_1` … `ic_4` | Processor 1…4 | ✔ | ✔ brown / yellow / blue / red |

**Pipeline** (reproducible, `/tmp/fix` generator): render page at 4× → mask the ISO projection
symbol → detect the inner sheet frame by long-run scan → crop inside it → white out the Siemens
title block → erase a 104 px edge band (centring ticks) → autocrop to ink → split the shaded
region out as `_iso` → invert the remainder to **white on transparent** for the navy panel.

⚠ **Nothing from the title block survives** — no `SIEMENS`, no `DRAWN BY Thiago`, no sheet name,
no `Processor 1` label. Per instruction 2026-08-06: *"Forget the labels of the DWG, can u maybe
just pick up the dimensions and the view (colour component)."* The part names on screen come
from the payload, not from the sheets.

## 8. Open items

1. **Device check at 0.75 m** — is the 372 × 122 drawing preview readable enough to be worth a
   tap, or should the enlarged state be the only drawing state? Gates §4.5.
2. ~~`Processor 1…4` in the thesis~~ — **CLOSED 2026-08-06.** The four blocks now carry their
   BOM_v4 row names (§3.3b). The write-up must still state that four CAD blocks are a geometric
   proxy for **seven** BOM entries, but it no longer has to explain away four "processors".
3. **Tin double-count, 3.9 vs 4.6 g** (§2) — the only substantive BOM defect this spec found.
   Blocks nothing in the UI, blocks the LCA write-up. Decide before the Results chapter.
4. **BOM closure 660.1565 vs "closes at 660 ✔"** (§2) — restate or rebalance.
5. **`BOM_v4.md` ↔ `VCU_BOM_v4.xlsx` divergence** on FCBGA split and passives mass (§2) —
   pick one canonical, make the other follow.
6. **Zoom without pan** (§4.5) — acceptable for A4-landscape sheets at 372 wide? Test.
7. Board-material rows are inert by design (§4.3). If user testing shows people tap them, they
   need either a detail page or a clearer non-affordance.
8. `00` still needs the `navy/rail` and `heat/high` tokens and the 130 + 230 button row
   (carried from `04` §7).

---

*Last updated: 2026-08-06 · Status: §4.6 coded (payload v0.16), awaiting device test · §4.1–4.5 superseded draft · Parent: 04_DPP_page.md*
