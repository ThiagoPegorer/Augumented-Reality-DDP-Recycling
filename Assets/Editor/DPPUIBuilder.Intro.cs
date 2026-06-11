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
    /// Phase 3 builder — Screen 03: Disassembly intro (spec 03 v2, approved 2026-06-11).
    /// Split layout: 2×2 stat matrix left, teardown preview floating right,
    /// full-width teal Start CTA. Built under the existing DPPPanelCanvas.
    ///
    /// The teardown hero is the static background-removed PNG at
    /// Assets/Textures/Intro/vcu_teardown.png (spec 03 §5.1); the animated
    /// frame-sequence / 3D loop upgrade path (spec 03 §5.2) swaps the Image
    /// content only — layout stays.
    ///
    /// Safety banner (spec 03 §4) is intentionally NOT built — the MS 50.4 has
    /// no hazards; DisassemblyIntroView logs a warning if data ever disagrees.
    /// Safe to re-run (rebuilds the DisassemblyIntro object).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const string TeardownTexPath = "Assets/Textures/Intro/vcu_teardown.png";

        [MenuItem("DPP/Build Phase 3 — Disassembly Intro", false, 3)]
        public static void BuildPhase3()
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

            var old = canvasRT.Find("DisassemblyIntro");
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

            var screen = Stretch("DisassemblyIntro", canvasRT);
            Undo.RegisterCreatedObjectUndo(screen.gameObject, "Build DPP Disassembly Intro");
            var view = screen.gameObject.AddComponent<DisassemblyIntroView>();

            AddImage(Stretch("PanelBG", screen), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // ---- Header (shared with 02; Disassembly active) ----
            MakeTabHeader(screen, router, disassemblyActive: true);

            // ---- Left half: eyebrow + 2×2 stat matrix (spec 03 §2/§6) ----
            AddText(TL("Eyebrow", screen, 24, 96, 200, 16), "Job overview", 12.5f, DPPTheme.TextCaption, bold: false);

            var tools = MakeStatCard(screen, "ToolsCard", 24, 116, "Tools", "Torx driver", "+ spudger", accent: false);
            SetRef(view, "toolsValue", tools.value);
            SetRef(view, "toolsSub", tools.sub);

            var time = MakeStatCard(screen, "TimeCard", 174, 116, "Est. time", "~8 min", null, accent: false, valueSize: 17);
            SetRef(view, "timeValue", time.value);

            var scope = MakeStatCard(screen, "ScopeCard", 24, 206, "Scope", "5 steps", "12 parts", accent: false);
            SetRef(view, "scopeValue", scope.value);
            SetRef(view, "scopeSub", scope.sub);

            var recover = MakeStatCard(screen, "RecoverCard", 174, 206, "Recover", "2 high-value", "connectors · silicon", accent: true);
            SetRef(view, "recoverValue", recover.value);
            SetRef(view, "recoverSub", recover.sub);

            // ---- Right half: teardown preview floating on navy (spec 03 §5.1) ----
            var teardownSprite = LoadTeardownSprite();
            var hero = TLCenter("TeardownPreview", screen, 468, 205, 242, 225);
            var heroImg = hero.gameObject.AddComponent<Image>();
            heroImg.sprite = teardownSprite;
            heroImg.preserveAspect = true;
            heroImg.raycastTarget = false;
            if (teardownSprite == null)
            {
                heroImg.color = DPPTheme.Hex("#0c2348"); // visible placeholder if PNG missing
                Debug.LogWarning($"[DPPUIBuilder] Teardown image not found at {TeardownTexPath} — placeholder shown.");
            }

            AddText(TLCenter("PreviewCaption", screen, 468, 346, 300, 14),
                "Teardown preview · illustrative", 11, DPPTheme.TextTip, bold: false,
                align: TextAlignmentOptions.Center);

            // ---- Start CTA (spec 03 §7) ----
            BuildStartButton(screen, router);

            // ---- Wiring ----
            if (router != null) SetRef(router, "disassemblyIntro", screen.gameObject);
            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "disassemblyIntro", view);
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager in scene — intro stats not bound to backend data.");

            screen.gameObject.SetActive(false); // router shows MainPage by default

            Selection.activeGameObject = screen.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 3 — Disassembly Intro built. Save the scene.");
        }

        // =================================================================
        private struct StatCard
        {
            public TMP_Text value, sub;
        }

        /// <summary>140×80 stat card (spec 03 §6). Accent = teal Recover styling.</summary>
        private static StatCard MakeStatCard(RectTransform screen, string name, float x, float y,
            string label, string demoValue, string demoSub, bool accent, float valueSize = 15)
        {
            var card = TL(name, screen, x, y, 140, 80);

            AddImage(CenterIn("Stroke", card, 142, 82), DPPSpriteFactory.RoundedR13,
                accent ? DPPTheme.TealAccent : DPPTheme.RowStroke, sliced: true);
            AddImage(CenterIn("Fill", card, 140, 80), DPPSpriteFactory.RoundedR13,
                accent ? DPPTheme.Hex("#0e2335") : DPPTheme.RowFill, sliced: true);

            Color labelColor = accent ? DPPTheme.TealMuted : DPPTheme.TextLabel;
            Color valueColor = accent ? DPPTheme.TealText : DPPTheme.TextOnNavy;
            Color subColor   = accent ? DPPTheme.TealMuted : DPPTheme.TextCaption;

            AddText(TL("Label", card, 0, 12, 140, 16), label, 12, labelColor, bold: false,
                align: TextAlignmentOptions.Center);

            bool hasSub = demoSub != null;
            var value = AddText(TL("Value", card, 0, hasSub ? 32 : 30, 140, hasSub ? 22 : 26),
                demoValue, valueSize, valueColor, bold: true, align: TextAlignmentOptions.Center);

            TMP_Text sub = null;
            if (hasSub)
                sub = AddText(TL("Sub", card, 0, 56, 140, 16), demoSub, 11, subColor, bold: false,
                    align: TextAlignmentOptions.Center);

            return new StatCard { value = value, sub = sub };
        }

        private static void BuildStartButton(RectTransform screen, ScreenRouter router)
        {
            var btn = TL("StartButton", screen, 24, 368, 592, 48);

            var outline = AddImage(CenterIn("HoverOutline", btn, 600, 56), DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
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

        /// <summary>Loads the teardown PNG, fixing import settings on first use.</summary>
        private static Sprite LoadTeardownSprite()
        {
            var importer = AssetImporter.GetAtPath(TeardownTexPath) as TextureImporter;
            if (importer == null)
            {
                AssetDatabase.Refresh(); // file may exist but not yet imported
                importer = AssetImporter.GetAtPath(TeardownTexPath) as TextureImporter;
            }
            if (importer == null) return null;

            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.maxTextureSize = 1024;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(TeardownTexPath);
        }
    }
}
