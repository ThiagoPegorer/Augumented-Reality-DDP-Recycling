using System.Globalization;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Screen 09 — Completion summary (spec 09 v2.1). Receives the session
    /// result from StepFlowController, binds passport data, and posts the
    /// recovery report.
    ///
    /// Single-action flow (2026-06-11 revision): only "Send recovery report"
    /// is shown at first. On success the same button becomes "Done" (→ Main
    /// Page) and a confirmation message appears beside it. On failure the
    /// button offers retry. No "✓" glyphs — not covered by the SF Pro atlas.
    ///
    /// Future (spec 09 §9): PDF report server-side; post-send options
    /// "Close application" / "Scan a new device".
    /// </summary>
    public class CompletionSummaryView : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private DPPClient client;
        [SerializeField] private ScreenRouter router;

        [Header("Header")]
        [SerializeField] private TMP_Text eyebrowText;       // "MS 50.4 · {serial}"

        [Header("Stat cards")]
        [SerializeField] private TMP_Text timeValue;         // "4 min 12 s"
        [SerializeField] private TMP_Text stepsValue;        // "5 / 5"

        [Header("Recovered cards (data-bound values)")]
        [SerializeField] private TMP_Text aluminiumTitle;    // "Aluminium housing · 363 g"
        [SerializeField] private TMP_Text co2Title;          // "CO2 avoided · up to 6.6 kg"

        [Header("Action button (Send → Done) + confirmation message")]
        [SerializeField] private TMP_Text actionLabel;
        [SerializeField] private Button actionButton;
        [SerializeField] private GameObject actionChevron;
        [SerializeField] private TMP_Text sentMessage;       // "Report was successfully sent"

        private DPPData _data;
        private int _elapsedS;
        private int _stepsCompleted;
        private bool _sent;

        // ---- called by DPPManager on fetch ----
        public void Populate(DPPData data)
        {
            _data = data;
            if (data == null) return;

            if (eyebrowText != null)
            {
                string model = ShortModel(data.identity?.model);
                string serial = data.identity?.serial_number ?? "—";
                eyebrowText.text = $"{model} · {serial}";
            }

            var housing = data.components?.FirstOrDefault(c => c.id == "housing");
            if (aluminiumTitle != null && housing != null)
                aluminiumTitle.text = $"Aluminium housing · {housing.weight_g:0} g";

            float? co2 = data.environmental?.recovery_potential?.total_avoidable_kg;
            if (co2Title != null && co2.HasValue)
                co2Title.text = $"CO2 avoided · up to {co2.Value.ToString("0.0", CultureInfo.InvariantCulture)} kg";
        }

        // ---- called by StepFlowController when the flow finishes ----
        public void SetSession(int elapsedSeconds, int stepsCompleted, int totalSteps)
        {
            _elapsedS = elapsedSeconds;
            _stepsCompleted = stepsCompleted;

            if (timeValue != null)
                timeValue.text = $"{elapsedSeconds / 60} min {elapsedSeconds % 60:00} s";
            if (stepsValue != null)
                stepsValue.text = $"{stepsCompleted} / {totalSteps}";

            ResetState();
        }

        /// <summary>Single action button: sends the report, then acts as Done.</summary>
        public void OnActionButton()
        {
            if (_sent)
            {
                if (router != null) router.ShowMainPage();
                return;
            }
            SendReport();
        }

        private void SendReport()
        {
            if (client == null || _data == null)
            {
                Debug.LogWarning("[CompletionSummaryView] Missing client or data — cannot send report.");
                return;
            }

            string productId = _data.product_id ?? "vcu_001";
            string ids = _data.components != null
                ? string.Join(",", _data.components.Select(c => $"\"{c.id}\""))
                : "";
            float? co2 = _data.environmental?.recovery_potential?.total_avoidable_kg;

            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"product_id\":\"{productId}\",");
            sb.Append($"\"timestamp\":\"{System.DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}\",");
            sb.Append($"\"elapsed_s\":{_elapsedS},");
            sb.Append($"\"steps_completed\":{_stepsCompleted},");
            sb.Append($"\"recovered_component_ids\":[{ids}],");
            sb.Append(co2.HasValue
                ? $"\"co2_avoided_kg\":{co2.Value.ToString("0.00", CultureInfo.InvariantCulture)}"
                : "\"co2_avoided_kg\":null");
            sb.Append("}");

            if (actionLabel != null) actionLabel.text = "Sending…";
            if (actionButton != null) actionButton.interactable = false;

            StartCoroutine(client.PostReport(productId, sb.ToString(), OnSent, OnSendError));
        }

        private void OnSent()
        {
            _sent = true;
            if (actionLabel != null) actionLabel.text = "Done";
            if (actionChevron != null) actionChevron.SetActive(false);
            if (actionButton != null) actionButton.interactable = true;
            if (sentMessage != null) sentMessage.gameObject.SetActive(true);
            Debug.Log("[CompletionSummaryView] Recovery report stored by backend.");
        }

        private void OnSendError(string error)
        {
            Debug.LogWarning($"[CompletionSummaryView] {error}");
            if (actionLabel != null) actionLabel.text = "Could not send — retry";
            if (actionButton != null) actionButton.interactable = true;
        }

        private void ResetState()
        {
            _sent = false;
            if (actionLabel != null) actionLabel.text = "Send recovery report";
            if (actionChevron != null) actionChevron.SetActive(true);
            if (actionButton != null) actionButton.interactable = true;
            if (sentMessage != null) sentMessage.gameObject.SetActive(false);
        }

        private static string ShortModel(string model)
        {
            if (string.IsNullOrEmpty(model)) return "—";
            int idx = model.IndexOf("MS", System.StringComparison.Ordinal);
            return idx >= 0 ? model.Substring(idx) : model;
        }
    }
}
