# P2 MECHANICAL, whole document, find-and-replace
Task 1, the global half. Run 2026-08-28. One operation each, 164 pages at a time.
**Sev A** wrong or self-contradictory · **Sev B** an examiner marks it · **Sev C** cosmetic.
⚠ **P2-A01 must be the LAST action before the PDF export**, not the first.

## SEVERITY A

| ID | p. | Live text | Change to | Why |
|---|---|---|---|---|
| P2-A01 | 5, 6 | TOC prints `4 Results`, `VII.APPENDIX VII`, `VIII. Appendix VIII` | refresh the TOC field, and both list fields | the body headings read `4 RESULTS`, `VII. APPENDIX VII`, `VIII. APPENDIX VIII`. The field is stale |
| P2-A02 | 56 | `66,2 kWh` in the Table 4 result row | `66.2 kWh` | the row below prints `0.009 kW` with a period, and the prose two lines down prints `66.2` |
| P2-A03 | 142 | Appendix VIII: `0,0175` `0,018739` `6,54` and 30 more | periods throughout | backlog B1. ⚠ **do not touch `kg 1,4-DCB`**, that comma is the chemical name |
| P2-A04 | 74 | Table 8 at six decimals: Sc1 and Sc3 both `0.018739`, Sc2 and Sc4 both `0.018740` | eight decimals in the minerals row | the paragraph below separates all four. Backlog B2 |

## SEVERITY B

| ID | p. | Live text | Change to | Why |
|---|---|---|---|---|
| P2-B01 | throughout | `%` 140 times, `percent` 39 times, `per cent` once (p. 37) | **`%` everywhere** | Chapters 1 to 3 use the symbol, 4.1 spells it out, 4.1.3 uses both in one subsection. 39 replacements, all in 4.1, plus `86.6857 %` closing to `86.6857%` and `6-13 %` in Appendix IV |
| P2-B02 | 72, 74, 86 | `Ionizing radiation (human health)` in Table 7 · `Ionising radiation (human health)` in Table 18 | pick one. **`Ionising`** is the official EF 3.1 category name, so use it in both | the same category is spelled two ways in two tables |
| P2-B03 | throughout | `modelled` / `modelling` 9 times, `modeled` 3 times | **modeled / modeling** | |
| P2-B04 | 24, 30, 113 | `judgement` 3 times | `judgment` | |
| P2-B05 | 32, 38, 41 | `standardised` 3 times | `standardized` for two. ⚠ **the third is inside the quoted regulation text** *"unrestricted, standardised and non-discriminatory access"*. **Leave the quotation alone** | |
| P2-B06 | 37, 40 | `recognises` / `recognised` | `recognizes` / `recognized` | |
| P2-B07 | 35, 39 | `behaviour` / `behavioural` | `behavior` / `behavioral` | |
| P2-B08 | 25 | `miniaturisation` | `miniaturization` | |
| P2-B09 | 36, 37 | `decentralised` twice | `decentralized` | |
| P2-B10 | 41 | `organisation` | `organization` | |
| P2-B11 | 39 | `labelled` | `labeled` | |
| P2-B12 | 42 | `analyse` | `analyze` | |
| P2-B13 | 52 | `travelling` | `traveling` | |
| P2-B14 | 107 | `the tonnes of ore` | `the tons of ore` | Chapter 2 already prints "27 tons" and "twenty-three thousand tons" |
| P2-B15 | 124 | `Aluminium (alloy)` in the Appendix IV composition table | `Aluminum (alloy)` | ⚠ **the other 16 "aluminium" hits are ecoinvent dataset names**, `market for aluminium, primary, ingot`. **Never change those** |
| P2-B16 | 136 | `anonymised`, `anonymisation`, 7 times in Appendix VI | **LEAVE THEM.** This is the signed consent form | never edit a document participants signed |
| P2-B17 | throughout | 6 variants of the Source line: `own elaboration` 64 · `Google forms` 33 · `openLCA model – own elaboration` 8 · `openLCA model - own elaboration` 5 · `own elaboration tests` 3 · `OpenLCA model – own elaboration` 3 · `Own illustration` 1 | three forms only: `Source: own elaboration` · `Source: openLCA model, own elaboration` · `Source: Google Forms` | `own elaboration tests` is not a phrase. `OpenLCA` is wrong, the vendor writes `openLCA`, including in the 3.3.2 heading |
| P2-B18 | 74, 439 | `Table 9 – Stage 1 to 4...` · `Figure 40 – Prior Participant...` | hyphen | the other 121 captions use a hyphen |
| P2-B19 | 151 | `Figure 78- Comparing Conditions` | `Figure 78 - Comparing Conditions` | no space before the hyphen |
| P2-B20 | 76 | Table 10 caption, `each scenario**´**s gross result` | `each scenario's gross result` | acute accent, not an apostrophe. Table 17's caption already has the right one |
| P2-B21 | throughout | headings in three styles at one level | **sentence case throughout** | mixed cases: `2.5 …Industrial tasks`, `3.3.3 …and uncertainty`, `4.1.3 …Stage contributions`, `4.1.6 …ReCiPe 2016 Midpoint`, `5.2 …Teardown model`. Title Case: 1.1 to 1.4, 3.2.1 to 3.5, 4.1. Sentence case: 2.1 to 2.6, 4.1.1 to 4.3.3, 5.1, 5.3, 6.1 |
| P2-B22 | 15, 20 | `1.1 Context **&** Research Gap` · `1.3 Scope **&** Limitations` | `and`, to match `1.2 Objectives and Goals` | |
| P2-B23 | 74 | Table 9: `6.226E-8`, `1.806E-6`, `5.687E-4` beside `0.00008`, `0.00032` | one notation for the whole table | two notations in the same column |
| P2-B24 | 87 | `1.201 x 10^5` | `1.201 × 10⁵` with a real superscript, or `120,051 kg Cu eq` | the caret is typed literally |
| P2-B25 | throughout | 49 straight `"` against 13 curly · 63 straight `'` against 7 curly | one set | pasted text carried its own quote marks in. p. 32 has a straight opening quote and a curly closing quote in the same pair |
| P2-B26 | 155, 158 | `Https://Single-Market-Economy.Ec.Europa.Eu/...` and 2 more | lowercase the URLs | Word auto-capitalized them |
| P2-B27 | 28, 32 | `waste**(**He et al., 2024` · `requires alignment**(**Regulation` | space before the parenthesis | |
| P2-B28 | 35, 40, 41 | `Plociennik et al. (2024)selected` · `Kaarlela et al. (2025)use` · `Kühn et al. (2025)build` | space after the parenthesis | |
| P2-B29 | throughout | `End-of-Life` 7 · `end-of-life` 36 · `end of life` 8 | **`end-of-life`** before a noun, **`end of life`** as a noun phrase, never capitalized mid-sentence | |
| P2-B30 | throughout | `Augmented Reality` 46 · `augmented reality` 10 | pick one and apply it | the lowercase ones cluster in 2.5 and on p. 60 |
| P2-B31 | throughout | `Life Cycle Assessment` 5 · `life cycle assessment` 8 | lowercase in prose, capitals only in headings | |
| P2-B32 | 43 | `MS50.4` · `MS 50.4` · `datasheet` · `data sheet` | `MS 50.4` and `data sheet` | both pairs appear on the same page |
| P2-B33 | throughout | numbers as digits in 4.3 (`78 of the 90`, `26 s`) and spelled out in 5.3 (`seventy-eight of the ninety`, `twenty-six seconds`) | one rule | the same values, two styles, four pages apart |
| P2-B34 | 42, 72 | `four stages` for the work phases | `four phases` | it collides with the life cycle's five stages |
| P2-B35 | 15 to 21 | `Therefore` / `therefore` 11 times in 7 pages | cut to about four | |

## SEVERITY C

| ID | p. | Live text | Change to |
|---|---|---|---|
| P2-C01 | 68, 69 | `centimeters` in prose, `cm` in Table 6 | one form |
| P2-C02 | 29 | `1,513g` and `213g` | `1,513 g` and `213 g`, as `386 kg` and `2 g` already are |
| P2-C03 | 58 | `The 9g of reused components` | `The 9 g of reused components` |
| P2-C04 | 43 | `Source: Bosch Motorsport Data Sheet 234686731, 27 March 2026.` | drop the closing period, no other Source line has one |
| P2-C05 | 96 | `A dialogue reports that the session is stored` | `A dialog reports...` |
| P2-C06 | 98 | Table 19 column mixes `Innovation manager`, `Marketing Analyst`, `Strategic design`, `Electrical Engineer` | one capitalization |
| P2-C07 | 98 | `Marketing & Social Midia` | `Marketing and social media` |
| P2-C08 | 101 | Table 21 prints `TRUE` / `FALSE` | `Yes` / `No` |
| P2-C09 | 18 | `(PHEVs)` and `(BEVs)` | keep the terms, drop the abbreviations, each is used once |

## VERIFIED SOUND, do not re-check
- **Figures 1 to 86**, none missing, none duplicated, every front-list entry matched in the body.
- **Tables 1 to 37**, same check passed.
- **Equation 1** appears once in the body and once in the list.
- **Appendices I to IX**, roman numerals correct and in order.
- **The 4 em dashes are all inside published titles in the reference list.** Correct as they are.
