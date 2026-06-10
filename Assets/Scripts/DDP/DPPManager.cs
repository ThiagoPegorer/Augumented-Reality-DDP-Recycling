using UnityEngine;
using Newtonsoft.Json;
using DPP.Models;

namespace DPP
{
    /// <summary>
    /// Orchestrates the DPP fetch flow:
    ///   - Calls DPPClient to fetch JSON from the FastAPI backend
    ///   - Deserializes via Newtonsoft.Json (handles nullable fields like co2_footprint_kg)
    ///   - Hands the parsed DPPData to DPPDashboard for display
    ///
    /// In Phase 1 (colloquium prototype), `fetchOnStart` with a hardcoded
    /// `testProductId` lets us verify the backend ↔ Unity pipeline in the Editor
    /// before QR scanning is wired up.
    ///
    /// In Phase 2 (with QR), the QR scanner script will call FetchAndPopulate(productId)
    /// when a QR is detected.
    /// </summary>
    public class DPPManager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private DPPClient client;

        [Tooltip("Legacy colloquium dashboard. Optional — superseded by the Canva-design screens.")]
        [SerializeField] private DPPDashboard dashboard;

        [Tooltip("Screen 01 — Main Page view (Canva design). Populated with serial + step count.")]
        [SerializeField] private DPP.UI.MainPageView mainPage;

        [Tooltip("Screen 02 — Information tab view (Canva design). Populated with full v0.3 passport data.")]
        [SerializeField] private DPP.UI.InfoTabView infoTab;

        [Header("Editor test")]
        [Tooltip("On Start, fetch this product_id without waiting for a QR scan. Useful in Editor.")]
        [SerializeField] private bool fetchOnStart = true;
        [SerializeField] private string testProductId = "vcu_001";

        void Start()
        {
            if (fetchOnStart)
            {
                FetchAndPopulate(testProductId);
            }
        }

        /// <summary>
        /// Fetch DPP for the given product_id and populate the dashboard.
        /// Call this from the QR scanner once a QR is decoded.
        /// </summary>
        public void FetchAndPopulate(string productId)
        {
            if (client == null)
            {
                Debug.LogError("[DPPManager] DPPClient reference is missing in the Inspector.");
                return;
            }
            if (dashboard == null && mainPage == null)
            {
                Debug.LogWarning("[DPPManager] No UI assigned (dashboard/mainPage) — fetch will run but nothing will display.");
            }

            StartCoroutine(client.GetDPP(productId, OnDPPSuccess, OnDPPError));
        }

        private void OnDPPSuccess(string json)
        {
            try
            {
                DPPData data = JsonConvert.DeserializeObject<DPPData>(json);
                if (data == null)
                {
                    Debug.LogError("[DPPManager] Deserialized DPP is null.");
                    return;
                }
                if (dashboard != null) dashboard.Populate(data);
                if (mainPage != null)  mainPage.Populate(data);
                if (infoTab != null)   infoTab.Populate(data);
                Debug.Log($"[DPPManager] Populated UI for product_id={data.product_id}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[DPPManager] Failed to parse DPP JSON: {ex.Message}");
            }
        }

        private void OnDPPError(string error)
        {
            Debug.LogError($"[DPPManager] {error}");
        }
    }
}
