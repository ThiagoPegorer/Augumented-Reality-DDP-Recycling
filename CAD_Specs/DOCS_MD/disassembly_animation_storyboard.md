# Disassembly animation storyboard — VCU teardown

The motion script for the 5 disassembly steps. Each backend `action` = one **task** = one **GIF**.
The same motions drive the **interactive explode** on the 3D panel. Distances are in metres
(the model imported at real scale: ~0.20 × 0.15 × 0.065 m; glTF is **Y-up**, so "up" = **+Y**).

GameObject names (children of `VCU_assembly`), by group:
- **lid** = `housing_upper` · **bottomShell** = `housing_bottom`
- **connectors** = `connector`, `connector001`, `connector002`
- **pcb** = `pcb` · **chips** = `component1..4`, `component001`, `component002` (6)
- **lidScrews** = `screws_housing*` (4) · **connectorScrews** = `screws_connector*` (6) · **pcbScrews** = `PCB_screw*` (4)

Directions marked **(verify)** depend on how the model sits in your scene — nudge the offset vector in the Inspector while watching the Scene view.

| Step | Task (= GIF) | Parts that move | Motion | Suggested offset · duration · ease |
|---|---|---|---|---|
| **1 Open housing** | 1.1 Remove 4 lid screws | lidScrews (4) | spin + back out (up) | +Y 0.03 · spin 720° · 1.0 s · InQuad |
| | 1.2 Lift off the top cover | lid | translate up, clear of the box | +Y 0.09 · 1.2 s · OutQuad |
| **2 Remove connectors** | 2.1 Unscrew 3 connectors (6 screws) | connectorScrews (6) | spin + back out along face normal **(verify)** | +X 0.03 · spin 720° · 1.0 s · InQuad |
| | 2.2 Pull connectors out | connectors (3) | translate out along bore axis **(verify)** | +X 0.07 · 1.2 s · OutQuad |
| **3 Lift out PCB** | 3.1 Unscrew 4 board screws | pcbScrews (4) | spin + back out (up) | +Y 0.03 · spin 720° · 1.0 s · InQuad |
| | 3.2 Lift the PCB out flat | pcb | translate up out of the cavity | +Y 0.06 · 1.2 s · OutQuad |
| **4 Recover silicon** | 4.1 Locate the chips | chips (6) | scale pulse (glow substitute) — no big move | ×1.15 pulse · 0.6 s · InOutSine |
| | 4.2 Pop the chips out | chips (6) | translate up off the board, **staggered** | +Y 0.03 · 0.9 s · OutBack · 0.08 s stagger |
| **5 Sort housing** | 5.1 Shells to aluminium stream | bottomShell (+ lid already up) | translate down/aside into "bin" | (0.08, −0.04, 0) · 1.2 s · InOutQuad |
| | 5.2 Peel labels & residue | — (no dedicated mesh) | no motion — highlight/caption only | n/a |

## How each output uses it
- **Per-task GIF**: frame the camera close on the moving part, call `PlayTask(step, task)`, capture with Unity Recorder → export GIF. Loop = play forward, hold ~0.3 s, reset.
- **Interactive explode**: `PlayFullTeardown()` runs all steps to fully exploded; `Reassemble()` reverses. The user can also rotate/zoom the model freely at any state.
- **Step-driven (StepFlowController)**: when the worker advances a step, call `PlayStep(n)` to move the real parts on the 3D panel — matches the instruction canvas.

## Notes
- Screws back out **before** the part they hold (task order handles this automatically per step).
- Step 5.2 has no mesh (labels/misc) — show a caption/highlight instead, or skip the GIF.
- Bolts add polygons; if the PICO frame-rate drops, hide the 14 screw objects in the interactive panel and keep them only for the GIFs.
- Tune all offset vectors + durations in the Inspector; the defaults above are starting guesses, especially the **(verify)** connector directions.
