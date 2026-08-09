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
    /// RBv2_1_1/05 builder — Screen 03: Disassembly intro (spec 03 v3, 2026-07-10).
    ///
    /// v3 changes (approved mock 03_intro_v3.svg):
    ///   - 2×2 stat-card matrix REMOVED (boxes read as buttons). Left half is
    ///     now plain label/value rows: Tools · Est. time · Scope.
    ///   - Recover card REMOVED; replaced by a "Dismantling" bullet list bound
    ///     to backend disassembly.parts[] (physical part groups).
    ///   - Teardown hero: static PNG replaced by a LIVE 3D loop — a preview
    ///     camera films VCU_assembly (driven by DisassemblyAnimator via
    ///     TeardownPreviewLoop) into a RenderTexture shown by a RawImage in
    ///     the same slot. Caption: "Teardown preview · live 3D".
    ///
    /// RBv2.0 (2026-07-30): header rebuilt in place of the tab bar — back arrow
    /// → DPP Canva, eyebrow + title. `Patch v2.0 — Intro header` is now redundant
    /// and was deleted; this builder produces the v2.0 header directly.
    ///
    /// Safety banner (spec 03 §4) is still intentionally NOT built.
    /// Safe to re-run (rebuilds DisassemblyIntro + TeardownPreviewCamera only —
    /// never the canvas).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const string PreviewCamName = "TeardownPreviewCamera";

        // Demo content baked at build time; Populate() overwrites from backend.
        // Laid out as TWO columns of three (v3.1): index 0–2 = column 1, 3–5 = column 2.
        private static readonly string[] DemoParts =
        {
            "VCU case",
            "PCB board",
            "3 processors",
            "3 chips",
            "3 connectors",
            "14 screws",
        };

        [MenuItem("RBv2_1_1/05 — Disassembly intro", false, 5)]
        public static void Build4_DisassemblyIntro()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var canvasGO = GameObject.Find("DPPPanelCanvas");
            if (canvasGO == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — run 'RBv2_0 → 1 — Panel canvas + router' first.");
                return;
            }
            var canvasRT = (RectTransform)canvasGO.transform;
            var router = canvasGO.GetComponent<ScreenRouter>();

            var old = canvasRT.Find("DisassemblyIntro");
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

            var screen = Stretch("DisassemblyIntro", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build DPP Disassembly Intro");
            var view = screen.gameObject.AddComponent<DisassemblyIntroView>();

            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---- Header (shared with 02; Disassembly active) ----
            // RBv2.0: tab bar removed. Back goes ONE level, to the DPP Canva.
            // (Was MakeTabHeader(..., disassemblyActive: true) — re-running that
            // is what used to resurrect the v1.0 tab bar + Home→ShowMainPage.)
            MakeScreenHeader(screen, "Digital Product Passport", "Disassembly",
                rightCaption: null, backTarget: router, backMethod: nameof(ScreenRouter.ShowDppCanva));

            // ---- Left half: unboxed job overview (spec 03 v3) ----
            AddText(TL("JobOverviewEyebrow", screen, 24, 96, 200, 16), "Job overview", 12.5f, DPPTheme.TextCaption, bold: false);

            SetRef(view, "toolsValue", MakeOverviewRow(screen, "ToolsRow", 124, "Tools", "Allen key (hex 2.5 mm)"));
            SetRef(view, "timeValue",  MakeOverviewRow(screen, "TimeRow",  152, "Est. time", "~5 min"));
            SetRef(view, "scopeValue", MakeOverviewRow(screen, "ScopeRow", 180, "Scope", "5 steps"));

            // Divider between overview and dismantling list.
            AddImage(TL("Divider", screen, 24, 210, 276, 1.5f), DPPSpriteFactory.Grip,
                DPPTheme.Hex("#1a335f"), sliced: false);

            // ---- Dismantling list (bound to disassembly.parts[]) ----
            AddText(TL("DismantlingEyebrow", screen, 24, 226, 200, 16), "Dismantling", 12.5f, DPPTheme.TextCaption, bold: false);

            var partLabels = new TMP_Text[DemoParts.Length];
            for (int i = 0; i < DemoParts.Length; i++)
            {
                // Two columns of three: rows 0–2 at x24, rows 3–5 at x168.
                float colX = i < 3 ? 24f : 168f;
                float rowY = 248f + (i % 3) * 22f;
                var row = TL($"PartRow{i + 1}", screen, colX, rowY, 140, 18);
                var dot = TLCenter("Dot", row, 6, 9, 6, 6);
                AddImage(dot, DPPSpriteFactory.Circle64, DPPTheme.TealAccent);
                // Same style as the Job-overview values ("Allen key (hex 2.5 mm)").
                partLabels[i] = AddText(TL("Label", row, 18, 0, 122, 18),
                    DemoParts[i], 14, DPPTheme.TextOnNavy, bold: true);
            }
            SetRefArray(view, "partLabels", partLabels);

            // ---- Right half: LIVE 3D teardown preview (spec 03 v3 §5) ----
            var hero = TLCenter("TeardownPreview", screen, 468, 205, 242, 225);
            var raw = hero.gameObject.AddComponent<RawImage>();
            raw.raycastTarget = false;
            raw.enabled = false; // TeardownPreviewLoop enables it with the RT at runtime

            AddText(TLCenter("PreviewCaption", screen, 468, 346, 300, 14),
                "Teardown preview", 11, DPPTheme.TextTip, bold: false,
                align: TextAlignmentOptions.Center);

            // Preview camera (scene root, disabled — the loop turns it on).
            RemoveByName(PreviewCamName);
            var camGO = new GameObject(PreviewCamName, typeof(Camera));
            Undo.RegisterCreatedObjectUndo(camGO, "Build DPP Disassembly Intro");
            var cam = camGO.GetComponent<Camera>();
            cam.enabled = false;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0f, 0f, 0f, 0f);

            var loop = screen.gameObject.AddComponent<TeardownPreviewLoop>();
            SetRef(loop, "previewCamera", cam);
            SetRef(loop, "target", raw);
            var animator = Object.FindFirstObjectByType<DisassemblyAnimator>();
            if (animator != null) SetRef(loop, "vcuAnimator", animator);
            else Debug.LogWarning("[DPPUIBuilder] No DisassemblyAnimator found (VCU_assembly missing?) — preview loop will retry at runtime.");

            // ---- Start CTA (spec 03 §7, unchanged) ----
            BuildStartButton(screen, router);

            // ---- Wiring ----
            if (router != null) SetRef(router, "disassemblyIntro", screen.gameObject);
            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "disassemblyIntro", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in scene — intro stats not bound to backend data.");

            screen.gameObject.SetActive(false); // router shows MainPage by default

            Selection.activeGameObject = screen.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/05 — Disassembly Intro v3 built. Save the scene.");
        }

        /// <summary>One unboxed overview row: grey label at x24, bold white value at x112.</summary>
        private static TMP_Text MakeOverviewRow(RectTransform screen, string name, float y,
            string label, string demoValue)
        {
            var row = TL(name, screen, 24, y, 296, 20);
            AddText(TL("Label", row, 0, 1, 84, 18), label, 12.5f, DPPTheme.TextLabel, bold: false);
            return AddText(TL("Value", row, 88, 0, 208, 20), demoValue, 14, DPPTheme.TextOnNavy, bold: true);
        }

        /// <summary>Assigns a private [SerializeField] ARRAY by name via SerializedObject.</summary>
        private static void SetRefArray(Object target, string fieldName, Object[] values)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop == null || !prop.isArray)
            {
                Debug.LogWarning($"[DPPUIBuilder] Array field '{fieldName}' not found on {target.GetType().Name}.");
                return;
            }
            prop.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
                prop.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void BuildStartButton(RectTransform screen, ScreenRouter router)
        {
            var btn = TL("StartButton", screen, 24, 368, 592, 48);

            var outline = AddImage(CenterIn("HoverOutline", btn, 598, 54), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            var fill = AddImage(CenterIn("Fill", btn, 592, 48), DPPSpriteFactory.RoundedR13,
                DPPTheme.TealAccent, sliced: true, raycast: true);

            AddText(Stretch("Label", btn), "Start disassembly", 16, DPPTheme.TextOnNavy,
                bold: true, align: TextAlignmentOptions.Center);

            // Chevron › from two capsule bars, right of the centered label.
            var top = TLCenter("ChevronTop", btn, 384, 20, 13, 2.5f);
            top.localRotation = Quaternion.Euler(0, 0, -45);
            AddImage(top, DPPSpriteFactory.Grip, Color.white);
            var bottom = TLCenter("ChevronBottom", btn, 384, 28, 13, 2.5f);
            bottom.localRotation = Quaternion.Euler(0, 0, 45);
            AddImage(bottom, DPPSpriteFactory.Grip, Color.white);

            var button = btn.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = fill;
            if (router != null) WireClick(button, router, nameof(ScreenRouter.ShowStepFlow));

            var hover = btn.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
        }
    }
}
