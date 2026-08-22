# Session 38 — 2026-08-21 (closed 2026-08-22, 03:11 CEST)

**Focus:** Methodology section 3.3, Life cycle assessment, written and closed end to end.

## What was produced

- **3.3 intro** — why an LCA, the denominator argument, the Peitzmeier precedent, the three-part map
- **3.3.1 Life cycle stages** — functional unit and boundary in the first paragraph, five stages, four
  end-of-life routes, the claim boundary. Figure 7
- **3.3.2 Building and quantifying the model** — tools, provider-selection rule, process versus product
  system, the five stages quantified, **Equation 1**. Figures 8 to 12, Appendices III and IV
- **3.3.3 Impact assessment and uncertainty** — EF 3.1, the screening rule, ReCiPe midpoint, the
  documented rejection of ReCiPe normalization, uncertainty distributions, Monte Carlo
- **Six Miro figures**, all in board "Master Thesis" (`uXjVGpjOAoU`), Figure 6 palette
- **Appendix III** — bill of materials with source tags, delivered as .docx
- **Appendix IV** — life cycle inventory of eleven processes, delivered as .docx

## Two supervisor rulings that changed how results are reported

Neither requires recomputation. The model already keeps burdens and credits in separate product systems.

1. **The term "net" is abolished. "Gross" stays.** Report gross and avoided separately, plotted above and
   below one axis. Thiago's first proposal, to rename gross to "baseline" meaning Sc1, was withdrawn
   after the objection that gross is per-scenario while baseline would leave Sc2 to Sc4 with no word for
   their own burdens.
2. **ReCiPe 2016 Endpoint is dropped everywhere.**

## What this forces, and is not yet done

- The framework's FINAL RESULTS table is headed "Sc2 net / Sc3 net / Sc4 net", and **the core robustness
  claim, monotonic ordering across all 25 categories, is computed on net.** It must move onto the saving
  column and **monotonicity must be re-verified there.**
- Dropping the endpoint **kills half of §4.2.1**. The weighting rejection is an endpoint argument. The
  normalization rejection is a midpoint argument and survives; 3.3.3 is built on it.
- `LCA_framework_v4.md` needs five corrections: the net vocabulary in §0.3, §4.1 and the results table;
  the endpoint in §0.4 and §4.2; the §4.4-versus-changelog contradiction on `mc_net.csv`; the stale Sc1
  split in `s5_build_log.txt`; and 245099915 in the BOM header.

## What the openLCA export corrected, including one of my own figures

Thiago exported all eleven processes to .xlsx. Reconciling them against the figures found a real error.

**Waste flows are OUTPUTS, not inputs.** `inert waste`, `waste plastic, mixture`, `electronics scrap` and
`waste electric and electronic equipment` sit in the Outputs sheet, routed to their treatment datasets.
The first Figure 12 listed them as inputs and was rebuilt. This is the framework's own lesson from
2026-07-25: **logs report intent, the database view verifies.**

Five more things the export settled:

1. **S2's reference output is `VCU assembled (S2)`** — recorded in no build log and no framework section.
   It had been left as a placeholder rather than guessed. Figures 10 and 11 patched.
2. **Sc3's gold credit is 8.66E-05 kg**, not the 8.7e-05 the build log rounded to.
3. Scenario reference outputs are named `VCU life cycle, Sc1 (no recycling)` and so on.
4. Credit processes have their own reference outputs: 0.3527, 0.4086 and 0.4141 kg.
5. **Sc1 in the database carries the ruled 0.5875 / 0.0725 split.** `s5_build_log.txt` is the stale file.

The exports carry UUIDs for both flows and providers, so the identifier column in Appendix IV is real.
Everything else matched the logs to the digit.

## Equation 1, the use phase

> **E_use = (P · d) / (v · η_c · η_d)** = (0.009 × 225,000) / (40 × 0.85 × 0.90) = **66.2 kWh**

The 9 W own-draw value was traced first-hand: `ecu_ms_5-0_en.pdf`, **p. 6**, "Power consumption. (w/o
loads) Approx. 9 W at 14 V". That page also gives the device as 140 × 109 × 40.5 mm and 650 g against the
MS 50.4's 166 × 121 × 41 mm and ≤660 g, which is what makes the family proxy defensible. Worth the trace,
because a phantom 48 W rating produced a 3.3-fold error in the previous version of the model.

**The equation reproduces the recorded Monte Carlo band exactly**: 189.1 kWh at the pessimistic corner of
every range, 54.2 kWh at the optimistic one, against the framework's recorded "≈54 to 189 kWh". So it is
the formula that was actually run, not a reconstruction.

**Consequence:** the template's LIST OF EQUATIONS must now be kept, not deleted.

## Peitzmeier: the precedent is an omission, not a method

`Peitzmeier2025` is `Driving circularity.pdf`, real title *Driving circularity: An approach to identify
potentials for circular design of automotive electronics*, Proceedings of the Design Society Vol. 5
(ICED25), DOI 10.1017/pds.2025.10153. This recovers the real title, closing a defect flagged since
10 August.

**They did not run an LCA.** Page 8 recommends that quantitative environmental impact assessment,
"including life cycle assessment", be added to their approach as future work. A scaffold was about to
claim they applied the same method, and it was caught only by opening the PDF. Their omission is the
thesis's justification, which is stronger than the false version would have been.

## The method authority problem

**ISO 14040 and 14044 are not on disk.** All fourteen index hits are citations inside other papers'
reference lists. `JRC130796_01.pdf` was recommended as a substitute before it was opened; it is
Andreasi Bassi et al. (2023), characterisation and normalisation factors only, and defines no framework.

**Resolved:** cite **Pokhrel, Lin & Tsai (2020)**, *Journal of Environmental Management* 276, 111276,
p. 2, already in Mendeley as `Pokhrel2020`. It is itself an LCA of waste printed circuit board recycling,
so it serves twice. Attribute the four phases to ISO and cite Pokhrel as where they were read.

Also corrected in the academic-writer facts register: **Huijbregts et al. (2017) IS present** in
`export.bib`, contrary to the register's "MISSING ENTIRELY".

## Rules confirmed this session

- **No untagged numbers.** Thiago: *"We can say that was assumptions, that is no problem. The problem is
  add number and have no identification."* Assumptions are welcome; unidentified numbers are not.
- **Shorter replies.** *"Be more assertive and directly in your answers when we are discussing, long text
  make me tired and I dont abosorve any of your ideas."*
- **If one stage gets a model graph, all do**, for standard. His call, accepted.
- **Deleting a `[CITATION NEEDED]` flag does not close the gap.** The ISO sentence was removed when
  pasting and the method lost its only authority.

## Live defects still in the .docx

1. The opener's false sentence about the frozen version
2. The opener says "four stages" for the work phases while 3.3.1 says the life cycle has five
3. "This work around four stages" has no verb
4. Chapter 1: "The replica reproduces the geometry"

## Schedule

**21 August was to carry 3.3, 3.4, 3.5 and 3.6. It carried 3.3 alone.** So 22 August now carries 3.4,
3.5, 3.6 and the Results chapter, and 3.5 and Results are blocked by the same missing data: the
manual-condition timings, the study design, the interface feedback and the participant backgrounds.
