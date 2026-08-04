using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Mechanism #4 — constrained component drag (v4.6.1, 2026-07-20).
    /// Two selection methods, one shared drag core:
    ///
    /// METHOD 1 — direct: ray over a part brightens it (hover). Pinch selects:
    /// the part highlights, every other part drops to 50 % opacity, and while
    /// the pinch is held the part slides along its real extraction axis
    /// (ray↔axis closest-point). Open the pinch → the part PARKS (no snap).
    ///
    /// METHOD 2 — the "+" list: fans to the user's RIGHT of the "+" as a
    /// masked 3-ROW WINDOW; pinch-drag vertically anywhere inside the window
    /// scrolls it. Pinch-HOLD a name → that part isolates; the OTHER hand
    /// pinches ANYWHERE (zero aiming) and pulls the part along its axis.
    /// Release the name → part parks, list stays open.
    ///
    /// Dependencies (engine v4.6.1): a part unlocks at 50 % of its
    /// prerequisite's travel, and the prerequisite can't return below that
    /// threshold while the dependent is displaced. Locked part → red shake.
    /// While any part session is active the twist/zoom gesture is blocked.
    /// </summary>
    public class ZonePartInteraction : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private ExplodedZoneInteraction zone;
        [SerializeField] private TwoHandTwistRotate twist;
        [SerializeField] private Button plusButton;
        [SerializeField] private RectTransform plusGlyph;     // rotated 45° while the list is open
        [SerializeField] private Button regroupButton;        // recycle icon — reassemble everything
        [SerializeField] private RectTransform listRoot;
        [SerializeField] private RectTransform viewport;      // RectMask2D 3-row window
        [SerializeField] private RectTransform content;       // scrolled inside the viewport
        [SerializeField] private RectTransform rowTemplate;   // inactive template, cloned per body

        [Header("Feel")]
        [SerializeField] private float maxRayDistance = 5f;
        [Tooltip("Hits within this depth window compete by size — smaller part wins (screws over lid).")]
        [SerializeField] private float pickDepthWindow = 0.02f;
        [SerializeField] private float rowStep = 36f;
        [Tooltip("Vertical hand movement (canvas px) that turns a pinch inside the list into a SCROLL instead of a selection hold.")]
        [SerializeField] private float scrollThreshold = 20f;

        private static readonly Color RowAvailable = Color.white;
        private static readonly Color RowLocked    = new Color(0.365f, 0.451f, 0.588f); // #5d7396
        private static readonly Color RowBgNormal  = new Color(0.039f, 0.122f, 0.267f); // #0a1f44
        private static readonly Color RowBgHeld    = new Color(0.180f, 0.353f, 0.627f); // #2e5aa0

        private class Row
        {
            public RectTransform rt;
            public Image bg, outline;
            public TMP_Text label;
            public ConstrainedTeardownModel.Body body;
        }

        private readonly List<Row> _rows = new List<Row>();
        private bool _listOpen;

        // Part sessions
        private ConstrainedTeardownModel.Body _hover;
        private ConstrainedTeardownModel.Body _held;    // list method: row pinch-held
        private bool _holdIsLeft;
        private ConstrainedTeardownModel.Body _drag;    // either method: actively dragged
        private bool _dragIsLeft;
        private float _param0, _travel0, _worldPerModel;
        private Vector3 _axisOrigin, _axisDir;

        // Scroll gesture (inside the viewport)
        private bool _scrollCandidate;   // pinch started inside the window; undecided
        private bool _scrollMode;        // threshold tripped → scrolling
        private bool _scrollIsLeft;
        private float _scrollStartY;     // viewport-local Y at pinch-down
        private float _contentStartY;

        private ConstrainedTeardownModel Model => zone != null ? zone.Model : null;

        private void Awake()
        {
            if (zone == null) zone = GetComponent<ExplodedZoneInteraction>();
            if (twist == null) twist = GetComponent<TwoHandTwistRotate>();
            if (plusButton != null) plusButton.onClick.AddListener(ToggleList);
            if (regroupButton != null) regroupButton.onClick.AddListener(RegroupAll);
            if (listRoot != null) listRoot.gameObject.SetActive(false);
        }

        /// <summary>Regroup button: end any part session and cascade every
        /// moved component back home (reverse dependency order in the engine).</summary>
        public void RegroupAll()
        {
            ResetSession();
            var m = Model;
            if (m != null) m.ReassembleAll();
        }

        private void OnEnable() { ResetSession(); }
        private void OnDisable() { ResetSession(); }

        private void ResetSession()
        {
            SetHover(null);
            _held = null;
            _drag = null;
            _scrollCandidate = false;
            _scrollMode = false;
            var m = Model;
            if (m != null) m.ClearIsolation();
            if (twist != null) twist.ExternallyBlocked = false;
        }

        private void ToggleList() => SetListOpen(!_listOpen);

        private void SetListOpen(bool open)
        {
            _listOpen = open;
            if (listRoot != null)
            {
                if (open) EnsureRows();
                listRoot.gameObject.SetActive(open);
            }
            if (plusGlyph != null)
                plusGlyph.localRotation = Quaternion.Euler(0f, 0f, open ? 45f : 0f);
        }

        /// <summary>One row per draggable body, stacked top-down inside the
        /// scrolled content (disassembly order).</summary>
        private void EnsureRows()
        {
            var m = Model;
            if (m == null || rowTemplate == null || content == null || _rows.Count > 0) return;

            var bodies = new List<ConstrainedTeardownModel.Body>();
            foreach (var b in m.Bodies)
                if (b.draggable && !b.referenceOnly) bodies.Add(b);

            for (int i = 0; i < bodies.Count; i++)
            {
                var rt = Instantiate(rowTemplate, content);
                rt.name = $"Row_{bodies[i].name}";
                rt.gameObject.SetActive(true);
                rt.anchoredPosition = new Vector2(0f, -(i * rowStep) - 4f);

                var row = new Row
                {
                    rt = rt,
                    body = bodies[i],
                    bg = rt.Find("BG") != null ? rt.Find("BG").GetComponent<Image>() : null,
                    outline = rt.Find("Outline") != null ? rt.Find("Outline").GetComponent<Image>() : null,
                    label = rt.GetComponentInChildren<TMP_Text>(true),
                };
                if (row.label != null) row.label.text = bodies[i].displayName ?? bodies[i].name;
                if (row.outline != null) row.outline.gameObject.SetActive(false);
                _rows.Add(row);
            }

            content.sizeDelta = new Vector2(0f, bodies.Count * rowStep + 8f);
        }

        private float MaxScroll()
            => Mathf.Max(0f, (content != null ? content.sizeDelta.y : 0f) - (viewport != null ? viewport.rect.height : 0f));

        private void SetHover(ConstrainedTeardownModel.Body b)
        {
            if (b == _hover) return;
            var m = Model;
            if (m != null)
            {
                if (_hover != null) m.SetHover(_hover, false);
                if (b != null) m.SetHover(b, true);
            }
            _hover = b;
        }

#if !PICO_OPENXR_SDK
        [Header("PICO hands (auto-found at runtime if left empty)")]
        [SerializeField] private PXR_Hand leftHand;
        [SerializeField] private PXR_Hand rightHand;
        private Transform _leftPoint, _rightPoint;
        private bool _leftWasPinching, _rightWasPinching;

        private void Start()
        {
            if (leftHand == null || rightHand == null)
            {
                foreach (var hand in FindObjectsByType<PXR_Hand>(FindObjectsSortMode.None))
                {
                    string n = hand.gameObject.name;
                    if (leftHand == null && n.Contains("Left")) leftHand = hand;
                    else if (rightHand == null && n.Contains("Right")) rightHand = hand;
                }
            }
            if (leftHand != null)  _leftPoint  = leftHand.transform.Find("RayPose");
            if (rightHand != null) _rightPoint = rightHand.transform.Find("RayPose");
        }

        private void Update()
        {
            var m = Model;
            if (m == null) return;
            if (twist != null && twist.Paused)      // guide modal open — stand down
            {
                ResetSession();
                return;
            }

            bool lPinch = IsPinching(leftHand);
            bool rPinch = IsPinching(rightHand);
            bool lRise = lPinch && !_leftWasPinching;
            bool rRise = rPinch && !_rightWasPinching;
            _leftWasPinching = lPinch;
            _rightWasPinching = rPinch;

            bool lRay = TryGetRay(leftHand, _leftPoint, out Ray leftRay);
            bool rRay = TryGetRay(rightHand, _rightPoint, out Ray rightRay);

            // ---- 0. Scroll gesture (candidate → mode) ------------------------
            if (_scrollCandidate)
            {
                bool pinch = _scrollIsLeft ? lPinch : rPinch;
                bool rayOk = _scrollIsLeft ? lRay : rRay;
                if (!pinch || !rayOk)
                {
                    _scrollCandidate = false;
                    _scrollMode = false;
                }
                else if (ViewportLocalY(_scrollIsLeft ? leftRay : rightRay, out float y))
                {
                    float delta = y - _scrollStartY;
                    if (!_scrollMode && Mathf.Abs(delta) > scrollThreshold)
                    {
                        _scrollMode = true;
                        HandPinchAudio.ObjectGrabbed(!_scrollIsLeft);
                        // A hold that started on the same pinch was a misread — cancel it.
                        if (_held != null && _holdIsLeft == _scrollIsLeft)
                        {
                            _held = null;
                            if (_drag == null) m.ClearIsolation();
                        }
                    }
                    if (_scrollMode && content != null)
                    {
                        var p = content.anchoredPosition;
                        p.y = Mathf.Clamp(_contentStartY + delta, 0f, MaxScroll());
                        float dy = p.y - content.anchoredPosition.y;
                        content.anchoredPosition = p;
                        HandPinchAudio.DragTick(!_scrollIsLeft,
                            viewport != null ? viewport.TransformVector(new Vector3(0f, dy, 0f)) : Vector3.zero);
                    }
                }
            }

            // ---- 1. Active drag: follow / end -------------------------------
            if (_drag != null)
            {
                bool stillPinching = _dragIsLeft ? lPinch : rPinch;
                bool rayOk = _dragIsLeft ? lRay : rRay;
                if (!stillPinching || !rayOk)
                {
                    _drag = null;                       // PARK — no snap, part stays
                    if (_held == null) m.ClearIsolation();
                }
                else
                {
                    float p = ParamOnAxis(_dragIsLeft ? leftRay : rightRay);
                    // Audio tracks the PART's real motion, not the hand's — a part
                    // pinned at a dependency limit falls silent even if the hand moves.
                    Vector3 before = _drag.container.position;
                    m.SetTravel(_drag, _travel0 + (p - _param0) / _worldPerModel);
                    HandPinchAudio.DragTick(!_dragIsLeft, _drag.container.position - before);
                }
            }

            // ---- 2. List hold session ---------------------------------------
            if (_held != null)
            {
                bool holdPinch = _holdIsLeft ? lPinch : rPinch;
                if (!holdPinch)
                {
                    _held = null;                       // session over; part parked; list stays open
                    if (_drag == null) m.ClearIsolation();
                }
                else if (_drag == null)
                {
                    bool otherRise = _holdIsLeft ? rRise : lRise;
                    bool otherRay = _holdIsLeft ? rRay : lRay;
                    if (otherRise && otherRay && m.BeginDrag(_held))
                        StartDrag(_held, !_holdIsLeft, _holdIsLeft ? rightRay : leftRay);
                }
            }

            // ---- 3. Idle hands: rows + direct hover/select ------------------
            if (_drag == null && _held == null && !_scrollMode)
            {
                Row hotRow = null; bool hotIsLeft = false; bool hotRise = false;
                Ray hotRayVal = default;
                if (_listOpen)
                {
                    if (rRay && (hotRow = RowUnderRay(rightRay)) != null) { hotIsLeft = false; hotRise = rRise; hotRayVal = rightRay; }
                    else if (lRay && (hotRow = RowUnderRay(leftRay)) != null) { hotIsLeft = true; hotRise = lRise; hotRayVal = leftRay; }
                }
                UpdateRowVisuals(hotRow);

                if (hotRow != null)
                {
                    SetHover(null);
                    if (hotRise)
                    {
                        BeginScrollCandidate(hotIsLeft, hotRayVal);
                        if (!m.IsUnlocked(hotRow.body)) m.BeginDrag(hotRow.body);  // triggers LockedFeedback
                        else
                        {
                            _held = hotRow.body;
                            _holdIsLeft = hotIsLeft;
                            m.Isolate(_held);
                            HandPinchAudio.ObjectGrabbed(!hotIsLeft);
                        }
                    }
                }
                else
                {
                    // Pinch in the window's empty space also starts a scroll candidate.
                    if (_listOpen && !_scrollCandidate)
                    {
                        if (rRise && rRay && ViewportLocalY(rightRay, out _)) BeginScrollCandidate(false, rightRay);
                        else if (lRise && lRay && ViewportLocalY(leftRay, out _)) BeginScrollCandidate(true, leftRay);
                    }

                    // Direct method: hover + pinch-select on the model itself.
                    ConstrainedTeardownModel.Body hit = null; bool hitIsLeft = false; bool hitRise = false;
                    if (rRay && (hit = PickBody(m, rightRay)) != null) { hitIsLeft = false; hitRise = rRise; }
                    else if (lRay && (hit = PickBody(m, leftRay)) != null) { hitIsLeft = true; hitRise = lRise; }

                    SetHover(hit);
                    if (hit != null && hitRise && !_scrollCandidate && m.BeginDrag(hit))
                    {
                        SetHover(null);
                        m.Isolate(hit);
                        StartDrag(hit, hitIsLeft, hitIsLeft ? leftRay : rightRay);
                    }
                }
            }
            else
            {
                UpdateRowVisuals(null);
            }

            if (twist != null)
                twist.ExternallyBlocked = _drag != null || _held != null || _scrollMode;
        }

        private void BeginScrollCandidate(bool isLeft, Ray ray)
        {
            if (!ViewportLocalY(ray, out float y)) return;
            _scrollCandidate = true;
            _scrollMode = false;
            _scrollIsLeft = isLeft;
            _scrollStartY = y;
            _contentStartY = content != null ? content.anchoredPosition.y : 0f;
        }

        /// <summary>Viewport-local Y of the ray ∩ viewport plane; false when the
        /// point is outside the window rect.</summary>
        private bool ViewportLocalY(Ray ray, out float y)
        {
            y = 0f;
            if (viewport == null) return false;
            var plane = new Plane(-viewport.forward, viewport.position);
            if (!plane.Raycast(ray, out float dist) || dist > maxRayDistance) return false;
            Vector3 local = viewport.InverseTransformPoint(ray.GetPoint(dist));
            Rect r = viewport.rect;
            if (local.x < r.xMin || local.x > r.xMax || local.y < r.yMin || local.y > r.yMax) return false;
            y = local.y;
            return true;
        }

        private static bool IsPinching(PXR_Hand hand)
            => hand != null && hand.Computed && hand.Pinch;

        private static bool TryGetRay(PXR_Hand hand, Transform point, out Ray ray)
        {
            ray = default;
            if (hand == null || !hand.Computed || !hand.RayValid || point == null) return false;
            ray = new Ray(point.position, point.forward);
            return true;
        }
#endif

        // ------------------------------------------------------------------
        // Shared drag core
        // ------------------------------------------------------------------

        private void StartDrag(ConstrainedTeardownModel.Body b, bool isLeft, Ray ray)
        {
            _drag = b;
            _dragIsLeft = isLeft;
            HandPinchAudio.ObjectGrabbed(!isLeft);

            var parent = b.container.parent;
            _axisDir = parent.TransformDirection(b.axisLocal).normalized;
            _axisOrigin = parent.TransformPoint(b.homeLocalPos);
            _worldPerModel = Mathf.Max(1e-5f, parent.TransformVector(b.axisLocal.normalized).magnitude);
            _travel0 = b.travel;
            _param0 = ParamOnAxis(ray);
        }

        /// <summary>Closest-point parameter (world metres) of the part's axis
        /// line to the hand ray — the aim-free drag: only the ray's MOTION
        /// along the axis matters, not what it points at.</summary>
        private float ParamOnAxis(Ray ray)
        {
            Vector3 w0 = _axisOrigin - ray.origin;
            float b = Vector3.Dot(_axisDir, ray.direction);
            float d = Vector3.Dot(_axisDir, w0);
            float e = Vector3.Dot(ray.direction, w0);
            float denom = 1f - b * b;
            if (denom < 1e-4f) return _param0;          // ray ∥ axis — hold position
            return (b * e - d) / denom;
        }

        /// <summary>Physics pick with small-part-wins: among hits within the
        /// depth window of the nearest, the smallest collider wins — so screws
        /// beat the lid that encloses them.</summary>
        private ConstrainedTeardownModel.Body PickBody(ConstrainedTeardownModel m, Ray ray)
        {
            var hits = Physics.RaycastAll(ray, maxRayDistance);
            if (hits.Length == 0) return null;
            System.Array.Sort(hits, (a, b2) => a.distance.CompareTo(b2.distance));

            ConstrainedTeardownModel.Body best = null;
            float bestVolume = float.MaxValue;
            float d0 = -1f;
            foreach (var h in hits)
            {
                var body = m.FindBodyByCollider(h.collider);
                if (body == null) continue;
                if (d0 < 0f) d0 = h.distance;
                if (h.distance > d0 + pickDepthWindow) break;
                float v = ConstrainedTeardownModel.ColliderVolume(h.collider);
                if (v < bestVolume) { bestVolume = v; best = body; }
            }
            return best;
        }

        // ------------------------------------------------------------------
        // List rows
        // ------------------------------------------------------------------

        private Row RowUnderRay(Ray ray)
        {
            // The mask hides overflowing rows visually, but our manual plane
            // test doesn't know that — require the point inside the WINDOW too.
            if (!ViewportLocalY(ray, out _)) return null;

            foreach (var row in _rows)
            {
                if (row.rt == null || !row.rt.gameObject.activeInHierarchy) continue;
                var plane = new Plane(-row.rt.forward, row.rt.position);
                if (!plane.Raycast(ray, out float dist) || dist > maxRayDistance) continue;
                Vector3 local = row.rt.InverseTransformPoint(ray.GetPoint(dist));
                Rect r = row.rt.rect;
                if (local.x >= r.xMin && local.x <= r.xMax && local.y >= r.yMin && local.y <= r.yMax)
                    return row;
            }
            return null;
        }

        private void UpdateRowVisuals(Row hot)
        {
            var m = Model;
            foreach (var row in _rows)
            {
                bool held = _held != null && row.body == _held;
                bool unlocked = m != null && m.IsUnlocked(row.body);
                if (row.bg != null) row.bg.color = held ? RowBgHeld : RowBgNormal;
                if (row.label != null)
                    row.label.color = held ? Color.white : (unlocked ? RowAvailable : RowLocked);
                if (row.outline != null) row.outline.gameObject.SetActive(held || row == hot);
            }
        }
    }
}
