using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// ReBuilt v2.0 — Welcome canvas (journey node OPEN APP, spec 12).
    ///
    /// The app's first screen and the universal return target of the journey:
    ///
    ///   LAUNCH APP → WELCOME ──CONTINUE BUTTON──→ SCAN QR CODE
    ///                       └──CLOSE APP BUTTON─→ quit
    ///
    /// Owns app entry so that QRScanController no longer starts scanning at
    /// launch (its waitForWelcome flag). Blocks 2–3 of RBv2.0 call
    /// <see cref="ShowWelcome"/> for the DPP-canvas Back edge and the
    /// end-of-process loop.
    ///
    /// Lives on its own world-space canvas ("WelcomeCanvas") with its own
    /// GraphicRaycaster and grabber bar — required by the on-plane rule in
    /// DPP_UI_Specs/00 §4 for any independently-moving UI group.
    /// </summary>
    public class WelcomeController : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [Tooltip("Root of the main 640x430 canvas — hidden until a passport is loaded.")]
        [SerializeField] private GameObject mainCanvasRoot;

        [Tooltip("Scan screen owner. Continue hands off to BeginNewScan().")]
        [SerializeField] private QRScanController scanner;

        [Header("Behaviour")]
        [Tooltip("Show the welcome canvas at launch. OFF = legacy entry (scanner owns launch).")]
        [SerializeField] private bool showOnStart = true;

        private void Start()
        {
            // Whoever runs first wins the same result: nothing but Welcome is up.
            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(false);

            if (!showOnStart)
            {
                gameObject.SetActive(false);
                return;
            }

            ShowWelcome();
        }

        /// <summary>
        /// Bring the welcome canvas back up and hide everything else. Called at
        /// launch, and (RBv2.0 blocks 2–3) by the DPP-canvas Back button and the
        /// "END OF PROCESS → scan a new QR code" loop.
        /// </summary>
        public void ShowWelcome()
        {
            gameObject.SetActive(true);
            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(false);
            Debug.Log("[Welcome] Welcome canvas shown.");
        }

        /// <summary>CONTINUE BUTTON → hand off to the QR scan screen.</summary>
        public void ContinueToScan()
        {
            gameObject.SetActive(false);

            if (scanner == null)
            {
                Debug.LogError("[Welcome] No QRScanController wired — cannot start the scan.");
                return;
            }
            scanner.BeginNewScan();
        }

        /// <summary>CLOSE APP BUTTON → quit. No-ops harmlessly in the Editor.</summary>
        public void CloseApp()
        {
            Debug.Log("[Welcome] Close app requested.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
