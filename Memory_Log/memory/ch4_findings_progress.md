---
name: ch4-findings-progress
description: "[P] Live record of Chapter 4 Findings, refreshed 2026-08-25 (Session 42). RESULTS IS CLOSED. 4.1 complete, 4.2 drafted from 44 screenshots, 4.3 drafted end to end in three subsections from a fully audited data chain. Holds the live numbering, what is still unpasted, the affidavit exposure, and the defects that survive into compile day."
type: project
---

# 🟢 RESULTS IS CLOSED, 2026-08-25. Thiago: *"Results is done and close for now."*

| section | state |
|---|---|
| **4.1 LCA Results** | written end to end in the .docx, four subsections. 🔴 **the ReCiPe block is still NOT PASTED** |
| **4.2 AR DPP Prototype (RBv2.1.1)** | drafted from 44 headset screenshots, `section_4_2_draft.md` |
| **4.3 Test Participants Results** | drafted end to end, three subsections, `section_4_3_full_draft.md` |

# ✅ 4.3 STRUCTURE, his ruling 2026-08-25: three subsections

- **4.3.1 Participants** — three figures and a table
- **4.3.2 Task completion time and errors**
- **4.3.3 Perceived usability and interview responses**

Participants get their own numbered subsection rather than an unnumbered lead-in, since they carry
three figures and a table. Draft is **1,562 words of prose**, no em dashes, no boundary words, no
section numbers cited.

🔴 **Every number in it is derived by `ARDPP_study_analysis.ipynb` and reconciled by 21 audit
checks.** See [[study_results_verified]] for the whole dataset, the three response scales, the
condition order and the three open discrepancies against the study spec.

🔴 **`section_4_3_draft.md` is SUPERSEDED and carries stale numbers** (median difference +15.5,
P03 AR 325, P02 AR 458). Renamed `_superseded_` on the working side. **Do not open it.**

# ✅ 4.2, closed 2026-08-25

Drafted from the 44 headset screenshots taken 14:15 to 14:33. Verified the application displays the
EF 3.1 screening at 72.5 / 6.7 / 6.6 / 14.3 % and a 7 min 30 s summary.

**Two paragraphs deleted on his instruction, do not reinstate:** the participant-driven-changes
paragraph and the `co2_avoided_kg` paragraph. He said *"Dont argue."*

⚠ **My colour-contradiction claim was WRONG.** His design standards §8 (printed) and §8.1 (CAD)
already document brown/yellow/green against grey/near-black as two different objects. The
screenshots match the printed unit. **All colours match.**

# 🔴 NUMBERING, read from the .docx 2026-08-24. Read it again, never assert from memory.

Tables **7** screening · **8** gross · **9** stage 1 to 4 · **10** stage 5 share of gross ·
**11 to 14** stage 5 per scenario (stacked) · **15 to 17** stage 5 per category (cross-tab) ·
**18** Appendix II CIRPASS.
Figures **18** threshold · **19** gross · **20** stage pies · **21** avoided · **22** balance ·
**23** Monte Carlo.
After the ReCiPe block lands: pairing table **18**, CIRPASS **19**, ranking **Figure 24**,
ratios **Figure 25**. Then 4.2's images and 4.3's six figures continue from there.

# 🔴🔴 THE AFFIDAVIT EXPOSURE. Still the most under-attended risk in the thesis.

Claude's sentences are pasted verbatim in 4.1, and 4.2 and 4.3 are now **also delivered as prose**.
His own `academic-writer` skill sets **scaffold** as the default precisely to prevent this, and the
SRH affidavit requires AI-generated or AI-paraphrased phrasing to be **denoted in the core text**,
not merely logged.

**Owed before signing:** a pass through 4.1, 4.2 and 4.3 marking retained AI phrasing, or a rewrite
in his words. The **Session 41 AI-use log entry is still not pasted** into `ai_use_log.md`, and
Session 42's is not written.

# 🔴 STILL UNPASTED INTO THE .docx

1. **The ReCiPe block** and its **three Methodology edits** (`recipe_block_draft.md` Part 2).
2. **The three open-answer provenance edits** (`open_answers_provenance_fix.md`): the session
   sequence, the instrument paragraph, and the one clause in 4.3.
3. All of 4.2 and all of 4.3.

# 🔴 DEFECTS THAT SURVIVE INTO COMPILE DAY

1. **The stage 5 data is published TWICE**, Tables 11 to 14 against 15 to 17. He kept both.
2. Decimal separators: Tables 7, 9, 11 to 17 use commas; Tables 8, 10 and all prose use periods.
3. Table 8 prints 6 significant figures; its own paragraph prints 7.
4. `%` against "percent" spelled out.
5. Subsection heading capitalisation; 4.1.1 is the only en dash in the thesis.
6. Grammar items in `ch4_1_review.md` section 4.
7. 4.1.2 holds about 70 % of 4.1 and does three jobs. 4.1.3's title says "(Scenario 2 to 4)" while
   the balance paragraph inside it reports Sc1.

⚠ **The screening paragraph's citations were never missing.** Mendeley field citations
(`w:tag w:val="MENDELEY_CITATION_v3_…"`) are invisible to python-docx. The real finding is narrower:
none carries a page locator. **Never conclude a citation is absent from a python-docx read.**

# ✅ RESOLVED THIS SESSION

- **The Sc4 band contradiction**, by relocating the 3.3.3 sentence. Deterministic first, simulated
  second. Replacement sentence in [[thesis-schedule]].
- **The comparative-scale anchors**, blocked since 22 August. 1 is the two-dimensional manual, 5 is
  the Augmented Reality model. Corroborated by the Session 12 Notion page.
- **The condition order**, blocked since 22 August. Four and four among the eight.
- **The background scale labels.** Endpoints only. The three invented middle labels were removed.
- **The instrument naming.** Left unnamed. No citation owed, no benchmark sentence.

# 🔴 DECISIONS TAKEN AGAINST ADVICE. Recorded, not to be reopened.

1. The chapter opener omits the two-build fact. **4.3 must carry it** and currently does not.
2. The sixteen-category ordering statement was cut from 4.1.
3. **Both stage 5 table sets stay.**
4. **The participants figure was split into three.** My objection was that three figure numbers and
   three captions carry what one carried, and the disassembly chart has one empty level and four
   bars of length 1 or 2. His call, executed.
5. **A chart was built for the comparative items** after I argued a table was more honest. Built as
   a response matrix that averages nothing, which is the version I can defend.

# ✅ WHAT GOES TO THE DISCUSSION, not to Results

1. **The simplicity of the replica**, which stands behind both the absent errors and the eight of
   nine who found the recovery information unhelpful. One cause, two findings.
2. **Training rather than throughput.** Eight of nine name training, all nine would use it, the
   times show no consistent gain. Three measures pointing the same way.
3. **The hardware ceiling.** Passthrough sharpness 6 of 9, comfort 7 of 9.
4. **The ceiling on the comparative items.** Ten items returning the same median from the same five
   participants is a limit of the instrument on this sample.
5. **The order effect.** Five of eight faster in their second condition, and AR slower for three of
   four who ran it first against two of four who ran it second.
6. **The ReCiPe "why it mismatches" block**, scaffold at `why_it_mismatches_placement.md`.

Related: [[study_results_verified]], [[recipe_cross_check_verified]], [[lca_results_verified_ch4]],
[[thesis-schedule]], [[research_questions_final]], [[voice_and_verification_rules]],
[[lca_methodology_3_3]]
