using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_1_1/1 — THE SUPER PANEL RIG (spec `04_DPP_page.md` v2).
    ///
    /// Builds `DppSuperPanel`: one rig carrying three toed-in world-space
    /// canvases — rail 220, stage 340, data 420 — plus the model and one grab bar.
    ///
    /// ⚠ IT DOES NOT TOUCH DPPPanelCanvas. Welcome, QR, stakeholder, disassembly,
    /// steps and summary all still live there as flat 640 × 430 children. Turning
    /// that canvas into the super panel would have put six working screens in the
    /// blast radius to rebuild one. The two are alternatives: ScreenRouter shows
    /// the rig and hides the panel's grabber, or the reverse.
    ///
    /// SAFE TO RE-RUN: destroys and rebuilds only `DppSuperPanel` and
    /// `DppFreeModel`.
    ///
    /// GEOMETRY IS COMPUTED, NOT TYPED. The spec quotes −26.9° and +20.5°; those
    /// are atan(offset / distance) for 0.75 m, and open item 1 says to verify the
    /// distance on device. Hardcoding the angles would silently decouple them from
    /// the distance the moment it changes, so both come from <see cref="SpDistance"/>.
    ///
    /// SEAMS: the two side canvases pivot on their INNER edge, not their centre.
    /// Yawing about the centre leaves a ~12 mm lateral gap and a 50 mm depth step
    /// at each seam; pivoting on the shared edge makes the three surfaces a
    /// continuous shallow arc. The resulting centre direction differs from the
    /// nominal angle by under 1°.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // ---- rig geometry (spec §2) ----
        // Stage 340 → 400 (device test 2026-08-06): a model wide enough to be worth
        // looking at sweeps past a 340 stage and clips the rail and data canvases
        // even while LOCKED. Every other number here is computed from these three,
        // so widening the stage moves the seams and re-derives both yaw angles.
        private const float SpRailW = 220f, SpStageW = 400f, SpDataW = 420f, SpH = 430f;
        private const float SpTotalW = SpRailW + SpStageW + SpDataW;   // 980
        private const float SpScale = 0.001f;
        private const float SpDistance = 0.75f;      // ⚠ open item 1 — verify on device
        private const float SpEyeHeight = 1.1176f;   // matches the XR rig's Camera Y Offset

        // Panel-local centres, measured from the assembly centre (490).
        private const float SpRailCx  = (SpRailW * 0.5f) - (SpTotalW * 0.5f);                      // -380
        private const float SpStageCx = SpRailW + (SpStageW * 0.5f) - (SpTotalW * 0.5f);           // -100
        private const float SpDataCx  = SpRailW + SpStageW + (SpDataW * 0.5f) - (SpTotalW * 0.5f); // +280

        private static readonly string[] SpTabL1 = { "Product", "Usage &", "Environmental", "Training" };
        private static readonly string[] SpTabL2 = { "specifications", "service", "impact", "disassembly" };
        private static readonly string[] SpTabIcons =
            { "ic_product_specs", "ic_usage_history", "ic_environmental", "ic_training" };

        /// <summary>First tab's y, pushed down to clear the certificates entry.
        /// 24 + 44 + 16 = 84; four tabs at 80 pitch then end at 392 of 430.</summary>
        private const float SpTabTop = 84f;

        [MenuItem("RBv2_1_1/1 — Super panel rig", false, 1)]
        public static void Build_SuperPanel()
        {
            AssetDatabase.Refresh();
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            RemoveByName("DppSuperPanel");
            RemoveByName("DppFreeModel");

            var rigGO = new GameObject("DppSuperPanel");
            Undo.RegisterCreatedObjectUndo(rigGO, "Build super panel");
            var rig = rigGO.transform;
            rig.position = new Vector3(0f, SpEyeHeight, SpDistance);

            var view = rigGO.AddComponent<SuperPanelView>();

            // The freed model's root is a SIBLING of the rig, not a child: once
            // unlocked the model must stop following the rig, or dragging the
            // panels would drag the "freed" model with them (spec §2.3).
            var freeRootGO = new GameObject("DppFreeModel");
            Undo.RegisterCreatedObjectUndo(freeRootGO, "Build free model root");
            freeRootGO.transform.position = rig.position;

            // ---------------- the three canvases ----------------
            // Stage first: its edges are where the side canvases hinge.
            float stageLeft  = (SpStageCx - SpStageW * 0.5f) * SpScale;   // -0.270
            float stageRight = (SpStageCx + SpStageW * 0.5f) * SpScale;   // +0.070

            var stage = SpCanvas(rig, "StageCanvas", SpStageW, SpH,
                pivot: new Vector2(0.5f, 0.5f),
                localX: SpStageCx * SpScale,
                yawDeg: 0f);                                              // spec §2.1 — deliberately flat

            var rail = SpCanvas(rig, "RailCanvas", SpRailW, SpH,
                pivot: new Vector2(1f, 0.5f),                             // hinge on the inner (right) edge
                localX: stageLeft,
                yawDeg: SpYaw(SpRailCx * SpScale));

            var data = SpCanvas(rig, "DataCanvas", SpDataW, SpH,
                pivot: new Vector2(0f, 0.5f),                             // hinge on the inner (left) edge
                localX: stageRight,
                yawDeg: SpYaw(SpDataCx * SpScale));

            // ---------------- rail ----------------
            AddImage(Stretch("RailBG", rail), DPPSpriteFactory.RoundedR22, DPPTheme.Hex("#081733"), sliced: true);
            AddImage(TL("Divider", rail, 219f, 18f, 1f, 394f), null, DPPTheme.Hex("#1a335f"));

            // Certificates sits ABOVE the four tabs (Thiago, 2026-08-06) and the
            // bottom badge is gone: one rail entry, one thing in the data canvas,
            // five entries with one grammar.
            var certBtn = SpCertEntry(rail, out var certFill, out var certStroke,
                out var certLabel, out var certIcon);

            var tabRoots = new RectTransform[SuperPanelView.TabCount];
            var tabFills = new Image[SuperPanelView.TabCount];
            var tabStrokes = new Image[SuperPanelView.TabCount];
            var tabAccents = new Image[SuperPanelView.TabCount];
            var tabTicks = new GameObject[SuperPanelView.TabCount];
            var tabL1 = new TMP_Text[SuperPanelView.TabCount];
            var tabL2 = new TMP_Text[SuperPanelView.TabCount];
            var tabIcons = new Image[SuperPanelView.TabCount];
            var tabButtons = new Button[SuperPanelView.TabCount];

            for (int i = 0; i < SuperPanelView.TabCount; i++)
            {
                tabButtons[i] = SpRailTab(rail, i, out tabRoots[i], out tabFills[i], out tabStrokes[i],
                    out tabAccents[i], out tabTicks[i], out tabL1[i], out tabL2[i], out tabIcons[i]);
                WireClick(tabButtons[i], view, "SelectTab" + i);
            }

            var router = SpFindRouter();
            WireClick(certBtn, view, nameof(SuperPanelView.ShowCertificates));

            // ---------------- stage ----------------
            // NO background image. The stage is empty space between two canvases,
            // not a transparent hole in one — nothing can bleed through it and
            // there is no raycast target to suppress (spec §2.4).
            AddImage(CenterIn("StageFrame", stage, SpStageW - 20f, SpH - 20f),
                DPPSpriteFactory.RoundedR22, new Color(0.36f, 0.79f, 0.65f, 0.16f), sliced: true);

            var ghost = CenterIn("GhostOutline", stage, 300f, 300f);
            AddImage(ghost, DPPSpriteFactory.RoundedR22, new Color(0.36f, 0.79f, 0.65f, 0.20f), sliced: true);
            AddText(TL("GhostLabel", ghost, 0f, 308f, 300f, 16f), "return here to re-lock",
                9.5f, DPPTheme.TextTip, bold: false, align: TextAlignmentOptions.Center);
            ghost.gameObject.SetActive(false);

            var lockBtn = SpLockButton(stage, view, out var lockGlyph, out var lockLabel);

            // The model's home: an empty transform at the stage centre, on the
            // stage's own plane. The model parents here when locked.
            var homeGO = new GameObject("ModelHome");
            var home = homeGO.transform;
            home.SetParent(stage, false);
            home.localPosition = new Vector3(0f, 20f, -40f);   // canvas units; slightly toward the user
            home.localRotation = Quaternion.identity;
            home.localScale = Vector3.one;

            var model = SpBuildStageModel(home);

            // One grabber under the stage moves the WHOLE rig (spec §2.3).
            SpGrabber(stage, rig, "RigGrabber", DPPTheme.GrabberGrip, spawnDistance: SpDistance);

            // The freed model's own bar lives on ITS OWN canvas under the free
            // root, not on the stage — a bar nailed to the stage while the model
            // floats across the room is not "the model's bar", it is a second rig
            // bar in disguise. Tinted teal so which-moves-what needs no experiment.
            var freeCanvas = new GameObject("FreeModelCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var freeRT = (RectTransform)freeCanvas.transform;
            var freeCv = freeCanvas.GetComponent<Canvas>();
            freeCv.renderMode = RenderMode.WorldSpace;
            freeCv.worldCamera = Camera.main;
            freeRT.SetParent(freeRootGO.transform, false);
            freeRT.sizeDelta = new Vector2(240f, 40f);
            freeRT.localScale = Vector3.one * SpScale;
            freeRT.localPosition = new Vector3(0f, -0.170f, 0f);   // under the freed model
            var freeBar = SpGrabber(freeRT, freeRootGO.transform, "FreeModelGrabber",
                DPPTheme.TealLight, spawnDistance: SpDistance, recenter: false);
            freeBar.anchoredPosition = new Vector2(120f, -9f);      // centred in its own 240 × 40 canvas

            // ---------------- data canvas ----------------
            AddImage(Stretch("DataBG", data), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var placeholder = Stretch("PlaceholderPage", data);
            var placeholderLbl = AddText(CenterIn("Label", placeholder, 340f, 120f),
                "not built yet", 14f, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.Center);
            placeholderLbl.textWrappingMode = TextWrappingModes.Normal;

            // Tab pages are filled in by their own phases. Product specs is
            // RBv2_1_1/2, which re-parents the page RBv2_1/9 already built.
            var certPage = SpCertPage(data, view);

            var tabPages = new GameObject[SuperPanelView.TabCount];
            var existing = SpFind("ProductSpecsPage");
            if (existing != null) tabPages[0] = existing;

            // ---------------- wiring ----------------
            SetRef(view, "router", router);
            SetRef(view, "welcome", Object.FindFirstObjectByType<WelcomeController>(FindObjectsInactive.Include));
            SetRef(view, "scanner", Object.FindFirstObjectByType<QRScanController>(FindObjectsInactive.Include));
            SetRefArray(view, "tabRoots", tabRoots);
            SetRefArray(view, "tabFills", tabFills);
            SetRefArray(view, "tabStrokes", tabStrokes);
            SetRefArray(view, "tabAccents", tabAccents);
            SetRefArray(view, "tabTicks", tabTicks);
            SetRefArray(view, "tabLine1", tabL1);
            SetRefArray(view, "tabLine2", tabL2);
            SetRefArray(view, "tabIcons", tabIcons);
            SetRefArray(view, "tabButtons", tabButtons);
            SetRefArray(view, "tabPages", tabPages);
            SetRef(view, "certFill", certFill);
            SetRef(view, "certStroke", certStroke);
            SetRef(view, "certLabel", certLabel);
            SetRef(view, "certIcon", certIcon);
            SetRef(view, "certButton", certBtn);
            SetRef(view, "certPage", certPage.gameObject);
            SetRef(view, "placeholderPage", placeholder.gameObject);
            SetRef(view, "placeholderLabel", placeholderLbl);
            SetRef(view, "stageModelHome", home);
            SetRef(view, "freeModelRoot", freeRootGO.transform);
            SetRef(view, "model", model);
            SetRef(view, "ghostOutline", ghost.gameObject);
            SetRef(view, "freeModelGrabber", freeCanvas);
            SetRef(view, "lockedSprite", LoadPageIcon("ic_lock"));
            SetRef(view, "unlockedSprite", LoadPageIcon("ic_unlock"));
            SetRef(view, "lockLabel", lockLabel);
            SetRef(view, "lockGlyph", lockGlyph);

            if (router != null)
            {
                SetRef(router, "dppSuperPanel", rigGO);
                SetRef(router, "freeModelRoot", freeRootGO);
                var grab = SpFind("GrabberBar");
                if (grab != null) SetRef(router, "panelGrabber", grab);
                else Debug.LogWarning("[DPPUIBuilder] No GrabberBar found — both grab bars will be live at once.");
            }

            freeCanvas.SetActive(false);   // shown only while the model is actually free
            rigGO.SetActive(false);
            freeRootGO.SetActive(false);

            Selection.activeGameObject = rigGO;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log($"[DPPUIBuilder] RBv2_1_1/1 — super panel built at {SpDistance} m. " +
                      $"Yaw: rail {SpYaw(SpRailCx * SpScale):0.0}°, stage 0°, data {SpYaw(SpDataCx * SpScale):0.0}°. " +
                      "Now run RBv2_1_1/2, then RBv2_1/Tools/Verify wiring, then SAVE THE SCENE.");
        }

        // =================================================================
        // RBv2_1_1/2 — move the Product specs page into the data canvas
        // =================================================================

        /// <summary>
        /// RBv2_1/9 authored Product specs as a 420 × 430 root precisely so this
        /// step is a re-parent and not a rebuild. Nothing inside the page moves:
        /// the data canvas is exactly the size the page was drawn at.
        /// </summary>
        [MenuItem("RBv2_1_1/2 — Product specs into the data canvas", false, 2)]
        public static void Build_ProductSpecsIntoRig()
        {
            var rig = SpFind("DppSuperPanel");
            if (rig == null) { Debug.LogError("[DPPUIBuilder] No DppSuperPanel — run RBv2_1_1/1 first."); return; }
            var data = rig.transform.Find("DataCanvas") as RectTransform;
            if (data == null) { Debug.LogError("[DPPUIBuilder] DppSuperPanel has no DataCanvas — re-run RBv2_1_1/1."); return; }

            var page = SpFind("ProductSpecsPage");
            if (page == null) { Debug.LogError("[DPPUIBuilder] No ProductSpecsPage — run RBv2_1/9 first."); return; }

            Undo.SetTransformParent(page.transform, data, "Move Product specs into the data canvas");
            var rt = (RectTransform)page.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.localRotation = Quaternion.identity;
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(SpDataW, SpH);

            // ⚠ anchoredPosition only writes x and y. Undo.SetTransformParent
            // preserves the WORLD pose, so the page arrives carrying whatever z
            // the move implied — and a page floating a few mm off the canvas it
            // sits on reads, through a canvas yawed 20°, as a SECOND panel offset
            // beside the first. That is the "two panels on the right" from the
            // 2026-08-06 device test; it was never a modal.
            var lp = rt.localPosition;
            rt.localPosition = new Vector3(lp.x, lp.y, 0f);

            var view = rig.GetComponent<SuperPanelView>();
            if (view != null)
            {
                var pages = new GameObject[SuperPanelView.TabCount];
                pages[0] = page;
                SetRefArray(view, "tabPages", pages);
            }

            // The page's own bottom bar now drives the walkthrough instead of
            // routing straight out of the passport.
            var ps = page.GetComponent<ProductSpecsView>();
            if (ps != null) SetRef(ps, "owner", view);

            // ScreenRouter must stop treating the page as a sibling screen — it is
            // a child of the rig now, and Show() deactivating it would blank the
            // data canvas the moment the rig appeared.
            var router = SpFindRouter();
            if (router != null) SetRef(router, "productSpecs", null);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/2 — Product specs re-parented into the data canvas. " +
                      "ScreenRouter.productSpecs cleared (it is no longer a sibling screen). SAVE THE SCENE.");
        }

        // =================================================================
        // Pieces
        // =================================================================

        /// <summary>Yaw that turns a canvas at local x = <paramref name="offsetX"/>
        /// to face a viewer <see cref="SpDistance"/> in front. A canvas's visible
        /// face is its −z, so the sign follows atan2(offset, distance) directly.</summary>
        private static float SpYaw(float offsetX) => Mathf.Atan2(offsetX, SpDistance) * Mathf.Rad2Deg;

        private static RectTransform SpCanvas(Transform rig, string name, float w, float h,
            Vector2 pivot, float localX, float yawDeg)
        {
            var go = new GameObject(name, typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;      // editor mouse interaction

            var rt = (RectTransform)go.transform;
            rt.SetParent(rig, false);
            rt.pivot = pivot;
            rt.sizeDelta = new Vector2(w, h);
            rt.localScale = Vector3.one * SpScale;
            rt.localPosition = new Vector3(localX, 0f, 0f);
            rt.localRotation = Quaternion.Euler(0f, yawDeg, 0f);
            return rt;
        }

        private static Button SpRailTab(RectTransform rail, int i, out RectTransform root,
            out Image fill, out Image stroke, out Image accent, out GameObject tick,
            out TMP_Text l1, out TMP_Text l2, out Image icon)
        {
            root = TL($"Tab{i}", rail, 18f, SpTabTop + i * 80f, 184f, 68f);
            AddShadow(root, 184f, 68f, DPPSpriteFactory.RoundedR13);

            var outline = AddImage(CenterIn("HoverOutline", root, 184f + HoverHalo, 68f + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            stroke = AddImage(CenterIn("Stroke", root, 184f, 68f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowStroke, sliced: true);
            fill = AddImage(CenterIn("Fill", root, 182f, 66f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);
            AddGloss(root, 184f, 68f, DPPSpriteFactory.RoundedR13);

            accent = AddImage(TL("Accent", root, 0f, 14f, 4f, 40f), DPPSpriteFactory.RoundedR3,
                DPPTheme.TealLight, sliced: true);
            accent.gameObject.SetActive(false);

            var iconRT = TL("Icon", root, 20f, 22f, 24f, 24f);
            var sprite = LoadPageIcon(SpTabIcons[i]);
            icon = iconRT.gameObject.AddComponent<Image>();
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            if (sprite != null) icon.sprite = sprite;
            else Debug.LogWarning($"[DPPUIBuilder] Rail icon '{SpTabIcons[i]}' not found — tab {i} drawn without one.");

            l1 = AddText(TL("Line1", root, 54f, 18f, 124f, 16f), SpTabL1[i], 12.5f, DPPTheme.TextSecondary, bold: false);
            l2 = AddText(TL("Line2", root, 54f, 34f, 124f, 16f), SpTabL2[i], 12.5f, DPPTheme.TextSecondary, bold: false);

            // Visited marker — the check icon, small, in the top-right corner
            // (Thiago, 2026-08-06). It replaces a plain teal dot, which said
            // "something is here" rather than "you have read this". Shown for the
            // RECYCLER ONLY: the Product user has no walkthrough, so a progress
            // marker on tabs that were never gated would invent a sequence that
            // does not exist.
            var tickRT = TLCenter("Tick", root, 170f, 13f, 15f, 15f);
            var tickImg = tickRT.gameObject.AddComponent<Image>();
            tickImg.preserveAspect = true;
            tickImg.raycastTarget = false;
            var tickSprite = LoadPageIcon("ic_visited");
            if (tickSprite != null) tickImg.sprite = tickSprite;
            else
            {
                // Without a sprite an Image draws a solid quad, which reads as a
                // rendering fault rather than a missing asset.
                tickImg.enabled = false;
                Debug.LogWarning("[DPPUIBuilder] Icon 'ic_visited' not found — visited ticks will not show.");
            }
            tick = tickRT.gameObject;
            tick.SetActive(false);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        /// <summary>
        /// The fifth rail entry. Shorter than a tab (44 vs 68) because it is a
        /// reference page rather than a step, and it keeps the red stroke of
        /// 00 §2.1 meaning 4 — regulatory marking, outline and glyph only, never
        /// fill, with a label that names what it marks.
        /// </summary>
        private static Button SpCertEntry(RectTransform rail, out Image fill, out Image stroke,
            out TMP_Text label, out Image icon)
        {
            var root = TL("CertEntry", rail, 18f, 24f, 184f, 44f);
            AddShadow(root, 184f, 44f, DPPSpriteFactory.RoundedR13);

            var outline = AddImage(CenterIn("HoverOutline", root, 184f + HoverHalo, 44f + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            stroke = AddImage(CenterIn("Stroke", root, 184f, 44f), DPPSpriteFactory.RoundedR13,
                DPPTheme.Hex("#e24b4a"), sliced: true);
            fill = AddImage(CenterIn("Fill", root, 181f, 41f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);
            AddGloss(root, 184f, 44f, DPPSpriteFactory.RoundedR13);

            var iconRT = TL("Icon", root, 20f, 13f, 18f, 18f);
            icon = iconRT.gameObject.AddComponent<Image>();
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            icon.color = DPPTheme.TextSecondary;
            var sprite = LoadPageIcon("ic_certificates");
            if (sprite != null) icon.sprite = sprite;
            else icon.enabled = false;

            label = AddText(TL("Label", root, 50f, 14f, 128f, 16f), "Certificates & safety",
                11.5f, DPPTheme.TextSecondary, bold: false);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        /// <summary>
        /// Certificates &amp; safety as a PAGE IN THE DATA CANVAS, replacing the
        /// sibling screen RBv2_1/8 built. Four rows over the standard content band,
        /// and one button: with a single action there is nothing for 00 §5's
        /// primary-always-right to arbitrate, so the action IS the primary.
        /// </summary>
        private static RectTransform SpCertPage(RectTransform data, SuperPanelView view)
        {
            var page = Stretch("CertificatesPage", data);
            AddImage(Stretch("PageBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var icon = TL("Icon", page, 24f, 24f, 18f, 18f);
            var sprite = LoadPageIcon("ic_certificates");
            if (sprite != null)
            {
                var img = icon.gameObject.AddComponent<Image>();
                img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            }
            else Object.DestroyImmediate(icon.gameObject);

            AddText(TL("Title", page, 50f, 20f, 300f, 26f), "Certificates & safety",
                16f, DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Rule", page, 24f, 62f, 372f, 1f), null, DPPTheme.Hex("#1a335f"));

            // Four rows across the 76-360 band: 4 x 71 = 284 exactly.
            for (int i = 0; i < DpCertRows.GetLength(0); i++)
            {
                var row = TL($"Row{i}", page, 0f, 76f + i * 71f, 420f, 71f);

                var chip = TL("Chip", row, 24f, 20f, 76f, 22f);
                AddImage(Stretch("Fill", chip), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);
                AddText(Stretch("Label", chip), DpCertRows[i, 0], 10.5f,
                    DPPTheme.Hex(DpCertRows[i, 1]), bold: false, align: TextAlignmentOptions.Center);

                AddText(TL("Title", row, 110f, 6f, 286f, 16f), DpCertRows[i, 2], 11.5f,
                    DPPTheme.TextOnNavy, bold: true);
                var l1 = AddText(TL("Line1", row, 110f, 24f, 286f, 18f), DpCertRows[i, 3], 9f,
                    DPPTheme.TextSecondary, bold: false);
                var l2 = AddText(TL("Line2", row, 110f, 44f, 286f, 18f), DpCertRows[i, 4], 9f,
                    DPPTheme.Hex("#6f86a8"), bold: false);
                l1.textWrappingMode = TextWrappingModes.Normal;
                l2.textWrappingMode = TextWrappingModes.Normal;
            }

            // Grey, short, and just "Back" (Thiago, 2026-08-06). The green pill
            // read as "continue" on a page that goes nowhere, and at 230 × 46 it
            // took a fifth of the panel to say one word. Same capsule sprite and
            // same 50-unit hit root as every other bottom-bar button.
            var back = PsSmallPill(page, "BackButton", 24f + 110f * 0.5f, 110f, "Back",
                primary: false, out _);
            WireClick(back, view, nameof(SuperPanelView.CloseCertificates));

            page.gameObject.SetActive(false);
            return page;
        }

        private static Button SpComplianceBadge(RectTransform rail)
        {
            var root = TL("ComplianceBadge", rail, 18f, 372f, 184f, 30f);
            var outline = AddImage(CenterIn("HoverOutline", root, 184f + HoverHalo, 30f + HoverHalo),
                DPPSpriteFactory.Pill, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            // 00 §2.1 meaning 4 — regulatory marking: outline and glyph only,
            // never fill, and the label always names what it marks.
            AddImage(CenterIn("Stroke", root, 184f, 30f), DPPSpriteFactory.Pill,
                DPPTheme.Hex("#e24b4a"), sliced: true);
            var fill = AddImage(CenterIn("Fill", root, 181f, 27f), DPPSpriteFactory.Pill,
                DPPTheme.RowFill, sliced: true, raycast: true);

            var icon = TLCenter("Icon", root, 26f, 15f, 16f, 16f);
            var sprite = LoadPageIcon("ic_certificates");
            if (sprite != null)
            {
                var img = icon.gameObject.AddComponent<Image>();
                img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            }
            else Object.DestroyImmediate(icon.gameObject);

            AddText(TL("Label", root, 42f, 7f, 136f, 16f), "CE · REACH · WEEE 5 · IP67",
                9f, DPPTheme.Hex("#dbe4f0"), bold: false, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        private static Button SpLockButton(RectTransform stage, SuperPanelView view,
            out Image glyph, out TMP_Text label)
        {
            var root = TLCenter("LockButton", stage, SpStageW * 0.5f, 372f, 52f, 52f);
            AddShadow(root, 43f, 43f, DPPSpriteFactory.Circle64);

            var outline = AddImage(CenterIn("HoverOutline", root, 40f + HoverHalo, 40f + HoverHalo),
                DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", root, 43f, 43f), DPPSpriteFactory.Circle64, DPPTheme.Hex("#f0c879"));
            var fill = AddImage(CenterIn("Fill", root, 40f, 40f), DPPSpriteFactory.Circle64,
                DPPTheme.CardBlue, sliced: false, raycast: true);

            var glyphRT = CenterIn("Glyph", root, 18f, 18f);
            glyph = glyphRT.gameObject.AddComponent<Image>();
            glyph.preserveAspect = true;
            glyph.raycastTarget = false;
            glyph.color = DPPTheme.Hex("#f0c879");
            var locked = LoadPageIcon("ic_lock");
            if (locked != null) glyph.sprite = locked;
            // A sprite-less Image draws a solid gold quad, which reads as a bug.
            // The LOCKED/UNLOCKED word below carries the state either way.
            else { glyph.enabled = false; Debug.LogWarning("[DPPUIBuilder] Icon 'ic_lock' not found — the lock button shows its label only."); }

            label = AddText(TLCenter("Label", stage, SpStageW * 0.5f, 406f, 120f, 16f), "LOCKED",
                9f, DPPTheme.Hex("#f0c879"), bold: true, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            WireClick(btn, view, nameof(SuperPanelView.ToggleLock));
            return btn;
        }

        /// <summary>
        /// A standard 200 × 22 grabber (00 §5) whose <c>panelRoot</c> is an
        /// arbitrary transform, so the same component drives the rig and the freed
        /// model. <c>recenter</c> is off for the freed model: it must appear
        /// exactly where the user released it, not jump to a spawn pose.
        /// </summary>
        private static RectTransform SpGrabber(RectTransform canvas, Transform panelRoot,
            string name, Color gripColor, float spawnDistance, bool recenter = true)
        {
            var bar = TLCenter(name, canvas, SpStageW * 0.5f, SpH + 24f, 200f, 22f);
            AddImage(CenterIn("Stroke", bar, 202f, 24f), DPPSpriteFactory.Pill, DPPTheme.GrabberStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", bar, 200f, 22f), DPPSpriteFactory.Pill,
                DPPTheme.GrabberFill, sliced: true, raycast: true);
            var grip = AddImage(CenterIn("Grip", bar, 44f, 4f), DPPSpriteFactory.Grip, gripColor);

            var handle = bar.gameObject.AddComponent<PanelGrabHandle>();
            SetRef(handle, "panelRoot", panelRoot);
            SetRef(handle, "barFill", fill);
            SetRef(handle, "grip", grip);
            SetFloat(handle, "spawnDistance", spawnDistance);
            SetFloat(handle, "spawnHeightOffset", 0f);   // the rig is already authored at eye height
            SetBool(handle, "recenterOnStart", recenter);
            return bar;
        }

        /// <summary>
        /// The stage model is a CLONE, and it has to be.
        ///
        /// `RBv2_0/5` puts the original VCU_assembly on the "DPPPreview" layer and
        /// strips that layer from the main camera, so the original is invisible to
        /// the user by design — it exists to be filmed into the intro and how-to
        /// RenderTextures. Re-parenting it into the stage would have produced an
        /// empty stage and broken both loops at once.
        ///
        /// The clone drops DisassemblyAnimator (nothing drives it here), returns to
        /// the Default layer and is scaled to sit inside the stage.
        /// </summary>
        private static Transform SpBuildStageModel(Transform home)
        {
            var animator = Object.FindFirstObjectByType<DisassemblyAnimator>(FindObjectsInactive.Include);
            if (animator == null)
            {
                Debug.LogWarning("[DPPUIBuilder] No DisassemblyAnimator in the scene (VCU_assembly missing?) — " +
                                 "the stage is empty. Import the model and re-run RBv2_1_1/1.");
                return null;
            }

            // The idle yaw must spin the box about ITS OWN centre. A glTF import
            // puts the origin wherever the exporter left it — usually a corner —
            // and rotating that transform swings the mesh around a circle instead
            // of turning it on the spot. On device that read as a model changing
            // size as it rotated and clipping both side canvases (2026-08-06).
            //
            // So the model the view rotates is a PIVOT, and the mesh hangs off it
            // offset by minus its own bounds centre.
            var pivotGO = new GameObject("ModelPivot");
            var pivot = pivotGO.transform;
            pivot.SetParent(home, false);
            pivot.localPosition = Vector3.zero;
            pivot.localRotation = Quaternion.identity;
            pivot.localScale = Vector3.one;

            var clone = Object.Instantiate(animator.gameObject, pivot);
            clone.name = "StageModel";
            foreach (var a in clone.GetComponentsInChildren<DisassemblyAnimator>(true)) Object.DestroyImmediate(a);
            foreach (var c in clone.GetComponentsInChildren<Collider>(true)) c.enabled = false;
            SetLayerRecursiveEditor(clone.transform, 0);   // back onto Default so the main camera sees it

            var t = clone.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;

            // Fit to ~260 of the stage's 340 canvas units.
            //
            // ⚠ Renderer.bounds is ALREADY world space, and the parent canvas is at
            // 0.001 — so `span` here is the on-screen size in metres, not the mesh's
            // local size. Dividing by SpScale a second time inflates the result by
            // 1000× (the first build logged a scale of 1,299,978). The fix is to
            // scale RELATIVE to the current localScale, which is also correct when
            // the source prefab is not authored at scale 1.
            var bounds = SpRendererBounds(t);
            float span = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            if (span > 1e-9f)
            {
                float targetWorld = 260f * SpScale;                 // 0.26 m on screen
                float k = t.localScale.x * (targetWorld / span);
                t.localScale = Vector3.one * k;
                Debug.Log($"[DPPUIBuilder] Stage model: world span {span * 1000f:0.##} mm at scale " +
                          $"{t.localScale.x / k:0.###} → fitted local scale {k:0.###} (target {targetWorld * 1000f:0.#} mm).");
            }
            else Debug.LogWarning("[DPPUIBuilder] Stage model has no measurable renderer bounds — scale left at 1.");

            // Re-measure AFTER scaling, then slide the mesh so its bounds centre
            // sits exactly on the pivot origin.
            var fitted = SpRendererBounds(t);
            Vector3 centreLocal = pivot.InverseTransformPoint(fitted.center);
            t.localPosition -= centreLocal;
            Debug.Log($"[DPPUIBuilder] Stage model re-centred by {(-centreLocal * 1000f)} mm (pivot-local) — " +
                      "the idle yaw now turns it on the spot.");

            return pivot;
        }

        private static Bounds SpRendererBounds(Transform root)
        {
            var rends = root.GetComponentsInChildren<Renderer>(true);
            if (rends.Length == 0) return new Bounds(root.position, Vector3.zero);
            var b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
            return b;
        }

        /// <summary>
        /// Find a GameObject by name ANYWHERE in the open scene, including
        /// inactive ones.
        ///
        /// <c>GameObject.Find</c> skips inactive objects, and every screen this
        /// builder makes is left deactivated — so phase 2 could not see the rig
        /// phase 1 had just built and reported "run RBv2_1_1/1 first" immediately
        /// after it had been run.
        /// </summary>
        private static GameObject SpFind(string name)
        {
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name == name) return root;
                var hit = SpFindIn(root.transform, name);
                if (hit != null) return hit;
            }
            return null;
        }

        private static GameObject SpFindIn(Transform t, string name)
        {
            foreach (Transform c in t)
            {
                if (c.name == name) return c.gameObject;
                var hit = SpFindIn(c, name);
                if (hit != null) return hit;
            }
            return null;
        }

        private static ScreenRouter SpFindRouter()
        {
            var canvas = SpFind("DPPPanelCanvas");
            if (canvas != null)
            {
                var r = canvas.GetComponent<ScreenRouter>();
                if (r != null) return r;
            }
            var found = Object.FindFirstObjectByType<ScreenRouter>(FindObjectsInactive.Include);
            if (found == null) Debug.LogWarning("[DPPUIBuilder] No ScreenRouter in the scene — the rig will not route.");
            return found;
        }

        // =================================================================
        // Serialized-value helper (SetRef only handles Object references)
        // =================================================================

        private static void SetFloat(Object target, string fieldName, float value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop == null) { Debug.LogWarning($"[DPPUIBuilder] Float field '{fieldName}' not found on {target.GetType().Name}."); return; }
            prop.floatValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // SetBool already exists in DPPUIBuilder.StepFlow.cs — same partial class,
        // so it is in scope here. Only SetFloat was missing.
    }
}
