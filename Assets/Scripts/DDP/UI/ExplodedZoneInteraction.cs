using UnityEngine;

namespace DPP.UI
{
    /// <summary>
    /// Exploded action zone — RESET (v3.0, 2026-07-19).
    ///
    /// After on-device testing, the v2.x mechanism set (rotation arcs + knobs,
    /// zoom slider, part list, axis arrow, hover cues) was judged not
    /// user-friendly and removed wholesale. The zone now shows ONLY:
    ///   - the runtime-cloned model (solid, assembled), and
    ///   - the grabber bar to move the panel in AR space.
    ///
    /// The constrained-body engine (ConstrainedTeardownModel: bodies, axes,
    /// travel limits, dependencies, snap, screw spin) is still built on the
    /// clone and fully functional — dormant, ready for whatever interaction
    /// mechanism replaces v2.x. This component now only manages the clone's
    /// lifecycle.
    /// </summary>
    public class ExplodedZoneInteraction : MonoBehaviour
    {
        [Header("Model")]
        [Tooltip("The original VCU_assembly (has DisassemblyAnimator). Auto-found if empty.")]
        [SerializeField] private Transform modelSource;
        [Tooltip("Anchor inside the zone canvas the clone is parented to (scale compensates the canvas 0.001).")]
        [SerializeField] private Transform modelAnchor;
        [Range(0.2f, 1.5f)]
        [SerializeField] private float modelFitScale = 0.9f;

        [Header("Grab handle (circle pinned below the model's front face)")]
        [SerializeField] private RectTransform grabHandle;
        [Tooltip("Gap between the model's bottom and the handle, metres.")]
        [SerializeField] private float handleGapBelow = 0.035f;
        [Tooltip("How far beyond the model's front face the handle sits, metres.")]
        [SerializeField] private float handleGapFront = 0.015f;   // tuned on device 2026-07-19

        private ConstrainedTeardownModel _model;
        private Vector3 _anchorBaseScale;
        private Renderer[] _modelRenderers;

        /// <summary>The transform the clone hangs from — gesture components
        /// (e.g. TwoHandTwistRotate) rotate/scale this, never the clone itself,
        /// so the constrained-body engine's local axes stay untouched.</summary>
        public Transform ModelAnchor => modelAnchor;

        private void OnEnable()
        {
            if (_model == null)
            {
                if (modelSource == null)
                {
                    var animator = FindFirstObjectByType<DisassemblyAnimator>();
                    if (animator != null) modelSource = animator.transform;
                }
                if (modelSource == null || modelAnchor == null)
                {
                    Debug.LogWarning("[ExplodedZone] Missing model source or anchor — zone inactive.");
                    enabled = false;
                    return;
                }
                _model = ConstrainedTeardownModel.CreateFrom(modelSource, modelAnchor);
                _anchorBaseScale = modelAnchor.localScale * modelFitScale;
                modelAnchor.localScale = _anchorBaseScale;
                _modelRenderers = _model.GetComponentsInChildren<Renderer>(true);
            }

            _model.ResetInstant();
            _model.transform.localRotation = Quaternion.identity;
        }

        private void OnDisable()
        {
            if (_model != null) _model.ResetInstant();
        }

        /// <summary>Pins the circular grab handle just below the model's bottom
        /// edge on the side FACING the user — it orbits with the viewpoint, so
        /// it always reads as attached to the model's front and never hides
        /// behind it (the old bar sat static on the canvas plane, behind the
        /// model).</summary>
        private void LateUpdate()
        {
            if (grabHandle == null || _model == null || _modelRenderers == null || _modelRenderers.Length == 0) return;
            var head = Camera.main;
            if (head == null) return;

            Bounds b = _modelRenderers[0].bounds;
            for (int i = 1; i < _modelRenderers.Length; i++) b.Encapsulate(_modelRenderers[i].bounds);

            // Horizontal direction from the model to the user.
            Vector3 toHead = head.transform.position - b.center;
            toHead.y = 0f;
            if (toHead.sqrMagnitude < 1e-6f) toHead = -transform.forward;
            toHead.Normalize();

            // AABB support: how far the box extends toward the user along toHead.
            float frontExtent = Mathf.Abs(toHead.x) * b.extents.x + Mathf.Abs(toHead.z) * b.extents.z;

            Vector3 pos = b.center + toHead * (frontExtent + handleGapFront);
            pos.y = b.min.y - handleGapBelow;
            grabHandle.position = pos;

            // Billboard the circle to the user so its hit plane faces the ray.
            Vector3 face = head.transform.position - pos;
            if (face.sqrMagnitude > 1e-6f)
                grabHandle.rotation = Quaternion.LookRotation(-face.normalized, Vector3.up);
        }
    }
}
