using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Data bindings for the RBv2.0 passport screens (specs 13 + 14).
    ///
    /// ONE instance serves BOTH screens: it lives on DppCanva and its LCA fields
    /// point at objects inside ModelExploration. Every field is null-guarded, so
    /// a screen that is not built simply does not update.
    ///
    /// CLEANUP 2026-07-30: removed the retired full-width LCA card summary field
    /// and an LCA caption field that was never wired.
    /// The builder creates the visual hierarchy with sensible demo defaults;
    /// Populate(DPPData) overwrites every bound element from the payload,
    /// including the hazard row's conditional neutral/red styling and the
    /// LCA bars whose widths are data-driven.
    /// </summary>
    public class InfoTabView : MonoBehaviour
    {
        private const string Dash = "—";

        [Header("Identity")]
        [SerializeField] private TMP_Text manufacturerValue;
        [SerializeField] private TMP_Text modelValue;
        [SerializeField] private TMP_Text typeNumberValue;
        [SerializeField] private TMP_Text productionValue;
        [SerializeField] private TMP_Text specsValue;
        [SerializeField] private TMP_Text serviceLifeValue;

        [Header("Materials")]
        [SerializeField] private TMP_Text housingValue;
        [SerializeField] private TMP_Text connectorsValue;
        [SerializeField] private TMP_Text pcbValue;
        [SerializeField] private TMP_Text activesValue;
        [SerializeField] private TMP_Text preciousValue;
        [SerializeField] private TMP_Text recycledValue;

        [Header("Hazard row (conditional styling, spec 02 §5.4)")]
        [SerializeField] private Image hazardFill;
        [SerializeField] private Image hazardStroke;
        [SerializeField] private GameObject hazardIconNeutral;
        [SerializeField] private GameObject hazardIconRed;
        [SerializeField] private TMP_Text hazardTitle;
        [SerializeField] private TMP_Text hazardBadge;
        [SerializeField] private Image hazardChevron;
        [SerializeField] private TMP_Text hazardBatteryValue;
        [SerializeField] private TMP_Text hazardSubstancesValue;
        [SerializeField] private TMP_Text hazardSolderValue;
        [SerializeField] private TMP_Text hazardTreatmentValue;

        [Header("Compliance")]
        [SerializeField] private TMP_Text ceValue;
        [SerializeField] private TMP_Text rohsValue;
        [SerializeField] private TMP_Text reachValue;
        [SerializeField] private TMP_Text weeeValue;
        [SerializeField] private TMP_Text routeValue;

        [Header("LCA — headline (lives on the Model Exploration screen)")]
        [SerializeField] private TMP_Text lcaHeadlineValue;   // big number

        [Header("LCA — recovery potential")]
        [SerializeField] private TMP_Text recoveryTitle;
        [SerializeField] private RectTransform recoveryBar0;
        [SerializeField] private RectTransform recoveryBar1;
        [SerializeField] private RectTransform recoveryBar2;
        [SerializeField] private RectTransform recoveryBar3;
        [SerializeField] private TMP_Text recoveryLabel0;
        [SerializeField] private TMP_Text recoveryLabel1;
        [SerializeField] private TMP_Text recoveryLabel2;
        [SerializeField] private TMP_Text recoveryLabel3;
        private const float RecoveryBarMaxWidth = 134f; // v3 modal layout

        [Header("LCA — stage strip + grid")]
        [SerializeField] private RectTransform stageSeg0;
        [SerializeField] private RectTransform stageSeg1;
        [SerializeField] private RectTransform stageSeg2;
        [SerializeField] private RectTransform stageSeg3;
        [SerializeField] private TMP_Text stageLabel0;
        [SerializeField] private TMP_Text stageLabel1;
        [SerializeField] private TMP_Text stageLabel2;
        [SerializeField] private TMP_Text stageLabel3;
        [SerializeField] private TMP_Text stageValue0;
        [SerializeField] private TMP_Text stageValue1;
        [SerializeField] private TMP_Text stageValue2;
        [SerializeField] private TMP_Text stageValue3;
        private const float StageStripWidth = 592f; // v3 modal: full content width

        [Header("LCA — method")]
        [SerializeField] private TMP_Text methodValue;

        public void Populate(DPPData data)
        {
            if (data == null) return;

            PopulateIdentity(data);
            PopulateMaterials(data);
            PopulateHazard(data);
            PopulateCompliance(data);
            PopulateLca(data);
        }

        // ----------------------------------------------------------------

        private void PopulateIdentity(DPPData d)
        {
            Set(manufacturerValue, d.identity?.manufacturer);
            Set(modelValue, d.identity?.model);
            Set(typeNumberValue, d.identity?.type_number);

            string prod = d.identity?.production_date;
            if (!string.IsNullOrEmpty(prod) && prod.Length >= 7) prod = prod.Substring(0, 7);
            string country = d.identity?.country_of_origin;
            Set(productionValue, JoinDots(prod, country));

            var s = d.specifications;
            Set(specsValue, s == null ? null : JoinDots(
                string.IsNullOrEmpty(s.size_mm) ? null : $"{s.size_mm} mm",
                s.weight_g.HasValue ? $"{s.weight_g.Value:0} g" : null,
                s.protection_class));

            var u = d.environmental?.usage_profile;
            Set(serviceLifeValue, u == null ? null : JoinDots(
                u.service_life_years.HasValue ? $"{u.service_life_years.Value} y" : null,
                u.lifetime_distance_km.HasValue
                    ? string.Format(CultureInfo.InvariantCulture, "{0:N0} km", u.lifetime_distance_km.Value)
                    : null));
        }

        private void PopulateMaterials(DPPData d)
        {
            var comps = d.components;
            if (comps != null)
            {
                var housing = comps.FirstOrDefault(c => c.id == "housing");
                if (housing != null) Set(housingValue, $"{housing.material} · {housing.weight_g:0} g");

                var conn = comps.FirstOrDefault(c => c.id == "connectors");
                if (conn != null) Set(connectorsValue, $"{conn.material} · {conn.weight_g:0} g");

                float pcbMass = comps.Where(c => c.disassembly_step == 3).Sum(c => c.weight_g);
                if (pcbMass > 0f) Set(pcbValue, $"FR-4 · Cu · ≈{pcbMass:0} g");

                var act = comps.FirstOrDefault(c => c.id == "actives");
                if (act != null) Set(activesValue, $"Silicon · {act.weight_g:0} g");
            }

            if (d.precious_metals != null && d.precious_metals.Count > 0)
            {
                var parts = d.precious_metals.Take(3).Select(m =>
                {
                    string sym = m.metal;
                    int open = sym.IndexOf('('); int close = sym.IndexOf(')');
                    if (open >= 0 && close > open) sym = sym.Substring(open + 1, close - open - 1);
                    return $"{sym} {Mathf.RoundToInt(m.mass_mg)}";
                });
                Set(preciousValue, string.Join(" · ", parts) + " mg");
            }

            float? rec = d.environmental?.recycled_content_pct;
            Set(recycledValue, rec.HasValue ? $"{rec.Value:0}%" : null);
        }

        private void PopulateHazard(DPPData d)
        {
            bool containsBattery = d.end_of_life?.contains_battery ?? false;
            bool hasWarnings = d.end_of_life?.hazardous_warnings != null && d.end_of_life.hazardous_warnings.Count > 0;
            bool anyComponentHazard = d.components != null && d.components.Any(c => c.hazardous);
            bool hazardous = containsBattery || hasWarnings || anyComponentHazard;

            // Conditional styling (spec 02 §5.4): neutral unless real hazards.
            if (hazardFill != null)   hazardFill.color   = hazardous ? DPPTheme.SafetyFillRow : DPPTheme.RowFill;
            if (hazardStroke != null) hazardStroke.color = hazardous ? DPPTheme.Hex("#7a3a4a") : DPPTheme.RowStroke;
            if (hazardIconNeutral != null) hazardIconNeutral.SetActive(!hazardous);
            if (hazardIconRed != null)     hazardIconRed.SetActive(hazardous);
            if (hazardTitle != null)   hazardTitle.color   = hazardous ? DPPTheme.Hex("#f3b6b6") : DPPTheme.TextOnNavy;
            if (hazardChevron != null) hazardChevron.color = hazardous ? DPPTheme.Hex("#d98a8a") : DPPTheme.TextSecondary;

            if (hazardBadge != null)
            {
                hazardBadge.color = hazardous ? DPPTheme.Hex("#d98a8a") : DPPTheme.TextSecondary;
                hazardBadge.text = hazardous
                    ? (containsBattery ? "contains battery" : "see warnings")
                    : "no battery · lead-free";
            }

            Set(hazardBatteryValue, containsBattery ? "Yes" : "No");
            Set(hazardSubstancesValue, hasWarnings
                ? string.Join(", ", d.end_of_life.hazardous_warnings)
                : "None documented");

            var solder = d.components?.FirstOrDefault(c => c.id == "solder");
            Set(hazardSolderValue, solder?.material);
            Set(hazardTreatmentValue, d.end_of_life?.recycling_route);
        }

        private void PopulateCompliance(DPPData d)
        {
            Set(ceValue, BoolText(d.compliance?.ce));
            Set(rohsValue, BoolText(d.compliance?.rohs));
            Set(reachValue, BoolText(d.compliance?.reach));
            Set(weeeValue, d.compliance?.weee_category);
            Set(routeValue, d.end_of_life?.recycling_route);
        }

        private void PopulateLca(DPPData d)
        {
            var env = d.environmental;
            if (env == null) return;

            if (lcaHeadlineValue != null && env.co2_footprint_kg.HasValue)
                lcaHeadlineValue.text = env.co2_footprint_kg.Value.ToString("0.0", CultureInfo.InvariantCulture);

            // Recovery potential
            var rp = env.recovery_potential;
            if (rp != null)
            {
                if (recoveryTitle != null)
                    recoveryTitle.text = $"Recovery potential — up to {rp.total_avoidable_kg.ToString("0.0", CultureInfo.InvariantCulture)} kg CO2e";

                var bars = new[] { recoveryBar0, recoveryBar1, recoveryBar2, recoveryBar3 };
                var labels = new[] { recoveryLabel0, recoveryLabel1, recoveryLabel2, recoveryLabel3 };
                float max = rp.credits != null && rp.credits.Count > 0 ? rp.credits.Max(c => c.avoided_kg) : 1f;

                for (int i = 0; i < bars.Length; i++)
                {
                    bool has = rp.credits != null && i < rp.credits.Count;
                    if (bars[i] != null)
                    {
                        bars[i].gameObject.SetActive(has);
                        if (has)
                            bars[i].sizeDelta = new Vector2(
                                Mathf.Max(6f, rp.credits[i].avoided_kg / max * RecoveryBarMaxWidth),
                                bars[i].sizeDelta.y);
                    }
                    if (labels[i] != null)
                    {
                        labels[i].gameObject.SetActive(has);
                        if (has)
                            labels[i].text = $"{rp.credits[i].material} · {rp.credits[i].avoided_kg.ToString("0.0", CultureInfo.InvariantCulture)}";
                    }
                }
            }

            // Stage strip + grid
            var stages = env.lifecycle_stages;
            if (stages != null && stages.Count > 0)
            {
                float total = stages.Sum(s => s.co2_kg);
                var segs = new[] { stageSeg0, stageSeg1, stageSeg2, stageSeg3 };
                var labs = new[] { stageLabel0, stageLabel1, stageLabel2, stageLabel3 };
                var vals = new[] { stageValue0, stageValue1, stageValue2, stageValue3 };

                float x = 0f;
                for (int i = 0; i < segs.Length; i++)
                {
                    bool has = i < stages.Count && total > 0f;
                    if (segs[i] != null)
                    {
                        segs[i].gameObject.SetActive(has);
                        if (has)
                        {
                            float w = Mathf.Max(2f, stages[i].co2_kg / total * StageStripWidth);
                            segs[i].anchoredPosition = new Vector2(x, 0f);
                            segs[i].sizeDelta = new Vector2(w, segs[i].sizeDelta.y);
                            x += w;
                        }
                    }
                    if (has)
                    {
                        bool noted = !string.IsNullOrEmpty(stages[i].note);
                        if (labs[i] != null) labs[i].text = $"{stages[i].id} {ShortStageName(stages[i].name)}{(noted ? "*" : "")}";
                        if (vals[i] != null) vals[i].text = stages[i].co2_kg.ToString("0.0", CultureInfo.InvariantCulture);
                    }
                }
            }

            Set(methodValue, env.method);
        }

        // ----------------------------------------------------------------

        private static string ShortStageName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            // "Raw material extraction" → "Raw materials" etc. — keep grid compact.
            if (name.StartsWith("Raw material")) return "Raw materials";
            return name;
        }

        private static string BoolText(bool? b) => b.HasValue ? (b.Value ? "Yes" : "No") : null;

        private static string JoinDots(params string[] parts)
        {
            var kept = parts.Where(p => !string.IsNullOrEmpty(p)).ToArray();
            return kept.Length == 0 ? null : string.Join(" · ", kept);
        }

        private static void Set(TMP_Text target, string value)
        {
            if (target != null) target.text = string.IsNullOrEmpty(value) ? Dash : value;
        }
    }
}
