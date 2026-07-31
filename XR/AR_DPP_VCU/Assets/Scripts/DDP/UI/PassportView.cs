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
        private const float SvcTrackX = 24f, SvcTrackW = 544f;   // timeline geometry (mirrors the builder)
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
        [SerializeField] private RectTransform supplyChipRoot;   // v8 - Electrical tile
        [SerializeField] private TMP_Text supplyChipLabel;       // v8
        [SerializeField] private Image docStatusDot;
        [SerializeField] private TMP_Text docStatusLine;

        // ---------------- Service & repair detail (spec 13 v9) ----------------
        [Header("Service & repair detail")]
        [SerializeField] private TMP_Text svcUpdateCount;
        [SerializeField] private TMP_Text svcUpdateCaption;
        [SerializeField] private TMP_Text svcRepairCount;
        [SerializeField] private TMP_Text svcRepairCaption;
        [SerializeField] private TMP_Text svcVersionRange;
        [SerializeField] private RectTransform[] svcTicks;
        [SerializeField] private RectTransform svcRepairMarker;
        [SerializeField] private RectTransform[] svcMonthTicks;
        [SerializeField] private TMP_Text[] svcMonthLabels;
        [SerializeField] private Image[] svcLogDots;
        [SerializeField] private TMP_Text[] svcLogDates;
        [SerializeField] private TMP_Text[] svcLogDescs;
        [SerializeField] private TMP_Text[] svcLogRights;
        [SerializeField] private TMP_Text svcLogFooter;

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
            PopulateElectrical(data);
            PopulateService(data);
            PopulateStatusTiles(data);
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

        /// <summary>Supply-voltage chip on the Electrical data tile. Widened to its own
        /// text the same way the spec chips are, and HIDDEN rather than left showing a dash
        /// when the payload has no value.
        ///
        /// GetPreferredValues, not preferredWidth — Populate runs while the screen is still
        /// inactive (see FillChips).</summary>
        private void PopulateElectrical(DPPData d)
        {
            string v = d.specifications?.supply_voltage;
            bool has = !string.IsNullOrEmpty(v);
            if (supplyChipRoot != null) supplyChipRoot.gameObject.SetActive(has);
            if (!has || supplyChipLabel == null || supplyChipRoot == null) return;

            supplyChipLabel.text = v;
            float w = Mathf.Max(44f, supplyChipLabel.GetPreferredValues(v).x + 22f);
            supplyChipRoot.sizeDelta = new Vector2(w, supplyChipRoot.sizeDelta.y);
        }

        private void PopulateStatusTiles(DPPData d)
        {
            // 0 electrical parts · 2/3 compliance · 4/5 service · 6/7 usage
            //
            // ⚠ v8 removed the substances rows with the tile. The passport no longer says
            // anything about substances of concern (Table 6 #5 #6 #7 #16 #17). Restoring it
            // is a tile plus the block below, which is why the code is left here in comment
            // rather than deleted:
            //     int soc = d.substances_of_concern?.Count ?? 0;
            //     string socBasis = d.end_of_life?.substances_basis ?? DppBasis.NotProvided;
            //     -> "no substance declaration made" when soc == 0 and basis is not_provided
            //
            // Row 0 counts the PHYSICAL DEMONSTRATOR's parts, not product data: it must
            // agree with the coloured blocks the participant is holding.
            var pu = d.physical_unit;
            int partCount = pu?.parts?.Sum(p => p == null ? 0 : Mathf.Max(1, p.count)) ?? 0;
            SetRow(0, partCount > 0 ? (string.IsNullOrEmpty(pu.basis) ? DppBasis.Measured : pu.basis)
                                    : DppBasis.NotProvided,
                partCount > 0 ? $"{partCount} electrical parts" : "parts list — not provided");

            var comp = d.compliance;
            SetBadge(0, "CE", comp?.ce);
            SetBadge(1, "RoHS", comp?.rohs);
            SetBadge(2, "REACH", comp?.reach);
            SetRow(2, comp?.basis ?? DppBasis.NotProvided,
                string.IsNullOrEmpty(comp?.weee_category) ? "WEEE category not stated" : comp.weee_category);
            int certs = d.certifications?.Count(c => c != null && c.status == DppStatus.Available) ?? 0;
            SetRow(3, certs > 0 ? DppBasis.Declared : DppBasis.NotProvided,
                certs > 0 ? $"{certs} supply-chain certification(s)" : "no supply-chain certification");

            // v9: the face carries the two TOTALS Thiago asked for. The lines it used to
            // show — "disassembly guide in this app" (T6 #12) and the spare-parts state
            // (T6 #15) — move to the detail page; they are not lost, just one level down.
            var svc = d.service;
            int updates = svc?.software_updates?.Count(u => u != null) ?? 0;
            SetRow(4, updates > 0 ? Basis(svc.software_update_basis) : DppBasis.NotProvided,
                updates > 0 ? $"{updates} automatic updates" : "no update history");

            var repEvents = d.repair_history?.events?.Where(e => e != null).ToList();
            int repairs = repEvents?.Count ?? 0;
            SetRow(5, repairs > 0 ? Basis(d.repair_history.basis) : DppBasis.NotProvided,
                repairs > 0 ? $"{repairs} repair · {MonthYear(repEvents[repairs - 1].date)}"
                            : "no repair recorded");

            var up = d.environmental?.usage_profile;
            SetRow(6, DppBasis.Assumed, up == null ? "no design life stated" : Join(" · ",
                up.service_life_years.HasValue ? $"design life {up.service_life_years.Value} y" : null,
                up.lifetime_distance_km.HasValue
                    ? string.Format(CultureInfo.InvariantCulture, "{0:N0} km", up.lifetime_distance_km.Value) : null));

            var uh = d.usage_history; var rh = d.repair_history;
            bool hasUse = uh != null && uh.basis != DppBasis.NotProvided;
            int rhCount = rh?.events?.Count ?? 0;
            // v9: take the basis from the RECORD, never assume "measured". The repair log
            // is simulated, and this row must not upgrade it to a firm source.
            string useBasis = hasUse ? Basis(uh.basis)
                            : rhCount > 0 ? Basis(rh.basis) : DppBasis.NotProvided;
            SetRow(7, useBasis,
                hasUse || rhCount > 0 ? $"{rhCount} repair event(s) recorded" : "no measured use or repair data");
        }

        // ---------------- Service & repair detail (spec 13 v9) ----------------

        /// <summary>Fills the update/repair counters, places one tick per software update on
        /// the timeline, hangs the repair marker below the axis and lists the most recent
        /// entries. Every number and position comes from the payload.
        ///
        /// ⚠ The payload marks both collections "simulated". Basis() passes that through so
        /// the dots stay dim — nothing here may present invented data as measured.</summary>
        private void PopulateService(DPPData d)
        {
            var ups = d.service?.software_updates?.Where(u => u != null && ParseDate(u.date).HasValue)
                        .OrderBy(u => ParseDate(u.date).Value).ToList() ?? new List<SoftwareUpdate>();
            var reps = d.repair_history?.events?.Where(e => e != null && ParseDate(e.date).HasValue)
                        .OrderBy(e => ParseDate(e.date).Value).ToList() ?? new List<RepairEvent>();

            if (svcUpdateCount != null) svcUpdateCount.text = ups.Count.ToString();
            if (svcRepairCount != null) svcRepairCount.text = reps.Count.ToString();
            if (svcUpdateCaption != null)
                svcUpdateCaption.text = ups.Count > 1 ? CadenceCaption(ups) : "no update history";
            if (svcRepairCaption != null)
                svcRepairCaption.text = reps.Count > 0
                    ? $"manual · {LongDate(reps[reps.Count - 1].date)}" : "none recorded";
            if (svcVersionRange != null)
                svcVersionRange.text = ups.Count > 0 ? $"{ups[0].version} → {ups[ups.Count - 1].version}" : Dash;

            // Timeline spans first update -> one cadence beyond the last, so the axis shows
            // when the next one is due without inventing an event for it.
            DateTime t0 = ups.Count > 0 ? ParseDate(ups[0].date).Value : DateTime.MinValue;
            DateTime tN = ups.Count > 0 ? ParseDate(ups[ups.Count - 1].date).Value : t0;
            double cadence = ups.Count > 1 ? (tN - t0).TotalDays / (ups.Count - 1) : 14.0;
            DateTime t1 = tN.AddDays(cadence);
            double span = Math.Max(1.0, (t1 - t0).TotalDays);

            if (svcTicks != null)
                for (int i = 0; i < svcTicks.Length; i++)
                {
                    bool has = i < ups.Count;
                    if (svcTicks[i] == null) continue;
                    svcTicks[i].gameObject.SetActive(has);
                    if (!has) continue;
                    float x = TrackX(ParseDate(ups[i].date).Value, t0, span);
                    svcTicks[i].anchoredPosition = new Vector2(x - 1f, svcTicks[i].anchoredPosition.y);
                }

            if (svcRepairMarker != null)
            {
                bool has = reps.Count > 0 && ups.Count > 0;
                svcRepairMarker.gameObject.SetActive(has);
                if (has)
                {
                    float x = TrackX(ParseDate(reps[0].date).Value, t0, span);
                    svcRepairMarker.anchoredPosition = new Vector2(x - 6f, svcRepairMarker.anchoredPosition.y);
                }
            }

            PlaceMonths(t0, t1, span, ups.Count > 0);
            FillLog(ups, reps);
        }

        /// <summary>One label per month boundary inside the timeline range.</summary>
        private void PlaceMonths(DateTime t0, DateTime t1, double span, bool any)
        {
            if (svcMonthTicks == null || svcMonthLabels == null) return;
            var marks = new List<DateTime>();
            if (any)
            {
                var m = new DateTime(t0.Year, t0.Month, 1);
                while (m <= t1)
                {
                    if (m >= t0) marks.Add(m);
                    m = m.AddMonths(1);
                }
            }
            int n = Mathf.Min(svcMonthTicks.Length, svcMonthLabels.Length);
            for (int i = 0; i < n; i++)
            {
                bool has = i < marks.Count;
                if (svcMonthTicks[i] != null) svcMonthTicks[i].gameObject.SetActive(has);
                if (svcMonthLabels[i] != null) svcMonthLabels[i].gameObject.SetActive(has);
                if (!has) continue;
                float x = TrackX(marks[i], t0, span);
                if (svcMonthTicks[i] != null)
                    svcMonthTicks[i].anchoredPosition = new Vector2(x, svcMonthTicks[i].anchoredPosition.y);
                if (svcMonthLabels[i] != null)
                {
                    svcMonthLabels[i].text = marks[i].ToString("MMM", CultureInfo.InvariantCulture);
                    svcMonthLabels[i].rectTransform.anchoredPosition =
                        new Vector2(x - 20f, svcMonthLabels[i].rectTransform.anchoredPosition.y);
                }
            }
        }

        /// <summary>Most recent entries first, updates and repairs merged.</summary>
        private void FillLog(List<SoftwareUpdate> ups, List<RepairEvent> reps)
        {
            if (svcLogDates == null) return;
            var rows = new List<(DateTime when, string date, string desc, string right, bool repair)>();
            foreach (var u in ups)
                rows.Add((ParseDate(u.date).Value, LongDate(u.date), "Automatic software update",
                          string.IsNullOrEmpty(u.version) ? Dash : u.version, false));
            foreach (var e in reps)
                rows.Add((ParseDate(e.date).Value, LongDate(e.date),
                          "Repair — " + (string.IsNullOrEmpty(e.description) ? Dash : e.description),
                          e.cost_eur.HasValue
                            ? string.Format(CultureInfo.InvariantCulture, "€ {0:N2}", e.cost_eur.Value) : "",
                          true));
            rows = rows.OrderByDescending(r => r.when).ToList();

            int n = svcLogDates.Length;
            for (int i = 0; i < n; i++)
            {
                bool has = i < rows.Count;
                if (svcLogDots != null && i < svcLogDots.Length && svcLogDots[i] != null)
                {
                    svcLogDots[i].gameObject.SetActive(has);
                    if (has) svcLogDots[i].color = rows[i].repair ? SvcRepairColor : DPPTheme.TealLight;
                }
                SetLogCell(svcLogDates, i, has, has ? rows[i].date : null);
                SetLogCell(svcLogDescs, i, has, has ? rows[i].desc : null);
                SetLogCell(svcLogRights, i, has, has ? rows[i].right : null);
                if (has && svcLogRights != null && i < svcLogRights.Length && svcLogRights[i] != null)
                    svcLogRights[i].color = rows[i].repair ? SvcRepairColor : DPPTheme.TextTip;
            }
            if (svcLogFooter != null)
            {
                int hidden = Mathf.Max(0, rows.Count - n);
                svcLogFooter.text = hidden > 0
                    ? $"{hidden} earlier entries not listed · full log in the payload" : "";
            }
        }

        private static void SetLogCell(TMP_Text[] pool, int i, bool has, string text)
        {
            if (pool == null || i >= pool.Length || pool[i] == null) return;
            pool[i].gameObject.SetActive(has);
            if (has) pool[i].text = text ?? "";
        }

        private static float TrackX(DateTime d, DateTime t0, double span) =>
            SvcTrackX + (float)((d - t0).TotalDays / span) * SvcTrackW;

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

        /// <summary>Cadence stated from the DATA, not assumed: median gap between updates.</summary>
        private static string CadenceCaption(List<SoftwareUpdate> ups)
        {
            var a = ParseDate(ups[0].date).Value;
            var b = ParseDate(ups[ups.Count - 1].date).Value;
            int days = Mathf.RoundToInt((float)((b - a).TotalDays / (ups.Count - 1)));
            return days % 7 == 0 && days > 0
                ? $"automatic · every {days / 7} weeks"
                : $"automatic · every {days} days";
        }

        private static string Basis(string b) => string.IsNullOrEmpty(b) ? DppBasis.NotProvided : b;

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
