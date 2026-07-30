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
    ///   housing_upper   → PETG yellow #f2c11e   (printed part 2 — Bambu Lab
    ///                                            PETG yellow, approximated;
    ///                                            tune BrownHex/YellowHex and
    ///                                            re-run if the shade is off)
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
        private const string BrownHex  = "#8a5a3b";   // housing_bottom (part 1)
        private const string YellowHex = "#f2c11e";   // housing_upper  (part 2, Bambu PETG yellow approx.)
        private const string GreenHex  = "#2e7d4f";   // connectors + PCB
        private const string MatFolder = "Assets/Materials/DPPRealColors";

        [MenuItem("RBv2_0/Tools/Apply real-life colors", false, 30)]
        public static void ApplyRealLifeColors()
        {
            var animator = Object.FindFirstObjectByType<DisassemblyAnimator>();
            if (animator == null)
            {
                Debug.LogError("[DPPUIBuilder] No DisassemblyAnimator in the scene — is VCU_assembly loaded?");
                return;
            }

            Color brown  = DPPTheme.Hex(BrownHex);
            Color yellow = DPPTheme.Hex(YellowHex);
            Color green  = DPPTheme.Hex(GreenHex);

            EnsureMatFolder();

            var cache = new Dictionary<Material, Material>();
            int renderers = 0;

            foreach (Transform child in animator.transform)
            {
                string n = child.gameObject.name;
                Color target;

                if      (SameName(n, "housing_bottom"))                target = brown;
                else if (SameName(n, "housing_upper"))                 target = yellow;
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
            Debug.Log($"[DPPUIBuilder] Real-life colors applied to {renderers} renderers " +
                      $"(bottom={BrownHex}, upper={YellowHex}, connectors+PCB={GreenHex}). " +
                      "Zone clone, previews and ghosting inherit automatically.");
        }

        /// <summary>Clone-or-update: originals get a saved "_real" clone with
        /// the target color; clones from a previous run are re-tinted in place
        /// so re-running with tweaked hexes never duplicates assets.</summary>
        private static Material GetRecolored(Material source, Color target, Dictionary<Material, Material> cache)
        {
            if (source.name.EndsWith("_real"))
            {
                SetBaseColor(source, target);
                EditorUtility.SetDirty(source);
                return source;
            }
            if (cache.TryGetValue(source, out var existing)) return existing;

            // Reuse a previously saved clone of this material if one exists.
            string path = $"{MatFolder}/{source.name}_real.mat";
            var saved = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (saved != null)
            {
                SetBaseColor(saved, target);
                EditorUtility.SetDirty(saved);
                cache[source] = saved;
                return saved;
            }

            var clone = new Material(source) { name = source.name + "_real" };
            SetBaseColor(clone, target);
            AssetDatabase.CreateAsset(clone, path);
            cache[source] = clone;
            return clone;
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
