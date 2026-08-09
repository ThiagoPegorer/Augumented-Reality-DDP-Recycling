using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 — 07: ENVIRONMENTAL IMPACT (spec `RB2_1_1/07_environmental_impact.md`,
    /// mock 04d_v2 approved 2026-08-08). Four sub-tabs: LCA explorer (default) ·
    /// Main impacts · Per stage · Recycling.
    ///
    /// NO PAGE TITLE (general rule): the rail already names the tab.
    /// NO MODEL TINT on this tab (approved decision) — the stage model stays neutral.
    ///
    /// Data contract (spec §2): everything renders from the payload's
    /// `environmental` block — impact_recovery (pareto shares + recycling) and
    /// lifecycle_stages v0.19 (explorer cards + per-stage values). A StageImpact
    /// with value == null renders "[pending openLCA]", NEVER a number: the
    /// per-stage openLCA run is Thiago's stage_contributions.py, not this client.
    /// </summary>
    public class EnvImpactView : MonoBehaviour
    {
        public const int TabCount = 4;      // LCA explorer · Main impacts · Per stage · Recycling
        public const int StageCount = 5;    // S1..S5 (explorer); Per stage charts S1..S4 only
        public const int CategoryCount = 3; // minerals · climate · eutrophication (freshwater)

        [Header("Wiring (RBv2_1_1/4)")]
        [SerializeField] private SuperPanelView owner;

        [Header("Sub-tab pills")]
        [SerializeField] private Image[] tabFills;
        [SerializeField] private Image[] tabStrokes;
        [SerializeField] private TMP_Text[] tabLabels;
        [SerializeField] private GameObject[] tabRoots;   // index-aligned with the pills

        [Header("LCA explorer")]
        [SerializeField] private TMP_Text[] stageCardTitles;   // 5 — "S1 · Materials & construction"
        [SerializeField] private TMP_Text[] stageCardBodies;   // 5

        [Header("Main impacts (pareto)")]
        [SerializeField] private TMP_Text[] paretoLabels;      // 4 — 3 categories + "All others"
        [SerializeField] private RectTransform[] paretoBars;
        [SerializeField] private TMP_Text[] paretoPcts;
        [SerializeField] private TMP_Text paretoCumLine;
        [SerializeField] private float paretoTrack = 160f;

        [Header("Per stage — one panel per impact, own unit & scale (approved)")]
        [SerializeField] private TMP_Text[] stagePanelTitles;  // 3
        [SerializeField] private RectTransform[] stageBars;    // 12 = panel*4 + stageIndex(S1..S4)
        [SerializeField] private TMP_Text[] stageValues;       // 12
        [SerializeField] private GameObject[] stagePending;    // 3 — "[pending openLCA]" watermark
        [SerializeField] private float stageBarTrack = 150f;

        [Header("Recycling")]
        [SerializeField] private TMP_Text[] scenarioTitles;    // 4 — Sc1..Sc4 cards
        [SerializeField] private TMP_Text[] scenarioBodies;    // 4
        [SerializeField] private RectTransform[] reductionBars; // 9 = catIndex*3 + (Sc2,Sc3,Sc4)
        [SerializeField] private TMP_Text[] reductionPcts;      // 9
        [SerializeField] private TMP_Text[] reductionGroupTitles; // 3
        [SerializeField] private float reductionTrack = 210f;
        [SerializeField] private float reductionScaleMaxPct = 50f;  // 47.3 % ~ full track

        [Header("Bottom bar")]
        [SerializeField] private TMP_Text primaryLabel;

        /// <summary>EF 3.1 verbatim names, in screen order. Verbatim match against the
        /// payload — a renamed category must fail loudly, not silently mis-map.</summary>
        private static readonly string[] Categories =
        {
            "Resource use minerals and metals",
            "Climate change",
            "Eutrophication freshwater",
        };
        private static readonly string[] CategoryShort = { "Minerals & metals", "Climate change", "Eutrophication FW" };

        private DPPData _data;

        // Trap 1: the pill fills are state-coloured AND hover-brightened, so state
        // colours must go through HoverHighlight.SetRestFillColor — a direct write
        // survives only until the next hover ease repaints the captured colour.
        private HoverHighlight[] _tabHovers;

        private void OnEnable()
        {
            if (_data == null)
            {
                var mgr = FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
                if (mgr != null && mgr.Latest != null) Populate(mgr.Latest);
            }
            if (primaryLabel != null && owner != null) primaryLabel.text = owner.PrimaryLabel;
            ShowTab(0);   // LCA explorer is the default
        }

        public void Populate(DPPData data)
        {
            _data = data;
            var env = data?.environmental;
            if (env == null)
            {
                Debug.LogWarning("[EnvImpact] Payload has no environmental block — builder-baked values stay.");
                return;
            }

            PopulateExplorer(env);
            PopulatePareto(env);
            PopulatePerStage(env);
            PopulateRecycling(env);
        }

        // =================================================================
        // Tab 0 — LCA explorer
        // =================================================================

        private void PopulateExplorer(Environmental env)
        {
            var stages = env.lifecycle_stages;
            if (stages == null || stages.Count == 0) return;   // baked cards stay
            for (int i = 0; i < StageCount; i++)
            {
                bool has = i < stages.Count && stages[i] != null;
                // Round 2: titles live on the pinwheel STAGE PILLS ("Stage 1: …"),
                // descriptions on the grey cards beside them.
                if (stageCardTitles != null && i < stageCardTitles.Length && stageCardTitles[i] != null && has)
                    stageCardTitles[i].text = $"Stage {i + 1}: {stages[i].name}";
                if (stageCardBodies != null && i < stageCardBodies.Length && stageCardBodies[i] != null && has)
                    stageCardBodies[i].text = stages[i].description ?? "";
            }
        }

        // =================================================================
        // Tab 1 — Main impacts (screening pareto)
        // =================================================================

        private void PopulatePareto(Environmental env)
        {
            if (env.impact_recovery == null || env.impact_recovery.Count == 0) return;

            float sum = 0f;
            var shares = new float[CategoryCount];
            for (int c = 0; c < CategoryCount; c++)
            {
                var rec = FindCategory(env, Categories[c]);
                shares[c] = rec?.screening_share_pct ?? 0f;
                sum += shares[c];
            }
            if (sum <= 0f) return;   // payload without screening shares — baked values stay
            float others = Mathf.Max(0f, 100f - sum);

            float maxShare = Mathf.Max(shares[0], 0.0001f);   // minerals dominates by design
            for (int i = 0; i < 4; i++)
            {
                float v = i < CategoryCount ? shares[i] : others;
                if (paretoLabels != null && i < paretoLabels.Length && paretoLabels[i] != null)
                    paretoLabels[i].text = i < CategoryCount ? CategoryShort[i] : "All others (13)";
                if (paretoPcts != null && i < paretoPcts.Length && paretoPcts[i] != null)
                    paretoPcts[i].text = $"{v:0.0} %";
                if (paretoBars != null && i < paretoBars.Length && paretoBars[i] != null)
                    paretoBars[i].sizeDelta = new Vector2(paretoTrack * v / maxShare, paretoBars[i].sizeDelta.y);
            }
            if (paretoCumLine != null)
                paretoCumLine.text = $"cumulative  {shares[0]:0.0} → {shares[0] + shares[1]:0.0} → " +
                                     $"{sum:0.0} → 100 %";
        }

        // =================================================================
        // Tab 2 — Per stage, grouped BY IMPACT (approved correction)
        // =================================================================

        private void PopulatePerStage(Environmental env)
        {
            var stages = env.lifecycle_stages;
            for (int c = 0; c < CategoryCount; c++)
            {
                // gather S1..S4 for this category; ANY missing value = whole panel pending
                var vals = new float[4];
                string unit = null;
                bool complete = stages != null && stages.Count >= 4;
                for (int s = 0; complete && s < 4; s++)
                {
                    var imp = FindImpact(stages[s], Categories[c]);
                    if (imp == null || imp.value == null || imp.basis == DppBasis.NotProvided)
                        complete = false;
                    else { vals[s] = imp.value.Value; unit = imp.unit; }
                }

                if (stagePanelTitles != null && c < stagePanelTitles.Length && stagePanelTitles[c] != null)
                    stagePanelTitles[c].text = unit != null
                        ? $"{CategoryShort[c].ToUpperInvariant()} — {unit}"
                        : CategoryShort[c].ToUpperInvariant();

                if (stagePending != null && c < stagePending.Length && stagePending[c] != null)
                    stagePending[c].SetActive(!complete);

                float max = 0.0001f;
                if (complete) for (int s = 0; s < 4; s++) max = Mathf.Max(max, Mathf.Abs(vals[s]));
                for (int s = 0; s < 4; s++)
                {
                    int i = c * 4 + s;
                    bool show = complete;
                    if (stageBars != null && i < stageBars.Length && stageBars[i] != null)
                    {
                        stageBars[i].gameObject.SetActive(show);
                        if (show) stageBars[i].sizeDelta = new Vector2(
                            stageBarTrack * Mathf.Abs(vals[s]) / max, stageBars[i].sizeDelta.y);
                    }
                    if (stageValues != null && i < stageValues.Length && stageValues[i] != null)
                    {
                        stageValues[i].gameObject.SetActive(show);
                        if (show) stageValues[i].text = FormatValue(vals[s]);
                    }
                }
            }
        }

        // =================================================================
        // Tab 3 — Recycling
        // =================================================================

        private void PopulateRecycling(Environmental env)
        {
            if (env.impact_recovery == null) return;
            for (int c = 0; c < CategoryCount; c++)
            {
                var rec = FindCategory(env, Categories[c]);
                if (reductionGroupTitles != null && c < reductionGroupTitles.Length && reductionGroupTitles[c] != null)
                    reductionGroupTitles[c].text = rec != null
                        ? $"{CategoryShort[c].ToUpperInvariant()} — Sc1 {FormatValue(rec.baseline)} {rec.unit}"
                        : CategoryShort[c].ToUpperInvariant();
                for (int s = 0; s < 3; s++)   // Sc2, Sc3, Sc4
                {
                    int i = c * 3 + s;
                    var sc = FindScenario(rec, $"Sc{s + 2}");
                    float pct = sc?.reduction_pct ?? 0f;
                    if (reductionPcts != null && i < reductionPcts.Length && reductionPcts[i] != null)
                        reductionPcts[i].text = sc != null ? $"−{pct:0.0} %" : "—";
                    if (reductionBars != null && i < reductionBars.Length && reductionBars[i] != null)
                        reductionBars[i].sizeDelta = new Vector2(
                            reductionTrack * Mathf.Clamp01(pct / reductionScaleMaxPct),
                            reductionBars[i].sizeDelta.y);
                }
            }
        }

        // =================================================================
        // Sub-tabs
        // =================================================================

        public void ShowTab0() => ShowTab(0);
        public void ShowTab1() => ShowTab(1);
        public void ShowTab2() => ShowTab(2);
        public void ShowTab3() => ShowTab(3);

        private void ShowTab(int index)
        {
            if (_tabHovers == null && tabFills != null)
            {
                _tabHovers = new HoverHighlight[tabFills.Length];
                for (int i = 0; i < tabFills.Length; i++)
                    if (tabFills[i] != null)
                        _tabHovers[i] = tabFills[i].GetComponentInParent<HoverHighlight>();
            }

            for (int i = 0; i < TabCount; i++)
            {
                bool on = i == index;
                if (tabRoots != null && i < tabRoots.Length && tabRoots[i] != null)
                    tabRoots[i].SetActive(on);
                Color fill = on ? DPPTheme.Hex("#16305c") : DPPTheme.Hex("#0E2950");
                if (tabFills != null && i < tabFills.Length && tabFills[i] != null)
                    tabFills[i].color = fill;
                if (_tabHovers != null && i < _tabHovers.Length && _tabHovers[i] != null)
                    _tabHovers[i].SetRestFillColor(fill);   // trap 1 — the write that persists
                if (tabStrokes != null && i < tabStrokes.Length && tabStrokes[i] != null)
                    tabStrokes[i].color = on ? DPPTheme.TealAccent : DPPTheme.Hex("#21407a");
                if (tabLabels != null && i < tabLabels.Length && tabLabels[i] != null)
                    tabLabels[i].color = on ? Color.white : DPPTheme.TextSecondary;
            }
        }

        // ---- bottom bar (04 page grammar) ----
        public void OnBack() { if (owner != null) owner.PrevTab(); }
        public void OnPrimary() { if (owner != null) owner.NextTab(); }

        // =================================================================

        private static ImpactRecovery FindCategory(Environmental env, string category)
        {
            if (env?.impact_recovery == null) return null;
            foreach (var r in env.impact_recovery)
                if (r != null && r.category == category) return r;
            return null;
        }

        private static ImpactRecoveryScenario FindScenario(ImpactRecovery rec, string id)
        {
            if (rec?.scenarios == null) return null;
            foreach (var s in rec.scenarios)
                if (s != null && s.id == id) return s;
            return null;
        }

        private static StageImpact FindImpact(LifecycleStage stage, string category)
        {
            if (stage?.impacts == null) return null;
            foreach (var i in stage.impacts)
                if (i != null && i.category == category) return i;
            return null;
        }

        /// <summary>Three orders of magnitude apart across the panels (73.4 kg CO2 vs
        /// 0.0187 kg Sb), so the format adapts instead of fixing decimals.</summary>
        public static string FormatValue(float v)
        {
            float a = Mathf.Abs(v);
            if (a >= 10f) return v.ToString("0.0");
            if (a >= 1f) return v.ToString("0.00");
            if (a >= 0.001f) return v.ToString("0.0000");
            if (a <= 0f) return "0";
            return v.ToString("0.0e+0");
        }
    }
}
