using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_1/8 — Screen 04: THE DPP PAGE (spec `04_DPP_page.md`, mock
    /// `drafts/04_v11_dpp_canva.svg`).
    ///
    /// One panel, four tabs, two roles. Merges RB2.0's spec 13 (DPP Canva) and
    /// spec 14 (Composition &amp; impact) into a single screen.
    ///
    /// SAFE TO RE-RUN: destroys and rebuilds only "DppPage".
    ///
    /// IT DOES NOT DESTROY THE RB2.0 "DppCanva". That object still carries
    /// PassportView, the four RB2.0 detail pages and DPPManager.passport;
    /// destroying it here would strand those references mid-rebuild. Instead it is
    /// deactivated and renamed "DppCanva_RB2_0_legacy", and ScreenRouter.dppCanva is
    /// re-pointed at the new page. Delete the legacy object by hand once the per-tab
    /// phases (spec 05-08) have replaced its detail pages.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // Panel-local geometry, straight off the approved mock (spec 04 §2).
        private const float DpTileW = 290f, DpTileH = 118f;
        private const float DpColL = 24f, DpColR = 326f;
        private const float DpRow1 = 90f, DpRow2 = 218f;
        private const float DpBandTop = 48f, DpBandBottom = 110f;   // content band inside a tile
        private const float DpChipH = 18f, DpChipGap = 6f, DpChipPad = 24f;

        [MenuItem("RBv2_1/8 — DPP page", false, 8)]
        public static void Build_DppPage()
        {
            // Icons dropped onto disk outside Unity are not in the AssetDatabase
            // until a refresh - without this the first build of the day draws
            // every tile without its icon (2026-08-05 device test).
            AssetDatabase.Refresh();
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_1/1 first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();
            if (router == null)
                Debug.LogWarning("[DPPUIBuilder] No ScreenRouter on DPPPanelCanvas — the page will not route.");

            RetireLegacyCanva(canvasRT);
            DestroyChild(canvasRT, "DppPage");

            var screen = Stretch("DppPage", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build DPP page");
            var view = screen.gameObject.AddComponent<DppPageView>();
            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---------------- header ----------------
            var back = DpBackButton(screen);
            var title = AddText(TL("Title", screen, 24f, 21f, 430f, 34f),
                "Digital Product Passport", 25f, DPPTheme.TextOnNavy, bold: true);
            var badge = DpComplianceBadge(screen);
            AddImage(TL("Rule", screen, 24f, 76f, 592f, 1f), null, DPPTheme.Hex("#1a335f"));

            // ---------------- tabs ----------------
            // Row counts differ per tile, so each stack is centred in the band
            // rather than pinned - a fixed top leaves three of the four bottom-heavy.
            var t1 = DpTile(screen, "Tab1_ProductSpecs", DpColL, DpRow1, "ic_product_specs",
                "Product specifications", view, nameof(DppPageView.OpenTab1));
            float y = DpStackTop(2, DpChipGap - 1f);
            var chipModel  = DpChip(t1, "ChipModel",  24f, y,  "Vehicle Control Unit MS 50.4", out var lblModel,  false);
            var chipMaker  = DpChip(t1, "ChipMaker",  24f, y + DpChipH + 5f, "Bosch Motorsport", out var lblMaker, false);
            var chipSerial = DpChip(t1, "ChipSerial", 148f, y + DpChipH + 5f, "VCU0001",         out var lblSerial, false);

            var t2 = DpTile(screen, "Tab2_UsageHistory", DpColR, DpRow1, "ic_usage_history",
                "Usage history", view, nameof(DppPageView.OpenTab2));
            y = DpStackTop(1, 0f);
            var chipEnergy   = DpChip(t2, "ChipEnergy",   24f,  y, "66.2 kWh",   out var lblEnergy,   false);
            var chipDistance = DpChip(t2, "ChipDistance", 100f, y, "225,000 km", out var lblDistance, false);
            var chipHours    = DpChip(t2, "ChipHours",    190f, y, "5,625 h",    out var lblHours,    false);

            var t3 = DpTile(screen, "Tab3_Environmental", DpColL, DpRow2, "ic_environmental",
                "Environmental impact", view, nameof(DppPageView.OpenTab3));
            y = DpStackTop(3, 4f);
            var chipCo2      = DpChip(t3, "ChipCo2",      24f, y,              "CO2 Emissions 73.25 kg CO2 eq",      out var lblCo2,      true);
            var chipMinerals = DpChip(t3, "ChipMinerals", 24f, y + 22f,        "Minerals & Metals 0.01874 kg Sb eq", out var lblMinerals, true);
            var chipEutro    = DpChip(t3, "ChipEutro",    24f, y + 44f,        "Eutroph. Freshwater 0.11592 kg P eq", out var lblEutro,    true);

            var t4 = DpTile(screen, "Tab4_Training", DpColR, DpRow2, "ic_training",
                "Training disassembly", view, nameof(DppPageView.OpenTab4));
            y = DpStackTop(1, 0f);
            var chipSteps   = DpChip(t4, "ChipSteps",   24f,  y, "5 steps",    out var lblSteps,   false);
            var chipActions = DpChip(t4, "ChipActions", 90f,  y, "10 actions", out var lblActions, false);
            var chipMinutes = DpChip(t4, "ChipMinutes", 172f, y, "~5 min",     out var lblMinutes, false);

            // ---------------- bottom bar (00 §5 — primary always right) ----------------
            var leftBtn = DpPill(screen, "LeftButton", 114f, 376f, 180f, 52f, "Back",
                out var leftFill, out var leftStroke, out var leftLbl);
            var primaryBtn = BuildPillButton(screen, "PrimaryButton", cx: 422f, cy: 376f, w: 388f, h: 52f,
                label: "Continue to disassembly", labelSize: 16f, primary: true, chevron: true);

            // ---------------- certificates: a SIBLING SCREEN, not a modal ----------------
            DestroyChild(canvasRT, "CertificatesPage");
            var cert = DpCertificatesPage(canvasRT, router);

            // ---------------- wiring ----------------
            SetRef(view, "router", router);
            SetRef(view, "welcome", Object.FindFirstObjectByType<WelcomeController>(FindObjectsInactive.Include));
            SetRef(view, "scanner", Object.FindFirstObjectByType<QRScanController>(FindObjectsInactive.Include));
            SetRef(view, "backButton", back.gameObject);
            SetRef(view, "title", title.rectTransform);
            SetRef(view, "leftLabel", leftLbl);
            SetRef(view, "leftFill", leftFill);
            SetRef(view, "leftStroke", leftStroke);
            SetRef(view, "primaryLabel", primaryBtn.GetComponentInChildren<TMP_Text>(true));

            SetRef(view, "chipModel", chipModel);       SetRef(view, "lblModel", lblModel);
            SetRef(view, "chipMaker", chipMaker);       SetRef(view, "lblMaker", lblMaker);
            SetRef(view, "chipSerial", chipSerial);     SetRef(view, "lblSerial", lblSerial);
            SetRef(view, "chipEnergy", chipEnergy);     SetRef(view, "lblEnergy", lblEnergy);
            SetRef(view, "chipDistance", chipDistance); SetRef(view, "lblDistance", lblDistance);
            SetRef(view, "chipHours", chipHours);       SetRef(view, "lblHours", lblHours);
            SetRef(view, "chipCo2", chipCo2);           SetRef(view, "lblCo2", lblCo2);
            SetRef(view, "chipMinerals", chipMinerals); SetRef(view, "lblMinerals", lblMinerals);
            SetRef(view, "chipEutro", chipEutro);       SetRef(view, "lblEutro", lblEutro);
            SetRef(view, "chipSteps", chipSteps);       SetRef(view, "lblSteps", lblSteps);
            SetRef(view, "chipActions", chipActions);   SetRef(view, "lblActions", lblActions);
            SetRef(view, "chipMinutes", chipMinutes);   SetRef(view, "lblMinutes", lblMinutes);

            WireClick(back, view, nameof(DppPageView.OnBack));
            if (router != null) WireClick(badge, router, nameof(ScreenRouter.ShowCertificates));
            WireClick(leftBtn, view, nameof(DppPageView.OnLeftButton));
            WireClick(primaryBtn, view, nameof(DppPageView.OnPrimary));

            // The router keeps its `dppCanva` field name — every caller in the
            // journey already points at it, so re-pointing beats renaming.
            if (router != null)
            {
                SetRef(router, "dppCanva", screen.gameObject);
                SetRef(router, "certificates", cert.gameObject);
            }

            // Bind data: one extra Populate call alongside the existing views.
            var mgr = Object.FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
            if (mgr != null) SetRef(mgr, "dppPage", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in the scene — the page will show baked values.");

            screen.gameObject.SetActive(false);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1/8 — DPP page built. The RB2.0 canva is retired, not deleted. " +
                      "Run RBv2_1/Tools/Verify wiring, then SAVE THE SCENE.");
        }

        // =================================================================
        // Pieces
        // =================================================================

        /// <summary>Top of a vertically centred chip stack inside a tile's content band.</summary>
        private static float DpStackTop(int rows, float gap)
        {
            float total = rows * DpChipH + Mathf.Max(0, rows - 1) * gap;
            return DpBandTop + ((DpBandBottom - DpBandTop) - total) * 0.5f;
        }

        private static Button DpBackButton(RectTransform screen)
        {
            var root = TLCenter("BackButton", screen, 44f, 44f, 52f, 52f);   // 52 hit (00 §4)
            var outline = AddImage(CenterIn("HoverOutline", root, 40f + HoverHalo, 40f + HoverHalo),
                DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", root, 40f, 40f), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var fill = AddImage(CenterIn("Fill", root, 37f, 37f), DPPSpriteFactory.Circle64,
                DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", root, 18f, 18f), DPPSpriteFactory.IcBack, Color.white);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        /// <summary>
        /// Compliance badge — a BUTTON, and red per 00 §2.1 meaning 4: outline and
        /// glyph only, never fill. The label always names what it marks, so the
        /// colour never carries the message alone.
        /// </summary>
        private static Button DpComplianceBadge(RectTransform screen)
        {
            var root = TLCenter("ComplianceBadge", screen, 536f, 38f, 200f, 50f);   // 50 hit, 30 visual
            var outline = AddImage(CenterIn("HoverOutline", root, 200f + HoverHalo, 30f + HoverHalo),
                DPPSpriteFactory.Pill, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            AddImage(CenterIn("Stroke", root, 200f, 30f), DPPSpriteFactory.Pill,
                DPPTheme.Hex("#e24b4a"), sliced: true);
            var fill = AddImage(CenterIn("Fill", root, 197f, 27f), DPPSpriteFactory.Pill,
                DPPTheme.RowFill, sliced: true, raycast: true);

            var icon = TLCenter("Icon", root, 26f, 25f, 18f, 18f);
            var sprite = LoadPageIcon("ic_certificates");
            if (sprite != null)
            {
                var img = icon.gameObject.AddComponent<Image>();
                img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            }
            else Object.DestroyImmediate(icon.gameObject);

            AddText(TL("Label", root, 44f, 15f, 148f, 20f), "CE · REACH · WEEE 5 · IP67",
                10.5f, DPPTheme.Hex("#dbe4f0"), bold: false, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        private static RectTransform DpTile(RectTransform screen, string name, float x, float y,
            string iconAsset, string titleText, DppPageView view, string plusMethod)
        {
            var tile = TL(name, screen, x, y, DpTileW, DpTileH);
            AddImage(CenterIn("Stroke", tile, DpTileW + 2f, DpTileH + 2f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#21407a"), sliced: true);
            AddImage(CenterIn("Fill", tile, DpTileW, DpTileH),
                DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);

            var iconRT = TL("Icon", tile, 18f, 14f, 28f, 28f);
            var sprite = LoadPageIcon(iconAsset);
            if (sprite != null)
            {
                var img = iconRT.gameObject.AddComponent<Image>();
                img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            }
            else
            {
                Debug.LogWarning($"[DPPUIBuilder] Icon '{iconAsset}' not found — tile '{name}' drawn without one.");
                Object.DestroyImmediate(iconRT.gameObject);
            }

            AddText(TL("Title", tile, 56f, 18f, 200f, 20f), titleText, 14f, DPPTheme.TextOnNavy, bold: true);

            // "+" — 40 visual inside a 52 hit area, centred so the four sit on one line.
            var plus = TLCenter("Plus", tile, 260f, 30f, 52f, 52f);
            var pOutline = AddImage(CenterIn("HoverOutline", plus, 40f + HoverHalo, 40f + HoverHalo),
                DPPSpriteFactory.Circle64, Color.white);
            pOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", plus, 43f, 43f), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var pFill = AddImage(CenterIn("Fill", plus, 40f, 40f), DPPSpriteFactory.Circle64,
                DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Glyph", plus, 16f, 16f), DPPSpriteFactory.IcPlus, Color.white);

            var pBtn = plus.gameObject.AddComponent<Button>();
            pBtn.transition = Selectable.Transition.None;
            pBtn.targetGraphic = pFill;
            var pHover = plus.gameObject.AddComponent<HoverHighlight>();
            SetRef(pHover, "highlightOutline", pOutline.gameObject);
            WireClick(pBtn, view, plusMethod);

            return tile;
        }

        /// <summary>
        /// The only content element on this screen (spec 04 §3). The width baked
        /// here is a starting value for the editor preview — DppPageView.Populate
        /// re-fits every chip to preferredWidth + 24 once the payload lands.
        /// </summary>
        private static RectTransform DpChip(RectTransform parent, string name, float x, float y,
            string text, out TMP_Text label, bool leftAlign)
        {
            var root = TL(name, parent, x, y, 120f, DpChipH);
            AddImage(Stretch("Fill", root), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);

            var labelRT = Stretch("Label", root);
            label = AddText(labelRT, text, 10.5f, DPPTheme.Hex("#dbe4f0"), bold: false,
                align: leftAlign ? TextAlignmentOptions.MidlineLeft : TextAlignmentOptions.Center);
            if (leftAlign) labelRT.offsetMin = new Vector2(12f, 0f);

            root.sizeDelta = new Vector2(label.preferredWidth + DpChipPad, DpChipH);
            return root;
        }

        // =================================================================
        // Certificates & safety modal (spec 04 §5)
        // =================================================================

        private static readonly string[,] DpCertRows =
        {
            { "CE", "#5dcaa5", "Conformity marking",
              "Declared under 2014/30/EU (EMC) on 09 Oct 2020 · tested to ECE R10 rev.6.",
              "Assessment is only valid once the unit is installed in its final product." },
            { "REACH", "#e24b4a", "Chemicals regulation",
              "2 substances of very high concern declared above 0.1 % w/w:",
              "lead (CAS 7439-92-1) and lead monoxide (CAS 1317-36-8)." },
            { "WEEE 5", "#5dcaa5", "Waste electricals category",
              "Category 5, small equipment · selective treatment recommended at end of life.",
              "Do not dispose of this device in household waste." },
            { "IP67", "#5dcaa5", "Ingress protection",
              "Dust tight, and protected against temporary immersion in water.",
              "Declared for the product; the printed demonstrator is not sealed." },
        };

        /// <summary>
        /// Certificates &amp; safety as a full SCREEN, parented to DPPPanelCanvas
        /// beside the DPP page — not as a child overlay of it.
        ///
        /// It always covered the whole panel, so it was a page pretending to be a
        /// modal. Worse, an overlay on this canvas plane lets PicoHandUIBridge
        /// resolve a click to a button underneath it: the old Close pill sat on the
        /// primary CTA's coordinates and fired it (device test 2026-08-05).
        ///
        /// ScreenRouter owns show/hide, and it is listed in Show()'s deactivate
        /// pass like every other panel screen.
        /// </summary>
        private static RectTransform DpCertificatesPage(RectTransform canvasRT, ScreenRouter router)
        {
            var modal = Stretch("CertificatesPage", canvasRT);
            Undo.RegisterCreatedObjectUndo(modal.gameObject, "Build Certificates page");
            // Same navy as the main panel (Thiago, 2026-08-05) - the darker
            // modal/panel fill read as a different surface material on device.
            AddImage(Stretch("PanelBG", modal), DPPSpriteFactory.RoundedR22,
                DPPTheme.NavyPanel, sliced: true);

            var icon = TL("Icon", modal, 24f, 30f, 20f, 20f);
            var sprite = LoadPageIcon("ic_certificates");
            if (sprite != null)
            {
                var img = icon.gameObject.AddComponent<Image>();
                img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            }
            else Object.DestroyImmediate(icon.gameObject);

            AddText(TL("Title", modal, 54f, 21f, 420f, 34f), "Certificates & safety",
                25f, DPPTheme.TextOnNavy, bold: true);

            // Close is an X in the header's right corner, returning to the DPP page.
            var close = TLCenter("CloseX", modal, 598f, 38f, 52f, 52f);
            var cOutline = AddImage(CenterIn("HoverOutline", close, 36f + HoverHalo, 36f + HoverHalo),
                DPPSpriteFactory.Circle64, Color.white);
            cOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", close, 36f, 36f), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var cFill = AddImage(CenterIn("Fill", close, 33f, 33f), DPPSpriteFactory.Circle64,
                DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Glyph", close, 15f, 15f), DPPSpriteFactory.IcCross, Color.white);
            var cBtn = close.gameObject.AddComponent<Button>();
            cBtn.transition = Selectable.Transition.None;
            cBtn.targetGraphic = cFill;
            var cHover = close.gameObject.AddComponent<HoverHighlight>();
            SetRef(cHover, "highlightOutline", cOutline.gameObject);
            if (router != null) WireClick(cBtn, router, nameof(ScreenRouter.ShowDppCanva));

            AddImage(TL("Rule", modal, 24f, 76f, 592f, 1f), null, DPPTheme.Hex("#1a335f"));

            // Four rows over the whole content band now that the bottom button is
            // gone: 96 -> 400, step 76, mirror-equal padding (00 s.1.1).
            for (int i = 0; i < DpCertRows.GetLength(0); i++)
            {
                float top = 96f + i * 76f;
                var row = TL($"Row{i}", modal, 0f, top, 640f, 76f);

                // Chip is centred against its OWN paragraph, not pinned to line 1.
                var chip = TL("Chip", row, 24f, 22f, 92f, 24f);
                AddImage(Stretch("Fill", chip), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);
                AddText(Stretch("Label", chip), DpCertRows[i, 0], 11.5f,
                    DPPTheme.Hex(DpCertRows[i, 1]), bold: false, align: TextAlignmentOptions.Center);

                AddText(TL("Title", row, 132f, 8f, 484f, 18f), DpCertRows[i, 2], 13f,
                    DPPTheme.TextOnNavy, bold: true);
                AddText(TL("Line1", row, 132f, 28f, 484f, 16f), DpCertRows[i, 3], 11f,
                    DPPTheme.TextSecondary, bold: false);
                AddText(TL("Line2", row, 132f, 45f, 484f, 16f), DpCertRows[i, 4], 11f,
                    DPPTheme.Hex("#6f86a8"), bold: false);
            }

            modal.gameObject.SetActive(false);
            return modal;
        }

        /// <summary>
        /// A no-chevron pill whose fill, stroke and label the view can recolour at
        /// runtime - the left slot is a grey `Back` for the recycler and a red
        /// `Quit` for the product user, and one button cannot be baked as both.
        /// </summary>
        private static Button DpPill(RectTransform parent, string name, float cx, float cy,
            float w, float h, string label, out Image fill, out Image stroke, out TMP_Text text)
        {
            var root = TLCenter(name, parent, cx, cy, w, h);
            var outline = AddImage(CenterIn("HoverOutline", root, w + HoverHalo, h + HoverHalo),
                DPPSpriteFactory.Pill, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            stroke = AddImage(CenterIn("Stroke", root, w, h), DPPSpriteFactory.Pill,
                DPPTheme.Hex("#324a6d"), sliced: true);
            fill = AddImage(CenterIn("Fill", root, w - 2f, h - 2f), DPPSpriteFactory.Pill,
                DPPTheme.Hex("#1a2740"), sliced: true, raycast: true);
            text = AddText(Stretch("Label", root), label, 16f, DPPTheme.TextSecondary,
                bold: true, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        // =================================================================
        // Helpers
        // =================================================================

        /// <summary>
        /// Load an authored tile icon, coercing the importer to Sprite the first
        /// time. Freshly dropped PNGs import as plain Textures, and a null sprite
        /// here would silently draw a tile with a hole in it.
        /// </summary>
        private static Sprite LoadPageIcon(string name)
        {
            string[] dirs = { "Assets/Textures/Icons", "Assets/Textures/UI" };
            foreach (string dir in dirs)
            {
                string path = $"{dir}/{name}.png";
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sprite != null) return sprite;

                if (AssetImporter.GetAtPath(path) is TextureImporter imp)
                {
                    imp.textureType = TextureImporterType.Sprite;
                    imp.spriteImportMode = SpriteImportMode.Single;   // Sprite type alone is not enough
                    imp.alphaIsTransparency = true;
                    imp.mipmapEnabled = false;
                    imp.SaveAndReimport();
                    sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    if (sprite != null) return sprite;
                }
            }
            return null;
        }

        /// <summary>Retire the RB2.0 canva without destroying it (see the class note).</summary>
        private static void RetireLegacyCanva(RectTransform canvasRT)
        {
            var legacy = canvasRT.Find("DppCanva");
            if (legacy == null) return;
            legacy.gameObject.SetActive(false);
            legacy.gameObject.name = "DppCanva_RB2_0_legacy";
            Debug.Log("[DPPUIBuilder] RB2.0 'DppCanva' deactivated and renamed 'DppCanva_RB2_0_legacy'. " +
                      "It still holds PassportView and the RB2.0 detail pages — delete it by hand after spec 05-08.");
        }
    }
}
