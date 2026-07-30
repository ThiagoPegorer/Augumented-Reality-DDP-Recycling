# DPP UI Spec — Screen 9: Completion Summary (per-step performance + recovery table)

> The finale of the Disassembly flow. Standard **640 × 430** single panel (no
> exploded-view canvas). Reads with `00_design_standards.md`.

> **Revision 2026-07-14 (v3, approved & Editor-tested).** Approved mock:
> `drafts/09_summary_v3_2.svg`. Full redesign around the STEPS (user direction:
> "always think around the steps") and text minimalism:
> 1. **Removed:** eyebrow `MS 50.4 · {serial}`; ALL non-button boxes (stat cards,
>    recovered grid); "Steps completed" card; the `Total time · start → finish`
>    label; the "Total recovered / 660 g / 5 material streams" headline; the old
>    RECOVERED 2×2 grid (gold pins / silicon / aluminium / CO₂) — the recycler
>    stakeholder doesn't value CO₂/eco framing; the reward is per-step output.
> 2. **New core: per-step table** — one row per step: title, a
>    materials-with-grams line, the step's TIME split, and the step's total
>    RECOVERED mass. Longest step tagged gold (mastery cue + thesis instrument).
> 3. **Per-step timing:** StepFlowController records a split at every Confirm;
>    splits ship in the recovery report as `step_times_s[]` (user-test dataset).
> 4. **Material composition:** new additive backend field
>    `Component.material_breakdown[]` for aggregate components, `basis:
>    "assumed - validate in openLCA"` — on-screen italic footnote carries the
>    disclaimer. LCA lines / precious_metals / CO₂ remain FROZEN.
> 5. Single-action Send → Done flow kept unchanged from v2.1.

---

## 1. Purpose & context

Shown after `Finish & see summary` on Step 5. Rewards the worker with their
measurable output — time per step and grams recovered per step — and posts the
recovery report, closing the UC4 data-feedback loop.

- **Reached from:** Step 5. **Exits:** `Done` (post-send) → Main Page.
- **Gamification model (v3):** mastery + output, matched to the recycler's real
  KPIs (throughput and mass per fraction). Time is an achievement, never speed
  pressure. The longest-step tag answers "where can I improve?".

---

## 2. Layout (640 × 430, panel-local coords)

| Element | x | y | w | h | Notes |
|---|---|---|---|---|---|
| Done icon | cx 40 | cy 48 | r 20 | — | `#10241e` + `teal/accent` ring, teal check |
| Title | 72 | 36 | 480 | 28 | `Nice work — unit fully dismantled`, 22 bold white |
| Total time value | 20 | 84 | 320 | 38 | **value only, no label** — `4 min 12 s`, 27 bold white |
| Divider | 20 | 132 | 600 | 1.5 | `#1a335f` |
| Column headers | 384 / 496 | 144 | — | 14 | `TIME` / `RECOVERED`, 10.5 `text/caption`, right-aligned |
| Step rows ×5 | 20 | 166 + i·38 | — | 34 | §3 |
| Footnote | 20 | 358 | 400 | 14 | `material splits: assumed · to be validated in openLCA`, 10 italic `text/tip` |
| Sent message slot | 20 | 372 | 290 | 44 | hidden until sent (§5) |
| Action button | 326 | 372 | 290 | 44 | Send → Done (§5) |
| Grabber bar | centered | below panel | 200 | 22 | per `00` §5 |

Everything above the action row is **unboxed** — plain text on the panel.

---

## 3. Per-step rows

Row anatomy (per step i):

| Part | Position | Style | Content |
|---|---|---|---|
| Title | x 20 | 13.5 bold white | `{n} · {steps[i].title}` |
| Materials line | x 20, +16 | 11 `text/caption` | top-3 materials by weight with grams, remainder → `other {x} g`, joined ` · ` |
| `longest` tag | →422 right | 10.5 `#f0c879` | visible only on the longest step |
| Time split | →484 right | 13.5; longest = bold gold | `M:SS` from `step_times_s[i]` |
| Step mass | →616 right | 13.5 bold white | sum of the step's component weights |

**Data (vcu_001):** step masses 20 / 58 / 185 / 20 / 378 g (Σ 660 g — total not
displayed since v3). Materials per step come from `components[]` grouped by
`disassembly_step`; a component with `material_breakdown[]` contributes its
entries, otherwise it contributes one entry (short display label by id, mapped
in `CompletionSummaryView.ShortLabel`).

**Gram formatting:** ≥ 2 g → integer; < 2 g → one decimal (`gold plating 0.8 g`).

---

## 4. Timing behaviour

- Flow stopwatch: starts on step-flow entry, stops at final Confirm (unchanged).
- **Splits (v3):** `StepFlowController` records `now − stepStart` at every
  Confirm and resets the step timer; the array ships to
  `CompletionSummaryView.SetSession(elapsed, done, total, int[] splits)`.
- Cancelled runs (Back → modal → Yes) discard their splits (cleared on entry).
- Longest = max split; gets the gold tag + bold gold time. Suppressed when only
  one split exists.

---

## 5. Actions — single-button flow (unchanged from v2.1)

| State | Left slot (290×44 at x 20) | Action button (290×44 at x 326) |
|---|---|---|
| Initial | empty | `Send recovery report ›`, teal primary → POST (§6) |
| Sending | empty | `Sending…`, non-interactive |
| Sent | `Report was successfully sent`, 13 `teal/text`, right-aligned | `Done`, chevron hidden → Main Page |
| Failed | empty | `Could not send — retry` |

No "✓" glyphs in text (not in the SF Pro SDF atlas).

---

## 6. Recovery report (backend v0.6)

**Endpoint:** `POST /dpp/{product_id}/report` — payload gains `step_times_s`:

```json
{
  "product_id": "vcu_001",
  "timestamp": "2026-07-14T10:32:08Z",
  "elapsed_s": 252,
  "steps_completed": 5,
  "step_times_s": [38, 104, 51, 29, 30],
  "recovered_component_ids": ["...all 12"],
  "co2_avoided_kg": 6.57
}
```

`RecoveryReport.step_times_s: List[int]` (default empty — old clients stay
valid). Per-step splits are the thesis user-test dataset: every run stores a
per-step timing record server-side at zero extra effort.

---

## 7. Data bindings

| UI element | Source | Fallback |
|---|---|---|
| Total time | StepFlowController stopwatch | `— min — s` |
| Step titles | `disassembly.steps[].title` | `Step n` |
| Materials lines | `components[]` by step; `material_breakdown[]` where present | builder-baked demo |
| Step masses | Σ `components[].weight_g` per step | baked |
| Time splits + longest | `SetSession` splits | `—` |
| Report payload | §6 | — |

**Schema additions (2026-07-14):** `MaterialShare {material, weight_g}`;
`Component.material_breakdown: MaterialShare[]` + `material_breakdown_basis`
(vcu_001: fasteners, connectors, actives — splits sum exactly to the frozen
component totals); `RecoveryReport.step_times_s`. Mirrored in `DPPModels.cs`.
`schema/dpp_schema.json` still not regenerated (known drift).

---

## 8. Removed in v3 (kept here so nobody reintroduces them blindly)

Eyebrow (model · serial) · Total-time label · Steps-completed card + segments ·
RECOVERED label + 2×2 grid (gold pins with stale "+ USB" copy, silicon, aluminium,
CO₂-avoided card) · "Total recovered 660 g · 5 streams" headline (v3.0 draft only).
Rationale: text minimalism + recycler-relevant reward (per-step output beats
abstract eco-figures for this stakeholder).

---

## 9. Future implementation

1. PDF recovery report server-side (unchanged plan).
2. Post-send options: `Close application` / `Scan a new device`.
3. Real material splits from openLCA replace the assumed `material_breakdown`
   values (then drop the footnote), plus real recovered masses after physical
   teardowns.
4. Per-step benchmark line (e.g. vs personal best) — deferred; conflicts with
   the v3 minimal-text direction for now.

---

## 10. Behaviour notes

- Single panel; exploded canvas hidden here.
- Entering the summary marks the session complete; re-entering the flow restarts
  timer, splits and task states.
- All figures are estimates until a real teardown (basis flags apply).

---

## 11. Approved SVG mockup (v3.2, 2026-07-14)

> Source: `DPP_UI_Specs/drafts/09_summary_v3_2.svg`. Shown values are demo;
> Unity binds live data. The v3.2 mock still shows the totals-row variant with
> "Total recovered 660 g" — the approved build removes that block and the
> time label (final direction: value only). Treat this spec as authoritative
> over the mock for those two removals.

---

*Last updated: 2026-07-14 · Status: approved & Editor-tested (v3 — per-step table; "works for now", may iterate after user tests) · Prev: 08 Step 5 · Completes the Disassembly flow (01–09).*
