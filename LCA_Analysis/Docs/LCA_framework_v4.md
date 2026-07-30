# VCU Life Cycle Assessment — Framework v4

**Product under study:** Vehicle Control Unit (VCU), reference device Bosch Motorsport MS 50.4
(datasheet anchors: mass ≤ 660 g; envelope 166 × 121 × 41 mm; die-cast aluminium housing; 4-layer FR-4 PCB; 3 connectors / 198 pins; supply 5–18 V; sensor supplies max 4 A @ 12 V + 2 A @ 5 V; outputs 2×7.5 A + 4×2.2 A).
> **Correction 2026-07-25:** the previously cited "max 48 W" `[D]` does NOT appear in the
> datasheet — it was an untagged derivation (likely 12 V × 4 A sensor-supply capacity) that had
> been promoted to a device power rating. Removed everywhere; S4 now uses a sourced own-draw
> basis (see §3/S4).
**Author:** Thiago Pegorer — Master's thesis, SRH. **Version:** v4, 2026-07-03.
**Status (2026-07-28):** **ALL COMPUTATION COMPLETE.** S1–S4 + four EoL scenarios built and
Bigum-refined · EF 3.1 primary results + PEF screening · ReCiPe 2016 Midpoint (H) + Endpoint (H)
cross-check (characterised; AoP damage totals) · Monte Carlo n = 1000 × 7 systems. Remaining work
is documentation and figures, not modelling.

---

## 0. EXECUTION GUIDELINE FOR AI AGENTS (openLCA build, v4)

**Read this before touching openLCA or any build script. These rules are binding.**

### 0.1 Source of truth
- **`BOM_v4.md` (same folder) is the single source of truth for all masses and materials.**
  Table 1 = component view (17 lines, material breakdown per component); Table 2 = material view
  (whole 660 g device — this is the S1 input list). The companion workbook `VCU_BOM_v4.xlsx`
  (sheet `BOM_Data`) holds the same data in machine-readable tidy form.
- If a needed quantity is not in BOM_v4.md, **STOP and ask the human.** Do not improvise numbers.

### 0.2 Data discipline (inherits §0.5)
1. **No invented ecoinvent dataset names.** Always resolve providers against the LIVE database
   (IPC `get_descriptors` search, or the resolver-script pattern in `Scripts/`), present candidates
   with name + UUID + location + reference unit, and get human approval before linking.
2. Selection criteria: prefer `market for …` datasets (they embed average inbound transport —
   framework §3/S1 anti-double-count convention); prefer EU/RER, then GLO; match the reference
   unit to the BOM amount; inspect `is_input` convention for waste datasets (S5).
3. Every provider choice, amount, and unit gets logged (build-log pattern in `Scripts/`), so the
   human can audit each link at the per-stage checkpoint.
4. Proxy rule: a proxy must match on **function and scale** (process slots need process datasets —
   the lost-wax and populated-board mistakes are documented counterexamples). Any proxy is
   declared with rationale in the process description.
5. Assumptions are allowed only as **declared** `[A]` items with ranges (Monte Carlo inputs) —
   never silent.

### 0.3 Allocation & recycling-benefit convention

- Database: **ecoinvent 3.8, APOS**, unit processes, openLCA 2.6.2 (IPC-scripted build).
- Recycling benefit modeled **explicitly at EoL**: recovered mass × recovery rate × avoided
  virgin-production dataset, reported as **savings (avoided impact)** — *not* negative emissions.
  Reported per scenario: **gross footprint, saving, net (gross − saving)**.
- **Declared limitation:** APOS partially allocates recycled content in background datasets; an
  explicit EoL credit on top carries a mild double-count risk. Disclosed and accepted; single-database
  consistency (APOS everywhere) is preserved. (Supervisor-agreed 2026-07-02.)

### 0.4 Impact assessment methods (binding choices)

1. **EF 3.1 (adapted)** — primary method, EU/DPP-aligned. Category *reporting set* derived by
   normalization + weighting screening (PEF "most relevant" ≥80% cumulative rule) **plus** the
   goal-pinned categories: *resource use, minerals & metals* and *climate change*. All 16 shown.
2. **ReCiPe 2016 Midpoint (H)** (openLCA LCIA pack) — cross-check at category level
   (supervisor request). *The ecoinvent-shipped `ei - ReCiPe …` packs were used until
   2026-07-28 and then replaced: their category names and units (metal depletion in kg Fe-eq,
   ALOP/ULOP, HTPinf, WDP, the `(H,A)` notation) identify them as ReCiPe **2008**, and they
   carry no normalisation set. Cite Huijbregts et al. (2017), not Goedkoop et al. (2009).*
3. **ReCiPe 2016 Endpoint (H)** (openLCA LCIA pack) — aggregated damage view, reported as the
   three area-of-protection totals; no single score (§4.2.1).

### 0.5 Data-quality hierarchy (governs the BOM and all inventory)

Every quantity used in the model carries a source tag:

| Tier | Tag | Meaning |
|---|---|---|
| 1 | `[DATASHEET]` | From the Bosch MS 50.4 datasheet (mass, dimensions, pins, power) |
| 2 | `[LITERATURE]` | From a citable scientific source (paper / EU project / standard), value + citation |
| 3 | `[ASSUMPTION]` | Declared engineering assumption with rationale AND an uncertainty range for Monte Carlo |

**Rule: no untagged numbers.** Tier 3 is allowed only where Tiers 1–2 are demonstrably unavailable;
each Tier-3 line must state why.

**Cut-off criterion:** constituents below **1 % of their carrier's mass** are excluded from the
inventory **unless impact-relevant** (e.g., precious metals at ppm levels stay in — they dominate
resource depletion; regulated toxics would stay in likewise). Each applied exclusion is declared
where it occurs. A fourth tag `[REGULATORY]` is available for values/exclusions grounded in law
(e.g., ELV Directive 2000/53/EC Annex II).

Candidate literature families for the BOM (to vet next, one line at a time):
EU H2020 **TREASURE** project (car-electronics dismantling & characterization, incl. ECUs; associated
Univ. Zaragoza recyclability study), published automotive-ECU LCA/teardown studies, WPCB
metal-concentration literature (e.g., Cui & Zhang 2008; Hagelüken), published die-casting and
SMT/reflow process-energy studies, and EoL recovery-rate studies for WEEE/ELV electronics.

### 0.6 Deliverables from this framework

1. **BOM v4** — component lines with per-line source tags (next document).
2. Stage inventories S2–S5 with the same tagging.
3. Rebuilt openLCA model (scripts re-run with sourced data; structure unchanged).
4. Scenario results: gross / saving / net per scenario × 3 methods; EF screening chart (all 16).
5. Monte Carlo on Tier-3 parameters (Au/Ag/Pd concentrations, recovery rates first).

## 1. Goal

Quantify the multi-impact environmental footprint of one VCU over its full life cycle and compare
end-of-life (EoL) strategies — in particular AR/DPP-guided disassembly against current practice —
across EF 3.1 and ReCiPe 2016 impact categories, with recycling benefits reported as **avoided
impact (savings)**.

**Position of the thesis intervention:** the AR-DPP prototype targets **dismantling and
separation** — the *first* process step of the recycling chain. Dismantling/separation quality is
the gateway that determines what all downstream recovery (smelting, hydrometallurgy) can achieve:
bulk shredding mixes and dilutes material fractions (precious metals lost to slag and dust), while
guided dismantling delivers clean, sorted fractions to recovery. The EoL scenarios therefore differ
primarily **at the dismantling/separation step**, holding downstream recovery technology constant.

## 2. Functional unit

**1 VCU operated in a battery-electric passenger vehicle in Germany for 15 years at
15,000 km/year (225,000 km total).**

> **Race-vs-passenger caveat (declared):** the MS 50.4 datasheet specifies a 220 h / 2-year
> maintenance interval — it is race hardware. This study models the device as a *generic as-built
> VCU* in passenger service, where maintenance intervals extend accordingly; the temporal boundary
> is the **vehicle's end-of-life (15 years)**, not the motorsport maintenance schedule.

## 3. System boundary — five stages (cradle-to-grave)

### Stage 1 — Materials & Construction (cradle-to-Bosch-gate)
Extraction and refinement of raw materials AND manufacturing of the electronic
components (capacitors, ICs/transistors, connectors, PCB laminate, solder, housing alloy,
fasteners, coatings), up to finished parts ready for assembly. Includes transportation of
components/materials from their native manufacturing sites to the Bosch plant.
> **Transport convention (anti-double-count):** ecoinvent `market for …` datasets already include
> average producer→consumer transport. Inbound logistics are therefore **embedded in the market
> datasets**; no separate supplier→Bosch legs are added unless a specific, sourced leg exceeds
> the market average (must be justified and cited).

#### Stage 1 — BUILD SPECIFICATION (hybrid method — approved 2026-07-24, pending final human sign-off)

Process: `VCU S1 Materials & Construction` · ref. output "VCU materials (S1)" 0.66 kg.
**Hybrid rule:** actives/passives = ecoinvent COMPONENT datasets (capture fab energy);
rest = MATERIAL markets from BOM Table 2, with component-contained masses SUBTRACTED (no double-count).

**(a) Component-dataset inputs** (amounts from BOM Table 1; providers to be RESOLVED live, §0.2):
| Input | Amount | Provider search (candidates only) |
|---|---|---|
| ICs: processors 7 + flash 1 + transceivers 1.5 + AFE 1.2 + regulators 3 + IMU 1 | 14.7 g | "integrated circuit, logic type" (prev. resolved 92416fbe…) — IMU/regulators folded in `[A]` declared proxy |
| Power stages | 9 g | "transistor"/power discrete dataset — resolve candidates |
| Ta capacitors | 2.5 g | "capacitor, tantalum-…" (seen: 5ef6e692…) |
| MLCCs/passives (incl. resistors, inductors `[A]` proxy) | 44.9 g | "capacitor, for surface-mounting" (prev. 40ca41dd…) |
| Solder joints | 3.2 g | "solder, paste/bar, lead-free" — resolve; fallback tin market |

**(b) Material-market inputs** (Table 2 minus component-contained masses):
| Material | Amount | Note |
|---|---|---|
| Aluminium | 439 g | housing 344 + connector shells 95 (on-board 3.3 g inside datasets) + casting service in S2 |
| Copper | 49 g | PCB foils 14 + connector contacts 35 |
| Stainless steel | 12 g | fasteners |
| Polymers/epoxy | 43 g | FR-4 resin 20 + insert 15 + coating 3 + labels 5 |
| Glass fibre | 29 g | bare PCB |
| Silicone | 12.5 g | TIM + seals |
| Gold | 92 mg | connector plating 60 `[A]` + board 31.7 `[L]` — see (c) |
| Silver / Palladium / Tantalum / Nickel (conn.) | 59 mg / 5.5 mg / 0.9 g / 0.5 g | see (c) |

**(c) Declared conservative overlap:** trace metals (Au/Ag/Pd/Ta, board Ni) are added EXPLICITLY
as markets even though component datasets contain some internally. Rationale: preserves the
resource-depletion signal (thesis headline) under our own literature-anchored masses; overlap is
<0.1 % of device mass, declared, and sensitivity-checked (run once without explicit traces).
**(d) Bare-PCB fabrication note:** glass fibre + epoxy + Cu foils modeled as materials; board
LAMINATION/fab energy belongs to S2 process-energy TODO (declared) — declared gap.

### Stage 2 — Hardware Assembly (Bosch)
VCU-specific manufacturing at the provider (Bosch): **(a)** die-casting of the aluminium housing,
**(b)** PCB assembly — SMT placement and soldering, **(c)** final assembly and end-of-line test.
The VCU "is born" here as hardware + software.
Content = **process energy and consumables only** (all embodied component burdens live in Stage 1).
> **Declared exclusion — software:** software development effort does not scale per unit and is
> conventionally excluded from product LCAs; flashing energy is negligible.

#### Stage 2 — BUILD SPECIFICATION (COMPLETE — die-casting + SMT mounting BUILT, checkpoint passed 2026-07-24)

**Die-casting process energy (per kg cast), Dalquist & Gutowski 2004, Table 3 `[L]`:**

| Function | Energy | Carrier |
|---|---|---|
| Die prep | ~0.5 MJ | electricity |
| Metal prep (melt+hold furnaces, 2.465+0.493) | 3.0 MJ | natural gas (gas-fired per D&G) |
| Casting (machine 2.5 + cooling 0.65) | 3.2 MJ | electricity |
| Finishing | 1.2 MJ | electricity |
| **Total** | **7.9 MJ/kg ≈ 2.2 kWh/kg** | gas 3.0 + electricity 4.9 |

**Carrier split → per-VCU derivation (housing cast mass 0.344 kg, BOM #1 — only the housing is
die-cast):** gas 3.0 MJ/kg × 0.344 kg = **1.03 MJ**; electricity 4.9 MJ/kg × 0.344 kg = 1.69 MJ
÷ 3.6 MJ/kWh = **0.47 kWh**. Closure: 1.03 + 0.47×3.6 = 2.72 MJ = 7.9 × 0.344 ✔ — the split
re-partitions the 7.9 MJ/kg by carrier, it does not reduce it. Carriers are separated because
gas heat and grid electricity carry different upstream burdens (one lumped "energy" input would
be unmodelable in openLCA). Declared nuance: the gas/electric assignment of non-melt functions
follows D&G's process description (melt/hold = gas-fired furnaces; machine, hydraulics, cooling,
trim = electric), not a per-carrier measurement at a single foundry.

- **Die casting (housing, 0.344 kg):** total process energy **7.9 MJ/kg [6.5–9] `[L]`** —
  Dalquist, S. & Gutowski, T. (2004), *Life Cycle Analysis of Conventional Manufacturing
  Techniques: Die Casting*, MIT working paper LMP-MIT-TGG-03-12-09-2004, **Table 3**
  (die prep ~0.5 + metal prep 3.0 + casting 3.2 + finishing 1.2 MJ/kg; range from Upton 2.5 vs
  Roberts 1.8 MJ/kg machine estimates). Modeled as 3.0 MJ/kg natural gas (melt/hold) +
  4.9 MJ/kg electricity → per VCU: **1.03 MJ gas + 0.47 kWh electricity** (DE datasets).
  Double-count check: S1 ingot dataset ends at ingot; foundry re-melt is S2's burden ✔. **Why NOT Table 3's 14.9 MJ/kg 'including loss' row:** that row adds electricity generation/distribution losses by hand; our ecoinvent electricity market datasets already model the full upstream chain (plants, fuel, grid losses). LCA convention: input FINAL energy at plant gate; background DB supplies the losses. Using 14.9 would double-count them.
- **SMT placement + reflow soldering (board population) — APPROVED 2026-07-24:** modeled with the
  ecoinvent process dataset **`market for mounting, surface mount technology, Pb-free solder`**
  (GLO, ref. unit **m² of mounted board**; documented in ecoinvent report No. 18 Part II §2.3).
  Per §0.2 this is the preferred route: a `market for …` PROCESS dataset matching S2(b) on function
  and scale — solder-paste printing + component placement + reflow energy and consumables in one
  documented activity. The literature alternative (measured Pb-free oven power 10.6–11.5 kWh/h,
  Kaznica 2006, Circuits Assembly / Flextronics) needs stacked `[A]` throughput and
  placement-energy assumptions and is kept only as a plausibility cross-check for the thesis text.
  **Amount: 0.0158 m²** = 150 × 105 mm board `[A+ind.spec]`, **board-area basis — CONFIRMED
  2026-07-24** from the dataset description ("soldering material input and production efforts for
  1 m² of mounted PWB … input of PWB and components are not included"): the mounted board is the
  reference object, area counted once, no per-side convention. Declared sensitivity upper bound
  0.0315 m² `[A]` (×2) covering the residual per-side ambiguity AND the density mismatch — the
  dataset is v2-legacy, from five lab-scale trials at two European producers in "average
  conditions", while this VCU board is densely populated automotive SMD (v2-era caveat goes into
  the thesis limitations).
  **Anti-double-count (i) — forbidden neighbours:** NOT `printed wiring board, surface mounted,
  unspecified, Pb free` (whole populated board — re-imports all S1 components; the documented
  §0.2 counterexample) and NOT `printed wiring board production, for surface mounting, Pb free
  surface` (bare-board fab — the S1 note (d) gap stays declared).
  **Anti-double-count (ii) — declared overlap, QUANTIFIED at checkpoint 2026-07-24:** per VCU the
  mounting dataset carries 1.27 g solder paste (0.0806 kg/m²) and 0.068 kWh process electricity
  (4.32 kWh/m²), while S1 keeps its explicit 3.2 g solder joints (BOM = source of truth). Overlap
  = 1.27 g ≈ 0.19 % of device mass — kept declared, §S1(c) pattern, conservative direction. Flag:
  SAC paste is 3.9 % Ag, so the overlap carries ~50 mg Ag-equivalent (same order as the explicit
  59 mg Ag line) — included in the planned run-without-explicit-traces sensitivity, relevant to
  the resource-depletion headline only.
  **As-built links (2026-07-24 log):** SMT = `market for mounting, surface mount technology,
  Pb-free solder` [33c6bb4d-7e0e-3e2f-a175-1eb4b1122e16, GLO] at 0.0158 m²; heat = `market for
  heat, district or industrial, natural gas` [Europe without Switzerland] 1.03 MJ; electricity =
  `market for electricity, medium voltage` [Germany] 0.47 kWh; chained 0.66 kg from S1 v4.
- **Die-cast energy carriers — provider convention:** the 1.03 MJ gas enters as `market for heat,
  district or industrial, natural gas` (Europe w/o CH) — a HEAT dataset, so furnace combustion
  emissions are included and the final-energy-at-gate convention (losses note above) holds; the
  0.47 kWh as `market for electricity, medium voltage` (DE).

### Stage 3 — Distribution to OEM
Transport of the finished VCU from the Bosch plant to the vehicle OEM
(working example: Volkswagen Group, Germany). Short road freight leg (e.g., Abstatt → Wolfsburg).

#### Stage 3 — BUILD SPECIFICATION (approved 2026-07-24)

Process: `VCU S3 Distribution` · ref. output "VCU delivered (S3)" 0.66 kg · chains S2 (0.66 kg).

- **Boundary (v3 → v4 change, anti-double-count):** v3 carried a sea leg (Asia → DE, 5.28 tkm)
  and a second 500 km road leg for inbound components. Both are REMOVED in v4: inbound
  component/material logistics are embedded in the S1 `market for …` datasets (§3/S1 transport
  convention), so keeping them would double-count. S3 models ONLY the outbound leg of the
  finished unit — a flow no market dataset covers, because S1/S2 are foreground processes.
- **Leg:** Abstatt (Bosch plant) → Wolfsburg (VW), road ≈ **490 km** `[A, 450–550]` (working
  example; route choice is illustrative by design). Load = whole device 0.66 kg →
  **0.66 kg × 490 km = 0.323 tkm** [0.297–0.363 → Monte Carlo].
- **Dataset:** `market for transport, freight, lorry >32 metric ton, EURO6` (RER) — `[A]` class
  choice: ~490 km inter-plant consolidated freight is typical long-haul semitrailer duty; the
  16–32 t class is logged as the alternate candidate at the checkpoint. The market embeds average
  utilization/empty-return; EURO6 reflects a current DE plant-to-OEM fleet.
- **Declared exclusion — packaging:** transport packaging (< 1 % of carrier mass, returnable
  totes standard between Tier-1 and OEM) excluded under the §0.5 cut-off criterion.

### Stage 4 — Use Phase
Operation inside a BEV in Germany: 15 years × 15,000 km/yr. Scope = the **VCU's own electricity
consumption only**, charged to the German grid.

#### Stage 4 — BUILD SPECIFICATION (approved 2026-07-25)

Process: `VCU S4 Use Phase` · ref. output "VCU used, 15 y (S4)" 0.66 kg (renamed from
"end-of-life-ready" 2026-07-25 — stage-exit STATE naming: materials → assembled → delivered →
used) · chains S3 (0.66 kg).

- **Own power draw: 9 W `[L]`** — Bosch Motorsport ECU MS 5.0 manual: "Approx. 9 W at 14 V"
  (device consumption without external loads), used as a **declared family proxy** (same
  manufacturer/product family and electronics class as the MS 50.4). Range **9–20 W `[A]`**
  covering computational load and output-stage self-losses in service. Boundary: the datasheet's
  supply-rail capacities (4 A @ 12 V etc.) are power *delivered to external* sensors/actuators —
  those loads belong to other components' inventories, NOT to this FU ("own consumption only").
- **Operating hours: 5,625 h** = 225,000 km ÷ **40 km/h `[L]`** — MiD 2017 (German national
  mobility survey): cars average ~30 km in ~45 min/day; its 14,700 km/yr fleet average also
  independently corroborates the FU's 15,000 km/yr. Speed range 35–45 km/h → 5,000–6,430 h.
- **Grid→12 V conversion: η = 0.765 [0.68–0.83]** = AC charging 0.85 `[L]` (Apostolaki-Iosifidou
  et al. 2017, *Energy* 127, 730–742: measured charging losses 10–20 %) × DC/DC 12 V conversion
  0.90 `[A]` (typical converter efficiency, declared).
- **Use-phase electricity: 9 W × 5,625 h ÷ 0.765 ≈ 66.2 kWh** grid electricity over 15 years
  [MC range ≈ 54–189 kWh], as `market for electricity, low voltage` (DE) — low voltage `[A]`:
  AC home/wallbox charging assumed dominant for a passenger BEV.
- **v3 → v4 change (recorded):** v3 charged 216 kWh = 48 W × 4,500 h — built on the phantom
  48 W rating AND on treating deliverable capacity as own draw. 66.2 kWh replaces it with
  sourced values; a 3.3× reduction that is a correction, not an optimistic assumption.
- **Declared exclusions:** vehicle-off sleep draw (~1 mA class ≈ 1.5–2 kWh over 15 y, < 3 % of
  central, inside MC width); carried-mass energy (see note below).
> **Declared exclusion — carried-mass energy:** a vehicle also spends energy transporting the mass
> of every component it carries. The VCU adds 0.66 kg; the extra vehicle energy to haul 0.66 kg for
> 225,000 km is negligible next to the device's direct draw, and is excluded. Stated so the boundary
> is explicit (standard practice for sub-kg components).

### Stage 5 — End-of-Life (scenario stage)
The comparison stage. Scenario data policy: **Sc1 as already built; Sc2–Sc4 built from
ecoinvent datasets + published literature** (no empirical recovery data is claimed).
- **Sc1 — No recycling (KEPT AS BUILT):** collection, landfill (inert fraction 0.528 kg) +
  incineration (combustible fraction 0.132 kg). No changes.
- **Sc2 — Current practice (datasets + literature):** bulk shredding, mechanical separation,
  smelting; partial recovery. Process burdens from ecoinvent treatment datasets; recovery rates
  from published recycling studies `[LITERATURE]`.
- **Sc3 — AR/DPP-guided disassembly (literature scenario):** the thesis intervention, acting at
  the **dismantling & separation step** — targeted component removal and clean fraction sorting
  *before* material recovery. Downstream recovery technology is the same as Sc2; what changes is
  the quality of the input fractions. Because the thesis prototype is a printed replica (no true
  VCU was disassembled), recovery rates and disassembly parameters come from published
  disassembly/recycling studies `[LITERATURE]`, applied to this VCU's BOM.
- **Sc4 — Full disassembly + reuse (literature scenario):** upper-bound recycling + component
  reuse; reuse fractions and recovery rates likewise `[LITERATURE]`.

> **Claim boundary (thesis honesty note):** the environmental results of Sc3/Sc4 quantify what
> guided disassembly *would* save according to published data — they are literature-parameterized
> scenarios, not measurements from the AR prototype. The prototype user study evidences
> feasibility and guidance quality (task time, errors, usability), not recovery rates. The thesis
> text must keep this distinction explicit.

#### Stage 5 — SCENARIO SPECIFICATIONS & AS-BUILT MODEL (all four scenarios BUILT; current as of 2026-07-26)

**Sc1 — AS BUILT (split RULED 2026-07-27, BOM-derived):** `VCU S5 EoL Sc1 (no recycling)`:
chain S4 + inert **0.5875 kg** → sanitary landfill [Europe w/o CH, 1b875518…] + combustible
**0.0725 kg** (polymers 60 g + silicone 12.5 g, BOM v4.1 Table 2) → waste plastic market
[CH proxy, d4694e95…]; closure 0.660 ✔. The former 0.528/0.132 split was v3 legacy without a
v4 source and inconsistent with Sc2–Sc4's BOM-based combustible accounting — replaced per
§0.1 (BOM = source of truth). Result effect ≤~0.2 % on Sc1 climate; CSVs regenerated.

**Sc2 semantics — the constraint that carries the comparison:** the device enters the shredder
**without any manual or precise dismantling** — mechanical macro-separation only (magnetic →
ferrous, eddy-current → Al, density/sensor → Cu-rich, rest → plastics/dust). The board is never
extracted as a board: its precious metals travel as fragments and mostly land in the WRONG
streams. Only the share reaching the Cu-rich stream continues to the integrated smelter.
Sc3 = targeted removal + clean fractions BEFORE any shredding (populated PCB intact → direct
smelter feed); downstream technology identical to Sc2. Sc4 = Sc3 + functional component reuse.

**Scenario source set:**

| # | Source | Role |
|---|---|---|
| S-1 | Chancerel, Meskers, Hagelüken & Rotter (2009), *J. Ind. Ecology* 13(5), 791–810. <https://doi.org/10.1111/j.1530-9290.2009.00171.x> | Sc2 preprocessing stream-splits — full-scale German plant, 27 t WEEE, no manual dismantling |
| S-2 | Marra, Cesaro & Belgiorno (2018), *J. Cleaner Production* — [open PDF](https://www.iris.unisa.it/retrieve/e2915b35-a6c7-8981-e053-6605fe0a83a3/4718887.pdf) | Corroborates S-1 (PMs to Cu fraction ~33 %); small-WEEE Al caution |
| S-3 | Hagelüken (2006), *World of Metallurgy – ERZMETALL* 59(3), 152–161 | Integrated smelter–refinery route (Umicore); whole-board feed practice |
| S-4 | Bigum, Brogaard & Christensen (2012), *J. Hazard. Mater.* 207–208, 8–14 — **read first-hand 2026-07-26** | **Per-metal yields, Table 8 p. 11 (adopted):** Au/Pd 98 % · Ag 97 % · Cu 95 % · Fe 100 % · Al remelt 79 % · Al pre-treat 86 % · Fe pre-treat 96 %. Their refinery model IS Rönnskär/Boliden = our ecoinvent SE smelter dataset (p. 9); their pre-treatment rates ARE Chancerel's (fn. a) — independent cross-confirmation. Method quote (p. 13): recovery "should be quantified with respect to the individual metals … not as a bulk metal recovery rate". |
| S-5 | Lee, Kim & Lee (2012), *J. Hazard. Mater.* 241–242, 387–394 | Sc3 dismantling & separation parameters (90–98 % damage-free removal) |
| S-6 | Cui & Zhang (2008), *J. Hazard. Mater.* 158(2–3), 228–256 | Smelter-route review; Ta non-recovery |
| S-7 | Zhao et al. (2023), *Environ. Sci.: Adv.* 2, 196–214 — **read cover-to-cover from PDF** | Sc4 literature basis (table below) |

---

### Sc2 — Current practice, bulk route

**Parameter table (per 1 VCU, BOM v4.1 masses).** Net = stream share × downstream yield; credit
= net mass × avoided-virgin market (S1-validated providers, §0.3 convention):

| Material (mass) | To correct stream | Downstream yield | **Net recovered** | Credit dataset |
|---|---|---|---|---|
| Gold 92 mg | 25.6 % `[L S-1]` | 98 % `[L S-4]` | **23.1 mg (25.1 %)** | market for gold |
| Palladium 5.5 mg | 25.6 % `[L S-1]` | 98 % `[L S-4]` | **1.38 mg** | market for palladium |
| Silver 59 mg | 11.5 % `[L S-1]` | 97 % `[L S-4]` | **6.6 mg (11.2 %)** | market for silver |
| Copper 63.4 g | 60 % `[L S-1]` | 95 % `[L S-4]` | **36.1 g (57 %)** | market for copper, cathode |
| Aluminium 442 g | 86 % `[L S-4]` | 79 % remelt `[L S-4]` | **300.3 g (68 %)** | market for aluminium, primary, ingot |
| Steel/Fe 17 g | 96 % `[L S-4]` | 100 % `[L S-4]` | **16.3 g (96 %)** | market for steel, chromium steel 18/8 |
| Tantalum 0.9 g | — | — | **0 — lost to slag** `[L S-6]` (thesis headline) | none |
| Tin 3.9 g | with Cu stream | — | **0 central** `[A, 0–50 % MC]` | none central |
| Polymers/ceramics/residues | — | — | 0; residual streams → treatment | gross side |

**Build structure — `VCU S5 EoL Sc2 (bulk recycling)` (gross) + `…Sc2 credits (avoided virgin)`:**

| # | Entry | Dataset | Amount |
|---|---|---|---|
| 1 | Chain input | `VCU used, 15 y (S4)` ← S4 | 0.66 kg |
| 2 | Shredding + mechanical sorting | `treatment of waste electric and electronic equipment, shredding` [a6106197…, GLO] — waste-output convention | 0.66 kg |
| 3 | Smelter feed (Cu-stream fragments) | `treatment of electronics scrap, metals recovery in copper smelter` [01e00045…, Sweden] | 0.038 kg `[A: smelter-route mass ÷ 0.95]` |
| 4 | Residual combustible | `market for waste plastic, mixture` (Sc1 UUID) | 0.073 kg `[A]` |
| 5 | Residual inert | `treatment of inert waste, sanitary landfill` (Sc1 UUID) | 0.1526 kg (ledger) |

Mass ledger: 0.660 = 0.3964 scrap-out (Al 380.1 + Fe 16.3, remelt losses downstream inside `[L]`
yields) + 0.038 smelter + 0.073 combustible + 0.1526 inert ✔. Credits: Al 300.3 g · Cu 36.1 g ·
steel 16.3 g · Au 23.1 mg · Ag 6.6 mg · Pd 1.38 mg — total **352.7 g** ✔ AS BUILT 2026-07-26.

*Declared approximations:* both scenarios charge the SAME Swedish smelter dataset pro-rata to
their actual feed (no asymmetry); Sn no central credit; residual combustible/inert split `[A]`.

---

### Sc3 — AR/DPP-guided disassembly

**Recovery-route methodology (thesis text must state this):** there is no "manual gold
extraction" step and none is claimed. The **integrated smelter–refinery IS the chemical
recovery process**: the intact board enters a copper smelter, copper collects Au/Ag/Pd, and
downstream electro-/hydrometallurgical refining separates them. Dismantling\'s role is
*routing*, not extraction: it determines whether the board ARRIVES at that gate intact (Sc3,
96 % `[L S-5]`) or shredded into the wrong streams first (Sc2, 25.6 % of Au arriving).

**Declared deviation from the ecoinvent default chain:** ecoinvent\'s `market for used printed
wiring boards` routes 100 % of boards to `treatment of scrap printed wiring boards, shredding
and separation` even after manual dismantling. Sc3 deliberately bypasses that step and charges
the smelter directly with the intact board — industrial practice at integrated e-scrap smelters
accepts whole boards as feed (Hagelüken 2006); shredding dismantled boards would re-introduce
exactly the fragment losses guided disassembly exists to avoid.

**Parameter table:**

| Fraction | Route | Recovery basis | **Net** |
|---|---|---|---|
| Housing + conn. shells Al 439 g | clean Al scrap → remelt | 98 % `[L S-5]` × 79 % `[L S-4]` | **339.9 g (77 %)** |
| Fasteners 12 g (board Fe 5 g → slag, uncredited `[conservative]`) | steel scrap | 98 % `[L S-5]` × 100 % `[L S-4]` | **11.8 g** |
| Populated PCB 138 g + coating 3 g | **intact → integrated smelter [SE]** | 96 % arrival `[L S-5]` × per-metal `[L S-4]` | **Au 86.6 mg (94 %) · Ag 54.9 mg · Pd 5.2 mg · board Cu 25.5 g** |
| Connector Cu contacts 35 g | Cu scrap | 94 % `[L S-5]` × 95 % `[L S-4]` | **31.3 g** |
| Ta caps (0.9 g) | separated stream | no at-scale route `[L S-6]` | **0 credited** — Sc3 *enables*, does not credit |
| Polymers/silicone 32.5 g | → incineration | — | no material credit |

**Build structure — `VCU S5 EoL Sc3 (guided disassembly)` + `…Sc3 credits (avoided virgin)`:**

| # | Entry | Dataset | Amount |
|---|---|---|---|
| 1 | Chain input | `VCU used, 15 y (S4)` ← S4 | 0.66 kg |
| 2a | Dismantling — facility share | `market for manual treatment facility, waste electric and electronic equipment` [58807b1b…] at ecoinvent\'s own manual-dismantling intensity (1.6e-8 units/kg, from `treatment of used laptop computer, manual dismantling` [db7889d7…] — intensity borrowed, laptop fraction-routing NOT imported) | 1.06e-8 units |
| 2b | Dismantling — electricity (tools + AR headset, ~15 min) | `market for electricity, low voltage` [Germany] | 0.01 kWh `[A, 0.002–0.03]` |
| 3 | Smelter feed (intact board + coating) | `treatment of electronics scrap, metals recovery in copper smelter` [01e00045…, Sweden — Boliden-type; ~0.046 kWh + 0.375 kg quicklime per kg feed; slag inside dataset] | 0.141 kg |
| 4 | Residual combustible (insert+TIM+labels+fluorosilicone) | `market for waste plastic, mixture` | 0.0325 kg |
| 5 | Residual inert — ON-SITE sorting losses only | `treatment of inert waste, sanitary landfill` | 0.0111 kg |

Mass ledger: 0.660 ≈ 0.4749 scrap-out + 0.141 smelter + 0.0325 combustible + 0.0111 sorting
losses (remelt/refining losses occur at the secondary-metal plants, already netted inside the
`[L]` yields — not our landfill). Credits: Al 339.9 g · Cu 56.8 g · steel 11.8 g · Au 86.6 mg ·
Ag 54.9 mg · Pd 5.2 mg — total **408.6 g** ✔ AS BUILT 2026-07-26.

*Assumptions register (Sc3):* ① dismantling electricity 0.01 kWh `[A]`; ② residual split
simplified `[A]`; ③ Al remelt 79 % `[L S-4]` central — clean-cast-scrap upside 79–95 % in MC;
④ contacts via clean Cu-scrap route inside credit convention (identical in Sc2/Sc3 — no bias).

---

### Sc4 — Full disassembly + component reuse (EXPLORATORY UPPER BOUND)

**Literature basis — verified first-hand (Zhao et al. 2023 `[S-7]`, page anchors):**

> **The distinction that governs every Sc4 number — removal ≠ damage-free ≠ functional:**
> a *removal rate* counts components that came OFF the board; a *damage-free rate* counts those
> that survived intact; a *functional yield* counts those that pass testing for reuse. The
> literature quantifies the first two; the third is nowhere quantified.

| Verified figure | What it measures | Source & page |
|---|---|---|
| **94 %** (250 °C IR, 0.33 cm/s, 70 s/board) | REMOVAL rate — survival not quantified | Park et al., in Zhao p. 207 |
| **100 % damage-free**, direct reuse "without additional processing" (137 s/board) | removal + survival, robotic selective | Marconi et al., in Zhao p. 208 |
| **39.73 %** small SMDs (hot air + pulse jets) | method-specific small-SMD removal | Chen et al. 2013, in Zhao p. 207 |
| Functional pass rates | **NOT quantified anywhere** — Zhao Table 5 (p. 208) gives criteria only | — |
| "ECs are at their optimal period of stable operation when WPCBs are discarded" | bathtub argument for functional yield | Zhao p. 197 (Peter) |
| Component **traceability** "essential to estimate the remaining life … and to establish a market for used devices" | the DPP argument in the literature\'s own words | Zhao pp. 208–209 (+ Conti & Orcioni 2020) |
| WEEELABEX: boards > 10 cm² separated; EN 50625-1: reuse-prep mass counts, "avoid damage where there is potential for preparation for re-use" | regulatory anchors `[R]` | Zhao pp. 200–201 |
| Metals recovery from WPCBs (except Au) worse than mining (Pokhrel); hydromet 100 kg WPCB up to 702 kg CO₂-eq (Iannicelli-Zubiani) | reuse-beats-recycling case | Zhao p. 197 |
| SAC melts 217–225 °C; no gain > 250 °C; toxic gases ~280 °C; moisture delamination = main damage mechanism | process window | Zhao pp. 202, 209 |

**Framing rule:** Sc4\'s functional yield is a declared assumption — Sc4 is an exploratory
upper-bound with wide Monte Carlo bands, never presented with Sc2/Sc3 confidence.

**Eligible set:** processors 7 g + flash 1 g + power stages 9 g = **17 g** (large robust
packages; small SMDs excluded — 39.73 % removal `[L]`, per-piece testing impractical).

**Three-factor reuse chain:** harvest **0.94** `[L Park removal; upper 1.00 L Marconi]` ×
functional **0.70 `[A, 0.50–0.90]`** (unsourced; bathtub + Conti & Orcioni qualitative; the
DPP\'s documented provenance argues the upper half) × substitution **0.80 `[A, 0.5–1.0]`** =
**52.6 % of eligible ≈ 9.0 g reused**. Failed harvested parts return to the smelter feed.

**Build structure:** as Sc3, except dismantling electricity **0.02 kWh `[A, 0.005–0.05]`**
(component-level removal + functional-testing bench) and smelter feed **0.132 kg** (0.141 −
0.009 reused). Ledger: 0.4749 scrap-out + 0.0111 sorting losses + 0.132 smelter + 0.0325
combustible + 0.009 reused ≈ 0.660 ✔.

**Credits — `VCU S5 EoL Sc4 credits (avoided virgin + components)`:**

| Credit | Amount | Basis |
|---|---|---|
| **Avoided IC production** (`market for integrated circuit, logic type`, S1 UUID) | **4.2 g** | reused processors + flash; the most burden-dense credit in the model |
| **Avoided transistor production** (`market for transistor, surface-mounted`, S1 UUID) | **4.7 g** | reused power stages |
| Al primary ingot / chromium steel | 339.9 g / 11.8 g | as Sc3 |
| Cu cathode | **53.3 g** | Sc3\'s 56.8 − 3.5 g contained in reused parts |
| Au / Ag / Pd | **84.5 / 51.1 / 4.8 mg** | Sc3 values − proportional haircut `[A]` (reused 9 g = 6.5 % of board mass carries its PM share) — reused chips\' metals credited ONCE, inside the component credit |

Total **414.0 g** ✔ AS BUILT 2026-07-26.

---

### FINAL RESULTS (EF 3.1, 1 VCU cradle-to-grave, regenerated 2026-07-27 after the Sc1 split ruling — the thesis numbers)

All closures ✔ · credits exact ✔ · ordering Sc1>Sc2>Sc3>Sc4 monotonic in all 25 categories ✔ ·
Sc4 saving ≥ Sc3 saving everywhere ✔.

| | Sc1 | Sc2 net | Sc3 net | Sc4 net |
|---|---|---|---|---|
| Climate change [kg CO₂-eq] | 73.4 | 69.1 (**−6.0 %**) | 65.2 (**−11.1 %**) | 58.0 (**−21.0 %**) |
| Resource use, minerals & metals [kg Sb-eq] | 0.0187 | 0.0169 (**−10.0 %**) | 0.0127 (**−32.2 %**) | 0.0099 (**−47.3 %**) |
| Saving ratios | — | — | Sc3/Sc2: climate 1.85× · minerals 3.22× | Sc4/Sc3: climate 1.89× · minerals 1.47× |

**Findings frozen for the thesis:** ① reuse pays disproportionately in carbon/fossils (avoided
IC fabrication), dismantling-for-smelting pays in minerals — the two interventions answer
different environmental questions; ② the reused 9 g = 1.4 % of device mass deliver ~45 % of
Sc4\'s climate saving — recovery priorities should be burden-weighted, not mass-weighted:
exactly the component-level information a DPP carries; ③ the use phase (~46 % of climate) caps
what EoL can do for carbon, while EoL strategy commands ~⅓–½ of the mineral footprint — guided
disassembly is primarily a critical-raw-materials instrument (EU DPP alignment); ④ results are
robust to the Bigum refinement (≤0.7 pp shift); ⑤ Sc4 exploratory — MC bands mandatory.

**openLCA implementation (all scenarios):** each scenario = own S5 process chaining the SAME S4
output; gross-side treatment datasets live-resolved (§0.2); savings as explicit credit
processes — **never netted silently**, always gross | saving | net (§0.3); Monte Carlo ranges
from the `[A]` tags; APOS caveat (§0.3) declared throughout.

**Changelog (methods history — details in session logs, not here):**
- 2026-07-25: Sc1–Sc3 built · smelter-burden asymmetry identified and fixed (both scenarios
  charge the SE smelter pro-rata) · dismantling slot filled with linkable exchanges · kWh-unit
  incident found via GUI inspection and repaired in place (lesson: logs report intent, the
  database view verifies).
- 2026-07-26: Sc4 specified from first-hand literature (removal ≠ damage-free ≠ functional
  correction) and built · Bigum Table 8 read first-hand and adopted — six `[A]` yield cells
  became `[L]` · residue ledger corrected (remelt losses at remelter, not our landfill) ·
  final results computed.
- 2026-07-27: 13 uncertainty distributions written; 7 product systems created; impact suite run
  (EF 3.1 + ReCiPe Mid/End + corrected screening) · **Sc1 split ruled BOM-derived
  (0.5875/0.0725, in-place edit)** — the last v3-legacy number removed; all CSVs regenerated ·
  **Monte Carlo completed** (n = 1000 × 7 systems, GUI route after the IPC server's ~300-draw
  leak; credit distributions non-overlapping, `mc_net.csv` ruled unusable for scenario
  comparison — §5).
- 2026-07-28: **ReCiPe version corrected.** The ecoinvent-shipped packs were identified as
  ReCiPe **2008** (category names/units, no normalisation set) and replaced by the openLCA-pack
  **ReCiPe 2016 Midpoint (H) + Endpoint (H)**; both result CSVs regenerated in place (no
  superseded twins). ReCiPe normalisation and weighting tested and **rejected on evidence**
  (§4.2.1); the endpoint is reported as three AoP damage totals instead of a single score.
  Prioritisation stays with EF 3.1.

**Open items:** ① VCU_BOM_v4.xlsx regeneration; ② optional GUI figures (contribution tree +
Sankey, Sc1 + Sc3, EF 3.1). All computation is complete.
## 4. From model to results — concepts behind the analysis steps (methodology-chapter source)

This chapter explains, in plain terms, WHAT each step between the finished model and the
reported results is and WHY it is performed. It documents ideas, not code — the scripts in
`Scripts\` implement exactly what is described here.

### 4.1 Product systems — what they are and why we create them

A *process* in openLCA is a single recipe: one box with inputs and outputs, where each input
names a preferred supplier (its default provider) but is not yet connected to it. A *product
system* is what the model becomes when openLCA resolves those links: starting from one final
process, it follows every default-provider reference recursively — through our five foreground
stages and onward into the thousands of ecoinvent background datasets — until a complete,
closed supply network exists that can be solved as one matrix. Creating a product system is
therefore the step that turns a set of documented recipes into a computable model of the whole
life cycle.

Why persistent product systems rather than the on-the-fly calculations used during model
checking: a saved product system is a permanent, inspectable object. It provides (a) the
contribution tree — which stage and which background chain drives each impact category, the
basis of the thesis result figures; (b) the Sankey diagram of impact flows; and (c) the anchor
for Monte Carlo simulation, which openLCA runs on product systems only. All systems are linked
with the "prefer default providers" rule, so the resolved network is exactly the chain of
providers approved at each build checkpoint (§0.2).

**The seven product systems** follow the gross | saving | net reporting convention (§0.3 —
savings are never netted silently inside a model):

| Product system | Role |
|---|---|
| `VCU S5 EoL Sc1 (no recycling)` | Sc1 total (baseline; no credits exist) |
| `VCU S5 EoL Sc2 (bulk recycling)` | Sc2 GROSS (burdens incl. shredding + smelter feed) |
| `VCU S5 EoL Sc2 credits (avoided virgin)` | Sc2 SAVING (avoided virgin production) |
| `VCU S5 EoL Sc3 (guided disassembly)` | Sc3 GROSS |
| `VCU S5 EoL Sc3 credits (avoided virgin)` | Sc3 SAVING |
| `VCU S5 EoL Sc4 (disassembly + reuse)` | Sc4 GROSS |
| `VCU S5 EoL Sc4 credits (avoided virgin + components)` | Sc4 SAVING (incl. avoided IC/transistor production) |

Net result per scenario = gross − saving, assembled in the results scripts.

### 4.2 Impact assessment methods — what EF 3.1 and ReCiPe are and why both are used

An inventory result is thousands of elementary flows (kg CO₂, mg silver ore, MJ crude oil…).
An *impact assessment method* converts them into a small set of environmental indicators by
multiplying each flow with a characterization factor. Two levels exist: *midpoint* indicators
sit early in the cause–effect chain (e.g. kg CO₂-eq of climate forcing) and are scientifically
robust; *endpoint* indicators aggregate further into damages (e.g. years of healthy life lost)
and are easier to communicate but more model-dependent.

**EF 3.1 (adapted)** — the primary method. It is the European Commission's Environmental
Footprint method (the basis of the Product Environmental Footprint, PEF), with 16 midpoint
categories and standardized characterization factors. It is chosen as primary because this
thesis argues in an EU policy context — the Digital Product Passport is an EU instrument, and
EF is the EU's own measurement convention. The two goal-pinned categories are *climate change*
(kg CO₂-eq) and *resource use, minerals and metals* (kg Sb-eq — antimony-equivalents from
abiotic depletion, the category where precious metals dominate). EF additionally provides
*normalization* (expressing each category result as the fraction of an average person's annual
impact — person-equivalents) and *weighting* (EU-agreed importance factors), which enable the
PEF "most relevant categories" screening: rank weighted category contributions and report the
set covering ≥80 % of the cumulative total, plus the goal-pinned categories. All 16 remain shown.

**ReCiPe 2016 Midpoint (H)** — the cross-check (openLCA pack; Huijbregts et al. 2017). An
independent, globally used method with 18
midpoint categories. The (H) is the *Hierarchist* perspective: consensus-based modeling choices
(e.g. 100-year global warming horizon) — the standard middle ground between the short-term
Individualist and precautionary Egalitarian perspectives. If a conclusion holds under both EF
and ReCiPe characterization, it is robust to the choice of method — a supervisor-requested check.

**ReCiPe 2016 Endpoint (H)** — the aggregated view. It compresses the midpoints into damage to
three *areas of protection*: human health (DALY — disability-adjusted life years), ecosystem
quality (species·year lost) and resource scarcity (USD2013 of added future extraction cost).
Its purpose here is communication: a small set of damage scores, readable without LCA training.
**Reported as the three characterised AoP damage totals — no normalisation, no weighting, no
single score** (reasoning in §4.2.1).

#### 4.2.1 Why ReCiPe normalisation and weighting are NOT used (decided 2026-07-28)

Both were tested and both were rejected on evidence, not preference.

*Normalisation.* ReCiPe's World (2010) references were read out of the database and verified
against the published table (global warming 7 990 kg CO₂-eq; mineral resource scarcity
1.201 × 10⁵ kg Cu-eq; fossil 980.4 kg oil-eq — exact matches). Normalising this study's results
against them puts **mineral resource scarcity at 0.0 %** of the profile and the three toxicity
categories at ~90 %, because the VCU's 2.2 kg Cu-eq is negligible against a per-capita mineral
reference of 120 000 kg Cu-eq while its 35 kg 1,4-DCB is large against an ecotoxicity reference
of 25 kg. Toxicity dominance of normalised ReCiPe profiles is a known property of the method's
reference inventory, not a statement about this device. Reporting it as a prioritisation would
assert that the critical-raw-materials question this thesis studies is irrelevant.

*Weighting / single score.* openLCA's endpoint NW sets normalise each endpoint category by its
own midpoint-derived reference and then apply the AoP weight, rather than aggregating damages
within an area of protection and normalising against the per-capita AoP reference. Verified
numerically: endpoint points ÷ weight reproduces the **midpoint** person-equivalents
(ratios 0.995–1.001 across freshwater ecotoxicity, marine ecotoxicity, human carcinogenic
toxicity, global warming and mineral resource scarcity). The resulting "single score" is
therefore weighted midpoint normalisation in damage-unit clothing: it adds no endpoint
information and inherits the toxicity dominance in full — resources land at 0.13 % of it.
Rejected.

*What is reported instead.* The three AoP damage totals, summed within their native units.
Endpoint damages are additive by construction, so this requires no factor, no reference and no
value judgement — the most defensible form of the aggregated view available from these data.

*Consequence worth stating in the thesis.* EF 3.1 and ReCiPe select **completely disjoint**
reporting sets from the identical inventory — EF: minerals & metals, climate, freshwater
eutrophication; ReCiPe: freshwater ecotoxicity, marine ecotoxicity, human carcinogenic
toxicity. That disjointness is the strongest available evidence for the standing caveat that
normalisation and weighting are the most value-laden layer in LCA. Prioritisation therefore
stays with EF 3.1, whose weights are the European Commission's — the same policy frame the DPP
regulation itself sits in, so the value system matches the research question. ReCiPe's role is
**characterisation robustness**, at characterised level only.

### 4.3 Uncertainty distributions — what they are and why the model carries them

The model was built deterministically: every quantity is a single number carrying a source tag
(§0.5). But the tag system records more than the central value — every `[A]` assumption was
declared WITH a range, and several `[L]` values come with a published spread. An *uncertainty
distribution* formalizes that range: instead of "the use-phase draws 66.2 kWh", the exchange
states "between 54 and 189 kWh, most likely 66.2" — a *triangular* distribution (minimum,
mode, maximum). Where no value within the range is more likely than another (the SMT board-area
question), a *uniform* distribution is used. The distributions are written directly onto the
model's exchanges, so they travel with the database, are visible in the GUI, and feed the
simulation.

Why: single numbers overstate certainty. The honest tag system would be wasted if the reported
results dropped the ranges it so carefully declared. Thirteen exchanges carry distributions —
the use-phase electricity (widest absolute range), die-casting energy (Dalquist & Gutowski's
6.5–9 MJ/kg spread), SMT area, the transport leg, both dismantling electricities, the three
aluminium credits (the 79–95 % remelt question), Sc3's gold credit (Lee's 90–98 % arrival
range) and Sc4's two component credits (the unsourced functional yield, 0.5–0.9 — deliberately
the widest relative band in the model). **Declared limitation:** distributions are sampled
independently per exchange; correlations (e.g. one dismantling-quality draw driving gold,
silver and palladium jointly) are not represented — a standard simplification, stated openly.

### 4.4 Monte Carlo simulation — what it is and why it is the final step

A Monte Carlo simulation recalculates the entire product system many times (here: on the order
of 1,000 iterations), each time drawing a random value from every uncertainty distribution.
The output is no longer one number per impact category but a *distribution* of results, from
which percentiles and confidence intervals are read (e.g. "Sc3's climate net lies between X
and Y in 90 % of draws").

Why it matters to this thesis: the scenario comparison must survive the declared uncertainty.
The central results say Sc3 outperforms Sc2 in every category — Monte Carlo answers whether
that ordering holds when every assumption is allowed to vary across its declared range. For
Sc4 it is not optional but mandated by the framework: Sc4's functional yield is an unsourced
assumption, so Sc4 may only be reported as a band, never as a point value. Gross and credit
systems are simulated separately and the net distribution is assembled from them; this is
statistically clean here because no distributed parameter appears in both a gross and a credit
system (the sets are disjoint), so the two simulations are genuinely independent.
## 5. Numerical result outputs — deterministic CSVs + Monte Carlo (COMPLETE 2026-07-27)

All quantitative thesis results live in these four files in
`LCA_Analysis\Outputs\3_impact_assessment\` (folder reorganization 2026-07-27: Scripts\ and
Outputs\ are grouped by development stage — `0_utilities`, `1_stage_builds`, `2_eol_scenarios`,
`3_impact_assessment`, `4_monte_carlo`; each script writes its outputs to the matching
Outputs\ subfolder; Monte Carlo files incl. the GUI `simulation_result_*.xlsx` exports live in
`Outputs\4_monte_carlo\`). They are
the authoritative numerical record: every results table and chart is built from them, and any
GUI calculation of the same product systems reproduces them exactly (same model, same solver).
Common structure of the three impact files — one row per impact category with columns
`sc1 · sc2_gross · sc2_saving · sc2_net · sc3_gross · sc3_saving · sc3_net · sc4_gross ·
sc4_saving · sc4_net · unit`, where net = gross − saving (§0.3 convention, never netted
silently) and all values are per functional unit (1 VCU, cradle-to-grave).

| File | Content |
|---|---|
| `impact_EF31.csv` | **Primary results.** EF 3.1 (adapted), all 25 rows (16 categories + sub-indicators, e.g. the climate-change biogenic/fossil/land-use split). Source of the headline table: climate 73.4 kg CO₂-eq (Sc1) → −6.0/−11.1/−21.0 %; minerals & metals 0.0187 kg Sb-eq → −10.0/−32.2/−47.3 %. |
| `impact_ReCiPe_mid.csv` | **Method cross-check.** `ReCiPe 2016 Midpoint (H)` (openLCA pack), 18 midpoint categories, regenerated 2026-07-28. Ordering Sc1>Sc2>Sc3>Sc4 **monotonic in all 18** (verified). Key Sc4 reductions: global warming 74.4 kg CO₂-eq → −21.1 % (EF climate −21.0 %, near-identical); mineral resource scarcity 2.203 kg Cu-eq → **−38.9 %** (EF minerals in Sb-eq → −47.3 % — same direction, magnitude depends on how scarcity is characterised); fossil −21.4 %; freshwater eutrophication −25.7 %. |
| `impact_ReCiPe_end.csv` | **Aggregated damage view.** `ReCiPe 2016 Endpoint (H)` (openLCA pack), 22 categories in native damage units (DALY / species·yr / USD2013), regenerated 2026-07-28. Characterised only — see §4.2.1 for why no normalisation, weighting or single score is applied. |
| `impact_ReCiPe_end_aop.csv` | **The damage headline.** The three area-of-protection totals, summed within their native units (additive by construction, no factors applied): human health 3.318×10⁻⁴ DALY → −9.4 / −21.1 / −32.5 %; ecosystem quality 4.657×10⁻⁷ species·yr → −7.4 / −15.9 / −25.8 %; resources 3.714 USD2013 → −9.1 / −17.2 / −29.2 %. Monotonic in all three. This replaces the former single score. |
| `impact_screening_ReCiPe_mid.csv` | **Not a prioritisation table — evidence for §4.2.1.** ReCiPe midpoint normalised on the World (2010) H set (normalisation only; ReCiPe defines no midpoint weights). Ranks freshwater ecotoxicity 37.8 % / marine ecotoxicity 29.0 % / human carcinogenic toxicity 24.5 %, with mineral resource scarcity at **0.0 %**. Cite the cumulative column as 'cumulative share of normalised impact', never as 'weighted footprint'. |
| `impact_screening.csv` | **EF normalization + weighting screening** on the Sc1 baseline (NW set of the EF method; corrected PEF rule — smallest set reaching ≥80 % cumulative, crossing category included). Result: minerals & metals **72.5 %** of the weighted footprint; reporting set = minerals & metals + climate change + freshwater eutrophication (85.7 % cum.); both goal-pinned categories inside it. All 16 categories are still reported. |

### Monte Carlo outputs (`Outputs\4_monte_carlo\`, COMPLETE 2026-07-27, n = 1000/system)

Route: openLCA **GUI** simulation per product system (1000 runs, EF 3.1, NW set none), exported
as `simulation_result_<key>.xlsx` (raw source, all draws) and parsed by
`Scripts\4_monte_carlo\mc_gui_parse.py` into the working CSVs. The IPC simulation route was
abandoned (server-side leak, ~300–350 draws/session hard ceiling, memory-independent — verified
empirically; GUI simulator unaffected). Samples OUR 13 foreground distributions (ch. 4.3) PLUS
ecoinvent's background pedigree uncertainties.

| File | Content |
|---|---|
| `mc_raw_<key>.csv` | One row per draw × 25 EF categories, per system (7 files, n = 1000). |
| `mc_summary.csv` | Per system × category: n, mean, sd, p5/p25/p50/p75/p95. |
| `mc_net.csv` | Net = gross − saving per scenario, draw-paired (independent-sampling approximation, ch. 4.4). |

**MC results to report (frozen 2026-07-27):**
- **Absolute footprints ~±20 %:** Sc1 climate 94.4 ± 18.4 kg CO₂-eq (p5–p95 70.0–128.1);
  minerals tighter, ±5 % (0.01895 ± 0.00096 kg Sb-eq). Driven by uncertainty SHARED across
  scenarios (use-phase triangle + ecoinvent background).
- **Credit totals tight and cleanly separated** (the scenario-differentiating quantities):
  climate saving Sc2 4.65 ± 0.17 / Sc3 8.42 ± 0.26 / Sc4 15.85 ± 0.99 kg CO₂-eq (CV 3–6 %);
  minerals saving 0.00187 ± 0.00001 / 0.00595 ± 0.00010 / 0.00890 ± 0.00043 kg Sb-eq
  (CV 0.7–4.8 %); fossils 61.7 ± 2.6 / 113.4 ± 4.2 / 212.2 ± 13.6 MJ (CV 3.7–6.4 %).
  In these three categories the p5–p95 bands of neighbouring scenarios **do not touch**
  (Sc4 p5 sits 1.6× / 1.4× / 1.6× above Sc3 p95). **Uncertainty statement: absolute values
  carry ±20 %, the scenario ranking does not.**
- **Exception, state it honestly:** in *freshwater eutrophication* (third reporting-set
  category) the Sc3 and Sc4 credit bands **overlap slightly** (Sc3 p95 0.0243 vs Sc4 p5 0.0229
  kg P-eq; means 0.0190 vs 0.0301 still ordered). Sc2/Sc3 remain clean. Report as: separation
  is unambiguous for minerals, climate and fossils; for freshwater eutrophication the Sc3–Sc4
  ranking holds in the mean but is not resolved at the 5th/95th percentile.
- **MC mean > deterministic central** (climate 94.4 vs 73.4): expected — asymmetric use-phase
  triangle (54–66.2–189 kWh; mean 103 vs mode 66.2). Report deterministic centrals (§ above) +
  MC intervals; never mix the two as if interchangeable.
- **`mc_net.csv` usage rule — DO NOT compare scenarios with it.** Each product system was
  simulated independently, so the gross systems (deterministically identical to within 0.07 kg
  CO₂-eq: 73.479 / 73.409 / 73.410) drew means 2.54 kg apart purely by sampling luck. That
  noise contaminates the nets: the true Sc2→Sc3 climate gap is 3.81 kg (69.06 → 65.25), but the
  MC net means show only 1.24 kg (87.36 → 86.12) — the scenario advantage is understated by
  two thirds. **Scenario ranking = deterministic table; uncertainty of scenario DIFFERENCES =
  the credit distributions above; `mc_net.csv` rows illustrate within-scenario spread only.**
- **Quarantined under MC** (ecoinvent pedigree lognormals dominate; CV on Sc1 in brackets):
  water use (3484 %), human toxicity cancer & non-cancer (1100–1850 %, sign flips), ionising
  radiation (113 %), land use (57 %). MC bands are reported only for minerals & metals (CV 5 %),
  climate (19 %), fossils (20 %) and — with the caveat above — freshwater eutrophication
  (63 % on the gross side, 16–18 % on the credits), with the exclusion and its reason stated.

Visual outputs (contribution trees, Sankey diagrams) are generated on demand from the persistent
product systems in the openLCA GUI and are pictures of these same numbers, not separate results.
