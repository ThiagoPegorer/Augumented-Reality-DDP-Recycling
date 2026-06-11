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
    /// Phase 5 builder — Screen 09: Completion summary (spec 09 v2, approved
    /// 2026-06-11). Single 640×430 screen on the DPPPanelCanvas: done header,
    /// time + steps stat cards, 2×2 recovered grid, Done + Send report buttons.
    /// Demo values baked = vcu_001; CompletionSummaryView binds data + session.
    /// Safe to re-run (rebuilds the CompletionSummary object).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("DPP/Build Phase 5 — Completion Summary", false, 5)]
        public static void BuildPhase5()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run Phase 1 first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();

            var old = canvasRT.Find("CompletionSummary");
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

            var screen = Stretch("CompletionSummary", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build DPP Completion Summary");
            var view = screen.gameObject.AddComponent<CompletionSummaryView>();

            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---- Done header ----
            var done = TLCenter("DoneIcon", screen, 40, 48, 40, 40);
            AddImage(CenterIn("Ring", done, 44, 44), DPPSpriteFactory.Circle64, DPPTheme.TealAccent);
            AddImage(CenterIn("Fill", done, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.Hex("#10241e"));
            AddImage(CenterIn("Check", done, 24, 24), DPPSpriteFactory.IcCheck, DPPTheme.TealLight);

            var eyebrow = AddText(TL("Eyebrow", screen, 72, 30, 400, 18), "MS 50.4 · VCU-2026-001", 13, DPPTheme.TealMuted, bold: false);
            AddText(TL("Title", screen, 72, 48, 480, 28), "Nice work — unit fully dismantled", 22, DPPTheme.TextOnNavy, bold: true);

            // ---- Stat cards ----
            // Time (teal accent)
            var timeCard = TL("TimeCard", screen, 20, 88, 290, 80);
            AddImage(CenterIn("Stroke", timeCard, 292, 82), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true);
            AddImage(CenterIn("Fill", timeCard, 290, 80), DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#0e2335"), sliced: true);
            AddImage(TLCenter("Icon", timeCard, 46, 40, 26, 26), DPPSpriteFactory.IcClock, DPPTheme.TealLight);
            AddText(TL("Label", timeCard, 72, 16, 210, 16), "Total time · start → finish", 12, DPPTheme.TextLabel, bold: false);
            var timeValue = AddText(TL("Value", timeCard, 72, 36, 210, 32), "— min — s", 26, DPPTheme.TextOnNavy, bold: true);

            // Steps
            var stepsCard = TL("StepsCard", screen, 326, 88, 290, 80);
            AddImage(CenterIn("Stroke", stepsCard, 292, 82), DPPSpriteFactory.RoundedR13, DPPTheme.RowStroke, sliced: true);
            AddImage(CenterIn("Fill", stepsCard, 290, 80), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);
            AddText(TL("Label", stepsCard, 26, 16, 200, 16), "Steps completed", 12, DPPTheme.TextLabel, bold: false);
            var stepsValue = AddText(TL("Value", stepsCard, 26, 36, 140, 32), "5 / 5", 26, DPPTheme.TextOnNavy, bold: true);
            for (int i = 0; i < 5; i++)
                AddImage(TL($"Seg{i}", stepsCard, 204 + i * 14, 32, 10, 16), DPPSpriteFactory.RoundedR3, DPPTheme.TealLight, sliced: true);

            // ---- Recovered grid ----
            AddText(TL("RecoveredLabel", screen, 20, 188, 200, 16), "RECOVERED", 13, DPPTheme.TextCaption, bold: false);

            MakeRecoveryCard(screen, "GoldPinsCard", 20, 208, gold: true, impact: false,
                DPPSpriteFactory.IcStar, "Gold — 198 connector pins", "3 connectors + USB separated", out _, out _);
            MakeRecoveryCard(screen, "SiliconCard", 326, 208, gold: true, impact: false,
                DPPSpriteFactory.IcStar, "Processors & memory ICs", "high-value silicon, not shredded", out _, out _);
            MakeRecoveryCard(screen, "AluminiumCard", 20, 268, gold: false, impact: false,
                DPPSpriteFactory.IcLayers, "Aluminium housing · 363 g", "sorted to metal fraction", out TMP_Text aluTitle, out _);
            MakeRecoveryCard(screen, "Co2Card", 326, 268, gold: false, impact: true,
                DPPSpriteFactory.IcLeaf, "CO2 avoided · up to 6.6 kg", "vs no recycling · net of process", out TMP_Text co2Title, out _);

            // ---- Action row: confirmation message (left, hidden) + single action button (right) ----
            // No separate Done button (spec 09 v2.1): the action button is
            // "Send recovery report" and becomes "Done" after a successful send.
            var sentMessage = AddText(TL("SentMessage", screen, 20, 360, 290, 50),
                "Report was successfully sent", 13, DPPTheme.TealText, bold: false,
                align: TextAlignmentOptions.MidlineRight);
            sentMessage.gameObject.SetActive(false);

            var sendRT = TL("ActionButton", screen, 326, 360, 290, 50);
            var sendOutline = AddImage(CenterIn("HoverOutline", sendRT, 298, 58), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            sendOutline.gameObject.SetActive(false);
            var sendFill = AddImage(CenterIn("Fill", sendRT, 290, 50), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true, raycast: true);
            var sendLabel = AddText(Stretch("Label", sendRT), "Send recovery report", 15.5f, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            var chevron = NewRT("Chevron", sendRT);
            chevron.anchorMin = chevron.anchorMax = new Vector2(1f, 0.5f);
            chevron.pivot = new Vector2(0.5f, 0.5f);
            chevron.anchoredPosition = new Vector2(-26, 0);
            chevron.sizeDelta = new Vector2(16, 16);
            var chevTop = TLCenter("Top", chevron, 8, 4, 11, 2.4f);
            chevTop.localRotation = Quaternion.Euler(0, 0, -45);
            AddImage(chevTop, DPPSpriteFactory.Grip, Color.white);
            var chevBot = TLCenter("Bottom", chevron, 8, 11, 11, 2.4f);
            chevBot.localRotation = Quaternion.Euler(0, 0, 45);
            AddImage(chevBot, DPPSpriteFactory.Grip, Color.white);
            var sendBtn = sendRT.gameObject.AddComponent<Button>();
            sendBtn.transition = Selectable.Transition.None;
            sendBtn.targetGraphic = sendFill;
            WireClick(sendBtn, view, nameof(CompletionSummaryView.OnActionButton));
            var sendHover = sendRT.gameObject.AddComponent<HoverHighlight>();
            SetRef(sendHover, "highlightOutline", sendOutline.gameObject);

            // ---- Wiring ----
            var manager = Object.FindFirstObjectByType<DPPManager>();
            var client = Object.FindFirstObjectByType<DPPClient>();
            SetRef(view, "client", client);
            SetRef(view, "router", router);
            SetRef(view, "eyebrowText", eyebrow);
            SetRef(view, "timeValue", timeValue);
            SetRef(view, "stepsValue", stepsValue);
            SetRef(view, "aluminiumTitle", aluTitle);
            SetRef(view, "co2Title", co2Title);
            SetRef(view, "actionLabel", sendLabel);
            SetRef(view, "actionButton", sendBtn);
            SetRef(view, "actionChevron", chevron.gameObject);
            SetRef(view, "sentMessage", sentMessage);

            if (router != null) SetRef(router, "completionSummary", screen.gameObject);
            if (manager != null) SetRef(manager, "completionSummary", view);

            // StepFlowController hand-off (summary ref) — wire if Phase 4 is built.
            var controller = canvasRT.GetComponentInChildren<StepFlowController>(true);
            if (controller != null) SetRef(controller, "summary", view);
            else Debug.LogWarning("[DPPUIBuilder] StepFlowController not found — run Phase 4, then re-run Phase 5 to wire the hand-off.");

            screen.gameObject.SetActive(false);

            Selection.activeGameObject = screen.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 5 — Completion Summary built. Save the scene.");
        }

        private static void MakeRecoveryCard(RectTransform screen, string name, float x, float y,
            bool gold, bool impact, string iconSprite, string demoTitle, string demoSubtitle,
            out TMP_Text title, out TMP_Text subtitle)
        {
            var card = TL(name, screen, x, y, 290, 52);

            Color fill   = gold ? DPPTheme.Hex("#241c0e") : impact ? DPPTheme.Hex("#10241e") : DPPTheme.RowFill;
            Color stroke = gold ? DPPTheme.GoldPartStroke : impact ? DPPTheme.TealAccent : DPPTheme.RowStroke;
            Color titleC = impact ? DPPTheme.TealText : DPPTheme.TextOnNavy;
            Color subC   = gold ? DPPTheme.Hex("#e6c489") : impact ? DPPTheme.TealMuted : DPPTheme.TextSecondary;
            Color iconC  = gold ? DPPTheme.Hex("#f0c879") : DPPTheme.TealLight;

            AddImage(CenterIn("Stroke", card, 292, 54), DPPSpriteFactory.RoundedR13, stroke, sliced: true);
            AddImage(CenterIn("Fill", card, 290, 52), DPPSpriteFactory.RoundedR13, fill, sliced: true);
            AddImage(TLCenter("Icon", card, 24, 26, 18, 18), iconSprite, iconC);

            title = AddText(TL("Title", card, 42, 6, 240, 20), demoTitle, 13.5f, titleC, bold: true);
            subtitle = AddText(TL("Subtitle", card, 42, 28, 240, 16), demoSubtitle, 11.5f, subC, bold: false);
        }
    }
}
