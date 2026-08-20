---
name: teardown-model-as-built
description: "[P] The as-built CAD record, print settings and NX drawing sheets, read 2026-08-19. The physical object is a GENERIC TEARDOWN MODEL INSPIRED BY the MS 50.4, explicitly NOT a 1:1 replica and explicitly not matching the datasheet envelope. This CONTRADICTS Chapter 1, which calls it a replica that reproduces the geometry. Also confirms PETG and the Bambu Lab P2S, the 200 x 150 x 60 mm envelope, and the state of the eight drawing PDFs behind Appendix I."
type: reference
---

# [P] Read 2026-08-19 (Session 36) from `AR_DPP/CAD_Specs/`

Sources: `DOCS_MD/VCU_AsBuilt_Design_Record.md` (v3.0, last modified 2026-07-19, "the authoritative record
of the physical model as modelled in NX"), `DOCS_MD/bambu_print_settings.md` (rev v3, 2026-07-18), and the
eight NX drawing PDFs in `CAD/DWG/`.

# 🔴 THE CONTRADICTION WITH CHAPTER 1

**As-built record, opening lines, verbatim:**
> "Generic VCU device *inspired by* the Bosch MS 50.4 — **not** a 1:1 replica (no access to the real
> unit), so it does not need to match the datasheet envelope."

It also states its own purpose verbatim: *"physical teardown artifact for AR user tests."*

**Chapter 1 says, verbatim** ([[ch1_verbatim_facts]]):
> "Participants worked on a **3D-printed replica** rather than an original unit... **The replica
> reproduces the geometry** and the assembly sequence, and it does not reproduce the glued joints, the
> conformal coating, or the fastener behavior of the real device."

**These cannot both be true.** The build record says the model is not a replica and deliberately does not
match the datasheet envelope. Chapter 1 says it is a replica that reproduces the geometry. The dimensions
settle it: the model is **200 × 150 × 60 mm**, the reference unit is **166 × 121 × 41 mm**. Roughly 20 %
larger in each dimension and about twice the volume.

**Consequences:**
1. **The Methodology opener's original wording, "printed teardown artifact", was CORRECT** and matches the
   build record's own phrase. ⚠ **The recommendation made on 2026-08-18 to change it to "3D-printed
   replica" was WRONG and was formally WITHDRAWN on 2026-08-19.**
2. **Chapter 1's sentence is the text that needs fixing**, not the opener. Minimum change: "replica" to
   "teardown model", and "reproduces the geometry" to something true, such as reproduces the assembly
   sequence and the disassembly steps. Chapter 1 is closed and pasted, so this is a small targeted edit.
   ⚠ **Now urgent**: 3.2.1 as written states the prototype is larger in every dimension and was not
   scaled from the datasheet envelope, so the contradiction is live inside the document.
3. `physical_unit.is_replica: true` and `replica_of` in the payload also over-claim. The build is frozen,
   so the field stays; 3.2.1 uses the accurate word.

# WHY THE MODEL IS LARGER, and it is a decision not an error

The record gives the reason in its own first sentence: there was **no access to the real unit**, so the
model was built as a generic device inspired by the datasheet rather than measured from hardware. The
enlargement follows from designing for hand-disassembly with M3 hardware and heat-set inserts rather than
from copying an envelope. ✅ **3.2.1 states this**, in Thiago's own words: *"It was sized for hand
disassembly with M3 hardware and heat-set inserts rather than scaled from the datasheet envelope, since no
real unit was available to measure."*

# VERIFIED BUILD FACTS, all `[P]`, usable in 3.2.1

- **Material: PETG.** Bambu Studio "PETG Basic", textured PEI plate.
- **Printer: Bambu Lab P2S**, 0.4 mm stock nozzle. In-house; *"the earlier Facturee outsourcing was
  cancelled."* (This closes the `[M]` flag that had been open on PETG and the printer since the facts
  register was written.) ✅ Both now appear in the closing sentence of 3.2.1.
- **Form:** deep-tray plus lid enclosure. **Overall 200 × 150 × 45 mm (bottom) + 20 mm lid
  (15 body + 5 lip)**, the 5 mm lip projecting into the cavity, giving 200 × 150 × 60 overall.
- **Disassembly: 5 steps**, matching `disassembly.total_steps` in the payload.
- **14 heat-set inserts**, all in `housing_bottom`: 4 lid bosses, 6 connector face, 4 PCB mounts.
  Ø4.0 with a 0.5 × 45° chamfer.
- **Fits validated by printed test coupons.** Coupon v1 at Ø3.8 FAILED (insert would not self-centre and
  set crooked); coupon v2 at Ø4.0 PASSED. ⚠ **Deliberately NOT written into 3.2.1** — Thiago ruled on
  2026-08-19 that Appendix I supplies the final dimensions and the coupon narrative adds process, not
  method. Keep the fact here; it may be worth one clause in 3.6 as evidence of physical validation.
- Lid locating lip 189 × 139 projecting 5 mm into a 190 × 140 cavity, reduced from 1 mm/side to
  0.5 mm/side because 1 mm let the lid sit visibly skewed on a demo people handle.
- Connector bore Ø24.5 against a Ø23.4 connector body, about 1.1 mm diametral clearance.
- Assembly file `assembly_model.prt`, base component `housing_bottom`.
- Status at v3.0: **CLEARED FOR PRINT**, 2026-07-19.

# 📐 THE NX DRAWING SHEETS BEHIND APPENDIX I — state at 2026-08-19

`AR_DPP/CAD_Specs/CAD/DWG/` holds 8 exported PDFs, all prefixed `thiag_`. Appendix I reproduces them as
PNGs with captions of the form *"<component name> drawing"*.

- 🔴 **`thiag_bottom_draw.pdf` WAS WRONG and is now FIXED.** It printed **`M3 - Ø 3,8 Custom`** — the exact
  diameter the insert coupon **failed** at — while the as-built record has read Ø4.0 since v3.0 on
  19 July. Thiago regenerated it on 2026-08-19 at 09:57. **Verified after regeneration: it now reads
  `M3 - Ø 4 Custom` with `CSINK Ø 4,6 X90°`.**
- ✅ **`thiag_dwg_upper.pdf` came back byte-identical** to the 31 July export after the 08:31
  regeneration. **That is the correct outcome, not a failed export**: the lid locating lip is not a
  dimensioned feature on that sheet, so nothing on it changed.
- ⚠ **Two connector sheets sit side by side**: `thiag_connectors.pdf` and `thiag_connectors_dwg.pdf`, the
  latter regenerated 2026-08-19 at 13:26 together with `connectors_dwg.prt` and `assembly_model.prt`.
  **Decide which is current and move the other out before the compile.**
- ⚠ **All sheets still read `SHEET REV A`, `FIRST ISSUED 09/07/26`** even where the geometry changed.
  Bump the revision block during the 24–25 August review.
- ⚠ **Verify the bottom-housing PNG pasted into the .docx Appendix I shows Ø4, not Ø3,8.** The PDF is
  confirmed correct; the image inside the document has not been checked.
- ⚠ Appendix figures currently share the body figure sequence. Renumber to Figure I.1 … before the compile.

# 🔴 THE OTHER COLLISION: CHAPTER 1 ALREADY ARGUES THE CASE

Chapter 1's Scope & Limitations already contains the full justification that 3.2.1 was going to write.
Verbatim:
> "A Vehicle Control Unit was chosen because it is essential to an electric vehicle and because the
> automotive sector is in the middle of a shift from combustion to electric drivetrains. That shift puts
> a growing number of these units into the fleet, and every one of them will eventually reach
> end-of-life. The unit therefore represents a class of device that recyclers will meet in volume before
> the decade closes. The Bosch Motorsport MS 50.4 (Bosch Motorsport, 2026) was chosen as **the reference
> unit for the Bill of Materials and for the life cycle model**. Although it is motorsport hardware, it
> is the only vehicle control unit for which a complete datasheet could be obtained. Manufacturers of
> series-production units do not publish component-level specifications, while motorsport components are
> sold to the general public and documented for the buyer. That substitution bounds the model, and it is
> also an instance of the problem this thesis addresses..."

And it already covers what 3.2.2 and 3.2.3 were going to state:
> "The passport built here is a model, not a compliance instrument. The fields follow the CIRPASS
> reference structure and the requirements set out under the European framework, but the author is not
> the producer of the device, so **most of the values are assumptions or generic data rather than
> manufacturer records**. The mandatory fields are populated to show what the structure demands, and the
> data is **served from a simple JSON file**."

**RULING: Chapter 1 has already done the ARGUING. Section 3.2 does the SPECIFYING.**
3.2.1 must NOT re-justify the case, must NOT re-explain the motorsport substitution, and must NOT restate
that the values are assumptions. ✅ Honoured: 3.2.1 as written points back in one clause — *"The Scope &
Limitations section of the introduction gives the reasons for this choice"* — and supplies what Chapter 1
does not.

⚠ Note also that Chapter 1 **already uses the phrase "the reference unit"**, so the three-role framing
agreed on 2026-08-18 is consistent with the written thesis rather than new to it.

Related: [[ch1_verbatim_facts]], [[ch3_methodology_progress]], [[vcu_datasheet_verified]],
[[modelled_unit_composition]], [[cad_prototype_scope]], [[dpp_payload_verified]], [[thesis-schedule]]
