# ReBuilt Prototype — Version Registry

> One entry per released prototype version. A version = a state of the AR-DPP prototype that was
> **fully device-validated end-to-end** and pushed to GitHub. Living document — every new version
> gets an entry; the per-feature living specs (`00`–`12`) hold the detail, this file holds the map.
> Repo: `Augumented-Reality-DDP-Recycling` · Hardware: PICO 4 Ultra + 3D-printed VCU (Bambu Lab P2S, PETG).

---

## RBv2.0 — 🚧 IN PROGRESS (started 2026-07-29)

**Not a release yet.** RBv1.0 remains the build to use for the studies until the full end-to-end pass
below is green. Scope: Elle Langer's AR-supervisor feedback → Miro `USER JOURNEY DIAGRAM` v4.
Headline changes: **tab bar removed** (single linear path + one-step-back hierarchy), a **Welcome
canvas** as the app's entry and universal return target, a **gesture tutorial**, and the **digital
model exploration** moved out of the disassembly step flow into its own screen ahead of the timer.

| Block | Routine | Spec | State |
|---|---|---|---|
| 1 | **Open App** — Welcome canvas + `FIRST TIME USING THE APP?` prompt; `waitForWelcome` entry; user-facing demo fallback removed | `12_welcome_first_run.md` | ✅ **built + device-tested 2026-07-29** |
| 2 | **DPP** — split the Information tab into `DPP Canva` (product info) + `Digital Model Exploration` (LCA overview panel + exploded zone); `CONTINUE TO DISASSEMBLY?` gate | `02` split → `02` + new | ⬜ pending |
| 3 | **Disassembly** — remove zone activation from the step flow; cancel modal retargeted Main Page → intro canvas; intro Back → DPP Canva | `03` / `04` / `10` | ⬜ pending |
| 4 | **Tutorial** — pinch a button → pinch-drag a canvas onto a marked AR location → `FINISH TUTORIAL?` | new | ⬜ pending |
| 5 | **Full device-validation pass** → then this entry becomes the RBv2.0 release | — | ⬜ pending |

⚠ **Blocks 2 and 3 are ONE device-test cycle**, not two: the exploded zone's builders live in
`Editor/DPPUIBuilder.StepFlow.cs` and its lifecycle is bound to step-flow activation in
`ScreenRouter.Show()`. Rebuild rule unchanged: **Build Phase 4 → always re-run Phase 5.**

**Locked decisions:** timer still starts at `Start disassembly` (tutorial + DPP time sit outside it,
`step_times_s` semantics unchanged) · LCA in v2.0 = **device-level overview only**, per-component
analysis deferred to RBv2.1 · entry is **QR-only** for participants.

---

## RBv1.0 — "Rebuilt version 1.0" · 2026-07-21

**The first complete kiosk loop.** A participant can, without any operator intervention:
scan the physical unit's QR → read its Digital Product Passport → run the guided 5-step
disassembly with a fully interactive 3D twin → send the timed dismantling report → hand over,
scan again. Everything below is device-validated on the PICO 4 Ultra.

### Feature set (with the spec that defines it)

| Feature | Spec | State |
|---|---|---|
| Design standards: tokens, typography, hover rule, hand-ray hit-area rule (≥50 px), on-plane/own-canvas rule, modal-state rule | `00_design_standards.md` (rewritten 2026-07-20) | ✅ |
| Main page (2 tabs, no serial) | `01_main_page.md` (v2) | ✅ |
| Information tab (card grid + LCA modal) | `02_information_tab.md` (v3) | ✅ |
| Disassembly intro (unboxed rows, 2-col Dismantling, live 3D teardown loop) | `03_disassembly_intro.md` (v3) | ✅ |
| Guided step flow (task gating red→green, cancel modal, frameless state-aware how-to loops with 10 % step-focus ghosting, per-step timing capture) | `04_disassembly_step1.md` (v3) + `05–08` | ✅ |
| Completion summary (per-step table: time splits · recovered mass · material grams, longest-step gold tag, `Send dismantling report` → post-report loop modal) | `09_completion_summary.md` (v3) | ✅ |
| **Exploded action zone** — transparent canvas, grab-circle reposition, two-hand band gestures (twist→yaw 5–25 cm · zoom dial >25 cm), gesture HUD column + guide modal, constrained part drag (direct + list methods), 50 % dependency physics both directions, regroup cascade, real-life part colors | `10_action_zone.md` (v4.6.2) | ✅ |
| **QR entry + kiosk loop** — PXR_CameraImage + ZXing (RGBA8888 @30 fps, ~61 ms decodes), head-follow minimal viewfinder, 10 s demo fallback, backend-error state, post-report scan-again cycle | `11_qr_entry.md` (complete) | ✅ |

### Technical baseline

- **Unity 6000.0.73f1** · PICO Unity Integration SDK **3.4.0** (native loader, hand tracking via
  PXR_Hand/RayPose) · TextMeshPro · DOTween · Newtonsoft.Json · glTFast (NX→STEP→FreeCAD→glTF) ·
  ZXing.Net.
- **Backend:** FastAPI ≥v0.5 — `GET /dpp/{id}`, `POST /dpp/{id}/report`; `vcu_001.json` with
  `material_breakdown` (basis: assumed), `step_times_s` in the recovery report (the study dataset);
  10 s client timeout; QR payload `dpp:<product_id>` via `qr_generator.py`.
- **Scene build system:** editor builders `DPP → Build Phase 1–6` (Phase 4 → always re-run
  Phase 5; Phase 6 = QR screen; `DPP → Apply Real-Life Colors` for the model tint).
  *(RBv2.0 adds Phase 7 = Welcome + First Run.)*
- **Android:** custom main manifest with `android.permission.CAMERA` (headset grants it manually
  in Settings → Apps — no runtime dialog on PICO OS).
- **Hardware companion:** printed VCU per CAD print release v3.0 (Ø4.0 heat-set inserts),
  brown bottom `#8a5a3b` / yellow lid `#f2c11e` / green connectors+PCB `#2e7d4f` —
  AR model matches via the real-life color tokens.

### Known constraints (accepted for v1.0)

- Backend reachability: university Wi-Fi has AP isolation — **personal hotspot on study days**;
  `DPPClient.BaseUrl` must match the active network's PC IP.
- Material-split percentages are assumed pending openLCA validation (footnoted in-app).
- One product (`vcu_001`); multi-product needs only more backend JSONs + QR labels.
- Ghost/fade shader must stay in Always Included Shaders (isolation + step-focus ghosting).

### Git

- Zone through v4.5: `c823299` · mechanism #4 (v4.6.2) and QR entry: pushes of 2026-07-20/21.

---

## Template for the next entry

```
## RBvX.Y — "name" · date
What changed since the previous version (one paragraph).
| Feature | Spec | State |  ← only NEW/CHANGED rows
Technical changes · Known constraints delta · Git ref
```

*Registry created 2026-07-21 alongside RBv1.0.*
