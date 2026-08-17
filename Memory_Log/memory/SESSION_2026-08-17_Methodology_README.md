# Session 34, 2026-08-17: the Methodology chapter opened

Second session of 2026-08-17. Session 33 earlier the same day closed Chapter 2.

## What was closed

- The Methodology OPENER, two paragraphs, accepted after four edits by Thiago.
- Section 3.1 Research design, three paragraphs, accepted.
Both are stored verbatim in ch3_methodology_progress.md. DO NOT RE-DRAFT THEM.

## Structure decided

Six sections, written linearly. 3.2 gained a sub-structure: 3.2.1 The product, 3.2.2 The passport data
model, 3.2.3 Storing and serving the record. See ch3_methodology_progress.md for everything.

## Sources verified this session, all [P]

- Jensen et al. (2023), all 14 pages. Recyclers need four data points and environmental footprint is not
  one of them. ADDED TO CHAPTER 2 block 2.4 this session; it was absent from the whole thesis.
- CIRPASS D2.2 Table 6, pp. 41-42. The hardlink that blocked this file for weeks was broken by copying
  it to CIRPASS_D2.2_DPP_UseCases_Report_v2.0_FIXED.pdf. THE ORIGINAL STILL NEEDS DELETING BY HAND.
- CIRPASS D2.1, July 2023. Its Table 6 p. 38 names WASTE TREATMENT as the designated data user for the
  LOCATION of dangerous substances, mandated by WEEE, delivered by "Manual, electronic media". That row
  is the legal spine of the thesis argument.
- backend/data/vcu_001.json, the live payload. It declares its own reference framework and carries
  location only as free prose with component_id null.
- DPP_UI_Specs/RB2_0/13b_information_model.md, the 22-attribute Table 6 coverage map. ITS COVERAGE IS
  RBv2.0, NOT THE FROZEN BUILD.
- Chapter 1's Scope section, read verbatim from the .docx. It corrected two beliefs memory carried from
  the signed proposal.

## Decisions taken

- Method framing: no named methodology framework. Each section explains its own method.
- The template is guidance, not a checklist.
- CIRPASS citations: HYBRID. Prose names the project, the parenthetical names the authors.
- Table 6: do not reproduce CIRPASS's table; put the author-made mapping in 3.2.2.
- American spelling.
- Never cite a section or chapter number in running prose.
- The three-limitations rule: 1.3 excluded in advance, 3.6 inherent to each method, Discussion actual.
- The Bosch datasheet appears in the appendix as an author-made extract table, not reproduced.

## The first job next session

RE-DERIVE THE 22-ATTRIBUTE TABLE 6 COVERAGE AGAINST THE FROZEN PAYLOAD. The existing map is RBv2.0 and
eleven days stale. Its output is the table that goes in the body of 3.2.2.

## Files written to this folder this session

MEMORY.md, ch3_methodology_progress.md, methodology_start_here.md, thesis-schedule.md,
voice_and_verification_rules.md, ch1_verbatim_facts.md, jensen_2023_data_needs.md,
cirpass_d22_table6.md, cirpass_d21_requirements.md, dpp_payload_verified.md, table6_coverage_map.md
