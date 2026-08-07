using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

// MonoBehaviour derives from UnityEngine.Component, so the unqualified name
// `Component` in this file is ambiguous with DPP.Models.Component. Aliasing is
// clearer than fully qualifying it at every use site.
using ModelComponent = DPP.Models.Component;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 — 04c: THE PRODUCT SPECIFICATIONS TAB (spec `04c_product_specs.md`,
    /// mock `drafts/04c_v3_product_specs.svg`, revised on the 2026-08-06 device test).
    ///
    /// Four states inside the 420 × 430 data canvas:
    ///
    ///   PRODUCT ID        the seven declared identity rows.
    ///   COMPONENT ID      the parts list — bodies that exist in real life.
    ///   DETAIL            one part: its NX drawing above, its materials below.
    ///                     The split is PROPORTIONAL — see LayoutDetail.
    ///   DRAWING           the drawing alone, filling the panel.
    ///
    /// THE PAGE HAS NO TITLE. Thiago, 2026-08-06: the sub-tab pills already say
    /// which data is on screen, so a 19 pt "Product specs" above them named the
    /// page for a third time (after the rail and the pills) and cost 40 units of
    /// content band.
    ///
    /// ⚠ THE SECOND PILL NEVER CHANGES ITS LABEL. It reads "Component ID" in every
    /// state (Thiago, device test 2026-08-06). It used to take the open component's
    /// name, on the theory that the selector should keep naming what is below it —
    /// on device that read as a tab MOVING UNDER THE HAND. The user has just tapped
    /// the component out of a list, so he already knows which one it is; what he
    /// loses is the one fixed landmark he navigates by. A selector that relabels
    /// itself has stopped being a selector.
    ///
    /// THE LIST SHOWS PARTS ONLY. The payload's seven `board_material` rows —
    /// solder, coating, passives, Ta caps, TIM, misc, fasteners — are not bodies a
    /// dismantler can pick up, and listing them made the page read as an inventory
    /// dump. ⚠ They are still in the payload and still in the LCA: between them
    /// they carry 79.4 g, ALL of the tantalum and most of the on-board precious
    /// metals, so something still has to declare them (04d is the obvious home).
    ///
    /// EVERY VALUE COMES FROM THE PAYLOAD, generated from VCU_BOM_v4.xlsx
    /// `By_Component`. Nothing here computes chemistry.
    ///
    /// ⚠ THE BUILDER'S BAKED STRINGS ARE PLACEHOLDERS THAT LOOK REAL. On
    /// 2026-08-06 a stale reference meant this page served them on device, and the
    /// only visible symptom was an empty list — everything else looked correct.
    /// <see cref="OnEnable"/> now PULLS from DPPManager when nothing was pushed.
    /// </summary>
    public class ProductSpecsView : MonoBehaviour
    {
        public enum State { Identity, Parts, Detail, Drawing }

        // =================================================================
        // Wiring (set by RBv2_1/9)
        // =================================================================
        [Header("Routing")]
        [SerializeField] private ScreenRouter router;

        [Tooltip("RBv2.1.1 — the super panel that owns this page, set by RBv2_1_1/2. When it is " +
                 "present the bottom bar walks the passport (previous/next tab) instead of routing " +
                 "out of it; when it is null the page behaves as the standalone screen RBv2_1/9 built.")]
        [SerializeField] private SuperPanelView owner;

        [Header("Sub-tab selector — this IS the page's title")]
        [SerializeField] private Image subIdFill;
        [SerializeField] private Image subIdStroke;
        [SerializeField] private TMP_Text subIdLabel;
        [SerializeField] private Image subCompFill;
        [SerializeField] private Image subCompStroke;
        [SerializeField] private TMP_Text subCompLabel;

        [Header("Bottom bar")]
        [SerializeField] private TMP_Text backLabel;
        [SerializeField] private TMP_Text primaryLabel;

        [Tooltip("The WHOLE primary button, not just its label — hidden in the enlarged Drawing " +
                 "state (RBv2.1.1). A full-panel drawing has nothing to advance to, so a Next " +
                 "there offered to leave the tab from the deepest screen in it, which is the one " +
                 "place the user is least likely to mean it.")]
        [SerializeField] private GameObject primaryButton;

        [Header("State roots")]
        [SerializeField] private GameObject identityRoot;
        [SerializeField] private GameObject partsRoot;
        [SerializeField] private GameObject detailRoot;
        [SerializeField] private GameObject drawingRoot;

        [Header("Product ID — seven declared rows")]
        [SerializeField] private TMP_Text[] identityValues;

        [Header("Component list — pooled rows, parts only")]
        [SerializeField] private RectTransform listContent;
        [SerializeField] private RectTransform[] listRows;
        [SerializeField] private TMP_Text[] listNames;
        [SerializeField] private Image[] listFills;
        [SerializeField] private Button[] listButtons;

        [Header("Detail — upper half: the drawing, RESIZED PER COMPONENT")]
        [Tooltip("The whole drawing card. LayoutDetail sets its HEIGHT; its stroke, its " +
                 "fill, the drawing itself and the View button are all stretch-anchored " +
                 "inside it, so they follow without a single extra assignment.")]
        [SerializeField] private RectTransform drawingCard;
        [SerializeField] private Image detailDrawing;
        [SerializeField] private GameObject viewButton;

        [Header("Detail — lower half: ONE chart column")]
        [Tooltip("Everything below the drawing. LayoutDetail moves it up or down so the " +
                 "table always ends on the band bottom, whatever the material count.")]
        [SerializeField] private RectTransform lowerBlock;
        [SerializeField] private RectTransform[] detailRows;
        [SerializeField] private TMP_Text[] matNames;
        [SerializeField] private TMP_Text[] matMasses;
        [SerializeField] private Image[] matPriorityBars;
        [SerializeField] private TMP_Text[] matPriorityLabels;
        [SerializeField] private Image[] matRecoveryBars;
        [SerializeField] private TMP_Text[] matRecoveryLabels;

        [Tooltip("Decade ticks over the impact bar — TicksPerRow of them per row, flattened " +
                 "(index = slot * TicksPerRow + k) because Unity does not serialize 2-D arrays. " +
                 "They are the ONLY mark on the row saying the upper bar is logarithmic and " +
                 "the lower one is not; delete them and the chart lies quietly.")]
        [SerializeField] private Image[] matTicks;

        [Header("Detail — the chart explanation")]
        [Tooltip("Opened by the \"i\", closed by Got it. Any state change closes it too.")]
        [SerializeField] private GameObject infoModal;

        [Header("Detail geometry — mirrors the Ps* constants in DPPUIBuilder.ProductSpecs.cs")]
        [Tooltip("Width of the SHARED bar track. Impact runs a log axis over it, recovery a " +
                 "linear one — same origin, same width, deliberately different heights.")]
        [SerializeField] private float trackWidth = 152f;
        [SerializeField] private float detailBandTop = 76f;
        [SerializeField] private float detailBandHeight = 284f;
        [SerializeField] private float detailHeadHeight = 14f;
        [SerializeField] private float detailRowPitch = 20f;
        [SerializeField] private float detailGap = 10f;
        [SerializeField] private float drawingMinHeight = 114f;
        [SerializeField] private float drawingMaxHeight = 240f;

        [Header("Drawing, enlarged — the drawing and nothing else")]
        [SerializeField] private Image drawingLarge;

        [Header("Layout (spec 04c §4.3)")]
        [SerializeField] private float rowPitch = 35f;
        [SerializeField] private float viewportHeight = 284f;

        private static readonly Color GoldAccent    = new Color32(0xF0, 0xC8, 0x79, 0xFF);
        private static readonly Color TealLight     = new Color32(0x5D, 0xCA, 0xA5, 0xFF);
        private static readonly Color TextOnNavy    = Color.white;
        private static readonly Color TextSecondary = new Color32(0x9F, 0xB3, 0xD1, 0xFF);
        private static readonly Color TabOnFill     = new Color32(0x0D, 0x2A, 0x57, 0xFF);
        private static readonly Color TabOnStroke   = new Color32(0x2E, 0x5A, 0xA0, 0xFF);
        private static readonly Color TabOffFill    = new Color32(0x0E, 0x29, 0x50, 0xFF);
        private static readonly Color TabOffStroke  = new Color32(0x21, 0x40, 0x7A, 0xFF);
        private static readonly Color TextTip       = new Color32(0x6F, 0x86, 0xA8, 0xFF);
        private static readonly Color Heat          = new Color32(0xF0, 0x8A, 0x3C, 0xFF);
        private static readonly Color PriorityBar   = new Color32(0x2E, 0xB0, 0x86, 0xFF);
        private static readonly Color RecoveryBar   = new Color32(0x1F, 0x77, 0xB4, 0xFF);

        /// <summary>Decade tick where the impact bar has already covered it — dark, so it
        /// reads as a notch cut out of the bar rather than a mark laid over it.</summary>
        private static readonly Color TickOnBar     = new Color32(0x0A, 0x1F, 0x44, 0x73);
        private static readonly Color TickOffBar    = new Color32(0x2A, 0x4A, 0x80, 0xE6);

        /// <summary>Ticks per row. Three of them put the axis at 25 / 50 / 75 % of the
        /// track, which is two decades apart on the 8-decade log axis.</summary>
        private const int TicksPerRow = 3;

        /// <summary>The material name the payload uses for gold — the one value a
        /// recycler hunts for, so the only colour exception on the page.</summary>
        private const string GoldMaterial = "Gold";

        private const string CompTabName = "Component ID";

        private DPPData _data;
        private readonly List<ModelComponent> _parts = new List<ModelComponent>();
        private State _state = State.Identity;
        private int _selected = -1;

        // =================================================================
        // Lifecycle
        // =================================================================

        private void OnEnable()
        {
            // PULL if nothing was PUSHED — see the class note on the 2026-08-06
            // failure. A broken reference must cost nothing, and the warning has
            // to name the phase to re-run.
            if (_data == null)
            {
                var mgr = FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
                if (mgr != null && mgr.Latest != null) Populate(mgr.Latest);
                else Debug.LogWarning("[ProductSpecs] No payload — showing baked previews. " +
                                      "Check DPPManager.productSpecs (RBv2_1/9) and that the backend is up.");
            }

            Show(State.Identity);
        }

        /// <summary>Called by DPPManager on every successful fetch.</summary>
        public void Populate(DPPData data)
        {
            _data = data;
            if (data == null) return;

            PopulateIdentity(data);
            SplitComponents(data);
            FillList();
        }

        // =================================================================
        // Product ID
        // =================================================================

        private void PopulateIdentity(DPPData data)
        {
            var id = data.identity;
            if (id == null || identityValues == null) return;

            SetAt(identityValues, 0, id.manufacturer);
            SetAt(identityValues, 1, id.model);
            SetAt(identityValues, 2, id.type_number);
            SetAt(identityValues, 3, id.serial_number);
            SetAt(identityValues, 4, id.production_date);
            SetAt(identityValues, 5, id.country_of_origin);
            SetAt(identityValues, 6, id.product_category);
        }

        // =================================================================
        // The list
        // =================================================================

        /// <summary>
        /// Keep only `group == "part"`, preserving payload order.
        ///
        /// The fallback matters: a pre-0.14 payload has no `group` at all and
        /// <see cref="ModelComponent.IsPart"/> then reports true for every row.
        /// That yields 15 rows with no drawings — wrong, but VISIBLY wrong, which
        /// beats a blank list nobody can diagnose.
        /// </summary>
        private void SplitComponents(DPPData data)
        {
            _parts.Clear();
            if (data.components == null) return;
            foreach (var c in data.components)
                if (c != null && c.IsPart) _parts.Add(c);
        }

        private void FillList()
        {
            if (listRows == null) return;

            int slot = 0;
            for (; slot < _parts.Count && slot < listRows.Length; slot++)
            {
                var rt = listRows[slot];
                if (rt == null) continue;

                rt.gameObject.SetActive(true);
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -slot * rowPitch);
                SetAt(listNames, slot, _parts[slot].name);

                if (listFills != null && slot < listFills.Length && listFills[slot] != null)
                    listFills[slot].raycastTarget = true;

                if (listButtons != null && slot < listButtons.Length && listButtons[slot] != null)
                {
                    var btn = listButtons[slot];
                    btn.interactable = true;
                    int captured = slot;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OpenPart(captured));
                }
            }

            if (_parts.Count > listRows.Length)
                Debug.LogWarning($"[ProductSpecs] {_parts.Count} parts but only {listRows.Length} row slots — " +
                                 "re-run RBv2_1/9 to build more.");

            for (int i = slot; i < listRows.Length; i++)
                if (listRows[i] != null) listRows[i].gameObject.SetActive(false);

            // Eight parts fit the band without scrolling, but the content is sized
            // honestly so PinchScrollArea still behaves if the list ever grows.
            if (listContent != null)
                listContent.sizeDelta = new Vector2(listContent.sizeDelta.x,
                    Mathf.Max(slot * rowPitch, viewportHeight));
        }

        // =================================================================
        // Detail
        // =================================================================

        public void OpenPart(int index)
        {
            if (index < 0 || index >= _parts.Count) return;
            _selected = index;
            FillDetail(_parts[index]);
            Show(State.Detail);
        }

        /// <summary>
        /// The 1 × 2 grid (04c §4.4, revised 2026-08-06): the NX drawing owns the
        /// upper half, and the lower half answers the only question a dismantler
        /// has about a part — WHAT IS IN IT, and WHICH OF IT IS WORTH RECOVERING.
        ///
        /// Rows are sorted by IMPACT, not by mass, and that inversion is the whole
        /// point. On the connector, gold is 0.04 % of the mass and 98.5 % of the
        /// minerals footprint; aluminium is 63 % of the mass and 0.000003 %. A
        /// mass-ordered list — or a pie of mass, which is what this replaced —
        /// tells a recycler to care about exactly the wrong material.
        ///
        /// This is `LCA_framework_v4.md` finding ② rendered: *"recovery priorities
        /// should be burden-weighted, not mass-weighted: exactly the
        /// component-level information a DPP carries."*
        /// </summary>
        private void FillDetail(ModelComponent c)
        {
            var dwg = LoadDrawing(c.drawing_id, "_dwg");
            SetSprite(detailDrawing, dwg);
            SetSprite(drawingLarge, dwg);
            if (viewButton != null) viewButton.SetActive(dwg != null);

            // ⚠ NOTHING ELSE IS DRAWN IN THIS STATE. `reuse_eligible` / `reuse_note`
            // are still in the payload and still in the LCA, but the panel is the
            // drawing and the table (Thiago, 2026-08-06). 04d declares the reuse set.

            var mats = SortedByImpact(c);
            int shown = Mathf.Min(mats.Count, DetailSlots);
            if (mats.Count > DetailSlots)
                Debug.LogWarning($"[ProductSpecs] {c.name} declares {mats.Count} materials but only " +
                                 $"{DetailSlots} row slots exist — re-run RBv2_1/9 with a bigger PsDetailSlots.");

            for (int slot = 0; slot < shown; slot++)
            {
                var m = mats[slot];
                bool gold = m.material == GoldMaterial;
                float share = Share(m);
                Color impColour = gold ? GoldAccent : PriorityBar;

                detailRows[slot].gameObject.SetActive(true);
                SetAt(matNames, slot, Short(m.material));
                if (matNames != null && slot < matNames.Length && matNames[slot] != null)
                    matNames[slot].color = share >= 1f ? TextOnNavy : TextSecondary;
                SetAt(matMasses, slot, Mass(m.weight_g));

                // ---- UPPER BAR: recovery impact, LOG axis ----
                // The shares span 98 % to 3e-6 %; on a linear axis only gold would be
                // visible and the chart would say nothing about anything else.
                float impW = trackWidth * LogFraction(share);
                SetBar(matPriorityBars, slot, impW, impColour);
                PaintTicks(slot, impW);
                SetAt(matPriorityLabels, slot, ShareLabel(m));
                if (matPriorityLabels != null && slot < matPriorityLabels.Length && matPriorityLabels[slot] != null)
                {
                    matPriorityLabels[slot].color = impColour;
                    matPriorityLabels[slot].fontStyle = gold ? FontStyles.Bold : FontStyles.Normal;
                }

                // ---- LOWER BAR: max recovery rate, LINEAR 0-100 ----
                // A missing rate and a zero rate are NOT the same thing and must not
                // render the same: nickel, tantalum and tin are credited in no
                // scenario, which is an answer, not a gap.
                float rec = m.recovery_pct.HasValue ? m.recovery_pct.Value : -1f;
                SetBar(matRecoveryBars, slot, rec > 0f ? trackWidth * rec / 100f : 0f, RecoveryBar);
                SetAt(matRecoveryLabels, slot, rec < 0f ? "?" : $"{rec:0}%");
                if (matRecoveryLabels != null && slot < matRecoveryLabels.Length && matRecoveryLabels[slot] != null)
                    matRecoveryLabels[slot].color = rec < 0f ? Heat : (rec > 0f ? RecoveryBar : TextTip);
            }

            for (int i = shown; i < DetailSlots; i++)
                if (detailRows != null && i < detailRows.Length && detailRows[i] != null)
                    detailRows[i].gameObject.SetActive(false);

            LayoutDetail(shown);
        }

        /// <summary>
        /// 04c §4.6 — the band is split PROPORTIONALLY, not 50/50.
        ///
        /// A single-material part (either housing: aluminium, and nothing else) left
        /// roughly 150 units of empty navy under a one-row table while its drawing sat
        /// squeezed into a 140-unit letterbox. A six-material part had the opposite
        /// problem. The fix is to size the LOWER block to its content and give the
        /// drawing whatever is left:
        ///
        ///     lower   = head + rows x pitch
        ///     drawing = band - gap - lower,  clamped to [min, max]
        ///
        /// Unclamped, those two close EXACTLY on the band bottom, which is why the
        /// constants are not round numbers. 1 material gives the drawing 240 units,
        /// 6 materials give it 140.
        ///
        /// ⚠ NOTHING IS RESERVED UNDER THE LAST ROW. An earlier pass kept 26 units
        /// there for the "i" and it read as empty navy on every component. The "i"
        /// moved to the button line instead, into the slot Next vacated.
        ///
        /// ⚠ The clamp bites past ~7 materials, and when it does the table really does
        /// run past the band into the button row. That warns rather than silently
        /// overlapping — a table sliding under a button is the kind of defect that only
        /// shows up on the one component nobody opened during testing.
        /// </summary>
        private void LayoutDetail(int rowCount)
        {
            float lower = detailHeadHeight + rowCount * detailRowPitch;
            float want  = detailBandHeight - detailGap - lower;
            float dh    = Mathf.Clamp(want, drawingMinHeight, drawingMaxHeight);

            if (want < drawingMinHeight)
                Debug.LogWarning($"[ProductSpecs] {rowCount} material rows need {lower:0} units. The drawing " +
                                 $"is clamped at {drawingMinHeight:0}, so the table overflows the content band " +
                                 $"by {drawingMinHeight - want:0} units and will run into the button row.");

            if (drawingCard != null)
                drawingCard.sizeDelta = new Vector2(drawingCard.sizeDelta.x, dh);

            if (lowerBlock != null)
            {
                lowerBlock.anchoredPosition = new Vector2(lowerBlock.anchoredPosition.x,
                                                          -(detailBandTop + dh + detailGap));
                lowerBlock.sizeDelta = new Vector2(lowerBlock.sizeDelta.x, lower);
            }

        }

        /// <summary>
        /// The decade ticks are drawn OVER the impact bar, not behind it.
        ///
        /// Behind, the bar hides exactly the part of the axis you need in order to read
        /// it — and the entire reason they exist is that this bar shares an origin and a
        /// track width with a LINEAR bar directly underneath. Without them, an impact of
        /// 98.5 % and a recovery of 94 % end four units apart and read as a comparison.
        /// They are not comparable. 04c §4.6 and the mock's push-back note.
        /// </summary>
        private void PaintTicks(int slot, float barWidth)
        {
            if (matTicks == null) return;
            for (int k = 0; k < TicksPerRow; k++)
            {
                int i = slot * TicksPerRow + k;
                if (i >= matTicks.Length || matTicks[i] == null) continue;
                float x = trackWidth * (k + 1) / (TicksPerRow + 1);
                matTicks[i].color = x <= barWidth ? TickOnBar : TickOffBar;
            }
        }

        private List<MaterialShare> SortedByImpact(ModelComponent c)
        {
            var list = new List<MaterialShare>();
            if (c.material_breakdown != null)
                foreach (var m in c.material_breakdown) if (m != null) list.Add(m);
            list.Sort((a, b) => Share(b).CompareTo(Share(a)));
            return list;
        }

        private static float Share(MaterialShare m)
            => m.impact_share_pct.HasValue ? m.impact_share_pct.Value : 0f;

        /// <summary>Position on a log axis running 1e-6 % to 100 %, clamped to [0,1].</summary>
        private static float LogFraction(float pct)
        {
            if (pct <= 0f) return 0f;
            return Mathf.Clamp01((Mathf.Log10(pct) + 6f) / 8f);
        }

        /// <summary>
        /// "—" and "&lt;0.01 %" are DIFFERENT ANSWERS and must never collapse into one.
        ///
        ///   —        the material has NO EF 3.1 characterisation factor. Glass fibre,
        ///            polymers, silicon, ceramics. Not characterised.
        ///   &lt;0.01 %  it has a factor and the share is negligible. Connector aluminium
        ///            is 63 % of the mass and 3.3e-6 % of the impact.
        ///
        /// ⚠ Which one it is comes from `impact_kg_sb_eq`, NEVER from the share. Until
        /// payload v0.16 the share was stored rounded to 4 dp, so aluminium's 3.3e-6 %
        /// was written as 0.0 — identical to polymers — and the panel showed both as
        /// "—". Reading the raw impact keeps this correct even on an old payload.
        /// </summary>
        private static string ShareLabel(MaterialShare m)
        {
            bool characterised = m.impact_kg_sb_eq.HasValue && m.impact_kg_sb_eq.Value > 0f;
            if (!characterised) return "—";
            float pct = Share(m);
            return pct >= 0.05f ? $"{pct:0.0}%" : "<0.01%";
        }

        private static void SetBar(Image[] arr, int i, float width, Color colour)
        {
            if (arr == null || i >= arr.Length || arr[i] == null) return;
            var rt = arr[i].rectTransform;
            rt.sizeDelta = new Vector2(Mathf.Max(0f, width), rt.sizeDelta.y);
            arr[i].enabled = width > 0.5f;
            arr[i].color = colour;
        }

        private int DetailSlots => detailRows != null ? detailRows.Length : 0;

        /// <summary>Sprites live in Resources/dwg because they are chosen by a
        /// payload string, not a scene reference. Missing is not an error: board
        /// materials have no drawing by design.</summary>
        private static Sprite LoadDrawing(string drawingId, string suffix)
            => string.IsNullOrEmpty(drawingId) ? null : Resources.Load<Sprite>($"dwg/{drawingId}{suffix}");

        private static void SetSprite(Image img, Sprite s)
        {
            if (img == null) return;
            img.sprite = s;
            img.enabled = s != null;
            img.preserveAspect = true;
        }

        // =================================================================
        // State
        // =================================================================

        public void Show(State state)
        {
            _state = state;
            if (identityRoot != null) identityRoot.SetActive(state == State.Identity);
            if (partsRoot != null)    partsRoot.SetActive(state == State.Parts);
            if (detailRoot != null)   detailRoot.SetActive(state == State.Detail);
            if (drawingRoot != null)  drawingRoot.SetActive(state == State.Drawing);

            bool identity = state == State.Identity;
            Paint(subIdFill, subIdStroke, subIdLabel, identity);
            Paint(subCompFill, subCompStroke, subCompLabel, !identity);

            // The pill is a LANDMARK, not a readout — see the class note. Set
            // unconditionally rather than only in the states that used to change it,
            // so a stale name can never survive a transition.
            if (subCompLabel != null) subCompLabel.text = CompTabName;

            // THE HEADER CARRIES NO CAPTION AT ALL from RBv2.1.1. "1 of 8 · 108.5 g"
            // repeated the list the user had just come out of, and the mass was already
            // in the row directly below it. It was also the last thing in the header
            // that changed between components — see the class note on the pill.

            // Any navigation closes the chart explanation. A modal that survives a state
            // change is the kind that reappears over the wrong screen three steps later.
            if (infoModal != null) infoModal.SetActive(false);

            SetText(backLabel, state == State.Identity ? (owner != null ? owner.BackLabel : "Back")
                             : state == State.Detail   ? "All parts"
                             : state == State.Drawing  ? "Back to data"
                                                       : "Back");

            // NEXT DOES NOT EXIST ONCE A COMPONENT IS OPEN (Detail or Drawing). Both are
            // leaves of the drill, and the only move that makes sense from a leaf is
            // back up it — a Next there offered to leave the whole tab from its deepest
            // screen. The list and Product ID keep theirs, so the tab still has a way
            // forward. Hiding the whole button rather than blanking its label also stops
            // its hit area swallowing pinches meant for the "i", which now sits on the
            // same line.
            bool onComponent = state == State.Detail || state == State.Drawing;
            if (primaryButton != null) primaryButton.SetActive(!onComponent);

            if (owner != null && primaryLabel != null)
                primaryLabel.text = state == State.Identity ? "Next" : owner.PrimaryLabel;
        }

        private static void Paint(Image fill, Image stroke, TMP_Text label, bool on)
        {
            if (fill != null)   fill.color   = on ? TabOnFill : TabOffFill;
            if (stroke != null) stroke.color = on ? TabOnStroke : TabOffStroke;
            if (label != null)  label.color  = on ? TextOnNavy : TextSecondary;
        }

        // =================================================================
        // Button targets
        // =================================================================

        public void ShowIdentity() => Show(State.Identity);

        /// <summary>The second pill. From a component detail it steps back to the
        /// list rather than re-opening the same part.</summary>
        public void ShowParts()
        {
            _selected = -1;
            Show(State.Parts);
        }

        /// <summary>The "i" under the chart. It replaced a 372-wide footnote that stated
        /// both formulas — true, unreadable at 0.75 m, and permanently on screen.</summary>
        public void ShowInfo()
        {
            if (infoModal != null) infoModal.SetActive(true);
        }

        /// <summary>"Got it", and the scrim behind it.</summary>
        public void HideInfo()
        {
            if (infoModal != null) infoModal.SetActive(false);
        }

        public void ShowDrawing()
        {
            if (_selected < 0) return;
            if (drawingLarge != null && drawingLarge.sprite == null) return;
            Show(State.Drawing);
        }

        /// <summary>Bottom-left. One level up each press, never straight out — the
        /// drill is four levels deep and a Back that jumped to the rail from a
        /// component detail would lose the user's place in the list.</summary>
        public void OnBack()
        {
            switch (_state)
            {
                case State.Drawing:  Show(State.Detail);   break;
                case State.Detail:   ShowParts();          break;
                case State.Parts:    Show(State.Identity); break;
                default:
                    if (owner != null) owner.PrevTab();
                    else if (router != null) router.ShowDppCanva();
                    else Debug.LogWarning("[ProductSpecs] No router — cannot leave the page.");
                    break;
            }
        }

        /// <summary>Bottom-right. Product ID advances to the component list;
        /// everywhere deeper it leaves the tab, so the Recycler passes through both
        /// sub-tabs before the rail unlocks the next one (04c §5).</summary>
        public void OnPrimary()
        {
            if (_state == State.Identity) { Show(State.Parts); return; }
            if (owner != null) { owner.NextTab(); return; }
            if (router != null) router.ShowDppCanva();
        }

        // =================================================================
        // Formatting
        // =================================================================

        /// <summary>Masses round to one decimal, and switch to milligrams below a gram
        /// so 0.06 g of connector gold reads as "60 mg" rather than "0.1 g". The payload
        /// keeps four decimals — the housing area split is exact and the 660.1565 g
        /// closure check depends on it — but that precision is for the data, not the
        /// panel.</summary>
        private static string Mass(float g) => g >= 1f ? $"{g:0.#} g" : $"{g * 1000f:0.#} mg";

        /// <summary>Material names carry their qualifier ("Polymers/epoxy",
        /// "Tin (solder)"); the 108-unit column does not. The full string is in
        /// the payload and in the spec.</summary>
        private static string Short(string material)
        {
            if (string.IsNullOrEmpty(material)) return "";
            int cut = material.IndexOfAny(new[] { '/', '(' });
            return (cut > 0 ? material.Substring(0, cut) : material).TrimEnd();
        }

        private static void SetAt(TMP_Text[] arr, int i, string v)
        {
            if (arr == null || i < 0 || i >= arr.Length || arr[i] == null) return;
            if (v != null) arr[i].text = v;
        }

        private static void SetText(TMP_Text t, string v)
        {
            if (t != null && v != null) t.text = v;
        }
    }
}
