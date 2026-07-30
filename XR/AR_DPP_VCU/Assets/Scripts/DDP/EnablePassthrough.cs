using UnityEngine;
using Unity.XR.PXR;

namespace DPP
{
    /// <summary>
    /// Forces PICO 4 Video Seethrough (camera passthrough) on at runtime.
    ///
    /// PXR_Manager's Video Seethrough checkbox configures the SDK feature,
    /// but on Unity 6 + PICO SDK 3.4 the actual passthrough stream needs an
    /// explicit runtime assignment of PXR_Manager.EnableVideoSeeThrough = true.
    /// Attach this script to the Main Camera so it fires on Start.
    ///
    /// Also enforces a transparent camera background so the passthrough
    /// video is visible behind the rendered 3D content.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class EnablePassthrough : MonoBehaviour
    {
        [Tooltip("Whether to also force the camera's clear flags and background color " +
                 "to fully transparent (recommended).")]
        [SerializeField] private bool forceTransparentBackground = true;

        void Start()
        {
            // 1. Make sure the camera renders with a transparent background so
            //    the passthrough video shows through.
            if (forceTransparentBackground)
            {
                var cam = GetComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0f, 0f, 0f, 0f);
            }

            // 2. Tell PICO to start the video seethrough camera stream.
            //    PICO SDK 3.4+: EnableVideoSeeThrough is a PROPERTY (not a method).
            PXR_Manager.EnableVideoSeeThrough = true;
            Debug.Log("[EnablePassthrough] PXR_Manager.EnableVideoSeeThrough = true");
        }
    }
}
