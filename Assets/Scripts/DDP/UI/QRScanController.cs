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
    /// QR entry — spec 11, updated for ReBuilt v2.0 (spec 12, 2026-07-29).
    ///
    /// RBv1.0 flow (superseded): launch → scan screen → decode → main page.
    /// RBv2.0 flow:
    ///
    ///   LAUNCH APP → WELCOME (WelcomeController)
    ///              → CONTINUE BUTTON → BeginNewScan()
    ///              → "dpp:&lt;id&gt;" decoded → camera stops → Found beat (~1 s)
    ///              → DPPManager.FetchAndPopulate(id)
    ///              → FIRST TIME USING THE APP? prompt → main canvas + main page.
    ///
    /// Three changes vs RBv1.0:
    ///   1. waitForWelcome — the controller no longer starts scanning at launch;
    ///      WelcomeController owns entry and calls BeginNewScan().
    ///   2. The user-facing 10 s "Continue with demo unit" fallback is REMOVED
    ///      (Thiago, 2026-07-29: entry must be QR-only). The scanOnStart flag
    ///      remains as the operator-level study-day kill-switch, and the
    ///      Editor-only auto-continue below is a dev convenience that cannot
    ///      exist on device.
    ///   3. On a successful fetch the first-run prompt is shown instead of
    ///      opening the main canvas directly.
    ///
    /// States: Scanning (sweep line + Searching…) · Found (check + loading) ·
    /// BackendError (Retry / Scan again — backend unreachable is a DIFFERENT
    /// failure from scan failure and says so).
    /// </summary>
    public class QRScanController : MonoBehaviour
    {
        [Header("Wiring (set by builder)")]
        [SerializeField] private DPPManager manager;
        [SerializeField] private ScreenRouter router;
        [Tooltip("Root of the main 640×430 canvas — hidden until a passport is loaded.")]
        [SerializeField] private GameObject mainCanvasRoot;
        [Tooltip("RBv2.0: asked after every successful scan, before the main canvas opens. Null = open the main canvas directly (RBv1.0 behaviour).")]
        [SerializeField] private FirstRunPrompt firstRunPrompt;

        [Header("Scan screen groups")]
        [SerializeField] private GameObject scanGroup;
        [SerializeField] private GameObject foundGroup;
        [SerializeField] private GameObject errorGroup;
        [SerializeField] private RectTransform sweepLine;
        [SerializeField] private TMP_Text searchingLabel;
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
        [Tooltip("Master switch: OFF = no QR entry, app behaves as before (DPPManager.fetchOnStart). Operator-level study-day kill-switch.")]
        [SerializeField] private bool scanOnStart = true;
        [Tooltip("RBv2.0: ON = the Welcome canvas owns launch; this screen stays down until BeginNewScan(). OFF = RBv1.0 behaviour (scan at launch).")]
        [SerializeField] private bool waitForWelcome = true;
        [SerializeField] private string demoProductId = "vcu_001";
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
            if (retryButton != null) retryButton.onClick.AddListener(RetryFetch);
            if (scanAgainButton != null) scanAgainButton.onClick.AddListener(RestartScan);
            if (manager != null) manager.FetchCompleted += OnFetchCompleted;

            if (!scanOnStart)
            {
                gameObject.SetActive(false);          // QR disabled — legacy entry
                return;
            }

            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(false);

            // RBv2.0: the Welcome canvas is the app's first screen. Stay down
            // until its Continue button calls BeginNewScan(). Listeners above
            // are already registered, so re-activation is safe.
            if (waitForWelcome)
            {
                gameObject.SetActive(false);
                return;
            }

            EnterState(State.Scanning);

#if UNITY_EDITOR
            // No camera in the Editor — auto-continue with the demo unit so
            // Play Mode still reaches the main page. Editor-only: this path
            // cannot exist on device, so entry stays QR-only for participants.
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

        /// <summary>Enter the scan flow: from the Welcome canvas' Continue
        /// button, and mid-session from the post-report "Scan new QR code" loop.
        /// Reactivates the scan screen, hides the main canvas, and rebuilds the
        /// camera session.</summary>
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
            StopCamera();                              // one scan per cycle
            EnterState(State.Found);
        }

        /// <summary>Editor-only entry so Play Mode reaches the main page without
        /// a camera. Never reachable on device — the user-facing fallback button
        /// was removed in RBv2.0.</summary>
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
            if (!ok)
            {
                EnterState(State.BackendError);
                return;
            }

            EnterState(State.Done);
            gameObject.SetActive(false);               // scan screen retires for this cycle

            // RBv2.0: ask the first-run question before opening the passport.
            if (firstRunPrompt != null)
            {
                firstRunPrompt.Show();
                return;
            }

            // RBv1.0 fallback (prompt not wired).
            if (mainCanvasRoot != null) mainCanvasRoot.SetActive(true);
            if (router != null) router.ShowMainPage();
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
