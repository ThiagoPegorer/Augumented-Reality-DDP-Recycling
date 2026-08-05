# DPP UI Spec — RB2.1 / 04: DPP page — **v1**

> **Living spec.** Supersedes RB2.0 `13` (DPP Canva), `13b`–`13e` (detail pages) and `14`
> (Composition & impact): one panel screen, four tabs, one free-floating model.
> Standards: `00_design_standards_rbv2.md` · Routine: `RB2_1_routine.md` · Prev: `03` · Next: `05`
> Mock: `../drafts/04_v11_dpp_canva.svg` · Builder: `RBv2_1/1 — DPP page` · View: `DppPageView.cs`
> **Status: phase 1 BUILT and device-tested 2026-08-05. The four `+` targets are stubs.**

---

## 1. Why this screen exists in this form

Two findings from P02 and P03 drove the rebuild:

| Finding | Cause | Answer |
|---|---|---|
| "Too much information — what do I do with it?" | facts with no task attached | four tabs instead of six tiles + three blocks, and a **Training disassembly** tab that gives the facts a job |
| "I didn't perceive there was a 3D model there" | the model lived one screen *after* the information | the model is **persistent and free-floating**, and it spawns **before** the panel (§7) |

Thiago, 2026-08-04: *"Split it is being confuse and not a smart way for the user."* RB2.0's two
passport screens merge here. The merge is only safe because spec `14`'s composition data becomes
something the **model** shows, not more rows in the panel (§7).

## 2. Screen anatomy (panel 640 × 430, panel-local coordinates)

| Element | x | y | w | h | Notes |
|---|---|---|---|---|---|
| Back arrow (circle) | 24 | 24 | 40 | 40 | **Product user only** — 52 px hit area |
| Title `Digital Product Passport` | 24 *(76 with arrow)* | — | — | 25 bold | vertically centred in the 0–76 header band |
| Compliance badge (button) | 436 | 23 | 200 | 30 | §5 |
| Header rule | 24 | 76 | 592 | 1 | `#1a335f` |
| Tab 1 Product specifications | 24 | 90 | 290 | 118 | |
| Tab 2 Usage history | 326 | 90 | 290 | 118 | |
| Tab 3 Environmental impact | 24 | 218 | 290 | 118 | |
| Tab 4 Training disassembly | 326 | 218 | 290 | 118 | **both roles** — a DPP without the disassembly steps is not a DPP (Thiago, 2026-08-05) |
| Left button | cx 114 | cy 376 | 180 | 52 | label depends on role (§4) |
| Primary CTA | cx 422 | cy 376 | 388 | 52 | label + target depend on role (§4) |

Columns and the button row are the standard geometry of `00` §1.1 and §5 — nothing moves between
this screen and the others.

### 2.1 Tab anatomy

Fixed header row, pinned so the four `+` buttons stay on one line across the grid:

- icon 28 × 28 at (18, 14) — authored PNG, rendered as drawn, no tint
- title 14 bold `text/on-navy` at x 56, baseline y 28
- `+` button, 40 visual / 52 hit, centred at (260, 30)

Content band **y 48 → 110**, and the chip stack is **vertically centred inside it**. Tiles carry
different numbers of rows, so a fixed top would leave three of the four looking bottom-heavy.

## 3. The chip — the only content element on this screen

`00` §5 addition, agreed 2026-08-04:

| Property | Value |
|---|---|
| Fill | `card/blue` `#13366b`, capsule sprite |
| Height | 18 · corner radius = h / 2 |
| Text | 10.5 `#dbe4f0`, single line |
| **Width** | **`label.preferredWidth + 24`, measured at runtime** |
| Alignment | centred by default · **left-aligned (12 px padding) when the chip carries a label-plus-value pair** |
| Sets | chips that belong to one set share the **widest** width in the set |

⚠ **Never hardcode a chip width.** Every value on this screen comes from the payload; one extra
digit clips a fixed chip. The builder bakes a starting width only so the mock reads correctly in
the editor — `DppPageView.Populate()` re-fits every chip after binding.

## 4. The two roles

`ScreenRouter.Mode` is set on the stakeholder screen (`03`) and read here in `OnEnable`. **One
screen, built once, in two modes** — building two canvases would double every future DPP edit.

| | **Product user** | **Recycler** |
|---|---|---|
| Header back arrow | **shown** → `ScreenRouter.ShowStakeholder()` | **hidden** |
| Title x | 76 (arrow occupies 24) | 24 |
| Left button | **`Quit`, RED** → `WelcomeController.ShowWelcome()` | `Back`, grey → `ScreenRouter.ShowStakeholder()` |
| Primary CTA | `Scan next product` → `QRScanController.BeginNewScan()` | `Continue to disassembly` → `ScreenRouter.ShowDisassembly()` |
| Tab 4 Training disassembly | shown | shown |
| Tabs 1–3 | identical | identical |

**Why the back affordance moves.** The Recycler's bottom bar is a `Back` / `Continue` pair, which
is the clearest possible reading of a linear step. The Product user has no forward step, so the
bottom-left slot is free for `Quit` and the one-step-back edge returns to the header arrow. A
participant is only ever one role, so the affordance never moves under them mid-session.
**This is a deliberate exception to `00` §5**, which defines Back as the header circle.

**`Quit` is red** (`00` §2.1 meaning 3 — the action ends the session) and follows §5's rule that
"an edge that leaves the session says Quit, never Back". It leaves the **session**, not the app.
One baked button cannot be both a grey `Back` and a red `Quit`, so the left slot is built by a local
`DpPill` helper whose fill, stroke and label the view recolours in `ApplyMode()`.

**Spec `03` matched on 2026-08-05:** the stakeholder screen's `Close app` became **`Quit`** (red),
returning to Welcome. `StakeholderSelect.CloseApp()` → `Quit()` with a `welcome` reference. In a
kiosk loop `Application.Quit` ends the study for everyone behind the current participant; quitting to
Welcome ends it for one. The real `Application.Quit` stays on the Welcome screen.

## 5. Compliance badge and the certificates screen

The badge is a **button**, 200 × 30, 50 px hit area, reading `CE · REACH · WEEE 5 · IP67` beside
the certificates shield. It is present on the screen regardless of tab, because compliance is a
property of the product, not of one tab. Treatment follows `00` §2.1 **meaning 4**: `safety/stroke`
as a 1.4 px **outline and glyph only, never fill**, and the label always names what it marks.

`IP67` lives here rather than on tab 1: Thiago, 2026-08-04 — *"since this a security and safe
acronym"*.

**`Certificates & safety` is a SCREEN, not a modal** (changed 2026-08-05). It is a sibling of
`DppPage` under `DPPPanelCanvas`, owned by `ScreenRouter.ShowCertificates()`, with the same
`navy/panel` surface as the main panel and a close **X** at the header's right (cx 598, cy 38,
36 visual / 52 hit) returning to `ShowDppCanva()`.

⚠ **Why it stopped being a modal.** It always covered the whole panel, so it was a page pretending
to be an overlay. Worse, `PicoHandUIBridge` resolves a click by intersecting the hand ray with the
**canvas plane**, and a child overlay shares that plane with the page: the old 388 px `Close` pill
sat on the primary CTA's exact coordinates (cx 422, cy 376) and fired it — "Scan next product" for
the Product user, "Continue to disassembly" for the Recycler. **Any overlay sharing a canvas plane
with live controls will do this.**

⚠ **Rule that comes with it:** a new panel screen MUST be added to `ScreenRouter.Show()`'s
`DeactivateUnless` pass. Omitting it for `stakeholderDecision` caused the 2026-08-04 regression where
every downstream button returned to the DPP canvas.

Content — four rows over the full band, 96 → 400, step 76, chip 92 wide, text column at 132:

| Chip | Colour | Content |
|---|---|---|
| `CE` | `teal/light` | Conformity marking · declared under 2014/30/EU (EMC) 09 Oct 2020 · tested to ECE R10 rev.6. Assessment is only valid once installed in the final product. |
| `REACH` | **`safety/stroke`** | Chemicals regulation · 2 substances of very high concern declared above 0.1 % w/w: lead (CAS 7439-92-1) and lead monoxide (CAS 1317-36-8). |
| `WEEE 5` | `teal/light` | Category 5, small equipment · selective treatment recommended at end of life. Do not dispose of in household waste. |
| `IP67` | `teal/light` | Dust tight, protected against temporary immersion. Declared for the product; **the printed demonstrator is not sealed.** |

Chips are **vertically centred against their own paragraph**, not pinned to the first line.
A chip is red **only when the marking itself reports something adverse** — currently `REACH` alone.

All four texts come from `compliance` and `substances_of_concern` in the payload.

## 6. Tab faces — what each headline is bound to

| Tab | Chips | Payload source |
|---|---|---|
| 1 Product specifications | `Vehicle Control Unit MS 50.4` (own row) · `Bosch Motorsport` · `VCU0001` | `identity.model`, `identity.manufacturer`, `identity.serial_number` |
| 2 Usage history | `66.2 kWh` · `225,000 km` · `5,625 h` | `environmental.usage_profile.lifetime_energy_kwh / lifetime_distance_km / operating_hours` |
| 3 Environmental impact | `CO2 Emissions 73.25 kg CO2 eq` · `Minerals & Metals 0.01874 kg Sb eq` · `Eutroph. Freshwater 0.11592 kg P eq` | Thiago-supplied life-cycle-stage figures ⚠ see below · `environmental.impact_recovery[]` |
| 4 Training disassembly | `5 steps` · `10 actions` · `~5 min` | `disassembly.steps[]` (count, action count, `estimated_minutes`) |

Left-aligned, equal-width set on tab 3 only.

⚠ **Open data issue, carried until resolved.** `73.25` does **not** match
`LCA_Analysis/Outputs/3_impact_assessment/impact_EF31.csv`, which gives **73.4326** kg CO2 eq for
`sc1`. The other two values match that same `sc1` column exactly (`0.0187391` Sb eq, `0.11592`
P eq). Either the climate figure excludes the incomplete end-of-life stage while the other two
include it — in which case the tile shows **two boundaries under one heading** — or one number is
wrong. This appears in the thesis results as well; it must be settled before the screen is cited.

⚠ **Bosch Motorsport on the face** states that this object *is* a Bosch unit, while `00` §8 calls
the prototype "a generic 5-step VCU (inspired by the Bosch Motorsport MS 50.4, generic as-built)".
Thiago's call, made deliberately 2026-08-04.

## 7. The model (specified here, built in `05`)

- **Free-floating, never inside the panel.** Embedding it would cost the 3D liberty that made it
  the best-liked part of the prototype.
- **Persistent for the whole passport phase** — it spawns once on arrival from `03` and does not
  disappear between tabs.
- **Spawn sequence:** the model appears **alone**, centred, settles and rotates slowly (~1 s),
  *then* the panel unfolds from its edge. You cannot overlook the only object in the scene.
- **Panel → model highlight, both directions.** Rule: *a highlight always answers "where does this
  fact live?" — and sometimes the answer is the whole device.* Device-level facts pulse the whole
  assembly; part-level facts light the parts. Tab 3 tints the model by material and glows by
  recovery value. Model → panel is the same map inverted, reusing `ZonePartInteraction`.

## 8. Icons

`Assets/Textures/Icons/` — authored PNGs, rendered as drawn:
`ic_certificates` (badge + modal) · `ic_product_specs` · `ic_environmental` · `ic_usage_history` ·
`ic_training`.

⚠ Three sources arrived at 6000 × 3375 with the glyph occupying ~8 % of the canvas; they were
trimmed to content, squared on transparency and resampled to 256 × 256. **Import at 256, not at
source size** — at 28 px on screen an untrimmed source resolves to about 5 px of artwork.

⚠ **Glyph clash:** `ic_environmental` (recycling arrows) is the same glyph as `ic_recycler` on the
stakeholder screen, where it means *"you are a recycler"*. One glyph, two meanings, two screens
apart. Unresolved.

## 9. Build and wiring

**Menu: `RBv2_1/1 — DPP page`.** Safe to re-run: destroys and rebuilds only `DppPage`.

The builder **does not destroy the RB2.0 `DppCanva`**. It deactivates it and renames it
`DppCanva_RB2_0_legacy`, then re-points `ScreenRouter.dppCanva` at the new `DppPage`. Reason:
`DPPManager.passport`, `PassportView` and the RB2.0 detail pages are still referenced elsewhere,
and destroying them mid-rebuild would strand those references. **Delete the legacy object once the
per-tab work replaces its detail pages** (`05`–`08`).

Wiring set by the builder: `DppPageView.router / welcome / scanner / backButton / leftButton /
primaryButton / trainingTile / certificatesModal` + every chip root and label.
`DPPManager.dppPage` is added so `Populate` runs on every successful fetch — one extra field and
one extra line in `OnDPPSuccess`, alongside the existing view calls.

Run `RBv2_0/Tools/Verify wiring` after building, then **save the scene**.

## 10. Phase plan

| Phase | Scope | State |
|---|---|---|
| 1 | Four tabs + header + badge + certificates + role-driven buttons | ✅ **built 2026-08-04, device-tested and fixed 2026-08-05** |
| 2 | Tab 1 `+` — model, drag bar, explode, per-component detail, R/L side tab | ⬜ next |
| 3 | Tab 2 `+` — usage + service log, scroll region from the start (`13` v10 defect 3) | ⬜ |
| 4 | Tab 3 `+` — impact detail, recovery rates, model tint by material and value | ⬜ |
| 5 | Tab 4 `+` — step summary before the real run | ⬜ |

The four `+` buttons call `DppPageView.OpenTab1..4`, which currently log and do nothing else.
They are the seams the per-tab work plugs into.

## 11. Open items

1. `73.25` vs `73.4326` — which boundary do the three figures share (§6).
2. `ic_environmental` / `ic_recycler` glyph clash (§8).
3. ~~Tab 4 hidden for the Product user~~ — **resolved 2026-08-05: shown for both roles.**
4. The small caption line was removed from every tile on 2026-08-04. Nothing on the face now says
   what is behind a `+`. Two tiles have the room if it should come back.
5. ~~Whether the stakeholder screen gains a `Home` button beside `Close app`~~ — **resolved
   2026-08-05: `Close app` became a red `Quit` returning to Welcome; there is no separate Home.**
6. `RBv2_0/Tools/Verify wiring` does not yet cover `StakeholderSelect.welcome`, `ScreenRouter.certificates`
   or any `DppPageView` field — a dangling reference in the new screen passes silently.

## 12. Iteration log

- **2026-08-05** — device test. Three defects fixed: chips collapsed to 24 px stubs (TMP reports
  `preferredWidth = 0` while the screen is inactive — now re-fitted in `OnEnable` after
  `ForceMeshUpdate`); icons missing (`AssetDatabase.Refresh()` + `spriteImportMode = Single`);
  certificates background to `navy/panel`. Then: Training disassembly restored for both roles, `Home`
  → red `Quit`, and the certificates modal promoted to a screen (§5).
- **2026-08-04** — mocks v1 → v11 (`../drafts/04_v*.svg`): 2 × 2 grid, centred then left headline,
  compliance badge promoted to a button with a modal, small text removed, chip standardised,
  authored icons landed. Spec written from v11; phase 1 built.

*Created 2026-08-04 · Status: phase 1 built · Legacy source: `../RB2_0/13*.md`, `../RB2_0/14_model_exploration.md`*
