---
name: voice-and-verification-rules
description: The complete set of voice rules and verification rules learned from Thiago's edits, as of 2026-08-22. ELEVEN voice rules and four verification rules plus the chapter-boundary rule. READ THIS FILE BEFORE WRITING ANY THESIS PROSE. Rules 10 and 11 and verification rule D are new on 2026-08-22 and are absent from the synced academic-writer skill.
type: reference
---

# 🔴 WHY THIS FILE EXISTS

⚠ **The synced skill files are a READ-ONLY CACHE.** Editing
`/root/.claude/skills/synced/academic-writer/references/voice_rules.md` does not change Thiago's
account skill, and the session filesystem is discarded at close. **New rules go in THIS file.**

**The synced copy is behind this file on five things**, so this file remains authoritative:

| Item | Synced skill | This file |
|---|---|---|
| Rule 5, sentence length | "a hard limit, not a preference" | "a check, not an aspiration", with the block-2.4 boundary that the limit targets *stacked clauses*, not length |
| **Rule 10, cut the scaffolding** | **absent** | **present, 2026-08-22** |
| **Rule 11, no headline openers in Methodology** | **absent** | **present, 2026-08-22** |
| **Verification rule D, flags get a flag-free twin** | **absent** | **present, 2026-08-22** |
| Verification rules B and C, and the chapter-boundary rule | absent | present |

Read both. Where they disagree, **this file wins**.

# THE ELEVEN VOICE RULES

### 1. No category-word subjects, and no metaphor standing in for the claim
Rejected: "An impact figure is not a single quantity." · "European recovery law counts in kilograms."
If the grammatical subject is a category word (*a figure, a result, the law, the field, the literature*) or
a metaphor doing the claim's work, rewrite it. Put a named actor, a named instrument, a physical object or
the actual quantity in the subject slot. **Generic wording is a symptom of an unsharp claim.**

### 2. Never open with "Whether"
It converts a claim into a suspended question. Legal mid-sentence after a governing verb (assess whether,
test whether) and inside quotations.

### 3. "Figure" means an image, never a number
Substitute number, value, target, rate, share, score, percentage, result, quantity. Same care with "table".

### 4. Organise by argument, not by source
One topic per paragraph, ending on its own conclusion. No ordinal scaffolding unless the enumeration was
already given.

### 5. Sentence length is a check, not an aspiration
About twenty words, one idea. **Boundary:** the accepted block 2.4 runs longer wherever the argument needs
it. The limit targets *stacked clauses*, not length. Over-applying it produces staccato source reporting.

### 6. Paragraph shape depends on the paragraph's job
**Open on a synthesised claim** when the paragraph compares positions across several authors (block 2.4).
**Open on the author** when it carries one author's position or one study's result (block 2.5). Both **land
on a paired contrast**: two short sentences, the second usually negative.

✅ **The Methodology variant:** one mechanism per paragraph, opened on the mechanism, closed on its
consequence or its reason. No paired contrast. See rule 11 for how NOT to open it.

### 7. Summarise the author's position, do not report their study
Omit n, allocation, task description, apparatus, rounds and exclusions **by default**. Target about 100
words and no more than three sources per paragraph.

⚠ **Rule 7 INVERTS in Methodology.** The SRH contract demands the author's own procedure at a level
permitting replication. Sample sizes, protocols and apparatus are **required** there.

### 8. Never lead a result sentence with the baseline
**The thing that changed takes the subject slot and the verb states the direction.** Never open with
"Against X", "Compared with X" or "Relative to X".

### 9. Do not carry the source's vocabulary into the paragraph
Explain the mechanism in ordinary words and keep the page number. **Exception:** terms the thesis itself
defines stay exact (guided disassembly mode, digital model exploration, ReBuilt, RBv2.1.1, the stack).

### 10. 🔴 2026-08-22 — cut every sentence whose only job is to announce what comes next
Three deletions in one edit of 3.4.2, all the same defect:

1. **The figure roll-call.** "Figures 14 to 16 draw them in that order." **Never narrate the figure list.**
2. **The preview paragraph.** A whole paragraph listing what the following paragraphs would say.
3. **The negative opener.** "ReBuilt opens no menu and offers no home screen" became "The application
   session runs in three routines." **State what the thing does, not what it lacks.**

**A Methodology paragraph earns its place by carrying a mechanism, a number, a reason or a decision. A
paragraph that only orients the reader is deleted.**

### 11. 🔴 2026-08-22 — in Methodology, no compressed headline openers
Thiago on the first 3.5 draft: *"make it more simple and direct language. Those short sentences in the
beginning of each paragraph is not been a good idea in this section."*

**The rejected openers, all mine, all from one draft:**
"Three objects carried each session." · "Eligibility was open." · "Understanding is the primary outcome."
· "No tutorial preceded the AR condition." · "A session ran between forty-five and sixty minutes."

They are grammatical and they obey rules 1 and 5. They still read wrong, because a Methodology section is
an **account of what was done**, and a slogan is not an account.

**The rule:** open a Methodology paragraph with a full, ordinary sentence that describes the action or the
arrangement, then continue. The main point still comes first; it is stated, not compressed.

| Rejected | Written instead |
|---|---|
| "Three objects carried each session." | "Each session used three objects: the printed teardown artifact, a printed manual and a head-mounted display." |
| "Eligibility was open." | "Anyone could take part, with no requirement for a particular profession, prior disassembly experience or prior experience with headsets." |
| "No tutorial preceded the AR condition." | "Participants worked out the gestures and the interface for themselves, because no tutorial was given before the AR condition." |
| "Understanding is the primary outcome." | "The main measures ask the participant to compare the two versions directly." |

⚠ **This does NOT contradict rule 10.** Rule 10 deletes paragraphs that only orient. Rule 11 governs how a
paragraph that does carry substance opens. It does not license long openers.

⚠ **Scope: Methodology.** Chapter 2's accepted blocks land on short paired contrasts and that stays.

# THE FOUR VERIFICATION RULES

### A. A value the source does not print is a DERIVED value
Li's Table 1 prints 316, 272, 265; the differences appear nowhere. **Either print the source's own numbers
or name the derivation. Never write a computed value in the grammatical form of a reported one.**

### B. Multi-measure studies, and check the baseline before naming it
Caught on Malta p. 7. **Before writing any result, list every outcome measure the study reports.** Where
measures diverge, the divergence is usually the better finding. **Read a control condition's description,
not its one-word label.**

### C. Never cite a review for a primary study's measurement
Go to the primary study or drop the number. A review is citable for its own synthesis and verdict.

### D. 🔴 Deleting a `[CITATION NEEDED]` flag does not close the gap. TWICE NOW.
**2026-08-21:** the ISO sentence was dropped when pasting and the LCA method lost its only authority.
**2026-08-22:** the flag was stripped from the red-beside-green sentence in 3.4.2, leaving an unsourced
empirical claim about human colour vision.

**He edits by deleting the bracket, not the claim.** So never hand over a bare flag. **Offer the flag-free
rewrite in the same breath**, phrased so the sentence states his own design rule rather than an external
fact:

> before: "...color alone is not a reliable distinction for an operator with a red-green color vision
> deficiency `[CITATION NEEDED]`"
> after: "...so that no control is distinguished by color alone."

**Exception that must be said out loud:** where no flag-free version exists, say so explicitly. The
usability scale in 3.5 is a published instrument; using it unattributed is not a missing reference.

# THE CHAPTER-BOUNDARY RULE

**Methodology's own boundary:** if a sentence about the prototype would still be true had the study never
run, it is description and belongs in Methodology, not Findings. **Methodology states, the Discussion
defends.**

🔴 **AMENDED 2026-08-22: there is no 3.6.** The SRH template puts methodological limitations in the
Discussion and the strengths-and-weaknesses summary in the Conclusion. The old three-way split
(1.3 / 3.6 / Discussion) was a Claude-recorded ruling and the template outranks it.

# STANDING PREFERENCES

- **Never use the em dash.** Anywhere, including in replies to Thiago.
- Active voice by default. No inflated vocabulary. APA author-date.
- **AMERICAN spelling.**
- **Never cite a section or chapter number in running prose.** Appendices ARE cited by number.
- **No untagged numbers.** Assumptions are welcome; unidentified numbers are not.
- **Page numbers on every quoted number and every close paraphrase.**
- Study participants are P01 to P05, never named. 🔴 **And never described in a way that identifies them:**
  do not attach "was my supervisor" to a code the thesis uses elsewhere.
- **Never cite Pegorer et al. (2025)** as independent evidence.
- Git is manual: Claude edits, Thiago pushes from PowerShell.
- Still open: the Oxford comma.
- Terminology he expects back: "digital model exploration", "guided disassembly mode", "ReBuilt", RBv2.1.1.
- **Shorter replies when discussing.** Deliverables may be long; arguments may not.

# HOW HE WORKS

He rejected five drafts of 2.5 and three of 2.6, and every rejection was correct. He reads the sources
himself and checks tables. **When he says a paragraph is confusing, the sentence is defective, not his
reading.** He wants disagreement with structure and a reason, not compliance.

**His editing pattern:** he keeps mechanism and reason, merges paragraphs that split one topic, and deletes
orientation, previews and bracketed flags. Draft accordingly: fewer, denser paragraphs, ordinary openers,
every flag paired with a flag-free alternative.

⚠ **He corrects himself, sometimes more than once, and the last correction is not always right either.**
On 2026-08-22 the condition order changed three times before he checked the notebook. **When a fact comes
from recall, ask for the record before building anything on it.**

Related: [[writing_phase_setup]], [[ch2_block2526_final]], [[methodology_start_here]],
[[study_design_verified]], [[ch3_methodology_progress]], [[working_agreements]]
