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
    /// RBv2_0/Legacy — the two passport screens (specs 13 v2 + 14 v2, mocks
    /// drafts/13_v2_C_dpp_canva.svg and drafts/14_v4_composition_impact.svg).
    ///
    ///   DppCanva          — Identity hero + four declaration tiles. Back → Welcome.
    ///                       Continue → Composition &amp; impact.
    ///   ModelExploration  — three blocks: composition by material, climate across the
    ///                       four EoL scenarios, recovery rate per impact category.
    ///                       Shown beside the exploded action zone. Back → DppCanva.
    ///                       "Continue to disassembly" → the intro, no gate between
    ///                       (interstitial gate removed 2026-08-01).
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
    /// Run RBv2_1/1 first (needs DPPPanelCanvas), then RBv2_1/2 → /6, then this.
    /// Safe to re-run. ⚠ It deletes any leftover RBv1.0 "InformationTab".
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // Geometry — MUST match the constants in PassportView.
        private const float CompBarW = 548f;
        private const int   CompSegmentPool = 12;
        private const int   CompLegendPool = 5;
        private const int   SpecChipPool = 6;
        private const int   ElecChipPool = 2;    // v10 - supply voltage + component count
        private const int   UsageChipPool = 2;   // v11 - years + km
        private const int   UsageYearPool = 18;  // v11 - annual distance rows (16 + spare)
        private const int   UsageStatCount = 6;  // v11 - right-column stats
        private const int   CompChipPool  = 2;   // v12 - CE + SVHC chips
        private const int   CompStatCount = 6;   // v12 - right-column entries
        private const int   SvcChipPool   = 2;   // v13 - updates + repairs chips
        private const int   SvcRepairPool = 27;  // v13 - repair rows (25 + spare)
        private const int   SvcUpdatePool = 62;  // v13 - update rows (60 + spare)
        private const float ScenarioBarH = 52f;
        private const float RecoveryTrackW = 240f;

        [MenuItem("RBv2_0/Legacy — DPP Canva + Model Exploration (superseded by RBv2_1/8)", false, 90)]
        public static void Build7_DppCanvaAndExploration()
        {
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
            {
                Debug.LogError("[DPPUIBuilder] No ScreenRouter on DPPPanelCanvas — re-run RBv2_0/1.");
                return;
            }

            var welcome = FindAnyIncludingInactive<WelcomeController>();
            if (welcome == null)
                Debug.LogWarning("[DPPUIBuilder] No WelcomeController — run RBv2_1/3, then re-run this to wire Back and Quit.");

            DestroyChild(canvasRT, "DppCanva");
            DestroyChild(canvasRT, "ModelExploration");
            RemoveByName("ContinueGateCanvas"); // gate retired 2026-08-01 — this clears pre-removal scenes

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
            // v3 (2026-07-30): no back arrow and no 19 pt product title on this screen.
            // Back moved to a labelled "Home" pill in the bottom bar, and the
            // caption slides left into the arrow's slot. Reclaims 28 px of height.
            MakeCaptionHeader(landing, "Digital Product Passport");

            BuildProductInfoBlock(landing, view);

            // v13 grid — EVERY tile is a chip tile now; the statusDots/statusTexts
            // pools (2 rows per tile, spec 13 v2) are gone with the last dot-row face.
            //
            // ⚠ Substances of concern render on Compliance & Safety since v12 (the Bosch
            // REACH statement); the old substances tile computation left with the array.

            // Mechanical data — full width, spec chips, inert card, "+" opens detail1.
            // v14.1 (2026-08-01): "+" HIDDEN — the Mechanical detail page is deferred
            // to RB2.1, so the face carries the chips and nothing opens.
            var tMec = MakeTileCard(landing, 24, 100, "MechanicalCard", DPPSpriteFactory.IcCube,
                "Mechanical data", canvaRouter, null,
                rows: 0, w: 592f, plusButton: true, heroStroke: true, pngIcon: "ic_mechanical");
            BuildSpecChipPool(tMec.card, view);

            // v10: no status row — the component count became a chip beside the voltage.
            // v14.1: "+" HIDDEN — Electrical detail page deferred to RB2.1.
            var tEle = MakeTileCard(landing, 24, 180, "ElectricalCard", DPPSpriteFactory.IcBolt,
                "Electrical data", canvaRouter, null,
                rows: 0, plusButton: true, pngIcon: "ic_electrical");
            BuildFaceChipPool(tEle.card, view, "ElecChips", "elecChipRoots", "elecChipLabels", ElecChipPool);

            // v13: Service & repair joins the chip family — 60 updates · 25 repairs.
            var tSvc = MakeTileCard(landing, 326, 180, "ServiceCard", DPPSpriteFactory.IcWrench,
                "Service & repair", canvaRouter, nameof(PassportRouter.Open4), rows: 0, plusButton: true, pngIcon: "ic_service");
            BuildFaceChipPool(tSvc.card, view, "SvcChips", "svcChipRoots", "svcChipLabels", SvcChipPool);

            // v11: Usage Profile — chip tile like Electrical; no status rows.
            var tUse = MakeTileCard(landing, 24, 260, "UsageCard", DPPSpriteFactory.IcClock,
                "Usage Profile", canvaRouter, nameof(PassportRouter.Open5), rows: 0, plusButton: true, pngIcon: "ic_usage");
            BuildFaceChipPool(tUse.card, view, "UsageChips", "usageChipRoots", "usageChipLabels", UsageChipPool);

            // v12: Compliance & Safety — chip tile; the tri-state badges died with the
            // old face (RoHS is not false, it is OUT OF SCOPE for means of transport).
            var tCom = MakeTileCard(landing, 326, 260, "ComplianceCard", DPPSpriteFactory.IcShield,
                "Compliance & Safety", canvaRouter, nameof(PassportRouter.Open3), rows: 0, plusButton: true, pngIcon: "ic_compliance");
            BuildFaceChipPool(tCom.card, view, "CompChips", "compChipRoots", "compChipLabels", CompChipPool);

            // Bottom bar (v3) — secondary LEFT, primary RIGHT, the order Welcome, the
            // first-run prompt and the disassembly gate all use. 180 + 24 + 388 = 592,
            // exactly the content width.
            //
            // The dot legend that used to sit bottom-left is GONE. It was a third
            // encoding of the same fact: PassportView.SetRow already writes the words
            // ("— not provided") into the row AND dims it to text/tip. Nothing is lost.
            var toWelcome = BuildWideCta(landing, "HomeButton", x: 24, y: 354, w: 180,
                label: "Home", primary: false, chevron: false);
            if (welcome != null) WireClick(toWelcome, welcome, nameof(WelcomeController.ShowWelcome));
            else Debug.LogWarning("[DPPUIBuilder] Home button left unwired — no WelcomeController in the scene.");

            var toModel = BuildWideCta(landing, "ContinueButton", x: 228, y: 354, w: 388, label: "Continue");
            WireClick(toModel, router, nameof(ScreenRouter.ShowModelExploration));

            // Detail shells — chrome only, bodies deliberately unbuilt.
            // v4: no IdentityDetail shell — the product info block is not tappable.
            // v8 built Mechanical/Electrical shells here; v14.1 removed them — their
            // "+" is hidden until RB2.1, and an unreachable placeholder page is scene
            // clutter. detail1/detail2 stay null; PassportRouter null-guards.
            var dCompliance = MakeShellPage(canva, canvaRouter, "ComplianceDetail", "Compliance & Safety", null,
                                            showIcon: false, placeholder: false);
            BuildComplianceDetail(dCompliance, view);
            var dService    = MakeShellPage(canva, canvaRouter, "ServiceDetail",    "Service & repair", null,
                                            showIcon: false, placeholder: false);
            BuildServiceDetail(dService, view);
            var dUsage      = MakeShellPage(canva, canvaRouter, "UsageDetail", "Usage Profile", null,
                                            showIcon: false, placeholder: false);
            BuildUsageDetail(dUsage, view);
            SetRef(canvaRouter, "landing", landing.gameObject);
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
            // The interstitial gate is gone (Thiago, 2026-08-01) — the CTA now names
            // its destination and jumps straight to the disassembly intro.
            var toDisassembly = BuildWideCta(exLanding, "ContinueButton", x: 288, y: 354, w: 328, label: "Continue to disassembly");

            var xMaterial = MakeShellPage(explore, exploreRouter, "MaterialLocationDetail", "Material location per component", DPPSpriteFactory.IcLayers);
            var xLifecycle = MakeShellPage(explore, exploreRouter, "LifecycleDetail", "Life-cycle process detail", DPPSpriteFactory.IcLeaf);
            var xRecovery = MakeShellPage(explore, exploreRouter, "RecoveryDetail", "Recovery detail", DPPSpriteFactory.IcLeaf);
            SetRef(exploreRouter, "landing", exLanding.gameObject);
            SetRef(exploreRouter, "detail1", xMaterial.gameObject);
            SetRef(exploreRouter, "detail2", xLifecycle.gameObject);
            SetRef(exploreRouter, "detail3", xRecovery.gameObject);

            // =================================================================
            // Audio + wiring
            // =================================================================
            // ---- UI click audio (P02 feedback, 2026-08-01): one sweep object ----
            RemoveByName("UIClickAudio");
            var audioGO = new GameObject("UIClickAudio", typeof(AudioSource));
            var clickAudio = audioGO.AddComponent<UIClickAudio>();
            var clickClip = AssetDatabase.LoadAssetAtPath<AudioClip>(UIClickClipPath);
            if (clickClip == null)
            {
                AssetDatabase.Refresh();
                clickClip = AssetDatabase.LoadAssetAtPath<AudioClip>(UIClickClipPath);
            }
            if (clickClip != null) SetRef(clickAudio, "clip", clickClip);
            else Debug.LogWarning($"[DPPUIBuilder] {UIClickClipPath} not found — UI clicks will be silent.");
            Undo.RegisterCreatedObjectUndo(audioGO, "Build UI click audio");

            // ---- per-hand pinch audio (P02 feedback): water-drop down + ripple hold ----
            RemoveByName("HandPinchAudio");
            var pinchGO = new GameObject("HandPinchAudio");
            var pinchAudio = pinchGO.AddComponent<HandPinchAudio>();
            WireClip(pinchAudio, "pinchRight", AudioDir + "pinch_right.wav");
            WireClip(pinchAudio, "pinchLeft", AudioDir + "pinch_left.wav");
            WireClip(pinchAudio, "dragLoopRight", AudioDir + "drag_loop_right.wav");
            WireClip(pinchAudio, "dragLoopLeft", AudioDir + "drag_loop_left.wav");
            Undo.RegisterCreatedObjectUndo(pinchGO, "Build hand pinch audio");

            // No gate between exploration and disassembly (2026-08-01): the CTA
            // says where it goes and goes there. RemoveByName above still clears
            // any ContinueGateCanvas a previous build left in the scene.
            WireClick(toDisassembly, router, nameof(ScreenRouter.ShowDisassembly));

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

            Selection.activeGameObject = canva.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_0/Legacy — DPP Canva + Composition & impact + gate built. " +
                      "Detail shells are chrome-only by design. Save the scene.");
        }

        // =================================================================
        // Product information (spec 13 v4) — plain text, NOT a card.
        //
        // v4 (2026-07-30, Thiago): "This tab will not be a button, I just want to
        // plot the name." The tappable Identity & specifications card is therefore
        // gone entirely — no fill, no stroke, no hover outline, no icon, no chevron,
        // no MakeTappable. A non-interactive block must not wear the same costume as
        // the four tiles below it, or the tiles stop reading as tappable.
        //
        // DROPPED WITH THE CARD, and homeless until a later tab claims them:
        //   * the 5 spec chips (size · weight · protection class · supply voltage ·
        //     operating temperature). PassportView.FillChips returns early when
        //     specChipRoots is unwired, so nothing breaks — it renders nothing.
        //   * the product_category caption ("EEE — electronic control unit …")
        //   * the documents status line (the Table 6 #1/#2 not-applicable statement)
        //   * production date and country of origin, which rode on the identity line
        //
        // A "Product information" label was drawn once and cut (Thiago): the screen
        // already says "Digital Product Passport" 14 px above this line.
        //
        // The IdentityDetail shell is no longer built and detail1 stays null.
        // PassportRouter null-guards every slot, so the now-unreachable Open1()
        // degrades to Back() rather than throwing.
        // =================================================================
        private static void BuildProductInfoBlock(RectTransform landing, PassportView view)
        {
            // "manufacturer | model" — bound in PassportView, never a literal here.
            var name = AddText(TL("ProductName", landing, 24, 62, 592, 22),
                "manufacturer | model", 16, DPPTheme.TextOnNavy, bold: true);
            SetRef(view, "identityLine", name);

            // v4.1: the serial moves ONTO the name's line — "… MS 50.4 - VCU0001" —
            // while keeping its own 11 pt text/tip styling.
            //
            // TWO TMP objects, not one with rich text: AddText assigns the dedicated
            // BOLD font asset (_fontBold) for the name, and no rich-text tag can switch
            // a font ASSET back off, so an inline serial would render bold. Same rect y
            // and height (62, 22) so the two midlines coincide; PassportView slides this
            // one right by the measured width of the name.
            var serial = AddText(TL("SerialNumber", landing, 24, 62, 200, 22),
                "serial", 11, DPPTheme.TextTip, bold: false);
            SetRef(view, "serialLine", serial);
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
            int rows, float firstRowY = 32f, float w = 290f, bool plusButton = false,
            bool heroStroke = false, string pngIcon = null)
        {
            const float H = 72f;
            var card = TL(name, landing, x, y, w, H);

            // v8: plusButton tiles are INERT cards — only the circle is clickable
            // (Thiago, 2026-07-31). They therefore get no card-wide hover outline and no
            // MakeTappable; the circle carries both. ⚠ This is an affordance split: the
            // other tiles respond anywhere, these two respond on one 52 px circle.
            Image outline = null;
            if (!plusButton)
            {
                outline = AddImage(CenterIn("HoverOutline", card, w + HoverHalo, H + HoverHalo),
                    DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
                outline.gameObject.SetActive(false);
            }
            AddImage(CenterIn("Stroke", card, w + 2, H + 2), DPPSpriteFactory.RoundedR13,
                heroStroke ? DPPTheme.TabActiveStroke : DPPTheme.RowStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, w, H), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true, raycast: true);

            // v14: the tab icons are Thiago's authored PNGs (DPP_UI_Specs/Icons, trimmed
            // + squared at import). Drawn UNTINTED — they carry their own green, the same
            // no-recolour decision as the brand logo. Generated-sprite fallback keeps the
            // never-blank rule when the PNG is missing on a fresh clone.
            var iconRT = TLCenter("Icon", card, 26, 36, 22, 22);
            var tileIcon = pngIcon != null ? LoadTileIcon(pngIcon) : null;
            if (tileIcon != null)
            {
                var iconImg = iconRT.gameObject.AddComponent<Image>();
                iconImg.sprite = tileIcon;
                iconImg.preserveAspect = true;
                iconImg.raycastTarget = false;
            }
            else
            {
                if (pngIcon != null)
                    Debug.LogWarning($"[DPPUIBuilder] Tile icon '{pngIcon}' not found in {TileIconDir} — using the generated glyph.");
                AddImage(iconRT, iconSprite, DPPTheme.Hex("#7fd3b6"));
            }
            AddText(TL("Title", card, 48, 10, w - 90f, 20), title, 14, DPPTheme.TextOnNavy, bold: true);

            var dots = new Image[rows];
            var texts = new TMP_Text[rows];
            for (int i = 0; i < rows; i++)
            {
                float ry = firstRowY + i * 16f;
                dots[i] = AddImage(TLCenter($"Dot{i}", card, 53, ry + 8, 7, 7), DPPSpriteFactory.Circle64, DPPTheme.TextTip);
                // Narrower when a "+" sits at the right edge, so a long row can never
                // slide under the circle.
                texts[i] = AddText(TL($"Row{i}", card, 62, ry, plusButton ? 168f : 200f, 16),
                    "—", 11, DPPTheme.TextSecondary, bold: false);
            }

            // v14.1: openMethod == null -> a fully INERT tile — no "+", no chevron,
            // no tap. Used while a tile's detail page is deferred (RB2.1): an absent
            // control is honest; a "+" onto "Full entry is not built yet." is a dead
            // end a study participant will walk into.
            if (openMethod == null) { /* chips only */ }
            else if (plusButton) BuildPlusButton(card, w - 30f, 36f, r, openMethod);
            else
            {
                AddChevron(card, w - 24f, 36f);
                MakeTappable(card, fill, outline, r, openMethod);
            }
            return new TileParts { card = card, dots = dots, texts = texts };
        }

        /// <summary>Circular "+" that opens a detail page. 40 px visual inside a 52 px hit
        /// rect — 5.2 cm at panel scale, ~5° at 0.6 m, comfortably above hand-ray jitter and
        /// over the 00 §4 minimum. Same stroke/fill/hover recipe as the screen-header back
        /// button so the two read as the same class of control.</summary>
        private static Button BuildPlusButton(RectTransform card, float cx, float cy,
            PassportRouter router, string method)
        {
            var root = TLCenter("PlusButton", card, cx, cy, 52, 52);
            var outline = AddImage(CenterIn("HoverOutline", root, 46, 46), DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", root, 43, 43), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var fill = AddImage(CenterIn("Fill", root, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", root, 20, 20), DPPSpriteFactory.IcPlus, Color.white);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            WireClick(btn, router, method);

            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        /// <summary>Spec chip pool on the Mechanical data row — the SAME pool the identity
        /// hero owned in v2/v3, so PassportView.FillChips still drives it and every value
        /// stays bound. Supply voltage deliberately left out: it belongs to Electrical data.</summary>
        private static void BuildSpecChipPool(RectTransform card, PassportView view)
        {
            // y 41, not 30: the title rect ends at 30 and the card at 72, so a 20 px
            // chip row at 41 leaves 11 px above and 11 px below (Thiago, 2026-07-31 —
            // the chips were hard against the headline with a 22 px hole underneath).
            var chipRow = TL("SpecChips", card, 48, 41, 500, 20);
            var roots = new RectTransform[SpecChipPool];
            var labels = new TMP_Text[SpecChipPool];
            for (int i = 0; i < SpecChipPool; i++)
            {
                roots[i] = TL($"Chip{i}", chipRow, 0, 0, 80, 20);
                AddImage(Stretch("Fill", roots[i]), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);
                labels[i] = AddText(Stretch("Label", roots[i]), "—", 10.5f, DPPTheme.Hex("#dbe4f0"),
                    bold: false, align: TextAlignmentOptions.Center);
                roots[i].gameObject.SetActive(false);
            }
            SetRefArray(view, "specChipRoots", roots);
            SetRefArray(view, "specChipLabels", labels);
        }

        /// <summary>Face chip pool — the SAME widget on every chip tile (Mechanical uses
        /// its own historical pool; Electrical and Usage Profile share this). Chip row at
        /// y 41 centres title + chips in the 72 px card. v11 generalised from the
        /// electrical-only builder; the field names are parameters so one method serves
        /// any tile.</summary>
        private static void BuildFaceChipPool(RectTransform card, PassportView view,
            string rowName, string rootsField, string labelsField, int pool)
        {
            var chipRow = TL(rowName, card, 48, 41, 200, 20);
            var roots = new RectTransform[pool];
            var labels = new TMP_Text[pool];
            for (int i = 0; i < pool; i++)
            {
                roots[i] = TL($"Chip{i}", chipRow, 0, 0, 80, 20);
                AddImage(Stretch("Fill", roots[i]), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);
                labels[i] = AddText(Stretch("Label", roots[i]), "—", 10.5f, DPPTheme.Hex("#dbe4f0"),
                    bold: false, align: TextAlignmentOptions.Center);
                roots[i].gameObject.SetActive(false);
            }
            SetRefArray(view, rootsField, roots);
            SetRefArray(view, labelsField, labels);
        }

        // =================================================================
        // Compliance & Safety detail (spec 13d) — Usage Profile family.
        //
        // Everything on this page comes off the signed Bosch EC/EU Declaration of
        // Conformity (Operation Manual VCU MS 50.4P pp. 132-134, 09 Oct 2020) —
        // the firmest source class in the passport. basis: declared throughout.
        //
        // Left: ONE scrollable card (chrome = touchable) with the DoC's further-
        // explanations + disposal text and the full SVHC names/CAS numbers, as a
        // single wrapping rich-text block whose height the view measures.
        // Right: six plain groups, titles LEFT, values CENTRED — same pattern as
        // BuildUsageDetail (kept as duplication, not shared, so the compiled and
        // device-tested Usage code stays untouched).
        // =================================================================
        private static readonly string[] CompStatTitles =
        {
            "CE CONFORMITY", "TESTED TO", "ROHS 2011/65/EU",
            "REACH SVHC", "WEEE CATEGORY", "DECLARATION"
        };

        private static void BuildComplianceDetail(RectTransform page, PassportView view)
        {
            // ---- left: scrollable declaration notes ----
            var nc = TL("NotesCard", page, 24, 88, 290, 330);
            AddImage(CenterIn("Stroke", nc, 292, 332), DPPSpriteFactory.RoundedR13, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", nc, 290, 330), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);
            AddText(TL("Head", nc, 14, 8, 180, 16), "DECLARATION NOTES", 10, DPPTheme.TealLight, bold: true);
            var docLine = AddText(TL("DocRef", nc, 14, 9, 262, 15), "—", 9, DPPTheme.TextTip,
                bold: false, align: TextAlignmentOptions.MidlineRight);

            var viewGO = new GameObject("Viewport", typeof(RectTransform), typeof(RectMask2D));
            var viewport = (RectTransform)viewGO.transform;
            viewport.SetParent(nc, false);
            viewport.anchorMin = viewport.anchorMax = new Vector2(0f, 1f);
            viewport.pivot = new Vector2(0f, 1f);
            viewport.anchoredPosition = new Vector2(14f, -28f);
            viewport.sizeDelta = new Vector2(262f, 294f);
            AddImage(Stretch("HitArea", viewport), null, new Color(0f, 0f, 0f, 0f), raycast: true);

            var contentGO = new GameObject("Content", typeof(RectTransform));
            var content = (RectTransform)contentGO.transform;
            content.SetParent(viewport, false);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0f, 294f);            // height set by the view

            var scroll = viewGO.AddComponent<PinchScrollArea>();
            SetRef(scroll, "viewport", viewport);
            SetRef(scroll, "content", content);

            // One wrapping text block, not a row pool: the notes are paragraphs of
            // varying length. AddText defaults to NoWrap — turned on here, and the
            // view sizes the content from GetPreferredValues at the fixed width.
            var notes = AddText(TL("Notes", content, 0, 2, 254, 290), "—", 9.5f,
                DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.TopLeft);
            notes.textWrappingMode = TextWrappingModes.Normal;

            // ---- right: six plain groups (same pattern as the Usage page) ----
            var statValues = new TMP_Text[CompStatCount];
            for (int i = 0; i < CompStatCount; i++)
            {
                float y = 88f + i * 57f;
                AddText(TL($"StatHead{i}", page, 326, y + 4f, 220, 15),
                    CompStatTitles[i], 9.5f, DPPTheme.TealLight, bold: true);
                statValues[i] = AddText(TL($"StatVal{i}", page, 326, y + 20f, 290, 22),
                    "—", 16, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
                if (i < CompStatCount - 1)
                    AddImage(TL($"StatRule{i}", page, 326, y + 51f, 290, 1), null, DPPTheme.Hex("#12294e"));
            }

            SetRef(view, "compNotesText", notes);
            SetRef(view, "compNotesContent", content);
            SetRef(view, "compDocLine", docLine);
            SetRefArray(view, "compStatValues", statValues);
        }

        // =================================================================
        // Block 1 — composition by material (spec 14 v2 §3), 592 × 100
        // =================================================================
        private static void BuildCompositionBlock(RectTransform page, PassportView view, PassportRouter r)
        {
            var card = TL("CompositionBlock", page, 24, 88, 592, 100);
            const float W = 592f, H = 100f;

            var outline = AddImage(CenterIn("HoverOutline", card, W + HoverHalo, H + HoverHalo),
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

            var outline = AddImage(CenterIn("HoverOutline", card, W + HoverHalo, H + HoverHalo),
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

            var outline = AddImage(CenterIn("HoverOutline", card, W + HoverHalo, H + HoverHalo),
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
            string name, string title, string iconSprite,
            bool showIcon = true, string rightCaption = null, bool placeholder = true,
            string pngIcon = null)
        {
            var page = Stretch(name, screen);

            var back = TLCenter("BackButton", page, 42, 44, 40, 40);
            var outline = AddImage(CenterIn("HoverOutline", back, 46, 46), DPPSpriteFactory.Circle64, Color.white);
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

            // v9: the icon is optional. Service & repair drops it (Thiago, 2026-07-31 —
            // the wrench read as a magnifying glass) and the title slides into its slot.
            // ⚠ The other four shells still show one; this is the odd header out.
            if (showIcon)
            {
                var shellIconRT = TLCenter("TitleIcon", page, 88, 44, 20, 20);
                var shellIcon = pngIcon != null ? LoadTileIcon(pngIcon) : null;   // v14: match the tile
                if (shellIcon != null)
                {
                    var img = shellIconRT.gameObject.AddComponent<Image>();
                    img.sprite = shellIcon;
                    img.preserveAspect = true;
                    img.raycastTarget = false;
                }
                else AddImage(shellIconRT, iconSprite, DPPTheme.Hex("#7fd3b6"));
            }
            AddText(TL("Title", page, showIcon ? 102 : 76, 31, 480, 26), title, 19, DPPTheme.TextOnNavy, bold: true);
            if (!string.IsNullOrEmpty(rightCaption))
                AddText(TL("RightCaption", page, 316, 36, 300, 16), rightCaption, 10.5f,
                    DPPTheme.Hex("#e2a44a"), bold: false, align: TextAlignmentOptions.MidlineRight);
            AddImage(TL("Separator", page, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));
            if (placeholder)
                AddText(TL("Placeholder", page, 24, 110, 592, 20),
                    "Full entry is not built yet.", 13, DPPTheme.TextTip, bold: false);

            page.gameObject.SetActive(false);
            return page;
        }

        // =================================================================
        // Slim header (v3) — caption + rule, no arrow, no product title.
        //
        // DPP CANVA LANDING ONLY. MakeScreenHeader below is shared with the
        // disassembly intro (RBv2_0/4) and the Composition & impact landing, and
        // MakeShellPage builds its own copy of the same arrow, so the arrow must
        // NOT be stripped there — those screens still need a one-step-back edge.
        // Here the back edge lives in the bottom bar as "Home".
        //
        // Caption sits at x 24 (the arrow's old slot) and the rule at y 48 instead
        // of 76, which is the 28 px handed to the tab block.
        // =================================================================
        private static void MakeCaptionHeader(RectTransform parent, string caption)
        {
            AddText(TL("Eyebrow", parent, 24, 24, 300, 15), caption, 11.5f, DPPTheme.TextCaption, bold: false);
            AddImage(TL("Separator", parent, 24, 48, 592, 1), null, DPPTheme.Hex("#1a335f"));
        }

        // =================================================================
        // Screen header (RBv2.0) — also used by RBv2_0/4 (disassembly intro).
        // =================================================================
        private static void MakeScreenHeader(RectTransform parent, string eyebrow, string title,
            string rightCaption, Object backTarget, string backMethod)
        {
            var back = TLCenter("BackButton", parent, 42, 44, 40, 40);
            var outline = AddImage(CenterIn("HoverOutline", back, 46, 46), DPPSpriteFactory.Circle64, Color.white);
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
        // Service & repair detail (spec 13e) — 1 x 2, two scrollable histories.
        //
        // v13 replaces the v9/v10 page (counter cards, timeline card, 5-row log)
        // wholesale: both columns are pinch-scrollable cards — the only chrome,
        // and both genuinely touchable. LEFT: 25 repairs as two-line rows (none
        // in service years 1-5, rising to 3/yr toward end of life). RIGHT: 60
        // quarterly updates v1.1 (Apr 2011) -> v15.4 (Jan 2026). Everything ends
        // before the Mar 2026 retirement — the old fortnightly 2026 story
        // postdated it. The "→" version-range label (missing-glyph bug) is gone.
        // Counts live in the card heads and the tile chips; no stats column.
        // =================================================================
        private static void BuildServiceDetail(RectTransform page, PassportView view)
        {
            // ---- LEFT: repairs ----
            var rc = TL("RepairCard", page, 24, 88, 290, 330);
            AddImage(CenterIn("Stroke", rc, 292, 332), DPPSpriteFactory.RoundedR13, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", rc, 290, 330), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);
            var repHead = AddText(TL("Head", rc, 14, 8, 180, 16), "REPAIRS", 10, DPPTheme.TealLight, bold: true);
            var repMeta = AddText(TL("Meta", rc, 14, 9, 262, 15), "", 9, DPPTheme.TextTip,
                bold: false, align: TextAlignmentOptions.MidlineRight);

            var repViewport = MakeScrollWindow(rc, out var repContent);
            var repRows = new RectTransform[SvcRepairPool];
            var repDates = new TMP_Text[SvcRepairPool];
            var repDescs = new TMP_Text[SvcRepairPool];
            for (int i = 0; i < SvcRepairPool; i++)
            {
                var row = TL($"RepairRow{i}", repContent, 0, i * 34f, 262, 34);
                repRows[i] = row;
                AddImage(TLCenter("Dot", row, 6, 10, 7, 7), DPPSpriteFactory.Circle64, DPPTheme.Hex("#e2a44a"));
                repDates[i] = AddText(TL("Date", row, 18, 2, 240, 15), "—", 10.5f, DPPTheme.TextCaption, bold: false);
                repDescs[i] = AddText(TL("Desc", row, 18, 17, 244, 14), "—", 9.5f, DPPTheme.TextSecondary, bold: false);
                repDescs[i].overflowMode = TextOverflowModes.Truncate;
                AddImage(TL("Rule", row, 0, 33, 262, 1), null, DPPTheme.Hex("#16335f"));
                row.gameObject.SetActive(false);
            }

            // ---- RIGHT: software updates ----
            var uc = TL("UpdateCard", page, 326, 88, 290, 330);
            AddImage(CenterIn("Stroke", uc, 292, 332), DPPSpriteFactory.RoundedR13, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", uc, 290, 330), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);
            var upHead = AddText(TL("Head", uc, 14, 8, 200, 16), "SOFTWARE UPDATES", 10, DPPTheme.TealLight, bold: true);
            var upMeta = AddText(TL("Meta", uc, 14, 9, 262, 15), "", 9, DPPTheme.TextTip,
                bold: false, align: TextAlignmentOptions.MidlineRight);

            var upViewport = MakeScrollWindow(uc, out var upContent);
            var upRows = new RectTransform[SvcUpdatePool];
            var upDates = new TMP_Text[SvcUpdatePool];
            var upVersions = new TMP_Text[SvcUpdatePool];
            for (int i = 0; i < SvcUpdatePool; i++)
            {
                var row = TL($"UpdateRow{i}", upContent, 0, i * 25f, 262, 25);
                upRows[i] = row;
                AddImage(TLCenter("Dot", row, 6, 12, 7, 7), DPPSpriteFactory.Circle64, DPPTheme.TealLight);
                upDates[i] = AddText(TL("Date", row, 18, 4, 160, 16), "—", 10.5f, DPPTheme.TextCaption, bold: false);
                upVersions[i] = AddText(TL("Version", row, 0, 4, 262, 16), "—", 10.5f, DPPTheme.TextOnNavy,
                    bold: true, align: TextAlignmentOptions.MidlineRight);
                AddImage(TL("Rule", row, 0, 24, 262, 1), null, DPPTheme.Hex("#16335f"));
                row.gameObject.SetActive(false);
            }

            SetRef(view, "svcRepairHead", repHead);
            SetRef(view, "svcRepairMeta", repMeta);
            SetRef(view, "svcRepairContent", repContent);
            SetRefArray(view, "svcRepairRows", repRows);
            SetRefArray(view, "svcRepairDates", repDates);
            SetRefArray(view, "svcRepairDescs", repDescs);
            SetRef(view, "svcUpdateHead", upHead);
            SetRef(view, "svcUpdateMeta", upMeta);
            SetRef(view, "svcUpdateContent", upContent);
            SetRefArray(view, "svcUpdateRows", upRows);
            SetRefArray(view, "svcUpdateDates", upDates);
            SetRefArray(view, "svcUpdateVersions", upVersions);
        }

        /// <summary>The standard 262 x 294 masked, pinch-scrollable window inside a
        /// 290 x 330 list card — the same construction the Usage year list and the
        /// Compliance notes use, extracted now that FOUR cards build it.</summary>
        private static RectTransform MakeScrollWindow(RectTransform card, out RectTransform content)
        {
            var viewGO = new GameObject("Viewport", typeof(RectTransform), typeof(RectMask2D));
            var viewport = (RectTransform)viewGO.transform;
            viewport.SetParent(card, false);
            viewport.anchorMin = viewport.anchorMax = new Vector2(0f, 1f);
            viewport.pivot = new Vector2(0f, 1f);
            viewport.anchoredPosition = new Vector2(14f, -28f);
            viewport.sizeDelta = new Vector2(262f, 294f);
            AddImage(Stretch("HitArea", viewport), null, new Color(0f, 0f, 0f, 0f), raycast: true);

            var contentGO = new GameObject("Content", typeof(RectTransform));
            content = (RectTransform)contentGO.transform;
            content.SetParent(viewport, false);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0f, 294f);

            var scroll = viewGO.AddComponent<PinchScrollArea>();
            SetRef(scroll, "viewport", viewport);
            SetRef(scroll, "content", content);
            return viewport;
        }

        // =================================================================
        // Usage Profile detail (spec 13c, rev G) — 6 x 2 grid.
        //
        // Left: ONE full-height card holding the km-per-year list. It is the only
        // chrome on the page because it is the only touchable thing (chrome =
        // touchable — Thiago's collected feedback, 2026-07-31). PinchScrollArea
        // scrolls it; 11 full rows visible, the clipped 12th is the affordance.
        //
        // Right: six plain stat groups, titles LEFT, values CENTRED (cx 471),
        // hairline separators, red full-width bar under Total distance = design
        // life consumed, unit due for recycling (first state-use of red here).
        //
        // Content spans y 88..418 — 12 px below the rule, 12 px above the panel
        // edge, mirror-equal (rev F).
        //
        // ⚠ NO basis marker anywhere on this page (caption and estimated tag both
        // removed on instruction). The car-energy estimate is visually identical
        // to the modelled S4 values. Accepted, user-directed — spec 13c logs it.
        // =================================================================
        private static readonly string[] UsageStatTitles =
        {
            "TOTAL DISTANCE", "OPERATING HOURS", "OWN ENERGY USE",
            "CAR ENERGY USE", "AVERAGE SPEED", "DAILY USE"
        };

        private static void BuildUsageDetail(RectTransform page, PassportView view)
        {
            // ---- left: the year list (the page's one block) ----
            var lc = TL("YearListCard", page, 24, 88, 290, 330);
            AddImage(CenterIn("Stroke", lc, 292, 332), DPPSpriteFactory.RoundedR13, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", lc, 290, 330), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);
            AddText(TL("Head", lc, 14, 8, 180, 16), "KM DRIVEN PER YEAR", 10, DPPTheme.TealLight, bold: true);
            var range = AddText(TL("Range", lc, 14, 9, 262, 15), "—", 9, DPPTheme.TextTip,
                bold: false, align: TextAlignmentOptions.MidlineRight);

            var viewGO = new GameObject("Viewport", typeof(RectTransform), typeof(RectMask2D));
            var viewport = (RectTransform)viewGO.transform;
            viewport.SetParent(lc, false);
            viewport.anchorMin = viewport.anchorMax = new Vector2(0f, 1f);
            viewport.pivot = new Vector2(0f, 1f);
            viewport.anchoredPosition = new Vector2(14f, -28f);
            viewport.sizeDelta = new Vector2(262f, 294f);
            AddImage(Stretch("HitArea", viewport), null, new Color(0f, 0f, 0f, 0f), raycast: true);

            var contentGO = new GameObject("Content", typeof(RectTransform));
            var content = (RectTransform)contentGO.transform;
            content.SetParent(viewport, false);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0f, 294f);        // height set by the view

            var scroll = viewGO.AddComponent<PinchScrollArea>();
            SetRef(scroll, "viewport", viewport);
            SetRef(scroll, "content", content);

            var rows = new RectTransform[UsageYearPool];
            var yearLabels = new TMP_Text[UsageYearPool];
            var kmValues = new TMP_Text[UsageYearPool];
            for (int i = 0; i < UsageYearPool; i++)
            {
                var row = TL($"YearRow{i}", content, 0, i * 25f, 262, 25);
                rows[i] = row;
                yearLabels[i] = AddText(TL("Year", row, 0, 4, 150, 16), "—", 11, DPPTheme.TextSecondary, bold: false);
                kmValues[i] = AddText(TL("Km", row, 0, 4, 262, 16), "—", 11, DPPTheme.TextOnNavy,
                    bold: true, align: TextAlignmentOptions.MidlineRight);
                AddImage(TL("Rule", row, 0, 24, 262, 1), null, DPPTheme.Hex("#16335f"));
                row.gameObject.SetActive(false);
            }

            // ---- right: six plain stat groups — no cards, they are not buttons ----
            var statValues = new TMP_Text[UsageStatCount];
            for (int i = 0; i < UsageStatCount; i++)
            {
                float y = 88f + i * 57f;                       // 6 x 45 + 5 x 12 = 330
                AddText(TL($"StatHead{i}", page, 326, y + 4f, 220, 15),
                    UsageStatTitles[i], 9.5f, DPPTheme.TealLight, bold: true);
                statValues[i] = AddText(TL($"StatVal{i}", page, 326, y + 20f, 290, 22),
                    "—", 16, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
                if (i == 0)
                    AddImage(TL("LifeBar", page, 326, y + 41f, 290, 4),
                        null, DPPTheme.Hex("#e24b4a"));        // red: design life consumed
                if (i < UsageStatCount - 1)
                    AddImage(TL($"StatRule{i}", page, 326, y + 51f, 290, 1), null, DPPTheme.Hex("#12294e"));
            }

            SetRef(view, "usageRangeLabel", range);
            SetRef(view, "usageListContent", content);
            SetRefArray(view, "usageYearRows", rows);
            SetRefArray(view, "usageYearLabels", yearLabels);
            SetRefArray(view, "usageKmValues", kmValues);
            SetRefArray(view, "usageStatValues", statValues);
        }

        // =================================================================
        // Shared bits
        // =================================================================

        /// <summary>Wide CTA anchored top-left in spec coords. primary = teal pill with a
        /// chevron; secondary = dark fill inside a grey stroke and no chevron — the same
        /// treatment BuildPillButton gives Welcome's "Close app". Defaults keep every
        /// pre-v3 call site behaving exactly as before.</summary>
        private static Button BuildWideCta(RectTransform parent, string name,
            float x, float y, float w, string label, bool primary = true, bool chevron = true)
        {
            const float H = 52f;
            var root = TL(name, parent, x, y, w, H);

            var outline = AddImage(CenterIn("HoverOutline", root, w + HoverHalo, H + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            if (!primary)
                AddImage(CenterIn("Stroke", root, w + 4f, H + 4f),
                    DPPSpriteFactory.RoundedR13, DPPTheme.TabInactiveFill, sliced: true);

            var fill = AddImage(CenterIn("Fill", root, w, H), DPPSpriteFactory.RoundedR13,
                primary ? DPPTheme.TealAccent : DPPTheme.SecondaryButtonFill, sliced: true, raycast: true);

            AddText(Stretch("Label", root), label, 16,
                primary ? DPPTheme.TextOnNavy : DPPTheme.TextSecondary,
                bold: primary, align: TextAlignmentOptions.Center);

            if (chevron)
            {
                // Chevron from two capsule bars — the SF Pro SDF atlas has no glyph (00 §3).
                // anchoredPosition.y is UP, so the UPPER bar tilts -45.
                float cx = w * 0.5f - 26f;
                CtaChevronBar(root, "ChevronTop", cx, 4f, -45f);
                CtaChevronBar(root, "ChevronBottom", cx, -4f, 45f);
            }

            var button = root.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = fill;

            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", root);

            return button;
        }

        /// <summary>Loads an AudioClip and assigns it to a serialized field, warning
        /// loudly on a miss — an unwired clip is silent, and silence hides the miss.</summary>
        private static void WireClip(Object target, string field, string path)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            if (clip == null)
            {
                AssetDatabase.Refresh();
                clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            }
            if (clip != null) SetRef(target, field, clip);
            else Debug.LogWarning($"[DPPUIBuilder] {path} not found — '{field}' will be silent.");
        }

        private const string TileIconDir = "Assets/Textures/Icons/";
        private const string UIClickClipPath = "Assets/Audio/UI/ui_click.wav";   // "Buttons sound"
        private const string AudioDir = "Assets/Audio/UI/";

        /// <summary>Loads an authored tab icon, fixing its import settings on first use —
        /// the same treatment LoadBrandLogo gives the brand mark, so a fresh clone needs
        /// no manual Inspector step. Kept OUT of Assets/Textures/UI/, which
        /// DPPSpriteFactory owns and regenerates.</summary>
        private static Sprite LoadTileIcon(string name)
        {
            string path = TileIconDir + name + ".png";
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                AssetDatabase.Refresh();
                importer = AssetImporter.GetAtPath(path) as TextureImporter;
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
                importer.maxTextureSize = 256;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
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
