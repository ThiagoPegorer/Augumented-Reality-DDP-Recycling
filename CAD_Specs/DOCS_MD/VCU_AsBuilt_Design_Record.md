# VCU generic teardown model — AS-BUILT design record

**Version: v3.0 · Last modified: 2026-07-19**

The authoritative record of the physical model as modelled in NX. Generic VCU device *inspired by*
the Bosch MS 50.4 — **not** a 1:1 replica (no access to the real unit), so it does not need to match
the datasheet envelope.

**Status: CLEARED FOR PRINT.** All fits validated by printed test coupons.

---

## Overview
- Deep-tray + lid enclosure, 3D-printed **PETG**.
- Overall: **200 × 150 × 45 mm** (bottom) + **20 mm** lid (15 body + 5 lip).
- Purpose: physical teardown artifact for AR user tests. Disassembly = **5 steps**.
- Assembly file: `assembly_model.prt`. Base component = `housing_bottom`.
- **Printing: in-house on a Bambu Lab P2S** (the earlier Facturee outsourcing was cancelled).

---

## 🔄 v3.0 CHANGES (2026-07-19)

Two changes, both driven by evidence from printed coupons rather than estimates.

### 1. Heat-set insert holes: Ø3.8 → **Ø4.0** + 0.5 × 45° chamfer

| | |
|---|---|
| **Applies to** | all **14** insert holes — every one is in `housing_bottom` |
| **Breakdown** | 4 lid bosses · 6 connector face · 4 PCB mounts |
| **Evidence** | Coupon v1 @ Ø3.8 **FAILED** — needed heavy pressure, insert wouldn't self-centre, set crooked. Coupon v2 (10×10×10 cube, Ø4.0 × 5 deep) **PASSED** — sinks under light pressure, clean finish. |
| **Chamfer** | 0.5 × 45° at the mouth — this is what lets the insert sit square before the heat goes in |

Ø4.0 against a ~4.6 mm knurl still grips fully — no holding strength lost.

### 2. Lid locating lip: 1 mm/side → **0.5 mm/side** (189 × 139)

The 1 mm slack was a hedge for the one-shot outsourced order. Home printing + verified dimensional
accuracy made it unnecessary, and 1 mm let the lid sit visibly skewed on a demo people handle.
Lip **189 × 139**, projecting 5 mm into the 190 × 140 cavity.

> ⚠️ **Both `housing_bottom` AND `housing_upper` STLs must be re-exported** for v3.0.
> The other 6 STLs are unaffected.

---

## Current dimension set (v3.0)

### Fits & clearances

| Feature | Value | Note |
|---|---|---|
| **Insert holes** (14, all in housing_bottom) | **Ø4.0** + 0.5×45° chamfer | coupon-validated |
| Insert hole depths | lid 6 · connector 5 (thru 5 wall) · PCB 5 (+2 solid below) | |
| Lid locating lip | **189 × 139**, 5 deep (0.5 mm/side) | in the 190×140 cavity |
| Connector bore (housing) | **Ø24.5** | |
| Connector body (back) | **Ø23.4** | ~1.1 mm diametral clearance |
| Lid screw clearance holes | **Ø4.2** | absorbs drift over a 200 mm part |
| Lid counterbores | **Ø6.3 × 3.5** | for M3 cap head |
| PCB mount clearance holes | **Ø4.2** | |
| Connector flange holes | **Ø4.0** | *clearance* for M3 shaft — not an insert hole |
| Chip ↔ PCB rib clearance | **0.5 mm both faces** | chips must pop out in step 4 |

> **Two different Ø4.0s — do not confuse them.** The 14 *insert* holes in `housing_bottom` (heat-set melts in) and the 6 *clearance* holes in the `connector` flange (M3 shaft passes through). They landed on the same number for unrelated reasons.

### Parts list

| Part file | Represents | Key dims (mm) | Colour | Teardown step |
|---|---|---|---|---|
| `housing_bottom` | Lower shell (deep tray) | 200×150×45, wall 5, R10 corners · 14× Ø4.0 insert holes · 3× Ø24.5 connector bores | grey | 5 |
| `housing_upper` | Lid | 200×150×20 (15+5 lip), R10/R5 · 4× Ø4.2 + Ø6.3×3.5 cbore on a 170×120 pattern · lip 189×139 × 5 | grey | 1 |
| `connector` **×3** | Round connectors | L43 (20 back + 3 flange + 20 front) · back Ø23.4 · flange 3 thick, R17 lobes, 2× Ø4.0 at ±20 (spacing 40) · bore Ø20 · collar Ø26 | R/Y/B *(optional — currently one colour)* | 2 |
| `pcb` | Main board | 170×120×4 (2 board + 2 rib), R5 corners · 4× Ø4.2 mount holes · raised ribs carry the chips | green | 3 |
| `component1` | Removable chip | 70×40×5 | yellow | 4 |
| `component2` | Removable chip | 50×30×5 | brown | 4 |
| `component3` | Removable chip | 60×40×5 | blue | 4 |
| `component4` **×3** | Removable bar | 40×10×5 | red | 4 |
| `bolt_M3` ×14 | M3 socket-cap (Allen) | shaft Ø3, head Ø5.5×3 | steel | shown exploded |

**6 removable chips total** (comp1 + comp2 + comp3 + 3× comp4), all 5 mm thick, recessed to plug onto the PCB ribs with 0.5 mm clearance.

### Assembly structure (from Assembly Navigator)
- `housing_bottom` = fixed base
- `housing_upper` → Touch on rim + Align on the 4 screw axes
- `connector` ×3 → Touch flange-to-front-face + Align to bore + screw holes
- `pcb` → Align + Touch on the floor mounts
- `component1–4` → Touch/Align onto the PCB ribs
- `bolt_M3` ×14 → at each screw hole (4 lid + 6 connector + 4 PCB)

---

## Fasteners
- **14 heat-set inserts** — 4 lid (M3×5×4) · 6 connector (M3×5×4) · 4 PCB (M3×4×4). All holes **Ø4.0** + chamfer.
- **14 M3 socket-cap (Allen) bolts** — lid needs **M3×16 or M3×18** (through the 15 mm lid + 5 mm into the insert); connector and PCB take shorter.
- Kit: **Preciva M3 360-pc (`B0F9WDPG6S`)** — covers inserts, bolts and driver.
- **Install technique:** iron at **250–260 °C** for PETG. Rest the tip 2–3 s and let the insert sink under its own weight — **pressing while cold is what makes it go crooked**. Square it flush at the end with a cold flat tool (ruler / caliper face).

---

## Export chain
NX **cannot** export STL (`3d_sla_systems` licence module not in the seat).
**NX → STEP → FreeCAD → STL** (one per part, mm, watertight).
Slicer settings: see `bambu_print_settings.md` in this folder.

**Print quantities:** housing_bottom ×1 · housing_upper ×1 · **connector ×3** · pcb ×1 · component1/2/3 ×1 each · **component4 ×3**.
**Do NOT print bolts or inserts** — those are hardware from the Preciva kit.

⚠️ **Always check STL timestamps are newer than the `.prt` files before slicing.** Stale exports have bitten this project once already.

---

## Verify before printing
- [ ] Lip corner radius **≤** cavity internal corner radius (~R5) — sharp lip corners bind before the flats meet. Matters more now at 0.5 mm than it did at 1 mm.
- [ ] `housing_bottom` + `housing_upper` STLs re-exported for v3.0.

---

## Version history
| Ver | Date | Change |
|---|---|---|
| v1 | 2026-07-09 | As-built captured from NX drawings after hand-modelling |
| v2 | 2026-07-13 | All fits opened up for a one-shot outsourced print (Facturee) |
| **v3.0** | **2026-07-19** | **Insert holes Ø3.8→Ø4.0 + chamfer (coupon-validated); lid lip 1→0.5 mm/side; printing moved in-house to Bambu P2S** |

**Backend (`vcu_001.json`) sync: ✅ DONE 2026-07-09** — generic identity, 200×150×60 specs, Allen-key tools, 5-step teardown, 14 inserts + 14 bolts, 6 removable chips. LCA/CO₂ figures deliberately frozen until the openLCA model is finished.
