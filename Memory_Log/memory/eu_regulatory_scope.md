---
name: eu-regulatory-scope
description: Which EU instrument actually governs a vehicle control unit at end of life, verified article by article 2026-08-14. WEEE excludes vehicles, ELV 2000/53 covers components but its mandatory stripping list names no electronics, ESPR covers components and excludes vehicles only partially. Defence-critical.
type: reference
---

# Why this file exists

An examiner can ask "which law actually applies to your device, and does the ESPR even cover it?"
Until 2026-08-14 the thesis had no answer. Every claim below is quoted from the legal text and traced
to a page in the local index. **Do not paraphrase from memory; re-open the PDF.**

# 1. WEEE Directive 2012/19/EU — EXCLUDES VEHICLES

`CELEX_32012L0019_EN_TXT.pdf`

- **p. 6, Art. 2(4)(d)** `[P]`: from 15 August 2018 the Directive "shall not apply to… **means of
  transport for persons or goods**, excluding electric two-wheel vehicles which are not
  type-approved". Vehicle electronics are therefore absent from every WEEE collection statistic,
  including GEM's 22.3 %.
- **p. 8, Art. 7** `[P]`: annual collection rate = **65 % of average weight placed on the market over
  the three preceding years, or alternatively 85 % of WEEE generated**. **p. 3, recital 17** calls
  the two "broadly equivalent". Chapter 1 states only the 85 % basis; fix it.

# 2. Directive 2000/53/EC on end-of-life vehicles — COVERS COMPONENTS, BUT DOES NOT REQUIRE THEIR REMOVAL

`Directive_2000_53_EC_ELV.pdf`, obtained from Thiago 2026-08-14, indexed, 9 pages, committed to
`LITERATURE\`.

- **p. 3, Art. 3(1)** `[P]`: "This Directive shall cover **vehicles and end-of life vehicles,
  including their components and materials.**"
- **p. 3, Art. 2(13), THE DEFINITION THAT MATTERS MOST TO THIS THESIS** `[P]`:
  > "'dismantling information' means all information required for the correct and environmentally
  > sound treatment of end-of life vehicles. It shall be made available to authorised treatment
  > facilities by vehicle manufacturers and component producers **in the form of manuals or by means
  > of electronic media (e.g. CD-ROM, on-line services)**."

  The obligation to supply the information has existed since 2000. The **delivery channel** the law
  imagines is a manual, a CD-ROM or a website. Nothing about reaching the operator at the bench.
  **This is the single best sentence in the corpus for the "the application layer is empty"
  argument. Spend it in block 2.3, not earlier.**
- **p. 6, Art. 8(3)** `[P]`: producers must provide dismantling information for each type of new
  vehicle **within six months** of it being put on the market, and that information "shall identify…
  the different vehicle components and materials, and **the location of all hazardous substances in
  the vehicles**". Per-component location data has been an EU legal requirement since 2000. Pairs
  directly with CIRPASS D2.2 Table 6 #5 and #6.
- **p. 6, Art. 8(4)** `[P]`: component manufacturers must make available to authorised treatment
  facilities, "**as far as it is requested by these facilities**", information on dismantling,
  storage and testing of components that can be reused. Weaker duty: on request only.
- **p. 6, Art. 8(1)-(2)** `[P]`: producers must use component and material **coding standards** to
  facilitate identification of parts suitable for reuse and recovery; the Commission was to establish
  them by 21 October 2001.
- **p. 5, Art. 7(2)** `[P]`: reuse and recovery to reach **at least 85 % by average weight per
  vehicle and year** no later than 1 January 2006.
- **p. 8, ANNEX I, THE OMISSION.** Mandatory depollution (I.3): batteries, liquefied gas tanks,
  potentially explosive components such as airbags, fluids, and "removal, as far as feasible, of all
  components identified as containing mercury". Mandatory operations to promote recycling (I.4):
  "removal or catalysts", metal components containing **copper, aluminium and magnesium** where those
  metals would not otherwise be segregated in shredding, tyres and large plastic components, and
  "removal of glass".

  **No electronic component, no printed circuit board and no control unit appears anywhere on either
  list.** A control unit therefore reaches the shredder unless the treatment facility chooses to
  remove it. This is the concrete legal fact behind the whole thesis, and it explains why Restrepo
  et al.'s Swiss study exists.

# 3. ESPR (EU) 2024/1781 — COVERS COMPONENTS, EXCLUDES VEHICLES ONLY PARTIALLY

`OJ_L_202401781_EN_TXT.pdf`

- **p. 26, Art. 1(2)** `[P]`: "This Regulation applies to any physical goods that are placed on the
  market or put into service, **including components and intermediate products**. However, it does
  not apply to: … (h) **vehicles** as referred to in Article 2(1) of Regulation (EU) No 167/2013, in
  Article 2(1) of Regulation (EU) No 168/2013 and in Article 2(1) of Regulation (EU) 2018/858, **in
  respect of those product aspects for which requirements are set under sector-specific Union
  legislative acts applicable to those vehicles**."
- **p. 26, Art. 2(2)** `[P]`: "'component' means a product intended to be incorporated into another
  product."
- **p. 4, recital** `[P]`: the sector-specific acts named are **Directive 2000/53/EC**, Directive
  **2005/64/EC** (type-approval on reusability, recyclability and recoverability) and **Regulation
  (EU) 2018/858**.
- **p. 76, Art. 75(2)** `[P]`: by **19 July 2030** and every six years the Commission must evaluate
  the Regulation including "as regards… **the vehicles referred to in Article 1(2), point (h)**".
  The carve-out is itself under review.

# 4. The reading the thesis should state, and its limits

The exclusion in Art. 1(2)(h) is (i) about **vehicles**, not components in general, and (ii)
**aspect-limited**, biting only where vehicle-specific law already sets requirements. A VCU is a
component within Art. 2(2), and the ESPR covers components expressly.

**This is a legal reading, not a settled fact.** State it explicitly in block 2.3, quote Art. 1(2)
and Art. 2(2), and clear it with Saman Ghobadian before the defence. Objective 1 in section 1.2
promises a passport "structured on the data requirements set out under the European framework", so
the thesis is already exposed on this point.

# 5. The argument this unlocks for Chapter 2 and Chapter 5

1. The duty to supply dismantling information, **including the location of hazardous substances per
   component**, has existed in EU law since 2000 (Art. 8(3)).
2. The channel the law imagines is a manual, a CD-ROM or a website (Art. 2(13)).
3. Electronics are on **no** mandatory removal list (Annex I).
4. The ESPR adds a passport 24 years later, and its application to a vehicle component is contestable.

**So the information layer was legislated in 2000 and the application layer was never specified.**
A stronger version of Chapter 1's framing, sourced from the legal texts rather than asserted.

Not yet checked, do not assert without a source: whether the proposed ELV Regulation replacing
2000/53/EC introduces a circularity vehicle passport, and the content of Directive 2005/64/EC.

Related: literature_review_chapter2, literature, ch2_evidence_block22, dpp_data_model_cirpass,
introduction_progress
