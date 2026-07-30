# Unity + PICO 4 Setup Guide

Step-by-step setup for the AR-DPP prototype Unity project. This is the manual work you do in the Unity Editor — neither Cowork nor Claude Code can drive Unity's GUI.

## 1. Install Unity Hub and Editor

If you haven't already:

1. Download **Unity Hub** from https://unity.com/download
2. In Unity Hub, install **Unity 6.0 LTS** (latest patch, e.g. `6000.0.x f1`).
   - PICO Unity Integration SDK v3.4.0 (Apr 2026) supports Unity 2021.3.26 to Unity 6. Unity 6.0 LTS is the safest fit: modern, inside the supported range, and battle-tested with the SDK by release time.
   - **Avoid:** Unity 2022.3 LTS (older, near end-of-LTS), Unity 6.3 LTS (too new — released around the same time as the SDK, not yet hardened).
   - Modules to include: **Android Build Support** (with **OpenJDK** + **Android SDK & NDK Tools**) — required because PICO 4 builds run as Android.

## 2. Create the project

1. Unity Hub → **New project**
2. Template: **3D (URP)** — Universal Render Pipeline. Required by PICO XR.
3. Project name: `AR-DPP-VCU`
4. Location: anywhere outside OneDrive (recommended). Suggest: `C:\Dev\AR-DPP-VCU\`
5. Click **Create project**

## 3. Configure for Android / PICO 4

1. **File → Build Settings** → switch platform to **Android**
2. **Edit → Project Settings → Player → Android tab**:
   - **Other Settings → Identification → Package Name:** `com.thiagopegorer.ardppvcu`
   - **Other Settings → Configuration → Scripting Backend:** IL2CPP
   - **Other Settings → Configuration → Target Architectures:** check **ARM64** (uncheck ARMv7)
   - **Other Settings → Configuration → Minimum API Level:** API Level 29 (Android 10)
   - **Other Settings → Configuration → Target API Level:** Automatic (Highest installed)

## 4. Install PICO Unity Integration SDK (PUI)

1. Download from PICO's developer portal: https://developer-global.pico-interactive.com/sdk
   - Pick **PICO Unity Integration SDK** (not the Native one)
   - Latest version compatible with Unity 2022.3 LTS
2. In Unity: **Assets → Import Package → Custom Package** → select the downloaded `.unitypackage`
3. After import, follow the in-Editor wizard: it'll ask to enable XR Plug-in Management → enable **PICO** under **Android tab**
4. Verify: **Edit → Project Settings → XR Plug-in Management → Android** → **PICO** is checked

## 5. Install required packages

Open **Window → Package Manager** and install:

- **Newtonsoft Json** (for parsing the DPP JSON properly with nullable fields)
  - Click **+** → **Add package by name** → enter `com.unity.nuget.newtonsoft-json` → Add
- **TextMeshPro** (likely already installed; needed for clean AR text)
  - If not present: `+` → search "TextMeshPro" → install
  - When prompted, also import the **TMP Essentials**

## 6. Import DOTween (for explosion animation)

1. **Window → Asset Store** → search **DOTween** → open the page in browser
2. Get the **free** DOTween (HOTween v2) by Demigiant
3. Back in Unity: **Window → Package Manager** → **Packages: My Assets** → find DOTween → **Download** then **Import**
4. After import, DOTween shows a setup wizard — click **Setup DOTween...** and accept defaults

## 7. Add a QR scanning library

For Phase 1 colloquium, **ZXing.Net** is the simplest free option.

⚠️ **Do NOT clone the full ZXing.Net source repo** into your `Assets/` folder. That repo contains bindings for OpenCV, EmguCV, ASP.NET, etc. which Unity can't compile and will produce hundreds of errors. We only need the precompiled Unity DLL.

**Correct steps:**

1. Go to https://github.com/micjahn/ZXing.Net/releases/latest
2. Under **Assets** on the release page, download a file with `unity` in its name (e.g. `ZXing.Net.unity_0.x.y.z.zip`). NOT `Source code (zip)`, NOT any of the EmguCV variants.
3. Extract the ZIP locally
4. Drop only `zxing.unity.dll` (and its `.meta` if present) into `Assets/Plugins/` of your Unity project
5. Reopen Unity if it's running; the Console should be clean

If you accidentally imported the full source: close Unity, delete `Assets/Plugins/ZXing.Net-master/` and its `.meta` from File Explorer, then redo the steps above.

We'll use the PICO 4 passthrough camera with `WebCamTexture` to feed frames into ZXing.

> Alternative: **Vuforia** is more robust but adds licensing complexity. Stick with ZXing for the colloquium demo.

## 8. Drop in our DPP scripts

Copy all 5 files from this repo's `unity/` folder into your Unity project at `Assets/Scripts/DPP/`:

- `DPPClient.cs`
- `DPPModels.cs`
- `DPPDashboard.cs`
- `ExplosionController.cs`
- `ComponentMetadata.cs`

The scripts compile against the namespaces we set up. Check the Console for errors after copying.

## 9. Build the dashboard scene

This is the hands-on part — once you've done steps 1-8, ask Claude to walk you through assembling the scene (Canvas with tabs, hooking up `DPPClient` to fetch data, wiring up `ExplosionController` to your imported VCU CAD).

## Open decisions still to make

- **QR library final choice:** ZXing (recommended) vs Vuforia
- **JSON deserialization:** Newtonsoft.Json (recommended for nullable handling) vs Unity's built-in JsonUtility (limited)
- **AR anchor strategy:** world anchor on QR detection (recommended for stability) vs continuous tracking
