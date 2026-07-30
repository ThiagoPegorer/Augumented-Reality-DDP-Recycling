# DPP UI Spec — 13b: Information model of the DPP Canva (CIRPASS Table 6 mapping)

> Companion to `13_dpp_canva.md`. Decides **what the passport screen must carry** before deciding how it
> looks. Source: CIRPASS **D2.2** "DPP use cases in battery, electronics and textile sectors",
> §4.2.2 **Table 6** (pp. 41–42), read 2026-07-30. `M` = mandatory by legislation · `U` = used by
> DPP-related initiatives (voluntary).
>
> Table 6 note, verbatim: *"product identification and company information are not listed in this table
> but are referred to in report D2.1."* So identity/manufacturer data is required, just sourced elsewhere.

---

## 1. Table 6, transcribed with prototype coverage

### Functional and technical specifications

| # | DPP data attribute | M/U | In RBv2.0? |
|---|---|---|---|
| 1 | Product information sheet on energy consumption & performance | **M** | ❌ absent |
| 2 | Technical documentation with product-model specific information (test results, measurement method) — Energy Labelling Regulation | **M** | ❌ absent |
| 3 | CE-marking | **M** | ⚠ field exists, value `—` (unverified) |
| 4 | Disposal, return and collection scheme information | **M** | ⚠ partial — `recycling_route` only |

### Material and composition information

| # | DPP data attribute | M/U | In RBv2.0? |
|---|---|---|---|
| 5 | Information on different materials **and location of** dangerous substances and mixtures (WEEE) | **M** | ⚠ materials yes, **location no** |
| 6 | Substances of concern: name, **location within the product**, **concentration** at product / main component / spare part level | **M** | ⚠ "None documented" only |
| 7 | Hazardous substances (REACH, POP, CLP, Ecodesign, WEEE) | **M** | ✅ neutral case stated |
| 8 | Individual material declaration | U | ✅ per-component material + mass |
| 9 | Full material composition | U | ✅ 4 aggregated groups |
| 10 | Recycled content | U | ⚠ field exists, value `—` |
| 11 | Recycling oriented information | U | ✅ route + recovery credits |

### Product design and service

| # | DPP data attribute | M/U | In RBv2.0? |
|---|---|---|---|
| 12 | Use, repair information (maintenance, spare parts, updates) | **M** | ❌ absent |
| 13 | Repair information incl. **disassembly instructions, component map** (Ecodesign) | **M** | ✅ the guided step flow + 3D twin |
| 14 | Disassembly instructions (WEEE) | **M** | ✅ the guided step flow |
| 15 | Resale options, end-of-life options, service availability for waste handling | U | ❌ absent |
| 16 | Instructions for safe use | **M** | ❌ absent |
| 17 | User manuals, instructions, warnings or safety information | **M** | ⚠ per-step task rows only |
| 18 | Information relevant for disassembly | **M** | ✅ tools · time · scope · part list |

### Usage history · Repair history · Indicators · Certification

| # | DPP data attribute | M/U | In RBv2.0? |
|---|---|---|---|
| 19 | Usage data (purchase date, use cycles, etc.) | U | ⚠ design service life only |
| 20 | Repair data (date, exchanged parts, costs, images) | U | ❌ absent |
| 21 | Circularity indicator (repairability, reuse, recycling index), environmental and social impact indicator, Product Environmental Footprint, **Life Cycle Assessment** | **U / M** ¹ | ⚠ LCA yes; **no circularity index** |
| 22 | Responsibility supply chain certifications | U | ❌ absent |

¹ Table 6 footnote 45: for **smartphones and tablets**, ecodesign + repairability information on the Energy
Label becomes mandatory from **June 2025**. A VCU is not in that product group, so for this prototype the
indicator row is treated as **U**.

## 2. Scoreboard

Table 6 lists **22 attributes: 13 M and 9 U** (#21 counted as U — see footnote ¹).

| | M (13) | U (9) |
|---|---|---|
| ✅ fully covered | **4** — #7 #13 #14 #18 | **3** — #8 #9 #11 |
| ⚠ partial | **5** — #3 #4 #5 #6 #17 | **3** — #10 #19 #21 |
| ❌ absent | **4** — #1 #2 #12 #16 | **3** — #15 #20 #22 |

So the prototype fully satisfies **4 of 13 mandatory attributes**, and the four it misses outright
(#1 #2 #12 #16) are exactly the ones the current redesign brief would not touch.

**Two structural observations:**

1. **Most of the absent M attributes are documents, not data** — a product information sheet, technical
   documentation, a user manual, instructions for safe use. They are PDFs in the real world. They cannot
   be rendered as icons or charts, and a headset is a poor place to read them. **Recommendation: represent
   them as a presence/availability row** (a "documents" chip row: available / not provided), not as content.
   That is honest, costs almost no space, and makes the compliance gap visible instead of hidden.
2. **The two most demanding M attributes are spatial** — #5 *location of* dangerous substances, #6
   substances of concern *located within the product*. A flat table cannot satisfy them; a 3D model with
   per-part callouts can. This is the same conclusion CIRPASS reaches for dismantling in **Table 8** (p.56)
   and **UC4 / Figure 16 step 2**, and it is the strongest available argument that AR beats a paper WEEE
   sheet. **The redesign should spend its budget here, not on decorating what is already legible.**

## 3. What belongs on the DPP Canva (proposal)

| Priority | Block | Why | Visual form |
|---|---|---|---|
| 1 | **Composition by mass** | #8 #9, and the anchor for #5/#6 | treemap / stacked bar — **CIRPASS uses exactly this convention in Figure 13 (p.40)**, so the form is citable, not invented |
| 2 | **Hazard & substance status** | #5 #6 #7 (M) | traffic-light chip row, each chip naming the part that carries it |
| 3 | **Recoverable value** | #11 + the study's framing | precious-metals strip in mg, or mass-recovered bars |
| 4 | **Compliance badges** | #3 #4 (M) | CE / RoHS / REACH / WEEE cat. — tri-state: verified · declared · not provided |
| 5 | **Identity + key specs** | D2.1, not Table 6 | header line + small spec chips (mm · g · IP · service life) |
| 6 | **Document availability** | #1 #2 #12 #16 #17 (M) | one chip row, presence only |
| 7 | **Circularity / LCA** | #21 | already the Model Exploration screen — do not duplicate |

## 4. Honest-labelling rule (SRH data-integrity + thesis defensibility)

Every value on this screen must carry its basis. Three states only:

- **verified** — from the physical unit or a primary source
- **assumed / modelled** — the current material split and the LCA figures
- **not provided** — `—`, never a blank and never a plausible-looking guess

⚠ RBv2.0 currently shows three static card subtitles that are **not bound to the payload**
(Identity, Materials, Compliance). They must be bound or removed in this redesign — a static
subtitle that looks like data is the worst of the three states.

## 5. Open decisions

- [ ] Does per-part hazard/value mapping live on this screen, or stay on Model Exploration?
- [ ] Do the absent M documents get a presence row, or an explicit "out of scope for the prototype"
      sentence in Limitations, or both?
- [ ] Scroll is banned on AR panels (spec 02 v3 §2). If everything above does not fit 640 × 430, what
      gets cut — or does the panel grow?

*Created 2026-07-30 · Source: CIRPASS D2.2 Table 6 (pp. 41–42), Figure 13 (p. 40), Table 8 (p. 56)*
