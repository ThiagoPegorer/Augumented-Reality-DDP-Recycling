---
name: file-placement-rule
description: "[P] Thiago's ruling 2026-08-25: MAIN PAPER holds only the .docx, ai_use_log.md and IMGS. EVERY other file Claude produces goes to C:\\Claude\\Projects\\AR_DPP\\Memory_Log\\memory. Corrects every stale MAIN PAPER path in older memory files and records the figure names he chose himself."
type: project
---

# 🔴 THE RULE, his words 2026-08-25

> *"every memory file, add to the folder: C:\\Claude\\Projects\\AR_DPP\\Memory_Log\\memory. Not to my
> folder MAIN PAPER. in MAIN PAPER must be just the .docx, ai_log and the images folder."*

## MAIN PAPER holds exactly three things

`C:\Users\thiag\OneDrive\Documents\MASTER THESIS\MAIN PAPER\`

| item | what |
|---|---|
| `Master_Thesis_ThiagoPegorer_100003505.docx` | the thesis |
| `ai_use_log.md` | the AI-use log |
| `IMGS\` | every figure and photograph, **flat, no subfolders** |

**Nothing else.** No workbooks, no notebooks, no drafts, no scaffolds, no review documents, no
checklists, no `.py` cells.

## Everything else goes here

`C:\Claude\Projects\AR_DPP\Memory_Log\memory\`

That single folder now holds the memory mirror **and** every working artefact: the study workbooks,
`ARDPP_study_analysis.ipynb`, `LCA_explorer.ipynb`, the section drafts, the notebook cells, the
review documents and the session READMEs. It is git-tracked.

## 🔴 SO EVERY OLDER MEMORY PATH READING "MAIN PAPER/<working file>" IS WRONG

Files such as `recipe_block_draft.md`, `route_to_scenario_checklist.md`,
`why_it_mismatches_placement.md`, `section_4_2_draft.md`, `ch4_1_review.md`,
`open_answers_provenance_fix.md`, `ARDPP_study_data.xlsx`, `ARDPP_study_analysis.ipynb`,
`ARDPP_study_results.xlsx`, `ARDPP_figure_audit.xlsx`, `cell_figure_audit.py`,
`cell_stage_pies.py`, `stage5_summary_stacked.xlsx`, `stage_1_to_4_contributions.xlsx`,
`recipe_cross_check.ipynb`, `recipe_cross_check.xlsx`, `stage_blocks_text.md`,
`uncertainty_block_revised.md` and the three `section_4_3_*.md` drafts are **all in
`Memory_Log\memory\`**, whatever an older note says.

# 🔴 HE PLACES AND NAMES THE FIGURES HIMSELF

He saves each figure into `IMGS\` under his own short name, as he decides to use it. **Do not commit
figures there, and do not create subfolders.** A `FIGS_4_3\` and a `FIGS_4_1\` folder were created on
2026-08-25 and he deleted both.

Names he chose for the six section 4.3 figures:

| his name in IMGS | the notebook's output |
|---|---|
| `age.png` | `fig_participants_age` |
| `participant_ar_xp.png` | `fig_participants_headset_experience` |
| `participant_ee_xp.png` | `fig_participants_disassembly_experience` |
| `test_timelapse.png` | `fig_completion_times` |
| `perceived_usabillity.png` | `fig_usability_scores` |
| `comparative_questionary.png` | `fig_comparative_items` |

Earlier ones already in place: `stage_1_4_contribuition.png` · `ReCiPe_midpoint_ranked.png` ·
`ReCiPe_EF31.png` · `Monte_Carlo_boxplot.png` · `gross_avoided.png` ·
`avoided_burden_scenario.png` · `fig_4_1_ef31_screening.png` and `.svg` · `weight_norm_ef3_1.png` ·
`gross_ef3_1.png` · `Impact_gross.png` · `Stages_LCA.png` · `graph.png` · `prototype_explode.png` ·
`prototype_isometric.png` · the Methodology `TP THESIS - …` set · `IMGS\AR DPP\` for the headset
screenshots.

⚠ `perceived_usabillity.png` is his spelling. **Do not correct it**; the .docx references it.

# ✅ HOW TO DELIVER, from now on

1. Build in the session workspace.
2. `SendUserFile` so he can see and take it.
3. **Commit only to `Memory_Log\memory\`.** Never to MAIN PAPER without an explicit instruction.
4. Give him the git command for `Memory_Log/`. **Never run git.**

⚠ **Ask before committing anywhere new.** On 2026-08-25 twenty-four files were written into MAIN
PAPER unasked and he had to move all of them.

Related: [[session_logging_routine]], [[git_workflow]], [[github_repo_setup]],
[[study_results_verified]], [[thesis-schedule]]
