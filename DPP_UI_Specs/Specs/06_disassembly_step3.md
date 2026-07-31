# DPP UI Spec — Screen 6: Disassembly Step 3 (Lift out the main PCB)

> **Per-step content spec.** Layout, anatomy, interaction, data bindings and the
> CAD-dependent future implementation are defined ONCE in `04_disassembly_step1.md`
> (v2, 2026-06-11) — this file only specifies what changes for step 3.

## Step 3 content (structural)

| Field | Value |
|---|---|
| Title | `Lift out the main PCB` |
| Progress | fill = 378 x 3/5 · label `3/5` |
| Card 1 | lever · teal · `Free the board from its mounts` · `Spudger · gentle leverage, no bending` (value: false) |
| Card 2 | board · teal · `Lift the PCB out flat` · `Carries processors, memory & sensors` (value: false) |
| Component ids | `pcb_substrate, pcb_copper, solder, passives, tim, coating, wiring` |
| Tool | spudger |
| CTA | `Confirm & next` |

## Notes

All disassembly_step==3 components ride along with the board. 'No bending' protects the step-4 silicon.

## Mockup

Approved draft: `drafts/06_v2_step3_instruction.svg` (+ shared
`drafts/04_v2_exploded_canvas_static.svg`). Static previews per 04 §5/§6;
animation/highlight per 04 §11 when the CAD model lands.

---

*Last updated: 2026-06-11 · Status: approved (v2) · Template: 04_disassembly_step1.md*
