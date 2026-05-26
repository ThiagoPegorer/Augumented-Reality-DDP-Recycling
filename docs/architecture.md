# Architecture — AR-DPP for VCU

## Overview

```
+------------------+       QR scan        +-----------------+
|  Physical VCU    |  ------------------> |  PICO 4 (Unity) |
|  (3D printed)    |   product_id         |                 |
+------------------+                      |  - QR tracking  |
                                          |  - DPPClient    |
                                          |  - Dashboard    |
                                          |  - Explosion    |
                                          +--------+--------+
                                                   |
                                                   | HTTP GET /dpp/{product_id}
                                                   v
                                          +-----------------+
                                          |   FastAPI       |
                                          |   backend       |
                                          |                 |
                                          |   data/*.json   |
                                          +-----------------+
```

## Layers

### Data layer — JSON files

- One file per VCU instance under `backend/data/{product_id}.json`.
- Conforms to `schema/dpp_schema.json` (CIRPASS-aligned).
- No database for prototype scope.

### Backend — FastAPI

- `GET /` — health check
- `GET /dpp` — list known product_ids
- `GET /dpp/{product_id}` — return DPP JSON
- Auto-generated OpenAPI docs at `/docs` (useful for thesis appendix).

### Frontend — Unity (PICO 4)

- `DPPClient.cs` — UnityWebRequest call to backend.
- `DPPModels.cs` — C# mirror of the JSON schema.
- `DPPDashboard.cs` — tab controller (Info + Explosion View for colloquium).
- `ExplosionController.cs` — DOTween animation of child components.
- `ComponentMetadata.cs` — DPP metadata attached to each 3D component.

## Phase 1 → Phase 2 expansion

| Layer    | Phase 1 (colloquium)             | Phase 2 (thesis)                                   |
|----------|----------------------------------|----------------------------------------------------|
| Data     | Identity + components + minimal env | + full BOM, real CO₂ data, hazardous flags        |
| Backend  | GET endpoints only               | + GPT-4o proxy endpoint for chatbot               |
| Frontend | Info + Explosion View tabs       | + Materials, Environmental, End-of-life, AI Assist |
| AR       | QR → world-anchored dashboard    | + component-level labels, gaze interaction        |

## Locked tech decisions (Apr 29)

| Decision | Choice | Reason |
|---|---|---|
| Unity version | **6.0 LTS** (6000.0.x) | Inside PICO SDK v3.4.0 supported range; modern but battle-tested by April 2026. |
| QR library | **ZXing.Net** (Unity DLL) | Free, MIT license, lighter than Vuforia, sufficient for colloquium scope. |
| JSON deserializer | **Newtonsoft.Json** (`com.unity.nuget.newtonsoft-json`) | Handles nullable fields (e.g. `co2_footprint_kg: null`) which `JsonUtility` cannot. |
| AR anchor strategy | **World anchor on QR detection** (one-shot) | VCU is stationary during disassembly. QR will be occluded often by recycler's hands → continuous tracking would flicker. World anchor gives stable AR content the user can walk around freely. |
| QR payload format | `dpp:<product_id>` | Custom URI scheme so Unity can validate "this is a DPP QR" and ignore random text. Backend-agnostic — QR stays valid if backend host changes. |
| Animation library | **DOTween (free)** | Industry-standard, simple API for the explosion animation. |

## Future / Phase 2 considerations

- **Hybrid anchor strategy:** initial world-anchor + periodic refresh every 2-3 seconds while QR is visible, to correct for SLAM drift. Improves robustness for thesis defense demo.
- **Image tracking via PICO XR Image Anchor API** as alternative to QR — lets you anchor on the VCU itself rather than a sticker.
- **GPT-4o chatbot** integration for AI-powered disassembly guidance.
