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
    /// RBv2_1_1/09 — 04c: THE PRODUCT SPECIFICATIONS TAB (spec `04c_product_specs.md`,
    /// mock `drafts/04c_v3_product_specs.svg`, revised on the 2026-08-06 device test).
    ///
    /// Four states in one 420 × 430 page: Product ID · component list · component
    /// detail · drawing enlarged.
    ///
    /// AUTHORED AT 420 WIDE, NOT AT THE PANEL'S WIDTH, so `RBv2_1_1/11` moves it
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
        // Geometry standardized across ALL DPP data tabs (Thiago, 2026-08-09,
        // Environmental impact is the reference): Back cx 69 / w 90, Next cx 321 /
        // w 150, cy 402, 11 pt — the buttons must not appear to move as the user
        // walks the tabs. cy 389 → 402 moves the InfoButton with them (it shares
        // the default), keeping the specs page's own bar on one line.
        private const float PsBtnCy = 402f, PsBtnVisualH = 34f, PsBtnHitH = 50f;
        private const float PsBackW = 90f, PsPrimaryW = 150f;

        /// <summary>Width reserved for the identity key column. Values start here
        /// and auto-shrink rather than running under the key.</summary>
        private const float PsKeyColW = 106f;

        // 8 rows across the 284 band: 8 × 35 = 280, so the list fills it evenly
        // instead of stopping 28 short (device test 2026-08-06).
        private const float PsRowH = 30f, PsRowPitch = 35f;

        /// <summary>Row width. Chosen so the HOVERED row still fits the 372 viewport:
        /// 348 x 1.015 + 3 of shadow = 356, clear of the mask on both sides.</summary>
        private const float PsRowW = 348f;
        private const int   PsListSlots = 12;    // 8 parts today, headroom for the payload growing
        private const int   PsDetailSlots = 8;

        // The 1 x 2 grid (04c §4.6). Upper half is the drawing; lower half is the
        // material table. Column x are row-local, so every bar shares one axis.
        //
        // ⚠ THE SPLIT IS PROPORTIONAL, NOT 50/50. These are only the RESTING pose;
        // ProductSpecsView.LayoutDetail recomputes the drawing height and the lower
        // block's y for every component it opens:
        //
        //     lower   = PsDetailHeadH + rows x PsMatPitch
        //     drawing = 284 - PsDetailGap - lower,  clamped to [PsDwgMinH, PsDwgMaxH]
        //
        // 1 material -> 240 units of drawing; 3 -> 200; 6 -> 140. Unclamped the two
        // close EXACTLY on the band bottom, which is why none of these is round.
        // The resting values below are the 3-material case.
        //
        // ⚠ THERE IS NO INFO STRIP ANY MORE (2026-08-06 round 4). Reserving 26 units
        // under the last row for the "i" left a band of empty navy on every component
        // — "there are a bit empty space in the bottom part". The "i" moved onto the
        // BUTTON LINE, into the slot Next vacated, so the table now runs to the band
        // bottom and the drawing gains 26 units on every part.
        private const float PsDwgH0     = 200f;
        private const float PsLowerTop0 = 286f;
        private const float PsMatPitch  = 20f;
        private const float PsDetailHeadH = 14f, PsDetailGap = 10f;
        private const float PsDwgMinH = 114f, PsDwgMaxH = 240f;

        // ONE chart column (04c §4.6). Both bars share this track: recovery impact on
        // a LOG axis above, max recovery rate LINEAR below. Different heights, because
        // two equal bars on one origin read as one measurement — which they are not.
        private const float PsTrackX = 112f, PsTrackW = 152f;
        private const float PsImpY = 2f, PsImpH = 7f;
        private const float PsRecY = 11f, PsRecH = 5f;
        private const int   PsTicksPerRow = 3;
        private const float PsMassRight = 104f;      // right edge of the MASS column
        private const float PsImpPctRight = 316f;    // right edge of the IMP % column

        // The "i" sits on the BUTTON LINE at the far right, where Next used to be in
        // this state. A 22-unit dot in a 44 x PsBtnHitH root — the same hit height as
        // every other button on that line, so the row is uniform to the hand.
        //
        // Placing it there also removed a whole class of bug: it is at a FIXED
        // position, so nothing at runtime moves it. When it lived under the last row,
        // LayoutDetail moved it and HoverHighlight.OnEnable then reset it to the pose
        // it had captured on first enable — which is why it appeared beside the wrong
        // row on every component.
        private const float PsInfoDot = 22f, PsInfoSlotW = 44f;

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

        [MenuItem("RBv2_1_1/09 — Product specs tab", false, 9)]
        public static void Build_ProductSpecs()
        {
            AssetDatabase.Refresh();
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();
            PsImportDrawings();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_1_1/01 first.");
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
            var tabComp = PsSubTab(screen, "SubComponentDetail", PsMargin + PsPillW + PsPillGap, "Component ID",
                out var compFill, out var compStroke, out var compLabel);
            AddImage(TL("Rule", screen, PsMargin, 62f, PsContentW, 1f), null, DPPTheme.Hex("#1a335f"));

            // NO CAPTION. "1 of 8 · 108.5 g" repeated the list the user had just left
            // and the mass already in the row below it (Thiago, device test 2026-08-06).
            // The pills are now the only text in the header, and they never change.

            // ---------------- bottom bar ----------------
            var backBtn = PsSmallPill(screen, "BackButton", PsMargin + PsBackW * 0.5f, PsBackW, "Back",
                primary: false, out var backLbl, fontSize: 11f);
            var primaryBtn = PsSmallPill(screen, "PrimaryButton",
                PsMargin + PsContentW - PsPrimaryW * 0.5f, PsPrimaryW, "Next",
                primary: true, out var primaryLbl, fontSize: 11f);

            // ---------------- state 1: Product ID ----------------
            var identity = TL("StateIdentity", screen, 0f, 0f, PsW, PsH);
            var idValues = new TMP_Text[PsIdentityKeys.Length];
            for (int i = 0; i < PsIdentityKeys.Length; i++)
            {
                float y = 96f + i * 34f;                       // 7 × 34 = 238, centred in the 284 band
                AddText(TL($"Key{i}", identity, PsMargin, y, PsKeyColW, 16f),
                    PsIdentityKeys[i], 9f, DPPTheme.Hex("#5dcaa5"), bold: false);

                // The value rect STOPS at the key column and auto-shrinks inside it.
                // CATEGORY is "EEE - electronic control unit (WEEE cat. 5, small
                // equipment)" — 58 characters that ran straight across the key at a
                // fixed 12.5 pt (device test 2026-08-06). Truncating would have hidden
                // the WEEE category, which is the part a recycler needs, so the text
                // shrinks instead and only the longest row pays for it.
                idValues[i] = AddText(TL($"Val{i}", identity, PsMargin + PsKeyColW, y,
                        PsContentW - PsKeyColW, 16f),
                    PsIdentityPreview[i], 12.5f, DPPTheme.TextOnNavy, bold: true,
                    align: TextAlignmentOptions.MidlineRight);
                idValues[i].enableAutoSizing = true;
                idValues[i].fontSizeMin = 8f;
                idValues[i].fontSizeMax = 12.5f;
                AddImage(TL($"Hair{i}", identity, PsMargin, y + 24f, PsContentW, 1f), null, DPPTheme.Hex("#12294e"));
            }

            // ---------------- state 2: component list ----------------
            var parts = TL("StateParts", screen, 0f, 0f, PsW, PsH);
            var listContent = PsScrollList(parts, out var rows, out var names, out var fills, out var buttons);

            // ---------------- state 3: component detail, a 1 x 2 grid ----------------
            // Upper half: the NX drawing, alone, with a View button. Lower half: ONE
            // chart column carrying BOTH bars, then two colour-matched % columns
            // (04c §4.6). Everything here is the RESTING pose — the split is
            // proportional and ProductSpecsView.LayoutDetail redoes it per component.
            var detail = TL("StateDetail", screen, 0f, 0f, PsW, PsH);

            // The card is the only thing whose height changes, so every part of it is
            // ANCHORED to it rather than positioned beside it. Position them and the
            // card grows while its contents stay where they were.
            var dwgCard = TL("DrawingCard", detail, PsMargin, PsBandTop, PsContentW, PsDwgH0);
            AddImage(Inset("CardStroke", dwgCard, -1f, -1f, -1f, -1f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#21407a"), sliced: true);
            AddImage(Inset("CardFill", dwgCard, 0f, 0f, 0f, 0f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#07142c"), sliced: true);

            var detailDrawing = Inset("Drawing", dwgCard, 6f, 8f, 6f, 8f)
                .gameObject.AddComponent<Image>();
            detailDrawing.preserveAspect = true; detailDrawing.raycastTarget = false;

            // View hangs off the card's BOTTOM-RIGHT corner and rides its height.
            var viewSlot = NewRT("ViewSlot", dwgCard);
            viewSlot.anchorMin = viewSlot.anchorMax = new Vector2(1f, 0f);
            viewSlot.pivot = new Vector2(1f, 0f);
            viewSlot.anchoredPosition = new Vector2(-6f, 2f);
            viewSlot.sizeDelta = new Vector2(46f, PsBtnHitH);
            var viewBtn = PsSmallPill(viewSlot, "ViewButton", 23f, 46f, "View",
                primary: true, out _, cy: PsBtnHitH * 0.5f, visualH: 26f, fontSize: 10.5f);
            WireClick(viewBtn, view, nameof(ProductSpecsView.ShowDrawing));

            // ⚠ NO REUSE BADGE. "Sc4 reuse eligible — processors and flash…" used to
            // sit in the card on two components. Thiago, 2026-08-06: remove all text
            // from this state; it is the drawing and the table, nothing else. The flag
            // is still in the payload and still in the LCA — 04d is where it belongs.

            // ---- the lower block: one chart column, two % columns ----
            float lowerH0 = PsDetailHeadH + 3f * PsMatPitch;
            var lower = TL("LowerBlock", detail, PsMargin, PsLowerTop0, PsContentW, lowerH0);

            // THE HEADS ARE THE LEGEND. Each is in its own bar's colour and the % column
            // under it repeats that colour, so the panel spends no row on a key.
            AddText(TL("HMat", lower, 0f, 0f, 100f, 12f), "MATERIAL", 7.5f,
                DPPTheme.Hex("#5dcaa5"), bold: false);
            AddText(TL("HMass", lower, 0f, 0f, PsMassRight, 12f), "MASS", 7.5f,
                DPPTheme.Hex("#5dcaa5"), bold: false, align: TextAlignmentOptions.MidlineRight);
            AddText(TL("HImp", lower, PsTrackX, 0f, 100f, 12f), "RECOVERY IMPACT", 7.5f,
                DPPTheme.Hex("#2eb086"), bold: false);
            AddText(TL("HRate", lower, PsTrackX + 84f, 0f, 60f, 12f), "/  RATE", 7.5f,
                DPPTheme.Hex("#1f77b4"), bold: false);
            AddText(TL("HImpPct", lower, 0f, 0f, PsImpPctRight, 12f), "IMP %", 7.5f,
                DPPTheme.Hex("#2eb086"), bold: false, align: TextAlignmentOptions.MidlineRight);
            AddText(TL("HRecPct", lower, 0f, 0f, PsContentW, 12f), "REC %", 7.5f,
                DPPTheme.Hex("#1f77b4"), bold: false, align: TextAlignmentOptions.MidlineRight);

            var detailRows = new RectTransform[PsDetailSlots];
            var matNames   = new TMP_Text[PsDetailSlots];
            var matMasses  = new TMP_Text[PsDetailSlots];
            var matImp     = new Image[PsDetailSlots];
            var matImpLbl  = new TMP_Text[PsDetailSlots];
            var matRec     = new Image[PsDetailSlots];
            var matRecLbl  = new TMP_Text[PsDetailSlots];
            var matTicks   = new Image[PsDetailSlots * PsTicksPerRow];

            for (int i = 0; i < PsDetailSlots; i++)
            {
                var row = TL($"MatRow{i}", lower, 0f, PsDetailHeadH + i * PsMatPitch,
                    PsContentW, PsMatPitch);
                detailRows[i] = row;

                matNames[i]  = AddText(TL("Name", row, 0f, 0f, 96f, PsMatPitch), "—", 9.5f,
                    DPPTheme.TextOnNavy, bold: false);
                matMasses[i] = AddText(TL("Mass", row, 0f, 0f, PsMassRight, PsMatPitch), "", 9.5f,
                    DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.MidlineRight);

                // Bars anchor LEFT so the view can drive width alone; a centred rect
                // would grow in both directions and never line up on a shared axis.
                AddImage(TL("ImpTrack", row, PsTrackX, PsImpY, PsTrackW, PsImpH),
                    DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#12294e"), sliced: true);
                matImp[i] = AddImage(TL("ImpBar", row, PsTrackX, PsImpY, PsTrackW, PsImpH),
                    DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#2eb086"), sliced: true);

                // Ticks are built AFTER the bar so they render ON TOP of it. Behind it
                // they would be hidden exactly where the axis has to be read.
                for (int k = 0; k < PsTicksPerRow; k++)
                {
                    float tx = PsTrackX + PsTrackW * (k + 1) / (PsTicksPerRow + 1) - 0.5f;
                    matTicks[i * PsTicksPerRow + k] = AddImage(
                        TL($"Tick{k}", row, tx, PsImpY, 1f, PsImpH), null, DPPTheme.Hex("#2a4a80"));
                }

                AddImage(TL("RecTrack", row, PsTrackX, PsRecY, PsTrackW, PsRecH),
                    DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#12294e"), sliced: true);
                matRec[i] = AddImage(TL("RecBar", row, PsTrackX, PsRecY, PsTrackW, PsRecH),
                    DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#1f77b4"), sliced: true);

                matImpLbl[i] = AddText(TL("ImpPct", row, 0f, 0f, PsImpPctRight, PsMatPitch), "", 8.5f,
                    DPPTheme.Hex("#2eb086"), bold: false, align: TextAlignmentOptions.MidlineRight);
                matRecLbl[i] = AddText(TL("RecPct", row, 0f, 0f, PsContentW, PsMatPitch), "", 8.5f,
                    DPPTheme.Hex("#1f77b4"), bold: false, align: TextAlignmentOptions.MidlineRight);

                row.gameObject.SetActive(false);
            }

            // ---- the "i", on the button line where Next used to be ----
            // Built on `detail`, NOT on `lower`: nothing may move it at runtime.
            var infoBtn = PsSmallPill(detail, "InfoButton",
                PsMargin + PsContentW - PsInfoSlotW * 0.5f, PsInfoSlotW, "i",
                primary: false, out _, visualH: PsInfoDot, fontSize: 11f);
            WireClick(infoBtn, view, nameof(ProductSpecsView.ShowInfo));

            // ---------------- state 4: drawing enlarged ----------------
            var drawing = TL("StateDrawing", screen, 0f, 0f, PsW, PsH);
            AddImage(TL("LargeCardStroke", drawing, PsMargin - 1f, PsBandTop - 1f, PsContentW + 2f, 286f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#21407a"), sliced: true);
            AddImage(TL("LargeCard", drawing, PsMargin, PsBandTop, PsContentW, 284f),
                DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#07142c"), sliced: true);
            var largeRT = TL("DrawingLarge", drawing, PsMargin + 14f, PsBandTop + 14f, PsContentW - 28f, 256f);
            var drawingLarge = largeRT.gameObject.AddComponent<Image>();
            drawingLarge.preserveAspect = true; drawingLarge.raycastTarget = false;
            // NOTHING ELSE GOES IN THIS STATE. No caption, no component name, no scale
            // note (Thiago, device test 2026-08-06: "in this tab is just the draw").

            // ---------------- the chart explanation ----------------
            // Built LAST so it draws over every other state, and left inactive. It is UI
            // on the page's own canvas, so 00 §4.2's modal-depth rule (a 3D mesh always
            // wins the depth test) does not apply — the model lives on the stage canvas.
            var modal = TL("InfoModal", screen, 0f, 0f, PsW, PsH);
            var scrim = AddImage(Stretch("Scrim", modal), null,
                new Color(0f, 0f, 0f, 0.55f), raycast: true);
            var scrimBtn = modal.gameObject.AddComponent<Button>();
            scrimBtn.transition = Selectable.Transition.None;
            scrimBtn.targetGraphic = scrim;
            WireClick(scrimBtn, view, nameof(ProductSpecsView.HideInfo));

            var card = TLCenter("Card", modal, PsW * 0.5f, PsH * 0.5f, 344f, 214f);
            AddShadow(card, 344f, 214f, DPPSpriteFactory.RoundedR22);
            AddImage(Stretch("Fill", card), DPPSpriteFactory.RoundedR22,
                DPPTheme.Hex("#0d2a57"), sliced: true);
            AddGloss(card, 344f, 214f, DPPSpriteFactory.RoundedR22);

            AddText(TL("Title", card, 18f, 12f, 300f, 18f), "How to read this chart", 13f,
                DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Rule", card, 18f, 34f, 308f, 1f), null, DPPTheme.Hex("#1a335f"));

            AddImage(TL("SwImp", card, 18f, 44f, 22f, 6f), DPPSpriteFactory.RoundedR3,
                DPPTheme.Hex("#2eb086"), sliced: true);
            AddText(TL("HdImp", card, 48f, 40f, 260f, 14f), "RECOVERY IMPACT", 9.5f,
                DPPTheme.Hex("#2eb086"), bold: true);
            string[] psImpLines =
            {
                "This material's share of the component's minerals and",
                "metals footprint (EF 3.1 ADP). LOG scale — gold is 0.04 %",
                "of the connector mass and 98 % of its impact.",
                "— means the material has no EF 3.1 factor.",
            };
            for (int i = 0; i < psImpLines.Length; i++)
                AddText(TL($"ImpL{i}", card, 48f, 56f + i * 12f, 290f, 12f), psImpLines[i],
                    8.5f, DPPTheme.TextSecondary, bold: false);

            AddImage(TL("SwRec", card, 18f, 114f, 22f, 6f), DPPSpriteFactory.RoundedR3,
                DPPTheme.Hex("#1f77b4"), sliced: true);
            AddText(TL("HdRec", card, 48f, 110f, 260f, 14f), "MAX RECOVERY RATE", 9.5f,
                DPPTheme.Hex("#1f77b4"), bold: true);
            string[] psRecLines =
            {
                "The share that survives the Scenario 4 route once the",
                "material arrives in the right stream. LINEAR 0-100 %.",
                "Source: Bigum et al. 2012, Table 8.   0 % = credited in",
                "no scenario, which is an answer, not a gap.",
            };
            for (int i = 0; i < psRecLines.Length; i++)
                AddText(TL($"RecL{i}", card, 48f, 126f + i * 12f, 290f, 12f), psRecLines[i],
                    8.5f, DPPTheme.TextSecondary, bold: false);

            AddText(TL("DimNote", card, 18f, 180f, 200f, 12f), "Drawing dimensions in mm.", 8f,
                DPPTheme.Hex("#6f86a8"), bold: false);

            // Grey, not teal: dismissing an explanation is not the page's primary
            // action, and "Back" is the word this app uses for leaving a thing (00 §5).
            var back = PsSmallPill(card, "BackButton", 344f - 18f - 37f, 74f, "Back",
                primary: false, out _, cy: 214f - 22f);
            WireClick(back, view, nameof(ProductSpecsView.HideInfo));
            modal.gameObject.SetActive(false);

            // ---------------- wiring ----------------
            SetRef(view, "router", router);
            SetRef(view, "subIdFill", idFill);
            SetRef(view, "subIdStroke", idStroke);
            SetRef(view, "subIdLabel", idLabel);
            SetRef(view, "subCompFill", compFill);
            SetRef(view, "subCompStroke", compStroke);
            SetRef(view, "subCompLabel", compLabel);
            SetRef(view, "backLabel", backLbl);
            SetRef(view, "primaryLabel", primaryLbl);
            SetRef(view, "primaryButton", primaryBtn.gameObject);
            SetRef(view, "identityRoot", identity.gameObject);
            SetRef(view, "partsRoot", parts.gameObject);
            SetRef(view, "detailRoot", detail.gameObject);
            SetRef(view, "drawingRoot", drawing.gameObject);
            SetRef(view, "listContent", listContent);
            SetRef(view, "drawingCard", dwgCard);
            SetRef(view, "detailDrawing", detailDrawing);
            SetRef(view, "viewButton", viewSlot.gameObject);
            SetRef(view, "lowerBlock", lower);
            SetRef(view, "infoModal", modal.gameObject);
            SetRef(view, "drawingLarge", drawingLarge);

            SetRefArray(view, "identityValues", idValues);
            SetRefArray(view, "listRows", rows);
            SetRefArray(view, "listNames", names);
            SetRefArray(view, "listFills", fills);
            SetRefArray(view, "listButtons", buttons);
            SetRefArray(view, "detailRows", detailRows);
            SetRefArray(view, "matNames", matNames);
            SetRefArray(view, "matMasses", matMasses);
            SetRefArray(view, "matPriorityBars", matImp);
            SetRefArray(view, "matPriorityLabels", matImpLbl);
            SetRefArray(view, "matRecoveryBars", matRec);
            SetRefArray(view, "matRecoveryLabels", matRecLbl);
            SetRefArray(view, "matTicks", matTicks);

            // ONE SOURCE OF TRUTH for the proportional split. The view has to recompute
            // the layout at runtime, so it needs these numbers — but they are declared
            // here, and pushed, rather than typed twice and left to drift.
            SetFloat(view, "trackWidth", PsTrackW);
            SetFloat(view, "detailBandTop", PsBandTop);
            SetFloat(view, "detailBandHeight", PsBandBottom - PsBandTop);
            SetFloat(view, "detailHeadHeight", PsDetailHeadH);
            SetFloat(view, "detailRowPitch", PsMatPitch);
            SetFloat(view, "detailGap", PsDetailGap);
            SetFloat(view, "drawingMinHeight", PsDwgMinH);
            SetFloat(view, "drawingMaxHeight", PsDwgMaxH);

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
            Debug.Log("[DPPUIBuilder] RBv2_1_1/09 — Product specs tab built (420 × 430). " +
                      "Run RBv2_1_1/11 to move it into the data canvas, then Verify wiring, then SAVE THE SCENE.");
        }

        // =================================================================
        // Pieces
        // =================================================================

        // =================================================================
        // THE ELEVATION KIT (00 §4, RBv2.1.1)
        //
        // Every clickable surface in the app is built from the same three pieces,
        // in this order:
        //
        //   Shadow   a darker copy, 3 units larger and 3 lower, BEHIND everything
        //   ...      the element's own stroke / fill / icon / label
        //   Gloss    a 10 %-white sliver across the upper 40 % of the fill
        //
        // HoverHighlight then finds `Shadow` and `Fill` BY NAME and animates them,
        // which is why the names are fixed and why the helpers below are the only
        // sanctioned way to make a button. Hand-rolling one still works — it just
        // will not respond to hover.
        // =================================================================

        /// <summary>
        /// The cast shadow. Call FIRST, before any other child, so it sits behind.
        /// Sized from the element's VISUAL box, not its hit box — a shadow around
        /// an invisible 50-unit hit area is a grey halo in mid-air.
        /// </summary>
        private static Image AddShadow(RectTransform root, float w, float h,
            string sprite = null, float capsuleHeight = 0f)
        {
            var img = AddImage(TLCenter("Shadow", root, root.sizeDelta.x * 0.5f,
                    root.sizeDelta.y * 0.5f + 3f, w + 3f, h + 3f),
                sprite, new Color(0f, 0f, 0f, 0.32f), sliced: sprite != null);
            if (capsuleHeight > 0f) Capsule(img, capsuleHeight);
            return img;
        }

        /// <summary>Above this height an element is treated as a SURFACE rather
        /// than a button, and its sheen becomes a thin top-edge highlight.</summary>
        private const float GlossSmallMax = 40f;

        /// <summary>
        /// The sheen. Call AFTER the fill so it sits on top of it, and BEFORE the
        /// label so it never washes out text. Inset 12 units: a highlight that
        /// reaches the edge reads as a second border, not as light.
        ///
        /// ⚠ IT DOES NOT SCALE WITH THE ELEMENT, and it must not. Thiago,
        /// 2026-08-06: *"the glow in the tabs might be too much, creating a big grey
        /// section… the small buttons are okay."* 40 % of a 34-unit pill is a 14-unit
        /// band across a curved top — light on a curve. 40 % of a 170-unit role card
        /// is a 68-unit grey slab, because a big flat card has no curve for the light
        /// to fall on; only its top EDGE is lit.
        ///
        /// So the sheen is proportional below <see cref="GlossSmallMax"/> and a
        /// capped 12-unit strip hugging the top edge above it, at half the alpha.
        /// </summary>
        private static Image AddGloss(RectTransform root, float w, float h,
            string sprite = null, float capsuleHeight = 0f, bool subtle = false)
        {
            // `subtle` forces the large treatment on something short. A list row is
            // 30 high, so it counts as small — but its label is LEFT-aligned and sits
            // across the upper half, where a 12-unit 10 % band washes it out. A
            // button's label is centred and short, so the same band misses it. Height
            // alone cannot tell those apart; the caller can.
            bool small = h <= GlossSmallMax && !subtle;
            float gh    = small ? h * 0.40f : Mathf.Min(h * 0.18f, 12f);
            float alpha = small ? 0.10f : 0.05f;
            float top   = (root.sizeDelta.y - h) * 0.5f;
            // Small: centred on the upper third. Large: pinned 3 under the top edge.
            float cy    = small ? top + h * 0.29f : top + 3f + gh * 0.5f;

            var img = AddImage(TLCenter("Gloss", root, root.sizeDelta.x * 0.5f, cy,
                    Mathf.Max(8f, w - 12f), gh),
                sprite, new Color(1f, 1f, 1f, alpha), sliced: sprite != null);
            if (capsuleHeight > 0f) Capsule(img, capsuleHeight);
            return img;
        }

        /// <summary>
        /// A child that STRETCHES with its parent, inset by the given margins (a
        /// NEGATIVE margin expands past the parent's edge — that is how the card's
        /// 1-unit stroke is drawn).
        ///
        /// ⚠ The drawing card is RESIZED AT RUNTIME by ProductSpecsView.LayoutDetail.
        /// Anything positioned beside it rather than anchored to it stays where it was
        /// while the card grows, which reads as the drawing sliding out of its frame.
        /// </summary>
        private static RectTransform Inset(string name, Transform parent,
            float left, float top, float right, float bottom)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(left, bottom);
            rt.offsetMax = new Vector2(-right, -top);
            return rt;
        }

        /// <summary>
        /// Make an Image render as a TRUE CAPSULE at an arbitrary height.
        ///
        /// ⚠ `DPPSpriteFactory.Pill` is NOT a 9-sliced sprite. It is a 400 × 44
        /// rounded rect with r22 and a ZERO border, authored to be STRETCHED at
        /// 200 × 22 (the grabber bar). Drawn at 110 × 34 its corners stretch to
        /// 6 units wide by 17 tall — an ellipse that reads as an angular corner.
        /// That is the "not round as supposed to be" from the 2026-08-06 test.
        ///
        /// The fix uses the 9-sliced r22 sprite instead and scales its border with
        /// `pixelsPerUnitMultiplier`, which divides the slice size. Border is
        /// radius + 2 = 24 px, so a multiplier of 24 / (h / 2) lands the corner
        /// radius exactly on half the height — the definition of a capsule — at any
        /// height, with one sprite.
        /// </summary>
        private static Image Capsule(Image img, float visualHeight)
        {
            img.sprite = DPPSpriteFactory.Load(DPPSpriteFactory.RoundedR22);
            img.type = Image.Type.Sliced;
            img.pixelsPerUnitMultiplier = 24f / Mathf.Max(1f, visualHeight * 0.5f);
            return img;
        }

        /// <summary>One of the two equal header pills. Both are <see cref="PsPillW"/>
        /// wide: unequal widths read as a heading beside a button rather than as a
        /// pair of alternatives.</summary>
        private static Button PsSubTab(RectTransform parent, string name, float x, string label,
            out Image fill, out Image stroke, out TMP_Text text)
        {
            var root = TL(name, parent, x, PsPillY, PsPillW, PsPillH);
            AddShadow(root, PsPillW, PsPillH, capsuleHeight: PsPillH + 3f);

            var outline = Capsule(AddImage(CenterIn("HoverOutline", root, PsPillW + HoverHalo, PsPillH + HoverHalo),
                null, Color.white), PsPillH + HoverHalo);
            outline.gameObject.SetActive(false);

            stroke = Capsule(AddImage(CenterIn("Stroke", root, PsPillW, PsPillH), null,
                DPPTheme.Hex("#21407a")), PsPillH);
            fill = Capsule(AddImage(CenterIn("Fill", root, PsPillW - 2f, PsPillH - 2f), null,
                DPPTheme.RowFill, raycast: true), PsPillH - 2f);
            AddGloss(root, PsPillW, PsPillH, capsuleHeight: PsPillH * 0.40f);
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
            string label, bool primary, out TMP_Text text,
            float cy = PsBtnCy, bool destructive = false,
            float visualH = PsBtnVisualH, float fontSize = 12.5f)
        {
            // ⚠ THE HIT ROOT STAYS PsBtnHitH WHATEVER visualH IS. Shrinking a button
            // means shrinking what is DRAWN; shrinking what can be pressed at 0.75 m
            // turns every press into a near-miss (00 §4.2). `View` draws at 26 and is
            // still hit at 50.
            var root = TLCenter(name, parent, cx, cy, w, PsBtnHitH);

            AddShadow(root, w, visualH, capsuleHeight: visualH + 3f);

            var outline = Capsule(AddImage(CenterIn("HoverOutline", root, w + HoverHalo, visualH + HoverHalo),
                null, Color.white), visualH + HoverHalo);
            outline.gameObject.SetActive(false);

            bool filled = primary || destructive;
            if (!filled)
                Capsule(AddImage(CenterIn("Stroke", root, w, visualH), null,
                    DPPTheme.Hex("#324a6d")), visualH);

            // Destructive = the session-ending action (00 §2.1 meaning 3): SOLID
            // red with a white bold label, never a red outline — an outline on a
            // dark fill reads as a warning state rather than something to press.
            Color fillColour = destructive ? DPPTheme.SafetyStroke
                             : primary     ? DPPTheme.TealAccent
                                           : DPPTheme.Hex("#1a2740");
            float fw = filled ? w : w - 2f, fh = filled ? visualH : visualH - 2f;
            var fill = Capsule(AddImage(CenterIn("Fill", root, fw, fh), null, fillColour, raycast: true), fh);

            AddGloss(root, w, visualH, capsuleHeight: visualH * 0.40f);

            text = AddText(Stretch("Label", root), label, fontSize,
                filled ? DPPTheme.TextOnNavy : DPPTheme.TextSecondary,
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
                // Width is set by what the ROW BECOMES WHEN IT RISES, not by what it
                // is at rest. 00 §4 scales a hovered element by 1.03 and drops a
                // shadow 3 further; on a 366-wide row that pushes the edges ~8 units
                // out, past the RectMask2D, and the mask slices them off — the
                // "overlapping the margin" of 2026-08-06.
                //
                // Two changes, because either alone is not enough: the row is
                // narrower, AND wide elements get a gentler rise. 1.03 is a few units
                // on a button and eleven on a full-width row — a scale factor is the
                // wrong unit for something this wide.
                float rowW = PsRowW;
                var row = TL($"Row{i}", content, (PsContentW - rowW) * 0.5f, i * PsRowPitch, rowW, PsRowH);
                rows[i] = row;

                AddShadow(row, rowW, PsRowH, DPPSpriteFactory.RoundedR13);

                var outline = AddImage(CenterIn("HoverOutline", row, rowW + HoverHalo, PsRowH + HoverHalo),
                    DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
                outline.gameObject.SetActive(false);

                fills[i] = AddImage(Stretch("Fill", row), DPPSpriteFactory.RoundedR13,
                    DPPTheme.RowFill, sliced: true, raycast: true);
                AddGloss(row, rowW, PsRowH, DPPSpriteFactory.RoundedR13, subtle: true);
                names[i] = AddText(TL("Name", row, 16f, 6f, rowW - 32f, 18f), "—", 12f,
                    DPPTheme.TextOnNavy, bold: false);

                var btn = row.gameObject.AddComponent<Button>();
                btn.transition = Selectable.Transition.None;
                btn.targetGraphic = fills[i];
                buttons[i] = btn;

                var hover = row.gameObject.AddComponent<HoverHighlight>();
                SetRef(hover, "highlightOutline", outline.gameObject);
                SetFloat(hover, "hoverScale", 1.015f);   // wide element, see the width note above

                row.gameObject.SetActive(false);
            }

            var scroll = viewport.gameObject.AddComponent<PinchScrollArea>();
            SetRef(scroll, "viewport", viewport);
            SetRef(scroll, "content", content);
            return content;
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
