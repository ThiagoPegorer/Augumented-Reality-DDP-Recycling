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

        public string BaseUrl
        {
            get => baseUrl;
            set => baseUrl = value;
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
