using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace DPP
{
    /// <summary>
    /// The interactive "action zone" model (exploded canvas v2, 2026-07-16).
    ///
    /// A runtime CLONE of VCU_assembly restructured into draggable BODIES:
    /// each body slides along its real extraction axis up to its real travel
    /// distance (mirroring DisassemblyAnimator's tuned values), respects the
    /// disassembly dependencies, snaps to the nearer end on release, and
    /// screws spin proportionally to their travel. Screw sets and the three
    /// connectors act as single grouped bodies; chips ride the PCB (their
    /// body containers are nested under the PCB body) and become individually
    /// draggable once the PCB is out. The bottom shell is selectable as a
    /// zoom reference but never draggable.
    ///
    /// Created via CreateFrom() by ExplodedZoneInteraction. Self-contained:
    /// detects parts by name and the connector bore axis by geometry, exactly
    /// like DisassemblyAnimator.
    /// </summary>
    public class ConstrainedTeardownModel : MonoBehaviour
    {
        public class Body
        {
            public string name;
            public string displayName;           // shown in the hover label
            public Transform container;          // moved when dragging
            public Vector3 axisLocal;            // in the container's PARENT frame
            public float maxTravel;              // metres, model space
            public float travel;                 // current, 0..maxTravel
            public bool spin;                    // screw twist with travel
            public bool draggable = true;
            public bool referenceOnly;           // bottom shell: select for zoom only
            public readonly List<Body> dependencies = new List<Body>();

            public Vector3 homeLocalPos;
            public readonly List<Collider> colliders = new List<Collider>();  // tight, per part mesh
            public Renderer[] renderers;
            public Color[] baseColors;

            // per-member spin data (screws)
            public Transform[] members;
            public Vector3[] memberHomePos;
            public Quaternion[] memberHomeRot;
            public Vector3[] memberCenter;       // in container space

            public bool Extracted => travel >= maxTravel * 0.999f;
        }

        [Header("Feel")]
        [SerializeField] private float snapDuration = 0.28f;
        [SerializeField] private float spinDegrees = 720f;
        [SerializeField] private float lockedFlashSeconds = 0.35f;

        private static readonly Color LockedTint = new Color(0.886f, 0.294f, 0.29f);     // #e24b4a

        private readonly List<Body> _bodies = new List<Body>();
        private readonly Dictionary<Collider, Body> _colliderMap = new Dictionary<Collider, Body>();
        private MaterialPropertyBlock _mpb;

        public IReadOnlyList<Body> Bodies => _bodies;

        // =================================================================
        // Factory
        // =================================================================

        /// <summary>Clones the source model under `parent` (localPosition zero)
        /// and restructures it into constrained bodies.</summary>
        public static ConstrainedTeardownModel CreateFrom(Transform source, Transform parent)
        {
            // The how-to loop may have step-focus ghost materials applied to the
            // original RIGHT NOW (it enables before the zone does). Clear them so
            // the clone copies clean materials, then restore the focus.
            var srcAnimator = source.GetComponent<DisassemblyAnimator>();
            int focus = srcAnimator != null ? srcAnimator.FocusStep : 0;
            if (srcAnimator != null && focus > 0) srcAnimator.ClearFocus();

            GameObject clone = Instantiate(source.gameObject, parent, false);

            if (srcAnimator != null && focus > 0) srcAnimator.SetStepFocus(focus);
            clone.name = "VCU_ZoneModel";
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = Vector3.one;

            // The clone must not run the instructional animator, and must live
            // on the Default layer (the original sits on the preview-only layer).
            var animator = clone.GetComponent<DisassemblyAnimator>();
            if (animator != null) Destroy(animator);
            SetLayerRecursive(clone.transform, 0);

            var model = clone.AddComponent<ConstrainedTeardownModel>();
            model.Build();
            return model;
        }

        private static void SetLayerRecursive(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            foreach (Transform c in t) SetLayerRecursive(c, layer);
        }

        // =================================================================
        // Build: discover parts, detect axis, group bodies, add colliders
        // =================================================================

        private void Build()
        {
            _mpb = new MaterialPropertyBlock();

            var lidScrews = new List<Transform>();
            var connScrews = new List<Transform>();
            var pcbScrews = new List<Transform>();
            var chips = new List<Transform>();
            var connectors = new List<Transform>();
            Transform lid = null, shell = null, pcb = null;

            foreach (Transform c in transform)
            {
                string n = c.name;
                if      (Starts(n, "screws_housing"))   lidScrews.Add(c);
                else if (Starts(n, "screws_connector")) connScrews.Add(c);
                else if (Starts(n, "pcb_screw"))         pcbScrews.Add(c);
                else if (Starts(n, "component"))         chips.Add(c);
                else if (Starts(n, "connector"))         connectors.Add(c);
                else if (Same(n, "housing_upper"))       lid = c;
                else if (Same(n, "housing_bottom"))      shell = c;
                else if (Same(n, "pcb"))                 pcb = c;
            }

            Vector3 bore = DetectBoreAxis(connectors, pcb);

            // Travel distances mirror DisassemblyAnimator's tuned values.
            Body bLidScrews = MakeGroupBody("LidScrewsBody", lidScrews, Vector3.up, 0.14f, spin: true);
            Body bLid       = MakeSingleBody("LidBody", lid, Vector3.up, 0.14f);
            Body bConnScrews = MakeGroupBody("ConnScrewsBody", connScrews, bore, 0.11f, spin: true);
            Body bConnectors = MakeGroupBody("ConnectorsBody", connectors, bore, 0.09f, spin: false);
            Body bPcbScrews = MakeGroupBody("PcbScrewsBody", pcbScrews, Vector3.up, 0.08f, spin: true);
            Body bPcb       = MakeSingleBody("PcbBody", pcb, Vector3.up, 0.06f);
            Body bShell     = MakeSingleBody("ShellBody", shell, Vector3.up, 0f);
            if (bShell != null) { bShell.draggable = false; bShell.referenceOnly = true; }

            SetDisplay(bLidScrews, "Lid screws");
            SetDisplay(bLid, "Upper housing");
            SetDisplay(bConnScrews, "Connector screws");
            SetDisplay(bConnectors, "Connectors");
            SetDisplay(bPcbScrews, "Board screws");
            SetDisplay(bPcb, "PCB");
            SetDisplay(bShell, "Lower housing");

            // Chips: nested under the PCB body so they ride it; individually draggable.
            var chipBodies = new List<Body>();
            if (bPcb != null)
            {
                for (int i = 0; i < chips.Count; i++)
                {
                    Body bc = MakeSingleBody($"ChipBody{i + 1}", chips[i], Vector3.up, 0.05f, parentOverride: bPcb.container);
                    if (bc != null) { SetDisplay(bc, $"Chip {i + 1}"); chipBodies.Add(bc); }
                }
            }

            // Dependencies (physical truth): screws free their part; the lid
            // gates the interior; the PCB gates the chips.
            AddDep(bLid, bLidScrews);
            AddDep(bConnectors, bConnScrews);
            AddDep(bPcbScrews, bLid);
            AddDep(bPcb, bLid);
            AddDep(bPcb, bPcbScrews);
            foreach (var bc in chipBodies) AddDep(bc, bPcb);
        }

        private static void AddDep(Body body, Body dep)
        {
            if (body != null && dep != null) body.dependencies.Add(dep);
        }

        private static void SetDisplay(Body b, string label)
        {
            if (b != null) b.displayName = label;
        }

        private Body MakeGroupBody(string name, List<Transform> members, Vector3 axis, float travel, bool spin)
        {
            if (members == null || members.Count == 0) return null;
            var container = NewContainer(name, transform);
            foreach (var m in members) m.SetParent(container, true);
            return FinishBody(name, container, members.ToArray(), axis, travel, spin);
        }

        private Body MakeSingleBody(string name, Transform part, Vector3 axis, float travel, Transform parentOverride = null)
        {
            if (part == null) return null;
            var container = NewContainer(name, parentOverride != null ? parentOverride : transform);
            part.SetParent(container, true);
            return FinishBody(name, container, new[] { part }, axis, travel, spin: false);
        }

        private static Transform NewContainer(string name, Transform parent)
        {
            var go = new GameObject(name);
            var t = go.transform;
            t.SetParent(parent, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.gameObject.layer;
            return t;
        }

        private Body FinishBody(string name, Transform container, Transform[] members,
            Vector3 axis, float travel, bool spin)
        {
            var b = new Body
            {
                name = name,
                container = container,
                axisLocal = axis.normalized,
                maxTravel = travel,
                spin = spin,
                homeLocalPos = container.localPosition,
                members = members,
                memberHomePos = members.Select(m => m.localPosition).ToArray(),
                memberHomeRot = members.Select(m => m.localRotation).ToArray(),
                memberCenter = new Vector3[members.Length],
            };

            b.renderers = container.GetComponentsInChildren<Renderer>(true);
            b.baseColors = b.renderers.Select(ReadBaseColor).ToArray();

            // Mesh centres (for in-place screw spin) + TIGHT per-mesh colliders.
            // One fat group box made overlapping bodies unpickable (the lid's box
            // spanned the whole top) — per-part boxes keep hits honest.
            for (int i = 0; i < members.Length; i++)
            {
                var rend = members[i].GetComponentInChildren<Renderer>();
                if (rend == null) continue;
                b.memberCenter[i] = container.InverseTransformPoint(rend.bounds.center);

                var mf = rend.GetComponent<MeshFilter>();
                var box = rend.gameObject.AddComponent<BoxCollider>();
                if (mf != null && mf.sharedMesh != null)
                {
                    box.center = mf.sharedMesh.bounds.center;
                    box.size = mf.sharedMesh.bounds.size * 1.03f; // slight grab padding
                }
                b.colliders.Add(box);
                _colliderMap[box] = b;
            }

            _bodies.Add(b);
            return b;
        }

        private static Color ReadBaseColor(Renderer r)
        {
            var m = r.sharedMaterial;
            if (m == null) return Color.white;
            if (m.HasProperty("_BaseColor")) return m.GetColor("_BaseColor");
            if (m.HasProperty("_Color")) return m.color;
            return Color.white;
        }

        /// <summary>Bore axis: horizontal ⊥ the connectors' row, pointing away
        /// from the device centre (same method as DisassemblyAnimator v2).</summary>
        private Vector3 DetectBoreAxis(List<Transform> connectors, Transform pcb)
        {
            if (connectors.Count < 2) return Vector3.forward;
            Vector3 row = Vector3.zero;
            float best = 0f;
            for (int i = 0; i < connectors.Count; i++)
                for (int j = i + 1; j < connectors.Count; j++)
                {
                    Vector3 d = connectors[j].localPosition - connectors[i].localPosition;
                    d.y = 0f;
                    if (d.sqrMagnitude > best) { best = d.sqrMagnitude; row = d; }
                }
            if (best < 1e-8f) return Vector3.forward;

            Vector3 bore = Vector3.Cross(Vector3.up, row.normalized);
            Vector3 center = pcb != null ? pcb.localPosition : Vector3.zero;
            Vector3 avg = Vector3.zero;
            foreach (var c in connectors) avg += c.localPosition;
            avg /= connectors.Count;
            if (Vector3.Dot(avg - center, bore) < 0f) bore = -bore;

            return (Mathf.Abs(bore.x) >= Mathf.Abs(bore.z)
                ? new Vector3(Mathf.Sign(bore.x), 0f, 0f)
                : new Vector3(0f, 0f, Mathf.Sign(bore.z)));
        }

        private static bool Starts(string a, string b) => a.StartsWith(b, System.StringComparison.OrdinalIgnoreCase);
        private static bool Same(string a, string b) => a.Equals(b, System.StringComparison.OrdinalIgnoreCase);

        // =================================================================
        // Runtime API (called by ExplodedZoneInteraction)
        // =================================================================

        public Body FindBodyByCollider(Collider c) => _colliderMap.TryGetValue(c, out var b) ? b : null;

        /// <summary>Volume of one collider — the picker prefers small parts over
        /// big enclosing shells when hit distances are similar.</summary>
        public static float ColliderVolume(Collider c)
        {
            var s = c.bounds.size;
            return s.x * s.y * s.z;
        }

        public bool IsUnlocked(Body b) => b != null && b.dependencies.All(d => d.Extracted);

        /// <summary>True if the drag may start. Locked bodies get red-flash + shake.</summary>
        public bool BeginDrag(Body b)
        {
            if (b == null || !b.draggable || b.referenceOnly) return false;
            if (!IsUnlocked(b)) { LockedFeedback(b); return false; }
            DOTween.Kill(b.container);
            return true;
        }

        /// <summary>Sets travel (metres, model space) and applies position + screw spin.</summary>
        public void SetTravel(Body b, float metres)
        {
            b.travel = Mathf.Clamp(metres, 0f, b.maxTravel);
            b.container.localPosition = b.homeLocalPos + b.axisLocal * b.travel;

            if (b.spin && b.maxTravel > 1e-5f)
            {
                float angle = spinDegrees * (b.travel / b.maxTravel);
                Quaternion R = Quaternion.AngleAxis(angle, b.axisLocal);
                for (int i = 0; i < b.members.Length; i++)
                {
                    var m = b.members[i];
                    m.localPosition = b.memberCenter[i] + R * (b.memberHomePos[i] - b.memberCenter[i]);
                    m.localRotation = R * b.memberHomeRot[i];
                }
            }
        }

        /// <summary>Release: snap to the nearer end with a short tween.</summary>
        public void Release(Body b)
        {
            if (b == null) return;
            float target = b.travel > b.maxTravel * 0.5f ? b.maxTravel : 0f;
            DOTween.To(() => b.travel, v => SetTravel(b, v), target, snapDuration)
                   .SetEase(Ease.OutCubic)
                   .SetTarget(b.container);
        }

        /// <summary>All bodies tween home (chips first is unnecessary — pure translation).</summary>
        public void ReassembleAll()
        {
            foreach (var b in _bodies)
            {
                if (b.maxTravel <= 0f) continue;
                DOTween.Kill(b.container);
                DOTween.To(() => b.travel, v => SetTravel(b, v), 0f, snapDuration * 1.6f)
                       .SetEase(Ease.InOutQuad)
                       .SetTarget(b.container);
            }
        }

        public void ResetInstant()
        {
            foreach (var b in _bodies)
            {
                DOTween.Kill(b.container);
                SetTravel(b, 0f);
            }
        }

        // (Mesh-tint highlight removed 2026-07-16 — selection feedback is the
        // hover label on the zone frame; only the locked red flash tints.)

        private void LockedFeedback(Body b)
        {
            for (int i = 0; i < b.renderers.Length; i++)
                TintRenderer(b.renderers[i], Color.Lerp(b.baseColors[i], LockedTint, 0.7f), clear: false);
            b.container.DOShakePosition(lockedFlashSeconds, 0.004f, 20, 90f)
                       .SetTarget(b.container)
                       .OnComplete(() =>
                       {
                           b.container.localPosition = b.homeLocalPos + b.axisLocal * b.travel;
                           for (int i = 0; i < b.renderers.Length; i++)
                               TintRenderer(b.renderers[i], b.baseColors[i], clear: true);
                       });
        }

        private void TintRenderer(Renderer r, Color c, bool clear)
        {
            if (r == null) return;
            if (clear)
            {
                r.SetPropertyBlock(null);
                return;
            }
            r.GetPropertyBlock(_mpb);
            _mpb.SetColor("_BaseColor", c);
            _mpb.SetColor("_Color", c);
            r.SetPropertyBlock(_mpb);
        }

        /// <summary>World-space centre of a body (zoom reference), or the whole model.</summary>
        public Vector3 ReferencePoint(Body b)
        {
            if (b != null && b.renderers.Length > 0)
            {
                var bb = b.renderers[0].bounds;
                foreach (var r in b.renderers) bb.Encapsulate(r.bounds);
                return bb.center;
            }
            var rends = GetComponentsInChildren<Renderer>();
            if (rends.Length == 0) return transform.position;
            var bounds = rends[0].bounds;
            foreach (var r in rends) bounds.Encapsulate(r.bounds);
            return bounds.center;
        }
    }
}
