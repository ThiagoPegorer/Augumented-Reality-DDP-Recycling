using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Two-level navigation inside the DPP Canva (spec 13; was spec 02 v3 §2):
    /// landing card grid ⇄ one full-page category modal at a time.
    /// Category cards call Open*(); the modal's back arrow calls Back().
    /// Entering the screen always shows the grid (state does not persist).
    ///
    /// CLEANUP 2026-07-30: the LCA modal field and its Open method were removed —
    /// in RBv2.0 the life cycle analysis is its own screen (spec 14).
    /// </summary>
    public class InfoTabRouter : MonoBehaviour
    {
        [SerializeField] private GameObject landing;
        [SerializeField] private GameObject identityModal;
        [SerializeField] private GameObject materialsModal;
        [SerializeField] private GameObject hazardModal;
        [SerializeField] private GameObject complianceModal;

        private void OnEnable() => Back();

        public void OpenIdentity()   => Open(identityModal);
        public void OpenMaterials()  => Open(materialsModal);
        public void OpenHazard()     => Open(hazardModal);
        public void OpenCompliance() => Open(complianceModal);
        public void Back()           => Open(null);

        private void Open(GameObject modal)
        {
            SetActiveSafe(landing, modal == null);
            SetActiveSafe(identityModal,   modal == identityModal   && modal != null);
            SetActiveSafe(materialsModal,  modal == materialsModal  && modal != null);
            SetActiveSafe(hazardModal,     modal == hazardModal     && modal != null);
            SetActiveSafe(complianceModal, modal == complianceModal && modal != null);
        }

        private static void SetActiveSafe(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active) go.SetActive(active);
        }
    }
}
