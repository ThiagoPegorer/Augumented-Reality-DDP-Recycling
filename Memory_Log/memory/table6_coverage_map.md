---
name: table6-coverage-map
description: "[P] DPP_UI_Specs/RB2_0/13b_information_model.md read 2026-08-17. The 22-attribute CIRPASS Table 6 mapping with M/U marks and coverage, the scoreboard, the footnote-45 judgement, the spatial observation and the honest-labelling rule. ITS COVERAGE COLUMN IS RBv2.0, NOT THE FROZEN RBv2.1.1 STUDY BUILD."
type: reference
---

# [P] Read 2026-08-17 from AR_DPP/DPP_UI_Specs/RB2_0/13b_information_model.md (119 lines, created 2026-07-30)

This file is the methodological instrument behind SQ1's answer. Thiago used CIRPASS D2.2 Table 6's
M (mandatory by legislation) and U (used by DPP-related initiatives) marks to decide what the passport
must and should carry. Section 3.2.2 is built on it.

# THE VERSION PROBLEM, resolve before writing any coverage number

The coverage column is headed "In RBv2.0?". The study build is RBv2.1.1, frozen 2026-08-10, with a
payload dated 2026-08-09. The map is two versions and eleven days behind the build participants used.
Every count below is RBv2.0's, not the study build's.
RE-DERIVE THE COVERAGE AGAINST backend/data/vcu_001.json BEFORE ANY NUMBER ENTERS THE THESIS.
This is the largest verification job standing in front of section 3.2.

Evidence that coverage already improved: attribute 3 (CE-marking) was "field exists, value -" and the
frozen build renders a CE badge in the header (CE / REACH / WEEE 5 / IP67); attribute 7 was "none
documented" and the live payload declares two REACH SVHCs with CAS numbers.

# THE SCOREBOARD AS RECORDED FOR RBv2.0 (do not quote until re-derived)

Table 6 lists 22 attributes: 13 M and 9 U (#21 counted as U, see the footnote judgement below).

| | M (13) | U (9) |
|---|---|---|
| fully covered | 4 (#7 #13 #14 #18) | 3 (#8 #9 #11) |
| partial | 5 (#3 #4 #5 #6 #17) | 3 (#10 #19 #21) |
| absent | 4 (#1 #2 #12 #16) | 3 (#15 #20 #22) |

WARNING: "4 of 13 mandatory attributes" and "grouped into 4 tabs" are DIFFERENT MEASURES that share the
number four. Whatever reaches the thesis must say which one it counts. See ch3_methodology_progress.

# THE 22 ATTRIBUTES, numbered as in the file

FUNCTIONAL AND TECHNICAL SPECIFICATIONS
1 product information sheet on energy consumption & performance (M) - absent in RBv2.0
2 technical documentation, product-model specific, Energy Labelling Regulation (M) - absent
3 CE-marking (M) - field exists, value unverified
4 disposal, return and collection scheme information (M) - partial, recycling_route only

MATERIAL AND COMPOSITION INFORMATION
5 materials AND LOCATION OF dangerous substances (WEEE) (M) - materials yes, location no
6 substances of concern: name, LOCATION WITHIN THE PRODUCT, concentration (M) - "None documented" only
7 hazardous substances, REACH/POP/CLP/Ecodesign/WEEE (M) - neutral case stated
8 individual material declaration (U) - per-component material + mass
9 full material composition (U) - 4 aggregated groups
10 recycled content (U) - field exists, value -
11 recycling oriented information (U) - route + recovery credits

PRODUCT DESIGN AND SERVICE
12 use, repair information, maintenance, spare parts, updates (M) - absent
13 repair information incl. disassembly instructions and component map, Ecodesign (M) - guided step flow
   + 3D twin
14 disassembly instructions, WEEE (M) - the guided step flow
15 resale options, end-of-life options, service availability for waste handling (U) - absent
16 instructions for safe use (M) - absent
17 user manuals, instructions, warnings or safety information (M) - per-step task rows only
18 information relevant for disassembly (M) - tools, time, scope, part list

USAGE / REPAIR / INDICATORS / CERTIFICATION
19 usage data, purchase date, use cycles (U) - design service life only
20 repair data, date, exchanged parts, costs, images (U) - absent
21 circularity indicator, environmental and social impact indicator, PEF, LCA (U/M) - LCA yes, no
   circularity index
22 responsibility supply chain certifications (U) - absent

# THREE THINGS IN THIS FILE THAT BELONG IN THE THESIS

### 1. The footnote-45 judgement, a documented pre-build decision
Table 6 marks attribute 21 as U/M because ecodesign and repairability information on the Energy Label
becomes mandatory FOR SMARTPHONES AND TABLETS from June 2025. A VCU is not in that product group, so the
prototype treats the row as U. A judgement call, made in writing, before the build. Exactly what the
replication standard asks for.

### 2. The spatial observation, recorded 2026-07-30, BEFORE the study
Verbatim: "The two most demanding M attributes are spatial - #5 location of dangerous substances,
#6 substances of concern located within the product. A flat table cannot satisfy them; a 3D model with
per-part callouts can... it is the strongest available argument that AR beats a paper WEEE sheet."
It also notes CIRPASS reaches the same conclusion for dismantling in D2.2 Table 8 (p. 56) and
UC4 / Figure 16 step 2. THE THESIS'S CENTRAL CLAIM WAS REACHED FROM THE DATA MODEL, IN A BUILD SPEC,
BEFORE ANY PARTICIPANT. That timestamp is worth stating.

### 3. The honest-labelling rule, a methodology decision with an integrity rationale
Every value carries its basis, three states only: VERIFIED (from the physical unit or a primary source),
ASSUMED / MODELLED (the material split and the LCA figures), NOT PROVIDED ("-", never a blank and never a
plausible-looking guess). This explains the basis and status fields found in the live payload.

Also recorded: most absent mandatory attributes (#1 #2 #12 #16) ARE DOCUMENTS, NOT DATA (product
information sheet, technical documentation, user manual, safe-use instructions), which is why the design
represents them as a presence row rather than content. A defensible design decision, worth one sentence.

# TABLE 6 REPRODUCTION IS PERMITTED

D2.2 p. 4 legal notice, verbatim: "(c) CIRPASS Consortium, 2024. Reproduction is authorised provided the
source is acknowledged." Grant Agreement 101083432, DIGITAL-2021-TRUST-01.
RULING 2026-08-17: do not reproduce CIRPASS's table. Put the author-made 22-row mapping in the body of
3.2.2 instead, sourced to Wautelet & Ayed (2024, pp. 41-42). It carries Table 6's attributes and marks
AND the implementation decision in one object.

The file also records Table 6's own note: "product identification and company information are not listed
in this table but are referred to in report D2.1." So identity data is required, sourced from D2.1.

Related: cirpass_d22_table6, cirpass_d21_requirements, dpp_payload_verified, annex_vi_schema_gap,
ch3_methodology_progress, jensen_2023_data_needs, rb2_1_dpp_page
