using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using Unity.XR.PXR;
using ZXing;

namespace DPP.UI
{
    /// <summary>
    /// QR entry STAGE 1 — camera feasibility probe (spec 11 §4, 2026-07-20).
    ///
    /// Answers ONE question with one device build: can this app read raw
    /// passthrough camera frames on the PICO 4 Ultra via PXR_CameraImage?
    /// No ZXing, no UI design — just the pipeline with every step's result
    /// printed to a floating debug panel:
    ///
    ///   permissions → available cameras → capabilities → create device
    ///   → create capture session (RGBA8888, raw CPU buffer) → begin capture
    ///   → acquire/release loop with live frame counter.
    ///
    /// Attach to ANY GameObject in the scene (e.g. an empty "QRProbe").
    /// It builds its own debug canvas 0.8 m in front of the user at start.
    /// DELETE the GameObject when stage 1 is validated — stage 2 (ZXing)
    /// gets a proper component.
    /// </summary>
    public class QRCameraProbe : MonoBehaviour
    {
        [Tooltip("Which passthrough camera to probe.")]
        [SerializeField] private XrCameraIdPICO cameraId = XrCameraIdPICO.XR_CAMERA_ID_RGB_LEFT_PICO;

        [Tooltip("Preferred capture width; the nearest supported resolution is used.")]
        [SerializeField] private int preferredWidth = 1024;

        private const string CameraPermission = "android.permission.CAMERA";
        private const string SpatialPermission = "com.picovr.permission.SPATIAL_DATA";

        private readonly StringBuilder _log = new StringBuilder();
        private TextMeshProUGUI _text;
        private bool _capturing;
        private long _lastCaptureTime;
        private int _frames;
        private float _fpsWindowStart;
        private int _fpsWindowFrames;
        private float _measuredFps;
        private uint _w, _h, _stride, _bufSize;

        // ---- Stage 2: ZXing decode (one in-flight worker, reused buffers) ----
        private byte[] _pixels;
        private volatile bool _decodeBusy;
        private float _lastDecodeStart;
        private string _lastQR = "—";
        private long _lastDecodeMs;
        private int _decodes, _hits;
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
            BuildDebugPanel();
            Log("QR camera probe v1 (stage 1)");
#if UNITY_EDITOR
            Log("EDITOR: PXR camera API is device-only — build to the PICO.");
#else
            RequestPermissionsThenRun();
#endif
        }

        private bool _pipelineStarted;

        private void RequestPermissionsThenRun()
        {
            bool cam = Permission.HasUserAuthorizedPermission(CameraPermission);
            bool spatial = Permission.HasUserAuthorizedPermission(SpatialPermission);
            Log($"perm CAMERA: {(cam ? "granted" : "requesting…")}  SPATIAL_DATA: {(spatial ? "granted" : "requesting…")}");

            // CAMERA is the hard requirement; SPATIAL_DATA is best-effort (it
            // may be install-time or not user-toggleable — don't gate on it).
            if (cam) { StartPipelineOnce(); return; }

            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += p => Log($"perm granted: {p}");
            callbacks.PermissionDenied += p => Log($"perm DENIED: {p} — instant denial usually means the permission is missing from the APK manifest, OR must be enabled in headset Settings → Apps → this app → Permissions. Polling for a manual grant…");
            callbacks.PermissionDeniedAndDontAskAgain += p => Log($"perm DENIED (don't ask again): {p} — enable manually in headset Settings. Polling…");
            Permission.RequestUserPermissions(new[] { CameraPermission, SpatialPermission }, callbacks);

            // Grant-in-settings friendly: keep checking so a manual grant
            // starts the pipeline WITHOUT restarting the app.
            InvokeRepeating(nameof(PollPermissions), 2f, 2f);
        }

        private void PollPermissions()
        {
            if (_pipelineStarted) { CancelInvoke(nameof(PollPermissions)); return; }
            if (Permission.HasUserAuthorizedPermission(CameraPermission))
            {
                CancelInvoke(nameof(PollPermissions));
                bool spatial = Permission.HasUserAuthorizedPermission(SpatialPermission);
                Log($"CAMERA granted (via settings), SPATIAL_DATA: {(spatial ? "granted" : "not granted — proceeding anyway")} — starting pipeline");
                StartPipelineOnce();
            }
        }

        private void StartPipelineOnce()
        {
            if (_pipelineStarted) return;
            _pipelineStarted = true;
            _ = RunPipeline();
        }

        private async Task RunPipeline()
        {
            try
            {
                // 1 — enumerate cameras
                var r = PXR_CameraImage.GetAvailableCameras(out var ids);
                Log($"GetAvailableCameras: {r}  [{(ids != null ? string.Join(",", ids) : "none")}]");
                if (r != PxrResult.SUCCESS || ids == null || ids.Length == 0) return;

                // 2 — capabilities of the chosen camera
                PXR_CameraImage.GetCameraImageResolutionCapability(cameraId, out var resolutions);
                PXR_CameraImage.GetCameraImageFpsCapability(cameraId, out var fpsList);
                PXR_CameraImage.GetCameraImageFormatCapability(cameraId, out var formats);
                PXR_CameraImage.GetCameraDataTransferTypeCapability(cameraId, out var transfers);

                string resStr = "?";
                int w = 0, h = 0;
                if (resolutions != null && resolutions.Length > 0)
                {
                    var sb = new StringBuilder();
                    int bestDiff = int.MaxValue;
                    foreach (var res in resolutions)
                    {
                        sb.Append($"{res.width}x{res.height} ");
                        int diff = Mathf.Abs(res.width - preferredWidth);
                        if (diff < bestDiff) { bestDiff = diff; w = res.width; h = res.height; }
                    }
                    resStr = sb.ToString();
                }
                Log($"resolutions: {resStr}→ using {w}x{h}");
                Log($"fps: {ListOf(fpsList)}  formats: {ListOf(formats)}  transfer: {ListOf(transfers)}");
                if (w == 0) { Log("no resolutions — abort"); return; }

                var fps = XrCameraImageFpsPICO.XR_CAMERA_IMAGE_FPS_30_PICO;

                // 3 — create device (async native future)
                var devResult = await PXR_CameraImage.CreateCameraDeviceAsync(cameraId);
                Log($"CreateCameraDevice: {devResult}");
                if (devResult != PxrResult.SUCCESS) return;

                // 4 — create capture session: RGBA8888, raw CPU buffer, pinhole
                var sessResult = await PXR_CameraImage.CreateCameraCaptureSessionAsync(
                    cameraId, w, h, fps,
                    XrCameraImageFormatPICO.XR_CAMERA_IMAGE_FORMAT_RGBA_8888_PICO,
                    XrCameraDataTransferTypePICO.XR_CAMERA_DATA_TRANSFER_TYPE_RAW_BUFFER_PICO,
                    XrCameraModelPICO.XR_CAMERA_MODEL_PINHOLE_PICO);
                Log($"CreateCaptureSession: {sessResult}");
                if (sessResult != PxrResult.SUCCESS) return;

                // 5 — begin capture
                var beginResult = PXR_CameraImage.BeginCameraCapture(cameraId);
                Log($"BeginCameraCapture: {beginResult}");
                if (beginResult != PxrResult.SUCCESS) return;

                _capturing = true;
                _fpsWindowStart = Time.time;
                Log("CAPTURING — acquiring frames…");
            }
            catch (Exception e)
            {
                Log($"EXCEPTION: {e.GetType().Name}: {e.Message}");
            }
        }

        private void Update()
        {
            if (!_capturing) return;

            var r = PXR_CameraImage.AcquireCameraImage(cameraId, _lastCaptureTime, out ulong imageId, out long captureTime);
            if (r != PxrResult.SUCCESS) return;   // includes NO_UPDATE between frames
            _lastCaptureTime = captureTime;

            var dataResult = PXR_CameraImage.GetCameraImageData(cameraId, imageId, out var raw);
            if (dataResult == PxrResult.SUCCESS)
            {
                _frames++;
                _fpsWindowFrames++;
                _w = raw.width; _h = raw.height; _stride = raw.stride; _bufSize = raw.bufferSize;

                // Stage 2: hand a copy of this frame to the ZXing worker
                // (~6/sec, one in flight). Copy MUST happen before Release.
                if (!_decodeBusy && Time.time - _lastDecodeStart > 0.15f && raw.buffer != IntPtr.Zero)
                {
                    if (_pixels == null || _pixels.Length != (int)raw.bufferSize)
                        _pixels = new byte[(int)raw.bufferSize];
                    Marshal.Copy(raw.buffer, _pixels, 0, (int)raw.bufferSize);
                    _decodeBusy = true;
                    _lastDecodeStart = Time.time;
                    int dw = (int)raw.width, dh = (int)raw.height;
                    Task.Run(() => DecodeWorker(_pixels, dw, dh));
                }

                if (Time.time - _fpsWindowStart >= 1f)
                {
                    _measuredFps = _fpsWindowFrames / (Time.time - _fpsWindowStart);
                    _fpsWindowStart = Time.time;
                    _fpsWindowFrames = 0;
                }
                RefreshLiveLine();
            }
            else
            {
                Log($"GetCameraImageData: {dataResult}");
                _capturing = false;
            }

            PXR_CameraImage.ReleaseCameraImage(cameraId, imageId);
        }

        private void OnDestroy()
        {
            if (_capturing) PXR_CameraImage.EndCameraCapture(cameraId);
            PXR_CameraImage.DestroyCameraCaptureSession(cameraId);
            PXR_CameraImage.DestroyCameraDevice(cameraId);
        }

        // ------------------------------------------------------------------

        private static string ListOf<T>(T[] arr)
            => arr == null || arr.Length == 0 ? "none" : string.Join(",", arr);

        /// <summary>Background ZXing decode; result marshalled back via fields
        /// (single writer, UI reads on the main thread).</summary>
        private void DecodeWorker(byte[] pixels, int width, int height)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            string text = null;
            try
            {
                var source = new RGBLuminanceSource(pixels, width, height, RGBLuminanceSource.BitmapFormat.RGBA32);
                var result = _reader.Decode(source);
                text = result?.Text;
            }
            catch (Exception e)
            {
                text = $"<decode error: {e.GetType().Name}>";
            }
            sw.Stop();

            _lastDecodeMs = sw.ElapsedMilliseconds;
            _decodes++;
            if (!string.IsNullOrEmpty(text))
            {
                _hits++;
                _lastQR = text;
            }
            _decodeBusy = false;
        }

        private string _liveLine = "";

        private void RefreshLiveLine()
        {
            string qrColor = _lastQR != "—" ? "#5dcaa5" : "#8ba3c4";
            _liveLine = $"\n<color=#5dcaa5>FRAMES OK  {_w}x{_h}  stride {_stride}  buf {_bufSize / 1024} KB  #{_frames}  ~{_measuredFps:0} fps</color>" +
                        $"\n<color={qrColor}>QR: {_lastQR}   decodes {_decodes} · hits {_hits} · last {_lastDecodeMs} ms</color>";
            if (_text != null) _text.text = _log + _liveLine;
        }

        private void Log(string line)
        {
            _log.AppendLine(line);
            Debug.Log($"[QRProbe] {line}");
            if (_text != null) _text.text = _log + _liveLine;
        }

        /// <summary>Self-contained floating panel: 0.5 m wide, 0.8 m ahead.</summary>
        private void BuildDebugPanel()
        {
            var head = Camera.main != null ? Camera.main.transform : null;

            var go = new GameObject("QRProbePanel", typeof(RectTransform), typeof(Canvas));
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            var rt = (RectTransform)go.transform;
            rt.sizeDelta = new Vector2(500f, 380f);
            rt.localScale = Vector3.one * 0.001f;

            if (head != null)
            {
                Vector3 fwd = head.forward; fwd.y = 0f; fwd.Normalize();
                rt.position = head.position + fwd * 0.8f + Vector3.up * -0.05f;
                rt.rotation = Quaternion.LookRotation(fwd, Vector3.up);
            }

            var bgGO = new GameObject("BG", typeof(RectTransform));
            var bg = (RectTransform)bgGO.transform;
            bg.SetParent(rt, false);
            bg.anchorMin = Vector2.zero; bg.anchorMax = Vector2.one;
            bg.offsetMin = Vector2.zero; bg.offsetMax = Vector2.zero;
            var img = bgGO.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.03f, 0.06f, 0.12f, 0.92f);

            var txtGO = new GameObject("Text", typeof(RectTransform));
            var txt = (RectTransform)txtGO.transform;
            txt.SetParent(rt, false);
            txt.anchorMin = Vector2.zero; txt.anchorMax = Vector2.one;
            txt.offsetMin = new Vector2(14f, 14f); txt.offsetMax = new Vector2(-14f, -14f);
            _text = txtGO.AddComponent<TextMeshProUGUI>();
            if (TMP_Settings.defaultFontAsset != null) _text.font = TMP_Settings.defaultFontAsset;
            _text.fontSize = 13f;
            _text.color = new Color(0.86f, 0.89f, 0.94f);
            _text.alignment = TextAlignmentOptions.TopLeft;
            _text.textWrappingMode = TextWrappingModes.Normal;
            _text.richText = true;
        }
    }
}
