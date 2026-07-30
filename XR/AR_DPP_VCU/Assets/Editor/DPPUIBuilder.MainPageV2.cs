using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DPP.EditorTools
{
    /// <summary>
    /// One-shot in-place patch for spec 01 v2 (2026-07-10) — applies the
    /// Main Page changes to the EXISTING scene without rebuilding the canvas
    /// (rerunning "Build Phase 1" would delete DPPPanelCanvas and with it
    /// screens 02–09, so never use that for small revisions).
    ///
    ///   1. Deletes the SerialText hero ("VCU-2026-001").
    ///   2. Re-centers both choice cards vertically (y 170 → 165).
    ///   3. Informations subtitle → "Digital Product Passport".
    ///   4. Disassembly subtitle  → "Guided recycling" (step count dropped).
    ///
    /// Safe to run more than once (idempotent). Save the scene afterwards.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("DPP/Patch Main Page v2 (in-place)", false, 100)]
        public static void PatchMainPageV2()
        {
            // Find the canvas via scene roots — GameObject.Find misses inactive
            // objects, and MainPage may be inactive if another screen is showing.
            Transform canvas = null;
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
                if (root.name == "DPPPanelCanvas") { canvas = root.transform; break; }

            if (canvas == null)
            {
                Debug.LogError("[DPPUIBuilder] DPPPanelCanvas not found — is MainScene open?");
                return;
            }

            var page = canvas.Find("MainPage");
            if (page == null)
            {
                Debug.LogError("[DPPUIBuilder] MainPage not found under DPPPanelCanvas.");
                return;
            }

            // 1. Serial hero out.
            var serial = page.Find("SerialText");
            if (serial != null)
            {
                Undo.DestroyObjectImmediate(serial.gameObject);
                Debug.Log("[DPPUIBuilder] Removed SerialText.");
            }

            // 2–4. Cards: re-center + new subtitles.
            PatchChoiceCard(page, "InformationsCard", "Digital Product Passport");
            PatchChoiceCard(page, "DisassemblyCard", "Guided recycling");

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] Main Page v2 patch applied — save the scene to persist it.");
        }

        private static void PatchChoiceCard(Transform page, string cardName, string subtitleText)
        {
            var card = page.Find(cardName) as RectTransform;
            if (card == null)
            {
                Debug.LogWarning($"[DPPUIBuilder] '{cardName}' not found — skipped.");
                return;
            }

            Undo.RecordObject(card, "Main Page v2");
            card.anchoredPosition = new Vector2(card.anchoredPosition.x, -165f);

            var subtitle = card.Find("Subtitle");
            var tmp = subtitle != null ? subtitle.GetComponent<TMP_Text>() : null;
            if (tmp != null)
            {
                Undo.RecordObject(tmp, "Main Page v2");
                tmp.text = subtitleText;
            }
            else Debug.LogWarning($"[DPPUIBuilder] Subtitle text not found on '{cardName}'.");
        }
    }
}
