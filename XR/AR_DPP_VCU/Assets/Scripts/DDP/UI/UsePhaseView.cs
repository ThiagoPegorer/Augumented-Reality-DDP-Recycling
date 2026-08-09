using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 — 06: USAGE &amp; SERVICE (spec `RB2_1_1/06_usage_service.md` v2,
    /// mock 04a_v6 approved 2026-08-09). Three lenses — Thermal Data (default),
    /// Electrical Data, Software — that RE-TINT THE STAGE MODEL through the model
    /// link; on this tab a pinch opens the part's USAGE RECORD, not Component ID.
    ///
    /// NO PAGE TITLE (general rule, 2026-08-09): the rail already names the tab.
    ///
    /// WHY THIS SCREEN EXISTS (spec §1): Sc4's functional reuse yield is declared
    /// [A] 0.5–0.9 because literature cannot supply it. This screen derives 0.767
    /// by mass from per-component verdicts — the passport supplies evidence for
    /// the assumption the LCA had to make.
    /// </summary>
    public class UsePhaseView : MonoBehaviour
    {
        public const int LensCount = 3;   // Thermal Data · Electrical Data · Software

        [Header("Wiring (RBv2_1_1/3)")]
        [SerializeField] private SuperPanelView owner;
        [SerializeField] private ModelLinkController modelLink;

        [Header("Lens pills")]
        [SerializeField] private Image[] lensFills;
        [SerializeField] private Image[] lensStrokes;
        [SerializeField] private TMP_Text[] lensLabels;

        [Header("Lens content roots (index-aligned with the pills)")]
        [SerializeField] private GameObject[] lensRoots;

        [Header("Thermal Data lens")]
        [SerializeField] private TMP_Text sohValue;
        [SerializeField] private TMP_Text sohMechanism;
        [SerializeField] private RectTransform flashBar;      // width against barTrack
        [SerializeField] private TMP_Text flashPct;
        [SerializeField] private RectTransform fatigueBar;
        [SerializeField] private TMP_Text fatiguePct;
        [SerializeField] private TMP_Text reuseFraction;
        [SerializeField] private TMP_Text[] deltaBandLabels;  // 5 rows
        [SerializeField] private TMP_Text[] deltaCycles;
        [SerializeField] private TMP_Text[] deltaDamage;
        [SerializeField] private RectTransform[] deltaDamageBars;
        [SerializeField] private TMP_Text damageSum;
        [SerializeField] private TMP_Text[] findingLabels;    // 2 rows
        [SerializeField] private TMP_Text[] findingValues;
        [SerializeField] private float barTrack = 160f;
        [SerializeField] private float damageBarTrack = 140f;

        [Header("Electrical Data lens")]
        [SerializeField] private TMP_Text elTransients;
        [SerializeField] private TMP_Text elUndervoltage;
        [SerializeField] private TMP_Text elLoadDumps;
        [SerializeField] private TMP_Text daFlash;
        [SerializeField] private TMP_Text daCpu;
        [SerializeField] private TMP_Text daEccResets;
        [SerializeField] private TMP_Text daCan;
        [SerializeField] private TMP_Text daDtc;

        [Header("Software lens")]
        [SerializeField] private TMP_Text swFirmware;
        [SerializeField] private TMP_Text swMaps;
        [SerializeField] private TMP_Text swRecal;
        [SerializeField] private TMP_Text swLinkage;

        [Header("Part record (opened by a pinch on the stage model)")]
        [SerializeField] private GameObject partRecordRoot;
        [SerializeField] private TMP_Text partName;
        [SerializeField] private Image partVerdictChip;
        [SerializeField] private TMP_Text partVerdict;
        [SerializeField] private TMP_Text partMass;
        [SerializeField] private TMP_Text partReason;

        [Header("Bottom bar")]
        [SerializeField] private TMP_Text primaryLabel;

        // ---- palette (spec §5: red appears nowhere on this screen) ----
        private static readonly Color VerdictReuse      = DPPTheme.Hex("#2eb086");
        private static readonly Color VerdictAfterTest  = DPPTheme.Hex("#f0c879");
        private static readonly Color VerdictRecovery   = DPPTheme.Hex("#21407a");   // deliberately NOT red
        private static readonly Color VerdictConsumable = DPPTheme.Hex("#6f86a8");
        private static readonly Color LitBlue           = DPPTheme.Hex("#4da3ff");
        private static readonly Color Neutral           = DPPTheme.Hex("#3a4a63");

        private DPPData _data;
        private int _lens;
        private bool _recordOpen;

        // Trap 1: the pill fills are state-coloured AND hover-brightened, so the
        // colour must go through HoverHighlight.SetRestFillColor — a direct write
        // survives only until the next hover ease repaints the captured colour.
        private HoverHighlight[] _lensHovers;

        private void OnEnable()
        {
            if (_data == null)
            {
                var mgr = FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
                if (mgr != null && mgr.Latest != null) Populate(mgr.Latest);
            }
            _recordOpen = false;
            if (primaryLabel != null && owner != null) primaryLabel.text = owner.PrimaryLabel;
            ShowLens(0);   // Thermal Data is the default
        }

        private void OnDisable()
        {
            // Leaving the tab must leave the model as the other tabs expect it.
            if (modelLink != null)
            {
                modelLink.ClearLensTint();
                modelLink.ClearSelection();
            }
        }

        public void Populate(DPPData data)
        {
            _data = data;
            var u = data?.unit_use_phase;
            if (u == null)
            {
                Debug.LogWarning("[UsePhase] Payload has no unit_use_phase — builder-baked demo values stay.");
                return;
            }

            // ---- Thermal Data ----
            if (u.health != null)
            {
                if (sohValue != null) sohValue.text = $"{u.health.soh_pct} %";
                if (sohMechanism != null)
                    sohMechanism.text = $"limiting: {Pretty(u.health.soh_limiting_mechanism)}";
                if (reuseFraction != null)
                    reuseFraction.text = $"{u.health.reuse_fraction_by_mass:0.000} of mass carries a reuse verdict";

                foreach (var ind in u.health.indicators ?? new List<HealthIndicator>())
                {
                    if (ind.id == "flash_endurance") SetBar(flashBar, flashPct, ind.value_pct);
                    if (ind.id == "thermal_fatigue") SetBar(fatigueBar, fatiguePct, ind.value_pct);
                }

                var f = u.health.findings;
                for (int i = 0; i < 2; i++)
                {
                    bool has = f != null && i < f.Count;
                    SetText(findingLabels, i, has ? f[i].label : "—");
                    SetText(findingValues, i, has ? f[i].value : "");
                }
            }
            if (u.exposure != null)
            {
                if (damageSum != null)
                    damageSum.text = $"Σ damage {u.exposure.fatigue_consumed:0.000} → consumed " +
                                     $"{u.exposure.fatigue_consumed * 100f:0.0} % · remaining {u.exposure.fatigue_remaining_pct} %";
                var bands = u.exposure.delta_t_histogram;
                float maxDamage = 0.0001f;
                if (bands != null) foreach (var b in bands) maxDamage = Mathf.Max(maxDamage, b.damage);
                for (int i = 0; i < 5; i++)
                {
                    bool has = bands != null && i < bands.Count;
                    SetText(deltaBandLabels, i, has ? bands[i].band : "—");
                    SetText(deltaCycles, i, has ? $"{bands[i].cycles:n0}" : "");
                    SetText(deltaDamage, i, has ? $"{bands[i].damage:0.000}" : "");
                    if (deltaDamageBars != null && i < deltaDamageBars.Length && deltaDamageBars[i] != null && has)
                        deltaDamageBars[i].sizeDelta = new Vector2(
                            damageBarTrack * bands[i].damage / maxDamage, deltaDamageBars[i].sizeDelta.y);
                }
            }

            // ---- Electrical Data ----
            if (u.electrical != null)
            {
                if (elTransients != null) elTransients.text = $"{u.electrical.voltage_transients_logged:n0}  ({u.electrical.transient_standard})";
                if (elUndervoltage != null) elUndervoltage.text = $"{u.electrical.undervoltage_events}";
                if (elLoadDumps != null) elLoadDumps.text = $"{u.electrical.load_dump_events}";
            }
            if (u.compute != null)
            {
                if (daFlash != null) daFlash.text = $"{u.compute.flash_write_cycles_used:n0} / {u.compute.flash_write_cycle_limit:n0} · {u.compute.flash_endurance_remaining_pct} % left";
                if (daCpu != null) daCpu.text = $"{u.compute.cpu_hours_above_80pct} h";
                if (daEccResets != null) daEccResets.text = $"{u.compute.ecc_corrected_errors} · {u.compute.unexpected_resets}";
            }
            if (u.diagnostics != null)
            {
                if (daCan != null) daCan.text = $"{u.diagnostics.can_error_frames:n0} error frames · {u.diagnostics.bus_off_events} bus-off";
                if (daDtc != null) daDtc.text = $"{u.diagnostics.dtc_total} total · {u.diagnostics.dtc_active} active · {u.diagnostics.dtc_cleared} cleared";
            }

            // ---- Software ----
            if (u.calibration != null)
            {
                if (swFirmware != null) swFirmware.text = $"{u.calibration.firmware_versions_installed} versions · {u.calibration.firmware_first} → {u.calibration.firmware_last}";
                if (swMaps != null) swMaps.text = $"{u.calibration.calibration_map_changes}";
                if (swRecal != null) swRecal.text = $"{u.calibration.sensor_recalibrations}";
            }
            if (u.diagnostics != null && swLinkage != null)
                swLinkage.text = $"{u.diagnostics.dtc_linked_to_service_events} diagnostic codes tie to logged service " +
                                 "events — the unit raises codes for the whole vehicle, which is why the service log " +
                                 "carries vehicle systems as well as its own faults.";

            if (gameObject.activeInHierarchy) ShowLens(_lens);
        }

        // =================================================================
        // Lenses
        // =================================================================

        public void ShowLens0() => ShowLens(0);
        public void ShowLens1() => ShowLens(1);
        public void ShowLens2() => ShowLens(2);

        private void ShowLens(int index)
        {
            _lens = index;
            _recordOpen = false;
            if (partRecordRoot != null) partRecordRoot.SetActive(false);

            if (_lensHovers == null && lensFills != null)
            {
                _lensHovers = new HoverHighlight[lensFills.Length];
                for (int i = 0; i < lensFills.Length; i++)
                    if (lensFills[i] != null)
                        _lensHovers[i] = lensFills[i].GetComponentInParent<HoverHighlight>();
            }

            for (int i = 0; i < LensCount; i++)
            {
                bool on = i == index;
                if (lensRoots != null && i < lensRoots.Length && lensRoots[i] != null)
                    lensRoots[i].SetActive(on);
                Color fill = on ? DPPTheme.Hex("#16305c") : DPPTheme.Hex("#0E2950");
                if (lensFills != null && i < lensFills.Length && lensFills[i] != null)
                    lensFills[i].color = fill;
                if (_lensHovers != null && i < _lensHovers.Length && _lensHovers[i] != null)
                    _lensHovers[i].SetRestFillColor(fill);   // trap 1 — the write that persists
                if (lensStrokes != null && i < lensStrokes.Length && lensStrokes[i] != null)
                    lensStrokes[i].color = on ? DPPTheme.TealAccent : DPPTheme.Hex("#21407a");
                if (lensLabels != null && i < lensLabels.Length && lensLabels[i] != null)
                    lensLabels[i].color = on ? Color.white : DPPTheme.TextSecondary;
            }

            ApplyLensToModel(index);
        }

        /// <summary>Pills re-tint the same model — they never open a new screen.
        /// Software is the only non-spatial lens: tint cleared (spec §5).</summary>
        private void ApplyLensToModel(int lens)
        {
            if (modelLink == null) return;
            modelLink.ClearSelection();

            switch (lens)
            {
                case 0:   // Thermal Data — verdict colours (decision 1; the heat ramp died)
                    var tints = new Dictionary<string, Color>();
                    var verdicts = _data?.unit_use_phase?.health?.reuse_assessment;
                    if (verdicts != null)
                        foreach (var v in verdicts) tints[v.component_id] = VerdictColor(v.verdict);
                    modelLink.SetLensTint(tints, Neutral);
                    break;
                case 1:   // Electrical Data — connectors + flash IC + bus interface lit
                    modelLink.SetLensTint(new Dictionary<string, Color>
                    {
                        ["connectors"] = LitBlue, ["ic_1"] = LitBlue, ["ic_4"] = LitBlue,
                    }, Neutral);
                    break;
                default:  // Software — non-spatial
                    modelLink.ClearLensTint();
                    break;
            }
        }

        public static Color VerdictColor(string verdict)
        {
            switch (verdict)
            {
                case "reuse": return VerdictReuse;
                case "reuse_after_test": return VerdictAfterTest;
                case "consumable": return VerdictConsumable;
                default: return VerdictRecovery;
            }
        }

        // =================================================================
        // Part record — the pinch target on THIS tab (Thiago, 2026-08-09)
        // =================================================================

        /// <summary>Called by ModelLinkController while the Usage tab is active.
        /// Pinching another (ghosted) body switches the record directly.</summary>
        public bool OpenComponentById(string componentId)
        {
            var verdicts = _data?.unit_use_phase?.health?.reuse_assessment;
            if (verdicts == null) return false;
            ReuseVerdict hit = null;
            foreach (var v in verdicts)
                if (v.component_id == componentId) { hit = v; break; }
            if (hit == null) return false;

            _recordOpen = true;
            if (lensRoots != null)
                foreach (var root in lensRoots)
                    if (root != null) root.SetActive(false);
            if (partRecordRoot != null) partRecordRoot.SetActive(true);

            if (partName != null) partName.text = hit.name;
            if (partVerdict != null) partVerdict.text = Pretty(hit.verdict);
            if (partVerdictChip != null) partVerdictChip.color = VerdictColor(hit.verdict);
            if (partMass != null) partMass.text = $"{hit.mass_g:0.0} g";
            if (partReason != null) partReason.text = hit.reason ?? "";

            if (modelLink != null) modelLink.SelectComponent(componentId);
            Debug.Log($"[UsePhase] Part record → '{componentId}'.");
            return true;
        }

        /// <summary>The record's back — return to the active lens.</summary>
        public void CloseRecord() => ShowLens(_lens);

        // ---- bottom bar (04 page grammar) ----
        public void OnBack()
        {
            if (_recordOpen) { CloseRecord(); return; }
            if (owner != null) owner.PrevTab();
        }
        public void OnPrimary() { if (owner != null) owner.NextTab(); }

        // =================================================================

        private static void SetText(TMP_Text[] arr, int i, string value)
        {
            if (arr != null && i < arr.Length && arr[i] != null) arr[i].text = value;
        }

        private void SetBar(RectTransform bar, TMP_Text pct, int valuePct)
        {
            if (bar != null) bar.sizeDelta = new Vector2(barTrack * valuePct / 100f, bar.sizeDelta.y);
            if (pct != null) pct.text = $"{valuePct} %";
        }

        private static string Pretty(string s) => string.IsNullOrEmpty(s) ? "—" : s.Replace('_', ' ');
    }
}
