# DPP UI Spec — 13d: Compliance & Safety (tile + detail page)

> **Living spec** — ReBuilt v2.0. Status: **CODED 2026-07-31 (spec 13 v12), awaiting device check.**
> Renames **Compliance & certification → Compliance & Safety**. Mock:
> `drafts/13_v14_compliance_safety.svg`. Style: Usage Profile family (13c).
> **Primary source:** Bosch **EC/EU Declaration of Conformity**, Bosch Engineering GmbH, Abstatt,
> **09 Oct 2020** — Operation Manual VCU MS 50.4P, `Vehicle_Control_Unit_VCU_MS_50.4P_Manual`,
> **pp. 132–134** (§22 Legal: DoC + REACH Statement; §23 Disposal). Schema **v0.12**.

---

## 1. What the DoC actually says (and what it corrected)

The declaration covers **VCU MS 50.4 (F02U.V02.965-01)** and **MS 50.4P (F02U.V02.966-01)**, from
date of manufacture 01.03.2020.

| Finding | Consequence in the passport |
|---|---|
| Of 8 directive checkboxes, **only 2014/30/EU (EMC) is ticked** | `ce: true` with `ce_scope: "2014/30/EU (EMC)"` — the CE claim is scoped, not blanket |
| Measured per **ECE R10**, equivalent **ECE-R10.06 rev.6 : 2019** (UNECE vehicle-EMC type approval) | `tested_to` field |
| **RoHS 2011/65/EU unchecked** — means of transport are outside RoHS scope | `rohs: null` + **`rohs_applicable: false`** — "not applicable" is a third state, distinct from non-conformance. The old tri-state badge model (CE/RoHS/REACH pills) could not express this and was retired |
| **REACH Statement (Art. 33):** SVHC > 0.1 % w/w — **Lead (CAS 7439-92-1)** and **Lead monoxide / lead oxide (CAS 1317-36-8)** | `substances_of_concern[]` populated, `basis: declared` — **Table 6 #5/#6 render again** after dying with the Substances tile in v8. `end_of_life.substances_basis → declared` |
| §23 Disposal: sort for recycling, never household waste | 4th declaration note |

**Regulatory background (for the thesis):** 2014/30/EU is the EU EMC Directive (CE-marking basis);
UN ECE R10 rev.6 (2019) is the automotive EMC type-approval regime under which vehicle electronics
are assessed; REACH Art. 33 (EC 1907/2006) is an *information duty*, not a certification — which is
why REACH was never honestly a pass/fail badge.

**Consistency notes:** the lead SVHCs coexist with the BOM's lead-free SAC305 solder — PbO sits in
component ceramics/glass frits, not solder (`location_note` says so). The DoC names type
`-01`; the 2026 datasheet order number (and our payload) is `-02` — recorded here, payload unchanged.
`documents[].declaration_of_conformity` flips **not_provided → available** (we hold the manual).

## 2. Landing tile (290 × 72 @ panel-local 326, 260)

`Compliance & Safety` · chips (family position, y 41): **`CE (EMC)`** + **`2 SVHC declared`** — both
computed, not typed (`ShortScope()` extracts "EMC" from `ce_scope`; the count from
`substances_of_concern`). `+` circle only. The tri-state badges, the WEEE face row and the
certificate-count row are gone; `statusDots` 2/3 join 0/1/6/7 as dead slots — only 4/5 (Service)
remain live.

## 3. Detail page — Usage Profile family

Header: back circle + `Compliance & Safety` 19 pt, no icon, no caption. Content y 88–418.

### 3.1 Left — DECLARATION NOTES (24, 88, 290 × 330, the only chrome)

Scrollable card (`PinchScrollArea`, third instance). Head + right-aligned `Bosch DoC · 09 Oct 2020`
(bound to `declaration_date`). Body is **one wrapping rich-text block**, not a row pool — the notes
are variable-length paragraphs. The view composes it from `compliance.declaration_notes[]`
(Assessment · EMC after installation · Instructions · Disposal — condensed from the DoC's "Further
explanations" and §23) **plus an auto-appended SVHC section with full names and CAS numbers** — Art.
33 requires the identities somewhere on the page; the face carries only symbols. Content height =
`GetPreferredValues(text, 254, 0).y` (the same inactive-object-safe measurement as FillChips).

### 3.2 Right — six plain groups (titles left x 326, values centred cx 471, hairlines)

| # | Title | Value | Source |
|---|---|---|---|
| 0 | CE CONFORMITY | `2014/30/EU (EMC)` | `ce_scope`, shown only if `ce == true` |
| 1 | TESTED TO | `ECE R10 · rev.6 : 2019` | `tested_to` |
| 2 | ROHS 2011/65/EU | `not applicable` (dim) | `rohs_applicable == false`; conforms / does not conform if `rohs` ever set |
| 3 | REACH SVHC | `2 declared · Pb, PbO` | count + `symbol` fields (v0.12) |
| 4 | WEEE CATEGORY | `Cat. 5 · small equipment` | `weee_category` — still OUR classification (assumed), the DoC does not state it |
| 5 | DECLARATION | `09 Oct 2020` | `declaration_date` |

The two-slot lead listing from the first mock was merged into slot 3 (freeing slot 4 for WEEE — a
mandatory Table 6 attribute outranks a second CAS line on the face); the CAS numbers moved into the
notes card.

## 4. Data & code (schema v0.12)

- `Compliance` + `ce_scope`, `tested_to`, `declaration_date`, `rohs_applicable`,
  `declaration_notes[]` (new `DeclarationNote {title, body}`); `basis → declared`.
- `SubstanceOfConcern` + `symbol` ("Pb", "PbO"); two entries with regulation `REACH SVHC`,
  `threshold_pct_w_w 0.1`, `above_threshold true`, `basis declared`, location notes.
- Builder: `BuildComplianceDetail`; `BuildComplianceBadges` **deleted** (callerless);
  tile switched to `BuildFaceChipPool`. Right-column loop kept as deliberate duplication of the
  Usage pattern so the device-tested Usage code stays untouched.
- View: `PopulateCompliance`, `SetCompStat` (with a `dim` state for not-applicable),
  `ShortScope`; `SetBadge` and the badge fields **deleted**; status rows 2/3 retired.

## 5. Accepted risks / open items

- `weee_category` renders beside five DoC-declared values but is itself an assumption — the one
  non-declared value on the page, indistinguishable on screen (consistent with the no-basis-marker
  direction of 13c §4).
- `certifications[]` ("1 certificate on file") lost its render spot; content unchanged in the payload.
- Concentrations are not stated by Bosch (only "> 0.1 %") — `concentration_pct_w_w` stays null.

## 6. Iteration log

- **2026-07-31 (a)** — Manual pp. 130–134 read; DoC + REACH statement extracted; mock
  `13_v14_compliance_safety.svg` approved ("Perfect. Can move on with it") with the lead-merge +
  WEEE-slot recommendation adopted, DECLARATION date taking the sixth slot.
- **2026-07-31 (b)** — Coded: schema v0.12, payload, builder, view. 49 serialized refs verified,
  0 orphaned; attribute sweep clean; newline escapes verified as real C# `\n`.

*Last updated: 2026-07-31 · Status: coded, awaiting device check · Parent: 13_dpp_canva.md · Siblings: 13c*
