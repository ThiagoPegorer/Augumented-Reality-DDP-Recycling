---
name: lca-methodology-3-3
description: "[P] THE record of Methodology section 3.3, written and closed in Session 38 (2026-08-21/22). Carries the two supervisor rulings that changed the model's reporting (NET ABOLISHED, ReCiPe ENDPOINT DROPPED), every number in 3.3 with its trace, the six Miro figures, the two new appendices, the openLCA export findings that corrected a figure, and the framework corrections Thiago still owes."
type: project
---

# 🔴 TWO SUPERVISOR RULINGS THAT CHANGED THE MODEL'S REPORTING

Both from Prof. Dr. Saman, relayed by Thiago 2026-08-21. Neither requires recomputation.

## 1. The term "net" is ABOLISHED. "Gross" stays.

Thiago first said to rename gross → "baseline" meaning Sc1. He **withdrew that** after the objection
that gross is per-scenario while baseline would be Sc1 only, leaving Sc2–Sc4 with no word for their own
burdens. **Final: gross stays as gross, net is gone.**

- Report per route: **gross footprint** and **avoided impact**, never combined.
- Plot both on one axis: gross above the line, avoided below.
- The supervisor's own reference chart (a biochar GHG figure he showed Thiago) **does draw a "GHG
  balance" line**, which is arithmetically a net. Unresolved: confirm with him whether a balance line is
  permitted and what it is called. Raised, not answered.

**Consequences not yet actioned:**
- `LCA_framework_v4.md` §0.3 still specifies "gross / saving / net"; §4.1 says "Net result per scenario
  = gross − saving"; the FINAL RESULTS table's columns are literally headed **"Sc2 net", "Sc3 net",
  "Sc4 net"**, and every percentage in it (−6.0, −11.1, −21.0 climate; −10.0, −32.2, −47.3 minerals) is
  a net-against-Sc1 comparison. **All must be restated.**
- 🔴 **The core robustness claim is net-based.** "Ordering Sc1>Sc2>Sc3>Sc4 monotonic in all 25
  categories" is computed on net. With net gone it has no quantity. **Move the comparison onto the
  SAVING column and RE-VERIFY monotonicity there across all 25 categories before writing it as a
  finding.** It was verified on net, not on saving. Partial evidence exists: the framework records
  "Sc4 saving ≥ Sc3 saving everywhere ✔" and saving ratios Sc3/Sc2 of 1.85× climate and 3.22× minerals.
- **The model needs NO change.** Seven product systems already keep gross and credits separate, and the
  framework's own words are "never netted silently". Only the results script's final subtraction and the
  vocabulary change.
- Findings ② (9 g = 45 % of Sc4's climate saving) and ③ (use phase ≈46 % of climate) are saving-side and
  gross-side respectively and both survive untouched.

## 2. ReCiPe 2016 ENDPOINT is dropped everywhere.

- Framework §0.4 and §4.2 still specify it. Correct them.
- 🔴 **This kills HALF of §4.2.1.** The weighting/single-score rejection is entirely an endpoint argument
  (openLCA endpoint NW sets reproduce midpoint person-equivalents, ratios 0.995–1.001). With no endpoint
  reported it has nothing to attach to.
- **The NORMALIZATION half survives intact and 3.3.3 is built on it:** normalizing against ReCiPe's
  per-capita references puts *mineral resource scarcity at 0.0 %* of the profile and toxicity at ~90 %.
  That is a midpoint argument and stands alone.
- ⚠ Thiago challenged "ReCiPe World (2010)" as a version error. **He was wrong and the correction is
  mine, not his.** 2010 is the normalization REFERENCE YEAR; the method is 2016. Proof from his own
  file: §4.2.1 records the verified reference as *mineral resource scarcity 1.201 × 10⁵ kg Cu-eq*, and
  that category/unit exists only in ReCiPe 2016 (2008 used *metal depletion* in kg Fe-eq, which is the
  exact test §0.4 used on 2026-07-28). **My sentence invited the misreading; reword it, do not change
  the fact.**

# 🔴 CONTRADICTION IN THE FRAMEWORK, unresolved

§4.4 says the Monte Carlo net distribution "is assembled from them" and is "statistically clean here".
The 2026-07-27 changelog says `mc_net.csv` was "**ruled unusable for scenario comparison**".
Both cannot be current. With net abolished the fix is to report the two distributions separately, but
**the framework must stop contradicting itself.**

# ✅ 3.3 STRUCTURE AS BUILT (differs from the plan)

| § | Contents | State |
|---|---|---|
| **3.3** intro | why an LCA, the denominator argument, the Peitzmeier precedent, the three-part map | ✅ CLOSED, pasted |
| **3.3.1** Life cycle stages | functional unit + boundary folded into P1, five stages, four routes, claim boundary | ✅ CLOSED |
| **3.3.2** Building and quantifying | tools, provider rule, process vs product system, the five stages quantified, Equation 1 | ✅ CLOSED |
| **3.3.3** Impact assessment and uncertainty | EF 3.1, normalization/weighting, ReCiPe midpoint, the rejection, distributions, Monte Carlo | ✅ drafted, one reword pending |

⚠ **3.3.1a was deleted on Thiago's instruction.** He asked for no scope-and-limitations content in
3.3.1. The functional unit was NOT dropped with it: it is folded into the first paragraph, because an
LCA with no stated functional unit fails on its own terms. Three items moved out:
- the APOS partial-double-count declaration → the 3.6 limitations register
- the one percent cut-off criterion → the 3.6 limitations register
- the gross-above / avoided-below reporting convention → 3.3.3

⚠ **The motorsport 220 h maintenance caveat was CUT ENTIRELY.** Thiago is right: the paper already
carries it in three places, including Table 3's declined attributes, which name it as "maintenance
interval, a motorsport service regime that does not transfer". Do not reintroduce it.

# 🔴 NAMING COLLISION STILL LIVE IN THE .DOCX

The Methodology opener says **"This work around four stages, each depending on the one before"** and
"Each stage answers one of the four sub-questions". Those four stages are the work phases. 3.3.1 says the
life cycle has **five stages**, three pages later. Thiago said the legacy prototype-roadmap staging is
gone, and he is right about that, but this is a different, live use.
**Fix: call the four work items PHASES in the opener, reserve STAGES for the life cycle.** That sentence
is also missing its verb ("This work **is organized** around four stages").

# ✅ SIX FIGURES, all in Miro board "Master Thesis" (`uXjVGpjOAoU`), Figure 6 palette

| # | Frame | Contents |
|---|---|---|
| **7** | Methodology 3.3.1 - Figure 7 | System boundary, five stages, four routes. Routes 2–4 CONVERGE into one shared recovery block; Sc1 bypasses it; a single DASHED arrow leaves the boundary to "avoided primary production" |
| **8** | Methodology 3.3.2 - Figure 8 | The quantification pattern: four grouped background input classes → four foreground processes chained at 0.66 kg |
| **9** | Methodology 3.3.2 - Figure 9 | Materials and construction, all 17 datasets |
| **10** | Methodology 3.3.2 - Figure 10 | Hardware assembly |
| **11** | Methodology 3.3.2 - Figure 11 | Distribution and use, two panels |
| **12** | Methodology 3.3.2 - Figure 12 (verified) | End of life, 2 columns × 4 rows portrait |

⚠ **THREE Figure 12 frames exist.** Keep **"(verified)"**. The landscape one and the first portrait one
are superseded and must be deleted by Thiago; the connector cannot delete without his confirmation.

**Two design rulings that are corrections, not styling:**
1. Four parallel branches off Stage 5 would draw a model he did not build. Framework §1: the routes
   "differ primarily at the dismantling/separation step, holding downstream recovery technology
   constant". Routes 2–4 must converge.
2. The circular Canva version contradicts cradle-to-grave with avoided burden. The credit LEAVES the
   boundary; it does not return to this unit.

**Thiago's own ruling on the model graphs:** if one stage gets one, all do, for standard. Accepted. The
compromise offered (three sparse stages as panels of one figure) is what Figure 11 does.

# ✅ TWO NEW APPENDICES

- **Appendix III — Bill of materials.** Table III.1 components (17 rows) and Table III.2 materials
  (14 rows), each with its source tag, plus the sources list, declared exclusions and consistency checks.
- **Appendix IV — Life cycle inventory.** Eleven tables, one per process, five pages. Columns: Flow,
  Amount, Unit, Provider dataset and location, Identifier.

Both delivered as .docx. **The inventory was originally built as Appendix III and renumbered to IV** on
Thiago's instruction. The 3.3.2 prose and all figure captions must now point at **Appendix IV** for the
inventory and **Appendix III** for the BOM. Miro captions still say Appendix III and need updating.

⚠ **Correction made in Appendix III without asking:** `BOM_v4.md`'s header cites the reference as "data
sheet 245099915", which is the FILENAME. The appendix uses **234686731**, the number printed in the
document's own footer. **Fix the .md.**

# 🔴 THE openLCA EXPORT CORRECTED ONE OF MY OWN FIGURES

Thiago exported all eleven processes to .xlsx. Reconciling them against the figures I had built from the
text build logs found a real error, mine:

**Waste flows are OUTPUTS, not inputs.** `inert waste`, `waste plastic, mixture`, `electronics scrap`
and `waste electric and electronic equipment` all sit in the Outputs sheet, routed to their treatment
datasets. My first Figure 12 listed them as inputs. This is exactly the framework's own lesson from
2026-07-25: **"logs report intent, the database view verifies."**

Five more things the export settled:
1. **S2's reference output is `VCU assembled (S2)`.** It is recorded in no build log and no framework
   section; I had left it as a placeholder rather than guess it. Figures 10 and 11 patched.
2. **Sc3's gold credit is 8.66E-05 kg**, not the 8.7e-05 the build log rounded to.
3. Scenario reference outputs are named `VCU life cycle, Sc1 (no recycling)` and so on.
4. Credit processes have their own reference outputs: **0.3527 / 0.4086 / 0.4141 kg**. Useful in the
   defence: it shows a credit is a product in its own system, not a negative number inside the burden.
5. **Sc1 in the database carries the RULED split 0.5875 / 0.0725.** `s5_build_log.txt` still carries the
   superseded 0.528 / 0.132. **The log is the stale file; fix it.**

**The exports DO carry UUIDs** for both flows and providers, so the identifier column in Appendix IV is
real. Everything else in Figures 9–12 matched the logs to the digit.

# ✅ EVERY NUMBER IN 3.3 WITH ITS TRACE

**Functional unit** [P framework §2]: 1 VCU in a BEV in Germany, 15 years at 15,000 km/yr, 225,000 km.

**Stage inputs** [P build logs + xlsx export]:
- S1: 17 datasets. Gold 9.20E-05, silver 5.90E-05, palladium 5.50E-06 kg. Output `VCU materials (S1)` 0.66 kg
- S2: chain 0.66 kg + heat 1.03 MJ + electricity 0.47 kWh + SMT 0.0158 m². Output `VCU assembled (S2)`
- S3: chain + 0.323 t·km lorry >32 t EURO6. Output `VCU delivered (S3)`
- S4: chain + 66.2 kWh DE low voltage. Output `VCU used, 15 y (S4)`

**Die casting** [L, framework §3/S2]: Dalquist & Gutowski (2004), MIT working paper
LMP-MIT-TGG-03-12-09-2004, **Table 3**. 7.9 MJ/kg [6.5–9] split 3.0 MJ gas + 4.9 MJ electricity, applied
to the 0.344 kg housing. **Table 3's 14.9 MJ/kg "including loss" row deliberately NOT used**, because
ecoinvent's electricity markets already model grid losses; using it would double-count. `[FILL: page]`

**SMT 0.0158 m²** = the 150 × 105 mm board `[A + industry spec]`. Dataset reference unit is 1 m² of
mounted board, confirmed from the dataset description. Declared sensitivity upper bound 0.0315 m².

**🔴 EQUATION 1, the use phase.** Thiago asked for a formula and this is it:

> **E_use = (P · d) / (v · η_c · η_d)**

| Symbol | Value | Identification |
|---|---|---|
| P, own power draw | 0.009 kW | **[P] traced 2026-08-21: `ecu_ms_5-0_en.pdf`, p. 6, "Power consumption. (w/o loads) Approx. 9 W at 14 V"**. Range 0.009–0.020 `[A]` |
| d | 225,000 km | functional unit |
| v, average speed | 40 km/h | MiD 2017 `[CITATION NEEDED]`. Range 35–45 `[A]` |
| η_c, charging | 0.85 | Apostolaki-Iosifidou et al. (2017), *Energy* 127, 730–742 `[CITATION NEEDED]` |
| η_d, DC/DC to 12 V | 0.90 | `[A]` |

(0.009 × 225,000) / (40 × 0.85 × 0.90) = 2,025 / 30.6 = **66.2 kWh**.
✅ **The equation reproduces the recorded Monte Carlo band exactly**: pessimistic corner 189.1 kWh,
optimistic 54.2 kWh, against the framework's "≈54–189". So Equation 1 IS the formula that was run.
⚠ **The template's LIST OF EQUATIONS must now be kept**, not deleted.
⚠ The MS 5.0 manual is a THIRD Bosch document, not 2026a (data sheet) or 2026b (MS 50.4 manual). It
needs its own reference entry and year letter. p. 6 also gives that device as 140 × 109 × 40.5 mm and
650 g against the MS 50.4's 166 × 121 × 41 mm and ≤660 g, which is what makes the family proxy defensible.

**End-of-life parameters** [L, framework §3/S5]:
- Stream splits under bulk shredding: Chancerel et al. (2009), full-scale German plant, 27 t WEEE, no
  manual dismantling. Au and Pd 25.6 %, Ag 11.5 %, Cu 60 %
- Downstream yields: **Bigum et al. (2012), Table 8, p. 11**. Au/Pd 98 %, Ag 97 %, Cu 95 %, Fe 100 %,
  Al remelt 79 %, Al pre-treat 86 %
- Board arrival under guided disassembly 96 %: Lee et al. (2012)
- Tantalum credited at ZERO in every route, no route at scale: Cui & Zhang (2008). **Thesis headline**
- Whole boards accepted as smelter feed: Hagelüken (2006)
- Sc4 removal rates: Zhao et al. (2023) — Park 94 %, Marconi 100 % damage-free, Chen 39.73 % small SMDs
- Corroboration of Chancerel: Marra et al. (2018)

**BOM concentrations** [L]: Zhu et al. (2023), Table 1 — Au 230 ppm, Ag 430 ppm, Pd 40 ppm, Sn 2.8 % of
populated board. Ta 30–40 wt% of a Ta capacitor: Oke & Potgieter (2024) after Niu et al. (2017).

**Impact methods:** EF 3.1 = **Andreasi Bassi et al. (2023), EUR 31414 EN** (verified on disk as
`JRC130796_01.pdf`, 57 pp). ReCiPe 2016 Midpoint (H) = **Huijbregts et al. (2017)**, *IJLCA* 22(2),
138–147, DOI 10.1007/s11367-016-1246-y — **verified present in `export.bib`**. 16 EF categories,
18 ReCiPe categories, PEF ≥80 % screening rule, 13 exchanges with distributions, n = 1,000 × 7 systems.

# 🔴 THE BIGGEST NUMBERS ARE ASSUMPTIONS. Say so wherever they are cited.

Thiago's ruling: *"We can say that was assumptions, that is no problem. The problem is add number and
have no identification."* So the tagging rule itself goes into the prose, and these are named:
- **Housing 344 g**, over half the device mass — a geometry calculation, not a measurement
- Connector internal split and the **60 mg of connector gold**
- Dismantling electricity 0.01 and 0.02 kWh
- **Sc4 functional yield 0.70 [0.50–0.90]** — the framework says it is "nowhere quantified" in the
  literature — and substitution 0.80
- Residual combustible/inert split; Sn no central credit

# 🔴 OPEN ITEMS THIS SECTION CREATED

1. `[CITATION NEEDED]` **the LCA method authority.** ISO 14040/14044 are NOT on disk; all 14 index hits
   are citations inside other papers' reference lists. **Recommended and accepted: Pokhrel, Lin & Tsai
   (2020), *J. Environ. Manage.* 276, 111276, p. 2** — already in Mendeley as `Pokhrel2020`, DOI
   10.1016/j.jenvman.2020.111276, and it is itself an LCA of waste-PCB recycling, so it earns its place
   twice. Attribute the four phases to ISO and cite Pokhrel as where they were read.
   ⚠ `s13563-018-0160-0.pdf` is **Manhart et al., Mineral Economics**, a mining paper. Do NOT cite it
   for the method definition. (Its bib entry says `Manhart2019`; the PDF says 2018.)
   ⚠ **JRC130796_01.pdf does NOT define the LCA framework.** I recommended it before opening it; it is
   characterisation and normalisation factors only. It belongs in 3.3.3, not 3.3.
2. `[CITATION NEEDED]` **MiD 2017** (40 km/h) and **Apostolaki-Iosifidou et al. (2017)** (0.85). Both
   are in Equation 1 and neither is in the library. An untraced symbol in a printed equation is more
   visible than an untraced sentence.
3. `[FILL]` the page of Dalquist & Gutowski's Table 3.
4. `[FILL]` Peitzmeier's page is **p. 8** — Thiago dropped "(p. 8)" when pasting. **Restore it.**
5. ⚠ **S1 is marked "approved 2026-07-24, pending final human sign-off"** and its log still carries two
   `??` lines flagged REVIEW at checkpoint (the six power stages auto-linked to candidate [0], and the
   mounting dataset). Either close the sign-off or describe the choice as resolved and reviewed, not
   approved.
6. Re-verify **monotonicity on the SAVING column across all 25 categories**.

# ✅ PEITZMEIER: THE PRECEDENT IS AN OMISSION, NOT A METHOD

`Peitzmeier2025` = **`Driving circularity.pdf`**, real title *Driving circularity: An approach to
identify potentials for circular design of automotive electronics*, Proceedings of the Design Society
Vol. 5 (ICED25), DOI 10.1017/pds.2025.10153. **This closes the §3a "title field holds the journal name"
defect** — the real title is now recovered.

🔴 **They did NOT run an LCA.** p. 8 recommends that quantitative environmental impact assessment,
"including life cycle assessment, circularity metrics and value retention calculations", **be added** to
their approach as future work. That is their stated gap. A scaffold I wrote was about to claim they
"applied the same method" — caught only by opening the PDF. **Their omission is the thesis's
justification**, which is stronger than the false version.

`Alcoceba-Pascual2025` = **`evaluating_recyclability.pdf`**, *J. Cleaner Production* 513 (2025) 145725.
Method NOT confirmed; deliberately left out of the prose.

# ✅ VOICE AND PROCESS RULES CONFIRMED THIS SESSION

- **Thiago asked for SHORTER replies**: *"Be more assertive and directly in your answers when we are
  discussing, long text make me tired and I dont abosorve any of your ideas."* Two or three points, not
  ten. This is durable.
- The `academic-writer` skill must be LOADED before drafting. He caught prose written without it.
- `figure` means an image only. A draft opened "A figure that describes a saving…" in a thesis with
  numbered Figures. Banned rule, broken by me.
- No category-word subjects. "This section is divided into three parts" → "Three parts follow."
- He restores deleted `[CITATION NEEDED]` flags by deleting the sentence. **Deleting the flag does not
  close the gap.** He did this to the ISO sentence and the method lost its only authority.

Related: [[ch3_methodology_progress]], [[lca_findings_for_writing]], [[lca_method_sources]],
[[lca_scenario_source_audit]], [[lca_scope_verified]], [[thesis-schedule]],
[[voice_and_verification_rules]], [[bosch_sources_verified]]
