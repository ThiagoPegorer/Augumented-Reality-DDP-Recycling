using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Lets the PICO pinch-ray drag the Information tab scrollbar (spec 02 §7).
    /// Sits on an invisible, widened hit area around the thin scrollbar track;
    /// while the worker pinches with the ray over that area, the scroll
    /// position follows the ray's vertical hit point on the track.
    ///
    /// Mouse input needs nothing from this script — Unity's Scrollbar and
    /// ScrollRect already handle mouse drag and wheel in the Editor.
    /// Same PXR_Hand polling pattern as PanelGrabHandle / PicoHandUIBridge.
    /// </summary>
    public class ScrollbarGrabHandle : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;

        [Tooltip("Thumb image, brightened while the ray hovers/drags.")]
        [SerializeField] private Image thumb;
        [SerializeField] private Color thumbRestingColor = new Color(0.365f, 0.792f, 0.647f); // #5dcaa5
        [SerializeField] private Color thumbActiveColor  = new Color(0.62f, 0.88f, 0.80f);

        [SerializeField] private float maxRayDistance = 5f;

        private RectTransform _rect;

#if !PICO_OPENXR_SDK
        [Header("PICO hands (auto-found at runtime if left empty)")]
        [SerializeField] private PXR_Hand leftHand;
        [SerializeField] private PXR_Hand rightHand;

        private PXR_Hand _dragHand;
        private bool _leftWasPinching, _rightWasPinching;
        private bool _anyHover;
#endif

        private void Awake()
        {
            _rect = (RectTransform)transform;
        }

#if !PICO_OPENXR_SDK
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
        }

        private void Update()
        {
            if (_dragHand != null)
            {
                if (!UpdateDrag()) _dragHand = null;
            }
            else
            {
                bool overL = TryStart(leftHand, ref _leftWasPinching);
                bool overR = _dragHand == null && TryStart(rightHand, ref _rightWasPinching);
                _anyHover = overL || overR;
            }
            ApplyThumbColor();
        }

        private bool TryStart(PXR_Hand hand, ref bool wasPinching)
        {
            if (!TryGetRay(hand, out Vector3 origin, out Vector3 dir)) { wasPinching = false; return false; }

            bool over = RayToTrackNorm(origin, dir, out _);
            bool nowPinching = hand.Pinch;
            bool rising = nowPinching && !wasPinching;
            wasPinching = nowPinching;

            if (over && rising) _dragHand = hand;
            return over;
        }

        /// <summary>Returns false when the drag ended (pinch released / ray lost).</summary>
        private bool UpdateDrag()
        {
            if (_dragHand == null || !_dragHand.Pinch) return false;
            if (!TryGetRay(_dragHand, out Vector3 origin, out Vector3 dir)) return false;

            // While dragging, follow the ray even slightly outside the hit area.
            if (RayToTrackNorm(origin, dir, out float norm) && scrollRect != null)
                scrollRect.verticalNormalizedPosition = norm;
            return true;
        }

        private static bool TryGetRay(PXR_Hand hand, out Vector3 origin, out Vector3 direction)
        {
            origin = default; direction = default;
            if (hand == null || !hand.Computed || !hand.RayValid) return false;
            Transform rayPose = hand.transform.Find("RayPose");
            if (rayPose == null) return false;
            origin = rayPose.position;
            direction = rayPose.forward;
            return true;
        }

        /// <summary>
        /// Ray ∩ this rect's plane → normalized track position (1 = top).
        /// Returns true if the hit lies inside the (widened) hit rect.
        /// </summary>
        private bool RayToTrackNorm(Vector3 origin, Vector3 dir, out float norm)
        {
            norm = 0f;
            Plane plane = new Plane(-_rect.forward, _rect.position);
            Ray ray = new Ray(origin, dir);
            if (!plane.Raycast(ray, out float dist) || dist > maxRayDistance) return false;

            Vector3 local = _rect.InverseTransformPoint(ray.GetPoint(dist));
            Rect r = _rect.rect;
            norm = Mathf.Clamp01((local.y - r.yMin) / r.height);
            return local.x >= r.xMin && local.x <= r.xMax &&
                   local.y >= r.yMin && local.y <= r.yMax;
        }
#endif

        private void ApplyThumbColor()
        {
            if (thumb == null) return;
            bool active = false;
#if !PICO_OPENXR_SDK
            active = _anyHover || _dragHand != null;
#endif
            thumb.color = active ? thumbActiveColor : thumbRestingColor;
        }
    }
}
