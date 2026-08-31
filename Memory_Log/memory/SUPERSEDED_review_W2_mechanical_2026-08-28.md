# W2, MECHANICAL CONSISTENCY SWEEP, whole document
Run 2026-08-28 on `..._to_review_version.pdf`, 164 pages. Every count below is machine-counted.

## M1. THE TABLE OF CONTENTS IS STALE. Refresh before anything else. SEV 1
Three entries disagree with the body heading they point to:

| TOC prints | Body heading | |
|---|---|---|
| `4 Results` | `4 RESULTS` | |
| `VII.APPENDIX VII` | `VII. APPENDIX VII` | missing space |
| `VIII. Appendix VIII` | `VIII. APPENDIX VIII` | |

The headings were fixed after the TOC was last generated. **Refresh the TOC, the LIST OF FIGURES
and the LIST OF TABLES as the last action before the PDF export**, and re-run this sweep after,
because some counts below are taken from those lists.

## M2. "%" vs "percent". SEV 1, most visible item in the sweep
**140 uses of `%` against 39 of "percent" spelled out**, and the split runs by chapter.
Chapters 1, 2 and 3 use the symbol in prose (`22.3%`, `42.8%`, `65%`, `85%`, `95%`, `90%`, `98%`,
`96%`, `94%`, `100%`, `80%`). Section 4.1 spells it out (`72.45 percent`, `6.67 percent`,
`0.2404 percent`, `62.2 to 66.0 percent`). Section 4.1.3 then uses `%` again inside the same
subsection, in the Table 9 column head `Share (%)`, and 4.1.3's prose uses a third form with a
space, `86.6857 %`.

**Recommendation: use `%` everywhere.** 140 against 39, and Chapter 4 is the number-dense chapter
where a spelled-out unit reads worst. That is 39 replacements, all inside 4.1, plus closing the
space in `86.6857 %`. Appendix table `6-13 %` closes too.

## M3. DECIMAL SEPARATORS. SEV 1
34 comma-decimals remain. Two locations, both real:

| Where | Live | Fix |
|---|---|---|
| Equation 1 symbol table, section 3.3 | `66,2 kWh` | `66.2 kWh`. The row directly below prints `0.009 kW` with a period, in the same table |
| Appendix VIII, whole table | `0,0175` `0,018739` `6,54` and 30 more | periods throughout. Backlog B1 |

⚠ **Do not touch `kg 1,4-DCB`** in Table 18 and the ReCiPe text. That comma is part of the chemical
name 1,4-dichlorobenzene, not a decimal.
⚠ `7,990 kg CO2 eq` in 4.1.6 is a thousands separator. Once Appendix VIII is fixed it stops being
ambiguous, but consider `7990` for safety.

## M4. SCIENTIFIC NOTATION, three forms. SEV 2
| Live | Where |
|---|---|
| `6.226E-8`, `1.806E-6`, `5.687E-4` | Table 9 |
| `1.201 x 10^5` | 4.1.6 prose, caret typed literally |
| `0.00008`, `0.00032` | Table 9, same columns as the E-notation |

Pick one. Suggested: Table 9 goes to a fixed number of significant figures with no E-notation, and
`1.201 x 10^5` becomes `1.201 × 10⁵` with a real superscript, or `120,051 kg Cu eq` written out.

## M5. TABLE 8 PRECISION. SEV 1. Backlog B2, still live
Table 8 prints six decimals, so Sc1 and Sc3 both read `0.018739` and Sc2 and Sc4 both read
`0.018740`. The paragraph immediately beneath prints eight decimals and separates all four
(`0.01873912`, `0.01873964`, `0.01873949`, `0.01873952`). **The table contradicts its own paragraph.**
Take Table 8 to eight decimals in the minerals row, or to seven significant figures throughout.

## M6. EN DASH. SEV 2
47 en dashes. Three groups:

| Group | Count | Action |
|---|---|---|
| `Table 9 – Stage 1 to 4...` and `Figure 40 – Prior Participant...` | 2 | **hyphen.** The other 121 captions use a hyphen |
| `Source: openLCA model – own elaboration` | 11 | see M7 |
| `Step 1 – Open the housing` etc., Appendix V manual | rest | consistent inside the appendix. Leave, or align with the captions |

`Figure 78- Comparing Conditions, question 2` has no space before its hyphen. All others do.

**Em dashes: 4, all inside published titles in the reference list.** Correct as they are.

## M7. THE SOURCE LINE, six variants of the same sentence. SEV 2
| Count | Live |
|---|---|
| 64 | `Source: own elaboration` |
| 33 | `Source: Google forms` |
| 8 | `Source: openLCA model – own elaboration` |
| 5 | `Source: openLCA model - own elaboration` |
| 3 | `Source: own elaboration tests` |
| 3 | `Source: OpenLCA model – own elaboration` |
| 1 | `Source: Own illustration` |
| 1 | `Source: Own elaboration. Categories of origin from...` |

Standardize on: `Source: own elaboration` · `Source: openLCA model, own elaboration` ·
`Source: Google Forms` (capital F, it is a product name). `Source: own elaboration tests` is not a
phrase, replace it. `openLCA` is the vendor's own capitalization, so `OpenLCA` is wrong in three
places and in the heading of 3.3.2.

## M8. HEADING CAPITALIZATION. SEV 2
Three styles are in use at the same level.

| Title Case | sentence case | mixed |
|---|---|---|
| 1.1 Context & Research Gap · 1.2 Objectives and Goals · 1.3 Scope & Limitations · 1.4 Research Questions · 3.2.1 The Product · 3.2.2 The Passport Data Model · 3.3 Life Cycle Assessment · 3.3.1 System Boundary and Life Cycle Stages · 3.4 Augmented Reality Model · 3.5 Prototype Test · 4.1 Life Cycle Assessment Results | 2.1 to 2.6 · 3.1 Research design · 4.1.1, 4.1.2, 4.1.4, 4.1.5 · 4.3.1 to 4.3.3 · 5.1 · 5.3 · 6.1 | 2.5 Augmented Reality to assist **I**ndustrial tasks · 3.3.3 Impact Assessment and **u**ncertainty · 4.1.3 Life Cycle Stage **c**ontributions · 4.1.6 Cross-check with ReCiPe 2016 **M**idpoint · 5.2 Digital Product Passport and Teardown **m**odel |

**Recommendation: sentence case throughout**, since Chapter 2 and most of Chapter 4 already use it
and it is the SRH template's own style. Also settle `&` against `and`: 1.1 and 1.3 use `&`, 1.2 uses
`and`.

Heading grammar, separate from capitalization:
| Live | Fix |
|---|---|
| 3.2 The Product and **it** Digital Product Passport | **its** (backlog C, still live) |
| 4.3 Voluntary **participants tests** | Voluntary **participant** tests |
| 5.3 Voluntary **participants tests** discussion | Voluntary **participant** tests discussion |

## M9. US ENGLISH. SEV 2
| Live | Count | Fix |
|---|---|---|
| modelled / modelling | 9 | modeled / modeling. Three "modeled" already present, so the document is split |
| judgement | 3 | judgment |
| standardised | 2 of 3 | standardized. ⚠ the third is inside the quoted regulation text *"unrestricted, standardised and non-discriminatory access"*. **Leave the quotation alone.** |
| recognises / recognised | 2 | recognizes / recognized |
| behaviour / behavioural | 2 | behavior / behavioral |
| analyse | 1 | analyze |
| organisation | 1 | organization |
| labelled | 1 | labeled |
| characteristics | 1 | correct already, no change |
| Centre | 1 | ⚠ **Joint Research Centre** is a proper noun. **Leave it.** |
| catalogue | 1 | catalog, in the BOM table |

⚠ `Ionising radiation (human health)` in Table 18 is the official EF 3.1 category name. Leave it.

## M10. PUNCTUATION AND TYPOGRAPHY. SEV 2 and 3
| Where | Live | Fix |
|---|---|---|
| Table 8, header column | `Resource use**.** minerals and metals` · `Eutrophication**.** freshwater` | commas. Table 9 already uses commas for the same two names |
| Table 10 caption | `each scenario**´**s gross result` | apostrophe, not an acute accent. Table 17's caption already uses the right one |
| 4.1.6 | `(Huijbregts et al., 2017) **.**This cross-check` | `(Huijbregts et al., 2017). This cross-check` |
| 2.2 | `waste**(**He et al., 2024, pp. 14 to 15)` | space before the parenthesis |
| 2.3 | `requires alignment**(**Regulation (EU) 2026/1738` | space before the parenthesis |
| References, 3 entries | `Https://Single-Market-Economy.Ec.Europa.Eu/...` · `Https://Www.Re-Flow.Io/...` | lowercase the URLs. Word auto-capitalized them |
| throughout | 49 straight `"` against 13 curly `“ ”`; 63 straight `'` against 7 curly `’` | turn on smart quotes and reapply, or convert all to straight |
| Figure 19 caption | `EF3.1 method **apply** to the four different gross LCA scenarios` | `EF 3.1 method **applied** to the four gross LCA scenarios` |
| captions vs prose | `EF3.1` in captions, `EF 3.1` in prose | `EF 3.1` everywhere |
| Table 8 caption | `across all **4** LCA scenarios` | `across all **four** LCA scenarios` |
| Figure 22 caption | `Gross burden **x** avoided impact` | `Gross burden against avoided impact` |

## M11. NUMBERING, verified sound
- **Figures 1 to 86, none missing, none duplicated.** Every caption in the LIST OF FIGURES has a
  matching caption in the body.
- **Tables 1 to 37, none missing, none duplicated.** Same check passed.
- **Equation 1** appears once in the body and once in the list.
- **Appendices I to IX**, roman numerals correct and in order in the body.

## M12. SEV 3, one line each
- `Voluntary participants tests` also appears as running text, not only as a heading.
- `6-13 %` in the Appendix IV composition table has a space before `%`, `9%` in the same table does not.
- The Table 9 column heads print `Share (%)` while the same quantity is spelled out in prose four
  lines above.
