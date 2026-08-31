---
name: section-3-1-ai-tools-draft
description: "Draft of the new Methodology section 3.1 Use of generative AI tools, written 2026-08-28 by work type so it forward-references nothing. Four blocks, about 330 words, plus the two reference entries and the insertion order. A draft to rewrite, not to paste."
type: project
---

# 3.1 USE OF GENERATIVE AI TOOLS. Draft to rewrite.

⚠ **Insert this section LAST**, after the 204 review findings are applied. Every finding is keyed to
a section number and this insertion shifts all fourteen Methodology headings.
✅ Numbering is automatic (`numId 39` on the Heading styles), so the shift costs no manual edits.
⚠ **A paragraph declaring AI use that was written by AI is the one paragraph that must be yours.**

Written **by type of work**, not by section, so it forward-references nothing.

---

**3.1 Use of generative AI tools**

Generative artificial intelligence tools were used in three kinds of work in this thesis: the code of
the Augmented Reality application, the code that analyzed the results, and the text itself. Two
models from one vendor were used in Claude Cowork, [FABLE IDENTIFIER] between 9 July and 10 August
2026 for the application code, and claude-opus-5 from 10 August 2026 for the analysis code and for
the text.

The application code was produced inside a feedback loop set up by the author. The author defined the
application routines, the user experience and the user interface design, and the tool then generated
the C# scripts that implement those definitions. After each implementation the author tested the
build on the headset, corrected the small errors, and reported the larger ones back into the loop,
where the tool revised the script and returned it. The process continued until a result the author
judged satisfactory was reached (Anthropic, 2026a).

The code that analyzed the results was written the same way, and the data behind it is the author's
own. The author built the life cycle model in openLCA, ran the calculations and exported the results
as comma-separated files, and the author conducted the participant sessions and recorded the raw
responses and timings in a workbook that holds raw values only. Working from those files and to a
specification set by the author, the tool wrote the code that produced the result tables and the
figures drawn from them. Both notebooks carry verification routines: the life cycle notebook stops
execution where a table and its chart do not reconcile, and the participant notebook recomputes each
figure's table from the raw responses by a second route and halts on any disagreement
(Anthropic, 2026b).

The thesis text was worked out with the same tool. The author used it to extract the content
requirements of each chapter from the university template, to structure arguments, and to search and
analyze literature that the author then read in the original. Every source, number and table in this
work was verified by the author against the primary document or against the author's own project
files. In Chapters 4, 5 and 6 and in the last two paragraphs of the Abstract, formulations proposed
by the tool were reviewed, adapted and retained rather than rewritten in full (Anthropic, 2026b).

Responsibility for this work rests with the author. Every source cited, every argument made and every
formulation retained was checked and accepted by the author, and no output was adopted without
review.

---

# FIVE DECISIONS INSIDE THE DRAFT, each reversible

1. **The Figures 13 to 17 clause was dropped.** At 3.1 those figures are fifty pages away. Put it
   back if you want the concrete anchor: *"the application routines (described by the diagrams in
   Figures 13 to 17)"*. Forward figure references are legal, they are just far.
2. **"the figures drawn from them", not "every figure in the results chapter."** Figures 26 to 38 are
   your own headset screen captures and the tool did not produce them.
3. **The verification routines are stated without saying who required them.** They are checkable in
   the files, which is what makes the sentence worth having. Do not upgrade it to a claim about whose
   idea they were.
4. 🔴 **"Chapters 4, 5 and 6 and the last two paragraphs of the Abstract" is the labelling sentence.**
   The guidance lists transparency and labelling as separate obligations, and this is the only line
   that meets the second. **Confirm the list against what you actually pasted** and shorten it if you
   rewrote any of it.
5. **The closing paragraph echoes the guidance's own wording**, *"Students remain fully responsible
   for all results"*. It is the sentence a supervisor reads as the point of the section.

# THE TWO REFERENCE ENTRIES

> Anthropic. (2026a). *Claude* ([FABLE IDENTIFIER]) [Large language model]. Claude Cowork. Used
> 9 July to 10 August 2026 for the generation of C# scripts. https://claude.ai
>
> Anthropic. (2026b). *Claude* (claude-opus-5) [Large language model]. Claude Cowork. Used from
> 10 August 2026 for structuring and formulating the text and for the analysis code.
> https://claude.ai

⚠ Check the letters after the citation manager sorts the list. Both titles are "Claude". If opus
sorts first, a and b swap and all five in-text citations swap with them.

# WHAT COMES OUT WHEN THIS GOES IN

- The AI paragraph currently drafted for **3.4** is absorbed here. Remove it, or 3.4 and 3.1 drift
  apart on the next edit.
- No AI paragraph goes into 3.3.3 or 3.5. One place only.

# STILL OPEN

1. 🔴 The exact **Fable model string**, read off the app, used identically in the section and in the
   reference entry.
2. 🔴 Whether the **Unity sessions also ran in Claude Cowork**. Both entries say so.
3. 🔴 One email to Ghobadian about the affidavit sentence claiming the prompts are listed in the
   reference list, which the 15 July 2025 guidance says is not required.
4. ⚠ The **Heading 1 style on the "Figure 53" paragraph** where APPENDIX V belongs. He says the
   screenshot was replaced, but the heading style is still on that paragraph in the saved .docx.
   Fix it before the TOC refresh.
