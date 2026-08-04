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
| `04` | — | DPP Canva + detail pages | DPP | ⬜ **next** |
| `05` | — | Digital model exploration | DPP | ⬜ |
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

*Created 2026-08-01 · Rewritten 2026-08-04 for the numbering restart · Legacy: `../RB2_0/` · Registry: `../VERSIONS.md`*
