using UnityEngine;

namespace DPP
{
    /// <summary>
    /// Attach to each component GameObject (housing parts, PCB, etc.) inside the VCU model
    /// so its DPP metadata travels with the 3D object. Used by labels and the chatbot in Phase 2.
    /// </summary>
    public class ComponentMetadata : MonoBehaviour
    {
        [Header("DPP Component Info")]
        public string componentId;
        public string componentName;
        public string material;
        public float weightGrams;
        public string recyclingCode;
        public int disassemblyOrder;
        public bool isHazardous;

        [TextArea(2, 4)]
        public string notes;
    }
}
