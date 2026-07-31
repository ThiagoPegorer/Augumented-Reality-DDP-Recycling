# DPP UI Spec — 13: DPP Canva (product info) — **v9**

> **Living spec** — ReBuilt v2.0. Status: **v9 CODED 2026-07-31, awaiting device check.**
> v1 (built + device-tested 2026-07-29) was a 2×2 grid of four title-only cards opening full-page
> modals. v2 kept the drill-down but put **live data on the face of every tile** and realigned the
> categories to CIRPASS D2.2 Table 6. v3 strips the screen chrome — no back arrow, no product title,
> no dot legend — to hand vertical space to the tab block. v4 turns the Identity hero into two plain
> bound text lines. Mocks: `drafts/13_v4_product_information.svg` (current),
> `drafts/13_v3_dpp_canva.svg` (v3), `drafts/13_v2_C_dpp_canva.svg` (v2).
> Information model + Table 6 coverage: **`13b_information_model.md`**. Builder: `RBv2_0/7`.

---

## 1. What changed in v2, and why

| | v1 (built) | v2 |
|---|---|---|
| Problem | four cards showing only a title — nothing invites a tap | main info on the face; the modal is for the full entry |
| Categories | Identity · Materials · Hazardous · Compliance | **Table 6 aligned**, and **Materials + Indicators MOVED to spec 14** |
| Hero | none (LCA card) | **Identity & specifications**, full width |
| Empty data | invisible | **explicit** — filled dot = declared, hollow dot = not provided |
| Forward CTA | `Continue to 3D model` | **`Continue`** |

**Why Materials left this screen (Thiago, 2026-07-30):** composition and the circularity indicators
belong next to the 3D model, not in a text screen. See spec 14.

**Consequence, accepted:** with composition gone, four of the five tiles are declarations and most read
"not provided". The Identity hero exists to stop the first screen after a scan being a grid of blanks.

## 1.1 What changed in v3 (2026-07-30)

Thiago's brief: *"move the arrow to a go back, to a button on the right side of the Continue and write
'main menu'. Delete the Vehicle Control Unit… This changes will bring more space for the tabs block."*
Two things changed on review: the button order was swapped to **Home left, Continue right**, and the
label went `Main menu` → **`Home`** — the destination is `WelcomeController.ShowWelcome()`, which is the
entry screen, not a menu.

| Change | Was | Now |
|---|---|---|
| Back edge | circular arrow, c(42, 44), 40 px | **`Home` labelled pill**, bottom bar left |
| Screen title | `Vehicle Control Unit` 19 bold @ x 76 | **deleted** |
| Caption | `Digital Product Passport` 11.5 @ x 76 | same styling, **x 24** — into the arrow's old slot |
| Rule | y 76 | **y 48** |
| Hero / tiles | y 88 / 192 / 272 | **y 60 / 164 / 244** (−28) |
| Dot legend | 2 lines @ (24, 366) | **deleted** |
| Bottom bar | one CTA, 328 @ x 288 | **`Home` 180 @ x 24 · 24 gutter · `Continue` 388 @ x 228** |
| Gap above the bar | 10 px | **38 px** — the room handed to the tab remodel |

**Why the legend could go without losing the honesty mechanism.** `PassportView.SetRow` already writes
the words into the row (`"— not provided"`, `"no substance declaration made"`) **and** dims the row to
`text/tip` when `basis == not_provided`. The dot was a third encoding of a fact stated twice already.
⚠ **Open question for the tab remodel:** if the text carries it, do the dots still earn their place?

**Why the bottom bar is secondary-left / primary-right.** Every other screen in RBv2.0 does this —
Welcome (`Close app` / `Scan to start`), the first-run prompt (`Skip` / `Tutorial`), the disassembly
gate (`Quit` / `Continue`). The first v3 draft put the back pill on the right, as briefed; it was
swapped on review to keep the convention. 180 + 24 + 388 = 592, exactly the content width.

⚠ **What the rename does NOT fix.** `Home` describes the destination honestly, but Welcome offers only
`Close app` and `Scan to start` — there is no path back into the passport except re-scanning the QR
code. An accidental tap costs the participant a re-scan. Not addressed in v3; options if it bites on
device are a confirm step or a `Resume` edge on Welcome while a passport is loaded.

⚠ **`MakeScreenHeader` was NOT changed.** It is shared with the disassembly intro (`RBv2_0/4`) and the
Composition & impact landing, and `MakeShellPage` builds its own copy of the same arrow. v3 adds a
separate **`MakeCaptionHeader`** used by the DPP Canva landing alone; every other screen keeps its
arrow, because they have no bottom-bar back edge to move it to.

**Deferred:** the Composition & impact landing (spec 14) still has the arrow, the 19 pt title and the
328-wide CTA at x 288. It gets the same treatment when we reach it.

## 1.2 What changed in v4 (2026-07-30) — Product information

Thiago: *"Lets focus first in the one: Identity & specifications. Change the name to Product
information. This tab will not be a button, I just want to plot the name: Bosch Motorsport | Vehicle
Control Unit MS 50.4. Add in small typo the serie number… My goal here is provide as much as easy
readble information I can to the user without make him stay surfing inside tabs."*
Then, on the mock: drop the `Product information` label, name at **16 pt**.

The tappable **Identity & specifications** card is replaced by **two lines of plain text**:

| Element | Binding | Type | x | y | w | h |
|---|---|---|---|---|---|---|
| `ProductName` | `identity.manufacturer` + a pipe separator + `identity.model` | 16 bold `text/on-navy` | 24 | 62 | 592 | 22 |
| `SerialNumber` | `" - "` + `identity.serial_number` | 11 `text/tip` | 24 + measured | 62 | 200 | 22 |

**v4.1 (2026-07-30):** the serial moved **onto the name's line** — `Bosch Motorsport | Vehicle Control
Unit MS 50.4 - VCU0001` — keeping its own 11 pt `text/tip` styling.

It is **two TMP objects, not one rich-text string.** `AddText` assigns the dedicated **bold font asset**
for the name, and no rich-text tag can switch a font *asset* back off — an inline serial would render
bold. Both rects share y 62 and height 22 so their midlines coincide;
`PassportView.PlaceSerialAfterName()` slides the serial right by
`GetPreferredValues(identityLine.text).x + 2`.

⚠ `GetPreferredValues`, **not** `preferredWidth`: `Populate` runs while both passport screens are still
inactive, where `preferredWidth` reads 0 — the serial would land on top of the name. Same trap and same
fix as `FillChips`.

No fill, no stroke, no hover outline, no icon, no chevron, no `MakeTappable`. A non-interactive block
must not wear the same costume as the four tiles below it, or the tiles stop reading as tappable.

**Removed with the card, and now rendered NOWHERE until a later tab claims them:**

| Lost | Payload source | Table 6 |
|---|---|---|
| 5 spec chips — size, weight, protection class, supply voltage, operating temperature | `specifications.*` | D2.1 |
| Category caption `EEE — electronic control unit (WEEE cat. 5, small equipment)` | `identity.product_category` | #3 |
| Documents status line — the not-applicable statement | `documents[]` | #1 #2 #16 #17 |
| Production date, country of origin | `identity.*` | D2.1 |

⚠ **This runs against the stated goal in the short term.** "As much readable information as I can
without surfing tabs" — and v4 takes six live values off the screen and defers them to tabs that do not
exist yet. Accepted deliberately (Thiago, 2026-07-30, "drop them now"); the mechanical tab must land
before the study, or the passport shows less than v3 did.

**No code was deleted to achieve it.** `categoryCaption`, `specChipRoots`, `specChipLabels`,
`docStatusDot` and `docStatusLine` all remain in `PassportView` — the builder simply stops wiring them.
`FillChips` returns early on a null array and every other consumer null-guards, so the values are one
`SetRef` away from coming back. `ShortMonth` is likewise kept for whichever tab claims the date.
`SpecChipPool = 6` stays declared in the builder for the same reason.

**Navigation side effect:** the block is not tappable, so the `IdentityDetail` shell is **no longer
built** and `PassportRouter.detail1` stays null. `Open1()` is unreachable; if it were ever called,
`SetActiveSafe` null-guards and it degrades to `Back()`.

**Data edits to `backend/data/vcu_001.json`:**

| Field | Was | Now | Why |
|---|---|---|---|
| `identity.model` | `VCU Bosch MS 50.4 (model)` | `Vehicle Control Unit MS 50.4` | the old string carried a `(model)` placeholder into the UI |
| `identity.serial_number` | `VCU-2026-001` | `VCU0001` | Thiago's value — ⚠ **must match the marking on the physical unit**, or a participant will spot the mismatch |

## 1.3 What changed in v5–v8 (2026-07-30 / 31) — the tab remodel

Mocks: `drafts/13_v8_dpp_canva.svg` (landing), `drafts/13_v7_electrical_parts.svg` (Electrical "+").

| Tile | v4 | v8 |
|---|---|---|
| — | Substances & safety (24, 114) | **renamed Mechanical data, full width 592 × 72 @ (24, 100)** |
| — | Compliance & certification (326, 114) | **Electrical data (24, 180)** |
| — | Service & repair (24, 194) | Service & repair (326, 180) |
| — | Usage & repair history (326, 194) | Usage & repair history (24, 260) |
| — | — | **Compliance & certification (326, 260)** |

**Mechanical data** carries the spec chip pool that the identity hero owned in v2/v3 —
`specChipRoots` + `FillChips`, unchanged, so every chip is still bound. Renders
`200 x 150 x 60 mm · 660 g · IP67 · -20 to 80 °C`. Supply voltage was removed from this pool.

**Electrical data** has its own single `SupplyChip` (`5 to 18 V`, widened to its text by
`PassportView.PopulateElectrical`) plus one bound row, `9 electrical parts`, counted from
`physical_unit.parts[]`.

**Both carry a circular `+` instead of a chevron and their CARDS ARE INERT** (Thiago,
2026-07-31). `BuildPlusButton` — 40 px visual in a 52 px hit rect, ring + fill + hover, the
same recipe as the screen-header back button. ⚠ **Affordance split:** Service, Usage and
Compliance respond anywhere on the card; these two respond only on one circle. Watch for
participants tapping the row and concluding it is inert.

### 1.3.1 Product vs. demonstrator — the modelling decision

The physical study unit is a **3D-printed replica**, not a Bosch MS 50.4. Thiago, 2026-07-31:
*"if I go in this direction I will be modelating the real Bosch VCU and not my prototype,
being confuse to my users."*

New payload block **`physical_unit`** (schema v0.8) keeps replica facts out of
`specifications`, where a reader would take them for the product's declared values:

```
physical_unit { is_replica, replica_of, size_mm, basis, note, parts[] }
parts[] = { id, name, count, colour, swatch_hex, photo_id, note }
```

Populated with 4 groups / 9 parts: 3 connectors (grey) · 2 processors (blue + yellow,
667 MHz dual core) · 1 CPU (brown) · 3 sensors (red).

**Where each number comes from now:**

| Value on the Mechanical row | Describes | Source |
|---|---|---|
| `200 x 150 x 60 mm` | the printed demonstrator | measured |
| `660 g` | the Bosch product | datasheet 234686731 (`≤ 660 g`) |
| `IP67` | the Bosch product | datasheet |
| `-20 to 80 °C` | the Bosch product | datasheet |

⚠ **ACCEPTED RISK — no marker on screen.** A caption reading *"size measured on this unit ·
IP67, mass and temperature declared for the product"* was drafted and **removed on Thiago's
instruction (2026-07-31)**. The row therefore mixes one measured replica dimension with three
declared product values and gives the reader no way to tell them apart. This is the same class
of defect the study is designed to detect. Most exposed value: **660 g** — a participant can
pick the box up, and a 3D print does not feel like 660 g.

### 1.3.2 Substances & safety no longer exists

Renaming that tile deleted the only place the passport spoke about **Table 6 #5 #6 #7 #16 #17**
— five mandatory attributes. `PassportView` keeps the bindings; nothing renders them. The
computation is preserved as a comment block in `PopulateStatusTiles` so restoring it is a tile
plus six lines. **This is the largest open compliance gap in RBv2.0.**

### 1.3.3 Datasheet reconciliation (Bosch 234686731, 27 Mar 2026)

| Field | Was | Now |
|---|---|---|
| `identity.type_number` | `null` | **`F02U.V02.965-02`** |
| `specifications.protection_class` | `IP67 (representative)` | `IP67` |
| `disassembly.parts` | `3 processors`, `3 chips` | `2 processors`, `1 CPU`, `3 sensors` |
| step 5 subtitle | `Die-cast AlSi · 363 g` | **`344 g`** — it disagreed with BOM_v4 |

The v1 "hardcoded strings disagree with the payload" bug in §4 is now **explained**: the builder
held the datasheet's `166 x 121 x 41 mm`, the payload held the replica's `200 x 150 x 60`.
Neither was wrong — they described two different objects and nothing said so.

⚠ **BOM_v4 totals 660.2 g, which exceeds the datasheet's own `≤ 660 g` ceiling.** Small, but it
is your LCA mass against the manufacturer's spec.

**New sprites:** `IcCube` (Mechanical), `IcBolt` (Electrical), `IcPlus`. `IcWarning` is now
unused by this screen.

## 1.4 What changed in v9 (2026-07-31) — Service & repair, and "+" everywhere

Mock: `drafts/13_v9_service_repair.svg`. Builder: `BuildServiceDetail`.

**Every tile now uses the `+` circle and an inert card** (Thiago: *"always the '+'. the '+'
feature will be implemented in the whole DPP Canva"*). `MakeTileCard` gained `heroStroke` so
only the full-width Mechanical row keeps `tab/active-stroke`; the rest stay on `row/stroke`.
Row text narrows from 200 to 168 px on `+` tiles so a long line can never slide under the circle.

### 1.4.1 `simulated` — a new basis value

`BASIS_VALUES` and `DppBasis` gain **`simulated`**: data INVENTED for the demonstrator.
Deliberately excluded from `IsFirmSource()`, so every dot bound to it renders dim, exactly
like `modelled`. The detail header also carries a `simulated data` caption in amber.

⚠ **Study-design risk, not a UI one.** Service & repair is now the ONLY tile with rich
content — Substances is gone, Compliance shows one certificate, Usage is empty, Mechanical is
four chips. A participant judging completeness or trustworthiness will judge largely from the
one screen that is full, and that screen is fabricated. Decide deliberately whether that is
acceptable for the comparison.

### 1.4.2 The data

`service.software_updates[]` — **11 entries**, every 14 days from Mon **02 Mar 2026** to
**20 Jul 2026**, `v4.12.1 → v4.14.1`, channel `automatic`, `software_update_basis: simulated`.
`repair_history` — **1 event**, 14 May 2026, *SENS-B connector replaced after intermittent
signal loss*, `exchanged_component_ids: ["connectors"]`, €148.00, `basis: simulated`.

**Nothing is dated in the future.** The brief said "Mar/26 until Aug/26"; August had not begun,
so the last real entry is 20 Jul and the axis simply runs one cadence further to show the next
one falling due.

### 1.4.3 Face rows

| Row | v8 | v9 |
|---|---|---|
| 4 | `disassembly guide in this app` (T6 #12) | **`11 automatic updates`** |
| 5 | `spare parts · manuals not provided` (T6 #15) | **`1 repair · May 2026`** |

The two displaced lines move to the detail page. ⚠ Row 7 on the Usage tile used to hardcode
`DppBasis.Measured` whenever a repair existed — with a simulated log that would have upgraded
invented data to a firm source. It now reads the basis off the record.

### 1.4.4 Detail page (first populated shell)

Counters (24, 88) and (326, 88) · timeline card (24, 170, 592 × 86) with track x 24 w 544,
axis y 52, update ticks above the axis and the repair diamond below it so same-day events never
collide · month labels generated from the data range · log card (24, 268, 592 × 116) showing the
5 most recent entries merged, newest first, with an "N earlier entries not listed" footer.

Pools: `SvcTickPool 16`, `SvcMonthPool 8`, `SvcLogPool 5`. **14 serialized refs, all wired,
none orphaned.** Cadence in the caption is computed from the data (mean gap), not assumed.

**`MakeShellPage` gained `showIcon` / `rightCaption` / `placeholder`.** Service & repair drops
its icon — the `IcWrench` glyph read as a magnifying glass (⚠ in the *mock*; the real sprite is
an open C arc, so it looks different on device). It is now the only shell header without an
icon; the other four still show one.

## 2. Layout (panel 640 × 430; panel-local = mock SVG minus 20) — **v4**

| Element | x | y | w | h |
|---|---|---|---|---|
| Caption `Digital Product Passport` 11.5 `text/caption` | 24 | 24 | 300 | 15 |
| Separator `#1a335f` | 24 | 48 | 592 | 1 |
| **`ProductName` 16 bold** (plain text, not a card) | 24 | 62 | 592 | 22 |
| **`SerialNumber` 11 `text/tip`**, same line, slid right at runtime | 24 + name width + 2 | 62 | 200 | 22 |
| Card — Substances & safety | 24 | 114 | 290 | 72 |
| Card — Compliance & certification | 326 | 114 | 290 | 72 |
| Card — Service & repair | 24 | 194 | 290 | 72 |
| Card — Usage & repair history | 326 | 194 | 290 | 72 |
| Secondary CTA `Home` → **Welcome** | 24 | 354 | 180 | 52 |
| Primary CTA `Continue ›` | 228 | 354 | 388 | 52 |

**Free space above the bottom bar: 88 px** (was 38 in v3, 10 in v2). That is more than one whole tile
row (72 + 8 gutter) — the remodel can add a fifth tile without touching anything else.

Removed in v3: back button, `Vehicle Control Unit` title, right caption (`rightCaption` was already
being passed as `null` in the build — the old spec row was stale), and the two legend lines.
Removed in v4: the whole Identity hero card and everything it carried (see §1.2).

Cards stroke `row/stroke`; all fills `row/fill`. Hover outline per 00 §4.

**Card internals (290 × 72), unchanged by v3:** icon c(26,36) 22² · title (48,10) 14 bold · status rows
from y 32 at 16 px pitch, each = 3.5 r dot at c(53, y+8) + 11 text at (62, y) · chevron c(266,36).
The compliance card spends its first row on tri-state badges and starts its single text row at y 48.

## 3. The five tiles and what they bind to

| Tile | Table 6 | Face content (live) |
|---|---|---|
| ~~Identity & specifications (hero)~~ | D2.1 + #1 #2 #3 | **v4: replaced by the plain Product information lines. The spec chips and the documents statement have no home — see §1.2.** |
| Substances & safety | #5 #6 #7 #16 #17 | ● `no battery · lead-free solder` / ○ `no substance declaration made` |
| Compliance & certification | #3 #4 #22 | tri-state CE / RoHS / REACH badges · ● `WEEE cat. 5 · selective treatment` |
| Service & repair | #12 #15 #20 | ● `disassembly guide in this app` / ○ `spare parts · manuals not provided` |
| Usage & repair history | #19 | ● `design life 15 y · 225,000 km` / ○ `no measured use or repair data` |

## 4. The dot vocabulary (this is the honesty mechanism)

| Mark | Meaning | Source |
|---|---|---|
| ● filled `hand/pinch` or `teal/light` | declared / datasheet / measured | `basis` ∈ {declared, datasheet, measured} |
| ● filled dimmer | assumed / modelled | `basis` ∈ {assumed, modelled} |
| ○ hollow `text/tip` | not provided | `basis` = `not_provided`, or empty collection |
| pill outline only | tri-state badge, value `null` | `compliance.*` = null |

**v3:** the on-screen legend explaining this table is gone. The vocabulary itself is unchanged, and the
row text plus the dimmed colour still carry the meaning without it. This table stays here as the
authoritative definition for the thesis write-up.

⚠ **Every face value must be BOUND.** v1 shipped three static card subtitles; two of them disagreed with
the payload (builder said `166 × 121 × 41 mm` / type `F02U.V02.965-02`, payload had `200 × 150 × 60` /
`null`). No hardcoded data strings in v2+ — demo defaults only, overwritten by `Populate`.

## 5. Navigation

`Home` → `WelcomeController.ShowWelcome()` · tile → its detail shell (`PassportRouter.Open2..5`,
the shell's own back arrow returns to the landing) · `Continue` → `ScreenRouter.ShowModelExploration()`.

**v4:** `Open1` / `detail1` are unassigned — the Product information block is not tappable and
`IdentityDetail` is not built.

## 6. Sprites

`DPPSpriteFactory` provides IcPerson, IcWarning, IcShield, **IcWrench**, **IcClock**, IcChevron, IcBack,
Circle64, CircleRing, RoundedR13/R20/R22, Pill, Grip. v3 adds none — deleting the arrow does not remove
`IcBack`, which the shell pages and the other screen headers still use.

## 7. Open items

- [ ] Modals for the five tiles are **not** in this build round (Thiago, 2026-07-30: "build just the
      tabs, later we populate the modals"). Tiles are tappable; the shell bodies come next.
- [ ] Identity and Usage may not need a modal at all — the face already shows everything the payload holds.
- [ ] **Tab remodel** — the next step. **88 px** of vertical room is now free above the bottom bar.
- [ ] Do the status dots survive the remodel, given the row text already states the same thing?
- [ ] **A tab must claim the orphaned values** (§1.2): 5 spec chips, product category, the documents
      not-applicable statement, production date and country. Until then the app renders less than v3 did.
- [ ] Confirm the physical unit is marked **VCU0001** — the payload now says so.
- [x] `DPPSpriteFactory.IcPerson` is now referenced **only by its own generator** — no builder consumes
      it. Harmless (it just generates one unused PNG); prune it if the sprite set is ever tidied.

## 8. Iteration log

- **2026-07-29** — v1 built (four title-only cards + four modals) and device-tested.
- **2026-07-30 (a)** — CIRPASS Table 6 mapped (`13b`); 13 M / 9 U attributes; schema v0.6 then v0.7.
- **2026-07-30 (b)** — v2 designed: Table 6 categories, identity hero, dot vocabulary, Materials and
  Indicators moved to spec 14, CTA relabelled `Continue`. Built via `RBv2_0/7`.
- **2026-07-30 (c)** — **v3 coded.** Back arrow → `Home` pill in the bottom bar (briefed as
  `Main menu`, renamed on review — Welcome is not a menu); product title and
  dot legend deleted; caption moved to x 24; content shifted up 28 px. `MakeCaptionHeader` added so the
  shared `MakeScreenHeader` (intro + exploration + shells) keeps its arrow. `BuildWideCta` gained
  `primary` / `chevron` switches, defaulted so no existing call site changes.
  Mock `drafts/13_v3_dpp_canva.svg`.
- **2026-07-30 (d)** — **v4 coded.** Identity hero card → two plain bound text lines (`ProductName`
  16 bold, `SerialNumber` 11 tip); not tappable, `IdentityDetail` shell dropped. Spec chips, category
  caption, documents status line, production date and country all stop rendering — kept in
  `PassportView`, unwired, pending a later tab. `vcu_001.json`: model → `Vehicle Control Unit MS 50.4`,
  serial → `VCU0001`. Mock `drafts/13_v4_product_information.svg`.
- **2026-07-30 (e)** — **v4.1.** Serial moved onto the product-name line as trailing 11 pt
  `text/tip` (`… MS 50.4 - VCU0001`); two TMP objects with runtime measurement, because the
  name uses the bold font asset. `PassportView.PlaceSerialAfterName()` added.

*Last updated: 2026-07-31 · Status: v9 coded, awaiting device check · Prev: 12 Open App · Next: 14 Composition & impact*
