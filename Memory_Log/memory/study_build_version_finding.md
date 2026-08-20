---
name: study-build-version-finding
description: "🔴 READ BEFORE WRITING 3.4, 3.5, 3.6, THE OPENER OR CHAPTER 4. CONFIRMED by Thiago 2026-08-20: PARTICIPANTS USED RBv1.0, not the frozen RBv2.1.1. The Methodology opener claims the build was frozen before any participant used it, which is FALSE and already pasted in the .docx. Carries the ten session report files with their data, the ruling that co2_avoided_kg cannot be reported, what is STILL MISSING for 3.5, and the two-payload-copies resolution."
type: project
---

# 🔴 THE FINDING, confirmed by Thiago 2026-08-20 in his own words

> "The participants was expose to the version RBv1.0. However, those tests was very important to I
> collect feedback of UI and UX and build the RBv2.1.1. But, the disassembly sequence didn't change
> between versions, only words to refeer to the parts of the 3D prototype, what dont generate big
> changes. The main goal of the study is to collect timestamp of disassembly of the user using the AR
> and disassembly using the 2D manual, and UI and UX feedback. The column CO2 avoided can't be apply,
> since we did not test in real units."

# 🔴 CONSEQUENCE 1: A SENTENCE ALREADY IN THE .DOCX IS FALSE

Methodology opener, paragraph 2:
> "The prototype was frozen at a fixed version before any participant used it."

Participants used RBv1.0. RBv2.1.1 was frozen 2026-08-10, a week after the last session on 2026-08-03.

**Replacement to use:**
> "Participants worked with an earlier version of the prototype, and the sessions informed the version
> described in this chapter. The disassembly sequence was the same in both."

# 🔴 CONSEQUENCE 2: THE STUDY DESCRIBES RBv1.0, THE CHAPTER DESCRIBES RBv2.1.1

- **3.4** names RBv2.1.1 and states that the tested version was earlier.
- **3.5** describes **RBv1.0**.
- **3.6** takes the limitation with Thiago's defence attached: the disassembly sequence was identical
  across the two versions and only the words naming the printed parts changed. State it as his
  reasoning, not as a measured equivalence.
- **Chapter 4** reports results measured on RBv1.0.
- Research question 3 asks how the passport performs against a conventional manual on time, errors and
  perceived usability. The answer describes RBv1.0. Scope it, or say plainly which version it belongs to.

# 🔴 CONSEQUENCE 3: co2_avoided_kg IS NOT A RESULT

Thiago's ruling: it cannot be applied because no real unit was tested. Every report file carries the
field, so Chapter 4 excludes it explicitly rather than silently. The value is the modelled
environmental.recovery_potential.total_avoidable_kg (15.4315) echoed back by the app.

# THE TEN SESSION REPORTS

XR/AR_DPP_VCU/backend/data/reports/. Every file has exactly these keys and no others:
product_id, timestamp, elapsed_s, steps_completed, step_times_s, recovered_component_ids, co2_avoided_kg.

| timestamp | tag | elapsed_s | steps | n recovered | co2 | step_times_s |
|---|---|---|---|---|---|---|
| 2026-07-21T08:12:44Z | P01 | 418 | 5 | 12 | 6.57 | 149, 127, 80, 24, 38 |
| 2026-07-31T18:30:48Z | P02 | 458 | 5 | 11 | 15.43 | 116, 154, 124, 31, 35 |
| 2026-08-01T13:11:47Z | P03 | 325 | 5 | 11 | 15.43 | 62, 86, 76, 67, 33 |
| 2026-08-03T15:17:33Z | P04 | 376 | 5 | 11 | 15.43 | 127, 78, 63, 66, 42 |
| 2026-08-03T16:45:45Z | P05 | 314 | 5 | 11 | 15.43 | 133, 84, 44, 32, 23 |
| 2026-08-09 20:24 to 22:31 | untagged x5 | 31 to 73 | 5 | 15 | 15.43 | see files |

The five untagged 2026-08-09 runs are Thiago's own verification passes on the night of the freeze.
31 to 73 seconds is not a person disassembling a device, and they use the frozen build's fifteen
component vocabulary. EXCLUDE THEM FROM ANY RESULT.

⚠ P01 is not strictly comparable to P02 to P05: 12 components against 11, and 6.57 kg against 15.43.
Both the vocabulary and the life cycle model changed between P01 and P02. Only elapsed_s and
step_times_s matter, and those hold IF the sequence was identical. Flag P01 in 3.6.

⚠ The _P01 to _P05 suffixes were added by hand. The server names files from the identifier and a
timestamp only. Worth one sentence in 3.5 on how sessions were identified.

# 🔴 WHAT IS STILL MISSING FOR 3.5 AND CHAPTER 4

1. THE 2D-MANUAL TIMINGS. The comparison is the study's main goal and the app writes a report only for
   its own runs, so there is no condition field and no manual arm anywhere in the repository.
2. Design: within subjects or between? If both conditions per participant, what order, counterbalanced?
3. The interface feedback collected in the sessions, in whatever form.
4. Participant backgrounds. Chapter 1 says "of mixed background: people experienced in electronics,
   people experienced in Augmented Reality, and people meeting both the device and the technology for
   the first time." Which participant was which.
5. Confirmation that n = 5 and that P01 to P05 are one session each.

# ✅ THE TWO PAYLOAD COPIES, resolved 2026-08-20

- XR/AR_DPP_VCU/backend/data/vcu_001.json, 63,244 B, 2026-08-09 22:15. THE LIVE FILE.
  backend_open.txt launches uvicorn from XR\AR_DPP_VCU\backend.
- backend/data/vcu_001.json, 63,340 B, 2026-08-19 12:18. An orphan that nothing serves. It received the
  "Voltage regulator" rename, so THAT RENAME NEVER REACHED THE PROTOTYPE.

Thiago's ruling: delete the orphan and its .bak_before_rename, keep the copy inside the Unity project.
He does the deletion; the bridge cannot delete.

Consequences. The frozen study artifact was never modified; its last_updated still reads 2026-08-09, so
the "frozen payload" question is closed. And the 2026-08-19 rename turned out to be WRONG under the
mapping settled on 2026-08-20, so deleting the orphan erases the error.

# ✅ THE REMODELLING, confirmed against the file

Thiago removed vehicle-level data a VCU does not hold: vehicle range and the power consumption of the
vehicle and of the unit. Verified: specifications.power_consumption_w null; usage_history every field
null with basis "not_provided"; unit_use_phase.note verbatim "Use-phase telemetry a vehicle control unit
would record about ITSELF. Every figure is derived from data already in this passport.";
environmental.usage_profile keeps 15 years and 225,000 km correctly, as an LCA duty-cycle input.

⚠ Table 6 row 19 is PARTIAL BECAUSE OF SCOPE, not because the data is simulated. It is the third
instance of the same ruling, alongside the null service.maintenance_interval and the two not-applicable
energy-label attributes. Verdicts unchanged: of 9 voluntary attributes, 3 covered, 3 partial, 3 declared
absent.

⚠ "Germany" is NOT in the record. The only country field is country of origin. If the thesis says a
German duty cycle, it needs a basis, most likely the electricity mix geography in the openLCA model.

Related: [[ch3_methodology_progress]], [[bosch_sources_verified]], [[table6_coverage_map]],
[[dpp_payload_verified]], [[thesis-schedule]], [[ch1_verbatim_facts]], [[rbv2-1-1-handoff]],
[[teardown_model_as_built]]
