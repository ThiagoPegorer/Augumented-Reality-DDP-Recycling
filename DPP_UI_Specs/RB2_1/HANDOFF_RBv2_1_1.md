# HANDOFF — ReBuilt v2.1.1 (RBv2.1.1)

*Written 2026-08-07 at the end of a long session, for a fresh chat that will continue the work.
Read §0 and §1 before doing anything.*

---

## 0. What you are working on

**ReBuilt v2.1.1** — an AR Digital Product Passport for a **Bosch Motorsport VCU MS 50.4**,
running on a **PICO 4 Ultra** in **Unity 6**. It is Thiago Pegorer's SRH Master's thesis
prototype. A 3D-printed mock unit sits on the desk in front of the participant; the AR model on
screen is deliberately recoloured to match it.

**The passport describes the BOSCH product.** The CAD prototype is a stand-in for it. `VCU_BOM_v4.xlsx`
and `LCA_framework_v4.md` are the **source of truth for materials** — do not invent a number that
belongs in either of them.

### Standing constraints — do not violate these

| Rule | Why |
|---|---|
| **Git is Thiago's.** You edit files; he commits and pushes. | The device bridge cannot delete files, so `git add` leaves orphan temp objects and a stale `.git/index.lock`. It has now broken twice. Give him the commands, never run them. |
| **Never create `_to_delete/` inside `Assets/`.** | Unity compiles it → duplicate class definitions → 139 errors. Put it in `XR/AR_DPP_VCU/_to_delete_outside_assets/`. |
| Bosch datasheet / manual PDFs never enter the repo. Participant data never reaches GitHub. | |
| No mid-study changes to disassembly difficulty or removal order. | |
| Freeze rule: last headset build ≥ 1 hour before the first participant. | |
| Each test gets a new version number. **Do not debate this.** | Thiago's ruling. |

### How Thiago wants to be talked to

Lead with what he does not want to hear. Tag claims `[Certain]` / `[Likely]` / `[Guessing]`.
Disagree with structure and hold the position unless given genuinely new information. No warm-up
paragraphs, no "great question". He overrules with facts and expects you to fold **only** then.

---

## 1. STATE OF PLAY — read this before touching anything

### 1.1 Not committed

`HEAD` is `243fefe "RBv2.1.1 building"`. The **entire model-link feature is uncommitted**:

```
 M DPP_UI_Specs/RB2_1/00_design_standards_rbv2.md
 M DPP_UI_Specs/RB2_1/04c_product_specs.md
 M XR/AR_DPP_VCU/Assets/Editor/DPPUIBuilder.SuperPanel.cs
 M XR/AR_DPP_VCU/Assets/Editor/DPPUIBuilder.Verify.cs
 M XR/AR_DPP_VCU/Assets/Scripts/DDP/UI/ProductSpecsView.cs
 M XR/AR_DPP_VCU/Assets/Scripts/DDP/UI/SuperPanelView.cs
?? XR/AR_DPP_VCU/Assets/Scripts/DDP/UI/ModelLinkController.cs
?? XR/AR_DPP_VCU/Assets/Scripts/DDP/UI/ModelLinkController.cs.meta
```

### 1.2 ⚠ The scene is missing two screens

`MainScene.unity` greps: **`DisassemblyIntroView` → 0 occurrences. `ExplodedZoneInteraction` → 0.**
`StepFlowController` and `CompletionSummaryView` survive.

`RBv2_1/1` is **destructive by design** — it deletes `DPPPanelCanvas` and every screen under it,
and the disassembly phases were never in the chain being re-run. This is documented in
`RETIREMENT_PLAN.md` §0 and was never acted on. The scene lost ~64,500 lines in the last commit.

**Fix first, before anything else:** `RBv2_0/4` → `RBv2_0/5` → `RBv2_0/6` → Verify.

Until that runs, the new **Continue to disassembly** button routes to a screen that does not exist.

### 1.3 ⚠ Never run on device

The **whole model link** (§3) has never executed. Not once. It compiles-clean by inspection only —
brace/paren balance, cross-file method resolution and serialized-field wiring were checked
statically. Treat every number in it as a first guess.

### 1.4 The run order

```
RBv2_0/4 → RBv2_0/5 → RBv2_0/6          (restore the disassembly screens)
RBv2_1/9                                 (Product specs tab)
RBv2_1_1/1                               (super panel rig + stage model + model link)
RBv2_1_1/2                               (Product specs into the data canvas, close the link)
RBv2_1/Tools/Verify wiring
SAVE THE SCENE
```

Also available: `RBv2_1/Tools/Apply real-life colors` (recolours the model to match the printed
prototype — brown `#8a5a3b` bottom, yellow `#f2c11e` upper, green `#2e7d4f` connectors + PCB).

---

## 2. What was finished this session — 04c Component ID

The Product specifications tab, sub-tab **Component ID**, is complete and device-tested through
four rounds. Spec: `DPP_UI_Specs/RB2_1/04c_product_specs.md` §4.6 (§4.1–4.5 are a **superseded
pre-build draft** — read them for reasoning, build from §4.6).

**Four states** in the 420 × 430 data canvas: `Product ID` · `Component ID` (the parts list) ·
`Detail` · `Drawing`.

**Detail is a proportional 1 × 2 grid:**

```
lower   = 14 (heads) + rows × 20
drawing = 284 − 10 − lower,   clamped to [114, 240]
```

Closes **exactly** on the band bottom for all 8 parts (1 material → 240 of drawing, 6 → 140). Past
7 materials `LayoutDetail` **warns** rather than sliding the table under the button row.

**One merged chart column,** two bars sharing an origin over a 152 track:
impact 7 high (**log**, 1e-6 %…100 %) above, recovery rate 5 high (**linear**) below. Then two
colour-matched % columns — `#2eb086` green for impact, `#1f77b4` blue for rate. **The column heads
are the legend.** Three decade ticks at 25/50/75 % are drawn **over** the impact bar and
recoloured per row; they are the only mark saying the two axes differ. **Do not remove them.**

**Bottom bar in the component states:** `All parts` · … · `i`. **No Next** — Detail and Drawing
are leaves of the drill. The `i` opens a 344 × 214 modal explaining both axes; grey `Back`, scrim
and any state change close it. `Drawing dimensions in mm` lives in that modal.

### Rules that came out of it — now app-wide in `00`

* **§4.3 A selector never relabels itself.** The Component ID pill reads the same in every state.
  A tab that renames itself to the open component read as a tab *moving under the hand*. Varying
  text goes in the caption; **hide a button, never blank its label** (a blank label leaves a
  ≥50-unit invisible hit area).
* **§4.4 Never move a RectTransform that `HoverHighlight` lifts.** `OnEnable` calls `Apply()`,
  which writes `lift.localPosition = _restPos` — the pose captured the *first* time that object
  was enabled. Layout code that repositions the same transform is silently undone. Either keep the
  element fixed, or give `HoverHighlight.lift` a **child** to raise. This cost a device round.
* **§4.2** `DPPSpriteFactory.Pill` is **not** a capsule — use `Capsule(img, visualHeight)`.
  `PsSmallPill` now takes `visualH` / `fontSize`: **shrink what is drawn, never the hit root.**

---

## 3. What was BUILT BUT NEVER RUN — the model link

`Assets/Scripts/DDP/UI/ModelLinkController.cs` (~440 lines), on `ModelPivot`.
Spec: `04c` §4.7. Map: `00` §8.1.

**LINKED** (was `LOCKED`; the words the user reads are now **LINKED / FREE** — field names keep
the old spelling so scene wiring survives):
the model plays the **disassembly intro's own teardown** on entry and stays open; **no idle yaw**;
opening a component keeps its bodies at true colour and darkens everything else to 35 %; pinching
a body switches the rail to Product specs and opens that page.

**FREE:** yaw / zoom / reposition exactly as before, link cut both ways. Re-linking opens
**instantly** (no second five-step animation) and re-selects whatever the panel shows.

### The four traps already paid for — do not undo them

1. **The explode is `DisassemblyAnimator`, not a bespoke one.** The first cut computed directions
   from node positions; `component3` and `component4` sit at almost the same point and landed on
   each other. The animator has ordered, tuned travel and is the animation the participant already
   saw in the intro.
2. **The animation must not depend on the payload.** The first cut gated motion on the map
   resolving → one unmatched node name → **model completely frozen on device**. It now opens
   regardless; only highlight and pick need the map.
3. **Index every descendant, not only renderer-carrying transforms.** glTF puts meshes on children
   of the named node; a renderer-only index is keyed by names the payload never heard of.
4. **`RBv2_1_1/1` used to disable every collider on the stage clone.** A disabled collider
   registers fine and then silently never raycasts. It now leaves them enabled and the controller
   re-enables defensively.

Picking uses `PicoHandUIBridge.OnPinch3D` — a hook that has existed unused since RB2.0 commented
*"VCU mesh later"*. Dimming writes `_BaseColor` through a `MaterialPropertyBlock` (no alpha, no
transparent queue, no PICO sorting risk) — the same technique `ConstrainedTeardownModel` uses.

**Tuning knobs, all inspector floats — tune in play mode, do not ask for a rebuild:**
`targetWorldSpan` (0.26 — the one to move if the open model is the wrong size on the stage),
`dimFactor` (0.35), `highlightSeconds` (0.15), `openStep` (5).

### Also new: the Training disassembly tab

Tab 3 now builds a real page (title, two lines, **Continue to disassembly** → `ScreenRouter.ShowDisassembly()`),
ahead of the 04b spec, because that button is the only route into the teardown. For the Recycler
the rail is a walkthrough — `IsReachable(3)` needs tabs 0, 1, 2 visited first; clicking through
the placeholders is enough.

---

## 4. Data — payload schema v0.17

Two copies, **both must be written**:
`XR/AR_DPP_VCU/backend/data/vcu_001.json` (**live — the server runs from here**) and
`backend/data/vcu_001.json` (mirror). Mirrored in `backend/models.py` (Pydantic) and
`Assets/Scripts/DDP/DPPModels.cs` (C#).

* **v0.15** — per material: `impact_kg_sb_eq`, `impact_share_pct`, `recovery_pct`; per component:
  `minerals_impact_kg_sb_eq`, `reuse_eligible`, `reuse_note`; top-level `material_reference`.
* **v0.16** — `impact_share_pct` to **6 significant figures**. v0.15 rounded to 4 dp, which stored
  connector aluminium's real 3.28e-6 % share as `0.0` — identical to polymers, which have **no**
  characterisation factor. `ShareLabel` decides *characterised vs not* from `impact_kg_sb_eq`,
  never from the share. `—` = not characterised; `<0.01 %` = characterised and negligible.
* **v0.17** — `components[].mesh_nodes`: the CAD ↔ passport map, in **data**, not code.

### The CAD ↔ passport map (`00` §8.1) — the one table three specs depend on

| passport id | display name | glTF nodes | CAD colour |
|---|---|---|---|
| `housing_upper` | Upper housing shell (HPDC) | `housing_upper` | grey |
| `housing_bottom` | Bottom housing shell (HPDC) | `housing_bottom` | grey |
| `pcb` | Bare PCB, 4-layer FR-4 | `pcb` | green |
| `connectors` | Connectors 3× AS018-35 | `connector`, `connector001`, `connector002` | near-black |
| `ic_1` | Processors 2× FCBGA + flash 2× 4 GB | `component1` | **gold `#E0C14A`** |
| `ic_2` | Regulators + analog front-end | `component3` | **blue `#003C99`** |
| `ic_3` | Power stages 6× (DPAK) | `component4`, `component001`, `component002` | **red `#CB3636`** |
| `ic_4` | Comm transceivers + MEMS sensors | `component2` | **brown `#8F5110`** |
| `fasteners` | Fasteners (~12 × M3) | 14 screw nodes | grey |

Confirmed by Thiago 2026-08-07 from the CAD colours. All 26 mesh-bearing nodes claimed exactly
once, verified both directions.

**Three consequences that are easy to get wrong:** one row can be several bodies (a single-node
highlight is a bug) · one body can stand for several devices (`component1` = 2 × FCBGA + 2 × flash)
· **screws belong to the teardown, not to Component ID** — `fasteners` is a board material with no
page, so it must stay non-selectable in 04c and is the whole subject of 04b.

### The 8 parts (total 660.1565 g, matches `VCU_BOM_v4.xlsx` DEVICE TOTAL exactly)

```
Upper housing shell (HPDC)          108.4757 g   1 material
Bottom housing shell (HPDC)         235.5243 g   1
Bare PCB, 4-layer FR-4               63.0    g   3
Connectors 3x AS018-35              150.06   g   6   <- gold 0.06 g = 98.5 % of impact
Processors 2x FCBGA + flash 2x 4 GB   8.0    g   5   REUSE   <- tin 80.9 % of impact
Regulators + analog front-end         4.2    g   3
Power stages 6x (DPAK)                9.0    g   3   REUSE
Comm transceivers + MEMS sensors      2.5    g   4
```

Plus **7 `board_material` rows** (79.4 g: passives, Ta caps, solder, TIM, coating, misc,
fasteners) that carry **all** the tantalum and most on-board precious metals and are **declared
nowhere in the UI**. **04d owns this.**

### Sources — do not invent alternatives

* **Impact**: EF 3.1 ADP (ultimate reserves), kg Sb eq/kg, from Thiago's own openLCA pack →
  `LCA_Analysis/Docs/adp_ef31_factors.json`. Gold 52.0 · Silver 1.18 · Antimony 1.0 · Pd 0.571 ·
  Sn 0.0162 · Cu 1.37e-3 · Ni 6.53e-5 · Ta 4.06e-5 · Fe 5.24e-8 · Al 1.09e-9 · Si 1.4e-11.
* **Recovery**: `LCA_framework_v4.md` Sc4, source S-4 = **Bigum, Brogaard & Christensen (2012)**
  *J. Hazard. Mater.* 207-208, 8-14, Table 8 p. 11. Au/Pd 98 % · Ag 97 % · Cu 95 % · Fe 100 % ·
  Al remelt 79 %. **Net recovery = arrival × downstream yield.**

> ⚠ **Pattern to avoid: I once invented recovery rates and built the UI before searching
> `LCA_Analysis/Docs`.** All three placeholders were wrong, two in the direction that flattered the
> design. **Search the LCA docs first, build second.**

---

## 5. Open items

1. **Fastener count, 12 vs 14.** BOM row says "~12 × M3" / 12.000 g; `00` §8 and the glTF both have
   **14** (4 housing, 6 connector, 4 PCB). No UI number moves, but 04b counts screws out loud.
   **Decide before writing that step text.**
2. **Tin double-count, 3.9 vs 4.6 g** between `BOM_v4.md` and the xlsx. The only substantive BOM
   defect found. Blocks the Results chapter, not the UI.
3. **Nickel is uncredited in every scenario and the framework never says why** — unlike tantalum,
   which has an explicit S-6 non-recovery note. Add the sentence.
4. **On 5 of 8 parts one material carries 100.0 % of the impact** (housings = aluminium alone; the
   three IC groups = copper alone, everything else uncharacterised). A full-width 100 % bar there
   is trivially true and reads as a finding. The chart earns its keep on **connectors** and
   **ic_1** only. Consider a sentence on the single-material components instead of a bar.
5. **Colour mismatch, print vs CAD** — `00` §8 describes the printed prototype (bottom brown, upper
   yellow, connectors green); the raw glTF is grey/near-black. `RBv2_1/Tools/Apply real-life
   colors` fixes it — **run it and confirm on device.**
6. **Board materials undeclared in the UI** (79.4 g) — 04d.
7. Device-verify `SpDistance` 0.75 m and the toe-in; freed-model containment volume.
8. RB2.0 retirement waves per `RETIREMENT_PLAN.md`. ⚠ **Move `DestroyChild` out of
   `DPPUIBuilder.DppCanva.cs` before deleting it** — three live files call it. And **do not retire
   `ZonePartInteraction`**: its `RaycastAll` → `FindBodyByCollider` → smallest-volume-wins picker
   is the reference implementation for per-part selection.
9. Backfill unlogged Notion sessions (2026-08-02/03, 08-04, 08-06, 08-07).

---

## 6. Next steps, in Thiago's order

He overruled a different ordering with a real architecture constraint — the disassembly steps and
intro are not reachable until the training-disassembly tab exists. His order stands:

1. **Get the model link working on device.** It has never run. Expect the first pass to need
   `targetWorldSpan` tuning and possibly node-name corrections.
2. **04b — training disassembly.** Spec not written. The route in now exists (tab 3 button).
3. **Rewrite the disassembly step texts** with the final component nomenclature from §4's table.
   Because that table already exists, this should be a verification pass, not a rewrite.

Then: **04d environmental impact** (must declare the 79.4 g of board materials; can reuse the
priority chart at device level) and **04a usage & service** (spec written, needs building).

---

## 7. File map

```
DPP_UI_Specs/RB2_1/
  00_design_standards_rbv2.md     §4.1 elevation kit · §4.2 corners+hit areas · §4.3 selectors
                                  §4.4 HoverHighlight vs layout · §8 product · §8.1 CAD↔passport
  04c_product_specs.md            §4.6 as built · §4.7 the model link · §8 open items
  RETIREMENT_PLAN.md              what can be deleted, in what order, and the DestroyChild trap
  drafts/04c_v6_component_id.svg  the mock this UI was built from

XR/AR_DPP_VCU/Assets/
  Editor/DPPUIBuilder.*.cs        partial class, ~12 files, one per screen; [MenuItem] phases
    .ProductSpecs.cs              RBv2_1/9 · owns the elevation kit (AddShadow/AddGloss/Capsule/
                                  Inset/PsSmallPill) and every Ps* geometry constant
    .SuperPanel.cs                RBv2_1_1/1 and /2 · stage model · model link · training tab
    .Verify.cs                    the wiring map — CORRECT THIS THE DAY A SCREEN STOPS BEING USED,
                                  not the day its files are deleted. The last four alarms were fossils.
    .Colors.cs                    RBv2_1/Tools/Apply real-life colors
  Scripts/DDP/
    DPPModels.cs                  C# mirror of models.py
    DisassemblyAnimator.cs        THE teardown animation: PlayFullTeardown / ApplyStepInstant(1..5)
    ConstrainedTeardownModel.cs   per-part bodies, tight colliders, FindBodyByCollider, isolation
    PicoHandUIBridge.cs           hand ray → UI + OnPinch3D (the 3D pick hook)
    UI/ProductSpecsView.cs        04c, four states, LayoutDetail, the merged chart
    UI/ModelLinkController.cs     NEW, UNTESTED — the model ↔ data bridge
    UI/SuperPanelView.cs          rail + stage + data canvas, LINKED/FREE
    UI/HoverHighlight.cs          global hover: rise 6, scale 1.03, shadow drop, fill brighten
  CAD model/VCU_assembly.gltf     41 nodes, 26 with meshes

LCA_Analysis/Docs/
  LCA_framework_v4.md             SCENARIOS AND RECOVERY RATES — read before quoting any number
  adp_ef31_factors.json           EF 3.1 ADP characterisation factors
```

---

*End of handoff. If something here contradicts the code, the code is newer — say so rather than
following the document.*
