# Session 39 — 2026-08-22

**Focus:** Methodology 3.4 and 3.5 written and closed. Chapter 3 is drafted end to end. Section 3.6
deleted from the Methodology on the template's authority.

## What was produced

- **3.4.2 AR user interface** — the three routines, the passport rig of three canvases, the four tabs
  and the gate, and the design standards as **Table 5**
- **3.4.3 AR user experience** — hand tracking, hover elevation, the pinch family, the two-hand
  separation bands, the guards, the gesture readout, the three sounds and their silences.
  **Table 6**, the seven gestures
- **3.5 User study**, in three parts he specified: purpose and conditions, the manual, the
  questionnaire. **Table 7**, the measures
- **Three appendix documents**: the 2D manual v2 (Appendix V), and the questionnaire (Appendix VII).
  The consent form (Appendix VI) was already on disk
- **The Discussion's limitations section**, six paragraphs, drafted and collision-checked against 1.3

## 🔴 3.6 was deleted, and Thiago was right to raise it

Two lines in the SRH template inside his own `.docx` settle it. Under DISCUSSION OF FINDINGS: *"The
discussion chapter includes the methodological limitations of your research."* Under CONCLUSION:
*"Briefly summarize whether the inherent strengths and weaknesses of your methodological approach also
manifested themselves in your findings."* Nothing in the template's Methodology instructions asks for
a limitations section.

**The three-way split (1.3 / 3.6 / Discussion) was a Claude-recorded ruling and the template outranks
it.** Methodology now ends at 3.5. The drafted text moves to the Discussion, where it loses its
strengths half and gains a clause per item saying whether the limitation showed up in the results.

## 🔴 3.5 was never blocked. Five days lost to a category error.

The block recorded since 2026-08-17 listed the manual timings, the study design, the interface
feedback and the participant backgrounds. **Three of those four are results, not method.** The design
had been sitting in `AR_DPP/Docs/` since 2026-07-21: the questionnaire specification, the form builder
script, the consent form and the 2D manual. **Check what a block actually is before recording it.**

## 🔴 The condition order was corrected three times in one session

He first said the order was not alternated, then that participants used AR first, then checked his
notebook. The notebook is authoritative: P01 AR only, P02 manual first, P03 AR first, P04 manual
first, P05 manual first. **Three of four met the manual first**, so the second-attempt advantage helps
the AR condition, which is the direction that works against the thesis.

**Rule learned: when a fact comes from recall, ask for the record.** Two full analyses were built and
discarded before the notebook was opened.

## 🔴 A contradiction found inside Chapter 3

His Methodology already contains, in the 3.4 introduction: *"the version the participants worked with,
RBv1.0."* His new 3.5 opened: *"the AR DPP prototype, named as RBv2.1.1, a test using voluntary
participants is conducted."* Three pages apart, in one chapter. **Fix agreed: delete three words.**
His preference not to reopen versions in 3.5 is satisfied by saying nothing there rather than by
naming the wrong one.

## Two decisions he made against advice, both recorded and not reopened

1. **Block 2 runs RBv2.1.1 with manual v2**, while block 1 ran RBv1.0 with manual v1. The study is now
   two blocks and nothing pools across them.
2. **Version information stays out of 3.5.** Costs him the tested-version limitation in the Discussion.

## One he reversed after being shown the cost

He wanted the APOS credit overlap out of the paper because it would confuse readers. His own framework
records it as **supervisor-agreed 2026-07-02 and "declared throughout"**, so dropping it would have
reversed an agreement with the LCA supervisor in the supervisor's own specialty. Shown that the
disclosure costs three jargon-free sentences, he accepted it. The third sentence is the payoff of the
no-net ruling and reads as competence rather than apology.

## What the source files corrected

- **`TabCount = 4`**, and the rail gate requires all four tabs visited. Project memory said
  certificates sat outside the walkthrough, and `04e_rail_gate.md` §1 and §2 say so too. **Both are
  stale; the file's own header marks §1 struck, and `SuperPanelView.cs` wins.**
- **The locked-gate hint is one string, "Visit every tab first".** The spec's second variant is not in
  the code.
- **The guided disassembly reuses the passport rig**, swapping rail and data content in place. It does
  not route to separate screens.
- **`HoverHighlight.useOutline = false`.** The gesture table's "hover outline appears" is superseded by
  elevation; the stakeholder spec is stale the same way.
- **1.3 already owns six things** that were about to be repeated in the limitations section.
- **1.3 carries a live defect, now precise:** "The teardown model reproduces the geometry." It does
  not. The print is 200 x 150 x 60 mm and the reference unit is 166 x 121 x 41 mm.

## Two new voice rules, both from his edits

- **Rule 10, cut the scaffolding.** He deleted the figure roll-call, a preview paragraph, and a
  negative opener from 3.4.2. A Methodology paragraph earns its place by carrying a mechanism, a
  number, a reason or a decision.
- **Rule 11, no compressed headline openers in Methodology.** "Eligibility was open." "Understanding is
  the primary outcome." All grammatical, all rejected. A Methodology section is an account of what was
  done, and a slogan is not an account.

**Verification rule D:** deleting a `[CITATION NEEDED]` flag does not close the gap, and he has now
done it twice. Never hand over a bare flag; offer the flag-free rewrite in the same breath.

## The one gap that cannot be reworded away

The ten-item usability scale is a published instrument and its 0-100 scoring rule and benchmark come
from published sources. His questionnaire file states the benchmark with no reference. Reproduction
rights for the ten items in Appendix VII also need confirming.

## Tomorrow

Results chapter, and five more participant sessions on the same day. The standing recommendation,
given twice and still unanswered: **run all five AR-first**, which takes the order split from 3:1 to
3:6 and tilts the residual imbalance conservatively.
