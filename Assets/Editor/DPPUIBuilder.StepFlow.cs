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

        /// <summary>Sets a private [SerializeField] bool by name (SetRef handles object refs only).</summary>
        private static void SetBool(Object target, string fieldName, bool value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop == null) { Debug.LogWarning($"[DPPUIBuilder] Bool field '{fieldName}' not found on {target.GetType().Name}."); return; }
            prop.boolValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
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
        // Exploded ACTION ZONE (v3.0 RESET, 2026-07-19)
        // After on-device testing the v2.x control set was removed wholesale.
        // The zone now contains ONLY the model anchor (runtime clone) and the
        // grabber bar. ExplodedZoneInteraction manages the clone lifecycle;
        // the constrained-body engine stays dormant underneath for the next
        // interaction design.
        // =================================================================
        private static GameObject BuildExplodedCanvas(RectTransform mainCanvasRT)
        {
            var go = new GameObject("ExplodedCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            var rt = (RectTransform)go.transform;
            const float ZW = 340f, ZH = 300f;
            rt.sizeDelta = new Vector2(ZW, ZH);
            rt.position = mainCanvasRT.position + new Vector3(0.55f, 0f, 0f);
            rt.localScale = Vector3.one * 0.001f;

            // Model anchor: clone parented here at runtime. Canvas scale is
            // 0.001, so 1000 restores world scale; slightly in front of the plane.
            var anchorGO = new GameObject("ModelAnchor", typeof(RectTransform));
            var anchor = (RectTransform)anchorGO.transform;
            anchor.SetParent(rt, false);
            anchor.anchoredPosition3D = new Vector3(0f, 0f, -70f);
            anchor.localScale = Vector3.one * 1000f;

            var interaction = go.AddComponent<ExplodedZoneInteraction>();
            var vcuAnimator = Object.FindFirstObjectByType<DisassemblyAnimator>();
            if (vcuAnimator != null) SetRef(interaction, "modelSource", vcuAnimator.transform);
            else Debug.LogWarning("[DPPUIBuilder] No DisassemblyAnimator (VCU_assembly missing?) — zone model will retry at runtime.");
            SetRef(interaction, "modelAnchor", anchor);

            // Rotation v4.0: two-hand twist → yaw. Rotates the anchor, so the
            // clone's constrained-body axes are unaffected.
            var twist = go.AddComponent<TwoHandTwistRotate>();
            SetRef(twist, "target", anchor);

            // Grab handle (v3.1): CIRCLE pinned below the model's front face —
            // black outer disc, gray inner disc (→ white on hover/drag, driven
            // by PanelGrabHandle's existing hover colors). ExplodedZoneInteraction
            // repositions it every frame; the initial pose is a placeholder.
            var handleGO = new GameObject("GrabHandle", typeof(RectTransform));
            var handleCircle = (RectTransform)handleGO.transform;
            handleCircle.SetParent(rt, false);
            handleCircle.anchorMin = handleCircle.anchorMax = new Vector2(0.5f, 0.5f);
            handleCircle.pivot = new Vector2(0.5f, 0.5f);
            handleCircle.anchoredPosition3D = new Vector3(0f, -(ZH / 2f) - 20f, -70f);
            handleCircle.sizeDelta = new Vector2(52f, 52f);   // hit area > visual (unchanged — grabbing stays easy)
            // Visual sizes tuned by Thiago on device 2026-07-19: small & discreet.
            var fill = AddImage(CenterIn("Fill", handleCircle, 15, 15), DPPSpriteFactory.Circle64, DPPTheme.GrabberFill, sliced: false, raycast: true);
            var grip = AddImage(CenterIn("Grip", handleCircle, 5, 5), DPPSpriteFactory.Circle64, DPPTheme.GrabberGrip);
            var handle = handleGO.AddComponent<PanelGrabHandle>();
            SetRef(handle, "panelRoot", rt);
            SetRef(handle, "barFill", fill);
            SetRef(handle, "grip", grip);
            SetBool(handle, "recenterOnStart", false);   // zone keeps its spot beside the main panel
            SetRef(interaction, "grabHandle", handleCircle);

            // Preview-layer split unchanged: original films for the RT previews,
            // the user sees only the clone.
            ConfigurePreviewLayer(vcuAnimator);

            Undo.RegisterCreatedObjectUndo(go, "Build Exploded Canvas");
            return go;
        }

        /// <summary>Puts the original VCU_assembly on a dedicated "DPPPreview"
        /// layer, hides that layer from the main camera, and restricts the
        /// shared preview camera to it — so the intro/how-to RenderTextures
        /// keep filming the original while the user only ever sees the clone.</summary>
        private static void ConfigurePreviewLayer(DisassemblyAnimator vcuAnimator)
        {
            int layer = EnsureLayer("DPPPreview");
            if (layer < 0 || vcuAnimator == null) return;

            SetLayerRecursiveEditor(vcuAnimator.transform, layer);

            var mainCam = Camera.main;
            if (mainCam != null)
            {
                mainCam.cullingMask &= ~(1 << layer);
                EditorUtility.SetDirty(mainCam);
            }

            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name != PreviewCamName) continue;
                var cam = root.GetComponent<Camera>();
                if (cam != null) { cam.cullingMask = 1 << layer; EditorUtility.SetDirty(cam); }
            }
            Debug.Log($"[DPPUIBuilder] Preview layer '{"DPPPreview"}' (index {layer}) configured: original model hidden from the main camera.");
        }

        private static void SetLayerRecursiveEditor(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            EditorUtility.SetDirty(t.gameObject);
            foreach (Transform c in t) SetLayerRecursiveEditor(c, layer);
        }

        /// <summary>Finds or creates a named layer in TagManager. Returns its index, or −1.</summary>
        private static int EnsureLayer(string name)
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (assets == null || assets.Length == 0) { Debug.LogWarning("[DPPUIBuilder] TagManager not found."); return -1; }
            var tagManager = new SerializedObject(assets[0]);
            var layers = tagManager.FindProperty("layers");

            for (int i = 8; i < 32; i++)
                if (layers.GetArrayElementAtIndex(i).stringValue == name) return i;

            for (int i = 8; i < 32; i++)
            {
                var slot = layers.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(slot.stringValue))
                {
                    slot.stringValue = name;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"[DPPUIBuilder] Created layer '{name}' at index {i}.");
                    return i;
                }
            }
            Debug.LogWarning("[DPPUIBuilder] No free layer slot for DPPPreview.");
            return -1;
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
