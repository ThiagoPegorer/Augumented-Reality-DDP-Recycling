# Session 37, 2026-08-20: participants used RBv1.0, and sections 3.2.2 and 3.2.3 closed

## THE FINDING, and it outranks everything else in this file

Reading XR/AR_DPP_VCU/backend/data/reports/ turned up five participant session files dated 21 July to
3 August, five untagged runs from the night of the freeze, and nothing after 9 August. Two signals said
the participant runs predate the frozen build: they use a component vocabulary the current payload no
longer has, and the first reports a different avoided-carbon figure.

THIAGO CONFIRMED IT, in his own words:

  "The participants was expose to the version RBv1.0. However, those tests was very important to I
   collect feedback of UI and UX and build the RBv2.1.1. But, the disassembly sequence didn't change
   between versions, only words to refeer to the parts of the 3D prototype, what dont generate big
   changes. The main goal of the study is to collect timestamp of disassembly of the user using the AR
   and disassembly using the 2D manual, and UI and UX feedback. The column CO2 avoided can't be apply,
   since we did not test in real units."

WHAT IT BREAKS:

1. The Methodology opener says "The prototype was frozen at a fixed version before any participant used
   it." THAT IS FALSE AND IT IS ALREADY PASTED IN THE .DOCX. Replacement:
   "Participants worked with an earlier version of the prototype, and the sessions informed the version
    described in this chapter. The disassembly sequence was the same in both."
2. Section 3.5 describes RBv1.0. Section 3.4 describes RBv2.1.1 and must name the difference.
   Section 3.6 takes the limitation with Thiago's defence attached: the sequence was identical and only
   the part names changed. State it as his reasoning, not as measured equivalence.
3. Research question 3's answer belongs to the tested version. Scope it or say so.
4. co2_avoided_kg is NOT a result. Chapter 4 excludes it explicitly, not silently.
5. P01 is not strictly comparable to P02-P05: 12 components against 11, 6.57 kg against 15.43.
6. THE 2D-MANUAL TIMINGS ARE NOT IN THE REPOSITORY. That is the study's main comparison and its data is
   still outside the project. It blocks 3.5 AND Chapter 4.

THE FIVE PARTICIPANT RUNS (elapsed seconds, then per-step):
  P01 2026-07-21  418  [149, 127, 80, 24, 38]
  P02 2026-07-31  458  [116, 154, 124, 31, 35]
  P03 2026-08-01  325  [62, 86, 76, 67, 33]
  P04 2026-08-03  376  [127, 78, 63, 66, 42]
  P05 2026-08-03  314  [133, 84, 44, 32, 23]
The five untagged 2026-08-09 runs (31 to 73 s) are Thiago's own verification passes. Exclude them.
The _P01.._P05 suffixes were added by hand; the server names files from the id and a timestamp only.

## Sections closed

3.2.2 THE PASSPORT DATA MODEL. Drafted, reviewed against Thiago's own text, corrected, then rewritten in
plain language because his supervisor does not read code. Five paragraphs: the instrument, the two rules,
Table 3 with the provenance of each category, the additive divergence (environmental content), and the
subtractive one (location), landing on "The record can state that the lead sits in the electronic
components. It cannot state which one."

3.2.3 STORING AND SERVING THE RECORD. Four paragraphs, no jargon, two figures. Vocabulary rule for this
section: no "top-level", "schema", "endpoint", "model", "validation", "port", no framework names. Say
"named blocks", "a document that defines what a record must contain", "checked", "compulsory blocks".

FIGURES 5 AND 6 authored as vector files and pushed into the Miro board "Master Thesis" (uXjVGpjOAoU),
frame "Methodology 3.2.3 - Figures 5 and 6". MIRO IS NOW THE STANDARD DIAGRAM CHANNEL. Thiago exports
PNG from there. Tell him to zoom in first; Miro exports at the current zoom.

## Rulings taken

- CIRPASS BECOMES A GROUP AUTHOR. In text (CIRPASS, 2024a, pp. 41-42); reference list CIRPASS Consortium.
  This CANCELS the five-citation conversion task and replaces it with a year-letter audit, because three
  deliverables share 2024. Thiago has corrected the letters; verify once at the compile.
- APPENDIX II REPRODUCES TABLE 6 AS A TRUE COPY. Permission verified verbatim on D2.2 page 4:
  "(c) CIRPASS Consortium, 2024. Reproduction is authorised provided the source is acknowledged."
  Report is PU (Public). Screenshot pages 41 and 42; caption it as a FIGURE (Figure II.1) so it does not
  collide with the thesis's own table numbering.
- THE THESIS QUOTES SCHEMA VERSION 0.13, NOT 0.19. See below.
- THE RECORD'S mesh_nodes BINDING IS AUTHORITATIVE for what each printed bar represents.
- DELETE backend/data/vcu_001.json, keep the copy inside the Unity project.
- NOTHING IN THE FROZEN BUILD GETS PATCHED. Every defect found is stated, not fixed.

## The two payload copies, and the serving layer

Two copies existed, identical except for the date and the renamed part. backend_open.txt launches the
server from XR\AR_DPP_VCU\backend, SO THE LIVE FILE IS THE ONE INSIDE THE UNITY PROJECT and the other is
served by nothing. The 19 August rename never reached the prototype, and the frozen artifact was never
modified. Its last_updated still reads 2026-08-09.

TWENTY-ONE BLOCKS GO IN, NINETEEN COME OUT. Proved by running Thiago's own models.py against his own
payload. The endpoint returns the validated model, not the file. Dropped in transit, complete list:
  material_reference
  schema_version (top level)
  unit_use_phase.health.reuse_assessment[].verdict_inherited_from
Nothing else. THAT REVERSES THE SCHEMA-VERSION RULING: 0.19 is a file annotation the running system
never transmits; the number the study build served is dpp_meta.schema_version = 0.13.

THREE DESCRIPTIONS OF ONE RECORD DISAGREE: file 21 blocks, backend model 19, Unity model 20. The Unity
model declares material_reference and the backend never sends it. Nothing renders it, so nothing broke.
This is exactly the failure the project's own export_schema.py header warns about. It is a 3.6
limitation, not a 3.2.3 description.

## Table 2 in section 3.2.1, CORRECTED

Two mappings existed and disagreed on two of four internal bars. Thiago checked and ruled that the
record's mesh_nodes binding is right. Corrected pairing:
  yellow  component1  -> both processors (2 x 667 MHz dual core) plus 2 x 4 GB logger memory
  brown   component2  -> power stages for the outputs (2 x 7.5 A high side, 4 x 2.2 A low side)
  blue    component3  -> sensor supplies (5 x 12 V, 5 x switchable 5/12 V) and the analog front end
  red x3  component4  -> communication transceivers and the internal sensors
Also: connector row now carries the part numbers AND the red/yellow/blue colour coding, which the printed
model loses by making all three grey. PTEG -> PETG. Missing bracket closed. "Inferred" -> "assumed".
THE OLD LINE "three red bars are the three internal sensors" IS DEAD. Cut it wherever it survives.
The 19 August rename to "Voltage regulator" was WRONG under this mapping; it lives only in the orphan
copy Thiago is deleting, so it erases itself.

## The Bosch sources, read end to end

245102859 IS A FILENAME NUMBER and appears nowhere in the 176-page manual, exactly like 245099915 for the
data sheet. Cite data sheet 234686731 (printed p. 4) and the manual by title and version (1.2,
02/02/2026). Full page map in bosch_sources_verified.md.

- "Maintenance Interval: 220 h or a maximum of two years" IS ON DATA SHEET PAGE 2. Open item closed.
- REACH Statement: manual section 24.2, PAGE 149. Disposal instruction: chapter 25, PAGE 174.
- The declaration of conformity is NOT in the MS 50.4 manual. It is in the MS 50.4P manual, page 132,
  dated 09 October 2020, and it names BOTH variants, so citing it for the MS 50.4 is valid. Only
  2014/30/EU (EMC) is ticked; RoHS is on the form and left unticked.
- THE REACH STATEMENT IS THE THESIS'S BEST EVIDENCE. Bosch gives substance name and CAS number and
  NOTHING ELSE. No location, no concentration. So the location WEEE asks a recycler to have is missing
  AT SOURCE, not only in this model. The passport goes further than its own source and still cannot
  reach the component.

GAPS between the sources and the record: economic_operator is null although the data sheet prints the
address on p. 4 (a mandatory identifier); brand is null; the 198 pins, the vibration profile, the
connector part numbers and colours, the named internal sensors and the memory size are all in the source
and not in the record; the producer's disposal instruction was dropped; three documents are marked as
having no source while the manual supplying them was read.

## The limitations register for 3.6, nine items

1 the tested version  2 the unusable carbon column  3 P01's comparability  4 the three-model drift
5 the null economic operator  6 the dropped disposal instruction  7 three documents declared absent
although they exist  8 the part list disagreeing with the binding  9 the open cross-origin policy
Plus the two carried: the 660 g ceiling treated as a point value, and all fifteen components assumed.

## Three durable lessons

1. CHECK THE THING, NOT THE NAME OF THE THING. Three errors this session came from reasoning about names
   instead of opening the artifact: the brown bar renamed from component names without reading the
   binding; a manual declared to lack its REACH statement after a search that never used the word REACH;
   a version number chosen from what the file says rather than what the server sends.
2. A RIGHT DECISION ON A WRONG REASON IS STILL A DEFECT. Second occurrence.
3. CONSISTENCY IS NOT THE TEST. Carried from 19 August and still true.

## Files updated in project memory this session

study_build_version_finding.md (updated) - bosch_sources_verified.md (NEW) -
ch3_methodology_progress.md (rewritten) - thesis-schedule.md (rewritten) -
cirpass_d22_table6.md (rewritten) - MEMORY.md (index)
dpp_payload_verified.md is SUPERSEDED on the schema version only.
