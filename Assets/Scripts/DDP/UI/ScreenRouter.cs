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
        [SerializeField] private GameObject stepFlow;
        [SerializeField] private GameObject completionSummary;

        [Tooltip("Separate world-space exploded-view canvas — active only during the step flow.")]
        [SerializeField] private GameObject explodedCanvas;

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

        /// <summary>
        /// Start button on the Disassembly intro → the two-canvas step flow
        /// (instruction screen + separate exploded-view canvas).
        /// </summary>
        public void ShowStepFlow()
        {
            if (stepFlow == null)
            {
                Debug.Log("[ScreenRouter] Step flow not built yet (phase 4).");
                return;
            }
            Show(stepFlow, "Step flow");
        }

        /// <summary>Step 5 'Finish & see summary' → completion summary (09).</summary>
        public void ShowCompletion()
        {
            if (completionSummary == null)
            {
                Debug.Log("[ScreenRouter] Completion summary not built yet (phase 5).");
                return;
            }
            Show(completionSummary, "Completion summary");
        }

        private void Show(GameObject target, string label)
        {
            if (target == null)
            {
                Debug.LogWarning($"[ScreenRouter] '{label}' is not assigned.");
                return;
            }

            // PASS 1 — deactivate every non-target FIRST. Outgoing screens
            // release shared resources in OnDisable (the preview camera, the
            // model's pose via ResetInstant). Activating the incoming screen
            // before that (old behaviour) let e.g. the intro's loop claim the
            // camera and then the step flow's OnDisable switched it back off —
            // the intro animation appeared dead after Back from step 1.
            DeactivateUnless(mainPage,          target);
            DeactivateUnless(informationTab,    target);
            DeactivateUnless(disassemblyIntro,  target);
            DeactivateUnless(stepFlow,          target);
            DeactivateUnless(completionSummary, target);
            if (target != stepFlow) SetActiveSafe(explodedCanvas, false);

            // PASS 2 — activate the target (and its companion canvas).
            SetActiveSafe(target, true);
            if (target == stepFlow) SetActiveSafe(explodedCanvas, true);

            Debug.Log($"[ScreenRouter] Showing {label}.");
        }

        private static void DeactivateUnless(GameObject go, GameObject target)
        {
            if (go != null && go != target) SetActiveSafe(go, false);
        }

        private static void SetActiveSafe(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active) go.SetActive(active);
        }
    }
}
