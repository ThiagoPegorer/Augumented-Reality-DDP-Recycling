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
    /// RBv2_1/3 builder — ReBuilt v2.0 Open App routine (spec 12, mocks
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
    /// destroys and rebuilds its own two canvases. Run RBv2_1/1 first.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("RBv2_1/3 — Welcome + first run", false, 3)]
        public static void Build3_WelcomeFirstRun()
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
            // MUST include inactive objects: the scan canvas deactivates itself at
            // launch (waitForWelcome) and stays inactive in a saved scene, so a plain
            // FindFirstObjectByType returns NULL and Continue silently wires to nothing.
            // That made RBv2_1/3 depend on being run right after RBv2_1/2. It no longer does.
            var scanner = FindAnyIncludingInactive<QRScanController>();
            if (scanner == null)
                Debug.LogWarning("[DPPUIBuilder] No QRScanController in the scene — run RBv2_1/2 first, then re-run RBv2_1/3 to wire Continue.");

            RemoveByName("WelcomeCanvas");
            RemoveByName("FirstRunCanvas");

            var welcomeGO = BuildWelcomeCanvas(mainGO, out var welcome, out var continueBtn, out var closeBtn);

            // ---- wiring: Welcome ----
            SetRef(welcome, "mainCanvasRoot", mainGO);
            SetRef(welcome, "scanner", scanner);
            WireClick(continueBtn, welcome, nameof(WelcomeController.ContinueToScan));
            WireClick(closeBtn, welcome, nameof(WelcomeController.CloseApp));

            // ---- wiring: hand entry over to Welcome ----
            if (scanner != null)
            {
                SetBool(scanner, "waitForWelcome", true);
                // firstRunPrompt intentionally left NULL (RB2.1 spec 01 §4): the prompt
                // is deleted, so a successful fetch opens the passport directly. When
                // spec 03 lands, this is where the stakeholder screen gets wired.
            }

            Undo.RegisterCreatedObjectUndo(welcomeGO, "Build Welcome Canvas");
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1/3 — Welcome + First Run built. " +
                      "QRScanController.waitForWelcome enabled (entry is now Welcome → Continue → scan). " +
                      "RB2.1: first-run prompt removed, Close app is red. Save the scene.");
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
                label: "Close app", labelSize: 16, primary: false, chevron: false,
                destructive: true);                               // RB2.1 spec 01 §2.2
            continueBtn = BuildPillButton(page, "ContinueButton", cx: 422, cy: 376, w: 388, h: 52,
                label: "Scan to start", labelSize: 16, primary: true, chevron: true);

            BuildGrabberBar(rt);                                  // 00 §5 — draggable panel
            return go;
        }

        // =================================================================
        // The FIRST-RUN PROMPT ("First time using ReBuilt?" -> Skip / Tutorial)
        // was DELETED in RB2.1 (spec 01 §4). The tutorial is no longer one gated
        // sequence before the product; it is a pop-up on each page (spec 09), so
        // there is nothing left for an upfront yes/no question to gate.
        //
        // Requirement that must NOT die with it: the prompt appeared after EVERY
        // successful scan, not once per install, so participant 2 was offered the
        // tutorial as reliably as participant 1. If a per-page pop-up ever
        // persists a "seen" flag across participants, the kiosk cycle silently
        // degrades for everyone after the first. Spec 09 owns that.
        //
        // RemoveByName("FirstRunCanvas") above stays, so re-running this phase
        // clears the canvas from any pre-RB2.1 scene.
        // =================================================================

        // =================================================================
        // Shared pill button — primary (teal) or secondary, with the
        // hover-only white outline required by 00 §4 and a >=50 px hit area.
        // =================================================================
        private static Button BuildPillButton(RectTransform parent, string name,
            float cx, float cy, float w, float h, string label, float labelSize,
            bool primary, bool chevron, bool destructive = false)
        {
            var root = TLCenter(name, parent, cx, cy, w, h);

            // Hover-only white outline, behind everything, off at rest (00 §4).
            var outline = AddImage(CenterIn("HoverOutline", root, w + HoverHalo, h + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            // Destructive = the session-ending action (00 §2.1, RB2.1). Solid red,
            // white bold, no stroke — it reads as a filled button like the primary
            // does, because a red OUTLINE on a dark fill reads as a warning state
            // rather than as something you are meant to press.
            if (!primary && !destructive)
                AddImage(CenterIn("Stroke", root, w + 4f, h + 4f),
                    DPPSpriteFactory.RoundedR13, DPPTheme.TabInactiveFill, sliced: true);

            Color fillColor = destructive ? DPPTheme.SafetyStroke
                            : primary     ? DPPTheme.TealAccent
                                          : DPPTheme.SecondaryButtonFill;
            var fill = AddImage(CenterIn("Fill", root, w, h), DPPSpriteFactory.RoundedR13,
                fillColor, sliced: true, raycast: true);

            AddText(Stretch("Label", root), label, labelSize,
                (primary || destructive) ? DPPTheme.TextOnNavy : DPPTheme.TextSecondary,
                bold: primary || destructive, align: TextAlignmentOptions.Center);

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
