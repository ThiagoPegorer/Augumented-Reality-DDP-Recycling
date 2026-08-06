# RB2.1 — spec index

> **Living index.** RB2.1 is the version being built now: the **stakeholder split**, the
> **disassembly back/save changes**, **P03 (Domenik)'s usability findings**, and — **last** — a
> **per-page tutorial pop-up**.
> Routine: `RB2_1_routine.md` · Standards: `00_design_standards_rbv2.md` · Diagram: `Routine_RB2_1.pdf`

---

## 1. RB2.1 is a self-contained set (changed 2026-08-04)

**Superseded rule:** RB2.1 originally inherited by reference — a spec existed here only if it
changed, and everything else stayed authoritative in `../RB2_0/`.

**Current rule:** RB2.1 **renumbers from `00` and stands alone.** Thiago, 2026-08-04: *"lets
rename those .md files, to start from 01 again, since the others specs are legacy and ALMOST
CERTAIN that I might delete this after complete it."*

Consequences, stated plainly because they cost work:

- **Every screen needs a full spec written here**, not just the changed ones. Nothing in RB2.1
  may say "see RB2_0 for the rest" — the moment `../RB2_0/` is deleted, such a spec is a hole.
- The RB2.0 files remain readable until deletion and are the **source material** for each new
  spec, but they are not a dependency.
- ⚠ **Do not delete `../RB2_0/` until every row in §2 reads `written`.** It is also the only
  record of what P02 and P03 actually saw — if it goes, keep it in git history at minimum.

## 2. Spec set

| # | File | Screen | Routine | State |
|---|---|---|---|---|
| `00` | `00_design_standards_rbv2.md` | UI + UX standards | — | ✅ **written** 2026-08-04 |
| `01` | `01_welcome_page.md` | Welcome / Open App | Open App | ✅ **approved, ready to build** |
| `02` | `02_scan_qr_code.md` | Scan QR + scan-failure loop | Open App | ✅ **approved, ready to build** |
| `03` | `03_stakeholder_decision.md` | Stakeholder decision (**new screen**) | Open App | ✅ **draft** — blocked on card icons |
| `04` | `04_DPP_page.md` | DPP page — **980 × 430 super panel**, rail + model stage + data, two roles | DPP | ✅ **v2 specced** 2026-08-05 — not yet coded |
| `04a` | `04a_use_phase.md` | Usage & service tab (`+` target) — VCU self-telemetry, SOH | DPP | ✅ **written** 2026-08-05 |
| `04b` | — | Training disassembly tab (`+` target) | DPP | ⬜ |
| `04c` | `04c_product_specs.md` | Product specs tab (`+` target) — Identity · Mechanical & electrical · component detail | DPP | ✅ **written** 2026-08-06 — mock approved, not yet coded |
| `04d` | — | Environmental impact tab (`+` target) | DPP | ⬜ |
| `05` | — | Digital model — floating object, lenses + per-part detail | DPP | ⬜ (no longer a separate *screen* — see `04` §2) |
| `06` | — | Disassembly intro | Disassembly | ⬜ |
| `07` | — | Disassembly steps 1–5 | Disassembly | ⬜ |
| `08` | — | Summary + report | Disassembly | ⬜ |
| `09` | — | Tutorial pop-up (shared component + 8 contents) | all | ⬜ **last, by instruction** |

**Build order is journey order** (`RB2_1_routine.md` §10): Open App → DPP → Disassembly, so each
device session tests one complete linear path. Tutorials come after the whole routine works.

**Process per screen** (`RB2_1_routine.md` §11): write the spec → flip its row here → log the
change in §3 → build → device-check.

## 3. Change log — what RB2.1 changes and why

| # | Change | Origin | Spec | State |
|---|---|---|---|---|
| 1 | Stakeholder fork: DPP USER (passport only) vs RECYCLER (passport → disassembly) | Routine rev 1 | `03`, `04` | specified in routine |
| 2 | Back = previous step (steps 2–5); cancel modal only on step 1 | Thiago 2026-08-04 | `07` | ruled, not specified |
| 3 | Step task state **saved** across back-navigation | Thiago 2026-08-04 | `07` | ruled, not specified |
| 4 | Timing model → **accumulate all visits**, `Σ splits == total` | Thiago 2026-08-04 | `07`, `08` | ruled, not specified |
| 5 | Tutorial → **per-page pop-up** (8), replacing the single gated onboarding flow | Thiago 2026-08-04 | `09` | deferred to last |
| 6 | First-run prompt **removed** (nothing left for it to gate) | consequence of 5 | `01` | ✅ specified |
| 7 | Numbering restart; RB2.1 self-contained | Thiago 2026-08-04 | all | ✅ done |
| 8 | `Close app` → **solid red**; red gains a third meaning (exit actions) | Thiago 2026-08-04 | `00` §2.1, `01` §2.2 | ✅ specified |
| 9 | Scan-error panel → `Close app` (red) / **`Scan again`** (green, primary, right); `Retry` deleted; subtitle corrected | Thiago 2026-08-04 | `02` §5 | ✅ specified |
| 10 | Primary CTA is **always right** — rule written because the RB2.0 error panel broke it | consequence of 9 | `00` §5 | ✅ specified |
| 11 | Stakeholder cards: role title + action description; `DPP User` → **`Product user`** | Thiago 2026-08-04 | `03` §4.1 | ✅ specified |
| 12 | `Back` from DPP Canva → stakeholder screen, closing the mis-tap dead-end (**extended to both branches**) | Thiago 2026-08-04 | `03` §4.2, `04` | ✅ specified |
| 13 | DPP Canva built **once, in two modes**, not as two canvases | implementation | `03` §5, `04` | ✅ specified |
| 14 | Stakeholder mode recorded in the dismantling report | study variable | `03` §5, `08` | ⬜ to specify in `08` |
| 15 | **DPP Canva absorbs Composition & impact** — one panel screen, not two | Thiago 2026-08-04 (*"split it is being confuse"*) | `04` | ✅ specified |
| 16 | **2 × 2 tab grid** — Product Specifications · Usage History · Impact & Environmental · **Training Disassembly** (new) | Thiago 2026-08-04 | `04` §3 | ✅ specified |
| 17 | **Model is persistent and free-floating** across the passport phase; spawns alone, then the panel unfolds from it | P02/P03: *"didn't perceive there was a 3D model"* | `04` §2 | ✅ specified |
| 18 | **Panel → model highlight**, both directions; Impact tab tints the model by material and glows by value | P02/P03: *"what do I do with this information"* | `04` §4 | ✅ specified |
| 19 | Drag circle → **drag bar** (circle too small a target) | participant feedback | `04` §5 | ⬜ which handle, open item #5 |
| 20 | Compliance: device-level badge in the header; SVHC per-part marking **not supported by the payload** (`component_id: null`, all `hazardous: false`) | data check 2026-08-04 | `04` §6 | ⬜ Thiago's call, open item #1 |
| 21 | Chip standardised as the only content element on `04`: `card/blue`, h 18, **width = preferredWidth + 24 at runtime** | Thiago 2026-08-04 | `00` §5, `04` §3 | ✅ built |
| 22 | Role-driven navigation on `04`: Product user keeps the header arrow + `Home`; Recycler drops the arrow and takes `Back` in the bottom bar | Thiago 2026-08-04 | `04` §4 | ✅ built |
| 23 | **Super panel**: one 980 × 430 rig, 220 rail + 340 transparent model stage + 420 data, side panels toed in; model LOCK/UNLOCK | Thiago 2026-08-05 (*"the model was hide in the AR space"*) | `04` §2–3 | ✅ specified |
| 24 | **Recycler walkthrough**: lands on Product specs, later tabs dimmed until visited, CTA `Next`; `Continue to disassembly` only on the Training tab | Thiago 2026-08-05 | `04` §6, `04c` §5 | ✅ specified |
| 25 | `Usage History` → **`Usage & service`**; car-centric telemetry replaced by VCU self-telemetry | Thiago 2026-08-05 | `04a` | ✅ specified |
| 26 | **The passport describes the Bosch MS 50.4**; the NX model is the AR mock. `size_mm` 200 × 150 × 60 → **166 × 121 × 41** | Thiago 2026-08-06 | `04c` §1.1 | ✅ specified, payload 0.14 |
| 27 | Components **11 → 15 rows**, split `part` (8, with NX drawing) / `board_material` (7, none); every mass and material bound to `VCU_BOM_v4.xlsx` `By_Component` | Thiago 2026-08-06 (*"LCA and BOM_v4 is the source of truth"*) | `04c` §3 | ✅ specified, payload 0.14 |
| 28 | Housing → two shells **108.4757 / 235.5243 g**, split by the BOM's own 637 cm² area basis | derivation 2026-08-06 | `04c` §3.3a | ✅ specified |
| 29 | Four CAD IC blocks carry BOM rows 5–11 (23.70 g): P1 FCBGA + flash · P2 regulators + AFE · P3 power stages · P4 transceivers + IMU | Thiago 2026-08-06 | `04c` §3.3b | ✅ approved |
| 30 | NX sheets stripped to **geometry + shaded view**; title block, frame and all sheet labels removed; 16 sprite assets generated | Thiago 2026-08-06 (*"forget the labels of the DWG"*) | `04c` §7 | ✅ assets built |
| 31 | ⚠ **BOM defect found: 0.7 g tin double-count** (`BOM_v4.md` 3.9 g vs xlsx 4.6 g) + closure states 660 but sums 660.1565 | data check 2026-08-06 | `04c` §2 | ⬜ **Thiago's call, blocks the LCA write-up** |
| — | **P03 (Domenik) usability items** | P03 session | per screen | ⬜ none on `01`/`02` — both approved as-is |

⚠ **Row "P03" is the gap.** Thiago's instruction is that Domenik's feedback gets *folded into
each canvas as it is rebuilt* rather than tracked as separate work. That is fine for building —
but a fix that is made and never recorded **cannot be cited in the thesis as evidence that user
testing changed the design**, which is the entire reason the session was run. One row per item,
here, naming the spec that answers it.

## 4. Inherited unchanged from RB2.0 (verify before assuming)

These were device-tested in RB2.0 and are expected to carry over without redesign. Each still
needs its own RB2.1 spec file (§1), but the *content* is a transcription, not a decision:

- Panel geometry, colour tokens, typography, hover / hit-area / on-plane rules → now `00`
- Two-hand rotate + zoom gestures, action-zone behaviour → `00` §6, detail in `05`
- The audio system (click, per-hand grab, directional-EQ wind drag) → `00` §7
- DPP Canva tile structure and the four detail pages → `04`
- Completion summary layout → `08`

## 5. Standing constraints

- **No mid-study changes to disassembly difficulty or removal order** — confounds the comparison.
- **Freeze rule:** last headset build ≥ 1 hour before the first participant.
- Bosch datasheet / manual PDFs never enter the repo · participant data never reaches GitHub.
- Git is manual: Claude edits files, Thiago commits and pushes.

*Created 2026-08-01 · Rewritten 2026-08-04 for the numbering restart · `04a`–`04d` tab specs added 2026-08-06 · Legacy: `../RB2_0/` · Registry: `../VERSIONS.md`*
