using UnityEngine;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Screen 01 — Main Page (spec 01 v2, 2026-07-10).
    ///
    /// v2 removed the serial hero text and dropped the "· N steps" suffix,
    /// so this screen currently has NO live data bindings — all copy is static:
    ///   - Informations subtitle: "Digital Product Passport"
    ///   - Disassembly subtitle:  "Guided recycling"
    ///
    /// The class (and DPPManager's Populate call) is kept as the hook for
    /// future bindings — e.g. a scanned-product confirmation line once QR
    /// entry lands.
    /// </summary>
    public class MainPageView : MonoBehaviour
    {
        public void Populate(DPPData data)
        {
            // No dynamic content on this screen since v2 (serial removed,
            // step count no longer shown on the Disassembly card).
        }
    }
}
