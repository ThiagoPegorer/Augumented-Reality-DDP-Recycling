### 2.5 Augmented reality for industrial task support

Malta et al. (2023) define augmented reality as a technology that allows virtual elements, whether text,
graphics or other objects, to be superimposed over images of real contexts (p. 1). They describe modern
smart glasses as carrying a global positioning system, a microphone and gesture recognition, and they
name the property that matters at a workbench: these devices allow users to have their hands free to
perform tasks while they receive instructions in real time through the glasses (p. 1). Malta et al. add
that achieving augmented reality with a simple mobile phone is often the cheapest and most practical
approach, while a head-mounted display, worn so that the overlay is drawn into the wearer's own view,
can provide a superior experience (p. 1).

Webel et al. (2013) approach augmented reality as a powerful industrial technology for training
technicians to acquire new maintenance and assembly skills. Those tasks are becoming more complex, and
technicians must be trained in the underlying sensorimotor and cognitive skills (p. 1). Webel et al.
(2013, p. 4) warn of a potential dependence on augmented reality features by the user during exercise
tasks. The authors propose that augmented reality training systems have to be distinguished from
augmented reality guiding applications. A training system has to reach the cognitive learning level,
while a guiding application stops at clear instruction.

Windhausen et al. (2024) tested smart glasses against a printed picking list in a warehouse task.
Pickers wearing the glasses worked faster and made fewer mistakes, 7.28 minutes against 12.81 and an
error rate of 0.01 against 0.10 (p. 6). Ariansyah et al. (2022) found the same direction in maintenance
assembly, where every augmented reality condition beat a paper manual on completion time (p. 1). They
also compared two ways of presenting the same task, and 3D animation was 14 % faster than video
(p. 14). The overlay wins when the thing it replaces is paper.

Li et al. (2023) asked a harder question. Not whether the overlay beats a manual, but whether it beats
experience. Junior workers using a technical manual needed 316 s to reach the disassembly quality
standard on a vehicle power battery, and junior workers using the augmented reality system needed 272 s
(p. 15). Skilled workers without the system needed 265 s (p. 15). Li et al. (2023, p. 15) read the three
averages as a reduction of the disparity between unskilled and skilled disassembly workers. Their table
reports averages without a significance test, so the size of the effect is indicative rather than
established. Webel et al. (2013, p. 4) measured a comparable gain on quality rather than speed, with
significantly fewer unsolved errors after augmented reality training than after video training,
t(18) = 2.52, p = 0.02. Both effects were measured on the same working day, and Daling et al. (2023, p. 1)
found no such advantage two weeks after training.

Chen et al. (2023) built a waste-sorting cell in which a robot does the picking and a person checks its
judgement. The camera either recognises a piece of waste and names its material, or sees only that
something is there (p. 7). When it names the material, the robot sorts unaided. When it cannot, the
decision falls to the person. Augmented reality is how that person sees the machine's opinion. Each
recognised piece appears on the real waste as a coloured block labelled with the material it was assigned,
and a ghost robot shows what the machine is about to do (p. 10). Two buttons let the person overrule it,
one to flag a piece the camera missed and one to change a material it got wrong (p. 10). Adding that
correction step raised sorting accuracy by 10 % on separated waste and by 15 % on waste partly buried
under other pieces, measured over twenty sample sets in each condition (pp. 16, 17). The larger gain came
where the camera failed more often. Even with correction, obscured waste stayed near 90 % accurate,
because the segmentation merges overlapping pieces into one object before anyone can judge them (p. 17).

No one was recruited, counted or trained for that experiment, and no statistical test is reported, so what
Chen et al. compare is two configurations of their own software. Mao et al. (2025) supply the field
evidence Chen lacks. They took a head-mounted display out of the laboratory and into two iterations on a
real train fault at a rail operator, with each procedure fetched from a cloud knowledge base by component
reference (p. 8). Every participant finished the maintenance procedure whether or not they had used
augmented reality before, error rates stayed low, and 91 % were satisfied with the information shown
(p. 13). Mao et al. also record that 45 % of participants needed information about neighbouring
subcomponents to finish (p. 13). That is the closest thing in this literature to a measured demand for
product data the operator does not already hold.

Ariansyah et al. (2022) looked at the operator instead of the clock. Eye movements showed how often
attention switched and how hard the task was to follow, and participants reported two problems the
timings never showed: misguidance and over-reliance (p. 1). Ariansyah et al. read both through the
Multiple Resources Model, which holds that competing demands draw on the same limited attention. Their
over-reliance is the dependence Webel et al. warned about, reached from the opposite direction.
Windhausen et al. (2024, p. 7) show that this cost falls unevenly, since workers low in technology
confidence gained nothing and rated their own productivity lower. Li et al. (2023, p. 11) meet the same
problem in design and answer it. Showing every recognised part overloads the operator and hides real
objects, so their system reads the tool in the operator's hand and highlights only the screws that
matter. What the overlay shows is a decision, not a display setting.

The equipment is less ready than the results suggest. Danielsson et al. (2020) compared commercial smart
glasses and found displays mature while tracking is not, concluding that the devices do not yet suit a
full working day (pp. 1, 8). Sik Lanyi and Withers (2020) set out to test whether industrial augmented
and virtual reality systems are ready, and confirmed that most are neither readily available nor
suitable for use (pp. 3, 13). Koutromanos and Kazakou (2023, p. 1) add that the field has studied users
mainly as consumers rather than as professionals, and that no acceptance model for these glasses exists.
Yang et al. (2023, p. 16) place a further gap one level higher, in disassembly planning, where
computational intelligence and management practice barely meet.

One question runs through every system above. Where does the content of the guidance come from? Li et al.
(2023, p. 6) retrieve their steps from a disassembly sequence diagram prepared in advance, once computer
vision has located the parts. Frizziero et al. (2019, p. 24) author theirs by hand from computer-aided
design models, and in their later study the display only validates a sequence already chosen by other
methods (Frizziero et al., 2022, p. 2). Cantarelli et al. (2025, p. 9) scan the physical objects.
Kaarlela et al. (2025, p. 1) use a digital twin of the robot and the battery. Chen et al. (2023, p. 16)
use a perception module that the operator corrects. Mao et al. (2025) hold theirs in a cloud repository
of structured knowledge objects defined by a domain ontology, and the augmented reality client requests
the matching procedure by component reference through an application programming interface (pp. 7, 8).
Abdel-Aty et al. (2025, p. 4), described in section 2.4, read the content from a Digital Product Passport
and show it on a computer screen beside the work. Mao et al. come closest to joining the two, because
their instructions are fetched by component reference and anchored to the equipment the technician is
working on (p. 8). Their knowledge base is nevertheless assembled and maintained inside one operator's
own organisation, from its sensor data and its maintenance history (p. 7). None of these systems reads
its guidance from a standardised product record issued with the product itself.

---

## === Check before using ===

**Output mode: PROSE, not scaffold.** Requested explicitly. Log as at risk for denoting under §13.

### Eleven paragraphs, on your spine, in the voice of your Webel paragraph

Your paragraph 2 is the model. What I copied from it: one idea per sentence, the author's **position**
summarised rather than their study reported, plain words, a page number only where a specific argument is
cited, and a short paragraph that ends on the useful point. Average length below is about 105 words. No
paragraph carries more than three sources.

| # | Movement | Paragraph does | Sources |
|---|---|---|---|
| 1 | Context | what AR is, and why industry wants it | Malta |
| 2 | Context | the two jobs: training against guiding | Webel (your text) |
| 3 | Works | beats paper on time and errors | Windhausen, Ariansyah |
| 4 | Works | beats inexperience, which is the harder test | Li |
| 5 | Works | the overlay as the correction channel between machine and operator | Chen |
| 5b | Works | the only field validation in the corpus, and a measured need for component data | Mao |
| 6 | Discussion | the cost sits in the operator. **Frame 4 installs here** | Ariansyah, Windhausen, Li |
| 7 | Discussion | equipment and acceptance are unfinished | Danielsson, Sik Lanyi, Koutromanos, Yang |
| 8 | Conclusion | where the guidance content comes from, and the gap | Li, Frizziero ×2, Cantarelli, Kaarlela, Chen, Mao, Abdel-Aty |

**Cut on 2026-08-17, in order:** the retention paragraphs (Daling, Farr) → Farr out entirely, Daling reduced
to one clause in paragraph 4; the counter-evidence paragraph (Malta's borrowed measurements, Windhausen's
borrowed percentages) → out as secondary citations; the systems-not-results paragraph (Cantarelli, Kaarlela,
Frizziero, Fernández-Moyano) → out as low-value. **Eight paragraphs, four movements.**

### Your five decisions, applied

1. **Mao et al. (2025) is in**, in paragraphs 5 and 11. Full citation: Mao, W., Scheffer, S., & Majumdar,
   A. (2025). Augmented reality-enabled knowledge management in industrial maintenance: The DILEAF
   framework. *Computers & Industrial Engineering, 208*, 111363. Peer-reviewed, not a preprint.
2. **Fernández-Moyano et al. appears only for its verdict**, in paragraph 8. Not in paragraph 1.
3. **Ariansyah's 14 % is repeated here**, in paragraph 3, and stays in Chapter 1 as well.
4. Your Webel paragraph is used as written, with "skills" restored and the dependence claim in Webel's
   own register. Its final sentence keeps your distinction, phrased so the cognitive-level half reads as
   your reading rather than as their prescription.
5. 🔴 **Gonzalez-Franco et al. (2017) is EXCLUDED**, as you instructed. It was the weakest fit for
   paragraph 6 in any case, because it tested immediately after training while Daling and Farr both test
   at two weeks. Paragraph 6 is now Daling and Farr only, and it is tighter for it. ⚠ Check whether
   Chapter 1 cites Gonzalez-Franco before deleting the entry from the reference list.

### 🔴 STILL UNRULED: the Li tension

My earlier numbering slipped, so this was never answered. Paragraph 4 rests on Li's Table 1, and Li's
abstract claims a significance and an error reduction the paper never measured. **v5 implements my
recommendation:** keep Li, use the table, close paragraph 4 on one clause saying the averages come without
a significance test, and never quote the abstract. Li is deliberately absent from paragraph 8. Overrule
this if you want it handled differently, but I would not drop Li: it is the only study in the corpus that
compares augmented reality guidance against experienced workers on a real disassembly task.

### New evidence used here for the first time, all read today

- **Kaarlela et al., p. 4:** their own Table II totals, 258 s by hand against 474 s robotised. The
  teleoperated cell was slower.
- **Malta et al., p. 7:** 42 s with AR against 34.5 s on an LCD screen. Their review contains a case where
  an ordinary screen beat AR.
- **Malta et al., p. 8:** 4 min 55 s with paper against 4 min 57 s with AR.
- **Windhausen et al., p. 3:** the 43.8 % initial gain and the 23 % penalty on repetition without the aid.
- **Windhausen et al., pp. 6, 7:** the actual Study 1 numbers, and the finding that low technology
  confidence removes the benefit.
- **Li et al., p. 11:** information overload, occlusion of real objects, and the tool-attention response.
- **Mao et al., p. 13:** all participants completed regardless of AR experience; 91 % satisfied; **45 %
  needed information on neighbouring subcomponents.**
- **Danielsson et al., p. 8:** not suitable for a full working day, which is stronger than the abstract.

### 🔴 The `[FILL]` is closed, and it narrows the gap claim again

Mao et al. (2025) describe their content source in full, and it is closer to this thesis than anything
else in the corpus:

- p. 7: "a cloud-based repository stores structured knowledge objects defined by a domain ontology".
- p. 7: historical maintenance records were "manually uploaded into the backend knowledge base in
  structured JSON format, including component IDs, timestamps, and fault descriptions", held in a cloud
  database and "retrieved by the AR system via API calls".
- p. 8: "For each fault trigger or component reference, the system called a corresponding maintenance
  procedure via API requests to the backend knowledge base. The procedures were stored in structured JSON
  format and included stepwise instructions, visual highlights, and simple schematic diagrams. The AR
  interface parsed this information and displayed relevant instructions using Unity and MRTK, with
  holographic overlays anchored to real equipment through spatial mapping."
- p. 8: "Each instruction remained active until the technician confirmed completion using gesture-based
  input, after which the system advanced to the next step."
- pp. 3-4, their own stated gaps, one of which is "Limited empirical validation in field environments".

**Consequence.** The claim "no study serves augmented reality guidance from a structured record keyed on
components" is dead. Mao et al. do exactly that, anchored to the equipment, advanced by gesture. The
surviving distinction is the **kind** of record: theirs is an internal knowledge base built and maintained
by one operator from its own sensor data and maintenance history, not a standardised passport issued with
the product by the producer. Paragraph 11 now states this explicitly instead of overclaiming. **2.6 has to
be corrected to match, because it currently carries the wider claim.**

### 🔴 A class of defect you caught, now removed everywhere

You asked where Li states that the junior group "started 51 s behind and finished within seven seconds".
Nowhere. Table 1 prints 316 s, 272 s and 265 s, and both differences were my subtraction presented as if
the paper reported them. Checking the section for the same defect found two more:

| Where | What I had written | What the source prints | Fix applied |
|---|---|---|---|
| P4, Li | "started 51 s behind and finished within seven seconds" | 316, 272, 265 as three averages, Table 1, p. 15. No difference is stated | Both differences deleted. The three averages are given in consecutive sentences and Li's own disparity claim carries the interpretation |
| P3, Windhausen | "an error rate of 1 % against 10 %" | "MPrint = .10 vs. MARSG = 0.01", p. 6 | Given as 0.01 against 0.10, their own form. My conversion to percentages is gone |
| P7, Malta | "finished two seconds apart" | "4 min 55 s for the paper instructions and 4 min 57 s with the AR remote support tool", p. 8 | Both values now printed. The subtraction is left to the reader |

**The rule this establishes, worth recording in the skill:** a difference, a ratio or a percentage that the
source does not print is a derived value. Either print the source's own numbers and let the reader compare,
or name the derivation explicitly. Never write a computed value in the grammatical form of a reported one.
Li's 13.9 % is safe because Li prints it. My 51 s was not.

### 🔴 Chen et al. corrected: it is not a study of people

You asked where the augmented reality sat in Chen's experiment and how it produced the 10 % and 15 %.
Answering it exposed a misrepresentation in every earlier draft.

**How the system works (pp. 7, 8, 10).** The perception module produces two levels of information. The
first level is location plus category, obtained by object detection against a pre-prepared database
through the ROS `find_object_2d` package with OpenCV feature detectors. The second level is location only,
obtained by traditional morphological image processing, with no category attached. When the first level is
available the robot sorts on its own. When it is not, the human has to supply the category. The HoloLens 2
renders each detected category as a semi-transparent coloured cube on the real waste, with a label for
category and location, and shows a virtual robot previewing the real robot's next motion. The interface
carries three category buttons (wood, glass, plastic) and two function buttons, "Mark me" and "Remark me",
the second of which lets the operator select a wrongly classified item and set the correct category from
experience.

**Where the 10 % and 15 % come from (pp. 16, 17).** Two methods, the augmented reality human-robot
collaboration method against a conventional model-based method labelled "Without HRC", across two
scenarios, isolated waste and waste with obstructions, with twenty groups of samples per condition.
Accuracy rate is defined as true positives over the number of items to be sorted. Grabbing rate exceeded
98 % for both methods, so the gain is entirely in classification, not in handling.

🔴 **The correction that matters.** Chen et al. report **no human participants**: no number, no
recruitment, no background, no allocation, no training. The twenty groups are groups of waste samples. The
"operator" is generic throughout. No statistical test, no time and no cycle time is measured anywhere in
the paper. My earlier sentence, "placed the operator beside a sorting robot", implied a study of people.
The text now says what it is, a comparison of two software configurations, one with a human correction
step. That sentence also does the work of moving the paragraph on to Mao et al., who supply the field
evidence Chen lacks.

### 🔴 Two writing rules this exchange produced

**1. Never lead a comparison with the baseline.** I wrote "Against the same system running without that
human step, accuracy rose by 10 %". A reader cannot tell from that sentence which side rose, and you read
it as removing the human improving accuracy. The rule: the thing that changed goes in the subject slot and
the verb states the direction. "Adding that correction step raised sorting accuracy by 10 %." Never open a
result sentence with "Against X", "Compared with X" or "Relative to X".

**2. Do not stitch the source's vocabulary into a paragraph.** Terms such as "first level of information",
"perception module", "semi-transparent coloured cube" and "morphology-based segmentation" are the paper's
words for the paper's readers. Carried across, they make a paragraph that reads as assembled rather than
written, and they hide the mechanism instead of explaining it. The Chen paragraph now says camera, name the
material, coloured block, ghost robot, two buttons, overlapping pieces merged into one object. Same facts,
same page numbers, ordinary language. Both rules belong in the skill's `voice_rules.md`.

### 🔴 CUT: Daling et al. and Farr et al. removed from 2.5 (2026-08-17)

Both retention paragraphs are gone. The section is now nine paragraphs. Two consequences were handled, and
one objection stands.

🔴 **Correction to what I told you.** I said Webel's result had to be rescued from the paragraph you cut.
That was wrong on the facts. Webel's result was never in the paragraph you quoted, and it was never anywhere
in v5 until I added it myself during the rebuild an hour earlier. Moving it into paragraph 4 is therefore an
**addition**, not a recovery, and you should accept or reject it on its own merit. The case for it: paragraph
2 otherwise gives Webel a position with no evidence behind it, and his effect belongs beside Li's because
both are gains for the less experienced worker. Say the word and it comes out again.

**Handled: the movement no longer needs renaming.** With Daling and Farr gone, "where it does not work" is
carried by paragraph 7 (an LCD screen beating augmented reality, and the 23 % penalty on repetition without
the aid) and paragraph 8 (systems reported instead of results). Those are genuine counter-evidence, so the
heading now fits.

**Applied: Farr out, Daling kept as one clause.** Paragraph 4 now ends: "Both effects were measured on the
same working day, and Daling et al. (2023, p. 1) found no such advantage two weeks after training." That is
Daling's foothold in Chapter 2. It carries no design detail, no explanation and no verdict, so the Discussion
can expand it without introducing a source the reader has never met. The full Daling material, including the
authors' own reasons for finding nothing, is held at the foot of this file for that chapter.

⚠ **Check Chapter 1 before deleting Farr et al. from the reference list.**

### Daling et al., held for the Discussion chapter

2.5 now names the limit in one clause and explains nothing. The explanation belongs where the prototype's
own training claim is argued. Everything below is page-verified and ready to use there.

**Daling et al.'s own account of why no difference appeared.** The decline over two weeks is attributed
"either due to the high complexity of the assembly steps or because participants were not explicitly
informed that they were supposed to perform the assembly again from memory after two weeks" (p. 8). They
then concede a measurement gap: "no differentiation of errors into solved vs. unsolved errors has been
realized, which could have contributed to a better understanding of the effects of AR- vs. VR-based
training on error count" (p. 9). Task load was measured only by subjective rating, which they say "could
limit the expressiveness of the results" (p. 9).

**Their conclusion is positive, not negative.** "AR-based training in particular can be considered an
effective alternative to video-based training to ensure short- and long-term training success in manual
assembly tasks" (p. 10). Do not cite Daling et al. as evidence that augmented reality training fails. They
report equivalence with video, plus significantly higher perceived task load and lower usability for
virtual reality than for augmented reality or video (p. 1).

**The synthesis to deploy in the Discussion.** Daling et al. regret not separating solved from unsolved
errors (p. 9). Webel et al. (2013, p. 4) made exactly that separation, and their significant difference
appeared in unsolved errors and nowhere else. One paper's confessed measurement gap is precisely where the
other paper's effect lives. Both halves are sourced, and it is the strongest single link this corpus
supports.

**Design detail if the Discussion needs it.** 103 participants allocated to video, augmented reality or
virtual reality, 34 / 34 / 35, with seven of the virtual reality group later excluded for technical
problems (p. 3). The task was a LEGO MINDSTORMS EV3 robot in ten steps, and the augmented reality device
was a first-generation HoloLens (p. 3). Both are worth naming when transferability is argued.

### Farr et al., cut entirely

Removed from the Literature Review on 2026-08-17. If it is ever needed: their null is attributed by the
authors themselves to an underpowered retention sample, 44 against 106 valid cases at first assessment,
caused by Covid quarantines, and "all descriptive indicators" favoured the immersive condition (pp. 4, 5).
Their own conclusion is that the two media "could be used interchangeably" (p. 5). ⚠ Check Chapter 1 before
deleting the reference-list entry.

### 🔴 CUT 2026-08-17: paragraphs 7 and 8 removed, and the spine is now four movements

**Why paragraph 7 had to go, and you diagnosed it correctly.** Malta et al. is a systematic literature
review. The 42 s, 34.5 s, 4.9 s and 9.2 s values are not their measurements; they belong to a primary study
that Malta et al. summarise. Citing Malta for those numbers is a secondary citation, and a Literature Review
that leans on one is weak precisely where it should be strongest. The same fault ran through the Windhausen
sentence in that paragraph: the 43.8 % and 23 % figures sit in Windhausen's Table 1 as someone else's
result, not as theirs. My scaffold had flagged this ("attribute to Malta et al. reporting, not to Malta as
the finding") and the prose then ignored the flag.

**Malta et al. survives in paragraph 1, and should.** A review is a legitimate source for a definition and
for a field-level characterisation, which is all paragraph 1 asks of it: what augmented reality is, the
hands-free property, and the phone against head-mounted display comparison. Those are Malta et al.'s own
framing statements. Only the borrowed measurements had to go.

**Paragraph 8 removed as low-value.** Its four sentences carried one idea, that some studies report a
system rather than a result, and it needed four sources to say it.

**What leaves the section entirely:**

| Source | Consequence |
|---|---|
| **Cantarelli et al. (2025)** | Now appears only in the closing paragraph, for its scanning pipeline. If you want it out altogether, say so |
| **Fernández-Moyano et al. (2025)** | Leaves 2.5 completely. You had kept it for its p. 17 verdict, and that verdict was in paragraph 8. The section now contains no field-level statement about how mature this literature is, and no evidence of its size. Both burdens fall on the chapter opener |
| **Kaarlela's 474 s against 258 s** | Gone. Kaarlela remains only in the closing paragraph, for the digital twin |
| **Frizziero's "the saving is not an augmented reality effect"** | Gone. Frizziero remains only in the closing paragraph, for CAD authoring |

⚠ **Your five-movement spine is now four.** There is no "where it does not work" movement left. The honest
description of the section as it stands is: context (1, 2), where it works (3 to 5b), discussion of what it
costs and what is not ready (6, 7), conclusion (8). The critical function has not disappeared, it has moved
into the discussion: over-reliance, uneven benefit, information overload, immature tracking, no acceptance
model, absent planning integration. **Decide whether that is the shape you want, or whether one piece of
genuine counter-evidence should be restored from a primary source rather than from a review.**

**The rule this establishes, and it is the fifth today.** In a Literature Review, never cite a review for a
measurement generated by a primary study it cites. Go to the primary study or drop the number. A review is
citable for its own synthesis, its own method, its own funnel and its own verdict.

### Superseded: the paragraph 7 rebuild, kept for the record

You stopped on the Malta result. Reading p. 7 in full showed three faults, not one.

**1. The baseline was not "an ordinary screen".** Malta et al. describe the LCD condition as presenting "a
single static 3D scene rendered in VR", generated by "the same engine used to generate the virtual content
for the AR condition", with "identical text instructions, 3D labels, close-up graphics, and animated
sequences" (p. 7). It is the same content on a flat display, not a plain monitor and not a manual. My wording
made it sound like AR lost to an ordinary computer.

**2. The study reports two measures and I used only one.** Task completion: AR 42 s, head-up display 55.2 s,
flat display 34.5 s. Task **location**: AR 4.9 s, head-up display 11.1 s, flat display 9.2 s (p. 7). The
overlay was slower to finish and roughly twice as fast to find the part. I reported the measure that suited
the paragraph and omitted the one that did not, which is the same defect as cherry-picking in AR's favour,
only pointing the other way.

**3. The authors' own reading is positive.** "The authors stated that their qualitative results provide
additional incentives for the application of AR in maintenance tasks. They further noted that future AR
systems should contain lighter and more comfortable screens with larger viewing areas and higher
resolutions" (p. 7). They attribute the shortfall to the hardware, not to the concept.

**Why the corrected version is better for your thesis than the wrong one was.** Separating *locating* from
*doing* is exactly the distinction your prototype turns on. An operator opening an unfamiliar control unit
has to find and identify components before removing them, and the one thing this study shows the overlay
doing well is telling the worker where to look. That is a sourced argument for what the ReBuilt guidance is
for, and I had deleted it by accident.

**The rule this establishes, and it is the fourth today.** When a source reports several outcome measures,
report the ones that bear on the claim **and** any that run against it, or state which measure is being used
and why. Selecting a single measure from a multi-measure study is a verification failure, not an editorial
choice. Recording this in `verification_protocol.md` alongside the derived-value rule.

### Assumptions and flags

1. Chapter 1 expands augmented reality and virtual reality on first use, so short forms are used after
   paragraph 1.
2. Chen et al. page numbers are pre-print pages. The published article is *Journal of Environmental
   Management, 348*, 119341, and every Chen citation needs re-paging before submission.
4. Paragraph 9's sentence linking Ariansyah's over-reliance to Webel's dependence is a mention, not a
   second explanation, so it stays inside the Mention Rule.
5. Gavgiotaki et al. (2023) and Shi et al. (2023) are deliberately absent. They belong to Methodology,
   and Shi et al. is a virtual reality study.
