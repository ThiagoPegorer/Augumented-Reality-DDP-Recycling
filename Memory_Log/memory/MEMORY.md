# Memory index

Refreshed 2026-09-16 in **Session 47 (log numbering)**. ⚠ Project memory was NOT reachable that
session, so this exported copy is the only memory a later session can read until the tool works again. If you cannot find something, it is probably
here; do not re-derive it.

## WHERE FILES GO. Read this before writing anything to his disk.

**[file_placement_rule.md](file_placement_rule.md) — his ruling 2026-08-25.** `MAIN PAPER` holds
exactly three things: the `.docx`, `ai_use_log.md`, and `IMGS\` (flat). **EVERY other file Claude
produces goes to `C:\Claude\Projects\AR_DPP\Memory_Log\memory\`.** Ask before committing elsewhere.

## NEXT CHAT STARTS HERE

**THE THESIS IS SUBMITTED.** 2026-08-30/31, a 163-page signed PDF plus the USB drive to the
Examination Office, two days before the 2 September 16:00 deadline. Signed PDF and a OneDrive link
also emailed to both supervisors. **Nothing about the paper is open.** See Notion Session 46.

**THE DEFENSE IS STILL NOT CONFIRMED.** The supervisors proposed **25 or 28 September**; the Task
Tracker carries a tentative row on **28 September at 10:00** (local). **20 minutes presentation, 10
minutes questions, 30 minutes while the supervisors grade.** He wants a **LIVE PICO demo**.

## 🔴 THE DEFENSE DECK IS THE LIVE WORK. Read these two files before anything else.

- **[defense_storyline_2026-09-16.md](defense_storyline_2026-09-16.md)** — the argument the whole
  talk defends, the **five findings to front-run**, the demo protocol with the 40 second abort rule,
  what must not be said in the room, and the shipped defects with a ready answer each.
- **[defense_agenda_17_18_sep.md](defense_agenda_17_18_sep.md)** — **the v2 Act spine, 18 slides,
  exactly 20:00**, and the four work blocks for 17 and 18 September.

**The v2 order is HIS, ruled 2026-09-16: the openLCA methodology, results and discussion come BEFORE
the prototype.** It matches the thesis's own logical work order, SQ1, SQ4, SQ2, SQ3. The artifact is
still revealed early for forty seconds on slide 7, and the live demo stays late, after the LCA
results, so it lands as the payoff rather than as a gadget.

**Division of labor, his ruling: he designs the slides, Claude supplies the data.** Every number
handed to him names the file it came out of.

🔴 **Two open risks no amount of deck work closes:** the defense date is unconfirmed, and the four
research questions rewritten on 2026-08-13 still have **no written agreement from Saman Ghobadian**.

**The writing phase is over.** The 204-item review, the AI-use declaration and the citation sweep
are all closed. `REVIEW_INDEX_2026-08-28.md` and the review files in `Memory_Log\memory\` are now
**history, not a backlog** — do not reopen them unless a defense question needs them.

**New live thread from 2026-09-01: ReBuilt as a public-facing project.** He is posting the
RBv2.1.1 demo video on LinkedIn. See [rebuilt_public_comms.md](rebuilt_public_comms.md) for the
approved caption, the tagline, and the claims that must not be made in public.

**The Task Tracker row "Final thesis submission (USB drive to Exam Office)" is still
`Not Started`.** It is done. Ask him before changing it; never edit the tracker unprompted.

**Where the thesis record lives, if a defense question needs it:**

1. [study_results_verified.md](study_results_verified.md) — every number in 4.3 and the interview counts
2. [lca_results_verified_ch4.md](lca_results_verified_ch4.md) · [recipe_cross_check_verified.md](recipe_cross_check_verified.md)
3. [research_questions_final.md](research_questions_final.md) · [ch6_conclusion_progress.md](ch6_conclusion_progress.md)
   · [ch5_discussion_progress.md](ch5_discussion_progress.md)
4. [ai_disclosure_closed.md](ai_disclosure_closed.md) — section 3.1 and the SRH guidance that governs it
5. [thesis_review_pass_2026-08-28.md](thesis_review_pass_2026-08-28.md) — the review, now closed
6. [voice_and_verification_rules.md](voice_and_verification_rules.md) — **READ BEFORE WRITING PROSE**

## THE CLAIMS HE MUST NOT MAKE, IN THE DEFENSE OR IN PUBLIC

These outlived the writing phase. They are defense ammunition now.

1. **DISMANTLING DOES NOT DECREASE THE UNIT'S ENVIRONMENTAL IMPACT.** The gross burden is identical
   across all four scenarios. The Abstract caveat was removed three times and restored.
2. **THE "INCREASE RECYCLING RATES" VERB IS AT NINE CATCHES.** Write the obstacle, never the gain.
   It reappeared in his LinkedIn caption draft on 2026-09-01, making ten.
3. **THE CLOSING INTERVIEW SAYS THE DATA DID NOT HELP, 8 of 9.** Counterweights: would use it at a
   real workstation 9 of 9, training as the use 8 of 9.
4. **THE HEADSET WAS SLOWER**, and a practice effect plausibly contributes.
5. **PRIOR DISASSEMBLY EXPERIENCE IS THE ONLY BACKGROUND VARIABLE THAT SEPARATES THE RESPONSES.**
6. **THE PRINTED MODEL IS NOT A REPLICA.** Generic device inspired by the datasheet, 200 x 150 x 60
   mm against the reference unit's 166 x 121 x 41. Never call it a replica in public.
7. **Per-part hazard highlighting was NOT implemented.** It would have been invented data.
8. **DO NOT MAKE THE TWO-BUILD DIFFERENCE EXPLICIT ANYWHERE.**
9. **"SCENARIO", NEVER "ROUTE"** for the four modelled constructs.

## KNOWN DEFECTS THAT SHIPPED IN THE SUBMITTED PDF

Do not re-litigate them. Know them in case an examiner asks.

- **4.1.6 states a wrong cause** for the ReCiPe ranking mismatch: it is the normalization references,
  not the units.
- **"Prototype" names TWO objects**, ReBuilt and the 3D printed model, on pages 42 to 69.
- **The Acknowledgements call Elle Langer "Ms." where the title page reads "Prof."**
- **`LCA_explorer.ipynb` contains 36 uses of the abolished word "net"** across 16 cells. It went on
  the USB as a working file. Deferred deliberately, not missed.
- `recipe_screening_log.txt` carries a superseded ReCiPe 2008 endpoint screening.

## Lessons, all durable

1. **Read the document, never assert its state from a memory note.**
2. **Diff a returned edit for DELETIONS, not only changes.** Verification rule E.
3. **Deliver every table as .xlsx, never as pasted text.**
4. **Check the .docx XML before claiming a document defect.** `MENDELEY_CITATION_v3_` for citations,
   and the Heading styles for numbering.
5. **Raw data in the workbook, every derived value in the notebook. Every table auditable.**
6. **He will not accept block-by-block drafting of a section.**
7. **He improves drafts too, and he is sometimes right against Claude.**
8. **He rejects an output format that changes between runs.** **One schema, forever:**
   `ID | p. | Live text | Change to | Why`, grouped by pass then by severity, A/B/C.
9. **A backlog he has not read is not a backlog.**
10. **Never run git; give him the command.** **Never create or edit Task Tracker rows without asking.**
11. **Never cite a section or chapter number in running prose.** Appendices ARE cited by number.
12. **Never use the em dash.** AMERICAN spelling.
13. **He drops details when pasting.** Hand him citations inline in the sentence.

## Start here for the project itself

[working_agreements.md](working_agreements.md) · [file_placement_rule.md](file_placement_rule.md)
· [user_role.md](user_role.md) · [rebuilt_public_comms.md](rebuilt_public_comms.md)

## The prototype

- [rbv2_1_1_ar_system_verified.md](rbv2_1_1_ar_system_verified.md) — **THE verified record**
- [teardown_model_as_built.md](teardown_model_as_built.md) — **the printed model is NOT a replica**
- [device-round1-fixes.md](device-round1-fixes.md) · [rb2_1_scope.md](rb2_1_scope.md) · [exploded_zone.md](exploded_zone.md)
  · [usage_history_data.md](usage_history_data.md) · [rbv2_menu_and_cleanup.md](rbv2_menu_and_cleanup.md)
  · [rebuilt_v2_user_journey.md](rebuilt_v2_user_journey.md) · [prototype_concept.md](prototype_concept.md)
  · [vcu_bosch_ms504.md](vcu_bosch_ms504.md) · [cad_prototype_scope.md](cad_prototype_scope.md)
- Build history: [rebuilt_v2_scope.md](rebuilt_v2_scope.md) · [rbv2_passport_frontend.md](rbv2_passport_frontend.md)
  · [ui_phase1_main_page.md](ui_phase1_main_page.md) · [ui_phase2_information_tab.md](ui_phase2_information_tab.md)
  · [ui_phase3_disassembly_intro.md](ui_phase3_disassembly_intro.md) · [ui_phase4_step_flow.md](ui_phase4_step_flow.md)
  · [ui_phase5_completion_summary.md](ui_phase5_completion_summary.md) · [dpp_ux_flow.md](dpp_ux_flow.md)
  · [dpp_visual_design.md](dpp_visual_design.md) · [unity_state.md](unity_state.md)
- Hardware: [pico_device_test.md](pico_device_test.md) · [pico_xr_loader_choice.md](pico_xr_loader_choice.md)
  · [pinch_gesture_implementation.md](pinch_gesture_implementation.md) · [gesture_ux_polish.md](gesture_ux_polish.md)
  · [render_quality_config.md](render_quality_config.md)

## The user study and the results

[study_results_verified.md](study_results_verified.md) · [ch5_discussion_progress.md](ch5_discussion_progress.md)
· [ch4_findings_progress.md](ch4_findings_progress.md) · [recipe_cross_check_verified.md](recipe_cross_check_verified.md)
· [lca_results_verified_ch4.md](lca_results_verified_ch4.md) · [study_design_verified.md](study_design_verified.md)
· [results_chapter_start_here.md](results_chapter_start_here.md) (numbering stale)
· [study_build_version_finding.md](study_build_version_finding.md)

**2026-08-28: sections 4.1 and 4.3 were recomputed from the source files. ZERO data errors**,
one rounding aside. All seventeen per-step rows of Table 22 sum correctly. Do not re-check them.
**2026-08-30/31: all 28 legal pinpoints verified against the Official Journal on EUR-Lex.**
The data carrier requirement is ESPR **Art. 10(1)(a) and (b)**, not Art. 11. The plastics target
is **1 January 2032**, not 2030.

## Data and schema

[table6_coverage_map.md](table6_coverage_map.md) · [dpp_payload_verified.md](dpp_payload_verified.md)
· [cirpass_d22_table6.md](cirpass_d22_table6.md) · [cirpass_d21_requirements.md](cirpass_d21_requirements.md)
· [annex_vi_schema_gap.md](annex_vi_schema_gap.md) · [dpp_payload_v07_bom_reconciliation.md](dpp_payload_v07_bom_reconciliation.md)
· [backend_state.md](backend_state.md) · [repo_architecture.md](repo_architecture.md)
· [github_repo_setup.md](github_repo_setup.md) · [dpp_data_model_cirpass.md](dpp_data_model_cirpass.md) (superseded)

## LCA and sustainability

[lca_results_verified_ch4.md](lca_results_verified_ch4.md) · [recipe_cross_check_verified.md](recipe_cross_check_verified.md)
· [lca_methodology_3_3.md](lca_methodology_3_3.md) · [modelled_unit_composition.md](modelled_unit_composition.md)
· [lca_scope_verified.md](lca_scope_verified.md) · [lca_findings_for_writing.md](lca_findings_for_writing.md)
· [lca_scenario_source_audit.md](lca_scenario_source_audit.md) · [lca_v4_build_state.md](lca_v4_build_state.md)
· [lca_method_sources.md](lca_method_sources.md) · [sustainability_scope.md](sustainability_scope.md)
· [co2_scenarios.md](co2_scenarios.md) · [openlca_setup.md](openlca_setup.md)

BOM is `LCA_Analysis/Docs/BOM_v4.md`, v4.1, document **234686731**.
**The three notebooks live in `Memory_Log/memory/`**, not in `LCA_Analysis/LCA_Notebook/`.
**`Outputs/2_eol_scenarios/scenarios_results.csv` DISAGREES with `impact_EF31.csv`** (climate Sc1
73.5718 against 73.4326). **The thesis uses `impact_EF31.csv`, which is the verified set.**
The 54 to 189 kWh triangular range for use-phase electricity is `LCA_framework_v4.md` line 656.
- **ReCiPe is a CHARACTERISATION cross-check** on **RATIOS AGAINST Sc2**. Endpoint DROPPED everywhere.
- **The Sc3 and Sc4 avoided intervals OVERLAP in freshwater eutrophication.**
- **The deterministic GROSS value is not the central estimate.** False for the credits.

## Writing the thesis

[ai_disclosure_closed.md](ai_disclosure_closed.md) · [thesis_review_pass_2026-08-28.md](thesis_review_pass_2026-08-28.md)
· [front_matter_progress.md](front_matter_progress.md) · [ch6_conclusion_progress.md](ch6_conclusion_progress.md)
· [ch5_discussion_progress.md](ch5_discussion_progress.md) · [ch4_findings_progress.md](ch4_findings_progress.md)
· [ch3_methodology_progress.md](ch3_methodology_progress.md)
· [voice_and_verification_rules.md](voice_and_verification_rules.md) · [research_questions_final.md](research_questions_final.md)
· [ch1_verbatim_facts.md](ch1_verbatim_facts.md) · [writing_phase_setup.md](writing_phase_setup.md)
· [literature_review_chapter2.md](literature_review_chapter2.md) · [ch2_block2526_final.md](ch2_block2526_final.md)
· [ch2_block24_dpp_final.md](ch2_block24_dpp_final.md) · [ch2_block25_ar_sources.md](ch2_block25_ar_sources.md)
· [ch2_block25_structure_rule.md](ch2_block25_structure_rule.md) · [ch2_evidence_block22.md](ch2_evidence_block22.md)
· [introduction_progress.md](introduction_progress.md) · [methodology_start_here.md](methodology_start_here.md)

## Literature and law

[eu_regulatory_scope.md](eu_regulatory_scope.md) — **REGULATION (EU) 2026/1738. THE law file**
· [elv_regulation_partC_finding.md](elv_regulation_partC_finding.md) · [jensen_2023_data_needs.md](jensen_2023_data_needs.md)
· [bosch_sources_verified.md](bosch_sources_verified.md) · [vcu_datasheet_verified.md](vcu_datasheet_verified.md)
· [literature_gaps_ar.md](literature_gaps_ar.md) · [paper-scout-skill.md](paper-scout-skill.md)
· [literature.md](literature.md) — **Index stale since 2026-08-16**

**ISO 14040/14044 are NOT on disk** — use Pokhrel et al. (2020), p. 2.
**Recommendation (EU) 2021/2279 is NOT on disk.**

## Process, tools and rules

[file_placement_rule.md](file_placement_rule.md) · [session_logging_routine.md](session_logging_routine.md)
· [git_workflow.md](git_workflow.md) · [notion_workspace.md](notion_workspace.md)
· [session_history_and_pace.md](session_history_and_pace.md)

**Google Calendar returns ZERO events**, nine consecutive wake-ups. Do not investigate.
**The Notion MCP query tool takes its arguments under a `data` object.** Date columns are
`"date:Date:start"`, never `"Date"`. Task Tracker status values carry emoji (`'Done'` with a check).
**The Notion SQL parser rejects a WHERE clause on a colon-named date column unless you also use
`SELECT *`.** A named column list plus `WHERE date("date:Date:start") ...` fails with "could not be
parsed safely". Use `SELECT * ... WHERE date("date:Date:start") >= date('YYYY-MM-DD')`.
**Reading the thesis: convert the PDF with `pdftotext -layout` and split on the form feed.**
**Reading the .docx: unzip `word/document.xml` and `word/styles.xml`.**
**Project memory is NOT reachable from device_bash.** It lives outside the mounted folders, so the
memory export has to be written into `Memory_Log/memory/` by hand, not copied.

## Identity

[thesis_identity.md](thesis_identity.md) · [conference_talk_nextappcon.md](conference_talk_nextappcon.md)
· [rebuilt_public_comms.md](rebuilt_public_comms.md)

## Stale or superseded

[thesis_schedule.md](thesis_schedule.md) (SUPERSEDED by `thesis-schedule.md`, delete)
· [thesis-schedule.md](thesis-schedule.md) (**fully spent** — submission happened)
· [ch4_findings_progress.md](ch4_findings_progress.md) ("STILL UNPASTED" section is stale)
· [results_chapter_start_here.md](results_chapter_start_here.md) · [rb2_1_dpp_page.md](rb2_1_dpp_page.md)
· [rbv2-1-1-handoff.md](rbv2-1-1-handoff.md) · [dpp_payload_verified.md](dpp_payload_verified.md)
· [registered_research_design.md](registered_research_design.md) · [dpp_data_model_cirpass.md](dpp_data_model_cirpass.md)
· [ch2_block25_ar_sources.md](ch2_block25_ar_sources.md) · [unity_next_steps.md](unity_next_steps.md)
· [openlca_setup.md](openlca_setup.md) · [lca_v4_build_state.md](lca_v4_build_state.md)
· [sustainability_scope.md](sustainability_scope.md) · [session_history_and_pace.md](session_history_and_pace.md)
· [thesis_identity.md](thesis_identity.md) · [backend_state.md](backend_state.md)
