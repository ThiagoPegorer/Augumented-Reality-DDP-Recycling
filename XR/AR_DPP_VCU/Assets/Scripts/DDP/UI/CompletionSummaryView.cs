using System.Collections.Generic;
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
    /// Screen 09 — Completion summary (spec 09 v3, 2026-07-14).
    ///
    /// v3: eyebrow (model · serial) and all non-button boxes removed. One big
    /// total-time value, then a per-step table: each row = step title, a
    /// materials-with-grams line, the step's time split, and the step's total
    /// recovered mass. Longest step tagged gold. Masses come from components[]
    /// grouped by disassembly_step; aggregate components use their
    /// material_breakdown (basis: assumed — on-screen footnote).
    ///
    /// Single-action flow kept from v2.1: "Send recovery report" → "Done".
    /// The report now includes step_times_s[] (per-step splits — user-test data).
    /// </summary>
    public class CompletionSummaryView : MonoBehaviour
    {
        private const int MaxMaterialsShown = 3;   // top-N by weight, rest → "other"

        [Header("Wiring")]
        [SerializeField] private DPPClient client;
        [SerializeField] private ScreenRouter router;

        [Header("Total time (big value, no label)")]
        [SerializeField] private TMP_Text timeValue;

        [Header("Step table rows (index 0–4, wired by builder)")]
        [SerializeField] private TMP_Text[] stepTitles;
        [SerializeField] private TMP_Text[] stepMaterials;
        [SerializeField] private TMP_Text[] stepTimes;
        [SerializeField] private TMP_Text[] stepMasses;
        [SerializeField] private GameObject[] longestTags;

        [Header("Action button (Send → Done) + confirmation message")]
        [SerializeField] private TMP_Text actionLabel;
        [SerializeField] private Button actionButton;
        [SerializeField] private GameObject actionChevron;
        [SerializeField] private TMP_Text sentMessage;       // "Dismantling report sent"

        [Header("Post-report modal (v4.7): scan new product / main menu")]
        [SerializeField] private GameObject nextModal;

        private QRScanController _scanController;

        /// <summary>Spec 10 (guided mode): the mode controller wraps the action
        /// button — Send runs here, Done exits the mode — and needs to know which
        /// of the two states the button is in.</summary>
        public bool Sent => _sent;

        private DPPData _data;
        private int _elapsedS;
        private int _stepsCompleted;
        private int[] _splits = System.Array.Empty<int>();
        private bool _sent;

        private static readonly Color TimeNormal = Color.white;
        private static readonly Color TimeGold = DPPTheme.Hex("#f0c879");

        // Short display labels for single-material components (id → label).
        private static readonly Dictionary<string, string> ShortLabel = new Dictionary<string, string>
        {
            { "pcb_substrate", "FR-4 board" },
            { "pcb_copper",    "copper" },
            { "solder",        "solder" },
            { "passives",      "passives" },
            { "tim",           "TIM" },
            { "coating",       "coating" },
            { "wiring",        "wiring" },
            { "housing",       "aluminium shells" },
            { "misc",          "labels & adhesive" },
            { "fasteners",     "fasteners" },
            { "connectors",    "connectors" },
            { "actives",       "chips" },
        };

        // ---- called by DPPManager on fetch ----
        public void Populate(DPPData data)
        {
            _data = data;
            if (data == null) return;

            var steps = data.disassembly?.steps;
            int rows = RowCount();

            for (int i = 0; i < rows; i++)
            {
                int stepId = i + 1;

                if (stepTitles != null && i < stepTitles.Length && stepTitles[i] != null)
                {
                    string title = steps != null && i < steps.Count ? steps[i].title : $"Step {stepId}";
                    stepTitles[i].text = $"{stepId} · {title}";
                }

                var comps = data.components?.Where(c => c.disassembly_step == stepId).ToList();
                if (comps == null || comps.Count == 0) continue;

                if (stepMasses != null && i < stepMasses.Length && stepMasses[i] != null)
                    stepMasses[i].text = FormatG(comps.Sum(c => c.weight_g));

                if (stepMaterials != null && i < stepMaterials.Length && stepMaterials[i] != null)
                    stepMaterials[i].text = MaterialsLine(comps);
            }
        }

        /// <summary>Top-N materials of a step with grams; remainder → "other".</summary>
        private static string MaterialsLine(List<DPP.Models.Component> comps)
        {
            var entries = new List<(string label, float w)>();
            foreach (var c in comps)
            {
                if (c.material_breakdown != null && c.material_breakdown.Count > 0)
                    foreach (var m in c.material_breakdown)
                        entries.Add((m.material, m.weight_g));
                else
                    entries.Add((ShortLabel.TryGetValue(c.id, out var l) ? l : (c.name ?? c.id), c.weight_g));
            }

            entries = entries.OrderByDescending(e => e.w).ToList();
            var shown = entries.Take(MaxMaterialsShown).ToList();
            float rest = entries.Skip(MaxMaterialsShown).Sum(e => e.w);
            if (rest > 0.05f) shown.Add(("other", rest));

            return string.Join(" · ", shown.Select(e => $"{e.label} {FormatG(e.w)}"));
        }

        // ---- called by StepFlowController when the flow finishes ----
        public void SetSession(int elapsedSeconds, int stepsCompleted, int totalSteps, int[] stepSplits)
        {
            _elapsedS = elapsedSeconds;
            _stepsCompleted = stepsCompleted;
            _splits = stepSplits ?? System.Array.Empty<int>();

            if (timeValue != null)
                timeValue.text = $"{elapsedSeconds / 60} min {elapsedSeconds % 60:00} s";

            int longest = -1, longestVal = -1;
            for (int i = 0; i < _splits.Length; i++)
                if (_splits[i] > longestVal) { longestVal = _splits[i]; longest = i; }

            int rows = RowCount();
            for (int i = 0; i < rows; i++)
            {
                bool has = i < _splits.Length;
                if (stepTimes != null && i < stepTimes.Length && stepTimes[i] != null)
                {
                    stepTimes[i].text = has ? $"{_splits[i] / 60}:{_splits[i] % 60:00}" : "—";
                    stepTimes[i].color = i == longest ? TimeGold : TimeNormal;
                    stepTimes[i].fontStyle = i == longest ? FontStyles.Bold : FontStyles.Normal;
                }
                if (longestTags != null && i < longestTags.Length && longestTags[i] != null)
                    longestTags[i].SetActive(i == longest && _splits.Length > 1);
            }

            ResetState();
        }

        private int RowCount() => stepTitles != null ? stepTitles.Length : 0;

        private static string FormatG(float w)
        {
            string num = w >= 2f
                ? Mathf.RoundToInt(w).ToString(CultureInfo.InvariantCulture)
                : w.ToString("0.0", CultureInfo.InvariantCulture);
            return $"{num} g";
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
                // Visible failure — a silent return here looked like a frozen
                // button on device (2026-07-20). Button stays enabled for retry.
                Debug.LogWarning("[CompletionSummaryView] Missing client or data — cannot send report.");
                if (actionLabel != null) actionLabel.text = "No data — retry";
                return;
            }

            string productId = _data.product_id ?? "vcu_001";
            string ids = _data.components != null
                ? string.Join(",", _data.components.Select(c => $"\"{c.id}\""))
                : "";
            string splits = string.Join(",", _splits.Select(s => s.ToString(CultureInfo.InvariantCulture)));
            float? co2 = _data.environmental?.recovery_potential?.total_avoidable_kg;

            var sb = new StringBuilder();
            sb.Append("{");
            sb.Append($"\"product_id\":\"{productId}\",");
            sb.Append($"\"timestamp\":\"{System.DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}\",");
            sb.Append($"\"elapsed_s\":{_elapsedS},");
            sb.Append($"\"steps_completed\":{_stepsCompleted},");
            sb.Append($"\"step_times_s\":[{splits}],");
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
            // Loop routine (2026-07-21): report stored → offer the next cycle.
            if (nextModal != null) nextModal.SetActive(true);
            Debug.Log("[CompletionSummaryView] Recovery report stored by backend.");
        }

        /// <summary>Post-report modal: start a fresh scan cycle.</summary>
        public void OnScanNewProduct()
        {
            if (nextModal != null) nextModal.SetActive(false);
            if (_scanController == null)
                _scanController = FindFirstObjectByType<QRScanController>(FindObjectsInactive.Include);
            if (_scanController != null) _scanController.BeginNewScan();
            else if (router != null) router.ShowMainPage();   // QR entry not built — degrade gracefully
        }

        /// <summary>Post-report modal: back to the main menu with the current unit.</summary>
        public void OnMainMenu()
        {
            if (nextModal != null) nextModal.SetActive(false);
            if (router != null) router.ShowMainPage();
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
            if (actionLabel != null) actionLabel.text = "Send dismantling report";
            if (actionChevron != null) actionChevron.SetActive(true);
            if (actionButton != null) actionButton.interactable = true;
            if (sentMessage != null) sentMessage.gameObject.SetActive(false);
            if (nextModal != null) nextModal.SetActive(false);
        }
    }
}
