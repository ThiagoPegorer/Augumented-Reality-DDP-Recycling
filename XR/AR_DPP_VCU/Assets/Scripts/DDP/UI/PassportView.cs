using System;
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
        private const float ProductNameX = 24f;           // ProductName rect x (spec 13 §2)
        private const float SerialGap = 2f;               // the leading space is 11 pt, name is 16
        private static readonly Color SvcRepairColor = DPPTheme.Hex("#e2a44a");

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

        // ---------------- Product information (spec 13 §2, v4) ----------------
        // categoryCaption / specChip* / docStatus* are KEPT but the v4 builder no
        // longer wires them — that content moves to a later tab. Every consumer
        // below null-guards, so they simply render nothing in the meantime.
        [Header("Product information")]
        [SerializeField] private TMP_Text identityLine;
        [SerializeField] private TMP_Text serialLine;
        [SerializeField] private TMP_Text categoryCaption;
        [SerializeField] private RectTransform[] specChipRoots;
        [SerializeField] private TMP_Text[] specChipLabels;
        [SerializeField] private RectTransform[] elecChipRoots;  // v10 - Electrical tile
        [SerializeField] private TMP_Text[] elecChipLabels;      // v10

        // ---------------- Usage Profile (spec 13c) ----------------
        [Header("Usage Profile")]
        [SerializeField] private RectTransform[] usageChipRoots;
        [SerializeField] private TMP_Text[] usageChipLabels;
        [SerializeField] private TMP_Text usageRangeLabel;
        [SerializeField] private RectTransform usageListContent;
        [SerializeField] private RectTransform[] usageYearRows;
        [SerializeField] private TMP_Text[] usageYearLabels;
        [SerializeField] private TMP_Text[] usageKmValues;
        [SerializeField] private TMP_Text[] usageStatValues;

        // ---------------- Compliance & Safety (spec 13d) ----------------
        [Header("Compliance & Safety")]
        [SerializeField] private RectTransform[] compChipRoots;
        [SerializeField] private TMP_Text[] compChipLabels;
        [SerializeField] private TMP_Text compNotesText;
        [SerializeField] private RectTransform compNotesContent;
        [SerializeField] private TMP_Text compDocLine;
        [SerializeField] private TMP_Text[] compStatValues;
        [SerializeField] private Image docStatusDot;
        [SerializeField] private TMP_Text docStatusLine;

        // ---------------- Service & repair (spec 13e) ----------------
        [Header("Service & repair")]
        [SerializeField] private RectTransform[] svcChipRoots;
        [SerializeField] private TMP_Text[] svcChipLabels;
        [SerializeField] private TMP_Text svcRepairHead;
        [SerializeField] private TMP_Text svcRepairMeta;
        [SerializeField] private RectTransform svcRepairContent;
        [SerializeField] private RectTransform[] svcRepairRows;
        [SerializeField] private TMP_Text[] svcRepairDates;
        [SerializeField] private TMP_Text[] svcRepairDescs;
        [SerializeField] private TMP_Text svcUpdateHead;
        [SerializeField] private TMP_Text svcUpdateMeta;
        [SerializeField] private RectTransform svcUpdateContent;
        [SerializeField] private RectTransform[] svcUpdateRows;
        [SerializeField] private TMP_Text[] svcUpdateDates;
        [SerializeField] private TMP_Text[] svcUpdateVersions;

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

        [Header("Dot sprites (set by builder) — filled = declared, ring = not provided")]
        [SerializeField] private Sprite dotFilledSprite;
        [SerializeField] private Sprite dotRingSprite;

        // =================================================================

        public void Populate(DPPData data)
        {
            if (data == null) return;
            PopulateIdentity(data);
            PopulateElectrical(data);
            PopulateService(data);
            PopulateUsageProfile(data);
            PopulateCompliance(data);
            PopulateComposition(data);
            PopulateScenarios(data);
            PopulateRecovery(data);
        }

        // ---------------- Product information ----------------

        private void PopulateIdentity(DPPData d)
        {
            var id = d.identity;
            // v4: manufacturer | model on one line, serial on its own beneath it.
            // production_date and country_of_origin used to ride here and now have
            // nowhere to render — ShortMonth is kept for whichever tab claims them.
            if (identityLine != null)
                identityLine.text = Join(" | ", id?.manufacturer, id?.model);

            if (serialLine != null)
            {
                string sn = string.IsNullOrEmpty(id?.serial_number) ? Dash : id.serial_number;
                serialLine.text = " - " + sn;
                PlaceSerialAfterName();
            }

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
                // v8: supply_voltage is NOT here any more — it is the Electrical
                // data tile's own chip. Degree sign is safe: U+00B0 is in the SF Pro
                // SDF atlas (unlike U+2264 and U+2212, which must never be used).
                if (!string.IsNullOrEmpty(s.operating_temp_c)) chips.Add($"{s.operating_temp_c} °C");
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

        /// <summary>Parks the serial immediately after the product name, on the same line.
        ///
        /// Two TMP objects rather than one rich-text string: the name is drawn with the
        /// dedicated BOLD font asset, and no rich-text tag can switch a font ASSET off,
        /// so an inline serial would come out bold.
        ///
        /// GetPreferredValues, NOT preferredWidth: Populate runs while BOTH passport
        /// screens are still INACTIVE (the fetch completes on the scan screen) and
        /// preferredWidth reads 0 on a disabled TMP object — the serial would land on
        /// top of the name. Same trap, same fix, as FillChips.</summary>
        private void PlaceSerialAfterName()
        {
            if (identityLine == null || serialLine == null) return;
            float w = identityLine.GetPreferredValues(identityLine.text).x;
            var rt = serialLine.rectTransform;
            rt.anchoredPosition = new Vector2(ProductNameX + w + SerialGap, rt.anchoredPosition.y);
        }

        private void FillChips(List<string> chips) => LayoutChips(specChipRoots, specChipLabels, chips);

        /// <summary>Lays a chip row out left to right, each chip sized to its own text and
        /// unused pool entries hidden. v10: shared by the Mechanical and the Electrical
        /// rows so the two are literally the same widget, not two that merely look alike.</summary>
        private static void LayoutChips(RectTransform[] roots, TMP_Text[] labels, List<string> chips)
        {
            var specChipRoots = roots; var specChipLabels = labels;
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
                // GetPreferredValues, NOT ForceMeshUpdate + preferredWidth: Populate runs
                // while BOTH passport screens are still INACTIVE (the fetch completes on
                // the scan screen), and preferredWidth on a disabled TMP object can read 0
                // — every chip would collapse to the 44 px minimum.
                float w = Mathf.Max(44f, specChipLabels[i].GetPreferredValues(chips[i]).x + pad);
                specChipRoots[i].sizeDelta = new Vector2(w, specChipRoots[i].sizeDelta.y);
                specChipRoots[i].anchoredPosition = new Vector2(x, specChipRoots[i].anchoredPosition.y);
                x += w + gap;
            }
        }

        // ---------------- Usage Profile (spec 13c) ----------------

        /// <summary>Fills the Usage Profile tile chips and the 6 x 2 detail page: the
        /// pinch-scrollable km-per-year list (content height drives PinchScrollArea)
        /// and the six centred stat values. Everything from environmental.usage_profile —
        /// the S4 use-phase model plus the out-of-boundary car estimate; no literals.</summary>
        private void PopulateUsageProfile(DPPData d)
        {
            var up = d.environmental?.usage_profile;

            var chips = new List<string>();
            if (up?.service_life_years != null) chips.Add($"{up.service_life_years.Value} years");
            if (up?.lifetime_distance_km != null)
                chips.Add(string.Format(CultureInfo.InvariantCulture, "{0:N0} km", up.lifetime_distance_km.Value));
            LayoutChips(usageChipRoots, usageChipLabels, chips);

            if (usageRangeLabel != null) usageRangeLabel.text = up?.service_period ?? "";

            var years = up?.annual_distances?.Where(a => a != null).ToList() ?? new List<AnnualDistance>();
            if (usageYearRows != null)
            {
                int n = usageYearRows.Length;
                for (int i = 0; i < n; i++)
                {
                    bool has = i < years.Count;
                    if (usageYearRows[i] != null) usageYearRows[i].gameObject.SetActive(has);
                    if (!has) continue;
                    bool partial = !string.IsNullOrEmpty(years[i].note);
                    if (usageYearLabels != null && i < usageYearLabels.Length && usageYearLabels[i] != null)
                    {
                        usageYearLabels[i].text = partial ? $"{years[i].year}  ·  {years[i].note}" : years[i].year;
                        usageYearLabels[i].color = partial ? DPPTheme.TextCaption : DPPTheme.TextSecondary;
                    }
                    if (usageKmValues != null && i < usageKmValues.Length && usageKmValues[i] != null)
                        usageKmValues[i].text = string.Format(CultureInfo.InvariantCulture, "{0:N0} km", years[i].distance_km);
                }
                if (usageListContent != null)
                    usageListContent.sizeDelta = new Vector2(usageListContent.sizeDelta.x,
                        Mathf.Min(n, years.Count) * 25f + 4f);
            }

            SetStat(0, up?.lifetime_distance_km != null
                ? string.Format(CultureInfo.InvariantCulture, "{0:N0} km", up.lifetime_distance_km.Value) : null);
            SetStat(1, up?.operating_hours != null
                ? string.Format(CultureInfo.InvariantCulture, "{0:N0} h", up.operating_hours.Value) : null);
            SetStat(2, up?.lifetime_energy_kwh != null
                ? string.Format(CultureInfo.InvariantCulture, "{0:0.#} kWh", up.lifetime_energy_kwh.Value) : null);
            SetStat(3, up?.car_energy_kwh_estimate != null
                ? string.Format(CultureInfo.InvariantCulture, "{0:N0} kWh", up.car_energy_kwh_estimate.Value) : null);
            SetStat(4, up?.avg_speed_kmh != null
                ? string.Format(CultureInfo.InvariantCulture, "{0:0.#} km/h", up.avg_speed_kmh.Value) : null);
            SetStat(5, string.IsNullOrEmpty(up?.daily_use) ? null : up.daily_use);
        }

        private void SetStat(int i, string text)
        {
            if (usageStatValues == null || i >= usageStatValues.Length || usageStatValues[i] == null) return;
            usageStatValues[i].text = string.IsNullOrEmpty(text) ? Dash : text;
        }

        // ---------------- Compliance & Safety (spec 13d) ----------------

        /// <summary>Chips, the six declared values and the scrollable declaration
        /// notes — all off the Bosch EC/EU DoC (manual pp. 132-134). The notes are one
        /// wrapping rich-text block; its measured height drives PinchScrollArea.</summary>
        private void PopulateCompliance(DPPData d)
        {
            var comp = d.compliance;
            var soc = d.substances_of_concern?.Where(x => x != null).ToList() ?? new List<SubstanceOfConcern>();

            var chips = new List<string>();
            if (comp?.ce == true)
                chips.Add(string.IsNullOrEmpty(comp.ce_scope) ? "CE" : $"CE ({ShortScope(comp.ce_scope)})");
            if (soc.Count > 0) chips.Add($"{soc.Count} SVHC declared");
            LayoutChips(compChipRoots, compChipLabels, chips);

            SetCompStat(0, comp?.ce == true ? comp.ce_scope : null, false);
            SetCompStat(1, comp?.tested_to, false);
            // RoHS: three genuinely different states — conformant, non-conformant and
            // OUT OF SCOPE. "not applicable" renders dimmer than a real value.
            SetCompStat(2, comp?.rohs == true ? "conforms"
                         : comp?.rohs == false ? "does not conform"
                         : comp?.rohs_applicable == false ? "not applicable" : null,
                         dim: comp?.rohs == null);
            string symbols = string.Join(", ", soc.Where(x => !string.IsNullOrEmpty(x.symbol)).Select(x => x.symbol));
            SetCompStat(3, soc.Count > 0
                ? (symbols.Length > 0 ? $"{soc.Count} declared · {symbols}" : $"{soc.Count} declared")
                : "none declared", dim: soc.Count == 0);
            SetCompStat(4, comp?.weee_category, false);
            SetCompStat(5, LongDate(comp?.declaration_date), false);

            if (compDocLine != null)
                compDocLine.text = comp?.declaration_date != null
                    ? $"Bosch DoC · {LongDate(comp.declaration_date)}" : "";

            // ---- notes: DoC sections + the full SVHC identities (Art. 33 needs the
            // names and CAS numbers somewhere on the page; the face shows symbols) ----
            if (compNotesText != null)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var nte in comp?.declaration_notes ?? new List<DeclarationNote>())
                {
                    if (nte == null) continue;
                    sb.Append("<color=#5dcaa5><size=8.8>").Append(nte.title.ToUpperInvariant())
                      .Append("</size></color>\n").Append(nte.body).Append("\n\n");
                }
                if (soc.Count > 0)
                {
                    sb.Append("<color=#5dcaa5><size=8.8>SVHC (REACH ART. 33, > 0.1 % W/W)</size></color>\n");
                    foreach (var x in soc)
                        sb.Append(x.name)
                          .Append(string.IsNullOrEmpty(x.cas_number) ? "" : $" · CAS {x.cas_number}")
                          .Append("\n");
                }
                compNotesText.text = sb.ToString().TrimEnd();

                if (compNotesContent != null)
                {
                    float h = compNotesText.GetPreferredValues(compNotesText.text, 254f, 0f).y;
                    compNotesContent.sizeDelta = new Vector2(compNotesContent.sizeDelta.x, h + 8f);
                }
            }
        }

        private void SetCompStat(int i, string text, bool dim)
        {
            if (compStatValues == null || i >= compStatValues.Length || compStatValues[i] == null) return;
            compStatValues[i].text = string.IsNullOrEmpty(text) ? Dash : text;
            compStatValues[i].color = dim ? DPPTheme.TextSecondary : DPPTheme.TextOnNavy;
        }

        /// <summary>"2014/30/EU (EMC)" → "EMC" for the tile chip.</summary>
        private static string ShortScope(string scope)
        {
            int a = scope.IndexOf('('); int z = scope.IndexOf(')');
            return a >= 0 && z > a ? scope.Substring(a + 1, z - a - 1) : scope;
        }

        // ---------------- Tile status rows ----------------

        /// <summary>Supply-voltage chip on the Electrical data tile. Widened to its own
        /// text the same way the spec chips are, and HIDDEN rather than left showing a dash
        /// when the payload has no value.
        ///
        /// GetPreferredValues, not preferredWidth — Populate runs while the screen is still
        /// inactive (see FillChips).</summary>
        private void PopulateElectrical(DPPData d)
        {
            // v10: supply voltage and the component count are two chips on one row, in the
            // same style as Mechanical data. The count is the sum of physical_unit.parts[],
            // i.e. the coloured blocks actually inside the demonstrator.
            var chips = new List<string>();
            string v = d.specifications?.supply_voltage;
            if (!string.IsNullOrEmpty(v)) chips.Add(v);

            int parts = d.physical_unit?.parts?.Sum(p => p == null ? 0 : Mathf.Max(1, p.count)) ?? 0;
            if (parts > 0) chips.Add($"{parts} components");

            LayoutChips(elecChipRoots, elecChipLabels, chips);
        }

        // ---------------- Service & repair detail (spec 13e) ----------------

        /// <summary>Spec 13e — the twin service histories. Chips on the tile, heads with
        /// counts, and two scrollable lists whose content heights drive PinchScrollArea.
        /// Both collections carry basis "simulated" in the payload; ordering is oldest
        /// first, like the Usage year list.</summary>
        private void PopulateService(DPPData d)
        {
            var ups = d.service?.software_updates?.Where(u => u != null && ParseDate(u.date).HasValue)
                        .OrderBy(u => ParseDate(u.date).Value).ToList() ?? new List<SoftwareUpdate>();
            var reps = d.repair_history?.events?.Where(e => e != null && ParseDate(e.date).HasValue)
                        .OrderBy(e => ParseDate(e.date).Value).ToList() ?? new List<RepairEvent>();

            var chips = new List<string>();
            if (ups.Count > 0) chips.Add($"{ups.Count} updates");
            if (reps.Count > 0) chips.Add($"{reps.Count} repairs");
            LayoutChips(svcChipRoots, svcChipLabels, chips);

            if (svcRepairHead != null) svcRepairHead.text = $"REPAIRS · {reps.Count}";
            if (svcRepairMeta != null)
                svcRepairMeta.text = reps.Count > 0 ? $"first: {MonthYear(reps[0].date)}" : "none recorded";
            if (svcUpdateHead != null) svcUpdateHead.text = $"SOFTWARE UPDATES · {ups.Count}";
            if (svcUpdateMeta != null) svcUpdateMeta.text = ups.Count > 1 ? CadenceCaption(ups) : "";

            if (svcRepairRows != null)
            {
                for (int i = 0; i < svcRepairRows.Length; i++)
                {
                    bool has = i < reps.Count;
                    if (svcRepairRows[i] != null) svcRepairRows[i].gameObject.SetActive(has);
                    if (!has) continue;
                    if (svcRepairDates != null && i < svcRepairDates.Length && svcRepairDates[i] != null)
                        svcRepairDates[i].text = LongDate(reps[i].date);
                    if (svcRepairDescs != null && i < svcRepairDescs.Length && svcRepairDescs[i] != null)
                        svcRepairDescs[i].text = string.IsNullOrEmpty(reps[i].description) ? Dash : reps[i].description;
                }
                if (svcRepairContent != null)
                    svcRepairContent.sizeDelta = new Vector2(svcRepairContent.sizeDelta.x,
                        Mathf.Min(svcRepairRows.Length, reps.Count) * 34f + 4f);
            }

            if (svcUpdateRows != null)
            {
                for (int i = 0; i < svcUpdateRows.Length; i++)
                {
                    bool has = i < ups.Count;
                    if (svcUpdateRows[i] != null) svcUpdateRows[i].gameObject.SetActive(has);
                    if (!has) continue;
                    if (svcUpdateDates != null && i < svcUpdateDates.Length && svcUpdateDates[i] != null)
                        svcUpdateDates[i].text = LongDate(ups[i].date);
                    if (svcUpdateVersions != null && i < svcUpdateVersions.Length && svcUpdateVersions[i] != null)
                        svcUpdateVersions[i].text = string.IsNullOrEmpty(ups[i].version) ? Dash : ups[i].version;
                }
                if (svcUpdateContent != null)
                    svcUpdateContent.sizeDelta = new Vector2(svcUpdateContent.sizeDelta.x,
                        Mathf.Min(svcUpdateRows.Length, ups.Count) * 25f + 4f);
            }
        }

        private static DateTime? ParseDate(string s) =>
            DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var d) ? d : (DateTime?)null;

        private static string LongDate(string s)
        {
            var d = ParseDate(s);
            return d.HasValue ? d.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : Dash;
        }

        private static string MonthYear(string s)
        {
            var d = ParseDate(s);
            return d.HasValue ? d.Value.ToString("MMM yyyy", CultureInfo.InvariantCulture) : Dash;
        }

        /// <summary>Cadence stated from the DATA, not assumed: mean gap between updates,
        /// in months when the gap is month-scale (the v13 history is quarterly).</summary>
        private static string CadenceCaption(List<SoftwareUpdate> ups)
        {
            var a = ParseDate(ups[0].date).Value;
            var b = ParseDate(ups[ups.Count - 1].date).Value;
            int days = Mathf.RoundToInt((float)((b - a).TotalDays / (ups.Count - 1)));
            if (days >= 28)
            {
                int months = Mathf.Max(1, Mathf.RoundToInt(days / 30.44f));
                return $"automatic · every {months} months";
            }
            return days % 7 == 0 && days > 0
                ? $"automatic · every {days / 7} weeks"
                : $"automatic · every {days} days";
        }

        private static string Basis(string b) => string.IsNullOrEmpty(b) ? DppBasis.NotProvided : b;

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
