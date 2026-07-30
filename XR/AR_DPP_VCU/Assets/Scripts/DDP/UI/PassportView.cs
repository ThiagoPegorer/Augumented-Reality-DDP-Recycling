using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// Data bindings for the RBv2.0 passport screens — DPP Canva (spec 13 v2) and
    /// Composition &amp; impact (spec 14 v2).
    ///
    /// WHY A NEW VIEW instead of extending InfoTabView: the v2 screens are charts,
    /// not field rows. Their element counts are DATA-DRIVEN (one composition segment
    /// per material present, one legend entry per top-N, one column per end-of-life
    /// scenario), so the builder creates a fixed POOL and this class shows, sizes and
    /// positions only what the payload actually contains. InfoTabView stays as it is
    /// for the label/value modals.
    ///
    /// THE HONESTY RULE (spec 13 §4): every value on screen carries its basis.
    ///   ● filled bright  = declared / datasheet / measured
    ///   ● filled dim     = assumed / modelled
    ///   ○ hollow         = not provided
    /// Nothing renders as a blank, and nothing hardcoded in the builder survives a
    /// Populate — the builder's strings are Editor placeholders only.
    ///
    /// COMPOSITION IS DERIVED, NOT STORED: material shares are aggregated from
    /// components[].material_breakdown at runtime, so the chart can never drift from
    /// the BOM the LCA figures came from. Verified 2026-07-30 to reproduce
    /// VCU_BOM_v4.xlsx's By_Material sheet exactly.
    /// </summary>
    public class PassportView : MonoBehaviour
    {
        private const string Dash = "—";

        // Layout constants — MUST match DPPUIBuilder.DppCanva.cs.
        private const float CompositionBarWidth = 548f;
        private const float CompositionMinSegment = 2f;   // keeps trace materials visible
        private const float ScenarioBarHeight = 52f;      // full height = baseline scenario
        private const float RecoveryTrackWidth = 240f;
        private const float RecoveryScaleMaxPct = 50f;    // 240 px = 50 %

        // Segment palette by descending mass rank (00 §2 tokens + neutrals).
        private static readonly Color[] MaterialPalette =
        {
            DPPTheme.Hex("#2e5aa0"), DPPTheme.Hex("#f0c879"), DPPTheme.Hex("#2e7d4f"),
            DPPTheme.Hex("#5dcaa5"), DPPTheme.Hex("#9fb3d1"), DPPTheme.Hex("#5d7396"),
            DPPTheme.Hex("#324a6d"), DPPTheme.Hex("#4da3ff"), DPPTheme.Hex("#7f9bc4"),
            DPPTheme.Hex("#1d9e75"), DPPTheme.Hex("#16335f"), DPPTheme.Hex("#0a2344"),
        };

        private static readonly Color DotDeclared = DPPTheme.Hex("#27c46c");
        private static readonly Color DotModelled = DPPTheme.TealLight;
        private static readonly Color DotMissing  = DPPTheme.TextTip;

        // ---------------- Identity hero (spec 13 §2) ----------------
        [Header("Identity hero")]
        [SerializeField] private TMP_Text identityLine;
        [SerializeField] private TMP_Text categoryCaption;
        [SerializeField] private RectTransform[] specChipRoots;
        [SerializeField] private TMP_Text[] specChipLabels;
        [SerializeField] private Image docStatusDot;
        [SerializeField] private TMP_Text docStatusLine;

        [Header("Dot sprites (set by builder) — filled = declared, ring = not provided")]
        [SerializeField] private Sprite dotFilledSprite;
        [SerializeField] private Sprite dotRingSprite;

        // ---------------- Tile status rows ----------------
        // Two rows per tile, in tile order: substances, compliance, service, usage.
        [Header("Tile status rows (2 per tile: substances, compliance, service, usage)")]
        [SerializeField] private Image[] statusDots;
        [SerializeField] private TMP_Text[] statusTexts;

        [Header("Compliance tri-state badges (CE, RoHS, REACH)")]
        [SerializeField] private Image[] complianceBadgeStrokes;
        [SerializeField] private TMP_Text[] complianceBadgeLabels;

        // ---------------- Block 1 — composition (spec 14 §3) ----------------
        [Header("Composition")]
        [SerializeField] private RectTransform[] compositionSegments;
        [SerializeField] private Image[] compositionSegmentImages;
        [SerializeField] private TMP_Text compositionInlineLabel;
        [SerializeField] private Image[] legendSwatches;
        [SerializeField] private TMP_Text[] legendLabels;
        [SerializeField] private TMP_Text traceMetalsLine;
        [SerializeField] private TMP_Text compositionFooter;
        [SerializeField] private TMP_Text componentCountCaption;

        // ---------------- Block 2 — climate scenarios (spec 14 §4) ----------------
        [Header("Climate scenarios")]
        [SerializeField] private RectTransform[] scenarioNetBars;
        [SerializeField] private RectTransform[] scenarioSavingBars;
        [SerializeField] private TMP_Text[] scenarioValues;
        [SerializeField] private TMP_Text[] scenarioAxisLabels;
        [SerializeField] private TMP_Text scenarioCaption;

        // ---------------- Block 3 — recovery rates (spec 14 §5) ----------------
        [Header("Recovery rates")]
        [SerializeField] private TMP_Text[] recoveryCategoryLabels;
        [SerializeField] private TMP_Text[] recoveryTotals;
        [SerializeField] private RectTransform[] recoverySeg2;
        [SerializeField] private RectTransform[] recoverySeg3;
        [SerializeField] private RectTransform[] recoverySeg4;

        // =================================================================

        public void Populate(DPPData data)
        {
            if (data == null) return;
            PopulateIdentity(data);
            PopulateStatusTiles(data);
            PopulateComposition(data);
            PopulateScenarios(data);
            PopulateRecovery(data);
        }

        // ---------------- Identity hero ----------------

        private void PopulateIdentity(DPPData d)
        {
            var id = d.identity;
            if (identityLine != null)
                identityLine.text = Join(" · ",
                    id?.manufacturer, id?.model, id?.serial_number,
                    ShortMonth(id?.production_date), id?.country_of_origin);

            if (categoryCaption != null)
                categoryCaption.text = string.IsNullOrEmpty(id?.product_category) ? Dash : id.product_category;

            // Spec chips: one per non-null specification, in a fixed order. Unused
            // pool entries are hidden rather than left showing a stale placeholder.
            var s = d.specifications;
            var chips = new List<string>();
            if (s != null)
            {
                if (!string.IsNullOrEmpty(s.size_mm))          chips.Add($"{s.size_mm} mm");
                if (s.weight_g.HasValue)                        chips.Add($"{s.weight_g.Value:0} g");
                if (!string.IsNullOrEmpty(s.protection_class)) chips.Add(s.protection_class);
                if (!string.IsNullOrEmpty(s.supply_voltage))   chips.Add(s.supply_voltage);
                if (!string.IsNullOrEmpty(s.operating_temp_c)) chips.Add($"{s.operating_temp_c} C");
                if (s.power_consumption_w.HasValue)             chips.Add($"{s.power_consumption_w.Value:0} W");
            }
            FillChips(chips);

            // Mandatory documents that are absent or out of scope (Table 6 #1 #2 #16 #17).
            // "not_applicable" is a correct answer and is worth stating, not hiding.
            var docs = d.documents ?? new List<DocumentRef>();
            var na = docs.Where(x => x != null && x.mandatory && x.status == DppStatus.NotApplicable).ToList();
            var missing = docs.Where(x => x != null && x.mandatory && x.status == DppStatus.NotProvided).ToList();

            if (docStatusLine != null)
            {
                if (na.Count == 0 && missing.Count == 0)
                    docStatusLine.text = docs.Count == 0 ? "no documents referenced" : "all referenced documents available";
                else if (na.Count > 0 && missing.Count == 0)
                    docStatusLine.text = $"{na.Count} mandatory document(s) not applicable to this product group";
                else if (na.Count == 0)
                    docStatusLine.text = $"{missing.Count} mandatory document(s) not provided";
                else
                    docStatusLine.text = $"{na.Count} not applicable · {missing.Count} not provided (mandatory documents)";
            }
            if (docStatusDot != null)
                SetDot(docStatusDot, missing.Count > 0 ? DppBasis.NotProvided : DppBasis.Declared);
        }

        private void FillChips(List<string> chips)
        {
            if (specChipRoots == null || specChipLabels == null) return;
            int n = Mathf.Min(specChipRoots.Length, specChipLabels.Length);
            float x = 0f;
            const float pad = 22f, gap = 6f;
            for (int i = 0; i < n; i++)
            {
                bool has = i < chips.Count;
                if (specChipRoots[i] != null) specChipRoots[i].gameObject.SetActive(has);
                if (!has) continue;

                specChipLabels[i].text = chips[i];
                // Width follows the text so chips never clip or leave dead space.
                //
                // GetPreferredValues, NOT ForceMeshUpdate + preferredWidth: Populate runs
                // while BOTH passport screens are still INACTIVE (the fetch completes on
                // the scan screen), and preferredWidth on a disabled TMP object can read 0
                // — every chip would collapse to the 44 px minimum. GetPreferredValues
                // measures from the text without needing an active mesh.
                float w = Mathf.Max(44f, specChipLabels[i].GetPreferredValues(chips[i]).x + pad);
                specChipRoots[i].sizeDelta = new Vector2(w, specChipRoots[i].sizeDelta.y);
                specChipRoots[i].anchoredPosition = new Vector2(x, specChipRoots[i].anchoredPosition.y);
                x += w + gap;
            }
        }

        // ---------------- Tile status rows ----------------

        private void PopulateStatusTiles(DPPData d)
        {
            // 0/1 substances · 2/3 compliance · 4/5 service · 6/7 usage
            bool battery = d.end_of_life?.contains_battery ?? false;
            var solder = d.components?.FirstOrDefault(c => c.id == "solder");
            SetRow(0, DppBasis.Declared,
                Join(" · ", battery ? "contains battery" : "no battery",
                     solder != null ? solder.material : null));

            int soc = d.substances_of_concern?.Count ?? 0;
            string socBasis = d.end_of_life?.substances_basis ?? DppBasis.NotProvided;
            SetRow(1, soc > 0 ? socBasis : (socBasis == DppBasis.NotProvided ? DppBasis.NotProvided : socBasis),
                soc > 0 ? $"{soc} substance(s) of concern declared"
                        : (socBasis == DppBasis.NotProvided ? "no substance declaration made"
                                                            : "none declared"));

            var comp = d.compliance;
            SetBadge(0, "CE", comp?.ce);
            SetBadge(1, "RoHS", comp?.rohs);
            SetBadge(2, "REACH", comp?.reach);
            SetRow(2, comp?.basis ?? DppBasis.NotProvided,
                string.IsNullOrEmpty(comp?.weee_category) ? "WEEE category not stated" : comp.weee_category);
            int certs = d.certifications?.Count(c => c != null && c.status == DppStatus.Available) ?? 0;
            SetRow(3, certs > 0 ? DppBasis.Declared : DppBasis.NotProvided,
                certs > 0 ? $"{certs} supply-chain certification(s)" : "no supply-chain certification");

            var svc = d.service;
            var guide = d.documents?.FirstOrDefault(x => x != null && x.id == "disassembly_guide");
            SetRow(4, guide != null && guide.status == DppStatus.Available ? DppBasis.Declared : DppBasis.NotProvided,
                guide != null && guide.status == DppStatus.Available ? "disassembly guide in this app"
                                                                    : "no disassembly guide");
            int spares = svc?.spare_parts?.Count(p => p != null && p.status == DppStatus.Available) ?? 0;
            SetRow(5, spares > 0 ? DppBasis.Declared : DppBasis.NotProvided,
                spares > 0 ? $"{spares} spare part(s) listed" : "spare parts · manuals not provided");

            var up = d.environmental?.usage_profile;
            SetRow(6, DppBasis.Assumed, up == null ? "no design life stated" : Join(" · ",
                up.service_life_years.HasValue ? $"design life {up.service_life_years.Value} y" : null,
                up.lifetime_distance_km.HasValue
                    ? string.Format(CultureInfo.InvariantCulture, "{0:N0} km", up.lifetime_distance_km.Value) : null));

            var uh = d.usage_history; var rh = d.repair_history;
            bool hasUse = uh != null && uh.basis != DppBasis.NotProvided;
            int repairs = rh?.events?.Count ?? 0;
            SetRow(7, hasUse || repairs > 0 ? DppBasis.Measured : DppBasis.NotProvided,
                hasUse || repairs > 0 ? $"{repairs} repair event(s) recorded" : "no measured use or repair data");
        }

        private void SetRow(int i, string basis, string text)
        {
            if (statusDots != null && i < statusDots.Length) SetDot(statusDots[i], basis);
            if (statusTexts != null && i < statusTexts.Length && statusTexts[i] != null)
            {
                statusTexts[i].text = string.IsNullOrEmpty(text) ? Dash : text;
                statusTexts[i].color = basis == DppBasis.NotProvided ? DPPTheme.TextTip : DPPTheme.TextSecondary;
            }
        }

        /// <summary>Filled bright = firm source · filled dim = modelled/assumed ·
        /// hollow ring = not provided.
        ///
        /// The state is carried by SWAPPING THE SPRITE, not by Image.fillCenter:
        /// Circle64 is a plain disc with no 9-slice border, so fillCenter=false would
        /// render nothing at all. The builder injects both sprites.</summary>
        private void SetDot(Image dot, string basis)
        {
            if (dot == null) return;
            bool missing = basis == DppBasis.NotProvided;
            var wanted = missing ? dotRingSprite : dotFilledSprite;
            if (wanted != null) dot.sprite = wanted;
            dot.type = Image.Type.Simple;
            dot.color = missing ? DotMissing
                      : (DppBasis.IsFirmSource(basis) ? DotDeclared : DotModelled);
        }

        private void SetBadge(int i, string label, bool? value)
        {
            if (complianceBadgeLabels != null && i < complianceBadgeLabels.Length && complianceBadgeLabels[i] != null)
            {
                // No check/cross glyphs (00 §3 — not in the SF Pro SDF atlas).
                // Colour carries the state: teal = declared, red = declared false,
                // dim + em-dash = not provided.
                complianceBadgeLabels[i].text = value.HasValue ? label : $"{label} {Dash}";
                complianceBadgeLabels[i].color = !value.HasValue ? DPPTheme.TextTip
                    : (value.Value ? DPPTheme.TealText : DPPTheme.SafetyText);
            }
            if (complianceBadgeStrokes != null && i < complianceBadgeStrokes.Length && complianceBadgeStrokes[i] != null)
                complianceBadgeStrokes[i].color = !value.HasValue ? DPPTheme.TextTip
                    : (value.Value ? DPPTheme.TealAccent : DPPTheme.SafetyStroke);
        }

        // ---------------- Block 1 — composition ----------------

        private void PopulateComposition(DPPData d)
        {
            var totals = AggregateMaterials(d);
            float total = totals.Sum(kv => kv.Value);

            if (componentCountCaption != null)
                componentCountCaption.text = total > 0f
                    ? $"{total:0} g · {d.components?.Count ?? 0} components"
                    : Dash;

            int pool = compositionSegments?.Length ?? 0;
            if (pool == 0 || total <= 0f)
            {
                for (int i = 0; i < pool; i++) Show(compositionSegments[i], false);
                if (compositionInlineLabel != null) compositionInlineLabel.text = "";
                return;
            }

            // Materials beyond the pool are merged into the last segment so the bar
            // always sums to 100 % — a bar that does not reach the end would read as
            // missing data rather than as a rounding choice.
            int shown = Mathf.Min(totals.Count, pool);
            var widths = new float[shown];
            float used = 0f;
            for (int i = 0; i < shown; i++)
            {
                float grams = (i == shown - 1)
                    ? totals.Skip(i).Sum(kv => kv.Value)     // tail merged
                    : totals[i].Value;
                widths[i] = Mathf.Max(CompositionMinSegment, grams / total * CompositionBarWidth);
                used += widths[i];
            }
            // Min-width padding can overflow; scale back proportionally.
            if (used > CompositionBarWidth)
                for (int i = 0; i < shown; i++) widths[i] *= CompositionBarWidth / used;

            float x = 0f;
            for (int i = 0; i < pool; i++)
            {
                bool has = i < shown;
                Show(compositionSegments[i], has);
                if (!has) continue;
                compositionSegments[i].anchoredPosition = new Vector2(x, 0f);
                compositionSegments[i].sizeDelta = new Vector2(widths[i], compositionSegments[i].sizeDelta.y);
                if (compositionSegmentImages != null && i < compositionSegmentImages.Length && compositionSegmentImages[i] != null)
                    compositionSegmentImages[i].color = MaterialPalette[i % MaterialPalette.Length];
                x += widths[i];
            }

            if (compositionInlineLabel != null)
                compositionInlineLabel.text = $"{totals[0].Key} {totals[0].Value / total * 100f:0} %";

            int legend = Mathf.Min(legendLabels?.Length ?? 0, totals.Count);
            for (int i = 0; i < (legendLabels?.Length ?? 0); i++)
            {
                bool has = i < legend;
                if (legendLabels[i] != null)
                {
                    legendLabels[i].gameObject.SetActive(has);
                    if (has) legendLabels[i].text = $"{totals[i].Key} {totals[i].Value:0} g";
                }
                if (legendSwatches != null && i < legendSwatches.Length && legendSwatches[i] != null)
                {
                    legendSwatches[i].gameObject.SetActive(has);
                    if (has) legendSwatches[i].color = MaterialPalette[i % MaterialPalette.Length];
                }
            }

            if (traceMetalsLine != null)
            {
                var pm = d.precious_metals;
                traceMetalsLine.text = pm == null || pm.Count == 0
                    ? "trace metals not declared"
                    : string.Join(" · ", pm.OrderByDescending(m => m.mass_mg).Take(4)
                          .Select(m => $"{Symbol(m.metal)} {m.mass_mg:0}")) + " mg";
                traceMetalsLine.color = pm == null || pm.Count == 0 ? DPPTheme.TextTip : DPPTheme.TealText;
            }

            if (compositionFooter != null)
            {
                int more = Mathf.Max(0, totals.Count - legend);
                string basis = d.components?.FirstOrDefault(c => !string.IsNullOrEmpty(c.material_breakdown_basis))
                                   ?.material_breakdown_basis;
                compositionFooter.text = Join(" · ",
                    more > 0 ? $"+ {more} more" : null,
                    "tap for material location per component",
                    string.IsNullOrEmpty(basis) ? null : Truncate(basis, 46));
            }
        }

        /// <summary>Sums every component's material_breakdown into per-material totals,
        /// descending. Falls back to the component's single `material` string when it
        /// has no breakdown, so a partially-populated payload still charts.</summary>
        private static List<KeyValuePair<string, float>> AggregateMaterials(DPPData d)
        {
            var acc = new Dictionary<string, float>();
            if (d.components != null)
                foreach (var c in d.components)
                {
                    if (c == null) continue;
                    if (c.material_breakdown != null && c.material_breakdown.Count > 0)
                        foreach (var b in c.material_breakdown)
                        {
                            if (b == null || string.IsNullOrEmpty(b.material)) continue;
                            acc.TryGetValue(b.material, out float g);
                            acc[b.material] = g + b.weight_g;
                        }
                    else if (!string.IsNullOrEmpty(c.material))
                    {
                        acc.TryGetValue(c.material, out float g);
                        acc[c.material] = g + c.weight_g;
                    }
                }
            return acc.OrderByDescending(kv => kv.Value).ToList();
        }

        // ---------------- Block 2 — climate scenarios ----------------

        private void PopulateScenarios(DPPData d)
        {
            var climate = FindCategory(d, "Climate change");
            int pool = scenarioNetBars?.Length ?? 0;
            if (pool == 0) return;

            if (climate == null || climate.baseline <= 0f)
            {
                for (int i = 0; i < pool; i++)
                {
                    Show(scenarioNetBars[i], false);
                    if (scenarioSavingBars != null && i < scenarioSavingBars.Length) Show(scenarioSavingBars[i], false);
                    if (scenarioValues != null && i < scenarioValues.Length && scenarioValues[i] != null) scenarioValues[i].text = "";
                    if (scenarioAxisLabels != null && i < scenarioAxisLabels.Length && scenarioAxisLabels[i] != null) scenarioAxisLabels[i].text = "";
                }
                if (scenarioCaption != null) scenarioCaption.text = "scenario impacts not provided";
                return;
            }

            // Column 0 is the baseline (no saving); columns 1..n are the scenarios.
            var list = climate.scenarios ?? new List<ImpactRecoveryScenario>();
            int cols = Mathf.Min(pool, 1 + list.Count);
            for (int i = 0; i < pool; i++)
            {
                bool has = i < cols;
                Show(scenarioNetBars[i], has);
                if (scenarioSavingBars != null && i < scenarioSavingBars.Length) Show(scenarioSavingBars[i], has && i > 0);
                if (!has)
                {
                    if (scenarioValues != null && i < scenarioValues.Length && scenarioValues[i] != null) scenarioValues[i].text = "";
                    if (scenarioAxisLabels != null && i < scenarioAxisLabels.Length && scenarioAxisLabels[i] != null) scenarioAxisLabels[i].text = "";
                    continue;
                }

                float net = i == 0 ? climate.baseline : list[i - 1].net;
                float saving = i == 0 ? 0f : list[i - 1].saving;
                float netH = Mathf.Clamp(net / climate.baseline, 0f, 1f) * ScenarioBarHeight;
                float savH = Mathf.Clamp(saving / climate.baseline, 0f, 1f) * ScenarioBarHeight;

                // Bars are pivot-bottom: total height always = baseline, so the teal
                // cap IS the story. Four zero-based bars would look identical.
                SetHeight(scenarioNetBars[i], netH);
                if (scenarioSavingBars != null && i < scenarioSavingBars.Length && i > 0)
                {
                    SetHeight(scenarioSavingBars[i], savH);
                    scenarioSavingBars[i].anchoredPosition =
                        new Vector2(scenarioSavingBars[i].anchoredPosition.x, netH);
                }

                bool deepest = i == cols - 1;
                if (scenarioValues != null && i < scenarioValues.Length && scenarioValues[i] != null)
                {
                    scenarioValues[i].text = net.ToString("0.0", CultureInfo.InvariantCulture);
                    scenarioValues[i].color = deepest ? DPPTheme.TealLight : DPPTheme.Hex("#dbe4f0");
                }
                if (scenarioAxisLabels != null && i < scenarioAxisLabels.Length && scenarioAxisLabels[i] != null)
                {
                    scenarioAxisLabels[i].text = i == 0 ? climate.baseline_scenario : list[i - 1].id;
                    scenarioAxisLabels[i].color = deepest ? DPPTheme.Hex("#dbe4f0") : DPPTheme.TextLabel;
                }
            }

            if (scenarioCaption != null)
                scenarioCaption.text = $"{climate.unit} per unit · teal = avoided";
        }

        // ---------------- Block 3 — recovery rates ----------------

        private void PopulateRecovery(DPPData d)
        {
            var cats = d.environmental?.impact_recovery ?? new List<ImpactRecovery>();
            // Highest screening share first — that ordering IS the prioritisation
            // argument (minerals dominate the EF 3.1 weighted footprint).
            var ordered = cats.Where(c => c != null)
                              .OrderByDescending(c => c.screening_share_pct ?? 0f).ToList();
            int rows = recoveryCategoryLabels?.Length ?? 0;
            for (int i = 0; i < rows; i++)
            {
                bool has = i < ordered.Count;
                var lbl = recoveryCategoryLabels[i];
                if (lbl != null)
                {
                    lbl.gameObject.SetActive(has);
                    if (has) lbl.text = ShortCategory(ordered[i].category);
                }
                if (recoveryTotals != null && i < recoveryTotals.Length && recoveryTotals[i] != null)
                {
                    recoveryTotals[i].gameObject.SetActive(has);
                    if (has)
                    {
                        var deepest = ordered[i].scenarios != null && ordered[i].scenarios.Count > 0
                            ? ordered[i].scenarios[ordered[i].scenarios.Count - 1] : null;
                        recoveryTotals[i].text = deepest == null ? Dash
                            : $"-{deepest.reduction_pct.ToString("0.0", CultureInfo.InvariantCulture)} %";
                        recoveryTotals[i].color = i == 0 ? DPPTheme.TealLight : DPPTheme.TextSecondary;
                    }
                }

                // Stacked increments: each scenario shows only the EXTRA it buys over
                // the previous one, so "deeper recovery gains more" is visible.
                var segs = new[] { Get(recoverySeg2, i), Get(recoverySeg3, i), Get(recoverySeg4, i) };
                float x = 0f, prev = 0f;
                for (int s = 0; s < segs.Length; s++)
                {
                    var sc = has && ordered[i].scenarios != null && s < ordered[i].scenarios.Count
                        ? ordered[i].scenarios[s] : null;
                    if (segs[s] == null) continue;
                    if (sc == null) { Show(segs[s], false); continue; }

                    float w = Mathf.Max(0f, (sc.reduction_pct - prev)) / RecoveryScaleMaxPct * RecoveryTrackWidth;
                    w = Mathf.Clamp(w, 0f, RecoveryTrackWidth - x);
                    Show(segs[s], w > 0.5f);
                    segs[s].anchoredPosition = new Vector2(x, segs[s].anchoredPosition.y);
                    segs[s].sizeDelta = new Vector2(w, segs[s].sizeDelta.y);
                    x += w; prev = sc.reduction_pct;
                }
            }
        }

        // ---------------- helpers ----------------

        private static RectTransform Get(RectTransform[] a, int i) => a != null && i < a.Length ? a[i] : null;
        private static void Show(RectTransform rt, bool on) { if (rt != null && rt.gameObject.activeSelf != on) rt.gameObject.SetActive(on); }
        private static void SetHeight(RectTransform rt, float h) { if (rt != null) rt.sizeDelta = new Vector2(rt.sizeDelta.x, h); }

        private static ImpactRecovery FindCategory(DPPData d, string name)
            => d.environmental?.impact_recovery?.FirstOrDefault(
                   c => c != null && !string.IsNullOrEmpty(c.category) &&
                        c.category.IndexOf(name, System.StringComparison.OrdinalIgnoreCase) >= 0);

        /// <summary>EF 3.1 category names are long; the block is 290 px wide.</summary>
        private static string ShortCategory(string c)
        {
            if (string.IsNullOrEmpty(c)) return Dash;
            if (c.IndexOf("minerals", System.StringComparison.OrdinalIgnoreCase) >= 0) return "Minerals & metals";
            if (c.IndexOf("Eutrophication freshwater", System.StringComparison.OrdinalIgnoreCase) >= 0) return "Freshwater eutroph.";
            if (c.IndexOf("fossils", System.StringComparison.OrdinalIgnoreCase) >= 0) return "Fossil resources";
            return c;
        }

        /// <summary>"Gold (Au)" → "Au"; falls back to the full name.</summary>
        private static string Symbol(string metal)
        {
            if (string.IsNullOrEmpty(metal)) return Dash;
            int o = metal.IndexOf('('), c = metal.IndexOf(')');
            return o >= 0 && c > o ? metal.Substring(o + 1, c - o - 1) : metal;
        }

        private static string ShortMonth(string iso)
            => string.IsNullOrEmpty(iso) ? null : (iso.Length >= 7 ? iso.Substring(0, 7) : iso);

        private static string Truncate(string s, int n)
            => string.IsNullOrEmpty(s) || s.Length <= n ? s : s.Substring(0, n - 2).TrimEnd() + "..";

        private static string Join(string sep, params string[] parts)
        {
            var kept = parts.Where(p => !string.IsNullOrEmpty(p)).ToArray();
            return kept.Length == 0 ? Dash : string.Join(sep, kept);
        }
    }
}
