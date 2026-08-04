# DPP UI Spec — Screen 7: Disassembly Step 4 (Recover the silicon)

> **Per-step content spec.** Layout, anatomy, interaction, data bindings and the
> CAD-dependent future implementation are defined ONCE in `04_disassembly_step1.md`
> (v2, 2026-06-11) — this file only specifies what changes for step 4.

## Step 4 content (value)

| Field | Value |
|---|---|
| Title | `Recover the silicon` |
| Progress | fill = 378 x 4/5 · label `4/5` |
| Card 1 | magnify · teal · `Locate the two processors` · `667 MHz dual-core pair · centre of board` (value: false) |
| Card 2 | chip · GOLD accent · `Remove processors & memory ICs` · `High-value silicon · reuse stream` (value: true) |
| Component ids | `actives` |
| Tool | — |
| CTA | `Confirm & next` |

## Notes

The gold moment #2. Datasheet: 2x 667 MHz dual-core (control + logger) + 2x 4 GB logger memory.

## Mockup

Approved draft: `drafts/07_v2_step4_instruction.svg` (+ shared
`drafts/04_v2_exploded_canvas_static.svg`). Static previews per 04 §5/§6;
animation/highlight per 04 §11 when the CAD model lands.

---

*Last updated: 2026-06-11 · Status: approved (v2) · Template: 04_disassembly_step1.md*
