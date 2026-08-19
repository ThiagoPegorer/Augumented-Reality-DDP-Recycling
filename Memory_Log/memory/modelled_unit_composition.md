---
name: modelled-unit-composition
description: The ruling of 2026-08-18 that the modelled unit is a COMPOSITE, hardware parameters from a motorsport reference product and duty cycle from passenger-car assumptions. Says which datasheet values transfer to the BOM and which do not, where each piece is stated, and flags the one plausible claim that has no source.
type: project
---

# THE COMPOSITE, agreed 2026-08-18 (Session 35)

Thiago: "the main goal of use this VCU is to collect enough data from a BOM to build a LCA model and to
take similiar dimensions to build a physical prototype." Accepted and sharpened.

THE MODELLED UNIT IS A COMPOSITE. Say so plainly; it is stronger than calling it "representative".

| | source | transfers? |
|---|---|---|
| Hardware parameters | Bosch MS 50.4 data sheet 234686731 | YES. Size 166 x 121 x 41 mm, mass ceiling, aluminium housing, 3 connectors / 198 pins, two 667 MHz dual-core processors, 2 x 4 GB logger memory. These build the BOM and the replica geometry |
| Duty cycle | passenger-car assumptions made by the author | NO, and must not be taken from the sheet. 15-year service life, 225,000 km, 5,625 powered hours, 11,250 ignition cycles, the thermal histogram, Coffin-Manson fatigue accumulation |
| Service regime | the reference product's motorsport regime | NO. "Maintenance Interval: 220 h or a maximum of two years" is a race-car interval and does not describe a city car |

# WHY THE MAINTENANCE NULL IS NOT A DEFECT

service.maintenance_interval is null although the data sheet supplies 220 h / two years. THAT IS CORRECT
BEHAVIOUR, NOT AN OMISSION. The interval belongs to the reference product's service regime, not to the
modelled unit. Write it that way in 3.2.2: the record omits it because it does not transfer.

# WHERE EACH PIECE IS STATED, keeping the three-limitations rule intact

- 3.2.1 states the composition: hardware from the reference product, duty cycle assumed, mass budget set
  to the datasheet MAXIMUM because no unit was weighed.
- 3.3 owns the use-phase assumptions themselves, since that is where the inventory is specified.
- 3.6 takes ONE entry covering both the mass ceiling and the motorsport origin, because they are the same
  defect in two forms: a model built from a single reference product cannot tell you how representative
  that product is. Inherent to the method, which is what 3.6 is for.
- 1.3 already carries the choice: the MS 50.4 was used because no series-production VCU datasheet could
  be obtained. Do not restate that reasoning in Chapter 3.

# ONE CLAIM WITH NO SOURCE. DO NOT WRITE IT.

[Guessing] A motorsport control unit is plausibly heavier and higher-specified than a mass-market
passenger VCU, which would mean the BOM overstates a typical unit. Combined with the <= 660 g ceiling
that would be two independent biases pushing the same direction, upward.
THERE IS NO SOURCE FOR IT, and Thiago ruled on 2026-08-18 that there is no time to find one and the BOM
is NOT being changed. State the direction as unquantified or omit it entirely. Asserting it unsourced
turns a strength into a fabrication.

# THE DEFENCE THAT DOES EXIST, and it is enough

Thiago asked for "a defense on top what was developed". It already exists in the build:
- Every value carries its basis: verified, assumed / modelled, or not provided. Never a blank, never a
  plausible-looking guess. The rule was designed 2026-07-30 and applied before the study.
- The framework already states that absolute footprints carry roughly +/- 20 % and only the scenario
  ranking is robust.
A model whose every assumption is labelled at field level is more defensible than one claiming an
accuracy it cannot demonstrate. The assumed BOM constrains a claim the thesis is not making.
BOUNDARY: Methodology STATES, the Discussion DEFENDS. Putting the defence in Chapter 3 turns a
specification into an apology, which reads weaker than the specification alone.

Related: vcu_datasheet_verified, ch3_methodology_progress, dpp_payload_verified, lca_scope_verified,
ch1_verbatim_facts, research_questions_final, vcu_bosch_ms504
