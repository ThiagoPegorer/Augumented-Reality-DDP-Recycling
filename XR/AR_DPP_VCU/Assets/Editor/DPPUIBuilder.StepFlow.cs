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
    /// RBv2_1_1/06 builder — Screens 04–08: the guided step flow (spec 04 v3, 2026-07-10).
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
        [MenuItem("RBv2_1_1/06 — Step flow + action zone", false, 6)]
        public static void Build5_StepFlowAndZone()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run RBv2_1_1/01 first.");
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
            var homeOutline = AddImage(CenterIn("HoverOutline", home, 42, 42), DPPSpriteFactory.Circle64, Color.white);
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
            var r1 = MakeTaskRow(screen, "TaskRow1", 142,
                "Remove the 4 lid screws", "Allen key · M3 · keep them aside",
                controller, nameof(StepFlowController.ToggleTask1));
            var r2 = MakeTaskRow(screen, "TaskRow2", 210,
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
            var backOutline = AddImage(CenterIn("HoverOutline", back, 156, 58), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
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
            var confOutline = AddImage(CenterIn("HoverOutline", confirm, 404, 58), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
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
            SetRef(controller, "task1Cross", r1.cross);
            SetRef(controller, "task1Check", r1.check);
            SetRef(controller, "task1Title", r1.title);
            SetRef(controller, "task1Subtitle", r1.subtitle);
            SetRef(controller, "task2Fill", r2.fill);
            SetRef(controller, "task2Cross", r2.cross);
            SetRef(controller, "task2Check", r2.check);
            SetRef(controller, "task2Title", r2.title);
            SetRef(controller, "task2Subtitle", r2.subtitle);
            SetRef(controller, "confirmButton", confBtn);
            SetRef(controller, "confirmFill", confFill);
            SetRef(controller, "confirmLabel", confLabel);
            SetRef(controller, "confirmChevron1", chev1);
            SetRef(controller, "confirmChevron2", chev2);
            SetRef(controller, "confirmHover", confHover);

            if (router != null)
            {
                SetRef(router, "stepFlow", screen.gameObject);
                SetRef(router, "explodedCanvas", exploded);
            }
            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "stepFlow", controller);

            // SELF-HEALING WIRE (see the note in RBv2_1_1/03). RBv2_1_1/07 sets
            // StepFlowController.summary; rebuilding the step flow destroys that
            // controller, so running /5 after /6 would leave "Finish & see summary"
            // unable to hand the session over. Re-point it here.
            var summaryView = FindAnyIncludingInactive<CompletionSummaryView>();
            if (summaryView != null) SetRef(controller, "summary", summaryView);

            screen.gameObject.SetActive(false);
            exploded.SetActive(false);

            Selection.activeGameObject = screen.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/06 — Step Flow v3 + ExplodedCanvas built. Save the scene.");
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
            var outline = AddImage(CenterIn("HoverOutline", btn, 156, 52), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
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

        /// <summary>The shared preview camera (created by RBv2_1_1/05; recreated here if missing).</summary>
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
            public Image fill;
            public GameObject cross, check;
            public TMP_Text title, subtitle;
        }

        /// <summary>Unboxed task row (spec 04 v3): clickable status circle + bold
        /// title + subtitle. No card box.
        ///
        /// v3.2 (Thiago, 2026-08-01): the circle is now a pure BINARY status light —
        /// red ✗ = not done, green ✓ = done. It used to show the action's own glyph
        /// (screw, lever, board …) while pending, which read as decoration rather
        /// than as an unfinished task. Both marks are drawn from the same 3 px
        /// capsule so the pair matches in weight; per-action glyphs are gone.</summary>
        private static TaskRowParts MakeTaskRow(RectTransform screen, string name, float y,
            string demoTitle, string demoSubtitle,
            StepFlowController controller, string toggleMethod)
        {
            var row = TL(name, screen, 52, y, 320, 56);

            // Status button — the only interactive element of the row.
            var status = TLCenter("StatusButton", row, 18, 18, 36, 36);
            var outline = AddImage(CenterIn("HoverOutline", status, 42, 42), DPPSpriteFactory.Circle64, Color.white);
            outline.gameObject.SetActive(false);
            var fill = AddImage(CenterIn("Fill", status, 36, 36), DPPSpriteFactory.Circle64,
                DPPTheme.Hex("#e24b4a"), sliced: false, raycast: true);

            // Cross mark (shown at rest, on the red fill) — two capsule bars forming an ✗.
            var cross = CenterIn("Cross", status, 36, 36);
            var xbar1 = TLCenter("Bar1", cross, 18, 18, 20, 3);
            xbar1.localRotation = Quaternion.Euler(0, 0, 45);
            AddImage(xbar1, DPPSpriteFactory.Grip, Color.white);
            var xbar2 = TLCenter("Bar2", cross, 18, 18, 20, 3);
            xbar2.localRotation = Quaternion.Euler(0, 0, -45);
            AddImage(xbar2, DPPSpriteFactory.Grip, Color.white);

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

            return new TaskRowParts { fill = fill, cross = cross.gameObject, check = check.gameObject, title = title, subtitle = subtitle };
        }

        // WireIconLookup died with the per-action glyphs (v3.2, 2026-08-01): the
        // status circle is binary now, so the keys→sprite table it filled has no
        // reader. The action glyph sprites themselves are still generated by
        // DPPSpriteFactory — unused here, kept for a future non-status use.

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
            // Initial view (user, 2026-07-20, corrected same day): glTF default
            // showed the model's BACK — start at 180° yaw so the front faces
            // the user. Twist gesture resets back to this on every screen
            // entry. Tweak ModelAnchor's Y in the Inspector if it drifts.
            anchor.localRotation = Quaternion.Euler(0f, 180f, 0f);

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

            // Spawn fix (v4.6): zone appears at the main panel's right edge.
            SetRef(interaction, "mainPanel", mainCanvasRT);

            // Mechanism #4 (v4.6): constrained part drag — two selection
            // methods (direct + "+" list). Component on the zone root; the
            // column builder wires its list UI refs.
            var parts = go.AddComponent<ZonePartInteraction>();
            SetRef(parts, "zone", interaction);
            SetRef(parts, "twist", twist);

            // Gesture HUD column + help modal (v4.3, approved mock
            // zone_status_bar_v3.svg 2026-07-20).
            BuildGestureColumn(rt, twist, interaction, parts);

            // Preview-layer split unchanged: original films for the RT previews,
            // the user sees only the clone.
            ConfigurePreviewLayer(vcuAnimator);

            Undo.RegisterCreatedObjectUndo(go, "Build Exploded Canvas");
            return go;
        }

        // =================================================================
        // Gesture HUD (v4.3): vertical column — [?] help, L/R hand lights,
        // YAW / DIST / ZOOM rows — pinned to the model's front-left by
        // ExplodedZoneInteraction; plus the centered gesture-guide modal.
        // ZoneGestureHUD binds values and wires both buttons at runtime.
        // =================================================================
        private static void BuildGestureColumn(RectTransform zoneRT, TwoHandTwistRotate twist, ExplodedZoneInteraction interaction, ZonePartInteraction parts)
        {
            // OWN nested world-space canvas (v4.5.3): the column orbits OFF the
            // zone plane, so as plain children its clicks were parallax-
            // displaced (same root cause as the modal ×). As its own canvas
            // the bridge raycasts the column's billboarded plane — accurate
            // wherever it orbits.
            var colGO = new GameObject("GestureColumn", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster));
            var col = (RectTransform)colGO.transform;
            col.SetParent(zoneRT, false);
            colGO.GetComponent<Canvas>().worldCamera = Camera.main;
            col.anchorMin = col.anchorMax = new Vector2(0.5f, 0.5f);
            col.pivot = new Vector2(0.5f, 0.5f);
            col.anchoredPosition3D = new Vector3(-150f, 0f, -70f);   // placeholder — follower repositions
            col.sizeDelta = new Vector2(52f, 356f);                  // v4.6.2: ? + pill + "+" + regroup

            var hud = colGO.AddComponent<ZoneGestureHUD>();

            // ---- backing plate (user, 2026-07-20) ----
            // Sits behind the ? and the pill: visually grounds the column and
            // gives the reticle a big raycast surface to land on, so aiming at
            // the ? starts from a stable snap instead of free space.
            // v4.5.3 (UI standards, user): backplate = brand blue #2e5aa0
            // (tab/active-stroke), pill/buttons = navy/panel #0a1f44. Earlier
            // passes: #d3d9e0 camouflaged the white reticle; #333d4d was
            // off-brand grey.
            var plateColor = DPPTheme.Hex("#2e5aa0");
            plateColor.a = 160f / 255f;   // opacity tuned on device by Thiago 2026-07-20
            var backplate = AddImage(CenterIn("Backplate", col, 60, 364), DPPSpriteFactory.RoundedR13, plateColor, sliced: true, raycast: true);
            backplate.transform.SetAsFirstSibling();

            // ---- ? help button (top of the column) ----
            // Grab-handle trick (user feedback 2026-07-20: 26px was un-hittable
            // with the hand ray): 52×52 INVISIBLE hit area, small 30px visual.
            var helpRT = TL("HelpButton", col, 0, 0, 52, 52);
            var helpHit = AddImage(Stretch("HitArea", helpRT), DPPSpriteFactory.Circle64, new Color(0f, 0f, 0f, 0f), raycast: true);
            AddImage(CenterIn("Fill", helpRT, 30, 30), DPPSpriteFactory.Circle64, DPPTheme.Hex("#0a1f44"));
            AddText(Stretch("Glyph", helpRT), "?", 16, DPPTheme.Hex("#8fa3c0"), bold: true, align: TextAlignmentOptions.Center);
            var helpBtn = helpRT.gameObject.AddComponent<Button>();
            helpBtn.transition = Selectable.Transition.None;
            helpBtn.targetGraphic = helpHit;
            // Global hover rule: white outline ring while the ray is over the
            // hit area — makes the enlarged target visible while aiming.
            var helpOutline = AddImage(CenterIn("HoverOutline", helpRT, 36, 36), DPPSpriteFactory.Circle64, Color.white);
            helpOutline.transform.SetSiblingIndex(1);   // behind Fill + Glyph
            helpOutline.gameObject.SetActive(false);
            var helpHover = helpRT.gameObject.AddComponent<HoverHighlight>();
            SetRef(helpHover, "highlightOutline", helpOutline.gameObject);

            // ---- vertical pill ----
            var pill = TL("Pill", col, 6, 58, 40, 178);
            AddImage(pill, DPPSpriteFactory.RoundedR20, DPPTheme.Hex("#0a1f44"), sliced: true);

            // ---- hand chips ----
            var leftChip = MakeHandChip(pill, "ChipL", "L", 12, 10, out GameObject lOn, out GameObject lOff);
            var rightChip = MakeHandChip(pill, "ChipR", "R", 12, 30, out GameObject rOn, out GameObject rOff);
            AddImage(TL("Div1", pill, 10, 52, 20, 2), DPPSpriteFactory.Grip, DPPTheme.Hex("#1a2740"));

            // ---- value rows: caption over value, both centered ----
            var yawCap = AddText(TL("YawCap", pill, 0, 58, 40, 10), "YAW", 7.5f, DPPTheme.Hex("#5d7396"), bold: true, align: TextAlignmentOptions.Center);
            var yawVal = AddText(TL("YawVal", pill, 0, 68, 40, 14), "0°", 11, DPPTheme.Hex("#dbe4f0"), bold: true, align: TextAlignmentOptions.Center);
            AddImage(TL("Div2", pill, 10, 88, 20, 2), DPPSpriteFactory.Grip, DPPTheme.Hex("#1a2740"));

            var distCap = AddText(TL("DistCap", pill, 0, 94, 40, 10), "DIST", 7.5f, DPPTheme.Hex("#5d7396"), bold: true, align: TextAlignmentOptions.Center);
            var distVal = AddText(TL("DistVal", pill, 0, 104, 40, 14), "—", 11, DPPTheme.Hex("#dbe4f0"), bold: true, align: TextAlignmentOptions.Center);
            AddImage(TL("Div3", pill, 10, 124, 20, 2), DPPSpriteFactory.Grip, DPPTheme.Hex("#1a2740"));

            var zoomCap = AddText(TL("ZoomCap", pill, 0, 130, 40, 10), "ZOOM", 7.5f, DPPTheme.Hex("#5d7396"), bold: true, align: TextAlignmentOptions.Center);
            var zoomVal = AddText(TL("ZoomVal", pill, 0, 140, 40, 14), "1.00×", 11, DPPTheme.Hex("#dbe4f0"), bold: true, align: TextAlignmentOptions.Center);

            // ---- "+" part-list button (mechanism #4) ----
            // Same accessibility pattern as "?": 52px invisible hit, 30px visual.
            var plusRT = TL("PlusButton", col, 0, 244, 52, 52);
            var plusHit = AddImage(Stretch("HitArea", plusRT), DPPSpriteFactory.Circle64, new Color(0f, 0f, 0f, 0f), raycast: true);
            AddImage(CenterIn("Fill", plusRT, 30, 30), DPPSpriteFactory.Circle64, DPPTheme.Hex("#0a1f44"));
            var plusGlyph = CenterIn("Glyph", plusRT, 18, 18);
            AddImage(CenterIn("H", plusGlyph, 16, 2.6f), DPPSpriteFactory.Grip, Color.white);
            var plusV = CenterIn("V", plusGlyph, 16, 2.6f);
            plusV.localRotation = Quaternion.Euler(0, 0, 90);
            AddImage(plusV, DPPSpriteFactory.Grip, Color.white);
            var plusBtn = plusRT.gameObject.AddComponent<Button>();
            plusBtn.transition = Selectable.Transition.None;
            plusBtn.targetGraphic = plusHit;
            var plusOutline = AddImage(CenterIn("HoverOutline", plusRT, 36, 36), DPPSpriteFactory.Circle64, Color.white);
            plusOutline.transform.SetSiblingIndex(1);
            plusOutline.gameObject.SetActive(false);
            var plusHover = plusRT.gameObject.AddComponent<HoverHighlight>();
            SetRef(plusHover, "highlightOutline", plusOutline.gameObject);

            // ---- part list (v4.6.1): fans to the user's RIGHT of the "+",
            // masked 3-row window, pinch-drag anywhere inside to scroll. ----
            var listRoot = TL("PartList", col, 26, 270, 0, 0);

            // Viewport: RectMask2D window showing exactly 3 rows, vertically
            // centered on the "+", extending right (away from the model).
            var viewGO = new GameObject("Viewport", typeof(RectTransform), typeof(UnityEngine.UI.RectMask2D));
            var viewport = (RectTransform)viewGO.transform;
            viewport.SetParent(listRoot, false);
            viewport.anchorMin = viewport.anchorMax = new Vector2(0.5f, 0.5f);
            viewport.pivot = new Vector2(0f, 0.5f);
            viewport.anchoredPosition = new Vector2(34f, 0f);
            viewport.sizeDelta = new Vector2(190f, 110f);   // 3 × 36 + margin

            var contentGO = new GameObject("Content", typeof(RectTransform));
            var content = (RectTransform)contentGO.transform;
            content.SetParent(viewport, false);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0f, 110f);      // height set at runtime (rows × step)

            var rowGO = new GameObject("RowTemplate", typeof(RectTransform));
            var rowRT = (RectTransform)rowGO.transform;
            rowRT.SetParent(content, false);
            rowRT.anchorMin = rowRT.anchorMax = new Vector2(0.5f, 1f);
            rowRT.pivot = new Vector2(0.5f, 1f);
            rowRT.sizeDelta = new Vector2(170f, 30f);
            rowRT.anchoredPosition = Vector2.zero;          // runtime: (0, -i*rowStep)
            var rowOutline = AddImage(CenterIn("Outline", rowRT, 176, 36), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            rowOutline.gameObject.SetActive(false);
            AddImage(CenterIn("BG", rowRT, 170, 30), DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#0a1f44"), sliced: true, raycast: true);
            AddText(Stretch("Label", rowRT), "Part", 11.5f, Color.white, bold: true, align: TextAlignmentOptions.Center);
            rowGO.SetActive(false);

            // ---- regroup button (v4.6.2): recycle icon → reassemble all ----
            var regroupRT = TL("RegroupButton", col, 0, 300, 52, 52);
            var regroupHit = AddImage(Stretch("HitArea", regroupRT), DPPSpriteFactory.Circle64, new Color(0f, 0f, 0f, 0f), raycast: true);
            AddImage(CenterIn("Fill", regroupRT, 30, 30), DPPSpriteFactory.Circle64, DPPTheme.Hex("#0a1f44"));
            AddImage(CenterIn("Icon", regroupRT, 16, 16), DPPSpriteFactory.Recycle, Color.white);
            var regroupBtn = regroupRT.gameObject.AddComponent<Button>();
            regroupBtn.transition = Selectable.Transition.None;
            regroupBtn.targetGraphic = regroupHit;
            var regroupOutline = AddImage(CenterIn("HoverOutline", regroupRT, 36, 36), DPPSpriteFactory.Circle64, Color.white);
            regroupOutline.transform.SetSiblingIndex(1);
            regroupOutline.gameObject.SetActive(false);
            var regroupHover = regroupRT.gameObject.AddComponent<HoverHighlight>();
            SetRef(regroupHover, "highlightOutline", regroupOutline.gameObject);

            SetRef(parts, "plusButton", plusBtn);
            SetRef(parts, "plusGlyph", plusGlyph);
            SetRef(parts, "regroupButton", regroupBtn);
            SetRef(parts, "listRoot", listRoot);
            SetRef(parts, "viewport", viewport);
            SetRef(parts, "content", content);
            SetRef(parts, "rowTemplate", rowRT);

            // ---- gesture-guide modal (centered, hidden until ? is clicked) ----
            var modal = BuildZoneHelpModal(zoneRT, out Button closeBtn);

            // ---- wiring ----
            SetRef(hud, "twist", twist);
            SetRef(hud, "zone", interaction);
            SetRef(hud, "leftOn", lOn); SetRef(hud, "leftOff", lOff);
            SetRef(hud, "rightOn", rOn); SetRef(hud, "rightOff", rOff);
            SetRef(hud, "yawCap", yawCap); SetRef(hud, "yawValue", yawVal);
            SetRef(hud, "distCap", distCap); SetRef(hud, "distValue", distVal);
            SetRef(hud, "zoomCap", zoomCap); SetRef(hud, "zoomValue", zoomVal);
            SetRef(hud, "helpButton", helpBtn);
            SetRef(hud, "helpModal", modal);
            SetRef(hud, "modalCloseButton", closeBtn);
            SetRef(interaction, "statusColumn", col);
        }

        /// <summary>Hand light: 16×16 chip with an "On" stack (solid green +
        /// dark letter) and an "Off" stack (dim ring + gray letter). The HUD
        /// toggles the stacks; only one is visible at a time.</summary>
        private static RectTransform MakeHandChip(RectTransform parent, string name, string letter,
            float x, float y, out GameObject on, out GameObject off)
        {
            var chip = TL(name, parent, x, y, 16, 16);

            var onRT = TL("On", chip, 0, 0, 16, 16);
            AddImage(CenterIn("Fill", onRT, 16, 16), DPPSpriteFactory.Circle64, DPPTheme.Hex("#27c46c"));
            AddText(Stretch("Letter", onRT), letter, 9, DPPTheme.Hex("#06240f"), bold: true, align: TextAlignmentOptions.Center);
            on = onRT.gameObject;

            var offRT = TL("Off", chip, 0, 0, 16, 16);
            AddImage(CenterIn("Ring", offRT, 16, 16), DPPSpriteFactory.Circle64, DPPTheme.Hex("#5d7396"));
            AddImage(CenterIn("Inner", offRT, 13, 13), DPPSpriteFactory.Circle64, DPPTheme.Hex("#0a1f44"));   // matches pill bg → reads as a hollow ring
            AddText(Stretch("Letter", offRT), letter, 9, DPPTheme.Hex("#5d7396"), bold: true, align: TextAlignmentOptions.Center);
            off = offRT.gameObject;
            off.SetActive(false);

            return chip;
        }

        /// <summary>The "How to control the model" modal: solid navy panel
        /// centered in the zone; everything around it stays transparent.
        /// Starts inactive; ZoneGestureHUD opens/closes it and pauses the
        /// twist gesture while it is visible.</summary>
        private static GameObject BuildZoneHelpModal(RectTransform zoneRT, out Button closeBtn)
        {
            // OWN nested world-space canvas (v4.5.2): the modal is draggable
            // via a standard grabber bar, and PicoHandUIBridge raycasts per
            // CANVAS plane — as its own canvas, the modal's clicks stay
            // pixel-accurate wherever the user drags/billboards it. (As plain
            // zone-canvas children they'd go parallax-displaced the moment it
            // left the zone plane — the original un-clickable-× bug.)
            var modalGO = new GameObject("HelpModal", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster));
            var modal = (RectTransform)modalGO.transform;
            modal.SetParent(zoneRT, false);
            var modalCanvas = modalGO.GetComponent<Canvas>();
            modalCanvas.worldCamera = Camera.main;
            modal.anchorMin = modal.anchorMax = new Vector2(0.5f, 0.5f);
            modal.pivot = new Vector2(0.5f, 0.5f);
            // Starts ON the zone plane, centered; once the user drags it, its
            // position persists until the screen is rebuilt.
            modal.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            modal.sizeDelta = new Vector2(300f, 250f);

            AddImage(Stretch("BG", modal), DPPSpriteFactory.RoundedR22, DPPTheme.Hex("#0d1526"), sliced: true, raycast: true);
            AddText(TL("Title", modal, 18, 14, 230, 20), "How to control the model", 14, DPPTheme.TextOnNavy, bold: true);

            // close ×
            var closeRT = TL("Close", modal, 260, 10, 28, 28);
            var closeFill = AddImage(CenterIn("Fill", closeRT, 24, 24), DPPSpriteFactory.Circle64, DPPTheme.Hex("#1a2740"), raycast: true);
            var xa = CenterIn("Xa", closeRT, 12, 2.2f); xa.localRotation = Quaternion.Euler(0, 0, 45);
            AddImage(xa, DPPSpriteFactory.Grip, DPPTheme.Hex("#8fa3c0"));
            var xb = CenterIn("Xb", closeRT, 12, 2.2f); xb.localRotation = Quaternion.Euler(0, 0, -45);
            AddImage(xb, DPPSpriteFactory.Grip, DPPTheme.Hex("#8fa3c0"));
            closeBtn = closeRT.gameObject.AddComponent<Button>();
            closeBtn.transition = Selectable.Transition.None;
            closeBtn.targetGraphic = closeFill;
            // Global hover rule (spec 00 §4): white contour while the ray is
            // over the button.
            var closeOutline = AddImage(CenterIn("HoverOutline", closeRT, 28, 28), DPPSpriteFactory.Circle64, Color.white);
            closeOutline.transform.SetAsFirstSibling();   // ring behind the fill
            closeOutline.gameObject.SetActive(false);
            var closeHover = closeRT.gameObject.AddComponent<HoverHighlight>();
            SetRef(closeHover, "highlightOutline", closeOutline.gameObject);

            // rows: key + description
            MakeModalRow(modal, 48, "Move the panel", "Pinch the circle below the model and drag.");
            MakeModalRow(modal, 92, "Rotate", "Pinch BOTH hands 5–25 cm apart and twist them like a steering wheel — the model spins.");
            MakeModalRow(modal, 146, "Zoom", "Spread wider than 25 cm: the distance sets the size. 25 cm = normal · 55 cm = maximum.");
            MakeModalRow(modal, 200, "Hand lights", "Green = hand pinching · ring = hand open.");

            // Standard grabber bar (00 §5) docked below the modal — the user
            // can carry the guide anywhere in AR space, like any panel. No
            // startup recenter: it appears centered on the zone.
            BuildGrabberBar(modal);
            var modalHandle = modal.GetComponentInChildren<PanelGrabHandle>(true);
            if (modalHandle != null) SetBool(modalHandle, "recenterOnStart", false);

            modalGO.SetActive(false);
            return modalGO;
        }

        private static void MakeModalRow(RectTransform modal, float y, string key, string desc)
        {
            AddText(TL(key.Replace(" ", "") + "Key", modal, 18, y, 264, 16), key, 12, DPPTheme.TextOnNavy, bold: true);
            var d = AddText(TL(key.Replace(" ", "") + "Desc", modal, 18, y + 16, 264, 30), desc, 10.5f, DPPTheme.Hex("#aebdd6"), bold: false);
            d.textWrappingMode = TextWrappingModes.Normal;
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
