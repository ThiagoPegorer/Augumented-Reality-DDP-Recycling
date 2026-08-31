# CHAPTER 4.1, EVERY LCA NUMBER CHECKED AGAINST THE SOURCE FILES
Run 2026-08-28. Recomputed from the CSVs, not read from a memory note.

## VERDICT

**Every number in 4.1.1 through 4.1.6 reconciles with the source files. Zero data errors found.**
The defects in 4.1 are language, precision and one wrong causal claim. Not the arithmetic.

## WHAT WAS CHECKED AND AGAINST WHAT

### 4.1.1, category screening. `impact_screening.csv`
| Thesis | File | |
|---|---|---|
| minerals 72.45 % | 72.45238 | ok |
| climate 6.67 %, cumulative 79.12 % | 6.67100, 79.12339 | ok |
| freshwater eutrophication 6.58 %, closes at 85.70 % | 6.58123, 85.70462 | ok |

### 4.1.2 and Table 8, gross burden. `impact_EF31.csv`
All twelve table cells and all twelve prose values reproduce the file exactly at the precision
printed. Minerals `0.01873912 / 0.01873964 / 0.01873949 / 0.01873952`, climate
`73.43259 / 73.47943 / 73.40933 / 73.41032`, freshwater eutrophication
`0.1159196 / 0.1159353 / 0.1159295 / 0.1159373`. **ok**

### 4.1.3 and Table 9, stage contributions. `impact_stage_contributions.csv`
All 24 cells reproduce the file. Both share columns sum to 100 and each share recomputes from its
own impact column. Stage 1 to 4 climate total `73.25607` reproduces the sum of its four stages.
Stage 5 as 0.2404 % of the Sc1 climate result recomputes from `s5_sc1 / total_sc1`. **ok**

### 4.1.4, avoided impact and balance. `impact_EF31.csv`
Nine avoided values, six ratios and twelve balance values all reproduce.
Ratios recomputed independently: minerals 3.2193 and 4.7389, climate 1.8472 and 3.4921, freshwater
eutrophication 2.7748 and 4.3139. **ok, and the same four ratios in the Abstract are correct.**
Every balance value equals that scenario's own gross minus its own avoided, to the last digit
printed. **ok**

### 4.1.5, Monte Carlo. `mc_summary.csv`
Every spread was recomputed as `(p95 - p5) / p50`:
| Claim | Recomputed | |
|---|---|---|
| climate gross 62.2 to 66.0 % | 62.23 to 65.97 | ok |
| climate avoided 10.4 to 20.1 % | 10.41 to 20.07 | ok |
| minerals gross 16.5 to 17.7 % | 16.54 to 17.73 | ok |
| minerals avoided 2.2 to 15.6 % | 2.22 to 15.60 | ok |
| freshwater gross 153.7 to 160.9 % | 153.65 to 160.90 | ok |
| freshwater avoided 50.7 to 57.6 % | 50.68 to 57.57 | ok |
| Sc3 p95 0.006091 below Sc4 p5 0.008271, minerals | matches | ok |
| Sc3 p95 8.89 below Sc4 p5 14.32, climate | 8.8891, 14.3226 | ok |
| freshwater intervals meet, Sc3 to 0.024275, Sc4 from 0.022911 | matches | ok |
| Sc1 climate 73.43 below median 91.33 | 91.3281 | ok |
| freshwater 0.115920 below median 0.129911 | 0.1299107 | ok |
| minerals 0.0187391 against 0.0188976 | 0.0188976 | ok |
| Sc4 credit intervals, all three categories | all six endpoints match | ok |

### 4.1.6, ReCiPe cross-check. `impact_screening_ReCiPe_mid.csv` and `recipe_cross_check.xlsx`
| Claim | File | |
|---|---|---|
| freshwater ecotoxicity 37.77 %, marine ecotoxicity 29.03 %, human carcinogenic 24.52 % | 37.7733, 29.0333, 24.5192 | ok |
| cumulative 91.33 % | 91.32585 | ok |
| freshwater eutrophication fourth at 4.8311 % | rank 4, 4.831110 | ok |
| global warming eleventh at 0.2522 % | rank 11, 0.2521855 | ok |
| mineral resource scarcity eighteenth of eighteen at 0.0005 % | rank 18, 0.0004970 | ok |
| normalization references 1.201 x 10^5 · 25.17 · 7,990 · 0.6499 | 120051.21, 25.1747, 7990.408, 0.6498884 | ok |
| Sc1 mineral resource scarcity 2.203 kg Cu eq | 2.202782 | ok |
| fifteen of sixteen paired, terrestrial eutrophication has none | pairing sheet, 15 rows paired | ok |
| three unpaired on the ReCiPe side | 18 minus 15 | ok |
| four of fifteen pairs share a unit | climate, freshwater eutrophication, marine eutrophication, ozone depletion | ok |
| EF 1.847 / 3.492, ReCiPe 1.850 / 3.503 | 1.847198 / 3.492103, 1.849605 / 3.502650 | ok |
| both methods 2.775 / 4.314 in freshwater | 2.774750 / 4.313861, 2.774738 / 4.313861 | ok |
| EF 3.219 / 4.739, ReCiPe 2.467 / 3.494 in minerals | 3.219349 / 4.738924, 2.467151 / 3.494155 | ok |

## FINDINGS IN 4.1 THAT ARE NOT DATA

| # | p. | Cls | Sev | Live text | Change to | Why |
|---|---|---|---|---|---|---|
| 4.1 | 87 | X | **1** | The result of ranking between the two methods mismatch mainly because the units that those categories are measured are not the same. | The two rankings disagree mainly because the two methods normalize against different references. ReCiPe normalizes mineral resource scarcity against 1.201 x 10^5 kg Cu eq and freshwater ecotoxicity against 25.17 kg 1,4-DCB, a gap of four orders of magnitude that puts ecotoxicity at 37.77 % and minerals at 0.0005 %. | **backlog A2, still live.** Your own paragraph above prints both references. Units matter for a different claim, the one you make correctly at the end of the subsection |
| 4.2 | 87 | G | 2 | when the avoided impact is **analyze**, it **increase** from Scenario 2 | when the avoided impact is **analyzed**, it **increases** from Scenario 2 | backlog C |
| 4.3 | 85 | S | 2 | it is 57.57 percent, against 57.02 and 50.68 percent, a difference of less than one point. | it is 57.57 %, against 57.02 % for Sc2 and 50.68 % for Sc3, so it exceeds the Sc2 band by less than one point. | "less than one point" is true only against Sc2. Against Sc3 the gap is 6.9 points |
| 4.4 | 74 | F | **1** | Table 8, six decimals | eight decimals in the minerals row | the table shows two pairs as identical; the paragraph below separates all four. Backlog B2 |
| 4.5 | 74 | F | 2 | Table 8 header `Resource use**.** minerals` and `Eutrophication**.** freshwater` | commas | Table 9 prints the same names with commas |
| 4.6 | 81 | F | 2 | `86.6857 %`, `53.2725 %`, `96.5497 %` and 9 more | `86.69 %` or match 4.1.1's precision | four decimals on a share, against two decimals in 4.1.1 and one in 4.1.5 |
| 4.7 | 73 | G | 2 | Figure 19 caption, `EF3.1 method **apply** to` | `EF 3.1 method **applied** to` | |
| 4.8 | 85 | F | 2 | `(Huijbregts et al., 2017) **.**This` | `(Huijbregts et al., 2017). This` | backlog C |
| 4.9 | 86 | C | 2 | Table 18 supports the claim "four of the fifteen pairs share a unit" but does not show it | add a "same unit" column | the reader cannot check the claim from the table. The column already exists in `recipe_cross_check.xlsx` |

## ONE DATA-HYGIENE ITEM, not a thesis error

`Outputs/2_eol_scenarios/scenarios_results.csv` and `Outputs/3_impact_assessment/impact_EF31.csv`
**disagree**. Climate Sc1: `73.5718` against `73.4326`. Acidification Sc1 and the Sc3 climate credit
also differ. **The thesis reports the `impact_EF31.csv` values throughout, which is the set
`lca_results_verified_ch4.md` verified**, so nothing in the document is wrong. But
`scenarios_results.csv` looks like a superseded run and a future check that opens the wrong file
will report false errors. Mark it superseded or delete it.

Related: [[lca_results_verified_ch4]], [[recipe_cross_check_verified]], [[review_routine_2026-08-28]]
