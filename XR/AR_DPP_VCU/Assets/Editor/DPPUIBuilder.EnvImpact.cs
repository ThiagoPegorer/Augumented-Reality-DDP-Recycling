using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DPP;
using DPP.UI;

namespace DPP.EditorTools
{
    /// <summary>
    /// RBv2_1_1/13 — 07: ENVIRONMENTAL IMPACT into the data canvas (spec
    /// `RB2_1_1/07_environmental_impact.md` v1, mock 04d_v2 approved 2026-08-08).
    ///
    /// Four sub-tabs — LCA explorer (default) · Main impacts · Per stage ·
    /// Recycling — as pills at the very TOP of the page (no title, no subtitle).
    /// NO model tint on this tab (approved decision).
    ///
    /// Baked content policy (spec §6): pareto + recycling bake the REAL EF 3.1
    /// numbers (impact_screening.csv / impact_EF31.csv via the payload), so an
    /// offline build tells the true story. Per stage bakes the PENDING state —
    /// no per-stage openLCA export exists until stage_contributions.py runs, and
    /// placeholders must render as placeholders, never as invented numbers.
    ///
    /// Safe to re-run: rebuilds EnvironmentalPage only, then MERGES it into
    /// tabPages (trap 4: an overwrite orphans sibling pages into permanently-
    /// active ghosts with invisible hit areas).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const float EnvMargin = 24f, EnvW = 372f;   // content column on the 420-wide canvas

        [MenuItem("RBv2_1_1/13 — Environmental impact into the data canvas", false, 13)]
        public static void Build_EnvImpactIntoRig()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var rig = SpFind("DppSuperPanel");
            if (rig == null) { Debug.LogError("[DPPUIBuilder] No DppSuperPanel — run RBv2_1_1/10 first."); return; }
            var data = rig.transform.Find("DataCanvas") as RectTransform;
            if (data == null) { Debug.LogError("[DPPUIBuilder] DppSuperPanel has no DataCanvas — re-run RBv2_1_1/10."); return; }
            var view = rig.GetComponent<SuperPanelView>();

            var old = data.Find("EnvironmentalPage");
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

            var page = Stretch("EnvironmentalPage", data);
            Undo.RegisterCreatedObjectUndo(page.gameObject, "Build Environmental impact page");
            AddImage(Stretch("PageBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);
            var env = page.gameObject.AddComponent<EnvImpactView>();

            // ---------------- sub-tab pills — the top of the page ----------------
            string[] tabNames = { "LCA explorer", "Main impacts", "Per stage", "Recycling" };
            var tabFills = new Image[EnvImpactView.TabCount];
            var tabStrokes = new Image[EnvImpactView.TabCount];
            var tabLabels = new TMP_Text[EnvImpactView.TabCount];
            for (int i = 0; i < EnvImpactView.TabCount; i++)
            {
                var root = TL($"Tab{i}", page, EnvMargin + i * 93f, 20f, 88f, 32f);
                // Full elevation kit (00 §4.1) — shadow + gloss + rise, from day one
                // this time (04a shipped its pills flat and was called on it).
                AddShadow(root, 88f, 32f, DPPSpriteFactory.RoundedR13);
                var outline = AddImage(CenterIn("HoverOutline", root, 88f + HoverHalo, 32f + HoverHalo),
                    DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
                outline.gameObject.SetActive(false);
                tabStrokes[i] = AddImage(CenterIn("Stroke", root, 88f, 32f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#21407a"), sliced: true);
                tabFills[i] = AddImage(CenterIn("Fill", root, 86f, 30f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#0E2950"), sliced: true, raycast: true);
                AddGloss(root, 88f, 32f, DPPSpriteFactory.RoundedR13);
                tabLabels[i] = AddText(Stretch("Label", root), tabNames[i], 9f,
                    DPPTheme.TextSecondary, bold: i == 0, align: TextAlignmentOptions.Center);

                var btn = root.gameObject.AddComponent<Button>();
                btn.transition = Selectable.Transition.None;
                btn.targetGraphic = tabFills[i];
                WireClick(btn, env, "ShowTab" + i);
                var hover = root.gameObject.AddComponent<HoverHighlight>();
                SetRef(hover, "highlightOutline", outline.gameObject);
            }

            // ---------------- tab roots ----------------
            var explorer = Stretch("Explorer", page);
            var impacts = Stretch("Impacts", page);
            var stages = Stretch("Stages", page);
            var recycling = Stretch("Recycling", page);

            // ================= TAB 0 — LCA EXPLORER (default) =================
            // Round 3 (2026-08-08): the pinwheel is DEAD ("the arrows are not
            // pointing to nothing" — Thiago). Plain list: one row per stage, name
            // + description, payload-driven. ic_lca_arrow.png is retired with it
            // (delete in the next asset cleanup pass).
            string[] pillText =
            {
                "Stage 1: Materials & construction", "Stage 2: Hardware assembly",
                "Stage 3: Distribution", "Stage 4: Use phase", "Stage 5: End-of-life",
            };
            string[] cardText =
            {
                "Raw-material extraction and refinement; manufacture of the electronic components.",
                "Manufacturing at the provider: die-casting & SMT placement.",
                "Road freight of the finished unit to the vehicle OEM.",
                "Operation in a battery-electric vehicle in Germany.",
                "Recycling stage — collection and treatment. See the Recycling tab.",
            };
            var cardTitles = new TMP_Text[EnvImpactView.StageCount];
            var cardBodies = new TMP_Text[EnvImpactView.StageCount];
            for (int i = 0; i < EnvImpactView.StageCount; i++)
            {
                float y = 62f + i * 65f;
                AddImage(TL($"StageRow{i}", explorer, EnvMargin, y, EnvW, 58f),
                    DPPSpriteFactory.RoundedR13, DPPTheme.Hex("#0E2950"), sliced: true);
                cardTitles[i] = AddText(TL($"RowTitle{i}", explorer, EnvMargin + 12f, y + 8f, EnvW - 24f, 13f),
                    pillText[i], 8.5f, DPPTheme.TextOnNavy, bold: true);
                cardBodies[i] = AddText(TL($"RowBody{i}", explorer, EnvMargin + 12f, y + 24f, EnvW - 24f, 28f),
                    cardText[i], 7.5f, DPPTheme.TextSecondary, bold: false,
                    align: TextAlignmentOptions.TopLeft);
                cardBodies[i].textWrappingMode = TextWrappingModes.Normal;
            }

            // ================= TAB 1 — MAIN IMPACTS =================
            // Round 2: all text below the chart removed; the four rows are
            // redistributed over the freed space (thicker bars, larger labels).
            AddText(TL("ParetoCap", impacts, EnvMargin, 66f, EnvW, 12f),
                "SHARE OF THE WEIGHTED FOOTPRINT — EF 3.1 SCREENING · Sc1", 7.5f,
                DPPTheme.Hex("#5d7396"), bold: true);

            // Baked = impact_screening.csv (2026-07-27): 72.5 / 6.7 / 6.6 / 14.3.
            string[] pLabels = { "Minerals & metals", "Climate change", "Eutrophication FW", "All others (13)" };
            float[] pShare = { 72.5f, 6.7f, 6.6f, 14.3f };
            Color[] pColor = { DPPTheme.TealAccent, DPPTheme.Hex("#f0c879"),
                               DPPTheme.Hex("#4da3ff"), DPPTheme.Hex("#21407a") };
            var paretoLabels = new TMP_Text[4]; var paretoPcts = new TMP_Text[4];
            var paretoBars = new RectTransform[4];
            for (int i = 0; i < 4; i++)
            {
                float y = 104f + i * 70f;
                paretoLabels[i] = AddText(TL($"PLbl{i}", impacts, EnvMargin, y, 118f, 16f),
                    pLabels[i], 9f, i < 3 ? DPPTheme.TextOnNavy : DPPTheme.TextSecondary, bold: false);
                AddImage(TL($"PTrack{i}", impacts, 148f, y + 3f, 160f, 12f),
                    DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#0a1a38"), sliced: true);
                var bar = TL($"PBar{i}", impacts, 148f, y + 3f, 160f * pShare[i] / 72.5f, 12f);
                AddImage(bar, DPPSpriteFactory.RoundedR3, pColor[i], sliced: true);
                paretoBars[i] = bar;
                paretoPcts[i] = AddText(TL($"PPct{i}", impacts, 312f, y, 84f, 16f),
                    $"{pShare[i]:0.0} %", 9f, DPPTheme.TextOnNavy, bold: false,
                    align: TextAlignmentOptions.MidlineRight);
            }
            impacts.gameObject.SetActive(false);

            // ================= TAB 2 — PER STAGE (grouped BY IMPACT, approved) =================
            // Bakes the PENDING state: bars exist but are INACTIVE, the watermark is on.
            // EnvImpactView.PopulatePerStage flips them when the payload carries values.
            string[] panelTitles = { "MINERALS & METALS — kg Sb eq", "CLIMATE CHANGE — kg CO2 eq",
                                     "EUTROPHICATION FW — kg P eq" };
            string[] stageRowNames = { "S1 Materials", "S2 Assembly", "S3 Distribution", "S4 Use phase" };
            var spTitles = new TMP_Text[EnvImpactView.CategoryCount];
            var spPending = new GameObject[EnvImpactView.CategoryCount];
            var spBars = new RectTransform[EnvImpactView.CategoryCount * 4];
            var spValues = new TMP_Text[EnvImpactView.CategoryCount * 4];
            // Round 2: bottom margin matched to the top margin — panels 101 high at
            // 107 pitch end at y 377, mirroring the ~10-unit gap under the pills.
            for (int c = 0; c < EnvImpactView.CategoryCount; c++)
            {
                float y0 = 62f + c * 107f;
                AddImage(TL($"Panel{c}", stages, EnvMargin, y0, EnvW, 101f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#0E2950"), sliced: true);
                spTitles[c] = AddText(TL($"PanelTitle{c}", stages, EnvMargin + 12f, y0 + 8f, EnvW - 24f, 12f),
                    panelTitles[c], 8f, DPPTheme.TealAccent, bold: true);
                for (int s = 0; s < 4; s++)
                {
                    float ry = y0 + 26f + s * 19f;
                    int i = c * 4 + s;
                    AddText(TL($"StageLbl{c}_{s}", stages, EnvMargin + 12f, ry, 92f, 12f),
                        stageRowNames[s], 8f, DPPTheme.TextOnNavy, bold: false);
                    AddImage(TL($"StageTrack{c}_{s}", stages, 130f, ry + 2f, 150f, 8f),
                        DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#0a1a38"), sliced: true);
                    var bar = TL($"StageBar{c}_{s}", stages, 130f, ry + 2f, 10f, 8f);
                    AddImage(bar, DPPSpriteFactory.RoundedR3, pColor[c], sliced: true);
                    bar.gameObject.SetActive(false);          // pending until the payload has values
                    spBars[i] = bar;
                    spValues[i] = AddText(TL($"StageVal{c}_{s}", stages, 284f, ry, 100f, 12f),
                        "", 8f, DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
                    spValues[i].gameObject.SetActive(false);
                }
                var pend = AddText(TL($"Pending{c}", stages, 130f, y0 + 50f, 254f, 14f),
                    "[pending openLCA — run stage_contributions.py]", 7.5f,
                    DPPTheme.Hex("#5d7396"), bold: false, align: TextAlignmentOptions.Center);
                spPending[c] = pend.gameObject;
            }
            stages.gameObject.SetActive(false);

            // ================= TAB 3 — RECYCLING =================
            // Round 2: short scenario names (Thiago's wording, 2026-08-08), charts
            // kept, real margin above the bottom bar (chart ends y 362, bar ~388).
            // Sc4 keeps the exploratory [A] flag — framework rule: Sc4 is never
            // presented without it.
            (string title, string body)[] scText =
            {
                ("Scenario 1", "Landfill & incineration"),
                ("Scenario 2", "Bulk shredding and mechanical sorting"),
                ("Scenario 3", "Manual disassembly and shredding"),
                ("Scenario 4", "Manual disassembly and functional reuse of electronic components · exploratory [A]"),
            };
            var scTitles = new TMP_Text[4]; var scBodies = new TMP_Text[4];
            for (int i = 0; i < 4; i++)
            {
                float x = EnvMargin + (i % 2) * 192f, y = 62f + (i / 2) * 50f;
                AddImage(TL($"ScCard{i}", recycling, x, y, 180f, 44f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#0E2950"), sliced: true);
                scTitles[i] = AddText(TL($"ScTitle{i}", recycling, x + 10f, y + 5f, 160f, 11f),
                    scText[i].title, 7.5f, DPPTheme.TealAccent, bold: true);
                scBodies[i] = AddText(TL($"ScBody{i}", recycling, x + 10f, y + 17f, 160f, 24f),
                    scText[i].body, 6.6f, DPPTheme.TextOnNavy, bold: false,
                    align: TextAlignmentOptions.TopLeft);
                scBodies[i].textWrappingMode = TextWrappingModes.Normal;
            }

            AddText(TL("RedCap", recycling, EnvMargin, 166f, EnvW, 12f),
                "NET REDUCTION VS Sc1 — EF 3.1", 7.5f, DPPTheme.Hex("#5d7396"), bold: true);

            // Baked = impact_EF31.csv via the payload's impact_recovery (v0.18 values).
            string[] gTitles = { "MINERALS & METALS — Sc1 0.0187 kg Sb eq",
                                 "CLIMATE CHANGE — Sc1 73.4 kg CO2 eq",
                                 "EUTROPHICATION FW — Sc1 0.116 kg P eq" };
            float[,] gPct = { { 10.0f, 32.2f, 47.3f }, { 6.0f, 11.1f, 21.0f }, { 5.9f, 16.5f, 25.7f } };
            Color[] scColor = { DPPTheme.Hex("#6f86a8"), DPPTheme.TealAccent, DPPTheme.Hex("#f0c879") };
            var redGroupTitles = new TMP_Text[EnvImpactView.CategoryCount];
            var redBars = new RectTransform[EnvImpactView.CategoryCount * 3];
            var redPcts = new TMP_Text[EnvImpactView.CategoryCount * 3];
            for (int c = 0; c < EnvImpactView.CategoryCount; c++)
            {
                float gy = 184f + c * 62f;
                redGroupTitles[c] = AddText(TL($"Grp{c}", recycling, EnvMargin, gy, EnvW, 11f),
                    gTitles[c], 7.5f, DPPTheme.TealAccent, bold: true);
                for (int s = 0; s < 3; s++)
                {
                    float ry = gy + 13f + s * 15f;
                    int i = c * 3 + s;
                    AddText(TL($"GrpLbl{c}_{s}", recycling, EnvMargin, ry, 30f, 11f),
                        $"Sc{s + 2}", 8f, DPPTheme.TextSecondary, bold: false);
                    AddImage(TL($"RedTrack{c}_{s}", recycling, 58f, ry + 2f, 210f, 8f),
                        DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#0a1a38"), sliced: true);
                    var bar = TL($"RedBar{c}_{s}", recycling, 58f, ry + 2f,
                        210f * Mathf.Clamp01(gPct[c, s] / 50f), 8f);
                    AddImage(bar, DPPSpriteFactory.RoundedR3, scColor[s], sliced: true);
                    redBars[i] = bar;
                    redPcts[i] = AddText(TL($"RedPct{c}_{s}", recycling, 272f, ry, 76f, 11f),
                        $"−{gPct[c, s]:0.0} %", 8.5f, DPPTheme.TextOnNavy, bold: false);
                }
            }
            recycling.gameObject.SetActive(false);

            // ================= bottom bar =================
            var backBtn = PsSmallPill(page, "BackButton", EnvMargin + 45f, 90f, "Back",
                primary: false, out _, cy: 402f, fontSize: 11f);
            WireClick(backBtn, env, nameof(EnvImpactView.OnBack));
            var primaryBtn = PsSmallPill(page, "PrimaryButton", 420f - EnvMargin - 75f, 150f, "Next",
                primary: true, out var primaryLbl, cy: 402f, fontSize: 11f);
            WireClick(primaryBtn, env, nameof(EnvImpactView.OnPrimary));

            // ================= wiring =================
            SetRef(env, "owner", view);
            SetRefArray(env, "tabFills", tabFills);
            SetRefArray(env, "tabStrokes", tabStrokes);
            SetRefArray(env, "tabLabels", tabLabels);
            SetRefArray(env, "tabRoots", new GameObject[]
                { explorer.gameObject, impacts.gameObject, stages.gameObject, recycling.gameObject });

            SetRefArray(env, "stageCardTitles", cardTitles);
            SetRefArray(env, "stageCardBodies", cardBodies);

            SetRefArray(env, "paretoLabels", paretoLabels);
            SetRefArray(env, "paretoBars", paretoBars);
            SetRefArray(env, "paretoPcts", paretoPcts);
            // paretoCumLine intentionally unset — round 2 removed all text below
            // the chart; the view field is null-safe.
            SetFloat(env, "paretoTrack", 160f);

            SetRefArray(env, "stagePanelTitles", spTitles);
            SetRefArray(env, "stageBars", spBars);
            SetRefArray(env, "stageValues", spValues);
            SetRefArray(env, "stagePending", spPending);
            SetFloat(env, "stageBarTrack", 150f);

            SetRefArray(env, "scenarioTitles", scTitles);
            SetRefArray(env, "scenarioBodies", scBodies);
            SetRefArray(env, "reductionBars", redBars);
            SetRefArray(env, "reductionPcts", redPcts);
            SetRefArray(env, "reductionGroupTitles", redGroupTitles);
            SetFloat(env, "reductionTrack", 210f);
            SetFloat(env, "reductionScaleMaxPct", 50f);

            SetRef(env, "primaryLabel", primaryLbl);

            // ---- tabPages MERGE (trap 4 — never overwrite) ----
            if (view != null)
            {
                var pages = new GameObject[SuperPanelView.TabCount];
                var specs = SpFind("ProductSpecsPage");
                if (specs != null) pages[0] = specs;
                var usage = SpFind("UsagePage");
                if (usage != null) pages[1] = usage;
                pages[2] = page.gameObject;
                // data.Find, NOT SpFind — RBv2_1_1/08's legacy flat-canvas screen
                // shares the "CertificatesPage" name (device bug, 2026-08-08).
                var certs = data.Find("CertificatesPage");
                if (certs != null) pages[3] = certs.gameObject;
                SetRefArray(view, "tabPages", pages);
            }

            page.gameObject.SetActive(false);   // SuperPanelView shows it when tab 2 opens

            Selection.activeGameObject = page.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/13 — Environmental impact built into the data canvas. " +
                      "Run RBv2_1_1/Tools/Verify wiring, then SAVE THE SCENE.");
        }
    }
}
