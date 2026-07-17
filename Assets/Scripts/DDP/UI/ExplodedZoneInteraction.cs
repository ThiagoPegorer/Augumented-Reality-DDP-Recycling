using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Gesture controller for the transparent exploded ACTION ZONE (v2, 2026-07-16).
    ///
    /// Interaction model (approved spec):
    ///   - TAP a part (short pinch/click)  → sticky select: highlight + zoom slider.
    ///   - PINCH-HOLD a part and move      → constrained drag along its axis
    ///     (dependency-locked parts flash red and shake; release snaps to the
    ///     nearer end; screws spin with travel). Holding also selects.
    ///   - PINCH-HOLD empty space in frame → free arcball rotation of the model.
    ///   - Zoom slider (LEFT frame edge, visible while something is selected):
    ///     drag up/down = zoom 1.0×–2.5× around the selected part. The bottom
    ///     shell is selectable as a whole-model reference.
    ///   - Reassemble button → everything tweens home.
    ///
    /// Inputs: PICO hand rays via PicoHandUIBridge.TryGetHandRay (both hands,
    /// any hand can do anything), plus an Editor mouse fallback (LMB on part =
    /// drag, LMB on empty = rotate, wheel = zoom) so everything is testable
    /// before the device build.
    ///
    /// The model itself is a runtime clone (ConstrainedTeardownModel.CreateFrom)
    /// parented under `modelAnchor` inside the zone canvas — it follows the
    /// grabber bar automatically and activates/deactivates with the canvas.
    /// </summary>
    public class ExplodedZoneInteraction : MonoBehaviour
    {
        [Header("Model")]
        [Tooltip("The original VCU_assembly (has DisassemblyAnimator). Auto-found if empty.")]
        [SerializeField] private Transform modelSource;
        [Tooltip("Anchor inside the zone canvas the clone is parented to (scale compensates the canvas 0.001).")]
        [SerializeField] private Transform modelAnchor;
        [Range(0.2f, 1.5f)]
        [SerializeField] private float modelFitScale = 0.9f;   // relative fit inside the zone

        [Header("Hover label (part name at the top of the frame)")]
        [SerializeField] private TMP_Text hoverLabel;

        [Header("Zoom slider (left frame edge)")]
        [SerializeField] private GameObject sliderRoot;         // hidden until something is selected
        [SerializeField] private RectTransform sliderTrack;
        [SerializeField] private RectTransform sliderHandle;
        [SerializeField] private float zoomMax = 2.5f;          // min is 1.0 = default size

        [Header("Gizmo rig — arcs + slider glued to the model (follows mount + zoom)")]
        [SerializeField] private RectTransform gizmoRoot;

        [Header("Rotation arcs (one per axis; they replace the frame border)")]
        [SerializeField] private RectTransform rotArcTop;      // drag along → roll  (canvas forward)
        [SerializeField] private RectTransform rotArcRight;    // drag along → pitch (canvas right)
        [SerializeField] private RectTransform rotArcBottom;   // drag along → yaw   (canvas up)
        [Tooltip("Degrees of rotation per canvas unit dragged along an arc.")]
        [SerializeField] private float degreesPerUnit = 0.7f;

        [Header("Gesture tuning")]
        [SerializeField] private float tapMaxSeconds = 0.35f;
        [SerializeField] private float tapMaxMove = 0.01f;      // metres of hand-ray hit drift

        private ConstrainedTeardownModel _model;
        private ConstrainedTeardownModel.Body _selected;
        private RectTransform _zoneRect;
        private float _zoom = 1f;
        private Vector3 _anchorBaseScale;
        private Vector3 _anchorHomeLocalPos;   // zoom shifts world pos — restore on enable

        // One pointer state per source: 0 = mouse, 1 = left hand, 2 = right hand.
        private enum Mode { None, Pending, DragBody, RotateArc, Slider, TapEmpty }
        private class Pointer
        {
            public Mode mode;
            public bool wasHeld;
            public float downTime;
            public ConstrainedTeardownModel.Body body;
            public float axisParam0, travel0;
            public Vector3 downHit;
            public int arcAxis;        // 0 = top/roll, 1 = right/pitch, 2 = bottom/yaw
            public float arcPrev;      // last drag param along the arc
            public bool moved;
        }
        private readonly Pointer[] _ptr = { new Pointer(), new Pointer(), new Pointer() };

        // =================================================================

        private void OnEnable()
        {
            _zoneRect = transform as RectTransform;

            if (_model == null)
            {
                if (modelSource == null)
                {
                    var animator = FindFirstObjectByType<DisassemblyAnimator>();
                    if (animator != null) modelSource = animator.transform;
                }
                if (modelSource == null || modelAnchor == null)
                {
                    Debug.LogWarning("[ExplodedZone] Missing model source or anchor — zone inactive.");
                    enabled = false;
                    return;
                }
                _model = ConstrainedTeardownModel.CreateFrom(modelSource, modelAnchor);
                _anchorBaseScale = modelAnchor.localScale * modelFitScale;
                _anchorHomeLocalPos = modelAnchor.localPosition;
            }

            // Fresh state each time the step flow opens the zone.
            _model.ResetInstant();
            _model.transform.localRotation = Quaternion.identity;
            _zoom = 1f;
            modelAnchor.localScale = _anchorBaseScale;
            modelAnchor.localPosition = _anchorHomeLocalPos;
            Deselect();
        }

        private void OnDisable()
        {
            if (_model != null) _model.ResetInstant();
            Deselect();
            foreach (var p in _ptr) { p.mode = Mode.None; p.wasHeld = false; }
        }

        private void Update()
        {
            if (_model == null) return;

            // ---- Editor mouse fallback ----
#if UNITY_EDITOR
            var cam = Camera.main;
            if (cam != null)
            {
                bool held = Input.GetMouseButton(0);
                UpdatePointer(_ptr[0], cam.ScreenPointToRay(Input.mousePosition), held);
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (Mathf.Abs(scroll) > 0.0001f)
                    ApplyZoom(Mathf.Clamp(_zoom + scroll * 0.8f, 1f, zoomMax));
            }
#endif
            // ---- PICO hands (either hand can do anything) ----
#if !PICO_OPENXR_SDK
            var bridge = PicoHandUIBridge.Instance;
            if (bridge != null)
            {
                if (bridge.TryGetHandRay(true, out Ray lRay, out bool lPinch)) UpdatePointer(_ptr[1], lRay, lPinch);
                else EndPointer(_ptr[1]);
                if (bridge.TryGetHandRay(false, out Ray rRay, out bool rPinch)) UpdatePointer(_ptr[2], rRay, rPinch);
                else EndPointer(_ptr[2]);
            }
#endif
            UpdateHoverLabel();
        }

        /// <summary>Keeps the control rig glued to the model: same canvas-local
        /// position AND depth as the model mount, scaled with the zoom — so the
        /// arcs/slider wrap the model instead of sitting flat on the panel
        /// (where the zoomed model floated in front of them).</summary>
        private void LateUpdate()
        {
            if (gizmoRoot == null || modelAnchor == null) return;
            Vector3 mount = modelAnchor.localPosition;   // both are canvas children
            gizmoRoot.anchoredPosition3D = mount;
            float s = Mathf.Max(_zoom, 1f);
            gizmoRoot.localScale = new Vector3(s, s, 1f);
        }

        /// <summary>Part name at the top of the frame: white while hovering any
        /// part, teal for the sticky selection when nothing is hovered. This IS
        /// the selection feedback — mesh tinting was removed (looked washed out).</summary>
        private void UpdateHoverLabel()
        {
            if (hoverLabel == null) return;

            ConstrainedTeardownModel.Body hovered = null;
            bool anyDragging = false;
            foreach (var p in _ptr) if (p.mode == Mode.DragBody || p.mode == Mode.RotateArc || p.mode == Mode.Slider) anyDragging = true;

            if (!anyDragging)
            {
#if UNITY_EDITOR
                var cam = Camera.main;
                if (cam != null) hovered = RaycastBody(cam.ScreenPointToRay(Input.mousePosition), out _);
#endif
#if !PICO_OPENXR_SDK
                if (hovered == null && PicoHandUIBridge.Instance != null)
                {
                    if (PicoHandUIBridge.Instance.TryGetHandRay(false, out Ray rr, out _))
                        hovered = RaycastBody(rr, out _);
                    if (hovered == null && PicoHandUIBridge.Instance.TryGetHandRay(true, out Ray lr, out _))
                        hovered = RaycastBody(lr, out _);
                }
#endif
            }

            if (hovered != null)
            {
                hoverLabel.text = hovered.displayName ?? hovered.name;
                hoverLabel.color = Color.white;
                if (!hoverLabel.gameObject.activeSelf) hoverLabel.gameObject.SetActive(true);
            }
            else if (_selected != null)
            {
                hoverLabel.text = _selected.displayName ?? _selected.name;
                hoverLabel.color = new Color(0.365f, 0.792f, 0.647f); // teal #5dcaa5
                if (!hoverLabel.gameObject.activeSelf) hoverLabel.gameObject.SetActive(true);
            }
            else if (hoverLabel.gameObject.activeSelf) hoverLabel.gameObject.SetActive(false);
        }

        // =================================================================
        // Pointer state machine
        // =================================================================

        private void UpdatePointer(Pointer p, Ray ray, bool held)
        {
            if (held && !p.wasHeld) OnDown(p, ray);
            else if (held && p.wasHeld) OnHold(p, ray);
            else if (!held && p.wasHeld) OnUp(p, ray);
            p.wasHeld = held;
        }

        private void EndPointer(Pointer p)
        {
            if (p.wasHeld) OnUp(p, default);
            p.wasHeld = false;
        }

        private void OnDown(Pointer p, Ray ray)
        {
            p.downTime = Time.unscaledTime;
            p.moved = false;

            // 1. Slider (only when visible).
            if (sliderRoot != null && sliderRoot.activeSelf &&
                RayHitsRect(ray, sliderTrack, out _))
            {
                p.mode = Mode.Slider;
                OnHold(p, ray);
                return;
            }

            // 2. Rotation arcs (they sit at the zone edges, outside the model).
            if (TryArc(ray, rotArcTop, 0, p) || TryArc(ray, rotArcRight, 1, p) || TryArc(ray, rotArcBottom, 2, p))
                return;

            // 3. A body of the zone model.
            var body = RaycastBody(ray, out Vector3 hit);
            if (body != null)
            {
                p.mode = Mode.Pending;   // becomes tap-select or drag
                p.body = body;
                p.downHit = hit;
                p.travel0 = body.travel;
                p.axisParam0 = AxisParam(body, ray);
                return;
            }

            // 4. Empty space inside the zone → quick tap deselects.
            if (RayHitsRect(ray, _zoneRect, out _))
            {
                p.mode = Mode.TapEmpty;
                return;
            }

            p.mode = Mode.None;
        }

        private bool TryArc(Ray ray, RectTransform arc, int axis, Pointer p)
        {
            if (arc == null || !RayHitsRect(ray, arc, out Vector3 pt)) return false;
            p.mode = Mode.RotateArc;
            p.arcAxis = axis;
            p.arcPrev = ArcParam(arc, axis, pt);
            return true;
        }

        /// <summary>Drag parameter along an arc: local X for the horizontal arcs
        /// (top/bottom), local Y for the vertical one (right).</summary>
        private static float ArcParam(RectTransform arc, int axis, Vector3 worldPoint)
        {
            Vector3 local = arc.InverseTransformPoint(worldPoint);
            return axis == 1 ? local.y : local.x;
        }

        private void OnHold(Pointer p, Ray ray)
        {
            switch (p.mode)
            {
                case Mode.Pending:
                {
                    float drift = Mathf.Abs(AxisParam(p.body, ray) - p.axisParam0) * WorldScale();
                    if (drift > tapMaxMove || Time.unscaledTime - p.downTime > tapMaxSeconds)
                    {
                        // Long/moving pinch → this is a drag: select + begin.
                        Select(p.body);
                        if (_model.BeginDrag(p.body)) p.mode = Mode.DragBody;
                        else p.mode = Mode.None;     // locked (feedback fired)
                    }
                    break;
                }
                case Mode.DragBody:
                {
                    p.moved = true;
                    float delta = (AxisParam(p.body, ray) - p.axisParam0);
                    _model.SetTravel(p.body, p.travel0 + delta);
                    break;
                }
                case Mode.RotateArc:
                {
                    RectTransform arc = p.arcAxis == 0 ? rotArcTop : p.arcAxis == 1 ? rotArcRight : rotArcBottom;
                    // Keep tracking even when the ray drifts off the narrow arc strip.
                    if (arc == null || !RayHitsRect(ray, arc, out Vector3 pt, clampInside: true)) break;
                    float param = ArcParam(arc, p.arcAxis, pt);
                    float angle = (param - p.arcPrev) * degreesPerUnit;
                    p.arcPrev = param;

                    Vector3 axisWorld = p.arcAxis == 0 ? _zoneRect.forward
                                      : p.arcAxis == 1 ? _zoneRect.right
                                      : _zoneRect.up;
                    _model.transform.rotation =
                        Quaternion.AngleAxis(angle, axisWorld) * _model.transform.rotation;
                    break;
                }
                case Mode.Slider:
                {
                    if (!RayHitsRect(ray, sliderTrack, out Vector3 pt, clampInside: true)) break;
                    Vector2 local = sliderTrack.InverseTransformPoint(pt);
                    float t = Mathf.InverseLerp(sliderTrack.rect.yMin, sliderTrack.rect.yMax, local.y);
                    ApplyZoom(Mathf.Lerp(1f, zoomMax, t));
                    break;
                }
            }
        }

        private void OnUp(Pointer p, Ray ray)
        {
            if (p.mode == Mode.Pending)
            {
                // Short still pinch → tap: toggle selection.
                if (p.body == _selected) Deselect();
                else Select(p.body);
            }
            else if (p.mode == Mode.DragBody)
            {
                _model.Release(p.body);
            }
            else if (p.mode == Mode.TapEmpty)
            {
                if (Time.unscaledTime - p.downTime <= tapMaxSeconds) Deselect();
            }
            p.mode = Mode.None;
            p.body = null;
        }

        // =================================================================
        // Selection / zoom
        // =================================================================

        private void Select(ConstrainedTeardownModel.Body b)
        {
            if (_selected == b) return;
            _selected = b;   // feedback = teal name in the hover label (no mesh tint)
            if (_selected != null)
            {
                if (sliderRoot != null) sliderRoot.SetActive(true);
                UpdateSliderHandle();
            }
        }

        private void Deselect()
        {
            _selected = null;
            if (sliderRoot != null) sliderRoot.SetActive(false);
        }

        /// <summary>Scales the anchor around the selected body (or model centre),
        /// keeping that reference point fixed in world space. Floor = 1.0×.</summary>
        private void ApplyZoom(float zoom)
        {
            zoom = Mathf.Clamp(zoom, 1f, zoomMax);
            Vector3 reference = _model.ReferencePoint(_selected);
            Vector3 refLocal = modelAnchor.InverseTransformPoint(reference);

            _zoom = zoom;
            modelAnchor.localScale = _anchorBaseScale * zoom;
            Vector3 drift = modelAnchor.TransformPoint(refLocal) - reference;
            modelAnchor.position -= drift;

            UpdateSliderHandle();
        }

        private void UpdateSliderHandle()
        {
            if (sliderHandle == null || sliderTrack == null) return;
            float t = Mathf.InverseLerp(1f, zoomMax, _zoom);
            float y = Mathf.Lerp(sliderTrack.rect.yMin + 12f, sliderTrack.rect.yMax - 12f, t);
            sliderHandle.anchoredPosition = new Vector2(sliderHandle.anchoredPosition.x, y);
        }

        /// <summary>Reassemble button target: everything home, keep rotation/zoom.</summary>
        public void Reassemble()
        {
            if (_model != null) _model.ReassembleAll();
            Deselect();
        }

        // =================================================================
        // Geometry helpers
        // =================================================================

        private Vector3 ModelCenter() => _model.ReferencePoint(null);

        private float WorldScale() => modelAnchor.lossyScale.x;

        private ConstrainedTeardownModel.Body RaycastBody(Ray ray, out Vector3 hitPoint)
        {
            hitPoint = default;
            ConstrainedTeardownModel.Body best = null;
            float bestDist = float.PositiveInfinity;
            float bestVolume = float.PositiveInfinity;
            var hits = Physics.RaycastAll(ray, 5f, ~0, QueryTriggerInteraction.Collide);

            // Two-pass pick: nearest hit wins, EXCEPT a much smaller collider
            // within 4 cm behind it steals the pick — so a chip inside the
            // housing beats the shell box that encloses it.
            foreach (var h in hits)
            {
                if (!h.collider.transform.IsChildOf(_model.transform)) continue;
                var b = _model.FindBodyByCollider(h.collider);
                if (b == null) continue;

                float vol = ConstrainedTeardownModel.ColliderVolume(h.collider);
                bool closeEnough = h.distance <= bestDist + 0.04f * WorldScale();
                bool better = best == null
                    || h.distance < bestDist - 0.04f * WorldScale()
                    || (closeEnough && vol < bestVolume * 0.5f);
                if (!better) continue;

                best = b;
                bestDist = Mathf.Min(bestDist, h.distance);
                bestVolume = vol;
                hitPoint = h.point;
            }
            return best;
        }

        /// <summary>Parameter (model-space metres) of the ray's closest point on
        /// the body's constraint axis — drag deltas come from this.</summary>
        private float AxisParam(ConstrainedTeardownModel.Body b, Ray ray)
        {
            Transform parent = b.container.parent;
            Vector3 origin = parent.TransformPoint(b.homeLocalPos);
            Vector3 axis = parent.TransformDirection(b.axisLocal).normalized;

            Vector3 w = ray.origin - origin;
            float dot = Vector3.Dot(ray.direction, axis);
            float denom = 1f - dot * dot;
            float s;
            if (Mathf.Abs(denom) < 1e-4f) s = Vector3.Dot(axis, w);
            else s = (Vector3.Dot(axis, w) - dot * Vector3.Dot(ray.direction, w)) / denom;
            return s / Mathf.Max(WorldScale(), 1e-5f);   // world metres → model metres
        }

        private static bool RayHitsRect(Ray ray, RectTransform rect, out Vector3 point, bool clampInside = false)
        {
            point = default;
            if (rect == null) return false;
            Plane plane = new Plane(-rect.forward, rect.position);
            if (!plane.Raycast(ray, out float dist) || dist < 0f || dist > 5f) return false;
            point = ray.GetPoint(dist);
            if (clampInside) return true;   // slider: keep tracking even off-rect
            Vector3 local = rect.InverseTransformPoint(point);
            Rect r = rect.rect;
            return local.x >= r.xMin && local.x <= r.xMax &&
                   local.y >= r.yMin && local.y <= r.yMax;
        }
    }
}
