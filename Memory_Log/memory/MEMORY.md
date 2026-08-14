# Memory index

55 topic files. Rebuilt 2026-08-14 — the previous index listed 5, so sessions booted blind to the
rest. If you cannot find something, it is probably here; do not re-derive it.

## ⭐ Start here

- [rbv2-1-1-handoff.md](rbv2-1-1-handoff.md) — THE handoff. RBv2.1.1 frozen 2026-08-10: state, run order, standing rules, open items
- [thesis-schedule.md](thesis-schedule.md) — Live schedule: submission Aug 25, studies Aug 15/17, what slipped, the most at-risk chapter
- [working_agreements.md](working_agreements.md) — Zero-trust [P]/[M]/[A]/[X] verification, manual Git, no proposal docs as thesis input
- [user_role.md](user_role.md) — Thiago is a solo hands-on builder across Unity/C#, FastAPI, Creo, Git. Time is the constraint, not skill

## The prototype — what exists now

- [device-round1-fixes.md](device-round1-fixes.md) — Durable traps 1-7, the RBv2_1_1 menu, guided-mode device rounds, the as-built passport face
- [rb2_1_dpp_page.md](rb2_1_dpp_page.md) — Spec 04: the 4-tab 2-role DPP page, chip standard, role matrix, the plane-sharing bug
- [rb2_1_scope.md](rb2_1_scope.md) — RB2.1 spec-set layout, the DPP Canva redesign driven by P02/P03, the resolved nav regression
- [exploded_zone.md](exploded_zone.md) — The exploded action zone v4.5.3: separation bands, gesture HUD, help modal, rejected v2.x designs
- [usage_history_data.md](usage_history_data.md) — Usage tab is about the UNIT not the car. Coffin-Manson SOH 48 %, reuse fraction 0.767
- [rbv2_menu_and_cleanup.md](rbv2_menu_and_cleanup.md) — Unity menu map, global run-order numbering, the Legacy-reverts-the-DPP-page trap
- [rebuilt_v2_user_journey.md](rebuilt_v2_user_journey.md) — Miro diagram v4, every edge read from a PDF export: 4 routines + back-nav hierarchy
- [prototype_concept.md](prototype_concept.md) — Three prototype layers, locked tech decisions, and the 2026-07-15 cut of Stage 3 + AI chatbot
- [vcu_bosch_ms504.md](vcu_bosch_ms504.md) — Bosch MS 50.4 datasheet specs and the locked 5-step teardown plan derived from them
- [cad_prototype_scope.md](cad_prototype_scope.md) — As-built CAD v3.0, print release, the Ø4.0 insert-hole coupon test, Bambu PETG settings

## The prototype — build history (how each screen got here)

- [rebuilt_v2_scope.md](rebuilt_v2_scope.md) — RBv2.0 origin (Elle Langer feedback), locked decisions, verified traps, the one-block-per-chat rule
- [rbv2_passport_frontend.md](rbv2_passport_frontend.md) — PassportView + PassportRouter: the pool pattern, why composition is derived at runtime
- [ui_phase1_main_page.md](ui_phase1_main_page.md) — Phase 1 main page, the Editor-builder pattern, and the rebuild-deletes-everything warning
- [ui_phase2_information_tab.md](ui_phase2_information_tab.md) — Phase 2 info tab v3: card grid + modal pages replacing the accordion, icon rasterizer lessons
- [ui_phase3_disassembly_intro.md](ui_phase3_disassembly_intro.md) — Phase 3 intro v3 and the DisassemblyAnimator debugging: bore axis, screw spin, tuned travels
- [ui_phase4_step_flow.md](ui_phase4_step_flow.md) — Phase 4 step flow v3: task gating, cancel modal, frameless how-to loop, step-focus ghosting
- [ui_phase5_completion_summary.md](ui_phase5_completion_summary.md) — Phase 5 summary v3, the Jul-14 silent git revert that wiped it, the 10 s report timeout fix
- [dpp_ux_flow.md](dpp_ux_flow.md) — The locked user flow: reveal-never-quiz, two-canvas architecture, gamification, highlight horizons
- [dpp_visual_design.md](dpp_visual_design.md) — The design system: palette tokens, SF Pro, 640x430 canvases, grabber bars, hover-only highlight
- [unity_state.md](unity_state.md) — Script inventory, the glTF import, part names, DisassemblyAnimator API

## Hardware and platform

- [pico_device_test.md](pico_device_test.md) — First on-device run; the PICO 4 Ultra switch and what must be re-validated
- [pico_xr_loader_choice.md](pico_xr_loader_choice.md) — PICO native PXR_Loader not OpenXR: why XRI pinch bindings do not fire
- [pinch_gesture_implementation.md](pinch_gesture_implementation.md) — PicoHandUIBridge: the canvas-plane hit test, the click-eating background, reticles
- [gesture_ux_polish.md](gesture_ux_polish.md) — Always-visible reticle, panel billboard + recenter, and the render-quality pass
- [render_quality_config.md](render_quality_config.md) — Render scale, MSAA, Canvas Scaler World pixel density, SF Pro atlas rebuild, framerate dial-back order

## Data and schema

- [backend_state.md](backend_state.md) — FastAPI endpoints and the original DPP schema shape (early snapshot — see cirpass file for current)
- [dpp_data_model_cirpass.md](dpp_data_model_cirpass.md) — CIRPASS D2.2 Table 6 as the anchor, honest-labelling vocabularies, two open fabrication calls
- [dpp_payload_v07_bom_reconciliation.md](dpp_payload_v07_bom_reconciliation.md) — The precious-metal figures were wrong by 1.5-11x until 2026-07-30. Corrected values + provenance
- [repo_architecture.md](repo_architecture.md) — Dev workspace vs OneDrive thesis workspace, cross-workspace flows. ⚠ repo root path is stale
- [github_repo_setup.md](github_repo_setup.md) — Remote URL, gitignore contents, the never-`git add .` rule. ⚠ repo root path is stale

## LCA and sustainability

- [lca_scope_verified.md](lca_scope_verified.md) — Functional unit, five-stage boundary, Sc1-Sc4, and the rule that Sc3/Sc4 are modelled not measured
- [lca_findings_for_writing.md](lca_findings_for_writing.md) — Headline numbers, the four plotting rules, three stale CSV twins, five thesis findings
- [lca_v4_build_state.md](lca_v4_build_state.md) — openLCA v4 build: product systems, MC distributions, dataset UUIDs, IPC gotchas
- [sustainability_scope.md](sustainability_scope.md) — The CO2-to-multi-impact pivot and the EF 3.1 category-selection method
- [co2_scenarios.md](co2_scenarios.md) — The original supervisor-requested four-scenario framing and which scenarios are literature-backed
- [openlca_setup.md](openlca_setup.md) — Early openLCA/ecoinvent setup log and the APOS-with-explicit-credit ruling

## Writing the thesis

- [writing_phase_setup.md](writing_phase_setup.md) — SCAFFOLD-ONLY output mode, the SRH AI affidavit duty, document state, the 6-file writer skill, open gaps
- [introduction_progress.md](introduction_progress.md) — Chapter 1 closed 2026-08-13: the 1.1 narrative spine, 1.3's six boundaries, recurring draft defects
- [research_questions_final.md](research_questions_final.md) — The 1.4 questions as rewritten 2026-08-13; hypotheses dropped; routes-vs-scenarios rule
- [registered_research_design.md](registered_research_design.md) — Verbatim signed-proposal RQ/SQs/objectives and the three divergences to own in 1.3

## Literature

- [literature.md](literature.md) — The 53-source Mendeley library, mandatory local-index-first search order, export.bib defects
- [literature_gaps_ar.md](literature_gaps_ar.md) — Ariansyah's real result is AR vs paper (F=10.263, p=0.001); the 14 % animation figure is not significant
- [paper-scout-skill.md](paper-scout-skill.md) — The paper-scout skill: quick mode default, zero-trust citations, which sources are reachable

## Process, tools and rules

- [git_workflow.md](git_workflow.md) — Git is manual: Claude edits, Thiago pushes from PowerShell. Never attempt git from bash
- [session_logging_routine.md](session_logging_routine.md) — The wake-up Notion sync protocol and the session-log row format
- [notion_workspace.md](notion_workspace.md) — The Notion hub, Full Plan page, and the Task Tracker data source id
- [session_history_and_pace.md](session_history_and_pace.md) — How Thiago works: bursty, two sessions a day, rotates tracks, wants challenge not agreement

## Identity

- [thesis_identity.md](thesis_identity.md) — Thesis title, SRH programme, matriculation number, GitHub repo name spelling
- [conference_talk_nextappcon.md](conference_talk_nextappcon.md) — Oct 2026 Berlin XR talk: venture name ReBuilt, framed as founder not student

## ⚠ Stale or superseded — read the replacement first

- [thesis_schedule.md](thesis_schedule.md) — SUPERSEDED by [thesis-schedule.md](thesis-schedule.md). Two files, one hyphen apart. Delete this one
- [unity_next_steps.md](unity_next_steps.md) — May 2026 dated work blocks, all long past. Historical only
- [openlca_setup.md](openlca_setup.md) — Pre-v4: its paths and its 108 kg Sc1 figure are superseded by lca_scope_verified + lca_v4_build_state
- [lca_v4_build_state.md](lca_v4_build_state.md) — Says MC is 3/7 done at n=20; lca_findings_for_writing has it complete at n=1000 x 7
- [sustainability_scope.md](sustainability_scope.md) — Treats functional unit and scenario count as open; lca_scope_verified has them locked
- [session_history_and_pace.md](session_history_and_pace.md) — Session table stops at 07; the pace description still holds, the table does not
- [thesis_identity.md](thesis_identity.md) — Quotes the old registered research question; research_questions_final replaced it 2026-08-13
- [backend_state.md](backend_state.md) — May 2026 schema snapshot; dpp_data_model_cirpass and the payload file are current
