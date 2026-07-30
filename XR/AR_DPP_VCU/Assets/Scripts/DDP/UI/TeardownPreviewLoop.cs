using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// Live 3D teardown preview for Screen 03 (spec 03 §5.2 final stage).
    ///
    /// While the intro screen is active, drives the real VCU_assembly through
    /// an endless explode → hold → reassemble → hold cycle and films it with a
    /// dedicated preview camera into a RenderTexture shown by the RawImage in
    /// the old PNG slot. OnDisable everything stops and the model snaps back
    /// to the assembled pose, so the step flow always starts clean.
    ///
    /// The camera frames the model automatically from a ¾ view each time the
    /// screen is enabled, so it works wherever the model sits in the scene.
    ///
    /// NOTE (background capture): the preview camera renders with a fully
    /// transparent background but sees every layer by default. If scene props
    /// ever appear behind the model in the preview, put VCU_assembly (and
    /// children) on a dedicated layer and set that layer as this camera's
    /// culling mask in the Inspector.
    /// </summary>
    public class TeardownPreviewLoop : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private DisassemblyAnimator vcuAnimator;
        [SerializeField] private Camera previewCamera;
        [SerializeField] private RawImage target;

        [Header("Loop timing (s)")]
        [SerializeField] private float holdExploded  = 1.5f;
        [SerializeField] private float holdAssembled = 1.5f;

        [Header("Framing")]
        [Tooltip("Frame the connector face (device front) automatically from the animator's detected axis.")]
        [SerializeField] private bool autoFrameToConnectors = true;
        [Tooltip("Extra horizontal rotation of the auto-framed view, in degrees. Tune freely (90 / 180 / 270 / −45 …).")]
        [SerializeField] private float yawOffsetDeg = 0f;
        [Tooltip("Aim the camera this many metres ABOVE the assembled centre — the model sits lower in frame, giving the rising lid/screws headroom. ≈ (top rise − shell drop) / 2.")]
        [SerializeField] private float frameHeightBias = 0.06f;
        [Tooltip("Manual fallback: direction the camera looks FROM, in world space (used when auto-frame is off).")]
        [SerializeField] private Vector3 viewDirection = new Vector3(1f, 0.8f, -1f);
        [Tooltip("Distance multiplier so the exploded extents stay in frame. Lower = model fills more of the slot.")]
        [SerializeField] private float frameFactor = 1.5f;
        [SerializeField] private float fieldOfView = 30f;

        private RenderTexture _rt;
        private Coroutine _loop;

        private void OnEnable()
        {
            if (vcuAnimator == null) vcuAnimator = FindFirstObjectByType<DisassemblyAnimator>();
            if (vcuAnimator == null || previewCamera == null || target == null)
            {
                Debug.LogWarning("[TeardownPreviewLoop] Missing wiring (animator/camera/target) — preview disabled.");
                return;
            }

            EnsureRenderTexture();
            FrameModel();

            previewCamera.targetTexture = _rt;
            previewCamera.enabled = true;
            target.texture = _rt;
            target.enabled = true;

            vcuAnimator.ResetInstant();
            _loop = StartCoroutine(Loop());
        }

        private void OnDisable()
        {
            if (_loop != null) { StopCoroutine(_loop); _loop = null; }
            if (vcuAnimator != null) vcuAnimator.ResetInstant();   // assembled for the step flow
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
                yield return vcuAnimator.RunFullTeardown();
                yield return new WaitForSeconds(holdExploded);
                vcuAnimator.Reassemble();
                yield return new WaitForSeconds(vcuAnimator.ReassembleDuration + holdAssembled);
            }
        }

        /// <summary>RT sized to the RawImage rect (×2 for sharpness at AR scale).</summary>
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

        /// <summary>Places the preview camera on the ¾ view axis, fitted to the
        /// model's ASSEMBLED bounds × frameFactor (room for the explosion).</summary>
        private void FrameModel()
        {
            var renderers = vcuAnimator.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;

            var bounds = renderers[0].bounds;
            foreach (var r in renderers) bounds.Encapsulate(r.bounds);

            float radius = bounds.extents.magnitude * frameFactor;
            float dist = radius / Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);

            // Trimetric view matching the NX exploded render: connector wall
            // turned toward the viewer, connectors exiting toward lower-left,
            // ~28° elevation so the lid/board/chips stack stays readable.
            Vector3 dir;
            if (autoFrameToConnectors)
            {
                Vector3 front = vcuAnimator.ConnectorAxisWorld.normalized;
                Vector3 side = Vector3.Cross(front, Vector3.up).normalized; // front → screen-left
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
            previewCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);   // transparent → floats on navy
        }
    }
}
