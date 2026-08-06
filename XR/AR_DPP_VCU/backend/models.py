"""
Pydantic models for the DPP schema — v0.6.

The Unity side has parallel C# models in Assets/Scripts/DDP/DPPModels.cs.
`schema/dpp_schema.json` is now GENERATED from this file — run
`python export_schema.py` after any change here (see that script).

v0.6 (2026-07-30) — CIRPASS D2.2 Table 6 completeness pass.
    Every one of the 22 attributes in Table 6 (pp. 41-42) now has a slot, so the
    passport can state "not provided" explicitly instead of omitting a field.
    Mapping and coverage live in DPP_UI_Specs/13b_information_model.md.

    NEW blocks
      - documents[]            -> Table 6 #1 #2 #12 #16 #17 #22 (the paper layer)
      - substances_of_concern[]-> #5 #6  name + LOCATION (component_id) + concentration
      - collection_scheme      -> #4     disposal / return / take-back
      - service                -> #12 #15 spare parts, updates, resale & EoL options
      - usage_history          -> #19    measured use data (vs designed usage_profile)
      - repair_history         -> #20
      - indicators             -> #21    circularity indices (LCA stays in environmental)
      - certifications[]       -> #22
      - dpp_meta               -> schema version + provenance of the whole record

    HONEST-LABELLING RULE (DPP_UI_Specs/13b §4). Every added block carries a
    `basis` and/or `status` with a CLOSED vocabulary:
      basis  : "declared" | "datasheet" | "measured" | "assumed" | "modelled" | "not_provided"
      status : "available" | "not_provided" | "not_applicable"
    "not_applicable" is a real answer, not an evasion: Table 6 #1 and #2 come from
    the Energy Labelling Regulation (EU) 2017/1369, which applies to labelled
    product groups only. A motorsport control unit is not one, so the correct
    passport value is not_applicable with a reason — NOT an empty mandatory field.

    Nothing from v0.5 was removed or renamed; every new field is optional or
    defaults to an empty collection, so existing clients keep working.

v0.7 (2026-07-30) - openLCA v4 / EF 3.1 swap.
    environmental now carries `impact_recovery[]`: per impact category, the baseline and
    each end-of-life scenario's net / saving / reduction. Populated for the three
    categories EF 3.1 screening selects (minerals & metals 72.5 %, climate 6.7 %,
    freshwater eutrophication 6.6 % - 85.7 % cumulative).
    `lifecycle_stages` was CLEARED: no per-stage openLCA export exists, and the old
    assumed-BOM split summed to 63.9 kg against the correct 73.43 kg headline.
    `recovery_potential.credits` cleared for the same reason - per-material credit
    attribution is not exported by the openLCA runs.

v0.5 - recovery report (POST /dpp/{id}/report) with per-step splits.
v0.3 - Bosch Motorsport VCU MS 50.4 data model: specifications, components[]
       mirroring the LCA BOM, precious_metals[], lifecycle_stages[] +
       recovery_potential + usage_profile, compliance.
"""
from typing import List, Optional
from pydantic import BaseModel, Field


# ---------------------------------------------------------------------------
# Closed vocabularies (documented here, enforced by convention + the UI)
# ---------------------------------------------------------------------------
# "simulated" (v0.9) marks data that was INVENTED for the study demonstrator. It is
# not a weaker measurement - it is not a measurement at all - so the UI must never
# render it as a firm source. See DppBasis.IsFirmSource in DPPModels.cs.
BASIS_VALUES = ("declared", "datasheet", "measured", "assumed", "modelled",
                "simulated", "not_provided")
STATUS_VALUES = ("available", "not_provided", "not_applicable")


class Identity(BaseModel):
    manufacturer: str
    model: str
    type_number: Optional[str] = None   # manufacturer order/type no.
    serial_number: str
    production_date: str  # ISO 8601 date string (YYYY-MM-DD)
    country_of_origin: str  # ISO 3166-1 alpha-2 (e.g. "DE")
    # v0.6 - D2.1 product identification / economic operator
    product_category: Optional[str] = None      # e.g. "EEE - electronic control unit"
    economic_operator: Optional[str] = None     # who places it on the EU market
    brand: Optional[str] = None


class Specifications(BaseModel):
    """Datasheet facts shown in the Identity category."""
    size_mm: Optional[str] = None             # e.g. "166 x 121 x 41"
    # v0.14 - WHICH BODY this size describes. The project holds two: the Bosch
    # MS 50.4 product (166 x 121 x 41, 660 g) and the NX demonstrator that the AR
    # model is built from (200 x 150 x 60). RB2.0 put the demonstrator's measured
    # size in this block next to the product's mass. The passport declares the
    # PRODUCT; the demonstrator is the mock (spec 04c s.1.1).
    size_basis: Optional[str] = None
    weight_g: Optional[float] = None
    protection_class: Optional[str] = None    # e.g. "IP67"
    supply_voltage: Optional[str] = None      # e.g. "5-18 V"
    operating_temp_c: Optional[str] = None    # e.g. "-20 to 80"
    connectors: Optional[str] = None          # e.g. "3 motorsport (198 pins) + USB"
    # v0.6 - Table 6 #1 "energy consumption & performance"
    power_consumption_w: Optional[float] = None
    performance_note: Optional[str] = None
    energy_label_applicable: Optional[bool] = None   # False for a VCU - see module docstring
    energy_label_note: Optional[str] = None


class DocumentRef(BaseModel):
    """v0.6 - one document the passport must reference (Table 6 #1 #2 #12 #16 #17 #22).

    These are PDFs in the real world. The passport's job on a headset is to say
    WHETHER they exist and where, not to render them. `status` makes an absent
    mandatory document visible instead of silently missing.
    """
    id: str                                  # "energy_sheet" | "technical_doc" | "user_manual" | ...
    title: str
    kind: str                                # "sheet" | "documentation" | "manual" | "instructions" | "declaration" | "certificate"
    cirpass_ref: Optional[str] = None        # e.g. "T6 #16" - traceability to Table 6
    mandatory: bool = False                  # M in Table 6
    status: str = "not_provided"             # STATUS_VALUES
    url: Optional[str] = None
    note: Optional[str] = None               # why not_applicable / where to get it


COMPONENT_GROUPS = ("part", "board_material")


class MaterialShare(BaseModel):
    """One material line inside a component. Weights must sum to weight_g."""
    material: str
    weight_g: float


class Component(BaseModel):
    """One inventory row of the device.

    v0.14 splits the flat 11-row list into 15 rows carrying a `group`:

      "part"            a body a dismantler can physically pick up. Has an NX
                        drawing (`drawing_id`) and is a hit target in the UI.
      "board_material"  a material distributed over the board - solder, coating,
                        passives, TIM. No drawing exists and none should: these
                        are not discrete bodies. Rendered inert (spec 04c s.4.3).

    `bom_rows` and `represents` carry provenance, because two rows of this list
    are REGROUPINGS of VCU_BOM_v4.xlsx rather than 1:1 copies (spec 04c s.3.3):

      housing_upper / housing_bottom   BOM row 1 split by shell area
      ic_1 .. ic_4                     BOM rows 5-11 allocated to the four CAD
                                       blocks by footprint

    The passport must never claim `ic_1` IS a single processor - `represents`
    is what the UI shows so the regrouping stays visible to the user.
    """
    id: str
    name: str
    group: str = "part"          # COMPONENT_GROUPS
    material: str
    weight_g: float
    recycling_code: str
    disassembly_step: int       # which guided step (1-5) handles this part
    hazardous: bool = False
    high_value: bool = False     # worth dedicated recovery
    basis: Optional[str] = None  # BASIS_VALUES - BOM transparency
    drawing_id: Optional[str] = None   # Assets/Resources/dwg/<id>_dwg.png + _iso.png
    bom_rows: List[int] = Field(default_factory=list)   # VCU_BOM_v4.xlsx Table 1 row numbers
    represents: Optional[str] = None   # the BOM entries behind a regrouped row
    material_breakdown: List[MaterialShare] = Field(default_factory=list)
    material_breakdown_basis: Optional[str] = None


class SubstanceOfConcern(BaseModel):
    """v0.6 - Table 6 #5 and #6, the two hardest mandatory attributes.

    #6 requires the substance NAME, its LOCATION WITHIN THE PRODUCT and its
    CONCENTRATION at product / main-component / spare-part level. `component_id`
    is the location and links straight into components[], which is what lets the
    AR client highlight the physical part that carries the substance — the
    capability a paper WEEE sheet cannot offer (CIRPASS Table 8 p.56, UC4).

    An EMPTY list means "none declared", which is different from "none present".
    Say which one in end_of_life.substances_basis.
    """
    name: str                                # e.g. "Lead (Pb)"
    cas_number: Optional[str] = None
    regulation: Optional[str] = None         # "REACH SVHC" | "RoHS Annex II" | "POP" | "CLP"
    component_id: Optional[str] = None       # LOCATION -> components[].id
    location_note: Optional[str] = None      # free text when no single component fits
    concentration_pct_w_w: Optional[float] = None
    threshold_pct_w_w: Optional[float] = None    # e.g. 0.1 for REACH SVHC
    above_threshold: Optional[bool] = None
    symbol: Optional[str] = None             # v0.12 - short form for the UI ("Pb", "PbO")
    basis: str = "not_provided"              # BASIS_VALUES


class PreciousMetal(BaseModel):
    metal: str          # e.g. "Gold (Au)"
    location: str       # where in the device
    mass_mg: float


class LifecycleStage(BaseModel):
    id: str             # "S1".."S4"
    name: str           # e.g. "Raw material extraction"
    co2_kg: float
    note: Optional[str] = None


class RecoveryCredit(BaseModel):
    material: str
    avoided_kg: float


class RecoveryPotential(BaseModel):
    """Forward-looking: what proper recycling can avoid, as a single climate figure."""
    total_avoidable_kg: float
    note: Optional[str] = None
    credits: List[RecoveryCredit] = Field(default_factory=list)
    scenario: Optional[str] = None            # v0.7 - which EoL scenario this figure is
    basis: Optional[str] = None               # v0.7 - BASIS_VALUES


class ImpactRecoveryScenario(BaseModel):
    """v0.7 - one end-of-life scenario's outcome for one impact category."""
    id: str                                   # "Sc2" | "Sc3" | "Sc4"
    label: str                                # human label, e.g. "Guided dismantling"
    net: float                                # scenario total, credits applied
    saving: float                             # baseline - net
    reduction_pct: float                       # relative to the baseline scenario
    note: Optional[str] = None                # e.g. declares an exploratory assumption


class ImpactRecovery(BaseModel):
    """v0.7 - Table 6 #21, the quantitative half: how much of one impact category
    the end-of-life route can avoid.

    WHY THIS EXISTS: `RecoveryPotential.credits` is per MATERIAL and single-category
    (climate). The AR client needs per IMPACT CATEGORY across scenarios, because the
    thesis result is that the scenarios answer different environmental questions -
    reuse pays in carbon, dismantling-for-smelting pays in minerals. A single
    "recovery rate" without a named scenario and category is not interpretable.

    Populate from LCA_Analysis/Outputs/3_impact_assessment/impact_EF31.csv
    (`sc1`, `sc*_net`, `sc*_saving`) - never from mc_net.csv, whose independent
    sampling understates the scenario gaps."""
    category: str                             # EF 3.1 category name, verbatim
    unit: str
    screening_share_pct: Optional[float] = None   # from impact_screening.csv
    baseline: float
    baseline_scenario: str = "Sc1"
    scenarios: List[ImpactRecoveryScenario] = Field(default_factory=list)
    method: Optional[str] = None              # e.g. "EF 3.1"
    basis: str = "not_provided"               # BASIS_VALUES


class AnnualDistance(BaseModel):
    """v0.11 - one year of the modelled distance series (Usage Profile list)."""
    year: str
    distance_km: int
    note: Optional[str] = None               # "from Apr" / "to Mar" on partial years


class UsageProfile(BaseModel):
    """The LCA use-phase model (S4) as shown on the Usage Profile page.
    MODELLED assumptions, not measurements - measured use data would belong in
    usage_history. v0.11 carries the S4 parameters + the per-year distance
    series; car_energy_kwh_estimate is OUTSIDE the S4 boundary (own draw only)
    and is a plain average-BEV estimate, 17.5 kWh/100 km."""
    service_life_years: Optional[int] = None
    lifetime_distance_km: Optional[int] = None
    operating_hours: Optional[int] = None    # 5,625 = 225,000 km / 40 km/h
    lifetime_energy_kwh: Optional[float] = None  # OWN draw: 9 W x 5,625 h / 0.765 = 66.2
    annual_distances: List[AnnualDistance] = Field(default_factory=list)  # v0.11
    service_period: Optional[str] = None     # "Apr 2011 - Mar 2026"
    avg_speed_kmh: Optional[float] = None    # MiD 2017
    own_power_w: Optional[float] = None      # MS 5.0 family proxy
    charging_efficiency: Optional[float] = None
    car_energy_kwh_estimate: Optional[float] = None  # outside the LCA boundary
    daily_use: Optional[str] = None          # "~30 km · ~45 min" (MiD 2017)
    basis: str = "not_provided"              # BASIS_VALUES
    note: Optional[str] = None


class Environmental(BaseModel):
    co2_footprint_kg: Optional[float] = None        # headline: lifecycle total
    method: Optional[str] = None
    recycled_content_pct: Optional[float] = None    # Table 6 #10
    recycled_content_basis: Optional[str] = None    # v0.6 - BASIS_VALUES
    lifecycle_stages: List[LifecycleStage] = Field(default_factory=list)
    lifecycle_stages_basis: Optional[str] = None     # v0.7 - BASIS_VALUES
    lifecycle_stages_note: Optional[str] = None      # v0.7 - why it is empty, if it is
    recovery_potential: Optional[RecoveryPotential] = None
    impact_recovery: List[ImpactRecovery] = Field(default_factory=list)   # v0.7 - per category
    usage_profile: Optional[UsageProfile] = None
    lca_basis: Optional[str] = None                 # v0.6 - "modelled" | "assumed"
    lca_source: Optional[str] = None                # v0.6 - provenance of the figures on screen


class CircularityIndicator(BaseModel):
    """v0.6 - Table 6 #21. One index, with its scale and method stated.
    A score without a scale and a method is not defensible; all three or none."""
    id: str                                  # "repairability" | "recyclability" | "reusability"
    label: str
    score: Optional[float] = None
    scale_max: Optional[float] = None        # e.g. 10 or 100
    method: Optional[str] = None             # e.g. "EN 45554"
    basis: str = "not_provided"              # BASIS_VALUES


class Indicators(BaseModel):
    """v0.6 - Table 6 #21. The LCA itself stays in `environmental`; this block is
    the index layer (repairability / recyclability / reusability)."""
    circularity: List[CircularityIndicator] = Field(default_factory=list)
    recyclability_pct_by_mass: Optional[float] = None
    recyclability_basis: Optional[str] = None
    social_impact_note: Optional[str] = None
    pef_note: Optional[str] = None


class DeclarationNote(BaseModel):
    """v0.12 - one section of the DoC's 'Further explanations' / disposal text,
    rendered in the Compliance & Safety scroll card."""
    title: str
    body: str


class Compliance(BaseModel):
    """Nullable until verified - UI shows an em-dash for None.

    v0.12, sourced from the Bosch EC/EU Declaration of Conformity (Operation
    Manual VCU MS 50.4P pp. 132-134, dated 09 Oct 2020): ONLY 2014/30/EU (EMC)
    is declared; RoHS is out of scope for means of transport (rohs_applicable
    False, rohs stays None - 'not applicable' is not 'non-conformant')."""
    ce: Optional[bool] = None
    ce_scope: Optional[str] = None           # v0.12 - "2014/30/EU (EMC)"
    tested_to: Optional[str] = None          # v0.12 - "ECE R10 · rev.6 : 2019"
    declaration_date: Optional[str] = None   # v0.12 - ISO yyyy-mm-dd
    rohs: Optional[bool] = None
    rohs_applicable: Optional[bool] = None   # v0.12 - False: out of 2011/65/EU scope
    reach: Optional[bool] = None             # True = Art. 33 information duty fulfilled
    weee_category: Optional[str] = None
    declaration_notes: List[DeclarationNote] = Field(default_factory=list)  # v0.12
    # v0.6 - state HOW each flag is known, and point at the DoC document
    basis: Optional[str] = None                     # BASIS_VALUES
    declaration_of_conformity_doc_id: Optional[str] = None   # -> documents[].id


class Certification(BaseModel):
    """v0.6 - Table 6 #22 responsibility / supply-chain certifications."""
    name: str                                # e.g. "IATF 16949"
    scope: Optional[str] = None
    issuer: Optional[str] = None
    valid_until: Optional[str] = None        # ISO date
    doc_id: Optional[str] = None             # -> documents[].id
    status: str = "not_provided"             # STATUS_VALUES


class SparePart(BaseModel):
    """v0.6 - Table 6 #12 spare parts."""
    id: str
    name: str
    order_ref: Optional[str] = None
    availability_years: Optional[int] = None
    component_id: Optional[str] = None       # -> components[].id
    status: str = "not_provided"             # STATUS_VALUES


class SoftwareUpdate(BaseModel):
    """v0.9 - one entry in the software update log (Table 6 #12, updates)."""
    date: Optional[str] = None               # ISO yyyy-mm-dd
    version: Optional[str] = None
    channel: str = "automatic"               # automatic | manual
    note: Optional[str] = None


class Service(BaseModel):
    """v0.6 - Table 6 #12 (use / repair / maintenance / updates) and
    #15 (resale, end-of-life options, waste-handling service availability)."""
    spare_parts: List[SparePart] = Field(default_factory=list)
    maintenance_interval: Optional[str] = None
    software_update_policy: Optional[str] = None
    repair_doc_id: Optional[str] = None      # -> documents[].id
    resale_options: List[str] = Field(default_factory=list)
    eol_options: List[str] = Field(default_factory=list)
    waste_handling_services: List[str] = Field(default_factory=list)
    software_updates: List[SoftwareUpdate] = Field(default_factory=list)   # v0.9
    software_update_basis: str = "not_provided"                            # v0.9
    basis: str = "not_provided"              # BASIS_VALUES


class CollectionScheme(BaseModel):
    """v0.6 - Table 6 #4 disposal, return and collection scheme information (M)."""
    scheme_name: Optional[str] = None
    take_back: Optional[str] = None          # e.g. "producer take-back"
    scheme_operator: Optional[str] = None    # NOT "operator" - reserved word in C#
    contact: Optional[str] = None
    url: Optional[str] = None
    instructions: List[str] = Field(default_factory=list)
    basis: str = "not_provided"              # BASIS_VALUES


class UsageHistory(BaseModel):
    """v0.6 - Table 6 #19. MEASURED use data. In a real DPP this comes from
    telemetry or the service record; in this prototype it is not_provided, and
    the passport says so rather than reusing the designed usage_profile."""
    purchase_date: Optional[str] = None
    in_service_date: Optional[str] = None
    use_cycles: Optional[int] = None
    operating_hours: Optional[int] = None
    distance_km: Optional[int] = None
    basis: str = "not_provided"              # BASIS_VALUES


class RepairEvent(BaseModel):
    date: Optional[str] = None
    description: Optional[str] = None
    scope: str = "unit"                      # v0.13 - "unit" (the VCU) or "vehicle"
    category: Optional[str] = None           # maintenance | repair | inspection | fault
    system: Optional[str] = None             # affected system, e.g. "12 V electrical"
    odometer_km: Optional[int] = None        # vehicle reading at the event
    exchanged_component_ids: List[str] = Field(default_factory=list)
    cost_eur: Optional[float] = None
    image_url: Optional[str] = None


class RepairHistory(BaseModel):
    """v0.6 - Table 6 #20 repair data (date, exchanged parts, costs, images)."""
    events: List[RepairEvent] = Field(default_factory=list)
    basis: str = "not_provided"              # BASIS_VALUES


class StepAction(BaseModel):
    """One action card inside a guided step."""
    title: str
    subtitle: Optional[str] = None
    icon: str = "cross"          # cross|up|pins|usb|lever|board|magnify|chip|recycle|label
    value: bool = False          # true -> gold high-value accent


class Step(BaseModel):
    """One guided disassembly step. Content per DPP_UI_Specs 04-08."""
    id: int                      # 1-based, matches Component.disassembly_step
    title: str
    tool: Optional[str] = None
    component_ids: List[str] = Field(default_factory=list)
    actions: List[StepAction] = Field(default_factory=list)


class Disassembly(BaseModel):
    total_steps: int
    estimated_time_min: int
    tools: List[str] = Field(default_factory=list)
    parts: List[str] = Field(default_factory=list)   # physical part groups for the intro list
    steps: List[Step] = Field(default_factory=list)
    # v0.6 - Table 6 #16 #17: safety text that belongs WITH the procedure
    safety_warnings: List[str] = Field(default_factory=list)
    safe_use_doc_id: Optional[str] = None            # -> documents[].id


class EndOfLife(BaseModel):
    recycling_route: str
    contains_battery: bool = False
    hazardous_warnings: List[str] = Field(default_factory=list)
    # v0.6
    substances_basis: str = "not_provided"           # BASIS_VALUES - governs substances_of_concern
    recycling_instructions: List[str] = Field(default_factory=list)   # Table 6 #11
    collection_scheme: Optional[CollectionScheme] = None              # Table 6 #4


class PhysicalPart(BaseModel):
    """v0.8 - one part of the PHYSICAL DEMONSTRATOR the participant handles.

    This is NOT product data. The 3D-printed study unit stands in for a Bosch
    MS 50.4; its parts are printed blocks distinguished by colour. Keeping them
    in their own block stops replica facts leaking into `specifications`, where
    a reader would take them for the product's declared values."""
    id: str
    name: str
    count: int = 1
    colour: Optional[str] = None             # plain-language colour of the printed part
    swatch_hex: Optional[str] = None         # UI swatch, e.g. "#4da3ff"
    photo_id: Optional[str] = None           # -> Assets/Textures/Parts/<photo_id>.png
    note: Optional[str] = None


class PhysicalUnit(BaseModel):
    """v0.8 - facts about the demonstrator itself, kept apart from product data."""
    is_replica: bool = True
    replica_of: Optional[str] = None
    size_mm: Optional[str] = None
    parts: List[PhysicalPart] = Field(default_factory=list)
    basis: str = "measured"                  # BASIS_VALUES
    note: Optional[str] = None


class DppMeta(BaseModel):
    """v0.6 - provenance of the record itself, so a reader can tell how much of
    this passport is real. `attributes_not_provided` is filled by hand for now;
    it is what the UI reads to render the honest 'not provided' states."""
    schema_version: str = "0.6"
    reference_framework: Optional[str] = None    # "CIRPASS D2.2 Table 6 (pp.41-42)"
    last_updated: Optional[str] = None           # ISO date
    completeness_note: Optional[str] = None


class RecoveryReport(BaseModel):
    """v0.5 - recovery report posted by the AR client after a completed
    disassembly (spec 09 §7). Closes the UC4 data-feedback loop."""
    product_id: str
    timestamp: str               # ISO 8601 UTC, client-supplied
    elapsed_s: int               # start -> finish stopwatch, whole seconds
    steps_completed: int
    step_times_s: List[int] = Field(default_factory=list)
    recovered_component_ids: List[str] = Field(default_factory=list)
    co2_avoided_kg: Optional[float] = None



class TempBand(BaseModel):
    band: Optional[str] = None
    hours: Optional[int] = None


class DeltaTBand(BaseModel):
    """v0.13 - thermal cycles binned by SWING AMPLITUDE. Cycle count alone is not a
    wear measure: Coffin-Manson makes damage scale with dT^n, so a short errand and a
    motorway run are not the same event."""
    band: Optional[str] = None
    delta_t_mid_c: Optional[int] = None
    cycles: Optional[int] = None
    cycles_to_failure: Optional[int] = None      # N_f at this dT
    damage: Optional[float] = None               # cycles / N_f (Miner)


class FatigueReference(BaseModel):
    """v0.13 - the reference condition the damage model is anchored to. NOT a budget."""
    model: str = "Coffin-Manson"
    accumulation: str = "Miner's rule (linear damage)"
    cycles_to_failure: Optional[int] = None
    at_delta_t_c: Optional[int] = None
    exponent_n: Optional[float] = None
    basis: str = "assumed"
    note: Optional[str] = None


class UnitExposure(BaseModel):
    """v0.13 - what the unit physically endured."""
    powered_hours: Optional[int] = None
    ignition_cycles: Optional[int] = None
    thermal_cycles_logged: Optional[int] = None
    delta_t_histogram: List[DeltaTBand] = Field(default_factory=list)
    fatigue_reference: Optional[FatigueReference] = None
    fatigue_consumed: Optional[float] = None      # 0-1, sum of Miner damage
    fatigue_remaining_pct: Optional[int] = None
    board_temp_max_c: Optional[int] = None
    board_temp_limit_c: Optional[int] = None
    hours_above_limit: Optional[int] = None
    temp_histogram_h: List[TempBand] = Field(default_factory=list)


class UnitElectrical(BaseModel):
    voltage_transients_logged: Optional[int] = None
    transient_standard: Optional[str] = None      # ISO 7637-2
    undervoltage_events: Optional[int] = None
    load_dump_events: Optional[int] = None
    note: Optional[str] = None


class UnitCompute(BaseModel):
    cpu_hours_above_80pct: Optional[int] = None
    flash_write_cycles_used: Optional[int] = None
    flash_write_cycle_limit: Optional[int] = None
    flash_endurance_remaining_pct: Optional[int] = None
    ecc_corrected_errors: Optional[int] = None
    unexpected_resets: Optional[int] = None


class UnitDiagnostics(BaseModel):
    can_error_frames: Optional[int] = None
    bus_off_events: Optional[int] = None
    dtc_total: Optional[int] = None
    dtc_active: Optional[int] = None
    dtc_cleared: Optional[int] = None
    dtc_linked_to_service_events: Optional[int] = None
    note: Optional[str] = None


class UnitCalibration(BaseModel):
    firmware_versions_installed: Optional[int] = None
    firmware_first: Optional[str] = None
    firmware_last: Optional[str] = None
    calibration_map_changes: Optional[int] = None
    sensor_recalibrations: Optional[int] = None


class HealthIndicator(BaseModel):
    id: Optional[str] = None
    label: Optional[str] = None
    value_pct: Optional[int] = None
    detail: Optional[str] = None


class ReuseVerdict(BaseModel):
    """v0.13 - per-component end-of-use verdict. The mass-weighted share of the
    'reuse*' verdicts is the functional reuse yield that Sc4 declares [A]."""
    component_id: Optional[str] = None        # -> components[].id
    name: Optional[str] = None
    mass_g: Optional[float] = None
    verdict: Optional[str] = None             # reuse | reuse_after_test | material_recovery | consumable
    reason: Optional[str] = None


class HealthFinding(BaseModel):
    """v0.13 - reported but deliberately NOT scored."""
    id: Optional[str] = None
    label: Optional[str] = None
    value: Optional[str] = None
    note: Optional[str] = None


class UnitHealth(BaseModel):
    soh_pct: Optional[int] = None
    soh_method: Optional[str] = None
    soh_limiting_mechanism: Optional[str] = None
    findings: List[HealthFinding] = Field(default_factory=list)
    indicators: List[HealthIndicator] = Field(default_factory=list)
    reuse_assessment: List[ReuseVerdict] = Field(default_factory=list)
    reuse_fraction_by_mass: Optional[float] = None
    reuse_note: Optional[str] = None


class UnitUsePhase(BaseModel):
    """v0.13 - USE-PHASE TELEMETRY THE VCU RECORDS ABOUT ITSELF.

    Replaces the vehicle-centric usage map (dropped 2026-08-06): a product
    passport describes the product, not its owner's movements. Every figure is
    derived from data already present in the passport."""
    exposure: Optional[UnitExposure] = None
    electrical: Optional[UnitElectrical] = None
    compute: Optional[UnitCompute] = None
    diagnostics: Optional[UnitDiagnostics] = None
    calibration: Optional[UnitCalibration] = None
    health: Optional[UnitHealth] = None
    basis: str = "simulated"
    note: Optional[str] = None


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
    # ---- v0.6 additions (all optional / empty-by-default) ----
    dpp_meta: Optional[DppMeta] = None
    physical_unit: Optional[PhysicalUnit] = None   # v0.8
    documents: List[DocumentRef] = Field(default_factory=list)
    substances_of_concern: List[SubstanceOfConcern] = Field(default_factory=list)
    service: Optional[Service] = None
    usage_history: Optional[UsageHistory] = None
    repair_history: Optional[RepairHistory] = None
    unit_use_phase: Optional[UnitUsePhase] = None   # v0.13
    indicators: Optional[Indicators] = None
    certifications: List[Certification] = Field(default_factory=list)
