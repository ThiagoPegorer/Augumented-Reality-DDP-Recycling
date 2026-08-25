# Uncertainty block — revised

Every figure below recomputed from `Outputs/4_monte_carlo/mc_summary.csv`, n = 1000 per
product system. Band width is defined as (p95 − p5) / p50, expressed as a percentage.

---

## What I changed and why

| # | change | reason |
|---|---|---|
| 1 | Minerals gross band **"17 to 18 percent" → "16.5 to 17.7 percent"** | The true minimum is Sc1 at **16.54 %**. Integer rounding lifted the floor above the lowest value in the set, so the sentence claimed a tighter floor than the data has |
| 2 | Freshwater gross band **"154 to 161 percent" → "153.7 to 160.9 percent"** | Same defect. The minimum is Sc4g at **153.65 %** |
| 3 | The measure is now defined in the first sentence | "span 62 to 66 percent of their own median" does not tell a reader what was measured. It is stated once and then used |
| 4 | "credit systems" → "avoided-impact systems" | "Credit system" appears nowhere else in the thesis. The rest of 4.1 says avoided impact |
| 5 | **New paragraph on whether the intervals separate** | The block reported how wide the bands are and never whether the scenarios can be told apart. That is the question the section exists to answer, and one pair fails it |
| 6 | "The deterministic results" → "The deterministic **gross** results" | The claim is false for the credits. Sc4's avoided minerals value is 0.008870 against a median of 0.008870 |

Everything else in your three paragraphs verified correct: 15.60 / 2.22 / 5.15,
20.07 / 11.97 / 10.41, 57.57 / 57.02 / 50.68, the "less than one point" difference
(0.55 pp), 73.43 against 91.33, 0.115920 against 0.129911, 0.0187391 against 0.0188976.

---

## The revised block

> The spread of each distribution is reported as the distance between the fifth and
> ninety-fifth percentiles, expressed as a percentage of that system's own median. On this
> measure the gross systems carry a much wider band than the avoided-impact systems. In
> climate change the four gross systems span 62.2 to 66.0 percent, against 10.4 to 20.1
> percent for the three avoided-impact systems. In resource use, minerals and metals the
> gross systems span 16.5 to 17.7 percent and the avoided-impact systems 2.2 to 15.6
> percent. In freshwater eutrophication the gross systems span 153.7 to 160.9 percent and
> the avoided-impact systems 50.7 to 57.6 percent.

> Among the three avoided-impact systems, Sc4 carries the widest band in all three
> categories, and the margin differs by category. In resource use, minerals and metals its
> band is 15.60 percent, against 2.22 percent for Sc2 and 5.15 percent for Sc3. In climate
> change it is 20.07 percent, against 11.97 and 10.41 percent. In freshwater eutrophication
> it is 57.57 percent, against 57.02 and 50.68 percent, a difference of less than one point.

> The four gross distributions overlap one another across their full range in all three
> categories. Among the avoided-impact systems, the Sc2 and Sc3 intervals are separated in
> all three categories, and the Sc3 and Sc4 intervals are separated in two of them: the Sc3
> ninety-fifth percentile of 0.006091 kg Sb eq lies below the Sc4 fifth percentile of
> 0.008271 kg Sb eq, and the Sc3 ninety-fifth percentile of 8.89 kg CO2 eq lies below the
> Sc4 fifth percentile of 14.32 kg CO2 eq. In freshwater eutrophication the two intervals
> meet: Sc3 reaches 0.024275 kg P eq and Sc4 begins at 0.022911 kg P eq.

> The deterministic gross results reported above are not the central values of their
> distributions. In climate change the Sc1 gross result of 73.43 kg CO2 eq sits below the
> simulated median of 91.33 kg CO2 eq. In freshwater eutrophication the deterministic
> 0.115920 kg P eq sits below the median of 0.129911 kg P eq. In resource use, minerals and
> metals the two are close, 0.0187391 against 0.0188976.

> The functional yield assumed for Sc4 is unsourced, and its avoided impact is therefore
> reported with the simulated interval. Between the fifth and ninety-fifth percentiles the
> Sc4 credit spans 0.008271 to 0.009654 kg Sb eq in resource use, minerals and metals, 14.32
> to 17.50 kg CO2 eq in climate change, and 0.022911 to 0.039715 kg P eq in freshwater
> eutrophication. The deterministic values reported for Sc4 fall inside each of these
> intervals.

---

## The one thing you should decide before pasting

The third paragraph puts on the record that **the Sc3 and Sc4 avoided-impact intervals
overlap in freshwater eutrophication.** Sc3 runs to 0.024275 kg P eq and Sc4 starts at
0.022911 kg P eq, an overlap of 0.001364 kg P eq.

This is the strongest sentence in the block and also the most exposing one. It is the only
place in 4.1 where the simulation does not separate two scenarios. Leaving it out is not an
option once the band widths are published, since a reader with the appendix percentile table
can derive it in one subtraction, and finding it themselves is worse than reading it from
you. What it means belongs in the Discussion, not here.

The complementary fact, also true and also worth stating there rather than here: the same
two scenarios are cleanly separated in resource use, minerals and metals and in climate
change, and the Sc2 to Sc3 step is separated in all three.

---

## Supporting figures

| system | minerals | climate | freshwater |
|---|---|---|---|
| Sc1 gross | 16.54 % | 63.59 % | 153.65 % |
| Sc2 gross | 17.05 % | 65.96 % | 160.90 % |
| Sc3 gross | 17.73 % | 65.72 % | 154.91 % |
| Sc4 gross | 17.30 % | 62.23 % | 154.40 % |
| Sc2 avoided | 2.22 % | 11.97 % | 57.02 % |
| Sc3 avoided | 5.15 % | 10.41 % | 50.68 % |
| Sc4 avoided | 15.60 % | 20.07 % | 57.57 % |

Interval endpoints used in the separation paragraph:

| pair | category | Sc3 p95 | Sc4 p5 | separated |
|---|---|---|---|---|
| Sc3 / Sc4 | minerals | 0.00609149 | 0.00827080 | yes |
| Sc3 / Sc4 | climate | 8.88913 | 14.32265 | yes |
| Sc3 / Sc4 | freshwater | 0.02427490 | 0.02291071 | **no** |
| Sc2 / Sc3 | minerals | p95 0.00189376 | p5 0.00578428 | yes |
| Sc2 / Sc3 | climate | p95 4.94891 | p5 8.01290 | yes |
| Sc2 / Sc3 | freshwater | p95 0.00927926 | p5 0.01480270 | yes |
