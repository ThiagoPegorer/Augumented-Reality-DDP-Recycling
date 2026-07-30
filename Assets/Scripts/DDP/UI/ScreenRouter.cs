using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Switches between the DPP screens that live under the instruction panel
    /// canvas. Exactly one screen is active at a time.
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
    /// CLEANUP 2026-07-30: the two RBv1.0 screen fields and the tab-pill entry
    /// point were removed. Their only callers were the two-card Main Page and the
    /// tab pills, neither of which exists any more. If the console ever reports a
    /// missing method on this class, some serialized UnityEvent still points at
    /// the old API — run RBv2_0/Tools/Clean RBv1.0 leftovers.
    /// </summary>
    public class ScreenRouter : MonoBehaviour
    {
        [Header("Passport screens (built by RBv2_0/7)")]
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
        /// The app's main screen after a scan — the DPP Canva. Kept under this
        /// name because FirstRunPrompt calls it by name; identical to
        /// <see cref="ShowDppCanva"/>.
        /// </summary>
        public void ShowMainPage() => ShowDppCanva();

        /// <summary>DPP Canva — product info. The app's main screen.</summary>
        public void ShowDppCanva()
        {
            if (dppCanva == null)
            {
                Debug.LogWarning("[ScreenRouter] DPP Canva not assigned — run RBv2_0/7.");
                return;
            }
            Show(dppCanva, "DPP Canva");
        }

        /// <summary>Digital Model Exploration — LCA overview + the action zone (RBv2.0).</summary>
        public void ShowModelExploration()
        {
            if (modelExploration == null)
            {
                Debug.LogWarning("[ScreenRouter] Model exploration not assigned — run RBv2_0/7.");
                return;
            }
            Show(modelExploration, "Model exploration");
        }

        public void ShowDisassembly()
        {
            if (disassemblyIntro == null)
            {
                Debug.LogWarning("[ScreenRouter] Disassembly intro not assigned — run RBv2_0/4.");
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
                Debug.LogWarning("[ScreenRouter] Step flow not assigned — run RBv2_0/5.");
                return;
            }
            Show(stepFlow, "Step flow");
        }

        /// <summary>Step 5 'Finish &amp; see summary' → completion summary (09).</summary>
        public void ShowCompletion()
        {
            if (completionSummary == null)
            {
                Debug.LogWarning("[ScreenRouter] Completion summary not assigned — run RBv2_0/6.");
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
