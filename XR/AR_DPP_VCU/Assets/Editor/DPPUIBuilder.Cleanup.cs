using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2.0 housekeeping — removes RBv1.0 leftovers from an EXISTING scene
    /// without rebuilding anything.
    ///
    /// Why this exists: RBv2_1_1/01 is destructive (it deletes DPPPanelCanvas and
    /// every screen under it), so it is the wrong tool for "get rid of the old
    /// main page". This is the safe, in-place path.
    ///
    /// What it removes:
    ///   · MainPage        — the RBv1.0 two-card page (Informations | Disassembly).
    ///                       Its builder was deleted 2026-07-30; nothing routes
    ///                       to it. MainPageView.Populate() is an empty method and
    ///                       DPPManager.mainPage only feeds a null-guard warning,
    ///                       so removing it changes no behaviour.
    ///   · InformationTab  — the RBv1.0 passport screen, split into DppCanva +
    ///                       ModelExploration by RBv2_1_1/Legacy.
    ///   · DashboardCanvas — the pre-Canva colloquium dashboard, if any scene
    ///                       still carries one.
    ///
    /// Idempotent: run it as often as you like. Save the scene afterwards.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        [MenuItem("RBv2_1_1/Tools/Clean RBv1.0 leftovers", false, 103)]
        public static void CleanRBv1Leftovers()
        {
            int removed = 0;

            // Root-level legacy canvas (pre-Canva dashboard).
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (root.name != "DashboardCanvas") continue;
                Undo.DestroyObjectImmediate(root);
                Debug.Log("[DPPUIBuilder] Removed legacy root 'DashboardCanvas'.");
                removed++;
                break;
            }

            // Scene-root iteration, not GameObject.Find: the panel canvas may be
            // inactive while the Welcome canvas owns the screen.
            Transform canvas = null;
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
                if (root.name == "DPPPanelCanvas") { canvas = root.transform; break; }

            if (canvas == null)
            {
                Debug.LogWarning("[DPPUIBuilder] DPPPanelCanvas not found — is MainScene open? " +
                                 (removed > 0 ? "Legacy root canvas was still removed." : "Nothing to clean."));
                if (removed > 0) EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                return;
            }

            removed += DestroyIfFound(canvas, "MainPage");
            removed += DestroyIfFound(canvas, "InformationTab");

            if (removed == 0)
            {
                Debug.Log("[DPPUIBuilder] No RBv1.0 leftovers found — scene is already clean.");
                return;
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log($"[DPPUIBuilder] Removed {removed} RBv1.0 leftover object(s). Save the scene.");
        }

        private static int DestroyIfFound(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child == null) return 0;
            Undo.DestroyObjectImmediate(child.gameObject);
            Debug.Log($"[DPPUIBuilder] Removed '{name}' from {parent.name}.");
            return 1;
        }
    }
}
