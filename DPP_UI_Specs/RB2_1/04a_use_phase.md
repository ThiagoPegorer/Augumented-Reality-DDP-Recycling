# DPP UI Spec — RB2.1 / 04a: Use phase — **v1**

> **Living spec.** The `+` target of tab 2 (Usage history) on the DPP page (`04`).
> Standards: `00_design_standards_rbv2.md` · Parent: `04_DPP_page.md` · Mock: `../drafts/04a_v4_use_phase.svg`
> Payload: `unit_use_phase` (schema v0.13) · **Status: specified, not built.**

---

## 1. What this screen is, and what it stopped being

The tab was first designed around the **vehicle**: lifetime distance, energy, a DACH driving map,
car maintenance. That was dropped on 2026-08-06.

**A product passport describes the product.** A recycler holding this board does not need to know that
the car had new tyres in 2017, and a movement profile is personal data they have no basis to see.
What they need is whether **this unit** is still worth something. Everything on this screen is
telemetry a vehicle control unit records **about itself**.

The `usage_map` block, its two model classes and the mock row that showed it were all deleted the same
day they were written. Recorded here so the deletion is not mistaken for an oversight.

## 2. Why it matters beyond the demo

Spec `14` §4 declares scenario **Sc4's functional reuse yield as `[A]`, 0.5–0.9, "not quantified in
literature"** — and Sc4 is the best scenario in the LCA at 58.0 kg CO2-eq. The reason literature cannot
supply that number is that nobody knows whether a used unit is still good.

This screen produces **0.767 by mass** from per-component verdicts (§4.5), which falls inside that
band. **The passport supplies evidence for the assumption the LCA had to make.** That is the sentence
this screen exists for, and it belongs in Results and Discussion.

---

## 3. The data — `unit_use_phase`, six blocks

| Block | Contents |
|---|---|
| `exposure` | powered hours, ignition cycles, ΔT histogram, fatigue reference + damage, board temperature peak/limit/hours-above, hours-by-temperature histogram |
| `electrical` | logged transients (ISO 7637-2), undervoltage events, load dumps |
| `compute` | CPU hours above 80 %, flash write cycles used / limit / remaining, ECC corrected errors, unexpected resets |
| `diagnostics` | CAN error frames, bus-off events, DTC total / active / cleared, DTCs linked to service events |
| `calibration` | firmware versions installed, first/last, calibration map changes, sensor recalibrations |
| `health` | SOH, limiting mechanism, indicators, **findings**, per-component reuse verdicts, reuse fraction |

`basis: "simulated"` on the whole block. Component ids match `components[]`.

---

## 4. Derivations

**Rule applied throughout: no free-standing invention.** Every figure is computed from something
already in the passport, and the computation is stated so it can be reproduced or challenged.

### 4.1 Ignition and thermal cycles

```
ignition_cycles = lifetime_distance_km / mean_trip_km
                = 225,000 / 20
                = 11,250
```

`lifetime_distance_km` is the LCA functional unit (`environmental.usage_profile`). The 20 km mean trip
follows the daily-use figure already in the payload (`~30 km · ~45 min`, MiD 2017) at roughly 1.5 trips
per driving day. **One ignition cycle = one thermal cycle**: the board warms from ambient and cools
again.

### 4.2 Thermal fatigue — Coffin-Manson with Miner accumulation

**A cycle count is not a wear measure.** Two units with 11,250 cycles each can be in completely
different condition: a 5 km errand that swings the board 15 °C is not the same event as a motorway run
that swings it 70 °C. Solder-joint fatigue life scales with the **amplitude** of the swing, not with
how often it happened.

**Coffin-Manson:**

```
N_f(ΔT) = A · ΔT^(-n)          with   A = N_ref · ΔT_ref^n
```

**Reference condition (assumption, `basis: assumed`):** `N_ref = 15,000` cycles at `ΔT_ref = 40 °C`,
`n = 2.0`. Therefore `A = 15,000 · 40² = 24,000,000`.

`n` for SnAgCu solder is usually quoted in the range 2–3; **2.0 is the low end, so this estimate is
conservative** — a higher exponent would punish the large swings harder and consume more life.

**Miner's rule** (linear damage accumulation): total damage is the sum over bands of cycles divided by
cycles-to-failure at that band's amplitude.

```
D = Σ  n_i / N_f(ΔT_i)
```

| ΔT band | ΔT mid | cycles n_i | N_f(ΔT_i) | damage n_i/N_f |
|---|---|---|---|---|
| below 20 °C | 15 | 4,200 | 106,667 | 0.0394 |
| 20 to 40 °C | 30 | 4,600 | 26,667 | 0.1725 |
| 40 to 60 °C | 50 | 2,000 | 9,600 | **0.2083** |
| 60 to 80 °C | 70 | 420 | 4,898 | 0.0858 |
| above 80 °C | 90 | 30 | 2,963 | 0.0101 |
| | | **11,250** | | **D = 0.5161** |

```
fatigue life consumed  = 51.6 %
fatigue life remaining = 48 %
```

**The table is the argument, not the total.** The 30 most severe cycles contribute 1 % of the damage;
the 2,000 mid-range ones contribute 21 %. That is the thing a cycle counter cannot tell you, and it is
why the screen shows the distribution rather than a progress bar.

### 4.3 Flash endurance

```
flash_write_cycles_used = ignition_cycles × writes_per_cycle
                        = 11,250 × 4
                        = 45,000
endurance_remaining     = 1 − 45,000 / 100,000 = 55 %
```

Four writes per ignition cycle covers the statistics block, the DTC block, learned-adaptation values and
the trip record written at ignition-off. `100,000` write/erase cycles is a conventional automotive
EEPROM/flash endurance figure and is the **hard** limit on this screen — unlike fatigue, it is counted,
not modelled.

### 4.4 State of health — minimum, not mean

**Rejected formulation** (used in the first draft, 2026-08-06, and discarded the same day):

```
SOH = 0.40 · flash_remaining + 0.40 · thermal_remaining + 0.20 · (1 − hours_above_limit / powered_hours)
```

Three defects, all fatal in a viva:

1. **The weights were invented.** 0.40 / 0.40 / 0.20 came from the author, not from literature, and
   nothing defends them under questioning.
2. **The third term is a near-constant.** 41 h of 5,625 is 0.7 %, so the term returns ≈ 99 % almost
   regardless of what happened and hands the score ~20 free points. It looks like an indicator and
   behaves like a bias.
3. **Averaging is the wrong operator.** A component fails at its **weakest** mechanism, not at the mean
   of its mechanisms.

**Adopted formulation:**

```
SOH = min( remaining life across independent wear mechanisms )
    = min( flash endurance 55 %, thermal fatigue 48 % )
    = 48 %          limiting mechanism: thermal fatigue
```

No weights to defend; the score names **which** mechanism runs out first, which is precisely what a
remanufacturing decision needs. Mechanisms are treated as independent: flash wear is driven by write
count, fatigue by thermal amplitude, and neither accelerates the other.

**Temperature excursions move out of the score and into `findings`** — reported, not scored:
41 h above the declared 80 °C limit, peak 94 °C, implicated in the December 2025 processor failure.

### 4.5 Reuse fraction by mass

Each component carries a verdict in `health.reuse_assessment`:
`reuse` · `reuse_after_test` · `material_recovery` · `consumable`.

```
reuse_fraction_by_mass = Σ mass(reuse*) / Σ mass(all)
                       = (344.0 + 150.1 + 12.0) / 660.2
                       = 506.1 / 660.2
                       = 0.767
```

| Verdict | Components | Mass |
|---|---|---|
| `reuse` | Housing (344.0 g), Fasteners (12.0 g) | 356.0 g |
| `reuse_after_test` | Connectors (150.1 g) — one SENS-B reseat logged 2022, contact resistance to be verified | 150.1 g |
| `material_recovery` | PCB substrate, PCB copper, solder, passives, actives, coating, misc | 146.1 g |
| `consumable` | Thermal interface material (8.0 g) — renewed 2024 | 8.0 g |

**Verdicts follow the evidence on this screen**, not a rule of thumb: the board and actives go to
material recovery because 51.6 % of fatigue life is consumed and an over-temperature failure is already
logged; housing and fasteners have no electrical or thermal stress path.

### 4.6 Diagnostics ← the service log

`dtc_linked_to_service_events: 13` ties the DTC counts to `repair_history.events`. **This is the
board-computer argument made concrete**: the unit raises diagnostic codes for the whole vehicle, which
is why the service log legitimately contains vehicle systems (12 V battery, charger, brakes) alongside
the unit's own three faults.

---

## 5. Assumptions register

| # | Assumption | Value | Basis | Effect if wrong |
|---|---|---|---|---|
| A1 | Mean trip length | 20 km | assumed, from the payload's daily-use figure | scales cycle count and flash wear linearly |
| A2 | Coffin-Manson reference | 15,000 cycles at ΔT 40 °C | assumed | scales all damage by the same factor |
| A3 | Coffin-Manson exponent | n = 2.0 | assumed, low end of 2–3 | higher n → more damage from the large swings |
| A4 | ΔT distribution | 5 bands, table §4.2 | simulated | shifts damage between bands |
| A5 | Flash writes per cycle | 4 | assumed | scales flash wear linearly |
| A6 | Flash endurance | 100,000 cycles | conventional automotive figure | shifts the flash indicator only |
| A7 | Mechanisms are independent | min() valid | assumed | a coupled model would give a lower SOH |

**A2, A3 and A5 are the ones to state in the thesis.** They set the absolute level; the *shape* of the
damage distribution, which is what the screen argues from, is robust to all three.

## 6. Citations to attach

The field list originated from an AI-generated summary and **must not be cited as a source.** Cite the
claim — *a DPP should carry use-phase health data so an end-of-life actor can judge reuse* — from:

- *Evaluation of the Digital Product Passport for Remanufacturing: A Case Study Using Asset
  Administration Shell* — ScienceDirect S2405896325009085
- *Connecting Producers and Recyclers: A DPP Concept and Implementation Suitable for End-of-Life
  Management* — ScienceDirect S2212827124001549
- *Value Assessment of Consumer Electronics with Digital Product Passports* (washing machines,
  lifetime extension) — Springer 978-3-031-78338-8_5
- Aumovio, *Sustainable ECU remanufacturing* (industry)

Coffin-Manson / Miner need a reliability reference (IPC-9701 or an automotive electronics reliability
text). **Not yet sourced — do not write the equation into the thesis without one.**

⚠ **Excluded deliberately from the AI-generated list:** *gate driver degradation* (gate drivers are
inverter hardware, not a supervisory control unit), *drive-mode distribution* and *regen efficiency
ratio* (both describe the vehicle's behaviour, not the unit's).

---

## 7. The screen

### 7.1 Principle — the model is the index

Spec `04` §7 states the rule for the DPP page: *a highlight always answers "where does this fact
live?"* This screen **inverts** it — the object answers *"what happened to me, and where?"*

Almost every figure has a physical origin: 94 °C at the processor, 45,000 write cycles in one IC,
2,180 transients at the connectors, 6 bus-off events at the interface that was reseated in 2022,
eleven verdicts on eleven actual parts. Part picking reuses **`ZonePartInteraction`** (spec 10), so the
machinery already exists.

**Depth is not a third axis.** Anything placed on Z here would be a chart pretending to be an object.

### 7.2 Five lenses, one object

Pills **re-tint the same model** — they never open a new screen.

| Lens | Panel | Model |
|---|---|---|
| **Reuse** (default) | SOH, indicators, findings, reuse fraction | parts coloured by verdict |
| Thermal | ΔT damage distribution, fatigue consumed, reference condition | heat ramp, processor hottest |
| Electrical | transients, undervoltage, load dumps | connectors and power input lit |
| Data | flash, CPU, ECC, resets, CAN, DTC | flash IC and bus interface lit |
| History | 15 firmware versions, 13 service events | — **the only non-spatial block** |

**Reuse is the default** because it is the only state that answers the recycler's question, and because
0.767 is legible as colour before it is read as a number.

### 7.3 Part selected

Pinching a part dims the rest, lifts and pulses the selection, and the panel becomes that part's
record — pulling from four payload blocks at once. For `actives`: peak 94 °C, 41 h above limit,
11,250 cycles, 1 active DTC, replaced 2025-12-03 at 220,260 km. **That convergence is the argument for
the model-as-index.**

### 7.4 Palette decisions

| Meaning | Token | Why |
|---|---|---|
| `reuse` | `teal/accent` | |
| `reuse_after_test` | `gold/highlight` | already means "this matters" |
| `material_recovery` | `tab/inactive-fill` | **deliberately NOT red** — it is the correct outcome for a PCB, not a fault |
| `consumable` | `text/tip` | |
| heat ramp top | `#f08a3c` | ⚠ **not in the palette** — needs a `heat/high` token in `00` §2, or it falls back to gold, which already means high value |

**Red keeps its four reserved meanings** (`00` §2.1) and appears nowhere on this screen.

## 8. Open items

1. `heat/high` token, or the ramp tops out in gold.
2. SOH bar colour: flat gold, or a threshold ramp. A red band would collide with the reserved meanings.
3. Electrical and Data lenses are specified but not mocked.
4. Coffin-Manson / Miner reference still to source (§6).
5. The `History` pill's layout is undesigned — 15 firmware rows + 13 events need a scroll region
   **built as one from the start** (spec 13 v10 defect 3: service rows printed through each other).

## 9. Iteration log

- **2026-08-06 (a)** — vehicle-centric design: usage stats, DACH driving map, car service log. Mock
  `04a_v1`. Dropped the same day.
- **2026-08-06 (b)** — pivot to unit telemetry. `unit_use_phase` generated; mock `04a_v2` (model as
  index, reuse / thermal lenses).
- **2026-08-06 (c)** — Thiago: a 15,000-cycle "budget" is not defensible, since 15,000 short trips and
  15,000 motorway runs are not the same wear. Replaced by Coffin-Manson + Miner (§4.2); SOH replaced by
  min-of-mechanisms (§4.4). **Thermal 25 % → 48 %, SOH 52 % → 48 %.** Mock `04a_v4`.

*Created 2026-08-06 · Status: specified, not built · Parent: `04_DPP_page.md`*
