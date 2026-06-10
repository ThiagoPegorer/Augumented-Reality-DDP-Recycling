using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Switches between the DPP screens that live under the instruction panel
    /// canvas. Exactly one screen is active at a time.
    ///
    /// Build phases (Canva design implementation):
    ///   Phase 1: Main page (01)               — built
    ///   Phase 2: Information tab (02)         — pending
    ///   Phase 3: Disassembly intro (03)       — pending
    ///   Phase 4: Disassembly steps 1–5 (04–08)— pending
    ///   Phase 5: Completion summary (09)      — pending
    ///
    /// Screens not yet built may be left unassigned; navigation to them logs
    /// instead of breaking.
    /// </summary>
    public class ScreenRouter : MonoBehaviour
    {
        [Header("Screen roots (assign as phases are built)")]
        [SerializeField] private GameObject mainPage;
        [SerializeField] private GameObject informationTab;
        [SerializeField] private GameObject disassemblyIntro;

        public void ShowMainPage() => Show(mainPage, "Main page");

        public void ShowInformations()
        {
            if (informationTab == null)
            {
                Debug.Log("[ScreenRouter] Information tab not built yet (phase 2).");
                return;
            }
            Show(informationTab, "Information tab");
        }

        public void ShowDisassembly()
        {
            if (disassemblyIntro == null)
            {
                Debug.Log("[ScreenRouter] Disassembly intro not built yet (phase 3).");
                return;
            }
            Show(disassemblyIntro, "Disassembly intro");
        }

        private void Show(GameObject target, string label)
        {
            if (target == null)
            {
                Debug.LogWarning($"[ScreenRouter] '{label}' is not assigned.");
                return;
            }

            SetActiveSafe(mainPage,         target == mainPage);
            SetActiveSafe(informationTab,   target == informationTab);
            SetActiveSafe(disassemblyIntro, target == disassemblyIntro);

            Debug.Log($"[ScreenRouter] Showing {label}.");
        }

        private static void SetActiveSafe(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active) go.SetActive(active);
        }
    }
}
