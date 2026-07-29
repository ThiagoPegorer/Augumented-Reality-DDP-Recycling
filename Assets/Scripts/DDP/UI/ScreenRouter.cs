using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Switches between the DPP screens that live under the instruction panel
    /// canvas. Exactly one screen is active at a time.
    ///
    /// RBv1.0 (tab bar): Main Page → Informations | Disassembly, both reachable
    /// from the tab pills at any time.
    ///
    /// RBv2.0 (Miro journey v4, tab bar removed): a single linear path with a
    /// one-step-back hierarchy —
    ///
    ///   Welcome → scan → [first run?] → DPP CANVA
    ///                                     ├─Back→ Welcome
    ///                                     └─Continue→ DIGITAL MODEL EXPLORATION
    ///                                                   ├─Back→ DPP Canva
    ///                                                   └─Continue→ gate modal
    ///                                                        ├─Quit→ Welcome
    ///                                                        └─Continue→ intro → steps → summary
    ///
    /// The RBv1.0 screens (mainPage, informationTab) are kept as fields so an
    /// existing scene still deactivates them correctly, but nothing routes to
    /// them once <see cref="dppCanva"/> is assigned.
    /// </summary>
    public class ScreenRouter : MonoBehaviour
    {
        [Header("Screen roots (assign as phases are built)")]
        [Tooltip("RBv1.0 two-card main page. Superseded by dppCanva in RBv2.0 — kept so it still gets deactivated.")]
        [SerializeField] private GameObject mainPage;
        [Tooltip("RBv1.0 Information tab. Superseded by dppCanva + modelExploration in RBv2.0.")]
        [SerializeField] private GameObject informationTab;

        [Header("RBv2.0 screens (Phase 8)")]
        [Tooltip("Product info — the app's main screen in RBv2.0. Back goes to the Welcome canvas.")]
        [SerializeField] private GameObject dppCanva;
        [Tooltip("Life cycle overview + the exploded action zone. Back goes to the DPP Canva.")]
        [SerializeField] private GameObject modelExploration;

        [Header("Disassembly")]
        [SerializeField] private GameObject disassemblyIntro;
        [SerializeField] private GameObject stepFlow;
        [SerializeField] private GameObject completionSummary;

        [Tooltip("Separate world-space exploded-view canvas (root object, not a child of this canvas).")]
        [SerializeField] private GameObject explodedCanvas;

        [Tooltip("RBv2.0: ON = the zone is paired with Digital Model Exploration. " +
                 "OFF = RBv1.0 behaviour (the zone is paired with the step flow). " +
                 "Flip this to restore the zone inside the disassembly steps without a code change.")]
        [SerializeField] private bool zoneFollowsExploration = true;

        /// <summary>The screen the exploded action zone is shown alongside.</summary>
        private GameObject ZoneOwner => zoneFollowsExploration ? modelExploration : stepFlow;

        // =================================================================
        // Public navigation
        // =================================================================

        /// <summary>
        /// The app's main screen after a scan. RBv2.0 → DPP Canva; falls back to
        /// the RBv1.0 Main Page when Phase 8 has not been run. Kept under this
        /// name because FirstRunPrompt and the RBv1.0 buttons call it by name.
        /// </summary>
        public void ShowMainPage()
        {
            if (dppCanva != null) { Show(dppCanva, "DPP Canva"); return; }
            Show(mainPage, "Main page");
        }

        /// <summary>DPP Canva — product info (RBv2.0). Explicit alias of ShowMainPage.</summary>
        public void ShowDppCanva()
        {
            if (dppCanva == null)
            {
                Debug.Log("[ScreenRouter] DPP Canva not built yet (phase 8).");
                return;
            }
            Show(dppCanva, "DPP Canva");
        }

        /// <summary>Digital Model Exploration — LCA overview + the action zone (RBv2.0).</summary>
        public void ShowModelExploration()
        {
            if (modelExploration == null)
            {
                Debug.Log("[ScreenRouter] Model exploration not built yet (phase 8).");
                return;
            }
            Show(modelExploration, "Model exploration");
        }

        /// <summary>RBv1.0 Information tab. In RBv2.0 this resolves to the DPP Canva.</summary>
        public void ShowInformations()
        {
            if (dppCanva != null) { Show(dppCanva, "DPP Canva"); return; }
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
        /// Start button on the Disassembly intro → the step flow. In RBv1.0 this
        /// also raised the exploded canvas; in RBv2.0 the zone stays with the
        /// exploration screen (see <see cref="zoneFollowsExploration"/>).
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

        /// <summary>Step 5 'Finish &amp; see summary' → completion summary (09).</summary>
        public void ShowCompletion()
        {
            if (completionSummary == null)
            {
                Debug.Log("[ScreenRouter] Completion summary not built yet (phase 5).");
                return;
            }
            Show(completionSummary, "Completion summary");
        }

        // =================================================================

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
            DeactivateUnless(dppCanva,          target);
            DeactivateUnless(modelExploration,  target);
            DeactivateUnless(disassemblyIntro,  target);
            DeactivateUnless(stepFlow,          target);
            DeactivateUnless(completionSummary, target);

            var zoneOwner = ZoneOwner;
            if (target != zoneOwner) SetActiveSafe(explodedCanvas, false);

            // PASS 2 — activate the target (and its companion canvas).
            SetActiveSafe(target, true);
            if (target == zoneOwner) SetActiveSafe(explodedCanvas, true);

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
