// Lets PICO 4 native hand tracking click Unity UI buttons.
//
// Why this exists:
//   PICO's "PICO Hand Tracking" Building Block adds HandLeft/HandRight prefabs
//   that render the ray + drive the hand mesh + expose Pinch via PXR_Hand,
//   exactly mirroring the PICO Home system UX. What PICO does NOT ship is the
//   integration layer that turns a pinch into a Unity UI click — that's left
//   to the app. This bridge fills that gap: each frame it casts the PICO ray
//   into the scene and, on the rising edge of Pinch, fires the standard
//   pointerClickHandler / IPointerClickHandler / Button.onClick handlers
//   at the hit point.
//
// What you wire in the Editor:
//   1. Add this component to any persistent GameObject (e.g. XR Interaction
//      Manager).
//   2. Drag the PXR_Hand component from HandLeft  → leftHand.
//   3. Drag the PXR_Hand component from HandRight → rightHand.
//   4. Make sure your DashboardCanvas is World Space and has a regular
//      GraphicRaycaster (NOT TrackedDeviceGraphicRaycaster — we go through
//      EventSystem.RaycastAll which uses the standard GraphicRaycaster).
//   5. EventSystem can keep its default Standalone Input Module; we don't
//      depend on the XR UI Input Module.
//
// 3D hits:
//   The bridge also exposes UnityEvents that fire on pinch over any
//   non-UI collider, so when the VCU CAD lands you can wire those events
//   to ExplosionController.Explode/Collapse without writing more code.

#if !PICO_OPENXR_SDK
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DPP
{
    [DisallowMultipleComponent]
    public class PicoHandUIBridge : MonoBehaviour
    {
        [System.Serializable]
        public class HandHitEvent : UnityEvent<PXR_Hand, RaycastHit> { }

        [Header("PICO hand prefabs in the scene")]
        [Tooltip("Drag the PXR_Hand component from HandLeft here.")]
        [SerializeField] private PXR_Hand leftHand;

        [Tooltip("Drag the PXR_Hand component from HandRight here.")]
        [SerializeField] private PXR_Hand rightHand;

        [Header("Ray reach")]
        [Tooltip("How far the pinch ray reaches into the scene, in meters. PICO Home uses ~3-5m.")]
        [SerializeField] private float maxRayDistance = 5f;

        [Tooltip("Physics layers the 3D pinch raycast considers. Default = everything.")]
        [SerializeField] private LayerMask raycastMask = ~0;

        [Header("Optional: fires on pinch over a 3D collider (e.g. the VCU mesh)")]
        public HandHitEvent OnPinch3D = new HandHitEvent();

        // ---- runtime state ----
        private bool _leftWasPinching, _rightWasPinching;
        private static readonly List<RaycastResult> _uiBuf = new List<RaycastResult>();

        void Update()
        {
            Step(leftHand,  ref _leftWasPinching);
            Step(rightHand, ref _rightWasPinching);
        }

        private void Step(PXR_Hand hand, ref bool wasPinching)
        {
            if (hand == null) return;
            if (!hand.Computed || !hand.RayValid) { wasPinching = false; return; }

            bool nowPinching = hand.Pinch;
            bool risingEdge = nowPinching && !wasPinching;
            wasPinching = nowPinching;
            if (!risingEdge) return;

            // PICO writes the world-space ray into the `RayPose` child Transform
            // each frame. Use it directly — it's already in world coordinates
            // because the child sits under Camera Offset.
            Transform rayPose = hand.transform.Find("RayPose");
            if (rayPose == null) return;
            Vector3 origin = rayPose.position;
            Vector3 direction = rayPose.forward;

            // Try UI first. EventSystem.RaycastAll covers every Canvas with a
            // GraphicRaycaster in the scene.
            if (TryFireUIClick(origin, direction)) return;

            // Fall through to 3D physics so app logic (e.g. pinch-on-mesh)
            // can react too.
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxRayDistance, raycastMask, QueryTriggerInteraction.Collide))
            {
                OnPinch3D?.Invoke(hand, hit);
            }
        }

        // Sends a synthetic pointer-click through the EventSystem at the screen
        // position corresponding to where the PICO ray hits any world-space
        // Canvas. Returns true if a click was delivered.
        private bool TryFireUIClick(Vector3 origin, Vector3 direction)
        {
            if (EventSystem.current == null) return false;

            // We project the ray endpoint into every camera that renders the
            // hit canvas via the standard GraphicRaycaster path. To do this
            // robustly we shoot the ray, intersect with each candidate
            // Canvas's plane, and convert that 3D hit into the relevant
            // camera's screen point. The cheap version below works for the
            // common case: one main camera, one or two world-space Canvases.
            Camera cam = Camera.main;
            if (cam == null) return false;

            // Pick a far point along the ray and project it to screen space.
            // That gives EventSystem.RaycastAll a screen position to use; the
            // GraphicRaycaster will then do its own ray-vs-canvas intersection
            // from the camera through that pixel.
            Vector3 farPoint = origin + direction * maxRayDistance;
            Vector3 screen = cam.WorldToScreenPoint(farPoint);
            if (screen.z <= 0f) return false; // canvas behind us

            PointerEventData ped = new PointerEventData(EventSystem.current) { position = screen };
            _uiBuf.Clear();
            EventSystem.current.RaycastAll(ped, _uiBuf);

            for (int i = 0; i < _uiBuf.Count; i++)
            {
                GameObject go = _uiBuf[i].gameObject;
                // pointerClickHandler dispatches to Button.onClick AND to any
                // IPointerClickHandler component.
                GameObject handled = ExecuteEvents.ExecuteHierarchy(go, ped, ExecuteEvents.pointerClickHandler);
                if (handled != null) return true;
            }
            return false;
        }

        // Editor-only sanity draw so you can see the rays in the Scene view.
        void OnDrawGizmosSelected()
        {
            DrawRay(leftHand, Color.cyan);
            DrawRay(rightHand, new Color(1f, 0.6f, 0.2f));
        }

        private void DrawRay(PXR_Hand hand, Color c)
        {
            if (hand == null) return;
            Transform rp = hand.transform.Find("RayPose");
            if (rp == null) return;
            Gizmos.color = c;
            Gizmos.DrawLine(rp.position, rp.position + rp.forward * maxRayDistance);
        }
    }
}
#endif
