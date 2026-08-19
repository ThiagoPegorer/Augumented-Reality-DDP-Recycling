# Session 35, 2026-08-18: the Table 6 coverage re-derived, the product's three roles settled

## What happened

No new thesis prose was accepted. One paragraph was drafted (the 3.2 intro) and is awaiting Thiago.
The session's value was verification and rulings, and it removed the last unknowns in front of 3.2.

## The re-derivation, the session's main output

All 22 CIRPASS Table 6 attributes were scored against the FROZEN payload backend/data/vcu_001.json
instead of the RBv2.0 map from 30 July. Full table in table6_coverage_map.md.
Result: of the 13 mandatory attributes, 2 do not apply to this product group with the reason recorded in
the record itself; of the 11 that apply, 5 covered, 5 partial, 1 declared absent. Of the 9 voluntary,
4 covered, 2 partial, 3 declared absent.
Thiago's objection of the previous day was correct: "4 of 13" was badly stale, and the improvement is
real rather than a re-count. Mandatory attributes outright absent went from 4 to 1.

## The Bosch datasheet, read in full

- The two document numbers are not a contradiction. 234686731 is printed in the page-4 footer with the
  date 27 Mar 2026; 245099915 is only the filename. CITE 234686731.
- "Weight <= 660 g" is a MAXIMUM. The payload records 660 and the BOM sums to 660.1565 g, so the entire
  mass basis of the LCA is a ceiling treated as a point value. Stated in 3.2.1 and 3.6.
- Every other datasheet value checks out against the payload.

## Rulings taken

- THE PRODUCT HAS THREE ROLES: reference product (Bosch MS 50.4), modelled unit (a representative
  automotive control unit parameterised from it), study object (the 3D-printed replica).
- The modelled unit is a COMPOSITE: hardware parameters from a motorsport reference product, duty cycle
  from passenger-car assumptions. The maintenance interval does not transfer, which is why that field is
  null by design rather than by omission.
- The BOM is NOT being changed. Its defence is the field-level honest-labelling rule.
- Identity fields are illustrative; the build stays frozen and 3.2.2 says so plainly.
- METHODOLOGY STATES, THE DISCUSSION DEFENDS.

## Two defects found inside the frozen payload

1. Two schema_version values: top-level 0.19, dpp_meta 0.13.
2. All 15 components carry basis "assumed". Not one is verified.

## Where the next session starts

Confirm the 3.2 intro, then draft 3.2.1. Everything it needs is verified.
THE ONLY REAL BLOCKER REMAINS THE STUDY FACTS for 3.5, open since 2026-08-17.

## Files written to this folder this session

MEMORY.md, ch3_methodology_progress.md, thesis-schedule.md, table6_coverage_map.md,
vcu_datasheet_verified.md, modelled_unit_composition.md
