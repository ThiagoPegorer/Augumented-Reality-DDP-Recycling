# DPP UI Spec — 14: Composition & Impact (+ the disassembly gate) — **v2**

> **Living spec** — ReBuilt v2.0. Status: **v2 DESIGN APPROVED 2026-07-30, NOT BUILT.**
> v1 (built + device-tested 2026-07-29) showed the LCA modal promoted to a main panel.
> v2 receives **Materials & composition** and the **Indicators** category from spec 13 and restructures
> the panel into **three tappable blocks**. Mock: `drafts/14_v4_composition_impact.svg`.
> Builder: `RBv2_0/7`. Data provenance: `dpp_payload_v07_bom_reconciliation` (project memory).

---

## 1. Purpose

The comprehension phase, front-loaded on Ms. Elle Langer's feedback (2026-07-29). The participant meets
what the unit is **made of** and what that **costs**, next to the interactive 3D twin, before any timed
task. Screen title: **`Composition & impact`**.

**The timer does not run here** — it starts at `Start disassembly`. The panel says "No timer yet" out
loud, because a participant who thinks they are being timed rushes the phase this screen exists to create.

**The zone is absent from the 5 disassembly steps** (`ScreenRouter.zoneFollowsExploration`). Cost for
Limitations: spec 00 §9's three-layer model loses layer 3 during the task; layer 2, the how-to loop
inside the instruction panel, stays.

## 2. Layout (panel 640 × 430; panel-local = mock SVG minus 20)

Header per spec 13 §2. Title `Composition & impact`, right caption `660 g · 11 components`,
Back → `ScreenRouter.ShowDppCanva()`.

| Block | x | y | w | h |
|---|---|---|---|---|
| **1 — Materials & composition** (stroke `teal/accent`) | 24 | 88 | 592 | 100 |
| **2 — Climate · EoL scenarios** (stroke `row/stroke`) | 24 | 196 | 290 | 138 |
| **3 — Recovery rate** (stroke `teal/accent`) | 326 | 196 | 290 | 138 |
| Gesture hint (2 lines) 11 `text/tip` | 24 | 366 / 380 | 320 | 16 |
| Primary CTA `Continue ›` | 288 | 354 | 328 | 52 |

**The circularity-indicator row from the earlier draft is REMOVED** (Thiago, 2026-07-30): repairability,
recyclability and reusability are already expressed by these three blocks, so a separate row of hollow
"not provided" indices added nothing.

## 3. Block 1 — Materials & composition

**Aggregate by MATERIAL from `components[].material_breakdown`, never from a hardcoded grouping.** One
source of truth, and it is the convention CIRPASS itself uses for composition (D2.2 **Figure 13**, p. 40),
so the chart form is citable rather than stylistic.

- Stacked bar 548 px wide = 100 % of total mass, segments in descending mass order, min width 2 px so
  trace materials stay visible. Largest segment carries an inline `Aluminium 67 %` label.
- Legend: the top five by mass with absolute grams, then `+ N more`.
- Header right: the trace metals from `precious_metals[]` — currently **Ta 900 · Au 92 · Ag 59 · Pd 6 mg**.
- Footer: `tap for material location per component · splits [A] assumed, VCU_BOM_v4`.

Derived material shares (v0.7 payload, reproducing BOM_v4's By_Material sheet exactly):
Aluminium 442.3 g (67.00 %) · Copper 57.75 (8.75) · Polymers/epoxy 56.85 (8.61) · Glass fibre 29 (4.39) ·
Ceramics 25.6 (3.88) · Steel/Fe 17 (2.58) · Silicone 12.5 (1.89) · Mixed terminations 10 (1.51) ·
Tin 4.6 (0.70) · Silicon 2.7 (0.41) · Tantalum 0.9 · Nickel 0.5 · Other 0.3 · Au/Ag/Pd traces.

**Drill-down (later): material location per component** — this is Table 6 **#5/#6** and the capability a
paper WEEE sheet cannot offer (CIRPASS Table 8 p. 56, UC4 Figure 16 step 2).

## 4. Block 2 — Climate · EoL scenarios

Four columns, one per scenario, **each of total height = Sc1**. Dark `tab/inactive-fill` = the remaining
net footprint; **teal `teal/light` cap = the avoided emission**. So the bars are equal-height and the
*cap* is the story — otherwise four near-identical bars would hide the point.

| | Sc1 | Sc2 Recycling as usual | Sc3 Guided dismantling | Sc4 Dismantling + reuse* |
|---|---|---|---|---|
| kg CO₂-eq | **73.4** | 69.1 | 65.2 | **58.0** |
| avoided | — | 4.4 | 8.2 | 15.4 |

Footer: `use phase caps what EoL can do for carbon` — thesis finding #3, stated on screen.
\* Sc4 is exploratory: functional reuse yield declared **[A]** (0.5–0.9), not quantified in literature.

**Drill-down (later):** openLCA process inputs & outputs per stage.
⚠ **Why this block is scenarios and not S1–S4 stages:** there is **no per-stage impact export** in
`LCA_Analysis/Outputs/`. `SESSION_RESUME.md` tables every result file and the GUI contribution tree is
listed as optional/not produced. `environmental.lifecycle_stages` is therefore `[]` with
`lifecycle_stages_basis: not_provided`. Restoring an S1–S4 chart needs one openLCA GUI export
(Sc1, EF 3.1, climate change, contribution tree by stage).

## 5. Block 3 — Recovery rate

Three rows, one per EF 3.1 screening category, each a 240 px track = 50 %. The bar is **stacked by
scenario** so each deeper route's *extra* gain is visible: Sc2 `teal/text` → Sc3 `teal/light` →
Sc4 `teal/accent`. Total reduction right-aligned on the label line.

| Category | screening share | Sc1 baseline | Sc2 | Sc3 | Sc4 |
|---|---|---|---|---|---|
| Resource use minerals and metals | 72.45 % | 0.0187391 kg Sb-eq | −10.0 % | −32.2 % | **−47.3 %** |
| Climate change | 6.67 % | 73.4326 kg CO₂-eq | −6.0 % | −11.1 % | −21.0 % |
| Eutrophication freshwater | 6.58 % | 0.11592 kg P-eq | −5.9 % | −16.5 % | −25.7 % |

Bound to `environmental.impact_recovery[]` (schema v0.7). Source
`Outputs/3_impact_assessment/impact_EF31.csv`.
⚠ **Never populate from `mc_net.csv`** — independent sampling understates the scenario gaps by ~⅔.

**Blocks 2 and 3 together carry thesis finding #3:** climate barely moves while minerals move a lot →
the AR-DPP is primarily a **critical-raw-materials instrument**. Neither block says that alone.

## 6. The gate — `CONTINUE TO DISASSEMBLY?` (unchanged from v1)

Own root canvas `ContinueGateCanvas`, 440 × 210, `sortingOrder 10`, own GraphicRaycaster + grabber bar,
recenters 0.7 m in front on open. Title `Continue to disassembly?`; two subtitle lines; secondary
**`Quit`** c(119,156) → Welcome; primary `Continue ›` c(329,156) → disassembly intro.

**The asymmetry is deliberate** — every other Back edge moves one level; this one leaves the product
session, so the button is **Quit**, never Back. `ContinueGate.Quit()` routes through `ShowDppCanva()`
**before** `ShowWelcome()` because the zone is a separate ROOT canvas that Welcome cannot hide.

## 7. Open items

- [ ] Drill-down modals for all three blocks (deferred: "build just the tabs").
- [ ] openLCA GUI contribution-tree export → restores a real S1–S4 breakdown.
- [ ] `recovery_potential.credits` is `[]` — per-material credit attribution is not exported by the
      openLCA runs. If the per-material view is wanted, it needs a new export.
- [ ] Scenario labels are on screen and in the payload; confirmed 2026-07-30 but worth re-checking
      against `LCA_framework_v4.md` Stage 5 wording before the thesis figures are cut.

## 8. Iteration log

- **2026-07-29** — v1 built (LCA modal promoted to main panel) and device-tested.
- **2026-07-30 (a)** — Materials & composition and Indicators moved here from spec 13; three-block
  structure agreed; circularity row dropped.
- **2026-07-30 (b)** — payload swapped to openLCA EF 3.1 + VCU_BOM_v4; S1–S4 chart replaced by the
  four-scenario climate ladder because no per-stage export exists; composition switched to by-material.

*Last updated: 2026-07-30 · Status: v2 design approved, not built · Prev: 13 DPP Canva · Next: tutorial (block 4)*
