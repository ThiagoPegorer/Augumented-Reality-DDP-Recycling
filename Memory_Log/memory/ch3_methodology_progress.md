---
name: ch3-methodology-progress
description: THE AUTHORITATIVE live state of the Methodology chapter. Six-section structure with the 3.2 sub-structure agreed 2026-08-17, the accepted opener and 3.1 verbatim, the CIRPASS hybrid citation ruling, the "four tabs versus four attributes" trap, and the coverage re-derivation that must run before 3.2 is written.
type: project
---

# THE STRUCTURE LIVES IN THIS FILE. Do not read a structure table from any other memory.

State at the end of Session 34, 2026-08-17. Written linearly from the opener.

# STRUCTURE

| section | contents | est. | state |
|---|---|---|---|
| opener | verbatim below | 2 | CLOSED |
| 3.1 Research design | sub-question map, the SQ1 limit, the CIRPASS/Jensen disagreement | 3 | CLOSED |
| 3.2 The product and its passport | see sub-structure below | 10-12 | NEXT SESSION STARTS HERE |
| 3.3 Life cycle assessment | own method intro, functional unit, boundary, Sc1-Sc4, EF 3.1, ReCiPe | 7-9 | not blocked |
| 3.4 The ReBuilt prototype | own method intro, then the AR system ONLY | 6-8 | not blocked |
| 3.5 User study | own method intro, sample, conditions, procedure, measures | 5-7 | blocked on study facts |
| 3.6 Strengths, weaknesses, biases | inherent properties of each method | 4-5 | write last |

## 3.2 SUB-STRUCTURE, agreed 2026-08-17 (Thiago proposed, Claude flattened the third level)

- 3.2.1 The product: why a VCU, why the Bosch MS 50.4, the printed replica and what it does not
  reproduce (glued joints, conformal coating, fastener behavior).
- 3.2.2 The passport data model: CIRPASS Table 6 as the SELECTION INSTRUMENT, the author-made mapping
  table, the honest-labelling rule, the grouping logic, the two divergences.
- 3.2.3 Storing and serving the record: the JSON payload, one file per unit, FastAPI, no database.

Thiago originally proposed "3.2.2.1 DPP backend". Flattened to 3.2.3 for two reasons, both accepted:
the document goes three levels deep at most elsewhere, and Chapter 1 already declares data architecture
out of scope ("this thesis does not address blockchain, access control, or the security methods needed
to store and share passport data at industrial scale"). Nested under the data model the backend reads as
part of the passport's specification and invites the architecture question. As a sibling it reads as how
the record was served for this study. 3.2.3 must point at that scope sentence.

## GROUPING BOUNDARY, set 2026-08-17 to stop a duplication

The passport face has FOUR TABS: 1 Product specifications, 2 Usage history, 3 Environmental impact,
4 Training disassembly, plus a header compliance badge (CE / REACH / WEEE 5 / IP67) and a certificates
screen. See rb2_1_dpp_page.
- 3.2.2 owns the GROUPING LOGIC: which Table 6 attributes cluster together and why. One sentence naming
  the four groups.
- 3.4 owns the RENDERING: tabs, chips, layout, role matrix.

## THE "FOUR VERSUS FOUR" TRAP. Do not let this reach the thesis unlabelled.

Two different measures both produce the number four and an examiner will conflate them:
- FOUR TABS = the presentation grouping.
- "4 of 13 mandatory attributes fully covered" = the RBv2.0 attribute scoreboard in table6_coverage_map.

Thiago's correction, 2026-08-17: "I did not cover only 4, I group it into 4, but inside it, you can find
the mandatory information from the table 6." He is right that coverage improved and the scoreboard is
stale. Evidence found in the artifacts: attribute 3 (CE-marking) was "field exists, value -" and the
frozen build renders a CE badge; attribute 7 was "none documented" and the live payload declares two
REACH SVHCs with CAS numbers. Whatever number goes in the thesis must say which measure it counts.

## THE JOB THAT MUST RUN BEFORE 3.2.2 IS WRITTEN

Re-derive the 22-attribute Table 6 coverage against the FROZEN payload backend/data/vcu_001.json
(plus the RB2_1_1 specs), not against RBv2.0. The existing map in
DPP_UI_Specs/RB2_0/13b_information_model.md is dated 2026-07-30 and its column is headed "In RBv2.0?".
Output: the author-made table that goes in the body of 3.2.2. See table6_coverage_map.

## TABLE 6 REPRODUCTION RULING

D2.2 p. 4: "(c) CIRPASS Consortium, 2024. Reproduction is authorised provided the source is
acknowledged." DO NOT reproduce CIRPASS's table. Put the author-made mapping in the body of 3.2.2,
sourced to Wautelet & Ayed (2024, pp. 41-42). It carries Table 6's attributes and M/U marks AND the
implementation decision in one object. Thiago agreed: "we can mentioned that I follow the table 6 from
CIRPASS, but then reproduce my table".

# THE ACCEPTED OPENER, verbatim. Do not re-draft.

> 3 METHODOLOGY
>
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
> two-dimensional manual came last. The prototype was frozen at a fixed version before any participant
> used it. This chapter reports procedure only, and every number these stages produced appears in the
> next chapter.

OPEN: "printed teardown artifact" here versus "3D-printed replica" in Chapter 1. One object, two names.
Decide and unify. See ch1_verbatim_facts.

# THE ACCEPTED 3.1, verbatim. Do not re-draft.

> Each stage answers one of the four sub-questions stated in the introduction. The passport data model
> answers the first, on which component-level and environmental information matters most to end-of-life
> stakeholders. The life cycle assessment answers the fourth, on the environmental difference between
> guided dismantling and current bulk practice. The prototype answers the second, on delivering that
> information through an Augmented Reality interface that supports physical interaction with the product.
> The controlled comparison answers the third, on how the passport performs against a conventional manual
> on time, errors and perceived usability. The introduction presents these questions in sequence. The
> work took them first, fourth, second, third.
>
> The first sub-question is answered from the literature, not from the test. It asks which information
> end-of-life stakeholders need, and no professional dismantler took part. The introduction explains why
> that was a deliberate boundary. The answer therefore rests on two sources: studies of what practitioners
> report needing, and what the European regulation requires a passport to carry. The other three
> sub-questions are answered by the work itself.
>
> CIRPASS and Jensen et al. (2023) disagree on environmental content. The CIRPASS summary of electronics
> requirements lists Product Environmental Footprint and life cycle assessment among the attributes a
> passport should carry (CIRPASS, 2024b, pp. 41-42). The recyclers Jensen et al. surveyed named none of
> it. This thesis follows the regulatory side and shows environmental impact to a recycler. The next
> section sets the two lists side by side, and the discussion returns to what the disagreement means.

Both blocks are Claude-drafted prose, edited by Thiago. AT RISK FOR DENOTING under the affidavit.
The "(CIRPASS, 2024b, pp. 41-42)" in 3.1 must become "(Wautelet & Ayed, 2024, pp. 41-42)".

# CIRPASS CITATION RULING: HYBRID

Prose names the project. The parenthetical names the authors. Keeps Chapter 2's paired contrast
("CIRPASS specifies a recycler who also supplies it"), is APA-correct, and dissolves the 2024a/b/c
problem because all four works have different first authors.

| deliverable | parenthetical | year |
|---|---|---|
| D2.1 Mapping of legal and voluntary requirements... | Wagner et al. | 2023 |
| D2.2 Exploring possible DPP use cases... | Wautelet & Ayed | 2024 |
| D2.3 Stakeholder consultation on key-data | Wagner et al. | 2024 |
| D5.1 DPP Prototypes | Bernier & Danash | 2024 |

Five in-text citations in the .docx to convert: "(CIRPASS, 2024a, p. 16)" and "(CIRPASS, 2024a,
pp. 10, 16)" were D2.3 -> Wagner et al. (2024); "(CIRPASS, 2024b, p. 10)", "(..., p. 20)",
"(..., p. 8)" were D5.1 -> Bernier & Danash (2024). "(CIRPASS-2, 2026)" is unaffected.
Wagner is first author on both D2.1 (2023) and D2.3 (2024). Different years, no letter needed, but the
reference list must distinguish them by title.

# TWO NEW VOICE RULES, extracted from Thiago's edits

### Rule: never cite a section or chapter number in running prose
Seen twice: "Section 2.6" -> "the literature review"; "Chapter 4" -> "the next chapter".
OVERRIDES the academic-writer skill's cross-reference guidance.

### Rule: AMERICAN spelling, decided 2026-08-17
Convert: standardis->standardiz, organis->organiz, characteris->characteriz, recognis->recogniz,
optimis->optimiz, modelled->modeled, behaviour->behavior, modelling->modeling, labelled->labeled,
centre->center, artefact->artifact. Check "centre" is not inside a proper noun.
("analys" is NOT a signal: "analysis" is spelled the same in both variants.)

# STANDING RULINGS

- The template is guidance, not a checklist. Cover the substance; structure it as the argument wants.
- Method framing is OPTION B: no named methodology framework. Each section explains its own method and
  why it was chosen. Design science research considered and rejected.
- The three-limitations rule. 1.3 = excluded in advance. 3.6 = what each method cannot see even applied
  perfectly. Discussion = what actually went wrong.
- The Bosch datasheet in the appendix = an author-made specification extract table, values only, each
  cited to Data Sheet 245099915. The datasheet is NOT reproduced.
- The environmental-content departure is named in 3.1 (done), detailed in 3.2.2, defended in the
  Discussion. Do not collapse the three.
- Hardware rule. The printed replica is described once, in 3.2.1. Section 3.4 covers only the AR system
  and names the replica by reference.
- The routes-versus-scenarios rule moved to the top of 3.3, where scenarios first appear. Routes are
  pathways people choose; scenarios are the modeled constructs. One bridge sentence, once: the four
  scenarios represent the routes, and they are what the passport displays. DO NOT LOSE THIS.

# OPEN ITEMS CARRIED INTO THE NEXT SESSION

1. Re-derive the Table 6 coverage against the frozen payload. The single biggest job.
2. Two version numbers. dpp_meta.schema_version = "0.13", a separate top-level schema_version exists,
   and memory elsewhere calls the payload v0.19. Reconcile before quoting any version.
3. PETG and Bambu Lab P2S are [M], never traced. Confirm or drop from 3.2.1.
4. The replica naming decision (artifact versus replica).
5. The frozen build version is unconfirmed. If a bug-fix build was flashed after 2026-08-10,
   participants did not use RBv2.1.1 and 3.4 and 3.5 must name what they did use.
6. Study facts for 3.5: participant count, group assignment, within or between subjects, measures
   captured, where the raw files live, any protocol deviation.
7. Delete the original hardlinked CIRPASS_D2.2_DPP_UseCases_Report_v2.0.pdf and rename the _FIXED copy
   back. The bridge cannot delete. Then rebuild _index/lit_index.jsonl, stale since 2026-08-16.
8. The American spelling pass across the .docx.
9. Voice rule 7 INVERTS here. Methodology requires protocols; Chapter 2 strips them.

Related: methodology_start_here, ch1_verbatim_facts, cirpass_d22_table6, cirpass_d21_requirements,
dpp_payload_verified, table6_coverage_map, jensen_2023_data_needs, voice_and_verification_rules,
thesis-schedule, registered_research_design, annex_vi_schema_gap, rb2_1_dpp_page
