using System.Collections.Generic;
using UnityEngine;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 — THE BRIDGE BETWEEN THE STAGE MODEL AND THE DATA CANVAS
    /// (`00` §8.1, spec 04c §4.7). Thiago, 2026-08-07.
    ///
    /// LINKED (the state the passport opens in)
    ///   The model plays the teardown ONCE on entry and stays open. It does not
    ///   spin. Opening a component in the data canvas highlights its bodies;
    ///   pinching a body opens that component's Component ID page. Both directions
    ///   drive one piece of state, so they cannot disagree.
    ///
    /// FREE
    ///   The model is the user's — yaw, zoom, reposition, exactly as before. The
    ///   link is CUT both ways. Re-linking snaps home, re-opens the teardown and
    ///   re-selects whatever the data canvas is showing.
    ///
    /// ⚠ THE EXPLODE IS `DisassemblyAnimator`, NOT AN EXPLODE OF MY OWN.
    /// Thiago, 2026-08-07: *"use the animation in the disassembly intro to create
    /// the explode view."* He is right, and the first cut was wrong twice over:
    ///
    ///   * it computed directions from node positions, and `component3` and
    ///     `component4` sit at almost the same point, so they travelled the same
    ///     way and landed on each other;
    ///   * it required the payload map to resolve BEFORE anything could move, so a
    ///     single unmatched node name left the model completely frozen — which is
    ///     exactly what happened on device.
    ///
    /// The animator already has tuned, ordered, physically honest travel (screws
    /// out before the part they hold), it is the animation the participant has
    /// already seen in the intro, and it moves whether or not the passport data
    /// ever arrives. This class now only asks it for the open pose.
    ///
    /// ⚠ THE MAPPING IS NOT 1:1 AND LIVES IN THE PAYLOAD. `mesh_nodes` (schema
    /// v0.17) says which glTF nodes are which passport row: three `connector*`
    /// bodies are ONE row, three red `component4*` bodies are ONE row, the fourteen
    /// screws are a single `board_material` row. See `00` §8.1.
    ///
    /// ⚠ SCREWS ARE NOT SELECTABLE. `fasteners` is a board material and Component
    /// ID lists parts only, so a screw has no page to open. They still animate.
    ///
    /// ⚠ HIGHLIGHT = GHOSTING (round 6, Thiago 2026-08-09): non-selected bodies
    /// swap to TRANSPARENT fade-material twins — the technique the step-focus
    /// ghosting and the zone's part isolation already run on device — because a
    /// darker-tint dim read as "shadowed", not "de-emphasised". The SELECTED body
    /// keeps its exact real-life materials, because the point of
    /// `RBv2_1/Tools/Apply real-life colors` is that the model and the printed
    /// part in the user's hand read as one object. Device-build caveat: the fade
    /// shader (URP Lit / Standard) must be in Always Included Shaders.
    /// </summary>
    public class ModelLinkController : MonoBehaviour
    {
        [Header("Scene")]
        [Tooltip("The VCU clone whose children are the glTF nodes.")]
        [SerializeField] private Transform modelRoot;

        [Tooltip("The clone's own DisassemblyAnimator — the SAME animation the disassembly intro " +
                 "plays. It owns the open pose; this controller only asks for it.")]
        [SerializeField] private DPP.DisassemblyAnimator animator;

        [SerializeField] private ProductSpecsView productSpecs;
        [SerializeField] private SuperPanelView owner;

        [Tooltip("Index of the Product specs tab in the rail — a mesh pick switches to it.")]
        [SerializeField] private int productSpecsTab = 0;

        [Tooltip("Left null, it is found in the scene. Its OnPinch3D is the 3D pick hook " +
                 "PicoHandUIBridge has carried unused since RB2.0 (\"VCU mesh later\").")]
        [SerializeField] private PicoHandUIBridge handBridge;

        [Tooltip("RBv2.1.1 stage gestures. While BOTH hands pinch, the posture is a twist/zoom, " +
                 "not a selection — a pick that lands mid-gesture would yank the data canvas to " +
                 "another page while the user is just turning the model.")]
        [SerializeField] private TwoHandTwistRotate gestures;

        [Header("Fit")]
        [Tooltip("World metres of the CLOSED model's longest side at zoom 1.00 — the REAL device " +
                 "(the printed mock is 200 × 150 × 60 mm), so 1.00× reads as 1:1 with the unit on " +
                 "the participant's desk (device round 4, Thiago 2026-08-08). This replaced the " +
                 "open-envelope fit (0.26 → ÷1.2 → ÷1.5): guaranteeing stage containment of the " +
                 "EXPLODED pose made the closed model read as a toy.")]
        [SerializeField] private float realWorldSpan = 0.200f;

        [Tooltip("Highest DisassemblyAnimator step. 5 = fully torn down.")]
        [SerializeField] private int openStep = 5;

        [Header("Highlight")]
        [Tooltip("Round 6 (Thiago, 2026-08-09): non-selected bodies go TRANSPARENT (ghost), not " +
                 "darker — the same fade-material technique the step-focus ghosting and the zone's " +
                 "part isolation already use on device. This is that material's alpha.")]
        [SerializeField] private float ghostAlpha = 0.30f;

        // =================================================================

        private class Group
        {
            public string id;
            public bool selectable;                       // false for `fasteners`
            public readonly List<Renderer> renderers = new List<Renderer>();
            // Per renderer: the true material set, and its transparent ghost twin.
            public readonly List<Material[]> originals = new List<Material[]>();
            public readonly List<Material[]> ghosts = new List<Material[]>();
        }

        private readonly List<Group> _groups = new List<Group>();
        private readonly Dictionary<Collider, Group> _byCollider = new Dictionary<Collider, Group>();
        private readonly Dictionary<Material, Material> _ghostCache = new Dictionary<Material, Material>();
        private bool _built, _fitted, _pinchHooked, _linked = true;
        private bool _openDone;
        private Coroutine _openCo;

        /// <summary>Round 5 (Thiago, 2026-08-08): true once the entry teardown has
        /// finished (or an instant open ran). SuperPanelView gates the LINKED idle
        /// spin on this — the showcase rotation starts AFTER the animation, never
        /// during it.</summary>
        public bool OpenDone => _openDone;
        private string _selected;
        private int _retries;
        private float _retryAt;
        private const int MaxBuildRetries = 10;

        // ⚠ LORE THAT MUST SURVIVE THE GHOST REWRITE (round 6): glTFast's shaders
        // expose the base colour as `baseColorFactor`, not `_BaseColor` — an MPB
        // dim writing only the URP names silently did nothing on device (round 3).
        // The highlight is now a MATERIAL SWAP to transparent ghost copies (the
        // step-focus / zone-isolation technique), but the ghost's colour is still
        // READ from the source material — through all three property names, or a
        // glTFast source would produce a WHITE ghost. See ReadMatColor.

        /// <summary>Passport ids that are NOT parts and so have no page to open.</summary>
        private static readonly HashSet<string> NonSelectable = new HashSet<string> { "fasteners" };

        // =================================================================
        // Lifecycle
        // =================================================================

        private void OnEnable()
        {

            if (animator == null && modelRoot != null)
                animator = modelRoot.GetComponentInChildren<DPP.DisassemblyAnimator>(true);
            if (animator == null)
                Debug.LogWarning("[ModelLink] No DisassemblyAnimator on the stage model — it cannot open. " +
                                 "Re-run RBv2_1_1/1 (it used to strip the animator from the clone).");

            if (handBridge == null)
                handBridge = FindFirstObjectByType<PicoHandUIBridge>(FindObjectsInactive.Include);

            // A pinch that missed every canvas and hit a collider. PicoHandUIBridge has
            // raised this since RB2.0 with the comment "VCU mesh later"; this is later.
            //
            // ⚠ SUBSCRIBED ONCE, NEVER REMOVED, deliberately. RemoveListener needs the
            // SAME delegate instance, and holding one would mean naming PXR_Hand — a
            // PICO SDK type this file otherwise has no business knowing. The lambda's
            // parameter types are inferred instead, and HandlePick refuses a pick while
            // this component is disabled, which is what the removal was for.
            if (handBridge == null)
            {
                Debug.LogWarning("[ModelLink] No PicoHandUIBridge — the data canvas can still drive the " +
                                 "model, but pinching a body will do nothing.");
            }
            else if (!_pinchHooked)
            {
                handBridge.OnPinch3D.AddListener((hand, hit) => HandlePick(hit.collider));
                _pinchHooked = true;
            }

            TryBuild();

            // ⚠ OPEN THE MODEL WHETHER OR NOT THE MAP RESOLVED. The first cut gated the
            // motion on the payload, so one unmatched node name froze the model solid.
            // The animation is the participant's cue that the thing is alive; it must
            // not depend on a network fetch.
            PlayOpen();
        }

        // =================================================================
        // The open pose — the animator's, not mine
        // =================================================================

        /// <summary>Snap shut, then play the full teardown. Same routine as the intro.
        /// Sets <see cref="OpenDone"/> when the animation completes so the idle
        /// spin (round 5) starts after the show, not during it.</summary>
        public void PlayOpen()
        {
            if (animator == null) { _openDone = true; return; }
            EnsureFitted();
            if (_openCo != null) StopCoroutine(_openCo);
            _openCo = StartCoroutine(OpenRoutine());
        }

        private System.Collections.IEnumerator OpenRoutine()
        {
            _openDone = false;
            animator.StopAllCoroutines();   // a stray step tail keeps tweening otherwise
            animator.ResetInstant();
            yield return animator.RunFullTeardown();
            _openDone = true;
            _openCo = null;
        }

        /// <summary>
        /// Round 5: FREE shows the model CLOSED — the user floats the real-size
        /// assembled unit, as if holding the physical one. Animated reassembly;
        /// duration exposed so SuperPanelView can sequence the column expansion
        /// and the grab bar after it.
        /// </summary>
        public void PlayReassemble()
        {
            if (animator == null) return;
            if (_openCo != null) { StopCoroutine(_openCo); _openCo = null; }
            animator.StopAllCoroutines();   // unlock mid-teardown: kill the running step first
            animator.Reassemble();
            _openDone = false;              // not an open pose any more
        }

        public float ReassembleSeconds => animator != null ? animator.ReassembleDuration : 0f;

        /// <summary>The open pose with no animation — used for measuring, and for a re-link
        /// where a second full teardown would just make the user wait.</summary>
        private void OpenInstant()
        {
            if (animator == null) { _openDone = true; return; }
            if (_openCo != null) { StopCoroutine(_openCo); _openCo = null; }
            animator.StopAllCoroutines();
            animator.ResetInstant();
            for (int s = 1; s <= openStep; s++) animator.ApplyStepInstant(s);
            _openDone = true;
        }

        /// <summary>
        /// Fit ONCE: the CLOSED model's longest side becomes `realWorldSpan`, so
        /// zoom 1.00× is 1:1 with the physical unit (device round 4). The open
        /// pose is then whatever the teardown makes of a real-size model — the
        /// stage accepts it; containment is no longer the fit's job.
        /// </summary>
        private void EnsureFitted()
        {
            if (_fitted || animator == null || modelRoot == null) return;
            _fitted = true;

            animator.ResetInstant();
            float closed = WorldSpan();
            if (closed <= 1e-5f)
            {
                Debug.LogWarning("[ModelLink] Closed bounds unmeasurable — scale left alone.");
                return;
            }

            float k = realWorldSpan / closed;
            modelRoot.localScale *= k;

            // Re-centre for the OPEN pose (device round 3, 2026-08-08). RBv2_1_1/1
            // centres the CLOSED model on the pivot, but the teardown grows mostly
            // UPWARD — lid and screws rise, only the bottom shell drops — so the
            // exploded model rode high and clipped the stage frame's top edge on
            // device. Measure the open bounds at the fitted scale and slide the
            // clone so the OPEN pose is what sits on ModelHome; the closed pose
            // then rests slightly low, which is the pose the user sees least.
            OpenInstant();
            var rends = modelRoot.GetComponentsInChildren<Renderer>(true);
            if (rends.Length > 0)
            {
                Bounds b = rends[0].bounds;
                for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
                Vector3 centreLocal = transform.InverseTransformPoint(b.center);
                modelRoot.localPosition -= centreLocal;
            }
            float openSpan = WorldSpan();
            animator.ResetInstant();

            Debug.Log($"[ModelLink] Closed span {closed * 1000f:0.#} mm → ×{k:0.###} so zoom 1.00 is REAL SIZE " +
                      $"({realWorldSpan * 1000f:0} mm closed; open pose spans {openSpan * 1000f:0.#} mm).");
        }

        // =================================================================
        // Build — read the map out of the payload, never out of code
        // =================================================================

        private void TryBuild()
        {
            if (_built) return;
            var mgr = FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
            if (mgr != null && mgr.Latest != null) Build(mgr.Latest);
        }

        public void Build(DPPData data)
        {
            if (modelRoot == null)
            {
                Debug.LogError("[ModelLink] No modelRoot — run RBv2_1_1/1.");
                return;
            }
            if (data == null || data.components == null) return;

            _groups.Clear();
            _byCollider.Clear();

            // ⚠ INDEX EVERY DESCENDANT, not only the ones carrying a Renderer. A glTF
            // import often puts the mesh on a CHILD of the named node, so a
            // renderer-only index is keyed by names the payload has never heard of —
            // every lookup misses, no group is built, and nothing ever highlights.
            var byName = new Dictionary<string, Transform>();
            foreach (var t in modelRoot.GetComponentsInChildren<Transform>(true))
                if (!byName.ContainsKey(t.name)) byName[t.name] = t;

            int claimed = 0, missed = 0;
            foreach (var c in data.components)
            {
                if (c == null || c.mesh_nodes == null || c.mesh_nodes.Count == 0) continue;

                var g = new Group { id = c.id, selectable = !NonSelectable.Contains(c.id) };
                foreach (var nodeName in c.mesh_nodes)
                {
                    if (!byName.TryGetValue(nodeName, out var t))
                    {
                        missed++;
                        Debug.LogWarning($"[ModelLink] Payload names '{nodeName}' for {c.id}, but the stage " +
                                         "model has no such transform. Check 00 §8.1 against VCU_assembly.gltf.");
                        continue;
                    }
                    claimed++;

                    foreach (var r in t.GetComponentsInChildren<Renderer>(true))
                    {
                        g.renderers.Add(r);
                        var mats = r.sharedMaterials;
                        g.originals.Add(mats);
                        var ghost = new Material[mats.Length];
                        for (int m = 0; m < mats.Length; m++)
                            ghost[m] = mats[m] == null ? null : GhostOf(mats[m]);
                        g.ghosts.Add(ghost);
                    }

                    if (!g.selectable) continue;

                    // A pinch needs something to hit.
                    //
                    // ⚠ RBv2_1_1/1 USED TO DISABLE every collider on the stage clone —
                    // they were dead weight when the stage was a spinning picture. A
                    // disabled collider registers here perfectly well and then silently
                    // never raycasts, which reads as the pick logic being broken. So:
                    // re-enable what is there, and only add one when there is nothing.
                    var existing = t.GetComponentsInChildren<Collider>(true);
                    if (existing.Length > 0)
                    {
                        foreach (var col in existing)
                        {
                            col.enabled = true;
                            _byCollider[col] = g;
                        }
                    }
                    else
                    {
                        var mf = t.GetComponentInChildren<MeshFilter>(true);
                        if (mf != null && mf.sharedMesh != null)
                        {
                            var mc = mf.gameObject.AddComponent<MeshCollider>();
                            mc.sharedMesh = mf.sharedMesh;
                            _byCollider[mc] = g;
                        }
                        else Debug.LogWarning($"[ModelLink] '{nodeName}' has no mesh to collide with — " +
                                              "it will draw but can never be picked.");
                    }
                }
                if (g.renderers.Count > 0) _groups.Add(g);
            }

            _built = _groups.Count > 0;
            Debug.Log($"[ModelLink] {_groups.Count} groups, {claimed} nodes claimed, {missed} missing, " +
                      $"{_byCollider.Count} pickable colliders.");
            if (!_built)
                Debug.LogWarning("[ModelLink] No group resolved. The model will still open and can still be " +
                                 "moved; it just will not highlight or respond to a pinch.");
            ApplyGhosting();
        }

        // =================================================================
        // Update — bounded retry + the highlight ease
        // =================================================================

        private void Update()
        {
            // The payload is fetched asynchronously and routinely loses the race with
            // OnEnable. Retry, BOUNDED: a model that never maps must say so once and
            // then stop, not log every frame for the rest of the session.
            if (!_built && _retries < MaxBuildRetries)
            {
                _retryAt -= Time.deltaTime;
                if (_retryAt <= 0f)
                {
                    _retryAt = 1f;
                    _retries++;
                    TryBuild();
                    if (!_built && _retries >= MaxBuildRetries)
                        Debug.LogWarning($"[ModelLink] Still unmapped after {MaxBuildRetries} tries.");
                }
            }

        }

        // =================================================================
        // Selection — one piece of state, driven from either side
        // =================================================================

        /// <summary>Called by ProductSpecsView when a component page opens.</summary>
        public void SelectComponent(string componentId)
        {
            // Deliberate breadcrumb: the canvas→model direction failed SILENTLY on
            // device (no dim, no error). One line per selection makes the next
            // round diagnosable from the logcat alone.
            Debug.Log($"[ModelLink] SelectComponent('{componentId}') — built={_built}, groups={_groups.Count}, linked={_linked}.");
            SetSelected(componentId, instant: false);
        }

        /// <summary>Called when the data canvas is showing no single component.</summary>
        public void ClearSelection() => SetSelected(null, instant: false);

        private void SetSelected(string id, bool instant)
        {
            _selected = id;
            ApplyGhosting();
        }

        /// <summary>
        /// GHOST everything that is not selected (round 6): non-selected groups
        /// swap to transparent fade-material twins; the selected group — and
        /// every group, when nothing is selected or the link is cut — is written
        /// back to its TRUE materials, so a body can never keep a stale ghost.
        /// Material swap, not MPB: alpha needs a transparent queue, which an
        /// opaque material ignores whatever an MPB says. Device-build caveat
        /// (spec 10 §7): the fade shader must be in Always Included Shaders.
        /// </summary>
        private void ApplyGhosting()
        {
            bool any = !string.IsNullOrEmpty(_selected) && _linked;
            foreach (var g in _groups)
            {
                bool ghost = any && g.id != _selected;
                for (int i = 0; i < g.renderers.Count; i++)
                {
                    var r = g.renderers[i];
                    if (r == null) continue;
                    r.sharedMaterials = ghost ? g.ghosts[i] : g.originals[i];
                }
            }
        }

        /// <summary>
        /// Transparent twin of a material, cached. CreateFadeMaterial reads only
        /// the URP/Standard colour names, and a glTFast source keeps its colour in
        /// `baseColorFactor` — so the colour is re-read here through ALL names and
        /// written onto the fade copy, or ghosts of glTF materials come out white.
        /// </summary>
        private Material GhostOf(Material src)
        {
            if (_ghostCache.TryGetValue(src, out var cached)) return cached;

            var m = DPP.DisassemblyAnimator.CreateFadeMaterial(src, ghostAlpha);
            Color c = ReadMatColor(src);
            c.a = ghostAlpha;
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
            if (m.HasProperty("_Color")) m.color = c;

            _ghostCache[src] = m;
            return m;
        }

        // =================================================================
        // The model side of the link
        // =================================================================

        public void HandlePick(Collider col)
        {
            if (!isActiveAndEnabled || !_linked || col == null) return;
            // Two hands pinching = a twist/zoom posture, not a selection. Without
            // this, starting a rotation with the ray over a body opened that
            // body's page mid-gesture. (The first pinch of a two-hand gesture can
            // still slip through — device-tune item, same family as zone §6.4.)
            if (gestures != null && gestures.LeftPinching && gestures.RightPinching) return;
            if (!_byCollider.TryGetValue(col, out var g)) return;      // screws land here and stop
            if (!g.selectable || productSpecs == null) return;
            Debug.Log($"[ModelLink] Pick → '{g.id}'.");

            // The rail may be on another tab. Switch first, then open — the other order
            // opens a page the user cannot see.
            if (owner != null) owner.SelectTab(productSpecsTab);
            if (!productSpecs.OpenComponentById(g.id))
                Debug.LogWarning($"[ModelLink] Picked '{g.id}' but Product specs has no such part row.");
        }

        // =================================================================
        // Linked / free
        // =================================================================

        /// <summary>
        /// Called by SuperPanelView. FREE cuts the link BOTH ways: no highlight follows
        /// the data canvas and no pinch on a body navigates — the model is simply the
        /// object it was before the link existed. Re-linking restates it, so LINKED
        /// always means one thing however the user left it.
        ///
        /// A re-link opens INSTANTLY rather than replaying the teardown: the user has
        /// already watched it once, and a second five-step animation between them and
        /// the data they were reading is a toll, not a flourish.
        /// </summary>
        public void SetLinked(bool linked)
        {
            _linked = linked;
            if (linked)
            {
                EnsureFitted();
                OpenInstant();
            }
            SetSelected(_selected, instant: false);
        }

        // =================================================================
        // Helpers
        // =================================================================

        private float WorldSpan()
        {
            var rends = modelRoot.GetComponentsInChildren<Renderer>(true);
            if (rends.Length == 0) return 0f;
            Bounds b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
            return Mathf.Max(b.size.x, Mathf.Max(b.size.y, b.size.z));
        }

        private static Color ReadMatColor(Material m)
        {
            if (m == null) return Color.white;
            if (m.HasProperty("_BaseColor")) return m.GetColor("_BaseColor");
            if (m.HasProperty("_Color")) return m.GetColor("_Color");
            if (m.HasProperty("baseColorFactor")) return m.GetColor("baseColorFactor");   // glTFast
            return Color.white;
        }
    }
}
