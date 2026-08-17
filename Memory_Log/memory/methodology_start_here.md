---
name: methodology-start-here
description: Reference file for the Methodology chapter: what it owns, the gap at final width, the four frames, the page-verified method precedents, and the on-disk blockers. THE STRUCTURE AND LIVE BLOCK STATE ARE NOT HERE, they are in ch3_methodology_progress.md.
type: project
---

# THE STRUCTURE IS NOT IN THIS FILE

Read ch3_methodology_progress.md for the six-section structure, the 3.2 sub-structure, the accepted
opener and 3.1, the block state and every ruling. This file carries only the durable reference material.
An earlier version held an eight-section structure table. That table is SUPERSEDED and was removed so it
cannot mislead a later session.

Do not offer to re-litigate the schedule. Thiago has said twice that he has it under control.

# BLOCKERS, on disk

1. CIRPASS D2.2: a readable copy exists as CIRPASS_D2.2_DPP_UseCases_Report_v2.0_FIXED.pdf (made
   2026-08-17 by copying to break the hardlink). THE ORIGINAL IS STILL HARDLINKED and must be deleted by
   hand; the bridge cannot delete. Table 6 is transcribed in cirpass_d22_table6.
2. _index/lit_index.jsonl stale since eighteen AR PDFs plus Regulation (EU) 2026/1738 were added
   2026-08-16.
3. Adisorn page error already pasted into the document: "equipment necessary to really gain information
   access" is p. 9, not p. 10.
4. Chen et al. pages throughout 2.5 are pre-print pages. Re-page against Journal of Environmental
   Management, 348, 119341.

# STUDY FACTS MEMORY DOES NOT HOLD, and section 3.5 cannot be written without them

All participant sessions are complete (Thiago, 2026-08-17). Still unrecorded: participant count and group
assignment, within or between subjects, which measures were captured, where the raw files live, the build
version flashed, any protocol deviation. ASK HIM.

# WHAT METHODOLOGY MUST CONTAIN, from the SRH contract

Heading wording in the document: "3.1 Research design, analytic methodology employed & reason for
choice". Restate the research gap and the goal first, then: which methods, how each works with examples
of prior studies using them, what each explores and why it fills the gap, how each is applied in detail,
scope limits, ENOUGH PROCEDURAL DETAIL FOR INDEPENDENT REPLICATION, data sources and extraction process,
and the inherent strengths, weaknesses and biases of each method.
MUST NOT CONTAIN results, interpretation, or a re-run of the literature review beyond method
justification.
TREAT THIS AS GUIDANCE, NOT A CHECKLIST. Thiago's ruling, 2026-08-17.

Prototype placement (decided 2026-08-10): Methodology owns the prototype entirely as an object. Findings
may only report how it behaved as an outcome of the study. Test: if a sentence about the prototype would
still be true had the study never run, it is description and belongs here.

# WHAT METHODOLOGY OWNS EXCLUSIVELY

The Bosch MS 50.4 and why a VCU. The functional unit, system boundary, Sc1 to Sc4. EF 3.1 named as this
study's method, ReCiPe as cross-check. Why ReCiPe normalisation and weighting were rejected. ReBuilt's
architecture and the five-step sequence. The study design. CIRPASS Table 6 as the adopted selection
instrument. Every LCA and study number's provenance. The LCA assumptions.

# COLLISIONS ALREADY RESOLVED

- Method precedents. Li, Webel, Windhausen, Ariansyah, Kuehn and Mao appear in 2.5 for their positions
  and again in Chapter 3 for their protocols. Not duplication, because the treatment differs.
- Naming CIRPASS again. Not a collision. The Mention Rule permits naming a topic another chapter owns.
- Three limitations sections. 1.3 = excluded in advance. 3.6 = inherent to each method. Discussion =
  what actually went wrong.

# THE GAP METHODOLOGY MUST ANSWER, at its final width

No study has tested a STANDARDIZED record, ISSUED WITH THE PRODUCT, READ IN THE OPERATOR'S FIELD OF VIEW.
Narrowed three times over:
- Abdel-Aty et al. (2025) already put a passport in front of an operator, with measurements.
- Li et al. (2023) already ran AR-guided disassembly with forty workers.
- Mao et al. (2025) already served AR guidance from a structured knowledge base, fetched by component
  reference, anchored to the equipment. What their record is not: standardized, or issued with the
  product.

# THE FOUR FRAMES, and which questions they interpret

| frame | introduced | interprets |
|---|---|---|
| Information asymmetry at end of life | 2.3 / 2.4 | SQ1 |
| Disassemblability evaluation | 2.2 | SQ3 |
| Burden-weighted against mass-weighted recovery | 2.2 | SQ4 |
| Perceptual and cognitive load in AR | 2.5 paragraph 6 | SQ2, SQ3 |

Abdel-Aty et al.'s Operator Workload is (assembly + disassembly time) / cycle time, a share of working
time. IT IS NOT PERCEIVED MENTAL WORKLOAD. Cognitive load belongs to Ariansyah and the Multiple Resources
Model.

# METHOD PRECEDENTS ALREADY PAGE-VERIFIED

- Li et al. (2023) p. 14: four groups of ten workers, three rounds, ten replicates, identical battery
  packs, metric is time to reach the disassembly quality standard. Junior and skilled defined by
  experience and by top or bottom 20 % on efficiency and quality. Also three interface modes (text,
  image, simplified) and, p. 11, a tool-attention mechanism to avoid information overload.
- Webel et al. (2013) p. 4: twenty technicians, ten per group, video against AR, morning training and
  afternoon unaided performance, consulting a photo book recorded as an "aid", errors split into solved
  and unsolved. The solved/unsolved split is where their only significant effect appeared.
- Windhausen et al. (2024): two lab studies, printed list against glasses, PROCESS Model 7 moderated
  mediation, seven-point scales, reliabilities reported.
- Ariansyah et al. (2022): 2x2, information mode x interaction modality, plus eye gaze (fixations, area
  of interest transitions) and subjective usability.
- Kuehn et al. (2025) p. 19: DIN EN 45554 scored on disassembly depth, fastener system, tools, working
  environment, skill level, diagnostic support.
- Mao et al. (2025) pp. 7, 8: cloud repository of structured knowledge objects on a domain ontology,
  procedures in JSON fetched by component reference through an API, overlays anchored by spatial mapping,
  each step held until gesture confirmation. The closest architectural precedent that exists.
- Jensen et al. (2023) pp. 245-246: multiple-case study per Yin (2009), three circular supply chains,
  28 interviews then a survey with N = 22 at 71 % response, five-point scales on importance, availability
  and sensitivity.

# BEFORE WRITING A WORD

Read voice_and_verification_rules and ch3_methodology_progress. Two rules INVERT between Chapter 2 and
Chapter 3; both are documented in the voice file.

Related: ch3_methodology_progress, ch1_verbatim_facts, thesis-schedule, voice_and_verification_rules,
cirpass_d22_table6, cirpass_d21_requirements, dpp_payload_verified, table6_coverage_map,
jensen_2023_data_needs, annex_vi_schema_gap, lca_scope_verified, research_questions_final,
registered_research_design, vcu_bosch_ms504, rbv2-1-1-handoff
