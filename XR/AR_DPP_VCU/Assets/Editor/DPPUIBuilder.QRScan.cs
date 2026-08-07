using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_1/2 builder — QR scan entry screen (spec 11 stages 3+4, approved
    /// mock qr_scan_screen_v1.svg 2026-07-21).
    ///
    /// Own world-space canvas "QRScanCanvas" (440×300 + grabber bar). Three
    /// state groups (scan / found / error) driven by QRScanController.
    /// Safe to re-run (destroys and rebuilds QRScanCanvas).
    ///
    /// RBv2.0 (2026-07-29): the 10 s "Continue with demo unit" fallback button
    /// is REMOVED — entry is QR-only. The operator-level kill-switch is
    /// QRScanController.scanOnStart. The screen is no longer shown at launch
    /// either: the Welcome canvas (RBv2_1/3) owns entry and calls BeginNewScan().
    /// Re-run RBv2_1/3 after RBv2_1/2 so the wiring is restored.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("RBv2_1/2 — QR scan screen", false, 2)]
        public static void Build2_QRScanScreen()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var mainGO = GameObject.Find("DPPPanelCanvas");
            if (mainGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_1/1 first.");
                return;
            }
            var router = mainGO.GetComponent<ScreenRouter>();
            var manager = Object.FindFirstObjectByType<DPPManager>();

            var old = GameObject.Find("QRScanCanvas");
            if (old != null) Undo.DestroyObjectImmediate(old);

            // ---- canvas ----
            var go = new GameObject("QRScanCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(440f, 300f);
            rt.position = ((RectTransform)mainGO.transform).position + new Vector3(0f, 0f, -0.05f);
            rt.localScale = Vector3.one * 0.001f;

            // v3 MINIMAL (user, 2026-07-21): NO background at all — fully
            // transparent canvas showing only the teal scanner brackets +
            // sweep and the texts. The user fits the physical QR inside the
            // brackets through open passthrough.
            var controller = go.AddComponent<QRScanController>();

            // ================= scan group =================
            var scan = Stretch("ScanGroup", rt);

            AddText(TL("Title", scan, 20, 30, 400, 28), "Scan the product", 21, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TL("Subtitle", scan, 20, 60, 400, 18), "Look at the QR code on the unit", 13, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.Center);

            // scan frame: 4 corners × 2 bars, teal
            var frame = TLCenter("Frame", scan, 220, 162, 190, 110);
            MakeCorner(frame, -95f,  55f,  1f, -1f);   // top-left
            MakeCorner(frame,  95f,  55f, -1f, -1f);   // top-right
            MakeCorner(frame, -95f, -55f,  1f,  1f);   // bottom-left
            MakeCorner(frame,  95f, -55f, -1f,  1f);   // bottom-right

            var sweep = CenterIn("Sweep", frame, 160f, 2.5f);
            AddImage(sweep, DPPSpriteFactory.Grip, DPPTheme.TealLight);

            var searching = AddText(TL("Searching", scan, 20, 232, 400, 16), "Searching…", 12.5f, DPPTheme.TealMuted, bold: false, align: TextAlignmentOptions.Center);

            // RBv2.0: demo-fallback button removed (QR-only entry).

            // ================= found group =================
            var found = Stretch("FoundGroup", rt);
            // Compact card — the only surface in the found state (v3: no panel).
            AddImage(TLCenter("Patch", found, 220, 145, 300, 130), DPPSpriteFactory.RoundedR20, DPPTheme.NavyPanel, sliced: true);
            var circle = TLCenter("Check", found, 220, 110, 44, 44);
            AddImage(CenterIn("Ring", circle, 44, 44), DPPSpriteFactory.Circle64, DPPTheme.TealAccent);
            AddImage(CenterIn("Fill", circle, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.Hex("#10241e"));
            AddImage(CenterIn("Icon", circle, 24, 24), DPPSpriteFactory.IcCheck, DPPTheme.TealLight);
            AddText(TL("FoundTitle", found, 20, 148, 400, 22), "VCU found", 16, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TL("FoundSub", found, 20, 174, 400, 18), "Loading the product passport…", 12.5f, DPPTheme.TealText, bold: false, align: TextAlignmentOptions.Center);
            found.gameObject.SetActive(false);

            // ================= error group =================
            var error = Stretch("ErrorGroup", rt);
            // Compact card covering title + buttons (v3: no panel behind).
            AddImage(TLCenter("Patch", error, 220, 162, 340, 160), DPPSpriteFactory.RoundedR20, DPPTheme.NavyPanel, sliced: true);
            AddText(TL("ErrTitle", error, 20, 104, 400, 20), "Could not reach the passport server", 14, DPPTheme.Hex("#f3b0b0"), bold: true, align: TextAlignmentOptions.Center);
            AddText(TL("ErrSub", error, 20, 128, 400, 16), "Check the hotspot connection, then scan again.", 12, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.Center);

            // RB2.1 (spec 02 section 5): this panel IS the routine CONTINUE APP? question.
            // LEFT = No -> Close app (red, session-ending). RIGHT = Yes -> Scan again
            // (teal primary). RBv2.0 had "Retry" teal on the LEFT and "Scan again" dark
            // on the right, which put the go-forward action on the wrong side and
            // offered two buttons that both re-attempted the fetch.
            // ⚠ These two were hand-rolled and had NO HoverHighlight at all — they
            // never responded to the pointer in RB2.0 or RB2.1 and nobody noticed,
            // because the ring was the only feedback and its absence reads as "the
            // ray missed". Both now carry the elevation kit and the hover rise
            // (00 §4, §4.1).
            var closeRT = TLCenter("CloseAppButton", error, 145, 186, 140, 32);
            AddShadow(closeRT, 140, 32, DPPSpriteFactory.RoundedR13);
            var closeFill = AddImage(CenterIn("Fill", closeRT, 140, 32), DPPSpriteFactory.RoundedR13, DPPTheme.SafetyStroke, sliced: true, raycast: true);
            AddGloss(closeRT, 140, 32, DPPSpriteFactory.RoundedR13);
            AddText(Stretch("Label", closeRT), "Close app", 12.5f, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            var closeBtn = closeRT.gameObject.AddComponent<Button>();
            closeBtn.transition = Selectable.Transition.None;
            closeBtn.targetGraphic = closeFill;
            closeRT.gameObject.AddComponent<HoverHighlight>();

            var againRT = TLCenter("ScanAgainButton", error, 295, 186, 140, 32);
            AddShadow(againRT, 140, 32, DPPSpriteFactory.RoundedR13);
            var againFill = AddImage(CenterIn("Fill", againRT, 140, 32), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true, raycast: true);
            AddGloss(againRT, 140, 32, DPPSpriteFactory.RoundedR13);
            AddText(Stretch("Label", againRT), "Scan again", 12.5f, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            var againBtn = againRT.gameObject.AddComponent<Button>();
            againBtn.transition = Selectable.Transition.None;
            againBtn.targetGraphic = againFill;
            againRT.gameObject.AddComponent<HoverHighlight>();
            error.gameObject.SetActive(false);

            // ================= wiring =================
            SetRef(controller, "manager", manager);
            SetRef(controller, "router", router);
            SetRef(controller, "mainCanvasRoot", mainGO);
            SetRef(controller, "scanGroup", scan.gameObject);
            SetRef(controller, "foundGroup", found.gameObject);
            SetRef(controller, "errorGroup", error.gameObject);
            SetRef(controller, "sweepLine", sweep);
            SetRef(controller, "searchingLabel", searching);
            SetRef(controller, "closeAppButton", closeBtn);
            SetRef(controller, "scanAgainButton", againBtn);

            // SELF-HEALING WIRE. A phase that RECREATES an object must re-point
            // everyone who references it. The Welcome canvas (RBv2_1/3) holds a
            // reference to this controller; rebuilding the scan screen destroys the
            // old one, so running /2 after /3 used to leave "Scan to start" wired to
            // a dead object — the button appeared to do nothing at all.
            var welcomeCtl = FindAnyIncludingInactive<WelcomeController>();
            if (welcomeCtl != null)
            {
                SetRef(welcomeCtl, "scanner", controller);
                Debug.Log("[DPPUIBuilder] Re-pointed WelcomeController.scanner at the rebuilt scan controller.");
            }

            // The scanner owns the fetch now — kill the legacy auto-fetch.
            if (manager != null) SetBool(manager, "fetchOnStart", false);

            Undo.RegisterCreatedObjectUndo(go, "Build QR Scan Screen");
            Debug.Log("[DPPUIBuilder] RBv2_1/2 — QR scan screen built (RBv2.0: no demo fallback). " +
                      "Re-run RBv2_1/3 to restore the Welcome wiring (waitForWelcome + firstRunPrompt).");
        }

        /// <summary>One L-corner of the scan frame: horizontal + vertical bar
        /// meeting at (x,y); dx/dy point INTO the frame.</summary>
        private static void MakeCorner(RectTransform frame, float x, float y, float dx, float dy)
        {
            var h = CenterIn($"C{x}{y}h", frame, 24f, 3.5f);
            h.anchoredPosition = new Vector2(x + dx * 12f, y);
            AddImage(h, DPPSpriteFactory.Grip, DPPTheme.TealLight);

            var v = CenterIn($"C{x}{y}v", frame, 3.5f, 24f);
            v.anchoredPosition = new Vector2(x, y + dy * 12f);
            AddImage(v, DPPSpriteFactory.Grip, DPPTheme.TealLight);
        }
    }
}
