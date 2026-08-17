---
name: dpp-payload-verified
description: "[P] The live passport payload backend/data/vcu_001.json read directly on 2026-08-17. Its 20 top-level keys, the machine-recorded reference framework naming CIRPASS D2.2 Table 6 pp. 41-42, the T6 traceability marks, and the finding that substances_of_concern carry a free-text location_note with component_id NULL."
type: reference
---

# [P] Read 2026-08-17 (Session 34) from AR_DPP/backend/data/vcu_001.json (63 KB)

# The 20 top-level keys, verbatim

product_id, dpp_meta, identity, specifications, documents, components, substances_of_concern,
precious_metals, environmental, indicators, compliance, certifications, service, usage_history,
repair_history, disassembly, end_of_life, physical_unit, unit_use_phase, schema_version,
material_reference

# THE PAYLOAD DECLARES ITS OWN REFERENCE FRAMEWORK

    "dpp_meta": {
      "schema_version": "0.13",
      "reference_framework": "CIRPASS D2.2 Table 6 (pp. 41-42)",
      "last_updated": "2026-08-09",
      "completeness_note": "Prototype passport for a Master's thesis user study. Impact figures are the
       openLCA v4 / EF 3.1 deterministic centrals; material splits remain assumed pending VCU_BOM_v4
       reconciliation. Attributes with basis/status 'not_provided' have no source yet - coverage against
       CIRPASS Table 6 is in DPP_UI_Specs/13b_information_model.md."
    }

Strong material for section 3.2. The anchor is machine-recorded in the artifact with the exact page range
independently verified in cirpass_d22_table6. It was not retrofitted to the thesis.

Traceability marks exist per attribute. Each entry in "documents" carries cirpass_ref (e.g. "T6 #1"),
mandatory (bool) and status (e.g. not_applicable), with a note explaining the status.

TWO VERSION NUMBERS EXIST: dpp_meta.schema_version = "0.13" and a separate top-level schema_version key.
Memory elsewhere calls the payload v0.19. Reconcile before quoting any version.

# THE "location" FINDING, sharpened

annex_vi_schema_gap is CORRECT: "location" is absent from Component, while PreciousMetal.location and
SubstanceOfConcern.location_note exist. Verified against the live payload.

    "substances_of_concern": [
     {"name":"Lead","cas_number":"7439-92-1","regulation":"REACH SVHC",
      "component_id": null,
      "location_note":"electronic components; the solder itself is lead-free SAC305",
      "concentration_pct_w_w": null, "threshold_pct_w_w":0.1, "above_threshold":true,
      "symbol":"Pb","basis":"declared"},
     {"name":"Lead monoxide (lead oxide)","cas_number":"1317-36-8","regulation":"REACH SVHC",
      "component_id": null,
      "location_note":"component ceramics / glass frits",
      "concentration_pct_w_w": null, "threshold_pct_w_w":0.1, "above_threshold":true,
      "symbol":"PbO","basis":"declared"}
    ]

component_id IS NULL ON BOTH ENTRIES. So the record carries location only as free prose, and the
substance is bound to no component in the model. The passport can say the lead is "in electronic
components". It cannot say which component, and nothing can point at it.

THAT IS THE THESIS'S ARGUMENT IN ONE ARTIFACT. Not "the field is missing" but "the field exists as prose,
and prose cannot be pointed at". The law requires location (WEEE via CIRPASS D2.1 Table 6 p. 38, Annex VI
points 4(b) and 5(b)); the record renders it as a sentence; the AR layer renders it natively.
precious_metals has the same shape: "location": "Board (230 ppm [L]) + connector plating [A]", prose.

Other confirmed values: identity.manufacturer "Bosch Motorsport", model "Vehicle Control Unit MS 50.4",
type_number "F02U.V02.965-02", serial "VCU0001", country_of_origin "DE", product_category
"EEE - electronic control unit (WEEE cat. 5, small equipment)". specifications.size_mm "166 x 121 x 41",
weight_g 660, IP67, supply 5-18 V, operating -20 to 80 C, 3 round connectors (LIFE, SENS-A, SENS-B).
environmental.co2_footprint_kg 73.4326, method "EF 3.1 (EN 15804+A2) - climate change - ecoinvent 3.8
APOS". components list has 15 entries with material, weight_g, recycling_code, disassembly_step,
hazardous, basis, drawing_id, material_breakdown, minerals_impact_kg_sb_eq, reuse_eligible, mesh_nodes.

Related: cirpass_d22_table6, cirpass_d21_requirements, annex_vi_schema_gap, ch3_methodology_progress,
jensen_2023_data_needs, table6_coverage_map, dpp_payload_v07_bom_reconciliation
