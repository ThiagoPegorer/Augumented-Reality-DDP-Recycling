# 08 — Disassembly step texts · final nomenclature (RBv2.1.1)

**Status:** v1.1 · 2026-08-10 · rulings by Thiago (screws = 14; names = Component ID verbatim;
step 5 material-neutral — see ruling 3).
**Scope:** DATA ONLY — the payload's `disassembly` block (both copies). The RB2_0 step-flow
screens render these strings at runtime; no builder phase, no view change, no re-run of
`RBv2_0/4`/`/5` (forbidden while the stage clone exists). Step count, order and difficulty are
UNCHANGED — the no-mid-study-changes rule stands; this is nomenclature only.

## 1. The rulings

1. **Screw count = 14** (4 lid + 3×2 connector + 4 board — the count a dismantler physically
   verifies on the mock and the CAD). BOM row 2 budgets "~12 × M3" at 12.0 g; the delta is
   reconciled in the thesis, the mass stays the BOM figure. Consequence applied with it: the
   Component ID row renamed **"Fasteners (14 x M3)"** with a `represents` note carrying the
   BOM reconciliation — the passport no longer contradicts the step texts on adjacent screens.
2. **Step texts use the Component ID names** from Product specifications — the passport and
   the instructions speak one language. The mock-part vocabulary ("2 processors + 1 CPU +
   3 sensors") is gone from the payload; it survives only in `physical_unit` (the printed
   replica's own description, which is exactly where it belongs).
3. **Step 5 is material-neutral (2026-08-10, device round 3).** "Shells into the aluminium
   stream / die-cast AlSi" confused participants: the printed prototype in their hands is
   PETG, visibly not aluminium. The INSTRUCTIONS now describe only the physical actions
   (separate the shell, remove the QR sticker); the aluminium truth stays where it belongs —
   the passport data (Component ID, environmental, summary masses), which describes the
   BOSCH unit, not the mock. A bounded exception to ruling 2, cut along the same line as
   `physical_unit`: instructions address the object in hand, data describes the product.

## 2. As-shipped strings (payload 2026-08-09)

Intro parts list (7 entries, layout-safe): Housing shells 2x (HPDC aluminium) · Bare PCB,
4-layer FR-4 · Processors 2x FCBGA + flash 2x 4 GB · Power stages 6x (DPAK) · Regulators +
AFE / transceivers + MEMS · Connectors 3x AS018-35 · Fasteners 14x M3.

| Step | Action titles | Subtitles |
|---|---|---|
| 1 Open the housing | Remove the 4 lid screws / Lift off the **upper housing shell** | Allen key · M3 · keep them aside / Locating lip disengages · exposes the PCB |
| 2 Remove the connectors | Unscrew the 3 **connectors AS018-35** / Pull each connector out of its bore | 2 screws each · gold-plated contacts · recover / Set aside · **reuse after contact test** (= the usage verdict) |
| 3 Lift out the main PCB | Unscrew the 4 board screws / Lift the PCB out flat | Allen key · M3 · into the floor mounts / **Bare PCB, 4-layer FR-4** · carries the ICs & passives |
| 4 Recover the silicon | **Locate the four IC groups** / Pop the packages out of their pockets | Processors 2x FCBGA + flash · regulators + AFE · power stages 6x DPAK · transceivers + MEMS / **Reuse-eligible: processors + flash, power stages** (= `reuse_eligible` flags) |
| 5 Sort the housing | **Separate the bottom shell** / **Remove the QR code sticker** | Last housing part · set aside with the upper shell / Peel off · separate bin |

Screw arithmetic stated by the steps: 4 + 6 + 4 = 14 ✔ (matches the parts list and the
fasteners row).

## 3. Device check (next build)

Step-4's first subtitle is the longest string in the flow (~90 chars) — confirm it wraps to
two lines rather than clipping. If it clips, the fallback is dropping the "6x DPAK" qualifier,
nothing else.
