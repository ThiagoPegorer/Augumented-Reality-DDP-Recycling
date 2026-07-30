using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// ReBuilt v2.0 — "CONTINUE TO DISASSEMBLY?" gate (Miro journey v4, spec 14).
    ///
    ///   DIGITAL MODEL EXPLORATION ──Continue──▶ [ gate ]
    ///                                            ├─ Continue → disassembly intro
    ///                                            └─ Quit     → Welcome canvas
    ///
    /// ⚠ ASYMMETRY BY DESIGN (Thiago, 2026-07-29): every other Back edge in
    /// RBv2.0 moves exactly one step. This one leaves the product session
    /// entirely, so the button is labelled **Quit**, not Back — a participant
    /// must not read "No" as "one screen back" and lose their place.
    ///
    /// Own world-space canvas + GraphicRaycaster + grabber bar (00 §4 on-plane
    /// rule), and it recenters in front of the user on open — the participant
    /// has just been moving their hands around the model and may have drifted.
    ///
    /// Quit routes through <see cref="ScreenRouter.ShowDppCanva"/> BEFORE
    /// showing Welcome. That is not cosmetic: the exploded action zone is a
    /// separate ROOT canvas, so hiding the main panel alone would leave the 3D
    /// model floating in the room. Going through the router deactivates it.
    /// </summary>
    public class ContinueGate : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private WelcomeController welcome;

        [Header("Placement")]
        [Tooltip("Drop the gate in front of the user each time it opens.")]
        [SerializeField] private bool recenterOnShow = true;
        [SerializeField] private float spawnDistance = 0.7f;
        [SerializeField] private float spawnHeightOffset = -0.05f;

        /// <summary>Called by the Continue button on the exploration screen.</summary>
        public void Show()
        {
            gameObject.SetActive(true);
            if (recenterOnShow) RecenterInFront();
            Debug.Log("[ContinueGate] Gate shown.");
        }

        /// <summary>CONTINUE → the disassembly intro. The timer still starts at
        /// 'Start disassembly' inside the intro, not here.</summary>
        public void Continue()
        {
            gameObject.SetActive(false);
            if (router == null)
            {
                Debug.LogError("[ContinueGate] No ScreenRouter wired — cannot open the disassembly intro.");
                return;
            }
            router.ShowDisassembly();
        }

        /// <summary>QUIT → leave the product session and return to Welcome.</summary>
        public void Quit()
        {
            gameObject.SetActive(false);

            // Drop the action zone first (root canvas — Welcome cannot hide it).
            if (router != null) router.ShowDppCanva();

            if (welcome == null)
            {
                Debug.LogError("[ContinueGate] No WelcomeController wired — cannot return to Welcome.");
                return;
            }
            welcome.ShowWelcome();
        }

        private void RecenterInFront()
        {
            var head = Camera.main;
            if (head == null) return;

            Vector3 fwd = head.transform.forward;
            fwd.y = 0f;
            if (fwd.sqrMagnitude < 1e-6f) return;
            fwd.Normalize();

            transform.position = head.transform.position + fwd * spawnDistance + Vector3.up * spawnHeightOffset;
            transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
        }
    }
}
