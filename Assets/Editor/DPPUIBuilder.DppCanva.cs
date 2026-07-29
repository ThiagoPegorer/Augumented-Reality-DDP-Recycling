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
    /// Phase 8 builder — ReBuilt v2.0 DPP routine (specs 13 + 14, mocks
    /// drafts/13_dpp_canva.svg · 14_model_exploration.svg · 14b_continue_gate_modal.svg).
    ///
    /// Splits the RBv1.0 Information tab into the two screens of the Miro
    /// journey v4, and removes the tab bar:
    ///
    ///   DppCanva          — product info. Back → Welcome. Continue → exploration.
    ///                       Landing grid of FOUR category cards (the LCA card is
    ///                       gone — LCA is now the next screen's main panel) plus
    ///                       the four full-page category modals, unchanged.
    ///   ModelExploration  — the LCA overview promoted from modal to main panel,
    ///                       shown alongside the exploded action zone.
    ///                       Back → DppCanva. Continue → the gate.
    ///   ContinueGateCanvas — "Continue to disassembly?" · Quit / Continue.
    ///
    /// DATA: ONE InfoTabView instance lives on DppCanva and owns the bindings
    /// for BOTH screens — SetRef happily points it at objects inside
    /// ModelExploration, and InfoTabView null-guards every field, so the retired
    /// lcaCardSubtitle simply stays unassigned. DPPManager.infoTab is re-pointed
    /// at it. No new view class, no second Populate path to drift.
    ///
    /// ZONE: the exploded canvas itself is NOT rebuilt here. Phase 4 still
    /// builds it; this phase only flips ScreenRouter.zoneFollowsExploration so
    /// it is raised with the exploration screen instead of the step flow.
    ///
    /// Run Phase 1 first (needs DPPPanelCanvas), then Phase 4 (the zone) and
    /// Phase 7 (Welcome). Safe to re-run: it rebuilds only its own objects.
    /// ⚠ It DELETES the RBv1.0 "InformationTab" screen — that is the split.
    /// Re-run Phase 2 to get it back.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("DPP/Build Phase 8 — DPP Canva + Model Exploration", false, 8)]
        public static void BuildPhase8()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run 'DPP → Build Phase 1 — Main Page' first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();
            if (router == null)
            {
                Debug.LogError("[DPPUIBuilder] No ScreenRouter on DPPPanelCanvas — re-run Phase 1.");
                return;
            }

            var welcome = FindAnyIncludingInactive<WelcomeController>();
            if (welcome == null)
                Debug.LogWarning("[DPPUIBuilder] No WelcomeController in the scene — run Phase 7 first, then re-run Phase 8 to wire the Back and Quit edges.");

            // ---- clear previous builds of this phase ----
            DestroyChild(canvasRT, "DppCanva");
            DestroyChild(canvasRT, "ModelExploration");
            RemoveByName("ContinueGateCanvas");

            // ---- the split: the RBv1.0 Information tab is superseded ----
            var oldInfoTab = canvasRT.Find("InformationTab");
            if (oldInfoTab != null)
            {
                Undo.DestroyObjectImmediate(oldInfoTab.gameObject);
                Debug.Log("[DPPUIBuilder] Removed the RBv1.0 'InformationTab' — split into DppCanva + ModelExploration. Re-run Phase 2 to restore it.");
            }

            // =================================================================
            // Screen A — DPP CANVA
            // =================================================================
            var canva = Stretch("DppCanva", canvasRT);
            Undo.RegisterCreatedObjectUndo(canva.gameObject, "Build DPP Canva");
            var view = canva.gameObject.AddComponent<InfoTabView>();
            var modalRouter = canva.gameObject.AddComponent<InfoTabRouter>();

            AddImage(Stretch("PanelBG", canva), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var landing = Stretch("Landing", canva);
            MakeScreenHeader(landing, "Digital Product Passport", "Vehicle Control Unit",
                rightCaption: "scanned · vcu_001",
                backTarget: welcome, backMethod: welcome != null ? nameof(WelcomeController.ShowWelcome) : null);

            BuildDppCards(landing, view, modalRouter);

            AddText(TL("HintLine1", landing, 24, 362, 250, 16),
                "Tap a card to read that part of", 12.5f, DPPTheme.TextTip, bold: false);
            AddText(TL("HintLine2", landing, 24, 378, 250, 16),
                "the passport.", 12.5f, DPPTheme.TextTip, bold: false);

            var toModel = BuildWideCta(landing, "ContinueToModelButton", x: 288, y: 352, w: 328,
                label: "Continue to 3D model");
            WireClick(toModel, router, nameof(ScreenRouter.ShowModelExploration));

            // ---- the four category modals (unchanged content, spec 02 §5.1) ----
            var identity = BuildFieldModal(canva, modalRouter, "IdentityModal", "Identity & manufacturer",
                DPPSpriteFactory.IcPerson, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("Manufacturer", "Bosch Motorsport", "manufacturerValue"),
                    ("Model", "Vehicle Control Unit MS 50.4", "modelValue"),
                    ("Type number", "F02U.V02.965-02", "typeNumberValue"),
                    ("Production", "2026-03 · DE", "productionValue"),
                    ("Specifications", "166 x 121 x 41 mm · 660 g · IP67", "specsValue"),
                    ("Service life (design)", "15 y · 225,000 km", "serviceLifeValue"),
                }, view);

            var materials = BuildFieldModal(canva, modalRouter, "MaterialsModal", "Materials & substances",
                DPPSpriteFactory.IcLayers, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("Housing", "Die-cast aluminium (AlSi) · 363 g", "housingValue"),
                    ("Connectors", "Brass (Cu-Zn) + Au/Ni plating · 58 g", "connectorsValue"),
                    ("PCB assembly", "FR-4 · Cu · ≈185 g", "pcbValue"),
                    ("Active components", "Silicon · 20 g", "activesValue"),
                    ("Precious metals", "Au 63 · Ag 251 · Pd 28 mg", "preciousValue"),
                    ("Recycled content", "—", "recycledValue"),
                }, view, tealValueField: "preciousValue");

            var hazard = BuildFieldModal(canva, modalRouter, "HazardModal", "Hazardous & safety",
                DPPSpriteFactory.IcWarning, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("Contains battery", "No", "hazardBatteryValue"),
                    ("Hazardous substances", "None documented", "hazardSubstancesValue"),
                    ("Solder", "Lead-free SnAgCu (SAC305)", "hazardSolderValue"),
                    ("WEEE treatment", "Selective treatment recommended", "hazardTreatmentValue"),
                }, view);

            var compliance = BuildFieldModal(canva, modalRouter, "ComplianceModal", "Compliance & end-of-life",
                DPPSpriteFactory.IcShield, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("CE marking", "—", "ceValue"),
                    ("RoHS", "—", "rohsValue"),
                    ("REACH", "—", "reachValue"),
                    ("WEEE category", "Cat. 5 (small equipment) - to verify", "weeeValue"),
                    ("Recycling route", "WEEE - selective treatment recommended", "routeValue"),
                }, view);

            SetRef(modalRouter, "landing", landing.gameObject);
            SetRef(modalRouter, "identityModal", identity.gameObject);
            SetRef(modalRouter, "materialsModal", materials.gameObject);
            SetRef(modalRouter, "hazardModal", hazard.gameObject);
            SetRef(modalRouter, "complianceModal", compliance.gameObject);
            // lcaModal intentionally left unassigned — the LCA is a screen now.

            // =================================================================
            // Screen B — DIGITAL MODEL EXPLORATION
            // =================================================================
            var explore = Stretch("ModelExploration", canvasRT);
            Undo.RegisterCreatedObjectUndo(explore.gameObject, "Build Model Exploration");
            AddImage(Stretch("PanelBG", explore), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            MakeScreenHeader(explore, "Digital Product Passport", "Life cycle & 3D model",
                rightCaption: null,
                backTarget: router, backMethod: nameof(ScreenRouter.ShowDppCanva));

            BuildLcaMainPanel(explore, view);

            AddText(TL("HintLine1", explore, 24, 356, 300, 16),
                "Both hands pinching: twist to rotate,", 12.5f, DPPTheme.TextTip, bold: false);
            AddText(TL("HintLine2", explore, 24, 372, 300, 16),
                "pull apart to zoom. No timer yet.", 12.5f, DPPTheme.TextTip, bold: false);

            var toGate = BuildWideCta(explore, "ContinueButton", x: 328, y: 352, w: 288, label: "Continue");

            // =================================================================
            // Gate modal — "Continue to disassembly?" (own root canvas)
            // =================================================================
            var gateGO = BuildContinueGateCanvas(out var gate, out var quitBtn, out var continueBtn);
            SetRef(gate, "router", router);
            SetRef(gate, "welcome", welcome);
            WireClick(quitBtn, gate, nameof(ContinueGate.Quit));
            WireClick(continueBtn, gate, nameof(ContinueGate.Continue));
            WireClick(toGate, gate, nameof(ContinueGate.Show));

            // =================================================================
            // Wiring
            // =================================================================
            SetRef(router, "dppCanva", canva.gameObject);
            SetRef(router, "modelExploration", explore.gameObject);
            SetBool(router, "zoneFollowsExploration", true);

            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "infoTab", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in scene — the passport screens are not bound to backend data.");

            var zone = FindAnyIncludingInactive<ExplodedZoneInteraction>();
            if (zone == null)
                Debug.LogWarning("[DPPUIBuilder] No ExplodedZoneInteraction found — run Phase 4 so the exploration screen has a model to show.");

            canva.gameObject.SetActive(false);
            explore.gameObject.SetActive(false);
            gateGO.SetActive(false);

            Selection.activeGameObject = canva.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 8 — DPP Canva + Model Exploration + gate built. " +
                      "Tab bar removed; the exploded zone now follows the exploration screen. Save the scene.");
        }

        // =================================================================
        // Screen header (RBv2.0): back circle + eyebrow + title + right caption.
        // Replaces MakeTabHeader — the tab pills are gone (scope decision
        // 2026-07-29). The back circle sits where Home used to, so the hand
        // already knows the spot.
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
        // Four category cards, 290 × 110 (spec 13 §3). Taller than the RBv1.0
        // 92 px card: the retired LCA card freed the bottom row, and a bigger
        // pinch target is the right thing to spend it on (00 §4 hit-area rule).
        // =================================================================
        private static void BuildDppCards(RectTransform landing, InfoTabView view, InfoTabRouter modalRouter)
        {
            MakeDppCard(landing, modalRouter, nameof(InfoTabRouter.OpenIdentity), "IdentityCard",
                24, 90, DPPSpriteFactory.IcPerson, DPPTheme.Hex("#7fd3b6"),
                "Identity &", "manufacturer", "Bosch Motorsport · MS 50.4", DPPTheme.TextSecondary);

            MakeDppCard(landing, modalRouter, nameof(InfoTabRouter.OpenMaterials), "MaterialsCard",
                326, 90, DPPSpriteFactory.IcLayers, DPPTheme.Hex("#7fd3b6"),
                "Materials &", "substances", "Al housing · Au 63 · Ag 251 · Pd 28 mg", DPPTheme.TealText);

            var hz = MakeDppCard(landing, modalRouter, nameof(InfoTabRouter.OpenHazard), "HazardCard",
                24, 212, DPPSpriteFactory.IcWarning, DPPTheme.Hex("#7fd3b6"),
                "Hazardous &", "safety", "no battery · lead-free", DPPTheme.TextSecondary);

            // Red icon variant (hidden; toggled by data — spec 02 §5.4).
            var redIcon = TLCenter("IconRed", hz.card, 28, 55, 24, 24);
            AddImage(CenterIn("Circle", redIcon, 22, 22), DPPSpriteFactory.Circle64, DPPTheme.SafetyStroke);
            AddText(Stretch("Mark", redIcon), "!", 14, Color.white, bold: true, align: TextAlignmentOptions.Center);
            redIcon.gameObject.SetActive(false);

            SetRef(view, "hazardFill", hz.fill);
            SetRef(view, "hazardStroke", hz.stroke);
            SetRef(view, "hazardIconNeutral", hz.icon.gameObject);
            SetRef(view, "hazardIconRed", redIcon.gameObject);
            SetRef(view, "hazardTitle", hz.title);
            SetRef(view, "hazardBadge", hz.subtitle);
            SetRef(view, "hazardChevron", hz.chevron);

            MakeDppCard(landing, modalRouter, nameof(InfoTabRouter.OpenCompliance), "ComplianceCard",
                326, 212, DPPSpriteFactory.IcShield, DPPTheme.Hex("#7fd3b6"),
                "Compliance &", "end-of-life", "WEEE cat. 5 · to verify", DPPTheme.TextSecondary);
        }

        private struct DppCardParts
        {
            public RectTransform card;
            public Image fill, stroke, icon, chevron;
            public TMP_Text title, subtitle;
        }

        private static DppCardParts MakeDppCard(RectTransform landing, InfoTabRouter modalRouter,
            string openMethod, string name, float x, float y,
            string iconSprite, Color iconColor,
            string titleLine1, string titleLine2, string subtitleText, Color subtitleColor)
        {
            const float W = 290f, H = 110f;
            var card = TL(name, landing, x, y, W, H);

            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            var stroke = AddImage(CenterIn("Stroke", card, W + 2, H + 2), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, W, H), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);

            var icon = AddImage(TLCenter("Icon", card, 28, 55, 24, 24), iconSprite, iconColor);

            var title = AddText(TL("Title", card, 52, 28, 220, 22), titleLine1, 16, DPPTheme.TextOnNavy, bold: true);
            AddText(TL("Title2", card, 52, 50, 220, 22), titleLine2, 16, DPPTheme.TextOnNavy, bold: true);
            var subtitle = AddText(TL("Subtitle", card, 52, 74, W - 70, 18), subtitleText, 12f, subtitleColor, bold: false);

            var chevron = AddImage(TLCenter("Chevron", card, W - 26, 55, 16, 10), DPPSpriteFactory.IcChevron, DPPTheme.TextSecondary);
            chevron.rectTransform.localEulerAngles = new Vector3(0, 0, 90); // point right (navigate)

            var btn = card.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            WireClick(btn, modalRouter, openMethod);

            var hover = card.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", card);

            return new DppCardParts { card = card, fill = fill, stroke = stroke, icon = icon, chevron = chevron, title = title, subtitle = subtitle };
        }

        // =================================================================
        // LCA as the exploration screen's MAIN PANEL (spec 14 §3).
        // Geometry carried over from the retired spec 02 §5.2 modal, rhythm
        // tightened by ~10 px to free the CTA row. Bindings are the same
        // InfoTabView fields, so PopulateLca keeps working untouched.
        // =================================================================
        private static void BuildLcaMainPanel(RectTransform page, InfoTabView view)
        {
            AddText(TL("HeadLabel", page, 24, 94, 280, 18), "Lifecycle CO2 footprint", 13, DPPTheme.TextLabel, bold: false);
            var headline = AddText(TL("HeadValue", page, 24, 112, 120, 46), "63.9", 36, DPPTheme.TextOnNavy, bold: true);
            AddText(TL("HeadUnit", page, 114, 130, 110, 22), "kg CO2e", 16, DPPTheme.TextSecondary, bold: false);
            AddText(TL("HeadCaption", page, 24, 160, 280, 16), "per unit · cradle-to-grave", 12, DPPTheme.TextTip, bold: false);
            SetRef(view, "lcaHeadlineValue", headline);

            // Recovery potential panel (right column).
            var panel = TL("RecoveryPanel", page, 330, 88, 286, 104);
            AddImage(Stretch("BG", panel), DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#0a2344"), sliced: true);
            var rTitle = AddText(TL("Title", panel, 16, 8, 262, 16),
                "Recovery potential — up to 6.6 kg CO2e", 12.5f, DPPTheme.TealText, bold: false);
            SetRef(view, "recoveryTitle", rTitle);

            float[] demoW = { 134f, 113f, 17f, 18f };
            string[] demoLabel = { "Aluminium · 3.2", "Gold · 2.7", "Palladium · 0.4", "Other metals · 0.4" };
            for (int i = 0; i < 4; i++)
            {
                var bar = TL($"Bar{i}", panel, 16, 28 + i * 17, demoW[i], 11);
                AddImage(bar, DPPSpriteFactory.RoundedR3, i == 0 ? DPPTheme.TealLight : DPPTheme.TealAccent, sliced: true);
                var lbl = AddText(TL($"BarLabel{i}", panel, 156, 26 + i * 17, 124, 15),
                    demoLabel[i], 11.5f, DPPTheme.Hex("#bbccdd"), bold: false);
                SetRef(view, $"recoveryBar{i}", bar);
                SetRef(view, $"recoveryLabel{i}", lbl);
            }

            // Stage contribution strip (full width).
            AddText(TL("StripCaption", page, 24, 204, 200, 14), "Stage contribution", 11.5f, DPPTheme.TextTip, bold: false);
            var strip = TL("StageStrip", page, 24, 218, 592, 12);
            Color[] segColors = { DPPTheme.TabActiveStroke, DPPTheme.TealAccent, DPPTheme.TealLight, DPPTheme.TabInactiveFill };
            float[] segW = { 83f, 8f, 2f, 499f };
            float segX = 0f;
            for (int i = 0; i < 4; i++)
            {
                var seg = TL($"Seg{i}", strip, segX, 0, segW[i], 12);
                AddImage(seg, null, segColors[i]);
                SetRef(view, $"stageSeg{i}", seg);
                segX += segW[i];
            }

            // 2×2 stage grid.
            string[] gridLabels = { "S1 Raw materials", "S2 Manufacturing", "S3 Distribution", "S4 Use phase*" };
            string[] gridValues = { "8.9", "0.9", "0.1", "54.0" };
            for (int i = 0; i < 4; i++)
            {
                float gx = (i % 2 == 0) ? 24f : 346f;
                float gy = (i < 2) ? 242f : 266f;
                AddImage(TL($"Swatch{i}", page, gx, gy + 4, 8, 8), null, segColors[i]);
                var lbl = AddText(TL($"StageLabel{i}", page, gx + 14, gy, 170, 16), gridLabels[i], 12.5f, DPPTheme.TextLabel, bold: false);
                var val = AddText(TL($"StageValue{i}", page, gx + 14, gy, (i % 2 == 0) ? 252f : 256f, 16),
                    gridValues[i], 13, DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
                SetRef(view, $"stageLabel{i}", lbl);
                SetRef(view, $"stageValue{i}", val);
            }

            // Method + footnote.
            AddImage(TL("MethodDivider", page, 24, 298, 592, 1), null, DPPTheme.RowStroke);
            AddText(TL("MethodLabel", page, 24, 312, 100, 16), "Method", 12.5f, DPPTheme.TextLabel, bold: false);
            var method = AddText(TL("MethodValue", page, 24, 312, 592, 16),
                "ISO 14040 · GWP100 (AR6) · estimated BOM", 12.5f, DPPTheme.TextOnNavy, bold: false,
                align: TextAlignmentOptions.MidlineRight);
            SetRef(view, "methodValue", method);
            AddText(TL("Footnote", page, 24, 330, 540, 14),
                "*modelled use profile · recovery net of process emissions", 11.5f, DPPTheme.TextTip, bold: false);
        }

        // =================================================================
        // Gate modal — own root canvas, 440 × 210 (spec 14 §4)
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

            // "Quit", not "Back": this edge leaves the product session entirely
            // (Thiago, 2026-07-29 — the one deliberate break in the one-step-back
            // hierarchy, kept from the Miro diagram but relabelled).
            quitBtn = BuildPillButton(card, "QuitButton", cx: 119, cy: 156, w: 190, h: 52,
                label: "Quit", labelSize: 15, primary: false, chevron: false);
            continueBtn = BuildPillButton(card, "ContinueButton", cx: 329, cy: 156, w: 190, h: 52,
                label: "Continue", labelSize: 15, primary: true, chevron: true);

            BuildGrabberBar(rt);
            var handle = go.GetComponentInChildren<PanelGrabHandle>(true);
            if (handle != null) SetBool(handle, "recenterOnStart", false); // ContinueGate.Show does the placing

            Undo.RegisterCreatedObjectUndo(go, "Build Continue Gate");
            return go;
        }

        // =================================================================
        // Wide primary CTA (teal pill + chevron) anchored top-left in spec coords.
        // =================================================================
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

            // Chevron "›" from two capsule bars — the SF Pro SDF atlas has no glyph
            // (00 §3). anchoredPosition.y is UP, so the UPPER bar tilts -45.
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

        // =================================================================
        // Small helpers
        // =================================================================

        /// <summary>FindFirstObjectByType misses inactive objects — Welcome and the
        /// zone are both inactive most of the time, so always include them.</summary>
        private static T FindAnyIncludingInactive<T>() where T : Component
            => Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);

        private static void DestroyChild(RectTransform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null) Undo.DestroyObjectImmediate(child.gameObject);
        }
    }
}
