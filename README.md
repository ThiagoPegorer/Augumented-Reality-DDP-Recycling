# Master Thesis — AR-DPP for VCU

Digital Product Passport (DPP) in Augmented Reality for Disassembly and Recycling Analysis of a Vehicle Control Unit (VCU).

## Project structure

```
CODE/
├── backend/      FastAPI server — serves DPP JSON files
├── unity/        C# scripts — copied into the Unity project's Assets/Scripts/
├── schema/       Single source of truth: JSON Schema + sample VCU payload
├── docs/         Architecture notes, diagrams
└── README.md     This file
```

## Architecture summary

- **Backend:** FastAPI (Python). Serves DPP data as JSON. Storage = JSON files (one per VCU instance).
- **Frontend:** Unity UI Canvas, AR-native, world-anchored to the physical VCU on PICO 4.
- **Schema:** CIRPASS-aligned (EU DPP draft direction).
- **AR scan flow:** QR code → product_id → backend GET /dpp/{product_id} → DPP JSON → Unity dashboard + 3D exploded view.

## Quick start — backend

```powershell
cd backend
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
uvicorn main:app --reload
```

Then visit:
- http://localhost:8000/docs — auto-generated OpenAPI docs
- http://localhost:8000/dpp/vcu_001 — sample VCU passport

## Quick start — Unity

1. Open your Unity project (URP, PICO XR template).
2. Copy `unity/*.cs` into `Assets/Scripts/DPP/`.
3. Install DOTween from the Asset Store.
4. Configure `DPPClient.baseUrl` to point at your backend (e.g. `http://localhost:8000` for local dev, ngrok URL for headset testing).

## Thesis context

Phase 1 — Colloquium prototype (May 8, 2026): QR → DPP info + exploded view.
Phase 2 — Final thesis prototype (Aug 3, 2026): + GPT-4o chatbot, real PCB, user study.
