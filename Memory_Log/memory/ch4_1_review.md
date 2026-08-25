# Section 4.1 — read-only review

Read from `Master_Thesis_ThiagoPegorer_100003505.docx`, staged from your machine at
24 August 14:19 (device mtime 24 August 13:38). Nothing was modified.

Section 4.1 now runs from the "Life Cycle Assessment Results" heading to the Monte Carlo
block, four Heading 3 subsections, 17 body tables and 6 figures.

---

## 1. Blocking. These change what the chapter says.

### 1.1 Three citations are missing from the screening paragraph

The paragraph beginning *"Characterization under EF 3.1 produced a result for each of the
sixteen midpoint impact categories ."* has **three orphaned spaces before full stops**, each
one a slot where a citation was dropped on paste:

| slot | text before the gap | what belongs there |
|---|---|---|
| 1 | "sixteen midpoint impact categories" | Andreasi Bassi et al. (2023), p. 5 |
| 2 | "the perceived relative importance of each category" | Andreasi Bassi et al. (2023), p. 5 |
| 3 | "of the Environmental Footprint method to that ranking" | Commission Recommendation (EU) 2021/2279, Annex I, 6.3.1 |

This is the fourth time the same thing has happened on paste. As it stands, three
method definitions taken from published sources carry no attribution.
**Recommendation (EU) 2021/2279 is still not in your LITERATURE folder.**

### 1.2 Table 9 prints 0,00000 for two values that are not zero

In the stage 1 to 4 table, the Impact column is formatted to five decimals:

| cell | printed | actual |
|---|---|---|
| S3 Distribution, minerals | `0,00000` | 6.226 x 10^-8 kg Sb eq |
| S3 Distribution, freshwater | `0,00000` | 1.806 x 10^-6 kg P eq |
| S2 Hardware assembly, freshwater | `0,00057` | 5.687 x 10^-4 kg P eq |

A Findings table stating zero for a non-zero quantity is a factual error, not a rounding
choice. The Share column beside it correctly shows 0,00033 and 0,00156, so the table
contradicts itself. Fix: scientific notation in the Impact column, as you already used in
Tables 11 to 17.

### 1.3 The stage 5 process data is published twice

Tables 11 to 14 (one per scenario, three category blocks each, with Amount and absolute
Impact) and Tables 15 to 17 (one per category, four scenario columns, shares only) are the
**same underlying numbers in two layouts**. Seven tables, roughly 100 data rows, spanning
pages 74 to 78, for one dataset.

Both sets are introduced separately: *"The table 11 to 14 show the impact of those process
systems"* and then *"The tables below list every process and the share it holds..."*. A
reader who checks will find WEEE shredding at 86,69 % of the Sc2 minerals result in both
places.

Pick one. Tables 15 to 17 win on comparability, which is the argument you are making, and
cost three captions instead of four. Tables 11 to 14 carry the Amounts (0.66 kg, 0.141 kg,
0.01 kWh) which 15 to 17 drop. **Move 11 to 14 to an appendix and cite it once.** You keep
both, and 4.1 loses four pages of duplication.

### 1.4 The ReCiPe cross-check is promised and never delivered

The opening paragraph of 4.1 states *"ReCiPe 2016 Midpoint serves as a cross-check on that
characterization."* Nothing in 4.1 performs it. The section ends on the Monte Carlo
paragraph. As written, the chapter makes a methodological promise in its first paragraph
and does not keep it, which is the kind of thing a second reader finds in ten minutes.

### 1.5 The Sc4 band contradiction is still open

Methodology says, verbatim, that the fourth scenario *"may be reported only as a band and
never as a single value."* The avoided-impact paragraph reports 0.008870 kg Sb eq, 15.43 kg
CO2 eq and 0.029761 kg P eq as single values, and the balance paragraph reports 0.00986934,
57.9788 and 0.0861762 the same way. Two chapters still contradict each other.

The three clauses, already computed:

| Sc4 avoided | point value | simulated p5 to p95 |
|---|---|---|
| Resource use, minerals and metals | 0.008870 kg Sb eq | 0.008271 to 0.009654 |
| Climate change | 15.43 kg CO2 eq | 14.32 to 17.50 |
| Eutrophication, freshwater | 0.029761 kg P eq | 0.022911 to 0.039715 |

---

## 2. The subsections you added

Four Heading 3 levels now exist:

| # | heading | paragraphs | pages |
|---|---|---|---|
| 4.1.1 | Relevant Impact Categories – EF 3.1 method | 11 | 70 to 71 |
| 4.1.2 | Life Cycle Assessment Stages Analysis | **68** | 71 to 78 |
| 4.1.3 | Impact recycling scenarios (Scenario 2 to 4) | 9 | 79 to 81 |
| 4.1.4 | Uncertainty quantification (Monte Carlo simulation) | 8 | 82 |

**The split is right in principle and wrong in proportion.** 4.1.2 holds about 70 % of the
section and does three separate jobs: the gross burden compared across scenarios (Figure 19,
Table 8), the stage 1 to 4 analysis (Table 9, Figure 20), and the whole stage 5 process
inventory (Table 10 and Tables 11 to 17). Its title announces only the second of the three.
A reader looking for the gross burden comparison will not look under "Stages Analysis".

**4.1.3's title is wrong for half its content.** "(Scenario 2 to 4)" excludes Sc1, but the
balance paragraph inside it reports Sc1 at 73.4326 kg CO2 eq, 0.0187391 kg Sb eq and
0.115920 kg P eq. The balance is a four-scenario statement sitting under a three-scenario
heading.

Suggested set, which also gives ReCiPe a home:

| # | heading |
|---|---|
| 4.1.1 | Relevant impact categories under EF 3.1 |
| 4.1.2 | Gross burden across the four scenarios |
| 4.1.3 | Life cycle stage contributions |
| 4.1.4 | Avoided impact and balance |
| 4.1.5 | Uncertainty quantification |
| 4.1.6 | Cross-check with ReCiPe 2016 Midpoint |

Six short subsections read better than one seven-page subsection and three short ones.

**Capitalisation is inconsistent across the four you have:** two are title case
("Relevant Impact Categories", "Life Cycle Assessment Stages Analysis") and two are sentence
case ("Impact recycling scenarios", "Uncertainty quantification"). 4.1.1 is also the only
heading in the thesis using an en dash. Pick one convention for the whole document.

---

## 3. Consistency defects

### 3.1 Decimal separators are split across the section

| table | separator |
|---|---|
| Table 7 | comma — `0,022237`, `72,45` |
| Table 8 | **period** — `0.018739`, `73.4326` |
| Table 9 | comma — `0,01833`, `97,84334` |
| Table 10 | **period** — `0.0002`, `0.2404` |
| Tables 11 to 14 | comma — `1,981E-08`, `66,10` (Amount column is period: `0.5875 kg`) |
| Tables 15 to 17 | comma — `86,69`, `3,00E-08` |
| all prose | period |

Tables 11 to 14 are the worst case: the Amount column uses a period and the Impact column
uses a comma **in the same row**. Set the whole document to one mark before submission.

### 3.2 Table 8 and its own paragraph disagree on precision

Table 8 shows six significant figures (`0.018739`, `73.4326`). The paragraph under it now
shows seven (`0.01873912`, `73.43259`). The rounding is consistent, but at six figures
Table 8 shows Sc1 and Sc3 as the same number, which is exactly the distinction the paragraph
is making. Take Table 8 to seven figures.

### 3.3 Per cent notation is mixed

`%` in the stage 1 to 4 paragraphs and in every table; the word "percent" in the screening
paragraph, the stage 5 share paragraph, the Sc2 comparison paragraph and all three Monte
Carlo paragraphs. Adjacent paragraphs disagree.

### 3.4 Source lines

- Two consecutive Source lines sit between Table 9 and Figure 20, one with a hyphen and one
  with an en dash. One is redundant.
- Wording varies: "Source: openLCA model - own elaboration" in 4.1.1 and 4.1.2, plain
  "Source: own elaboration" for Table 10, Figures 21, 22 and 23.
- Table 9's caption uses an en dash; every other caption uses a hyphen.

### 3.5 Figures 20 and 22 have duplicate captions

Both captions appear twice in the file, and neither copy sits in the body text flow the way
the captions for Figures 18, 19, 21 and 23 do. This is the same defect you fixed for
Figure 19 earlier ("the paper one survives"). A caption anchored only to the image moves
with the image on repagination.

---

## 4. Sentence-level

| where | as written | issue |
|---|---|---|
| chapter opener | "The third, reports the test conducted with volunteer participants" | stray comma |
| start of 4.1.2 | "The Figure 19 show the result applying the EF3.1 method" | article and agreement |
| before Table 9 | "Those information are relevant to the identify the impact of each stage in the total of it." | three errors in one sentence |
| before Table 11 | "The table 11 to 14 show the impact of those process systems." | "Tables 11 to 14 show" |
| Table 10 caption | "each scenario gross result" | missing apostrophe |
| end of 4.1.1 | "Those 3 categories alone represent 85% of the Environmental Footprint 3.1." | repeats the 85.70 % already given three paragraphs earlier, less precisely, and "85 % of the Environmental Footprint 3.1" is not what the number measures. It is 85.70 % of the normalised and weighted total. Consider deleting the paragraph |

---

## 5. What is right, and worth keeping

- The gross burden paragraph is now at **seven significant figures**, which is the precision
  at which the four scenarios actually separate. That was an open item.
- Table 10's caption says **scenario**, not route. The cleanup has started.
- The stage 5 share paragraph now covers **all four scenarios** (0.2404, 0.3040, 0.2088,
  0.2101 per cent for climate), which the earlier version did not.
- The deterministic-versus-median disclosure survived the edit. That paragraph is the most
  honest thing in the chapter and it is the one a supervisor will respect.
- The dismantling electricity assumption is named as an assumption, in a Findings-legal way.
- LIST OF FIGURES and LIST OF TABLES are refreshed and correct as of this file.

---

## 6. Order to fix, if time is short

1. The three missing citations.
2. Table 9's two zeros.
3. Delete or relocate one of the two stage 5 table sets.
4. Sc4 band clauses.
5. ReCiPe, or delete the promise from the opening paragraph.
6. Decimal separators, document-wide.
7. Headings and titles.
8. Everything in section 4.
