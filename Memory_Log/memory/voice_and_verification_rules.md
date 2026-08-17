---
name: voice-and-verification-rules
description: The complete set of voice rules and verification rules learned from Thiago's edits, as of 2026-08-17. Nine voice rules, three verification rules, the chapter-boundary rule, and the two rules that INVERT in Methodology. READ THIS BEFORE WRITING ANY THESIS PROSE.
type: reference
---

# STATUS as of 2026-08-17 (Session 34)

Earlier versions said the synced academic-writer skill's voice_rules.md was EMPTY. That is no longer
true: it carries ten rules. But the synced copy is still BEHIND this file on four things, so this file
remains authoritative:
- Rule 5, sentence length: skill says "a hard limit, not a preference"; this file says "a check, not an
  aspiration", with the block-2.4 boundary that the limit targets STACKED CLAUSES, not length.
- Verification rule B (multi-measure studies): absent from the skill.
- Verification rule C (never cite a review for a primary measurement): absent from the skill.
- The chapter-boundary rule: absent from the skill.

# THE NINE VOICE RULES

### 1. No category-word subjects, and no metaphor standing in for the claim
Rejected: "An impact figure is not a single quantity." / "European recovery law counts in kilograms."
If the grammatical subject is a category word (a figure, a result, the law, the field, the literature) or
a metaphor doing the claim's work, rewrite it. Put a named actor, a named instrument, a physical object
or the actual quantity in the subject slot. Generic wording is a symptom of an unsharp claim, not a
vocabulary problem. Negation openers are the commonest hiding place.

### 2. Never open with "Whether"
It converts a claim into a suspended question. Legal mid-sentence after a governing verb.

### 3. "Figure" means an image, never a number
Substitute number, value, target, rate, share, score, percentage, result, quantity. Same for "table".

### 4. Organise by argument, not by source
One topic per paragraph, ending on its own conclusion. No ordinal scaffolding unless the enumeration was
already given.

### 5. Sentence length is a check, not an aspiration
About twenty words, one idea. BOUNDARY: the accepted block 2.4 runs longer wherever the argument needs
it. The limit targets STACKED CLAUSES, not length. Over-applying it produces staccato source reporting.

### 6. Paragraph shape depends on the paragraph's job
OPEN ON A SYNTHESISED CLAIM when the paragraph compares positions across several authors (block 2.4).
OPEN ON THE AUTHOR when the paragraph carries one author's position or one study's result (block 2.5).
Both LAND ON A PAIRED CONTRAST: two short sentences, the second usually negative. From 2.4: "Adisorn et
al. describe a recycler who receives product information. CIRPASS specifies a recycler who also supplies
it."
METHODOLOGY IS A THIRD CASE. It reports procedure, not positions. Both existing shapes were derived from
Literature Review blocks. Extract the Methodology variant from Thiago's edits rather than forcing one of
the existing two.

### 7. Summarise the author's position, do not report their study
The rule that finally made 2.5 work. Thiago's model paragraph: five sentences, about ninety words, no
sample size, no protocol, no apparatus. Verbs: approach, argue, propose. Omit n, allocation, task
description, device model, rounds, exclusions and procedure BY DEFAULT. A number enters only when that
number is the point. Target about 100 words and no more than three sources per paragraph.
RULE 7 INVERTS IN METHODOLOGY, and this is the single biggest carry-over risk into Chapter 3. The SRH
contract demands prior studies cited AS METHOD PRECEDENTS, and demands the author's own procedure at a
level permitting independent replication. Sample sizes, protocols, rounds and apparatus are REQUIRED
there. The rule governs how OTHER PEOPLE'S studies are summarised when justifying a method choice; it
does not govern the description of this study's own design.

### 8. Never lead a result sentence with the baseline
Rejected: "Against the same system running without that human step, accuracy rose by 10 %." Thiago read
it as removing the human improving accuracy. THE THING THAT CHANGED TAKES THE SUBJECT SLOT AND THE VERB
STATES THE DIRECTION: "Adding that correction step raised sorting accuracy by 10 %." Never open with
"Against X", "Compared with X" or "Relative to X".

### 9. Do not carry the source's vocabulary into the paragraph
Rejected as a "frankstein paragraph": "perception module", "first level of information",
"semi-transparent coloured cube", "morphology-based object segmentation". A paper's technical vocabulary
is written for that paper's readers; transplanted it reads as assembled and hides the mechanism behind a
label. Explain in ordinary words and keep the page number. EXCEPTION: a term the thesis itself defines
stays exact. In Methodology the exception is wide: the thesis's own named parts (guided disassembly mode,
digital model exploration, ReBuilt, RBv2.1.1) and the named stack stay exact.

# TWO RULES ADDED 2026-08-17 FROM THE METHODOLOGY OPENER

### 10. Never cite a section or chapter number in running prose
Seen twice in one block: "Section 2.6" became "the literature review"; "Chapter 4" became "the next
chapter". OVERRIDES the academic-writer skill's cross-reference guidance, which instructs "as introduced
in section 2.3". Name the chapter or give its relative position instead.

### 11. AMERICAN spelling
Thiago changed "standardised" to "standardized" on 2026-08-17. Consequence: artefact -> artifact,
organised -> organized, modelled -> modeled. The .docx is currently MIXED and needs a find-and-replace
pass. ("analys" is not a signal: "analysis" is spelled the same in both variants.)

# THE THREE VERIFICATION RULES

### A. A value the source does not print is a DERIVED value
Caught by Thiago: "The junior group started 51 s behind and finished within seven seconds." Li's Table 1
prints 316, 272 and 265; neither difference appears anywhere in the paper. Also caught: "an error rate of
1 % against 10 %" where Windhausen prints ".10" and "0.01". Either print the source's own numbers and let
the reader compare, or name the derivation. NEVER WRITE A COMPUTED VALUE IN THE GRAMMATICAL FORM OF A
REPORTED ONE. A derived value written as a reported one is indistinguishable from a fabricated one to
anyone checking the page. A coordinate read off a scatter plot is a derived value.

### B. Multi-measure studies, and check the baseline before naming it
Caught by Thiago on Malta p. 7. Three faults in one sentence: the baseline was misdescribed; only one of
two measures was reported (AR was slower to complete, 42 s against 34.5 s, and roughly twice as fast to
locate, 4.9 s against 9.2 s); and the authors' own positive reading was omitted. Before writing any
result, list every outcome measure the study reports. Report those bearing on the claim and any that run
against it, or state which measure is used. Where measures diverge, the divergence is usually the better
finding. And read a control condition's description, not its one-word label.

### C. Never cite a review for a primary study's measurement
Malta et al. is a systematic review. Its timing values belong to a primary study it summarises. Go to the
primary study or drop the number. A review is citable for its own synthesis, method, funnel and verdict.

# THE CHAPTER-BOUNDARY RULE

A Literature Review synthesis gathers findings that are on the page. A Discussion interprets findings
against the author's own results. The tell is citation density: a synthesis paragraph points at sources,
a discussion paragraph points at reasoning. A causal-chain draft of 2.6 was rejected on exactly this
ground and moved to the Discussion.
METHODOLOGY'S OWN BOUNDARY, from the contract: if a sentence about the prototype would still be true had
the study never run, it is description and belongs in Methodology, not Findings.

# A FOURTH VERIFICATION HABIT, learned 2026-08-17

CHECK THE WRITTEN DOCUMENT BEFORE TRUSTING MEMORY ABOUT ITS CONTENT. Section 3.1 was nearly drafted with
two claims taken from the signed proposal that the written thesis contradicts (participant composition,
and a "Stage 3 was cut" narrative that appears nowhere in the thesis). The document wins over the
proposal, and over memory. See ch1_verbatim_facts.

# STANDING PREFERENCES

- NEVER USE THE EM DASH. Anywhere, including in replies to Thiago.
- Active voice by default. No inflated vocabulary. APA author-date.
- Page numbers on every quoted number and every close paraphrase. Adopted 2026-08-16.
- Study participants are P01, P02, never named.
- Never cite Pegorer et al. (2025) as independent evidence.
- Git is manual: Claude edits, Thiago pushes from PowerShell.
- Still open: the Oxford comma.
- Terminology he uses and expects back: "digital model exploration" for the exploded action zone,
  "guided disassembly mode" for the five-step flow, "ReBuilt" as the prototype name, versioned RBv2.1.1.
- Watch for verbs that presuppose a positive result: "validate", "verify the efficiency", "improve",
  "increase". Each was caught and reverted three times.

# HOW HE WORKS

He rejected five drafts of 2.5 and three of 2.6. Every rejection was correct. He reads the sources
himself and checks tables. WHEN HE SAYS A PARAGRAPH IS CONFUSING, THE SENTENCE IS DEFECTIVE, NOT HIS
READING. Diagnose the specific defect before rewriting. He wants disagreement with structure and a
reason, not compliance. He overrules with reasons, and when the reason is genuinely new information,
concede cleanly and say why.

Related: writing_phase_setup, ch3_methodology_progress, ch1_verbatim_facts, ch2_block2526_final,
literature_review_chapter2, working_agreements
