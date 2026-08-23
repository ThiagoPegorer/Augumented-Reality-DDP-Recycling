---
name: research-questions-final
description: The main research question and four sub-questions as rewritten 2026-08-13, why each differs from the signed proposal, the hypotheses decision, and the CORRECTED routes-versus-scenarios terminology rule (narrowed by the author on 2026-08-23).
type: project
---

# The questions as they now stand (section 1.4, closed 2026-08-13)

**Main:**

> How can an Augmented Reality-based Digital Product Passport assist a recycler in disassembling a
> Vehicle Control Unit and in assessing the environmental consequences of its end-of-life routes?

**Sub-questions:**

1. Which component-level and environmental information is most relevant for end-of-life
   stakeholders when handling a Vehicle Control Unit?
2. How can this information be delivered through an Augmented Reality interface that supports
   physical interaction with the product, including an interactive exploded view and per-component
   material and impact data?
3. How does an Augmented Reality-delivered passport compare with a conventional two-dimensional
   manual on task completion time, errors, and perceived usability?
4. What environmental difference results from guided dismantling compared with current bulk
   practice at end-of-life?

# 🔴 TERMINOLOGY: "SCENARIO", NOT "ROUTE". Narrowed by Thiago 2026-08-23.

**His ruling, verbatim:** *"the word 'scenario' will be the one to call the EoL scenarios, not
route. Route can be used in the UI/UX, or in the literature review for route of a material, but not
to call the scenarios modeled in the LCA model."*

**This SUPERSEDES the earlier rule recorded here**, which allowed "routes" for the same four things
in the Introduction, the prototype description and the Discussion. That rule is dead.

| Word | Where it is allowed |
|---|---|
| **scenario** | **The four modeled end-of-life constructs, Sc1 to Sc4, EVERYWHERE in the thesis** |
| **route** | UI and UX text only (the navigation rail's "route into the teardown"); a material's real-world route in the Literature Review (the copper route); a transport route (the 490 km distribution leg); and the verb, as in "ecoinvent routes all boards to shredding" |

⚠ **NEVER find-and-replace.** Three occurrences break: the 490 km transport route, the verb in
3.3.2, and the main research question above.

🔴 **THE MAIN RESEARCH QUESTION STILL SAYS "end-of-life routes" and it was deliberately NOT swept.**
It asks about the real pathways a recycler assesses, not about the model constructs. **Changing it
changes the registered question, and the 2026-08-13 revisions have no written supervisor agreement
yet.** Treat it as its own decision, never as part of a cleanup.

The full classified hit list, 25 paragraphs, is at
`MASTER THESIS/MAIN PAPER/route_to_scenario_checklist.md`, built 2026-08-23 from the .docx.
It splits into thirteen unambiguous changes, six needing a decision, and two protected UI uses.

⚠ **Claude caused this cleanup.** The 4.1 opening paragraph and several 3.3 passages drafted in
earlier sessions used "route" for the modeled scenarios. **Write "scenario" from now on.**

# What was registered, and what changed

Signed proposal, p. 4, dated 08/05/2026:

> Main: How can an Augmented Reality-based Digital Product Passport support the disassembly and
> recycling analysis of a Vehicle Control Unit?
> SQ1 component-level information · SQ2 intuitive AR interface · SQ3 3D exploded view improving
> understanding · SQ4 reducing dependence on intuitive or experience-based practice

| Change | Reason |
|---|---|
| Main question names **a recycler** and **environmental consequences** | The registered phrase "recycling analysis" was ambiguous. It now reads as the analysis a recycler performs, not the analysis the thesis performs |
| Registered SQ3 (exploded view) **absorbed into SQ2** | It is an interface feature, not a separate question. Nothing registered was deleted |
| Registered SQ4 (reducing dependence on experience) **absorbed into new SQ3** | Participants had no product experience to begin with, so a reduction cannot be measured |
| **New SQ4 added** (environmental difference) | Nothing registered asked an environmental question, and roughly a third of the thesis is an LCA |
| "intuitive" removed from SQ2 | A judgement word that would have to be measured |

**⚠ Action still open: email Saman Ghobadian for written agreement on these changes.** All four
narrow rather than expand the scope, but the agreement should exist before the defence.

**Supervisors (proposal p. i):** First Supervisor **Saman Ghobadian**; Professorial Supervisor
**Elle Langer**. Both belong on the title page.

⚠ **SQ4 names "current bulk practice", which is Sc2.** That is why the scenario comparison in 4.1
takes Sc2 as its reference rather than Sc1: measuring against Sc2 answers the registered question.

# Hypotheses: DROPPED, and the earlier note was wrong

`introduction_progress` previously said the template expects hypotheses. **That was a Claude error.**
The SRH chapter contract mentions hypotheses nowhere. FINDINGS asks for a transparent exposition of
the data. DISCUSSION asks what the results mean "with reference to the research objectives".
Dropping them also removes a post-hoc integrity risk, since all results were known before Chapter 1
was written. **Section title is "1.4 Research Questions", not "Research Questions and Hypotheses".**

Consequence: SQ3 must name its three measures inside the question text, since it no longer has
hypotheses carrying the specificity. Chapter 5 answers each sub-question in order.

# ⚠ Architecture fact confirmed 2026-08-13 (changes how the thesis should be framed)

**The prototype displays the openLCA results and per-component material amounts to the user.** The
LCA is therefore *passport payload*, not a parallel workstream. Consequences:

- SQ1's answer is a data model with two kinds of content, component and environmental.
- SQ4 produces the numbers the prototype shows. METHODOLOGY must say so, or the LCA reads as a
  second thesis stapled to the first.
- The logical work order is SQ1, SQ4, SQ2, SQ3. The presentation order is 1, 2, 3, 4. Section 1.4
  closes with a sentence making that dependency visible.
- **Open risk for the defence:** the prototype shows deterministic point values, while the framework
  states absolute footprints carry about ±20 % and only the scenario ranking is robust. One honest
  paragraph in DISCUSSION on what a passport should display to a non-specialist. That paragraph is
  itself a design recommendation and feeds the fifth objective.
  🔴 **This risk got sharper on 2026-08-23:** the deterministic climate value sits near the sixth
  percentile of its own Monte Carlo distribution. See [[lca_results_verified_ch4]].

# Recurring pattern in the author's edits, flag it every time

Three times he introduced a verb that presupposes a positive result: **"validate" the application,
"verify the efficiency", "increase the recycling rate"**. Each was caught and reverted. Watch for
"improve", "increase", "better", and "validate" entering any question, objective, or aim.
⚠ Related, 2026-08-23: he wrote "increasing the recycling, you increase the environmental impact",
meaning the AVOIDED impact. In a Findings chapter that sentence reads as recycling causing harm.

Related: [[introduction_progress]], [[lca_scope_verified]], [[registered_research_design]],
[[ch4_findings_progress]], [[lca_results_verified_ch4]]
