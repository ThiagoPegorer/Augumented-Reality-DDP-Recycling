---
name: ch3-methodology-progress
description: THE AUTHORITATIVE live state of the Methodology chapter, updated end of Session 37 (2026-08-20). Sections 3.2.2 and 3.2.3 are CLOSED. Carries the structure, the accepted text, the citation scheme (CIRPASS is now a GROUP AUTHOR), the Appendix II ruling, the schema-version reversal (quote 0.13 not 0.19), and every open item. READ study_build_version_finding.md BEFORE writing 3.4, 3.5, 3.6 or the opener.
type: project
---

# ⭐ THE STRUCTURE LIVES IN THIS FILE.

State at the end of Session 37, 2026-08-20. Written LINEARLY from the opener.

# 🔴 READ FIRST: study_build_version_finding.md

PARTICIPANTS USED RBv1.0, not the frozen RBv2.1.1. Confirmed by Thiago 2026-08-20. The opener contains a
false sentence that is already pasted in the .docx. That file carries the full finding, the session
report data, the replacement wording and everything it forces in 3.4, 3.5, 3.6 and Chapter 4.

# ✅ STRUCTURE AND BLOCK STATE

| § | Contents | Est. | State |
|---|---|---|---|
| opener | verbatim below | 2 | ⚠ CLOSED but carries a FALSE sentence. Fix pending |
| 3.1 Research design | sub-question map, the SQ1 limit, the CIRPASS/Jensen disagreement | 3 | ✅ CLOSED |
| 3.2 intro | orienting lead | 1 | ✅ CLOSED |
| 3.2.1 The product | reference parameters, the pairing, the mass ceiling | 3-4 | ✅ CLOSED. Table 2 corrected 2026-08-20 |
| 3.2.2 The passport data model | the instrument, the two rules, the provenance table, the two divergences | 5 | ✅ CLOSED 2026-08-20 |
| 3.2.3 Storing and serving | the file, one per unit, the check, the path | 4 | ✅ CLOSED 2026-08-20, Figures 5 and 6 |
| 3.3 Life cycle assessment | own method intro, routes-versus-scenarios rule, functional unit, boundary, Sc1-Sc4, EF 3.1, ReCiPe | 7-9 | NEXT |
| 3.4 The ReBuilt prototype | own method intro, THE AR SYSTEM ONLY, must name RBv2.1.1 and say the tested version was earlier | 6-8 | unblocked |
| 3.5 User study | DESCRIBES RBv1.0, sample, conditions, procedure, measures | 5-7 | 🔴 BLOCKED on the manual-condition data |
| 3.6 Strengths, weaknesses, biases | inherent properties of each method | 4-5 | write last. NINE-ITEM REGISTER BELOW |

Hardware rule. The printed model is described once, in 3.2.1. Section 3.4 covers only the AR system.
Grouping boundary. 3.2.2 owns which attributes cluster and why. 3.4 owns tabs, chips, layout.
ROUTES-VERSUS-SCENARIOS RULE goes at the top of 3.3, not 3.1. Routes are pathways people choose;
scenarios are the modeled constructs. One bridge sentence: the four scenarios represent the routes, and
they are what the passport displays. DO NOT LOSE THIS. It is the next thing you need.

# ⚠ THE OPENER'S FALSE SENTENCE

Currently in the .docx, paragraph 2:
  "The prototype was frozen at a fixed version before any participant used it."
Replace with:
  "Participants worked with an earlier version of the prototype, and the sessions informed the version
   described in this chapter. The disassembly sequence was the same in both."

# ✅ ACCEPTED TEXT: the opener verbatim

> The literature review leaves one combination untested: a product record that is standardized, issued
> with the product, and read in the operator's field of view during disassembly. This thesis asks how an
> Augmented Reality-based Digital Product Passport can assist a recycler with two tasks: disassembling a
> Vehicle Control Unit, and assessing the environmental consequences of the routes that unit can take at
> end of life. One control unit carries the whole investigation. Participants worked on a printed
> teardown artifact of that unit, not on the unit itself.
>
> This work is organized around four stages, each depending on the one before. Specifying the passport
> data model came first, because a record has to exist before anything can display it. The life cycle
> assessment came second, since its results are fields inside that record rather than a separate
> analysis. The prototype came third, and the controlled comparison against a conventional
> two-dimensional manual came last. [FALSE SENTENCE HERE, see above.] This chapter reports procedure
> only, and every number these stages produced appears in the next chapter.

✅ "printed teardown artifact" STAYS. The 08-18 recommendation to change it was withdrawn on 08-19.

Section 3.1 Research design: unchanged, three paragraphs, closed. Full text in the .docx. Key: each stage
answers one sub-question; SQ1 is answered from the literature because no professional dismantler took
part; CIRPASS and Jensen et al. (2023) disagree on environmental content and the thesis follows the
regulatory side.

# ✅ 3.2.2 AND 3.2.3 ARE CLOSED. Final text is in the .docx.

3.2.2 The passport data model, five paragraphs:
1. What Table 6 is, why it was used, the electronics-versus-automotive qualification, pointer to Appendix II.
2. One-line hinge: the list gives the record its structure, filling it was a separate problem.
3. Table 3 and the provenance of each of the five categories, plus the three declined attributes.
4. Divergence one, additive: environmental content.
5. Divergence two, subtractive: location. Lands on "The record can state that the lead sits in the
   electronic components. It cannot state which one."

3.2.3 Storing and serving the record, four paragraphs, written IN PLAIN LANGUAGE FOR A SUPERVISOR
WITHOUT PROGRAMMING BACKGROUND. The file and its named blocks (Figure 5), one file per unit with no
database, the defined structure and the check, the path from file to participant (Figure 6).
⚠ Vocabulary rule for this section: no "top-level", "schema", "endpoint", "model", "validation", "port",
no framework names. Say "named blocks", "a document that defines what a record must contain", "checked",
"compulsory blocks".

# ✅ FIGURES AND TABLES SO FAR

Table 1 (3.2.1) reference product parameters, 14 rows - Table 2 (3.2.1) reference unit paired with the
printed model, CORRECTED 2026-08-20 - Table 3 (3.2.2) the five categories with provenance -
Figure 2 (3.2.1) Bosch photograph - Figures 3 and 4 (3.2.1) NX isometric and exploded view -
FIGURE 5 (3.2.3) extract from the passport record - FIGURE 6 (3.2.3) storage and delivery -
Appendix I the eight NX drawings - APPENDIX II CIRPASS Table 6 reproduced as a true copy.

Both new figures live in the MIRO board "Master Thesis" (uXjVGpjOAoU), frame
"Methodology 3.2.3 - Figures 5 and 6". MIRO IS NOW THIAGO'S STANDARD DIAGRAM CHANNEL; he exports PNG
from there. Vector originals were also delivered as SVG.

# 🔴 TABLE 2, CORRECTED 2026-08-20

The record's components[].mesh_nodes binding is AUTHORITATIVE for what each printed bar represents. The
physical_unit.parts list is not, and it disagrees on two of four.

| Reference Product | Prototype Part |
|---|---|
| Housing (166 x 121 x 41 mm, <= 660 g, IP67) | Housing (200 x 150 x 60 mm, PETG) |
| Connectors LIFE (red) AS018-35PN, SENS-A (yellow) AS018-35PA and SENS-B (blue) AS018-35PB | Connectors x 3 (grey, PETG) |
| Processor for customer code and processor for logger (2 x 667 MHz dual core), with 2 x 4 GB logger memory | Component 1 (yellow, PETG) |
| Power stages for the outputs (2 x 7.5 A high side, 4 x 2.2 A low side) | Component 2 (brown, PETG) |
| Sensor supplies (5 x 12 V and 5 x switchable 5/12 V, 400 mA each) and the analog input front end | Component 3 (blue, PETG) |
| Communication transceivers (Ethernet, CAN, LIN, USB, RS232) and the internal sensors: inertial, ambient pressure, ECU temperature | Component 4 (red, PETG) x 3 |
| Four-layer printed circuit board (not stated on the data sheet, assumed) | Printed circuit board |

⚠ The old "three red bars are the three internal sensors" line is DEAD. If any prose near Table 2 still
says it, cut it.
⚠ The 2026-08-19 rename of the brown bar to "Voltage regulator" was WRONG under this mapping. It went
into the orphan copy Thiago is deleting, so it erases itself.

# ✅ CITATION SCHEME CHANGED 2026-08-20: CIRPASS IS A GROUP AUTHOR

Thiago's call. In text: (CIRPASS, 2024a, pp. 41-42). In the reference list: CIRPASS Consortium.

CIRPASS Consortium. (2023). D2.1 Mapping of legal and voluntary requirements and screening of emerging
  DPP-related pilots. https://cirpassproject.eu/wp-content/uploads/2023/07/D2.1_July_2023.pdf
CIRPASS Consortium. (2024a). D2.2 Exploring possible Digital Product Passport (DPP) use cases in
  battery, electronics and textile value chains. https://doi.org/10.5281/zenodo.10974901

This CANCELS the old five-citation conversion task. Chapter 2 already used (CIRPASS, 2024a/b).
⚠ It replaces it with a YEAR-LETTER AUDIT: three CIRPASS deliverables share 2024 (D2.2, D2.3, D5.1) and
APA assigns letters by alphabetical order of title. Only D2.2's title is verified. Thiago says he has
corrected the letters; verify once during the compile.

Bosch reference entries: see bosch_sources_verified.md.

# ✅ APPENDIX II RULING, 2026-08-20

Table 6 is REPRODUCED AS A TRUE COPY, superseding the earlier stylistic "do not reproduce" ruling.
Permission verified verbatim on D2.2 page 4: "(c) CIRPASS Consortium, 2024. Reproduction is authorised
provided the source is acknowledged." The report is marked PU (Public). Screenshot pages 41 and 42; the
table spans a page break. CAPTION IT AS A FIGURE so it does not collide with the thesis's own table
numbering:
  Figure II.1 - CIRPASS Table 6, Summary of findings on current data used in the electronics sector in
  initiatives (U) or in legislation (M). Reproduced from CIRPASS (2024a, pp. 41-42). (c) CIRPASS
  Consortium, 2024. Reproduction is authorised provided the source is acknowledged.

# 🔴 SCHEMA VERSION: THE 2026-08-19 RULING IS REVERSED

QUOTE 0.13, NOT 0.19. dpp_payload_verified.md still says 0.19 and is SUPERSEDED on this point.

Proved by running Thiago's own models.py against his own payload: the endpoint returns the validated
model, not the file, so 21 blocks go in and 19 COME OUT. schema_version and material_reference are not
declared in the model and never leave the server, together with one nested field
unit_use_phase.health.reuse_assessment[].verdict_inherited_from. Nothing else is lost.

THREE DESCRIPTIONS OF ONE RECORD DISAGREE: file 21 blocks, backend model 19, Unity model 20. The Unity
model declares material_reference and the backend never sends it. Nothing renders it, so nothing broke.
This is a 3.6 LIMITATION, not a 3.2.3 description. 3.2.3 only says the response is built from the
validated model rather than from the file.

# 🔴 THE 3.6 LIMITATIONS REGISTER, nine items as of 2026-08-20

1. THE TESTED VERSION. Participants used RBv1.0; the chapter describes RBv2.1.1. Thiago's defence: the
   sequence was identical and only the part names changed. State it as his reasoning, not as measured.
2. co2_avoided_kg IS NOT REPORTABLE. No real unit was tested. Chapter 4 excludes it explicitly.
3. P01 is not strictly comparable to P02 to P05.
4. Three hand-written models of one record drifted, silently, on one field.
5. identity.economic_operator is NULL although the data sheet prints it. A mandatory identifier.
6. The producer's own disposal instruction was dropped; recycling_instructions is empty.
7. Three documents are declared absent although the manual supplying them exists and was read.
8. The demonstrator part list disagrees with the three-dimensional binding on two of four parts.
9. Cross-origin requests are open to any origin, with a note in the source to tighten before deployment.

Plus the two carried from before: the <= 660 g CEILING treated as a point value, and ALL FIFTEEN
COMPONENTS carrying basis "assumed" with none verified.

# ✅ STANDING RULINGS

- Methodology states, the Discussion defends.
- The template is guidance, not a checklist. Option B: each section explains its own method.
- Three-limitations rule. 1.3 = excluded in advance. 3.6 = what each method cannot see even applied
  perfectly. Discussion = what actually went wrong.
- CITE DATA SHEET 234686731, 27 March 2026, never 245099915. The manual's 245102859 is ALSO only a
  filename and is printed nowhere inside it.
- NOTHING IN THE FROZEN BUILD GETS PATCHED. Every defect found is stated, not fixed.
- AMERICAN spelling. NEVER cite a section or chapter number in running prose.
- CHECK THE THING, NOT THE NAME OF THE THING. Three errors on 2026-08-20 came from reasoning about names
  instead of opening the artifact. Open the artifact.

# ⚠ OPEN ITEMS

1. THE MANUAL-CONDITION TIMINGS, THE STUDY DESIGN, THE INTERFACE FEEDBACK, PARTICIPANT BACKGROUNDS.
   Open since 2026-08-17. The only thing between here and both 3.5 and Chapter 4.
2. Fix the opener's false sentence.
3. Fix Chapter 1: "The replica reproduces the geometry."
4. Write 3.3, then 3.4, 3.5, 3.6.
5. Heading typo: "The Product and it Digital Product Passport" -> its.
6. Copy-edit the closing sentence of 3.2.1.
7. Check for any surviving "three red bars are three sensors" line.
8. Delete backend/data/vcu_001.json and its .bak_before_rename.
9. File cleanup: hardlinked Operation-Manual_MS50.4P.pdf plus its (1) duplicate, the two data sheet
   duplicates. Rename the _FIXED copies back.
10. Two connector drawings side by side; decide which is current.
11. Verify the Appendix I bottom-housing image shows diameter 4.
12. Renumber Appendix figures; bump SHEET REV.
13. Audit the CIRPASS year letters across the whole document.
14. Add the three Bosch reference entries.
15. Rebuild _index/lit_index.jsonl.
16. American spelling pass.
17. Compile-day inconsistencies: BOM cited as 2026-07-25 while its header says v4.1 on 2026-07-24;
    type_number is the data sheet's ORDER number and differs from the type on the declaration; BOM says
    ~12 fasteners against 14 in the record and the CAD; 245099915 appears in the payload's size_basis
    and in the BOM header.
18. Confirm whether the guided flow displays components[].name, which decides what a participant
    actually read for the brown bar.
19. VOICE RULE 7 INVERTS HERE. Methodology requires protocols; Chapter 2 strips them.

Related: [[study_build_version_finding]], [[bosch_sources_verified]], [[table6_coverage_map]],
[[modelled_unit_composition]], [[vcu_datasheet_verified]], [[teardown_model_as_built]],
[[cirpass_d22_table6]], [[cirpass_d21_requirements]], [[dpp_payload_verified]],
[[jensen_2023_data_needs]], [[voice_and_verification_rules]], [[thesis-schedule]],
[[annex_vi_schema_gap]], [[rb2_1_dpp_page]], [[ch1_verbatim_facts]]
