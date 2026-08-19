---
name: table6-coverage-map
description: "[P] THE RE-DERIVED CIRPASS Table 6 coverage against the FROZEN RBv2.1.1 payload, computed 2026-08-18. All 22 attributes with verdict and evidence, the corrected scoreboard, and what changed against the stale RBv2.0 map. This is the table that goes in the body of section 3.2.2."
type: reference
---

# [P] Re-derived 2026-08-18 (Session 35) against backend/data/vcu_001.json (frozen, dated 2026-08-09)

This SUPERSEDES the RBv2.0 scoreboard in DPP_UI_Specs/RB2_0/13b_information_model.md (30 July), whose
column was headed "In RBv2.0?". That file remains the record of the SELECTION METHOD; this file is the
record of what the study build actually carries.

FOUR VERDICTS, NOT THREE. The frozen build declares two attributes NOT APPLICABLE with a legal reason
recorded in the artifact. That differs from failing to carry them.
- covered = a value is present and usable
- partial = some of the attribute's required elements are present
- declared absent = the field exists and honestly reports no value (not_provided)
- n/a = the legislation does not cover this product group, with the reason recorded

# THE TABLE (source: CIRPASS Table 6, Wautelet & Ayed, 2024, pp. 41-42)

| # | Attribute | M/U | Verdict | Evidence in the frozen build |
|---|---|---|---|---|
| 1 | Product information sheet, energy consumption & performance | M | n/a | documents[energy_sheet].status = not_applicable, "Not an energy-labelled product group" |
| 2 | Technical documentation, product-model specific | M | n/a | documents[technical_doc], Energy Labelling Regulation does not cover this group |
| 3 | CE-marking | M | covered | compliance.ce, scope 2014/30/EU (EMC), tested to ECE R10 rev.6:2019, declaration 2020-10-09, DoC available (Operation Manual pp. 132-133), header badge renders CE |
| 4 | Disposal, return and collection scheme information | M | partial | end_of_life.recycling_route = "WEEE - selective treatment recommended"; collection_scheme entirely null |
| 5 | Materials AND LOCATION OF dangerous substances (WEEE) | M | partial | materials complete (15/15); location only prose, hazardous false on ALL components |
| 6 | Substances of concern: name, location, concentration | M | partial | 2 SVHCs, CAS + threshold + above_threshold; concentration_pct_w_w null, component_id null |
| 7 | Hazardous substances (REACH, POP, CLP, Ecodesign, WEEE) | M | covered | compliance.reach true, rohs_applicable false, WEEE Cat. 5 small equipment, 2 declared SVHCs |
| 8 | Individual material declaration | U | covered | 15/15 components with material, weight_g, recycling_code |
| 9 | Full material composition | U | covered | 15/15 with material_breakdown |
| 10 | Recycled content | U | declared absent | recycled_content_pct null, recycled_content_basis = not_provided |
| 11 | Recycling oriented information | U | covered | route, recovery_potential.total_avoidable_kg 15.4315, per-material recovery % in material_reference, impact_recovery per scenario |
| 12 | Use, repair information (maintenance, spare parts, updates) | M | partial | 12 dated software updates + software_update_policy; spare_parts empty; maintenance_interval null BY DESIGN |
| 13 | Repair info incl. disassembly instructions, component map | M | covered | documents[disassembly_guide] available, cirpass_ref "T6 #13 #14 #18", note "Delivered interactively by the AR guided step flow instead of as a document"; 5 steps with component_ids, components carry mesh_nodes and drawing_id |
| 14 | Disassembly instructions (WEEE) | M | covered | same |
| 15 | Resale, end-of-life options, waste handling services | U | declared absent | three empty arrays, service.basis = not_provided |
| 16 | Instructions for safe use | M | declared absent | documents[safe_use].status = not_provided; disassembly.safety_warnings confirmed empty |
| 17 | User manuals, instructions, warnings, safety information | M | partial | no manual; per-step task rows carry title and subtitle |
| 18 | Information relevant for disassembly | M | covered | total_steps 5, estimated_time_min 5, 1 tool, 7-part list, per-step actions |
| 19 | Usage data (purchase date, use cycles) | U | covered | unit_use_phase.exposure: 5,625 powered hours, 11,250 ignition cycles, temperature and delta-T histograms; environmental.usage_profile: 15 years, 225,000 km, annual distances |
| 20 | Repair data (date, exchanged parts, costs, images) | U | partial | dated events with description, category, system, odometer; cost_eur, image_url, exchanged_component_ids all empty |
| 21 | Circularity indicator, impact indicator, PEF, LCA | U | partial | LCA complete: 73.4326 kg CO2 eq under EF 3.1 across 5 stages; all three circularity scores null, pef_note null, social_impact_note null |
| 22 | Responsibility supply chain certifications | U | declared absent | certifications empty array |

# THE SCOREBOARD, and the sentence for the thesis

Of the 13 mandatory attributes, 2 DO NOT APPLY to this product group and the reason is recorded in the
record itself. Of the 11 that apply: 5 COVERED, 5 PARTIAL, 1 DECLARED ABSENT.
Of the 9 voluntary attributes: 4 covered, 2 partial, 3 declared absent.

ALWAYS SAY WHICH MEASURE IS BEING COUNTED. "Grouped into four tabs" and "covered N of 13" are different
statements that used to collide on the number four.

# WHAT CHANGED AGAINST THE STALE RBv2.0 MAP

| | RBv2.0 (30 Jul) | RBv2.1.1 (frozen) |
|---|---|---|
| M covered | 4 | 5 (#3 CE moved partial to covered) |
| M partial | 5 | 5 (#3 left, #12 arrived from absent) |
| M absent | 4 | 1 (#1 and #2 became n/a with a recorded reason; #12 became partial) |
| U covered | 3 | 4 (#19 moved partial to covered) |
| U partial | 3 | 2 |
| U absent | 3 | 3 (#10 arrived, #20 left) |

Thiago's correction of 2026-08-17 was right: coverage improved and "4 of 13" was badly stale.

# THREE THINGS FROM THE SELECTION METHOD THAT STILL BELONG IN THE THESIS

1. The footnote-45 judgement. Table 6 marks attribute 21 U/M because ecodesign and repairability
   information on the Energy Label becomes mandatory FOR SMARTPHONES AND TABLETS from June 2025. A VCU is
   not in that product group, so the prototype treats the row as U. A judgement made in writing before
   the build.
2. The spatial observation, recorded 2026-07-30, BEFORE the study. Verbatim: "The two most demanding M
   attributes are spatial... A flat table cannot satisfy them; a 3D model with per-part callouts can."
   The thesis's central claim was reached from the data model, in a build spec, before any participant.
3. The honest-labelling rule. Every value carries its basis: verified, assumed / modelled, or not
   provided (never a blank, never a plausible-looking guess). It explains the basis and status fields
   throughout the payload, and it is why "declared absent" is a meaningful verdict above.

# TWO FACTS THAT MUST TRAVEL WITH THE TABLE

- All 15 components carry basis: "assumed". Not one component's material data is verified. State it in
  3.2.2 and carry it into 3.6.
- service.maintenance_interval is null BY DESIGN. The data sheet supplies "220 h or a maximum of two
  years", but that is the motorsport reference product's service regime and does not transfer to the
  modelled passenger-car unit. See modelled_unit_composition. Write it as a boundary, not an omission.

# REPRODUCTION

D2.2 p. 4: "(c) CIRPASS Consortium, 2024. Reproduction is authorised provided the source is
acknowledged." RULING: do not reproduce CIRPASS's table. The table above IS the author-made artifact for
the body of 3.2.2, sourced to Wautelet & Ayed (2024, pp. 41-42).

Related: cirpass_d22_table6, cirpass_d21_requirements, dpp_payload_verified, modelled_unit_composition,
vcu_datasheet_verified, annex_vi_schema_gap, ch3_methodology_progress, jensen_2023_data_needs
