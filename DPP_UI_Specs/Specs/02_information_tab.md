# DPP UI Spec — Screen 2: Information Tab

> Page-level design specification for the AR Digital Product Passport (VCU).
> Source of truth for the Unity implementation of the Information tab.
> Keep in sync with the approved SVG mockups (embedded at the end).
> Shares the global palette, typography and hover rule defined in `00_design_standards.md` / `01_main_page.md`.

> **Revision 2026-06-10 (v3, approved):** the accordion + scroll architecture (v2) is
> replaced by a **category card grid + modal detail pages**:
> - The tab landing view shows **five category cards** in a grid — everything fits the
>   standardized 640×430 panel with **no scrolling at all**.
> - Tapping a card opens that category as a **full-page modal** with a **top-left back
>   arrow** (same position as Home, so the hand already knows the spot). No bottom
>   back button.
> - Rationale (AR ergonomics): no scroll gesture needed, larger pinch targets, simple
>   two-level navigation, and each category gets the whole panel when open.
> - Content and data bindings are unchanged from v2 (backend schema v0.3: lifecycle
>   stages, recovery potential, usage profile, precious metals, specifications,
>   compliance; EoL *scenario comparison* stays in the thesis LCA model, not the UI;
>   hazard styling is conditional/neutral for this product).
> - Cost accepted: one extra tap per category; no side-by-side category comparison.

---

## 1. Purpose & context

The Information tab is the **passport / lookup side** of the DPP. It presents the CIRPASS D2.2 Table 6 information requirements for the scanned VCU, grouped into five categories.

- **Primary user:** an inspector / auditor / OEM / curious worker who wants to *read* the passport — not the dismantling walkthrough (that is the Disassembly tab).
- **Reached from:** Main Page → tap `Informations`; or the tab bar from anywhere.
- **Design principle:** calm by default — the landing view is just five labelled doors. Detail lives one tap deeper, where it gets the full panel.
- **Transparency principle:** the passport states *what is known and how it is known* — estimated BOM lines, modelled use phase and unverified compliance are labelled as such.

---

## 2. Architecture: two-level navigation

```
Information tab (landing)            Modal detail (one per category)
┌────────────────────────┐           ┌────────────────────────┐
│ [home]  [Info][Disasm] │   tap     │ [←back]  crumb + title │
│ ┌────────┐ ┌────────┐  │  card     │                        │
│ │Identity│ │Material│  │ ───────▶  │   full-page category   │
│ ├────────┤ ├────────┤  │           │   content (no scroll)  │
│ │Hazard  │ │Complnce│  │ ◀───────  │                        │
│ ├────────┴─┴────────┤  │   back    │                        │
│ │ Life cycle (wide) │  │           │                        │
└─└───────────────────┘──┘           └────────────────────────┘
```

- Exactly one view visible at a time within the Information tab screen.
- **No ScrollRect anywhere** — both levels fit the 430 panel.
- Unity mapping: landing grid + 5 modal page GameObjects under the InformationTab
  screen root; a small modal router (open/close) toggles them. Back → landing grid.

---

## 3. Layout — landing view (canvas 680 × 470 reference, panel 640 × 430)

| Region | x | y | w | h | Notes |
|---|---|---|---|---|---|
| Panel | 20 | 20 | 640 | 430 | radius 22 |
| Header zone | 44 | 36 | 592 | 60 | Home + tabs (identical to v2) |
| Separator line | 44→636 | 96 | — | — | 1 px `#1a335f` |
| Card grid | 44 | 106 | 592 | 300 | 2 columns + full-width last row |
| Grabber bar | centered | below panel | 200 | 22 | per `00` §5 |

**Grid geometry:** columns 290 wide, gutter 12 (44 / 346); rows 92 high, gap 12
(y 106 / 210 / 314). The LCA card spans the full 592 width on the last row.

### 3.1 Header (identical to v2)

| Element | Type | x | y | w | h | radius | Fill / stroke |
|---|---|---|---|---|---|---|---|
| Home button circle | circle | cx 62 | cy 64 | r 20 | — | — | `#13366b` fill, `#2e5aa0` stroke 1.5 |
| Home icon (house) | path | translate(62,64) | — | — | — | — | white stroke 2 |
| Tab — Informations (active) | rect | 150 | 46 | 180 | 38 | 19 | `#0d2a57` + `#2e5aa0` stroke |
| Tab — Disassembly (inactive) | rect | 342 | 46 | 180 | 38 | 19 | `#324a6d` |

Tab states and hover: per `00` §4 / v2 (unchanged). Home → Main Page.

---

## 4. Category cards (landing view)

| Property | Value |
|---|---|
| Size | 290 × 92 (LCA: 592 × 92) |
| Radius | 13 |
| Fill | `#0e2950` |
| Stroke | `#21407a` 1 px (LCA: `#1d9e75` 1.4 px) |
| Icon | left, cx 28 from card edge, vertically centered, teal `#7fd3b6` stroke 2 (LCA icon `#5dcaa5`) |
| Title | x 52 from card edge, 16 px bold white, wraps to two lines if needed |
| Subtitle / badge | under title, 12–12.5 px (see per-card table) |
| Chevron | right edge, points **right** (→ navigates), `#9fb3d1` |
| Hover | white outline + subtle lift (global rule) |

| Card | Position | Icon | Subtitle (collapsed info) |
|---|---|---|---|
| Identity & manufacturer | (44, 106) | person | — (title only, two lines) |
| Materials & substances | (346, 106) | layers | — (title only, two lines) |
| Hazardous & safety | (44, 210) | warning triangle (teal when neutral) | `no battery · lead-free`, `#9fb3d1` |
| Compliance & end-of-life | (346, 210) | shield-check | — (title only, two lines) |
| Life cycle analysis | (44, 314, full width) | lifecycle arrows (two opposing circular arrows, ↻ — replaced the leaf/arc glyph 2026-06-10, which read as a blob at 24 px) | `63.9 kg CO2e lifecycle · up to 6.6 recoverable`, `#9fe1cb` |

> **Hazard card conditional styling** (carried over from v2 §5.4): with no hazards
> (this product) the card is standard navy with the teal triangle icon and the neutral
> badge. If `contains_battery` / `hazardous_warnings` / any `components[].hazardous`
> is true → red set: fill `#2a1d2e`, stroke `#7a3a4a`, red `!` circle icon, title
> `#f3b6b6`, badge per hazard `#d98a8a`. Red always means "act differently".

> The LCA card subtitle is a **live summary** bound to
> `environmental.co2_footprint_kg` + `recovery_potential.total_avoidable_kg`.

---

## 5. Modal detail pages — common chrome

| Element | Type | x | y | Notes |
|---|---|---|---|---|
| Back button circle | circle | cx 62 | cy 64 | r 20, `#13366b` fill, `#2e5aa0` stroke 1.5 — same position as Home on the landing view |
| Back icon (← arrow) | path | translate(62,64) | — | white stroke 2, round caps |
| Category icon | path | ~(106, 64) | — | category icon, small, `#7fd3b6` (LCA `#5dcaa5`) |
| Category title | text | 122 | 70 | 19 px bold white, vertically centered on the back button |

> Breadcrumb ("Informations" caption above the title) **removed after Editor testing
> 2026-06-10** — redundant with the back arrow and added visual noise. The embedded
> mockup B still shows it; treat this table as authoritative.
| Separator | line | 44→636 | 96 | 1 px `#1a335f` |
| Content area | — | 44 | 106→436 | full width, no scroll |

- **Back** → returns to the landing grid (not to Main Page). One affordance only —
  the top-left arrow. No bottom back button (decided 2026-06-10).
- Tab pills are **not** shown inside a modal; the back arrow is the only chrome action.
- Hover: white outline on the back button per global rule.

### 5.1 Field-row modal pages (Identity, Materials, Hazardous, Compliance)

Field rows reuse the v2 style, now full-width: label left 13 px `#8ba3c4` at x 44;
value right-aligned 13.5 px white ending at x 636; pitch 26–30 px starting y ~130.

**Identity & manufacturer** (6 rows — bindings unchanged from v2):

| Label | Value (vcu_001) | Source |
|---|---|---|
| Manufacturer | Bosch Motorsport | `identity.manufacturer` |
| Model | Vehicle Control Unit MS 50.4 | `identity.model` |
| Type number | F02U.V02.965-02 | `identity.type_number` |
| Production | 2026-03 · DE | `identity.production_date` + `country_of_origin` |
| Specifications | 166×121×41 mm · 660 g · IP67 | `specifications.*` |
| Service life (design) | 15 y · 225,000 km | `environmental.usage_profile` |

**Materials & substances** (6 rows):

| Label | Value | Source |
|---|---|---|
| Housing | Die-cast aluminium (AlSi) · 363 g | component(`housing`) |
| Connectors | Brass + Au/Ni plating · 58 g | component(`connectors`) |
| PCB assembly | FR-4 · Cu · ≈185 g | Σ components with `disassembly_step == 3` |
| Active components | Silicon · 20 g | component(`actives`) |
| Precious metals | Au 63 · Ag 251 · Pd 28 mg | `precious_metals[]` (value `#9fe1cb`) |
| Recycled content | — | `environmental.recycled_content_pct` (null → `—`) |

**Hazardous & safety** (4 rows, neutral case):

| Label | Value | Source |
|---|---|---|
| Contains battery | No | `end_of_life.contains_battery` |
| Hazardous substances | None documented | `end_of_life.hazardous_warnings` |
| Solder | Lead-free SnAgCu (SAC305) | component(`solder`).material |
| WEEE treatment | Selective treatment recommended | `end_of_life.recycling_route` |

**Compliance & end-of-life** (5 rows): CE marking / RoHS / REACH (`—` until verified,
from `compliance.*`), WEEE category, Recycling route.

> With ≤6 rows in a 300-px content area, field-row modals may use a relaxed pitch
> (~34 px) and start lower (~y 140) so pages don't feel top-heavy. Implementation
> may center the block vertically; exact pitch is a builder constant.

### 5.2 Life cycle analysis modal (custom layout — see mockup B)

Content identical to v2 §6 but laid out across the full panel:

| Block | Position (svg) | Notes |
|---|---|---|
| Headline | label y136 · `63.9` 36 px bold y176 + unit · caption y198 | `environmental.co2_footprint_kg` |
| Recovery potential panel | rect 350,112 286×110 r10 `#0a2344` | title 12.5 `#9fe1cb`; 4 bars h11 (top `#5dcaa5`, rest `#1d9e75`), labels 11.5 `#bbccdd`; data `recovery_potential` (bars = gross credits, headline = net) |
| Stage contribution strip | caption y242 · bar 44→636 h12 y250 | segments ∝ share: S1 `#2e5aa0`, S2 `#1d9e75`, S3 `#5dcaa5`, S4 `#324a6d`; data `lifecycle_stages[]` |
| Stage grid 2×2 | rows y276 / y302 | swatch + label 12.5 `#8ba3c4` + value 13 white right-aligned (cols end x310 / x636) |
| Method row | divider y334 · row y358 | `Method` → `ISO 14040 · GWP100 (AR6) · estimated BOM` |
| Footnote | y380 | `*modelled use profile · recovery net of process emissions`, 11.5 `#6f86a8` |

---

## 6. Color tokens (additions to global palette)

| Token | Hex | Usage |
|---|---|---|
| `card/fill` | `#0e2950` | Category cards, (former row fill) |
| `card/stroke` | `#21407a` | Category card border |
| `card/icon-tint` | `#7fd3b6` | Category icons |
| `field/label` | `#8ba3c4` | Field labels |
| `field/value` | `#ffffff` | Field values |
| `divider` | `#21407a` | Dividers |
| `header/separator` | `#1a335f` | Header separator |
| `breadcrumb` | `#7f9bc4` | Modal breadcrumb |
| `hazard/*` | (see §4) | Red set used ONLY when hazards exist |
| `lca/stroke` | `#1d9e75` | LCA card + accents |
| `lca/icon` | `#5dcaa5` | LCA icon, top recovery bar, S3 segment |
| `lca/summary` | `#9fe1cb` | LCA card subtitle, recovery title, precious metals value |
| `lca/bar-label` | `#bbccdd` | Recovery bar labels |
| `stage/s1..s4` | `#2e5aa0` `#1d9e75` `#5dcaa5` `#324a6d` | Stage segments + swatches |
| `chevron` | `#9fb3d1` | Card chevrons, neutral hazard badge |

(Scroll tokens from v2 are retired — no scrolling in v3.)

---

## 7. Typography

(SF Pro, sentence case — per `00` §3.)

| Role | Size | Weight |
|---|---|---|
| Tab label | 15 | Bold (active) / Regular (inactive) |
| Card title | 16 | Bold |
| Card subtitle / badge | 12–12.5 | Regular |
| Modal breadcrumb | 11.5 | Regular |
| Modal title | 19 | Bold |
| Field label | 13 | Regular |
| Field value | 13.5 | Regular |
| LCA headline number | 36 | Bold |
| LCA unit | 16 | Regular |
| LCA captions / method / grid | 11.5–13 | Regular |

---

## 8. Interaction states

| Element | Resting | Hover | Tap |
|---|---|---|---|
| Tab pills | active=navy+outline, inactive=slate | white outline | switch tab |
| Home (landing) | `#13366b` + outline | white outline | → Main Page |
| Category card | navy (red only if hazard) | white outline + lift | opens that category's modal |
| Back (modal) | `#13366b` + outline | white outline | → landing grid |

All selection via pinch (`PicoHandUIBridge` → Button.onClick); mouse works in Editor.

---

## 9. Data bindings (summary — schema v0.3, unchanged from v2)

| UI element | Source | Fallback |
|---|---|---|
| Identity fields | `identity.*`, `specifications.*` | `—` |
| Service life | `environmental.usage_profile` | `—` |
| Materials fields | `components[]` (aggregated), `precious_metals[]` | `—` |
| Recycled content | `environmental.recycled_content_pct` | `—` |
| Hazard card state + fields | `end_of_life.*`, `components[].hazardous` | neutral |
| Compliance fields | `compliance.*` | `—` |
| LCA card subtitle | `co2_footprint_kg` + `recovery_potential.total_avoidable_kg` | `—` |
| LCA modal blocks | `lifecycle_stages[]`, `recovery_potential`, `method`, notes | hidden |

---

## 10. Behaviour notes

- Landing grid is the default view whenever the Information tab is entered.
- Opening a modal hides the grid; Back restores it. State (which modal) need not persist across tab switches.
- Hazard card styling is **data-driven** (neutral vs red).
- LCA is read-only in v1.
- **Grabber bar (v1):** standard bar below the panel (`00` §5) — moves the canvas; applies to both levels.

## 11. Open items / future

- [ ] Verify CE/RoHS/REACH/WEEE values; real `recycled_content_pct`.
- [ ] Completion summary (09) must stay numerically consistent with `recovery_potential`.
- [ ] Optional: slide/fade transition between grid and modal (DOTween) — polish, not v1.
- [ ] Loading/empty states for missing fields.

---

## 12. Approved SVG mockups (v3, 2026-06-10)

> Standard 640×430 panel — no tall-panel workaround needed anymore.
> Source files: `DPP_UI_Specs/drafts/02_v3_A_category_grid.svg`, `02_v3_B_modal_lca.svg`.

### 12.1 Landing view — category card grid

```svg
<svg width="100%" viewBox="0 0 680 520" role="img" xmlns="http://www.w3.org/2000/svg">
<title>Information tab v3 A — category card grid, no scrolling</title>
<desc>Navy DPP panel with home button and tabs, then five category cards in a grid: Identity, Materials, Hazardous, Compliance, and a full-width Life cycle analysis card. Tapping a card opens a modal detail page.</desc>
<rect x="20" y="20" width="640" height="430" rx="22" fill="#0a1f44"/>
<circle cx="62" cy="64" r="20" fill="#13366b" stroke="#2e5aa0" stroke-width="1.5"/>
<g transform="translate(62,64)" fill="none" stroke="#ffffff" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M-7 1 L0 -6 L7 1"/><path d="M-5 0 L-5 7 L5 7 L5 0"/>
</g>
<rect x="150" y="46" width="180" height="38" rx="19" fill="#0d2a57" stroke="#2e5aa0" stroke-width="1.5"/>
<text x="240" y="70" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="15" font-weight="bold" fill="#ffffff" text-anchor="middle">Informations</text>
<rect x="342" y="46" width="180" height="38" rx="19" fill="#324a6d"/>
<text x="432" y="70" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="15" fill="#c2cee0" text-anchor="middle">Disassembly</text>
<line x1="44" y1="96" x2="636" y2="96" stroke="#1a335f" stroke-width="1"/>

<rect x="44" y="106" width="290" height="92" rx="13" fill="#0e2950" stroke="#21407a" stroke-width="1"/>
<g transform="translate(72,152)" fill="none" stroke="#7fd3b6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<circle cx="0" cy="-4" r="4"/><path d="M-6 9 a6 7 0 0 1 12 0"/></g>
<text x="96" y="146" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">Identity &amp;</text>
<text x="96" y="166" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">manufacturer</text>
<polyline points="306,144 314,152 306,160" fill="none" stroke="#9fb3d1" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>

<rect x="346" y="106" width="290" height="92" rx="13" fill="#0e2950" stroke="#21407a" stroke-width="1"/>
<g transform="translate(374,152)" fill="none" stroke="#7fd3b6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M-5 -5 L5 -5 L5 5 L-5 5 Z"/><path d="M-2 -1 L2 -1 M-2 2 L2 2"/></g>
<text x="398" y="146" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">Materials &amp;</text>
<text x="398" y="166" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">substances</text>
<polyline points="608,144 616,152 608,160" fill="none" stroke="#9fb3d1" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>

<rect x="44" y="210" width="290" height="92" rx="13" fill="#0e2950" stroke="#21407a" stroke-width="1"/>
<g transform="translate(72,250)" fill="none" stroke="#7fd3b6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M0 -8 L8 6 L-8 6 Z"/><path d="M0 -2 L0 1.5"/><path d="M0 4 L0 4.01"/></g>
<text x="96" y="248" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">Hazardous &amp; safety</text>
<text x="96" y="270" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12" fill="#9fb3d1">no battery · lead-free</text>
<polyline points="306,248 314,256 306,264" fill="none" stroke="#9fb3d1" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>

<rect x="346" y="210" width="290" height="92" rx="13" fill="#0e2950" stroke="#21407a" stroke-width="1"/>
<g transform="translate(374,250)" fill="none" stroke="#7fd3b6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M-5 -6 L5 -6 L5 6 L-5 6 Z"/><path d="M-2 0 L0 2 L3 -3"/></g>
<text x="398" y="248" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">Compliance &amp;</text>
<text x="398" y="268" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">end-of-life</text>
<polyline points="608,248 616,256 608,264" fill="none" stroke="#9fb3d1" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>

<rect x="44" y="314" width="592" height="92" rx="13" fill="#0e2950" stroke="#1d9e75" stroke-width="1.4"/>
<g transform="translate(72,358)" fill="none" stroke="#5dcaa5" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M-6 0 a6 6 0 1 1 6 6"/><path d="M0 6 L0 -2 M0 6 L-4 3 M0 6 L4 3" stroke-width="1.6"/></g>
<text x="96" y="352" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" font-weight="bold" fill="#ffffff">Life cycle analysis</text>
<text x="96" y="374" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#9fe1cb">63.9 kg CO2e lifecycle · up to 6.6 recoverable</text>
<polyline points="608,352 616,360 608,368" fill="none" stroke="#9fb3d1" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>

<rect x="240" y="462" width="200" height="22" rx="11" fill="#0a0e16" stroke="#2a3344" stroke-width="1"/>
<rect x="318" y="471" width="44" height="4" rx="2" fill="#6b7686"/>
</svg>
```

### 12.2 Modal detail — Life cycle analysis

```svg
<svg width="100%" viewBox="0 0 680 520" role="img" xmlns="http://www.w3.org/2000/svg">
<title>Information tab v3 B — Life cycle analysis modal detail</title>
<desc>Navy DPP panel showing the LCA category as a full modal page: top-left back arrow button, breadcrumb, title, lifecycle headline, recovery potential panel, stage contribution bar with grid, and method row.</desc>
<rect x="20" y="20" width="640" height="430" rx="22" fill="#0a1f44"/>

<circle cx="62" cy="64" r="20" fill="#13366b" stroke="#2e5aa0" stroke-width="1.5"/>
<g transform="translate(62,64)" fill="none" stroke="#ffffff" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M3 -6 L-4 0 L3 6"/><path d="M-4 0 L8 0"/>
</g>
<text x="96" y="58" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#7f9bc4">Informations</text>
<g transform="translate(104,70)" fill="none" stroke="#5dcaa5" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
<path d="M-6 0 a6 6 0 1 1 6 6"/><path d="M0 6 L0 -2 M0 6 L-4 3 M0 6 L4 3" stroke-width="1.6"/></g>
<text x="122" y="76" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="19" font-weight="bold" fill="#ffffff">Life cycle analysis</text>
<line x1="44" y1="96" x2="636" y2="96" stroke="#1a335f" stroke-width="1"/>

<text x="44" y="136" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="13" fill="#8ba3c4">Lifecycle CO2 footprint</text>
<text x="44" y="176" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="36" font-weight="bold" fill="#ffffff">63.9</text>
<text x="132" y="176" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="16" fill="#9fb3d1">kg CO2e</text>
<text x="44" y="198" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12" fill="#6f86a8">per unit · cradle-to-grave</text>

<rect x="350" y="112" width="286" height="110" rx="10" fill="#0a2344" stroke="#21407a" stroke-width="1"/>
<text x="366" y="134" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#9fe1cb">Recovery potential — up to 6.6 kg CO2e</text>
<rect x="366" y="146" width="134" height="11" rx="3" fill="#5dcaa5"/>
<text x="508" y="156" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#bbccdd">Aluminium · 3.2</text>
<rect x="366" y="163" width="113" height="11" rx="3" fill="#1d9e75"/>
<text x="508" y="173" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#bbccdd">Gold · 2.7</text>
<rect x="366" y="180" width="17" height="11" rx="3" fill="#1d9e75"/>
<text x="508" y="190" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#bbccdd">Palladium · 0.4</text>
<rect x="366" y="197" width="18" height="11" rx="3" fill="#1d9e75"/>
<text x="508" y="207" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#bbccdd">Other metals · 0.4</text>

<text x="44" y="242" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#6f86a8">Stage contribution</text>
<rect x="44" y="250" width="83" height="12" fill="#2e5aa0"/>
<rect x="127" y="250" width="8" height="12" fill="#1d9e75"/>
<rect x="135" y="250" width="2" height="12" fill="#5dcaa5"/>
<rect x="137" y="250" width="499" height="12" fill="#324a6d"/>

<rect x="44" y="276" width="8" height="8" fill="#2e5aa0"/>
<text x="58" y="284" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#8ba3c4">S1 Raw materials</text>
<text x="310" y="284" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="13" fill="#ffffff" text-anchor="end">8.9</text>
<rect x="366" y="276" width="8" height="8" fill="#1d9e75"/>
<text x="380" y="284" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#8ba3c4">S2 Manufacturing</text>
<text x="636" y="284" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="13" fill="#ffffff" text-anchor="end">0.9</text>
<rect x="44" y="302" width="8" height="8" fill="#5dcaa5"/>
<text x="58" y="310" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#8ba3c4">S3 Distribution</text>
<text x="310" y="310" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="13" fill="#ffffff" text-anchor="end">0.1</text>
<rect x="366" y="302" width="8" height="8" fill="#324a6d"/>
<text x="380" y="310" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#8ba3c4">S4 Use phase*</text>
<text x="636" y="310" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="13" fill="#ffffff" text-anchor="end">54.0</text>

<line x1="44" y1="334" x2="636" y2="334" stroke="#21407a" stroke-width="1"/>
<text x="44" y="358" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#8ba3c4">Method</text>
<text x="636" y="358" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="12.5" fill="#ffffff" text-anchor="end">ISO 14040 · GWP100 (AR6) · estimated BOM</text>
<text x="44" y="380" font-family="-apple-system,'SF Pro Display','SF Pro Text',Helvetica,Arial,sans-serif" font-size="11.5" fill="#6f86a8">*modelled use profile · recovery net of process emissions</text>

<rect x="240" y="462" width="200" height="22" rx="11" fill="#0a0e16" stroke="#2a3344" stroke-width="1"/>
<rect x="318" y="471" width="44" height="4" rx="2" fill="#6b7686"/>
</svg>
```

---

*Last updated: 2026-06-10 · Status: approved (v3 — card grid + modals) · Prev: 01 Main page · Next: 03 Disassembly intro*
