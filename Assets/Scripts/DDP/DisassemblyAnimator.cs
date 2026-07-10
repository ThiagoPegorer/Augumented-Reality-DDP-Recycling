using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DPP
{
    /// <summary>
    /// Step-based teardown animator for the imported VCU_assembly glTF.
    /// Attach to the VCU_assembly root (its children are the named parts).
    ///
    /// Drives BOTH outputs from one motion set:
    ///   - per-task GIFs  (call PlayTask(step, task), record the Game view)
    ///   - interactive explode  (PlayFullTeardown / Reassemble)
    ///   - step-driven panel  (PlayStep(n) from StepFlowController)
    ///   - intro preview loop  (TeardownPreviewLoop yields RunFullTeardown)
    ///
    /// v2 (2026-07-10, motion review):
    ///   - CONNECTOR AXIS IS AUTO-DETECTED: the horizontal direction from the
    ///     device centre to the connectors, snapped to the dominant local axis.
    ///     Connectors + their screws travel OUT of the front face — no more
    ///     guessed +X. (Disable autoConnectorAxis to use manualConnectorAxis.)
    ///   - Lid screws rise clearly ABOVE the lifted lid (lidScrewRise > lidRise).
    ///   - Step 3 lifts the PCB WITH the chips riding on it; step 4 then pops
    ///     the chips from the raised board.
    ///   - Step 5 drops the bottom shell straight DOWN (was sideways).
    ///
    /// Model imported at real scale (~0.2 m), glTF Y-up. All distances are
    /// LOCAL-space metres — tune in the Inspector. Requires DOTween.
    /// </summary>
    public class DisassemblyAnimator : MonoBehaviour
    {
        [Header("Connector axis")]
        [Tooltip("Detect the connector face direction from part positions (recommended).")]
        [SerializeField] private bool autoConnectorAxis = true;
        [Tooltip("Used only when auto-detect is off. Local-space direction out of the connector face.")]
        [SerializeField] private Vector3 manualConnectorAxis = new Vector3(0f, 0f, -1f);

        [Header("Distances — local metres (tune in Scene view)")]
        [SerializeField] private float lidScrewRise      = 0.20f;  // above the lifted lid
        [SerializeField] private float lidRise           = 0.14f;  // clear of the popped chips (~0.10)
        [SerializeField] private float connectorScrewDist = 0.11f; // along the connector axis, beyond the pulled connectors
        [SerializeField] private float connectorDist     = 0.09f;  // along the connector axis
        [SerializeField] private float pcbScrewRise      = 0.08f;
        [SerializeField] private float pcbRise           = 0.06f;  // board + chips together
        [SerializeField] private float chipRise          = 0.035f; // chips off the raised board
        [SerializeField] private float shellDrop         = 0.08f;  // bottom shell straight down

        [Header("Step focus (how-to highlight)")]
        [Tooltip("Opacity of parts NOT relevant to the focused step (0.1 = 10%).")]
        [Range(0.02f, 1f)]
        [SerializeField] private float fadedAlpha = 0.1f;

        [Header("Timing")]
        [SerializeField] private float screwDur   = 1.0f;
        [SerializeField] private float partDur    = 1.2f;
        [SerializeField] private float chipDur    = 0.9f;
        [SerializeField] private float chipStagger = 0.08f;
        [SerializeField] private float spinDegrees = 720f;
        [SerializeField] private float pulseScale = 1.15f;
        [SerializeField] private float pulseDur   = 0.6f;

        // --- discovered part groups ---
        private Transform lid, bottomShell, pcb;
        private readonly List<Transform> connectors      = new List<Transform>();
        private readonly List<Transform> chips           = new List<Transform>();
        private readonly List<Transform> lidScrews       = new List<Transform>();
        private readonly List<Transform> connectorScrews = new List<Transform>();
        private readonly List<Transform> pcbScrews       = new List<Transform>();

        private struct Home { public Vector3 pos; public Quaternion rot; public Vector3 scale; }
        private readonly Dictionary<Transform, Home> home = new Dictionary<Transform, Home>();

        // Mesh centre of each part in ROOT-local space at the assembled pose.
        // Screw pivots from the glTF are not guaranteed to sit on the screw's
        // centerline — spinning about the pivot then makes the screw ORBIT
        // (looks like flipping). We spin about the axis through the mesh centre
        // instead, which lies on the centerline of any screw-shaped mesh.
        private readonly Dictionary<Transform, Vector3> meshCenter = new Dictionary<Transform, Vector3>();

        private Vector3 _connectorAxisLocal = new Vector3(0f, 0f, -1f);
        private float _chipLift; // how far the chips were carried up by the PCB (step 3)

        // --- step-focus material state ---
        private readonly Dictionary<Renderer, Material[]> _origMats = new Dictionary<Renderer, Material[]>();
        private readonly Dictionary<Material, Material> _fadeCache = new Dictionary<Material, Material>();

        /// <summary>World-space direction the connector face points at —
        /// the device's "front". Used by TeardownPreviewLoop to frame the camera.</summary>
        public Vector3 ConnectorAxisWorld => transform.TransformDirection(_connectorAxisLocal);

        void Awake()
        {
            foreach (Transform c in transform)
            {
                string n = c.name;
                if      (StartsWith(n, "screws_housing"))   lidScrews.Add(c);
                else if (StartsWith(n, "screws_connector")) connectorScrews.Add(c);
                else if (StartsWith(n, "pcb_screw"))         pcbScrews.Add(c);   // matches "PCB_screw*"
                else if (StartsWith(n, "component"))         chips.Add(c);
                else if (StartsWith(n, "connector"))         connectors.Add(c);
                else if (Equals(n, "housing_upper"))         lid = c;
                else if (Equals(n, "housing_bottom"))        bottomShell = c;
                else if (Equals(n, "pcb"))                   pcb = c;

                if (!home.ContainsKey(c))
                    home[c] = new Home { pos = c.localPosition, rot = c.localRotation, scale = c.localScale };

                var rend = c.GetComponentInChildren<Renderer>();
                if (rend != null && !meshCenter.ContainsKey(c))
                    meshCenter[c] = transform.InverseTransformPoint(rend.bounds.center);
            }

            foreach (var r in GetComponentsInChildren<Renderer>(true))
                _origMats[r] = r.sharedMaterials;

            _connectorAxisLocal = autoConnectorAxis ? DetectConnectorAxis() : manualConnectorAxis.normalized;
        }

        /// <summary>Bore axis of the connectors (v2): the three connectors form a
        /// horizontal ROW along the wall — the bore is the horizontal direction
        /// PERPENDICULAR to that row, signed to point away from the device centre.
        /// (v1 used the offset-from-centre direction, which glTF pivot placement
        /// could rotate 90° into the row direction — connectors then slid sideways.)</summary>
        private Vector3 DetectConnectorAxis()
        {
            if (connectors.Count < 2) return manualConnectorAxis.normalized;

            // Row direction: the longest span between any two connectors.
            Vector3 row = Vector3.zero;
            float best = 0f;
            for (int i = 0; i < connectors.Count; i++)
                for (int j = i + 1; j < connectors.Count; j++)
                {
                    Vector3 d = home[connectors[j]].pos - home[connectors[i]].pos;
                    d.y = 0f;
                    if (d.sqrMagnitude > best) { best = d.sqrMagnitude; row = d; }
                }
            if (best < 1e-8f)
            {
                Debug.LogWarning("[DisassemblyAnimator] Connector row degenerate — using manual axis.");
                return manualConnectorAxis.normalized;
            }

            Vector3 bore = Vector3.Cross(Vector3.up, row.normalized); // horizontal ⊥ row

            // Sign: point AWAY from the device centre (PCB, else mean of parts).
            Vector3 center;
            if (pcb != null && home.ContainsKey(pcb)) center = home[pcb].pos;
            else
            {
                center = Vector3.zero;
                foreach (var kv in home) center += kv.Value.pos;
                center /= home.Count;
            }
            Vector3 avg = Vector3.zero;
            foreach (var c in connectors) avg += home[c].pos;
            avg /= connectors.Count;

            float sign = Vector3.Dot(avg - center, bore);
            if (sign < 0f) bore = -bore;

            // Snap to the dominant local axis (model is axis-aligned).
            Vector3 axis = Mathf.Abs(bore.x) >= Mathf.Abs(bore.z)
                ? new Vector3(Mathf.Sign(bore.x), 0f, 0f)
                : new Vector3(0f, 0f, Mathf.Sign(bore.z));
            Debug.Log($"[DisassemblyAnimator] Connector bore axis auto-detected: local {axis} (row {row.normalized}).");
            return axis;
        }

        static bool StartsWith(string a, string b) => a.StartsWith(b, System.StringComparison.OrdinalIgnoreCase);
        static bool Equals(string a, string b)     => a.Equals(b, System.StringComparison.OrdinalIgnoreCase);

        // ---------- low-level helpers ----------
        private Tween MoveTo(Transform t, Vector3 offset, float dur, Ease ease)
        {
            if (t == null || !home.ContainsKey(t)) return null;
            return t.DOLocalMove(home[t].pos + offset, dur).SetEase(ease);
        }

        private float MoveGroup(List<Transform> list, Vector3 offset, float dur, Ease ease)
        {
            foreach (var t in list) MoveTo(t, offset, dur, ease);
            return dur;
        }

        /// <summary>True screw twist + back-out in ONE tween: rotates the part about
        /// the bore axis THROUGH ITS OWN MESH CENTRE (pivot-independent — off-axis
        /// glTF pivots would otherwise make the screw orbit/flip), while easing the
        /// translation along the same axis.</summary>
        private float Unscrew(List<Transform> screws, Vector3 offset, float dur)
        {
            Vector3 axis = offset.sqrMagnitude > 1e-6f ? offset.normalized : Vector3.up;
            foreach (var s in screws)
            {
                if (s == null || !home.ContainsKey(s)) continue;
                Home h = home[s];
                Vector3 c = meshCenter.TryGetValue(s, out var mc) ? mc : h.pos;

                var screw = s; // capture
                DOTween.To(() => 0f, u =>
                {
                    Quaternion R = Quaternion.AngleAxis(spinDegrees * u, axis);
                    float move = u * u; // InQuad ease on the back-out
                    screw.localRotation = R * h.rot;
                    screw.localPosition = c + R * (h.pos - c) + offset * move;
                }, 1f, dur)
                .SetEase(Ease.Linear)   // constant spin; move eased inside
                .SetTarget(s);          // so ResetInstant's DOTween.Kill stops it
            }
            return dur;
        }

        private float PulseChips()
        {
            foreach (var c in chips)
            {
                if (c == null || !home.ContainsKey(c)) continue;
                c.DOScale(home[c].scale * pulseScale, pulseDur * 0.5f)
                 .SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
            }
            return pulseDur;
        }

        /// <summary>Step 3.2 — the PCB rises WITH the chips riding on it.</summary>
        private float LiftBoardWithChips()
        {
            Vector3 rise = Vector3.up * pcbRise;
            MoveTo(pcb, rise, partDur, Ease.OutQuad);
            foreach (var c in chips) MoveTo(c, rise, partDur, Ease.OutQuad);
            _chipLift = pcbRise;
            return pcb != null ? partDur : 0f;
        }

        /// <summary>Step 4.2 — chips pop up off the (raised) board, staggered.</summary>
        private float PopChips()
        {
            Vector3 target = Vector3.up * (_chipLift + chipRise);
            for (int i = 0; i < chips.Count; i++)
            {
                var c = chips[i];
                if (c == null || !home.ContainsKey(c)) continue;
                c.DOLocalMove(home[c].pos + target, chipDur).SetEase(Ease.OutBack).SetDelay(i * chipStagger);
            }
            return chipDur + Mathf.Max(0, chips.Count - 1) * chipStagger;
        }

        // ---------- one task = one GIF. Returns its duration. ----------
        public float PlayTask(int step, int task)
        {
            Vector3 conn = _connectorAxisLocal;
            switch (step)
            {
                case 1: return task == 1 ? Unscrew(lidScrews, Vector3.up * lidScrewRise, screwDur)
                                         : MoveTo(lid, Vector3.up * lidRise, partDur, Ease.OutQuad) != null ? partDur : 0f;
                case 2: return task == 1 ? Unscrew(connectorScrews, conn * connectorScrewDist, screwDur)
                                         : MoveGroup(connectors, conn * connectorDist, partDur, Ease.OutQuad);
                case 3: return task == 1 ? Unscrew(pcbScrews, Vector3.up * pcbScrewRise, screwDur)
                                         : LiftBoardWithChips();
                case 4: return task == 1 ? PulseChips() : PopChips();
                case 5: return task == 1 ? (MoveTo(bottomShell, Vector3.down * shellDrop, partDur, Ease.InOutQuad) != null ? partDur : 0f)
                                         : 0f; // 5.2 labels/misc — no mesh, caption only
                default: return 0f;
            }
        }

        // ---------- sequencing ----------
        public void PlayStep(int step) => StartCoroutine(StepRoutine(step));

        private IEnumerator StepRoutine(int step)
        {
            float d1 = PlayTask(step, 1);
            yield return new WaitForSeconds(d1 + 0.15f);
            float d2 = PlayTask(step, 2);
            yield return new WaitForSeconds(d2);
        }

        /// <summary>Awaitable single step for external drivers (how-to loop):
        /// <c>yield return animator.RunStep(n);</c></summary>
        public IEnumerator RunStep(int step) => StepRoutine(step);

        /// <summary>Snap the model to the END state of the given step, no animation.
        /// Call ascending after ResetInstant() to precondition the model for a later
        /// step's how-to loop (parts removed in earlier steps are already out).</summary>
        public void ApplyStepInstant(int step)
        {
            switch (step)
            {
                case 1:
                    SnapGroup(lidScrews, Vector3.up * lidScrewRise);
                    Snap(lid, Vector3.up * lidRise);
                    break;
                case 2:
                    SnapGroup(connectorScrews, _connectorAxisLocal * connectorScrewDist);
                    SnapGroup(connectors, _connectorAxisLocal * connectorDist);
                    break;
                case 3:
                    SnapGroup(pcbScrews, Vector3.up * pcbScrewRise);
                    Snap(pcb, Vector3.up * pcbRise);
                    SnapGroup(chips, Vector3.up * pcbRise);
                    _chipLift = pcbRise;
                    break;
                case 4:
                    SnapGroup(chips, Vector3.up * (_chipLift + chipRise));
                    break;
                case 5:
                    Snap(bottomShell, Vector3.down * shellDrop);
                    break;
            }
        }

        private void Snap(Transform t, Vector3 offset)
        {
            if (t == null || !home.ContainsKey(t)) return;
            DOTween.Kill(t);
            t.localPosition = home[t].pos + offset;
        }

        private void SnapGroup(List<Transform> list, Vector3 offset)
        {
            foreach (var t in list) Snap(t, offset);
        }

        public void PlayFullTeardown() => StartCoroutine(FullRoutine());

        /// <summary>Awaitable full teardown for external drivers (e.g. the intro
        /// preview loop): <c>yield return animator.RunFullTeardown();</c></summary>
        public IEnumerator RunFullTeardown() => FullRoutine();

        /// <summary>How long Reassemble() takes — lets callers wait it out.</summary>
        public float ReassembleDuration => partDur;

        private IEnumerator FullRoutine()
        {
            for (int s = 1; s <= 5; s++)
                yield return StartCoroutine(StepRoutine(s));
        }

        /// <summary>Animate everything back to the assembled pose.</summary>
        public void Reassemble()
        {
            _chipLift = 0f;
            foreach (var kv in home)
            {
                var t = kv.Key; var h = kv.Value;
                t.DOLocalMove(h.pos, partDur).SetEase(Ease.InOutQuad);
                t.DOLocalRotateQuaternion(h.rot, partDur);
                t.DOScale(h.scale, partDur);
            }
        }

        /// <summary>Snap back to assembled instantly (use between GIF-capture loops).</summary>
        public void ResetInstant()
        {
            _chipLift = 0f;
            foreach (var kv in home)
            {
                DOTween.Kill(kv.Key);
                kv.Key.localPosition = kv.Value.pos;
                kv.Key.localRotation = kv.Value.rot;
                kv.Key.localScale    = kv.Value.scale;
            }
        }

        // ---------- step focus: ghost non-relevant parts ----------

        /// <summary>Fades every part NOT involved in the given step to fadedAlpha,
        /// so the how-to animation highlights only the relevant components.
        /// Parts removed in earlier steps ghost too (visible as history).</summary>
        public void SetStepFocus(int step)
        {
            var keep = new HashSet<Transform>();
            switch (step)
            {
                case 1: keep.UnionWith(lidScrews); if (lid != null) keep.Add(lid); break;
                case 2: keep.UnionWith(connectorScrews); keep.UnionWith(connectors); break;
                case 3: keep.UnionWith(pcbScrews); if (pcb != null) keep.Add(pcb); keep.UnionWith(chips); break;
                case 4: keep.UnionWith(chips); break;
                case 5: if (bottomShell != null) keep.Add(bottomShell); break; // lid already off & sorted — bottom shell only
                default: ClearFocus(); return;
            }

            foreach (var kv in _origMats)
            {
                var r = kv.Key;
                if (r == null) continue;
                r.sharedMaterials = IsUnderAny(r.transform, keep) ? kv.Value : FadedVersions(kv.Value);
            }
        }

        /// <summary>Restores every part's original materials.</summary>
        public void ClearFocus()
        {
            foreach (var kv in _origMats)
                if (kv.Key != null) kv.Key.sharedMaterials = kv.Value;
        }

        private bool IsUnderAny(Transform t, HashSet<Transform> roots)
        {
            for (var p = t; p != null && p != transform; p = p.parent)
                if (roots.Contains(p)) return true;
            return false;
        }

        private Material[] FadedVersions(Material[] mats)
        {
            var faded = new Material[mats.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                var src = mats[i];
                if (src == null) { faded[i] = null; continue; }
                if (!_fadeCache.TryGetValue(src, out var m))
                {
                    m = CreateFadeMaterial(src, fadedAlpha);
                    _fadeCache[src] = m;
                }
                faded[i] = m;
            }
            return faded;
        }

        /// <summary>Transparent ghost copy of a material. Tries URP Lit, falls back
        /// to built-in Standard (Fade). NOTE for device builds: whichever shader is
        /// used must be in Always Included Shaders, or ghosts render magenta/missing.</summary>
        private static Material CreateFadeMaterial(Material src, float alpha)
        {
            Color baseCol = src.HasProperty("_BaseColor") ? src.GetColor("_BaseColor")
                          : src.HasProperty("_Color") ? src.color : Color.white;
            baseCol.a = alpha;

            Shader urp = Shader.Find("Universal Render Pipeline/Lit");
            Material m;
            if (urp != null)
            {
                m = new Material(urp);
                m.SetFloat("_Surface", 1f); // transparent
                m.SetOverrideTag("RenderType", "Transparent");
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0);
                m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                m.SetColor("_BaseColor", baseCol);
                if (src.HasProperty("_BaseMap") && m.HasProperty("_BaseMap"))
                    m.SetTexture("_BaseMap", src.GetTexture("_BaseMap"));
            }
            else
            {
                m = new Material(Shader.Find("Standard"));
                m.SetFloat("_Mode", 2f); // Fade
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0);
                m.EnableKeyword("_ALPHABLEND_ON");
                m.renderQueue = 3000;
                m.color = baseCol;
            }
            m.name = src.name + " (ghost)";
            return m;
        }

        // ---------- editor test buttons (enter Play mode first) ----------
        [ContextMenu("Test / Full teardown")] void _tFull()  => PlayFullTeardown();
        [ContextMenu("Test / Reassemble")]    void _tBack()  => Reassemble();
        [ContextMenu("Test / Step 1")] void _t1() => PlayStep(1);
        [ContextMenu("Test / Step 2")] void _t2() => PlayStep(2);
        [ContextMenu("Test / Step 3")] void _t3() => PlayStep(3);
        [ContextMenu("Test / Step 4")] void _t4() => PlayStep(4);
        [ContextMenu("Test / Step 5")] void _t5() => PlayStep(5);
    }
}
