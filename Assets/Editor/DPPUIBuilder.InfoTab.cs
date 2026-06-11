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
    /// Phase 2 builder — Screen 02: Information tab (spec 02 **v3**, approved
    /// 2026-06-10): category card grid + full-page modal details. No scrolling.
    ///
    /// Structure built under DPPPanelCanvas (run Phase 1 first):
    ///   InformationTab
    ///   ├─ PanelBG
    ///   ├─ Landing      — home + tab pills + 5 category cards
    ///   ├─ IdentityModal / MaterialsModal / HazardModal / ComplianceModal / LcaModal
    ///   └─ (InfoTabRouter toggles Landing ⇄ one modal; back arrow top-left)
    ///
    /// Visual defaults match vcu_001; InfoTabView.Populate overwrites from the
    /// backend at runtime. Safe to re-run (rebuilds the InformationTab object).
    /// Panel-local coordinates: spec SVG x/y minus 20.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const float ModalContentX = 24f;   // content left edge
        private const float ModalContentW = 592f;  // content width (24 → 616)
        private const float ModalFieldStartY = 116f;
        private const float ModalFieldPitch = 34f;

        [MenuItem("DPP/Build Phase 2 — Information Tab", false, 2)]
        public static void BuildPhase2()
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

            var old = canvasRT.Find("InformationTab");
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

            var screen = Stretch("InformationTab", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build DPP Information Tab");
            var view = screen.gameObject.AddComponent<InfoTabView>();
            var modalRouter = screen.gameObject.AddComponent<InfoTabRouter>();

            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---- Landing: header + card grid ----
            var landing = Stretch("Landing", screen);
            MakeTabHeader(landing, router, disassemblyActive: false);
            BuildCategoryCards(landing, view, modalRouter);

            // ---- Modal pages ----
            var identity = BuildFieldModal(screen, modalRouter, "IdentityModal", "Identity & manufacturer",
                DPPSpriteFactory.IcPerson, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("Manufacturer", "Bosch Motorsport", "manufacturerValue"),
                    ("Model", "Vehicle Control Unit MS 50.4", "modelValue"),
                    ("Type number", "F02U.V02.965-02", "typeNumberValue"),
                    ("Production", "2026-03 · DE", "productionValue"),
                    ("Specifications", "166 x 121 x 41 mm · 660 g · IP67", "specsValue"),
                    ("Service life (design)", "15 y · 225,000 km", "serviceLifeValue"),
                }, view);

            var materials = BuildFieldModal(screen, modalRouter, "MaterialsModal", "Materials & substances",
                DPPSpriteFactory.IcLayers, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("Housing", "Die-cast aluminium (AlSi) · 363 g", "housingValue"),
                    ("Connectors", "Brass (Cu-Zn) + Au/Ni plating · 58 g", "connectorsValue"),
                    ("PCB assembly", "FR-4 · Cu · ≈185 g", "pcbValue"),
                    ("Active components", "Silicon · 20 g", "activesValue"),
                    ("Precious metals", "Au 63 · Ag 251 · Pd 28 mg", "preciousValue"),
                    ("Recycled content", "—", "recycledValue"),
                }, view, tealValueField: "preciousValue");

            var hazard = BuildFieldModal(screen, modalRouter, "HazardModal", "Hazardous & safety",
                DPPSpriteFactory.IcWarning, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("Contains battery", "No", "hazardBatteryValue"),
                    ("Hazardous substances", "None documented", "hazardSubstancesValue"),
                    ("Solder", "Lead-free SnAgCu (SAC305)", "hazardSolderValue"),
                    ("WEEE treatment", "Selective treatment recommended", "hazardTreatmentValue"),
                }, view);

            var compliance = BuildFieldModal(screen, modalRouter, "ComplianceModal", "Compliance & end-of-life",
                DPPSpriteFactory.IcShield, DPPTheme.Hex("#7fd3b6"), new[]
                {
                    ("CE marking", "—", "ceValue"),
                    ("RoHS", "—", "rohsValue"),
                    ("REACH", "—", "reachValue"),
                    ("WEEE category", "Cat. 5 (small equipment) - to verify", "weeeValue"),
                    ("Recycling route", "WEEE - selective treatment recommended", "routeValue"),
                }, view);

            var lca = BuildLcaModal(screen, modalRouter, view);

            // ---- Wiring ----
            SetRef(modalRouter, "landing", landing.gameObject);
            SetRef(modalRouter, "identityModal", identity.gameObject);
            SetRef(modalRouter, "materialsModal", materials.gameObject);
            SetRef(modalRouter, "hazardModal", hazard.gameObject);
            SetRef(modalRouter, "complianceModal", compliance.gameObject);
            SetRef(modalRouter, "lcaModal", lca.gameObject);

            if (router != null) SetRef(router, "informationTab", screen.gameObject);
            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "infoTab", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in scene — Information tab not bound to backend data.");

            screen.gameObject.SetActive(false); // router shows MainPage by default

            Selection.activeGameObject = screen.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 2 — Information Tab (v3 grid + modals) built. Save the scene.");
        }

        // =================================================================
        // Shared tab header: home + tab pills + separator (02 v3 §3.1 / 03 v2 §3).
        // Used by the Information tab (disassemblyActive=false) and the
        // Disassembly intro (disassemblyActive=true).
        // =================================================================
        internal static void MakeTabHeader(RectTransform parent, ScreenRouter router, bool disassemblyActive)
        {
            var home = TLCenter("HomeButton", parent, 42, 44, 40, 40);
            var homeOutline = AddImage(CenterIn("HoverOutline", home, 50, 50), DPPSpriteFactory.Circle64, Color.white);
            homeOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", home, 43, 43), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var homeFill = AddImage(CenterIn("Fill", home, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", home, 22, 22), DPPSpriteFactory.IcHouse, Color.white);

            var homeBtn = home.gameObject.AddComponent<Button>();
            homeBtn.transition = Selectable.Transition.None;
            homeBtn.targetGraphic = homeFill;
            if (router != null) WireClick(homeBtn, router, nameof(ScreenRouter.ShowMainPage));
            var homeHover = home.gameObject.AddComponent<HoverHighlight>();
            SetRef(homeHover, "highlightOutline", homeOutline.gameObject);

            BuildTabPill(parent, router, "TabInformations", 130, active: !disassemblyActive, label: "Informations",
                method: nameof(ScreenRouter.ShowInformations));
            BuildTabPill(parent, router, "TabDisassembly", 322, active: disassemblyActive, label: "Disassembly",
                method: nameof(ScreenRouter.ShowDisassembly));

            AddImage(TL("Separator", parent, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));
        }

        private static void BuildTabPill(RectTransform parent, ScreenRouter router,
            string name, float x, bool active, string label, string method)
        {
            var tab = TL(name, parent, x, 26, 180, 38);

            var outline = AddImage(CenterIn("HoverOutline", tab, 188, 46), DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            if (active)
                AddImage(CenterIn("Stroke", tab, 183, 41), DPPSpriteFactory.RoundedR20, DPPTheme.TabActiveStroke, sliced: true);

            var fill = AddImage(CenterIn("Fill", tab, 180, 38), DPPSpriteFactory.RoundedR20,
                active ? DPPTheme.TabActiveFill : DPPTheme.TabInactiveFill, sliced: true, raycast: true);

            AddText(Stretch("Label", tab), label, 15,
                active ? DPPTheme.TextOnNavy : DPPTheme.TabInactiveText,
                bold: active, align: TextAlignmentOptions.Center);

            var btn = tab.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            if (router != null) WireClick(btn, router, method);

            var hover = tab.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
        }

        // =================================================================
        // Category cards (spec 02 v3 §4)
        // =================================================================
        private static void BuildCategoryCards(RectTransform landing, InfoTabView view, InfoTabRouter modalRouter)
        {
            // Identity (two-line title)
            MakeCategoryCard(landing, modalRouter, nameof(InfoTabRouter.OpenIdentity), "IdentityCard",
                24, 86, 290, DPPSpriteFactory.IcPerson, DPPTheme.Hex("#7fd3b6"),
                "Identity &", "manufacturer", null, null, accent: false);

            // Materials (two-line title)
            MakeCategoryCard(landing, modalRouter, nameof(InfoTabRouter.OpenMaterials), "MaterialsCard",
                326, 86, 290, DPPSpriteFactory.IcLayers, DPPTheme.Hex("#7fd3b6"),
                "Materials &", "substances", null, null, accent: false);

            // Hazardous (single title + badge; conditional styling refs → view)
            var hz = MakeCategoryCard(landing, modalRouter, nameof(InfoTabRouter.OpenHazard), "HazardCard",
                24, 190, 290, DPPSpriteFactory.IcWarning, DPPTheme.Hex("#7fd3b6"),
                "Hazardous & safety", null, "no battery · lead-free", DPPTheme.TextSecondary, accent: false);

            // Red icon variant (hidden; toggled by data).
            var redIcon = TLCenter("IconRed", hz.card, 28, 46, 24, 24);
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

            // Compliance (two-line title)
            MakeCategoryCard(landing, modalRouter, nameof(InfoTabRouter.OpenCompliance), "ComplianceCard",
                326, 190, 290, DPPSpriteFactory.IcShield, DPPTheme.Hex("#7fd3b6"),
                "Compliance &", "end-of-life", null, null, accent: false);

            // LCA (full width, teal accent, live summary subtitle)
            var lcaCard = MakeCategoryCard(landing, modalRouter, nameof(InfoTabRouter.OpenLca), "LcaCard",
                24, 294, 592, DPPSpriteFactory.IcLeaf, DPPTheme.TealLight,
                "Life cycle analysis", null, "63.9 kg CO2e lifecycle · up to 6.6 recoverable",
                DPPTheme.TealText, accent: true);
            SetRef(view, "lcaCardSubtitle", lcaCard.subtitle);
        }

        private struct CardParts
        {
            public RectTransform card;
            public Image fill, stroke, icon, chevron;
            public TMP_Text title, subtitle;
        }

        private static CardParts MakeCategoryCard(RectTransform landing, InfoTabRouter modalRouter,
            string openMethod, string name, float x, float y, float w,
            string iconSprite, Color iconColor,
            string titleLine1, string titleLine2, string subtitleText, Color? subtitleColor, bool accent)
        {
            const float H = 92f;
            var card = TL(name, landing, x, y, w, H);

            var outline = AddImage(CenterIn("HoverOutline", card, w + 12, H + 12), DPPSpriteFactory.RoundedR20, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            var stroke = AddImage(CenterIn("Stroke", card, w + 2, H + 2), DPPSpriteFactory.RoundedR13,
                accent ? DPPTheme.TealAccent : DPPTheme.RowStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", card, w, H), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);

            var icon = AddImage(TLCenter("Icon", card, 28, 46, 24, 24), iconSprite, iconColor);

            TMP_Text title;
            if (titleLine2 != null)
            {
                title = AddText(TL("Title", card, 52, 22, 220, 22), titleLine1, 16, DPPTheme.TextOnNavy, bold: true);
                AddText(TL("Title2", card, 52, 44, 220, 22), titleLine2, 16, DPPTheme.TextOnNavy, bold: true);
            }
            else if (subtitleText != null)
            {
                title = AddText(TL("Title", card, 52, 24, 420, 22), titleLine1, 16, DPPTheme.TextOnNavy, bold: true);
            }
            else
            {
                title = AddText(TL("Title", card, 52, 34, 420, 22), titleLine1, 16, DPPTheme.TextOnNavy, bold: true);
            }

            TMP_Text subtitle = null;
            if (subtitleText != null)
                subtitle = AddText(TL("Subtitle", card, 52, 50, w - 90, 18), subtitleText, 12.5f,
                    subtitleColor ?? DPPTheme.TextLabel, bold: false);

            var chevron = AddImage(TLCenter("Chevron", card, w - 26, 46, 16, 10), DPPSpriteFactory.IcChevron, DPPTheme.TextSecondary);
            chevron.rectTransform.localEulerAngles = new Vector3(0, 0, 90); // point right (navigate)

            var btn = card.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            WireClick(btn, modalRouter, openMethod);

            var hover = card.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", card);

            return new CardParts { card = card, fill = fill, stroke = stroke, icon = icon, chevron = chevron, title = title, subtitle = subtitle };
        }

        // =================================================================
        // Modal chrome (spec 02 v3 §5): back arrow + breadcrumb + title
        // =================================================================
        private static RectTransform MakeModalPage(RectTransform screen, InfoTabRouter modalRouter,
            string name, string title, string iconSprite, Color iconColor)
        {
            var page = Stretch(name, screen);

            var back = TLCenter("BackButton", page, 42, 44, 40, 40);
            var backOutline = AddImage(CenterIn("HoverOutline", back, 50, 50), DPPSpriteFactory.Circle64, Color.white);
            backOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", back, 43, 43), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var backFill = AddImage(CenterIn("Fill", back, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", back, 22, 22), DPPSpriteFactory.IcBack, Color.white);

            var backBtn = back.gameObject.AddComponent<Button>();
            backBtn.transition = Selectable.Transition.None;
            backBtn.targetGraphic = backFill;
            WireClick(backBtn, modalRouter, nameof(InfoTabRouter.Back));
            var backHover = back.gameObject.AddComponent<HoverHighlight>();
            SetRef(backHover, "highlightOutline", backOutline.gameObject);

            // No breadcrumb (removed after Editor test 2026-06-10) — title centers on the back button.
            AddImage(TLCenter("TitleIcon", page, 88, 44, 20, 20), iconSprite, iconColor);
            AddText(TL("Title", page, 102, 31, 440, 26), title, 19, DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Separator", page, 24, 76, 592, 1), null, DPPTheme.Hex("#1a335f"));

            page.gameObject.SetActive(false);
            return page;
        }

        /// <summary>Modal with simple label/value field rows. Wires each row's value into the view by field name.</summary>
        private static RectTransform BuildFieldModal(RectTransform screen, InfoTabRouter modalRouter,
            string name, string title, string iconSprite, Color iconColor,
            (string label, string demo, string viewField)[] rows, InfoTabView view,
            string tealValueField = null)
        {
            var page = MakeModalPage(screen, modalRouter, name, title, iconSprite, iconColor);

            for (int i = 0; i < rows.Length; i++)
            {
                float y = ModalFieldStartY + i * ModalFieldPitch;
                AddText(TL($"Label{i}", page, ModalContentX, y, 300, 20), rows[i].label, 13, DPPTheme.TextLabel, bold: false);
                var value = AddText(TL($"Value{i}", page, ModalContentX, y - 1, ModalContentW, 22), rows[i].demo, 13.5f,
                    rows[i].viewField == tealValueField ? DPPTheme.TealText : DPPTheme.TextOnNavy,
                    bold: false, align: TextAlignmentOptions.MidlineRight);
                SetRef(view, rows[i].viewField, value);
            }
            return page;
        }

        // =================================================================
        // LCA modal — custom full-page layout (spec 02 v3 §5.2)
        // =================================================================
        private static RectTransform BuildLcaModal(RectTransform screen, InfoTabRouter modalRouter, InfoTabView view)
        {
            var page = MakeModalPage(screen, modalRouter, "LcaModal", "Life cycle analysis",
                DPPSpriteFactory.IcLeaf, DPPTheme.TealLight);

            // Headline (left column) — svg coords minus 20.
            AddText(TL("HeadLabel", page, 24, 104, 280, 18), "Lifecycle CO2 footprint", 13, DPPTheme.TextLabel, bold: false);
            var headline = AddText(TL("HeadValue", page, 24, 122, 120, 46), "63.9", 36, DPPTheme.TextOnNavy, bold: true);
            AddText(TL("HeadUnit", page, 114, 140, 110, 22), "kg CO2e", 16, DPPTheme.TextSecondary, bold: false);
            AddText(TL("HeadCaption", page, 24, 170, 280, 16), "per unit · cradle-to-grave", 12, DPPTheme.TextTip, bold: false);
            SetRef(view, "lcaHeadlineValue", headline);

            // Recovery potential panel (right column).
            var panel = TL("RecoveryPanel", page, 330, 92, 286, 110);
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
            AddText(TL("StripCaption", page, 24, 212, 200, 14), "Stage contribution", 11.5f, DPPTheme.TextTip, bold: false);
            var strip = TL("StageStrip", page, 24, 226, 592, 12);
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
                float gy = (i < 2) ? 250f : 276f;
                AddImage(TL($"Swatch{i}", page, gx, gy + 4, 8, 8), null, segColors[i]);
                var lbl = AddText(TL($"StageLabel{i}", page, gx + 14, gy, 170, 16), gridLabels[i], 12.5f, DPPTheme.TextLabel, bold: false);
                var val = AddText(TL($"StageValue{i}", page, gx + 14, gy, (i % 2 == 0) ? 252f : 256f, 16),
                    gridValues[i], 13, DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
                SetRef(view, $"stageLabel{i}", lbl);
                SetRef(view, $"stageValue{i}", val);
            }

            // Method + footnote.
            AddImage(TL("MethodDivider", page, 24, 314, 592, 1), null, DPPTheme.RowStroke);
            AddText(TL("MethodLabel", page, 24, 330, 100, 16), "Method", 12.5f, DPPTheme.TextLabel, bold: false);
            var method = AddText(TL("MethodValue", page, 24, 330, 592, 16),
                "ISO 14040 · GWP100 (AR6) · estimated BOM", 12.5f, DPPTheme.TextOnNavy, bold: false,
                align: TextAlignmentOptions.MidlineRight);
            SetRef(view, "methodValue", method);
            AddText(TL("Footnote", page, 24, 352, 540, 14),
                "*modelled use profile · recovery net of process emissions", 11.5f, DPPTheme.TextTip, bold: false);

            return page;
        }
    }
}
