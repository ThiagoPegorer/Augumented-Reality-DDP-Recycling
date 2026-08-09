using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_1_1/10 — THE SUPER PANEL RIG (spec `04_DPP_page.md` v2).
    ///
    /// Builds `DppSuperPanel`: one rig carrying three toed-in world-space
    /// canvases — rail 220, stage 340, data 420 — plus the model and one grab bar.
    ///
    /// ⚠ IT DOES NOT TOUCH DPPPanelCanvas. Welcome, QR, stakeholder, disassembly,
    /// steps and summary all still live there as flat 640 × 430 children. Turning
    /// that canvas into the super panel would have put six working screens in the
    /// blast radius to rebuild one. The two are alternatives: ScreenRouter shows
    /// the rig and hides the panel's grabber, or the reverse.
    ///
    /// SAFE TO RE-RUN: destroys and rebuilds only `DppSuperPanel` and
    /// `DppFreeModel`.
    ///
    /// GEOMETRY IS COMPUTED, NOT TYPED. The spec quotes −26.9° and +20.5°; those
    /// are atan(offset / distance) for 0.75 m, and open item 1 says to verify the
    /// distance on device. Hardcoding the angles would silently decouple them from
    /// the distance the moment it changes, so both come from <see cref="SpDistance"/>.
    ///
    /// SEAMS: the two side canvases pivot on their INNER edge, not their centre.
    /// Yawing about the centre leaves a ~12 mm lateral gap and a 50 mm depth step
    /// at each seam; pivoting on the shared edge makes the three surfaces a
    /// continuous shallow arc. The resulting centre direction differs from the
    /// nominal angle by under 1°.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // ---- rig geometry (spec §2) ----
        // Stage 340 → 400 (device test 2026-08-06): a model wide enough to be worth
        // looking at sweeps past a 340 stage and clips the rail and data canvases
        // even while LOCKED. Every other number here is computed from these three,
        // so widening the stage moves the seams and re-derives both yaw angles.
        private const float SpRailW = 220f, SpStageW = 400f, SpDataW = 420f, SpH = 430f;
        private const float SpTotalW = SpRailW + SpStageW + SpDataW;   // 980
        private const float SpScale = 0.001f;
        private const float SpDistance = 0.75f;      // ⚠ open item 1 — verify on device
        private const float SpEyeHeight = 1.1176f;   // matches the XR rig's Camera Y Offset

        // Panel-local centres, measured from the assembly centre (490).
        private const float SpRailCx  = (SpRailW * 0.5f) - (SpTotalW * 0.5f);                      // -380
        private const float SpStageCx = SpRailW + (SpStageW * 0.5f) - (SpTotalW * 0.5f);           // -100
        private const float SpDataCx  = SpRailW + SpStageW + (SpDataW * 0.5f) - (SpTotalW * 0.5f); // +280

        // 04e v2 (2026-08-08): Training disassembly is GONE from the rail.
        // 04e round 2 (same day): Certificates & safety is a FULL FOURTH TAB —
        // same size, same walkthrough rule, red identity painted by the view.
        private static readonly string[] SpTabL1 = { "Product", "Usage &", "Environmental", "Certificates &" };
        private static readonly string[] SpTabL2 = { "specifications", "service", "impact", "safety" };
        private static readonly string[] SpTabIcons =
            { "ic_product_specs", "ic_usage_history", "ic_environmental", "ic_certificates" };

        /// <summary>04e round 2 rail order: FOUR equal tabs from the top (band
        /// 34–342), then the gated CTA at 356–396 — top and bottom margins both 34.</summary>
        private const float SpTabTop = 34f;
        private const float SpCtaY = 356f;

        [MenuItem("RBv2_1_1/10 — Super panel rig", false, 10)]
        public static void Build_SuperPanel()
        {
            AssetDatabase.Refresh();
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            RemoveByName("DppSuperPanel");
            RemoveByName("DppFreeModel");

            // Name-trap defusal (2026-08-09): RBv2_1_1/08's legacy certificates screen
            // on DPPPanelCanvas shared the rig page's "CertificatesPage" name, and a
            // global find once put the flat-canvas screen into tabPages[3]. /8 now
            // builds it as "CertificatesScreen_RB2_1_legacy"; this renames any
            // pre-rename scene object so the old name can only mean the rig's page.
            var legacyCanvas = SpFind("DPPPanelCanvas");
            if (legacyCanvas != null)
            {
                var legacyCert = legacyCanvas.transform.Find("CertificatesPage");
                if (legacyCert != null)
                {
                    legacyCert.gameObject.name = "CertificatesScreen_RB2_1_legacy";
                    Debug.Log("[DPPUIBuilder] Legacy certificates screen renamed " +
                              "'CertificatesScreen_RB2_1_legacy' (name-trap defusal).");
                }
            }

            var rigGO = new GameObject("DppSuperPanel");
            Undo.RegisterCreatedObjectUndo(rigGO, "Build super panel");
            var rig = rigGO.transform;
            rig.position = new Vector3(0f, SpEyeHeight, SpDistance);

            var view = rigGO.AddComponent<SuperPanelView>();

            // The freed model's root is a SIBLING of the rig, not a child: once
            // unlocked the model must stop following the rig, or dragging the
            // panels would drag the "freed" model with them (spec §2.3).
            var freeRootGO = new GameObject("DppFreeModel");
            Undo.RegisterCreatedObjectUndo(freeRootGO, "Build free model root");
            freeRootGO.transform.position = rig.position;

            // ---------------- the three canvases ----------------
            // Stage first: its edges are where the side canvases hinge.
            float stageLeft  = (SpStageCx - SpStageW * 0.5f) * SpScale;   // -0.270
            float stageRight = (SpStageCx + SpStageW * 0.5f) * SpScale;   // +0.070

            var stage = SpCanvas(rig, "StageCanvas", SpStageW, SpH,
                pivot: new Vector2(0.5f, 0.5f),
                localX: SpStageCx * SpScale,
                yawDeg: 0f);                                              // spec §2.1 — deliberately flat

            var rail = SpCanvas(rig, "RailCanvas", SpRailW, SpH,
                pivot: new Vector2(1f, 0.5f),                             // hinge on the inner (right) edge
                localX: stageLeft,
                yawDeg: SpYaw(SpRailCx * SpScale));

            var data = SpCanvas(rig, "DataCanvas", SpDataW, SpH,
                pivot: new Vector2(0f, 0.5f),                             // hinge on the inner (left) edge
                localX: stageRight,
                yawDeg: SpYaw(SpDataCx * SpScale));

            // ---------------- rail ----------------
            AddImage(Stretch("RailBG", rail), DPPSpriteFactory.RoundedR22, DPPTheme.Hex("#081733"), sliced: true);
            AddImage(TL("Divider", rail, 219f, 18f, 1f, 394f), null, DPPTheme.Hex("#1a335f"));

            // 04e round 2 (Thiago, 2026-08-08): certificates is tab 3 of the FOUR
            // standard tabs — same size, sequential after Environmental impact.
            // The view paints its red identity; SpCertEntry is retired.
            var tabRoots = new RectTransform[SuperPanelView.TabCount];
            var tabFills = new Image[SuperPanelView.TabCount];
            var tabStrokes = new Image[SuperPanelView.TabCount];
            var tabAccents = new Image[SuperPanelView.TabCount];
            var tabTicks = new GameObject[SuperPanelView.TabCount];
            var tabL1 = new TMP_Text[SuperPanelView.TabCount];
            var tabL2 = new TMP_Text[SuperPanelView.TabCount];
            var tabIcons = new Image[SuperPanelView.TabCount];
            var tabButtons = new Button[SuperPanelView.TabCount];

            for (int i = 0; i < SuperPanelView.TabCount; i++)
            {
                tabButtons[i] = SpRailTab(rail, i, out tabRoots[i], out tabFills[i], out tabStrokes[i],
                    out tabAccents[i], out tabTicks[i], out tabL1[i], out tabL2[i], out tabIcons[i]);
                WireClick(tabButtons[i], view, "SelectTab" + i);
            }

            var router = SpFindRouter();
            // (Round 4: the LINKED/FREE control lives in the stage's gesture
            // column — see SpGestureHud. The round-3 rail tile lasted one build.)

            // ---- rail-bottom CTA (04e v2, approved 2026-08-08) ----
            // RECYCLER: "Continue to disassembly" — grey until all three tabs AND
            // the certificates page have been opened, then green; the ONLY route
            // into the teardown now that the Training tab is gone. PRODUCT USER:
            // the same slot reads "Back" → stakeholder fork. The VIEW paints all
            // states (PaintRailCta); the builder ships the grey rest pose.
            // Built with PsSmallPill — the raw Pill sprite is NOT a capsule (00
            // §4.2, and the corner bug Thiago photographed 2026-08-09); PsSmallPill
            // capsules every layer and keeps the 50-unit hit root. The view
            // repaints the fill/label per state via targetGraphic.
            var ctaBtn = PsSmallPill(rail, "RailCta", SpRailW * 0.5f, 184f,
                "Continue to disassembly", primary: false, out var ctaLabel,
                cy: SpCtaY + 20f, visualH: 40f, fontSize: 11.5f);
            WireClick(ctaBtn, view, nameof(SuperPanelView.OnRailCta));
            var ctaFill = ctaBtn.targetGraphic as Image;

            // ---------------- stage ----------------
            // NO background image AND NO frame (device round 8, Thiago: "the
            // background is perfect" only once the translucent green StageFrame
            // was gone). The stage is empty space between two canvases — nothing
            // can bleed through it and there is no raycast target to suppress
            // (spec §2.4). The GhostOutline died with the frame in the same round;
            // the view's ghostOutline field stays null-safe and UNWIRED.

            // The model's home: an empty transform at the stage centre, on the
            // stage's own plane. The model parents here when locked.
            var homeGO = new GameObject("ModelHome");
            var home = homeGO.transform;
            home.SetParent(stage, false);
            // y +20 → −10 (device round 3, Thiago 2026-08-08): the exploded pose
            // should sit toward the BOTTOM of the stage — together with the link's
            // open-pose re-centre this stops the lid riding the frame's top edge.
            home.localPosition = new Vector3(0f, -10f, -40f);  // canvas units; slightly toward the user
            // The POSE lives on the HOME, not the clone or the pivot — ReLock snaps
            // the pivot's localRotation back to identity, so anything baked below
            // the home would be silently undone on every re-lock.
            //
            //   · 180° yaw (round 2): the raw glTF faces the connector side AWAY
            //     from the user.
            //   · +25° pitch, +25° extra yaw (round 3): Thiago asked for a 3D
            //     ISOMETRIC presentation instead of the flat front elevation —
            //     seen slightly from above and off-axis, so depth reads.
            //     TUNE IN PLAY MODE by rotating ModelHome, then tell the builder
            //     the numbers. The teardown is unaffected: its part offsets are
            //     local to the clone and turn with it.
            home.localRotation = Quaternion.Euler(25f, 205f, 0f);
            home.localScale = Vector3.one;

            var model = SpBuildStageModel(home);

            // ---- Stage gestures (action-zone parity, mock 04_stage_gestures_v1,
            // decisions 1–3 agreed 2026-08-08) ----
            // The zone's own TwoHandTwistRotate, re-targeted at the pivot. It lives
            // on the RIG root so its child-search finds the RigGrabber's handle
            // ("panel drag wins", spec 10 §6.4); the freed model's bar is a SIBLING
            // root, wired below via extraBlockingHandles. maxZoom 1.5, not the
            // zone's 2 — at 2× the 173 mm open model reaches both side canvases.
            var twist = rigGO.AddComponent<TwoHandTwistRotate>();
            if (model != null) SetRef(twist, "target", model);
            // Round 6 (Thiago): FREE may zoom to 2×. Round 5: gestures are DISABLED
            // while LINKED — the view enables them at the end of the free sequence
            // and disables them on re-lock; shipping the component off means the
            // showcase can never be twisted. resetOnEnable off: the view owns pose
            // resets (a reset here snapped the idle spin's yaw on every re-enable).
            SetFloat(twist, "maxZoom", 2f);
            SetBool(twist, "m_Enabled", false);
            SetBool(twist, "resetOnEnable", false);
            var hudFollower = SpGestureHud(stage, twist, view, model,
                out var lockGlyph, out var lockLabel, out var hudBackplate, out var hudExtras);

            // One grabber under the stage moves the WHOLE rig (spec §2.3).
            SpGrabber(stage, rig, "RigGrabber", DPPTheme.GrabberGrip, spawnDistance: SpDistance);

            // The freed model's own bar lives on ITS OWN canvas under the free
            // root, not on the stage — a bar nailed to the stage while the model
            // floats across the room is not "the model's bar", it is a second rig
            // bar in disguise. Tinted teal so which-moves-what needs no experiment.
            var freeCanvas = new GameObject("FreeModelCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            var freeRT = (RectTransform)freeCanvas.transform;
            var freeCv = freeCanvas.GetComponent<Canvas>();
            freeCv.renderMode = RenderMode.WorldSpace;
            freeCv.worldCamera = Camera.main;
            freeRT.SetParent(freeRootGO.transform, false);
            freeRT.sizeDelta = new Vector2(240f, 40f);
            freeRT.localScale = Vector3.one * SpScale;
            freeRT.localPosition = new Vector3(0f, -0.170f, 0f);   // under the freed model
            var freeBar = SpGrabber(freeRT, freeRootGO.transform, "FreeModelGrabber",
                DPPTheme.TealLight, spawnDistance: SpDistance, recenter: false);
            freeBar.anchoredPosition = new Vector2(120f, -9f);      // centred in its own 240 × 40 canvas
            // Rounds 7–8: the drag bar FOLLOWS the freed model at its front-bottom
            // (capped drop — strictly-below vanished from view at 2× zoom) instead
            // of hanging at a fixed offset under the free root. alwaysFollow: the
            // canvas only exists while FREE, so no SetFree handshake is needed.
            var freeBarFollower = freeCanvas.AddComponent<StageGestureHudFollower>();
            if (model != null) SetRef(freeBarFollower, "model", model);
            SetBool(freeBarFollower, "followBelow", true);
            SetBool(freeBarFollower, "alwaysFollow", true);
            // Dragging the FREED model must also silence the twist/zoom — its
            // handle is on a sibling root the gesture's child-search cannot see.
            SetRefArray(twist, "extraBlockingHandles",
                new Object[] { freeBar.GetComponent<PanelGrabHandle>() });

            // ---------------- data canvas ----------------
            AddImage(Stretch("DataBG", data), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var placeholder = Stretch("PlaceholderPage", data);
            var placeholderLbl = AddText(CenterIn("Label", placeholder, 340f, 120f),
                "not built yet", 14f, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.Center);
            placeholderLbl.textWrappingMode = TextWrappingModes.Normal;

            // (04e v2: the interim TrainingPage — built here since 2026-08-07 as the
            // teardown's route in — is GONE with its tab. The route is now the
            // rail-bottom CTA above, and /1's full rebuild of the rig means no
            // orphaned TrainingPage can survive a re-run: trap 4 satisfied.)

            // Tab pages are filled in by their own phases. Product specs is
            // RBv2_1_1/11, which re-parents the page RBv2_1_1/09 already built.
            var certPage = SpCertPage(data, view);

            var tabPages = new GameObject[SuperPanelView.TabCount];
            var existing = SpFind("ProductSpecsPage");
            if (existing != null) tabPages[0] = existing;
            tabPages[3] = certPage.gameObject;   // 04e round 2: certificates IS tab 3

            // ---------------- wiring ----------------
            SetRef(view, "router", router);
            SetRef(view, "welcome", Object.FindFirstObjectByType<WelcomeController>(FindObjectsInactive.Include));
            SetRef(view, "scanner", Object.FindFirstObjectByType<QRScanController>(FindObjectsInactive.Include));
            SetRefArray(view, "tabRoots", tabRoots);
            SetRefArray(view, "tabFills", tabFills);
            SetRefArray(view, "tabStrokes", tabStrokes);
            SetRefArray(view, "tabAccents", tabAccents);
            SetRefArray(view, "tabTicks", tabTicks);
            SetRefArray(view, "tabLine1", tabL1);
            SetRefArray(view, "tabLine2", tabL2);
            SetRefArray(view, "tabIcons", tabIcons);
            SetRefArray(view, "tabButtons", tabButtons);
            SetRefArray(view, "tabPages", tabPages);
            SetRef(view, "railCtaButton", ctaBtn);
            SetRef(view, "railCtaFill", ctaFill);
            SetRef(view, "railCtaLabel", ctaLabel);

            SetRef(view, "placeholderPage", placeholder.gameObject);
            SetRef(view, "placeholderLabel", placeholderLbl);
            var stageLink = home != null ? home.GetComponentInChildren<ModelLinkController>(true) : null;
            if (stageLink != null)
            {
                SetRef(view, "modelLink", stageLink);
                SetRef(stageLink, "owner", view);
                // Two pinching hands are a gesture, not a selection (§6.4 family).
                SetRef(stageLink, "gestures", twist);
            }
            else Debug.LogWarning("[DPPUIBuilder] No ModelLinkController under the stage — the model and the " +
                                  "data canvas will not talk to each other. Is VCU_assembly in the scene?");

            SetRef(view, "stageModelHome", home);
            SetRef(view, "freeModelRoot", freeRootGO.transform);
            SetRef(view, "model", model);
            // ghostOutline deliberately unwired — removed with the StageFrame, round 8.
            SetRef(view, "freeModelGrabber", freeCanvas);
            // Round 6: Thiago's own padlock artwork carries the state — orange
            // CLOSED lock for LINKED, green OPEN lock for FREE; the view never
            // tints the glyph (the sprites are pre-coloured, white keyhole).
            SetRef(view, "lockedSprite", LoadPageIcon("ic_lock_linked"));
            SetRef(view, "unlockedSprite", LoadPageIcon("ic_lock_free"));
            SetRef(view, "lockLabel", lockLabel);
            SetRef(view, "lockGlyph", lockGlyph);
            SetRef(view, "stageGestures", twist);
            SetRef(view, "hudFollower", hudFollower);
            // Round 5: the column is COLLAPSED while LINKED (lock + state word only)
            // and the view grows the backplate / shows the extras in the free sequence.
            SetRef(view, "hudBackplate", hudBackplate);
            SetRef(view, "hudExtras", hudExtras);

            if (router != null)
            {
                SetRef(router, "dppSuperPanel", rigGO);
                SetRef(router, "freeModelRoot", freeRootGO);
                var grab = SpFind("GrabberBar");
                if (grab != null) SetRef(router, "panelGrabber", grab);
                else Debug.LogWarning("[DPPUIBuilder] No GrabberBar found — both grab bars will be live at once.");
            }

            freeCanvas.SetActive(false);   // shown only while the model is actually free
            rigGO.SetActive(false);
            freeRootGO.SetActive(false);

            Selection.activeGameObject = rigGO;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log($"[DPPUIBuilder] RBv2_1_1/10 — super panel built at {SpDistance} m. " +
                      $"Yaw: rail {SpYaw(SpRailCx * SpScale):0.0}°, stage 0°, data {SpYaw(SpDataCx * SpScale):0.0}°. " +
                      "Now run RBv2_1_1/11, then RBv2_1_1/Tools/Verify wiring, then SAVE THE SCENE.");
        }

        // =================================================================
        // RBv2_1_1/11 — move the Product specs page into the data canvas
        // =================================================================

        /// <summary>
        /// RBv2_1_1/09 authored Product specs as a 420 × 430 root precisely so this
        /// step is a re-parent and not a rebuild. Nothing inside the page moves:
        /// the data canvas is exactly the size the page was drawn at.
        /// </summary>
        [MenuItem("RBv2_1_1/11 — Product specs into the data canvas", false, 11)]
        public static void Build_ProductSpecsIntoRig()
        {
            var rig = SpFind("DppSuperPanel");
            if (rig == null) { Debug.LogError("[DPPUIBuilder] No DppSuperPanel — run RBv2_1_1/10 first."); return; }
            var data = rig.transform.Find("DataCanvas") as RectTransform;
            if (data == null) { Debug.LogError("[DPPUIBuilder] DppSuperPanel has no DataCanvas — re-run RBv2_1_1/10."); return; }

            var page = SpFind("ProductSpecsPage");
            if (page == null) { Debug.LogError("[DPPUIBuilder] No ProductSpecsPage — run RBv2_1_1/09 first."); return; }

            Undo.SetTransformParent(page.transform, data, "Move Product specs into the data canvas");
            var rt = (RectTransform)page.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.localRotation = Quaternion.identity;
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(SpDataW, SpH);

            // ⚠ anchoredPosition only writes x and y. Undo.SetTransformParent
            // preserves the WORLD pose, so the page arrives carrying whatever z
            // the move implied — and a page floating a few mm off the canvas it
            // sits on reads, through a canvas yawed 20°, as a SECOND panel offset
            // beside the first. That is the "two panels on the right" from the
            // 2026-08-06 device test; it was never a modal.
            var lp = rt.localPosition;
            rt.localPosition = new Vector3(lp.x, lp.y, 0f);

            var view = rig.GetComponent<SuperPanelView>();
            if (view != null)
            {
                // MERGE, never overwrite (trap 4): /2 can be re-run after /3 and /4
                // have built their pages, and an overwrite here orphaned a sibling
                // page into a permanently-active ghost with invisible hit areas.
                var pages = new GameObject[SuperPanelView.TabCount];
                pages[0] = page;
                var usage = SpFind("UsagePage");
                if (usage != null) pages[1] = usage;
                var envPage = SpFind("EnvironmentalPage");
                if (envPage != null) pages[2] = envPage;
                // data.Find, NOT SpFind: RBv2_1_1/08's legacy screen on DPPPanelCanvas
                // is ALSO named "CertificatesPage", and a global find grabbed it —
                // tab 3 then activated the flat-canvas screen at its own world pose
                // while the rig's page stayed dark (device, 2026-08-08).
                var certs = data.Find("CertificatesPage");
                if (certs != null) pages[3] = certs.gameObject;
                SetRefArray(view, "tabPages", pages);
            }

            // The page's own bottom bar now drives the walkthrough instead of
            // routing straight out of the passport.
            var ps = page.GetComponent<ProductSpecsView>();
            if (ps != null) SetRef(ps, "owner", view);

            // ---- close the model link, BOTH ways (00 §8.1) ----
            // This is the last phase that can do it: /1 builds the controller before the
            // page exists, and /9 builds the page before the rig exists. Neither can see
            // the other, so the cross-reference has to be made here or not at all.
            var link = rig.GetComponentInChildren<ModelLinkController>(true);
            if (link != null && ps != null)
            {
                SetRef(link, "productSpecs", ps);
                SetRef(ps, "modelLink", link);
                SetRef(link, "owner", view);
                SetInt(link, "productSpecsTab", 0);
                Debug.Log("[DPPUIBuilder] Model link closed: Product specs ↔ stage model.");
            }
            else Debug.LogWarning("[DPPUIBuilder] Could not close the model link — " +
                                  (link == null ? "no ModelLinkController under the rig (re-run RBv2_1_1/10 " +
                                                  "with VCU_assembly in the scene)."
                                                : "the page has no ProductSpecsView."));

            // ScreenRouter must stop treating the page as a sibling screen — it is
            // a child of the rig now, and Show() deactivating it would blank the
            // data canvas the moment the rig appeared.
            var router = SpFindRouter();
            if (router != null) SetRef(router, "productSpecs", null);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/11 — Product specs re-parented into the data canvas, " +
                      "model link closed. ScreenRouter.productSpecs cleared (it is no longer a sibling " +
                      "screen). SAVE THE SCENE.");
        }

        // =================================================================
        // Pieces
        // =================================================================

        /// <summary>Yaw that turns a canvas at local x = <paramref name="offsetX"/>
        /// to face a viewer <see cref="SpDistance"/> in front. A canvas's visible
        /// face is its −z, so the sign follows atan2(offset, distance) directly.</summary>
        private static float SpYaw(float offsetX) => Mathf.Atan2(offsetX, SpDistance) * Mathf.Rad2Deg;

        private static RectTransform SpCanvas(Transform rig, string name, float w, float h,
            Vector2 pivot, float localX, float yawDeg)
        {
            var go = new GameObject(name, typeof(Canvas), typeof(GraphicRaycaster));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;      // editor mouse interaction

            var rt = (RectTransform)go.transform;
            rt.SetParent(rig, false);
            rt.pivot = pivot;
            rt.sizeDelta = new Vector2(w, h);
            rt.localScale = Vector3.one * SpScale;
            rt.localPosition = new Vector3(localX, 0f, 0f);
            rt.localRotation = Quaternion.Euler(0f, yawDeg, 0f);
            return rt;
        }

        private static Button SpRailTab(RectTransform rail, int i, out RectTransform root,
            out Image fill, out Image stroke, out Image accent, out GameObject tick,
            out TMP_Text l1, out TMP_Text l2, out Image icon)
        {
            root = TL($"Tab{i}", rail, 18f, SpTabTop + i * 80f, 184f, 68f);
            AddShadow(root, 184f, 68f, DPPSpriteFactory.RoundedR13);

            var outline = AddImage(CenterIn("HoverOutline", root, 184f + HoverHalo, 68f + HoverHalo),
                DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            stroke = AddImage(CenterIn("Stroke", root, 184f, 68f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowStroke, sliced: true);
            fill = AddImage(CenterIn("Fill", root, 182f, 66f), DPPSpriteFactory.RoundedR13,
                DPPTheme.RowFill, sliced: true, raycast: true);
            AddGloss(root, 184f, 68f, DPPSpriteFactory.RoundedR13);

            accent = AddImage(TL("Accent", root, 0f, 14f, 4f, 40f), DPPSpriteFactory.RoundedR3,
                DPPTheme.TealLight, sliced: true);
            accent.gameObject.SetActive(false);

            var iconRT = TL("Icon", root, 20f, 22f, 24f, 24f);
            var sprite = LoadPageIcon(SpTabIcons[i]);
            icon = iconRT.gameObject.AddComponent<Image>();
            icon.preserveAspect = true;
            icon.raycastTarget = false;
            if (sprite != null) icon.sprite = sprite;
            else Debug.LogWarning($"[DPPUIBuilder] Rail icon '{SpTabIcons[i]}' not found — tab {i} drawn without one.");

            l1 = AddText(TL("Line1", root, 54f, 18f, 124f, 16f), SpTabL1[i], 12.5f, DPPTheme.TextSecondary, bold: false);
            l2 = AddText(TL("Line2", root, 54f, 34f, 124f, 16f), SpTabL2[i], 12.5f, DPPTheme.TextSecondary, bold: false);

            // Visited marker — the check icon, small, in the top-right corner
            // (Thiago, 2026-08-06). It replaces a plain teal dot, which said
            // "something is here" rather than "you have read this". Shown for the
            // RECYCLER ONLY: the Product user has no walkthrough, so a progress
            // marker on tabs that were never gated would invent a sequence that
            // does not exist.
            var tickRT = TLCenter("Tick", root, 170f, 13f, 15f, 15f);
            var tickImg = tickRT.gameObject.AddComponent<Image>();
            tickImg.preserveAspect = true;
            tickImg.raycastTarget = false;
            var tickSprite = LoadPageIcon("ic_visited");
            if (tickSprite != null) tickImg.sprite = tickSprite;
            else
            {
                // Without a sprite an Image draws a solid quad, which reads as a
                // rendering fault rather than a missing asset.
                tickImg.enabled = false;
                Debug.LogWarning("[DPPUIBuilder] Icon 'ic_visited' not found — visited ticks will not show.");
            }
            tick = tickRT.gameObject;
            tick.SetActive(false);

            var btn = root.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.targetGraphic = fill;
            var hover = root.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            return btn;
        }

        // (SpCertEntry DELETED 2026-08-09 in the retirement pass — certificates is
        // SpRailTab index 3 since 04e round 2.)

        /// <summary>
        /// Certificates &amp; safety as a PAGE IN THE DATA CANVAS, replacing the
        /// sibling screen RBv2_1_1/08 built. Four rows over the standard content band,
        /// and one button: with a single action there is nothing for 00 §5's
        /// primary-always-right to arbitrate, so the action IS the primary.
        /// </summary>
        private static RectTransform SpCertPage(RectTransform data, SuperPanelView view)
        {
            var page = Stretch("CertificatesPage", data);
            AddImage(Stretch("PageBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            var icon = TL("Icon", page, 24f, 24f, 18f, 18f);
            var sprite = LoadPageIcon("ic_certificates");
            if (sprite != null)
            {
                var img = icon.gameObject.AddComponent<Image>();
                img.sprite = sprite; img.preserveAspect = true; img.raycastTarget = false;
            }
            else Object.DestroyImmediate(icon.gameObject);

            AddText(TL("Title", page, 50f, 20f, 300f, 26f), "Certificates & safety",
                16f, DPPTheme.TextOnNavy, bold: true);
            AddImage(TL("Rule", page, 24f, 62f, 372f, 1f), null, DPPTheme.Hex("#1a335f"));

            // Four rows across the 76-360 band: 4 x 71 = 284 exactly.
            for (int i = 0; i < DpCertRows.GetLength(0); i++)
            {
                var row = TL($"Row{i}", page, 0f, 76f + i * 71f, 420f, 71f);

                var chip = TL("Chip", row, 24f, 20f, 76f, 22f);
                AddImage(Stretch("Fill", chip), DPPSpriteFactory.Pill, DPPTheme.CardBlue, sliced: true);
                AddText(Stretch("Label", chip), DpCertRows[i, 0], 10.5f,
                    DPPTheme.Hex(DpCertRows[i, 1]), bold: false, align: TextAlignmentOptions.Center);

                AddText(TL("Title", row, 110f, 6f, 286f, 16f), DpCertRows[i, 2], 11.5f,
                    DPPTheme.TextOnNavy, bold: true);
                var l1 = AddText(TL("Line1", row, 110f, 24f, 286f, 18f), DpCertRows[i, 3], 9f,
                    DPPTheme.TextSecondary, bold: false);
                var l2 = AddText(TL("Line2", row, 110f, 44f, 286f, 18f), DpCertRows[i, 4], 9f,
                    DPPTheme.Hex("#6f86a8"), bold: false);
                l1.textWrappingMode = TextWrappingModes.Normal;
                l2.textWrappingMode = TextWrappingModes.Normal;
            }

            // Grey, short, and just "Back" (Thiago, 2026-08-06). The green pill
            // read as "continue" on a page that goes nowhere, and at 230 × 46 it
            // took a fifth of the panel to say one word. Same capsule sprite and
            // same 50-unit hit root as every other bottom-bar button.
            // 04e round 2: the page is TAB 3 now, so Back = previous tab, like
            // every other page (CloseCertificates survives only as a wrapper).
            // Standardized bar geometry (Thiago, 2026-08-09): Back cx 69 / w 90 /
            // cy 402 / 11 pt on every data tab, Environmental as the reference.
            var back = PsSmallPill(page, "BackButton", 24f + 45f, 90f, "Back",
                primary: false, out _, cy: 402f, fontSize: 11f);
            WireClick(back, view, nameof(SuperPanelView.PrevTab));

            page.gameObject.SetActive(false);
            return page;
        }

        // (SpComplianceBadge DELETED 2026-08-09 in the retirement pass — the rail
        // badge died 2026-08-06 when certificates became a rail entry; no callers.)

        /// <summary>
        /// The zone's gesture HUD (spec 10 §3.2) on the stage. Round 4 (Thiago,
        /// 2026-08-08): the TOP SLOT is the LINKED/FREE toggle — a padlock, GREY
        /// when linked, GREEN when free — replacing the [?] help button (removed
        /// with its guide modal; the round-3 rail tile lasted one build). While
        /// the model is FREE the whole column FOLLOWS it via
        /// StageGestureHudFollower, the zone's §3.2 behaviour. Decisions 1–2
        /// stand: no [+] part list, no ⟲ regroup. Reuses ZoneGestureHUD with its
        /// help refs left null (every path through them is null-guarded).
        /// </summary>
        private static StageGestureHudFollower SpGestureHud(RectTransform stage,
            TwoHandTwistRotate twist, SuperPanelView view, Transform model,
            out Image lockGlyph, out TMP_Text lockLabel,
            out RectTransform hudBackplate, out GameObject hudExtras)
        {
            // TLCenter, not TL: the follower repositions by the column's CENTRE.
            // Round 5: cy 218 → 178 — with the collapsed backplate the LOCK, not
            // the column's middle, sits where the eye finds it.
            var col = TLCenter("GestureHud", stage, 370f, 178f, 44f, 180f);

            // Trap 5 (on-plane rule): the column TRAVELS with the freed model and
            // the hand-ray bridge resolves hits against CANVAS PLANES — travelling
            // UI needs its own nested Canvas + GraphicRaycaster or it stops being
            // touchable the moment it leaves the stage plane (device round 5).
            var colCanvas = col.gameObject.AddComponent<Canvas>();
            colCanvas.worldCamera = Camera.main;
            col.gameObject.AddComponent<GraphicRaycaster>();

            // Backplate anchored TOP-LEFT so it GROWS DOWNWARD: collapsed 70 while
            // LINKED (lock + state word only), expanded 180 by the free sequence.
            hudBackplate = TL("Backplate", col, 0f, 0f, 44f, 70f);
            AddImage(hudBackplate, DPPSpriteFactory.RoundedR13,
                new Color(0.180f, 0.353f, 0.627f, 0.627f), sliced: true);   // #2e5aa0 @ 160/255

            // LINKED/FREE toggle — 44 hit, 30 visual (00 §4.2).
            var lockRT = TLCenter("LockToggle", col, 22f, 22f, 44f, 44f);
            var lockFill = AddImage(CenterIn("Fill", lockRT, 30f, 30f), DPPSpriteFactory.Circle64,
                DPPTheme.Hex("#0a1f44"), sliced: false, raycast: true);
            var glyphRT = CenterIn("Glyph", lockRT, 16f, 16f);
            lockGlyph = glyphRT.gameObject.AddComponent<Image>();
            lockGlyph.preserveAspect = true;
            lockGlyph.raycastTarget = false;
            lockGlyph.color = Color.white;   // round 6: the sprite is pre-coloured, never tinted
            var lockedIcon = LoadPageIcon("ic_lock_linked");
            if (lockedIcon != null) lockGlyph.sprite = lockedIcon;
            else { lockGlyph.enabled = false; Debug.LogWarning("[DPPUIBuilder] Icon 'ic_lock_linked' not found — the lock toggle shows its word only."); }
            var lockBtn = lockRT.gameObject.AddComponent<Button>();
            lockBtn.transition = Selectable.Transition.None;
            lockBtn.targetGraphic = lockFill;
            lockRT.gameObject.AddComponent<HoverHighlight>();
            WireClick(lockBtn, view, nameof(SuperPanelView.ToggleLock));

            lockLabel = AddText(TLCenter("State", col, 22f, 46f, 44f, 9f), "LINKED",
                6.5f, DPPTheme.Hex("#8ba0bf"), bold: true, align: TextAlignmentOptions.Center);

            // Everything below the lock lives in the EXTRAS group — hidden while
            // LINKED, shown by the free sequence together with the plate growth.
            var extras = TL("Extras", col, 0f, 0f, 44f, 180f);
            hudExtras = extras.gameObject;

            // Hand lights: solid green = pinching, dark disc = open.
            var lOn  = AddImage(TLCenter("LOn",  extras, 14f, 62f, 11f, 11f), DPPSpriteFactory.Circle64, DPPTheme.Hex("#27c46c"));
            var lOff = AddImage(TLCenter("LOff", extras, 14f, 62f, 11f, 11f), DPPSpriteFactory.Circle64, DPPTheme.Hex("#23406e"));
            var rOn  = AddImage(TLCenter("ROn",  extras, 30f, 62f, 11f, 11f), DPPSpriteFactory.Circle64, DPPTheme.Hex("#27c46c"));
            var rOff = AddImage(TLCenter("ROff", extras, 30f, 62f, 11f, 11f), DPPSpriteFactory.Circle64, DPPTheme.Hex("#23406e"));

            TMP_Text Cap(string n, float y, string txt) =>
                AddText(TLCenter(n, extras, 22f, y, 42f, 10f), txt, 6.5f, DPPTheme.Hex("#5d7396"),
                    bold: true, align: TextAlignmentOptions.Center);
            TMP_Text Val(string n, float y) =>
                AddText(TLCenter(n, extras, 22f, y, 42f, 12f), "—", 8.5f, DPPTheme.Hex("#dbe4f0"),
                    bold: false, align: TextAlignmentOptions.Center);

            var yawCap  = Cap("YawCap", 80f, "YAW");    var yawVal  = Val("YawVal", 92f);
            var distCap = Cap("DistCap", 110f, "DIST"); var distVal = Val("DistVal", 122f);
            var zoomCap = Cap("ZoomCap", 140f, "ZOOM"); var zoomVal = Val("ZoomVal", 152f);

            var hud = col.gameObject.AddComponent<ZoneGestureHUD>();
            SetRef(hud, "twist", twist);
            SetRef(hud, "leftOn", lOn.gameObject);   SetRef(hud, "leftOff", lOff.gameObject);
            SetRef(hud, "rightOn", rOn.gameObject);  SetRef(hud, "rightOff", rOff.gameObject);
            SetRef(hud, "yawCap", yawCap);   SetRef(hud, "yawValue", yawVal);
            SetRef(hud, "distCap", distCap); SetRef(hud, "distValue", distVal);
            SetRef(hud, "zoomCap", zoomCap); SetRef(hud, "zoomValue", zoomVal);
            // helpButton / helpModal / modalCloseButton stay null — removed round 4.

            // Ships collapsed: extras hidden, plate at 70. The view's free
            // sequence is the ONLY thing that expands it (round 5).
            extras.gameObject.SetActive(false);

            var follower = col.gameObject.AddComponent<StageGestureHudFollower>();
            if (model != null) SetRef(follower, "model", model);
            return follower;
        }

        // (SpLockButton DELETED 2026-08-09 in the retirement pass — the LINKED/FREE
        // control lives in the stage's gesture column since 2026-08-08.)

        /// <summary>
        /// A standard 200 × 22 grabber (00 §5) whose <c>panelRoot</c> is an
        /// arbitrary transform, so the same component drives the rig and the freed
        /// model. <c>recenter</c> is off for the freed model: it must appear
        /// exactly where the user released it, not jump to a spawn pose.
        /// </summary>
        private static RectTransform SpGrabber(RectTransform canvas, Transform panelRoot,
            string name, Color gripColor, float spawnDistance, bool recenter = true)
        {
            var bar = TLCenter(name, canvas, SpStageW * 0.5f, SpH + 24f, 200f, 22f);
            AddImage(CenterIn("Stroke", bar, 202f, 24f), DPPSpriteFactory.Pill, DPPTheme.GrabberStroke, sliced: true);
            var fill = AddImage(CenterIn("Fill", bar, 200f, 22f), DPPSpriteFactory.Pill,
                DPPTheme.GrabberFill, sliced: true, raycast: true);
            var grip = AddImage(CenterIn("Grip", bar, 44f, 4f), DPPSpriteFactory.Grip, gripColor);

            var handle = bar.gameObject.AddComponent<PanelGrabHandle>();
            SetRef(handle, "panelRoot", panelRoot);
            SetRef(handle, "barFill", fill);
            SetRef(handle, "grip", grip);
            SetFloat(handle, "spawnDistance", spawnDistance);
            SetFloat(handle, "spawnHeightOffset", 0f);   // the rig is already authored at eye height
            SetBool(handle, "recenterOnStart", recenter);
            return bar;
        }

        /// <summary>
        /// The stage model is a CLONE, and it has to be.
        ///
        /// `RBv2_1_1/06` puts the original VCU_assembly on the "DPPPreview" layer and
        /// strips that layer from the main camera, so the original is invisible to
        /// the user by design — it exists to be filmed into the intro and how-to
        /// RenderTextures. Re-parenting it into the stage would have produced an
        /// empty stage and broken both loops at once.
        ///
        /// The clone drops DisassemblyAnimator (nothing drives it here), returns to
        /// the Default layer and is scaled to sit inside the stage.
        /// </summary>
        private static Transform SpBuildStageModel(Transform home)
        {
            var animator = Object.FindFirstObjectByType<DisassemblyAnimator>(FindObjectsInactive.Include);
            if (animator == null)
            {
                Debug.LogWarning("[DPPUIBuilder] No DisassemblyAnimator in the scene (VCU_assembly missing?) — " +
                                 "the stage is empty. Import the model and re-run RBv2_1_1/10.");
                return null;
            }

            // The idle yaw must spin the box about ITS OWN centre. A glTF import
            // puts the origin wherever the exporter left it — usually a corner —
            // and rotating that transform swings the mesh around a circle instead
            // of turning it on the spot. On device that read as a model changing
            // size as it rotated and clipping both side canvases (2026-08-06).
            //
            // So the model the view rotates is a PIVOT, and the mesh hangs off it
            // offset by minus its own bounds centre.
            var pivotGO = new GameObject("ModelPivot");
            var pivot = pivotGO.transform;
            pivot.SetParent(home, false);
            pivot.localPosition = Vector3.zero;
            pivot.localRotation = Quaternion.identity;
            pivot.localScale = Vector3.one;

            var clone = Object.Instantiate(animator.gameObject, pivot);
            clone.name = "StageModel";

            // ⚠ THE ANIMATOR STAYS. It used to be stripped here ("nothing drives it"),
            // which was true while the stage was a spinning picture. From RBv2.1.1 the
            // stage OPENS on entry using the same teardown the disassembly intro plays
            // (Thiago, 2026-08-07), and ModelLinkController drives this clone's copy.
            // Anything that would drive it on its OWN schedule has to go, though, or the
            // stage and the intro fight over the same animation.
            foreach (var loop in clone.GetComponentsInChildren<TeardownPreviewLoop>(true))
                Object.DestroyImmediate(loop);
            foreach (var ex in clone.GetComponentsInChildren<ExplosionController>(true))
                Object.DestroyImmediate(ex);
            // ⚠ COLLIDERS STAY. They were disabled when the stage was only a spinning
            // picture; from RBv2.1.1 the model is a SELECTION SURFACE and every body
            // needs something for the hand ray to hit. ModelLinkController re-enables
            // them anyway, but leaving them off here made that a silent dependency.
            foreach (var c in clone.GetComponentsInChildren<Collider>(true)) c.enabled = true;
            SetLayerRecursiveEditor(clone.transform, 0);   // back onto Default so the main camera sees it

            var t = clone.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;

            // Fit to ~260 of the stage's 340 canvas units.
            //
            // ⚠ Renderer.bounds is ALREADY world space, and the parent canvas is at
            // 0.001 — so `span` here is the on-screen size in metres, not the mesh's
            // local size. Dividing by SpScale a second time inflates the result by
            // 1000× (the first build logged a scale of 1,299,978). The fix is to
            // scale RELATIVE to the current localScale, which is also correct when
            // the source prefab is not authored at scale 1.
            var bounds = SpRendererBounds(t);
            float span = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            if (span > 1e-9f)
            {
                float targetWorld = 200f * SpScale;                 // 0.200 m — REAL SIZE, device round 4
                float k = t.localScale.x * (targetWorld / span);
                t.localScale = Vector3.one * k;
                Debug.Log($"[DPPUIBuilder] Stage model: world span {span * 1000f:0.##} mm at scale " +
                          $"{t.localScale.x / k:0.###} → fitted local scale {k:0.###} (target {targetWorld * 1000f:0.#} mm).");
            }
            else Debug.LogWarning("[DPPUIBuilder] Stage model has no measurable renderer bounds — scale left at 1.");

            // Re-measure AFTER scaling, then slide the mesh so its bounds centre
            // sits exactly on the pivot origin.
            var fitted = SpRendererBounds(t);
            Vector3 centreLocal = pivot.InverseTransformPoint(fitted.center);
            t.localPosition -= centreLocal;
            Debug.Log($"[DPPUIBuilder] Stage model re-centred by {(-centreLocal * 1000f)} mm (pivot-local) — " +
                      "the idle yaw now turns it on the spot.");

            // RBv2.1.1 — the bridge to the data canvas (00 §8.1). It lives on the PIVOT
            // so it travels with the model into the free root and back, and it takes the
            // CLONE as its root because that is what carries the glTF node names and the
            // scale it has to correct for the exploded envelope.
            var link = pivotGO.AddComponent<ModelLinkController>();
            SetRef(link, "modelRoot", t);
            SetRef(link, "animator", clone.GetComponentInChildren<DisassemblyAnimator>(true));
            SetRef(link, "handBridge", Object.FindFirstObjectByType<PicoHandUIBridge>(FindObjectsInactive.Include));
            // Round 4 (Thiago 2026-08-08): zoom 1.00 = REAL SIZE. The fit target is
            // now the CLOSED model's longest side = the physical unit's 200 mm
            // (printed mock 200 × 150 × 60), replacing the open-envelope shrink
            // chain (0.26 → ÷1.2 → ÷1.5) that made the closed model read as a toy.
            SetFloat(link, "realWorldSpan", 0.200f);

            return pivot;
        }

        private static Bounds SpRendererBounds(Transform root)
        {
            var rends = root.GetComponentsInChildren<Renderer>(true);
            if (rends.Length == 0) return new Bounds(root.position, Vector3.zero);
            var b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
            return b;
        }

        /// <summary>
        /// Find a GameObject by name ANYWHERE in the open scene, including
        /// inactive ones.
        ///
        /// <c>GameObject.Find</c> skips inactive objects, and every screen this
        /// builder makes is left deactivated — so phase 2 could not see the rig
        /// phase 1 had just built and reported "run RBv2_1_1/10 first" immediately
        /// after it had been run.
        /// </summary>
        private static GameObject SpFind(string name)
        {
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name == name) return root;
                var hit = SpFindIn(root.transform, name);
                if (hit != null) return hit;
            }
            return null;
        }

        private static GameObject SpFindIn(Transform t, string name)
        {
            foreach (Transform c in t)
            {
                if (c.name == name) return c.gameObject;
                var hit = SpFindIn(c, name);
                if (hit != null) return hit;
            }
            return null;
        }

        private static ScreenRouter SpFindRouter()
        {
            var canvas = SpFind("DPPPanelCanvas");
            if (canvas != null)
            {
                var r = canvas.GetComponent<ScreenRouter>();
                if (r != null) return r;
            }
            var found = Object.FindFirstObjectByType<ScreenRouter>(FindObjectsInactive.Include);
            if (found == null) Debug.LogWarning("[DPPUIBuilder] No ScreenRouter in the scene — the rig will not route.");
            return found;
        }

        // =================================================================
        // Serialized-value helper (SetRef only handles Object references)
        // =================================================================

        private static void SetInt(Object target, string fieldName, int value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop == null) { Debug.LogWarning($"[DPPUIBuilder] Int field '{fieldName}' not found on {target.GetType().Name}."); return; }
            prop.intValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetFloat(Object target, string fieldName, float value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop == null) { Debug.LogWarning($"[DPPUIBuilder] Float field '{fieldName}' not found on {target.GetType().Name}."); return; }
            prop.floatValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // SetBool already exists in DPPUIBuilder.StepFlow.cs — same partial class,
        // so it is in scope here. Only SetFloat was missing.
    }
}
