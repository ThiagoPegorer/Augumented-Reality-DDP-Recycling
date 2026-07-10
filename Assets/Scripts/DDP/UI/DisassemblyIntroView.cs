using System.Linq;
using TMPro;
using UnityEngine;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Data bindings for Screen 03 — Disassembly intro (spec 03 v3, 2026-07-10).
    ///
    /// v3 layout: the 2×2 stat-card matrix is gone (boxes read as buttons).
    /// Left half is now plain label/value rows (Tools · Est. time · Scope)
    /// plus a "Dismantling" bullet list fed by disassembly.parts[] — the
    /// PHYSICAL part groups, distinct from the material-based components[].
    /// The Recover card was dropped with it.
    ///
    /// The teardown hero is a live 3D loop (TeardownPreviewLoop) — this view
    /// no longer touches it.
    ///
    /// Note: the conditional safety banner (spec 03 §4) is still not built for
    /// the MS 50.4 (no hazards); this view logs a warning if data disagrees.
    /// </summary>
    public class DisassemblyIntroView : MonoBehaviour
    {
        [Header("Job overview rows")]
        [SerializeField] private TMP_Text toolsValue;
        [SerializeField] private TMP_Text timeValue;
        [SerializeField] private TMP_Text scopeValue;

        [Header("Dismantling list (row label texts; row root = label's parent)")]
        [SerializeField] private TMP_Text[] partLabels;

        public void Populate(DPPData data)
        {
            if (data == null) return;

            // Tools — single line now; multiple tools join with a middot.
            var tools = data.disassembly?.tools;
            if (toolsValue != null && tools != null && tools.Count > 0)
                toolsValue.text = string.Join(" · ", tools);

            if (timeValue != null && data.disassembly != null)
                timeValue.text = $"~{data.disassembly.estimated_time_min} min";

            if (scopeValue != null && data.disassembly != null)
                scopeValue.text = $"{data.disassembly.total_steps} steps";

            // Dismantling — physical part groups from disassembly.parts[].
            var parts = data.disassembly?.parts;
            if (partLabels != null)
            {
                for (int i = 0; i < partLabels.Length; i++)
                {
                    if (partLabels[i] == null) continue;
                    bool has = parts != null && i < parts.Count;
                    if (has) partLabels[i].text = parts[i];
                    // Toggle the whole row (dot + label) via the label's parent.
                    partLabels[i].transform.parent.gameObject.SetActive(has);
                }
                if (parts != null && parts.Count > partLabels.Length)
                    Debug.LogWarning($"[DisassemblyIntroView] Backend sends {parts.Count} dismantling parts but only {partLabels.Length} rows are built — extras not shown.");
            }

            // Safety banner is intentionally not built for this product (no
            // hazards). Surface a loud hint if data ever disagrees.
            bool hazardous = (data.end_of_life?.contains_battery ?? false)
                || (data.end_of_life?.hazardous_warnings?.Count ?? 0) > 0
                || (data.components != null && data.components.Any(c => c.hazardous));
            if (hazardous)
                Debug.LogWarning("[DisassemblyIntroView] Payload flags hazards but the intro has no safety banner built (spec 03 §4). Add it before using this product.");
        }
    }
}
