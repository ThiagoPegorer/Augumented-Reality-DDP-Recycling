# DPP Backend

FastAPI server that serves Digital Product Passport JSON files, plus a CLI to generate QR codes that Unity scans on PICO 4.

## Run locally

```powershell
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
uvicorn main:app --reload
```

## Endpoints

| Method | Path                  | Description                                      |
|--------|-----------------------|--------------------------------------------------|
| GET    | `/`                   | Health check                                     |
| GET    | `/dpp`                | List all known product_ids                       |
| GET    | `/dpp/{product_id}`   | Get DPP JSON (validated against the Pydantic schema) |
| GET    | `/docs`               | Interactive OpenAPI docs (Swagger UI)            |

The `/dpp/{product_id}` endpoint validates the response against the `DPP` Pydantic model (`models.py`) before returning it. If the JSON file is malformed or missing required fields, the endpoint returns **500** with a clear error.

## QR code generation

Generate a printable QR code for a VCU instance. The QR encodes `dpp:<product_id>` (custom URI scheme). Unity's QR reader validates the `dpp:` prefix, extracts the product_id, then calls `/dpp/{product_id}`.

```powershell
python qr_generator.py vcu_001
```

Output: `qr/vcu_001.png` (~360×360 px by default).

For a larger printable version:

```powershell
python qr_generator.py vcu_001 --size 16
```

Print the PNG and stick it on the physical VCU housing for the AR demo.

## Adding a new VCU

Drop a new JSON file into `data/` named `{product_id}.json`. It must conform to `../schema/dpp_schema.json` (and equivalently to the `DPP` Pydantic model in `models.py`).

Then generate its QR code:

```powershell
python qr_generator.py {product_id}
```

## Exposing to the PICO 4 headset

The headset cannot reach `localhost` on your laptop. Two options:

1. **Same Wi-Fi (LAN):** start uvicorn with `--host 0.0.0.0` and use your laptop's LAN IP from Unity (e.g. `http://192.168.x.x:8000`).
2. **ngrok** (works from anywhere): `ngrok http 8000` and paste the public URL into `DPPClient.baseUrl` in the Unity Inspector.
