---
name: dpp-payload-verified
description: "[P] The live passport payload backend/data/vcu_001.json, read 2026-08-17 and re-read 2026-08-19 after the one edit made to it. Its 20 top-level keys, the machine-recorded reference framework naming CIRPASS D2.2 Table 6 pp. 41-42, the T6 traceability marks, the substances_of_concern free-text location_note with component_id NULL, the physical_unit.parts list and the CPU-to-Voltage-regulator correction, and how Unity binds to those names. Also carries the CIRPASS citation ruling."
type: reference
---

# [P] Read 2026-08-17 (Session 34) from `AR_DPP/backend/data/vcu_001.json` (63 KB)
# [P] Re-read 2026-08-19 (Session 36) after the only edit ever made to it

# The 20 top-level keys, verbatim

`product_id` · `dpp_meta` · `identity` · `specifications` · `documents` · `components` ·
`substances_of_concern` · `precious_metals` · `environmental` · `indicators` · `compliance` ·
`certifications` · `service` · `usage_history` · `repair_history` · `disassembly` · `end_of_life` ·
`physical_unit` · `unit_use_phase` · `schema_version` · `material_reference`

# 🔴 THE PAYLOAD DECLARES ITS OWN REFERENCE FRAMEWORK

```
"dpp_meta": {
  "schema_version": "0.13",
  "reference_framework": "CIRPASS D2.2 Table 6 (pp. 41-42)",
  "last_updated": "2026-08-19",          <- was 2026-08-09 until the Session 36 edit
  "completeness_note": "Prototype passport for a Master's thesis user study. Impact figures are the
   openLCA v4 / EF 3.1 deterministic centrals; material splits remain assumed pending VCU_BOM_v4
   reconciliation. Attributes with basis/status 'not_provided' have no source yet - coverage against
   CIRPASS Table 6 is in DPP_UI_Specs/13b_information_model.md."
}
```

**This is strong material for section 3.2.** The anchor is machine-recorded in the artifact with the
exact page range independently verified in [[cirpass_d22_table6]]. It was not retrofitted to the thesis.

**Traceability marks exist per attribute.** Each entry in `documents` carries `cirpass_ref` (e.g.
`"T6 #1"`), `mandatory` (bool) and `status` (e.g. `not_applicable`), with a `note` explaining the status.

⚠ **Two version numbers exist:** `dpp_meta.schema_version` = **"0.13"** and top-level `schema_version` =
**0.19**. Both re-confirmed 2026-08-19. **Still unreconciled. Decide which the thesis quotes.**

⚠ `last_updated` now reads **2026-08-19**, after the study. The payload's substantive **content** did not
change for the study, only a display label. Decide whether the opener's *"frozen at a fixed version before
any participant used it"* needs a qualifier.

# 🔧 THE ONE EDIT: `physical_unit.parts` — "CPU" -> "Voltage regulator" (2026-08-19)

`physical_unit` is the block describing the **printed teardown model**, not the modelled product:

```
"physical_unit": {
  "is_replica": true,                          <- over-claims, see [[teardown_model_as_built]]
  "replica_of": "Bosch Motorsport Vehicle Control Unit MS 50.4",
  "size_mm": "200 x 150 x 60",
  "basis": "measured",
  "note": "3D-printed demonstrator built for the ReBuilt v2.0 study. The product's declared size is
           166 x 121 x 41 mm (Bosch data sheet 234686731).",
  "parts": [
    {"id":"connectors","name":"Connectors","count":3,"colour":"grey","swatch_hex":"#7f9bc4",
     "photo_id":"connectors","note":"stand in for LIFE, SENS-A and SENS-B"},
    {"id":"processors","name":"Processors","count":2,"colour":"blue and yellow","swatch_hex":"#4da3ff",
     "photo_id":"processors","note":"667 MHz dual core"},
    {"id":"cpu","name":"Voltage regulator","count":1,"colour":"brown","swatch_hex":"#8a6240",
     "photo_id":"cpu","note":"regulator and analog front-end; supplies the 12 V and switchable
      5/12 V sensor rails"},
    {"id":"sensors","name":"Sensors","count":3,"colour":"red","swatch_hex":"#e24b4a",
     "photo_id":"sensors","note":null}
  ]
}
```

**Why the rename was a correction and not a preference.** The `components` list contains **no CPU**. It
contains `ic_1` *Processors 2x FCBGA + flash 2x 4 GB*, which the blue and yellow bars already stand for,
and `ic_2` *Regulators + analog front-end*, which is what the brown bar stands for. Calling the brown bar
"CPU" duplicated the processors and left the regulator unnamed.

**`id` and `photo_id` deliberately left as `cpu`.** Renaming keys in a frozen study artifact for cosmetic
reasons is not something that should happen after freeze.

⚠ **CORRECTION `[X]`.** During the session the key was left alone on the stated ground that *"`photo_id`
resolves to an image asset."* **That is false.** Verified 2026-08-20: `photo_id` is declared in
`DPPModels.cs:260` and **read by no code anywhere**; no asset named `cpu` exists under `Assets` or
`backend`. It is a dormant field, not a live reference. The decision stands; the reason does not.

# 🔴 THE UNITY BINDING: the label lives in the payload, not the build

`Assets/Scripts/DDP/DPPModels.cs:245` declares `public PhysicalUnit physical_unit;` and `:256`
`public string name;`. **The headset deserialises the part name straight out of the JSON**, so the one
payload edit changed what participants would read without touching a line of C#.

**Verified after the edit:** no string `"Voltage regulator"` exists anywhere in `Assets`, `backend`,
`schema` or `docs`, and the only surviving whole word `CPU` in the Unity sources is
`Assets/Editor/DPPUIBuilder.UsePhase.cs:175` — a use-phase duty-cycle row, *"CPU above 80 % load", "675
h"* — plus two `raw CPU buffer` comments in `QRCameraProbe.cs`. **All three correctly mean the processor
and must stay.**

This is worth one sentence in **3.2.3**: the record is the single source of the displayed content; the
build renders whatever the payload says.

# 🔴 THE `location` FINDING, sharpened

[[annex_vi_schema_gap]] is **CORRECT**: `location` is absent from `Component`, while
`PreciousMetal.location` and `SubstanceOfConcern.location_note` exist. Verified against the live payload.

```
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
```

**`component_id` is NULL on both entries.** So the record carries location only as free prose, and the
substance is bound to no component in the model. The passport can say the lead is "in electronic
components". It cannot say which component, and nothing can point at it.

**That is the thesis's argument in one artifact.** Not "the field is missing" but "the field exists as
prose, and prose cannot be pointed at". The law requires location (WEEE via CIRPASS D2.1 Table 6 p. 38,
Annex VI points 4(b) and 5(b)); the record renders it as a sentence; the AR layer renders it natively.
`precious_metals` has the same shape: `"location": "Board (230 ppm [L]) + connector plating [A]"`, prose.
**This is paragraph 5 of the 3.2.2 skeleton.**

# THE 15 COMPONENTS, re-listed 2026-08-19. Every one carries `basis: "assumed"`

`housing_upper` (Upper housing shell, HPDC) · `housing_bottom` (Bottom housing shell, HPDC) ·
`pcb` (Bare PCB, 4-layer FR-4) · `connectors` (Connectors 3x AS018-35) ·
`ic_1` (Processors 2x FCBGA + flash 2x 4 GB) · `ic_2` (Regulators + analog front-end) ·
`ic_3` (Power stages 6x DPAK) · `ic_4` (Comm transceivers + MEMS sensors) ·
`passives` (MLCC, R, L, misc on-board) · `ta_caps` (Tantalum capacitors, 4-16x) ·
`solder` (SAC305, Pb-free) · `tim` (Thermal interface material) · `coating` (Conformal coating) ·
`misc` (Labels, adhesives) · `fasteners` (14 x M3)

Sum = **660.1565 g**, against a datasheet **ceiling** of <= 660 g. See [[vcu_datasheet_verified]].
**Not one component is `verified`.** 3.2.2 and 3.6.

# ✅ CIRPASS CITATION RULING, taken 2026-08-17: HYBRID

**Prose names the project. The parenthetical names the authors.** This keeps Chapter 2's accepted paired
contrast ("Adisorn et al. describe a recycler who receives product information. CIRPASS specifies a
recycler who also supplies it") and is APA-correct, and it dissolves the 2024a/b/c problem entirely
because all four works have different first authors.

| Deliverable | Parenthetical | Year |
|---|---|---|
| D2.1 Mapping of legal and voluntary requirements and screening of emerging DPP-related pilots | Wagner et al. | 2023 |
| D2.2 Exploring possible DPP use cases in battery, electronics and textile value chains | Wautelet & Ayed | 2024 |
| D2.3 Stakeholder consultation on key-data | Wagner et al. | 2024 |
| D5.1 DPP Prototypes | Bernier & Danash | 2024 |

⚠ **Wagner is first author on D2.1 (2023) and D2.3 (2024).** Different years, so no letter suffix needed,
but the two `Wagner et al.` entries must be distinguishable in the reference list by title.
⚠ **Five existing in-text citations in the .docx must be converted** from `(CIRPASS, 2024a/b, p. X)`:
`(CIRPASS, 2024a, p. 16)` and `(CIRPASS, 2024a, pp. 10, 16)` were D2.3 -> **Wagner et al. (2024)**;
`(CIRPASS, 2024b, p. 10)`, `(…, p. 20)`, `(…, p. 8)` were D5.1 -> **Bernier & Danash (2024)**.
`(CIRPASS-2, 2026)` is a different project and is unaffected.

⚠ `backend/data/vcu_001.json.bak_before_rename` exists (created 2026-08-19 12:18). **Delete before the
24–25 August compile.**

Related: [[cirpass_d22_table6]], [[cirpass_d21_requirements]], [[annex_vi_schema_gap]],
[[ch3_methodology_progress]], [[teardown_model_as_built]], [[jensen_2023_data_needs]],
[[dpp_data_model_cirpass]], [[vcu_datasheet_verified]]
