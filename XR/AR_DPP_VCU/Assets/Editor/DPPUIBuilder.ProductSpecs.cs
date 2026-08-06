using System.Collections.Generic;
using System.IO;
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
    /// RBv2_1/9 — 04c: THE PRODUCT SPECIFICATIONS TAB (spec `04c_product_specs.md`,
    /// mock `drafts/04c_v3_product_specs.svg`, revised on the 2026-08-06 device test).
    ///
    /// Four states in one 420 × 430 page: Product ID · component list · component
    /// detail · drawing enlarged.
    ///
    /// AUTHORED AT 420 WIDE, NOT AT THE PANEL'S WIDTH, so `RBv2_1_1/2` moves it
    /// into the super panel's data canvas with a re-parent instead of a rebuild.
    ///
    /// NO PAGE TITLE (2026-08-06). The two sub-tab pills say what is on screen, so
    /// a title above them named the page for a third time and cost 40 units of
    /// content band. They are equidistant — 180 + 12 + 180 across the 372 content
    /// width — because two pills of different widths read as a heading plus a
    /// button rather than as a pair of alternatives.
    ///
    /// BUTTONS ARE HALF-HEIGHT, FULL HIT AREA. Thiago asked for roughly half the
    /// bottom bar back; 00 §4 still wants a ≥50-unit target. So the pill DRAWS at
    /// 34 inside a root that stays 50 — the same trick the circular buttons already
    /// use (40 visual / 52 hit). Shrinking the root instead would have made every
    /// press a near-miss at arm's length.
    ///
    /// SAFE TO RE-RUN: destroys and rebuilds only "ProductSpecsPage".
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // Data-canvas geometry (spec 04c §4). Panel-local, 420 × 430.
        private const float PsW = 420f, PsH = 430f;
        private const float PsMargin = 24f, PsContentW = 372f;

        // With the title gone the pills own the header: y 24, rule at 62.
        private const float PsPillY = 24f, PsPillH = 30f, PsPillGap = 12f;
        private const float PsPillW = (PsContentW - PsPillGap) * 0.5f;   // 180
        private const float PsBandTop = 76f, PsBandBottom = 360f;        // 284 of content

        // Bottom bar: visual 34 inside a 50 hit root (see the class note).
        private const float PsBtnCy = 389f, PsBtnVisualH = 34f, PsBtnHitH = 50f;
        private const float PsBackW = 110f, PsPrimaryW = 150f;

        private const float PsRowH = 28f, PsRowPitch = 32f;
        private const int   PsListSlots = 12;    // 8 parts today, headroom for the payload growing
        private const int   PsDetailSlots = 8;

        private static readonly string[] PsIdentityKeys =
        {
            "MANUFACTURER", "NAME", "TYPE", "SERIAL", "PRODUCED", "ORIGIN", "CATEGORY"
        };

        /// <summary>
        /// ⚠ PLACEHOLDERS, NOT DATA. These are deliberately obvious: on 2026-08-06
        /// this page served realistic-looking baked strings on device because one
        /// serialized reference was stale, and nobody could tell. A placeholder
        /// that reads as real data is worse than no placeholder at all.
        /// </summary>
        private static readonly string[] PsIdentityPreview =
        {
            "— no payload —", "— no payload —", "— no payload —",
            "— no payload —", "— no payload —", "— no payload —", "— no payload —"
        };

        [MenuItem("RBv2_1/9 — Product specs tab", false, 9)]
        public static void Build_ProductSpecs()
        {
            AssetDatabase.Refresh();
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();
            PsImportDrawings();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_1/1 first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();

            DestroyChild(canvasRT, "ProductSpecsPage");

            var screen = CenterIn("ProductSpecsPage", canvasRT, PsW, PsH);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build Product specs");
            var view = screen.gameObject.AddComponent<ProductSpecsView>();
            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---------------- header: two equal pills, no title ----------------
            var tabId = PsSubTab(screen, "SubProductId", PsMargin, "Product ID",
                out var idFill, out var idStroke, out var idLabel);
            var tabComp = PsSubTab(screen, "SubComponentDetail", PsMargin + PsPillW + PsPillGap, "Component detail",
                out var compFill, out var compStroke, out var compLabel);
            AddImage(TL("Rule", screen, PsMargin, 62f, PsContentW, 1f), null, DPPTheme.Hex("#1a335f"));

            // Caption sits ON the rule line, right-aligned — used only by the
            // detail and drawing states; blank everywhere else.
            var caption = AddText(TL("Caption", screen, PsMargin + 172f, 44f, 200f, 16f),
                "", 9.5f, DPPTheme.Hex("#6f86a8"), bold: false, align: TextAlignmentOptions.MidlineRight);

            // ---------------- bottom bar ----------------
            var backBtn = PsSmallPill(screen, "BackButton", PsMargin + PsBackW * 0.5f, PsBackW, "Back",
                primary: false, out var backLbl);
            var primaryBtn = PsSmallPill(screen, "PrimaryButton",
                PsMargin + PsContentW - PsPrimaryW * 0.5f, PsPrimaryW, "Next",
                primary: true, out var primaryLbl);

            // ---------------- state 1: Product ID ----------------
            var identity = TL("StateIdentity", screen, 0f, 0f, PsW, PsH);
            var idValues = new TMP_Text[PsIdentityKeys.Length];
            for (int i = 0; i < PsIdentityKeys.Length; i++)
            {
                float y = 96f + i * 34f;                       // 7 × 34 = 238, centred in the 284 band
                AddText(TL($"Key{i}", identity, PsMargin, y, 180f, 16f),
                    PsIdentityKeys[i], 9f, DPPTheme.Hex("#5dcaa5"), bold: false);
                idValues[i] = AddText(TL($"Val{i}", identity, PsMargin, y, PsContentW, 16f),
                    PsIdentityPreview[i], 12.5f, DPPTheme.TextOnNavy, bold: true,
                    align: TextAlignmentOptions.MidlineRight);
                AddImage(TL($"Hair{i}", identity, PsMargin, y + 24f, PsContentW, 1f), null, DPPTheme.Hex("#12294e"));
            }

            // ---------------- state 2: component list ----------------
            var parts = TL("StateParts", screen, 0f, 0f, PsW, PsH);
            var listContent = PsScrollList(parts, out var rows, out var names, out var fills, out var buttons);

            // ---------------- state 3: component detail ----------------
            var detail = TL("StateDetail", screen, 0f, 0f, PsW, PsH);
            AddImage(TL("DwgCardStroke", detail, PsMargin - 1f, PsBandTop - 1f, PsContentW + 2f, 124f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#21407a"), sliced: true);
            AddImage(TL("DwgCard", detail, PsMargin, PsBandTop, PsContentW, 122f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#07142c"), sliced: true);

            var dwgRT = TL("Drawing", detail, PsMargin + 4f, PsBandTop + 10f, 268f, 102f);
            var detailDrawing = dwgRT.gameObject.AddComponent<Image>();
            detailDrawing.preserveAspect = true; detailDrawing.raycastTarget = false;

            AddImage(TL("IsoCard", detail, PsMargin + 272f, PsBandTop + 8f, 94f, 106f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#0b1e3d"), sliced: true);
            var isoRT = TL("Iso", detail, PsMargin + 278f, PsBandTop + 14f, 82f, 94f);
            var detailIso = isoRT.gameObject.AddComponent<Image>();
            detailIso.preserveAspect = true; detailIso.raycastTarget = false;

            var dimLine = AddText(TL("DimLine", detail, PsMargin + 10f, PsBandTop + 8f, 250f, 14f),
                "NX sheet  ·  all dimensions in mm", 8.5f, DPPTheme.Hex("#5dcaa5"), bold: false);

            var enlarge = PsEnlargeChip(detail, view);

            var detailRows = new RectTransform[PsDetailSlots];
            var detailKeys = new TMP_Text[PsDetailSlots];
            var detailVals = new TMP_Text[PsDetailSlots];
            for (int i = 0; i < PsDetailSlots; i++)
            {
                float y = 210f + i * 20f;                      // 8 × 20 = 160, ends at 370
                detailRows[i] = TL($"DetailRow{i}", detail, PsMargin, y, PsContentW, 18f);
                AddImage(Stretch("Fill", detailRows[i]), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);
                detailKeys[i] = AddText(TL("Key", detailRows[i], 12f, 2f, 180f, 14f),
                    "Material", 9.5f, DPPTheme.TextSecondary, bold: false);
                detailVals[i] = AddText(TL("Val", detailRows[i], 12f, 2f, PsContentW - 24f, 14f),
                    "—", 9.5f, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.MidlineRight);
                detailRows[i].gameObject.SetActive(false);
            }

            // ---------------- state 4: drawing enlarged ----------------
            var drawing = TL("StateDrawing", screen, 0f, 0f, PsW, PsH);
            AddImage(TL("LargeCardStroke", drawing, PsMargin - 1f, PsBandTop - 1f, PsContentW + 2f, 286f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#21407a"), sliced: true);
            AddImage(TL("LargeCard", drawing, PsMargin, PsBandTop, PsContentW, 284f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#07142c"), sliced: true);
            var largeRT = TL("DrawingLarge", drawing, PsMargin + 14f, PsBandTop + 14f, PsContentW - 28f, 256f);
            var drawingLarge = largeRT.gameObject.AddComponent<Image>();
            drawingLarge.preserveAspect = true; drawingLarge.raycastTarget = false;
            var drawingCaption = AddText(TL("DrawingCaption", drawing, PsMargin + 10f, PsBandTop + 8f, 250f, 14f),
                "—", 8.5f, DPPTheme.Hex("#5dcaa5"), bold: false);

            // ---------------- wiring ----------------
            SetRef(view, "router", router);
            SetRef(view, "subIdFill", idFill);
            SetRef(view, "subIdStroke", idStroke);
            SetRef(view, "subIdLabel", idLabel);
            SetRef(view, "subCompFill", compFill);
            SetRef(view, "subCompStroke", compStroke);
            SetRef(view, "subCompLabel", compLabel);
            SetRef(view, "caption", caption);
            SetRef(view, "backLabel", backLbl);
            SetRef(view, "primaryLabel", primaryLbl);
            SetRef(view, "identityRoot", identity.gameObject);
            SetRef(view, "partsRoot", parts.gameObject);
            SetRef(view, "detailRoot", detail.gameObject);
            SetRef(view, "drawingRoot", drawing.gameObject);
            SetRef(view, "listContent", listContent);
            SetRef(view, "detailDrawing", detailDrawing);
            SetRef(view, "detailIso", detailIso);
            SetRef(view, "detailDimLine", dimLine);
            SetRef(view, "detailEnlargeChip", enlarge.gameObject);
            SetRef(view, "drawingLarge", drawingLarge);
            SetRef(view, "drawingCaption", drawingCaption);

            SetRefArray(view, "identityValues", idValues);
            SetRefArray(view, "listRows", rows);
            SetRefArray(view, "listNames", names);
            SetRefArray(view, "listFills", fills);
            SetRefArray(view, "listButtons", buttons);
            SetRefArray(view, "detailRows", detailRows);
            SetRefArray(view, "detailKeys", detailKeys);
            SetRefArray(view, "detailValues", detailVals);

            WireClick(tabId, view, nameof(ProductSpecsView.ShowIdentity));
            WireClick(tabComp, view, nameof(ProductSpecsView.ShowParts));
            WireClick(backBtn, view, nameof(ProductSpecsView.OnBack));
            WireClick(primaryBtn, view, nameof(ProductSpecsView.OnPrimary));

            if (router != null) SetRef(router, "productSpecs", screen.gameObject);
            var mgr = Object.FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
            if (mgr != null) SetRef(mgr, "productSpecs", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in the scene — Product specs will show placeholders.");

            screen.gameObject.SetActive(false);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1/9 — Product specs tab built (420 × 430). " +
                      "Run RBv2_1_1/2 to move it into the data canvas, then Verify wiring, then SAVE THE SCENE.");
        }

        // =================================================================
        // Pieces
        // =================================================================

        /// <summary>One of the two equal header pills. Both are <see cref="PsPillW"/>
        /// wide: unequal widths read as a heading beside a button rather than as a
        /// pair of alternatives.</summary>
        private static Button PsSubTab(RectTransform parent, string name, float x, string label,
            out Image fill, out Image stroke, out TMP_Text text)
        {
            var root = TL(name, parent, x, PsPillY, PsPillW, PsPillH);
            var outline = AddImage(CenterIn("HoverOutline", root, PsPillW + HoverHalo, PsPillH + HoverHalo),
                DPPSpriteFactory.Pill, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            stroke = AddImage(CenterIn("Stroke", root, PsPillW, PsPillH), DPPSpriteFactory.Pill,
                DPPTheme.Hex("#21407a"), sliced: true);
            fill = AddImage(CenterIn("Fill", root, PsPillW - 2f, PsPillH - 2f), DPPSpriteFactory.Pill,
                DPPTheme.RowFill, sliced: true, raycast: true);
            text = AddText(Stretch("Label", root), label, 12f, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        /// <summary>
        /// A bottom-bar pill drawn at <see cref="PsBtnVisualH"/> inside a root that
        /// keeps the <see cref="PsBtnHitH"/> hit area 00 §4 requires. Uses the
        /// capsule sprite throughout, so no corner can render square regardless of
        /// how short the button gets.
        /// </summary>
        private static Button PsSmallPill(RectTransform parent, string name, float cx, float w,
            string label, bool primary, out TMP_Text text)
        {
            var root = TLCenter(name, parent, cx, PsBtnCy, w, PsBtnHitH);

            var outline = AddImage(CenterIn("HoverOutline", root, w + HoverHalo, PsBtnVisualH + HoverHalo),
                DPPSpriteFactory.Pill, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            if (!primary)
                AddImage(CenterIn("Stroke", root, w, PsBtnVisualH), DPPSpriteFactory.Pill,
                    DPPTheme.Hex("#324a6d"), sliced: true);

            var fill = AddImage(CenterIn("Fill", root, primary ? w : w - 2f, primary ? PsBtnVisualH : PsBtnVisualH - 2f),
                DPPSpriteFactory.Pill, primary ? DPPTheme.TealAccent : DPPTheme.Hex("#1a2740"),
                sliced: true, raycast: true);

            text = AddText(Stretch("Label", root), label, 12.5f,
                primary ? DPPTheme.TextOnNavy : DPPTheme.TextSecondary,
                bold: true, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        /// <summary>
        /// The component list: masked viewport + pooled rows + PinchScrollArea.
        ///
        /// A row is now the PART NAME ALONE (2026-08-06). The drawing glyph, the
        /// mass and the material summary all left: the glyph marked a distinction
        /// that no longer exists now that board materials are gone, and mass and
        /// material are on the detail page one tap away. Three columns of 8.5 pt
        /// text at 0.75 m bought density nobody could read.
        /// </summary>
        private static RectTransform PsScrollList(RectTransform parent,
            out RectTransform[] rows, out TMP_Text[] names, out Image[] fills, out Button[] buttons)
        {
            float h = PsBandBottom - PsBandTop;

            var viewport = TL("Viewport", parent, PsMargin, PsBandTop, PsContentW, h);
            viewport.gameObject.AddComponent<RectMask2D>();
            AddImage(Stretch("HitArea", viewport), null, new Color(0f, 0f, 0f, 0f), raycast: true);

            var content = TL("Content", viewport, 0f, 0f, PsContentW, h);

            rows = new RectTransform[PsListSlots];
            names = new TMP_Text[PsListSlots];
            fills = new Image[PsListSlots];
            buttons = new Button[PsListSlots];

            for (int i = 0; i < PsListSlots; i++)
            {
                var row = TL($"Row{i}", content, 0f, i * PsRowPitch, PsContentW, PsRowH);
                rows[i] = row;

                var outline = AddImage(CenterIn("HoverOutline", row, PsContentW + HoverHalo, PsRowH + HoverHalo),
                    DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
                outline.gameObject.SetActive(false);

                fills[i] = AddImage(Stretch("Fill", row), DPPSpriteFactory.RoundedR13,
                    DPPTheme.RowFill, sliced: true, raycast: true);
                names[i] = AddText(TL("Name", row, 16f, 5f, PsContentW - 32f, 18f), "—", 12f,
                    DPPTheme.TextOnNavy, bold: false);

                var btn = row.gameObject.AddComponent<Button>();
                btn.transition = Selectable.Transition.None;
                btn.targetGraphic = fills[i];
                buttons[i] = btn;

                var hover = row.gameObject.AddComponent<HoverHighlight>();
                SetRef(hover, "highlightOutline", outline.gameObject);

                row.gameObject.SetActive(false);
            }

            var scroll = viewport.gameObject.AddComponent<PinchScrollArea>();
            SetRef(scroll, "viewport", viewport);
            SetRef(scroll, "content", content);
            return content;
        }

        private static Button PsEnlargeChip(RectTransform parent, ProductSpecsView view)
        {
            var root = TL("EnlargeChip", parent, PsMargin + 176f, PsBandTop + 100f, 88f, 16f);
            var outline = AddImage(CenterIn("HoverOutline", root, 88f + HoverHalo, 16f + HoverHalo),
                DPPSpriteFactory.Pill, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            var fill = AddImage(Stretch("Fill", root), DPPSpriteFactory.Pill,
                DPPTheme.CardBlue, sliced: true, raycast: true);
            AddText(Stretch("Label", root), "tap to enlarge", 8.5f, DPPTheme.Hex("#dbe4f0"),
                bold: false, align: TextAlignmentOptions.Center);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            WireClick(btn, view, nameof(ProductSpecsView.ShowDrawing));
            return btn;
        }

        // =================================================================
        // Drawings
        // =================================================================

        private static readonly string[] PsDrawingKeys =
        {
            "upper_housing", "bottom_housing", "pcb", "connector", "ic_1", "ic_2", "ic_3", "ic_4"
        };

        /// <summary>
        /// Ensure the 16 NX assets are importable Sprites under Resources/dwg.
        ///
        /// They cannot be wired as scene references: which sprite a row wants is a
        /// payload string (`Component.drawing_id`), unknown at build time.
        /// A missing file is a warning, never an error — the view hides the card.
        /// </summary>
        private static void PsImportDrawings()
        {
            const string dir = "Assets/Resources/dwg";
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder(dir))
                AssetDatabase.CreateFolder("Assets/Resources", "dwg");

            var missing = new List<string>();
            foreach (string key in PsDrawingKeys)
            {
                foreach (string suffix in new[] { "_dwg", "_iso" })
                {
                    string path = $"{dir}/{key}{suffix}.png";
                    if (!File.Exists(path)) { missing.Add($"{key}{suffix}.png"); continue; }

                    // Freshly dropped PNGs import as plain Textures; Resources.Load<Sprite>
                    // then returns null and every drawing card silently hides.
                    if (AssetImporter.GetAtPath(path) is TextureImporter imp &&
                        (imp.textureType != TextureImporterType.Sprite ||
                         imp.spriteImportMode != SpriteImportMode.Single))
                    {
                        imp.textureType = TextureImporterType.Sprite;
                        imp.spriteImportMode = SpriteImportMode.Single;
                        imp.alphaIsTransparency = true;
                        imp.mipmapEnabled = false;
                        imp.maxTextureSize = 2048;
                        imp.SaveAndReimport();
                    }
                }
            }

            if (missing.Count > 0)
                Debug.LogWarning($"[DPPUIBuilder] {missing.Count} of {PsDrawingKeys.Length * 2} NX assets missing from " +
                                 $"{dir} — unzip vcu_dwg_assets.zip there. Missing: {string.Join(", ", missing)}");
            else
                Debug.Log($"[DPPUIBuilder] All {PsDrawingKeys.Length * 2} NX assets present in {dir}.");
        }
    }
}
