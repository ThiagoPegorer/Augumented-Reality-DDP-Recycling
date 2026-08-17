---
name: lca-method-sources
description: The citable sources for EF 3.1 and ReCiPe 2016, both page-verified 2026-08-14, plus the chapter split — Lit Review describes the methods, Methodology justifies the choice. Neither EF 3.1 document has a Mendeley entry.
type: reference
---

# The two method sources, both indexed and page-verified `[P]` 2026-08-14

## ReCiPe 2016 — RESOLVED, was the top blocker

Thiago supplied the PDF on 2026-08-14. Now `LITERATURE\Huijbregts_etal_ReCiPe2016.pdf`, 10 pages,
in the index.

> Huijbregts, M. A. J., Steinmann, Z. J. N., Elshout, P. M. F., Stam, G., Verones, F., Vieira, M.,
> Zijp, M., Hollander, A., & van Zelm, R. ReCiPe2016: a harmonised life cycle impact assessment
> method at midpoint and endpoint level. *International Journal of Life Cycle Assessment*.
> **DOI 10.1007/s11367-016-1246-y** `[P]` p. 1.

Type: "COMMENTARY AND DISCUSSION ARTICLE" `[P]` p. 1. Received and accepted 29 November 2016;
copyright line reads Springer-Verlag Berlin Heidelberg **2016**; the journal issue is
**22, 138-147, 2017**.

**Year call to make and apply everywhere: 2016 or 2017.** Project notes have used "Huijbregts et al.
(2017)". APA takes the issue year, so **2017** is the safer choice, but it must match Mendeley and
every in-text citation. Pick one, record it here.

Abstract framing worth using in the method block, `[P]` p. 1: LCIA "translates emissions and resource
extractions into a limited number of environmental impact scores by means of so-called
characterisation factors", derived "at midpoint level and at endpoint level". That midpoint/endpoint
distinction is the concept the block has to install, because the thesis reports both.

## EF 3.1 — the primary document was never properly identified

`LITERATURE\JRC130796_01.pdf`, 57 pages, in the index since 2026-08-10 but recorded in memory only
as a filename. Its title page `[P]` p. 1:

> Andreasi Bassi, S., Biganzoli, F., Ferrara, N., Amadei, A., Valente, A., Sala, S., & Ardente, F.
> (2023). *Updated characterisation and normalisation factors for the Environmental Footprint 3.1
> method*. Publications Office of the European Union. **EUR 31414 EN**, ISSN 1831-9424.

Contents `[P]` p. 3: §2 Summary of the EF3.1 LCIA · §3 Characterisation factors · normalisation
factors. This is the document that authorises every EF 3.1 statement in the thesis.

Second EF source on disk: `The-EU-Product-Environmental-Footprint-Methodology.pdf`, 10 pages, the
EU PEF methodology brief.

# NEITHER EF 3.1 DOCUMENT HAS A MENDELEY ENTRY

Checked against the 2026-08-14 `export.bib`: zero hits for "Andreasi", "Sala", "Biganzoli", "JRC"
and "PEF". The only "Environmental Footprint" string in the whole file sits inside `Dalquist2004`'s
abstract. **The thesis's primary impact-assessment method currently cannot be cited.**

Add both to Mendeley as reports: the JRC EUR 31414 EN report, and the PEF methodology brief.
This blocks the LCA method block and Methodology equally.

# Which chapter says what (chapter_contracts §14, applied)

§14 gives "LCA method: EF 3.1, ReCiPe 2016, functional unit, system boundary" to **Methodology**,
with "Lit. Review: **the method literature only, not this study's setup**".

| Question | Chapter |
|---|---|
| What midpoint and endpoint characterisation are; what EF 3.1 and ReCiPe 2016 are; that EF 3.1 is the Commission's own recommended method; what comparable studies do; known critiques | **Lit Review, LCA method block** |
| **Why EF 3.1 was chosen for this study**, why ReCiPe 2016 was used as a cross-check, why ReCiPe normalisation and weighting were rejected, the functional unit, the system boundary, Sc1-Sc4 | **Methodology** |

The Methodology contract's question 3 is literally "Justify the choice of your method and why it is
the best suitable approach". That is where the justification lives, **not** in the Literature Review.
Writing it into the method block is a topic-ownership defect and also breaks "never write forward".

**Structural bonus of the block's position:** EF 3.1 is the European Commission's own recommended
method, and the regulation block follows it. The method the EU recommends, then the law the EU wrote.
The join is free; use it.

**Boundary hazard.** The method block sits immediately after the problem block, so sliding into this
study's own setup is easy. **It is written entirely about other people's studies and must not mention
the VCU at all.**

Related: literature, literature_review_chapter2, lca_scope_verified, lca_findings_for_writing,
sustainability_scope
