using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// One collapsible category row of the Information tab (spec 02 §5).
    /// The header is always visible; Toggle() shows/hides the body and flips
    /// the chevron. Heights are resolved by the layout system (the row sits
    /// in a VerticalLayoutGroup; the body has its own layout group).
    /// Wire the header's Button.onClick to Toggle().
    /// </summary>
    public class AccordionRow : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private RectTransform chevron;
        [SerializeField] private bool startOpen;

        private void Start()
        {
            SetOpen(startOpen);
        }

        public bool IsOpen => body != null && body.activeSelf;

        public void Toggle() => SetOpen(!IsOpen);

        public void SetOpen(bool open)
        {
            if (body != null && body.activeSelf != open) body.SetActive(open);

            // Chevron sprite points down; expanded = up (spec 02 §5.1).
            if (chevron != null)
                chevron.localEulerAngles = new Vector3(0f, 0f, open ? 180f : 0f);

            // Row height changed — let the scroll content re-stack.
            var parentRect = transform.parent as RectTransform;
            if (parentRect != null) LayoutRebuilder.MarkLayoutForRebuild(parentRect);
        }
    }
}
