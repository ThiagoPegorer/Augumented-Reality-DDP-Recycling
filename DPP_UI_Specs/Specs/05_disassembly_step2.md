# DPP UI Spec — Screen 5: Disassembly Step 2 (Remove the connectors)

> **Per-step content spec.** Layout, anatomy, interaction, data bindings and the
> CAD-dependent future implementation are defined ONCE in `04_disassembly_step1.md`
> (v2, 2026-06-11) — this file only specifies what changes for step 2.

## Step 2 content (access + value (gold))

| Field | Value |
|---|---|
| Title | `Remove the connectors` |
| Progress | fill = 378 x 2/5 · label `2/5` |
| Card 1 | pins · GOLD accent · `Unscrew the 3 round connectors` · `198 gold-plated pins · recover` (value: true) |
| Card 2 | usb · teal · `Disconnect the USB lead` · `Set aside · reusable part` (value: false) |
| Component ids | `connectors` |
| Tool | — |
| CTA | `Confirm & next` |

## Notes

The gold moment #1: the connector card uses the gold accent (stroke #b7842f, subtitle #f0c879) mirroring components[].high_value. LIFE (red), SENS-A (yellow), SENS-B (blue) per datasheet; USB is the 4th, reusable.

## Mockup

Approved draft: `drafts/05_v2_step2_instruction.svg` (+ shared
`drafts/04_v2_exploded_canvas_static.svg`). Static previews per 04 §5/§6;
animation/highlight per 04 §11 when the CAD model lands.

---

*Last updated: 2026-06-11 · Status: approved (v2) · Template: 04_disassembly_step1.md*
