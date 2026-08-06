# Bill of Materials v4.1 — Vehicle Control Unit

**Reference device:** generic as-built VCU, inspired by Bosch Motorsport MS 50.4 (data sheet
245099915: ≤660 g · 166×121×41 mm · 3× AS018-35 connectors, 198 pins · IP67 · supply 5–18 V,
sensor supplies max 4 A @ 12 V + 2 A @ 5 V, outputs 2×7.5 A + 4×2.2 A ·
2× 667 MHz dual-core processors · 8 GB flash · MEMS IMU + pressure sensor · 6 power stages ·
10 sensor-supply rails · 3×Ethernet, 4×CAN, LIN, USB, RS232).
**Version:** v4.1 (restructured), 2026-07-24 · **Author:** Thiago Pegorer.
**Tags:** `[D]`=datasheet · `[L]`=literature · `[R]`=regulatory · `[A]`=assumption (declared, with range → Monte Carlo).

---

## Table 1 — Components (what dismantling acts on)

Central masses; ranges in brackets. Material splits inside components are `[A]` unless noted,
constrained by the literature totals in Table 2. Populated board = bare PCB + population = 138 g.

| # | Component | Total mass g [range] | Material breakdown (g, central) | Source of mass |
|---|---|---|---|---|
| 1 | Housing, 2 shells, HPDC | **344** [300–380] | Al alloy 344 | `[A]` geometry: 637 cm² `[D]` × 1.8–2.2 mm × 2.70 g/cm³; steel infeasible (≈1 kg > device); HPDC = volume-economic process |
| 2 | Fasteners (~12) | **12** [8–20] | stainless steel 12 | `[A]` ~1 g/screw |
| 3 | Bare PCB, 4-layer FR-4 | **63** [55–70] | glass fibre 29 · epoxy 20 · Cu foils 14 | `[A+ind.spec]` 150×105×1.6 mm; FR-4 1.9 g/cm³; 4×35 µm Cu 50–90 % coverage (sources §Notes) |
| 4 | Connectors 3× AS018-35 | **150** [135–165] | Al shell 95 · Cu-alloy contacts 35 · thermoplastic insert 15 · fluorosilicone 4.5 · **Au plating 0.06 [0.02–0.10]** · Ni underplating 0.5 | mass: distributor spec 50.00 g × 3 (M-CAL); materials: TE catalog. Internal split `[A]`; Au = plating area × 0.5–1 µm calc `[A]` |
| 5 | Processors 2× (FCBGA) | **7** [4–10] | substrate/epoxy 3.65 · Cu 1.85 · Si 0.9 · solder (Sn) 0.6 | `[A]` 2–5 g each, vol × density; split typical FCBGA |
| 6 | Flash 2× 4 GB | **1** [0.5–2] | epoxy 0.5 · Cu 0.25 · Si 0.15 · solder 0.1 | `[A]` |
| 7 | MEMS IMU + pressure (2–3 pcs) | **1** [0.5–2] | ceramic/epoxy 0.6 · Cu 0.2 · Si 0.2 | `[A]` |
| 8 | Power stages 6× (DPAK class) | **9** [5–13] | Cu 4.5 · epoxy 3.6 · Si 0.9 | `[A]` count `[D]` |
| 9 | Regulators ~10× | **3** [2–5] | epoxy 1.5 · Cu 1.2 · Si 0.3 | `[A]` count `[D]` |
| 10 | Comm transceivers ~10× | **1.5** [1–3] | epoxy 0.85 · Cu 0.5 · Si 0.15 | `[A]` count `[D]` |
| 11 | Analog front-end 4–8× | **1.2** [0.8–2] | epoxy 0.7 · Cu 0.4 · Si 0.1 | `[A]` |
| 12 | Ta capacitors 4–16× | **2.5** [0.6–5.6] | **Ta 0.9 [0.2–2.2]** (30–40 wt% `[L]` Oke & Potgieter 2024 ← Niu 2017) · MnO₂/epoxy/Ag rest | count `[A]` anchored to rail count `[D]`; 0.15–0.35 g/cap `[A]` |
| 13 | MLCCs, resistors, inductors, small passives + misc on-board | **44.9** [25–60] | ceramics 25 · termination Cu 4.85 `[A→closes Zhu board-Cu 27.6 g]` · termination Ni 0.70 `[L Zhu board-Ni]` · glass/ceramic frit 4.45 `[A]` · Fe 5.0 `[L]` · on-board Al 3.3 `[L]` · epoxy 1.6 | residual of 75 g population budget; class shares prior `[L]` Lee 2012 Fig. 5 (era-caveated) |
| 14 | Solder joints (SAC305 `[A]`, Pb-free `[R]`) | **3.2** [0.8–5.5] | Sn 3.2 (joints; +0.7 g Sn in package balls #5/6 → board Sn total 3.9 `[L]`) | `[L]` Zhu 2023: 2.8 % [0.7–4] of populated board |
| 15 | TIM | **8** [4–12] | silicone 8 | `[A]` |
| 16 | Conformal coating | **3** [1–5] | acrylic 3 | `[A]` |
| 17 | Labels/adhesives/misc | **5** [2–10] | mixed polymer 5 | `[A]` balancing item |
| | **Device total** | **660** `[D]` ✔ | | closure check §Notes |

Board population (#5–14) = **75 g** [40–90]; low anchor 17.5 % of populated board `[L]` Lee 2012
Table 1 — sparse 1996–2005 VCR boards, dense automotive SMD sits higher → central kept, range widened.

## Table 2 — Materials (whole device, 660 g — the LCA inventory view)

| Material | Conc. of device | Range | Mass (central) | Mass range | Found in (components #) |
|---|---|---|---|---|---|
| Aluminium (alloy) | **67 %** | 58–76 % | **442 g** | 385–500 g | housing 1 · connector shells 4 · on-board 13 |
| Copper | **9.6 %** | 6–13 % | **63.4 g** | 40–85 g | PCB foils 3 · contacts 4 · leadframes 5–11 · terminations 13 |
| Polymers/epoxy (molds, inserts, labels, coating, FR-4 resin) | **9 %** | 6–13 % | **60 g** | 40–85 g | 3, 4, 5–13, 16, 17 |
| Glass fibre (E-glass) | **4.4 %** | 3.5–5.5 % | **29 g** | 23–36 g | bare PCB 3 |
| Ceramics (incl. glass frit) | **4.6 %** | 2.5–6.5 % | **30.5 g** | 15–45 g | MLCCs/passives + terminations 13 · IMU 7 |
| Steel / Fe | **2.6 %** | 1.5–5 % | **17 g** | 10–33 g | fasteners 2 · leads/shields 13 (Fe 3.6 % of board `[L]`) |
| Silicone (TIM + seals) | **1.9 %** | 1–2.5 % | **12.5 g** | 6–17 g | 15 · 4 |
| Tin (solder) | **0.6 %** | 0.1–1 % | **3.9 g** | 0.8–6.4 g | 14 (2.8 % of board `[L]`) |
| Silicon (semiconductor) | **0.4 %** | 0.2–0.8 % | **2.7 g** | 1.3–5 g | dies 5–11 |
| Nickel | **0.18 %** | 0.05–0.6 % | **1.2 g** | 0.3–3.7 g | terminations 13 (0.5 % of board `[L]`) · connector underplating 4 `[A]` |
| Tantalum | **0.14 %** | 0.03–0.33 % | **0.9 g** | 0.2–2.2 g | Ta caps 12 (30–40 wt% `[L]`) |
| **Gold** | **0.014 %** | 0.005–0.04 % | **92 mg** | 30–270 mg | board 31.7 mg (230 ppm `[L]`) + connector plating 60 mg `[A]` |
| **Silver** | 0.009 % | 0.002–0.04 % | **59 mg** | 11–276 mg | solder, terminations (430 ppm of board `[L]`) |
| **Palladium** | 0.0008 % | 0.0002–0.002 % | **5.5 mg** | 1.1–13.8 mg | MLCC terminations (40 ppm of board `[L]`) |

Board-level concentrations (`[L]` = per populated-board mass 138 g): Zhu et al. 2023, Table 1.

---

## Notes, sources & checks

**Primary literature:**
- Zhu, Y. et al. (2023). Process Saf. Environ. Prot. 173, 437–451, Table 1 (16-study WPCB metal
  compilation; medians of typical-PCB studies, outliers excluded). <https://doi.org/10.1016/j.psep.2023.03.018>
- Lee, J., Kim, Y., Lee, J.-C. (2012). J. Hazard. Mater. 241–242, 387–394 (component/board split,
  class shares, separation efficiencies; VCR-era caveat). <https://doi.org/10.1016/j.jhazmat.2012.09.053>
- Oke, E.A. & Potgieter, H. (2024). J. Mater. Cycles Waste Manag. 26, 1277–1293 (Ta 30–40 wt% of
  Ta caps, citing Niu et al. 2017 <https://doi.org/10.1021/acssuschemeng.6b01839>).
  <https://doi.org/10.1007/s10163-024-01917-7>
- Connector: [M-CAL AS018-35PN product spec](https://m-cal.com/en-gb/2048043165-mc03-as018-35pn-deutsch-autosport-as-connector-66-way-shell-size-18-pin-layout-18-35-style-0-flange-receptacle-red-n-keyway-pins-standard) (50.00 g) ·
  [TE Deutsch Autosport catalog](https://cdn.wirecare.com/assets/pdfs/pages/customer-service/catalog-deutsch-autosport.pdf) (Al shell, Au-plated contacts).
- PCB parameters: [FR-4 density](https://www.bestpcbs.com/blog/2024/12/what-is-the-density-of-fr4/) ·
  [standard thickness](https://ksnpcb.com/demystifying-standard-fr4-thickness-a-comprehensive-guide-for-pcb-designers/) ·
  [Cu foil](https://www.allpcb.com/blog/pcb-manufacturing/fr4-material.html) (industry specs).

**Exclusions:** Pb `[R]` — ELV 2000/53/EC Annex II ex. 8(a): lead board-solder only pre-2016 type
approvals → current VCU lead-free ([Annex II](https://www.legislation.gov.uk/eudr/2014/13/annexes/data.xht) ·
[guide](https://www.imds-professional.com/wp-content/uploads/2021/09/Automotive-Industry-Interpretation-Guide-for-ELV-Annex-II.pdf)).
Zn (~0.6 %) — below 1 % cut-off criterion (framework §6), not impact-critical.

**Consistency checks:** ① device closes at 660 g `[D]` ✔ ② board metal medians ≈ 29.4 % vs
literature ≈30 % metals ✔ (Zhu; corroborated by Lee: Cu ~16 %, Au ~300 ppm, Ag ~500 ppm, Pd
~100 ppm — within ranges) ③ housing: steel infeasible by mass ✔ ④ Table 2 column sums to ~660 g ✔.

**Sc3/Sc4 parameters (from Lee 2012, for the scenario build):** dismantling removes 90–98 wt% of
components without board damage; 3-step separation (sieve/magnetic/dense-medium) sorts most
component groups at 74–90 % (condensers 77.2 %, inductors 81.5 %, resistors 78.9 %; diodes/
transistors <60 %); Au 62.9 % recovery at 7.15× concentration; Ni 72.5 %/5.0×; Sn 40.2 %/6.2×.

**v3 → v4 (thesis method narrative):** connectors 57.75 g brass → 150 g Al-shell/Au-contacts
(sourced); Au 62.7 → 92 mg (now incl. connector plating); Ag 250 → 59 mg; Pd 27.6 → 5.5 mg;
housing 363 → 344 g; Pb excluded by regulation. Smaller, defensible precious-metal inventory.

**Open follow-ups:** KEMET T491 datasheet (Ta-cap unit weight); TE engineering drawing for
AS018-35; component-level masses could be refined further if an automotive-ECU teardown study is found.
