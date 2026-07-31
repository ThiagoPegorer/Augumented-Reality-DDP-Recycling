# DPP UI Spec — 13c: Usage Profile (tile + detail page)

> **Living spec** — ReBuilt v2.0. Status: **CODED 2026-07-31 (spec 13 v11), awaiting device check.**
> Replaces the **Usage & repair history** tile. Mock (final): `drafts/13_v13_usage_profile_grid.svg`
> (rev G); earlier drafts `13_v11_use_data.svg`, `13_v12_usage_profile.svg`.
> Data source: **`environmental.usage_profile`** — the LCA use-phase model (S4), schema v0.11.
> Builder: `BuildUsageDetail` + the Usage tile block in `RBv2_0/7`.

---

## 1. What this page is

The **LCA use-phase model rendered as a passport page.** Thiago, 2026-07-31: *"review the data to
MATCH with the data of the 'S4 - Use phase' of our LCA model in the OpenLCA."* Every value is an S4
exchange or its documented source (`LCA_Analysis/Docs/LCA_framework_v4.md` §Stage 4), so the app and
the thesis LCA chapter cite the same numbers:

| Value | Figure | S4 source |
|---|---|---|
| Service life | 15 y | FU definition |
| Lifetime distance | 225,000 km | FU: 15,000 km/yr |
| Operating hours | 5,625 h | 225,000 ÷ 40 km/h |
| Average speed | 40 km/h | MiD 2017 `[L]`, range 35–45 |
| Own power draw | 9 W | Bosch MS 5.0 manual, family proxy `[L]`, range 9–20 |
| Charging efficiency | 0.765 | 0.85 AC charging `[L]` × 0.90 DC/DC `[A]` |
| **Own energy use** | **66.2 kWh / 15 y** | 9 × 5,625 ÷ 0.765 · DE low-voltage grid (MC 54–189) |
| Daily use | ~30 km · ~45 min | MiD 2017 |
| **Car energy use** | **39,375 kWh / 15 y** | **NOT S4** — estimate, 17.5 kWh/100 km avg BEV |

**Renames along the way:** Usage & repair history → Use data → **Usage Profile** (matches the
payload block name). The evolution v11→v12 flipped the page from *simulated telemetry* to the
*modelled profile* — measured-vs-design fictions (in-service 2011 backstory, 226,500 km odometer)
were dropped; the design values are shown exactly because they ARE the model.

## 2. Landing tile (290 × 72 @ panel-local 326, 260)

`Usage Profile` · chip pool (family position, y 41): **`15 years`** + **`225,000 km`** · `+` circle
(the only hit target; card inert). Same structure as the Electrical tile — both use the generalised
`BuildFaceChipPool`. Status rows 6/7 of the flat 8-array are dead; with 0/1 also dead (v10), only
2 (compliance) and 4/5 (service) remain live.

## 3. Detail page — 6 × 2 grid (rev G)

Content spans **y 88–418** panel-local: 12 px below the header rule, 12 px above the panel edge,
mirror-equal (rev F). Header: back circle + `Usage Profile` 19 pt at x 76 (no icon, Service
precedent). **No right caption** (removed rev F).

### 3.1 Left column — the km-per-year list (24, 88, 290 × 330)

The **only card and only touchable surface** on the page. *Chrome = touchable* — Thiago's collected
user feedback (2026-07-31): blocks around static content read as buttons.

- Head `KM DRIVEN PER YEAR` 10 pt teal-light + right-aligned range label ← `service_period`
  (`Apr 2011 – Mar 2026`).
- Viewport: `RectMask2D`, card-local (14, 28), 262 × 294 + transparent raycast HitArea.
  **`PinchScrollArea`** (shared with the Service log) scrolls the Content; 11 full rows visible at
  25 px pitch, the clipped 12th row is the scroll affordance.
- Row pool `UsageYearPool = 18`; view fills from `annual_distances[]`, sets Content height, hides
  spares. Partial years render `2011 · from Apr` in `text/caption`; full years `text/secondary`;
  km right-aligned bold white; 1 px hairline `#16335f` per row.

**The 16-row series** (simulated around the 15,000 km/yr FU; **sum asserted = 225,000** in the
generator and again at payload write):

| | | | |
|---|---|---|---|
| 2011 · from Apr — 11,300 | 2012 — 14,850 | 2013 — 15,620 | 2014 — 16,240 |
| 2015 — 14,380 | 2016 — 15,910 | 2017 — 13,970 | 2018 — 15,480 |
| 2019 — 16,050 | 2020 — 11,890 | 2021 — 13,540 | 2022 — 15,760 |
| 2023 — 16,180 | 2024 — 15,390 | 2025 — 14,840 | 2026 · to Mar — 3,600 |

Apr 2011 → Mar 2026 = 180 months = **exactly the 15.0 y the LCA models** — the partial end-years are
what reconcile "2011 to 2026" with S4. 2020 dips (pandemic year). ⚠ Accepted fiction: in-service
2011 predates the 2026 datasheet; nothing shows both dates together.

### 3.2 Right column — six plain stat groups (x 326, w 290)

No cards (not buttons). Rows at y 88 + i·57 (6 × 45 + 5 × 12 = 330). Titles 9.5 pt teal-light
**left** at x 326; values 16 pt bold white **centred** at cx 471. Hairlines `#12294e` between rows.

| # | Title | Value (bound) |
|---|---|---|
| 0 | TOTAL DISTANCE | `225,000 km` + **red bar** 290 × 4 |
| 1 | OPERATING HOURS | `5,625 h` |
| 2 | OWN ENERGY USE | `66.2 kWh` |
| 3 | CAR ENERGY USE | `39,375 kWh` |
| 4 | AVERAGE SPEED | `40 km/h` |
| 5 | DAILY USE | `~30 km · ~45 min` |

**The red bar** (`#e24b4a`, full width): design life consumed — the unit is at end of life and due
for recycling. **First state-use of red on a passport page; reserve the meaning.** (Presentational:
always full width, since the profile by definition describes the completed design life.)

## 4. Accepted risks (all user-directed, logged in order)

1. **No basis marker anywhere on the page.** The `estimated` tag on car energy (rev D) and the
   `modelled · LCA use phase (S4)` header caption (rev F) were both removed. The out-of-boundary
   39,375 kWh is visually indistinguishable from the five modelled S4 values. Defensible framing:
   the passport contents are the study stimulus, not the provenance labels — but the distinction now
   exists only here and in the payload (`basis`, `note` fields keep it machine-readable).
2. **Durations dropped from the energy values** (rev G): `66.2 kWh` carries no "/15 y" on screen;
   the span lives only in the list's date range.
3. **On-screen derivations removed** (rev B): nothing says 66.2 = 9 W × 5,625 h ÷ 0.765. Spec + LCA
   chapter carry it.
4. **The 3-fault / SENS-B cross-link line was cut** by the 6×2 layout (offer to relocate it into the
   Service log was not taken). The two simulated/modelled tabs no longer corroborate each other.

## 5. Data & code

- **Payload** `environmental.usage_profile` (schema v0.11): adds `annual_distances[16]`,
  `service_period`, `avg_speed_kmh`, `own_power_w`, `charging_efficiency`,
  `car_energy_kwh_estimate`, `daily_use`, `basis: "modelled"`.
  ⚠ **Stale-data fix:** `operating_hours` 4500 → **5,625** and `lifetime_energy_kwh` 216 → **66.2**.
  The old values were the v3 LCA's phantom-rating figures (48 W × 4,500 h), corrected by the v4
  framework but never propagated to the payload.
- `backend/models.py` + `DPPModels.cs`: new `AnnualDistance`; `UsageProfile` extended (doc updated —
  it now explicitly describes the S4 model and marks the car estimate as out-of-boundary).
- `DPPUIBuilder.DppCanva.cs`: `BuildUsageDetail`; `BuildElecChipPool` generalised to
  `BuildFaceChipPool(card, view, rowName, rootsField, labelsField, pool)` serving Electrical + Usage.
- `PassportView.cs`: `PopulateUsageProfile` + `SetStat`; the old rows-6/7 code (design-life dot row,
  repair-count row) deleted — repairs render on the Service tile only.
- No new sprites; `IcClock` stays on the tile.

## 6. Iteration log

- **2026-07-31 (v11 draft)** — "Use data": simulated telemetry, measured ≠ design (226,500 km),
  2011 backstory, ECU temp + fault tiles. Mock `13_v11_use_data.svg`.
- **2026-07-31 (v12)** — renamed **Usage Profile**, matched to LCA S4; vehicle-kWh boundary error
  caught (2,590 kWh/y proposal was ~600× the VCU's own draw); telemetry tiles → S4 parameters.
- **2026-07-31 (rev A–G)** — 4×2 → **6×2**; subtitles cut; chrome only on the scroll list;
  `estimated` tag cut; right column centred; header caption cut; margins mirrored; energy durations
  cut; **red end-of-life bar**; tile face at family chip position (earlier insets drew chips at
  y 31 — a mock error; the builder was always centred at y 41).
- **2026-07-31 (coded)** — schema v0.11, builder, view; 47 serialized refs verified, 0 orphaned;
  attribute sweep clean; year-series sum asserted 225,000 at write time.

*Last updated: 2026-07-31 · Status: coded, awaiting device check · Parent: 13_dpp_canva.md*
