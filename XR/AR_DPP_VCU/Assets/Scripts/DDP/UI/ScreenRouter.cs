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
    /// <summary>Who is using the app this session (RBv2.1 spec 03). `None` until
    /// the user chooses — the screen cannot be skipped.</summary>
    public enum StakeholderMode { None, ProductUser, Recycler }

    public class ScreenRouter : MonoBehaviour
    {
        [Header("Passport screens — DPP page + certificates by RBv2_1/8, legacy exploration by RBv2_0/Legacy")]
        [Tooltip("RBv2.1 spec 03 — the role fork, opened by a successful scan. Built by RBv2_1/7.")]
        [SerializeField] private GameObject stakeholderDecision;
        [Tooltip("Product info — the app's main screen in RBv2.0. Back goes to the Welcome canvas.")]
        [SerializeField] private GameObject dppCanva;
        [Tooltip("Life cycle overview + the exploded action zone. Back goes to the DPP Canva.")]
        [SerializeField] private GameObject modelExploration;

        [Tooltip("RBv2.1 spec 04 §5 — Certificates & safety. A full SCREEN, not a modal: it covers " +
                 "the whole panel anyway, and an overlay sharing this canvas plane with live controls " +
                 "lets clicks resolve to the buttons underneath it. Built by RBv2_1/8.")]
        [SerializeField] private GameObject certificates;

        [Tooltip("RBv2.1 spec 04c — the Product specifications tab as a SIBLING screen on this canvas. " +
                 "Set by RBv2_1/9 and CLEARED by RBv2_1_1/2, which re-parents the page into the super " +
                 "panel's data canvas. Non-null only while the rig does not exist.")]
        [SerializeField] private GameObject productSpecs;

        [Header("RBv2.1.1 — the super panel (spec 04 v2)")]
        [Tooltip("The three-canvas rig, a SCENE ROOT rather than a child of this canvas. Built by " +
                 "RBv2_1_1/1. When it is assigned it replaces the flat DPP page: ShowDppCanva() routes " +
                 "here so every existing caller keeps working unchanged.")]
        [SerializeField] private GameObject dppSuperPanel;

        [Tooltip("Root the model re-parents into when unlocked. A sibling of the rig, so a freed model " +
                 "stops following the panels. Hidden with the rig.")]
        [SerializeField] private GameObject freeModelRoot;

        [Tooltip("This canvas's own grabber bar. Hidden while the rig is up, because the rig carries its " +
                 "own and two live grab bars on one screen is a coin toss for the user.")]
        [SerializeField] private GameObject panelGrabber;

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

        // ---------------- RBv2.1: the stakeholder role ----------------

        /// <summary>Which audience is using the app this session. Read by the DPP
        /// Canva to decide whether the disassembly route is offered at all.</summary>
        public StakeholderMode Mode { get; private set; } = StakeholderMode.None;

        /// <summary>Set by the role cards (spec 03).</summary>
        public void SetStakeholderMode(StakeholderMode mode)
        {
            Mode = mode;
            Debug.Log($"[ScreenRouter] Stakeholder mode = {mode}.");
        }

        /// <summary>Opened by a successful scan (spec 02 section 4), and re-entered by
        /// Back from the DPP Canva so a wrong tap costs one press, not a whole session.
        ///
        /// Clearing the mode HERE is what makes the kiosk cycle safe: every new scan
        /// passes through this screen, so participant 2 can never inherit participant
        /// 1's role, and no extra reset call has to be remembered anywhere else.</summary>
        public void ShowStakeholder()
        {
            Mode = StakeholderMode.None;
            if (stakeholderDecision == null)
            {
                // Never strand a participant on a missing screen — fall through to
                // the passport, which is what both roles see first anyway.
                Debug.LogWarning("[ScreenRouter] Stakeholder screen not assigned — run RBv2_1/7. Opening the passport directly.");
                ShowDppCanva();
                return;
            }
            Show(stakeholderDecision, "Stakeholder decision");
        }


        /// <summary>Certificates &amp; safety (spec 04 §5). Opened by the compliance
        /// badge; its X returns to the DPP page.</summary>
        public void ShowCertificates()
        {
            if (certificates == null)
            {
                Debug.LogWarning("[ScreenRouter] Certificates page not assigned — run RBv2_1/8.");
                return;
            }
            Show(certificates, "Certificates & safety");
        }

        /// <summary>Product specifications (spec 04c). Opened by tab 1's "+"; its
        /// Back walks Drawing → Detail → Parts → Identity and only then returns
        /// here, so a four-level drill costs one press per level.</summary>
        public void ShowProductSpecs()
        {
            if (productSpecs == null)
            {
                Debug.LogWarning("[ScreenRouter] Product specs page not assigned — run RBv2_1/9.");
                return;
            }
            Show(productSpecs, "Product specifications");
        }

        /// <summary>
        /// The passport. RBv2.1.1: if the super panel rig exists it wins, and every
        /// caller of this method — the stakeholder cards, the certificates X, the
        /// first-run prompt — reaches the new screen with no edit. The flat v1 page
        /// stays in the scene and is reachable again the moment the rig reference
        /// is cleared, which is the whole rollback plan.
        /// </summary>
        public void ShowDppCanva()
        {
            if (dppSuperPanel != null) { Show(dppSuperPanel, "DPP super panel"); return; }
            if (dppCanva == null)
            {
                Debug.LogWarning("[ScreenRouter] DPP page not assigned — run RBv2_1/8.");
                return;
            }
            Show(dppCanva, "DPP Canva");
        }

        /// <summary>Explicitly the super panel, ignoring the v1 fallback.</summary>
        public void ShowSuperPanel()
        {
            if (dppSuperPanel == null)
            {
                Debug.LogWarning("[ScreenRouter] Super panel not assigned — run RBv2_1_1/1.");
                return;
            }
            Show(dppSuperPanel, "DPP super panel");
        }

        /// <summary>Digital Model Exploration — LCA overview + the action zone (RBv2.0).</summary>
        public void ShowModelExploration()
        {
            if (modelExploration == null)
            {
                Debug.LogWarning("[ScreenRouter] Model exploration not assigned — run RBv2_0/Legacy.");
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
            // RBv2.1 RULE — EVERY panel screen belongs in this list, no exceptions.
            // The stakeholder screen was missing from it once and cost a day. It is a full-panel child of DPPPanelCanvas carrying two
            // 290x170 card buttons, so once shown it stayed ACTIVE on top of every
            // screen that followed and kept stealing raycasts. Both cards call
            // ShowDppCanva(), which is exactly the reported "every button sends me
            // back to the DPP canvas". Every panel screen must be listed here.
            DeactivateUnless(stakeholderDecision, target);
            DeactivateUnless(certificates,      target);
            DeactivateUnless(productSpecs,      target);
            DeactivateUnless(dppCanva,          target);
            DeactivateUnless(dppSuperPanel,     target);
            DeactivateUnless(modelExploration,  target);
            DeactivateUnless(disassemblyIntro,  target);
            DeactivateUnless(stepFlow,          target);
            DeactivateUnless(completionSummary, target);

            var zoneOwner = ZoneOwner;
            if (target != zoneOwner) SetActiveSafe(explodedCanvas, false);

            // The rig is a scene ROOT, not a child of this canvas, so it needs its
            // own companions handled: the freed-model root goes with it, and this
            // canvas's grabber steps aside so only one grab bar is ever live.
            bool rigUp = dppSuperPanel != null && target == dppSuperPanel;
            if (!rigUp) SetActiveSafe(freeModelRoot, false);
            SetActiveSafe(panelGrabber, !rigUp);

            // PASS 2 — activate the target (and its companion canvas).
            SetActiveSafe(target, true);
            if (rigUp) SetActiveSafe(freeModelRoot, true);
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
