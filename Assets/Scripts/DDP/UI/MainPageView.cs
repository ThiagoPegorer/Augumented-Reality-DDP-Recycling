using TMPro;
using UnityEngine;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Data bindings for Screen 01 — Main Page (DPP_UI_Specs/01 §6).
    ///   - Product serial  ← identity.serial_number   (fallback "VCU-DEMO-001")
    ///   - "N steps" label ← disassembly.total_steps  (fallback 5)
    /// All other text on the page is static UI copy.
    /// </summary>
    public class MainPageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text serialText;
        [SerializeField] private TMP_Text disassemblySubtitleText;

        public void Populate(DPPData data)
        {
            if (data == null) return;

            if (serialText != null && !string.IsNullOrEmpty(data.identity?.serial_number))
                serialText.text = data.identity.serial_number;

            int steps = data.disassembly != null ? data.disassembly.total_steps : 0;
            if (disassemblySubtitleText != null && steps > 0)
                disassemblySubtitleText.text = $"Guided recycling · {steps} steps";
        }
    }
}
