using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Per-step "HOW TO" animation loop for the step flow (spec 04 v3).
    ///
    /// While the step flow is active, plays ONLY the current step's motion on
    /// the real VCU_assembly, on repeat: reset → snap earlier steps to their
    /// end state (their parts are already out) → play step n → hold → repeat.
    /// Filmed by the shared TeardownPreviewCamera into a RenderTexture shown
    /// by the RawImage in the how-to slot.
    ///
    /// The intro's TeardownPreviewLoop and this component share the camera and
    /// the model — never active at the same time (ScreenRouter guarantees one
    /// screen at a time, and each OnDisable resets the model + releases the
    /// camera).
    /// </summary>
    public class StepHowToLoop : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private DisassemblyAnimator vcuAnimator;
        [SerializeField] private Camera previewCamera;
        [SerializeField] private RawImage target;

        [Header("Loop timing (s)")]
        [SerializeField] private float holdBefore = 0.4f;   // beat before the motion starts
        [SerializeField] private float holdAfter  = 1.0f;   // beat on the completed state

        [Header("Framing (matches the intro loop)")]
        [SerializeField] private bool autoFrameToConnectors = true;
        [SerializeField] private float yawOffsetDeg = 0f;
        [SerializeField] private float frameHeightBias = 0.06f;
        [SerializeField] private float frameFactor = 1.5f;
        [SerializeField] private float fieldOfView = 30f;
        [Tooltip("Manual fallback when auto-frame is off.")]
        [SerializeField] private Vector3 viewDirection = new Vector3(1f, 0.8f, -1f);

        private int _step = 1;
        private RenderTexture _rt;
        private Coroutine _loop;

        /// <summary>1-based step whose motion should loop. Restarts the loop if running.</summary>
        public void SetStep(int oneBasedStep)
        {
            _step = Mathf.Clamp(oneBasedStep, 1, 5);
            if (isActiveAndEnabled && _loop != null)
            {
                StopCoroutine(_loop);
                if (vcuAnimator != null) vcuAnimator.SetStepFocus(_step);
                _loop = StartCoroutine(Loop());
            }
        }

        private void OnEnable()
        {
            if (vcuAnimator == null) vcuAnimator = FindFirstObjectByType<DisassemblyAnimator>();
            if (vcuAnimator == null || previewCamera == null || target == null)
            {
                Debug.LogWarning("[StepHowToLoop] Missing wiring (animator/camera/target) — how-to preview disabled.");
                return;
            }

            vcuAnimator.ResetInstant();
            vcuAnimator.SetStepFocus(_step);   // ghost non-relevant parts
            EnsureRenderTexture();
            FrameModel();

            previewCamera.targetTexture = _rt;
            previewCamera.enabled = true;
            target.texture = _rt;
            target.enabled = true;

            _loop = StartCoroutine(Loop());
        }

        private void OnDisable()
        {
            if (_loop != null) { StopCoroutine(_loop); _loop = null; }
            if (vcuAnimator != null)
            {
                vcuAnimator.ResetInstant();
                vcuAnimator.ClearFocus();      // restore full opacity for intro/AR view
            }
            if (previewCamera != null)
            {
                previewCamera.enabled = false;
                previewCamera.targetTexture = null;
            }
            if (target != null) target.enabled = false;
            if (_rt != null) { _rt.Release(); Destroy(_rt); _rt = null; }
        }

        private IEnumerator Loop()
        {
            while (true)
            {
                vcuAnimator.ResetInstant();
                for (int s = 1; s < _step; s++) vcuAnimator.ApplyStepInstant(s);
                yield return new WaitForSeconds(holdBefore);
                yield return vcuAnimator.RunStep(_step);
                yield return new WaitForSeconds(holdAfter);
            }
        }

        private void EnsureRenderTexture()
        {
            var rect = target.rectTransform.rect;
            int w = Mathf.Max(64, Mathf.RoundToInt(rect.width) * 2);
            int h = Mathf.Max(64, Mathf.RoundToInt(rect.height) * 2);
            if (_rt != null && (_rt.width != w || _rt.height != h)) { _rt.Release(); Destroy(_rt); _rt = null; }
            if (_rt == null)
            {
                _rt = new RenderTexture(w, h, 24, RenderTextureFormat.ARGB32) { antiAliasing = 4 };
                _rt.Create();
            }
        }

        private void FrameModel()
        {
            var renderers = vcuAnimator.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;

            var bounds = renderers[0].bounds;
            foreach (var r in renderers) bounds.Encapsulate(r.bounds);

            float radius = bounds.extents.magnitude * frameFactor;
            float dist = radius / Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);

            Vector3 dir;
            if (autoFrameToConnectors)
            {
                Vector3 front = vcuAnimator.ConnectorAxisWorld.normalized;
                Vector3 side = Vector3.Cross(front, Vector3.up).normalized;
                Vector3 horizontal = Quaternion.AngleAxis(yawOffsetDeg, Vector3.up)
                                     * (front * 0.9f + side * 0.55f);
                dir = (horizontal + Vector3.up * 0.55f).normalized;
            }
            else dir = viewDirection.normalized;

            Vector3 lookTarget = bounds.center + Vector3.up * frameHeightBias;
            previewCamera.transform.position = lookTarget + dir * dist;
            previewCamera.transform.LookAt(lookTarget);
            previewCamera.fieldOfView = fieldOfView;
            previewCamera.nearClipPlane = Mathf.Max(0.01f, dist - radius * 2f);
            previewCamera.farClipPlane = dist + radius * 2f;
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
        }
    }
}
