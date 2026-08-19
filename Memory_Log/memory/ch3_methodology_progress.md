---
name: ch3-methodology-progress
description: THE AUTHORITATIVE live state of the Methodology chapter, updated end of Session 35 (2026-08-18). Six-section structure with the 3.2 sub-structure, the accepted opener and 3.1 verbatim, the drafted 3.2 intro, the three-role product framing, the CIRPASS hybrid citation ruling, and every open item.
type: project
---

# THE STRUCTURE LIVES IN THIS FILE. Do not read a structure table from any other memory.

State at the end of Session 35, 2026-08-18. Written LINEARLY from the opener.

# STRUCTURE AND BLOCK STATE

| section | contents | est. | state |
|---|---|---|---|
| opener | verbatim below | 2 | CLOSED |
| 3.1 Research design | sub-question map, the SQ1 limit, the CIRPASS/Jensen disagreement | 3 | CLOSED |
| 3.2 intro | orienting lead, no argument | 1 | DRAFTED, awaiting Thiago |
| 3.2.1 The product | three roles, the composite, the mass ceiling, the replica | 3-4 | NEXT. Fully unblocked |
| 3.2.2 The passport data model | Table 6 as selection instrument, the author-made mapping table, honest labelling, grouping logic, the two divergences | 4-5 | unblocked, table is derived |
| 3.2.3 Storing and serving the record | JSON payload, one file per unit, FastAPI, no database | 1-2 | unblocked |
| 3.3 Life cycle assessment | own method intro, routes-versus-scenarios rule, functional unit, boundary, Sc1-Sc4, EF 3.1, ReCiPe | 7-9 | unblocked |
| 3.4 The ReBuilt prototype | own method intro, then the AR system ONLY | 6-8 | unblocked |
| 3.5 User study | own method intro, sample, conditions, procedure, measures | 5-7 | BLOCKED on study facts |
| 3.6 Strengths, weaknesses, biases | inherent properties of each method | 4-5 | write last |

Hardware rule: the printed replica is described once, in 3.2.1. Section 3.4 covers only the AR system.
Grouping boundary: 3.2.2 owns which Table 6 attributes cluster and why. 3.4 owns tabs, chips, layout.
Routes-versus-scenarios rule goes at the top of 3.3, not 3.1. Routes are pathways people choose;
scenarios are the modeled constructs. One bridge sentence: the four scenarios represent the routes, and
they are what the passport displays. DO NOT LOSE THIS.

# ACCEPTED TEXT, verbatim. Do not re-draft.

## Opener (3 METHODOLOGY)

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

OPEN: "printed teardown artifact" should become "3D-printed replica". Chapter 1 uses replica, and the
payload carries physical_unit.is_replica true and replica_of. One-word edit, not yet confirmed.

## Section 3.1 Research design

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
> passport should carry (Wautelet & Ayed, 2024, pp. 41-42). The recyclers Jensen et al. surveyed named
> none of it. This thesis follows the regulatory side and shows environmental impact to a recycler. The
> next section sets the two lists side by side, and the discussion returns to what the disagreement means.

## 3.2 intro, DRAFTED 2026-08-18, not yet confirmed

> The passport specified here is a record of one particular object, so this section covers both. It opens
> with the product: which control unit supplied the parameters, and what participants actually held. It
> then sets out the record, the fields it carries and the instrument used to choose them. It closes with
> how those fields were stored and served during the study. The object comes first. The record follows it.

All three blocks are Claude-drafted prose, edited by Thiago. AT RISK FOR DENOTING under the affidavit.

# THE THREE-ROLE PRODUCT FRAMING, agreed 2026-08-18

| name | what it is | role |
|---|---|---|
| Reference product | the Bosch MS 50.4 | source of geometric, electrical and mass parameters. Cited, never claimed as the object studied |
| Modelled unit | a representative automotive control unit parameterised from that sheet | what the LCA assesses; the functional unit |
| Study object | the 3D-printed replica | what participants held |

THE MODELLED UNIT IS A COMPOSITE: hardware parameters from a motorsport reference product, duty cycle
from passenger-car assumptions. Say it that way; it is stronger than "representative".

Three facts that must appear:
1. "Weight <= 660 g" is a CEILING. The payload records 660 and the BOM sums to 660.1565 g. State it in
   3.2.1 as a parameter note and in 3.6 as a limitation.
2. All 15 components carry basis: "assumed". Not one is verified. 3.2.2 and 3.6.
3. Identity fields are illustrative. The passport names Bosch, the MS 50.4, a type number and a serial
   while all material data is assumed. BUILD STAYS FROZEN; 3.2.2 states it plainly, 3.6 flags it as an
   inherent risk of the approach.

service.maintenance_interval is null BY DESIGN, not an omission. The sheet's 220 h / two years is the
motorsport reference product's service regime and does not transfer to a passenger-car duty cycle.

DO NOT WRITE that a motorsport unit is heavier or higher-specified than a series passenger VCU.
[Guessing], no source, and Thiago has no time to find one. Ruled 2026-08-18: THE BOM IS NOT BEING
CHANGED. Its defence is the field-level honest-labelling rule, not a comparison.

# THE TABLE 6 COVERAGE IS RE-DERIVED

Done 2026-08-18 against the frozen payload. Full table and evidence in table6_coverage_map.
Headline: of the 13 mandatory attributes, 2 DO NOT APPLY to this product group with the reason recorded
in the record itself; of the 11 that apply, 5 covered, 5 partial, 1 declared absent. Of the 9 voluntary:
4 covered, 2 partial, 3 declared absent.
ALWAYS SAY WHICH MEASURE IS COUNTED. "Four tabs" and "N of 13 attributes" are different statements.

# STANDING RULINGS

- METHODOLOGY STATES, THE DISCUSSION DEFENDS. A defence written before results is an assertion.
- The template is guidance, not a checklist.
- Option B: no named methodology framework. Each section explains its own method.
- Three-limitations rule. 1.3 = excluded in advance. 3.6 = what each method cannot see even applied
  perfectly. Discussion = what actually went wrong.
- Bosch datasheet in the appendix = author-made specification extract, values only. Not reproduced.
  CITE DATA SHEET 234686731, DATED 27 MARCH 2026, the number printed in the page-4 footer. 245099915 is
  only the filename.
- CIRPASS citations: HYBRID. Prose names the project, the parenthetical names the authors.
  D2.1 = Wagner et al. (2023) | D2.2 = Wautelet & Ayed (2024) | D2.3 = Wagner et al. (2024) |
  D5.1 = Bernier & Danash (2024). FIVE in-text citations still to convert in the .docx:
  (CIRPASS, 2024a, p. 16) and (..., pp. 10, 16) were D2.3 -> Wagner et al. (2024);
  (CIRPASS, 2024b, p. 10), (..., p. 20), (..., p. 8) were D5.1 -> Bernier & Danash (2024).
- Table 6 is not reproduced. The author-made mapping goes in 3.2.2, sourced to Wautelet & Ayed (2024,
  pp. 41-42). Permitted anyway: "(c) CIRPASS Consortium, 2024. Reproduction is authorised provided the
  source is acknowledged."
- AMERICAN spelling. NEVER cite a section or chapter number in running prose.

# OPEN ITEMS

1. STUDY FACTS FOR 3.5. Participant count, group assignment, within or between subjects, measures
   captured, where the raw files live, protocol deviations. Open since 2026-08-17. The only true blocker.
2. Confirm the 3.2 intro paragraph, then draft 3.2.1.
3. Change "printed teardown artifact" to "3D-printed replica" in the opener.
4. Delete the hardlinked CIRPASS_D2.2_DPP_UseCases_Report_v2.0.pdf; rename the _FIXED copy back.
5. Rebuild _index/lit_index.jsonl, stale since 2026-08-16 and now missing D2.1.
6. Convert the five CIRPASS in-text citations.
7. American spelling pass across the .docx.
8. Confirm which build version was flashed for the sessions.
9. Two schema_version values in the payload: top-level 0.19, dpp_meta 0.13. Contradiction inside the
   frozen artifact. Decide which the thesis quotes.
10. Adisorn p. 9 not p. 10; Chen et al. pages need re-paging against the published article.
11. Voice rule 7 INVERTS here. Methodology requires protocols; Chapter 2 strips them.

Related: methodology_start_here, ch1_verbatim_facts, table6_coverage_map, modelled_unit_composition,
vcu_datasheet_verified, cirpass_d22_table6, cirpass_d21_requirements, dpp_payload_verified,
jensen_2023_data_needs, voice_and_verification_rules, thesis-schedule, annex_vi_schema_gap
