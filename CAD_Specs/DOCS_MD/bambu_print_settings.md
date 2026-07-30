# Bambu Studio print settings — VCU teardown model

**Printer:** Bambu Lab P2S · **Nozzle:** 0.4 mm (stock — do NOT swap to 0.2, it roughly triples print time)
**Filament:** PETG Basic (35 % RH — fine to print) · **Plate:** Textured PEI
**STL source:** `CAD_Specs/PRINT FILES/FreeCAD/*.stl`
**Rev:** v3 — updated 2026-07-18 after the coupon test

---

## 🔴 BLOCKER — do this in NX before printing anything

The coupon proved **Ø3.8 is too tight**: the insert needed heavy pressure, wouldn't self-centre, and set crooked.

| Feature | Was | **Change to** | Where |
|---|---|---|---|
| Heat-set insert holes | Ø3.8 | **Ø4.0** | all **14** — 4 lid bosses · 6 connector face · 4 PCB mounts |
| Hole mouth | — | **0.5 × 45° chamfer** | every insert hole — this is what lets the insert sit square before heat |

Ø4.0 against a ~4.6 mm knurl still grips fully — no strength lost, it just stops fighting you.
**Unchanged:** clearance holes (Ø4.2 lid, Ø4.2 PCB, Ø4.0 connector flange), connector bore Ø24.5, lip 188×138, chip clearance 0.5 mm.

→ Then **re-export STEP → re-export STLs (FreeCAD) → re-slice.**
→ **Reprint the coupon at Ø4.0 first** (~20 min). A bad insert fit found after a 12 h housing print costs the whole part.

### Insert technique (also from the coupon)
- Iron at **250–260 °C** for PETG.
- **Do not press cold.** Rest the tip on the insert with almost no force for 2–3 s — it should begin sinking under its own weight. Pressing hard = plastic not soft yet = crooked insert.
- **Square it at the end:** once flush, press briefly with something flat and cold (metal ruler / caliper face) while the plastic is still soft.
- Don't reuse an insert that's been pulled out — you have 130 in the kit.

---

## Infill & walls — the question that actually matters

**Short answer: 15 % infill is fine. Use 4 wall loops on the parts that carry inserts.**

Why: slicers print **wall loops around holes**, not just the outer perimeter. A Ø4.0 hole automatically gets 3–4 perimeters ringing it = **1.2–1.6 mm of solid plastic** exactly where the insert's knurls bite. The sparse infill sits *outside* that ring. This is why heat-set inserts work fine in ordinary 15–20 % prints.

| Part | Wall loops | Infill | Reason |
|---|---|---|---|
| `housing_bottom` | **4** | 15 % | carries 10 inserts |
| `pcb` | **4** | 15 % | carries 4 inserts |
| everything else | 3 | 15 % (connectors 25 %) | no inserts |

**Do NOT raise global infill on the big parts** — it adds hours for no benefit.
**Modifier cubes are NOT needed.** You'd need 14 of them, each hand-positioned (and each must enclose the boss through its *full height* or it does nothing). Only revisit if a real boss fails after the Ø4.0 change.
**Coupon is the exception:** print the coupon at **100 % infill / Rectilinear** so it measures hole diameter in solid plastic, not infill.

---

## Base profile — apply to EVERY part

Start from **0.20 mm Standard @BBL P2S**, then change:

| Tab | Setting | Value |
|---|---|---|
| Quality | Layer height | **0.20** |
| Quality | Initial layer height | **0.25** |
| Quality | Seam position | **Back** |
| Strength | Wall loops | **3** (→ **4** on housing_bottom & pcb) |
| Strength | Top / Bottom shell layers | **4 / 3** |
| Strength | Sparse infill density | **15 %** |
| Strength | Sparse infill pattern | **Grid** |
| Support | Enable support | **OFF** |
| Others | Brim type | **Outer brim only** (see note) |
| Others | Brim width | per-part below |

> **Brim type — use "Outer brim only", NOT "Outer and inner brim".** `housing_bottom` is a tray with a large enclosed cavity; inner brim would lay material *inside* that cavity against the interior wall, which you then have to peel out of a 190×140 box — and residue sits exactly where the PCB and standoffs need a flat floor. Warping pulls at the **outside** corners, so outer brim is the part that actually does the work.

**Leave alone:** X-Y compensation = **0** (would silently resize every hole). Prime tower / purge = multi-filament only, ignore.
**At 100 % infill** Bambu asks to switch to **Rectilinear** — accept (Grid self-overlaps when solid).

**STEP import dialog** (if importing .stp): Linear deflection **0.003**, Angle deflection **0.1** — the 0.50 default makes round holes polygonal and undersized.

---

## Per-part settings

### 1. `housing_bottom` — 200 × 150 × 45 · **LONGEST, run overnight**
| Setting | Value |
|---|---|
| Orientation | **Cavity opening UP**, flat outer bottom on plate |
| Brim | **Outer brim only, 5 mm** — a corner lift costs 12 h. Do NOT use inner brim (it prints inside the cavity) |
| Wall loops | **4** |
| Infill | 15 % · Supports **OFF** |

The 3 horizontal Ø24.5 connector bores will sag slightly at the top of the circle. **Accept it** — the Ø23.4 connector has 1.1 mm clearance. Supports inside a bore scar worse than the sag.

### 2. `housing_upper` (lid) — 200 × 150 × 20 (15 + 5 lip)
| Setting | Value |
|---|---|
| Orientation | **Flat TOP face DOWN**, lip pointing UP |
| Brim | **Outer brim only, 5 mm** |
| Wall loops | 3 · Infill 15 % · Supports **OFF** |

Top-face-down = best finish on the visible face, no supports. The Ø6.3 × 3.5 counterbores print fine as recesses.

### 3. `pcb` — 170 × 120 × 4 (2 board + 2 rib)
| Setting | Value |
|---|---|
| Orientation | **Flat, ribs UP** |
| Brim | Optional (thin + wide = most warp-prone after the housing) |
| Wall loops | **4** · Infill 15 % · Supports **OFF** |

### 4. `connector` — 50 × 34 × 43 · **QTY 3**
| Setting | Value |
|---|---|
| Orientation | **Flange DOWN**, tube UP |
| Brim | No · Wall loops 3 · Infill **25 %** · Supports **OFF** |

All 3 on one plate. Colour-code R/Y/B only if time allows — schedule marks it "optional, low".

### 5–8. Chips — all flat plates, same settings
| Part | Size | Qty |
|---|---|---|
| `component1` | 70 × 40 × 5 | 1 |
| `component2` | 50 × 30 × 5 | 1 |
| `component3` | 60 × 40 × 5 | 1 |
| `component4` | 10 × 40 × 5 | **3** |

Orientation **flat, recess UP** · No brim · Wall loops 3 · Infill 15 % · Supports **OFF**.
All 6 fit on **one plate together**.

---

## Plate order (single printer, ~25–30 h total)

| # | Plate | Est. | Actual |
|---|---|---|---|
| 0 | coupon @ Ø4.0 — 100 % infill, rectilinear | ~20 min | ✅ passed |
| 1 | **housing_bottom** | — | **4 h 27 m** · 208 g · 0 filament changes |
| 2 | housing_upper | ~2–3 h | |
| 3 | pcb + 3 connectors + 6 chips | ~3–4 h | |

**Total ≈ 10–12 h**, not the 25–30 h originally estimated. A 5 mm-walled hollow tray at 15 % infill has far less material than its bounding volume suggests.

⚠️ **Schedule:** lab study #1 is **Mon 20 Jul**. With the real timings there is slack — enough to colour-code the connectors (R/Y/B) and still finish comfortably.

**Suggested order change:** print `housing_upper` **second** (not the small parts) so you can test the **lip fit** and **insert seating** as soon as both housing halves exist — resolving the riskiest unknowns while the small parts print.

---

## Assembly reminders
- **14 inserts:** 4 lid bosses · 6 connector face · 4 PCB mounts.
- **Bolts:** lid needs **M3×16 or M3×18**; connectors and PCB take shorter. All from the Preciva kit (`B0F9WDPG6S`).
- Let the plate **cool before flexing** — PETG bonds hard to PEI and will tear the surface off a warm plate.
- If you see stringing or a rough surface on the housing → dry PETG at **65 °C for 4–6 h**.
