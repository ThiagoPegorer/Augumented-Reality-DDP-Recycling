# RBv2.1 — 01: Welcome page (`OPEN APP`)

> **Living spec.** Status: **✅ APPROVED 2026-08-04** — reviewed on device screenshots by Thiago:
> *"this page is very good, we can close all open questions."* One change requested: `Close app`
> becomes red. Everything else is as built and validated.
> Resolves against `00_design_standards_rbv2.md`. Routine: `RB2_1_routine.md` §3.
> Predecessor: `../RB2_0/12_welcome_first_run.md` (device-tested 2026-07-29, rev 3).

---

## 1. What this screen is

The app's **entry hall and its universal return target**. Two structural jobs, neither cosmetic:

1. **Every path home ends here.** Both DPP branches' `QUIT BUTTON` return to this screen, and so
   does the end-of-process loop. There is exactly one place the user comes back to.
2. **The kiosk cycle needs a resting state.** Between participants the headset sits on a table. A
   viewfinder hunting for a QR code is not a resting state; a welcome card is.

```
LAUNCH APP ─▶ WELCOME ──CONTINUE──▶ SCAN QR CODE  (spec 02)
                 │  ▲
       CLOSE APP │  └── QUIT from either DPP branch (spec 04)
                 ▼      END OF PROCESS → "scan a new QR code?" (spec 08)
             app quits
```

## 2. Layout — `WelcomeCanvas`, 640 × 430

Standard panel geometry (`00` §1), `navy/panel` background, own world-space Canvas +
GraphicRaycaster + grabber bar. Placement 0.6 m, eye height 1.1176 m, yaw-only.

| Element | Spec |
|---|---|
| Brand mark | **ReBuilt logo, 96 × 96, centred at (320, 124)**, no backing disc |
| Title | **"Welcome to ReBuilt"** — 32 pt bold, centred, y 206 |
| Subtitle | "Digital Product Passport for guided dismantling" — 14 pt `text/secondary`, y 241 |
| `Close app` | **destructive pill — solid `safety/stroke` `#e24b4a`, white bold label, no chevron** — 180 × 52, centre (114, 376) |
| `Scan to start` | primary teal pill + chevron **388 × 52, centre (422, 376)** |

Both buttons carry the hover-only outline (`00` §4) and exceed the 50 px hit minimum. The button
row uses the standard coordinates from `00` §5 — **these do not move on any screen.**

### 2.2 `Close app` in red — rev 2026-08-04

Changed from the dark secondary pill to **solid red**. Rationale: it is the only control on the
screen that ends the session, and red is the one colour a user reads as consequence without a
label. This adds a **third sanctioned meaning to red** in the standards (`00` §2.1: exit actions,
secondary slot only, one per screen).

Two constraints this puts on the screen, both satisfied here:

- **Red never carries the journey forward.** `Scan to start` stays teal, stays on the right,
  stays 388 px. Size and position hierarchy keep the intended action dominant even though red is
  the higher-salience hue.
- **Never colour alone.** The two buttons differ in label, position *and* size — required by
  `00` §2.1 because red-vs-green is the hardest pair for red-green colour deficiency (~8 % of
  men). A participant who cannot separate the hues still reads "Close app, small, left".

⚠ **Spelling.** `Routine_RB2_1.pdf` writes **"Welcome to ReBuit"**. The product is **ReBuilt**;
the build is correct and the diagram is wrong. Fix the diagram so it stops propagating.

### 2.1 Brand mark

`Assets/Textures/Brand/rebuilt_logo.png` — 512 × 512, transparent, `preserveAspect`. Kept **out
of** `Assets/Textures/UI/`, which `DPPSpriteFactory` owns and regenerates. The builder fixes
import settings on first use (Sprite / Single / alphaIsTransparency / **mipmaps off** / Clamp /
Bilinear / max 512), so a fresh clone needs no Inspector step. Missing asset → warning + fallback
mark; **the entry screen must never render blank.**

Two decisions carried from RB2.0, both still valid:

1. **No backing disc.** The logo carries its own circle and gradient; a teal disc behind it fights both.
2. **96 px, not 72.** The logo's thin strokes and interior infinity curve close up below ~96 px at panel scale.

⚠ **Accepted:** the logo's greens sit outside the `00` §2 tokens — lighter and yellower than
`teal/accent`, which is the colour of the CTA directly beneath it. The two greens are visibly not
the same green. Recolouring an authored logo to match a UI palette is the wrong trade.

## 3. Behaviour

| Trigger | Result |
|---|---|
| App launch | Welcome shows. **No scanning happens yet** (`QRScanController.waitForWelcome`). |
| `Scan to start` | `WelcomeController.ContinueToScan()` → `BeginNewScan()` → spec 02 |
| `Close app` | `Application.Quit()` |
| `QUIT` from a DPP branch | `WelcomeController.ShowWelcome()`. **Must route through `ScreenRouter.ShowDppCanva()` first** — the exploded action zone is a separate ROOT canvas that Welcome cannot hide, so skipping the router leaves a 3D model floating in an empty room. |
| `END OF PROCESS → NO` | same as above |

**Scanning is deliberately not started at launch.** RBv1.0 dropped the user straight into the
viewfinder; RB2.0 moved it behind the Continue press and that is retained.

## 4. What changes from RB2.0 — the first-run prompt is **removed**

RB2.0 showed a **`First time using ReBuilt?` → `Skip` / `Tutorial`** panel after every successful
scan. **RB2.1 deletes it**, because the tutorial has changed shape: it is no longer one gated
sequence before the product but **a pop-up on each page** (`RB2_1_routine.md` §5, spec `09`).
With per-page tutorials there is nothing left for an upfront yes/no question to gate.

`Routine_RB2_1.pdf` confirms this by omission: there is no `FIRST TIME USING THE APP?` node
anywhere. The Open App routine goes `OPEN APP → CONTINUE → SCAN QR CODE` with a Tutorial pop-up
hanging off `OPEN APP` itself.

**Consequences:**

| Asset | Fate |
|---|---|
| `FirstRunCanvas` | deleted from the build |
| `FirstRunPrompt.cs` | retired to `_to_delete/` — **device-tested code being removed, deliberately** |
| `FirstRunPrompt.TutorialRequested` event | dies with it; spec `09` attaches per page instead |
| `QRScanController.firstRunPrompt` reference | removed; fetch success opens the stakeholder screen (spec 03) directly |

⚠ **What is lost with it.** The RB2.0 prompt appeared **after every scan, not once per install**,
specifically so participant 2 was offered the tutorial as reliably as participant 1. Per-page
pop-ups must inherit that property: **if a tutorial pop-up ever persists a "seen" flag across
participants, the kiosk cycle silently degrades for everyone after the first.** Written here
because the requirement was born on this screen and must not die with it — spec `09` owns it.

⚠ **Also removed with it:** the only place the app ever explained itself before use. Until spec
`09` exists, RB2.1 has **no tutorial at all**, and the standing mitigation is unchanged — Thiago
reads a **one-page instruction script verbatim** to every participant. That script is a research
instrument, not a workaround, and it belongs in the thesis appendix.

## 5. Files

| File | Role |
|---|---|
| `Assets/Scripts/DDP/UI/WelcomeController.cs` | `ShowWelcome()` · `ContinueToScan()` · `CloseApp()` |
| `Assets/Editor/DPPUIBuilder.Welcome.cs` | builder phase for this canvas |

The builder phase destroys and rebuilds **only this canvas** — safe to re-run at any time, and it
does not touch `DPPPanelCanvas`.

## 6. Reserved hooks

- **`WelcomeController.ShowWelcome()`** — specs `04` (Quit, both branches) and `08` (end of
  process → NO) call this. Do not duplicate the routing; call the hook.
- **Tutorial pop-up anchor** — spec `09` attaches one pop-up to this page. Leave the top-right
  area of the header clear for its trigger affordance; do not fill it with content that would
  have to move later.

## 7. Closed questions (resolved 2026-08-04)

All open items from the draft are closed on Thiago's device review.

| Question | Resolution |
|---|---|
| P03 (Domenik) feedback for this screen | **None.** The screen was reviewed and approved as-is; his findings land on other screens. |
| Diagram says `CONTINUE BUTTON`, build says `Scan to start` | **Keep `Scan to start`** — it names its destination. The diagram label is generic, not a spec. |
| Close-app behaviour on PICO OS | **Accepted as-is.** `Application.Quit()` is the platform answer; no further work. |
| Placement: 0.7 m / −0.05 m vs 0.6 m / 1.1176 m | **Moot.** The only mid-session panel on this screen was the first-run prompt, now deleted (§4). Welcome uses the standard 0.6 m / 1.1176 m. |

**Remaining, and it belongs to `09` not here:** a per-page tutorial pop-up must **not** persist a
"seen" flag across participants (§4).

## 8. Iteration log

- **2026-08-04 (a)** — First RB2.1 draft. Carried the RB2.0 rev-3 canvas verbatim (logo 96 px,
  button geometry, labels); **removed the first-run prompt** and recorded what that costs (§4);
  added the Quit-routing rule the DPP branches depend on (§3).
- **2026-08-04 (b)** — Reviewed on device screenshots. **Approved.** `Close app` → solid red
  (§2.2), which added meaning 3 to the red rule in `00` §2.1. All open questions closed (§7).

*Created 2026-08-04 · Status: **approved, ready to build** · Standards: `00_design_standards_rbv2.md` · Next: `02` scan QR*
