# 07 — Environmental Impact tab (04d) · RBv2.1.1

**Status:** v1 · 2026-08-08 · mock `drafts/04d_v2_environmental_impact.svg` APPROVED 2026-08-08.
**Scope:** the "Environmental impact" tab of the DPP page — tab index 2 of the data canvas
(420 × 430), between Usage & service (1) and Training disassembly (3).
**Single source of truth for every number:** `LCA_Analysis/LCA_Notebook/LCA_explorer.ipynb`
+ the openLCA v4 model (ecoinvent 3.8 APOS, EF 3.1) — concretely, the CSVs in
`LCA_Analysis/Outputs/`. **No number on this screen may exist anywhere else first.**

---

## 1. Why this screen exists

The thesis headline is environmental: guided disassembly is primarily a
**critical-raw-materials instrument** (minerals & metals = 72.5 % of the weighted EF 3.1
footprint), and the EoL scenario comparison shows what the DPP enables (−10 → −47 % minerals,
−6 → −21 % climate). This tab puts that argument INSIDE the passport, next to the physical
device it describes. Four sub-tabs carry it in order: what the life cycle IS (LCA explorer),
what matters in it (Main impacts), where it happens (Per stage), and what the DPP changes
(Recycling).

## 2. Data — every value with its source file

### 2.1 Gross life-cycle totals (Sc1 baseline, deterministic centrals)

Source: `LCA_Analysis/Outputs/3_impact_assessment/impact_EF31.csv`, column `sc1`.

| Category (EF 3.1, verbatim) | Sc1 total | Unit |
|---|---|---|
| Resource use minerals and metals | 0.018739 | kg Sb eq |
| Climate change | 73.4326 | kg CO2 eq |
| Eutrophication freshwater | 0.115920 | kg P eq |

Deterministic centrals only — **never** `mc_net.csv` means (SESSION_RESUME rule 1: independent
sampling understates the scenario gaps by ~⅔).

### 2.2 Screening shares (Main impacts pareto)

Source: `LCA_Analysis/Outputs/3_impact_assessment/impact_screening.csv`
(EF NW screening on Sc1; PEF ≥80 % cumulative rule + goal-pinned categories).

| Category | share_pct | cum_pct |
|---|---|---|
| Resource use minerals and metals | 72.45 | 72.45 |
| Climate change | 6.67 | 79.12 |
| Eutrophication freshwater | 6.58 | 85.70 |
| All 13 others (aggregated for the chart) | 14.30 | 100.00 |

Reporting set = these three (both goal-pinned categories inside it). Axis label on screen is
"share of weighted footprint" — never reuse this wording for ReCiPe normalisation
(SESSION_RESUME rule 5).

### 2.3 Recycling scenarios

Source: `impact_EF31.csv`, columns `sc*_net`, `sc*_saving` (net = gross − saving, §0.3
convention). Already in the payload since v0.7 as `environmental.impact_recovery` — the tab
reads THAT, it does not re-import the CSV.

| Reduction vs Sc1 | Sc2 | Sc3 | Sc4 |
|---|---|---|---|
| Minerals & metals | −10.0 % | −32.2 % | −47.3 % |
| Climate change | −6.0 % | −11.1 % | −21.0 % |
| Eutrophication freshwater | −5.9 % | −16.5 % | −25.7 % |

Sc4 carries its standing caveat verbatim: exploratory — functional reuse yield declared
`[A]` (0.5–0.9), not quantified in literature.

### 2.4 Per-stage contributions — COMPUTED 2026-08-08

Source: `LCA_Analysis/Outputs/3_impact_assessment/impact_stage_contributions.csv`, produced
by `stage_contributions.py` (§4) against the live openLCA v4 database. Run quality: drift
check 0/25 categories — the check column reproduces `impact_EF31.csv` **exactly**, and the
five stage values sum to the totals in every category. Values injected into both payload
copies (`basis: "modelled"`); the legacy `co2_kg` mirrors now sum to the 73.4326 headline
(the old assumed split summed to 63.9 and was deleted for it).

| Category | S1 | S2 | S3 | S4 | S5 (Sc1) |
|---|---|---|---|---|---|
| Minerals & metals [kg Sb eq] | 0.018335 (97.8 %) | 8.34e-5 | 6.23e-8 | 3.21e-4 | 3.00e-8 |
| Climate change [kg CO2 eq] | 38.488 (52.4 %) | 0.441 | 0.0279 | 34.300 (46.7 %) | 0.177 |
| Eutrophication FW [kg P eq] | 0.06253 (53.9 %) | 5.69e-4 | 1.81e-6 | 0.05282 (45.6 %) | 2.17e-6 |

Reading (matches the framework's frozen finding ③): the use phase is ~46–47 % of climate and
eutrophication, capping what EoL can do for carbon — while minerals are 97.8 % S1 materials,
which is why EoL strategy commands the mineral footprint and the DPP argument is a
critical-raw-materials argument. In the minerals panel the S2–S4 bars are sub-pixel at
linear scale; that is honest and stays (no log axis — it would misrepresent dominance), the
value labels carry the magnitudes.

### 2.5 Stage descriptions (LCA explorer cards) — SHORTENED round 2

Source: `LCA_Analysis/Docs/LCA_framework_v4.md` §3 (system boundary), condensed to fit the
pinwheel layout's card sizes (2026-08-08, matching Thiago's reference diagram; typos in the
reference — "coolection", "Manufcaturing", "STM" — corrected, not copied). These live in the
payload (`lifecycle_stages[].description`) and the UI renders THAT:

| Stage | Pill | Card text |
|---|---|---|
| S1 | Stage 1: Materials & construction | Raw-material extraction and refinement; manufacture of the electronic components. |
| S2 | Stage 2: Hardware assembly | Manufacturing at the provider: die-casting & SMT placement. |
| S3 | Stage 3: Distribution | Road freight of the finished unit to the vehicle OEM. |
| S4 | Stage 4: Use phase | Operation in a battery-electric vehicle in Germany. |
| S5 | Stage 5: End-of-life | Recycling stage — collection and treatment. See the Recycling tab. |

The long-form boundary texts stay in the framework doc; the passport carries the card
versions only.

## 3. UI — four sub-tabs (mock 04d_v2, approved)

General rules (unchanged from 04a): NO page title, NO subtitle — the rail names the tab.
Sub-tab pills at the very top with the full elevation kit (00 §4.1: shadow + gloss + hover
rise); pill fills are state-coloured AND hover-brightened → every state write goes through
`HoverHighlight.SetRestFillColor` (trap 1). Bottom bar = Back / Next (04 page grammar,
PrevTab / NextTab). **No model tint on this tab** (approved decision): entering it leaves the
stage model neutral; Usage's OnDisable already clears its own tint.

Pills: 4 × 88 × 32 at 93 pitch from x = 24 — "LCA explorer" (default) · "Main impacts" ·
"Per stage" · "Recycling".

### 3.1 LCA explorer — LIST (round 3, replaces the pinwheel)
History: v1 node ring → round 2 pinwheel per reference diagram → round 3 KILLED both
("the arrows are not pointing to nothing"). Final form: a plain list, one row card per
stage — bold title "Stage 1: Materials & construction" + description (§2.5) — five rows,
58 high at 65 pitch from y 62, payload-driven via `lifecycle_stages`. No graphic. The
authored `ic_lca_arrow.png` is retired; delete it in the next asset cleanup pass.

### 3.2 Main impacts (round 2: chart only)
Caption "SHARE OF THE WEIGHTED FOOTPRINT — EF 3.1 SCREENING · Sc1". Horizontal pareto,
4 rows (§2.2), bars scaled to the minerals share; "all others" dimmed. ALL text below the
chart removed (cumulative line, takeaway box, source line — round 2); the rows are
redistributed over the freed space at 70-unit pitch with 12-unit bars and 9 pt labels.

### 3.3 Per stage — grouped BY IMPACT (approved correction)
Three panels, one per reporting-set category, each with its OWN unit and scale:
Minerals & metals (kg Sb eq) · Climate change (kg CO2 eq) · Eutrophication freshwater
(kg P eq). Inside each panel four horizontal bars S1–S4, scaled to the panel's own maximum.
**S5 is excluded** (approved: the Recycling tab owns EoL). Round 2: panels 101 high at 107
pitch, ending y 377 — bottom gap mirrors the ~10-unit top gap under the pills. Values
render from the payload (modelled, §2.4); a payload without them shows the
"[pending openLCA]" watermark instead of bars.

### 3.4 Recycling (round 2: short scenario names)
Four scenario cards (2 × 2, 44 high), Thiago's naming (2026-08-08): **Scenario 1 —
Landfill & incineration · Scenario 2 — Bulk shredding and mechanical sorting · Scenario 3 —
Manual disassembly and shredding · Scenario 4 — Manual disassembly and functional reuse of
electronic components**. Sc4's card keeps "· exploratory [A]" — framework rule: Sc4 is
never presented without the caveat. ⚠ Wording note, flagged 2026-08-08: the LCA models
Scenario 3 as manual disassembly with the intact board fed DIRECTLY to the smelter —
deliberately bypassing shredding (framework §Sc3 "declared deviation"); the label "and
shredding" is Thiago's choice and stands, but the thesis text must not let the label
contradict the model. Below the cards: "NET REDUCTION VS Sc1 — EF 3.1", three category
groups × three scenario bars with percentages (§2.3) from `impact_recovery`; chart ends
y 362, leaving a proper margin above the Back/Next bar (~388).

## 4. Per-stage extraction — `stage_contributions.py`

**Method (why differencing, not the upstream-tree API):** the five stages chain linearly
(S5←S4←S3←S2←S1, each pulling 0.66 kg from its predecessor). A product system built from the
S_n process therefore contains stages 1..n exactly once, so its EF 3.1 total is the
CUMULATIVE impact through stage n — and the per-stage contribution is the difference of
successive cumulative totals. This reproduces the GUI contribution tree at stage level using
only IPC calls already proven in this repo (`ps_build.py` creation pattern +
`impact_runs.py` calculation pattern); the olca-ipc upstream-tree API is avoided because it
is unproven here.

What the script does (openLCA open, IPC on 8080, DB = the v4 model):
1. Resolves the four stage processes by prefix ("VCU S1"… "VCU S4"); aborts with candidates
   listed if any prefix is not unique (§0.2 discipline — no guessed names).
2. Creates product systems for them (idempotent: deletes same-named systems first;
   prefer-default-providers, unit processes).
3. Calculates the four + the existing `VCU S5 EoL Sc1 (no recycling)` system under
   EF 3.1 (adapted) — 5 calculations, ~10 min.
4. **Drift check:** Sc1 totals are compared against `impact_EF31.csv` column `sc1`; any
   category drifting > 0.5 % is logged loudly (it would mean the DB changed since 07-27).
5. Writes `Outputs/3_impact_assessment/impact_stage_contributions.csv` — all 25 categories ×
   S1..S4 direct + S5(Sc1) + total + check column + unit.
6. Injects the three reporting-set categories into `environmental.lifecycle_stages` of BOTH
   payload copies (explicit paths, §5), flips `lifecycle_stages_basis` to `modelled`, and
   stamps the note with the run date.

Run: `py "C:\Claude\Projects\AR_DPP\LCA_Analysis\Scripts\3_impact_assessment\stage_contributions.py"`

**GUI fallback if IPC misbehaves:** open each stage product system's EF 3.1 result in
openLCA → Contribution tree → the three categories, read the top-level split, and enter the
values by hand into `impact_stage_contributions.csv` (same columns); re-running the script
with `--inject-only` is NOT provided — hand-edit the payloads instead, both copies.

## 5. Payload v0.19

`schema_version` 0.18 → **0.19**. Both copies as always:
`XR/AR_DPP_VCU/backend/data/vcu_001.json` (live) + `backend/data/vcu_001.json` (mirror).

`environmental.lifecycle_stages` (empty since v0.7) becomes 5 stage objects:

```json
{ "id": "S1", "name": "Materials & construction", "co2_kg": 0.0,
  "description": "…§2.5 text…",
  "impacts": [
    { "category": "Resource use minerals and metals", "unit": "kg Sb eq",
      "value": null, "basis": "not_provided" },
    { "category": "Climate change", "unit": "kg CO2 eq", … },
    { "category": "Eutrophication freshwater", "unit": "kg P eq", … } ],
  "note": null }
```

`value: null` + `basis: "not_provided"` = render "[pending openLCA]" (closed basis
vocabulary — no new tokens). The script fills `value`, sets `basis: "modelled"`, and mirrors
the climate value into the legacy `co2_kg` field. S5's entry is Sc1's EoL burden with a note
pointing at `impact_recovery` for Sc2–Sc4. Model mirrors updated in the same commit:
`backend/models.py` (source of truth) + `Assets/Scripts/DDP/DPPModels.cs`
(`LifecycleStage` gains `description`, `impacts[]`, `basis`; new `StageImpact`).

## 6. Build & wiring

New editor phase: **`RBv2_1_1/4 — Environmental impact into the data canvas`**
(`Assets/Editor/DPPUIBuilder.EnvImpact.cs`), view `Assets/Scripts/DDP/UI/EnvImpactView.cs`.

- Builds `EnvironmentalPage` under `DataCanvas`; rebuild-safe (destroys own page only).
- tabPages **MERGE** (trap 4): pages[0] ProductSpecsPage · [1] UsagePage ·
  [2] EnvironmentalPage · [3] TrainingPage — re-found via SpFind, never overwritten.
- Baked demo values are the REAL §2 numbers (pareto + recycling); Per stage bakes the
  pending state — an offline build shows placeholders, not invented stage numbers.
- Verify map: `EnvImpactView.owner → RBv2_1_1/4`.
- Editor chain is now: `RBv2_1_1/1 → RBv2_1/9 → RBv2_1_1/2 → RBv2_1_1/3 → RBv2_1_1/4 →
  RBv2_1/Tools/Verify wiring → SAVE`. Never re-run `RBv2_0/4` or `/5` while the stage clone
  exists.

## 7. Open items

1. ~~Run `stage_contributions.py`~~ — DONE 2026-08-08: clean run, 0/25 drift, all four stage
   prefixes resolved uniquely ("VCU S1 Materials & Construction" / "VCU S2 Hardware
   Assembly" / "VCU S3 Distribution" / "VCU S4 Use Phase"), payloads injected (§2.4).
2. Device pass on the built page pending (pills, ring layout, panel bars, recycling chart).
3. The Per stage panels populate from the payload at runtime (DPPManager → Populate); the
   scene itself stays baked in the pending state on purpose — one source of truth, no
   number duplicated into the scene.
