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
    ///   COMPONENT DETAIL  the parts list — bodies that exist in real life.
    ///   DETAIL            one part: its NX drawing, shaded view and materials.
    ///   DRAWING           the drawing alone, filling the panel.
    ///
    /// THE PAGE HAS NO TITLE. Thiago, 2026-08-06: the sub-tab pills already say
    /// which data is on screen, so a 19 pt "Product specs" above them named the
    /// page for a third time (after the rail and the pills) and cost 40 units of
    /// content band. In the component-detail state the second pill takes the
    /// COMPONENT'S NAME — the selector keeps naming what is below it instead of
    /// handing that job back to a title.
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
        [SerializeField] private TMP_Text caption;

        [Header("Bottom bar")]
        [SerializeField] private TMP_Text backLabel;
        [SerializeField] private TMP_Text primaryLabel;

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

        [Header("Detail")]
        [SerializeField] private Image detailDrawing;
        [SerializeField] private Image detailIso;
        [SerializeField] private TMP_Text detailDimLine;
        [SerializeField] private GameObject detailEnlargeChip;
        [SerializeField] private RectTransform[] detailRows;
        [SerializeField] private TMP_Text[] detailKeys;
        [SerializeField] private TMP_Text[] detailValues;

        [Header("Drawing, enlarged")]
        [SerializeField] private Image drawingLarge;
        [SerializeField] private TMP_Text drawingCaption;

        [Header("Layout (spec 04c §4.3)")]
        [SerializeField] private float rowPitch = 32f;
        [SerializeField] private float viewportHeight = 284f;

        private static readonly Color GoldAccent    = new Color32(0xF0, 0xC8, 0x79, 0xFF);
        private static readonly Color TealLight     = new Color32(0x5D, 0xCA, 0xA5, 0xFF);
        private static readonly Color TextOnNavy    = Color.white;
        private static readonly Color TextSecondary = new Color32(0x9F, 0xB3, 0xD1, 0xFF);
        private static readonly Color TabOnFill     = new Color32(0x0D, 0x2A, 0x57, 0xFF);
        private static readonly Color TabOnStroke   = new Color32(0x2E, 0x5A, 0xA0, 0xFF);
        private static readonly Color TabOffFill    = new Color32(0x0E, 0x29, 0x50, 0xFF);
        private static readonly Color TabOffStroke  = new Color32(0x21, 0x40, 0x7A, 0xFF);

        /// <summary>The material name the payload uses for gold — the one value a
        /// recycler hunts for, so the only colour exception on the page.</summary>
        private const string GoldMaterial = "Gold";

        private const string CompTabName = "Component detail";

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

        private void FillDetail(ModelComponent c)
        {
            var dwg = LoadDrawing(c.drawing_id, "_dwg");
            var iso = LoadDrawing(c.drawing_id, "_iso");
            SetSprite(detailDrawing, dwg);
            SetSprite(detailIso, iso);
            SetSprite(drawingLarge, dwg);
            if (detailEnlargeChip != null) detailEnlargeChip.SetActive(dwg != null);
            if (detailDimLine != null)
                detailDimLine.text = dwg != null ? "NX sheet  ·  all dimensions in mm"
                                                 : "no drawing for this entry";
            if (drawingCaption != null) drawingCaption.text = c.name;

            int slot = 0;
            if (c.material_breakdown != null)
            {
                foreach (var m in c.material_breakdown)
                {
                    if (m == null || slot >= DetailSlots) break;
                    SetDetailRow(slot++, m.material, MassPrecise(m.weight_g),
                        m.material == GoldMaterial ? GoldAccent : TextOnNavy);
                }
            }

            // The regrouping line. Where a row carries several BOM entries this is
            // the only thing standing between the passport and a false claim
            // (04c §3.3b) — printed before the step, never dropped for space.
            if (!string.IsNullOrEmpty(c.represents) && slot < DetailSlots)
                SetDetailRow(slot++, "LCA entries", c.represents, TealLight);

            if (c.disassembly_step > 0 && slot < DetailSlots)
                SetDetailRow(slot++, "Disassembly step", c.disassembly_step.ToString(), TextSecondary);

            for (int i = slot; i < DetailSlots; i++)
                if (detailRows != null && i < detailRows.Length && detailRows[i] != null)
                    detailRows[i].gameObject.SetActive(false);
        }

        private int DetailSlots => detailRows != null ? detailRows.Length : 0;

        private void SetDetailRow(int slot, string key, string value, Color valueColour)
        {
            if (detailRows == null || slot >= detailRows.Length || detailRows[slot] == null) return;
            detailRows[slot].gameObject.SetActive(true);
            SetAt(detailKeys, slot, key);
            SetAt(detailValues, slot, value);
            if (detailValues != null && slot < detailValues.Length && detailValues[slot] != null)
                detailValues[slot].color = valueColour;
        }

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

            // The second pill names the component once one is open — the selector
            // keeps describing what is below it, which is why the page needs no
            // title (Thiago, 2026-08-06).
            bool onPart = (state == State.Detail || state == State.Drawing)
                          && _selected >= 0 && _selected < _parts.Count;
            if (subCompLabel != null)
                subCompLabel.text = onPart ? _parts[_selected].name : CompTabName;

            if (caption != null)
            {
                if (state == State.Detail && onPart)
                    caption.text = $"{_selected + 1} of {_parts.Count}  ·  {Mass(_parts[_selected].weight_g)}";
                else if (state == State.Drawing)
                    caption.text = "scale 1:1  ·  mm";
                else
                    caption.text = "";   // no "660 g · BOM v4.1" — it said nothing a user acts on
            }

            SetText(backLabel, state == State.Identity ? (owner != null ? owner.BackLabel : "Back")
                             : state == State.Detail   ? "All parts"
                             : state == State.Drawing  ? "Back to data"
                                                       : "Back");

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

        /// <summary>Header masses round to one decimal. The payload keeps four (the
        /// housing area split is exact and the 660.1565 g closure check depends on
        /// it) — that precision is for the data, not the panel.</summary>
        private static string Mass(float g) => $"{g:0.#} g";

        /// <summary>Material rows go finer so 0.06 g of connector gold does not
        /// render as "0.1 g".</summary>
        private static string MassPrecise(float g) => g < 1f ? $"{g:0.####} g" : $"{g:0.##} g";

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
