"""
Pydantic models for the DPP schema.

These mirror schema/dpp_schema.json. Keep them in sync.
The Unity side has parallel C# models in unity/DPPModels.cs.
"""
from typing import List, Optional
from pydantic import BaseModel, Field


class Identity(BaseModel):
    manufacturer: str
    model: str
    serial_number: str
    production_date: str  # ISO 8601 date string (YYYY-MM-DD)
    country_of_origin: str  # ISO 3166-1 alpha-2 (e.g. "DE")


class Component(BaseModel):
    id: str
    name: str
    material: str
    weight_g: float
    recycling_code: str
    disassembly_order: int
    hazardous: bool = False


class Scenario(BaseModel):
    id: int
    name: str
    value_kg: Optional[float] = None


class Environmental(BaseModel):
    co2_footprint_kg: Optional[float] = None
    scenarios: List[Scenario] = Field(default_factory=list)


class Disassembly(BaseModel):
    total_steps: int
    estimated_time_min: int


class EndOfLife(BaseModel):
    recycling_route: str
    hazardous_warnings: List[str] = Field(default_factory=list)


class DPP(BaseModel):
    """Top-level Digital Product Passport for a VCU."""
    product_id: str
    identity: Identity
    components: List[Component]
    environmental: Environmental
    disassembly: Disassembly
    end_of_life: EndOfLife
