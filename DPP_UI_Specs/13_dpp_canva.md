# DPP UI Spec — 13: DPP Canva (product info) — **v2**

> **Living spec** — ReBuilt v2.0. Status: **v2 DESIGN APPROVED 2026-07-30, NOT BUILT.**
> v1 (built + device-tested 2026-07-29) was a 2×2 grid of four title-only cards opening full-page
> modals. v2 keeps the drill-down but puts **live data on the face of every tile**, and realigns the
> categories to CIRPASS D2.2 Table 6. Mock: `drafts/13_v2_C_dpp_canva.svg`.
> Information model + Table 6 coverage: **`13b_information_model.md`**. Builder: `RBv2_0/7`.

---

## 1. What changed in v2, and why

| | v1 (built) | v2 (this spec) |
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

## 2. Layout (panel 640 × 430; panel-local = mock SVG minus 20)

| Element | x | y | w | h |
|---|---|---|---|---|
| Back button (r20 visual / 50 hit) → **Welcome** | c42 | c44 | 40 | 40 |
| Eyebrow `Digital Product Passport` 11.5 | 76 | 24 | 300 | 15 |
| Title `Vehicle Control Unit` 19 bold | 76 | 40 | 440 | 24 |
| Right caption `scanned · vcu_001` 12.5, right | 316 | 36 | 300 | 16 |
| Separator `#1a335f` | 24 | 76 | 592 | 1 |
| **HERO — Identity & specifications** | 24 | 88 | 592 | 96 |
| Card — Substances & safety | 24 | 192 | 290 | 72 |
| Card — Compliance & certification | 326 | 192 | 290 | 72 |
| Card — Service & repair | 24 | 272 | 290 | 72 |
| Card — Usage & repair history | 326 | 272 | 290 | 72 |
| Legend (2 lines) 11 `text/tip` | 24 | 366 / 382 | 250 | 16 |
| Primary CTA `Continue ›` | 288 | 354 | 328 | 52 |

Hero stroke `tab/active-stroke`; cards stroke `row/stroke`; all fills `row/fill`. Hover outline per 00 §4.

**Hero internals:** person icon c(50,112 abs) · title 15 bold · right caption `EEE · WEEE cat. 5`
(`identity.product_category`) · identity line 13 (`manufacturer · model · serial · production · country`) ·
**spec chip row** — pills 20 high, `card/blue` fill, 10.5 text, one per non-null `specifications` field
(size · weight · protection class · supply voltage · operating temp · connectors) · dim status line for
the not-applicable documents.

**Card internals (290 × 72):** icon c(26,36) 22² · title (48,10) 14 bold · two status rows at
(48,30) and (48,46), each = 3.5 r dot + 11 text · chevron c(266,36).

## 3. The five tiles and what they bind to

| Tile | Table 6 | Face content (live) |
|---|---|---|
| **Identity & specifications** (hero) | D2.1 + #1 #2 #3 | identity line + spec chips + `energy label & technical documentation — not applicable (not a labelled product group)` |
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

⚠ **Every face value must be BOUND.** v1 shipped three static card subtitles; two of them disagreed with
the payload (builder said `166 × 121 × 41 mm` / type `F02U.V02.965-02`, payload had `200 × 150 × 60` /
`null`). No hardcoded data strings in v2 — demo defaults only, overwritten by `Populate`.

## 5. Navigation

Back → `WelcomeController.ShowWelcome()` · tile → its modal (`InfoTabRouter`, back arrow returns to the
grid) · `Continue` → `ScreenRouter.ShowModelExploration()`.

## 6. Sprites to add

`DPPSpriteFactory` needs **IcWrench** (Service & repair) and **IcClock** (Usage & repair history).
Existing: IcPerson, IcWarning, IcShield, IcChevron, IcBack, Circle64, RoundedR13/R20/R22, Pill, Grip.

## 7. Open items

- [ ] Modals for the five tiles are **not** in this build round (Thiago, 2026-07-30: "build just the
      tabs, later we populate the modals"). Tiles are tappable; the modal bodies come next.
- [ ] Identity and Usage may not need a modal at all — the face already shows everything the payload holds.

## 8. Iteration log

- **2026-07-29** — v1 built (four title-only cards + four modals) and device-tested.
- **2026-07-30 (a)** — CIRPASS Table 6 mapped (`13b`); 13 M / 9 U attributes; schema v0.6 then v0.7.
- **2026-07-30 (b)** — v2 designed: Table 6 categories, identity hero, dot vocabulary, Materials and
  Indicators moved to spec 14, CTA relabelled `Continue`.

*Last updated: 2026-07-30 · Status: v2 design approved, not built · Prev: 12 Open App · Next: 14 Composition & impact*
