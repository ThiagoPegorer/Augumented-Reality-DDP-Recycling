using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// The grabber bar (DPP_UI_Specs/00 §5): a dark pill docked below every
    /// panel. The worker pinch-grabs it and drags to reposition the whole
    /// canvas in AR space — visionOS-style spatial window chrome.
    ///
    /// Two input paths:
    ///   - PICO native hand tracking (device): this script polls PXR_Hand
    ///     rays itself (same pattern as PicoHandUIBridge). Pinch while the
    ///     ray is over the bar grabs the panel; the panel then follows the
    ///     ray at the grab distance until the pinch releases.
    ///   - Editor mouse: standard EventSystem drag events (needs the canvas'
    ///     worldCamera assigned, which the builder does).
    ///
    /// The bar deliberately has NO IPointerClickHandler, so PicoHandUIBridge
    /// ignores it for clicks and the reticle never competes with the grab.
    /// </summary>
    public class PanelGrabHandle : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("What gets moved")]
        [Tooltip("Root transform moved by the grab — usually the panel's world-space Canvas. Content and bar move together because the bar is its child.")]
        [SerializeField] private Transform panelRoot;

        [Header("Hover visuals (per global hover rule)")]
        [SerializeField] private Image barFill;
        [SerializeField] private Image grip;
        [SerializeField] private Color barRestingColor   = new Color(0.039f, 0.055f, 0.086f); // #0a0e16
        [SerializeField] private Color barHoverColor     = new Color(0.075f, 0.10f, 0.15f);
        [SerializeField] private Color gripRestingColor  = new Color(0.42f, 0.46f, 0.525f);   // #6b7686
        [SerializeField] private Color gripHoverColor    = new Color(0.85f, 0.88f, 0.92f);

        [Header("Hand ray")]
        [Tooltip("Max reach of the grab ray, meters. Match PicoHandUIBridge.")]
        [SerializeField] private float maxRayDistance = 5f;

        [Header("Billboard (face the user)")]
        [Tooltip("While dragging, rotate the panel to face the headset so it never goes edge-on. The rotation is kept when you let go (visionOS-style window placement).")]
        [SerializeField] private bool billboardWhileDragging = true;

        [Tooltip("If true, the panel faces the headset EVERY frame, not just while dragging. Feels HUD-like; can be uncomfortable.")]
        [SerializeField] private bool alwaysBillboard = false;

        [Tooltip("Keep the panel upright (yaw only — no pitch/roll toward the camera). Usually most comfortable for a wall-like panel.")]
        [SerializeField] private bool keepUpright = true;

        [Tooltip("Head/camera the panel faces. Leave empty to use Camera.main at runtime.")]
        [SerializeField] private Transform headCamera;

        [Header("Startup placement (spawn in front of the user)")]
        [Tooltip("On scene load, place the panel directly in front of the user at a comfortable distance/height, facing them. After that, dragging takes over.")]
        [SerializeField] private bool recenterOnStart = true;

        [Tooltip("How far in front of the user the panel spawns, in meters.")]
        [SerializeField] private float spawnDistance = 0.7f;

        [Tooltip("Vertical offset from the head, in meters. Slightly negative drops it from eye level toward a comfortable reading height.")]
        [SerializeField] private float spawnHeightOffset = -0.1f;

        private RectTransform _rect;
        private bool _mouseHover;
        private bool _recentered;
        private bool _mouseDragging;

        /// <summary>True while the panel is being dragged (hand pinch or editor
        /// mouse). Other gestures (e.g. TwoHandTwistRotate) poll this so they
        /// never fight the panel grab for the same pinch.</summary>
        public bool IsGrabbing { get; private set; }

        // Places the panel a comfortable distance ahead of the head at reading
        // height, then squares it up to the user. Yaw-only forward so it spawns
        // upright regardless of how the user's head is tilted. Returns false if
        // the head pose isn't available yet (caller retries next frame).
        private bool RecenterInFrontOfUser()
        {
            if (panelRoot == null) return false;
            Transform head = Head;
            if (head == null) return false;

            // Flatten the head's forward to the horizontal plane so the panel
            // appears straight ahead at eye level, not tilted up/down.
            Vector3 fwd = head.forward;
            fwd.y = 0f;
            if (fwd.sqrMagnitude < 1e-6f) fwd = Vector3.forward; // looking straight up/down
            fwd.Normalize();

            Vector3 pos = head.position + fwd * spawnDistance;
            pos.y = head.position.y + spawnHeightOffset;
            panelRoot.position = pos;

            FaceCamera();
            return true;
        }

        private Transform Head
        {
            get
            {
                if (headCamera != null) return headCamera;
                if (Camera.main != null) headCamera = Camera.main.transform;
                return headCamera;
            }
        }

        // Rotates panelRoot so its visible face (-forward, since the canvas
        // looks down -Z) points at the headset. With keepUpright, only yaw is
        // applied so the panel stays vertical like a wall-mounted screen.
        private void FaceCamera()
        {
            if (panelRoot == null) return;
            Transform head = Head;
            if (head == null) return;

            Vector3 toHead = head.position - panelRoot.position;
            if (keepUpright) toHead.y = 0f;
            if (toHead.sqrMagnitude < 1e-6f) return;

            // The canvas's readable side faces -forward, so aim -forward at the
            // head by making forward point AWAY from the head.
            Quaternion look = Quaternion.LookRotation(-toHead.normalized, Vector3.up);
            panelRoot.rotation = look;
        }

#if !PICO_OPENXR_SDK
        [Header("PICO hands (auto-found at runtime if left empty)")]
        [SerializeField] private PXR_Hand leftHand;
        [SerializeField] private PXR_Hand rightHand;

        private PXR_Hand _grabHand;
        private bool _leftWasPinching, _rightWasPinching;
        private float _grabDistance;
        private Vector3 _grabOffset;
        private bool _leftRayHover, _rightRayHover;
#endif

        private void Awake()
        {
            _rect = (RectTransform)transform;
            if (panelRoot == null && GetComponentInParent<Canvas>() != null)
                panelRoot = GetComponentInParent<Canvas>().transform;
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
            // One-shot startup placement: retry each frame until the head pose
            // is valid, then place the panel in front of the user and stop.
            if (recenterOnStart && !_recentered)
            {
                if (RecenterInFrontOfUser()) _recentered = true;
            }

            if (_grabHand != null)
            {
                UpdateGrab();
            }
            else
            {
                _leftRayHover  = TryGrabOrHover(leftHand,  ref _leftWasPinching);
                _rightRayHover = _grabHand == null && TryGrabOrHover(rightHand, ref _rightWasPinching);
            }

            // Continuous billboard (HUD-like) when enabled and not being dragged
            // (the drag path billboards itself so it tracks the new position).
            if (alwaysBillboard && _grabHand == null)
                FaceCamera();

            IsGrabbing = _grabHand != null || _mouseDragging;
            ApplyHoverVisual();
        }

        /// <summary>Returns true if this hand's ray currently hovers the bar; starts a grab on pinch rising edge.</summary>
        private bool TryGrabOrHover(PXR_Hand hand, ref bool wasPinching)
        {
            if (!TryGetRay(hand, out Vector3 origin, out Vector3 direction))
            {
                wasPinching = false;
                return false;
            }

            bool over = RayOverBar(origin, direction, out Vector3 hitPoint);

            bool nowPinching = hand.Pinch;
            bool risingEdge = nowPinching && !wasPinching;
            wasPinching = nowPinching;

            if (over && risingEdge)
            {
                _grabHand = hand;
                _grabDistance = Vector3.Distance(origin, hitPoint);
                _grabOffset = panelRoot.position - hitPoint;
            }
            return over;
        }

        private void UpdateGrab()
        {
            if (_grabHand == null) return;

            bool stillValid = TryGetRay(_grabHand, out Vector3 origin, out Vector3 direction);
            if (!stillValid || !_grabHand.Pinch)
            {
                _grabHand = null;
                return;
            }

            Vector3 target = origin + direction * _grabDistance;
            panelRoot.position = target + _grabOffset;

            // Turn to face the user as it moves, so dragging it to your side
            // never leaves it edge-on. Rotation persists after release.
            if (billboardWhileDragging || alwaysBillboard)
                FaceCamera();
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

        /// <summary>Ray ∩ bar plane, then point-in-rect test in the bar's local space.</summary>
        private bool RayOverBar(Vector3 origin, Vector3 direction, out Vector3 hitPoint)
        {
            hitPoint = default;
            Plane plane = new Plane(-_rect.forward, _rect.position);
            Ray ray = new Ray(origin, direction);
            if (!plane.Raycast(ray, out float dist)) return false;
            if (dist < 0f || dist > maxRayDistance) return false;

            hitPoint = ray.GetPoint(dist);
            Vector3 local = _rect.InverseTransformPoint(hitPoint);
            Rect r = _rect.rect;
            return local.x >= r.xMin && local.x <= r.xMax &&
                   local.y >= r.yMin && local.y <= r.yMax;
        }
#else
        // OpenXR backend has no PICO native hand path, but the panel still
        // needs startup placement and continuous billboard if enabled.
        private void Update()
        {
            if (recenterOnStart && !_recentered)
            {
                if (RecenterInFrontOfUser()) _recentered = true;
            }
            if (alwaysBillboard)
                FaceCamera();

            IsGrabbing = _mouseDragging;
        }
#endif

        // ---- Editor mouse path (works in Play Mode with worldCamera set) ----

        private Plane _dragPlane;
        private Vector3 _mouseGrabOffset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (panelRoot == null) return;
            _mouseDragging = true;
            _dragPlane = new Plane(-transform.forward, transform.position);
            if (TryMousePoint(eventData, out Vector3 p))
                _mouseGrabOffset = panelRoot.position - p;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (panelRoot == null) return;
            if (TryMousePoint(eventData, out Vector3 p))
                panelRoot.position = p + _mouseGrabOffset;

            if (billboardWhileDragging || alwaysBillboard)
                FaceCamera();
        }

        public void OnEndDrag(PointerEventData eventData) { _mouseDragging = false; }

        private bool TryMousePoint(PointerEventData eventData, out Vector3 point)
        {
            point = default;
            Camera cam = eventData.pressEventCamera != null ? eventData.pressEventCamera : Camera.main;
            if (cam == null) return false;
            Ray ray = cam.ScreenPointToRay(eventData.position);
            if (!_dragPlane.Raycast(ray, out float dist)) return false;
            point = ray.GetPoint(dist);
            return true;
        }

        public void OnPointerEnter(PointerEventData eventData) { _mouseHover = true;  ApplyHoverVisual(); }
        public void OnPointerExit(PointerEventData eventData)  { _mouseHover = false; ApplyHoverVisual(); }

        private void ApplyHoverVisual()
        {
            bool hovered = _mouseHover;
#if !PICO_OPENXR_SDK
            hovered |= _leftRayHover || _rightRayHover || _grabHand != null;
#endif
            if (barFill != null) barFill.color = hovered ? barHoverColor : barRestingColor;
            if (grip != null)    grip.color    = hovered ? gripHoverColor : gripRestingColor;
        }
    }
}
