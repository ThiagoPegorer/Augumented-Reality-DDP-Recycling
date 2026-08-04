# RB2_2 — parking lot

> **Nothing here is specified yet.** This folder holds work that was consciously **deferred
> out of RB2.0 and RB2.1**, so the decision to postpone it is recorded with its reason instead
> of being re-litigated every session. Thiago, 2026-08-01: *"the tutorial layer and other
> implementations will be part of the RB2_2."*
>
> Baseline when RB2.2 opens: `../RB2_1/` (which itself inherits from `../RB2_0/`).

---

## 1. The tutorial layer — the headline item

Planned shape (Miro user journey v4, block 4): **pinch a button → pinch-drag a canvas onto a
marked AR location → `FINISH TUTORIAL?`**. Never built.

**Why it keeps slipping, and what it costs right now:** without it, every participant is taught
the gestures **verbally by Thiago**. That works because participants are close colleagues, but it
makes the operator part of the instrument — two participants never get identically worded
instructions, and the study cannot claim the app is self-sufficient. The standing mitigation is a
**one-page instruction script read verbatim** to every participant; that script is the RB2.2
tutorial's requirements document, already written in participant-facing language.

## 2. Deferred detail pages (RB2.1-tagged during the DPP Canva build)

| Item | State | Note |
|---|---|---|
| **Mechanical data** detail page | Tile is inert — `+` hidden, chips only | Technical drawing page; a new UX was wanted, never designed |
| **Electrical data** detail page | Tile is inert — `+` hidden, chips only | 3 connectors · 2 processors · CPU · 3 sensors, per the physical prototype |

Both tiles read as static chip rows today, which is honest (no dead `+`), but the DPP is
advertising two categories it cannot open. Whichever RB2.x builds them, they are one design
conversation, not two.

## 3. Interaction work parked behind the study

| Item | Why parked |
|---|---|
| **Enforced removal order** for the coloured electrical parts (yellow · red · brown · blue) | Touches `ConstrainedTeardownModel`'s dependency core. Changing task difficulty **mid-study confounds the participant comparison** — it cannot land while data collection is running. Possibly config rather than code; unverified. |
| **AI voice-guide layer** | P02 (Waldek) asked for it. Large scope, no design yet. |
| **Per-component LCA** | Locked decision from the RBv2.0 scope: v2.0 ships device-level LCA only. |

## 4. Housekeeping carried forward

- **Stale mock links.** RB2_0 specs point at `drafts/…`; from a version folder the mocks are at
  `../drafts/…`. Left broken in the frozen set on purpose; fix in whichever folder is live.
- **`MakeScrollWindow` duplication.** Usage and Compliance still carry inline copies of the
  scroll-window construction that Service factored out — a cleanup pass, not a feature.
- **`StepAction.icon`** survives in the payload and schema with no reader (retired in `04` v3.2).
- **openLCA contribution-tree export** (Sc1, EF 3.1, climate change, by stage) would restore a
  real S1–S4 breakdown in spec `14`; today `lifecycle_stages` is `[]`.

*Created 2026-08-01 · Predecessor: `../RB2_1/` · Registry: `../VERSIONS.md`*
