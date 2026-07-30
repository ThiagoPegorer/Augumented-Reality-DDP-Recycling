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
    /// RBv2_0/7 — the two passport screens (specs 13 v2 + 14 v2, mocks
    /// drafts/13_v2_C_dpp_canva.svg and drafts/14_v4_composition_impact.svg).
    ///
    ///   DppCanva          — Identity hero + four declaration tiles. Back → Welcome.
    ///                       Continue → Composition &amp; impact.
    ///   ModelExploration  — three blocks: composition by material, climate across the
    ///                       four EoL scenarios, recovery rate per impact category.
    ///                       Shown beside the exploded action zone. Back → DppCanva.
    ///   ContinueGateCanvas — "Continue to disassembly?" · Quit / Continue.
    ///
    /// DATA: one <see cref="PassportView"/> on DppCanva owns the bindings for BOTH
    /// screens; SetRef happily points it at objects inside ModelExploration. Element
    /// counts are data-driven, so this builder creates POOLS (composition segments,
    /// legend entries, spec chips) and the view shows/sizes only what the payload has.
    /// DPPManager.passport is re-pointed at it.
    ///
    /// PLACEHOLDER STRINGS: every literal in this file is an Editor placeholder that
    /// Populate() overwrites. Spec 13 v2 §4 — no hardcoded data survives a fetch.
    /// RBv1.0 shipped three static subtitles and two of them disagreed with the payload.
    ///
    /// Run RBv2_0/1 first (needs DPPPanelCanvas), then RBv2_0/2 → /6, then this.
    /// Safe to re-run. ⚠ It deletes any leftover RBv1.0 "InformationTab".
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // Geometry — MUST match the constants in PassportView.
        private const float CompBarW = 548f;
        private const int   CompSegmentPool = 12;
        private const int   CompLegendPool = 5;
        private const int   SpecChipPool = 6;
        private const float ScenarioBarH = 52f;
        private const float RecoveryTrackW = 240f;

        [MenuItem("RBv2_0/7 — DPP Canva + Model Exploration", false, 7)]
        public static void Build7_DppCanvaAndExploration()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_0/1 first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();
            if (router == null)
            {
                Debug.LogError("[DPPUIBuilder] No ScreenRouter on DPPPanelCanvas — re-run RBv2_0/1.");
                return;
            }

            var welcome = FindAnyIncludingInactive<WelcomeController>();
            if (welcome == null)
                Debug.LogWarning("[DPPUIBuilder] No WelcomeController — run RBv2_0/3, then re-run this to wire Back and Quit.");

            DestroyChild(canvasRT, "DppCanva");
            DestroyChild(canvasRT, "ModelExploration");
            RemoveByName("ContinueGateCanvas");

            var oldInfoTab = canvasRT.Find("InformationTab");
            if (oldInfoTab != null)
            {
                Undo.DestroyObjectImmediate(oldInfoTab.gameObject);
                Debug.Log("[DPPUIBuilder] Removed the leftover RBv1.0 'InformationTab'.");
            }

            var dotFilled = DPPSpriteFactory.Load(DPPSpriteFactory.Circle64);
            var dotRing   = DPPSpriteFactory.Load(DPPSpriteFactory.CircleRing);

            // =================================================================
            // Screen A — DPP CANVA
            // =================================================================
            var canva = Stretch("DppCanva", canvasRT);
            Undo.RegisterCreatedObjectUndo(canva.gameObject, "Build DPP Canva");
            var view = canva.gameObject.AddComponent<PassportView>();
            var canvaRouter = canva.gameObject.AddComponent<PassportRouter>();
            AddImage(Stretch("PanelBG", canva), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var landing = Stretch("Landing", canva);
            MakeScreenHeader(landing, "Digital Product Passport", "Vehicle Control Unit",
                rightCaption: null,
                backTarget: welcome, backMethod: welcome != null ? nameof(WelcomeController.ShowWelcome) : null);

            BuildIdentityHero(landing, view, canvaRouter);

            // Four declaration tiles. statusDots/statusTexts are ONE flat array of 8
            // (2 rows per tile) so the view can address them by index; the compliance
            // tile spends its first row on tri-state badges, so index 3 stays null.
            var dots = new Image[8];
            var texts = new TMP_Text[8];

            var tSub = MakeTileCard(landing, 24, 192, "SubstancesCard", DPPSpriteFactory.IcWarning,
                "Substances & safety", canvaRouter, nameof(PassportRouter.Open2), rows: 2);
            dots[0] = tSub.dots[0]; texts[0] = tSub.texts[0];
            dots[1] = tSub.dots[1]; texts[1] = tSub.texts[1];

            var tCom = MakeTileCard(landing, 326, 192, "ComplianceCard", DPPSpriteFactory.IcShield,
                "Compliance & certification", canvaRouter, nameof(PassportRouter.Open3), rows: 1, firstRowY: 48);
            dots[2] = tCom.dots[0]; texts[2] = tCom.texts[0];
            dots[3] = null; texts[3] = null;                      // no slot on the face
            BuildComplianceBadges(tCom.card, view);

            var tSvc = MakeTileCard(landing, 24, 272, "ServiceCard", DPPSpriteFactory.IcWrench,
                "Service & repair", canvaRouter, nameof(PassportRouter.Open4), rows: 2);
            dots[4] = tSvc.dots[0]; texts[4] = tSvc.texts[0];
            dots[5] = tSvc.dots[1]; texts[5] = tSvc.texts[1];

            var tUse = MakeTileCard(landing, 326, 272, "UsageCard", DPPSpriteFactory.IcClock,
                "Usage & repair history", canvaRouter, nameof(PassportRouter.Open5), rows: 2);
            dots[6] = tUse.dots[0]; texts[6] = tUse.texts[0];
            dots[7] = tUse.dots[1]; texts[7] = tUse.texts[1];

            SetRefArray(view, "statusDots", dots);
            SetRefArray(view, "statusTexts", texts);

            // Legend + forward CTA. No ●/○ glyphs — 00 §3 keeps to the SF Pro atlas.
            AddText(TL("LegendLine1", landing, 24, 366, 250, 16),
                "Filled dot = declared,", 11, DPPTheme.TextTip, bold: false);
            AddText(TL("LegendLine2", landing, 24, 382, 250, 16),
                "ring = not provided", 11, DPPTheme.TextTip, bold: false);
            var toModel = BuildWideCta(landing, "ContinueButton", x: 288, y: 354, w: 328, label: "Continue");
            WireClick(toModel, router, nameof(ScreenRouter.ShowModelExploration));

            // Detail shells — chrome only, bodies deliberately unbuilt.
            var dIdentity   = MakeShellPage(canva, canvaRouter, "IdentityDetail",   "Identity & specifications", DPPSpriteFactory.IcPerson);
            var dSubstances = MakeShellPage(canva, canvaRouter, "SubstancesDetail", "Substances & safety",       DPPSpriteFactory.IcWarning);
            var dCompliance = MakeShellPage(canva, canvaRouter, "ComplianceDetail", "Compliance & certification",DPPSpriteFactory.IcShield);
            var dService    = MakeShellPage(canva, canvaRouter, "ServiceDetail",    "Service & repair",          DPPSpriteFactory.IcWrench);
            var dUsage      = MakeShellPage(canva, canvaRouter, "UsageDetail",      "Usage & repair history",    DPPSpriteFactory.IcClock);
            SetRef(canvaRouter, "landing", landing.gameObject);
            SetRef(canvaRouter, "detail1", dIdentity.gameObject);
            SetRef(canvaRouter, "detail2", dSubstances.gameObject);
            SetRef(canvaRouter, "detail3", dCompliance.gameObject);
            SetRef(canvaRouter, "detail4", dService.gameObject);
            SetRef(canvaRouter, "detail5", dUsage.gameObject);

            // =================================================================
            // Screen B — COMPOSITION & IMPACT
            // =================================================================
            var explore = Stretch("ModelExploration", canvasRT);
            Undo.RegisterCreatedObjectUndo(explore.gameObject, "Build Composition and Impact");
            var exploreRouter = explore.gameObject.AddComponent<PassportRouter>();
            AddImage(Stretch("PanelBG", explore), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var exLanding = Stretch("Landing", explore);
            MakeScreenHeader(exLanding, "Digital Product Passport", "Composition & impact",
                rightCaption: null, backTarget: router, backMethod: nameof(ScreenRouter.ShowDppCanva));
            var countCaption = AddText(TL("CountCaption", exLanding, 316, 36, 300, 16),
                "660 g", 12.5f, DPPTheme.TextCaption, bold: false, align: TextAlignmentOptions.MidlineRight);
            SetRef(view, "componentCountCaption", countCaption);

            BuildCompositionBlock(exLanding, view, exploreRouter);
            BuildScenarioBlock(exLanding, view, exploreRouter);
            BuildRecoveryBlock(exLanding, view, exploreRouter);

            AddText(TL("HintLine1", exLanding, 24, 366, 330, 16),
                "Both hands pinching: twist to rotate, pull apart", 11, DPPTheme.TextTip, bold: false);
            AddText(TL("HintLine2", exLanding, 24, 380, 330, 16),
                "to zoom. No timer yet.", 11, DPPTheme.TextTip, bold: false);
            var toGate = BuildWideCta(exLanding, "ContinueButton", x: 288, y: 354, w: 328, label: "Continue");

            var xMaterial = MakeShellPage(explore, exploreRouter, "MaterialLocationDetail", "Material location per component", DPPSpriteFactory.IcLayers);
            var xLifecycle = MakeShellPage(explore, exploreRouter, "LifecycleDetail", "Life-cycle process detail", DPPSpriteFactory.IcLeaf);
            var xRecovery = MakeShellPage(explore, exploreRouter, "RecoveryDetail", "Recovery detail", DPPSpriteFactory.IcLeaf);
            SetRef(exploreRouter, "landing", exLanding.gameObject);
            SetRef(exploreRouter, "detail1", xMaterial.gameObject);
            SetRef(exploreRouter, "detail2", xLifecycle.gameObject);
            SetRef(exploreRouter, "detail3", xRecovery.gameObject);

            // =================================================================
            // Gate + wiring
            // =================================================================
            var gateGO = BuildContinueGateCanvas(out var gate, out var quitBtn, out var continueBtn);
            SetRef(gate, "router", router);
            SetRef(gate, "welcome", welcome);
            WireClick(quitBtn, gate, nameof(ContinueGate.Quit));
            WireClick(continueBtn, gate, nameof(ContinueGate.Continue));
            WireClick(toGate, gate, nameof(ContinueGate.Show));

            SetRef(view, "dotFilledSprite", dotFilled);
            SetRef(view, "dotRingSprite", dotRing);

            SetRef(router, "dppCanva", canva.gameObject);
            SetRef(router, "modelExploration", explore.gameObject);
            SetBool(router, "zoneFollowsExploration", true);

            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "passport", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in scene — the passport screens are not bound to backend data.");

            if (FindAnyIncludingInactive<ExplodedZoneInteraction>() == null)
                Debug.LogWarning("[DPPUIBuilder] No ExplodedZoneInteraction — run RBv2_0/5 so the exploration screen has a model beside it.");

            canva.gameObject.SetActive(false);
            explore.gameObject.SetActive(false);
            gateGO.SetActive(false);

            Selection.activeGameObject = canva.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_0/7 — DPP Canva + Composition & impact + gate built. " +
                      "Detail shells are chrome-only by design. Save the scene.");
        }

        // =================================================================
        // Identity hero (spec 13 v2 §2) — 592 × 96, full width
        // =================================================================
        private static void BuildIdentityHero(RectTransform landing, PassportView view, PassportRouter r)
        {
            var card = TL("IdentityHero", landing, 24, 88, 592, 96);
            const float W = 592f, H = 96f;

            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Stroke", card, W + 2, H + 2), DPPSpriteFactory.RoundedR13, DPPTheme.TabActiveStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, W, H), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            AddImage(TLCenter("Icon", card, 26, 24, 22, 22), DPPSpriteFactory.IcPerson, DPPTheme.Hex("#7fd3b6"));
            AddText(TL("Title", card, 48, 12, 300, 20), "Identity & specifications", 15, DPPTheme.TextOnNavy, bold: true);
            var cat = AddText(TL("Category", card, 300, 12, 250, 20), "EEE", 11, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.MidlineRight);
            SetRef(view, "categoryCaption", cat);

            var line = AddText(TL("IdentityLine", card, 26, 34, 540, 18), "manufacturer", 13, DPPTheme.TextOnNavy, bold: false);
            SetRef(view, "identityLine", line);

            // Spec chip pool — the view sizes each chip to its text and hides the rest.
            var chipRow = TL("SpecChips", card, 26, 56, 540, 20);
            var chipRoots = new RectTransform[SpecChipPool];
            var chipLabels = new TMP_Text[SpecChipPool];
            for (int i = 0; i < SpecChipPool; i++)
            {
                chipRoots[i] = TL($"Chip{i}", chipRow, 0, 0, 80, 20);
                AddImage(Stretch("Fill", chipRoots[i]), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);
                chipLabels[i] = AddText(Stretch("Label", chipRoots[i]), "—", 10.5f, DPPTheme.Hex("#dbe4f0"),
                    bold: false, align: TextAlignmentOptions.Center);
                chipRoots[i].gameObject.SetActive(false);
            }
            SetRefArray(view, "specChipRoots", chipRoots);
            SetRefArray(view, "specChipLabels", chipLabels);

            var dot = AddImage(TLCenter("DocDot", card, 31, 87, 7, 7), DPPSpriteFactory.Circle64, DPPTheme.TextTip);
            SetRef(view, "docStatusDot", dot);
            var docLine = AddText(TL("DocStatus", card, 40, 80, 530, 14),
                "documents", 10.5f, DPPTheme.TextTip, bold: false);
            SetRef(view, "docStatusLine", docLine);

            AddChevron(card, 566, 22);
            MakeTappable(card, fill, outline, r, nameof(PassportRouter.Open1));
        }

        // =================================================================
        // Declaration tile — 290 × 72 (spec 13 v2 §2)
        // =================================================================
        private struct TileParts
        {
            public RectTransform card;
            public Image[] dots;
            public TMP_Text[] texts;
        }

        private static TileParts MakeTileCard(RectTransform landing, float x, float y, string name,
            string iconSprite, string title, PassportRouter r, string openMethod,
            int rows, float firstRowY = 32f)
        {
            const float W = 290f, H = 72f;
            var card = TL(name, landing, x, y, W, H);

            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Stroke", card, W + 2, H + 2), DPPSpriteFactory.RoundedR13, DPPTheme.RowStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, W, H), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            AddImage(TLCenter("Icon", card, 26, 36, 22, 22), iconSprite, DPPTheme.Hex("#7fd3b6"));
            AddText(TL("Title", card, 48, 10, 224, 20), title, 14, DPPTheme.TextOnNavy, bold: true);

            var dots = new Image[rows];
            var texts = new TMP_Text[rows];
            for (int i = 0; i < rows; i++)
            {
                float ry = firstRowY + i * 16f;
                dots[i] = AddImage(TLCenter($"Dot{i}", card, 53, ry + 8, 7, 7), DPPSpriteFactory.Circle64, DPPTheme.TextTip);
                texts[i] = AddText(TL($"Row{i}", card, 62, ry, 200, 16), "—", 11, DPPTheme.TextSecondary, bold: false);
            }

            AddChevron(card, 266, 36);
            MakeTappable(card, fill, outline, r, openMethod);
            return new TileParts { card = card, dots = dots, texts = texts };
        }

        /// <summary>Tri-state CE / RoHS / REACH badges on the compliance tile's first row.</summary>
        private static void BuildComplianceBadges(RectTransform card, PassportView view)
        {
            string[] labels = { "CE", "RoHS", "REACH" };
            float[] xs = { 48f, 86f, 136f };
            float[] ws = { 34f, 46f, 52f };
            var strokes = new Image[3];
            var texts = new TMP_Text[3];
            for (int i = 0; i < 3; i++)
            {
                var b = TL($"Badge{i}", card, xs[i], 30, ws[i], 16);
                strokes[i] = AddImage(Stretch("Stroke", b), DPPSpriteFactory.Pill, DPPTheme.TextTip, sliced: true);
                AddImage(CenterIn("Fill", b, ws[i] - 2f, 14f), DPPSpriteFactory.Pill, DPPTheme.RowFill, sliced: true);
                texts[i] = AddText(Stretch("Label", b), labels[i], 9.5f, DPPTheme.TextTip,
                    bold: false, align: TextAlignmentOptions.Center);
            }
            SetRefArray(view, "complianceBadgeStrokes", strokes);
            SetRefArray(view, "complianceBadgeLabels", texts);
        }

        // =================================================================
        // Block 1 — composition by material (spec 14 v2 §3), 592 × 100
        // =================================================================
        private static void BuildCompositionBlock(RectTransform page, PassportView view, PassportRouter r)
        {
            var card = TL("CompositionBlock", page, 24, 88, 592, 100);
            const float W = 592f, H = 100f;

            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Stroke", card, W + 2, H + 2), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, W, H), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            AddImage(TLCenter("Icon", card, 26, 24, 22, 22), DPPSpriteFactory.IcLayers, DPPTheme.Hex("#7fd3b6"));
            AddText(TL("Title", card, 48, 12, 300, 20), "Materials & composition", 15, DPPTheme.TextOnNavy, bold: true);
            var trace = AddText(TL("TraceMetals", card, 300, 12, 250, 20), "—", 11, DPPTheme.TealText,
                bold: false, align: TextAlignmentOptions.MidlineRight);
            SetRef(view, "traceMetalsLine", trace);

            // Segment pool. Left-anchored children of a 548-wide track; the view sets
            // each x and width from the derived material totals.
            var bar = TL("Bar", card, 26, 32, CompBarW, 24);
            var segs = new RectTransform[CompSegmentPool];
            var segImgs = new Image[CompSegmentPool];
            for (int i = 0; i < CompSegmentPool; i++)
            {
                segs[i] = LeftAnchored($"Seg{i}", bar, 10f, 24f);
                segImgs[i] = AddImage(segs[i], DPPSpriteFactory.RoundedR3, DPPTheme.TabActiveStroke, sliced: true);
                segs[i].gameObject.SetActive(false);
            }
            SetRefArray(view, "compositionSegments", segs);
            SetRefArray(view, "compositionSegmentImages", segImgs);

            var inline = AddText(TL("InlineLabel", card, 32, 36, 220, 16), "—", 11, DPPTheme.TextOnNavy, bold: true);
            SetRef(view, "compositionInlineLabel", inline);

            var swatches = new Image[CompLegendPool];
            var labels = new TMP_Text[CompLegendPool];
            for (int i = 0; i < CompLegendPool; i++)
            {
                float lx = 26f + i * 116f;
                swatches[i] = AddImage(TL($"Swatch{i}", card, lx, 66, 8, 8), null, DPPTheme.TabActiveStroke);
                labels[i] = AddText(TL($"LegendLabel{i}", card, lx + 14, 60, 100, 14), "—", 11, DPPTheme.TextLabel, bold: false);
            }
            SetRefArray(view, "legendSwatches", swatches);
            SetRefArray(view, "legendLabels", labels);

            var footer = AddText(TL("Footer", card, 26, 82, 540, 14), "—", 10.5f, DPPTheme.TextTip, bold: false);
            SetRef(view, "compositionFooter", footer);

            AddChevron(card, 566, 22);
            MakeTappable(card, fill, outline, r, nameof(PassportRouter.Open1));
        }

        // =================================================================
        // Block 2 — climate across the four EoL scenarios (spec 14 v2 §4), 290 × 138
        // =================================================================
        private static void BuildScenarioBlock(RectTransform page, PassportView view, PassportRouter r)
        {
            var card = TL("ScenarioBlock", page, 24, 196, 290, 138);
            const float W = 290f, H = 138f;

            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Stroke", card, W + 2, H + 2), DPPSpriteFactory.RoundedR13, DPPTheme.RowStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, W, H), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            AddText(TL("Title", card, 26, 14, 224, 20), "Climate · EoL scenarios", 13.5f, DPPTheme.TextOnNavy, bold: true);
            var caption = AddText(TL("Caption", card, 26, 34, 240, 16), "—", 11, DPPTheme.TextLabel, bold: false);
            SetRef(view, "scenarioCaption", caption);

            AddImage(TL("Axis", card, 26, 106, 248, 1), null, DPPTheme.RowStroke);

            // Each column is a 52-high slot whose BOTTOM sits on the axis. Bars are
            // bottom-anchored inside it, so the view grows them upward: total height
            // always = the baseline scenario, and the teal cap IS the saving. Four
            // zero-based bars would look near-identical (73.4 / 69.1 / 65.2 / 58.0).
            var nets = new RectTransform[4];
            var savings = new RectTransform[4];
            var values = new TMP_Text[4];
            var axis = new TMP_Text[4];
            for (int i = 0; i < 4; i++)
            {
                float cx = 38f + i * 58f;
                var slot = TL($"Slot{i}", card, cx, 106f - ScenarioBarH, 34f, ScenarioBarH);
                nets[i] = BottomAnchored($"Net{i}", slot, 34f, ScenarioBarH);
                AddImage(nets[i], null, DPPTheme.TabInactiveFill);
                savings[i] = BottomAnchored($"Saving{i}", slot, 34f, 0f);
                AddImage(savings[i], null, DPPTheme.TealLight);
                savings[i].gameObject.SetActive(false);

                values[i] = AddText(TLCenter($"Value{i}", card, cx + 17, 46, 44, 14), "—", 10,
                    DPPTheme.Hex("#dbe4f0"), bold: false, align: TextAlignmentOptions.Center);
                axis[i] = AddText(TLCenter($"Axis{i}", card, cx + 17, 116, 44, 14), "—", 10,
                    DPPTheme.TextLabel, bold: false, align: TextAlignmentOptions.Center);
            }
            SetRefArray(view, "scenarioNetBars", nets);
            SetRefArray(view, "scenarioSavingBars", savings);
            SetRefArray(view, "scenarioValues", values);
            SetRefArray(view, "scenarioAxisLabels", axis);

            AddText(TL("Footer", card, 26, 124, 248, 12),
                "use phase caps what EoL can do for carbon", 9.5f, DPPTheme.TextTip, bold: false);

            AddChevron(card, 266, 22);
            MakeTappable(card, fill, outline, r, nameof(PassportRouter.Open2));
        }

        // =================================================================
        // Block 3 — recovery rate per impact category (spec 14 v2 §5), 290 × 138
        // =================================================================
        private static void BuildRecoveryBlock(RectTransform page, PassportView view, PassportRouter r)
        {
            var card = TL("RecoveryBlock", page, 326, 196, 290, 138);
            const float W = 290f, H = 138f;

            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Stroke", card, W + 2, H + 2), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, W, H), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            AddText(TL("Title", card, 26, 14, 200, 20), "Recovery rate", 13.5f, DPPTheme.TextOnNavy, bold: true);

            var catLabels = new TMP_Text[3];
            var totals = new TMP_Text[3];
            var s2 = new RectTransform[3];
            var s3 = new RectTransform[3];
            var s4 = new RectTransform[3];
            Color[] shade = { DPPTheme.TealText, DPPTheme.TealLight, DPPTheme.TealAccent };
            for (int i = 0; i < 3; i++)
            {
                float ry = 34f + i * 30f;
                catLabels[i] = AddText(TL($"Cat{i}", card, 26, ry, 150, 14), "—", 10.5f, DPPTheme.TextLabel, bold: false);
                totals[i] = AddText(TL($"Total{i}", card, 176, ry, 90, 14), "—", 10.5f, DPPTheme.TextSecondary,
                    bold: true, align: TextAlignmentOptions.MidlineRight);

                var track = TL($"Track{i}", card, 26, ry + 14, RecoveryTrackW, 9);
                AddImage(track, DPPSpriteFactory.RoundedR3, DPPTheme.ScrollTrack, sliced: true);
                var segs = new RectTransform[3];
                for (int s = 0; s < 3; s++)
                {
                    segs[s] = LeftAnchored($"Seg{s + 2}", track, 0f, 9f);
                    AddImage(segs[s], DPPSpriteFactory.RoundedR3, shade[s], sliced: true);
                    segs[s].gameObject.SetActive(false);
                }
                s2[i] = segs[0]; s3[i] = segs[1]; s4[i] = segs[2];
            }
            SetRefArray(view, "recoveryCategoryLabels", catLabels);
            SetRefArray(view, "recoveryTotals", totals);
            SetRefArray(view, "recoverySeg2", s2);
            SetRefArray(view, "recoverySeg3", s3);
            SetRefArray(view, "recoverySeg4", s4);

            // Scenario legend — the labels are copy, the scenario ids come from data.
            string[] legend = { "Sc2 usual", "Sc3 dismantle", "Sc4 + reuse*" };
            float[] lx = { 26f, 96f, 184f };
            for (int i = 0; i < 3; i++)
            {
                AddImage(TL($"LegSwatch{i}", card, lx[i], 122, 8, 8), null, shade[i]);
                AddText(TL($"LegLabel{i}", card, lx[i] + 12, 116, 90, 14), legend[i], 9.5f, DPPTheme.TextTip, bold: false);
            }

            AddChevron(card, 266, 22);
            MakeTappable(card, fill, outline, r, nameof(PassportRouter.Open3));
        }

        // =================================================================
        // Detail shell — back arrow + icon + title + "not built yet" line.
        // Bodies come with the modal build round.
        // =================================================================
        private static RectTransform MakeShellPage(RectTransform screen, PassportRouter router,
            string name, string title, string iconSprite)
        {
            var page = Stretch(name, screen);

            var back = TLCenter("BackButton", page, 42, 44, 40, 40);
            var outline = AddImage(CenterIn("HoverOutline", back, 50, 50), DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", back, 43, 43), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var fill = AddImage(CenterIn("Fill", back, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", back, 22, 22), DPPSpriteFactory.IcBack, Color.white);

            var btn = back.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            WireClick(btn, router, nameof(PassportRouter.Back));
            var hover = back.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);

            AddImage(TLCenter("TitleIcon", page, 88, 44, 20, 20), iconSprite, DPPTheme.Hex("#7fd3b6"));
            AddText(TL("Title", page, 102, 31, 480, 26), title, 19, DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Separator", page, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));
            AddText(TL("Placeholder", page, 24, 110, 592, 20),
                "Full entry is not built yet.", 13, DPPTheme.TextTip, bold: false);

            page.gameObject.SetActive(false);
            return page;
        }

        // =================================================================
        // Screen header (RBv2.0) — also used by RBv2_0/4 (disassembly intro).
        // =================================================================
        private static void MakeScreenHeader(RectTransform parent, string eyebrow, string title,
            string rightCaption, Object backTarget, string backMethod)
        {
            var back = TLCenter("BackButton", parent, 42, 44, 40, 40);
            var outline = AddImage(CenterIn("HoverOutline", back, 50, 50), DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", back, 43, 43), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var fill = AddImage(CenterIn("Fill", back, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", back, 22, 22), DPPSpriteFactory.IcBack, Color.white);

            var btn = back.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            if (backTarget != null && backMethod != null) WireClick(btn, backTarget, backMethod);
            else Debug.LogWarning($"[DPPUIBuilder] Back button on '{parent.name}' left unwired.");

            var hover = back.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);

            AddText(TL("Eyebrow", parent, 76, 24, 300, 15), eyebrow, 11.5f, DPPTheme.TextCaption, bold: false);
            AddText(TL("Title", parent, 76, 40, 440, 24), title, 19, DPPTheme.TextOnNavy, bold: true);

            if (!string.IsNullOrEmpty(rightCaption))
                AddText(TL("RightCaption", parent, 316, 36, 300, 16), rightCaption, 12.5f,
                    DPPTheme.TextCaption, bold: false, align: TextAlignmentOptions.MidlineRight);

            AddImage(TL("Separator", parent, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));
        }

        // =================================================================
        // Gate modal — own root canvas, 440 × 210 (spec 14 §6)
        // =================================================================
        private static GameObject BuildContinueGateCanvas(out ContinueGate gate,
            out Button quitBtn, out Button continueBtn)
        {
            const float W = 440f, H = 210f;

            var go = new GameObject("ContinueGateCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingOrder = 10;

            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(W, H);
            rt.position = CanvasPos + new Vector3(0f, 0f, -0.05f);
            rt.localScale = Vector3.one * CanvasScale;

            gate = go.AddComponent<ContinueGate>();

            var card = Stretch("Card", rt);
            AddImage(Stretch("Stroke", card), DPPSpriteFactory.RoundedR20, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", card, W - 4f, H - 4f), DPPSpriteFactory.RoundedR20, DPPTheme.TabActiveFill, sliced: true);

            AddText(TLCenter("Title", card, 220, 52, 400, 26),
                "Continue to disassembly?", 18, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Subtitle1", card, 220, 80, 400, 20),
                "The guided 5-step dismantling comes next.", 13, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Subtitle2", card, 220, 98, 400, 20),
                "Timing starts when you press Start disassembly.", 13, DPPTheme.TextSecondary,
                bold: false, align: TextAlignmentOptions.Center);

            // "Quit", not "Back": this edge leaves the product session entirely — the one
            // deliberate break in the one-step-back hierarchy (Thiago, 2026-07-29).
            quitBtn = BuildPillButton(card, "QuitButton", cx: 119, cy: 156, w: 190, h: 52,
                label: "Quit", labelSize: 15, primary: false, chevron: false);
            continueBtn = BuildPillButton(card, "ContinueButton", cx: 329, cy: 156, w: 190, h: 52,
                label: "Continue", labelSize: 15, primary: true, chevron: true);

            BuildGrabberBar(rt);
            var handle = go.GetComponentInChildren<PanelGrabHandle>(true);
            if (handle != null) SetBool(handle, "recenterOnStart", false); // ContinueGate.Show places it

            Undo.RegisterCreatedObjectUndo(go, "Build Continue Gate");
            return go;
        }

        // =================================================================
        // Shared bits
        // =================================================================

        /// <summary>Wide primary CTA (teal pill + chevron) anchored top-left in spec coords.</summary>
        private static Button BuildWideCta(RectTransform parent, string name,
            float x, float y, float w, string label)
        {
            const float H = 52f;
            var root = TL(name, parent, x, y, w, H);

            var outline = AddImage(CenterIn("HoverOutline", root, w + 10f, H + 10f),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            var fill = AddImage(CenterIn("Fill", root, w, H), DPPSpriteFactory.RoundedR13,
                DPPTheme.TealAccent, sliced: true, raycast: true);

            AddText(Stretch("Label", root), label, 16, DPPTheme.TextOnNavy,
                bold: true, align: TextAlignmentOptions.Center);

            // Chevron from two capsule bars — the SF Pro SDF atlas has no glyph (00 §3).
            // anchoredPosition.y is UP, so the UPPER bar tilts -45.
            float cx = w * 0.5f - 26f;
            CtaChevronBar(root, "ChevronTop", cx, 4f, -45f);
            CtaChevronBar(root, "ChevronBottom", cx, -4f, 45f);

            var button = root.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = fill;

            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", root);

            return button;
        }

        private static void CtaChevronBar(RectTransform parent, string name, float dx, float dy, float zRot)
        {
            var bar = CenterIn(name, parent, 13f, 2.5f);
            bar.anchoredPosition = new Vector2(dx, dy);
            bar.localRotation = Quaternion.Euler(0f, 0f, zRot);
            AddImage(bar, DPPSpriteFactory.Grip, Color.white);
        }

        /// <summary>Right-pointing chevron at card-local (cx, cy).</summary>
        private static void AddChevron(RectTransform card, float cx, float cy)
        {
            var ch = AddImage(TLCenter("Chevron", card, cx, cy, 16, 10), DPPSpriteFactory.IcChevron, DPPTheme.TextSecondary);
            ch.rectTransform.localEulerAngles = new Vector3(0, 0, 90); // point right (navigate)
        }

        private static void MakeTappable(RectTransform card, Image fill, Image outline,
            PassportRouter router, string method)
        {
            var btn = card.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            WireClick(btn, router, method);

            var hover = card.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", card);
        }

        /// <summary>Child anchored to its parent's LEFT edge, vertically centred, growing
        /// rightwards. The view sets anchoredPosition.x and sizeDelta.x — used by every
        /// horizontal bar segment.</summary>
        private static RectTransform LeftAnchored(string name, Transform parent, float w, float h)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = rt.anchorMax = new Vector2(0f, 0.5f);
            rt.pivot = new Vector2(0f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(w, h);
            return rt;
        }

        /// <summary>Child anchored to its parent's BOTTOM edge, growing upwards. Used by
        /// the scenario columns so a height change reads as growth from the axis.</summary>
        private static RectTransform BottomAnchored(string name, Transform parent, float w, float h)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(w, h);
            return rt;
        }

        /// <summary>FindFirstObjectByType misses inactive objects — Welcome and the zone
        /// are both inactive most of the time, so always include them.</summary>
        private static T FindAnyIncludingInactive<T>() where T : Component
            => Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);

        private static void DestroyChild(RectTransform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null) Undo.DestroyObjectImmediate(child.gameObject);
        }
    }
}
