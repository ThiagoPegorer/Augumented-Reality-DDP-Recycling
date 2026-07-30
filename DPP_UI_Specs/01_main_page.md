# DPP UI Spec — Screen 1: Main Page

> Page-level design specification for the AR Digital Product Passport (VCU).
> This document is the source of truth for the Unity implementation of the Main Page.
> Keep it in sync with the approved SVG mockup (embedded at the end).

---

## 1. Purpose & context

The Main Page is the **first screen shown after the worker scans the VCU's QR code**. Its only job is to present a calm, two-way fork: view the product passport (`Informations`) or start the guided recycling flow (`Disassembly`). Nothing else competes for attention.

- **User:** non-expert WEEE dismantling worker, gloved, time-pressured, wearing a PICO passthrough AR headset.
- **Design principle enforced here:** minimal cognitive load. Only the product serial and the two choices are shown. No specs, no weight, no safety banner on this page (safety lives inside `Informations` and the first `Disassembly` step).
- **Entry:** QR scan resolves a `product_id` → backend returns DPP payload → this page renders with the serial bound.
- **Exits:** tap `Informations` → Information tab · tap `Disassembly` → Disassembly intro (step 0).

---

## 2. Layout

Canvas (world-space panel) reference size: **680 × 470** units (viewBox). The navy panel is the **standardized 640 × 430** (per `00_design_standards.md`), inset 20 each side, at x 20 / y 20.

> Standardized 2026-06-08: panel height changed 370 → **430** to match all other screens. Content (serial + two cards) keeps its positions; the extra height adds breathing room below and lowers the tip line.

| Element | Type | x | y | w | h | radius | Notes |
|---|---|---|---|---|---|---|---|
| Panel | rect | 20 | 20 | 640 | 430 | 22 | Main navy surface (standardized 640×430) |
| Product serial | text | 50 | 120 (baseline) | — | — | — | Hero text, bound to `serial_number` |
| Informations card | rect | 50 | 190 | 280 | 100 | 20 | Left choice |
| Informations icon circle | circle | cx 96 | cy 240 | r 22 | — | — | Navy circle, "i" glyph |
| Informations title | text | 132 | 234 | — | — | — | "Informations" |
| Informations subtitle | text | 132 | 257 | — | — | — | "Passport & materials" |
| Disassembly card | rect | 350 | 190 | 280 | 100 | 20 | Right choice (primary path) |
| Disassembly icon circle | circle | cx 396 | cy 240 | r 22 | — | — | Teal circle, recycling glyph |
| Disassembly title | text | 432 | 234 | — | — | — | "Disassembly" |
| Disassembly subtitle | text | 432 | 257 | — | — | — | "Guided recycling · N steps" |
| Disassembly chevron | polyline | 602→610→602 | 232–248 | — | — | — | Affordance arrow |
| Drag tip | text | 50 | 422 | — | — | — | Drag feature hint (v1 — describes a working feature) |

> Within the taller 430 panel, the two choice cards are vertically centered (y 190) with the serial above and the tip line near the bottom (y 422).

Cards are laid out as **two equal columns**: 50 px left margin, 20 px gutter between cards (330→350), 50 px right margin. Each card is 280 wide.

---

## 3. Color tokens

Define these once as a shared palette; every screen reuses them.

| Token | Hex | Usage |
|---|---|---|
| `navy/panel` | `#0a1f44` | Main panel surface, Informations icon circle |
| `navy/card-resting` | `#13366b` | BOTH choice cards default (resting) fill |
| `navy/card-stroke` | `#2e5aa0` | BOTH choice cards border (2 px) |
| `teal/accent` | `#1d9e75` | Recycling icon circle, primary accent |
| `text/on-navy` | `#ffffff` | Serial, card titles |
| `text/subtitle-navy` | `#aac4e6` | Card subtitles, chevron |
| `text/tip` | `#6f86a8` | Drag tip line |

> Removed 2026-06-10: `grey/card` `#e9edf3`, `grey/card-stroke` `#c7d2e0`, `text/on-grey`, `text/subtitle-light` — the light-grey Informations card style is no longer used on this screen (see revision note above).

> Note: the `Informations` card (light grey) and `Disassembly` card (lighter blue `#13366b`) are in their **resting** state in the approved mockup's intent. See §5 for interaction states.

> **Revision 2026-06-10 (Unity testing):** the Informations card no longer uses the light-grey style — it now matches the Disassembly card (`card/blue` `#13366b` fill, 2 px `#2e5aa0` stroke, white title, `#aac4e6` subtitle, icon circle stays `navy/panel`). Rationale: on the headset-scale panel the grey read as plain white and the white hover outline was indistinguishable from the fill. The embedded SVG mockup (§9), color tokens (§3) and interaction states (§5) are updated accordingly. Implemented in Unity (DPPUIBuilder, Phase 1).

---

## 4. Typography

- **Font family:** SF Pro (`SF Pro Display` for ≥18 px, `SF Pro Text` for <18 px). In Unity: import the SF Pro TTF into a TextMeshPro font asset. Fallback stack: `-apple-system, Helvetica, Arial, sans-serif`.
- **Case:** sentence case everywhere.

| Role | Size | Weight |
|---|---|---|
| Product serial (hero) | 32 | Bold (700) |
| Card title | 18 | Bold (700) |
| Card subtitle | 13 | Regular (400) |
| Icon "i" glyph | 21 | Regular (400) |
| Drag tip | 12.5 | Regular (400) |

---

## 5. Interaction states

**Global hover rule (applies to all buttons across the app):** the **white / bright-outline highlight appears only on hover** (when the worker's pinch-ray or gaze passes over an element). The resting state uses the element's normal fill. No element sits permanently in the highlighted state.

| Element | Resting | Hover (ray/gaze over) | Pressed/selected |
|---|---|---|---|
| Informations card | lighter-blue fill `#13366b`, 2 px stroke `#2e5aa0` | white highlight outline, subtle lift | navigates to Information tab |
| Disassembly card | lighter-blue fill `#13366b`, 2 px stroke `#2e5aa0` | white highlight outline (brighter), chevron emphasised | navigates to Disassembly intro |

Selection action: pinch (PICO native hand tracking) routed via `PicoHandUIBridge` → `Button.onClick`.

---

## 6. Data bindings

| UI field | Source (DPP payload) | Fallback |
|---|---|---|
| Product serial | `identity.serial_number` | `"VCU-DEMO-001"` (demo) |
| Disassembly step count ("N steps") | `disassembly.total_steps` | `5` (demo) |

All other text is static UI copy. No live data on the Informations/Disassembly *labels* themselves.

---

## 7. Behaviour notes

- The page renders only **after** a successful DPP fetch; show a lightweight loading state between scan and render (spec TBD).
- `Disassembly` is the **primary path** — it may carry slightly stronger visual weight than `Informations`, but both remain fully available.
- **Reposition (immersive agency, V1):** the panel carries the standard **grabber bar** docked below it (dark centered pill + grip, per `00_design_standards.md` §5). The worker grabs the bar to move the canvas in AR space. This **replaces** the earlier "Tip: drag this panel anywhere" text line — the bar is the affordance, so the tip text is dropped (the embedded SVG below still shows the old tip line; treat the grabber bar as authoritative). Add a `grabber bar` element centered below the panel (~y 478, w 200, h 22).
- No safety banner on this page by design.

---

## 8. Open items / future

- [ ] Loading state between QR scan and page render.
- [ ] Draggable-panel (immersive agency) — **v1**; implement grab + move.
- [ ] Confirm whether step count is always shown or hidden until data loads.
- [ ] Real product serial + step count wired from backend.

---

## 9. Approved SVG mockup

```svg
<svg width="100%" viewBox="0 0 680 470" role="img" xmlns="http://www.w3.org/2000/svg">
<title>VCU DPP main page (standardized 640x430 panel)</title>
<desc>Navy panel showing serial VCU-DEMO-001 and two choice cards, Informations and Disassembly with a recycling symbol, plus a drag tip.</desc>

<rect x="20" y="20" width="640" height="430" rx="22" fill="#0a1f44"/>

<text x="50" y="120" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="32" font-weight="bold" fill="#ffffff">VCU-DEMO-001</text>

<rect x="50" y="190" width="280" height="100" rx="20" fill="#13366b" stroke="#2e5aa0" stroke-width="2"/>
<circle cx="96" cy="240" r="22" fill="#0a1f44"/>
<text x="96" y="248" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="21" fill="#ffffff" text-anchor="middle">i</text>
<text x="132" y="234" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="18" font-weight="bold" fill="#ffffff">Informations</text>
<text x="132" y="257" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="13" fill="#aac4e6">Passport &amp; materials</text>

<rect x="350" y="190" width="280" height="100" rx="20" fill="#13366b" stroke="#2e5aa0" stroke-width="2"/>
<circle cx="396" cy="240" r="22" fill="#1d9e75"/>
<g transform="translate(396,240)" fill="#ffffff">
<g>
<path d="M -1.8 -9 L 1.8 -9 L 5.2 -3 L 8.4 -4.9 L 7 2 L 0.5 0.5 L 3.7 -1.3 L 0 -7.6 Z"/>
</g>
<g transform="rotate(120)">
<path d="M -1.8 -9 L 1.8 -9 L 5.2 -3 L 8.4 -4.9 L 7 2 L 0.5 0.5 L 3.7 -1.3 L 0 -7.6 Z"/>
</g>
<g transform="rotate(240)">
<path d="M -1.8 -9 L 1.8 -9 L 5.2 -3 L 8.4 -4.9 L 7 2 L 0.5 0.5 L 3.7 -1.3 L 0 -7.6 Z"/>
</g>
</g>
<text x="432" y="234" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="18" font-weight="bold" fill="#ffffff">Disassembly</text>
<text x="432" y="257" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="13" fill="#aac4e6">Guided recycling · 5 steps</text>
<polyline points="602,232 610,240 602,248" fill="none" stroke="#aac4e6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>

<text x="50" y="422" font-family="-apple-system, 'SF Pro Display', 'SF Pro Text', Helvetica, Arial, sans-serif" font-size="12.5" fill="#6f86a8">Tip: drag this panel anywhere in your space for a better view</text>
</svg>
```

---

*Last updated: 2026-06-08 · Status: approved · Next screen: Information tab (02), Disassembly intro (03)*
