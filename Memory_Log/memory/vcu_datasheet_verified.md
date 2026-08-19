---
name: vcu-datasheet-verified
description: "[P] The Bosch MS 50.4 data sheet read in full 2026-08-18, all four pages. Resolves the two-document-number question, and finds that the 660 g on which the entire BOM and LCA mass basis rests is a MAXIMUM, not a mass."
type: reference
---

# [P] Read 2026-08-18 (Session 35), all 4 pages

# THE DOCUMENT NUMBER QUESTION, resolved

The payload cites two numbers for the same document and neither is wrong:
- specifications.size_basis cites 245099915. This appears NOWHERE inside the PDF. It is the filename /
  Bosch asset ID.
- physical_unit.note cites 234686731. This is printed in the page-4 footer: "234686731 | en, , 27. Mar 2026".

RULING: cite 234686731, dated 27 March 2026, because that is what a reader can verify by opening the
document. Copyright line p. 4: "(c) Bosch Engineering GmbH 2026 | Data subject to change without notice".
The Operation Manual is a separate document, 245102859.

# THE 660 g IS A CEILING, NOT A MASS

Data sheet p. 1, Mechanical Data, verbatim: "Weight <= 660 g".
The payload records specifications.weight_g = 660 and the fifteen components sum to 660.1565 g
(computed 2026-08-18 from the live payload). THE WHOLE BOM WAS BUILT TO HIT AN UPPER BOUND. Every
absolute LCA figure inherits a ceiling treated as a point value.

This is verification rule A: a bound written in the grammatical form of a measurement. Ruling 2026-08-18:
state it in 3.2.1 as a parameter note (the mass budget was set to the datasheet maximum because no unit
was weighed) and in 3.6 as a limitation (absolute impact figures inherit the ceiling; the scenario
ranking does not depend on it).

# EVERY DATASHEET VALUE, verified against the payload

p. 1 Mechanical Data: Size 166 x 121 x 41 mm (matches) | Weight <= 660 g (see above) |
Protection classification IP67 (matches) | 3 motorsport connectors, 198 pins in total (matches) |
Max. vibration: Vibration profile 1 | Operating temperature internal -20 to 80 C (matches).
Electrical Data: Supply voltage 5 to 18 V (matches).
Processors: 667 MHz Dual Core for customer code, identical 667 MHz Dual Core for logging.
Logger: 1,500 channels; FULL_LOG_1 4 GB Partition 1; FULL_LOG_2 4 GB Partition 2; High Speed Logging
Package (5 microsecond sampling) optional; 600 kB/s using all features, >1,200 kB/s primary use case,
download up to 6.2 MB/s. LTE Ethernet telemetry, RS232 for GPS.
Communication: 3 Ethernet 100 Mbit, 4 CAN (+4 with Upgrade I/O Package).
Connectors p. 2: LIFE (red) AS018-35PN | SENS-A (yellow) AS018-35PA | SENS-B (blue) AS018-35PB.
Mating connectors AS618-35SN / -35SA / -35SB, NOT included.
Installation Notes p. 2: "Maintenance Interval: 220 h or a maximum of two years."
Ordering p. 3: Vehicle Control Unit MS 50.4, order number F02U.V02.965-02 (matches
identity.type_number). Accessories: Opening tool for shellsize 18, F02U.V01.394-01.
p. 2 "Legal Restrictions" is EXPORT CONTROL (blocked destinations), NOT end-of-life or disassembly law.
Do not cite it for regulatory context. The EU Declaration of Conformity the payload references (Bosch
Engineering GmbH, 09 Oct 2020) sits in the Operation Manual pp. 132-133.
p. 4 carries a yaw rate sensor dimension diagram (20, 86.6, 86.6, 61) and regional contacts.

# FOR THE APPENDIX EXTRACT TABLE

The agreed appendix artefact is an author-made specification extract, not a reproduction. Values above
are the candidate rows, each cited to Data Sheet 234686731 (27 Mar 2026).
WRITE "<= 660 g", NEVER "660 g".

Related: dpp_payload_verified, modelled_unit_composition, vcu_bosch_ms504, ch3_methodology_progress,
table6_coverage_map, lca_scope_verified, voice_and_verification_rules
