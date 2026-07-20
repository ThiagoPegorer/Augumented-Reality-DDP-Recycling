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
    /// Phase 5 builder — Screen 09: Completion summary (spec 09 v3, 2026-07-14).
    ///
    /// v3 (approved mock 09_summary_v3_2.svg): eyebrow (model · serial) and ALL
    /// non-button boxes removed. Layout: check + title, one big total-time
    /// value (no label), divider, TIME/RECOVERED column headers, five per-step
    /// rows (title + materials-with-grams line + time split + step mass),
    /// assumed-splits footnote, and the v2.1 single-action Send→Done button.
    /// Demo values baked = vcu_001; CompletionSummaryView binds data + session.
    /// Safe to re-run (rebuilds the CompletionSummary object).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // Demo rows baked at build time (vcu_001); Populate/SetSession overwrite.
        private static readonly (string title, string materials, string mass)[] DemoSteps =
        {
            ("1 · Open the housing",       "steel bolts 15 g · brass inserts 4.8 g",                    "20 g"),
            ("2 · Remove the connectors",  "brass (Cu-Zn) 46 g · polymer insert 11 g · gold plating 0.8 g", "58 g"),
            ("3 · Lift out the main PCB",  "FR-4 board 91 g · copper 41 g · passives 30 g · other 23 g", "185 g"),
            ("4 · Recover the silicon",    "epoxy package 10 g · Cu leadframe 5.8 g · silicon die 4.0 g", "20 g"),
            ("5 · Sort the housing",       "aluminium shells 363 g · labels & adhesive 15 g",            "378 g"),
        };

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

            // ---- Header: check + title (no eyebrow since v3) ----
            var done = TLCenter("DoneIcon", screen, 40, 48, 40, 40);
            AddImage(CenterIn("Ring", done, 44, 44), DPPSpriteFactory.Circle64, DPPTheme.TealAccent);
            AddImage(CenterIn("Fill", done, 40, 40), DPPSpriteFactory.Circle64, DPPTheme.Hex("#10241e"));
            AddImage(CenterIn("Check", done, 24, 24), DPPSpriteFactory.IcCheck, DPPTheme.TealLight);
            AddText(TL("Title", screen, 72, 36, 480, 28), "Nice work — unit fully dismantled", 22, DPPTheme.TextOnNavy, bold: true);

            // ---- Big total time (value only, no label since v3) ----
            var timeValue = AddText(TL("TimeValue", screen, 20, 84, 320, 38), "— min — s", 27, DPPTheme.TextOnNavy, bold: true);

            AddImage(TL("Divider", screen, 20, 132, 600, 1.5f), DPPSpriteFactory.Grip, DPPTheme.Hex("#1a335f"), sliced: false);

            // ---- Column headers ----
            AddText(TL("ColTime", screen, 384, 144, 100, 14), "TIME", 10.5f, DPPTheme.TextCaption,
                bold: false, align: TextAlignmentOptions.MidlineRight);
            AddText(TL("ColRecovered", screen, 496, 144, 120, 14), "RECOVERED", 10.5f, DPPTheme.TextCaption,
                bold: false, align: TextAlignmentOptions.MidlineRight);

            // ---- Step rows ----
            int n = DemoSteps.Length;
            var titles = new TMP_Text[n];
            var materials = new TMP_Text[n];
            var times = new TMP_Text[n];
            var masses = new TMP_Text[n];
            var tags = new GameObject[n];

            for (int i = 0; i < n; i++)
            {
                float y = 166 + i * 38;
                titles[i] = AddText(TL($"Step{i + 1}Title", screen, 20, y, 340, 18),
                    DemoSteps[i].title, 13.5f, DPPTheme.TextOnNavy, bold: true);
                materials[i] = AddText(TL($"Step{i + 1}Materials", screen, 20, y + 16, 460, 15),
                    DemoSteps[i].materials, 11, DPPTheme.TextCaption, bold: false);

                var tag = AddText(TL($"Step{i + 1}Longest", screen, 326, y + 2, 96, 14),
                    "longest", 10.5f, DPPTheme.Hex("#f0c879"), bold: false, align: TextAlignmentOptions.MidlineRight);
                tag.gameObject.SetActive(false);
                tags[i] = tag.gameObject;

                times[i] = AddText(TL($"Step{i + 1}Time", screen, 384, y, 100, 18),
                    "—", 13.5f, DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
                masses[i] = AddText(TL($"Step{i + 1}Mass", screen, 496, y, 120, 18),
                    DemoSteps[i].mass, 13.5f, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.MidlineRight);
            }

            // ---- Assumed-splits footnote ----
            var footnote = AddText(TL("Footnote", screen, 20, 358, 400, 14),
                "material splits: assumed · to be validated in openLCA", 10, DPPTheme.TextTip, bold: false);
            footnote.fontStyle = FontStyles.Italic;

            // ---- Action row (v2.1 single-button flow, unchanged) ----
            var sentMessage = AddText(TL("SentMessage", screen, 20, 372, 290, 44),
                "Dismantling report sent", 13, DPPTheme.TealText, bold: false,
                align: TextAlignmentOptions.MidlineRight);
            sentMessage.gameObject.SetActive(false);

            var sendRT = TL("ActionButton", screen, 326, 372, 290, 44);
            var sendOutline = AddImage(CenterIn("HoverOutline", sendRT, 298, 52), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            sendOutline.gameObject.SetActive(false);
            var sendFill = AddImage(CenterIn("Fill", sendRT, 290, 44), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true, raycast: true);
            var sendLabel = AddText(Stretch("Label", sendRT), "Send dismantling report", 15f, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
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
            SetRef(view, "timeValue", timeValue);
            SetRefArray(view, "stepTitles", titles);
            SetRefArray(view, "stepMaterials", materials);
            SetRefArray(view, "stepTimes", times);
            SetRefArray(view, "stepMasses", masses);
            SetRefArray(view, "longestTags", tags);
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
            Debug.Log("[DPPUIBuilder] Phase 5 — Completion Summary v3 built. Save the scene.");
        }
    }
}
