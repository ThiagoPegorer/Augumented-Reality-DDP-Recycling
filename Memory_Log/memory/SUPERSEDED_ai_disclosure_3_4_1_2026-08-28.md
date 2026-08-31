# THE AI DISCLOSURE FOR SECTION 3.4.1
Built 2026-08-28. His decision: the paragraph goes in **3.4.1 AR Architecture**. Declared scope:
**whole C# scripts generated and then device-tested**, plus **debugging of his own code**.

⚠ **If you paste my sentences, the paragraph declaring AI use is itself AI-written and needs its own
denotation.** The clean solution is the scaffold below: you write the sentences, I supply the
citation and reference strings, which are mechanical and carry no authorship.

## 1. THE SCAFFOLD. Four sentences, one job each.

| # | what the sentence must carry | what it must not do |
|---|---|---|
| 1 | name the tool, the model identifier and the vendor | no evaluation of the tool |
| 2 | state plainly what it produced: complete C# scripts to your specification, and corrections to scripts you had written | no proportion you cannot evidence |
| 3 | state your control: every script reviewed, built and tested on the PICO 4 Ultra before it was kept, failures returned for correction | no claim that you wrote what you did not |
| 4 | state the period and the boundary: how many development sessions, between which dates, and what remained yours | no apology, no justification |

## 2. A DRAFT TO REWRITE, not to paste

> The application was developed with the assistance of a generative artificial intelligence tool.
> Claude (claude-opus-5, Anthropic) produced complete C# scripts to specifications set by the author
> and corrected errors in scripts the author had written. Every script was reviewed, built into the
> project and tested on the PICO 4 Ultra before it was kept, and any behavior that failed on the
> device was returned for correction. The work ran across [N] development sessions between 9 July
> and 10 August 2026, and every design decision and every accepted build is the author's
> (Anthropic, 2026).

**[N] is yours to fill.** The Notion session log carries eleven candidate rows: Sessions 04, 06, 07,
20, 22, 23, 24, 25, 26, 27 and the RBv2.0 rebuild row, dated 2026-07-09 to 2026-08-10.

⚠ **The last clause is the one to check.** "Every design decision is the author's" is a claim about
your own work that I cannot verify, and several interface and interaction proposals in the session
record originated on my side and were then accepted or rejected by you. **Flag-free alternative that
is true either way:**
> ...and every build was accepted only after the author verified it on the device.

## 3. THE IN-TEXT DENOTATION

Short form, matching your APA secondary-source style: **(Anthropic, 2026)**.
Place it at the end of the paragraph above. Place it again anywhere else in the thesis where
retained AI-written code or text stands, per the affidavit.

## 4. THE REFERENCE-LIST ENTRY

APA 7 base form:
> Anthropic. (2026). *Claude* (claude-opus-5) [Large language model]. https://claude.ai

🔴 **This is not yet sufficient.** The SRH affidavit, p. 163, requires each use listed with **the
name of the tool, its version number, the date of retrieval, and the prompts used.** A prompt set
across roughly thirty sessions does not fit in a reference list.

**Two readings, and you must ask your supervisor which one applies:**

| reading | what you do | risk |
|---|---|---|
| literal | every prompt printed in the reference list | unusable, and it would run to many pages |
| workable | one reference entry naming the tool, the model, the retrieval range and pointing to a new appendix that carries the dated prompt log | needs supervisor agreement, and a new appendix |

The workable form:
> Anthropic. (2026). *Claude* (claude-opus-5) [Large language model]. Prompts and dates of use are
> listed in Appendix X. https://claude.ai

**Email Saman Ghobadian and Elle Langer before submission.** Do not decide this alone on a sworn
document.

## 5. WHAT IS STILL OPEN, and only you can close it

1. 🔴 **`ai_use_log.md` has no entry for any Unity session.** The log begins 2026-08-10; the build
   ran 2026-07-09 to 2026-08-10 across the eleven Notion rows named above. **The prompts are
   recoverable verbatim from the Cowork transcripts. Do not reconstruct them from memory.**
2. 🔴 **Sessions 41, 42 and 43 still have no prompts.** Same source, same rule.
3. ⚠ **The addendum ticks "Generation of code ☒"** and nothing in the reference list or the log
   supports it yet. Closing item 1 closes this too.
4. ⚠ **The affidavit's "Location, Date" line is unfilled.**
5. ⚠ **Two more code artifacts were not covered by your answer:** `ARDPP_study_analysis.ipynb`,
   which produces Tables 20, 21 and 22 and Figures 39 to 44, and `LCA_explorer.ipynb`, which
   produces Figures 18 to 25. Both are AI-written code that generated **published results**. Decide
   whether they fall under the same disclosure. I think they do.

## 6. THE VISUALIZATIONS QUESTION, reopened once

You ruled: leave "Generation of visualizations ☐" unticked, because the figures render your own data.
**That reason protects the charts. It does not reach thirteen of your figures.**

Figures 5 and 6 (Session 37), Figures 7 to 12 (Session 38) and Figures 13 to 17 (Session 39) were
authored as vector files in Miro and pushed to your board. **Figure 7, the system boundary, and
Figures 13 to 17, the AR architecture, the three routines and the canvas plan view, contain no data
at all.** They are conceptual diagrams, and the checklist item asks who generated the visualization.

Charts built from your verified numbers are a defensible "no". Thirteen authored diagrams are not
the same object. **Your call, but make it on the right fact.**

---

# UPDATE, same day. HIS PARAGRAPH, HIS PLACEMENT.

He wrote the paragraph himself and put it in **3.4, not 3.4.1**. ✅ **His placement is better than mine
and the reason is his own text:** the paragraph cites Figures 13 to 17, which span 3.4.1 (Figure 13,
the architecture) and 3.4.2 (Figures 14 to 17, the three routines and the canvas plan). A paragraph
whose scope crosses two subsections belongs in the section opener.

**Insertion point:** in 3.4, after the paragraph ending *"…the way the interface responds to a hand"*
and **before** *"This section explains how RBv2.1.1 was built."*

## 🔴 THE MODEL NAME. Resolve this before anything else.

| source | identifier |
|---|---|
| his new paragraph | **Claude Fable 5** / **claude-fable 5** |
| `ai_use_log.md`, every entry | **claude-opus-5**, 19 times. "fable" appears **0 times** |
| this session's configuration | **claude-opus-5** |

Two names for one tool inside one sworn submission. **I cannot resolve my own identifier reliably
and must not be the source for it.** He must open the model selector in Cowork, copy exactly what it
prints, and make the paragraph, the log's 19 entries and the reference entry all say that one string.

## THE CORRECTED PARAGRAPH, grammar only. His sentences, his structure.

> The application was developed with the assistance of a generative artificial intelligence tool. The
> author used the model [MODEL NAME] in Claude Cowork ([model identifier], Anthropic). A feedback
> loop was set up by the author, in which the author defined the application routines (described by
> the diagrams in Figures 13 to 17), the user experience and the user interface design, and the AI
> tool then generated the C# scripts that implement those definitions inside the software chosen to
> develop the AR application. After each implementation the author tested the build on the device,
> corrected the small errors, and reported the larger ones back into the feedback loop, where the AI
> tool revised the script and returned it. The process continued until a result the author judged
> satisfactory was reached (Anthropic, 2026).

**Ten corrections, all grammar or agreement:**
`with the with the` doubled · `Claude Fable 5` and `claude-fable 5` inconsistent in one sentence ·
`describe by the diagrams on Figure 13 to Figure 17` → `described by the diagrams in Figures 13 to
17` · `setup` as a verb → `set up` / `defined` · `generates` → `generated`, tense · `the software
choose` → `the software chosen` · `then debug small mistakes and big mistakes was pointed out` →
split into two parallel actions with agreement fixed · missing period after `to the author` ·
`continues` → `continued` · `a satisfy result … was achieve` → `a result the author judged
satisfactory was reached`.

**One thing kept deliberately:** his distinction between the small errors he fixed himself and the
larger ones returned to the loop. It is the strongest sentence in the paragraph for authorship,
because it is the one that shows judgment rather than acceptance.

**`(Anthropic, 2026)` added at the end.** That is the denotation the affidavit requires, and without
it the paragraph is a description rather than a declaration.

## 🔴 THE PROMPTS. He ruled they are not needed. I disagree, and the reason is his signature.

His position: *"I don't need to show or attach all the prompts to the paper, just explain how I use
the AI tool."*

The affidavit he signs, p. 163, states as fact: *"In my list of references, each use of generative AI
has been listed according to the citation style I used for full references, including the name of the
tool, its version number, the date of retrieval, **and the prompts used**."*

That sentence is not conditional and it is not about the addendum. It is above his signature, on a
document whose own Legal Caution page names imprisonment for a knowingly false affidavit.
**An explanation of how the tool was used is not a list of prompts.**

**Three ways out, all better than signing as it stands:**
1. Ask Ghobadian or Langer in writing whether an explanation plus an appendix of representative
   prompts satisfies the clause. Keep the reply.
2. Print a representative prompt per use category, not all thirty sessions, in a new appendix, and
   point the reference entry at it.
3. If neither, the affidavit sentence is false as written and he should say so to his supervisor
   before submitting, not after.

**This is the one item in the whole review I will not soften.**

## THE MIRO FIGURES. His argument improved, and it now contradicts his code ruling.

His new reason: *"you developed, but you follow my instructions and my data, not yours."* That is a
better argument than the first one, which only covered data, and it is the correct test: authorship
of the content against operation of the drawing tool.

🔴 **But the same test applies to the C# scripts, and he is ticking those.** His own paragraph says
he defined the routines, the user experience and the interface, and the tool implemented them. If
"generated to my instructions" means the visualizations are not AI-generated, then the code is not
either and `Generation of code ☒` should be unticked. If instructing the tool still counts as
generation for code, it counts for Figures 5 to 17.

**One rule, applied to both.** Whichever he picks, the addendum and the log must agree with it.

---

# CORRECTION, same day. THERE ARE TWO TOOLS, NOT ONE.

**He supplied new information and it withdraws my model-name objection.** `ai_use_log.md` covers the
**thesis writing only**, from 2026-08-10, on **claude-opus-5**. The **Unity application** was built
earlier on a **different model, Fable 5**. The log and his paragraph never contradicted each other,
because they describe different tools over different periods. I was wrong to read it as one tool.

**The clean split, and it is defensible:**

| period | work | model | record |
|---|---|---|---|
| 2026-07-09 to 2026-08-10 | the ReBuilt C# scripts, eleven Notion sessions | **Fable 5** | ❌ not logged anywhere |
| 2026-08-10 onward | the thesis text | **claude-opus-5** | ✅ `ai_use_log.md`, 19 entries |

## WHAT THIS CHANGES

1. **Two reference entries, not one.** APA sorts same author and year by the parenthetical, so `f`
   before `o`:
   > Anthropic. (2026a). *Claude* (claude-fable-5) [Large language model]. Claude Cowork. Used
   > 9 July to 10 August 2026 for the generation of C# scripts. https://claude.ai
   >
   > Anthropic. (2026b). *Claude* (claude-opus-5) [Large language model]. Claude Cowork. Used from
   > 10 August 2026 for structuring and formulating the text. https://claude.ai
2. **The 3.4 paragraph cites (Anthropic, 2026a)**, not the bare year. The writing disclosures in
   Chapters 4, 5, 6 and the Abstract cite **(Anthropic, 2026b)**.
3. **`ai_use_log.md` needs a Part B** for the Unity period, with the Fable identifier, the eleven
   session dates and the prompts. Its current preamble says the file is the record behind the
   addendum, which is now only half true.
4. 🔴 **The exact model string is still unverified.** "Claude Fable 5", "claude-fable 5" and
   "claude-fable-5" are three spellings of one identifier. He must copy what the model selector
   printed and use that one string everywhere.

## OPEN QUESTION FOR HIM

Were the Unity sessions also run in **Claude Cowork**, or in a different surface? The reference
entries above say Cowork for both. If the Unity work ran somewhere else, that field changes and the
addendum's tool list may need a second line.
