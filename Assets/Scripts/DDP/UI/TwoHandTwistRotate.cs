using UnityEngine;
using TMPro;

namespace DPP.UI
{
    /// <summary>
    /// Zone rotation + scale v4.2 — SEPARATION BANDS, 2026-07-20.
    ///
    /// Two-hand gesture (both hands pinching); the line between the two
    /// RayPose points drives everything, but rotation and zoom are now
    /// EXCLUSIVE, split by hand separation (Thiago's band design):
    ///
    ///   minHandSeparation .. zoomThreshold  →  ROTATION band
    ///       Twist = yaw (heading delta of the hand line, world-Y).
    ///       Zoom is frozen — no size breathing while rotating.
    ///
    ///   beyond zoomThreshold               →  ZOOM band
    ///       Rotation stops. Separation is an ABSOLUTE zoom dial:
    ///       zoomThreshold = 1× (default fit), zoomFullSeparation = maxZoom.
    ///       Spread wider → bigger; close back toward the threshold → smaller.
    ///       Hand distance = model size, always — fully deterministic.
    ///
    ///   ~1 cm hysteresis at the border so the mode doesn't flicker, and the
    ///   model GLIDES toward the mapped zoom (zoomResponse) instead of
    ///   snapping when the hands re-enter the zoom band at a different level.
    ///
    /// Yaw-only rotation by design; manipulates the MODEL ANCHOR, never the
    /// clone, so the constrained-body engine's local axes stay untouched.
    /// v4.1 (simultaneous map-app mode) is preserved behind the
    /// useSeparationBands toggle in case the band feel is rejected.
    /// </summary>
    public class TwoHandTwistRotate : MonoBehaviour
    {
        /// <summary>Which mechanism the current hand posture drives (v4.3 —
        /// read by ZoneGestureHUD to highlight the active row).</summary>
        public enum GestureBand { Idle, Rotate, Zoom }

        // ---- Live state for the gesture HUD (read-only from outside) ----
        public bool LeftPinching  { get; private set; }
        public bool RightPinching { get; private set; }
        public float Separation   { get; private set; }          // metres; 0 when not tracked
        public float CurrentZoom  => _zoom;
        public float CurrentYaw   => target != null ? target.localEulerAngles.y : 0f;
        public GestureBand CurrentBand { get; private set; } = GestureBand.Idle;

        /// <summary>While true (help modal open) the gesture updates state for
        /// the HUD chips but never rotates/scales the model.</summary>
        public bool Paused { get; set; }

        /// <summary>Set by ZonePartInteraction while a part is selected, held
        /// or dragged — part manipulation also uses two-pinch postures, so the
        /// twist/zoom must stand down to avoid cross-fire.</summary>
        public bool ExternallyBlocked { get; set; }

        [Header("Target")]
        [Tooltip("Transform that gets yawed/scaled. Auto-filled from ExplodedZoneInteraction.ModelAnchor on the same GameObject if empty.")]
        [SerializeField] private Transform target;

        [Header("Mode")]
        [Tooltip("ON (v4.2): rotation 5–25cm, zoom beyond 25cm, exclusive. OFF (v4.1): both run simultaneously, ratchet-based zoom.")]
        [SerializeField] private bool useSeparationBands = true;

        [SerializeField] private bool enableRotation = true;
        [SerializeField] private bool enableScale = true;

        [Header("Rotation")]
        [Tooltip("Degrees of model yaw per degree of hand twist. 1 = 1:1.")]
        [SerializeField] private float gain = 1f;

        [Tooltip("Per-frame deltas above this (degrees) are dropped as hand-tracking spikes, not intent.")]
        [SerializeField] private float maxDeltaPerFrame = 12f;

        [Header("Zoom bands (metres)")]
        [Tooltip("Hands closer than this don't manipulate at all.")]
        [SerializeField] private float minHandSeparation = 0.05f;

        [Tooltip("Band border: below = rotation only, above = zoom only. Zoom is 1× exactly here.")]
        [SerializeField] private float zoomThreshold = 0.25f;

        [Tooltip("Separation at which zoom reaches maxZoom. The dial runs zoomThreshold→this.")]
        [SerializeField] private float zoomFullSeparation = 0.55f;

        [Tooltip("Border hysteresis so the mode doesn't flicker at the threshold.")]
        [SerializeField] private float bandHysteresis = 0.01f;

        [Header("Zoom feel")]
        [Tooltip("Upper zoom limit as a multiple of the default size. Lower limit is always 1×.")]
        [SerializeField] private float maxZoom = 2f;

        [Tooltip("How fast the model glides toward the dialed zoom (per second). Prevents snaps on band re-entry.")]
        [SerializeField] private float zoomResponse = 5f;

        [Tooltip("Restore the model's default facing AND default zoom every time the zone screen opens.")]
        [SerializeField] private bool resetOnEnable = true;

        [Header("Debug")]
        [Tooltip("Raw dev readout at the bottom of the zone canvas. Superseded by the gesture HUD column (v4.3) — keep OFF unless debugging.")]
        [SerializeField] private bool debugOverlay = false;

        private PanelGrabHandle _panelHandle;   // don't manipulate while the panel itself is being dragged
        private Quaternion _initialLocalRot;
        private bool _initialCaptured;

        // Base scale = the anchor's scale AFTER ExplodedZoneInteraction's fit
        // (captured lazily on the first gesture frame, when init is done).
        private Vector3 _baseScale;
        private bool _baseScaleCaptured;
        private float _zoom = 1f;               // current zoom, 1..maxZoom

        private bool _armed;
        private bool _inZoomBand;
        private float _prevHeading;
        private float _armSep;                  // v4.1 ratchet mode only
        private float _armZoom;                 // v4.1 ratchet mode only
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
            _inZoomBand = false;
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

            // HUD state: chips always live, even when the gesture won't act.
            LeftPinching = lPinch;
            RightPinching = rPinch;

            if (!lPinch || !rPinch || panelBusy || Paused || ExternallyBlocked)
            {
                _armed = false;
                _inZoomBand = false;
                Separation = 0f;
                CurrentBand = GestureBand.Idle;
                ShowDebug($"L:{(lPinch ? "PINCH" : "open")}  R:{(rPinch ? "PINCH" : "open")}{(panelBusy ? "  [panel drag]" : "")}{(Paused ? "  [paused]" : "")}{(ExternallyBlocked ? "  [part session]" : "")}");
                return;
            }

            Vector3 v = _rightPoint.position - _leftPoint.position;
            v.y = 0f;                                   // yaw & zoom read the horizontal line
            float sep = v.magnitude;
            Separation = sep;
            if (sep < minHandSeparation)
            {
                _armed = false;
                _inZoomBand = false;
                CurrentBand = GestureBand.Idle;
                ShowDebug($"pinching, sep {sep:0.00}m < {minHandSeparation:0.00}m — spread hands");
                return;
            }

            float heading = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;

            if (!_armed)
            {
                _armed = true;
                _prevHeading = heading;
                _armSep = sep;
                _armZoom = _zoom;
                _inZoomBand = sep > zoomThreshold;      // start in whichever band the hands are in
                if (!_baseScaleCaptured)
                {
                    // Anchor scale is now post-fit (zone init ran) → safe base.
                    _baseScale = target.localScale;
                    _baseScaleCaptured = true;
                }
                ShowDebug($"ARMED  sep {sep:0.00}m");
                return;
            }

            if (useSeparationBands)
            {
                UpdateBands(sep, heading);
                CurrentBand = _inZoomBand ? GestureBand.Zoom : GestureBand.Rotate;
            }
            else
            {
                UpdateSimultaneous(sep, heading);       // v4.1 fallback
                CurrentBand = GestureBand.Rotate;       // both run; HUD shows rotate as primary
            }
        }

        // ---- v4.2: exclusive bands split at zoomThreshold -------------------
        private void UpdateBands(float sep, float heading)
        {
            // Hysteresis: cross the border only when clearly past it.
            if (_inZoomBand)  { if (sep < zoomThreshold - bandHysteresis) _inZoomBand = false; }
            else              { if (sep > zoomThreshold + bandHysteresis) _inZoomBand = true;  }

            float delta = Mathf.DeltaAngle(_prevHeading, heading);
            _prevHeading = heading;                     // track always → no jump on band exit

            if (_inZoomBand)
            {
                if (enableScale)
                {
                    // Absolute dial: threshold = 1×, zoomFullSeparation = maxZoom.
                    float t = Mathf.InverseLerp(zoomThreshold, zoomFullSeparation, sep);
                    float dialed = Mathf.Lerp(1f, maxZoom, t);
                    // Glide, don't snap — matters when re-entering the band
                    // while the model is at a different zoom level.
                    _zoom = Mathf.MoveTowards(_zoom, dialed, zoomResponse * Time.deltaTime * Mathf.Max(0.25f, Mathf.Abs(dialed - _zoom)));
                    target.localScale = _baseScale * _zoom;
                }
                ShowDebug($"ZOOM band  sep {sep:0.00}m  zoom {_zoom:0.00}×");
            }
            else
            {
                if (enableRotation && Mathf.Abs(delta) <= maxDeltaPerFrame)
                    target.Rotate(0f, delta * gain, 0f, Space.World);   // world yaw = turntable
                ShowDebug($"ROTATE band  sep {sep:0.00}m  yaw {target.localEulerAngles.y:0}°  zoom {_zoom:0.00}× (held)");
            }
        }

        // ---- v4.1 fallback: both dimensions at once, ratchet zoom -----------
        private void UpdateSimultaneous(float sep, float heading)
        {
            if (enableRotation)
            {
                float delta = Mathf.DeltaAngle(_prevHeading, heading);
                _prevHeading = heading;
                if (Mathf.Abs(delta) <= maxDeltaPerFrame)
                    target.Rotate(0f, delta * gain, 0f, Space.World);
            }
            if (enableScale)
            {
                _zoom = Mathf.Clamp(_armZoom * (sep / _armSep), 1f, maxZoom);
                target.localScale = _baseScale * _zoom;
            }
            ShowDebug($"ACTIVE  yaw {target.localEulerAngles.y:0}°  zoom {_zoom:0.00}×  sep {sep:0.00}m");
        }

        private static bool IsPinching(PXR_Hand hand)
            => hand != null && hand.Computed && hand.Pinch;
#endif
    }
}
