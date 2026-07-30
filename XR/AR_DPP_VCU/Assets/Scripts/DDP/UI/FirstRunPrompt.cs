using System;
using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// ReBuilt v2.0 — "First time using ReBuilt?" prompt (journey node
    /// FIRST TIME USING THE APP?, spec 12).
    ///
    ///   QR CODE SCAN SUCCESSFULLY? = YES → FIRST TIME USING THE APP?
    ///        YES → tutorial routine (RBv2.0 block 4)
    ///        NO  → OPEN MAIN CANVA
    ///
    /// This is a USER question, not an automatic branch: it is asked after
    /// every successful scan, inside the kiosk cycle, so every participant is
    /// offered the tutorial regardless of who used the headset before them.
    ///
    /// Own world-space canvas + GraphicRaycaster + grabber bar (00 §4 on-plane
    /// rule) so it stays click-accurate wherever the user drags it.
    ///
    /// BLOCK 1 BEHAVIOUR: the tutorial does not exist yet, so both answers land
    /// on the main canvas. <see cref="TutorialRequested"/> still fires on YES —
    /// block 4 subscribes to it without touching this file.
    /// </summary>
    public class FirstRunPrompt : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private GameObject mainCanvasRoot;
        [SerializeField] private ScreenRouter router;

        [Header("Placement")]
        [Tooltip("Drop the prompt in front of the user each time it opens (it is asked once per scan, and the user may have wandered).")]
        [SerializeField] private bool recenterOnShow = true;
        [SerializeField] private float spawnDistance = 0.7f;
        [SerializeField] private float spawnHeightOffset = -0.05f;

        /// <summary>Raised when the user answers YES. Block 4 (tutorial routine) subscribes.</summary>
        public event Action TutorialRequested;

        /// <summary>Called by QRScanController once the passport has loaded.</summary>
        public void Show()
        {
            gameObject.SetActive(true);
            if (recenterOnShow) RecenterInFront();
            Debug.Log("[FirstRun] Prompt shown.");
        }

        /// <summary>YES — "Yes, show me". Block 4 will route to the tutorial.</summary>
        public void ChooseYes()
        {
            Debug.Log("[FirstRun] YES — tutorial requested.");
            TutorialRequested?.Invoke();

            // Block 1: no tutorial yet — fall through to the main canvas.
            // Block 4 replaces this fall-through with the tutorial hand-off.
            OpenMainCanvas();
        }

        /// <summary>NO — "No, skip" → straight to the main canvas.</summary>
        public void ChooseNo()
        {
            Debug.Log("[FirstRun] NO — skipping the tutorial.");
            OpenMainCanvas();
        }

        private void OpenMainCanvas()
        {
            gameObject.SetActive(false);
            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(true);
            if (router != null) router.ShowMainPage();
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
