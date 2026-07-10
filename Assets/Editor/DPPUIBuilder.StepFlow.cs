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
    /// Phase 4 builder — Screens 04–08: the guided step flow (spec 04 v3, 2026-07-10).
    ///
    /// v3 changes (approved mock 04_step1_v3.svg):
    ///   - Header eyebrow ("DISASSEMBLY · MS 50.4") + "Step n of 5" REMOVED —
    ///     the progress rail's n/5 label carries that info. Home button stays.
    ///   - Action cards UNBOXED into task rows; the icon circle is now a
    ///     clickable STATUS BUTTON: red + action glyph (pending) → green +
    ///     check (done). Hint line explains the tap.
    ///   - "Confirm & next" starts LOCKED (grey); both tasks green unlock it.
    ///   - How-to panel: static PNG → live per-step animation (StepHowToLoop
    ///     films VCU_assembly via the shared TeardownPreviewCamera into a
    ///     RenderTexture/RawImage). Badge "static" → "loop".
    ///
    /// The exploded-view canvas is UNCHANGED in v3 (still the static render) —
    /// its live-3D/orbit/zoom upgrade is the next work block.
    /// Safe to re-run (rebuilds StepFlow + ExplodedCanvas only).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("DPP/Build Phase 4 — Step Flow", false, 4)]
        public static void BuildPhase4()
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

            var oldScreen = canvasRT.Find("StepFlow");
            if (oldScreen != null) Undo.DestroyObjectImmediate(oldScreen.gameObject);
            RemoveByName("ExplodedCanvas"); // root-iteration: also finds it when inactive

            // ================= Instruction screen =================
            var screen = Stretch("StepFlow", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build DPP Step Flow");
            var controller = screen.gameObject.AddComponent<StepFlowController>();

            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---- Progress bar (far-left, vertical) ----
            AddImage(TL("ProgressTrack", screen, 22, 26, 8, 378), DPPSpriteFactory.RoundedR3, DPPTheme.ScrollTrack, sliced: true);
            var fillRT = NewRT("ProgressFill", screen);
            fillRT.anchorMin = fillRT.anchorMax = new Vector2(0f, 1f);
            fillRT.pivot = new Vector2(0f, 1f); // grows downward (top→bottom)
            fillRT.anchoredPosition = new Vector2(22, -26);
            fillRT.sizeDelta = new Vector2(8, 76);
            var fillImg = fillRT.gameObject.AddComponent<Image>();
            fillImg.sprite = DPPSpriteFactory.Load(DPPSpriteFactory.RoundedR3);
            fillImg.type = Image.Type.Sliced;
            fillImg.color = DPPTheme.TealLight;
            fillImg.raycastTarget = false;
            var progressLabel = AddText(TLCenter("ProgressLabel", screen, 26, 416, 40, 14), "1/5", 11,
                DPPTheme.TealMuted, bold: false, align: TextAlignmentOptions.Center);

            // ---- Header: home button only (v3 — eyebrow + step indicator removed) ----
            var home = TLCenter("HomeButton", screen, 70, 46, 36, 36);
            var homeOutline = AddImage(CenterIn("HoverOutline", home, 46, 46), DPPSpriteFactory.Circle64, Color.white);
            homeOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Ring", home, 39, 39), DPPSpriteFactory.Circle64, DPPTheme.TabActiveStroke);
            var homeFill = AddImage(CenterIn("Fill", home, 36, 36), DPPSpriteFactory.Circle64, DPPTheme.CardBlue, sliced: false, raycast: true);
            AddImage(CenterIn("Icon", home, 20, 20), DPPSpriteFactory.IcHouse, Color.white);
            var homeBtn = home.gameObject.AddComponent<Button>();
            homeBtn.transition = Selectable.Transition.None;
            homeBtn.targetGraphic = homeFill;
            if (router != null) WireClick(homeBtn, router, nameof(ScreenRouter.ShowMainPage));
            var homeHover = home.gameObject.AddComponent<HoverHighlight>();
            SetRef(homeHover, "highlightOutline", homeOutline.gameObject);

            // ---- Title ----
            var title = AddText(TL("Title", screen, 52, 88, 320, 32), "Open the housing", 25, DPPTheme.TextOnNavy, bold: true);

            // ---- Task rows (unboxed, with status buttons) ----
            var r1 = MakeTaskRow(screen, "TaskRow1", 142, DPPSpriteFactory.IcCross,
                "Remove the 4 lid screws", "Allen key · M3 · keep them aside",
                controller, nameof(StepFlowController.ToggleTask1));
            var r2 = MakeTaskRow(screen, "TaskRow2", 210, DPPSpriteFactory.IcUp,
                "Lift off the top cover", "Locating lip disengages · exposes the PCB",
                controller, nameof(StepFlowController.ToggleTask2));

            AddText(TL("TaskHint", screen, 52, 278, 460, 16),
                "Tap the icon when a task is done — both green unlock the next step",
                11, DPPTheme.TextTip, bold: false);

            // ---- How-to preview (v3.1: frameless — floats on the navy panel
            // like the intro's preview, same 242×225 slot; no box, no eyebrow,
            // no badge, no caption) ----
            var howto = TLCenter("HowToPreview", screen, 468, 205, 242, 225);
            var howtoRaw = howto.gameObject.AddComponent<RawImage>();
            howtoRaw.raycastTarget = false;
            howtoRaw.enabled = false; // StepHowToLoop enables it with the RT at runtime

            // StepHowToLoop on the preview GO (enabled/disabled with the screen).
            var loop = howto.gameObject.AddComponent<StepHowToLoop>();
            SetRef(loop, "target", howtoRaw);
            SetRef(loop, "previewCamera", FindOrCreatePreviewCamera());
            var animator = Object.FindFirstObjectByType<DisassemblyAnimator>();
            if (animator != null) SetRef(loop, "vcuAnimator", animator);
            else Debug.LogWarning("[DPPUIBuilder] No DisassemblyAnimator found (VCU_assembly missing?) — how-to loop will retry at runtime.");

            // ---- Nav buttons ----
            var back = TL("BackButton", screen, 52, 350, 150, 52);
            var backOutline = AddImage(CenterIn("HoverOutline", back, 158, 60), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            backOutline.gameObject.SetActive(false);
            AddImage(CenterIn("Stroke", back, 152, 54), DPPSpriteFactory.RoundedR13, DPPTheme.TabInactiveFill, sliced: true);
            var backFill = AddImage(CenterIn("Fill", back, 150, 52), DPPSpriteFactory.RoundedR13, DPPTheme.SecondaryButtonFill, sliced: true, raycast: true);
            AddText(Stretch("Label", back), "‹ Back", 15, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.Center);
            var backBtn = back.gameObject.AddComponent<Button>();
            backBtn.transition = Selectable.Transition.None;
            backBtn.targetGraphic = backFill;
            WireClick(backBtn, controller, nameof(StepFlowController.BackStep));
            var backHover = back.gameObject.AddComponent<HoverHighlight>();
            SetRef(backHover, "highlightOutline", backOutline.gameObject);

            var confirm = TL("ConfirmButton", screen, 214, 350, 398, 52);
            var confOutline = AddImage(CenterIn("HoverOutline", confirm, 406, 60), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            confOutline.gameObject.SetActive(false);
            // Baked in the LOCKED state (tasks start pending); controller drives colors.
            var confFill = AddImage(CenterIn("Fill", confirm, 398, 52), DPPSpriteFactory.RoundedR13, DPPTheme.SecondaryButtonFill, sliced: true, raycast: true);
            var confLabel = AddText(Stretch("Label", confirm), "Confirm & next", 16, DPPTheme.Hex("#5d7396"), bold: true, align: TextAlignmentOptions.Center);
            var chevTop = TLCenter("ChevronTop", confirm, 290, 22, 12, 2.5f);
            chevTop.localRotation = Quaternion.Euler(0, 0, -45);
            var chev1 = AddImage(chevTop, DPPSpriteFactory.Grip, DPPTheme.Hex("#5d7396"));
            var chevBot = TLCenter("ChevronBottom", confirm, 290, 30, 12, 2.5f);
            chevBot.localRotation = Quaternion.Euler(0, 0, 45);
            var chev2 = AddImage(chevBot, DPPSpriteFactory.Grip, DPPTheme.Hex("#5d7396"));
            var confBtn = confirm.gameObject.AddComponent<Button>();
            confBtn.transition = Selectable.Transition.None;
            confBtn.targetGraphic = confFill;
            confBtn.interactable = false;
            WireClick(confBtn, controller, nameof(StepFlowController.Confirm));
            var confHover = confirm.gameObject.AddComponent<HoverHighlight>();
            SetRef(confHover, "highlightOutline", confOutline.gameObject);
            confHover.enabled = false;

            // ---- Cancel modal (hidden; Back opens it from any step) ----
            var modal = BuildCancelModal(screen, controller);
            SetRef(controller, "cancelModal", modal);

            // ================= Exploded-view canvas (unchanged in v3) =================
            var exploded = BuildExplodedCanvas(canvasRT);

            // ================= Wiring =================
            SetRef(controller, "router", router);
            SetRef(controller, "howToLoop", loop);
            SetRef(controller, "titleText", title);
            SetRef(controller, "progressFill", fillRT);
            SetRef(controller, "progressLabel", progressLabel);
            SetRef(controller, "task1Fill", r1.fill);
            SetRef(controller, "task1Icon", r1.icon);
            SetRef(controller, "task1Check", r1.check);
            SetRef(controller, "task1Title", r1.title);
            SetRef(controller, "task1Subtitle", r1.subtitle);
            SetRef(controller, "task2Fill", r2.fill);
            SetRef(controller, "task2Icon", r2.icon);
            SetRef(controller, "task2Check", r2.check);
            SetRef(controller, "task2Title", r2.title);
            SetRef(controller, "task2Subtitle", r2.subtitle);
            SetRef(controller, "confirmButton", confBtn);
            SetRef(controller, "confirmFill", confFill);
            SetRef(controller, "confirmLabel", confLabel);
            SetRef(controller, "confirmChevron1", chev1);
            SetRef(controller, "confirmChevron2", chev2);
            SetRef(controller, "confirmHover", confHover);
            WireIconLookup(controller);

            if (router != null)
            {
                SetRef(router, "stepFlow", screen.gameObject);
                SetRef(router, "explodedCanvas", exploded);
            }
            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "stepFlow", controller);

            screen.gameObject.SetActive(false);
            exploded.SetActive(false);

            Selection.activeGameObject = screen.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 4 — Step Flow v3 + ExplodedCanvas built. Save the scene.");
        }

        /// <summary>Confirmation modal over the step screen: dim overlay (blocks
        /// clicks) + centered card, "Want to cancel this disassembly?", green Yes
        /// (→ main page) and red No (→ dismiss). Built inactive, LAST sibling so
        /// it draws on top of everything.</summary>
        private static GameObject BuildCancelModal(RectTransform screen, StepFlowController controller)
        {
            var modal = Stretch("CancelModal", screen);

            // Dim overlay — raycast target so it blocks the UI behind.
            AddImage(Stretch("Dim", modal), DPPSpriteFactory.RoundedR22,
                new Color(0f, 0f, 0f, 0.55f), sliced: true, raycast: true);

            var card = CenterIn("Card", modal, 400, 170);
            AddImage(CenterIn("Stroke", card, 404, 174), DPPSpriteFactory.RoundedR20, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("Fill", card, 400, 170), DPPSpriteFactory.RoundedR20, DPPTheme.Hex("#0d2a57"), sliced: true);

            AddText(TL("Title", card, 20, 40, 360, 26), "Want to cancel this disassembly?",
                17, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);

            MakeModalButton(card, "YesButton", 43, DPPTheme.TealAccent, "Yes",
                controller, nameof(StepFlowController.CancelYes));
            MakeModalButton(card, "NoButton", 207, DPPTheme.Hex("#e24b4a"), "No",
                controller, nameof(StepFlowController.CancelNo));

            modal.gameObject.SetActive(false);
            return modal.gameObject;
        }

        private static void MakeModalButton(RectTransform card, string name, float x, Color fill,
            string label, StepFlowController controller, string method)
        {
            var btn = TL(name, card, x, 92, 150, 46);
            var outline = AddImage(CenterIn("HoverOutline", btn, 158, 54), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);
            var fillImg = AddImage(CenterIn("Fill", btn, 150, 46), DPPSpriteFactory.RoundedR13, fill, sliced: true, raycast: true);
            AddText(Stretch("Label", btn), label, 15, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            var b = btn.gameObject.AddComponent<Button>();
            b.transition = Selectable.Transition.None;
            b.targetGraphic = fillImg;
            WireClick(b, controller, method);
            var hover = btn.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
        }

        /// <summary>The shared preview camera (created by Phase 3; recreated here if missing).</summary>
        private static Camera FindOrCreatePreviewCamera()
        {
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
                if (root.name == PreviewCamName) return root.GetComponent<Camera>();

            var camGO = new GameObject(PreviewCamName, typeof(Camera));
            Undo.RegisterCreatedObjectUndo(camGO, "Build DPP Step Flow");
            var cam = camGO.GetComponent<Camera>();
            cam.enabled = false;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0f, 0f, 0f, 0f);
            return cam;
        }

        // =================================================================
        private struct TaskRowParts
        {
            public Image fill, icon;
            public GameObject check;
            public TMP_Text title, subtitle;
        }

        /// <summary>Unboxed task row (spec 04 v3): clickable status circle (36 px,
        /// red glyph → green check) + bold title + subtitle. No card box.</summary>
        private static TaskRowParts MakeTaskRow(RectTransform screen, string name, float y,
            string iconSprite, string demoTitle, string demoSubtitle,
            StepFlowController controller, string toggleMethod)
        {
            var row = TL(name, screen, 52, y, 320, 56);

            // Status button — the only interactive element of the row.
            var status = TLCenter("StatusButton", row, 18, 18, 36, 36);
            var outline = AddImage(CenterIn("HoverOutline", status, 46, 46), DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            var fill = AddImage(CenterIn("Fill", status, 36, 36), DPPSpriteFactory.Circle64,
                DPPTheme.Hex("#e24b4a"), sliced: false, raycast: true);
            var icon = AddImage(CenterIn("Icon", status, 18, 18), iconSprite, Color.white);

            // Check mark (hidden at rest) — two capsule bars forming a ✓.
            var check = CenterIn("Check", status, 36, 36);
            var bar1 = TLCenter("Bar1", check, 13, 21, 9, 3);
            bar1.localRotation = Quaternion.Euler(0, 0, -45);
            AddImage(bar1, DPPSpriteFactory.Grip, Color.white);
            var bar2 = TLCenter("Bar2", check, 21, 18, 16, 3);
            bar2.localRotation = Quaternion.Euler(0, 0, 45);
            AddImage(bar2, DPPSpriteFactory.Grip, Color.white);
            check.gameObject.SetActive(false);

            var btn = status.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            WireClick(btn, controller, toggleMethod);
            var hover = status.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);

            var title = AddText(TL("Title", row, 50, 0, 270, 20), demoTitle, 14.5f, DPPTheme.TextOnNavy, bold: true);
            var subtitle = AddText(TL("Subtitle", row, 50, 22, 270, 18), demoSubtitle, 12, DPPTheme.TextSecondary, bold: false);

            return new TaskRowParts { fill = fill, icon = icon, check = check.gameObject, title = title, subtitle = subtitle };
        }

        private static void WireIconLookup(StepFlowController controller)
        {
            string[] keys = { "cross", "up", "pins", "usb", "lever", "board", "magnify", "chip", "recycle", "label" };
            string[] sprites = {
                DPPSpriteFactory.IcCross, DPPSpriteFactory.IcUp, DPPSpriteFactory.IcPins,
                DPPSpriteFactory.IcUsb, DPPSpriteFactory.IcLever, DPPSpriteFactory.IcBoard,
                DPPSpriteFactory.IcMagnify, DPPSpriteFactory.IcChip, DPPSpriteFactory.Recycle,
                DPPSpriteFactory.IcLabel
            };
            var so = new SerializedObject(controller);
            var keysProp = so.FindProperty("iconKeys");
            var sprProp = so.FindProperty("iconSprites");
            keysProp.arraySize = keys.Length;
            sprProp.arraySize = keys.Length;
            for (int i = 0; i < keys.Length; i++)
            {
                keysProp.GetArrayElementAtIndex(i).stringValue = keys[i];
                sprProp.GetArrayElementAtIndex(i).objectReferenceValue = DPPSpriteFactory.Load(sprites[i]);
            }
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // =================================================================
        private static GameObject BuildExplodedCanvas(RectTransform mainCanvasRT)
        {
            var go = new GameObject("ExplodedCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(268, 430);
            // Default: to the right of the main panel (main 0.64 m wide → half 0.32,
            // gap 0.06, exploded half 0.134). Worker repositions via grabber bar.
            rt.position = mainCanvasRT.position + new Vector3(0.514f, 0f, 0f);
            rt.localScale = Vector3.one * 0.001f;

            // Panel surface (stroke-behind).
            var stroke = AddImage(Stretch("Stroke", rt), DPPSpriteFactory.RoundedR20, DPPTheme.Hex("#1a335f"), sliced: true);
            ((RectTransform)stroke.transform).offsetMin = new Vector2(-1, -1);
            ((RectTransform)stroke.transform).offsetMax = new Vector2(1, 1);
            AddImage(Stretch("Fill", rt), DPPSpriteFactory.RoundedR20, DPPTheme.NavyCanvas3D, sliced: true);

            AddText(TL("Eyebrow", rt, 18, 20, 160, 16), "EXPLORE · 3D model", 12, DPPTheme.TextCaption, bold: false);
            var badge = TL("Badge", rt, 160, 14, 92, 20);
            AddImage(CenterIn("BadgeStroke", badge, 94, 22), DPPSpriteFactory.RoundedR13, DPPTheme.TabActiveStroke, sliced: true);
            AddImage(CenterIn("BadgeFill", badge, 92, 20), DPPSpriteFactory.RoundedR13, DPPTheme.CardBlue, sliced: true);
            AddText(Stretch("BadgeLabel", badge), "static preview", 10.5f, DPPTheme.TextSubtitleNavy, bold: false, align: TextAlignmentOptions.Center);

            var img = TLCenter("Teardown", rt, 134, 210, 250, 232);
            var image = img.gameObject.AddComponent<Image>();
            image.sprite = LoadTeardownSprite();
            image.preserveAspect = true;
            image.raycastTarget = false;

            AddText(TLCenter("Note1", rt, 134, 372, 240, 14), "rotate · zoom · part highlight", 10.5f,
                DPPTheme.TextTip, bold: false, align: TextAlignmentOptions.Center);
            AddText(TLCenter("Note2", rt, 134, 388, 240, 14), "planned with the CAD model", 10.5f,
                DPPTheme.TextTip, bold: false, align: TextAlignmentOptions.Center);

            // Grabber bar (own transform → moves only this canvas).
            var barGO = new GameObject("GrabberBar", typeof(RectTransform));
            var bar = (RectTransform)barGO.transform;
            bar.SetParent(rt, false);
            bar.anchorMin = bar.anchorMax = new Vector2(0.5f, 0f);
            bar.pivot = new Vector2(0.5f, 1f);
            bar.anchoredPosition = new Vector2(0f, -12f);
            bar.sizeDelta = new Vector2(200f, 22f);
            AddImage(CenterIn("Stroke", bar, 202, 24), DPPSpriteFactory.Pill, DPPTheme.GrabberStroke);
            var fill = AddImage(CenterIn("Fill", bar, 200, 22), DPPSpriteFactory.Pill, DPPTheme.GrabberFill, raycast: true);
            var grip = AddImage(CenterIn("Grip", bar, 44, 4), DPPSpriteFactory.Grip, DPPTheme.GrabberGrip);
            var handle = barGO.AddComponent<PanelGrabHandle>();
            SetRef(handle, "panelRoot", rt);
            SetRef(handle, "barFill", fill);
            SetRef(handle, "grip", grip);

            Undo.RegisterCreatedObjectUndo(go, "Build Exploded Canvas");
            return go;
        }
    }

    /// <summary>Tiny fluent helper for one-off RectTransform tweaks in builders.</summary>
    internal static class RectTransformBuilderExt
    {
        public static RectTransform Also(this RectTransform rt, System.Action<RectTransform> f)
        {
            f(rt);
            return rt;
        }
    }
}
