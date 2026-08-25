# ReCiPe cross-check: Findings draft and Methodology edits

**This output is PROSE, not a scaffold.** Under the affidavit rule, any phrasing that survives
into the document is at risk of needing to be denoted in the core text. The AI-use log entry at
the end of this file records that.

Every number below was recomputed first-hand this session from
`impact_EF31.csv`, `impact_ReCiPe_mid.csv`, `impact_screening_ReCiPe_mid.csv` and
`recipe2016_nw_factors.txt`. None came from memory or from the facts register.

---

## PART 1 — Findings, new subsection at the end of 4.1

### Cross-check with ReCiPe 2016 Midpoint

EF 3.1 characterizes every result reported so far. ReCiPe 2016 Midpoint (H) characterizes the same
seven product systems (Huijbregts et al., 2017). This cross-check runs at characterized level.

Fifteen of the sixteen EF 3.1 categories have a ReCiPe counterpart. Eutrophication, terrestrial has
none, since ReCiPe 2016 defines no terrestrial eutrophication midpoint. Marine ecotoxicity,
terrestrial ecotoxicity and ozone formation for terrestrial ecosystems stay unpaired on the ReCiPe
side. Four of the fifteen pairs share a unit.

The three categories reported in this study sit inside that pairing. Climate change pairs with
global warming, and eutrophication, freshwater pairs with freshwater eutrophication. Both of those
pairs share a unit. Resource use, minerals and metals pairs with mineral resource scarcity, measured
in kg Sb eq and in kg Cu eq. Those three counterparts locate the reported categories inside ReCiPe's
own profile.

`[TABLE: Pairing of the reporting categories under EF 3.1 and ReCiPe 2016 Midpoint (H) |
source: own elaboration]`

ReCiPe defines no midpoint weighting set, so the ranking below is normalized only. The
Environmental Footprint ranking reported earlier is normalized and weighted. Freshwater ecotoxicity
holds 37.77 percent of the normalized ReCiPe profile, marine ecotoxicity 29.03 percent and human
carcinogenic toxicity 24.52 percent. Those three close the set at 91.33 percent cumulative. The
three counterparts fall outside it. Freshwater eutrophication ranks fourth at 4.8311 percent. Global
warming ranks eleventh at 0.2522 percent. Mineral resource scarcity ranks eighteenth of eighteen at
0.0005 percent.

`[FIGURE: ReCiPe 2016 Midpoint (H), categories ranked by normalized contribution, Sc1 basis |
source: openLCA model, own elaboration]`

ReCiPe normalizes mineral resource scarcity against 1.201 x 10^5 kg Cu eq. It normalizes freshwater
ecotoxicity against 25.17 kg 1,4-DCB, global warming against 7,990 kg CO2 eq and freshwater
eutrophication against 0.6499 kg P eq. The characterized Sc1 result for mineral resource scarcity is
2.203 kg Cu eq.

The avoided impact increases from Sc2 to Sc3 to Sc4 in all twenty-five EF 3.1 result rows and in all
eighteen ReCiPe categories.

The comparison of magnitudes is taken on ratios computed inside each method, so the two pairs that
share a unit and the pair that does not stay comparable. Each credit below is expressed as a
multiple of the same method's Sc2 credit. In the climate pair EF 3.1 returns 1.847 and 3.492, and
ReCiPe returns 1.850 and 3.503. In the freshwater pair both methods return 2.775 and 4.314. In the
minerals pair EF 3.1 returns 3.219 and 4.739, and ReCiPe returns 2.467 and 3.494.

`[FIGURE: Avoided impact relative to Sc2 under two characterization methods |
source: openLCA model, own elaboration]`

In the freshwater pair the two methods return the same multiple at both steps. In the climate pair
they differ by 0.003 at Sc3 and by 0.011 at Sc4. In the minerals pair EF 3.1 returns the larger
multiple at both steps, and the two methods differ by 0.752 and by 1.245. At characterized level
EF 3.1 and ReCiPe return almost the same freshwater eutrophication result. That agreement repeats a
shared characterization factor, and the climate agreement does not. Of the three reported pairs, the
minerals pair is the only one whose multiples differ. It is also the only one measured in two
different units.

---

## PART 2 — Methodology, three edits to "Impact Assessment and uncertainty"

### Edit 1, the opening paragraph

**Currently reads:**

> This study uses two such methods, for different purposes, and reports one of them at characterized
> level only.

**Replace with:**

> This study uses two such methods, for different purposes. The second is reported at characterized
> level, with one normalized ranking used to locate the reporting categories inside its own profile.

**Reason.** Publishing the ReCiPe ranking breaks the sentence as it stands. This is the same class
of defect as the Sc4 band contradiction, two chapters disagreeing about what the study reports.

### Edit 2, the ReCiPe paragraph

**Append these three sentences to the paragraph that ends** *"…that conclusion does not depend on
the choice of method."*

> ReCiPe 2016 Midpoint carries normalization factors but defines no midpoint weighting set. The
> ranking it produces is therefore normalized only, and it is not the same operation as the
> normalized and weighted Environmental Footprint screening. This study reports that ranking once,
> to place the three reporting categories inside ReCiPe's own profile, and does not use it to select
> categories.

**Reason.** The last clause is the one that matters. It states in Methodology that ReCiPe
normalization was tested and rejected as a selection basis, so the ranking in Findings reads as
evidence rather than as an argument against the author's own category selection. Your audit file
`recipe2016_nw_factors.txt` already reaches this conclusion in writing.

### Edit 3, the Monte Carlo paragraph

**Currently ends:**

> For the fourth scenario it is not optional. Its functional yield is unsourced, so that scenario may
> be reported only as a band and never as a single value.

**Replace the second sentence with:**

> Its functional yield is unsourced, so the deterministic result for that scenario is reported
> together with its simulated interval, and that interval is given with the uncertainty results.

**Reason.** Agreed earlier. It keeps the commitment and moves where the band appears, so the chapter
runs deterministic first and simulated second. The matching paragraph is already drafted for the end
of the uncertainty subsection.

---

## PART 3 — placement and numbering

Insert the subsection **after the uncertainty subsection**, as the last part of 4.1. The chapter
opener already promises ReCiPe as a cross-check, so nothing before it needs changing.

| element | number after insert |
|---|---|
| Pairing table | Table 18 |
| ReCiPe midpoint ranking | Figure 24 |
| Avoided impact relative to Sc2 | Figure 25 |
| Appendix II CIRPASS | Table 19 |

Refresh the LIST OF TABLES and LIST OF FIGURES fields afterwards.

---

```
=== Check before using ===

Assumptions I made:
  - Percent is spelled out, matching the stage 5 and Monte Carlo paragraphs. Your stage 1 to 4
    paragraphs use the % symbol. One convention must win across 4.1.
  - American spelling ("characterized", "normalized"), matching the .docx.
  - The subsection heading is sentence case, matching your last two headings and not the first two.

Needs a real citation:
  - (Huijbregts et al., 2017) is written inline for ReCiPe 2016. I verified the work IS in your
    Mendeley library and already cited elsewhere in the document, so insert it as a field, not as
    typed text. Add a page number if you quote the normalization set.
  - The normalization references (1.201 x 10^5 kg Cu eq and the other three) come from your own
    audit output `recipe2016_nw_factors.txt`, generated from the openLCA method pack. If you want
    them attributed to the published ReCiPe World-2010 table rather than to your own model, that
    citation does not yet exist on disk.

Needs a number or fact from you:
  - Nothing. Every value is recomputed and traced.

Derived values, named as derived in the text so they are not read as reported:
  - The Sc3/Sc2 and Sc4/Sc2 multiples ("expressed as a multiple of the same method's Sc2 credit").
  - The gross spread ("the difference between the largest and the smallest scenario value, divided
    by the smallest").
  - The 25 of 25 and 18 of 18 counts.

Needs your confirmation:
  - The full 15-pair mapping was REBUILT from the two category lists. Your own PAIRS list lives in
    your notebook, not on disk here. The rebuild reproduces all four constraints your earlier cell
    states (15 pairs, 4 sharing a unit, EF's Eutrophication terrestrial unpaired, and marine
    ecotoxicity, terrestrial ecotoxicity and ozone formation for terrestrial ecosystems unpaired),
    and the notebook asserts each one. Compare it against your PAIRS before publishing.

Possible contradiction:
  - **A claim carried in project memory is wrong and must not be written.** It records that "the
    pairs that agree are exactly those that share a unit". Across all fifteen pairs that is false.
    Water use and water consumption do not share a unit and agree to 0.000. Ozone depletion shares
    its unit and differs by 0.241. The claim holds only inside the three reported categories, and
    the draft states it only there.
  - **Publishing all fifteen pairs shows a divergence larger than the one the block discusses.**
    Human toxicity non-cancer differs by 1.554 at Sc4 against minerals at 1.245. If the fifteen-row
    table is printed, one sentence should name that, or a reader finds the larger gap unaddressed.
  - `facts_register.md` §5 records Huijbregts et al. (2017) as MISSING from the library and says the
    ReCiPe cross-check "has no citable method source". That entry is now stale: the work is cited in
    the current .docx. Correct the register.
  - The same register section states the ReCiPe cross-check as "global warming −21.1 %, mineral
    resource scarcity −38.9 % at Sc4". Those are Sc1-relative percentages, the basis you removed
    from the thesis. They are not wrong, they are simply no longer the reported basis. The draft
    above uses ratios against Sc2 instead.
  - `recipe_screening_log.txt` still contains a superseded ReCiPe 2008 endpoint screening in which
    metal depletion ranks first at 22.1 percent, directly above the rebuilt ReCiPe 2016 endpoint
    screening in which mineral resource scarcity is 0.0 percent. Two opposite answers sit in one
    file. The 2016 numbers are the live ones.
  - The log also says it wrote `impact_screening_ReCiPe_end.csv`. That file is not on disk.
```

---

## AI-use log entry, to paste before "## Submission checklist"

```markdown
## Session 41 — 2026-08-24

- **Tool:** Claude (Anthropic), Cowork session, model claude-opus-5.
- **Prompts, in order:** stage 5 process tables per scenario and per category; life cycle stage
  contribution pies for stages 1 to 4; review of section 4.1 as pasted into the .docx; revision of
  the uncertainty block; design and drafting of the ReCiPe 2016 Midpoint cross-check.
- **What was produced:**
  - `stage5_summary_stacked.xlsx`, `stage_1_to_4_contributions.xlsx`, `recipe_cross_check.xlsx`.
  - Two notebook cells and one notebook, `cell_stage_pies.py` and `recipe_cross_check.ipynb`,
    written and executed by Claude against the author's own Outputs files.
  - **Claude-drafted prose** for: the stage 1 to 4 paragraphs, the stage 5 process table
    introductions, the revised uncertainty block, and the ReCiPe cross-check subsection.
    **Phrasing at risk of being retained. Must be denoted.**
  - Three proposed edits to the Methodology impact assessment subsection.
- **Target sections:** Chapter 4, section 4.1; Chapter 3, "Impact Assessment and uncertainty".
- **Verification performed this session, first-hand against primary artefacts:**
  - `impact_EF31.csv`, `impact_ReCiPe_mid.csv`, `impact_screening_ReCiPe_mid.csv`,
    `impact_stage_contributions.csv`, `mc_summary.csv`, `recipe2016_nw_factors.txt` and
    `recipe_screening_log.txt` read and computed against directly.
  - The current `.docx` read directly for section 4.1 as pasted, including the Mendeley field
    citations.
  - **New this session:** the Sc3 and Sc4 avoided intervals overlap in freshwater eutrophication,
    Sc3 reaching 0.024275 kg P eq and Sc4 beginning at 0.022911 kg P eq. Separated in the other two
    reported categories.
  - **Corrected in the draft:** two gross band ranges had been rounded to integers in a way that
    raised their lower bound above the lowest value in the set.
- **Corrections made to earlier records:** an earlier claim in this session that three citations were
  missing from the screening paragraph was wrong. The citations are Mendeley field citations and were
  present. The finding was narrowed to the absence of page locators.
```
