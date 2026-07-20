using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Gesture HUD for the exploded action zone (v4.3, mock zone_status_bar_v3
    /// approved 2026-07-20): a vertical column pinned to the model's front-left
    /// (positioned by ExplodedZoneInteraction) showing, top to bottom:
    ///
    ///   [?]  help button → opens the gesture-guide modal
    ///   L/R  hand lights: solid green = pinching, dim ring = open
    ///   YAW  current model yaw (°)
    ///   DIST live hand separation (m)
    ///   ZOOM current zoom (×)
    ///
    /// The ACTIVE mechanism's row is tinted with the blue accent: YAW in the
    /// rotation band, DIST+ZOOM in the zoom band. While the modal is open,
    /// TwoHandTwistRotate.Paused is set so reading the guide can't spin the
    /// model. All UI objects are created by the builder; this script only
    /// binds values and wires the two buttons.
    /// </summary>
    public class ZoneGestureHUD : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private TwoHandTwistRotate twist;
        [SerializeField] private ExplodedZoneInteraction zone;

        [Header("Hand chips (On = green solid, Off = dim ring)")]
        [SerializeField] private GameObject leftOn;
        [SerializeField] private GameObject leftOff;
        [SerializeField] private GameObject rightOn;
        [SerializeField] private GameObject rightOff;

        [Header("Value rows (caption + value per row)")]
        [SerializeField] private TMP_Text yawCap;
        [SerializeField] private TMP_Text yawValue;
        [SerializeField] private TMP_Text distCap;
        [SerializeField] private TMP_Text distValue;
        [SerializeField] private TMP_Text zoomCap;
        [SerializeField] private TMP_Text zoomValue;

        [Header("Help modal")]
        [SerializeField] private Button helpButton;
        [SerializeField] private GameObject helpModal;
        [SerializeField] private Button modalCloseButton;

        // Palette (matches the approved mock / DPP theme).
        private static readonly Color ValNormal = new Color(0.86f, 0.89f, 0.94f);   // #dbe4f0
        private static readonly Color CapNormal = new Color(0.365f, 0.451f, 0.588f);// #5d7396
        private static readonly Color Active    = new Color(0.30f, 0.64f, 1f);      // #4da3ff

        private void Awake()
        {
            if (twist == null) twist = GetComponentInParent<TwoHandTwistRotate>();
            if (zone == null) zone = GetComponentInParent<ExplodedZoneInteraction>();
            if (helpButton != null) helpButton.onClick.AddListener(OpenModal);
            if (modalCloseButton != null) modalCloseButton.onClick.AddListener(CloseModal);
        }

        private void OnEnable()
        {
            CloseModal();   // never re-enter the zone with a stale open modal
        }

        private void OpenModal()
        {
            if (helpModal != null) helpModal.SetActive(true);
            if (twist != null) twist.Paused = true;
            // Modal state: hide model + handle + column so the panel owns the
            // zone (a 3D mesh always occludes world-space UI otherwise).
            if (zone != null) zone.SetSuppressed(true);
        }

        private void CloseModal()
        {
            if (helpModal != null) helpModal.SetActive(false);
            if (twist != null) twist.Paused = false;
            if (zone != null) zone.SetSuppressed(false);
        }

        private void Update()
        {
            if (twist == null) return;

            bool l = twist.LeftPinching, r = twist.RightPinching;
            if (leftOn  != null) leftOn.SetActive(l);
            if (leftOff != null) leftOff.SetActive(!l);
            if (rightOn  != null) rightOn.SetActive(r);
            if (rightOff != null) rightOff.SetActive(!r);

            if (yawValue != null)
                yawValue.text = $"{Mathf.RoundToInt(twist.CurrentYaw)}°";
            if (distValue != null)
                distValue.text = twist.Separation > 0f ? $"{twist.Separation:0.00} m" : "—";
            if (zoomValue != null)
                zoomValue.text = $"{twist.CurrentZoom:0.00}×";

            var band = twist.CurrentBand;
            bool rot = band == TwoHandTwistRotate.GestureBand.Rotate;
            bool zoom = band == TwoHandTwistRotate.GestureBand.Zoom;

            Tint(yawCap, yawValue, rot);
            Tint(distCap, distValue, zoom);
            Tint(zoomCap, zoomValue, zoom);
        }

        private static void Tint(TMP_Text cap, TMP_Text val, bool active)
        {
            if (cap != null) cap.color = active ? Active : CapNormal;
            if (val != null) val.color = active ? Active : ValNormal;
        }
    }
}
