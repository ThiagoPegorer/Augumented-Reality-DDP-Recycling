using UnityEngine;
using UnityEngine.EventSystems;

namespace DPP.UI
{
    /// <summary>
    /// Implements the global hover rule (DPP_UI_Specs/00 §4): a white/bright
    /// outline appears ONLY while a pointer ray is over the element, plus an
    /// optional subtle scale lift.
    ///
    /// Hover events arrive two ways:
    ///   - Editor mouse: standard EventSystem pointer enter/exit.
    ///   - PICO hand ray: PicoHandUIBridge dispatches pointerEnter/pointerExit
    ///     on the hovered target each frame (see bridge hover dispatch).
    ///
    /// Both hands can hover independently, so entries are ref-counted.
    /// </summary>
    public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("GameObject enabled while hovered (e.g. the white outline image behind the card).")]
        [SerializeField] private GameObject highlightOutline;

        [Tooltip("Transform scaled up slightly while hovered. Usually this same card. Optional.")]
        [SerializeField] private Transform lift;

        [Tooltip("Scale multiplier applied to 'lift' while hovered.")]
        [SerializeField] private float liftScale = 1.02f;

        private int _hoverCount;
        private Vector3 _restingScale = Vector3.one;
        private bool _restingCaptured;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _hoverCount++;
            Apply();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hoverCount = Mathf.Max(0, _hoverCount - 1);
            Apply();
        }

        private void OnDisable()
        {
            _hoverCount = 0;
            Apply();
        }

        private void Apply()
        {
            bool hovered = _hoverCount > 0;

            if (highlightOutline != null && highlightOutline.activeSelf != hovered)
                highlightOutline.SetActive(hovered);

            if (lift != null)
            {
                if (!_restingCaptured)
                {
                    _restingScale = lift.localScale;
                    _restingCaptured = true;
                }
                lift.localScale = hovered ? _restingScale * liftScale : _restingScale;
            }
        }
    }
}
