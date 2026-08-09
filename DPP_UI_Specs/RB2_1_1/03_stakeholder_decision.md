# RBv2.1 — 03: Stakeholder decision (`STAKEHOLDER DIRECTION`)

> **Living spec.** Status: **DRAFT 2026-08-04 — new screen, no predecessor.** This is the first
> genuinely new surface in RB2.1; everything before it carried forward from a device-tested
> RB2.0 screen. Resolves against `00_design_standards_rbv2.md`.
> Routine: `RB2_1_routine.md` §2 / §4. **Blocked on one asset: the two card icons.**

---

## 1. What this screen is

The fork that makes ReBuilt **one app for two audiences**. Thiago, 2026-08-04: *"My application
can be used for two different stakeholders: the ones that just want to read the DPP and the ones
that are recyclers."*

```
VCU FOUND (02) ─▶ STAKEHOLDER DECISION ─┬─ Recycler ─────▶ DPP Canva ──▶ Disassembly (06–08)
                                        └─ Product user ─▶ DPP Canva      (no disassembly)
```

Both roles land on the **same** DPP Canva. The difference is one button: the recycler sees
`Continue to disassembly`, the product user does not. **The passport is not optional for either
role** — reading precedes dismantling by design, and the recycler cannot skip it.

⚠ **The routine diagram draws two `DPP CANVA` boxes.** That is conceptual. Implementation is
**one screen in two modes** (§5) — two built canvases would double every future DPP edit and
guarantee they drift apart.

## 2. Layout — 640 × 430, standard panel

Standard geometry (`00` §1), `navy/panel`, own world-space Canvas + GraphicRaycaster + grabber bar.

| Element | Position | Spec |
|---|---|---|
| Title | centred, y 36 | **"Select your role"** — 25 pt bold, white |
| Subtitle | centred, y 66 | "You can go back and change this." — 13 pt `text/secondary` |
| Header rule | y 76, x 24 → 616 | 1 px `#1a335f` |
| **Card A — Recycler** | **(24, 168), 290 × 170** | left column (`00` §1.1) |
| **Card B — Product user** | **(326, 168), 290 × 170** | right column |

Cards are vertically centred in the content band: 80 px above, 80 px below (mirror margins).

**No back circle and no Close app on this screen.** There is nothing one step back — the scan is
finished and the passport is already fetched. Both cards lead forward, and both destinations
carry `Quit`. Accepted, with §4.2 as the recovery path.

### 2.1 Card anatomy (card-local coordinates)

| Part | Position | Spec |
|---|---|---|
| Fill | 290 × 170 | `row/fill` `#0e2950`, `RoundedR13` |
| Stroke | 292 × 172 | `row/stroke` `#21407a` |
| Hover outline | 300 × 180 | white, **hover only** (`00` §4) |
| **Icon** | **(20, 24), 40 × 40** | authored PNG, see §3 |
| **Title** | x 76, cy 44 | 16 pt **bold** white |
| **Description** | (20, 88), w 250 | 13 pt `text/secondary`, wrapping, ~3 lines |

**The whole card is the button.** Chrome is correct here and required — under the *chrome =
touchable* rule (`00` §4) a bordered card promises tappability, and this one delivers. The 290 ×
170 hit area is far above the 50 px minimum.

## 3. Icons — **blocked, asset needed**

| | |
|---|---|
| Size | **40 × 40** rendered |
| Format | transparent PNG, authored by Thiago |
| Delivery | `DPP_UI_Specs/Icons/` — **not** `Assets/Textures/UI/`, which `DPPSpriteFactory` owns and regenerates on every run (an icon placed there is destroyed silently) |
| Tint | monochrome art is tinted `teal/light` `#5dcaa5`; full-colour art renders as authored |
| Fallback | if the asset is missing the builder logs a warning and draws the card **without** an icon, shifting the title to x 20 — a card must never render with an empty hole where art failed to load |

## 4. The two roles

### 4.1 Wording — role title, action description

| | **Card A (left)** | **Card B (right)** |
|---|---|---|
| Title | **Recycler** | **Product user** |
| Description | "Read the passport, then dismantle the unit with guided steps and timing." | "Read the passport only — product data, materials, impact and history." |

**Why the title is the role and the description is the action.** The role carries the thesis
argument — the DPP has distinct audiences with distinct rights of access, which is the point
being demonstrated — so it earns the headline. But a role alone asks the user to *self-classify*,
and the original right-hand label, `DPP User`, was jargon at the exact moment the user has not yet
seen a passport. The description does the disambiguating in plain words, so nobody has to know
what "DPP" stands for to choose correctly.

**The word "only"** in Card B carries the exclusion without a negative sentence. The user must
understand that this path does not reach dismantling — that is precisely the mis-tap this screen
has to prevent.

> **For the thesis:** "product user" stands in for the non-recycler DPP audiences named in the
> EU framework — owners, consumers, repairers, market surveillance. RB2.1 does not differentiate
> among them; one read-only role represents them all.

### 4.2 Mis-selection recovery — `Back` on **both** DPP Canva branches

A wrong tap here used to cost the whole session: the routine gives the product-user branch no
route onward except `Quit` → Welcome → re-scan. Closed by adding a **`Back` to this screen from
the DPP Canva**.

⚠ **I extended the decision from one branch to both.** Thiago approved Back on the product-user
branch; applying it to the recycler branch too is required by `00` §5 — *every Back moves exactly
one step*. A screen where Back works in one mode and not the other is the anomaly, and the
recycler needs it just as much: someone who taps "Recycler" and then realises they only wanted to
read has the same problem in reverse. **Say if you want it on the product-user branch only.**

Resulting edges on the DPP Canva (spec `04`):

| Control | Goes to | Modes |
|---|---|---|
| `Back` (header circle) | **this screen** | both |
| `Quit` | Welcome (`01`) | both |
| `Continue to disassembly` | Disassembly intro (`06`) | **recycler only** |

Re-entering this screen must **not** re-scan or re-fetch — the passport is already loaded and the
physical unit has not changed. Back here is a mode switch, not a session restart.

## 5. Implementation — one screen, two modes

A session-level enum, set here and read by the DPP Canva:

```
enum StakeholderMode { Recycler, ProductUser }
```

| Concern | Decision |
|---|---|
| Where it lives | `ScreenRouter` (or the session object that already owns routing state) — **not** a static, so re-entry through this screen can change it cleanly |
| What reads it | `04` DPP Canva: shows or hides `Continue to disassembly` |
| Default | **none.** No mode until the user chooses; the screen cannot be skipped |
| Kiosk reset | cleared on `ShowWelcome()` and on `BeginNewScan()` — participant 2 must never inherit participant 1's role |

⚠ **Record the mode in the dismantling report.** It is free to capture and it is a study
variable: a report that does not say which role produced it cannot be interpreted later. If every
participant is a recycler the field is constant — which is itself the evidence that the
product-user branch went untested (`RB2_1_routine.md` §8.6).

## 6. No tutorial pop-up on this screen

The routine attaches Tutorial nodes to eight points; **this is not one of them**. Correct as
drawn: the screen is two labelled buttons with descriptions, and a pop-up explaining a choice
that the cards already explain would be noise. Spec `09` must not add one here.

## 7. Files (new)

| File | Role |
|---|---|
| `Assets/Scripts/DDP/UI/StakeholderSelect.cs` | `ChooseRecycler()` · `ChooseProductUser()` — set mode, then `ScreenRouter.ShowDppCanva()` |
| `Assets/Editor/DPPUIBuilder.Stakeholder.cs` | new builder phase; destroys and rebuilds only this canvas |
| `ScreenRouter` | `+ StakeholderMode Mode { get; }` · `ShowStakeholder()` |
| `QRScanController` | fetch success → `ShowStakeholder()` (replaces the deleted first-run prompt call, `01` §4) |

## 8. Open items

1. **Card icons — blocked on Thiago.** Everything else can be built; the fallback in §3 keeps the
   screen shippable meanwhile.
2. **`Back` on the recycler branch** — extended by me (§4.2); confirm or restrict.
3. **Diagram inversion.** `Routine_RB2_1.pdf` puts DPP USER on the left and RECYCLER on the
   right; this screen is the reverse, per Thiago's 2026-08-04 description. **The screen is
   authoritative** — update the diagram so it stops disagreeing.
4. **Title wording** — "Select your role" is placeholder-grade. It is clear and standard; it is
   not warm. Worth one pass with fresh eyes before build.

## 9. Iteration log

- **2026-08-04** — Screen designed from the routine plus Thiago's layout brief (two rectangles,
  icon left of the name, description below). Wording resolved to role-title + action-description
  (§4.1). Mis-selection dead-end found and closed with a `Back` edge (§4.2). One-screen-two-modes
  implementation chosen over two built canvases (§5).

*Created 2026-08-04 · Status: draft, blocked on icons · Standards: `00_design_standards_rbv2.md` · Prev: `02` · Next: `04` DPP Canva*
