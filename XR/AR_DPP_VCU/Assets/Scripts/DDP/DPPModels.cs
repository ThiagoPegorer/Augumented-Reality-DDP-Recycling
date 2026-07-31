using System;
using System.Collections.Generic;

namespace DPP.Models
{
    // C# mirror of the DPP schema. The source of truth is backend/models.py;
    // schema/dpp_schema.json is GENERATED from it (backend/export_schema.py).
    // Keep this file in sync with models.py by hand.
    //
    // v0.7 (2026-07-30) — openLCA v4 / EF 3.1 swap.
    //   Environmental gains impact_recovery[]: per impact category, the baseline and
    //   each EoL scenario's net / saving / reduction. Populated for the three
    //   categories EF 3.1 screening selects (minerals 72.5 %, climate 6.7 %,
    //   freshwater eutrophication 6.6 %).
    //   lifecycle_stages was CLEARED — no per-stage openLCA export exists and the old
    //   assumed split summed to 63.9 kg against the correct 73.43 kg headline. Render
    //   an honest empty state, not the stale numbers.
    //
    // v0.6 (2026-07-30) — CIRPASS D2.2 Table 6 completeness pass.
    //   Every one of the 22 attributes in Table 6 (pp. 41–42) now has a slot, so
    //   the passport can STATE "not provided" instead of omitting a field.
    //   Coverage map: DPP_UI_Specs/13b_information_model.md.
    //
    //   New: DppMeta · DocumentRef[] · SubstanceOfConcern[] · Service ·
    //        UsageHistory · RepairHistory · Indicators · Certification[]
    //        + CollectionScheme on EndOfLife, safety fields on Disassembly,
    //        basis/source fields on Environmental and Compliance.
    //
    //   HONEST-LABELLING VOCABULARIES (closed — the UI switches on these):
    //     basis  : declared | datasheet | measured | assumed | modelled | not_provided
    //     status : available | not_provided | not_applicable
    //   "not_applicable" is a real answer: Table 6 #1 and #2 come from the Energy
    //   Labelling Regulation, which does not cover automotive control units.
    //
    //   Nothing from v0.5 was removed or renamed — every new member is nullable
    //   or an empty collection, so a v0.5 payload still deserializes.
    //
    // NOTE: Unity's built-in JsonUtility does NOT support nullable types.
    //       Use Newtonsoft.Json: JsonConvert.DeserializeObject<DPPData>(json).

    /// <summary>Closed vocabularies from models.py. Compare with string equality —
    /// an unknown value must render as "not provided", never as a blank.</summary>
    public static class DppBasis
    {
        public const string Declared    = "declared";
        public const string Datasheet   = "datasheet";
        public const string Measured    = "measured";
        public const string Assumed     = "assumed";
        public const string Modelled    = "modelled";
        /// <summary>v0.9 — INVENTED for the study demonstrator. Not a weak measurement:
        /// not a measurement at all. Deliberately excluded from IsFirmSource so every dot
        /// bound to it renders dim.</summary>
        public const string Simulated   = "simulated";
        public const string NotProvided = "not_provided";

        /// <summary>True when a value may be shown as fact rather than as an estimate.</summary>
        public static bool IsFirmSource(string basis) =>
            basis == Declared || basis == Datasheet || basis == Measured;
    }

    public static class DppStatus
    {
        public const string Available     = "available";
        public const string NotProvided   = "not_provided";
        public const string NotApplicable = "not_applicable";
    }

    [Serializable]
    public class DPPData
    {
        public string product_id;
        public DppMeta dpp_meta;                              // v0.6
        public Identity identity;
        public Specifications specifications;
        public List<DocumentRef> documents;                   // v0.6 — T6 #1 #2 #3 #12 #16 #17
        public List<Component> components;
        public List<SubstanceOfConcern> substances_of_concern; // v0.6 — T6 #5 #6
        public List<PreciousMetal> precious_metals;
        public Environmental environmental;
        public Indicators indicators;                         // v0.6 — T6 #21
        public Compliance compliance;
        public List<Certification> certifications;            // v0.6 — T6 #22
        public Service service;                               // v0.6 — T6 #12 #15
        public UsageHistory usage_history;                    // v0.6 — T6 #19
        public RepairHistory repair_history;                  // v0.6 — T6 #20
        public Disassembly disassembly;
        public EndOfLife end_of_life;
        public PhysicalUnit physical_unit;                    // v0.8 - the demonstrator, not the product
    }

    /// <summary>v0.8 — one part of the PHYSICAL DEMONSTRATOR the participant handles.
    /// NOT product data: the printed study unit stands in for a Bosch MS 50.4 and its
    /// parts are coloured blocks. Kept out of Specifications so replica facts can never
    /// be read as the product's declared values.</summary>
    [Serializable]
    public class PhysicalPart
    {
        public string id;
        public string name;
        public int count = 1;
        public string colour;
        public string swatch_hex;
        public string photo_id;
        public string note;
    }

    /// <summary>v0.8 — facts about the demonstrator itself.</summary>
    [Serializable]
    public class PhysicalUnit
    {
        public bool is_replica = true;
        public string replica_of;
        public string size_mm;
        public List<PhysicalPart> parts;
        public string basis;
        public string note;
    }

    /// <summary>v0.6 — provenance of the record itself, so a reader can tell how
    /// much of this passport is real.</summary>
    [Serializable]
    public class DppMeta
    {
        public string schema_version;
        public string reference_framework;   // "CIRPASS D2.2 Table 6 (pp. 41-42)"
        public string last_updated;          // ISO date
        public string completeness_note;
    }

    [Serializable]
    public class Identity
    {
        public string manufacturer;
        public string model;
        public string type_number;        // manufacturer order/type no. (nullable)
        public string serial_number;
        public string production_date;    // ISO 8601 date string
        public string country_of_origin;  // ISO 3166-1 alpha-2
        // v0.6 — D2.1 product identification / economic operator
        public string product_category;   // "EEE - electronic control unit (WEEE cat. 5)"
        public string economic_operator;
        public string brand;
    }

    [Serializable]
    public class Specifications
    {
        public string size_mm;            // "200 x 150 x 60"
        public float? weight_g;
        public string protection_class;   // "IP67"
        public string supply_voltage;     // "5-18 V"
        public string operating_temp_c;   // "-20 to 80"
        public string connectors;
        // v0.6 — Table 6 #1 energy consumption & performance
        public float? power_consumption_w;
        public string performance_note;
        public bool? energy_label_applicable;   // false for a VCU
        public string energy_label_note;
    }

    /// <summary>v0.6 — one document the passport must reference (T6 #1 #2 #3 #12 #16 #17 #22).
    /// These are PDFs in the real world. On a headset the passport's job is to say
    /// WHETHER they exist, not to render them — so an absent mandatory document is
    /// visible instead of silently missing.</summary>
    [Serializable]
    public class DocumentRef
    {
        public string id;                 // "safe_use" | "user_manual" | ...
        public string title;
        public string kind;               // sheet | documentation | manual | instructions | declaration | certificate
        public string cirpass_ref;        // "T6 #16" — traceability to Table 6
        public bool mandatory;            // M in Table 6
        public string status;             // DppStatus
        public string url;
        public string note;               // why not_applicable / where to get it
    }

    [Serializable]
    public class MaterialShare
    {
        public string material;          // e.g. "brass (Cu-Zn)"
        public float weight_g;
    }

    [Serializable]
    public class Component
    {
        public string id;
        public string name;
        public string material;
        public float weight_g;
        public string recycling_code;
        public int disassembly_step;      // guided step (1-5) handling this part
        public bool hazardous;
        public bool high_value;
        public string basis;              // DppBasis
        public List<MaterialShare> material_breakdown;
        public string material_breakdown_basis;
    }

    /// <summary>v0.6 — Table 6 #5 and #6, the two hardest mandatory attributes.
    ///
    /// #6 requires the substance NAME, its LOCATION WITHIN THE PRODUCT and its
    /// CONCENTRATION. <see cref="component_id"/> is the location and links into
    /// components[], which is what lets the AR client highlight the physical part
    /// carrying the substance — the capability a paper WEEE sheet cannot offer
    /// (CIRPASS Table 8 p.56, UC4 Figure 16 step 2).
    ///
    /// An EMPTY list means "none DECLARED", which is a different claim from
    /// "none present". Read EndOfLife.substances_basis to tell them apart.</summary>
    [Serializable]
    public class SubstanceOfConcern
    {
        public string name;                    // "Lead (Pb)"
        public string cas_number;
        public string regulation;              // "REACH SVHC" | "RoHS Annex II" | "POP" | "CLP"
        public string component_id;            // LOCATION → components[].id
        public string location_note;
        public float? concentration_pct_w_w;
        public float? threshold_pct_w_w;       // e.g. 0.1 for REACH SVHC
        public bool? above_threshold;
        public string symbol;                  // v0.12 — short form for the UI ("Pb", "PbO")
        public string basis;                   // DppBasis
    }

    [Serializable]
    public class PreciousMetal
    {
        public string metal;              // "Gold (Au)"
        public string location;
        public float mass_mg;
    }

    [Serializable]
    public class LifecycleStage
    {
        public string id;                 // "S1".."S4"
        public string name;
        public float co2_kg;
        public string note;
    }

    [Serializable]
    public class RecoveryCredit
    {
        public string material;
        public float avoided_kg;
    }

    [Serializable]
    public class RecoveryPotential
    {
        public float total_avoidable_kg;
        public string note;
        public List<RecoveryCredit> credits;
        public string scenario;           // v0.7 — which EoL scenario this figure is
        public string basis;              // v0.7 — DppBasis
    }

    /// <summary>v0.7 — one end-of-life scenario's outcome for one impact category.</summary>
    [Serializable]
    public class ImpactRecoveryScenario
    {
        public string id;                 // "Sc2" | "Sc3" | "Sc4"
        public string label;              // "Guided dismantling"
        public float net;                 // scenario total, credits applied
        public float saving;              // baseline − net
        public float reduction_pct;
        public string note;               // declares an exploratory assumption, if any
    }

    /// <summary>v0.7 — Table 6 #21, the quantitative half: how much of one impact
    /// category the end-of-life route can avoid.
    ///
    /// WHY THIS EXISTS: <see cref="RecoveryPotential"/> is per MATERIAL and
    /// climate-only. The screen needs per IMPACT CATEGORY across scenarios, because
    /// the thesis result is that the scenarios answer different questions — reuse
    /// pays in carbon, dismantling-for-smelting pays in minerals. A "recovery rate"
    /// with no named scenario and category is not interpretable.
    ///
    /// Source: impact_EF31.csv (sc1, sc*_net, sc*_saving). NEVER mc_net.csv.</summary>
    [Serializable]
    public class ImpactRecovery
    {
        public string category;           // EF 3.1 category name, verbatim
        public string unit;
        public float? screening_share_pct;
        public float baseline;
        public string baseline_scenario;  // "Sc1"
        public List<ImpactRecoveryScenario> scenarios;
        public string method;             // "EF 3.1"
        public string basis;              // DppBasis
    }

    /// <summary>DESIGNED service-life assumptions — a specification, not a
    /// measurement. Measured use data lives in <see cref="UsageHistory"/>.</summary>
    [Serializable]
    public class UsageProfile
    {
        public int? service_life_years;
        public int? lifetime_distance_km;
        public int? operating_hours;             // v0.11: 5,625 = 225,000 km / 40 km/h
        public float? lifetime_energy_kwh;       // v0.11: OWN draw, 66.2 kWh (S4)
        public List<AnnualDistance> annual_distances;    // v0.11
        public string service_period;            // "Apr 2011 - Mar 2026"
        public float? avg_speed_kmh;
        public float? own_power_w;
        public float? charging_efficiency;
        public float? car_energy_kwh_estimate;   // OUTSIDE the S4 boundary
        public string daily_use;
        public string basis;                     // DppBasis
        public string note;
    }

    /// <summary>v0.11 — one year of the modelled distance series (Usage Profile).</summary>
    [Serializable]
    public class AnnualDistance
    {
        public string year;
        public int distance_km;
        public string note;              // "from Apr" / "to Mar" on partial years
    }

    [Serializable]
    public class Environmental
    {
        public float? co2_footprint_kg;   // headline: lifecycle total
        public string method;
        public float? recycled_content_pct;      // T6 #10
        public string recycled_content_basis;    // v0.6 — DppBasis
        public List<LifecycleStage> lifecycle_stages;
        public string lifecycle_stages_basis;    // v0.7 — DppBasis
        public string lifecycle_stages_note;     // v0.7 — why it is empty, if it is
        public RecoveryPotential recovery_potential;
        public List<ImpactRecovery> impact_recovery;   // v0.7 — per impact category
        public UsageProfile usage_profile;
        public string lca_basis;                 // v0.6 — "modelled" | "assumed"
        public string lca_source;                // v0.6 — provenance of the figures on screen
    }

    /// <summary>v0.6 — T6 #21. A score without a scale and a method is not
    /// defensible; render all three or render "not provided".</summary>
    [Serializable]
    public class CircularityIndicator
    {
        public string id;                 // "repairability" | "recyclability" | "reusability"
        public string label;
        public float? score;
        public float? scale_max;          // e.g. 10 or 100
        public string method;             // e.g. "EN 45554"
        public string basis;              // DppBasis
    }

    /// <summary>v0.6 — T6 #21. The LCA itself stays in <see cref="Environmental"/>;
    /// this is the index layer.</summary>
    [Serializable]
    public class Indicators
    {
        public List<CircularityIndicator> circularity;
        public float? recyclability_pct_by_mass;
        public string recyclability_basis;
        public string social_impact_note;
        public string pef_note;
    }

    [Serializable]
    public class Compliance
    {
        public bool? ce;
        public string ce_scope;                  // v0.12 — "2014/30/EU (EMC)"
        public string tested_to;                 // v0.12 — "ECE R10 · rev.6 : 2019"
        public string declaration_date;          // v0.12 — ISO yyyy-MM-dd
        public bool? rohs;
        public bool? rohs_applicable;            // v0.12 — False: out of 2011/65/EU scope
        public bool? reach;                      // True = Art. 33 duty fulfilled
        public string weee_category;
        public List<DeclarationNote> declaration_notes;   // v0.12
        // v0.6 — state HOW the flags are known, and point at the DoC document
        public string basis;                                  // DppBasis
        public string declaration_of_conformity_doc_id;        // → documents[].id
    }

    /// <summary>v0.12 — one section of the DoC's further-explanations / disposal text.</summary>
    [Serializable]
    public class DeclarationNote
    {
        public string title;
        public string body;
    }

    /// <summary>v0.6 — T6 #22 responsibility / supply-chain certifications.</summary>
    [Serializable]
    public class Certification
    {
        public string name;               // "IATF 16949"
        public string scope;
        public string issuer;
        public string valid_until;        // ISO date
        public string doc_id;             // → documents[].id
        public string status;             // DppStatus
    }

    /// <summary>v0.6 — T6 #12 spare parts.</summary>
    [Serializable]
    public class SparePart
    {
        public string id;
        public string name;
        public string order_ref;
        public int? availability_years;
        public string component_id;       // → components[].id
        public string status;             // DppStatus
    }

    /// <summary>v0.9 — one entry in the software update log (Table 6 #12).</summary>
    [Serializable]
    public class SoftwareUpdate
    {
        public string date;               // ISO yyyy-MM-dd
        public string version;
        public string channel;            // automatic | manual
        public string note;
    }

    /// <summary>v0.6 — T6 #12 (use / repair / maintenance / updates) and #15
    /// (resale, end-of-life options, waste-handling service availability).</summary>
    [Serializable]
    public class Service
    {
        public List<SparePart> spare_parts;
        public string maintenance_interval;
        public string software_update_policy;
        public string repair_doc_id;      // → documents[].id
        public List<string> resale_options;
        public List<string> eol_options;
        public List<string> waste_handling_services;
        public List<SoftwareUpdate> software_updates;   // v0.9
        public string software_update_basis;            // v0.9 — DppBasis
        public string basis;              // DppBasis
    }

    /// <summary>v0.6 — T6 #4 disposal, return and collection scheme information (M).</summary>
    [Serializable]
    public class CollectionScheme
    {
        public string scheme_name;
        public string take_back;          // e.g. "producer take-back"
        public string scheme_operator;    // NOT "operator" — reserved word in C#
        public string contact;
        public string url;
        public List<string> instructions;
        public string basis;              // DppBasis
    }

    /// <summary>v0.6 — T6 #19. MEASURED use data. Not the same thing as
    /// <see cref="UsageProfile"/>, which is the designed specification.</summary>
    [Serializable]
    public class UsageHistory
    {
        public string purchase_date;
        public string in_service_date;
        public int? use_cycles;
        public int? operating_hours;
        public int? distance_km;
        public string basis;              // DppBasis
    }

    [Serializable]
    public class RepairEvent
    {
        public string date;
        public string description;
        public List<string> exchanged_component_ids;
        public float? cost_eur;
        public string image_url;
    }

    /// <summary>v0.6 — T6 #20 repair data (date, exchanged parts, costs, images).</summary>
    [Serializable]
    public class RepairHistory
    {
        public List<RepairEvent> events;
        public string basis;              // DppBasis
    }

    [Serializable]
    public class StepAction
    {
        public string title;
        public string subtitle;
        public string icon;               // cross|up|pins|usb|lever|board|magnify|chip|recycle|label
        public bool value;                // true → gold high-value accent
    }

    [Serializable]
    public class Step
    {
        public int id;                    // 1-based, matches Component.disassembly_step
        public string title;
        public string tool;
        public List<string> component_ids;
        public List<StepAction> actions;
    }

    [Serializable]
    public class Disassembly
    {
        public int total_steps;
        public int estimated_time_min;
        public List<string> tools;
        public List<string> parts;        // physical part groups for the intro list
        public List<Step> steps;
        // v0.6 — T6 #16 #17: safety text that belongs WITH the procedure
        public List<string> safety_warnings;
        public string safe_use_doc_id;   // → documents[].id
    }

    [Serializable]
    public class EndOfLife
    {
        public string recycling_route;
        public bool contains_battery;
        public List<string> hazardous_warnings;
        // v0.6
        public string substances_basis;                  // DppBasis — governs substances_of_concern
        public List<string> recycling_instructions;      // T6 #11
        public CollectionScheme collection_scheme;       // T6 #4
    }
}
