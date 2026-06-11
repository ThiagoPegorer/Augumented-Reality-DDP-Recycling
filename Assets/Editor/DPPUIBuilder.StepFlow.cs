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
    /// Phase 4 builder — Screens 04–08: the guided step flow (spec 04 v2).
    /// Builds ONE StepFlow screen on the DPPPanelCanvas (content swapped per
    /// step by StepFlowController from backend v0.4 steps[]) plus the separate
    /// world-space ExplodedCanvas (268×430) with its own grabber bar.
    ///
    /// v1 interim: both previews show the static teardown PNG; animation,
    /// rotate/zoom and part highlighting are CAD-dependent (spec 04 §11).
    /// Demo content baked = step 1; runtime data overwrites.
    /// Safe to re-run (rebuilds StepFlow + ExplodedCanvas).
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

            // ---- Header: home + eyebrow + step indicator ----
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

            AddText(TL("Eyebrow", screen, 100, 28, 300, 16), "DISASSEMBLY · MS 50.4", 12, DPPTheme.TextCaption, bold: false);
            var stepInd = AddText(TL("StepIndicator", screen, 100, 44, 300, 20), "Step 1 of 5", 14.5f, DPPTheme.TextOnNavy, bold: true);

            // ---- Title ----
            var title = AddText(TL("Title", screen, 52, 88, 320, 32), "Open the housing", 25, DPPTheme.TextOnNavy, bold: true);

            // ---- Action cards ----
            var c1 = MakeActionCard(screen, "ActionCard1", 130, DPPSpriteFactory.IcCross,
                "Remove the housing screws", "Torx driver · keep them aside");
            var c2 = MakeActionCard(screen, "ActionCard2", 198, DPPSpriteFactory.IcUp,
                "Lift off the top cover", "Exposes the main PCB");

            // ---- How-to panel (static, spec 04 §5) ----
            var howto = TL("HowToPanel", screen, 368, 130, 244, 200);
            AddImage(Stretch("BG", howto), DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#08152b"), sliced: true);
            var howtoStroke = AddImage(Stretch("Stroke", howto), DPPSpriteFactory.RoundedR13, DPPTheme.RowStroke, sliced: true);
            howtoStroke.transform.SetSiblingIndex(0);
            ((RectTransform)howtoStroke.transform).offsetMin = new Vector2(-1, -1);
            ((RectTransform)howtoStroke.transform).offsetMax = new Vector2(1, 1);
            AddText(TL("Eyebrow", howto, 16, 12, 160, 14), "HOW TO · this step", 11, DPPTheme.TextCaption, bold: false);
            AddImage(CenterIn("BadgeBG", howto, 60, 18).Also(rt => { rt.anchorMin = rt.anchorMax = new Vector2(1, 1); rt.pivot = new Vector2(1, 1); rt.anchoredPosition = new Vector2(-12, -8); }),
                DPPSpriteFactory.RoundedR13, DPPTheme.CardBlue, sliced: true);
            AddText(TLCenter("BadgeLabel", howto, 202, 17, 56, 14), "static", 10, DPPTheme.TextSubtitleNavy, bold: false, align: TextAlignmentOptions.Center);
            var howtoImg = TLCenter("Preview", howto, 122, 102, 150, 140);
            var howtoImage = howtoImg.gameObject.AddComponent<Image>();
            howtoImage.sprite = LoadTeardownSprite();
            howtoImage.preserveAspect = true;
            howtoImage.raycastTarget = false;
            AddText(TLCenter("Caption", howto, 122, 186, 230, 12),
                "static preview · animates when the CAD model lands", 9.5f, DPPTheme.TextTip,
                bold: false, align: TextAlignmentOptions.Center);

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
            var confFill = AddImage(CenterIn("Fill", confirm, 398, 52), DPPSpriteFactory.RoundedR13, DPPTheme.TealAccent, sliced: true, raycast: true);
            var confLabel = AddText(Stretch("Label", confirm), "Confirm & next", 16, DPPTheme.TextOnNavy, bold: true, align: TextAlignmentOptions.Center);
            var chevTop = TLCenter("ChevronTop", confirm, 290, 22, 12, 2.5f);
            chevTop.localRotation = Quaternion.Euler(0, 0, -45);
            AddImage(chevTop, DPPSpriteFactory.Grip, Color.white);
            var chevBot = TLCenter("ChevronBottom", confirm, 290, 30, 12, 2.5f);
            chevBot.localRotation = Quaternion.Euler(0, 0, 45);
            AddImage(chevBot, DPPSpriteFactory.Grip, Color.white);
            var confBtn = confirm.gameObject.AddComponent<Button>();
            confBtn.transition = Selectable.Transition.None;
            confBtn.targetGraphic = confFill;
            WireClick(confBtn, controller, nameof(StepFlowController.Confirm));
            var confHover = confirm.gameObject.AddComponent<HoverHighlight>();
            SetRef(confHover, "highlightOutline", confOutline.gameObject);

            // ================= Exploded-view canvas =================
            var exploded = BuildExplodedCanvas(canvasRT);

            // ================= Wiring =================
            SetRef(controller, "router", router);
            SetRef(controller, "stepIndicator", stepInd);
            SetRef(controller, "titleText", title);
            SetRef(controller, "progressFill", fillRT);
            SetRef(controller, "progressLabel", progressLabel);
            SetRef(controller, "card1Ring", c1.ring);
            SetRef(controller, "card1Icon", c1.icon);
            SetRef(controller, "card1Title", c1.title);
            SetRef(controller, "card1Subtitle", c1.subtitle);
            SetRef(controller, "card2Ring", c2.ring);
            SetRef(controller, "card2Icon", c2.icon);
            SetRef(controller, "card2Title", c2.title);
            SetRef(controller, "card2Subtitle", c2.subtitle);
            SetRef(controller, "confirmLabel", confLabel);
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
            Debug.Log("[DPPUIBuilder] Phase 4 — Step Flow + ExplodedCanvas built. Save the scene.");
        }

        // =================================================================
        private struct ActionCardParts
        {
            public Image ring, icon;
            public TMP_Text title, subtitle;
        }

        private static ActionCardParts MakeActionCard(RectTransform screen, string name, float y,
            string iconSprite, string demoTitle, string demoSubtitle)
        {
            var card = TL(name, screen, 52, y, 300, 60);
            AddImage(CenterIn("Stroke", card, 302, 62), DPPSpriteFactory.RoundedR13, DPPTheme.RowStroke, sliced: true);
            AddImage(CenterIn("Fill", card, 300, 60), DPPSpriteFactory.RoundedR13, DPPTheme.RowFill, sliced: true);

            var ring = AddImage(TLCenter("IconRing", card, 28, 30, 32, 32), DPPSpriteFactory.Circle64, DPPTheme.TealLight);
            AddImage(TLCenter("IconBG", card, 28, 30, 29, 29), DPPSpriteFactory.Circle64, DPPTheme.CardBlue);
            var icon = AddImage(TLCenter("Icon", card, 28, 30, 17, 17), iconSprite, DPPTheme.TealLight);

            var title = AddText(TL("Title", card, 56, 8, 235, 20), demoTitle, 14.5f, DPPTheme.TextOnNavy, bold: true);
            var subtitle = AddText(TL("Subtitle", card, 56, 32, 235, 18), demoSubtitle, 12, DPPTheme.TextSecondary, bold: false);

            return new ActionCardParts { ring = ring, icon = icon, title = title, subtitle = subtitle };
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
