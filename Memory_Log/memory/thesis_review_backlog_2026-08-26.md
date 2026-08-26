# Thesis review backlog, built 2026-08-26 (Session 43)

Source: `Master_Thesis_ThiagoPegorer_100003505_discussion_conclusion_open.pdf`, read end to end.
Nothing here was corrected. Every item is for the review pass.

Three items were raised in conversation as alarming and are repeated here at the top.

---

## A. ALARMING, raised in session

### A1. Section 1.3 claims the printed model reproduces the geometry. It does not.

Live text: *"The teardown model reproduces the geometry and the assembly sequence, and it does not
reproduce the glued joints, the conformal coating, or the fastener behavior of the real device."*

The print is 200 x 150 x 60 mm. The Bosch MS 50.4 reference unit is 166 x 121 x 41 mm.

Agreed replacement:
> The teardown model reproduces the part breakdown and the assembly sequence at a larger size than
> the reference unit, and it does not reproduce the glued joints, the conformal coating, or the
> fastener behavior of the real device.

**This blocks section 5.2**, which compares the printed model against the reference unit.

### A2. Section 4.1.6 states a wrong cause for the ranking mismatch.

Live text: *"The result of ranking between the two methods mismatch mainly because the units that
those categories are measured are not the same."*

The ranking mismatch comes from the normalization references, not from the units. The same
subsection supplies both references two paragraphs later: ReCiPe normalizes mineral resource
scarcity against 1.201 x 10^5 kg Cu eq and freshwater ecotoxicity against 25.17 kg 1,4-DCB. That
four-order-of-magnitude gap is what puts ecotoxicity at 37.77 % and minerals at 0.0005 %.

Units matter for a different claim, made correctly in the closing paragraph of the same subsection:
the minerals pair is the only reported pair whose ratio multiples differ, and it is the only one
measured in two different units.

**Section 5.1 cannot be written correctly while this sentence stands.**

### A3. The main research question now reads "end-of-life scenarios".

- Signed proposal (08/05/2026): "the disassembly and recycling analysis of a Vehicle Control Unit"
- 2026-08-13 revision: "the environmental consequences of its end-of-life routes"
- Live text: "the environmental consequences of its end-of-life scenarios"

The Methodology opener still reads *"the routes that unit can take at end of life"*, so the document
is inconsistent with itself. There is still no written supervisor agreement on the 2026-08-13
revisions. **Email Saman Ghobadian.**

---

## B. GAPS AND INCONSISTENCIES

### B1. Decimal separators
Appendix VIII prints comma separators (`0,0175`, `0,018739`) while the core text and the Chapter 4
tables print periods. Pick one and apply it document-wide.

### B2. Table 8 precision
Table 8 prints six decimal places, so Sc2 and Sc4 both display `0.018740` and Sc1 and Sc3 both
display `0.018739`. The paragraph directly beneath prints seven significant figures and separates
all four. Take the table to seven significant figures.

### B3. The condition-order rule
The 2026-07-21 study specification sets *"odd participant IDs start 2D-first, even AR-first."* The
real order breaks it for P03 and P04. Check whether that rule is stated anywhere in the Methodology.
If it is, it contradicts Table 21.

### B4. The open questions count
The study specification lists seven open questions. The export carries six. The missing one is the
deliberate pro-2D probe. Correct the Methodology if it states seven.

### B5. The questionnaire export title
The export is titled "Guided Disassembly RBv1.0". Rename before it goes into the appendix, since the
document does not name versions in the study sections.

### B6. The usability instrument
It is a published instrument and it is currently used unattributed. This is the one citation gap
with no flag-free rewrite. A benchmark value, if one is quoted, needs its own source. Also confirm
reproduction rights for the ten items in the appendix.

### B7. SRH boilerplate still live
The template's own instruction paragraphs are still under **DISCUSSION OF FINDINGS** and
**CONCLUSION**. Delete both blocks. The table of contents also shows
`5.1 Summary and discussion of findings ... Error! Bookmark not defined.`

### B8. Adisorn page locator
Block 2.4 cites Adisorn "equipment necessary" at p. 10. It is **p. 9**.

### B9. Chen et al. page locators
Re-page every Chen et al. citation against *Journal of Environmental Management, 348*, 119341.

### B10. Open-answer provenance
Section 4.3.3 states the open answers are the experimenter's written record. Three provenance edits
are owed: the session sequence, the instrument paragraph, and one clause inside 4.3.3.

### B11. Stage 5 published twice
Tables 11 to 14 (per scenario) and Tables 15 to 17 (per category) carry the same numbers in two
arrangements, about a hundred rows for one dataset. Decision was to keep both. Consider moving one
set to an appendix at compile time.

---

## C. GRAMMAR AND TYPOGRAPHY

| Where | Live text | Fix |
|---|---|---|
| 3.2 heading | "The Product and **it** Digital Product Passport" | "its" |
| 1.2, last line of para 3 | "differ in measurable terms**..**" | single period |
| Methodology opener | "This work **around** four stages" | "This work **is organized** around four stages" |
| Methodology opener | "four stages" for the work phases | collides with the life cycle's five stages. Call the work items **phases** |
| Table 7 | "Ecotoxicity**.** freshwater", "Human toxicity**.** non-cancer", "Eutrophication**.** terrestrial", "Eutrophication**.** marine" | commas, not periods |
| Table 19 | "Marketing & Social **Midia**" | "Media" |
| 4.1.6 | "(Huijbregts et al., 2017) **.**This cross-check" | space and punctuation |
| 4.1.6 | "The result of ranking between the two methods **mismatch**" | "mismatches" (and see A2) |
| 4.1.6 | "when the avoided impact is **analyze**, it **increase**" | "is analyzed, it increases" |
| 4.2 | "After the user **click** the button" | "clicks" |
| Figure 33 caption | "components to be **dismantling**, steps to be **conclude**" | "to be dismantled, steps to be completed" |
| Figure 34 / 35 captions | "Guided disassembly, **steps 1** / **steps 2**" | "step 1" / "step 2" |
| Figure 36 caption | "after **complete** the task the user **check it box**" | "after completing the task the user checks the box" |
| Figure 40 caption | uses an en dash where every other caption uses a hyphen | hyphen |
| 4.1.1 heading area | the only en dash in the thesis sits on the 4.1.1 table caption | hyphen |
| throughout | `%` in some places, "percent" spelled out in others | pick one |
| throughout | subsection heading capitalisation is inconsistent | pick one |

---

## D. STILL OWED FROM EARLIER SESSIONS

1. **The three Methodology edits to the impact assessment subsection**, including the ReCiPe 2016
   normalization reword.
2. **The Sc4 band relocation.** 3.3.3 currently ends *"...may be reported only as a band and never as
   a single value."* Replace with the deterministic-plus-interval wording. The uncertainty subsection
   already carries the band.
3. **"route" to "scenario"**, 25 paragraphs, `route_to_scenario_checklist.md`. Never find-and-replace:
   the 490 km transport route, the verb in 3.3.2, and the main research question all break.
4. **Delete "named as RBv2.1.1"** from the 3.5 opening sentence.
5. **Denote retained AI phrasing** across 4.1, 4.2 and 4.3, and write the Session 41, 42 and 43
   entries into `ai_use_log.md`. That file was last modified 2026-08-23.
6. **LIST OF TABLES and LIST OF FIGURES**: refresh fields after every insert.
7. `LCA_explorer.ipynb`: fix cell 10's title, delete cells 14, 32, 35, 36.
8. `recipe_screening_log.txt` carries a superseded ReCiPe 2008 endpoint screening. Clean it.
9. Download Recommendation (EU) 2021/2279 into LITERATURE.
10. `facts_register.md` section 5 wrongly says Huijbregts et al. (2017) is missing. It is cited.

---

## E. VERIFIED CORRECT, do not re-check

Checked against the PDF on 2026-08-26 and found sound:

- The Methodology opener no longer carries the false "frozen before any participant used it" sentence.
- Section 4.3.2 arithmetic: manual mean 312.5 s and median 300.0 s over eight participants;
  Augmented Reality mean 318.2 s and median 320.0 s over the same eight; 329.3 s and 324.0 s over all
  nine runs. Individual differences run from 26 s faster to 25 s slower.
- Table 21: five of eight faster in the second condition; three of the four who began with the
  headset had it as the slower run, against two of the four who began with the manual.
- Section 4.3.1 condition split: five began with the headset including P01, four with the manual,
  four and four among the eight who ran both.
- Figure 44 counts: 78 fives, four threes, two fours and six ones sum to 90.
- Table 7 cumulative shares reach the 80 % threshold exactly where the text says they do.
