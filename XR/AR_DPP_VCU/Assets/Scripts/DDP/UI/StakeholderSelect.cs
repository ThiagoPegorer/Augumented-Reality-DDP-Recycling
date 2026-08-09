using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1 — 03: STAKEHOLDER DECISION. The fork that makes ReBuilt one app for
    /// two audiences (routine `Routine_RB2_1.pdf`, spec `03_stakeholder_decision.md`).
    ///
    ///   Product user → passport only. No dismantling.
    ///   Recycler     → passport, then the guided disassembly.
    ///
    /// Both roles land on the SAME DPP Canva. The difference is one button's
    /// visibility, driven by <see cref="ScreenRouter.Mode"/> — building two DPP
    /// canvases would double every future DPP edit and guarantee they drift apart.
    ///
    /// The role is stored on the router, not here and not in a static: the screen
    /// can be re-entered (Back from the DPP Canva) to change the choice, and a
    /// static would survive a scene reload and leak participant 1's role into
    /// participant 2's session.
    /// </summary>
    public class StakeholderSelect : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private WelcomeController welcome;

        /// <summary>LEFT card — read the passport, no dismantling.</summary>
        public void ChooseProductUser() => Choose(StakeholderMode.ProductUser);

        /// <summary>RIGHT card — passport, then the guided teardown.</summary>
        public void ChooseRecycler() => Choose(StakeholderMode.Recycler);

        private void Choose(StakeholderMode mode)
        {
            if (router == null)
            {
                Debug.LogError("[Stakeholder] No ScreenRouter wired — cannot open the passport.");
                return;
            }
            router.SetStakeholderMode(mode);
            router.ShowDppCanva();
        }

        /// <summary>Red pill, bottom-left — leave the SESSION from here.
        ///
        /// Added on review (Thiago, 2026-08-04): before this button the screen had
        /// NO exit at all. Both cards led forward, so a participant who scanned the
        /// wrong unit, or simply wanted out, had nowhere to go.
        ///
        /// 2026-08-05: `Close app` became `Quit` and now returns to the Welcome
        /// canvas instead of killing the process. In a kiosk loop, quitting the app
        /// ends the study session for everyone behind the participant; quitting to
        /// Welcome ends it for one. `Application.Quit` still lives on the Welcome
        /// screen's own Close app, which is where an operator would use it.
        /// Naming follows 00 §5: "an edge that leaves the session says Quit".</summary>
        public void Quit()
        {
            Debug.Log("[Stakeholder] Quit to Welcome.");
            if (welcome != null) welcome.ShowWelcome();
            else Debug.LogWarning("[Stakeholder] No WelcomeController wired — run RBv2_1_1/04.");
        }
    }
}
