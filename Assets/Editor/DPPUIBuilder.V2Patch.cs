using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// ReBuilt v2.0 — in-place patch for the Disassembly intro header (block 3).
    ///
    /// Re-running Phase 3 would rebuild the whole intro screen (and its live
    /// preview camera wiring); this only touches the header, following the
    /// PatchMainPageV2 pattern:
    ///
    ///   1. Deletes the two tab pills — the tab bar is gone in RBv2.0 (single
    ///      linear path with a one-step-back hierarchy).
    ///   2. Converts the Home button into a BACK button: house glyph → back
    ///      arrow, ShowMainPage → ShowDppCanva. It keeps the same position and
    ///      the same 50 px hit area, so the hand still knows the spot.
    ///   3. Adds the eyebrow + title that the tab pills used to stand in for.
    ///
    /// Idempotent — safe to run more than once. Save the scene afterwards.
    ///
    /// NOT done here (deliberately):
    ///   - Removing the exploded zone from the step flow: that is a ScreenRouter
    ///     flag (zoneFollowsExploration), set by Phase 8. No StepFlow rebuild.
    ///   - Retargeting the cancel modal: that is a one-line source change in
    ///     StepFlowController.CancelYes (ShowMainPage → ShowDisassembly).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("DPP/Patch v2.0 — Intro header (in-place)", false, 101)]
        public static void PatchIntroHeaderV2()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            // Scene-root iteration: GameObject.Find misses inactive objects, and
            // the intro is inactive whenever another screen is showing.
            Transform canvas = null;
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
                if (root.name == "DPPPanelCanvas") { canvas = root.transform; break; }

            if (canvas == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — is MainScene open?");
                return;
            }

            var router = canvas.GetComponent<ScreenRouter>();
            if (router == null)
            {
                Debug.LogError("[DPPUIBuilder] No ScreenRouter on DPPPanelCanvas.");
                return;
            }

            var intro = canvas.Find("DisassemblyIntro") as RectTransform;
            if (intro == null)
            {
                Debug.LogError("[DPPUIBuilder] DisassemblyIntro not found — run Phase 3 first.");
                return;
            }

            // ---- 1. tab pills out ----
            DestroyIfPresent(intro, "TabInformations");
            DestroyIfPresent(intro, "TabDisassembly");

            // ---- 2. Home → Back ----
            var home = intro.Find("HomeButton") ?? intro.Find("BackButton");
            if (home == null)
            {
                Debug.LogWarning("[DPPUIBuilder] No Home/Back button on the intro — header left unpatched.");
            }
            else
            {
                Undo.RecordObject(home.gameObject, "Intro header v2.0");
                home.name = "BackButton";

                var iconTf = home.Find("Icon");
                var icon = iconTf != null ? iconTf.GetComponent<Image>() : null;
                if (icon != null)
                {
                    Undo.RecordObject(icon, "Intro header v2.0");
                    icon.sprite = DPPSpriteFactory.Load(DPPSpriteFactory.IcBack);
                }
                else Debug.LogWarning("[DPPUIBuilder] Back button has no 'Icon' child — glyph not swapped.");

                var btn = home.GetComponent<Button>();
                if (btn != null)
                {
                    // Drop every persistent listener (ShowMainPage) before rewiring,
                    // otherwise a second run stacks two navigations on one click.
                    while (btn.onClick.GetPersistentEventCount() > 0)
                        UnityEventTools.RemovePersistentListener(btn.onClick, 0);

                    WireClick(btn, router, nameof(ScreenRouter.ShowDppCanva));
                }
                else Debug.LogWarning("[DPPUIBuilder] Back button has no Button component — click not rewired.");
            }

            // ---- 3. header eyebrow + title (where the pills used to be) ----
            DestroyIfPresent(intro, "HeaderEyebrow");
            DestroyIfPresent(intro, "HeaderTitle");
            AddText(TL("HeaderEyebrow", intro, 76, 24, 300, 15),
                "Digital Product Passport", 11.5f, DPPTheme.TextCaption, bold: false);
            AddText(TL("HeaderTitle", intro, 76, 40, 440, 24),
                "Disassembly", 19, DPPTheme.TextOnNavy, bold: true);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Intro header patched for RBv2.0 — tabs removed, Back → DPP Canva. Save the scene.");
        }

        private static void DestroyIfPresent(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null)
            {
                Undo.DestroyObjectImmediate(child.gameObject);
                Debug.Log($"[DPPUIBuilder] Removed '{name}' from {parent.name}.");
            }
        }
    }
}
