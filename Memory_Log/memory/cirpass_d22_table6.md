---
name: cirpass-d22-table6
description: "[P] CIRPASS D2.2 Table 6 read directly on 2026-08-17 after the hardlink was broken. Table 6 is a SUMMARY of existing legislation and initiatives, not a CIRPASS-designed field list; it originates in report D2.1; and it is for the ELECTRONICS sector. It lists Product Environmental Footprint and LCA as DPP attributes."
type: reference
---

# [P] Read 2026-08-17 (Session 34) from the un-hardlinked copy

File: LITERATURE/CIRPASS_D2.2_DPP_UseCases_Report_v2.0_FIXED.pdf (86 pages).
Table 6 sits on pp. 41-42, inside section 4.2.2 "Digital Product Passport", the ELECTRONICS chapter.
The original CIRPASS_D2.2_DPP_UseCases_Report_v2.0.pdf is still hardlinked (nlink 2) and unreadable by
the tooling. The _FIXED copy has nlink 1. Thiago must delete the original by hand; the bridge cannot
delete.

D2.2 v2.0, March 2024, delivered 25.03.2024. Authors: Thibaut Wautelet (+ImpaKT Luxembourg),
Anne-Christine Ayed (AOC Innovation). Grant Agreement 101083432, DIGITAL-2021-TRUST-01.
Legal notice p. 4, verbatim: "(c) CIRPASS Consortium, 2024. Reproduction is authorised provided the
source is acknowledged."

# THREE CORRECTIONS TO WHAT MEMORY SAID

Memory called Table 6 "the source of the schema's field list" and "the adopted field list". All three
parts of that are loose:
1. It is NOT a CIRPASS-designed schema. The caption reads, verbatim: "Table 6 - Summary of findings on
   current data used in the electronics sector in initiatives (U) or in legislation (M)". It is a survey
   of what already exists, each row marked M for mandatory by legislation or U for used by DPP-related
   initiatives.
2. It ORIGINATES in report D2.1, not D2.2. Verbatim, p. 41: "An initial set of information requirements
   for the electronics sector (see Table 6) was proposed in an earlier report, by extracting requirements
   from existing and upcoming legislative texts as well as those used in a large number of currently
   proposed DPP-related initiatives (see report D2.1 ...)." D2.2 reproduces it as a baseline.
3. It is the ELECTRONICS sector list. A Vehicle Control Unit is an automotive component. Using this list
   is defensible but it is a sector-adjacent choice and the Methodology must say so.

Also verbatim, p. 41, useful for the gap claim: "there is currently no solution implemented beyond
prototyping and piloting." And: "there is currently no indication of the content of the Digital Product
Passport for electronics or which product categories may be prioritized for a DPP in the initial stages."

# THE ROW THAT CHANGES THE THESIS'S ARGUMENT

Under the category INDICATORS, marked U / M (footnote 45):
> "Circularity indicator (repairability, reuse, recycling index), environmental and social impact
> indicator, Product Environmental Footprint, Life Cycle Assessment"

Product Environmental Footprint and Life Cycle Assessment ARE listed as DPP data attributes. Therefore
the claim "the literature does not ask for environmental content" is FALSE and must never be written.
The true position is sharper and better:
- The regulatory and initiative side (CIRPASS Table 6) lists environmental footprint and LCA.
- The practitioner side (Jensen et al. 2023, p. 251) found recyclers did not name it.
- The two sources disagree. The thesis follows the first, and whether a recycler actually uses that
  content is an open question.

# TABLE 6 IN FULL, transcribed pp. 41-42 (M = mandatory by legislation, U = used by initiatives)

FUNCTIONAL AND PERFORMANCE TECHNICAL SPECIFICATIONS
- Product information sheet on energy consumption & technical (M)
- Technical documentation with product-model specific information, e.g. test results, measurement
  method (Energy Labelling Regulation) (M)
- CE-marking (M)
- Disposal, return and collection scheme information (M)

MATERIAL AND COMPOSITION INFORMATION
- Information on different materials and location of dangerous substances and mixtures (WEEE) (M)
- Substances of concern: name, location within the product, concentration at the level of the product,
  main components or spare parts (M)
- Hazardous substances (REACH, POP, CLP, Ecodesign, WEEE) (M)
- Individual material declaration (U)
- Full material composition (U)
- Recycled content (U)
- Recycling oriented information (U)

PRODUCT DESIGN AND SERVICE
- Use, repair information (maintenance, spare parts, updates) (M)
- Repair information incl. disassembly instructions, component map, etc. (Ecodesign) (M)
- Disassembly instructions (WEEE) (M)
- Resale options, end-of-life options, service availability for waste handling (U)
- Instructions for safe use (M)
- User manuals, instructions, warnings or safety information (M)
- Information relevant for disassembly (M)

USAGE HISTORY
- Usage data (purchase date, use cycles, etc.) (U)

REPAIR AND REUSE HISTORY
- Repair data (date, exchanged parts, costs, images) (U)

INDICATORS
- Circularity indicator (repairability, reuse, recycling index), environmental and social impact
  indicator, Product Environmental Footprint, Life Cycle Assessment (U / M, footnote 45)

CERTIFICATION
- Responsibility supply chain certifications (U)

Table note, verbatim: "product identification and company information are not listed in this table but
are referred to in report D2.1".

Footnote 45: for smartphones and tablets, additional ecodesign requirements and repairability
information on the Energy Label become mandatory by June 2025. A VCU is not in that product group, which
is why the prototype treats the indicator row as U.

"location of dangerous substances" is mandatory here (WEEE row) and "location" is the field missing from
Component in dpp_schema.json. That is the same gap annex_vi_schema_gap found against Annex VI, now found
independently in the CIRPASS summary. Two sources, one missing field. Strong material for section 3.2.

# CITATION

Under the hybrid ruling of 2026-08-17: prose names CIRPASS, the parenthetical reads
(Wautelet & Ayed, 2024, pp. 41-42). Describe it as a SUMMARY of existing legislative and initiative
requirements for the electronics sector, never as CIRPASS's own proposed schema.

Related: cirpass_d21_requirements, annex_vi_schema_gap, jensen_2023_data_needs,
ch3_methodology_progress, table6_coverage_map, eu_regulatory_scope
