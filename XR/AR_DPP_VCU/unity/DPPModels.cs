using System;
using System.Collections.Generic;

namespace DPP.Models
{
    // C# mirror of the DPP JSON schema (schema/dpp_schema.json) and Pydantic models in backend/models.py.
    // Keep all three in sync.
    //
    // NOTE: Unity's built-in JsonUtility does NOT support nullable types or top-level arrays.
    //       Recommended deserializer: Newtonsoft.Json (com.unity.nuget.newtonsoft-json package).
    //       Example:
    //         using Newtonsoft.Json;
    //         var dpp = JsonConvert.DeserializeObject<DPPData>(jsonString);

    [Serializable]
    public class DPPData
    {
        public string product_id;
        public Identity identity;
        public List<Component> components;
        public Environmental environmental;
        public Disassembly disassembly;
        public EndOfLife end_of_life;
    }

    [Serializable]
    public class Identity
    {
        public string manufacturer;
        public string model;
        public string serial_number;
        public string production_date;     // ISO 8601 date string
        public string country_of_origin;   // ISO 3166-1 alpha-2
    }

    [Serializable]
    public class Component
    {
        public string id;
        public string name;
        public string material;
        public float weight_g;
        public string recycling_code;
        public int disassembly_order;
        public bool hazardous;
    }

    [Serializable]
    public class Environmental
    {
        public float? co2_footprint_kg;
        public List<Scenario> scenarios;
    }

    [Serializable]
    public class Scenario
    {
        public int id;
        public string name;
        public float? value_kg;
    }

    [Serializable]
    public class Disassembly
    {
        public int total_steps;
        public int estimated_time_min;
    }

    [Serializable]
    public class EndOfLife
    {
        public string recycling_route;
        public List<string> hazardous_warnings;
    }
}
