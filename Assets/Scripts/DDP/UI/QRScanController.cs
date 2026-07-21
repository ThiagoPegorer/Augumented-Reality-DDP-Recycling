using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using Unity.XR.PXR;
using ZXing;

namespace DPP.UI
{
    /// <summary>
    /// QR entry — stages 3+4 (spec 11, mock qr_scan_screen_v1 approved
    /// 2026-07-21). Owns the app's entry flow:
    ///
    ///   launch → scan screen (camera + ZXing, proven in QRCameraProbe)
    ///          → "dpp:&lt;id&gt;" decoded → camera stops → Found beat (~1 s)
    ///          → DPPManager.FetchAndPopulate(id) → main canvas + main page.
    ///
    /// States: Scanning (sweep line + Searching…, demo-fallback button fades
    /// in after fallbackSeconds) · Found (check + loading) · BackendError
    /// (Retry / Scan again — backend unreachable is a DIFFERENT failure from
    /// scan failure and says so). One-scan-per-launch: after success the
    /// camera is destroyed for the session.
    ///
    /// Editor: no camera — after a short beat the demo path runs automatically
    /// so Play Mode still reaches the main page. scanOnStart=false disables QR
    /// entirely (study kill-switch): main canvas stays up, DPPManager's
    /// fetchOnStart applies as before.
    /// </summary>
    public class QRScanController : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private DPPManager manager;
        [SerializeField] private ScreenRouter router;
        [Tooltip("Root of the main 640×430 canvas — hidden until a passport is loaded.")]
        [SerializeField] private GameObject mainCanvasRoot;

        [Header("Scan screen groups")]
        [SerializeField] private GameObject scanGroup;
        [SerializeField] private GameObject foundGroup;
        [SerializeField] private GameObject errorGroup;
        [SerializeField] private RectTransform sweepLine;
        [SerializeField] private TMP_Text searchingLabel;
        [SerializeField] private Button demoButton;
        [SerializeField] private CanvasGroup demoButtonGroup;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button scanAgainButton;

        [Header("Head follow (v2 viewfinder — no grabber bar)")]
        [Tooltip("The scan panel lazily follows the gaze: always in front, smoothed so it drifts rather than sticks (hard head-lock is uncomfortable).")]
        [SerializeField] private bool followHead = true;
        [SerializeField] private float followDistance = 0.75f;
        [SerializeField] private float followHeightOffset = -0.05f;
        [Tooltip("Positional catch-up speed, /s. Lower = floatier.")]
        [SerializeField] private float followLerp = 4f;

        [Header("Behaviour")]
        [Tooltip("Master switch: OFF = no QR entry, app behaves as before (DPPManager.fetchOnStart).")]
        [SerializeField] private bool scanOnStart = true;
        [SerializeField] private string demoProductId = "vcu_001";
        [Tooltip("Seconds without a decode before the demo fallback fades in.")]
        [SerializeField] private float fallbackSeconds = 10f;
        [Tooltip("Confirmation beat between decode and page open, seconds.")]
        [SerializeField] private float foundBeatSeconds = 1.0f;
        [SerializeField] private XrCameraIdPICO cameraId = XrCameraIdPICO.XR_CAMERA_ID_RGB_LEFT_PICO;
        [SerializeField] private int preferredWidth = 1024;
        [Tooltip("Sweep travel half-height inside the scan frame, canvas px.")]
        [SerializeField] private float sweepHalfHeight = 50f;

        private const string CameraPermission = "android.permission.CAMERA";

        private enum State { Idle, Scanning, Found, Fetching, BackendError, Done }
        private State _state = State.Idle;
        private float _stateTime;
        private string _pendingProductId;
        private bool _cameraLive;
        private long _lastCaptureTime;

        // ZXing worker (one in flight)
        private byte[] _pixels;
        private volatile bool _decodeBusy;
        private volatile string _decodedText;
        private float _lastDecodeStart;
        private readonly BarcodeReader _reader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new[] { BarcodeFormat.QR_CODE },
            },
        };

        // ------------------------------------------------------------------

        private void Start()
        {
            if (demoButton != null) demoButton.onClick.AddListener(UseDemoUnit);
            if (retryButton != null) retryButton.onClick.AddListener(RetryFetch);
            if (scanAgainButton != null) scanAgainButton.onClick.AddListener(RestartScan);
            if (manager != null) manager.FetchCompleted += OnFetchCompleted;

            if (!scanOnStart)
            {
                gameObject.SetActive(false);          // QR disabled — legacy entry
                return;
            }

            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(false);
            EnterState(State.Scanning);

#if UNITY_EDITOR
            // No camera in the Editor — auto-continue with the demo unit so
            // Play Mode still reaches the main page.
            Invoke(nameof(UseDemoUnit), 1.5f);
#else
            StartCameraWhenPermitted();
#endif
        }

        private void OnDestroy()
        {
            if (manager != null) manager.FetchCompleted -= OnFetchCompleted;
            StopCamera();
        }

        /// <summary>Re-enter the scan flow mid-session (post-report popup:
        /// "Scan new QR code"). Reactivates the scan screen where the user
        /// parked it, hides the main canvas, and rebuilds the camera session.</summary>
        public void BeginNewScan()
        {
            gameObject.SetActive(true);
            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(false);
            _decodedText = null;
            EnterState(State.Scanning);
#if UNITY_EDITOR
            Invoke(nameof(UseDemoUnit), 1.5f);
#else
            StartCameraWhenPermitted();
#endif
        }

        private void EnterState(State s)
        {
            _state = s;
            _stateTime = 0f;
            if (scanGroup != null) scanGroup.SetActive(s == State.Scanning);
            if (foundGroup != null) foundGroup.SetActive(s == State.Found || s == State.Fetching);
            if (errorGroup != null) errorGroup.SetActive(s == State.BackendError);
            if (s == State.Scanning && demoButtonGroup != null)
            {
                demoButtonGroup.alpha = 0f;
                demoButtonGroup.interactable = false;
                demoButtonGroup.blocksRaycasts = false;
            }
        }

        private void LateUpdate()
        {
            if (!followHead) return;
            var head = Camera.main;
            if (head == null) return;

            Vector3 fwd = head.transform.forward;
            fwd.y = 0f;
            if (fwd.sqrMagnitude < 1e-6f) return;
            fwd.Normalize();

            Vector3 target = head.transform.position + fwd * followDistance + Vector3.up * followHeightOffset;
            float t = 1f - Mathf.Exp(-followLerp * Time.deltaTime);   // framerate-independent lazy follow
            transform.position = Vector3.Lerp(transform.position, target, t);

            Quaternion look = Quaternion.LookRotation(fwd, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, t);
        }

        private void Update()
        {
            _stateTime += Time.deltaTime;

            switch (_state)
            {
                case State.Scanning:
                    UpdateScanVisuals();
                    PumpCamera();
                    ConsumeDecode();
                    break;

                case State.Found:
                    if (_stateTime >= foundBeatSeconds)
                    {
                        EnterState(State.Fetching);
                        if (manager != null) manager.FetchAndPopulate(_pendingProductId);
                        else OnFetchCompleted(false);
                    }
                    break;
            }
        }

        // ---- scanning visuals -------------------------------------------

        private void UpdateScanVisuals()
        {
            if (sweepLine != null)
            {
                float y = Mathf.PingPong(_stateTime * 55f, sweepHalfHeight * 2f) - sweepHalfHeight;
                var p = sweepLine.anchoredPosition;
                p.y = y;
                sweepLine.anchoredPosition = p;
            }
            if (searchingLabel != null)
            {
                var c = searchingLabel.color;
                c.a = 0.55f + 0.45f * Mathf.Sin(_stateTime * 2.5f).Remap();
                searchingLabel.color = c;
            }
            if (demoButtonGroup != null && _stateTime > fallbackSeconds && demoButtonGroup.alpha < 1f)
            {
                demoButtonGroup.alpha = Mathf.MoveTowards(demoButtonGroup.alpha, 1f, Time.deltaTime * 2f);
                bool on = demoButtonGroup.alpha > 0.5f;
                demoButtonGroup.interactable = on;
                demoButtonGroup.blocksRaycasts = on;
            }
        }

        // ---- decode consumption -----------------------------------------

        private void ConsumeDecode()
        {
            string text = _decodedText;
            if (string.IsNullOrEmpty(text)) return;
            _decodedText = null;

            if (!text.StartsWith("dpp:", StringComparison.OrdinalIgnoreCase)) return;  // foreign QR — keep scanning
            string id = text.Substring(4).Trim();
            if (id.Length == 0) return;

            _pendingProductId = id;
            StopCamera();                              // one-scan-per-launch
            EnterState(State.Found);
        }

        private void UseDemoUnit()
        {
            _pendingProductId = demoProductId;
            StopCamera();
            EnterState(State.Found);
        }

        private void RetryFetch()
        {
            EnterState(State.Fetching);
            if (foundGroup != null) foundGroup.SetActive(true);
            if (manager != null) manager.FetchAndPopulate(_pendingProductId);
        }

        private void RestartScan()
        {
            EnterState(State.Scanning);
#if !UNITY_EDITOR
            StartCameraWhenPermitted();
#else
            Invoke(nameof(UseDemoUnit), 1.5f);
#endif
        }

        private void OnFetchCompleted(bool ok)
        {
            if (_state != State.Fetching) return;
            if (ok)
            {
                EnterState(State.Done);
                if (mainCanvasRoot != null) mainCanvasRoot.SetActive(true);
                if (router != null) router.ShowMainPage();
                gameObject.SetActive(false);           // scan screen retires for this session
            }
            else
            {
                EnterState(State.BackendError);
            }
        }

        // ---- camera pipeline (as proven by QRCameraProbe) ----------------

        private void StartCameraWhenPermitted()
        {
            if (Permission.HasUserAuthorizedPermission(CameraPermission))
            {
                _ = StartCamera();
                return;
            }
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += p => { if (!_cameraLive) _ = StartCamera(); };
            Permission.RequestUserPermission(CameraPermission, callbacks);
            InvokeRepeating(nameof(PollPermission), 2f, 2f);
        }

        private void PollPermission()
        {
            if (_cameraLive) { CancelInvoke(nameof(PollPermission)); return; }
            if (Permission.HasUserAuthorizedPermission(CameraPermission))
            {
                CancelInvoke(nameof(PollPermission));
                _ = StartCamera();
            }
        }

        private async Task StartCamera()
        {
            try
            {
                var r = PXR_CameraImage.GetAvailableCameras(out var ids);
                if (r != PxrResult.SUCCESS || ids == null || ids.Length == 0) return;

                PXR_CameraImage.GetCameraImageResolutionCapability(cameraId, out var resolutions);
                int w = 0, h = 0, bestDiff = int.MaxValue;
                if (resolutions != null)
                    foreach (var res in resolutions)
                    {
                        int diff = Mathf.Abs(res.width - preferredWidth);
                        if (diff < bestDiff) { bestDiff = diff; w = res.width; h = res.height; }
                    }
                if (w == 0) return;

                if (await PXR_CameraImage.CreateCameraDeviceAsync(cameraId) != PxrResult.SUCCESS) return;
                var sess = await PXR_CameraImage.CreateCameraCaptureSessionAsync(
                    cameraId, w, h, XrCameraImageFpsPICO.XR_CAMERA_IMAGE_FPS_30_PICO,
                    XrCameraImageFormatPICO.XR_CAMERA_IMAGE_FORMAT_RGBA_8888_PICO,
                    XrCameraDataTransferTypePICO.XR_CAMERA_DATA_TRANSFER_TYPE_RAW_BUFFER_PICO,
                    XrCameraModelPICO.XR_CAMERA_MODEL_PINHOLE_PICO);
                if (sess != PxrResult.SUCCESS) return;
                if (PXR_CameraImage.BeginCameraCapture(cameraId) != PxrResult.SUCCESS) return;

                _lastCaptureTime = 0;
                _cameraLive = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[QRScan] camera start failed: {e.Message}");
            }
        }

        private void PumpCamera()
        {
            if (!_cameraLive) return;

            var r = PXR_CameraImage.AcquireCameraImage(cameraId, _lastCaptureTime, out ulong imageId, out long captureTime);
            if (r != PxrResult.SUCCESS) return;
            _lastCaptureTime = captureTime;

            if (PXR_CameraImage.GetCameraImageData(cameraId, imageId, out var raw) == PxrResult.SUCCESS &&
                !_decodeBusy && Time.time - _lastDecodeStart > 0.15f && raw.buffer != IntPtr.Zero)
            {
                if (_pixels == null || _pixels.Length != (int)raw.bufferSize)
                    _pixels = new byte[(int)raw.bufferSize];
                Marshal.Copy(raw.buffer, _pixels, 0, (int)raw.bufferSize);
                _decodeBusy = true;
                _lastDecodeStart = Time.time;
                int dw = (int)raw.width, dh = (int)raw.height;
                Task.Run(() => DecodeWorker(_pixels, dw, dh));
            }

            PXR_CameraImage.ReleaseCameraImage(cameraId, imageId);
        }

        private void DecodeWorker(byte[] pixels, int width, int height)
        {
            try
            {
                var source = new RGBLuminanceSource(pixels, width, height, RGBLuminanceSource.BitmapFormat.RGBA32);
                var result = _reader.Decode(source);
                if (!string.IsNullOrEmpty(result?.Text)) _decodedText = result.Text;
            }
            catch (Exception) { /* keep scanning */ }
            _decodeBusy = false;
        }

        private void StopCamera()
        {
            if (!_cameraLive) return;
            _cameraLive = false;
            PXR_CameraImage.EndCameraCapture(cameraId);
            PXR_CameraImage.DestroyCameraCaptureSession(cameraId);
            PXR_CameraImage.DestroyCameraDevice(cameraId);
        }
    }

    internal static class QRScanMath
    {
        /// <summary>Maps sin's [-1,1] to [0,1] for alpha pulsing.</summary>
        public static float Remap(this float sinValue) => (sinValue + 1f) * 0.5f;
    }
}
