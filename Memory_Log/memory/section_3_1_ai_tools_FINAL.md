---
name: section-3-1-ai-tools-FINAL
description: "CLOSED 2026-08-28. The final agreed text of Methodology section 3.1 Use of generative AI tools, his words with grammar corrected. Also the two reference entries and the four items still open around it. This supersedes section_3_1_ai_tools_draft.md and ai_disclosure_paragraphs_2026-08-28.md."
type: project
---

# 3.1 USE OF GENERATIVE AI TOOLS. CLOSED, his text.

🟢 **He wrote this section himself.** Claude corrected grammar only, and restored two things he had
dropped: the labelling sentence naming the chapters, and the verification sentence. Everything else
is his wording and his structure.

⚠ **INSERT LAST**, after the 204 review findings are applied. Every finding is keyed to a section
number and this insertion shifts all fourteen Methodology headings.
✅ Numbering is automatic (`numId 39` on the Heading 1, 2 and 3 styles), so the shift costs no manual
edits. Refresh the TOC and both lists after.

---

**3.1 Use of generative AI tools**

Generative artificial intelligence tools were used in three kinds of work in this thesis: as a code
assistant for the Augmented Reality application; as a code assistant in the generation of charts,
both from the author's own openLCA results and from the Google Forms answers of the voluntary
participants; and as an assistant in developing the thesis paper. The AI models were provided by
Anthropic under the product name Claude. Two models were used in Claude Cowork: Fable 5, between
9 July and 10 August 2026, for the application code, and Opus 5, from 10 August 2026, for the
analysis code and the text.

The Augmented Reality application code was produced inside a feedback loop set up by the author. The
author defined the application routines, the user experience (UX) and the user interface (UI) design.
The tool generated the C# scripts that implement those definitions. After each implementation the
author tested the build on the headset, corrected the small bugs, and reported the larger ones back
into the loop, where the tool revised the script and returned a corrected one. The process continued
until a result the author judged satisfactory was reached (Anthropic, 2026a).

The code assistance in the generation of charts followed the same procedure. The author built the
life cycle model in openLCA, ran the calculations and exported the results as .csv files. The author
then created a Jupyter Notebook in Python using the pandas library, and the AI tool assisted in
deriving the reported values and creating the charts. For the results of the voluntary participant
tests, the author exported the responses as .xlsx files and created a second Jupyter Notebook in
Python using the pandas library, where the AI tool assisted in the same way. Both notebooks carry
verification routines that halt execution where a reported value and its source do not reconcile
(Anthropic, 2026b).

In the development of the thesis paper, the author used the AI tool to extract the content
requirements of each chapter provided by the university template, to assist in structuring arguments,
and to assist in the analysis of literature previously read by the author. Every source, number,
table and figure in this work was verified by the author against the primary document or against the
author's own project files. In Chapters 4, 5 and 6 and in the last two paragraphs of the Abstract,
formulations proposed by the tool were reviewed, adapted and retained rather than rewritten in full
(Anthropic, 2026b).

The author is responsible for this work. Every source cited, every argument made and every
formulation retained was checked and accepted by the author, and no output was adopted without
review.

---

# THE TWO REFERENCE ENTRIES

> Anthropic. (2026a). *Claude* ([FABLE IDENTIFIER]) [Large language model]. Claude Cowork. Used
> 9 July to 10 August 2026 for the generation of C# scripts. https://claude.ai
>
> Anthropic. (2026b). *Claude* (claude-opus-5) [Large language model]. Claude Cowork. Used from
> 10 August 2026 for structuring and formulating the text and for the analysis code.
> https://claude.ai

⚠ Check the letters after the citation manager sorts the list. Both titles are "Claude". If opus
sorts first, a and b swap and all four in-text citations swap with them.

# WHAT COMES OUT WHEN THIS GOES IN

The AI paragraph drafted for **3.4** is absorbed here. Remove it, or the two drift apart. Nothing
goes into 3.3.3 or 3.5. **One place only.**

# 🔴 FOUR ITEMS STILL OPEN

1. **The Fable model string.** Written five ways across the session: "Claude Fable 5",
   "claude-fable 5", "claude-fable-5", "Fable 5", "Opus 5" for the other. **Read the model selector,
   copy one string, use it in the section and in the reference entry.**
2. **Whether the Unity sessions also ran in Claude Cowork.** Both reference entries say so.
3. **One email to Ghobadian** about the affidavit sentence claiming the prompts are listed in the
   reference list, which the 15 July 2025 guidance says is not required. One line, keep the reply.
4. **The Heading 1 style on the "Figure 53 - Manual disassembly prototype…" paragraph**, where
   APPENDIX V belongs. He replaced the screenshot but the heading style is still on that paragraph
   in the saved .docx. **It will hijack the APPENDIX V line the moment the TOC is refreshed.**

# TWO SENTENCES HE DROPPED AND CLAUDE RESTORED, both on the record

- **The labelling sentence**, naming Chapters 4, 5, 6 and the last two Abstract paragraphs. The SRH
  guidance lists transparency and labelling as separate mandatory items and this is the only line
  meeting the second. Third time a caveat of this kind has come out of a draft. Verification rule E.
- **The verification sentence** about the notebooks. It is the only checkable claim in the section
  and a supervisor can confirm it in thirty seconds.

# ✅ THE PROMPT QUESTION IS CLOSED, and he was right

SRH *Guidance framework for students on the use of generative AI*, 15 July 2025 v1.0, p. 2, verbatim:
*"Complete documentation of each individual prompt is not required. Instead, the context of use
should be described in a concise and comprehensible manner so that the author's own contribution
remains recognisable."* The same page names **"a separate methodology section"** as the first
suitable place, which is exactly what 3.1 is. `ai_use_log.md` stays private as his own record.

Related: [[thesis_review_pass_2026-08-28]], [[thesis-schedule]], [[voice_and_verification_rules]]
