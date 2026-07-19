using UnityEngine;
using TMPro;

namespace DPP.UI
{
    /// <summary>
    /// Zone rotation + scale v4.1 — TWO-HAND TWIST &amp; PINCH, 2026-07-19.
    ///
    /// Port of the touchscreen two-finger manipulation gestures (XRI
    /// TwistGesture / PinchGesture) to PICO hand tracking: the two pinching
    /// HANDS play the role of the two fingers. Both dimensions read the same
    /// line between the two RayPose points:
    ///   - ROTATION: how the line's horizontal heading changes → model yaw
    ///     (signed delta, the analog of TwistGesture.CalculateDeltaRotation).
    ///   - SCALE: how the line's length changes → model zoom. Spread hands =
    ///     zoom in, bring together = zoom out. Clamped so the model never
    ///     goes below its default size (floor 1×) and no larger than maxZoom.
    ///
    /// Both run simultaneously while both hands pinch (map-app feel); each
    /// can be disabled independently for isolated testing. Open either pinch
    /// → stops instantly, pose and zoom kept. Delta/ratio-based per stroke,
    /// so ratcheting works for both: twist/spread, release, reposition,
    /// pinch, continue.
    ///
    /// Yaw-only rotation by design; manipulates the MODEL ANCHOR, never the
    /// clone, so the constrained-body engine's local axes stay untouched.
    /// </summary>
    public class TwoHandTwistRotate : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("Transform that gets yawed/scaled. Auto-filled from ExplodedZoneInteraction.ModelAnchor on the same GameObject if empty.")]
        [SerializeField] private Transform target;

        [Header("Rotation")]
        [SerializeField] private bool enableRotation = true;

        [Tooltip("Degrees of model yaw per degree of hand twist. 1 = 1:1.")]
        [SerializeField] private float gain = 1f;

        [Tooltip("Per-frame deltas above this (degrees) are dropped as hand-tracking spikes, not intent.")]
        [SerializeField] private float maxDeltaPerFrame = 12f;

        [Header("Scale (pinch-zoom)")]
        [SerializeField] private bool enableScale = true;

        [Tooltip("Upper zoom limit as a multiple of the default size. Lower limit is always 1× (never smaller than default).")]
        [SerializeField] private float maxZoom = 2f;

        [Tooltip("Zoom multiplier per doubling of hand separation. 1 = separation maps 1:1 to scale ratio.")]
        [SerializeField] private float zoomGain = 1f;

        [Header("Gesture gates")]
        [Tooltip("Hands closer than this (metres) don't manipulate — the line between near-touching hands is too noisy to be meaningful.")]
        [SerializeField] private float minHandSeparation = 0.05f;

        [Tooltip("Restore the model's default facing AND default zoom every time the zone screen opens.")]
        [SerializeField] private bool resetOnEnable = true;

        [Header("Debug")]
        [Tooltip("Show a live gesture-state readout at the bottom of the zone canvas. Turn OFF once the gesture is validated.")]
        [SerializeField] private bool debugOverlay = true;

        private PanelGrabHandle _panelHandle;   // don't manipulate while the panel itself is being dragged
        private Quaternion _initialLocalRot;
        private bool _initialCaptured;

        // Base scale = the anchor's scale AFTER ExplodedZoneInteraction's fit
        // (captured lazily on the first gesture frame, when init is done).
        private Vector3 _baseScale;
        private bool _baseScaleCaptured;
        private float _zoom = 1f;               // current zoom, 1..maxZoom

        private bool _armed;
        private float _prevHeading;
        private float _armSep;                  // separation at arm time
        private float _armZoom;                 // zoom at arm time
        private TextMeshProUGUI _debugText;

        private void Awake()
        {
            if (target == null)
            {
                var zone = GetComponent<ExplodedZoneInteraction>();
                if (zone != null) target = zone.ModelAnchor;
            }
            if (target != null)
            {
                _initialLocalRot = target.localRotation;
                _initialCaptured = true;
            }
            else
            {
                Debug.LogWarning("[TwoHandTwist] No target transform — gesture inactive.");
                enabled = false;
            }

            _panelHandle = GetComponentInChildren<PanelGrabHandle>(true);
        }

        private void OnEnable()
        {
            if (resetOnEnable && _initialCaptured)
            {
                target.localRotation = _initialLocalRot;
                _zoom = 1f;
                if (_baseScaleCaptured) target.localScale = _baseScale;
            }
            _armed = false;
        }

        // ------------------------------------------------------------------
        // Debug overlay: created lazily on the zone canvas itself so it works
        // in a device build with zero scene setup.
        // ------------------------------------------------------------------
        private void ShowDebug(string msg)
        {
            if (!debugOverlay)
            {
                if (_debugText != null) _debugText.gameObject.SetActive(false);
                return;
            }
            if (_debugText == null)
            {
                var go = new GameObject("TwistDebug", typeof(RectTransform));
                var rt = (RectTransform)go.transform;
                rt.SetParent(transform, false);
                rt.anchorMin = new Vector2(0.5f, 0f);
                rt.anchorMax = new Vector2(0.5f, 0f);
                rt.pivot = new Vector2(0.5f, 0f);
                rt.anchoredPosition = new Vector2(0f, 4f);
                rt.sizeDelta = new Vector2(330f, 30f);
                _debugText = go.AddComponent<TextMeshProUGUI>();
                if (TMP_Settings.defaultFontAsset != null)
                    _debugText.font = TMP_Settings.defaultFontAsset;
                _debugText.fontSize = 11f;
                _debugText.alignment = TextAlignmentOptions.Bottom;
                _debugText.color = new Color(0.4f, 0.9f, 1f, 0.9f);
                _debugText.raycastTarget = false;
            }
            _debugText.gameObject.SetActive(true);
            _debugText.text = msg;
        }

#if !PICO_OPENXR_SDK
        [Header("PICO hands (auto-found at runtime if left empty)")]
        [SerializeField] private PXR_Hand leftHand;
        [SerializeField] private PXR_Hand rightHand;

        // The moving, tracked point of each hand: the "RayPose" child — the
        // same transform the panel grab and UI bridge already trust.
        private Transform _leftPoint, _rightPoint;

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
            if (leftHand == null || rightHand == null)
            {
                ShowDebug("hands: NOT FOUND");
                return;
            }
            if (_leftPoint == null)  _leftPoint  = leftHand.transform.Find("RayPose");
            if (_rightPoint == null) _rightPoint = rightHand.transform.Find("RayPose");
            if (_leftPoint == null || _rightPoint == null)
            {
                ShowDebug("RayPose child: NOT FOUND");
                return;
            }

            bool lPinch = IsPinching(leftHand);
            bool rPinch = IsPinching(rightHand);
            bool panelBusy = _panelHandle != null && _panelHandle.IsGrabbing;

            if (!lPinch || !rPinch || panelBusy)
            {
                _armed = false;
                ShowDebug($"L:{(lPinch ? "PINCH" : "open")}  R:{(rPinch ? "PINCH" : "open")}{(panelBusy ? "  [panel drag]" : "")}");
                return;
            }

            // The "two fingers on the screen" of the original TwistGesture /
            // PinchGesture, promoted to world space.
            Vector3 v = _rightPoint.position - _leftPoint.position;
            v.y = 0f;                                   // yaw & zoom read the horizontal line
            float sep = v.magnitude;
            if (sep < minHandSeparation)
            {
                _armed = false;
                ShowDebug($"pinching, sep {sep:0.00}m < {minHandSeparation:0.00}m — spread hands");
                return;
            }

            float heading = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;

            if (!_armed)
            {
                // Stroke starts NOW — reference heading/separation captured, no jump.
                _armed = true;
                _prevHeading = heading;
                _armSep = sep;
                _armZoom = _zoom;
                if (!_baseScaleCaptured)
                {
                    // Anchor scale is now post-fit (zone init ran) → safe base.
                    _baseScale = target.localScale;
                    _baseScaleCaptured = true;
                }
                ShowDebug($"ARMED  sep {sep:0.00}m");
                return;
            }

            // ---- ROTATION: signed heading delta → yaw -----------------------
            if (enableRotation)
            {
                float delta = Mathf.DeltaAngle(_prevHeading, heading);
                _prevHeading = heading;
                if (Mathf.Abs(delta) <= maxDeltaPerFrame)
                    target.Rotate(0f, delta * gain, 0f, Space.World);   // world yaw = turntable
            }

            // ---- SCALE: separation ratio vs arm point → zoom ----------------
            if (enableScale)
            {
                float ratio = Mathf.Pow(sep / _armSep, zoomGain);
                _zoom = Mathf.Clamp(_armZoom * ratio, 1f, maxZoom);
                target.localScale = _baseScale * _zoom;
            }

            ShowDebug($"ACTIVE  yaw {target.localEulerAngles.y:0}°  zoom {_zoom:0.00}×  sep {sep:0.00}m");
        }

        private static bool IsPinching(PXR_Hand hand)
            => hand != null && hand.Computed && hand.Pinch;
#endif
    }
}
