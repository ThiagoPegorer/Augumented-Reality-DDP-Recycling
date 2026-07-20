using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DPP
{
    /// <summary>
    /// HTTP client for the FastAPI DPP backend. Attach this to a persistent GameObject in the scene.
    /// </summary>
    public class DPPClient : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Backend base URL. Use http://localhost:8000 for editor testing, LAN IP or ngrok URL for PICO 4.")]
        private string baseUrl = "http://localhost:8000";

        [SerializeField]
        [Tooltip("Request timeout in seconds. Unity's default is 0 = wait FOREVER, which froze the summary screen when the backend was unreachable (2026-07-20). Never leave this at 0.")]
        private int timeoutSeconds = 10;

        public string BaseUrl
        {
            get => baseUrl;
            set => baseUrl = value;
        }

        /// <summary>
        /// POST a recovery report JSON to /dpp/{product_id}/report (v0.5, spec 09 §7).
        /// </summary>
        public IEnumerator PostReport(string productId, string jsonBody, Action onSuccess, Action<string> onError)
        {
            string url = $"{baseUrl}/dpp/{productId}/report";

            using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(body);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = Mathf.Max(1, timeoutSeconds);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                    onSuccess?.Invoke();
                else
                    onError?.Invoke($"Report POST failed: {request.error} (url={url}, body={request.downloadHandler.text})");
            }
        }

        /// <summary>
        /// Fetch a DPP by product_id and return the raw JSON string via callback.
        /// Caller is responsible for deserializing (see DPPModels.cs).
        /// </summary>
        public IEnumerator GetDPP(string productId, Action<string> onSuccess, Action<string> onError)
        {
            string url = $"{baseUrl}/dpp/{productId}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.timeout = Mathf.Max(1, timeoutSeconds);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke($"DPP fetch failed: {request.error} (url={url})");
                }
            }
        }
    }
}
