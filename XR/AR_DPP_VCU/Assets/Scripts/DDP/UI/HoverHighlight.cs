using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// The global hover response (DPP_UI_Specs/00 §4).
    ///
    /// RBv2.1.1 (2026-08-06) REPLACES THE WHITE RING. Thiago: *"the white border is
    /// a bit old school… create a highlight that gives the idea the button is
    /// jumping a bit."* A ring is a 2D convention borrowed from flat screens; in a
    /// headset the surface has real depth, so the hover now behaves like a physical
    /// object being picked up:
    ///
    ///   1. the element RISES toward the user along the canvas normal,
    ///   2. it SCALES up very slightly,
    ///   3. its shadow DROPS FURTHER and DARKENS — the cue that actually sells
    ///      elevation; scale alone reads as a zoom,
    ///   4. its fill BRIGHTENS a little, as if catching more light.
    ///
    /// All four are eased over <see cref="riseSeconds"/>. A snap reads as a state
    /// change; a rise reads as a response to the hand.
    ///
    /// ⚠ NOTHING HAD TO BE RE-WIRED. There are 34 of these across ten builder
    /// files, so the ring is retired by defaulting <see cref="useOutline"/> to
    /// false rather than by editing every call site. Existing `highlightOutline`
    /// references stay valid and switching the flag back on restores the old look —
    /// which is what makes this A/B-able on a participant.
    ///
    /// `Fill` and `Shadow` are resolved BY NAME from the children when not wired,
    /// because every button this project builds names them that way. A button that
    /// has neither still rises and scales; it just does not brighten.
    ///
    /// Hover arrives two ways — editor mouse via the EventSystem, and the PICO hand
    /// ray via PicoHandUIBridge dispatching enter/exit each frame. Both hands can
    /// hover independently, so entries are ref-counted.
    /// </summary>
    public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Legacy ring (00 §4 before RBv2.1.1)")]
        [Tooltip("The old white outline. Kept wired so the ring can be switched back on for comparison.")]
        [SerializeField] private GameObject highlightOutline;

        [Tooltip("OFF by default from RBv2.1.1 — the ring is replaced by the rise. Turn on to A/B it.")]
        [SerializeField] private bool useOutline = false;

        [Header("Rise")]
        [Tooltip("Transform that rises and scales. Defaults to this object's own transform.")]
        [SerializeField] private Transform lift;

        [Tooltip("Scale while hovered. Above ~1.06 the element starts to overlap its neighbours.")]
        [SerializeField] private float hoverScale = 1.03f;

        [Tooltip("Canvas units moved TOWARD the user while hovered. A world-space canvas front " +
                 "faces -z, so this is applied as -z. 6 units = 6 mm at the 0.001 canvas scale.")]
        [SerializeField] private float riseUnits = 6f;

        [Tooltip("Seconds to reach the hovered pose. Short enough to feel immediate, long enough " +
                 "not to read as a snap.")]
        [SerializeField] private float riseSeconds = 0.09f;

        [Header("Response (resolved by name when not wired)")]
        [Tooltip("Child named 'Shadow'. Drops further and darkens as the element rises.")]
        [SerializeField] private RectTransform shadow;

        [Tooltip("Extra downward offset of the shadow at full hover, in canvas units.")]
        [SerializeField] private float shadowDrop = 3f;

        [Tooltip("Extra shadow alpha at full hover.")]
        [SerializeField] private float shadowDarken = 0.12f;

        [Tooltip("Child named 'Fill'. Brightened while hovered.")]
        [SerializeField] private Graphic fill;

        [Tooltip("Added to each RGB channel of the fill at full hover.")]
        [SerializeField] private float brighten = 0.09f;

        private int _hoverCount;
        private float _t;                    // 0 = resting, 1 = fully hovered

        private Vector3 _restScale, _restPos;
        private Vector2 _restShadowPos;
        private Color _restShadowColor, _restFillColor;
        private Graphic _shadowGraphic;
        private bool _captured;

        private void Awake() => Capture();

        /// <summary>
        /// Resolve and remember the resting pose ONCE.
        ///
        /// It has to happen before the first hover: capturing lazily inside the
        /// hover handler would record the ALREADY-RAISED pose if the pointer were
        /// over the element on the frame it was enabled, and the button would then
        /// never come back down.
        /// </summary>
        private void Capture()
        {
            if (_captured) return;
            _captured = true;

            if (lift == null) lift = transform;
            _restScale = lift.localScale;
            _restPos = lift.localPosition;

            if (shadow == null)
            {
                var t = transform.Find("Shadow");
                if (t != null) shadow = t as RectTransform;
            }
            if (shadow != null)
            {
                _restShadowPos = shadow.anchoredPosition;
                _shadowGraphic = shadow.GetComponent<Graphic>();
                if (_shadowGraphic != null) _restShadowColor = _shadowGraphic.color;
            }

            if (fill == null)
            {
                var t = transform.Find("Fill");
                if (t != null) fill = t.GetComponent<Graphic>();
            }
            if (fill != null) _restFillColor = fill.color;
        }

        public void OnPointerEnter(PointerEventData eventData) => _hoverCount++;

        public void OnPointerExit(PointerEventData eventData)
            => _hoverCount = Mathf.Max(0, _hoverCount - 1);

        /// <summary>
        /// ⚠ RBv2.1.1 device round 1 (2026-08-07): state code and this hover FIGHT
        /// over the same Graphic. The step-flow status circles (red ✗ / green ✓)
        /// and the confirm fill are tinted by StepFlowController, but Apply()
        /// repaints from <c>_restFillColor</c> — captured ONCE at Awake — on every
        /// frame of the ease and on every enable/disable. On device the circle
        /// turned green for exactly as long as the hand still hovered it, then
        /// snapped back to red as the ray left: the ✓ mark (a GameObject toggle)
        /// survived, the colour (a Graphic write) did not — a red circle with a
        /// check mark, a state that ApplyTaskVisual cannot even produce.
        ///
        /// This is `00` §4.4 again, in colour: never move a RectTransform that
        /// HoverHighlight lifts, and never tint a Graphic that HoverHighlight
        /// brightens — without telling the hover. Any code that owns a
        /// hover-brightened fill's colour must hand the new colour through here
        /// instead of writing the Graphic alone.
        /// </summary>
        public void SetRestFillColor(Color c)
        {
            Capture();
            _restFillColor = c;
            if (fill != null) Apply();   // repaint NOW at the current hover ease
        }

        private void OnEnable()
        {
            Capture();
            _hoverCount = 0;
            _t = 0f;
            Apply();
        }

        private void OnDisable()
        {
            // Snap back rather than easing: the object is about to be hidden, and
            // a half-raised button that comes back next time it is shown is the
            // kind of state bug that only appears after a lot of navigation.
            _hoverCount = 0;
            _t = 0f;
            Apply();
        }

        private void Update()
        {
            float target = _hoverCount > 0 ? 1f : 0f;
            if (Mathf.Approximately(_t, target)) return;

            float step = riseSeconds <= 0f ? 1f : Time.unscaledDeltaTime / riseSeconds;
            _t = Mathf.MoveTowards(_t, target, step);
            Apply();
        }

        private void Apply()
        {
            // Ease-out: fast off the surface, settling at the top. A linear rise
            // reads mechanical.
            float e = 1f - (1f - _t) * (1f - _t);

            if (highlightOutline != null)
            {
                bool want = useOutline && _t > 0f;
                if (highlightOutline.activeSelf != want) highlightOutline.SetActive(want);
            }

            if (lift != null)
            {
                lift.localScale = Vector3.Lerp(_restScale, _restScale * hoverScale, e);
                // The canvas front faces -z, so toward the user is negative.
                lift.localPosition = _restPos + new Vector3(0f, 0f, -riseUnits * e);
            }

            if (shadow != null)
            {
                shadow.anchoredPosition = _restShadowPos + new Vector2(0f, -shadowDrop * e);
                if (_shadowGraphic != null)
                {
                    var c = _restShadowColor;
                    c.a = Mathf.Clamp01(_restShadowColor.a + shadowDarken * e);
                    _shadowGraphic.color = c;
                }
            }

            if (fill != null)
            {
                var c = _restFillColor;
                fill.color = new Color(
                    Mathf.Clamp01(c.r + brighten * e),
                    Mathf.Clamp01(c.g + brighten * e),
                    Mathf.Clamp01(c.b + brighten * e),
                    c.a);
            }
        }
    }
}
