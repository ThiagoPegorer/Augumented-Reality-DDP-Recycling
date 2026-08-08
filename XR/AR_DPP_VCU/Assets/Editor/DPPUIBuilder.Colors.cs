using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// "DPP → Apply Real-Life Colors" (2026-07-20): tints the VCU model to
    /// match the physical 3D-printed prototype so the AR model and the real
    /// unit on the desk read as the same object during the user studies.
    ///
    ///   housing_bottom  → PETG brown  #8a5a3b   (printed part 1)
    ///   housing_upper   → PETG brown  #8a5a3b   (2026-08-08: Thiago confirmed
    ///                                            the PHYSICAL printed lid is
    ///                                            brown — the earlier yellow
    ///                                            mapping matched a print that
    ///                                            does not exist. Both shells
    ///                                            share the print colour now.)
    ///   connector*      → green #2e7d4f
    ///   pcb (board)     → green #2e7d4f
    ///   screws / chips  → untouched
    ///
    /// Mechanics: classifies the TOP-LEVEL children of VCU_assembly by name
    /// (exact same rules as ConstrainedTeardownModel), then for every renderer
    /// under a matched node clones its material, sets the base color, and
    /// saves the clone under Assets/Materials/DPPRealColors so it survives
    /// scene reloads. The runtime zone clone, the RT preview loops and the
    /// step-focus ghosting all inherit automatically because they derive from
    /// this one scene instance. Safe to re-run: already-recolored materials
    /// ("*_real") are updated in place, never duplicated.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const string BrownHex  = "#8a5a3b";   // BOTH housing shells (print colour, 2026-08-08)
        private const string GreenHex  = "#2e7d4f";   // connectors + PCB
        private const string MatFolder = "Assets/Materials/DPPRealColors";

        [MenuItem("RBv2_1/Tools/Apply real-life colors", false, 30)]
        public static void ApplyRealLifeColors()
        {
            // ⚠ ALL animators, not FindFirstObjectByType (device round 1, 2026-08-07).
            // Since RBv2_1_1/1 the scene holds TWO DisassemblyAnimators — the original
            // VCU_assembly (filmed into the intro/how-to RTs, on the DPPPreview layer)
            // and the super-panel stage clone, which KEEPS its animator. Recolouring
            // REPLACES renderer.sharedMaterials on the object it walks, so a
            // first-found walk recolours one model and leaves the other on the old
            // materials. Include inactive: the rig is built SetActive(false).
            var animators = Object.FindObjectsByType<DisassemblyAnimator>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (animators.Length == 0)
            {
                Debug.LogError("[DPPUIBuilder] No DisassemblyAnimator in the scene — is VCU_assembly loaded?");
                return;
            }

            Color brown  = DPPTheme.Hex(BrownHex);
            Color green  = DPPTheme.Hex(GreenHex);

            EnsureMatFolder();

            var cache = new Dictionary<string, Material>();
            int renderers = 0;

            foreach (var animator in animators)
            foreach (Transform child in animator.transform)
            {
                string n = child.gameObject.name;
                Color target;

                if      (SameName(n, "housing_bottom"))                target = brown;
                else if (SameName(n, "housing_upper"))                 target = brown;   // print lid is brown (2026-08-08)
                else if (StartsName(n, "connector"))                   target = green;   // "screws_connector" starts with "screws_" → not matched
                else if (SameName(n, "pcb"))                           target = green;   // board only; pcb_screw* not matched
                else continue;

                foreach (var r in child.GetComponentsInChildren<Renderer>(true))
                {
                    var mats = r.sharedMaterials;
                    bool changed = false;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        if (mats[i] == null) continue;
                        mats[i] = GetRecolored(mats[i], target, cache);
                        changed = true;
                    }
                    if (changed)
                    {
                        Undo.RecordObject(r, "Apply Real-Life Colors");
                        r.sharedMaterials = mats;
                        EditorUtility.SetDirty(r);
                        renderers++;
                    }
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[DPPUIBuilder] Real-life colors applied to {renderers} renderers on " +
                      $"{animators.Length} model(s) " +
                      $"(both shells={BrownHex}, connectors+PCB={GreenHex}). " +
                      "Zone clone, previews and ghosting inherit automatically.");
        }

        /// <summary>
        /// Clone-or-update, keyed by SOURCE + TARGET COLOUR.
        ///
        /// ⚠ It used to be keyed by source alone ("{name}_real"), and the glTF
        /// gives BOTH housings the same material (`mat_0`) — so housing_bottom
        /// made "mat_0_real" brown and housing_upper then re-tinted the SAME
        /// asset yellow. Last writer won, and both housings came out yellow on
        /// device (2026-08-07). A shared source now forks into one saved clone
        /// per colour ("mat_0_real_8A5A3B", "mat_0_real_F2C11E"), and re-running
        /// with tweaked hexes still updates in place without duplicating assets.
        /// Legacy single-colour "*_real" clones from the old runs are treated as
        /// their base material and superseded; the orphaned .mat files in
        /// DPPRealColors are harmless and can be deleted by hand whenever.
        /// </summary>
        private static Material GetRecolored(Material source, Color target, Dictionary<string, Material> cache)
        {
            string hex = ColorUtility.ToHtmlStringRGB(target);
            string baseName = BaseMatName(source.name);
            string clonedName = $"{baseName}_real_{hex}";

            // Already the right per-colour clone → re-tint in place (hex tweaks).
            if (source.name == clonedName)
            {
                SetBaseColor(source, target);
                EditorUtility.SetDirty(source);
                return source;
            }
            if (cache.TryGetValue(clonedName, out var existing)) return existing;

            // Reuse a previously saved per-colour clone if one exists.
            string path = $"{MatFolder}/{clonedName}.mat";
            var saved = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (saved != null)
            {
                SetBaseColor(saved, target);
                EditorUtility.SetDirty(saved);
                cache[clonedName] = saved;
                return saved;
            }

            var clone = new Material(source) { name = clonedName };
            SetBaseColor(clone, target);
            AssetDatabase.CreateAsset(clone, path);
            cache[clonedName] = clone;
            return clone;
        }

        /// <summary>"mat_0" from "mat_0", "mat_0_real" (legacy) or "mat_0_real_8A5A3B".</summary>
        private static string BaseMatName(string n)
        {
            int i = n.IndexOf("_real", System.StringComparison.OrdinalIgnoreCase);
            return i < 0 ? n : n.Substring(0, i);
        }

        /// <summary>Covers URP/Lit (_BaseColor), Standard (_Color) and the
        /// glTFast PBR property (baseColorFactor) — whichever the material has.</summary>
        private static void SetBaseColor(Material m, Color c)
        {
            if (m.HasProperty("_BaseColor"))      m.SetColor("_BaseColor", c);
            if (m.HasProperty("_Color"))          m.SetColor("_Color", c);
            if (m.HasProperty("baseColorFactor")) m.SetColor("baseColorFactor", c);
        }

        private static void EnsureMatFolder()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Materials"))
                AssetDatabase.CreateFolder("Assets", "Materials");
            if (!AssetDatabase.IsValidFolder(MatFolder))
                AssetDatabase.CreateFolder("Assets/Materials", "DPPRealColors");
        }

        private static bool SameName(string a, string b)
            => string.Equals(a, b, System.StringComparison.OrdinalIgnoreCase);

        private static bool StartsName(string a, string b)
            => a.StartsWith(b, System.StringComparison.OrdinalIgnoreCase);
    }
}
