# CHAPTER 5 — DISCUSSION OF FINDINGS
## Full agreed text, Session 43, 2026-08-26

All four blocks signed off by Thiago in session. This is the paste source.
PROSE, at risk for affidavit denoting. Log it in `ai_use_log.md` before signing.

🔴 TWO ITEMS MUST BE FIXED IN THE DOCUMENT BEFORE PASTING:
1. Section 1.3 still says the teardown model "reproduces the geometry". 5.2 contradicts it.
2. Section 4.1.6 blames the units for the ReCiPe ranking mismatch. The cause is the normalization
   references. 5.1 does not currently contradict it in the agreed text, but the defence will.
Also delete the SRH boilerplate still live under DISCUSSION OF FINDINGS.

---

# 5 DISCUSSION OF FINDINGS (opener)

Guided dismantling raises how much of the device reaches the correct material stream
(Lee et al., 2012). A first reading of the impact results does not show this. The avoided primary
production does. Under EF 3.1 characterization, and taking bulk shredding as the reference
(Scenario 2), guided dismantling (Scenario 3) avoids 1.85 times as much climate impact and
dismantling with reuse (Scenario 4) 3.49 times as much. In resource use, minerals and metals the same
two scenarios avoid 3.22 and 4.74 times as much.

ReBuilt delivered a standardized product record into the operator's field of view, which is the
combination no reviewed study had tested. The application anchored the passport to the physical unit
as three panels in the room, carried the manufacturer record and the eight-component list, displayed
the Environmental Footprint screening shares that the life cycle model produced, and ran a five-step
guided sequence through to a stored session summary. The environmental result reaches the recycler as
a field inside the record rather than as a separate analysis, and it is read at the bench rather than
at a computer beside it.

The Augmented Reality prototype was slower in most of the tests, and most participants still
preferred it. No error was recorded in any of the seventeen runs, in either condition. The usability
score is higher for the Augmented Reality application for seven of the nine participants, and each of
the ten comparative items returns a median of 5 on a scale where 5 is the Augmented Reality model.
All nine recorded that they would use the tool at a real recycling workstation, and eight named
training rather than throughput as the use. Eight of the nine also recorded that the material and
recovery information did not help them perform the task on a prototype of this simplicity.

---

# 5.1 LCA model discussion

Applying normalization and weighting under the EF 3.1 method makes the sixteen impact categories
comparable on one scale. This is the first step that reveals which categories are relevant in the
model. For the vehicle control unit studied here, resource use, minerals and metals holds 72.45
percent of the weighted footprint. Looking more closely inside that category, materials and
construction account for 97.84 percent of it. The impact is created when raw materials are extracted
and processed into electronic components. It is already spent by the time the unit reaches a
recycler.

Demand for this equipment is rising rather than falling. Electric vehicle adoption accelerated
sharply over the past decade (International Energy Agency, 2026), and each generation of vehicle
carries more electronic content than the one before (Restrepo et al., 2019). More units mean more of
this burden unless recovery practice changes. The life cycle results show what changing it is worth.
Applying more selective methods at end of life, aimed at identifying the components whose materials
carry high impact, raises the avoided primary burden at every step.

Component reuse produces the largest gain of all, and it works through a different mechanism.
Recovering a material displaces the extraction of that material. Recovering a working component
displaces the extraction and the manufacture, since the ore processed into a few grams of transistors
and capacitors is avoided along with the metal itself. The credit system built for that scenario
includes avoided integrated circuit and transistor production for this reason. It is also the least
certain scenario in the model, because its reuse yield has no source and it carries the widest
uncertainty band in every reported category. Reuse is the largest opportunity this study identifies
and the one it can least prove.

The model built for this thesis rests on a unit that was never measured by the author. Its hardware
parameters come from the reference datasheet, and its mass is set at the datasheet ceiling because no
unit was weighed. The material composition behind the bill of materials rests on published figures
for comparable electronics. The data filling the remaining life cycle stages, including the use
phase, are assumptions made for an electric passenger car.

Every process built in openLCA is an approximation of the processes behind the reference product. The
end-of-life processes are approximations of the same kind. No primary data was collected from a
recycling operator. What reaches the correct material stream under bulk shredding comes from a
published plant study. What guided dismantling changes comes from a published disassembly study. The
rate at which removed components pass testing for reuse is quantified nowhere, so it was assumed
within a declared range. The simulation confirms what this costs. The four gross distributions
overlap one another completely, and no scenario can be told apart from another on the burden it
creates. The size of the impact reduction is therefore not a finding of this study.

The trend across the end-of-life scenarios is a finding. Every step toward a more selective recovery
method raises the avoided primary burden. The ordering holds in all sixteen EF 3.1 categories and in
all eighteen categories of the cross-check method. It also survives the declared input ranges where
the absolute values do not, because the credits separate from one another in the simulation while the
gross results do not. A recycler cannot be told how much a scenario saves. A recycler can be told
which scenario saves more. That is the claim this study defends.

---

# 5.2 Digital Product Passport and AR prototype discussion

The passport structure is satisfiable. What the record could not supply was the values. Those values
come from four different places, and the record states which. Certificates and safety carries
manufacturer data taken from the reference product documentation, including the conformity marking
and the two declared substances of very high concern. Product specifications mixes the reference
product with the built prototype, since the manufacturer, model and type come from the datasheet
while the serial number and production date belong to the unit made for this study. Usage and
service, and the repair history with it, are assumptions made for an electric passenger car.
Environmental impact is modelled output from the life cycle assessment. All fifteen components carry
their material data as assumed, and none of it is verified. Every field declares its own basis, so a
reader can see which kind of value they are looking at instead of guessing.

The limitations of the passport also include data storage and management. Determining the best way to
store the record, so that multiple stakeholders can reach it, was not in the scope of this work. The
data is served from a simple JSON file on a local host. For this application a local file proved
sufficient, but scaling the approach would require established data storage practices.

The main limitation of the physical model is that it is not a replica of the reference unit. It is a
generic teardown model inspired by the Bosch MS 50.4, printed in PETG, and it holds no electronic
components. It has no glued joints, no conformal coating, and no fastener behavior taken from a real
device. Peitzmeier et al. (2025) found that two non-demountable glued joints were enough to prevent
an automotive control unit from being reused or refurbished as a spare part. The model therefore
removes the barrier that study identifies as decisive. What participants opened was a device designed
to come apart, so the study measures how well the interface communicates a procedure, and not how
hard the work is on a real unit.

The prototype RBv2.1.1 is an Augmented Reality guided assistant for disassembly, built on a Digital
Product Passport interface. This version is a minimum viable product rather than a tool for a working
recycler. It shows what a later version could do: give an end-of-life stakeholder an understanding of
the product before it is opened, guide that person through the disassembly steps, and highlight the
components whose material composition carries the highest environmental impact. Whether doing so
changes recovery in practice is a question this study does not answer.

---

# 5.3 Voluntary test participants discussion

One background variable separates the responses, and it is not the one usually expected. Every
participant who had never disassembled an electronic device rated the Augmented Reality application
higher than the two-dimensional manual. That is five of the nine. The only two who rated it lower
both reported prior disassembly experience. The relationship runs in one direction only, since two
other experienced participants also rated the application higher. Nothing in the data says that
experience makes the tool worse. It says that inexperience is the condition under which every
participant found it better, which is the condition the passport is built for.

Prior experience with head-mounted displays predicts nothing. Four participants had never used one.
One of them produced the most negative usability difference in the study, and two produced the two
largest positive ones. The participant who rated himself an expert sits in the middle of the range.
Age behaves differently again. The three participants aged 45 and over recorded the three slowest
runs with the headset, and the two of them who also performed the manual condition recorded the two
slowest manual times. Their usability differences cover the entire span of the sample, from the
lowest to the highest. Age therefore tracks how long the task took and not how the application was
received.

The dissent belongs almost entirely to one participant. P02 is the only participant to answer below
the midpoint on any comparative item, and he did so on five of the ten. Eight of the twelve responses
in the whole matrix that are not at the maximum are his. He scored the manual at 97.5 and the
application at 42.5, and the working of the score shows he answered in both directions rather than
straight-lining. His interview record gives a reason consistent with that score. He located the value
of the application in devices where a mistake can injure the operator, and recorded that on a
prototype of this simplicity it was disproportionate to the task. He is a trained electrician with no
headset experience. His objection is not that the application failed. It is that the task did not
need it.

The timed runs support him on one point. The application did not make the task faster. Five of the
eight participants who performed both conditions took longer with the headset, and no participant's
two times differ by more than twenty-six seconds while the participants themselves differ by more
than two hundred. A practice effect plausibly contributes to what difference there is. Five of the
eight recorded a shorter time in whichever condition they performed second, and the headset was the
slower run for three of the four who met it first against two of the four who met it second. Four per
side is a direction and not a measurement. Perception and the stopwatch also disagree. Of the five
participants who were measurably slower with the headset, three answered that the headset let them
work with more agility.

Two findings share a single cause. No error was recorded in any of the seventeen runs, and eight of
the nine participants recorded that the material and recovery information did not help them perform
the task. The teardown model is a simple object with five steps and one tool. A task with no
opportunity for error cannot show error prevention, and a task whose components can be identified by
looking at them does not need a record to identify them. The simplicity that made the study
manageable is the same property that removed the conditions under which the passport would earn its
place. Seven of those eight participants nonetheless recorded that the information gave them
knowledge beyond conventional product specifications, which is a different claim from usefulness at
the bench.

Three separate measures point at training rather than throughput. Eight of the nine named training as
the use they would put the tool to. All nine recorded that they would use it at a real recycling
workstation. The times show no consistent gain. Taken together these say the participants valued
learning a procedure over performing one, and that reading is consistent with the population tested,
none of whom had performed this task before.

Two ceilings limit what the study can claim. The first is hardware. Six of the nine named the
sharpness of the passthrough image as the main difficulty, recording that it reduced accuracy when
unscrewing fasteners, and seven named headset comfort over a long session as the constraint. Both sit
in the device rather than in the application, and neither is answerable by design work. The second is
the instrument. Ten comparative items returned the same median from the same nine participants, with
seventy-eight of the ninety responses at the maximum and five participants answering at the maximum
on every item. An instrument that separates one participant and nobody else is measuring at its
ceiling. The comparative results should be read as agreement on direction, and not as a measure of
magnitude.

What the study supports beyond these nine people is narrow. Nine participants performed one task on
one device with one interface, and the object they opened was designed to come apart. No professional
dismantler took part. That boundary was set deliberately, because the thesis argues the obstacle at
end of life is missing product knowledge, and a participant without product knowledge is the
condition under test. The boundary had a consequence, and the results show it: the participant with
the most hands-on disassembly experience is the one who rejected the application. The findings
therefore describe how an inexperienced operator receives guidance delivered in the field of view.
They do not describe how a trained dismantler would work with it, they do not extend to other devices
or other interaction models, and they do not measure recovery.

---

# OPEN FLAGS FOR THE WHOLE CHAPTER

Page locators owed: Lee et al. (2012) · International Energy Agency (2026) · Restrepo et al. (2019)
· Peitzmeier et al. (2025) · Alcoceba-Pascual et al. (2025) if the minerals-versus-mass paragraph is
ever restored.

`[CITATION NEEDED: ore input per unit mass of semiconductor components]` for the clause in 5.1 about
ore processed into a few grams of transistors. Currently written qualitatively.

Check: Chapter 4 states the avoided-impact ordering for all ReCiPe categories but not for all sixteen
EF 3.1 categories. Either add it to Results or drop "sixteen" from 5.1.

UNRESOLVED: whether the two conditions were timed by different instruments. 5.3 makes no claim
either way.

Cut from 5.1 and available if wanted: the mass-based recovery target and Article 33 paragraph, and
the method-dependence paragraph on the ReCiPe ranking. Both were drafted and both are strong.
