# DPP UI Spec — RB2.1.1 / 06: Usage & service — **v2 (three lenses)**

> **Living spec.** Tab 1 of the super panel's data canvas. Supersedes `RB2_1/04a_use_phase.md` v1
> (five lenses, flat-page era — kept there as the derivation source and iteration history).
> Standards: `../RB2_1/00_design_standards_rbv2.md` · Parent: `04_DPP_page.md` · Stage: `05_digital_model_panel.md`
> Mock: `drafts/04a_v6_use_phase_three_lenses.svg` — **approved 2026-08-09, with decisions 1–3.**
> Payload: `unit_use_phase` (v0.13 block; schema now v0.19) · **Status: BUILT + device-validated.**
> **2026-08-08 (04e round 2):** the part record's OWN Back pill is REMOVED — it duplicated the
> page-bottom Back, which already closes the record first (`OnBack → CloseRecord`). One Back on
> screen, ever.

---

## 1. What this screen is

Telemetry the unit records **about itself** — never the vehicle, never the driver (the vehicle-
centric design and its movement map were deleted 2026-08-06; a product passport describes the
product). The screen answers the recycler's question: *is this unit still worth something?*

**Why it matters beyond the demo:** the LCA declares Sc4's functional reuse yield as `[A]`,
0.5–0.9, "not quantified in literature" — and Sc4 is the best scenario (58.0 kg CO2-eq). This
screen derives **0.767 by mass** from per-component verdicts (§3.7), inside that band. **The
passport supplies evidence for the assumption the LCA had to make.** That sentence belongs in
Results and Discussion.

## 2. The three lenses (v6 simplification, Thiago 2026-08-09)

v5's five lenses were merged to three. Pills sit at the top of the page — **no page title, no
subtitle**: the rail already names the tab (new general rule; 04c has complied since 08-06; the
Certificates page keeps its title because it is a reference page, not a tab).

| Lens | Content | Stage model |
|---|---|---|
| **Thermal Data** (default) | SOH + limiting mechanism · indicator bars · 0.767 reuse strip · verdict legend with masses · ΔT damage table · findings | **tinted BY VERDICT** (decision 1 — the heat-ramp tint died with it) |
| **Electrical Data** | ELECTRICAL section (transients · undervoltage · load dumps) + DATA section (flash · CPU · ECC · resets · CAN · DTC) | connectors + `ic_1` + `ic_2` lit `#4da3ff`, rest neutral |
| **Software** | firmware versions · calibration map changes · sensor recalibrations · DTC↔service linkage note | untinted (non-spatial) |

Merging rationale, in Thiago's words: the 48 % *is declared from the thermal content* — SOH and
the ΔT table belong on one page. Electrical + Data merged as one instrumentation page. History
renamed **Software**, content kept.

`basis: "simulated"` prints as a small footnote line, not a subtitle.

---

## 3. THE 48 % — the full derivation chain

**Rule: no free-standing invention.** Every figure is computed from something already in the
passport, and the computation is stated so it can be reproduced or challenged. Assumed values are
tagged **⚠ [ASSUMED An]** and collected in §4.

### 3.1 Ignition and thermal cycles

```
ignition_cycles = lifetime_distance_km / mean_trip_km
                = 225,000 / 20                          ⚠ [ASSUMED A1: 20 km mean trip]
                = 11,250
```

`225,000 km` is the LCA functional unit (`environmental.usage_profile` — sourced, not assumed).
The 20 km mean trip follows the payload's daily-use figure (~30 km · ~45 min, MiD 2017) at ~1.5
trips per driving day. **One ignition cycle = one thermal cycle**: the board warms from ambient
and cools again.

### 3.2 Coffin-Manson — cycles-to-failure at a given swing

A cycle count is not a wear measure: a 5 km errand that swings the board 15 °C is not a motorway
run that swings it 70 °C. Solder-joint fatigue life scales with the **amplitude** of the swing:

```
N_f(ΔT) = A · ΔT^(−n)          A = N_ref · ΔT_ref^n
```

Reference condition: **N_ref = 15,000 cycles at ΔT_ref = 40 °C** ⚠ [ASSUMED A2] with exponent
**n = 2.0** ⚠ [ASSUMED A3 — low end of the 2–3 range quoted for SnAgCu solder, so the estimate
is CONSERVATIVE; a higher n would punish the large swings harder].

```
A = 15,000 · 40² = 24,000,000
```

### 3.3 Miner's rule — linear damage accumulation over the ΔT histogram

The ΔT distribution itself is simulated telemetry ⚠ [ASSUMED A4 — the five-band split].

```
D = Σ  n_i / N_f(ΔT_i)
```

| ΔT band | ΔT mid | cycles n_i | N_f = 24,000,000 / ΔT_mid² | damage n_i / N_f |
|---|---|---|---|---|
| below 20 °C | 15 | 4,200 | 106,667 | 0.0394 |
| 20 – 40 °C | 30 | 4,600 | 26,667 | 0.1725 |
| 40 – 60 °C | 50 | 2,000 | 9,600 | **0.2083** |
| 60 – 80 °C | 70 | 420 | 4,898 | 0.0858 |
| above 80 °C | 90 | 30 | 2,963 | 0.0101 |
| **Σ** | | **11,250** | | **D = 0.5161** |

**The table is the argument, not the total:** the 30 most severe cycles contribute 1 % of the
damage; the 2,000 mid-range ones contribute 21 %. A cycle counter cannot see this — which is why
the screen shows the distribution, and why the damage bars stay in the UI.

```
fatigue life consumed  = 51.6 %
fatigue life remaining = 100 − 51.6 ≈ 48 %
```

### 3.4 Flash endurance — counted, not modelled

```
flash_write_cycles_used = ignition_cycles × writes_per_cycle
                        = 11,250 × 4                    ⚠ [ASSUMED A5: 4 writes/cycle]
                        = 45,000
endurance_remaining     = 1 − 45,000 / 100,000          ⚠ [ASSUMED A6: 100k conventional limit]
                        = 55 %
```

Four writes per ignition-off: statistics block, DTC block, learned adaptations, trip record.
This is the **hard** limit on the screen — a count against a datasheet figure.

### 3.5 State of health — minimum of mechanisms, no invented weights

**Rejected** (first draft, discarded same day): `SOH = 0.40·flash + 0.40·thermal + 0.20·(1 −
hours_above/powered)`. Three defects, all fatal in a viva: the weights were invented; the third
term is a near-constant (41 h of 5,625 ≈ 0.7 % → ~20 free points); and **averaging is the wrong
operator** — a component fails at its weakest mechanism, not at the mean of them.

**Adopted:**

```
SOH = min( flash endurance 55 % , thermal fatigue 48 % )
    = 48 %          limiting mechanism: thermal fatigue
```

No weights to defend, and the score *names* which mechanism runs out first — which is what a
remanufacturing decision needs. Mechanisms treated as independent ⚠ [ASSUMED A7 — a coupled
model would give a lower SOH].

Temperature excursions are **reported, not scored** (findings): 41 h above the 80 °C limit,
peak 94 °C, implicated in the 2025-12 processor failure at 220,260 km.

### 3.6 Reuse fraction by mass — 0.767

Per-component verdicts (`health.reuse_assessment`, payload v0.18):

| Verdict | Components | Mass |
|---|---|---|
| `reuse` | housing shells (344.0 g), fasteners (12.0 g) | 356.0 g |
| `reuse_after_test` | connectors (150.1 g) — SENS-B reseat 2022, contact resistance to verify | 150.1 g |
| `material_recovery` | PCB, four IC groups, passives, Ta caps, solder, coating, misc | 146.1 g |
| `consumable` | thermal interface material (8.0 g), renewed 2024 | 8.0 g |

```
reuse_fraction_by_mass = (344.0 + 150.1 + 12.0) / 660.2 = 506.1 / 660.2 = 0.767
```

Verdicts **follow the evidence on this screen**: board and actives go to material recovery
because 51.6 % of fatigue life is consumed and an over-temperature failure is already logged;
shells and fasteners have no electrical or thermal stress path.

## 4. Assumptions register — what the thesis must own

| # | Assumption | Value | Effect if wrong |
|---|---|---|---|
| **A1** ⚠ | mean trip length | 20 km | scales cycle count and flash wear linearly |
| **A2** ⚠ | Coffin-Manson reference | 15,000 @ ΔT 40 °C | scales ALL damage by one factor |
| **A3** ⚠ | Coffin-Manson exponent | n = 2.0 (conservative) | higher n → more damage from large swings |
| **A4** ⚠ | ΔT distribution | 5 simulated bands | shifts damage between bands |
| **A5** ⚠ | flash writes per cycle | 4 | scales flash wear linearly |
| **A6** | flash endurance | 100,000 (conventional automotive) | shifts the flash indicator only |
| **A7** ⚠ | mechanisms independent | min() valid | coupled model → lower SOH |

**A2, A3 and A5 set the absolute level and must be stated in the thesis.** The *shape* of the
damage distribution — what the screen argues from — is robust to all three.

⚠ Coffin-Manson / Miner still need a reliability citation (IPC-9701 or an automotive electronics
reliability text). **Do not write the equation into the thesis without one.** DPP-should-carry-
use-phase-data citations: see `RB2_1/04a_use_phase.md` §6 (four sources listed there).

---

## 5. The screen (approved v6 layout)

420 × 430 data canvas. TL coordinates.

* **Pill row** y≈22: three pills 120 × 32 at 126 pitch — Thermal Data · Electrical Data ·
  Software. Active = `#16305c` fill + teal stroke + white label; rest = `#0E2950` + `#21407a`.
  **Full elevation kit** (00 §4.1): shadow + gloss + hover rise — the first build shipped them
  flat (corrected same day). ⚠ The fills are state-coloured AND hover-brightened, so the state
  colour goes through `HoverHighlight.SetRestFillColor` (trap 1) — a direct `Image.color` write
  survives only until the next hover ease repaints the captured colour.
* **Content region** y≈56–385, one root per lens, exactly one active.
* **Bottom bar** y≈390: grey `Back` (record open → closes record; else `owner.PrevTab`) and the
  primary pill (label from `owner.PrimaryLabel`: Next / Continue to disassembly / Scan next
  product) → `owner.NextTab`.
* **Part record** (pinch on the stage while this tab is active): name, verdict chip + word, mass,
  reason box; Back returns to the active lens; pinching another (ghosted) body switches the
  record directly. Screws stay non-selectable. Selected part keeps true materials; the rest
  ghost at α 0.30 — the 05 machinery unchanged.

### Palette

| Meaning | Colour |
|---|---|
| `reuse` | `#2eb086` teal |
| `reuse_after_test` | `#f0c879` gold |
| `material_recovery` | `#21407a` — deliberately NOT red: it is the correct outcome for a PCB, not a fault |
| `consumable` | `#6f86a8` text/tip |
| ΔT damage bars | `#f08a3c` — **plain chart colour** (decision 2), like 04c's `#1f77b4`; never on the model |

Red keeps its four reserved meanings (`00` §2.1) and appears nowhere on this screen.

### Model tint map (through the model link)

Thermal Data → verdict colours per `reuse_assessment` (only the 9 mesh-bearing groups can tint;
board-material verdicts appear in the panel only). Electrical Data → `connectors`, `ic_1` (flash),
`ic_4` (bus interface) lit `#4da3ff`, rest neutral `#3a4a63`. Software → tint cleared. Leaving the
tab clears tint and selection — the other tabs get the model back exactly as 05 specifies.

## 6. Decisions log

* **2026-08-09 v5 → v6 (Thiago):** five lenses → three (Reuse+Thermal = *Thermal Data*;
  Electrical+Data = *Electrical Data*; History → *Software*, content kept). Page titles removed
  on all data tabs — general rule. Approved with: (1) Thermal Data tints by VERDICT, the heat
  ramp died; (2) `#f08a3c` is a chart colour, not a palette token; (3) Software = counts only —
  the payload carries totals, not itemised firmware/service rows, so the v1 scroll list had no
  data behind it.
* **2026-08-09 (adaptation):** lenses tint the STAGE model via the model link; on this tab a
  pinch opens the part's usage record (per-tab pick routing) — Component ID keeps the pinch on
  every other tab.
* **2026-08-09 (build round 2, Thiago):** lens pills were flat — shadow, gloss and rise added
  (00 §4.1), and the state colour rerouted through `SetRestFillColor` before the hover-vs-state
  fight could reach a device round.

## 7. Implementation map (build plan)

| Piece | File |
|---|---|
| View: three lenses, part record, populate from `unit_use_phase` | `Scripts/DDP/UI/UsePhaseView.cs` (new) |
| Lens tint + per-tab pick routing | `Scripts/DDP/UI/ModelLinkController.cs` (additive) |
| `ActiveTab` accessor | `Scripts/DDP/UI/SuperPanelView.cs` (one property) |
| Builder: page into the data canvas, wiring, tabPages MERGE | `Editor/DPPUIBuilder.UsePhase.cs` (new) — menu `RBv2_1_1/3 — Usage & service into the data canvas` |
| Verifier rows | `Editor/DPPUIBuilder.Verify.cs` |

Rebuild note: `/2` and `/3` both MERGE into `tabPages` (trap 4 in the round log — an overwrite
orphans sibling pages into permanently-active ghosts).

*Created 2026-08-09 · v2 · Status: approved for build (mock v6) · Parent: `04_DPP_page.md`*
