using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_0/Tools — WIRING VERIFIER.
    ///
    /// The build phases are independent, but several of them write references INTO
    /// objects another phase owns. When a phase is re-run it destroys and recreates
    /// its objects, and every reference pointing at the old ones goes stale. Unity
    /// does not complain: a serialized reference to a destroyed object simply reads
    /// as null at runtime, so the symptom is always the same — a button that looks
    /// perfectly normal and does nothing.
    ///
    /// That has now cost two debugging sessions (Welcome's Continue after re-running
    /// RBv2_0/2; the same class of failure earlier). The phases now re-point what they
    /// break (see the SELF-HEALING WIRE notes in /2 and /5), and this menu item is the
    /// backstop: it walks every cross-phase reference and reports what is missing,
    /// with the phase to re-run.
    ///
    /// Run it after any sequence of phase builds, and before every device build.
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private struct Wire
        {
            public string owner, field, fixPhase;
            public Wire(string o, string f, string p) { owner = o; field = f; fixPhase = p; }
        }

        // owner component -> fields that must be non-null, and the phase that sets them.
        private static readonly Dictionary<System.Type, Wire[]> WiringMap = new Dictionary<System.Type, Wire[]>
        {
            [typeof(ScreenRouter)] = new[]
            {
                new Wire("ScreenRouter", "stakeholderDecision", "RBv2_0/8"),
                new Wire("ScreenRouter", "dppCanva",            "RBv2_0/7"),
                new Wire("ScreenRouter", "modelExploration",    "RBv2_0/7"),
                new Wire("ScreenRouter", "disassemblyIntro",    "RBv2_0/4"),
                new Wire("ScreenRouter", "stepFlow",            "RBv2_0/5"),
                new Wire("ScreenRouter", "completionSummary",   "RBv2_0/6"),
                new Wire("ScreenRouter", "explodedCanvas",      "RBv2_0/5"),
            },
            [typeof(WelcomeController)] = new[]
            {
                new Wire("WelcomeController", "mainCanvasRoot", "RBv2_0/3"),
                new Wire("WelcomeController", "scanner",        "RBv2_0/2 (or /3)"),
            },
            [typeof(QRScanController)] = new[]
            {
                new Wire("QRScanController", "manager",         "RBv2_0/2"),
                new Wire("QRScanController", "router",          "RBv2_0/2"),
                new Wire("QRScanController", "mainCanvasRoot",  "RBv2_0/2"),
                new Wire("QRScanController", "scanGroup",       "RBv2_0/2"),
                new Wire("QRScanController", "foundGroup",      "RBv2_0/2"),
                new Wire("QRScanController", "errorGroup",      "RBv2_0/2"),
                new Wire("QRScanController", "closeAppButton",  "RBv2_0/2"),
                new Wire("QRScanController", "scanAgainButton", "RBv2_0/2"),
            },
            [typeof(StakeholderSelect)] = new[]
            {
                new Wire("StakeholderSelect", "router", "RBv2_0/8"),
            },
            [typeof(StepFlowController)] = new[]
            {
                new Wire("StepFlowController", "router",        "RBv2_0/5"),
                new Wire("StepFlowController", "summary",       "RBv2_0/6 (or /5)"),
                new Wire("StepFlowController", "howToLoop",     "RBv2_0/5"),
                new Wire("StepFlowController", "confirmButton", "RBv2_0/5"),
                new Wire("StepFlowController", "cancelModal",   "RBv2_0/5"),
            },
        };

        [MenuItem("RBv2_0/Tools/Verify wiring", false, 20)]
        public static void VerifyWiring()
        {
            int missing = 0, dangling = 0, absent = 0, checkedCount = 0;

            foreach (var kv in WiringMap)
            {
                var comp = Object.FindFirstObjectByType(kv.Key, FindObjectsInactive.Include) as Component;
                if (comp == null)
                {
                    Debug.LogWarning($"[Verify] {kv.Key.Name} is NOT IN THE SCENE — its phase has never run.");
                    absent++;
                    continue;
                }

                var so = new SerializedObject(comp);
                foreach (var w in kv.Value)
                {
                    var prop = so.FindProperty(w.field);
                    if (prop == null)
                    {
                        Debug.LogError($"[Verify] {w.owner}.{w.field} — NO SUCH FIELD. The verifier map is out of date with the code.");
                        missing++;
                        continue;
                    }
                    checkedCount++;

                    // A destroyed target reads as null but keeps its instance id — that
                    // is precisely the "another phase overwrote me" signature, and it is
                    // worth separating from a reference that was simply never set.
                    if (prop.objectReferenceValue == null)
                    {
                        if (prop.objectReferenceInstanceIDValue != 0)
                        {
                            Debug.LogError($"[Verify] {w.owner}.{w.field} → DANGLING (target destroyed by a later phase). Re-run {w.fixPhase}.");
                            dangling++;
                        }
                        else
                        {
                            Debug.LogError($"[Verify] {w.owner}.{w.field} → NOT SET. Run {w.fixPhase}.");
                            missing++;
                        }
                    }
                }
            }

            if (missing + dangling == 0)
                Debug.Log($"[Verify] Wiring OK — {checkedCount} references checked, none missing." +
                          (absent > 0 ? $" ({absent} component(s) not in the scene yet.)" : ""));
            else
                Debug.LogError($"[Verify] {dangling} dangling + {missing} unset reference(s). " +
                               "Re-run the phases named above, then SAVE THE SCENE.");
        }
    }
}
