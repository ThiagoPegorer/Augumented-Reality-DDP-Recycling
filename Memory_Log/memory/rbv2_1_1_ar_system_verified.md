---
name: rbv2-1-1-ar-system-verified
description: "[P] THE verified record of the RBv2.1.1 AR system, re-derived from source code and specs on 2026-08-22 for Methodology 3.4. CORRECTS FOUR ERRORS carried in older memory and in the specs: there is NO world anchor anywhere; the four tabs changed on 2026-08-08 and TabCount really is 4 with the gate requiring all four; the guided disassembly reuses the passport rig rather than routing to separate screens; and the hover outline is retired in favour of elevation. Also carries two dated, code-documented participant-driven design changes."
type: project
---

# 🔴 FOUR CORRECTIONS. All were about to be written into the thesis.

## 1. THERE IS NO WORLD ANCHOR. There never was.
A whole-tree search of `XR/AR_DPP_VCU/Assets/Scripts` for `SpatialAnchor`, `PXR_MixedReality`,
`CreateAnchor`, `ARAnchor`, `TrackingOrigin` and `worldAnchor` returns **zero hits**. The only
"anchor" is `anchoredPosition`, a 2D RectTransform property. ✅ The claim never reached the .docx.
**What is true:** no spatial anchoring. Nothing is pinned to the room. The panel and the model are
positioned relative to the participant.

## 2. FOUR TABS, and the gate requires ALL FOUR. `TabCount = 4`.
`SuperPanelView.cs` line 45: `public const int TabCount = 4;` · line 46: `public const int CertTab = 3;`
`GateOpen` loops `for (int i = 0; i < TabCount; i++) if (!_visited[i]) return false;`

| # | Tab |
|---|---|
| 0 | **Product specifications** |
| 1 | **Usage & service** (⚠ NOT "Usage history") |
| 2 | **Environmental impact** |
| 3 | **Certificates & safety** (red regulatory identity) |

⚠ **`04e_rail_gate.md` §1 and §2 are STALE** and say certificates sits outside the walkthrough. The
file's own header marks §1 struck and records round 2 making certificates a real fourth tab. **The
code wins.** [[rb2_1_dpp_page]] is also superseded: tab 4 was Training disassembly, now removed.
`SuperPanelView.cs` line 40: *"04e v2 (2026-08-08): Training disassembly is GONE, repetitive with the
guided flow that follows."*

**The gate.** One control at the rail foot. Locked: grey `#2B3A52`, label "Continue to disassembly".
Open: green `#27C46C`. A locked press swaps the label to **"Visit every tab first" for 1.8 s**
(`CtaHint`, `WaitForSeconds(1.8f)`), then repaints. ⚠ The spec's second string, "Read Certificates &
safety first", is **not in the code**. Product user: label "Back", navy, always enabled, returns to the
stakeholder fork. Walkthrough gating is recycler-only.

## 3. THE GUIDED DISASSEMBLY REUSES THE RIG.
`OnRailCta`: *"Spec 10: the guided mode swaps THIS rig's rail + data in place — the model never leaves
the screen."* Seven rail entries replace the four tabs: intro, steps 1 to 5, summary. It does **not**
route to separate screens; the RB2_0 flat route survives only as a fallback.

## 4. THE HOVER OUTLINE IS RETIRED.
`HoverHighlight.cs` line 46: `useOutline = false`. Hover is elevation: rise toward the user,
`hoverScale = 1.03f`, deeper shadow, `brighten = 0.09f`, `riseSeconds = 0.09f`. ⚠ The design file's
gesture table still says "hover outline appears" and `03_stakeholder_decision.md` still lists a white
hover outline. Both stale, harmless to the build.

# 🔴 TWO PARTICIPANT-DRIVEN CHANGES, DATED AND DOCUMENTED IN THE CODE

**A. The super panel's geometry exists because two participants missed the model.**
`SuperPanelView.cs`, verbatim: *"WHY THIS EXISTS: P02 and P03 both reported not perceiving the 3D
model. v1 answered by spawning the model first; it was still BESIDE the passport. v2 puts it between
the navigation and the data, so there is no state in which the passport is visible and the model is
not."*

**B. All application sound exists because P02 asked for it.**
- `UIClickAudio.cs`: *"One click sound for every UI button (P02 feedback, 2026-08-01: 'I missed sounds
  when clicking and interacting')."*
- `HandPinchAudio.cs`: *"reworked on Thiago's review, 2026-08-01: 'add the sound just when the user
  drags some object; make the loop a WIND with a different equalizer per drag direction'."*

**The chain is one day tight.** P02's session ran **2026-07-31 at 18:30 UTC**; both audio components
were written **2026-08-01**.

# ✅ THE ARCHITECTURE, all [P] 2026-08-22

| Item | Value | Traced to |
|---|---|---|
| Engine | Unity 6, **6000.0.73f1** | `ProjectSettings/ProjectVersion.txt` |
| Headset SDK | **PICO Integration SDK 3.4**, namespace `Unity.XR.PXR` | `EnablePassthrough.cs` |
| Render | Universal Render Pipeline **17.0.4** | `Packages/manifest.json` |
| Hands | **XR Hands 1.8.0** | manifest |
| Input | Input System 1.19.0 · glTFast 6.19.0 · Newtonsoft JSON 3.2.2 | manifest |
| QR | **ZXing**, as `Assets/Plugins/zxing.unity.dll` | plugins folder + `using ZXing` |
| Tween | **DOTween** (`Assets/Plugins/Demigiant`) | plugins folder |

**Passthrough.** Video See-Through forced on at runtime: `PXR_Manager.EnableVideoSeeThrough = true`,
because on Unity 6 + PICO SDK 3.4 the inspector checkbox configures the feature but the stream needs an
explicit runtime assignment. The camera uses a fully transparent solid-colour background.

**Entry.** QR only. Content is `dpp:<product_id>`; anything else is ignored and the scan continues.
Welcome → Continue → `BeginNewScan()` → decode → camera stops → ~1 s "Found" → `FetchAndPopulate(id)`
→ first-run prompt → main canvas. 🔴 **The 10 s "Continue with demo unit" fallback was REMOVED
2026-07-29.** Three named states: **Scanning · Found · BackendError.**

**Backend** (`DPPClient.cs`, 76 lines, `UnityWebRequest`): `GET /dpp/{product_id}` ·
`POST /dpp/{product_id}/report` · base URL serialized (`http://localhost:8000` in editor, LAN on the
headset) · **timeout 10 s**, with the reason in the tooltip: *"Unity's default is 0 = wait FOREVER,
which froze the summary screen when the backend was unreachable (2026-07-20)."*

# ✅ THE SUPER PANEL, as built

**Three world-space canvases toed in about one rig:**

| Canvas | Size | Yaw | Role |
|---|---|---|---|
| RAIL | 220 × 430 | yawed to face the eye | navigation only |
| STAGE | 340 × 430 | 0 | the model, permanently on screen |
| DATA | 420 × 430 | yawed to face the eye | the active tab's content |

⚠ **No yaw angle is recorded anywhere.** The spec says only "yawed to face the eye". Any degree value
in a diagram is a drawing convention, not a measurement. **Never write a number for it.**

⚠ **The rig is NOT the panel canvas.** Every other screen stays on the flat **640 × 430
`DPPPanelCanvas`**; world scale 0.001 makes that 0.64 × 0.43 m in the room.

**Locked versus unlocked model.** Locked, the model is a child of the stage and yaws slowly. Unlocked,
it re-parents out with its own grab bar and the two-hand gestures. **Re-lock SNAPS it home**, because
once freed it no longer follows the rig. ⚠ **The layout never reflows** when the model leaves; the
stage keeps a ghost outline. Model picks are **inert outside Product specifications and Usage**.

# ✅ GESTURES AND SOUND, verified 2026-08-22

Hand tracking only, no controllers. Arc knobs and sliders were built, tested and rejected in RB2.0.
Every gesture reads the **`RayPose` child** of each `PXR_Hand`; the hand root is static and silently
gives wrong results.

`TwoHandTwistRotate.cs` serialized defaults: `minHandSeparation = 0.05f` · `zoomThreshold = 0.25f` ·
`zoomFullSeparation = 0.55f` · `maxZoom = 2f` · `bandHysteresis = 0.01f`. Bands are **exclusive**:
5 to 25 cm rotates (yaw 1:1, size frozen), beyond 25 cm is an absolute size dial (25 cm = fitted,
55 cm = 2x, rotation frozen). Guards: below 5 cm nothing happens · panel drag beats model gestures ·
part manipulation blocks the two-hand pair · gestures pause while a modal is open.

**Aim-free part drag:** the closest-point parameter between the hand ray and the part's axis, so only
motion along the axis matters.

**Gesture readout** (`ZoneGestureHUD.cs`): help button, L/R hand lights, YAW, DIST, ZOOM. The live
mechanism's row is tinted.

**Three sounds**, all 2D (`spatialBlend = 0`), files in `Assets/Audio/UI/`, 48 kHz mono 16-bit:
click on every `Button.onClick` (swept in once by `UIClickAudio`) · grab, a water drop pitched 680 Hz
right and 410 Hz left, panned ±0.6 · drag, a 2.0 s wind loop equalised by direction (up opens to
7500 Hz, down closes to 380 Hz, sideways pans and bends pitch, speed drives volume to max 0.4).
**Every sound has a defined silence:** an air pinch is silent (the interaction scripts call
`ObjectGrabbed`, they do not poll the hands); an object held still is silent; a part at its extraction
limit goes quiet while the hand keeps pulling, because the loop follows the part.
**Rule:** sound attaches to an outcome, never to a gesture. The first drag looped on any held pinch
and was rejected on feel within one test.

# ✅ THE DESIGN STANDARDS FILE

`DPP_UI_Specs/RB2_1/00_design_standards_rbv2.md`, 509 lines, splits at line 327 into `# UX`.
§1 canvas geometry · §2 colour tokens and the four sanctioned meanings of red · §3 typography and the
glyph rule · §4 interaction and the elevation kit · §5 components · §6 gestures · §7 sound ·
§8 product reference and the CAD-to-passport map.

⚠ **The glyph rule:** the SF Pro SDF atlas lacks `≤`, `→`, `Ω` and the true minus. A missing glyph
renders as a visible box on device. **Two shipped bugs** came from breaking it.
⚠ RB2_1_1 has **no 00 of its own**, so the RB2_1 file is the standing design system for the frozen build.

⚠ **A contradiction still open:** the design file gives the model three colours (brown lower housing,
yellow upper, green connectors and board) while Table 2 in 3.2.1 gives the printed model grey
connectors and yellow/brown/blue/red components. Both cannot describe the same object.

# ⚠ STILL NOT TRACED

- **The freeze date.** Project memory says 2026-08-10; the on-disk handoff is dated 2026-08-07.
  Currently written out of the thesis rather than guessed.

Related: [[rbv2-1-1-handoff]], [[rb2_1_dpp_page]] (⚠ superseded on the tabs),
[[study_build_version_finding]], [[ch3_methodology_progress]], [[study_design_verified]]
