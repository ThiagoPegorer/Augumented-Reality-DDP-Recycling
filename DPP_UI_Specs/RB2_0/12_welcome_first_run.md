# DPP UI Spec — 12: Open App routine (Welcome canvas + First-run prompt)

> **Living spec** — updated every iteration. Current status: **✅ BUILT & DEVICE-TESTED (2026-07-29)** —
> ReBuilt v2.0 **block 1 of 4**. Not yet frozen: RBv2.0 is released only after the full end-to-end pass.
> Journey source: Miro `USER JOURNEY DIAGRAM` v4 (`OPEN APP ROUTINE`). Mocks:
> `drafts/12_v2_welcome_canvas.svg`, `drafts/12b_v2_first_run.svg`
> (superseded: `12_welcome_canvas.svg`, `12b_first_run_modal.svg`).

---

## 1. Purpose & flow

RBv1.0 dropped the user straight into the scan viewfinder at launch. RBv2.0 puts a **Welcome canvas**
in front of it. Two reasons, both structural rather than cosmetic:

1. **Welcome is the universal return target.** With the tab bar removed (spec 01 / 00 §5 pills cut),
   navigation is a single linear path with one-step-back edges. Every chain terminates at Welcome —
   `DPP CANVA` →Back→ Welcome, and the end-of-process loop returns here.
2. **The kiosk cycle needs a resting state.** Between participants the headset sits on a table.
   A viewfinder hunting for a QR code is not a resting state; a Welcome card is.

```
LAUNCH APP → WELCOME ──Continue to scan──→ SCAN QR CODE ──decoded──→ FIRST TIME USING THE APP?
           └──Close app──→ quit                                        ├─ Yes → tutorial (block 4)
                                                                       └─ No  → OPEN MAIN CANVA
```

## 2. Welcome canvas (`WelcomeCanvas`, 640 × 430)

Standard panel geometry (00 §1), navy panel background, own world-space Canvas + GraphicRaycaster +
grabber bar (00 §4 on-plane rule, 00 §5).

| Element | Spec |
|---|---|
| Brand mark | **ReBuilt logo, 96 × 96, centred at (320, 124), no backing disc** (rev 2) |
| Title | "Welcome to ReBuilt" — 32 pt bold, centred, y = 206. **Note: the Miro diagram still reads "ReBuit"; the build is correct.** |
| Subtitle | "Digital Product Passport for guided dismantling" — 14 pt secondary, y = 241 |
| `Close app` | secondary pill, 180 × 52, centre (114, 376) |
| `Scan to start` | primary teal pill + chevron, 388 × 52, centre (422, 376) (rev 2 label) |

Both buttons carry the hover-only white outline (00 §4 global hover rule) via `HoverHighlight`,
and the pill hit area exceeds the ≥50 px minimum.

### 2.1 Brand mark — revision 2 (2026-07-30)

Asset: **`Assets/Textures/Brand/rebuilt_logo.png`** — 512 × 512, transparent, `preserveAspect`.
Derived from Thiago's master file (6000 × 3375, real alpha channel — the black in the source view was
never a background), trimmed to the mark's bounding box (1127 × 1007) and padded square with 6 % margin.
Kept OUT of `Assets/Textures/UI/`, which `DPPSpriteFactory` owns and regenerates.

`DPPUIBuilder.LoadBrandLogo()` fixes the import settings on first use (Sprite / Single /
alphaIsTransparency / **mipmaps OFF** / Clamp / Bilinear / max 512), so a fresh clone needs no manual
Inspector step. If the asset is missing the builder logs a warning and falls back to the RBv1.0 generated
mark — the entry screen must never render blank.

**Two decisions:**

1. **No backing disc.** The RBv1.0 mark was a white glyph on a `teal/accent` disc. The logo carries its own
   circle and its own blue→green gradient; a teal disc behind it fights both.
2. **96 px, not the old 72.** The recycling glyph was a chunky 3-arrow shape that survived at 40 px. The
   logo has thin strokes and an interior infinity curve that closes up below ~96 px at panel scale.

⚠ **Known and accepted:** the logo's palette sits outside the 00 §2 tokens — lighter and yellower than
`teal/accent`, which is the colour of the CTA directly beneath it. The two greens are visibly not the same
green. Recolouring a logo to match a UI palette is the wrong trade, so it stays as authored.

## 3. First-run prompt (`FirstRunCanvas`, 640 × 430) — rev 2

Shown **after a successful scan and passport fetch, before the main canvas opens**. Own canvas,
`sortingOrder = 10`, **panel chrome** (`RoundedR22` + `navy/panel`), own grabber bar, and it
**recenters in front of the user each time it opens** (0.7 m, −0.05 m height offset) — the participant
may have moved while scanning.

| Element | Spec |
|---|---|
| Title | "First time using ReBuilt?" — 30 pt bold, centred, baseline y = 168 |
| Subtitle | "A quick tutorial shows you how to interact in AR." — 14 pt secondary, baseline y = 198 |
| `Skip` | secondary pill 180 × 52, centre (114, 376) |
| `Tutorial` | primary teal pill + chevron 388 × 52, centre (422, 376) |

**Design decision (2026-07-29): the question is asked after EVERY successful scan, not once per
install.** It sits inside the kiosk cycle, so each participant is offered the tutorial regardless of
who wore the headset before them. A persisted "already seen" flag would silently skip the tutorial for
participant 2 onward — unacceptable for a comparative study.

### 3.1 Revision 2 (2026-07-30) — standard panel size

Mock: `drafts/12b_v2_first_run.svg` (option B). Four changes, in order of consequence:

1. **440 × 210 modal card → the standard 640 × 430 panel** (00 §1). Thiago's instruction: the panel
   size is now a project standard, and a one-off card size for a single question was the odd one out.
2. **Modal chrome → panel chrome.** The stroke-behind-fill `RoundedR20` card border read as a frame
   inside a frame once the card filled the standard footprint. `sortingOrder` stays **10** — this is
   still drawn on top of the panel canvases, it just no longer *looks* like a small floating dialog.
3. **`No, skip` / `Yes, show me` → `Skip` / `Tutorial`**, on the **Welcome canvas button geometry
   verbatim** (180 × 52 @ cx 114, 388 × 52 + chevron @ cx 422, cy 376). Welcome is the screen the
   participant came from two steps earlier, so neither hit target moves.
4. **Pinch glyph and a "Two steps · about a minute" caption were trialled and cut.** Both appeared in
   the first mock; Thiago removed them. That leaves two lines of text on a tall panel, so the text
   block is optically centred in the space **above** the buttons (baselines 168 / 198) instead of
   sitting on Welcome's baselines (216 / 246). With no logo above it, Welcome's empty top third would
   read as an image that failed to load rather than as deliberate air.

⚠ **Two accepted trade-offs, both worth naming in the methodology:**

- The asymmetric pair **weights the choice toward the tutorial** — 388 px of teal against 180 px of
  grey is not a neutral presentation of two options. Defensible because the tutorial is part of
  Condition B's design, but it is steering, and it should be reported as such.
- The narrow left pill was **`Close app`** on the Welcome canvas and is **`Skip`** here. Same size,
  same colour, same coordinates, different consequence. Judged low risk (the labels differ and the QR
  scanner sits between the two screens) but it is a real position-learning collision.

**Open:** `FirstRunPrompt.spawnDistance` is still **0.7 m / −0.05 m**, tuned when this was a small
floating card. The main panel sits at **0.6 m, eye height 1.1176**. Now that the prompt is the same
size as the main panel, matching those two values exactly would make the hand-off seamless. Not
changed — it touches runtime placement, which was outside the approved mock.

## 4. Changes to existing components

| Component | Change |
|---|---|
| `QRScanController` | New `waitForWelcome` flag (default ON): the controller no longer scans at launch; `WelcomeController.ContinueToScan()` calls `BeginNewScan()`. New `firstRunPrompt` reference: on fetch success it calls `firstRunPrompt.Show()` instead of opening the main canvas directly. Null ref = RBv1.0 behaviour. |
| **`Continue with demo unit` (spec 11 §5)** | **REMOVED as a user-facing button** — entry is QR-only (Thiago, 2026-07-29). `UseDemoUnit()` survives **Editor-only** (`#if UNITY_EDITOR`, 1.5 s auto-continue) so Play Mode still reaches the main page without a camera. On device the path is unreachable. |
| `scanOnStart` | Unchanged — operator-level study-day kill-switch, not a participant-facing bypass. |

⚠ **Single-point-of-failure note:** spec 11 §5 required the demo fallback so a participant could never
be stranded on a scanner that won't scan. That safety net is now operator-level only. Mitigation: QR
decode was validated at arm's length in ~61 ms (spec 11 stage 2), and the operator can flip
`scanOnStart` off before a session. **Verify the scan still succeeds on the study-day hotspot before
Jul 31.**

## 5. Files

| File | Role |
|---|---|
| `Assets/Scripts/DDP/UI/WelcomeController.cs` | `ShowWelcome()` · `ContinueToScan()` · `CloseApp()`. `showOnStart` toggles legacy entry. |
| `Assets/Scripts/DDP/UI/FirstRunPrompt.cs` | `Show()` · `ChooseYes()` · `ChooseNo()` · **`event Action TutorialRequested`** |
| `Assets/Editor/DPPUIBuilder.Welcome.cs` | `RBv2_0/3 — Welcome + first run` (`Build3_WelcomeFirstRun`) |

**Build order:** `RBv2_0/3` needs `RBv2_0/1` (finds `DPPPanelCanvas`) and `RBv2_0/2` (finds
`QRScanController` to wire Continue). It destroys and rebuilds only its own two canvases — safe to
re-run at any time, and it does not touch `DPPPanelCanvas`.

## 6. Hooks reserved for later blocks

- `WelcomeController.ShowWelcome()` — **block 2/3** call this for the `DPP CANVA` Back edge and the
  `END OF PROCESS → scan a new QR code?` = NO branch.
- `FirstRunPrompt.TutorialRequested` — **block 4** subscribes and replaces the current fall-through.
  Do not edit `FirstRunPrompt` to add the tutorial; subscribe to the event.

## 7. Open items

- `FIRST TIME? = NO` is final for that scan — there is no way back into the tutorial mid-session.
  Accepted for v2.0 (low priority; the prompt returns on the next scan anyway).
- Close-app quit behaviour is untested on PICO OS beyond `Application.Quit()`.

## 8. Iteration log

- **2026-07-29 (a)** — Spec drafted from Miro journey v4; mocks `12_welcome_canvas.svg` +
  `12b_first_run_modal.svg` approved.
- **2026-07-29 (b)** — Built (Phase 7) and **device-tested successfully** on the PICO 4 Ultra.
- **2026-07-29 (c)** — Spec file written retroactively: the build chat froze immediately after the
  device test and the spec was never committed to disk. Content reconstructed by reading the shipped
  code, not from memory of the conversation.

- **2026-07-30 (rev 2)** — Brand logo replaces the generated teal-disc mark (96 px, no disc);
  primary label `Continue to scan` → **`Scan to start`**. Mock `drafts/12_v2_welcome_canvas.svg`.
  Approved by Thiago before coding, per the mock-first routine.
- **2026-07-30 (rev 3)** — First-run prompt rebuilt at the standard **640 × 430** with panel chrome;
  labels → **`Skip` / `Tutorial`** on the Welcome canvas geometry; pinch glyph and the "Two steps"
  caption cut; text block optically centred above the buttons. Mock `drafts/12b_v2_first_run.svg`
  option B, approved before coding. See §3.1.

*Last updated: 2026-07-30 · Status: rev 3 coded, awaiting device check — RBv2.0 block 1 of 4*
