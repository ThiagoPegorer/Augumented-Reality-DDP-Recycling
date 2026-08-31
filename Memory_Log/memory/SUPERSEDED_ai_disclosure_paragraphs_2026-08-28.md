---
name: ai-disclosure-paragraphs
description: "The four AI-use disclosure paragraphs for the Methodology, written 2026-08-28 against the SRH Guidance framework of 15 July 2025. Placement, the facts each must carry, drafts to rewrite, and the two reference entries. The 3.4 paragraph is his and already written."
type: project
---

# THE FOUR DISCLOSURES. All in Methodology, none in Results.

The SRH guidance, p. 2, sanctions this placement: *"in a separate methodology section, or in the
research design, in footnotes or in a separate appendix"*. It also settles the prompt question:
*"Complete documentation of each individual prompt is not required."*

| # | section | placement | covers | model |
|---|---|---|---|---|
| 1 | **3.1 Research design** | last paragraph | the thesis text | claude-opus-5, from 2026-08-10 |
| 2 | **3.3.3** | last paragraph | the LCA analysis and its charts | claude-opus-5 |
| 3 | **3.4** | after the version paragraph, before "This section explains how RBv2.1.1 was built" | the C# scripts | **Fable 5**, 2026-07-09 to 2026-08-10. ✅ HIS, already written |
| 4 | **3.5 Prototype Test** | last paragraph | the participant data analysis | claude-opus-5 |

⚠ **Each is a draft to rewrite, not to paste.** A paragraph declaring AI use that was written by AI
is the one paragraph in the thesis that has to be the author's own sentences.

# 1. SECTION 3.1, THE THESIS TEXT

**Facts it must carry:** the tool and model · the period · what it did (chapter content contracts
from the template, argument structure, literature search and analysis) · that he read every source
in the original and verified every number against his own files · 🔴 **which parts of the text carry
retained AI formulation.** The guidance lists transparency and labelling as two separate mandatory
items, and naming the chapters is the labelling.

> The thesis text was produced with the assistance of a generative artificial intelligence tool. The
> author used the model [MODEL] in Claude Cowork ([identifier], Anthropic) from 10 August 2026, to
> extract the content requirements of each chapter from the university template, to structure
> arguments, and to search and analyze literature that the author then read in the original. Every
> source, number and table in this work was verified by the author against the primary document or
> against the author's own project files. In Chapters 4, 5 and 6 and in the last two paragraphs of
> the Abstract, formulations proposed by the tool were reviewed, adapted and retained rather than
> rewritten in full (Anthropic, 2026b).

🔴 **Confirm the chapter list against what you actually pasted.** The project record says Chapters 4,
5, 6 and Abstract paragraphs 3 and 4. If you rewrote any of them since, shorten the list. If you
would rather rewrite those passages than label them, that is cleaner academically and there is no
time for it before Monday.

# 2. SECTION 3.3.3, THE LIFE CYCLE ANALYSIS

**Facts it must carry:** the model is his, built and calculated in openLCA and exported by him · he
specified what each table and figure had to show · the tool wrote the code that read the exports ·
he checked every published value against the exports · the notebook halts on a mismatch.

> The results of the life cycle model were analyzed with the assistance of the same tool. The author
> built the model in openLCA, ran the calculations and exported the results as comma-separated
> files. The author then specified which comparison each table and each figure had to show, and the
> tool wrote the Python code that read those files and produced them. Every published value was
> checked by the author against the exported files, and the analysis notebook carries verification
> cells that stop execution where a table and its chart do not reconcile (Anthropic, 2026b).

⚠ **Adjust if you wrote any of the notebook cells yourself.** The draft credits all the code to the
tool.

# 3. SECTION 3.5, THE PARTICIPANT DATA

**Facts it must carry:** he ran the sessions and recorded the raw data · the workbook holds raw
values only · the tool wrote the analysis and figure code to his specification · the audit routine
recomputes every figure's table from the raw by a second route and halts on disagreement · he
verified against it.

> The responses collected from the participants were analyzed in the same way. The author conducted
> the sessions and recorded the raw responses and timings in a workbook that holds raw values only.
> To a specification set by the author, the tool wrote the code that derives every reported statistic
> and draws every figure from that workbook. An audit routine recomputes each figure's underlying
> table from the raw responses by a second route and halts on any disagreement, and the author
> verified the reported values against it (Anthropic, 2026b).

# THE TWO REFERENCE ENTRIES

> Anthropic. (2026a). *Claude* ([Fable identifier]) [Large language model]. Claude Cowork. Used
> 9 July to 10 August 2026 for the generation of C# scripts. https://claude.ai
>
> Anthropic. (2026b). *Claude* (claude-opus-5) [Large language model]. Claude Cowork. Used from
> 10 August 2026 for structuring and formulating the text and for the analysis code.
> https://claude.ai

⚠ **Check the letters after Mendeley sorts the list.** APA assigns a and b by the order the entries
end up in, and both titles are "Claude". If the sort puts opus first, the letters swap and every
in-text citation swaps with it.

# WHAT THIS CLOSES AND WHAT IT DOES NOT

| guidance obligation | state after these four paragraphs |
|---|---|
| tools named | ✅ both, once the Fable string is confirmed |
| documentation of how AI was used | ✅ four paragraphs in Methodology |
| labelling of AI-supported content | ✅ the last sentence of the 3.1 paragraph |
| prompts | ✅ not required |

⚠ Still open: the exact Fable model string, and the one email to Ghobadian about the affidavit
sentence that says prompts are listed in the reference list.

The notebooks will not be published. Decision 2026-08-28: they go on the USB drive for the
examination office as working files, where drafts are acceptable. The 36 uses of "net" in
`LCA_explorer.ipynb` are therefore deferred, not fixed.
