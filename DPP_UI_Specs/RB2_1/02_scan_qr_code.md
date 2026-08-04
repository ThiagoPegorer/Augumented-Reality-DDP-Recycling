# RBv2.1 — 02: Scan QR code (`SCAN QR CODE` + scan-failure loop)

> **Living spec.** Status: **✅ APPROVED 2026-08-04** — reviewed on device screenshots by Thiago:
> *"all UI/UX works very nice, scanner UI follows the user head, the 'VCU found' page looks good
> also, we also can close open questions."* One change requested, on the error panel (§5).
> Resolves against `00_design_standards_rbv2.md`. Routine: `RB2_1_routine.md` §3.
> Predecessor: `../RB2_0/11_qr_entry.md` (feature-complete + device-validated 2026-07-21).

---

## 1. What this screen is

The user points the headset at the **QR code taped on the physical unit**; the app decodes it,
fetches that unit's passport from the backend, and hands off to the stakeholder screen (`03`).
**One scan = one session** — the passport shown is the passport of the object on the desk, which
is the entire premise of the product.

```
WELCOME ──Scan to start──▶ SCANNING ──decoded──▶ VCU FOUND ──fetch ok──▶ STAKEHOLDER (03)
                              ▲                                │
                              │                            fetch fail
                              │                                ▼
                              └────Scan again──────── COULD NOT REACH SERVER ──Close app──▶ quit
```

This screen owns the routine's `SCAN QR CODE` checkpoint, its `QR CODE SCAN SUCCESSFULLY?`
decision, and the `ERROR MESSAGE → CONTINUE APP?` loop.

## 2. QR payload and decoding

| | |
|---|---|
| Format | **`dpp:<product_id>`** — a custom URI scheme carrying only the id, so the code stays valid if the backend host or IP changes |
| Generator | `backend/qr_generator.py` · printed asset `backend/qr/vcu_001.png`, taped to the housing |
| Decoder | **ZXing.Net** (QR-only, TryHarder, AutoRotate) on a worker thread, ~6 decodes/s |
| Frames | **`PXR_CameraImage`** (PICO SDK 3.4) → RGBA8888 raw CPU buffer → ZXing |
| Measured | **1024 × 768 @ ~30 fps** sustained; decode **~61 ms** at **arm's length** in home lighting, standard print size |

⚠ **`WebCamTexture` is dead on this platform** — PICO headsets return nothing through it. Anyone
"fixing" the camera pipeline by reaching for the Unity-standard API will lose a day.

⚠ **Camera permission does not prompt on PICO OS.** The runtime dialog never appears — the
request is instantly denied. The user grants **Camera** manually once in headset Settings → Apps
→ app → Permissions; the controller polls and starts when granted. **This is a study-day setup
step, not a code problem.** `android.permission.CAMERA` must be declared in a custom main
manifest (`SPATIAL_DATA` is not required for camera frames).

## 3. Scanning state — the viewfinder

**Fully transparent canvas, no panel background.** The user fits the physical QR inside the
brackets through passthrough, which also centres it in the camera frame and improves decode.

| Element | Spec |
|---|---|
| Title | **"Scan the product"** — screen title weight, bold, white, centred |
| Subtitle | "Look at the QR code on the unit" — `text/secondary`, centred |
| Brackets | four **`teal/light`** corner brackets forming the target area |
| Sweep | animated horizontal line travelling the bracket area |
| Status | **"Searching…"** — `text/tip`, pulsing, below the brackets |

### 3.1 Head-follow — confirmed working

The canvas **lazily follows the user's head** at **0.75 m**, framerate-independent lerp, and has
**no grabber bar** — it is the one screen in the product the user cannot reposition, deliberately:

- The user is **moving their head to aim**, which is the interaction. A panel that stayed put
  would slide out of view exactly when it is needed.
- A grabbable viewfinder invites the user to park it somewhere and then aim the *headset*
  elsewhere, breaking the alignment between what they see framed and what the camera sees.

**Lazy, not rigid.** A viewfinder locked 1:1 to the head is nauseating; the lag lets small head
motions settle without the UI chasing them. Device-validated 2026-07-21, re-confirmed
2026-08-04 (*"scanner UI follow the user head"*).

**This screen is the exception to two standards, both intentional:** it is not 640 × 430
(`00` §1) and it has no grab handle (`00` §5). It is not a panel; it is a sight.

### 3.2 Design history — do not re-add a background

The viewfinder went **v1 panel → v2 panel-with-hole → v3 transparent**. v3 was chosen on device
with the verdict *"less pollute and more functional"*. A navy panel behind the brackets hides the
passthrough view of the object the user is trying to aim at.

## 4. Found state — "VCU found"

A **compact navy card**, not a full panel: this state lasts about a second and a 640 × 430 panel
appearing and vanishing reads as a glitch.

| Element | Spec |
|---|---|
| Mark | green ring (`teal/accent`) with a white **✓** — drawn from capsule bars, never typed (`00` §3.1) |
| Title | **"VCU found"** — bold, white, centred |
| Subtitle | "Loading the product passport…" — `teal/text`, centred |

Shown for a ~1 s beat while `DPPClient.GetDPP(product_id)` runs. The beat is deliberate: an
instant transition gives no confirmation that the *right* unit was recognised.

On success → **stakeholder screen (`03`)**.

⚠ **Changed from RB2.0:** success used to open the first-run prompt, which no longer exists
(`01` §4). It now opens `03`. `QRScanController.firstRunPrompt` is removed; the reference becomes
the stakeholder screen.

## 5. Error state — the `CONTINUE APP?` question · **rev 2026-08-04**

Same compact card chrome as the found state.

| Element | Spec |
|---|---|
| Title | **"Could not reach the passport server"** — bold, `safety/stroke` |
| Subtitle | **"Check the hotspot connection, then scan again."** — `text/secondary` |
| Left button | **`Close app`** — destructive pill, solid `safety/stroke` `#e24b4a`, white bold, no chevron |
| Right button | **`Scan again`** — primary pill, `teal/accent`, white bold, chevron |

### 5.1 What changed and why it is right

RB2.0 showed **`Retry`** (green, **left**) and **`Scan again`** (dark, right). Three problems, all
fixed by one change:

1. **The primary action was on the left.** Every other screen puts the go-forward action on the
   right in the 388 px teal pill. This screen taught the opposite reflex. Now corrected —
   `00` §5 states the rule explicitly because of this panel.
2. **The panel did not match the routine.** `Routine_RB2_1.pdf` specifies `ERROR MESSAGE →
   CONTINUE APP?` with exactly two answers: **No → CLOSE APP**, **Yes → SCAN QR CODE**. The panel
   now *is* that question, with the two buttons the diagram calls for.
3. **`Retry` and `Scan again` were nearly the same action** presented as a choice. Both re-attempt
   the fetch; `Retry` merely skipped re-aiming. Two buttons that do almost the same thing make
   the user decide something that does not matter.

⚠ **The subtitle had to change too.** It read *"…then retry."*, naming a button that no longer
exists. Now *"…then scan again."* — a stale instruction pointing at a removed control is the kind
of small wrongness that erodes trust in everything else on screen.

⚠ **What is genuinely lost:** re-attempting the fetch **without re-aiming at the QR**. If the
hotspot drops while the user has already put the unit down, they must pick it up and re-frame the
code. Judged acceptable — the physical unit is on the desk in front of them and the scan takes
~61 ms once framed.

### 5.2 Red beside green

`Close app` red sits directly beside `Scan again` green. Permitted under `00` §2.1 **only because
the two differ in label, position and size as well as hue** — a user with red-green colour
deficiency still reads "Close app, left" against "Scan again, right, wider". Do not make them the
same size.

## 6. ⚠ Single point of failure — unresolved, carried from RB2.0

RB2.0 spec 11 §5 required a **study-day fallback**: if no QR decoded within ~10 s, the screen
offered **"Continue with demo unit"**, so *a participant could never be stranded on a scanner
that will not scan*. That button was **removed as user-facing** on 2026-07-29 (entry is QR-only
for participants); `UseDemoUnit()` survives **Editor-only** and is unreachable on device.

**So the safety net is now operator-level only:**

| Mitigation | Status |
|---|---|
| QR decode validated at arm's length, ~61 ms | ✅ measured |
| `scanOnStart` kill-switch — operator disables QR entry before a session | ✅ exists, requires a rebuild |
| Camera permission granted manually in headset settings | ⚠ **must be checked before every study day** |
| Backend reachable — `DPPClient.BaseUrl` matches the hotspot's PC IP | ⚠ **must be checked before every study day** |

**The failure mode this leaves open:** camera permission silently revoked, or the hotspot IP
changed, and the participant is already wearing the headset. There is no in-app recovery. The
practical answer is a **pre-session checklist**, not code — scan the QR yourself once, on the
study-day network, before the first participant arrives.

## 7. Files

| File | Role |
|---|---|
| `Assets/Scripts/DDP/UI/QRScanController.cs` | state machine Scanning → Found (1 s) → Fetching → Done / BackendError; `BeginNewScan()`; camera destroyed after each hit and rebuilt per cycle |
| `Assets/Editor/DPPUIBuilder.QRScan.cs` | builder phase for the scan screen |

**Per-cycle camera rebuild** is what makes the kiosk loop work: a fresh scan fully repopulates
every screen, validated across multiple cycles.

## 8. Closed questions (resolved 2026-08-04)

| Question | Resolution |
|---|---|
| Head-follow behaviour | **Confirmed on device.** No change. |
| Found-state card size | **Approved as-is** — compact card, not a panel. |
| Error panel button set | **Resolved** → `Close app` (red) / `Scan again` (green), §5. |
| P03 (Domenik) feedback for this screen | **None** — reviewed and approved as-is. |

Still open and **not** this spec's to close: the single point of failure in §6, which is a
study-day process item.

## 9. Iteration log

- **2026-08-04** — First RB2.1 spec, carrying the device-validated RB2.0 scan feature forward.
  Error panel reworked to match the routine's `CONTINUE APP?` node: `Retry` removed, `Close app`
  (red) added, `Scan again` promoted to primary on the right, subtitle corrected. Success now
  routes to `03` instead of the deleted first-run prompt. Approved.

*Created 2026-08-04 · Status: **approved, ready to build** · Standards: `00_design_standards_rbv2.md` · Prev: `01` · Next: `03` stakeholder decision*
