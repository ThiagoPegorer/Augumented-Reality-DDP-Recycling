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

            ApplyHoverVisual();
        }

        /// <summary>Returns true if this hand's ray currently hovers the bar; starts a grab on pinch rising edge.</summary>
        private bool TryGrabOrHover(PXR_Hand hand, ref bool wasPinch