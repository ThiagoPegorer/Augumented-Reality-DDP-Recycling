"""
Pydantic models for the DPP schema — v0.3.

These mirror schema/dpp_schema.json. Keep them in sync.
The Unity side has parallel C# models in Assets/Scripts/DDP/DPPModels.cs
(a copy lives in unity/DPPModels.cs).

v0.3 (2026-06-10) — Bosch Motorsport VCU MS 50.4 data model:
  - identity gains type_number; new `specifications` block (datasheet facts).
  - components[] mirrors the 12-line LCA BOM (VCU_LCA_Model_v3.xlsx);
    `disassembly_step` maps each component to its removal step (1-5).
  - new `precious_metals[]` (Au/Ag/Pd/Ta/Ni sub-inventory from the BOM).
  - environmental: `scenarios[]` REPLACED by `lifecycle_stages[]` (S1-S4)
    + `recovery_potential` (net avoidable CO2 + per-material credits) +
    `usage_profile` (design service-life assumptions, transparency story).
    EoL scenario comparison stays in the thesis LCA model, not the DPP UI.
  - end_of_life gains contains_battery; new nullable `compliance` block.
"""
from typing import List, Optional
from pydantic import BaseModel, Field


class Identity(BaseModel):
    manufacturer: str
    model: str
    type_number: Optional[str] = None   # manufacturer order/type no.
    serial_number: str
    production_date: str  # ISO 8601 date string (YYYY-MM-DD)
    country_of_origin: str  # ISO 3166-1 alpha-2 (e.g. "DE")


class Specifications(BaseModel):
    """Datasheet facts shown in the Identity category."""
    size_mm: Optional[str] = None             # e.g. "166 x 121 x 41"
    weight_g: Optional[float] = None
    protection_class: Optional[str] = None    # e.g. "IP67"
    supply_voltage: Optional[str] = None      # e.g. "5-18 V"
    operating_temp_c: Optional[str] = None    # e.g. "-20 to 80"
    connectors: Optional[str] = None          # e.g. "3 motorsport (198 pins) + USB"


class Component(BaseModel):
    id: str
    name: str
    material: str
    weight_g: float
    recycling_code: str
    disassembly_step: int       # which guided step (1-5) handles this part
    hazardous: bool = False
    high_value: bool = False     # v0.3.1 — worth dedicated recovery (Recover card, step value tags)
    basis: Optional[str] = None  # "datasheet" | "estimate" — BOM transparency


class PreciousMetal(BaseModel):
    metal: str          # e.g. "Gold (Au)"
    location: str       # where in the device
    mass_mg: float


class LifecycleStage(BaseModel):
    id: str             # "S1".."S4"
    name: str           # e.g. "Raw material extraction"
    co2_kg: float
    note: Optional[str] = None   # e.g. "modelled use profile, not telemetry"


class RecoveryCredit(BaseModel):
    material: str
    avoided_kg: float


class RecoveryPotential(BaseModel):
    """Forward-looking: what proper recycling can avoid (Sc2 vs Sc1 net swing)."""
    total_avoidable_kg: float
    note: Optional[str] = None   # e.g. "net of recycling process emissions"
    credits: List[RecoveryCredit] = Field(default_factory=list)


class UsageProfile(BaseModel):
    """Design service-life assumptions (transparency; telemetry in a real DPP)."""
    service_life_years: Optional[int] = None
    lifetime_distance_km: Optional[int] = None
    operating_hours: Optional[int] = None
    lifetime_energy_kwh: Optional[float] = None
    note: Optional[str] = None


class Environmental(BaseModel):
    co2_footprint_kg: Optional[float] = None        # headline: lifecycle total
    method: Optional[str] = None                    # e.g. "ISO 14040 · GWP100 (IPCC AR6)"
    recycled_content_pct: Optional[float] = None
    lifecycle_stages: List[LifecycleStage] = Field(default_factory=list)
    recovery_potential: Optional[RecoveryPotential] = None
    usage_profile: Optional[UsageProfile] = None


class Compliance(BaseModel):
    """Nullable until verified — UI shows an em-dash for None."""
    ce: Optional[bool] = None
    rohs: Optional[bool] = None
    reach: Optional[bool] = None
    weee_category: Optional[str] = None


class StepAction(BaseModel):
    """One action card inside a guided step (v0.4)."""
    title: str
    subtitle: Optional[str] = None
    icon: str = "cross"          # keyword mapped to a sprite in Unity:
                                 # cross|up|pins|usb|lever|board|magnify|chip|recycle|label
    value: bool = False          # true → gold high-value accent


class Step(BaseModel):
    """One guided disassembly step (v0.4). Content per DPP_UI_Specs 04–08."""
    id: int                      # 1-based, matches Component.disassembly_step
    title: str
    tool: Optional[str] = None
    component_ids: List[str] = Field(default_factory=list)
    actions: List[StepAction] = Field(default_factory=list)


class Disassembly(BaseModel):
    total_steps: int
    estimated_time_min: int
    tools: List[str] = Field(default_factory=list)  # v0.3.1 — e.g. ["Torx driver", "spudger"]; basis: estimate
    steps: List[Step] = Field(default_factory=list)  # v0.4 — guided step content


class EndOfLife(BaseModel):
    recycling_route: str
    contains_battery: bool = False
    hazardous_warnings: List[str] = Field(default_factory=list)


class DPP(BaseModel):
    """Top-level Digital Product Passport for a VCU."""
    product_id: str
    identity: Identity
    specifications: Optional[Specifications] = None
    components: List[Component]
    precious_metals: List[PreciousMetal] = Field(default_factory=list)
    environmental: Environmental
    compliance: Optional[Compliance] = None
    disassembly: Disassembly
    end_of_life: EndOfLife
