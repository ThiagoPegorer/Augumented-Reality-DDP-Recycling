using System;
using System.Collections.Generic;

namespace DPP.Models
{
    // C# mirror of the DPP JSON schema v0.3 (schema/dpp_schema.json) and the
    // Pydantic models in backend/models.py. Keep all three in sync.
    //
    // v0.3 (2026-06-10) — Bosch Motorsport VCU MS 50.4 data model:
    //   - Identity gains type_number; new Specifications block.
    //   - Component.disassembly_order renamed to disassembly_step (maps each
    //     of the 12 BOM lines to its guided step 1–5); new basis field.
    //   - New PreciousMetal list.
    //   - Environmental: scenarios[] replaced by lifecycle_stages[] (S1–S4),
    //     recovery_potential (net avoidable CO2 + per-material credits) and
    //     usage_profile. co2_footprint_kg is now the LIFECYCLE TOTAL.
    //   - New Compliance block (nullable until verified).
    //   - EndOfLife gains contains_battery.
    //
    // NOTE: Unity's built-in JsonUtility does NOT support nullable types.
    //       Use Newtonsoft.Json: JsonConvert.DeserializeObject<DPPData>(json).

    [Serializable]
    public class DPPData
    {
        public string product_id;
        public Identity identity;
        public Specifications specifications;
        public List<Component> components;
        public List<PreciousMetal> precious_metals;
        public Environmental environmental;
        public Compliance compliance;
        public Disassembly disassembly;
        public EndOfLife end_of_life;
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
    }

    [Serializable]
    public class Specifications
    {
        public string size_mm;            // "166 x 121 x 41"
        public float? weight_g;
        public string protection_class;   // "IP67"
        public string supply_voltage;     // "5-18 V"
        public string operating_temp_c;   // "-20 to 80"
        public string connectors;         // "3 motorsport (198 pins) + USB"
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
        public bool high_value;           // v0.3.1: worth dedicated recovery
        public string basis;              // "datasheet" | "estimate" (nullable)
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
        public string note;               // e.g. "modelled use profile" (nullable)
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
    }

    [Serializable]
    public class UsageProfile
    {
        public int? service_life_years;
        public int? lifetime_distance_km;
        public int? operating_hours;
        public float? lifetime_energy_kwh;
        public string note;
    }

    [Serializable]
    public class Environmental
    {
        public float? co2_footprint_kg;   // headline: lifecycle total
        public string method;
        public float? recycled_content_pct;
        public List<LifecycleStage> lifecycle_stages;
        public RecoveryPotential recovery_potential;
        public UsageProfile usage_profile;
    }

    [Serializable]
    public class Compliance
    {
        public bool? ce;
        public bool? rohs;
        public bool? reach;
        public string weee_category;
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
        public List<string> tools;        // v0.3.1: e.g. ["Torx driver", "spudger"]
        public List<Step> steps;          // v0.4: guided step content
    }

    [Serializable]
    public class EndOfLife
    {
        public string recycling_route;
        public bool contains_battery;
        public List<string> hazardous_warnings;
    }
}
