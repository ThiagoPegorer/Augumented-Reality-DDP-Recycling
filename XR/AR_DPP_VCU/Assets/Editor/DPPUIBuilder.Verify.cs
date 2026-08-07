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
    /// RBv2_1/2; the same class of failure earlier). The phases now re-point what they
    /// break (see the SELF-HEALING WIRE notes in /2 and /5), and this menu item is the
    /// backstop: it walks every cross-phase reference and reports what is missing,
    /// with the phase to re-run.
    ///
    /// Run it after any sequence of phase builds, and before every device build.
    ///
    /// WHAT BELONGS IN THE MAP: references one phase writes into an object another
    /// phase owns. References created and wired inside a single phase (a tile's own
    /// chips, a button's own label) cannot dangle without someone deleting them by
    /// hand, and listing them would bury the signal in noise.
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
                new Wire("ScreenRouter", "stakeholderDecision", "RBv2_1/7"),
                new Wire("ScreenRouter", "certificates",        "RBv2_1/8"),
                // `productSpecs` is NOT listed. RBv2_1_1/2 clears it on purpose: once
                // the super panel owns the page it is a child of the data canvas, not
                // a sibling screen, and leaving the reference set would make Show()
                // deactivate it the moment the rig appeared. Null is the correct
                // state here, so verifying it would report a fault on every run.
                new Wire("ScreenRouter", "dppSuperPanel",       "RBv2_1_1/1"),
                new Wire("ScreenRouter", "freeModelRoot",       "RBv2_1_1/1"),
                new Wire("ScreenRouter", "panelGrabber",        "RBv2_1_1/1"),
                // RBv2.1: dppCanva now points at the DPP PAGE, set by RBv2_1/8.
                // Re-running RBv2_0/Legacy rebuilds the legacy canva and does NOT restore
                // this reference — /1 is the phase to re-run.
                new Wire("ScreenRouter", "dppCanva",            "RBv2_1/8"),

                // ---- RB2.0 screens that are STILL ON THE PATH ----
                // The Recycler's "Continue to disassembly" runs straight into these.
                // They are legacy in NUMBERING only; spec 04b replaces them, and until
                // it does, an unset reference here means the teardown dead-ends.
                //
                // WARNING: RBv2_1/1 is DESTRUCTIVE - it deletes DPPPanelCanvas and
                // every screen under it, so re-running it always empties these four.
                // That is the whole reason this map exists.
                //
                // `modelExploration` was here and is RETIRED (2026-08-06). Spec 04 v2
                // put the model in the super panel stage, so the standalone exploration
                // screen has no route in and nothing calls it. The field and
                // ShowModelExploration() survive only because serialized UnityEvents on
                // the legacy DppCanva still point at the method - deleting it would log
                // missing-method errors rather than be silently unused. Kill both in the
                // RB2.0 retirement pass.
                new Wire("ScreenRouter", "disassemblyIntro",    "RBv2_0/4"),
                new Wire("ScreenRouter", "stepFlow",            "RBv2_0/5"),
                new Wire("ScreenRouter", "completionSummary",   "RBv2_0/6"),
                new Wire("ScreenRouter", "explodedCanvas",      "RBv2_0/5"),
            },
            [typeof(WelcomeController)] = new[]
            {
                new Wire("WelcomeController", "mainCanvasRoot", "RBv2_1/3"),
                new Wire("WelcomeController", "scanner",        "RBv2_1/2 (or /3)"),
            },
            [typeof(QRScanController)] = new[]
            {
                new Wire("QRScanController", "manager",         "RBv2_1/2"),
                new Wire("QRScanController", "router",          "RBv2_1/2"),
                new Wire("QRScanController", "mainCanvasRoot",  "RBv2_1/2"),
                new Wire("QRScanController", "scanGroup",       "RBv2_1/2"),
                new Wire("QRScanController", "foundGroup",      "RBv2_1/2"),
                new Wire("QRScanController", "errorGroup",      "RBv2_1/2"),
                new Wire("QRScanController", "closeAppButton",  "RBv2_1/2"),
                new Wire("QRScanController", "scanAgainButton", "RBv2_1/2"),
            },
            [typeof(StakeholderSelect)] = new[]
            {
                new Wire("StakeholderSelect", "router",  "RBv2_1/7"),
                new Wire("StakeholderSelect", "welcome", "RBv2_1/7"),   // Quit -> Welcome (RBv2.1)
            },
            // RBv2.1 spec 04. Only CROSS-PHASE references are listed: the chips and
            // their labels are created and wired inside the same run as the page, so
            // they cannot go stale on their own. router / welcome / scanner point at
            // objects three other phases own, and those are the ones that rot.
            [typeof(DppPageView)] = new[]
            {
                new Wire("DppPageView", "router",       "RBv2_1/8"),
                new Wire("DppPageView", "welcome",      "RBv2_1/8"),
                new Wire("DppPageView", "scanner",      "RBv2_1/8"),
                new Wire("DppPageView", "backButton",   "RBv2_1/8"),
                new Wire("DppPageView", "title",        "RBv2_1/8"),
                new Wire("DppPageView", "leftLabel",    "RBv2_1/8"),
                new Wire("DppPageView", "leftFill",     "RBv2_1/8"),
                new Wire("DppPageView", "leftStroke",   "RBv2_1/8"),
                new Wire("DppPageView", "primaryLabel", "RBv2_1/8"),
            },
            [typeof(ProductSpecsView)] = new[]
            {
                // 2026-08-06: panelTitle / panelCaption / subTabRow were deleted when
                // the page lost its title and the two pills became the header. The
                // map has to move with the code — a stale row reports a fault that
                // no phase can fix, which is exactly as bad as no verifier at all.
                new Wire("ProductSpecsView", "router",       "RBv2_1/9"),
                new Wire("ProductSpecsView", "subIdFill",    "RBv2_1/9"),
                new Wire("ProductSpecsView", "subIdLabel",   "RBv2_1/9"),
                new Wire("ProductSpecsView", "subCompFill",  "RBv2_1/9"),
                new Wire("ProductSpecsView", "subCompLabel", "RBv2_1/9"),
                new Wire("ProductSpecsView", "backLabel",    "RBv2_1/9"),
                new Wire("ProductSpecsView", "primaryLabel", "RBv2_1/9"),
                new Wire("ProductSpecsView", "primaryButton","RBv2_1/9"),
                new Wire("ProductSpecsView", "identityRoot", "RBv2_1/9"),
                new Wire("ProductSpecsView", "partsRoot",    "RBv2_1/9"),
                new Wire("ProductSpecsView", "detailRoot",   "RBv2_1/9"),
                new Wire("ProductSpecsView", "drawingRoot",  "RBv2_1/9"),
                new Wire("ProductSpecsView", "listContent",  "RBv2_1/9"),
                new Wire("ProductSpecsView", "drawingCard",  "RBv2_1/9"),
                new Wire("ProductSpecsView", "detailDrawing","RBv2_1/9"),
                new Wire("ProductSpecsView", "lowerBlock",   "RBv2_1/9"),
                new Wire("ProductSpecsView", "infoModal",    "RBv2_1/9"),
                new Wire("ProductSpecsView", "viewButton",   "RBv2_1/9"),
                new Wire("ProductSpecsView", "drawingLarge", "RBv2_1/9"),
                // `owner` is NOT listed: RBv2_1_1/2 sets it, and it is legitimately
                // null while the page runs standalone on DPPPanelCanvas.
            },
            [typeof(SuperPanelView)] = new[]
            {
                new Wire("SuperPanelView", "router",         "RBv2_1_1/1"),
                new Wire("SuperPanelView", "welcome",        "RBv2_1_1/1"),
                new Wire("SuperPanelView", "scanner",        "RBv2_1_1/1"),
                new Wire("SuperPanelView", "stageModelHome", "RBv2_1_1/1"),
                new Wire("SuperPanelView", "freeModelRoot",  "RBv2_1_1/1"),
                // `model` is deliberately absent: it is null when VCU_assembly is
                // not in the scene, and that is a warning at build time, not a
                // wiring fault the verifier should re-report every run.
                new Wire("SuperPanelView", "ghostOutline",   "RBv2_1_1/1"),
                new Wire("SuperPanelView", "lockLabel",      "RBv2_1_1/1"),
                new Wire("SuperPanelView", "placeholderPage","RBv2_1_1/1"),
            },
            [typeof(DPP.DPPManager)] = new[]
            {
                // Without this the page shows the values baked at build time and
                // silently stops tracking the payload - the failure mode spec 13
                // called out as "static copy that lies".
                new Wire("DPPManager", "dppPage",      "RBv2_1/8"),
                new Wire("DPPManager", "productSpecs", "RBv2_1/9"),
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

        [MenuItem("RBv2_1/Tools/Verify wiring", false, 20)]
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
