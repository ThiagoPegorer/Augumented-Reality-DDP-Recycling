using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 device round 4 (Thiago, 2026-08-08): while the model is FREE the
    /// gesture column leaves the stage and travels WITH the model — spec 10 §3.2's
    /// follower behaviour, reduced to what the stage needs. LINKED, the column
    /// sits at its built pose on the stage canvas. FREE, every LateUpdate pins it
    /// beside the model's live bounds — offset by the bounds' extent along the
    /// viewer's right, so zooming pushes it outward and it never overlaps the
    /// mesh — and yaw-billboards it to the user.
    ///
    /// The column stays a CHILD of the stage canvas even while following: a
    /// world-space canvas renders and raycasts its children at their transforms,
    /// on-plane or not, and keeping the parent means the column hides with the
    /// rig exactly like everything else. SuperPanelView drives the state; this
    /// component never asks who is LINKED.
    /// </summary>
    public class StageGestureHudFollower : MonoBehaviour
    {
        [Tooltip("The model the column follows while FREE — the stage pivot (ModelPivot).")]
        [SerializeField] private Transform model;

        [Tooltip("Gap between the model's side and the column, in metres.")]
        [SerializeField] private float gap = 0.055f;

        [Tooltip("Round 7: OFF = pin beside the model (the gesture column). ON = pin at its " +
                 "FRONT-BOTTOM (the freed model's drag bar), zone §3.1 style.")]
        [SerializeField] private bool followBelow = false;

        [Tooltip("Round 8: cap on how far below the bounds centre the drag bar may hang. At 2× " +
                 "zoom a strictly-below bar dropped out of the user's view entirely.")]
        [SerializeField] private float maxDrop = 0.14f;

        [Tooltip("ON for objects that only exist while FREE (the drag-bar canvas): follow " +
                 "whenever enabled, no SetFree handshake, no stage park pose.")]
        [SerializeField] private bool alwaysFollow = false;

        private Vector3 _homeLocalPos;
        private Quaternion _homeLocalRot;
        private bool _free;

        private void Awake()
        {
            _homeLocalPos = transform.localPosition;
            _homeLocalRot = transform.localRotation;
        }

        private void OnEnable()
        {
            // The rig can be hidden and reshown while the view force-relocks in
            // its own OnEnable; parking here makes the order irrelevant.
            if (!_free) Park();
        }

        /// <summary>Driven by SuperPanelView.Unlock / ReLock.</summary>
        public void SetFree(bool free)
        {
            _free = free;
            if (!free) Park();
        }

        private void Park()
        {
            transform.localPosition = _homeLocalPos;
            transform.localRotation = _homeLocalRot;
        }

        private void LateUpdate()
        {
            if (!(alwaysFollow || _free) || model == null) return;
            var cam = Camera.main;
            if (cam == null) return;

            var rends = model.GetComponentsInChildren<Renderer>(true);
            if (rends.Length == 0) return;
            Bounds b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);

            if (followBelow)
            {
                // Zone §3.1: the grab affordance pins to the model's FRONT-BOTTOM —
                // in front of the mesh toward the user, low but never gone. Round 8:
                // strictly-below placement dropped out of view at 2× zoom, so the
                // drop is capped and the bar rides the camera-facing front face
                // (a grab affordance SHOULD slide to face the hand that grabs it).
                Vector3 toCam = cam.transform.position - b.center;
                toCam.y = 0f;
                toCam = toCam.sqrMagnitude < 1e-6f ? Vector3.back : toCam.normalized;
                float front = Mathf.Abs(b.extents.x * toCam.x) + Mathf.Abs(b.extents.z * toCam.z);
                float drop = Mathf.Min(b.extents.y, maxDrop) + gap;
                transform.position = b.center + toCam * (front + 0.015f) + Vector3.down * drop;
            }
            else
            {
                // Round 9 — the zone's §3.2 geometry, verbatim this time: pinned
                // to the model's FRONT SIDE EDGE. Same camera-facing plane as the
                // drag bar (the round-8 model-frame anchor orbited with the
                // model's yaw — column at YAW 269° ended up behind the unit), at
                // the viewer's right of the bounds, offsets from the live AABB so
                // zoom pushes it outward and the gap to the model stays constant.
                Vector3 toCam = cam.transform.position - b.center;
                toCam.y = 0f;
                toCam = toCam.sqrMagnitude < 1e-6f ? Vector3.back : toCam.normalized;
                Vector3 side = Vector3.Cross(toCam, Vector3.up).normalized;   // viewer's right
                float sideExtent  = Mathf.Abs(b.extents.x * side.x)  + Mathf.Abs(b.extents.z * side.z);
                float frontExtent = Mathf.Abs(b.extents.x * toCam.x) + Mathf.Abs(b.extents.z * toCam.z);
                transform.position = b.center
                    + side * (sideExtent + gap)
                    + toCam * (frontExtent + 0.015f);
            }

            // Yaw-only billboard: face the user without pitching with them —
            // a bar that leans when the user crouches reads as broken. This is
            // what keeps the drag bar FRONTAL instead of lateral (round 7).
            Vector3 d = transform.position - cam.transform.position;
            d.y = 0f;
            if (d.sqrMagnitude > 1e-6f)
                transform.rotation = Quaternion.LookRotation(d.normalized, Vector3.up);
        }
    }
}
