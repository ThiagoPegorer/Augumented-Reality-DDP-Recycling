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
//   Each frame it also drives an optional pointer reticle per hand: the dot
//   appears at the ray's hit point on a clickable Button, hides over empty
//   space, and flashes briefly on a successful click.
//
// What you wire in the Editor:
//   1. Add this component to any persistent GameObject (e.g. XR Interaction
//      Manager).
//   2. Drag the PXR_Hand component from HandLeft  → leftHand.
//   3. Drag the PXR_Hand component from HandRight → rightHand.
//   4. (Optional) Drag two small sphere GameObjects into Left Reticle / Right
//      Reticle. They appear/move/recolor automatically.
//   5. Make sure your DashboardCanvas is World Space and has a regular
//      GraphicRaycaster (NOT TrackedDeviceGraphicRaycaster — we go through
//      EventSystem.RaycastAll which uses the standard GraphicRaycaster).
//   6. EventSystem can keep its default Standalone Input Module; we don't
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
using UnityEngine.UI;

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

        [Header("Pointer reticles (visual cursor at the ray's canvas hit point)")]
        [Tooltip("Small Transform that follows the LEFT ray's canvas hit point. Hidden when not over a button. Optional.")]
        [SerializeField] private Transform leftReticle;

        [Tooltip("Small Transform that follows the RIGHT ray's canvas hit point. Hidden when not over a button. Optional.")]
        [SerializeField] private Transform rightReticle;

        [Tooltip("Reticle color when hovering over a clickable button (not pinching).")]
        [SerializeField] private Color hoverColor = new Color(1f, 1f, 1f, 0.85f);

        [Tooltip("Reticle color briefly shown on a successful pinch (visual confirmation).")]
        [SerializeField] private Color pinchColor = new Color(1f, 0.8f, 0.2f, 1f);

        [Tooltip("Seconds the reticle stays in pinch color after a click before returning to hover color.")]
        [SerializeField] private float pinchFlashSeconds = 0.18f;

        [Tooltip("Small offset toward the camera, in meters, so the reticle doesn't z-fight with the canvas surface.")]
        [SerializeField] private float reticleSurfaceOffset = 0.003f;

        [Header("Diagnostics")]
        [Tooltip("If true, prints a status snapshot once per second to Logcat. Turn off after debugging.")]
        [SerializeField] private bool verboseLogging = true;

        // ---- runtime state ----
        private bool _leftWasPinching, _rightWasPinching;
        private float _nextLogTime;
        private Renderer _leftReticleRenderer, _rightReticleRenderer;
        private MaterialPropertyBlock _mpb;
        private float _leftPinchFlashUntil, _rightPinchFlashUntil;

        void Awake()
        {
            _mpb = new MaterialPropertyBlock();
            if (leftReticle != null)  _leftReticleRenderer  = leftReticle.GetComponentInChildren<Renderer>();
            if (rightReticle != null) _rightReticleRenderer = rightReticle.GetComponentInChildren<Renderer>();
        }

        void Update()
        {
            UpdateHand(leftHand,  ref _leftWasPinching,  ref _leftPinchFlashUntil,  leftReticle,  _leftReticleRenderer,  "LEFT");
            UpdateHand(rightHand, ref _rightWasPinching, ref _rightPinchFlashUntil, rightReticle, _rightReticleRenderer, "RIGHT");

            if (verboseLogging && Time.unscaledTime >= _nextLogTime)
            {
                _nextLogTime = Time.unscaledTime + 1f;
                LogStatus("LEFT",  leftHand);
                LogStatus("RIGHT", rightHand);
            }
        }

        private void LogStatus(string label, PXR_Hand hand)
        {
            if (hand == null) { Debug.Log($"[PicoBridge] {label}: hand reference is NULL — drag the [Building Block] PICO Hand Tracking GameObject into the slot."); return; }
            Debug.Log($"[PicoBridge] {label}: Computed={hand.Computed} RayValid={hand.RayValid} Pinch={hand.Pinch} PinchStrength={hand.PinchStrength:F2}");
        }

        // Per-frame logic for one hand:
        //   1. Compute where the hand's ray hits a clickable Button (if anywhere).
        //   2. Drive the pointer reticle: shown + positioned on hit, hidden otherwise.
        //   3. On pinch rising edge, fire the button's OnClick and flash the reticle.
        private void UpdateHand(PXR_Hand hand, ref bool wasPinching, ref float pinchFlashUntil,
                                Transform reticle, Renderer reticleRenderer, string label)
        {
            // Default: reticle hidden until proven we have a valid hit.
            bool wantReticleVisible = false;
            Vector3 reticlePos = Vector3.zero;
            Quaternion reticleRot = Quaternion.identity;

            if (hand == null)            { SetReticle(reticle, reticleRenderer, false, default, default, 0f, ref pinchFlashUntil); wasPinching = false; return; }
            if (!hand.Computed)          { SetReticle(reticle, reticleRenderer, false, default, default, 0f, ref pinchFlashUntil); wasPinching = false; return; }
            if (!hand.RayValid)          { SetReticle(reticle, reticleRenderer, false, default, default, 0f, ref pinchFlashUntil); wasPinching = false; return; }

            Transform rayPose = hand.transform.Find("RayPose");
            if (rayPose == null)         { SetReticle(reticle, reticleRenderer, false, default, default, 0f, ref pinchFlashUntil); wasPinching = false; return; }

            Vector3 origin = rayPose.position;
            Vector3 direction = rayPose.forward;

            // Find the closest clickable button (if any) along this ray.
            // Same logic as the pinch path — extracted so the reticle and the
            // click decision both use the identical hit result.
            CanvasHit hit = FindClickableHit(origin, direction);
            if (hit.hasHit)
            {
                wantReticleVisible = true;
                // Push the reticle a few mm toward the camera along the canvas
                // normal so it doesn't z-fight with the button graphic.
                Vector3 normal = hit.canvas.transform.forward * -1f;
                reticlePos = hit.worldPoint + normal * reticleSurfaceOffset;
                reticleRot = Quaternion.LookRotation(-normal, hit.canvas.transform.up);
            }

            // ---- pinch edge detection ----
            bool nowPinching = hand.Pinch;
            bool risingEdge = nowPinching && !wasPinching;
            wasPinching = nowPinching;

            if (risingEdge)
            {
                if (verboseLogging) Debug.Log($"[PicoBridge] {label}: PINCH RISING EDGE detected.");

                if (hit.hasHit)
                {
                    // Fire the click on the button under the ray.
                    DeliverClick(hit, label);
                    pinchFlashUntil = Time.unscaledTime + pinchFlashSeconds;
                }
                else
                {
                    // No UI button under ray — try 3D physics (VCU mesh later).
                    if (Physics.Raycast(origin, direction, out RaycastHit phys, maxRayDistance, raycastMask, QueryTriggerInteraction.Collide))
                    {
                        if (verboseLogging) Debug.Log($"[PicoBridge] {label}: 3D hit on '{phys.collider.name}' at {phys.point}.");
                        OnPinch3D?.Invoke(hand, phys);
                    }
                    else if (verboseLogging)
                    {
                        Debug.Log($"[PicoBridge] {label}: pinch detected, but raycast hit nothing (UI nor 3D). Origin={origin} Dir={direction}");
                    }
                }
            }

            SetReticle(reticle, reticleRenderer, wantReticleVisible, reticlePos, reticleRot,
                       Time.unscaledTime < pinchFlashUntil ? 1f : 0f, ref pinchFlashUntil);
        }

        // Sets the reticle's transform + visibility + color in one place.
        // `flashAmount` is 1 right after a click and 0 otherwise; we use it to
        // pick between pinchColor (flashing) and hoverColor (steady).
        private void SetReticle(Transform reticle, Renderer reticleRenderer, bool visible,
                                Vector3 pos, Quaternion rot, float flashAmount, ref float _)
        {
            if (reticle == null) return;
            if (reticle.gameObject.activeSelf != visible) reticle.gameObject.SetActive(visible);
            if (!visible) return;
            reticle.SetPositionAndRotation(pos, rot);
            if (reticleRenderer != null)
            {
                reticleRenderer.GetPropertyBlock(_mpb);
                Color c = flashAmount > 0f ? pinchColor : hoverColor;
                _mpb.SetColor("_BaseColor", c);
                _mpb.SetColor("_Color", c);
                reticleRenderer.SetPropertyBlock(_mpb);
            }
        }

        // Result of scanning all World Space Canvases for a clickable hit.
        private struct CanvasHit
        {
            public bool hasHit;
            public Canvas canvas;
            public GameObject target;     // the GameObject to send the click to
            public Vector3 worldPoint;    // world-space hit on the canvas plane
            public float distance;        // along the ray
        }

        // Pure query: where does this ray hit a clickable button? No side effects.
        // Used by both the reticle (every frame) and the click logic (on pinch).
        private CanvasHit FindClickableHit(Vector3 origin, Vector3 direction)
        {
            CanvasHit best = default;
            best.distance = float.PositiveInfinity;

            Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            for (int c = 0; c < canvases.Length; c++)
            {
                Canvas canvas = canvases[c];
                if (canvas == null || !canvas.isActiveAndEnabled) continue;
                if (canvas.renderMode != RenderMode.WorldSpace) continue;

                Transform t = canvas.transform;
                Plane plane = new Plane(-t.forward, t.position);
                Ray ray = new Ray(origin, direction);
                if (!plane.Raycast(ray, out float dist)) continue;
                if (dist < 0f || dist > maxRayDistance) continue;

                Vector3 worldHit = ray.GetPoint(dist);
                Graphic g = FindGraphicAt(canvas, worldHit);
                if (g == null) continue;

                if (dist < best.distance)
                {
                    best.hasHit = true;
                    best.canvas = canvas;
                    best.target = g.gameObject;
                    best.worldPoint = worldHit;
                    best.distance = dist;
                }
            }
            return best;
        }

        // Fires pointerClickHandler on the hit's target.
        private void DeliverClick(CanvasHit hit, string label)
        {
            if (EventSystem.current == null)
            {
                if (verboseLogging) Debug.LogWarning($"[PicoBridge] {label}: EventSystem.current is null. The scene needs an EventSystem GameObject.");
                return;
            }

            Camera eventCam = hit.canvas != null ? hit.canvas.worldCamera : null;
            if (eventCam == null) eventCam = Camera.main;
            Vector2 screenForPed = eventCam != null ? (Vector2)eventCam.WorldToScreenPoint(hit.worldPoint) : Vector2.zero;
            PointerEventData ped = new PointerEventData(EventSystem.current) { position = screenForPed };

            GameObject handled = ExecuteEvents.ExecuteHierarchy(hit.target, ped, ExecuteEvents.pointerClickHandler);
            if (handled != null)
            {
                if (verboseLogging) Debug.Log($"[PicoBridge] {label}: pointer click delivered to '{handled.name}'.");
            }
            else if (verboseLogging)
            {
                Debug.LogWarning($"[PicoBridge] {label}: hit graphic '{hit.target.name}' but ExecuteEvents.ExecuteHierarchy returned null.");
            }
        }

        // Returns the topmost CLICKABLE Graphic on the given Canvas whose
        // RectTransform contains the given world point. A graphic is
        // considered clickable if it (or any ancestor) has an
        // IPointerClickHandler — which is true for Unity UI Buttons and for
        // any custom component that listens for clicks. Plain decorative
        // Images (like a panel background with raycastTarget=true but no
        // listener) are skipped, so they don't block clicks to actual buttons.
        private static readonly List<Graphic> _graphicBuf = new List<Graphic>();
        private static Graphic FindGraphicAt(Canvas canvas, Vector3 worldPoint)
        {
            _graphicBuf.Clear();
            canvas.GetComponentsInChildren(false, _graphicBuf);
            Graphic best = null;
            int bestOrder = int.MinValue;
            for (int i = 0; i < _graphicBuf.Count; i++)
            {
                Graphic g = _graphicBuf[i];
                if (!g.raycastTarget) continue;
                RectTransform rt = g.rectTransform;
                // Project world point into the rect's local plane.
                Vector3 local = rt.InverseTransformPoint(worldPoint);
                Rect r = rt.rect;
                if (local.x < r.xMin || local.x > r.xMax) continue;
                if (local.y < r.yMin || local.y > r.yMax) continue;
                // Only consider this graphic if it (or an ancestor) actually
                // handles pointer clicks. This skips decorative panels.
                if (!HasClickHandler(g.transform)) continue;
                int order = ComputeDepth(g.transform);
                if (order > bestOrder)
                {
                    bestOrder = order;
                    best = g;
                }
            }
            return best;
        }

        // Returns true if the given Transform or any ancestor has an
        // IPointerClickHandler (which Button implements).
        private static bool HasClickHandler(Transform t)
        {
            while (t != null)
            {
                var handler = t.GetComponent<IPointerClickHandler>();
                if (handler != null) return true;
                t = t.parent;
            }
            return false;
        }

        // Cheap depth metric: walk the hierarchy and combine sibling indices.
        // Bigger = drawn later = on top.
        private static int ComputeDepth(Transform t)
        {
            int depth = 0;
            while (t != null)
            {
                depth = depth * 1000 + t.GetSiblingIndex();
                t = t.parent;
            }
            return depth;
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
