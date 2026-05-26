"""
DPP API — FastAPI backend for the AR-DPP thesis prototype.

Serves Digital Product Passport JSON files for VCUs.
Storage: JSON files in ./data/, one per product_id.

Each response is validated against the Pydantic DPP model (see models.py),
which mirrors schema/dpp_schema.json. If a JSON file is malformed or missing
required fields, the endpoint returns 500 with a clear validation error.
"""
from pathlib import Path
import json
import logging

from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import ValidationError

from models import DPP

logger = logging.getLogger("dpp")
logging.basicConfig(level=logging.INFO, format="%(asctime)s [%(levelname)s] %(message)s")

app = FastAPI(
    title="DPP API",
    description="Digital Product Passport API for VCU AR prototype",
    version="0.2.0",
)

# CORS — open during prototype dev so Unity (any host) can call the API.
# Tighten before any public deployment.
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["GET"],
    allow_headers=["*"],
)

DATA_DIR = Path(__file__).parent / "data"


@app.get("/")
def root():
    """Health check."""
    return {"status": "ok", "service": "DPP API", "version": app.version}


@app.get("/dpp", summary="List known product_ids")
def list_dpps():
    """Return the list of product_ids that have a JSON file in data/."""
    if not DATA_DIR.exists():
        return {"product_ids": []}
    return {
        "product_ids": sorted(p.stem for p in DATA_DIR.glob("*.json"))
    }


@app.get(
    "/dpp/{product_id}",
    response_model=DPP,
    summary="Get a DPP by product_id",
)
def get_dpp(product_id: str) -> DPP:
    """
    Load the DPP JSON for `product_id` and return it after validating
    against the Pydantic schema.

    - **404** if the JSON file does not exist.
    - **500** if the JSON file exists but is malformed or schema-invalid.
    """
    file_path = DATA_DIR / f"{product_id}.json"
    if not file_path.exists():
        raise HTTPException(
            status_code=404,
            detail=f"DPP not found for product_id: {product_id}",
        )

    try:
        with open(file_path, encoding="utf-8") as f:
            raw = json.load(f)
    except json.JSONDecodeError as exc:
        logger.error("Malformed JSON in %s: %s", file_path.name, exc)
        raise HTTPException(
            status_code=500,
            detail=f"Malformed JSON in {file_path.name}: {exc}",
        ) from exc

    try:
        return DPP(**raw)
    except ValidationError as exc:
        logger.error("Schema validation failed for %s: %s", file_path.name, exc)
        raise HTTPException(
            status_code=500,
            detail=f"DPP {product_id} fails schema validation: {exc.errors()}",
        ) from exc
