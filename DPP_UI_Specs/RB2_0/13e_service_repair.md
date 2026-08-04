# DPP UI Spec — 13e: Service & repair (tile + detail page, v13 rebuild)

> **Living spec** — ReBuilt v2.0. Status: **CODED 2026-07-31 (spec 13 v13), awaiting device check.**
> Replaces the v9/v10 page (spec 13 §1.4) wholesale. Mocks: `drafts/13_v15_service_audit.svg`
> (the audit that motivated this), `drafts/13_v16_service_1x2.svg` (approved layout).
> Data: `service.software_updates[60]` + `repair_history.events[25]`, both `basis: simulated`.

---

## 1. Why the rebuild

The v9 page was built one day before the standards existed. The audit (13_v15) found **eight
deviations**: chromed static counter cards (+ subtitles), a chromed timeline card, the amber basis
caption, non-mirrored margins, the last dot-row tile face, a 5-of-12 truncated log — and one
**confirmed bug**: the firmware range used `→` (U+2192), absent from both SF Pro SDF atlases,
rendering a missing-glyph box on device. The v9 history also **postdated the unit's own
retirement**: fortnightly updates Mar–Jul 2026 against a service end of Mar 2026 (13c).

## 2. The rebuilt history (Thiago's design, 2026-07-31)

Anchored to the Usage Profile timeline (Apr 2011 – Mar 2026):

- **60 software updates** — quarterly, automatic, `v{serviceYear}.{quarter}`:
  **v1.1 (01 Apr 2011) → v15.4 (01 Jan 2026)**. Thiago wrote "end at v15.1"; quarterly to the end
  of service mathematically ends at v15.4 — built so, flagged, unchallenged.
- **25 repairs** — none in service years 1–5; **2/year in years 6–10** (Sep + Feb); **3/year in
  years 11–15** (Jun + Oct + Feb). First: 12 Sep 2016 · last: 03 Feb 2026. Rising failure rate
  toward end of life — the narrative argument for the red bar's recycle verdict (13c §3.2).
  Descriptions rotate over real components (connector reseats, housing seal, TIM renewal,
  fasteners, coating, PCB remount) with `exchanged_component_ids` links.
  **`cost_eur: null` on every event — prices removed on instruction (2026-07-31).**
- Everything dated before the Mar 2026 retirement. Nothing in the future.

## 3. Layout — 1 × 2, two scrollable histories

Header: back circle + `Service & repair` 19 pt, **no icon, no caption** (amber `simulated data`
removed — consistent with 13c rev F / 13d; the `simulated` basis survives in the payload only).
Content y 88–418, mirror margins.

| | LEFT (24, 88, 290 × 330) | RIGHT (326, 88, 290 × 330) |
|---|---|---|
| Head | `REPAIRS · 25` (bound) | `SOFTWARE UPDATES · 60` (bound) |
| Meta (right-aligned) | `first: Sep 2016` (computed) | `automatic · every 3 months` (computed from mean gap) |
| Rows | 34 px, two-line: amber dot · date (caption) / description (truncated) | 25 px, one-line: teal dot · date (caption) · version right-bold |
| Pools | `SvcRepairPool 27` | `SvcUpdatePool 62` |

Both cards are `PinchScrollArea` windows — the only chrome, both genuinely touchable. Ordering
oldest-first (family rule, like the Usage year list). Content heights set by the view.

**`MakeScrollWindow`** extracted: the 262 × 294 masked viewport + content + PinchScrollArea
construction, now shared by both service cards (Usage and Compliance still carry their earlier
inline copies — candidates for the same helper in a cleanup pass).

## 4. Tile face

`Service & repair` · family chips **`60 updates`** + **`25 repairs`** (computed) · `+` circle.
The last dot-row face is gone — **all five tiles are now chip tiles**, and with it the entire
`statusDots`/`statusTexts` machinery died: builder arrays, `SetRefArray`s, view fields,
`SetRow`, `PopulateStatusTiles`. `SetDot` survives (documents dot in `PopulateIdentity`).

## 5. What died in code

v9/v10 remnants deleted, not orphaned: counter/timeline/log builder blocks, `SvcTickPool` /
`SvcMonthPool` / `SvcLogPool` / `SvcTrackX/W/AxisY`, `svcUpdateCount/Caption`, `svcRepairCount/
Caption`, `svcVersionRange` (and with it the `→` bug), `svcTicks`, `svcRepairMarker`,
`svcMonthTicks/Labels`, `svcLog*`, `PlaceMonths`, `FillLog`, `SetLogCell`, `TrackX`.
`CadenceCaption` rewritten to report months when the gap is month-scale.

## 6. Incident log (recorded because the process is part of the thesis)

The view patch's region-cut anchored on the *fields-area* "Tile status rows" comment instead of
the methods-area one and silently deleted `Populate()`, `PopulateIdentity`,
`PlaceSerialAfterName`, `FillChips`/`LayoutChips` and the composition/scenario/recovery field
blocks. Brace balance stayed clean — only the builder↔view reference-parity check caught it
(15 missing fields). Recovered from `git show HEAD` (post-v10 state) plus the archived v11 patch
text; every restored piece re-verified. Lesson applied: region cuts now anchor on
signature + section pairs, never on a bare section comment.

## 7. Verification

Braces/parens/brackets balanced (builder 81/703/103 pairs; view 113/460/231); **43 builder→view
refs, 0 missing, 0 orphaned**; all 12 Populate* entry points present exactly once; attribute
sweep clean; payload asserts: 60 updates v1.1→v15.4, 25 repairs 2016-09→2026-02, all dates
< 2026-04, all costs null.

*Last updated: 2026-07-31 · Status: coded, awaiting device check · Parent: 13_dpp_canva.md · Siblings: 13c, 13d*
