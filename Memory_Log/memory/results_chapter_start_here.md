---
name: results-chapter-start-here
description: "[P] THE starting file for the Results chapter (Chapter 4), written 2026-08-22 at the end of Session 39. Carries the exact on-disk location of every number the chapter needs, the SRH template's own instructions for the chapter, the reporting rules that bind it, what must be re-verified BEFORE writing, and the table-numbering collision that Table 7 is already taken."
type: project
---

# 🔴 READ THIS FIRST, then [[study_design_verified]] and [[lca_methodology_3_3]]

Chapter 3 is drafted end to end. Chapter 4 is next. Nothing in it may be written before the three
re-verifications below are done.

# ✅ WHERE EVERY NUMBER LIVES. All paths confirmed on disk 2026-08-22.

## The AR-condition study data

`AR_DPP/XR/AR_DPP_VCU/backend/data/reports/`

| File | Participant |
|---|---|
| `report_vcu_001_2026-07-21T08-12-44Z_P01.json` | **P01, the pilot.** AR condition only |
| `report_vcu_001_2026-07-31T18-30-48Z_P02.json` | P02 |
| `report_vcu_001_2026-08-01T13-11-47Z_P03.json` | P03 |
| `report_vcu_001_2026-08-03T15-17-33Z_P04.json` | P04 |
| `report_vcu_001_2026-08-03T16-45-45Z_P05.json` | P05 |
| five untagged files stamped `2026-08-09T20…` and `22…` | **verification runs, not participants** |

⚠ **These carry the AR per-step times.** The manual-condition times exist only in Thiago's paper
notebook, as do the errors and the interview notes. **They are not in the repository.**
⚠ Also not in the repository: **the Google Forms responses.** The usability scale, the ten comparison
items, the seven open questions and the demographics all live in Google Forms.

## The LCA results

`AR_DPP/LCA_Analysis/Outputs/`

| Folder | What is in it |
|---|---|
| `2_eol_scenarios/` | `scenarios_results.csv` · `sc1_sc2_results.csv` · `contribuition_tree.xlsx` · the four scenario build logs |
| `3_impact_assessment/` | `impact_EF31.csv` · `impact_ReCiPe_mid.csv` · `impact_screening.csv` · `impact_stage_contributions.csv` |
| `4_monte_carlo/` | `mc_summary.csv` · `mc_net.csv` · `mc_raw_Sc1..Sc4` (gross and saving separately) · `simulation_result_Sc1.xlsx` |
| `1_stage_builds/`, `0_utilities/` | stage builds and helpers |

⚠ `impact_ReCiPe_end.csv` and `impact_ReCiPe_end_aop.csv` are **ENDPOINT results and are DROPPED.**
Do not read a number out of those two files.
⚠ `mc_net.csv` is a **net** file and "net" is abolished. Use the gross and saving runs separately.

# 🔴 THREE THINGS TO DO BEFORE WRITING A WORD

1. **Restate the headline table off "net".** `LCA_framework_v4.md`'s FINAL RESULTS table is headed
   "Sc2 net / Sc3 net / Sc4 net". Report **gross burden** and **avoided impact** separately, plotted
   above and below one axis. **No recomputation is needed**, the model already keeps them in seven
   separate product systems.
2. **Re-verify monotonicity on the SAVING column, across all 25 categories.** The robustness claim
   "ordering Sc1 > Sc2 > Sc3 > Sc4 is monotonic in all 25 categories" was computed on **net**. **Until
   it is re-checked it is not a finding.**
3. **Decide the sample.** P01 is a pilot on two grounds (AR condition only, and run with a member of
   the supervisory team). Block 1 analyzed n is four. Block 2 adds five on 2026-08-23.

# 🔴 THE REPORTING RULES THAT BIND CHAPTER 4

- **"Net" is abolished. "Gross" stays.** Burdens and credits are never combined into one number.
- **ReCiPe 2016 Endpoint is dropped everywhere.**
- **Every method-dependent number carries its method in the sentence** (EF 3.1 or ReCiPe 2016 Midpoint).
- **`co2_avoided_kg` is NOT reportable.** No real unit was ever tested.
- **The study is TWO BLOCKS** and nothing pools across them without saying so. Block 1 is RBv1.0 with
  manual v1; block 2 is RBv2.1.1 with manual v2.
- **Comparison item C7 is reported BY BLOCK, never pooled.** Manual v2 dropped the composition tables,
  so the "what they are made of" half is not comparable; the value half is.
- **Comprehension is the primary outcome. Time and errors are context**, and the two conditions were
  timed by different instruments.
- **Three of four block-1 participants met the AR condition second**, so a second-attempt advantage runs
  toward AR. Any comparison finding carries that.
- **Participants are P01 to P05 only.** Never a name, and never a description that identifies one.

# ✅ WHAT THE SRH TEMPLATE ASKS OF THIS CHAPTER, verbatim from the .docx

> "It is a complete exposition of your results, in which you present the obtained data in the most
> transparent and objective way possible. Using graphic tools, data sets, tables, or other ways of
> displaying information can be a good way to complement the written information."
>
> "In the result section, you should present your results **without further comments, or an
> interpretation from your side.**"
>
> "Usually, you start the result section with the basic descriptive variables and the outcome dataset.
> For example, if you did a survey start with a description of your sample, such as the sample size,
> the response rate, demographic variables."
>
> "In case you did further, more sophisticated statistical analyses, modelling, etc., put the most
> exciting results at the end of your result chapter."
>
> "Additional analyses which are not necessary to answer your research question should not be included
> in the results chapter but put into an appendix."

**So the shape is: sample description first, then the outcome data, then the LCA modelling last.** No
interpretation anywhere in the chapter; that is the Discussion's job.

⚠ **Delete the template boilerplate from the .docx on compile day.** It is still live under FINDINGS,
DISCUSSION OF FINDINGS and CONCLUSION.

# 🔴 TABLE NUMBERING: Table 7 IS ALREADY TAKEN

Read from the thesis document's LIST OF TABLES on 2026-08-22:

| # | What | Where |
|---|---|---|
| 1, 2 | Reference product parameters · reference unit paired with the printed model | 3.2.1 |
| 3 | Information categories and attributes | 3.2.2 |
| **4** | **Variables use-phase equation** | 3.3.2. *This answers the open question about what Table 4 was* |
| 5 | Design standards applied in the AR prototype | 3.4.2 |
| 6 | Hand tracking gestures | 3.4.3 |
| **7** | **CIRPASS Table 6, reproduced** | **Appendix II** |
| 8, 9 | Components · materials of the whole device | Appendix III |
| 10 to 20 | The eleven process inventories | Appendix IV |

🔴 **The measures table drafted for 3.5 was written as "Table 7" and that number is occupied.** In the
body it becomes Table 7 and **every appendix table shifts up by one, to 8 through 21**. Word's caption
auto-numbering does this by itself, **but any table number typed by hand into running prose will
break.** Check before compiling.

⚠ Appendix tables are numbered in the main sequence rather than as II.1, III.1. The backlog item about
renumbering appendix figures applies to tables too.

# ⚠ A FLAG WORTH CHECKING, NOT A CLAIM

**P03's session report is stamped 2026-08-01T13:11 UTC, and the application's audio system was written
on 2026-08-01.** `UIClickAudio.cs` and `HandPinchAudio.cs` both carry that date, added after P02's
feedback of 2026-07-31. Whether the audio existed before or after 13:11 that day is not recorded
anywhere on disk. **If it landed before P03's session, then block 1 is not internally homogeneous
either: P02 used a silent build and P03 did not.** `git log` around 2026-08-01 would settle it in one
command. **Do not assert this without checking.**

# ⚠ STILL OPEN AND NEEDED FOR THIS CHAPTER

1. The manual-condition times, the errors and the interview notes, all in the paper notebook.
2. The Google Forms responses.
3. The final analyzed sample size.
4. The usability scale citation and its benchmark citation. **The one gap with no flag-free
   alternative**, since it is a published instrument.
5. Whether block 2 ran AR-first.
6. PICO 4 versus PICO 4 Ultra before the chapter names a device.

Related: [[study_design_verified]], [[lca_methodology_3_3]], [[ch3_methodology_progress]],
[[thesis-schedule]], [[voice_and_verification_rules]], [[study_build_version_finding]]
