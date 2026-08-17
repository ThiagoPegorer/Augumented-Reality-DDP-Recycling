---
name: cirpass-d21-requirements
description: "[P] CIRPASS D2.1 (July 2023) read 2026-08-17. Contains the LEGAL SPINE of the thesis argument: its Table 6 names waste treatment operators as the designated data user for the location of dangerous substances, mandated by the WEEE Directive, delivered by manual."
type: reference
---

# [P] Read 2026-08-17 (Session 34)

Publisher's own APA citation, printed on page 1 of the TU Delft repository version:

> Wagner, E., Rukanova, B. D., Bernier, C., Wautelet, T., Ayed, A.-C., Boell, M., Gayko, J.,
> Schneider, A., Bendzuck, K., & von Dalwigk, I. (2023). D2.1 Mapping of legal and voluntary
> requirements and screening of emerging DPP-related pilots. CIRPASS Consortium.
> https://cirpassproject.eu/wp-content/uploads/2023/07/D2.1_July_2023.pdf

Version 2.0, 07/2023. 68 pages. D2.1 is 2023, not 2024, so it takes no year letter.

# THE FINDING. This is the legal spine of the thesis's own argument.

D2.1 Table 6, p. 38, "Overview of information requirements for the electronics sector structured by data
provider, target data user and format". Five columns: Where (legislation), What (requirement),
Who (data provider), To whom (data user), How (data format).

THE WEEE ROW, transcribed verbatim:

| Where | What | Who | To whom | How |
|---|---|---|---|---|
| WEEE Directive | Information on different materials and LOCATION OF dangerous substances and mixtures in EEE | Producer | WASTE TREATMENT | MANUAL, ELECTRONIC MEDIA |

THE ECODESIGN ROW also names the recycler:

| Where | What | Who | To whom | How |
|---|---|---|---|---|
| Ecodesign Directive | Product group specific substances (e.g., cadmium in displays) | Manufacturer, Importer, authorised representative | RECYCLER, WASTE TREATMENT | Datasheet, website, packaging, Label |

Supporting sentence, p. 38, verbatim:
> "The WEEE Directive applies the same idea to waste treatment facility operators who are provided with
> information on different materials and their location in the product to facilitate the separation of
> parts which include hazardous materials."

Why this matters more than anything else found on 2026-08-17. The thesis argues that the information
layer was legislated and the delivery layer never was. This table proves all four parts at once:
1. The information is LEGALLY MANDATED.
2. Its designated DATA USER is the waste treatment operator, which is the thesis's recycler.
3. The mandated content includes LOCATION, the field dpp_schema.json renders only as prose.
4. The mandated FORMAT is "Manual, electronic media", exactly the delivery problem the prototype attacks.

"location" now has THREE independent sources: Annex VI of Regulation (EU) 2026/1738
(annex_vi_schema_gap), CIRPASS D2.2 Table 6 (cirpass_d22_table6), and this one, which alone also names
the user and the current format.

# A PROVENANCE POINT FOR CHAPTER 2

Chapter 2 currently writes: "Its key-data deliverable assigns usage history, repair history and
end-of-life information to downstream data providers, a group it defines as consumers, repair and
reconditioning operators, refurbishment and remanufacturing operators, and waste operators including
collectors, sorters and recyclers (CIRPASS, 2024a, p. 16)."

That framework ORIGINATES in D2.1 Table 3, p. 21 (July 2023), headed "DPP information categories" and
structured by data provider. Its downstream row reads, verbatim: "consumer, repair and reconditioning
operators, refurbishment, remanufacturing, waste operators including collectors, sorters, recyclers, etc."
Citing the 2024 restatement is not wrong, but D2.1 is the origin and it is now on disk.

D2.1 p. 21 also states the reason for the choice: "disassembly information might be used by user,
repairer and recycler. To avoid confusion, this study thus uses a data provider perspective for
structuring information requirements."

# CITATION: settled 2026-08-17 as the HYBRID scheme

Prose names the project, the parenthetical names the authors.
D2.1 = Wagner et al. (2023) | D2.2 = Wautelet & Ayed (2024) | D2.3 = Wagner et al. (2024) |
D5.1 = Bernier & Danash (2024). Wagner is first author twice, different years, no letter needed.

Related: cirpass_d22_table6, annex_vi_schema_gap, jensen_2023_data_needs, ch3_methodology_progress,
eu_regulatory_scope, dpp_data_model_cirpass, table6_coverage_map
