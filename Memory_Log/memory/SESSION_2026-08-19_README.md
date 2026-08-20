# Session 36, 2026-08-19: section 3.2.1 written and closed

## What happened

The first full writing day of the Methodology, and the first session in which Thiago wrote the prose and
Claude reviewed it rather than the other way round.

Closed today: the section 3.2 introduction and the whole of section 3.2.1, both now in the .docx with
two tables and three figures.

NOT closed today: 3.2.2, 3.2.3, 3.3, 3.4. The plan for 19 August was to close 3.2 through 3.5. The
Methodology is about one full day behind. See thesis-schedule.md for the arithmetic.

## The 3.2 introduction, reopened and rewritten

Thiago judged the 18 August draft too generic and specified the replacement line himself. In the .docx:

  "The Digital Product Passport specified here is a record of a control unit modelled on a Bosch
   Motorsport reference product, not of the Bosch unit itself. This section sets out the parameters that
   reference product supplied, how a replica was built for testing with participants, and how the record
   is served by the project's backend."

It carries the three-role framing settled on 18 August without having to name it.

## Section 3.2.1: what is now in the document

- Table 1, 14 rows, two columns (parameter / datasheet value). Source: Bosch Motorsport Data Sheet
  234686731, 27 March 2026.
- Figure 2, the reference product photograph, same source.
- Table 2, 8 rows, pairing the reference unit with the printed model. Source: own elaboration.
- Figure 3, the NX isometric. Figure 4, the exploded view.
- A pointer to Appendix I for full dimensions, PETG on a Bambu Lab P2S.

Rulings behind those tables, all Thiago's:

- TABLE 1 SHOWS ONLY PARAMETERS THAT HAVE A COUNTERPART IN THE MODEL. Communication, service and
  ordering, and inputs and outputs came out. A table that lists them invites the question of why they
  are empty.
- TABLE 2 PAIRS, IT DOES NOT INVENTORY. A proposed fastening-and-access row was rejected: "a screw just
  changes the size or the location of fixation; we ignore it in this section." The table lists
  substitutions, not parts.
- FR-4 PAIRS AT MATERIAL LEVEL, NOT PART LEVEL. The four-layer board pairs with the printed PCB plate as
  a geometric stand-in; the material does not transfer and the table says so.
- TWO REVIEW POINTS DROPPED, both on Thiago's call, both right. The composite paragraph moves to 3.2.2
  where the null service.maintenance_interval is explained. The heat-set insert coupon test is dropped
  entirely, because Appendix I supplies the final dimensions and the coupon narrative adds process, not
  method.

## The "CPU" correction to the frozen payload

The payload's fifteen-component list contains NO CPU. It contains ic_1 "Processors 2x FCBGA + flash 2x
4 GB", already paired with the blue and yellow bars, and ic_2 "Regulators + analog front-end", which is
what the brown bar stands for. Calling the brown bar "CPU" duplicated the processors and left the
regulator unnamed. So this was a factual correction, not a naming preference.

Edit made to backend/data/vcu_001.json, physical_unit.parts[2]:
  name  "CPU" -> "Voltage regulator"
  note  null  -> "regulator and analog front-end; supplies the 12 V and switchable 5/12 V sensor rails"
  dpp_meta.last_updated  2026-08-09 -> 2026-08-19
JSON re-validated. Backup at backend/data/vcu_001.json.bak_before_rename. DELETE BEFORE THE COMPILE.

TWO FINDINGS FROM VERIFYING THE RENAME:

1. IT NEEDED NO UNITY CODE. Assets/Scripts/DDP/DPPModels.cs:245 declares physical_unit and :256 declares
   the part name, so the headset deserialises the label straight out of the JSON. One payload edit
   changes what participants read. After the edit, no string "Voltage regulator" exists anywhere in
   Assets, backend, schema or docs, and the only surviving whole word CPU in the Unity sources is
   Assets/Editor/DPPUIBuilder.UsePhase.cs:175, a use-phase duty-cycle row "CPU above 80 % load / 675 h",
   plus two "raw CPU buffer" comments in QRCameraProbe.cs. All three correctly mean the processor and
   must stay. Worth one sentence in 3.2.3: the record is the single source of the displayed content.

2. A REASON GIVEN DURING THE SESSION WAS FALSE. [X] The id and photo_id keys were left as "cpu" on the
   stated ground that photo_id "resolves to an image asset". It does not. photo_id is declared in
   DPPModels.cs:260 and read by NO code anywhere, and no asset named cpu exists under Assets or backend.
   It is a dormant field, not a live reference. The decision stands, on the better ground that renaming
   keys in a frozen study artifact for cosmetic reasons should not happen after freeze. The reason does
   not stand, and the correction is recorded because a right decision on a wrong reason is still a
   defect.

## The NX drawings behind Appendix I

- thiag_bottom_draw.pdf WAS WRONG. It printed "M3 - D 3,8 Custom", the exact diameter the insert coupon
  FAILED at, while the as-built record has read 4,0 since v3.0 on 19 July. Thiago regenerated it and it
  now reads "M3 - D 4 Custom" with "CSINK D 4,6 X90 deg". Verified after regeneration.
- thiag_dwg_upper.pdf came back byte-identical to the 31 July export. That is CORRECT, not a failed
  re-export: the lid locating lip is not a dimensioned feature on that sheet.
- Two connector sheets now sit side by side, thiag_connectors.pdf and thiag_connectors_dwg.pdf, the
  latter regenerated 19 Aug 13:26. Decide which is current before the compile.
- All sheets still read SHEET REV A, FIRST ISSUED 09/07/26 even where the geometry changed.
- The Appendix I bottom-housing PNG has NOT been checked; the PDF is confirmed, the pasted image is not.

Verifying an appendix caught a two-month-old error two days before the appendix was needed.

## The 18 August "replica" recommendation is REVERSED

Claude had recommended changing "printed teardown artifact" to "3D-printed replica" in the Methodology
opener, to match Chapter 1. That was wrong. VCU_AsBuilt_Design_Record.md v3.0 opens:

  "Generic VCU device inspired by the Bosch MS 50.4 - NOT a 1:1 replica (no access to the real unit),
   so it does not need to match the datasheet envelope."

The opener is correct. CHAPTER 1 IS THE TEXT THAT NEEDS THE FIX: it says "3D-printed replica ... The
replica reproduces the geometry". It does not. The model is 200 x 150 x 60 mm against the reference
unit's 166 x 121 x 41 mm, roughly 20 % larger in each dimension and about twice the volume. 3.2.1 as
written now states the prototype is larger in every dimension, so the contradiction is live inside the
document.

DURABLE RULE: CONSISTENCY IS NOT THE TEST. Twice now a correct Methodology sentence was nearly edited to
match a wrong sentence elsewhere. The as-built record and the payload are the authorities.

## Section 3.2.2 skeleton, delivered, no prose written

1. The instrument. Table 6 is a summary of what legislation requires (M) and what initiatives use (U)
   for the electronics sector, originating in D2.1. Not a CIRPASS schema. Wautelet & Ayed (2024,
   pp. 41-42) and Wagner et al. (2023).
2. The selection rule, the footnote-45 judgement, and the honest-labelling rule: every value carries its
   basis, verified / assumed / not provided. Never a blank, never a plausible guess.
3. The coverage table and the scoreboard. Of 13 mandatory attributes, 2 do not apply with the reason
   recorded in the record itself; of the 11 that apply, 5 covered, 5 partial, 1 declared absent.
4. Divergence one, additive: environmental content. CIRPASS lists PEF and LCA; the recyclers Jensen
   et al. surveyed named none of it. This build carries it.
5. Divergence two, subtractive: location. D2.1 Table 6 p. 38 names the waste treatment operator as the
   data user for the location of dangerous substances, format "Manual, electronic media". Lands on the
   pair: the record can say the lead is in the electronic components; it cannot say which one. Both SVHC
   entries carry component_id null and only a free-text location_note.

ONE DECISION BLOCKS THE DRAFT: does the 22-row coverage table go in the body of 3.2.2 with a trimmed
evidence column (Claude's recommendation), or in Appendix II?

## Also owed, new this session

- Chapter 1: "The replica reproduces the geometry." One sentence, highest value of the backlog.
- Heading typo: "The Product and it Digital Product Passport" -> its.
- Copy-edit the closing sentence of 3.2.1: "This object is the one use for the participants disassembled."
- Delete backend/data/vcu_001.json.bak_before_rename.
- Renumber Appendix figures to their own sequence (Figure I.1 ...).
- Decide whether "frozen at a fixed version before any participant used it" needs a qualifier now that
  last_updated reads 2026-08-19. The payload's CONTENT did not change for the study; a label did.

## Still blocking, open since 17 August

THE STUDY FACTS. Participant count, group assignment, within or between subjects, measures captured,
where the raw files live, the build version flashed, protocol deviations. Section 3.5 and the whole
Findings chapter wait on this. Third consecutive session flagged.

## Files updated in project memory this session

ch3_methodology_progress.md (rewritten) - teardown_model_as_built.md (extended with the drawing state) -
dpp_payload_verified.md (the payload edit, the Unity binding, the photo_id correction) -
thesis-schedule.md (the schedule reality check and the grown backlog) - MEMORY.md (index)
