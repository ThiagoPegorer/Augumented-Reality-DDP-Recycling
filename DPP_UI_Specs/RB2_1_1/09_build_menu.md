# 09 — The Unity build menu (RBv2_1_1, consolidated 2026-08-09)

**Status:** v1 · one tab, one order. The RBv2_0 / RBv2_1 / RBv2_1_1 menu split is GONE — every
build phase lives under **RBv2_1_1**, numbered so that **running top-to-bottom is always safe**.
Every in-code reference (Verify hints, runtime warnings, comments) was renamed with it. Older
specs still cite the historical phase names; translate with the table below.

## 1. The menu

| # | Item | Builds | Was |
|---|---|---|---|
| 01 | Panel canvas + router | DPPPanelCanvas + ScreenRouter | RBv2_1/1 |
| 02 | Welcome + first run | Welcome canvas | RBv2_1/3 |
| 03 | QR scan screen | Scan screen (self-heals Welcome's scanner ref) | RBv2_1/2 |
| 04 | Stakeholder decision | Role fork | RBv2_1/7 |
| 05 | Disassembly intro | Teardown intro | RBv2_0/4 |
| 06 | Step flow + action zone | Guided steps | RBv2_0/5 |
| 07 | Completion summary | Summary | RBv2_0/6 |
| 08 | DPP page | Flat v1 page + legacy certificates screen (rollback fallback) | RBv2_1/8 |
| 09 | Product specs tab | 04c page (on the panel canvas) | RBv2_1/9 |
| 10 | Super panel rig | Rail + stage + data canvas + model link + rail CTA | RBv2_1_1/1 |
| 11 | Product specs into the data canvas | Re-parent + close the model link | RBv2_1_1/2 |
| 12 | Usage & service into the data canvas | 04a | RBv2_1_1/3 |
| 13 | Environmental impact into the data canvas | 04d | RBv2_1_1/4 |
| Tools | Verify wiring · Apply real-life colors · Generate UI sprites · Clean RBv1.0 leftovers | | RBv2_1/Tools + RBv2_0/Tools |
| Legacy | DPP Canva + Model Exploration (superseded by 08) | | RBv2_0/Legacy |

## 2. Why 05–07 sit BEFORE the DPP block (deviation from strict user-journey order)

The user journey is welcome → scan → role → passport → teardown. The menu instead puts the
teardown builders (05–07) before the rig (10), because **05 and 06 must never run while the
stage clone exists** — their animator resolution grabs the rig's clone and breaks both the
stage and the intro loop. Numbering them after the rig would make "run them in order" a trap;
numbering them before makes the naive top-to-bottom run the CORRECT full rebuild. A menu whose
order can be followed blindly beats a menu that matches the journey but requires a footnote.

## 3. The two standard sequences

- **Full scene rebuild:** 01 → 02 → … → 13 → Tools/Verify wiring → SAVE. (Then
  Tools/Apply real-life colors if the model colours drift.)
- **After any change to the data-canvas builders (the routine chain):**
  **09 → 10 → 11 → 12 → 13 → Tools/Verify wiring → SAVE** — strictly ascending; 09 must
  precede 11 (11 re-parents the page 09 builds), and nothing before 09 needs re-running.
- Never run 05 or 06 while the rig exists (§2). If they must be re-run, delete the rig first
  and rebuild 10–13 afterwards.

## 4. Renamed everywhere

The rename covered menu paths, `[MenuItem]` priorities, the Verify wiring map's fix-phase
hints, and every `Debug.Log/Warning` that tells a human what to re-run — 29 files. Historical
phase names survive only in older specs and session logs; this table is the translation.
