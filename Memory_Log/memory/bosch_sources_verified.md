---
name: bosch-sources-verified
description: "[P] BOTH Bosch documents read end to end on 2026-08-20, plus the MS 50.4P manual. Which number to cite and why the two 2450xxxxx numbers are filenames, the page for every fact the thesis uses, the three APA reference entries, and the AUDIT of what the sources contain that the passport record does not."
type: reference
---

# [P] Read in full 2026-08-20 (Session 37)

| Short name | Title on the document | Version and date | Pages | Filename number |
|---|---|---|---|---|
| Data sheet | Vehicle Control Unit MS 50.4 | 27 Mar 2026 | 4 | 245099915 |
| Manual | Vehicle Control Unit MS 50.4, Manual | Version 1.2, 02/02/2026 | 176 | 245102859 |
| P manual | Vehicle Control Unit VCU MS 50.4P, Manual | Version 1.0, 11/03/2021 | 136 | none |

# 🔴 WHICH NUMBER TO CITE

- Data sheet: 234686731. Printed in the page-4 footer: "234686731 | en, , 27. Mar 2026", above
  "(c) Bosch Engineering GmbH 2026 | Data subject to change without notice". 245099915 is only the filename.
- Manual: NO PRINTED NUMBER AT ALL. 245102859 appears nowhere inside the 176 pages; it is a filename,
  exactly like 245099915. Cite the manual by title and version.
- ⚠ 245099915 leaks into two project files: the payload's specifications.size_basis and the header of
  LCA_Analysis/Docs/BOM_v4.md. Do not let it reach the thesis.

# ✅ APA REFERENCE ENTRIES

Bosch Motorsport. (2021). Vehicle Control Unit VCU MS 50.4P: Manual (Version 1.0). Bosch Engineering GmbH.
Bosch Motorsport. (2026a). Vehicle Control Unit MS 50.4 (Data sheet 234686731). Bosch Engineering GmbH.
Bosch Motorsport. (2026b). Vehicle Control Unit MS 50.4: Manual (Version 1.2). Bosch Engineering GmbH.

The data sheet and the manual share 2026 and take letters, assigned alphabetically by title.

# 📄 PAGE MAP FOR EVERY FACT THE THESIS USES

| Fact | Where |
|---|---|
| Size 166 x 121 x 41 mm | data sheet p. 1; manual p. 8 |
| Weight <= 660 g (a CEILING) | data sheet p. 1; manual p. 8 |
| IP67, operating temperature -20 to 80 C, supply 5 to 18 V | data sheet p. 1; manual p. 8 |
| 3 motorsport connectors, 198 pins in total | data sheet p. 1; manual p. 8 |
| Max. vibration: Vibration profile 1 | data sheet p. 1; manual p. 8 |
| 2 x 667 MHz dual core; 2 x 4 GB partitions; 1,500 channels | data sheet p. 1; manual p. 8 |
| Internal measurements: ambient pressure, ECU temperature, three-axis acceleration plus roll/pitch/yaw | data sheet pp. 1-2; manual p. 9 |
| Outputs: 2 x 7.5 A high side, 4 x 2.2 A low side | data sheet p. 2; manual p. 9 |
| Sensor supplies: 5 x 12 V and 5 x switchable 5/12 V, 400 mA each | data sheet p. 2; manual p. 9 |
| Connectors LIFE (red) AS018-35PN, SENS-A (yellow) AS018-35PA, SENS-B (blue) AS018-35PB | data sheet p. 2; manual p. 10 |
| Communication: 3 Ethernet, 4 CAN, 1 LIN, 1 USB, 1 RS232 | data sheet p. 2; manual p. 10 |
| 🔴 "Maintenance Interval: 220 h or a maximum of two years", Installation Notes | DATA SHEET p. 2 |
| Legal Restrictions (export embargo list) | data sheet p. 2; manual p. 149 (24.1) |
| Order number F02U.V02.965-02, Ordering Information | data sheet p. 3 |
| Bosch Engineering GmbH, Robert-Bosch-Allee 1, 74232 Abstatt, Germany | data sheet p. 4 |
| 🔴 REACH Statement (24.2): lead monoxide 1317-36-8, lead 7439-92-1 | MANUAL p. 149 (also P manual p. 133) |
| 🔴 Disposal (25): "Do not dispose of this electronic device in your household waste." | MANUAL p. 174 |
| 🔴 EC/EU Declaration of Conformity (22.1) | P MANUAL p. 132 - NOT in the MS 50.4 manual |

# 🔴 THE DECLARATION OF CONFORMITY (P manual p. 132)

Dated 09 October 2020, signed at Abstatt. Declares BOTH products, so citing it for the MS 50.4 is valid:
  VCU MS 50.4, type F02U.V02.965-01, from date of manufacture 01.03.2020
  VCU MS 50.4P, type F02U.V02.966-01, from date of manufacture 01.03.2020
Of seven directives listed, ONLY Directive 2014/30/EU (EMC) is ticked. RoHS 2011/65/EU is on the form and
LEFT UNTICKED, which sources the record's rohs_applicable: false. Applied standard ECER10, measured
according to ECE-R10.06, rev. 6 : 2019. The three "further explanations" in the record's
compliance.declaration_notes are verbatim from this page.

⚠ TYPE NUMBER MISMATCH. The record's identity.type_number is F02U.V02.965-02, which is the data sheet's
ORDER number (p. 3). The declaration declares TYPE F02U.V02.965-01. Different fields, different suffixes.

# 🔴 THE REACH STATEMENT IS THE THESIS'S BEST EVIDENCE

Verbatim, manual p. 149:
  "According to the REACH regulations, any supplier of an article containing a substance of very high
   concern (SVHC) in a concentration above 0.1 % (w/w) has the duty to provide the recipient of the
   article with sufficient information to allow safe use of the article. Our product contains:"
Then a two-column table: SVHC Substance | CAS Number. Lead monoxide (lead oxide) 1317-36-8. Lead
7439-92-1. THAT IS ALL IT GIVES. No location. No concentration. No component.

So the location WEEE asks a recycler to have is MISSING AT SOURCE, not only in this model. The passport
goes further than its own source by adding a free-text location sentence, and still cannot reach the
component. This is paragraph 5 of section 3.2.2.

# 🔍 AUDIT: what the sources contain and the record does not

THE ONE REAL HOLE:
- 🔴 identity.economic_operator is NULL although the data sheet prints the full address on p. 4 and the
  declaration repeats it. Company information is a mandatory passport identifier. CIRPASS Table 6 does
  not list it, which is why the coverage map never caught it: the table note says product identification
  and company information sit in D2.1 instead.
- identity.brand is null although "Bosch Motorsport" is on every page.

IN THE SOURCE, NOT IN THE RECORD:
- 198 pins in total (in BOM v4.1's header, not in specifications)
- Connector part numbers AS018-35PN / PA / PB and their RED, YELLOW and BLUE coding. The record says only
  "Connectors 3x AS018-35"; the printed model makes all three grey. An identification aid lost twice.
- Max. vibration: Vibration profile 1. Absent entirely.
- The three internal sensors named individually. Only "MEMS sensors" inside a component name.
- Memory (2 x 4 GB) and 1,500 logger channels. Only inside a component name.
- The disposal instruction (manual p. 174). end_of_life.recycling_instructions is an empty array.
- THREE DOCUMENTS MARKED not_provided THAT EXIST: user manual (T6 #17), instructions for safe use
  (T6 #16), repair manual (T6 #12). The 176-page manual IS the user manual and its chapters 1 and 2 are
  the safety instructions. That is the difference between "no data exists" and "no data was linked".

IN THE RECORD, SUPPORTED BY NO SOURCE: the serial number VCU0001; the production date 2026-03-27, which
is the data sheet's own publication date; the WEEE category 5 assignment; every mass and material split;
the entire usage and repair history; all life cycle results. All labelled in the record.

DEFENSIBLE NULLS: power_consumption_w is null and the sources do not state the unit's own consumption.

# 🧹 FILE HOUSEKEEPING

- Operation-Manual_MS50.4P.pdf is HARDLINKED and unreadable by the tooling. A copy
  Operation-Manual_MS50.4P_FIXED.pdf was made 2026-08-20. Thiago must delete the original and the
  byte-identical "Operation-Manual_MS50.4P (1).pdf" by hand, then rename the fixed copy back.
- Two data sheet files exist, differing only in filename case. Delete one.
- The CIRPASS D2.2 original is gone and D2.1 is now present.

Related: [[vcu_datasheet_verified]], [[dpp_payload_verified]], [[ch3_methodology_progress]],
[[modelled_unit_composition]], [[table6_coverage_map]], [[cirpass_d22_table6]],
[[teardown_model_as_built]], [[study_build_version_finding]]
