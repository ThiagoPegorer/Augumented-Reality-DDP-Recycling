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
        [SerializeField] private DPPDashboard dashboard;

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
            if (client == null || dashboard == null)
            {
                Debug.LogError("[DPPManager] DPPClient or DPPDashboard reference is missing in the Inspector.");
                return;
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
                dashboard.Populate(data);
                Debug.Log($"[DPPManager] Populated dashboard for product_id={data.product_id}");
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
