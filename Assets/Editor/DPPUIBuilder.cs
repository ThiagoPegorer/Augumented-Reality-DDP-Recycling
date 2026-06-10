using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// Builds the Canva-design DPP screens in MainScene, one menu item per
    /// phase. Source of truth: DPP_UI_Specs/00_design_standards.md plus the
    /// per-screen spec (01_main_page.md for Phase 1).
    ///
    /// Phase 1 (DPP → Build Phase 1 — Main Page):
    ///   - generates the procedural UI sprites,
    ///   - deletes the legacy "DashboardCanvas" (XR rig, hands, reticles,
    ///     DDPManager and EventSystem are untouched),
    ///   - creates "DPPPanelCanvas" (world space, 640×430 @ 0.001 scale) with
    ///     ScreenRouter + grabber bar (PanelGrabHandle),
    ///   - builds the Main Page per spec 01 and wires DPPManager.mainPage.
    ///
    /// Safe to re-run: an existing DPPPanelCanvas is deleted and rebuilt.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        // ---- Canvas geometry (00 §1; placement carried over from the old DashboardCanvas) ----
        private const float PanelW = 640f, PanelH = 430f;
        // y matches the XR rig's Camera Y Offset (1.1176) so the panel sits at
        // eye height both in the Editor (no HMD → camera rests at the offset)
        // and on device. z = 0.6 m in front, same as the old DashboardCanvas.
        private static readonly Vector3 CanvasPos = new Vector3(0f, 1.1176f, 0.6f);
        private const float CanvasScale = 0.001f;

        private static TMP_FontAsset _fontRegular, _fontBold;

        [MenuItem("DPP/Build Phase 1 — Main Page", false, 1)]
        public static void BuildPhase1()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            RemoveByName("DashboardCanvas");
            RemoveByName("DPPPanelCanvas"); // allow clean rebuild

            // ---- Canvas root ----
            var canvasGO = new GameObject("DPPPanelCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            Undo.RegisterCreatedObjectUndo(canvasGO, "Build DPP Main Page");

            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main; // Editor mouse interaction

            var canvasRT = (RectTransform)canvasGO.transform;
            canvasRT.sizeDelta = new Vector2(PanelW, PanelH);
            canvasRT.position = CanvasPos;
            canvasRT.localScale = Vector3.one * CanvasScale;

            var router = canvasGO.AddComponent<ScreenRouter>();

            // ---- Screen 01: Main Page ----
            var mainPage = BuildMainPage(canvasRT, router);

            // ---- Grabber bar (00 §5) ----
            BuildGrabberBar(canvasRT);

            // ---- Wiring ----
            SetRef(router, "mainPage", mainPage.gameObject);

            var manager = Object.FindFirstObjectByType<DPPManager>();
            if (manager != null) SetRef(manager, "mainPage", mainPage.GetComponent<MainPageView>());
            else Debug.LogWarning("[DPPUIBuilder] No DPPManager found in the scene — serial/step bindings not wired.");

            Selection.activeGameObject = canvasGO;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Phase 1 — Main Page built. Save the scene to persist it.");
        }

        // =================================================================
        // Screen 01 — Main Page (spec 01_main_page.md §2)
        // =================================================================
        private static RectTransform BuildMainPage(RectTransform canvasRT, ScreenRouter router)
        {
            var page = Stretch("MainPage", canvasRT);
            var view = page.gameObject.AddComponent<MainPageView>();

            // Panel surface — navy, r22.
            AddImage(Stretch("PanelBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);

            // Hero serial — 32 bold white, baseline ≈ y100.
            var serial = AddText(TL("SerialText", page, 30, 64, 580, 44),
                "VCU-DEMO-001", 32, DPPTheme.TextOnNavy, bold: true);

            // ---- Informations card (left choice) ----
            // 2026-06-10: switched from the light-grey mockup style to the same
            // blue card style as Disassembly (user feedback from Editor testing —
            // grey read as white and drowned the hover outline).
            var infoCard = BuildChoiceCard(page, "InformationsCard", x: 30,
                fill: DPPTheme.CardBlue, stroke: DPPTheme.TabActiveStroke, strokeWidth: 2,
                circleColor: DPPTheme.NavyPanel,
                title: "Informations", titleColor: DPPTheme.TextOnNavy,
                subtitle: "Passport & materials", subtitleColor: DPPTheme.TextSubtitleNavy);
            AddText(Stretch("iGlyph", infoCard.iconCircle), "i", 21, DPPTheme.TextOnNavy,
                bold: false, align: TextAlignmentOptions.Center);
            WireClick(infoCard.button, router, nameof(ScreenRouter.ShowInformations));

            // ---- Disassembly card (right choice, primary path) ----
            var disCard = BuildChoiceCard(page, "DisassemblyCard", x: 330,
                fill: DPPTheme.CardBlue, stroke: DPPTheme.TabActiveStroke, strokeWidth: 2,
                circleColor: DPPTheme.TealAccent,
                title: "Disassembly", titleColor: DPPTheme.TextOnNavy,
                subtitle: "Guided recycling · 5 steps", subtitleColor: DPPTheme.TextSubtitleNavy);
            AddImage(CenterIn("RecycleIcon", disCard.iconCircle, 30, 30),
                DPPSpriteFactory.Recycle, Color.white);
            // Chevron › built from two capsule bars (font-independent).
            ChevronBar(disCard.card, "ChevronTop", cy: 46, zRot: -45f);
            ChevronBar(disCard.card, "ChevronBottom", cy: 54, zRot: 45f);
            WireClick(disCard.button, router, nameof(ScreenRouter.ShowDisassembly));

            // Bind view fields.
            SetRef(view, "serialText", serial);
            SetRef(view, "disassemblySubtitleText",
                disCard.card.Find("Subtitle").GetComponent<TMP_Text>());

            return page;
        }

        private struct ChoiceCard
        {
            public RectTransform card;
            public RectTransform iconCircle;
            public Button button;
        }

        /// <summary>One 280×100 r20 choice card at panel y170: hover outline + stroke + fill + icon circle + title + subtitle.</summary>
        private static ChoiceCard BuildChoiceCard(RectTransform page, string name, float x,
            Color fill, Color stroke, int strokeWidth, Color circleColor,
            string title, Color titleColor, string subtitle, Color subtitleColor)
        {
            const float W = 280f, H = 100f;
            var card = TL(name, page, x, 170, W, H);

            // Hover-only white outline (00 §4), behind everything, disabled at rest.
            var outline = AddImage(CenterIn("HoverOutline", card, W + 12, H + 12),
                DPPSpriteFactory.RoundedR22, Color.white, sliced: true);
            outline.gameObject.SetActive(false);

            // Resting stroke ring (fill-behind-fill technique).
            AddImage(CenterIn("Stroke", card, W + 2 * strokeWidth, H + 2 * strokeWidth),
                DPPSpriteFactory.RoundedR20, stroke, sliced: true);

            // Card fill — the click/raycast surface.
            var fillImg = AddImage(CenterIn("Fill", card, W, H),
                DPPSpriteFactory.RoundedR20, fill, sliced: true, raycast: true);

            // Icon circle at card-local (46,50), r22.
            var circle = TLCenter("IconCircle", card, 46, 50, 44, 44);
            AddImage(circle, DPPSpriteFactory.Circle64, circleColor);

            AddText(TL("Title", card, 82, 26, 170, 24), title, 18, titleColor, bold: true);
            AddText(TL("Subtitle", card, 82, 53, 190, 18), subtitle, 13, subtitleColor, bold: false);

            // Interaction: Button (pinch/click) + HoverHighlight (outline + lift).
            var button = card.gameObject.AddComponent<Button>();
            button.transition = Selectable.Transition.None;
            button.targetGraphic = fillImg;

            var hover = card.gameObject.AddComponent<HoverHighlight>();
            SetRef(hover, "highlightOutline", outline.gameObject);
            SetRef(hover, "lift", card);

            return new ChoiceCard { card = card, iconCircle = circle, button = button };
        }

        private static void ChevronBar(RectTransform card, string name, float cy, float zRot)
        {
            var bar = TLCenter(name, card, 256, cy, 13, 2.5f);
            bar.localRotation = Quaternion.Euler(0, 0, zRot);
            AddImage(bar, DPPSpriteFactory.Grip, DPPTheme.TextSubtitleNavy);
        }

        // =================================================================
        // Grabber bar (00 §5) — pill 200×22 docked 12 under the panel edge
        // =================================================================
        private static void BuildGrabberBar(RectTransform canvasRT)
        {
            var barGO = new GameObject("GrabberBar", typeof(RectTransform));
            var bar = (RectTransform)barGO.transform;
            bar.SetParent(canvasRT, false);
            bar.anchorMin = bar.anchorMax = new Vector2(0.5f, 0f);
            bar.pivot = new Vector2(0.5f, 1f);
            bar.anchoredPosition = new Vector2(0f, -12f);
            bar.sizeDelta = new Vector2(200f, 22f);

            AddImage(CenterIn("Stroke", bar, 202, 24), DPPSpriteFactory.Pill, DPPTheme.GrabberStroke);
            var fill = AddImage(CenterIn("Fill", bar, 200, 22), DPPSpriteFactory.Pill, DPPTheme.GrabberFill, raycast: true);
            var grip = AddImage(CenterIn("Grip", bar, 44, 4), DPPSpriteFactory.Grip, DPPTheme.GrabberGrip);

            var handle = barGO.AddComponent<PanelGrabHandle>();
            SetRef(handle, "panelRoot", canvasRT);
            SetRef(handle, "barFill", fill);
            SetRef(handle, "grip", grip);
        }

        // =================================================================
        // Rect / component helpers
        // =================================================================

        /// <summary>Full-stretch child rect.</summary>
        private static RectTransform Stretch(string name, Transform parent)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return rt;
        }

        /// <summary>Top-left anchored rect using spec coordinates (x right, y down from the parent's top-left).</summary>
        private static RectTransform TL(string name, Transform parent, float x, float y, float w, float h)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(x, -y);
            rt.sizeDelta = new Vector2(w, h);
            return rt;
        }

        /// <summary>Rect whose CENTER sits at spec coordinates (cx, cy) from the parent's top-left.</summary>
        private static RectTransform TLCenter(string name, Transform parent, float cx, float cy, float w, float h)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(cx, -cy);
            rt.sizeDelta = new Vector2(w, h);
            return rt;
        }

        /// <summary>Rect centered in its parent.</summary>
        private static RectTransform CenterIn(string name, Transform parent, float w, float h)
        {
            var rt = NewRT(name, parent);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(w, h);
            return rt;
        }

        private static RectTransform NewRT(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            return rt;
        }

        private static Image AddImage(RectTransform rt, string spriteName, Color color,
            bool sliced = false, bool raycast = false)
        {
            var img = rt.gameObject.AddComponent<Image>();
            img.sprite = DPPSpriteFactory.Load(spriteName);
            img.type = sliced ? Image.Type.Sliced : Image.Type.Simple;
            img.color = color;
            img.raycastTarget = raycast;
            return img;
        }

        private static TMP_Text AddText(RectTransform rt, string text, float size, Color color,
            bool bold, TextAlignmentOptions align = TextAlignmentOptions.MidlineLeft)
        {
            var tmp = rt.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = align;
            tmp.textWrappingMode = TextWrappingModes.NoWrap;
            tmp.raycastTarget = false;

            if (bold && _fontBold != null) tmp.font = _fontBold;
            else
            {
                if (_fontRegular != null) tmp.font = _fontRegular;
                if (bold) tmp.fontStyle = FontStyles.Bold; // synthetic bold fallback
            }
            return tmp;
        }

        private static void WireClick(Button button, object target, string methodName)
        {
            var action = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), target, methodName);
            UnityEventTools.AddPersistentListener(button.onClick, action);
        }

        /// <summary>Assigns a private [SerializeField] by name via SerializedObject (persists in the scene).</summary>
        private static void SetRef(Object target, string fieldName, Object value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop == null)
            {
                Debug.LogWarning($"[DPPUIBuilder] Field '{fieldName}' not found on {target.GetType().Name}.");
                return;
            }
            prop.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // =================================================================
        // Fonts: prefer SF Pro TMP assets in Assets/Fonts, else LiberationSans.
        // See docs/sf_pro_font_setup.md for how to import SF Pro.
        // =================================================================
        private static void ResolveFonts()
        {
            _fontRegular = _fontBold = null;

            var fontFolders = new System.Collections.Generic.List<string>();
            if (AssetDatabase.IsValidFolder("Assets/Fonts")) fontFolders.Add("Assets/Fonts");
            if (AssetDatabase.IsValidFolder("Assets/Font"))  fontFolders.Add("Assets/Font");

            if (fontFolders.Count > 0)
            {
                foreach (string guid in AssetDatabase.FindAssets("t:TMP_FontAsset", fontFolders.ToArray()))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
                    if (font == null) continue;

                    string lower = path.ToLowerInvariant();
                    if (lower.Contains("italic")) continue; // UI uses upright only

                    bool isBold = lower.Contains("bold");
                    bool isDisplay = lower.Contains("display");

                    // Prefer "Display" cuts (panel text is mostly >=13 px at AR scale);
                    // a Display asset replaces a previously-found Text asset.
                    if (isBold) { if (_fontBold == null || isDisplay) _fontBold = font; }
                    else        { if (_fontRegular == null || isDisplay) _fontRegular = font; }
                }
            }

            if (_fontRegular == null)
                _fontRegular = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                    "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");

            Debug.Log($"[DPPUIBuilder] Fonts → regular: {(_fontRegular ? _fontRegular.name : "TMP default")}, " +
                      $"bold: {(_fontBold ? _fontBold.name : "synthetic (FontStyles.Bold)")}");
        }

        private static void RemoveByName(string name)
        {
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name == name)
                {
                    Undo.DestroyObjectImmediate(root);
                    Debug.Log($"[DPPUIBuilder] Removed old '{name}'.");
                    return;
                }
            }
        }
    }
}
