using UnityEngine;
using TMPro;
using DPP.Models;

namespace DPP
{
    /// <summary>
    /// Tab controller for the DPP world-anchored AR dashboard.
    /// Phase 1 (colloquium): two tabs only — Info + Explosion View.
    /// Phase 2 (thesis): expand with Materials/BOM, Environmental, End-of-life, AI Assistant.
    ///
    /// Uses TextMeshPro for crisp text rendering at AR scale.
    /// </summary>
    public class DPPDashboard : MonoBehaviour
    {
        [Header("Tab roots")]
        [SerializeField] private GameObject infoTab;
        [SerializeField] private GameObject explosionTab;

        [Header("Info tab fields (TMP_Text)")]
        [SerializeField] private TMP_Text manufacturerText;
        [SerializeField] private TMP_Text modelText;
        [SerializeField] private TMP_Text serialText;
        [SerializeField] private TMP_Text productionDateText;

        [Header("Explosion view")]
        [SerializeField] private ExplosionController explosionController;

        public void ShowInfoTab()
        {
            if (infoTab != null) infoTab.SetActive(true);
            if (explosionTab != null) explosionTab.SetActive(false);
        }

        public void ShowExplosionTab()
        {
            if (infoTab != null) infoTab.SetActive(false);
            if (explosionTab != null) explosionTab.SetActive(true);
        }

        /// <summary>
        /// Populate the Info tab fields from a fetched DPP payload.
        /// </summary>
        public void Populate(DPPData data)
        {
            if (data == null || data.identity == null) return;

            if (manufacturerText != null)   manufacturerText.text   = data.identity.manufacturer;
            if (modelText != null)          modelText.text          = data.identity.model;
            if (serialText != null)         serialText.text         = data.identity.serial_number;
            if (productionDateText != null) productionDateText.text = data.identity.production_date;
        }

        public void TriggerExplosion()
        {
            if (explosionController != null) explosionController.Explode();
        }

        public void TriggerCollapse()
        {
            if (explosionController != null) explosionController.Collapse();
        }
    }
}
