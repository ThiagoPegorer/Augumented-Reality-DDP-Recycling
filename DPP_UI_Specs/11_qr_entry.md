# DPP UI Spec — 11: QR Code Entry (scan → DPP fetch)

> **Living spec** — updated every iteration. Current status: **✅ FEATURE COMPLETE & DEVICE-VALIDATED (2026-07-21)** — full kiosk loop working.
> This replaces `fetchOnStart` / `testProductId` as the app's entry mechanism.

---

## 1. Purpose & flow

The worker points the headset at the **QR code taped on the physical product**; the app decodes
it, fetches that product's Digital Product Passport from the backend, and opens the main panel
anchored in front of the user. One scan = one session; the DPP the user sees is the passport of
the physical unit on the desk.

```
launch → scan viewfinder (head-follow, transparent) → QR decoded "dpp:<product_id>"
       → "VCU found" beat → DPPClient.GetDPP(product_id) → main page opens
       → disassembly flow → summary → Send dismantling report
       → POST-REPORT MODAL: "Scan new product" (restarts the cycle) | "Main menu"
```

**The loop (2026-07-21):** the app is a kiosk cycle — scan → passport → dismantle → report →
scan again, indefinitely. "Scan new product" reactivates the scan screen, hides the main canvas
and rebuilds the camera capture session from scratch; a fresh scan fully repopulates all screens.

## 2. QR payload & assets (unchanged since April, still valid)

- **Format:** `dpp:<product_id>` (custom URI scheme) — backend-agnostic: the QR stays valid if
  the backend host/IP changes, because the code carries only the product id.
- **Generator:** `backend/qr_generator.py` · printed asset: `backend/qr/vcu_001.png` (tape it on
  the housing per Stage-2 build task).
- Decoder library: **ZXing.Net** Unity DLL (already installed; chosen over Vuforia).

## 3. Architecture decision (2026-07-20)

| Option | Verdict |
|---|---|
| `WebCamTexture` pipeline (old tracker plan) | **DEAD** — PICO headsets do not deliver passthrough frames through this API; it returns nothing on device. |
| **`PXR_CameraImage` (PICO SDK 3.4) → frames → ZXing decode** | **CHOSEN** — SDK 3.4.0 added camera image access (`PXR_CameraImage`, camera-data readback permission). Project already runs SDK 3.4. |
| PICO native marker tracking | Not chosen — ArUco sample exists but QR-with-payload decode via ZXing keeps our `dpp:` scheme and existing assets. Revisit only if stage 1 fails. |

**Known risk (unresolved until stage 1 runs):** PICO may gate camera data behind the SecureMR
privacy pipeline — raw CPU frame access for ZXing could be restricted or need extra permissions.
Stage 1 exists precisely to answer this with one device build.

## 4. Staged build plan (one provable increment per device test)

| Stage | Deliverable | Proves | Status |
|---|---|---|---|
| **1 — Camera probe** | `QRCameraProbe.cs`: permissions → enumerate → capabilities → device → session → acquire loop. | Frames are readable at all. | ✅ **PASSED 2026-07-20**: both RGB cameras enumerated; resolutions 2048×1536…640×480; RGBA8888 raw buffer confirmed; **1024×768 @ ~30 fps sustained** (stride 4096, 3 MB/frame). Permission note below. |
| **2 — Decode probe** | Probe v2: frame copies → ZXing worker thread (~6 decodes/s, QR-only, TryHarder, AutoRotate); live line shows decoded text + hit count + decode ms. | Decode quality, range, lighting tolerance. | ✅ **PASSED 2026-07-20**: `dpp:vcu_001` decoded reliably at **arm's length**, ~**61 ms**/decode, home lighting, standard print size — no larger label needed. |
| **3 — Flow wiring** | `QRScanController.cs`: state machine Scanning→Found(1s beat)→Fetching→Done/BackendError; `DPPManager.FetchCompleted` event; camera destroyed after each hit, rebuilt per cycle; Editor Play Mode auto-continues via demo path. `scanOnStart` = kill-switch. | End-to-end entry. | ✅ **PASSED 2026-07-21** (incl. multi-cycle loop) |
| **4 — Scan screen UI** | **v3 minimal viewfinder** (evolved v1 panel → v2 hole → v3 no background per user): fully transparent canvas that lazily HEAD-FOLLOWS (0.75 m, framerate-independent lerp, no grabber bar) showing only teal corner brackets + animated sweep, title/subtitle, pulsing "Searching…", demo-fallback button (fades in at 10 s, own dark fill). User fits the physical QR inside the brackets through passthrough — which also centres it in the camera frame (better decode). Found/error states = compact navy cards. Builder: **DPP → Build Phase 6** (also sets `fetchOnStart=false`). | UX. | ✅ **PASSED 2026-07-21** ("less pollute and more functional") |
| **5 — Loop routine** | Post-report modal on the summary (cancel-modal chrome): "Report sent — what's next?" → **Scan new product** (`QRScanController.BeginNewScan()`) / **Main menu**. Shown on send success; cleared by ResetState. | Kiosk cycle. | ✅ **PASSED 2026-07-21** |

## 5. Study-day fallback (REQUIRED — single-point-of-failure rule)

If no QR is decoded within **~10 s** of the scan screen opening (permission denied, lighting,
camera API failure), the screen offers **"Continue with demo unit"** → silently loads `vcu_001`
the old way. Scanning is a bonus, never a blocker: a participant must never be stranded on a
scanner that won't scan. The `scanOnStart` flag also allows disabling QR entirely for a study
session if stage 1–3 prove unstable.

## 6. Prerequisites / device checklist

- PICO Unity Integration SDK **3.4.0** ✅ (verified: `Packages/PICO Unity Integration SDK-3.4.0-20260226`).
- PICO 4 Ultra OS up to date (camera access is Ultra-era functionality).
- **Manifest declaration:** Unity → Project Settings → Player → Publishing Settings → enable
  **Custom Main Manifest**, then add inside `<manifest>` (before `</manifest>`):
  `<uses-permission android:name="android.permission.CAMERA" />` and
  `<uses-permission android:name="com.picovr.permission.SPATIAL_DATA" />`.
  Runtime dialog is handled by the probe.
- Backend reachable: personal hotspot at the university (AP isolation — see spec 00 / memory);
  `DPPClient.BaseUrl` must match the active network's PC IP.
- QR printed at a size scannable at arm's length (test in stage 2; reprint larger if range < ~40 cm).

## 7. Iteration log

- **2026-07-20 (a)** — Spec created. Architecture decided (`PXR_CameraImage` + ZXing), WebCamTexture
  plan declared dead, staged plan + fallback defined.
- **2026-07-20 (b)** — SDK 3.4.0 source verified in-project: full CPU pipeline exists
  (`GetAvailableCameras` → capabilities → `CreateCameraDeviceAsync` → `CreateCameraCaptureSessionAsync`
  → `BeginCameraCapture` → `AcquireCameraImage`/`GetCameraImageData`/`ReleaseCameraImage`).
  Format **RGBA8888**, transfer **raw CPU buffer** (`XrCameraImageDataRawBuffer`: width/height/
  stride/bytesPerPixel/bufferSize/pointer — ZXing-ready), cameras LEFT/RIGHT RGB passthrough,
  30/60 fps, pinhole model + intrinsics/extrinsics available (useful later for anchoring).
  **Stage 1 probe built** (`Scripts/DDP/UI/QRCameraProbe.cs`). Permissions used:
  `android.permission.CAMERA` + `com.picovr.permission.SPATIAL_DATA` (runtime request in probe).
  ⚠ No custom AndroidManifest exists in the project — permissions must be declared (see §6 note).

- **2026-07-20 (c)** — **Stage 1 PASSED on device.** Feasibility risk CLOSED: no SecureMR gating,
  raw CPU frames flow. Permission behaviour on PICO OS: runtime dialog does NOT appear — instant
  deny; the user grants **Camera manually** in headset Settings → Apps → app → Permissions
  (one-time). Probe polls and starts when granted. SPATIAL_DATA not required for camera frames
  (kept best-effort). Manifest: custom main manifest with `android.permission.CAMERA` required —
  GameActivity activity block removed from template (theme mismatch broke Gradle).
- **2026-07-20 (d)** — Stage 2 built into probe v2 (ZXing worker thread on frame copies).

- **2026-07-21 (a)** — Stages 3+4 built (mock approved). QRCameraProbe retired.
- **2026-07-21 (b)** — Fix: lambda parameter named `_` shadowed the task discard (CS0029).
  Build note: transient NDK linker OOM (`ld.lld` "out of memory") — retry/close apps/delete
  `Library\Bee`; unrelated to code.
- **2026-07-21 (c)** — **End-to-end PASSED on device.** Scan → passport → dismantle → report all
  working. Loop routine added (stage 5): post-report modal → scan new / main menu; "one scan per
  launch" superseded by per-cycle camera rebuild — multi-cycle validated.
- **2026-07-21 (d)** — Scan screen v2→v3: grabber bar removed, head-follow added (lazy lerp),
  background removed entirely (brackets + sweep + text only; found/error keep compact cards).
  Device-validated. **QR ENTRY FEATURE COMPLETE.**

*Last updated: 2026-07-21 · Status: COMPLETE — all five stages device-validated*
