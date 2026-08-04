using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Pinch-drag vertical scrolling for a masked list (spec 13 v10).
    ///
    /// The SAME gesture the exploded-zone part list uses: put the ray inside the
    /// window, pinch, drag up or down, release. That behaviour was written inside
    /// ZonePartInteraction and welded to its part-drag state machine, so it could not
    /// be reused as-is — this is the gesture on its own, with no dependency on the
    /// model, the twist gesture or a row template.
    ///
    /// A small threshold before scrolling starts keeps a pinch meant as a TAP from
    /// nudging the list, which matters here because rows may later become tappable.
    ///
    /// Mouse drag is handled by the EventSystem interfaces below, so the list is
    /// testable in Play Mode without a headset.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PinchScrollArea : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [Header("Wiring (set by builder)")]
        [Tooltip("The masked window. This component sits on it.")]
        [SerializeField] private RectTransform viewport;
        [Tooltip("Taller child that slides inside the viewport.")]
        [SerializeField] private RectTransform content;

        [SerializeField] private float maxRayDistance = 5f;
        [Tooltip("Viewport-local pixels of travel before a pinch counts as a scroll.")]
        [SerializeField] private float scrollThreshold = 12f;

        private float _startY, _contentStartY;

        private void Awake()
        {
            if (viewport == null) viewport = (RectTransform)transform;
        }

        /// <summary>Content travel available. 0 when everything already fits, which is
        /// why a short list simply cannot be dragged out of view.</summary>
        private float MaxScroll()
            => Mathf.Max(0f, (content != null ? content.sizeDelta.y : 0f)
                           - (viewport != null ? viewport.rect.height : 0f));

        /// <summary>Applies the scroll and returns the content travel actually
        /// used this call (clamps eat the rest) — the audio's motion signal.</summary>
        private float Apply(float delta)
        {
            if (content == null) return 0f;
            var p = content.anchoredPosition;
            p.y = Mathf.Clamp(_contentStartY + delta, 0f, MaxScroll());
            float dy = p.y - content.anchoredPosition.y;
            content.anchoredPosition = p;
            return dy;
        }

        /// <summary>Frame delta → world vector for HandPinchAudio (the wind's
        /// direction input). Uses the viewport plane so "up" matches the panel.</summary>
        private Vector3 WorldDelta(float dy)
            => viewport != null ? viewport.TransformVector(new Vector3(0f, dy, 0f)) : Vector3.zero;

        // ---------------- mouse (Editor / Play Mode) ----------------

        public void OnBeginDrag(PointerEventData e)
        {
            _contentStartY = content != null ? content.anchoredPosition.y : 0f;
            _startY = 0f;
            HandPinchAudio.ObjectGrabbed(true);
        }

        public void OnDrag(PointerEventData e)
        {
            // Same sign as the hand path: dragging UP reveals LATER entries, because the
            // content pivot is the top edge and anchoredPosition.y grows downward through
            // the list. Getting this backwards makes the list feel broken rather than
            // inverted, so the two input paths must agree.
            _startY += e.delta.y;
            HandPinchAudio.DragTick(true, WorldDelta(Apply(_startY)));
        }

#if !PICO_OPENXR_SDK
        // ---------------- PICO hand rays ----------------

        [Header("PICO hands (auto-found at runtime if left empty)")]
        [SerializeField] private PXR_Hand leftHand;
        [SerializeField] private PXR_Hand rightHand;

        private Transform _leftPoint, _rightPoint;
        private bool _leftWas, _rightWas;
        private bool _candidate, _scrolling, _isLeft;

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
            if (leftHand != null) _leftPoint = leftHand.transform.Find("RayPose");
            if (rightHand != null) _rightPoint = rightHand.transform.Find("RayPose");
        }

        private void OnDisable()
        {
            _candidate = _scrolling = false;
            _leftWas = _rightWas = false;
        }

        private void Update()
        {
            bool lPinch = IsPinching(leftHand);
            bool rPinch = IsPinching(rightHand);
            bool lRise = lPinch && !_leftWas;
            bool rRise = rPinch && !_rightWas;
            _leftWas = lPinch; _rightWas = rPinch;

            bool lRay = TryGetRay(leftHand, _leftPoint, out Ray leftRay);
            bool rRay = TryGetRay(rightHand, _rightPoint, out Ray rightRay);

            if (_candidate)
            {
                bool pinch = _isLeft ? lPinch : rPinch;
                bool rayOk = _isLeft ? lRay : rRay;
                if (!pinch || !rayOk) { _candidate = false; _scrolling = false; return; }

                if (LocalY(_isLeft ? leftRay : rightRay, out float y))
                {
                    float delta = y - _startY;
                    if (!_scrolling && Mathf.Abs(delta) > scrollThreshold)
                    {
                        _scrolling = true;
                        HandPinchAudio.ObjectGrabbed(!_isLeft);
                    }
                    if (_scrolling) HandPinchAudio.DragTick(!_isLeft, WorldDelta(Apply(delta)));
                }
                return;
            }

            if (rRise && rRay && LocalY(rightRay, out float ry)) Begin(false, ry);
            else if (lRise && lRay && LocalY(leftRay, out float ly)) Begin(true, ly);
        }

        private void Begin(bool isLeft, float y)
        {
            _candidate = true;
            _scrolling = false;
            _isLeft = isLeft;
            _startY = y;
            _contentStartY = content != null ? content.anchoredPosition.y : 0f;
        }

        /// <summary>Viewport-local Y of ray ∩ viewport plane; false outside the window.</summary>
        private bool LocalY(Ray ray, out float y)
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
    }
}
