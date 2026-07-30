# Exporting the VCU assembly from NX into Unity

Goal: get the assembled model into Unity with **every part separate** and in its **assembled
position**, so you can animate the teardown (explosion) at runtime with DOTween — matched to the
5 disassembly steps.

**You do NOT need an NX exploded view for this.** The explosion is done in Unity by moving parts.
The NX exploded view is only for static thesis figures — optional, later.

Unity can't read NX or STEP directly, so the path is: **NX → STEP → glTF → Unity.**

---

## Stage 1 — NX: export the assembly to STEP

1. Open `assembly_model.prt`.
2. **File → Export → STEP** (choose **AP242**, or AP214 if 242 isn't offered — AP242 carries colours best).
3. In the dialog: Data to export = **Entire Assembly**; include colours/attributes if there's an option.
4. Confirm units = **Millimeters**.
5. Save as `VCU_assembly.stp` in `AR_DPP/CAD_Specs/`.

This STEP file **also serves as your CAD archive** — so this covers the "STEP export" task too.

## Stage 2 — Convert STEP → glTF (free)

glTF (`.glb`) is chosen because it keeps the **part hierarchy + transforms + colours**, which FBX-from-STEP can't do for free.

**Option A — CAD Assistant (easiest, recommended)**
- Free tool by OpenCascade (download: cadassistant.com). Lightweight, GUI, no learning curve.
- Open `VCU_assembly.stp` → **Save / Export → glTF (.glb)**.
- Set **mesh deflection (quality) to moderate** — too fine = a huge mesh that lags on the PICO. Start medium and check the file size.

**Option B — FreeCAD (if you prefer / already have it)**
- File → Open the `.stp` → in the model tree **select all parts** → File → **Export → glTF (*.glb)**.

Either way: confirm the export **keeps parts separate** (don't "sew"/"merge" into one solid).

## Stage 3 — Unity import

1. Install the **glTFast** package: Window → Package Manager → **+ → Add package by name** → `com.unity.cloud.gltfast` (free, Unity's recommended glTF importer).
2. Drag `VCU_assembly.glb` into `Assets/`. It imports as a prefab with **each part as a child GameObject**.
3. **Scale check:** NX works in mm, Unity in metres. A 200 mm model should come in at ~0.2 units. If it's 1000× off, set the import scale (glTFast import settings) or scale the root object.
4. **Materials:** part colours become materials; swap them for your AR/DPP materials/shaders when you style it.

## After import — the explosion (in Unity)

- Each part is a child with its assembled local position.
- Animate the teardown by translating parts along axes, sequenced to the 5 steps:
  1. lid → up (+Z)  2. 3 connectors → out (−X)  3. PCB → up  4. 6 chips → up out of pockets  5. shells apart.
- Reuse **DOTween** (`transform.DOLocalMove(...)`) with a sequence per step. Drive it from your existing StepFlowController.

## Watch-outs

- **Poly count:** CAD tessellation of the round connectors + fillets can be heavy. Keep mesh deflection moderate for PICO performance; decimate later if the frame rate drops.
- **Separate parts:** verify in Unity you see multiple child objects, not one merged mesh. If merged, re-export without sewing.
- **Pivots:** glTF part pivots may sit at the world origin, not the part centre. Fine for straight-line explosion; if you need to rotate a part, re-centre its pivot (parent it to an empty at its centre).
- **Bolts:** 14 bolt instances add polygons. If performance suffers, hide bolts in the AR or use a low-poly stand-in — they're cosmetic.

## Two roles of the 3D model in the AR (confirmed 2026-07-09)

The imported model serves BOTH panels — author the part animations once, use them twice:

1. **Interactive 3D panel** — user rotates / zooms / triggers exploded view on the full model. This is the glTF model directly (orbit camera + pinch-zoom + an explode toggle in Unity).
2. **Per-task GIFs in the instruction panel** — each disassembly *task* shows a short looping animation of that action (e.g. "unscrew the bolt", "lift the lid"). **Render these from the same Unity model**: animate the part motion (DOTween / Animation clip), frame the camera on the relevant part, capture with the free **Unity Recorder** package → export GIF or MP4. Same model = consistent look across both panels.

**GIF list (~10, from the backend tasks):** remove-4-screws, lift-lid, unscrew-connectors, pull-connectors, unscrew-board-screws, lift-PCB, locate-chips, pop-chips, sort-shells, peel-labels. Each backend `action` = one GIF.

The part motions authored for the GIFs are the SAME translations used by the interactive explode — so build the animation set once and reuse.
