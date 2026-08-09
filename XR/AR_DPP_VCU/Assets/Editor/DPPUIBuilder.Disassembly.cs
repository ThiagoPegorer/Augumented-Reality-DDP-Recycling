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
    /// RBv2_1_1/14 — THE GUIDED DISASSEMBLY MODE (spec 10, mock
    /// `drafts/10_v1_disassembly_mode.svg` approved 2026-08-09).
    ///
    /// Builds INTO the existing rig, never beside it:
    ///   · `DisassemblyRail` — a second rail content group under RailCanvas:
    ///     eyebrow + 7 entries (Intro · steps 1–5 · Summary) at 184 × 44 / 52
    ///     pitch, exactly the band the 4 passport tabs + CTA occupy.
    ///   · `DisIntroPage` / `DisStepPage` / `DisSummaryPage` under DataCanvas —
    ///     briefing, the per-step task page (rebound per step at runtime), and
    ///     the per-step summary table (a second CompletionSummaryView instance,
    ///     all table/report logic reused).
    ///   · `DisassemblyModeController` on the rig, wired to everything.
    ///
    /// EVERY pressable surface goes through the elevation kit (00 §4.1):
    /// AddShadow → stroke/fill → AddGloss, capsules via PsSmallPill. The task
    /// rows are BOXED AND PRESSABLE (chrome = touchable): the whole row toggles
    /// its task; the status circle inside is a pure binary light, deliberately
    /// named "CircleFill" so the row's HoverHighlight can never repaint it.
    ///
    /// RUN ORDER: after /10 (the rig is this phase's canvas) — the routine chain
    /// grows to 09 → 10 → 11 → 12 → 13 → 14 → Tools/Verify → SAVE. Safe to
    /// re-run: destroys and rebuilds only its own three pages + rail group.
    /// The RB2_0 flat screens (menu 05/06/07) are untouched — rollback path.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const float DisMargin = 24f, DisW = 372f;   // 420-wide data canvas column

        private static readonly string[] DisEntryLabels =
            { "Intro — briefing", "Open housing", "Connectors", "Main PCB",
              "Recover silicon", "Sort housing", "Summary" };

        [MenuItem("RBv2_1_1/14 — Disassembly mode (guided rail + pages)", false, 14)]
        public static void Build_DisassemblyMode()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var rig = SpFind("DppSuperPanel");
            if (rig == null) { Debug.LogError("[DPPUIBuilder] No DppSuperPanel — run RBv2_1_1/10 first."); return; }
            var rail = rig.transform.Find("RailCanvas") as RectTransform;
            var data = rig.transform.Find("DataCanvas") as RectTransform;
            if (rail == null || data == null)
            { Debug.LogError("[DPPUIBuilder] Rig has no RailCanvas/DataCanvas — re-run RBv2_1_1/10."); return; }
            var view = rig.GetComponent<SuperPanelView>();

            // The STAGE CLONE's animator — the only DisassemblyAnimator under the
            // rig. (Menu 05/06 own the ORIGINAL; running them while this clone
            // exists is still forbidden — this phase never touches the original.)
            var stageAnimator = rig.GetComponentInChildren<DisassemblyAnimator>(true);
            if (stageAnimator == null)
                Debug.LogWarning("[DPPUIBuilder] No stage animator under the rig (VCU_assembly missing at /10?) — " +
                                 "the guided flow will run without model states.");
            // Ghost strength for the step focus: match the link's 0.30 standard —
            // the animator's default 0.1 was tuned for the how-to loop, and on
            // device it reads as parts MISSING rather than de-emphasised.
            else SetFloat(stageAnimator, "fadedAlpha", 0.30f);

            // ---- clean re-run ----
            var oldRail = rail.Find("DisassemblyRail");
            if (oldRail != null) Undo.DestroyObjectImmediate(oldRail.gameObject);
            foreach (var n in new[] { "DisIntroPage", "DisStepPage", "DisSummaryPage" })
            {
                var old = data.Find(n);
                if (old != null) Undo.DestroyObjectImmediate(old.gameObject);
            }

            var ctrl = rig.GetComponent<DisassemblyModeController>();
            if (ctrl == null) ctrl = Undo.AddComponent<DisassemblyModeController>(rig);

            // =============================================================
            // RAIL — 7 entries where the 4 tabs + CTA live
            // =============================================================
            var railGroup = Stretch("DisassemblyRail", rail);
            Undo.RegisterCreatedObjectUndo(railGroup.gameObject, "Build disassembly rail");

            AddText(TL("Eyebrow", railGroup, 18f, 10f, 184f, 14f), "GUIDED DISASSEMBLY", 9f,
                DPPTheme.TextCaption, bold: true);

            var eFills = new Image[7]; var eStrokes = new Image[7]; var eAccents = new Image[7];
            var eTicks = new GameObject[7]; var eLabels = new TMP_Text[7];
            var eDiscs = new Image[7]; var eDiscLabels = new TMP_Text[7]; var eButtons = new Button[7];

            for (int i = 0; i < 7; i++)
            {
                eButtons[i] = DisRailEntry(railGroup, i, out eFills[i], out eStrokes[i],
                    out eAccents[i], out eTicks[i], out eLabels[i], out eDiscs[i], out eDiscLabels[i]);
                WireClick(eButtons[i], ctrl, "OnEntry" + i);
            }

            // =============================================================
            // DATA — Intro page
            // =============================================================
            var intro = Stretch("DisIntroPage", data);
            Undo.RegisterCreatedObjectUndo(intro.gameObject, "Build disassembly intro page");
            AddImage(Stretch("PageBG", intro), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            AddText(TL("Title", intro, DisMargin, 27f, 300f, 26f), "Disassembly briefing", 19f,
                DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Rule", intro, DisMargin, 64f, DisW, 1f), null, DPPTheme.Hex("#1a335f"));

            AddText(TL("ToolsLbl", intro, DisMargin, 80f, 80f, 14f), "Tools", 10f, DPPTheme.TextLabel, bold: false);
            var introTools = AddText(TL("ToolsVal", intro, DisMargin + 86f, 78f, 286f, 16f),
                "Allen key (hex 2.5 mm)", 12.5f, DPPTheme.TextOnNavy, bold: true);
            AddText(TL("TimeLbl", intro, DisMargin, 104f, 80f, 14f), "Est. time", 10f, DPPTheme.TextLabel, bold: false);
            var introTime = AddText(TL("TimeVal", intro, DisMargin + 86f, 102f, 286f, 16f),
                "~5 min", 12.5f, DPPTheme.TextOnNavy, bold: true);
            AddText(TL("ScopeLbl", intro, DisMargin, 128f, 80f, 14f), "Scope", 10f, DPPTheme.TextLabel, bold: false);
            var introScope = AddText(TL("ScopeVal", intro, DisMargin + 86f, 126f, 286f, 16f),
                "5 steps", 12.5f, DPPTheme.TextOnNavy, bold: true);

            AddText(TL("DisEyebrow", intro, DisMargin, 158f, 300f, 14f), "DISMANTLING · 7 PART GROUPS", 9f,
                DPPTheme.TextCaption, bold: true);

            // Baked demo = the live payload's parts list (spec 08); Populate overwrites.
            string[] demoParts =
            {
                "Housing shells 2x (HPDC aluminium)", "Bare PCB, 4-layer FR-4",
                "Processors 2x FCBGA + flash 2x 4 GB", "Power stages 6x (DPAK)",
                "Regulators + AFE / transceivers + MEMS", "Connectors 3x AS018-35",
                "Fasteners 14x M3",
            };
            var partRows = new GameObject[7];
            var partLabels = new TMP_Text[7];
            for (int i = 0; i < 7; i++)
            {
                var row = TL($"Part{i}", intro, DisMargin, 180f + i * 24f, DisW, 20f);
                partRows[i] = row.gameObject;
                AddImage(TLCenter("Dot", row, 4f, 10f, 6f, 6f), DPPSpriteFactory.Circle64, DPPTheme.TealAccent);
                partLabels[i] = AddText(TL("Label", row, 16f, 1f, DisW - 16f, 18f), demoParts[i], 10.5f,
                    DPPTheme.TextOnNavy, bold: true);
            }

            var introBack = PsSmallPill(intro, "BackButton", DisMargin + 45f, 90f, "Back",
                primary: false, out _, cy: 402f, fontSize: 11f);
            WireClick(introBack, ctrl, nameof(DisassemblyModeController.OnIntroBack));
            var introStart = PsSmallPill(intro, "StartButton", 420f - DisMargin - 75f, 150f, "Start disassembly",
                primary: true, out _, cy: 402f, fontSize: 11f);
            WireClick(introStart, ctrl, nameof(DisassemblyModeController.OnIntroStart));

            // =============================================================
            // DATA — Step page (ONE page, rebound per step)
            // =============================================================
            var step = Stretch("DisStepPage", data);
            Undo.RegisterCreatedObjectUndo(step.gameObject, "Build disassembly step page");
            AddImage(Stretch("PageBG", step), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var stepTitle = AddText(TL("Title", step, DisMargin, 27f, 280f, 26f), "Open the housing", 19f,
                DPPTheme.TextOnNavy, bold: true);
            var stepCaption = AddText(TL("Caption", step, 280f, 33f, 116f, 14f), "Step 1 of 5", 10f,
                DPPTheme.TextTip, bold: false, align: TextAlignmentOptions.MidlineRight);
            AddImage(TL("Rule", step, DisMargin, 64f, DisW, 1f), null, DPPTheme.Hex("#1a335f"));

            var toolRow = TL("ToolRow", step, DisMargin, 74f, DisW, 18f);
            AddText(TL("ToolLbl", toolRow, 0f, 2f, 40f, 14f), "TOOL", 9f, DPPTheme.TextLabel, bold: true);
            var stepTool = AddText(TL("ToolVal", toolRow, 42f, 0f, 300f, 18f), "Allen key (hex 2.5 mm)", 11.5f,
                DPPTheme.TextOnNavy, bold: true);

            var t1 = DisTaskRow(step, "TaskRow1", 100f, ctrl, nameof(DisassemblyModeController.ToggleTask1),
                "Remove the 4 lid screws", "Allen key · M3 · keep them aside");
            var t2 = DisTaskRow(step, "TaskRow2", 174f, ctrl, nameof(DisassemblyModeController.ToggleTask2),
                "Lift off the upper housing shell", "Locating lip disengages · exposes the PCB");

            AddImage(TLCenter("LinkDot", step, DisMargin + 6f, 266f, 8f, 8f), DPPSpriteFactory.Circle64,
                DPPTheme.TealLight);
            AddText(TL("LinkLine", step, DisMargin + 18f, 259f, DisW - 18f, 15f),
                "Highlighted on the model: the parts this step removes", 10f, DPPTheme.TextCaption, bold: false);
            var gateHint = AddText(TL("GateHint", step, DisMargin, 286f, DisW, 15f),
                "Next unlocks when both tasks are ticked", 9.5f, DPPTheme.TextTip, bold: false);
            gateHint.fontStyle = FontStyles.Italic;

            var stepBack = PsSmallPill(step, "BackButton", DisMargin + 45f, 90f, "Back",
                primary: false, out var stepBackLbl, cy: 402f, fontSize: 11f);
            WireClick(stepBack, ctrl, nameof(DisassemblyModeController.OnBack));
            var stepNext = PsSmallPill(step, "NextButton", 420f - DisMargin - 75f, 150f, "Next",
                primary: true, out var stepNextLbl, cy: 402f, fontSize: 11f);
            WireClick(stepNext, ctrl, nameof(DisassemblyModeController.OnNext));
            var stepNextFill = stepNext.targetGraphic as Image;

            // ---- quit modal (step 1's Back; the run + timer are the stakes) ----
            var quitModal = Stretch("QuitModal", step);
            AddImage(Stretch("Blocker", quitModal), null, new Color(0f, 0f, 0f, 0.55f), raycast: true);
            var quitPanel = CenterIn("Panel", quitModal, 330f, 170f);
            AddImage(CenterIn("Stroke", quitPanel, 332f, 172f), DPPSpriteFactory.RoundedR20,
                DPPTheme.Hex("#1a2740"), sliced: true);
            AddImage(CenterIn("Fill", quitPanel, 330f, 170f), DPPSpriteFactory.RoundedR20,
                DPPTheme.Hex("#0d1526"), sliced: true, raycast: true);
            AddText(TL("Title", quitPanel, 24f, 24f, 282f, 20f), "Quit the disassembly?", 13.5f,
                DPPTheme.TextOnNavy, bold: true);
            var quitBody = AddText(TL("Body", quitPanel, 24f, 52f, 282f, 34f),
                "Progress and the run timer will be reset. You return to the briefing.", 10f,
                DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.TopLeft);
            quitBody.textWrappingMode = TextWrappingModes.Normal;
            var quitYes = PsSmallPill(quitPanel, "QuitYes", 89f, 110f, "Quit",
                primary: false, out _, cy: 128f, fontSize: 11f);
            WireClick(quitYes, ctrl, nameof(DisassemblyModeController.OnQuitYes));
            var quitNo = PsSmallPill(quitPanel, "QuitNo", 236f, 140f, "Keep working",
                primary: true, out _, cy: 128f, fontSize: 11f);
            WireClick(quitNo, ctrl, nameof(DisassemblyModeController.OnQuitNo));
            quitModal.gameObject.SetActive(false);

            // =============================================================
            // DATA — Summary page (spec 09 content refit to 420)
            // =============================================================
            var summary = Stretch("DisSummaryPage", data);
            Undo.RegisterCreatedObjectUndo(summary.gameObject, "Build disassembly summary page");
            AddImage(Stretch("PageBG", summary), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);
            var sumView = summary.gameObject.AddComponent<CompletionSummaryView>();

            AddText(TL("Title", summary, DisMargin, 18f, 320f, 20f), "Nice work — unit fully dismantled",
                14f, DPPTheme.TextOnNavy, bold: true);
            var timeValue = AddText(TL("TimeValue", summary, DisMargin, 40f, 220f, 30f), "— min — s", 22f,
                DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Rule", summary, DisMargin, 82f, DisW, 1f), null, DPPTheme.Hex("#1a335f"));
            AddText(TL("HdrTime", summary, 250f, 90f, 74f, 12f), "TIME", 8f, DPPTheme.TextCaption,
                bold: false, align: TextAlignmentOptions.MidlineRight);
            AddText(TL("HdrMass", summary, 320f, 90f, 76f, 12f), "RECOVERED", 8f, DPPTheme.TextCaption,
                bold: false, align: TextAlignmentOptions.MidlineRight);

            string[] demoTitles = { "1 · Open the housing", "2 · Remove the connectors",
                "3 · Lift out the main PCB", "4 · Recover the silicon", "5 · Sort the housing" };
            var sTitles = new TMP_Text[5]; var sMaterials = new TMP_Text[5];
            var sTimes = new TMP_Text[5]; var sMasses = new TMP_Text[5]; var sTags = new GameObject[5];
            for (int i = 0; i < 5; i++)
            {
                float y = 106f + i * 42f;
                sTitles[i] = AddText(TL($"RowTitle{i}", summary, DisMargin, y, 216f, 15f), demoTitles[i],
                    10.5f, DPPTheme.TextOnNavy, bold: true);
                sMaterials[i] = AddText(TL($"RowMats{i}", summary, DisMargin, y + 16f, 226f, 12f), "—",
                    8f, DPPTheme.TextCaption, bold: false);
                var tag = AddText(TL($"RowTag{i}", summary, 214f, y + 1f, 34f, 12f), "longest", 8f,
                    DPPTheme.Hex("#f0c879"), bold: false, align: TextAlignmentOptions.MidlineRight);
                sTags[i] = tag.gameObject;
                sTags[i].SetActive(false);
                sTimes[i] = AddText(TL($"RowTime{i}", summary, 250f, y, 74f, 15f), "—", 10.5f,
                    DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
                sMasses[i] = AddText(TL($"RowMass{i}", summary, 320f, y, 76f, 15f), "—", 10.5f,
                    DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.MidlineRight);
            }
            var footnote = AddText(TL("Footnote", summary, DisMargin, 322f, DisW, 14f),
                "material splits: assumed · to be validated in openLCA", 8f, DPPTheme.TextTip, bold: false);
            footnote.fontStyle = FontStyles.Italic;

            var sentMsg = AddText(TL("SentMsg", summary, DisMargin, 392f, 170f, 20f),
                "Report was successfully sent", 9f, DPPTheme.TealText, bold: false);
            sentMsg.gameObject.SetActive(false);
            var sumAction = PsSmallPill(summary, "ActionButton", 301f, 190f, "Send dismantling report",
                primary: true, out var sumActionLbl, cy: 402f, fontSize: 11f);
            WireClick(sumAction, ctrl, nameof(DisassemblyModeController.OnSummaryAction));

            // ---- post-report modal (round 2, feedback 3): the run is OVER ----
            // "Quit" (grey) → Welcome page; "Close app" (SOLID RED — 00 §2.1
            // meaning 3, the one sanctioned red button) → quits via the Welcome
            // controller so the app has a single quit path. The red/grey pair
            // differs in label, position AND size (mandatory colour-blind
            // mitigation, 00 §2.1).
            var nextModal = Stretch("NextModal", summary);
            AddImage(Stretch("Blocker", nextModal), null, new Color(0f, 0f, 0f, 0.55f), raycast: true);
            var nextPanel = CenterIn("Panel", nextModal, 330f, 170f);
            AddImage(CenterIn("Stroke", nextPanel, 332f, 172f), DPPSpriteFactory.RoundedR20,
                DPPTheme.Hex("#1a2740"), sliced: true);
            AddImage(CenterIn("Fill", nextPanel, 330f, 170f), DPPSpriteFactory.RoundedR20,
                DPPTheme.Hex("#0d1526"), sliced: true, raycast: true);
            AddText(TL("Title", nextPanel, 24f, 24f, 282f, 20f), "Report stored", 13.5f,
                DPPTheme.TextOnNavy, bold: true);
            AddText(TL("Body", nextPanel, 24f, 50f, 282f, 20f), "The dismantling session is complete.",
                10f, DPPTheme.TextSecondary, bold: false);
            var nextQuit = PsSmallPill(nextPanel, "QuitToWelcome", 84f, 100f, "Quit",
                primary: false, out _, cy: 128f, fontSize: 10.5f);
            WireClick(nextQuit, ctrl, nameof(DisassemblyModeController.OnSummaryQuit));
            var nextClose = PsSmallPill(nextPanel, "CloseApp", 236f, 130f, "Close app",
                primary: false, out _, cy: 128f, destructive: true, fontSize: 10.5f);
            WireClick(nextClose, ctrl, nameof(DisassemblyModeController.OnSummaryCloseApp));
            nextModal.gameObject.SetActive(false);

            // =============================================================
            // Wiring
            // =============================================================
            SetRef(sumView, "client", Object.FindFirstObjectByType<DPPClient>(FindObjectsInactive.Include));
            SetRef(sumView, "router", SpFindRouter());
            SetRef(sumView, "timeValue", timeValue);
            SetRefArray(sumView, "stepTitles", sTitles);
            SetRefArray(sumView, "stepMaterials", sMaterials);
            SetRefArray(sumView, "stepTimes", sTimes);
            SetRefArray(sumView, "stepMasses", sMasses);
            SetRefArray(sumView, "longestTags", sTags);
            SetRef(sumView, "actionLabel", sumActionLbl);
            SetRef(sumView, "actionButton", sumAction);
            SetRef(sumView, "sentMessage", sentMsg);
            SetRef(sumView, "nextModal", nextModal.gameObject);
            // actionChevron stays null — PsSmallPill has no chevron; the view is null-safe.

            SetRef(ctrl, "owner", view);
            var link = rig.GetComponentInChildren<ModelLinkController>(true);
            SetRef(ctrl, "modelLink", link);
            SetRef(ctrl, "stageAnimator", stageAnimator);
            SetRef(ctrl, "railGroup", railGroup.gameObject);
            SetRefArray(ctrl, "entryFills", eFills);
            SetRefArray(ctrl, "entryStrokes", eStrokes);
            SetRefArray(ctrl, "entryAccents", eAccents);
            SetRefArray(ctrl, "entryTicks", eTicks);
            SetRefArray(ctrl, "entryLabels", eLabels);
            SetRefArray(ctrl, "entryDiscs", eDiscs);
            SetRefArray(ctrl, "entryDiscLabels", eDiscLabels);
            SetRefArray(ctrl, "entryButtons", eButtons);
            SetRef(ctrl, "introPage", intro.gameObject);
            SetRef(ctrl, "stepPage", step.gameObject);
            SetRef(ctrl, "summaryPage", summary.gameObject);
            SetRef(ctrl, "introTools", introTools);
            SetRef(ctrl, "introTime", introTime);
            SetRef(ctrl, "introScope", introScope);
            SetRefArray(ctrl, "introPartRows", partRows);
            SetRefArray(ctrl, "introPartLabels", partLabels);
            SetRef(ctrl, "stepTitle", stepTitle);
            SetRef(ctrl, "stepCaption", stepCaption);
            SetRef(ctrl, "stepToolRow", toolRow.gameObject);
            SetRef(ctrl, "stepTool", stepTool);
            SetRef(ctrl, "task1Fill", t1.fill);
            SetRef(ctrl, "task1Cross", t1.cross);
            SetRef(ctrl, "task1Check", t1.check);
            SetRef(ctrl, "task1Title", t1.title);
            SetRef(ctrl, "task1Subtitle", t1.subtitle);
            SetRef(ctrl, "task1Button", t1.button);
            SetRef(ctrl, "task2Fill", t2.fill);
            SetRef(ctrl, "task2Cross", t2.cross);
            SetRef(ctrl, "task2Check", t2.check);
            SetRef(ctrl, "task2Title", t2.title);
            SetRef(ctrl, "task2Subtitle", t2.subtitle);
            SetRef(ctrl, "task2Button", t2.button);
            SetRef(ctrl, "gateHint", gateHint);
            SetRef(ctrl, "backButton", stepBack);
            SetRef(ctrl, "backLabel", stepBackLbl);
            SetRef(ctrl, "nextButton", stepNext);
            SetRef(ctrl, "nextFill", stepNextFill);
            SetRef(ctrl, "nextLabel", stepNextLbl);
            SetRef(ctrl, "quitModal", quitModal.gameObject);
            SetRef(ctrl, "summaryView", sumView);

            SetRef(view, "disassembly", ctrl);

            // Ships dormant: the gate CTA activates the group; pages activate per phase.
            railGroup.gameObject.SetActive(false);
            intro.gameObject.SetActive(false);
            step.gameObject.SetActive(false);
            summary.gameObject.SetActive(false);

            Selection.activeGameObject = rig;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/14 — guided disassembly mode built into the rig " +
                      "(rail group + 3 pages + controller). Run RBv2_1_1/Tools/Verify wiring, then SAVE THE SCENE.");
        }

        // =================================================================
        // Pieces
        // =================================================================

        /// <summary>One rail entry, full elevation kit. 184 × 44 at 52 pitch —
        /// 7 entries occupy y 34–390, the exact band of the passport's 4 tabs +
        /// CTA. Disc: "i" / step number / a three-bar summary glyph (drawn, never
        /// typed — glyph rule 00 §3.1).</summary>
        private static Button DisRailEntry(RectTransform group, int i, out Image fill, out Image stroke,
            out Image accent, out GameObject tick, out TMP_Text label, out Image disc, out TMP_Text discLabel)
        {
            var root = TL($"DisEntry{i}", group, 18f, 34f + i * 52f, 184f, 44f);
            AddShadow(root, 184f, 44f, DPPSpriteFactory.RoundedR13);

            var outline = AddImage(CenterIn("HoverOutline", root, 184f + HoverHalo, 44f + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            stroke = AddImage(CenterIn("Stroke", root, 184f, 44f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowStroke, sliced: true);
            fill = AddImage(CenterIn("Fill", root, 182f, 42f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);
            AddGloss(root, 184f, 44f, DPPSpriteFactory.RoundedR13);

            accent = AddImage(TL("Accent", root, 0f, 8f, 4f, 28f), DPPSpriteFactory.RoundedR3,
                DPPTheme.TealLight, sliced: true);
            accent.gameObject.SetActive(false);

            var discRT = TLCenter("Disc", root, 20f, 22f, 22f, 22f);
            disc = AddImage(discRT, DPPSpriteFactory.Circle64, DPPTheme.CardBlue);
            if (i == 6)
            {
                // Summary glyph: three capsule bars (a table), drawn not typed.
                discLabel = null;
                for (int b = 0; b < 3; b++)
                    AddImage(TLCenter($"Bar{b}", discRT, 11f, 7f + b * 4f, 10f, 2f),
                        DPPSpriteFactory.Grip, Color.white);
            }
            else
            {
                discLabel = AddText(Stretch("Num", discRT), i == 0 ? "i" : i.ToString(), 11f,
                    DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            }

            label = AddText(TL("Label", root, 38f, 14f, 130f, 16f), DisEntryLabels[i], 12f,
                DPPTheme.TextSecondary, bold: false);

            var tickRT = TLCenter("Tick", root, 168f, 12f, 14f, 14f);
            var tickImg = tickRT.gameObject.AddComponent<Image>();
            tickImg.preserveAspect = true;
            tickImg.raycastTarget = false;
            var tickSprite = LoadPageIcon("ic_visited");
            if (tickSprite != null) tickImg.sprite = tickSprite;
            else tickImg.enabled = false;
            tick = tickRT.gameObject;
            tick.SetActive(false);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        private struct DisTaskParts
        {
            public Button button;
            public Image fill;
            public GameObject cross, check;
            public TMP_Text title, subtitle;
        }

        /// <summary>A BOXED, PRESSABLE task row (chrome = touchable): the whole
        /// 372 × 64 row toggles the task — a far bigger target than the RB2_0
        /// 36-unit circle for a gloved hand. The status circle is a pure binary
        /// light: red ✗ / green ✓, marks drawn from capsule bars (00 §5), and its
        /// image is named "CircleFill" so the row's HoverHighlight (which resolves
        /// "Fill" by name) can never capture or repaint it — trap 1 avoided
        /// structurally.</summary>
        private static DisTaskParts DisTaskRow(RectTransform page, string name, float y,
            Component ctrl, string toggleMethod, string demoTitle, string demoSubtitle)
        {
            var row = TL(name, page, DisMargin, y, DisW, 64f);
            AddShadow(row, DisW, 64f, DPPSpriteFactory.RoundedR13);

            var outline = AddImage(CenterIn("HoverOutline", row, DisW + HoverHalo, 64f + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            AddImage(CenterIn("Stroke", row, DisW, 64f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowStroke, sliced: true);
            var rowFill = AddImage(CenterIn("Fill", row, DisW - 2f, 62f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);
            AddGloss(row, DisW, 64f, DPPSpriteFactory.RoundedR13, subtle: true);

            // Status circle — 34 visual, marks from 3 px capsule bars, never typed.
            var status = TLCenter("Status", row, 30f, 32f, 34f, 34f);
            var circle = AddImage(CenterIn("CircleFill", status, 34f, 34f), DPPSpriteFactory.Circle64,
                DPPTheme.Hex("#e24b4a"));

            var cross = CenterIn("Cross", status, 34f, 34f);
            var xb1 = TLCenter("Bar1", cross, 17f, 17f, 18f, 3f);
            xb1.localRotation = Quaternion.Euler(0f, 0f, 45f);
            AddImage(xb1, DPPSpriteFactory.Grip, Color.white);
            var xb2 = TLCenter("Bar2", cross, 17f, 17f, 18f, 3f);
            xb2.localRotation = Quaternion.Euler(0f, 0f, -45f);
            AddImage(xb2, DPPSpriteFactory.Grip, Color.white);

            var check = CenterIn("Check", status, 34f, 34f);
            var cb1 = TLCenter("Bar1", check, 12f, 20f, 8f, 3f);
            cb1.localRotation = Quaternion.Euler(0f, 0f, -45f);
            AddImage(cb1, DPPSpriteFactory.Grip, Color.white);
            var cb2 = TLCenter("Bar2", check, 20f, 17f, 15f, 3f);
            cb2.localRotation = Quaternion.Euler(0f, 0f, 45f);
            AddImage(cb2, DPPSpriteFactory.Grip, Color.white);
            check.gameObject.SetActive(false);

            var title = AddText(TL("Title", row, 58f, 12f, 300f, 18f), demoTitle, 12.5f,
                DPPTheme.TextOnNavy, bold: true);
            var subtitle = AddText(TL("Subtitle", row, 58f, 32f, 302f, 28f), demoSubtitle, 10f,
                DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.TopLeft);
            subtitle.textWrappingMode = TextWrappingModes.Normal;   // step-4's ~90-char subtitle wraps

            var btn = row.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = rowFill;
            WireClick(btn, ctrl, toggleMethod);
            var hover = row.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);

            return new DisTaskParts
            {
                button = btn, fill = circle, cross = cross.gameObject, check = check.gameObject,
                title = title, subtitle = subtitle,
            };
        }
    }
}
