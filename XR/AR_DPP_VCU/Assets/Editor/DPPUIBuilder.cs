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
    /// Builds the ReBuilt v2.0 DPP screens in MainScene, one menu item per
    /// step, numbered in RUN ORDER under the "RBv2_0" menu. Source of truth:
    /// DPP_UI_Specs/00_design_standards.md plus the per-screen spec.
    ///
    /// RBv2_1_1/01 — Panel canvas + router:
    ///   - generates the procedural UI sprites,
    ///   - deletes the legacy "DashboardCanvas" (XR rig, hands, reticles,
    ///     DDPManager and EventSystem are untouched),
    ///   - creates "DPPPanelCanvas" (world space, 640×430 @ 0.001 scale) with
    ///     ScreenRouter + grabber bar (PanelGrabHandle). No screens.
    ///
    /// ⚠ DESTRUCTIVE: re-running this DELETES DPPPanelCanvas and every screen
    /// under it — you must then re-run RBv2_1_1/03 → /6 and RBv2_1_1/04 → /8. For removing
    /// RBv1.0 leftovers from an existing scene use RBv2_1_1/Tools instead.
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

        [MenuItem("RBv2_1_1/01 — Panel canvas + router", false, 1)]
        public static void Build1_PanelCanvas()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            RemoveByName("DashboardCanvas");
            RemoveByName("DPPPanelCanvas"); // allow clean rebuild

            // ---- Canvas root ----
            var canvasGO = new GameObject("DPPPanelCanvas", typeof(Canvas), typeof(GraphicRaycaster));
            Undo.RegisterCreatedObjectUndo(canvasGO, "Build DPP panel canvas");

            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main; // Editor mouse interaction

            var canvasRT = (RectTransform)canvasGO.transform;
            canvasRT.sizeDelta = new Vector2(PanelW, PanelH);
            canvasRT.position = CanvasPos;
            canvasRT.localScale = Vector3.one * CanvasScale;

            canvasGO.AddComponent<ScreenRouter>();

            // ---- Grabber bar (00 §5) ----
            BuildGrabberBar(canvasRT);

            // RBv2.0 (2026-07-30): NO Main Page. The RBv1.0 two-card page
            // (Informations | Disassembly) went with the tab bar — the passport
            // canvas built by RBv2_1_1/Legacy is the app's main screen and the Welcome
            // canvas (RBv2_1_1/02) is the entry point. Nothing routes to a main page.

            Selection.activeGameObject = canvasGO;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/01 — panel canvas + router built (no screens). Now run RBv2_1_1/03 → /6, then RBv2_1_1/04 and RBv2_1_1/08, then save the scene.");
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
        /// <summary>Total size a hover outline adds to the element it rings, so the
        /// halo is HoverHalo/2 on each side. Reduced 10-12 -> 6 (5-6 px per side -> 3)
        /// on Thiago's device review, 2026-08-04: at arm's length the thicker ring read
        /// as a selected STATE rather than as a passing highlight.</summary>
        private const float HoverHalo = 6f;

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
