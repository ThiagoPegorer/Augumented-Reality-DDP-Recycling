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
    /// RBv2_0/3 builder — ReBuilt v2.0 Open App routine (spec 12, mocks
    /// drafts/12_v2_welcome_canvas.svg + drafts/12b_v2_first_run.svg).
    ///
    /// Builds TWO independent world-space canvases:
    ///   "WelcomeCanvas"  640x430 — OPEN APP: brand logo, title, subtitle,
    ///                    Close app (secondary) + Scan to start (primary).
    ///                    Rev 2 (2026-07-30): generated teal-disc mark replaced by
    ///                    Assets/Textures/Brand/rebuilt_logo.png at 96 px, no disc;
    ///                    primary label "Continue to scan" -> "Scan to start".
    ///   "FirstRunCanvas" 640x430 — FIRST TIME USING THE APP?, shown after a
    ///                    successful scan, before the main canvas.
    ///                    Rev 2 (2026-07-30): was a 440x210 modal card. Now the
    ///                    standard panel size with panel chrome, and the buttons
    ///                    are "Skip" / "Tutorial" on the Welcome canvas geometry.
    ///
    /// Both get their own GraphicRaycaster and grabber bar (00 §4 on-plane
    /// rule + §5), and every button carries the hover-only white outline
    /// (00 §4 global hover rule) via HoverHighlight.
    ///
    /// Does NOT touch DPPPanelCanvas — safe to re-run at any time; it only
    /// destroys and rebuilds its own two canvases. Run RBv2_0/1 first.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("RBv2_0/3 — Welcome + first run", false, 3)]
        public static void Build3_WelcomeFirstRun()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var mainGO = GameObject.Find("DPPPanelCanvas");
            if (mainGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_0/1 first.");
                return;
            }
            var router = mainGO.GetComponent<ScreenRouter>();
            var scanner = Object.FindFirstObjectByType<QRScanController>();
            if (scanner == null)
                Debug.LogWarning("[DPPUIBuilder] No QRScanController in the scene — run RBv2_0/2 first, then re-run RBv2_0/3 to wire Continue.");

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
            Debug.Log("[DPPUIBuilder] RBv2_0/3 — Welcome + First Run built. " +
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

            // Brand mark (rev 2, 2026-07-30) — the ReBuilt logo at 96 px, sitting
            // DIRECTLY on the navy. No backing disc: the mark carries its own circle
            // and its own blue-green gradient, which fought the teal disc the RBv1.0
            // recycling glyph sat on. 96 rather than the old 72 because the interior
            // infinity curve is thin-stroked and closes up at that size.
            var brand = TLCenter("BrandLogo", page, 320, 124, 96, 96);
            var brandImg = brand.gameObject.AddComponent<Image>();
            brandImg.raycastTarget = false;
            brandImg.preserveAspect = true;              // asset is square; never distort a logo

            var logo = LoadBrandLogo();
            if (logo != null)
            {
                brandImg.sprite = logo;
            }
            else
            {
                // The entry screen must never render blank, so fall back to the
                // generated RBv1.0 mark and say loudly why.
                Debug.LogWarning($"[DPPUIBuilder] Brand logo not found at {BrandLogoPath} — " +
                                 "falling back to the generated recycling mark. Is the PNG imported?");
                brandImg.sprite = DPPSpriteFactory.Load(DPPSpriteFactory.Circle64);
                brandImg.color = DPPTheme.TealAccent;
                AddImage(CenterIn("RecycleIcon", brand, 52, 52), DPPSpriteFactory.Recycle, Color.white);
            }

            AddText(TLCenter("Title", page, 320, 206, 600, 42),
                "Welcome to ReBuilt", 32, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Subtitle", page, 320, 241, 600, 22),
                "Digital Product Passport for guided dismantling", 14, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);

            closeBtn = BuildPillButton(page, "CloseButton", cx: 114, cy: 376, w: 180, h: 52,
                label: "Close app", labelSize: 16, primary: false, chevron: false);
            continueBtn = BuildPillButton(page, "ContinueButton", cx: 422, cy: 376, w: 388, h: 52,
                label: "Scan to start", labelSize: 16, primary: true, chevron: true);

            BuildGrabberBar(rt);                                  // 00 §5 — draggable panel
            return go;
        }

        // =================================================================
        // FIRST TIME USING THE APP? — full-size panel (spec 12 §3, rev 2)
        //
        // REV 2 (2026-07-30), mock drafts/12b_v2_first_run.svg option B:
        //
        //   * 440 x 210 modal card  ->  the standard 640 x 430 panel (00 §1).
        //   * Modal chrome (stroke-behind-fill RoundedR20 card) -> PANEL chrome
        //     (RoundedR22 + NavyPanel). At full panel size the card border read
        //     as a frame inside a frame. sortingOrder stays 10 — this is still
        //     drawn ON TOP of the panel canvases, it just no longer *looks* like
        //     a small floating dialog.
        //   * Labels "No, skip" / "Yes, show me" -> "Skip" / "Tutorial".
        //   * Buttons take the WELCOME CANVAS geometry exactly (180 @ cx 114,
        //     388 @ cx 422, cy 376). Asymmetric on purpose: Welcome is the screen
        //     the participant just came from, so neither hit target moves.
        //     WARNING — it also weights the choice toward the tutorial, and the
        //     narrow left pill was "Close app" one screen earlier. Both are
        //     accepted trade-offs; the steering is worth naming in the
        //     methodology because the tutorial is part of Condition B.
        //
        // A pinch glyph and a "Two steps · about a minute" caption were both
        // trialled in the mock and CUT (Thiago, 2026-07-30). That leaves two
        // lines of text on a tall panel, so the text block is optically centred
        // in the space ABOVE the buttons (baselines 168 / 198) rather than kept
        // on Welcome's baselines (216 / 246). With no logo above it, Welcome's
        // empty top third would read as an image that failed to load.
        // =================================================================
        private static GameObject BuildFirstRunCanvas(GameObject mainGO,
            out FirstRunPrompt prompt, out Button yesBtn, out Button noBtn)
        {
            var go = new GameObject("FirstRunCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingOrder = 10;                             // above the panel canvases

            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(PanelW, PanelH);           // 640 x 430 — standard panel (00 §1)
            rt.position = CanvasPos + new Vector3(0f, 0f, -0.05f);
            rt.localScale = Vector3.one * CanvasScale;

            prompt = go.AddComponent<FirstRunPrompt>();

            var page = Stretch("FirstRun", rt);
            AddImage(Stretch("PanelBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // TLCenter takes the RECT centre, not the baseline. TMP puts the
            // baseline roughly 10 px below centre at 30 pt and 5 px at 14 pt, so
            // the mock's 168 / 198 baselines become rect centres of 158 / 193.
            AddText(TLCenter("Title", page, 320, 158, 600, 42),
                "First time using ReBuilt?", 30, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Subtitle", page, 320, 193, 600, 22),
                "A quick tutorial shows you how to interact in AR.", 14, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);

            noBtn = BuildPillButton(page, "NoButton", cx: 114, cy: 376, w: 180, h: 52,
                label: "Skip", labelSize: 16, primary: false, chevron: false);
            yesBtn = BuildPillButton(page, "YesButton", cx: 422, cy: 376, w: 388, h: 52,
                label: "Tutorial", labelSize: 16, primary: true, chevron: true);

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

        private const string BrandLogoPath = "Assets/Textures/Brand/rebuilt_logo.png";

        /// <summary>Loads the ReBuilt logo, fixing its import settings on first use so
        /// the PNG does not have to be configured by hand after a fresh clone.
        ///
        /// mipmaps stay OFF to match every other UI sprite: the panel renders at
        /// Dynamic Pixels Per Unit 4, so a 512 px source drawn at 96 px is already
        /// oversampled and a lower mip would only soften it.</summary>
        private static Sprite LoadBrandLogo()
        {
            var importer = AssetImporter.GetAtPath(BrandLogoPath) as TextureImporter;
            if (importer == null)
            {
                AssetDatabase.Refresh();                 // file may exist but not be imported yet
                importer = AssetImporter.GetAtPath(BrandLogoPath) as TextureImporter;
            }
            if (importer == null) return null;

            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.filterMode = FilterMode.Bilinear;
                importer.maxTextureSize = 512;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(BrandLogoPath);
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
