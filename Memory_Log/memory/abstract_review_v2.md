# ABSTRACT v2 — review of HIS rewrite, 2026-08-26

**341 words. Budget is 300. You are 41 over.**

---

# 🔴 1. THE AFFIDAVIT IS NOT DISCHARGED. Paragraphs 3 and 4 are still Claude's sentences.

You rewrote paragraphs 1 and 2. Those are now yours and the exposure on them is gone.

**Paragraph 3 is unchanged from my draft except for four words** ("Through the EF 3.1 method"
replacing "Under EF 3.1 characterization"). **Paragraph 4 is unchanged word for word.** Together
that is about 130 of your 341 words, and they still have to be denoted in the core text.

Those two paragraphs are also where the 41 words have to come from. Rewriting them in your own words
solves both problems in one pass.

---

# 🔴 2. TWO CLAIMS THAT OUTRUN YOUR OWN DATA

## 2.1 "a high yield of engagement with the Augmented reality application"

**You did not measure engagement.** It is not one of your three instruments, which are completion
time, errors and perceived usability. "High yield" is also a manufacturing metaphor doing a claim's
work.

Worse, it is the softened version of your real finding. You wrote "no difference in time, but a high
yield of engagement", which reads as a mild loss followed by a warm win. **Your actual finding is
that three measures disagree**, and that disagreement is the result Chapter 6 is built on.

**What you can say instead, all verified in `study_results_verified.md`:**

| verified count | source |
|---|---|
| seven of nine rated the application higher | usability_scores |
| **all nine said they would use it at a real workstation** | closing interview, 9 of 9 |
| **eight of nine named training as its use** | closing interview, 8 of 9 |
| five of the eight who ran both were slower with the headset | timing |
| no error occurred in either condition | Chapter 4.3 |

Those last two interview counts are stronger than "engagement" and they are yours. Note that they are
stated intentions, not measured behavior, so write "said they would use it".

Also: **"no difference in time" is not what you found.** There WAS a difference and it was against
the headset. What is true is that there is no *advantage* in time. Write "no advantage".

## 2.2 "one of the main reasons that make the recovery rates of critical raw materials so low"

This is a **causal ranking** and nobody measured it. Restrepo et al. (2019, p. 2) report that the
composition of vehicle electronics is poorly understood and that this **hampers the development of
recycling strategies**. That is an obstacle claim. It is not "one of the main reasons rates are low".

This is the same defect as the "increase recycling rates" verb, turned around and stated as a cause.
Ninth catch. **Write the obstacle, never the ranking.**

---

# 🔴 3. ONE SENTENCE CONTRADICTS YOUR OWN INTERVIEW RESULT

> "It display component-level information and environmental impact data **to increase transparency
> and better understanding of the product in its End-of-Life**."

`study_results_verified.md`, closing interview counts, verbatim:
**"material and recovery information did not help the task 8 of 9"**.

Eight of your nine participants said that data did not help them. Claiming on the abstract page that
it increases understanding is contradicted on your own Chapter 4 page. **State what the application
displays. Do not state what it achieves.** Cutting the clause fixes it and saves eight words.

---

# ⚠ 4. YOU LOST THE REGISTERED QUESTION WORDING

You wrote: "assessing the environmental impact of it".
Your registered main question: "assessing the environmental consequences of its **end-of-life
routes**".

"routes" here is deliberate and is the one place the word survives the scenario ruling. Put the
phrase back. "the environmental impact of it" is also vague: impact of what, the device or the
disassembly?

---

# ✅ 5. WHAT YOU IMPROVED ON MY VERSION, and should keep

1. **The black box opening.** It is a metaphor, which my own voice rules distrust, but you gloss it
   in the same sentence, so it explains rather than substitutes. It is the most memorable line on the
   page and it is yours. Keep it.
2. **The architecture.** You gave each study its own paragraph, complete with its method and its
   result. That is better than my version, which put both methods together and both results together.
3. **The hardware and the reference product named.** PICO 4 Ultra and Bosch MS 50.4 belong on this
   page and I had left them out.

---

# PARAGRAPHS 1 AND 2, YOUR SENTENCES, GRAMMAR CORRECTED

> A device at its end of life is a black box: the recycler has no knowledge of what sits inside it.
> Poor information flow between the stakeholders in an electronic device's life cycle is a known
> obstacle to recovering the critical raw materials it contains. This thesis examines how a Digital
> Product Passport delivered through Augmented Reality can assist a recycler in disassembling a
> Vehicle Control Unit and in assessing the environmental consequences of its end-of-life routes.
>
> ReBuilt is the prototype developed in this thesis. It is a PICO 4 Ultra application that anchors
> the Digital Product Passport beside the physical unit and displays component-level and
> environmental impact data. A guided step-by-step disassembly runs on a 3D printed teardown model,
> designed with a Bosch MS 50.4 Vehicle Control Unit as its reference product. Nine participants used
> the application against a two-dimensional manual. Five of the eight who ran both conditions were
> slower with the headset and no error occurred in either condition, while seven of the nine rated
> the application higher, all nine said they would use it at a real workstation, and eight named
> training as its use.

**About 210 words.** That leaves **roughly 90 words** for paragraphs 3 and 4 combined. Yours currently
run 130, which is exactly your overrun.

## The grammar fixed

| yours | corrected |
|---|---|
| "in it End-of-life stage" | "at its end of life" |
| "no knowledge what sits inside of it" | "no knowledge of what sits inside it" |
| "a Life Cycle of a electronic device" | "an electronic device's life cycle" |
| "deployed in an Augmented Reality" | "delivered through Augmented Reality" |
| "the environmental impact of it" | "the environmental consequences of its end-of-life routes" |
| "It's a PICO 4 ultra base" | "It is a PICO 4 Ultra application" |
| "besides the physical unit" | "beside the physical unit" |
| "It display" | "It displays" |
| "modeled using a Vehicle Control Unit Bosch MS 50.4 as product reference" | "designed with a Bosch MS 50.4 Vehicle Control Unit as its reference product" |
| "is created to assist" (tense clash with "was conducted") | "runs on" |
| "A test using nine participants was conducted to test" | "Nine participants used" (test/test repetition) |
| "End-of-life" / "End-of-Life" / "end-of-life" all in one page | one form: **end-of-life**, lowercase, hyphenated as a modifier |
| "Augmented reality" / "Augmented Reality" | one form throughout |

---

# WHAT TO WRITE FOR PARAGRAPHS 3 AND 4, in your own words, about 90 words

Do not copy my sentences again. These are the facts, not the phrasing:

**Paragraph 3, the LCA.** Where the environmental data comes from: a cradle-to-grave life cycle
assessment of the reference unit, built in openLCA on ecoinvent, four end-of-life scenarios, EF 3.1
characterization, Monte Carlo uncertainty, ReCiPe 2016 midpoint cross-check. Then the result: guided
dismantling avoids 3.22 times as much resource use, minerals and metals as bulk shredding and 1.85
times as much climate impact; adding component reuse gives 4.74 and 3.49.
🔴 One sentence must say the gross burden is the same in all four scenarios and only the avoided
primary production changes. Without it, the four multiples read as an impact reduction, which your own
Table 8 contradicts.
⚠ If you are short of words, the ReCiPe cross-check clause is the one to cut.

**Paragraph 4, the close.** Two things, both needed. What the work shows: the record can be structured
and read on the object itself. What it does not show: that reading it there makes disassembly faster
or recovers more material. The second half is what makes the first believable.

---

=== Check before using ===

**Verified this turn, not recalled:** the 8-of-9 interview count and the 9-of-9 and 8-of-9 counts were
read from `study_results_verified.md` in this session.

**Needs a decision from you:** whether to keep the two interview counts. They are your strongest honest
positive and they reframe the tool as a training aid rather than a speed tool, which is what your data
actually supports. They also cost about 25 words.

**Still missing:** keywords. Not counted in the 300, and the template offers them as optional.
