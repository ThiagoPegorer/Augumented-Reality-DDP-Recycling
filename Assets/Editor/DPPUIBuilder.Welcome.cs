using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// Phase 7 builder — ReBuilt v2.0 Open App routine (spec 12, mocks
    /// drafts/12_welcome_canvas.svg + drafts/12b_first_run_modal.svg).
    ///
    /// Builds TWO independent world-space canvases:
    ///   "WelcomeCanvas"  640x430 — OPEN APP: brand mark, title, subtitle,
    ///                    Close app (secondary) + Continue to scan (primary).
    ///   "FirstRunCanvas" 440x210 — FIRST TIME USING THE APP? modal, shown
    ///                    after a successful scan, before the main canvas.
    ///
    /// Both get their own GraphicRaycaster and grabber bar (00 §4 on-plane
    /// rule + §5), and every button carries the hover-only white outline
    /// (00 §4 global hover rule) via HoverHighlight.
    ///
    /// Does NOT touch DPPPanelCanvas — safe to re-run at any time; it only
    /// destroys and rebuilds its own two canvases. Run Phase 1 first.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("DPP/Build Phase 7 — Welcome + First Run", false, 7)]
        public static void BuildPhase7()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var mainGO = GameObject.Find("DPPPanelCanvas");
            if (mainGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run Phase 1 first.");
                return;
            }
            var router = mainGO.GetComponent<ScreenRouter>();
            var scanner = Object.FindFirstObjectByType<QRScanController>();
            if (scanner == null)
                Debug.LogWarning("[DPPUIBuilder] No QRScanController in the scene — run Phase 6 first, then re-run Phase 7 to wire Continue.");

            RemoveByName("WelcomeCanvas");
            RemoveByName("FirstRunCanvas");

            var welcomeGO = BuildWelcomeCanvas(mainGO, out var welcome, out var continueBtn, out var closeBtn);
            var firstRunGO = BuildFirstRunCanvas(mainGO, out var prompt, out var yesBtn, out var noBtn);

            // ---- wiring: Welcome ----
            SetRef(welcome, "mainCanvasRoot", mainGO);
            SetRef(welcome, "scanner", scanner);
            SetRef(welcome, "firstRunRoot", firstRunGO);
            WireClick(continueBtn, welcome, nameof(WelcomeController.ContinueToScan));
            WireClick(closeBtn, welcome, nameof(WelcomeController.CloseApp));

            // ---- wiring: first-run prompt ----
            SetRef(prompt, "mainCanvasRoot", mainGO);
            SetRef(prompt, "router", router);
            WireClick(yesBtn, prompt, nameof(FirstRunPrompt.ChooseYes));
            WireClick(noBtn, prompt, nameof(FirstRunPrompt.ChooseNo));

            // ---- wiring: hand entry over to Welcome ----
            if (scanner != null)
            {
                SetBool(scanner, "waitForWelcome", true);
                SetRef(scanner, "firstRunPrompt", prompt);
            }

            firstRunGO.SetActive(false);

            Undo.RegisterCreatedObjectUndo(welcomeGO, "Build Welcome Canvas");
            Undo.RegisterCreatedObjectUndo(firstRunGO, "Build First Run Canvas");
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 7 — Welcome + First Run built. " +
                      "QRScanController.waitForWelcome enabled (entry is now Welcome → Continue → scan). Save the scene.");
        }

        // =================================================================
        // OPEN APP — Welcome canvas (spec 12 §2)
        // =================================================================
        private static GameObject BuildWelcomeCanvas(GameObject mainGO,
            out WelcomeController welcome, out Button continueBtn, out Button closeBtn)
        {
            var go = new GameObject("WelcomeCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(PanelW, PanelH);          // 640 x 430 (00 §1)
            rt.position = CanvasPos;                              // same eye-height spot as the main panel
            rt.localScale = Vector3.one * CanvasScale;

            welcome = go.AddComponent<WelcomeController>();

            var page = Stretch("Welcome", rt);
            AddImage(Stretch("PanelBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // Brand mark — teal circle + the recycling glyph from spec 01.
            var circle = TLCenter("BrandCircle", page, 320, 132, 72, 72);
            AddImage(circle, DPPSpriteFactory.Circle64, DPPTheme.TealAccent);
            AddImage(CenterIn("RecycleIcon", circle, 40, 40), DPPSpriteFactory.Recycle, Color.white);

            AddText(TLCenter("Title", page, 320, 206, 600, 42),
                "Welcome to ReBuilt", 32, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Subtitle", page, 320, 241, 600, 22),
                "Digital Product Passport for guided dismantling", 14, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);

            closeBtn = BuildPillButton(page, "CloseButton", cx: 114, cy: 376, w: 180, h: 52,
                label: "Close app", labelSize: 16, primary: false, chevron: false);
            continueBtn = BuildPillButton(page, "ContinueButton", cx: 422, cy: 376, w: 388, h: 52,
                label: "Continue to scan", labelSize: 16, primary: true, chevron: true);

            BuildGrabberBar(rt);                                  // 00 §5 — draggable panel
            return go;
        }

        // =================================================================
        // FIRST TIME USING THE APP? — modal canvas (spec 12 §3)
        // =================================================================
        private static GameObject BuildFirstRunCanvas(GameObject mainGO,
            out FirstRunPrompt prompt, out Button yesBtn, out Button noBtn)
        {
            const float W = 440f, H = 210f;

            var go = new GameObject("FirstRunCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingOrder = 10;                             // above the panel canvases

            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(W, H);
            rt.position = CanvasPos + new Vector3(0f, 0f, -0.05f);
            rt.localScale = Vector3.one * CanvasScale;

            prompt = go.AddComponent<FirstRunPrompt>();

            var card = Stretch("Card", rt);
            // Stroke-behind-fill (same technique as the choice cards).
            AddImage(Stretch("Stroke", card), DPPSpriteFactory.RoundedR20, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", card, W - 4f, H - 4f), DPPSpriteFactory.RoundedR20, DPPTheme.TabActiveFill, sliced: true);

            AddText(TLCenter("Title", card, 220, 52, 400, 26),
                "First time using ReBuilt?", 18, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Subtitle", card, 220, 82, 400, 20),
                "A quick tutorial shows you how to interact in AR.", 13, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);

            noBtn = BuildPillButton(card, "NoButton", cx: 119, cy: 156, w: 190, h: 52,
                label: "No, skip", labelSize: 15, primary: false, chevron: false);
            yesBtn = BuildPillButton(card, "YesButton", cx: 329, cy: 156, w: 190, h: 52,
                label: "Yes, show me", labelSize: 15, primary: true, chevron: true);

            BuildGrabberBar(rt);                                  // draggable anywhere in AR space
            return go;
        }

        // =================================================================
        // Shared pill button — primary (teal) or secondary, with the
        // hover-only white outline required by 00 §4 and a >=50 px hit area.
        // =================================================================
        private static Button BuildPillButton(RectTransform parent, string name,
            float cx, float cy, float w, float h, string label, float labelSize,
            bool primary, bool chevron)
        {
            var root = TLCenter(name, parent, cx, cy, w, h);

            // Hover-only white outline, behind everything, off at rest (00 §4).
            var outline = AddImage(CenterIn("HoverOutline", root, w + 10f, h + 10f),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            if (!primary)
                AddImage(CenterIn("Stroke", root, w + 4f, h + 4f),
                    DPPSpriteFactory.RoundedR13, DPPTheme.TabInactiveFill, sliced: true);

            var fill = AddImage(CenterIn("Fill", root, w, h), DPPSpriteFactory.RoundedR13,
                primary ? DPPTheme.TealAccent : DPPTheme.SecondaryButtonFill, sliced: true, raycast: true);

            AddText(Stretch("Label", root), label, labelSize,
                primary ? DPPTheme.TextOnNavy : DPPTheme.TextSecondary,
                bold: primary, align: TextAlignmentOptions.Center);

            if (chevron)
            {
                // Drawn from two capsule bars — the SF Pro SDF atlas has no glyph (00 §3).
                // anchoredPosition.y is UP in Unity UI, so the UPPER bar takes
                // dy = +4 with a clockwise (-45) tilt and the lower bar dy = -4
                // with +45 — that draws "›". Swapping them points it backwards.
                float chevronX = w * 0.5f - 26f;
                ChevronAt(root, "ChevronTop", chevronX, 4f, -45f);
                ChevronAt(root, "ChevronBottom", chevronX, -4f, 45f);
            }

            var button = root.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = fill;

            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", root);

            return button;
        }

        /// <summary>One capsule bar of a chevron, offset from the parent's centre.</summary>
        private static void ChevronAt(RectTransform parent, string name, float dx, float dy, float zRot)
        {
            var bar = CenterIn(name, parent, 13f, 2.5f);
            bar.anchoredPosition = new Vector2(dx, dy);
            bar.localRotation = Quaternion.Euler(0f, 0f, zRot);
            AddImage(bar, DPPSpriteFactory.Grip, Color.white);
        }
    }
}
