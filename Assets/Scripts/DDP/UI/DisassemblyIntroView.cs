using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Data bindings for Screen 03 — Disassembly intro (spec 03 v2 §10).
    /// The builder lays out the 2×2 stat matrix with vcu_001 demo values;
    /// Populate(DPPData) overwrites them from the backend payload.
    ///
    /// Note: the conditional safety banner (spec 03 §4) is not built for the
    /// MS 50.4 (no hazards). If a future product flags hazards, this view
    /// logs a warning so the banner isn't silently missing.
    /// </summary>
    public class DisassemblyIntroView : MonoBehaviour
    {
        [Header("Stat cards (2×2)")]
        [SerializeField] private TMP_Text toolsValue;
        [SerializeField] private TMP_Text toolsSub;
        [SerializeField] private TMP_Text timeValue;
        [SerializeField] private TMP_Text scopeValue;
        [SerializeField] private TMP_Text scopeSub;
        [SerializeField] private TMP_Text recoverValue;
        [SerializeField] private TMP_Text recoverSub;

        public void Populate(DPPData data)
        {
            if (data == null) return;

            // Tools — first entry is the primary tool, the rest join the sub line.
            var tools = data.disassembly?.tools;
            if (tools != null && tools.Count > 0)
            {
                if (toolsValue != null) toolsValue.text = tools[0];
                if (toolsSub != null)
                    toolsSub.text = tools.Count > 1 ? "+ " + string.Join(" · ", tools.Skip(1)) : "";
            }

            if (timeValue != null && data.disassembly != null)
                timeValue.text = $"~{data.disassembly.estimated_time_min} min";

            if (scopeValue != null && data.disassembly != null)
                scopeValue.text = $"{data.disassembly.total_steps} steps";
            if (scopeSub != null && data.components != null)
                scopeSub.text = $"{data.components.Count} parts";

            // Recover — count of high-value components + short names.
            if (data.components != null)
            {
                var hv = data.components.Where(c => c.high_value).ToList();
                if (recoverValue != null)
                    recoverValue.text = $"{hv.Count} high-value";
                if (recoverSub != null)
                    recoverSub.text = string.Join(" · ", hv.Select(ShortName));
            }

            // Safety banner is intentionally not built for this product (no
            // hazards). Surface a loud hint if data ever disagrees.
            bool hazardous = (data.end_of_life?.contains_battery ?? false)
                || (data.end_of_life?.hazardous_warnings?.Count ?? 0) > 0
                || (data.components != null && data.components.Any(c => c.hazardous));
            if (hazardous)
                Debug.LogWarning("[DisassemblyIntroView] Payload flags hazards but the intro has no safety banner built (spec 03 §4). Add it before using this product.");
        }

        private static string ShortName(DPP.Models.Component c)  // fully qualified: avoids clash with UnityEngine.Component
        {
            switch (c.id)
            {
                case "connectors": return "connectors";
                case "actives":    return "silicon";
                default:
                    string first = (c.name ?? c.id).Split(' ')[0].TrimEnd(',', '&');
                    return first.ToLower(CultureInfo.InvariantCulture);
            }
        }
    }
}
