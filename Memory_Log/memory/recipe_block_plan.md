# The ReCiPe block — how to build it without any Sc1-relative percentage

---

## First, the word

**A cross-check cannot prove robustness.** One alternative method can fail to overturn a
finding; it cannot establish that no method would. And in a Findings chapter you cannot even
write "robust" — that is a judgement, and judgements live in the Discussion. What 4.1 can
carry is the comparison itself, stated as data. The word "robust" then appears once, in the
Discussion, resting on what this block reports.

Write the block to answer one question: **does the finding change when the characterisation
factors change?** Not: is the model right.

---

## The design, four moves, no Sc1 anywhere

### Move 1 — name the level

Methodology already commits to it: ReCiPe is reported *"at characterized level only"*. Say
so in the first sentence. This also solves the reading problem created by ReCiPe sitting
after the Monte Carlo block, where the previous paragraph has just said the deterministic
gross values are off centre. Naming the level tells the reader this comparison is a
different operation, not a continuation.

No ReCiPe normalisation, no ReCiPe weighting. The screening rule that selected the three
categories is an EF procedure and is not repeated.

### Move 2 — the pairing, and its honest limit

| EF 3.1 category | ReCiPe counterpart | shared unit |
|---|---|---|
| Resource use, minerals and metals (kg Sb eq) | Mineral resource scarcity (kg Cu eq) | **no** |
| Climate change (kg CO2 eq) | Global warming (kg CO2 eq) | yes |
| Eutrophication, freshwater (kg P eq) | Freshwater eutrophication (kg P eq) | yes |

Two of three share a unit. The minerals pair does not: antimony equivalents against copper
equivalents, two different scarcity models. **This is why the test is built on ratios rather
than on levels.**

### Move 3 — the two tests

**Test A, ordering.** Purely ordinal, no percentages, no reference scenario.

> The avoided impact increases from Sc2 to Sc3 to Sc4 in **25 of the 25 EF 3.1 result rows**
> and in **18 of the 18 ReCiPe categories**.

That is the strongest single sentence available to you, and it costs one line. It tests the
ordering across ReCiPe's whole profile, not only the three paired categories, which is a
wider test than the EF selection can perform on itself.

**Test B, ratios against Sc2.** Dimensionless, so the kg Sb eq against kg Cu eq problem
disappears: each ratio is taken inside its own method. Sc2 is already the reference for the
scenario comparison in 4.1, so no new percentage basis enters the chapter.

| pair | method | Sc3 / Sc2 | Sc4 / Sc2 |
|---|---|---|---|
| minerals | EF 3.1 | 3.219 | 4.739 |
| minerals | ReCiPe | **2.467** | **3.494** |
| climate | EF 3.1 | 1.847 | 3.492 |
| climate | ReCiPe | 1.850 | 3.503 |
| freshwater | EF 3.1 | 2.775 | 4.314 |
| freshwater | ReCiPe | 2.775 | 4.314 |

Climate agrees to 0.003 and 0.011. Freshwater agrees to three decimal places. **Minerals
does not agree**, and the gap is large: 3.219 against 2.467, and 4.739 against 3.494.

### Move 4 — report the disagreement, do not bury it

The minerals divergence is the most interesting result in the block, not its weakness. Both
methods rank the scenarios identically, and they disagree on how much more Sc4 recovers than
Sc2, by roughly a quarter. That is exactly what a scarcity model does: antimony equivalents
and copper equivalents weight gold, silver, palladium and copper differently. State it and
let the Discussion explain it.

---

## The limit you must disclose, and probably do not want to

**The freshwater pair is not an independent test.** The two methods return nearly the same
number, not merely the same ratio: the Sc2 avoided impact is 0.00689894 kg P eq under EF 3.1
and 0.00689905 kg P eq under ReCiPe, agreeing to the fifth significant figure. When two
methods share a characterisation factor, their agreement is arithmetic, not corroboration.

So the block really tests **two** categories, not three. Say so. One sentence:

> The two methods return almost identical freshwater eutrophication results, so agreement in
> that category reflects a shared characterisation factor rather than an independent
> confirmation.

Without it, a reader who checks the numbers finds a claimed three-for-three that is really
two-for-three plus a tautology.

Climate is a real test: 4.41898 against 4.47268 kg CO2 eq for the Sc2 credit, 1.2 per cent
apart at level while the ratios agree to three decimals. That is the shape of genuine
corroboration, two methods with different factors landing on the same relationship.

---

## Optional fifth move: the gross burden

If you want the cross-check to cover the gross finding as well as the avoided finding, one
sentence does it, and it needs no chart:

| gross spread across the four scenarios | EF 3.1 | ReCiPe |
|---|---|---|
| minerals pair | 0.0028 % | 0.0066 % |
| climate pair | 0.0955 % | 0.0949 % |
| freshwater pair | 0.0153 % | 0.0153 % |

Under ReCiPe the largest gross spread across the four scenarios in **any** of its eighteen
categories is 0.7473 per cent, in human non-carcinogenic toxicity. The claim that the gross
burden barely moves therefore survives a change of method across a wider set of categories
than the thesis reports.

---

## What to leave out, and why

**The eighteen-category ranking table.** Recommended against in the last session; nothing has
changed. Three prices, all still live:

1. **Marine ecotoxicity has no EF counterpart** and holds 29.03 per cent of ReCiPe's
   normalised profile. Publishing the ranking puts a large unpairable category on the page.
2. **Human carcinogenic toxicity diverges by 11.28 points at Sc4** (ReCiPe 26.79 per cent
   against EF 38.07 per cent), a wider gap than the minerals divergence the block already
   carries.
3. **The ranking exposes mineral resource scarcity at 0.0005 per cent of ReCiPe's profile**,
   which contradicts the EF selection that the opening of 4.1 spends three paragraphs
   justifying, in a chapter that cannot explain the contradiction.

The ordering sentence in Test A already carries the whole-profile argument, at one line
instead of a table plus three paragraphs of defence.

**The net charts from the explorer.** Correctly excluded. They express Sc2, Sc3 and Sc4
against Sc1, which is the basis you removed from the thesis.

---

## If you want a figure

The ratios are the only quantity here that is comparable across methods, so the only honest
chart is a grouped bar of the six ratios: three categories, two methods, one panel for
Sc3 / Sc2 and one for Sc4 / Sc2. Climate and freshwater bars sit at the same height in both
panels; the minerals pair visibly does not. Say the word and I will write the notebook cell.

A chart is not required. Six numbers in a table carry this fine.
