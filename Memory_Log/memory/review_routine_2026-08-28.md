---
name: review-routine-2026-08-28
description: The chapter-by-chapter review routine for the final revision pass on Master_Thesis_ThiagoPegorer_100003505. Defines seven check classes, the controlled vocabulary, the source-of-truth files for every number, the output format, and the three passes that CANNOT be run chapter by chapter.
type: reference
---

# REVIEW ROUTINE, final revision pass
Built 2026-08-28. Target: `MAIN PAPER/Master_Thesis_ThiagoPegorer_100003505.docx`,
read through `..._to_review_version.pdf` (164 pp., PDF page = printed page).

## 0. BEFORE ANYTHING

1. Read `thesis_review_backlog_2026-08-26.md` first. **It is still live.** A spot check on
   2026-08-28 found 13 of the 15 group-C grammar items unfixed. Do not re-report an item that is
   already there under a new number: carry the backlog ID (A1, C7, ...) into the new table.
2. Read `voice_and_verification_rules.md`. Fifteen voice rules, five verification rules.
3. Never edit the .docx. Output is a findings table. Thiago applies every change himself.

## 1. THE SEVEN CHECK CLASSES

| # | Class | What counts as a finding |
|---|---|---|
| G | Grammar, US English | Agreement, tense, article, preposition, spelling. **American spelling** (analyze, modeled, organize, behavior). |
| S | Broken or incomplete sentence | A sentence with no verb, a dangling clause, a stub ("The trend across the scenarios is."), a doubled period, a sentence that ends where a clause was cut. |
| T | Terminology mismatch | A defined object called by a word that is not its defined word. See section 2. |
| X | Internal contradiction | Two statements in the thesis that cannot both be true. **Chapter-local only in this pass**; cross-chapter is pass W1. |
| D | Data mismatch | A number in prose, a table or a figure caption that disagrees with the source file. See section 3. |
| C | Citation defect | Uncited claim, wrong page locator, a source cited for something it does not say, a review cited for a primary measurement. |
| F | Formatting | Decimal separator, significant figures, en dash vs hyphen, % vs percent, heading capitalization, caption wording, figure/table numbering. |

Severity: **1** blocks submission (false claim, contradiction, wrong number) · **2** an examiner
would mark it (broken sentence, wrong term, missing citation) · **3** cosmetic.

## 2. CONTROLLED VOCABULARY. A word off this list is a class-T finding.

| Object | The ONLY correct word | Never |
|---|---|---|
| The 3D printed object | **teardown model** | replica, mock-up, copy, model of the VCU |
| Its relation to the Bosch unit | **reference product / reference unit** | original, real product |
| Geometry claim | reproduces the **part breakdown and assembly sequence, at a larger size** | reproduces the geometry, matches the envelope, 1:1 |
| The four modelled end-of-life constructs | **scenario** (Sc1 to Sc4) | route. ⚠ NEVER find-and-replace: the 490 km transport **route**, the material **route** in ch.2, "route" as a verb, and the main research question are all legitimate |
| The application | **ReBuilt**, RBv2.1.1 | the app, the tool, the system (loosely) |
| The two AR functions | **digital model exploration** · **guided disassembly mode** | free view, tutorial, walkthrough |
| The impact halves | **gross burden** · **avoided impact** | net (abolished by the LCA supervisor), saving, benefit |
| An image | **figure** | never use "figure" to mean a number. Use number, value, share, rate |
| Participants | **P01 to P09** | names, roles, any identifying description |
| Instruments measured | completion time · errors · perceived usability | engagement, understanding, satisfaction (**not instrumented**) |

**The verb pattern, caught nine times.** Any sentence saying the application or dismantling
*increases / improves / validates* recycling rates, recovery, efficiency or understanding is a
class-1 finding. Write the obstacle, never the gain, never a causal ranking.

**The gross-burden caveat.** Dismantling does NOT reduce the unit's environmental impact. The gross
burden is identical across all four scenarios; only the avoided primary production rises. Any
sentence implying otherwise is class-1.

**The closing interview.** Material and recovery information **did not help the task, 8 of 9**. Any
claim that the application increased understanding or transparency contradicts his own data.

## 3. SOURCE OF TRUTH FOR EVERY NUMBER

Paths relative to `C:\Claude\Projects\AR_DPP\`.

| Number type | File |
|---|---|
| EF 3.1 characterization, all categories | `LCA_Analysis/Outputs/3_impact_assessment/impact_EF31.csv` |
| Category screening / 80 % threshold | `.../impact_screening.csv` |
| Life cycle stage contributions | `.../impact_stage_contributions.csv` |
| ReCiPe 2016 midpoint cross-check | `.../impact_ReCiPe_mid.csv` + `LCA_Notebook/Excel/recipe_cross_check.xlsx` |
| Scenario results, gross and avoided | `LCA_Analysis/Outputs/2_eol_scenarios/scenarios_results.csv`, `sc1_sc2_results.csv` |
| Monte Carlo bands, medians, percentiles | `LCA_Analysis/Outputs/4_monte_carlo/mc_summary.csv`, `mc_raw_*.csv` |
| Every published LCA table | `LCA_Analysis/LCA_Notebook/Excel/paper_tables.xlsx` |
| Bill of materials, masses | `LCA_Analysis/Docs/BOM_v4.md` (v4.1, doc 234686731) + `VCU_BOM_v4.xlsx` |
| Study raw data | `Memory_Log/memory/ARDPP_study_data.xlsx` (**raw only**) |
| Study derived values, all tables and figures | `Memory_Log/memory/ARDPP_study_analysis.ipynb` → `ARDPP_study_results.xlsx` |
| Verified prose-level records | `lca_results_verified_ch4.md` · `recipe_cross_check_verified.md` · `study_results_verified.md` · `modelled_unit_composition.md` |
| The prototype, what it actually does | `Memory_Log/RBv2_1_1_ELEMENT_INVENTORY.md` · `rbv2_1_1_ar_system_verified.md` |
| The teardown model as built | `teardown_model_as_built.md` (200 x 150 x 60 mm vs the unit's 166 x 121 x 41 mm) |
| Regulation dates | `eu_regulatory_scope.md`. Regulation (EU) 2026/1738, p. 29 |

**Rule D-1.** A value the source does not print is a **derived** value. Either print the source's
number or name the derivation.
**Rule D-2.** Read the CSV, never a memory note, when a number is challenged.
**Rule D-3.** Check the .docx XML for `MENDELEY_CITATION_v3_` before ever claiming a citation is
missing. python-docx cannot see field citations.

## 4. PER-CHAPTER PROCEDURE

Chapter map, PDF page = printed page:

| Block | Pages | Run after |
|---|---|---|
| 1 Introduction | 15 to 21 | first |
| 2 Literature Review | 22 to 41 | |
| 3 Methodology | 42 to 70 | |
| 4 Results | 71 to 105 | ⚠ heaviest D load |
| 5 Discussion | 106 to 110 | |
| 6 Conclusion | 111 to 113 | |
| Appendices I to IX | 114 to 154 | |
| References | 155 to 164 | |
| **Abstract, Foreword, Acknowledgements** | 2 to 4 | **LAST**, deliberately |

For each block, in this order:

1. **Read the block end to end** before writing a single finding. No finding from a keyword hit alone.
2. **Pass G+S.** Sentence by sentence. Every finding gets the live string, verbatim.
3. **Pass T.** Against the table in section 2.
4. **Pass X, local.** Does any sentence contradict another sentence inside this block, or a table or
   figure caption inside it?
5. **Pass D.** Every number, every table cell, every caption figure, against section 3. Open the CSV.
6. **Pass C.** Every claim that is not his own result carries a citation with a page locator.
7. **Pass F.**
8. **Write the table.** Then state, in one line, what you did NOT check and why.

## 5. OUTPUT FORMAT. Nothing else.

One markdown table per block. Terse. No explanation paragraphs.

| # | p. | Cls | Sev | Live text | Change to | Why |
|---|---|---|---|---|---|---|
| 1.4 | 20 | T | 1 | replica of the unit | teardown model | not a replica, 200x150x60 vs 166x121x41 |

- **Live text**: copy verbatim, enough to find it with ctrl-F, never more than one line.
- **Change to**: the replacement string, ready to paste. Not a description of a change.
- **Why**: eight words maximum. If it needs more, it is a class-1 item and gets one extra line
  under the table, not inside it.
- Number findings `<chapter>.<n>`.
- A finding already in `thesis_review_backlog_2026-08-26.md` carries its ID in the Why column.

## 6. THE THREE PASSES THAT CANNOT BE RUN CHAPTER BY CHAPTER

Run these on the whole document, after all blocks are done.

**W1. Contradiction sweep.** Read every chapter's claims about the same object side by side.
Known fault lines, all of them already caught once:
- the teardown model's relation to the reference unit (1.3 vs 3.2.1 vs 5.2)
- the main research question's wording (1.4 vs 3.1 opener vs 6.1 vs the Abstract)
- "scenario" vs "route" (3.3 vs 4.1 vs 6.1)
- how Sc4 may be reported (3.3.3 says band only; 4.1 reports single values)
- the condition order (3.5 vs Table 21 vs the 2026-07-21 spec, broken for P03 and P04)
- what the application achieved (4.3.3 interview counts vs 5.3 vs 6.1 vs the Abstract)
- gross vs avoided (3.3.3 vs 4.1.4 vs 5.1 vs the Abstract)
- the prototype version named in 3.4 vs 3.5

**W2. Consistency sweep, mechanical.** Decimal separator · % vs percent · significant figures ·
en dash vs hyphen · heading capitalization · figure and table caption grammar · the numbering of
figures 1 to 44, tables 1 to 22 and equations against the front lists · appendix roman numerals.

**W3. Front matter last.** The Abstract is the most-read page and it restates every claim in the
thesis. It can only be checked once the body is settled. Verification rule E applies: **diff for
DELETIONS, not only changes.**

## 7. WHAT THE ROUTINE MUST NOT DO

- Never edit the document.
- Never assert a fact from memory. Open the file.
- Never delete a `[CITATION NEEDED]` flag without offering the flag-free rewrite in the same breath.
- Never use the em dash, in the thesis or in the report.
- Never cite a section or chapter number in running prose. Appendices ARE cited by number.
- Never make the two-build difference (RBv1.0 vs RBv2.1.1) explicit in the study sections.
- Never report a measure that was not instrumented.

Related: [[thesis-schedule]], [[voice_and_verification_rules]], [[study_results_verified]],
[[lca_results_verified_ch4]], [[recipe_cross_check_verified]], [[teardown_model_as_built]]
