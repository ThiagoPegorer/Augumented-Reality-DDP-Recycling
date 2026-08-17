---
name: ch1-verbatim-facts
description: "[P] Passages read VERBATIM from Master_Thesis_ThiagoPegorer_100003505.docx on 2026-08-17. What Chapter 1's Scope & Limitations actually says, which corrected two wrong beliefs carried from the signed proposal."
type: reference
---

# [P] Read directly from the .docx on 2026-08-17 (Session 34)

Why this file exists. Section 3.1 was about to be drafted with two claims taken from the signed proposal
that the written thesis contradicts. Project memory held the proposal's wording; the document had moved
on. THE DOCUMENT WINS. Check it before writing about Chapter 1's content.

# TWO CORRECTIONS TO BELIEFS MEMORY WAS CARRYING

### 1. Participants are NOT "students or technically-minded individuals"
That phrase comes from the signed proposal and is SUPERSEDED. The thesis says, verbatim:
> "The user test was run with participants of mixed background: people experienced in electronics,
> people experienced in Augmented Reality, and people meeting both the device and the technology for the
> first time."

### 2. There is NO "Stage 3 was cut" narrative in the thesis
The proposal described a third stage taking the prototype to professional recycling centres. The written
thesis never mentions it. It makes a stronger argument instead, verbatim:
> "Professional dismantlers were not available. This is a deliberate boundary rather than an omission,
> because the thesis argues that the obstacle at end-of-life is missing product knowledge, and a
> participant without product knowledge is exactly the condition under test."

NEVER reframe that as a dropped stage. Doing so contradicts the chapter and discards the argument.
A later chapter may point at it in one sentence, never re-explain it. Chapter 1 owns it.

# TERMINOLOGY COLLISION, open as of 2026-08-17

Chapter 1 calls the study object a "3D-printed replica". The accepted Methodology opener calls it a
"printed teardown artifact". One object, two names, two chapters. DECIDE AND MAKE IT UNIFORM.
Chapter 1's wording is already pasted, and "replica" fits its own caveat.

# OTHER VERBATIM SCOPE PASSAGES, useful and reusable

On the replica:
> "Participants worked on a 3D-printed replica rather than an original unit, since a production Vehicle
> Control Unit was too expensive to acquire for repeated disassembly. The replica reproduces the geometry
> and the assembly sequence, and it does not reproduce the glued joints, the conformal coating, or the
> fastener behavior of the real device. The test therefore measures how well the interface communicates a
> disassembly procedure, and not how physically hard the work is."

On the LCA:
> "The Life Cycle Assessment (LCA) is modelled, not measured. It covers the full life cycle of one unit,
> from raw material extraction to end-of-life, with a functional unit of one Vehicle Control Unit
> operated in a battery-electric vehicle in Germany for fifteen years. Four end-of-life scenarios are
> compared: Scenario 1, no recycling, where the device is processed through landfill and incineration;
> Scenario 2, current bulk shredding practice; Scenario 3, guided manual dismantling; and Scenario 4,
> guided dismantling with component reuse. Background data comes from ecoinvent, and the foreground
> inventory is estimated from the device itself, since the author is not the manufacturer and holds no
> production records. No primary process data was collected from a recycling operator. Therefore, the two
> guided scenarios describe what dismantling would save according to published recovery studies. They are
> not measurements taken from the prototype, and the user test does not feed them."

On data architecture, out of scope:
> "Data architecture is out of scope: this thesis does not address blockchain, access control, or the
> security methods needed to store and share passport data at industrial scale. The purpose is to show
> that passport data can be used at end-of-life, not to specify how it should be stored."

On the single interaction channel:
> "The prototype implements one interaction channel. Guidance is delivered through Augmented Reality
> alone. There is no voice command layer and no artificial intelligence assistant... The results
> therefore describe one design of Augmented Reality guidance on one head-mounted display, and they do
> not generalize across devices or interaction models."

# CHAPTER HEADINGS, as they stand in the document

1 INTRODUCTION | 1.1 Context & Research Gap | 1.1.1 Electrification in the automotive industry |
1.2 Objectives and Goals | 1.3 Scope & Limitations | 1.4 Research Questions
2 LITERATURE REVIEW | 2.1 The recycling rate problem | 2.2 Quantifying the environmental consequences |
2.3 From recovery targets to information duties | 2.4 The Digital Product Passport as data model |
2.5 Augmented Reality to assist Industrial tasks | 2.6 Literature synthesis
3 METHODOLOGY | 3.1 Research design, analytic methodology employed & reason for choice
4 FINDINGS | 5 DISCUSSION OF FINDINGS | 6 CONCLUSION

Note "1.1 Context & Research Gap", not "Subject Relevance". Memory elsewhere used the template's wording
rather than the document's.

# SIX DEFECTS FOUND IN SECTION 2.4 ON 2026-08-17, all reported to Thiago and fixed by him

1. "and no of the author s here defends it" -> "none of the authors here defends it"
2. "Abdel-Aty et al. (2025) implement t he tracking system assigns a Radio Frequency Identification
   (RFID) , to each gearbox..." -> the sentence is broken in the middle
3. "A passport reaches successfully its target groups, only if it is ensured that..." -> word order and
   a stray comma
4. "the information is used no t directly in the physical model" -> "not" is split
5. "for remanufacturers and refurbishes" -> "refurbishers"
6. In 2.3: "Alcoceba-Pascual et al. (2025) showed , on section 2.2 , that..." -> "in section 2.2",
   spaces before commas, and it cites a section by number which the new voice rule forbids

# SPELLING EVIDENCE FROM THIS PASS

"modelled" (British, double L) and "generalize" (American) appear in the SAME Scope paragraph.
The document is genuinely mixed; see ch3_methodology_progress for the full conversion list.

Related: ch3_methodology_progress, registered_research_design, introduction_progress,
research_questions_final, lca_scope_verified, voice_and_verification_rules
