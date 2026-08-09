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
    /// RBv2_1_1/3 — 06: USAGE &amp; SERVICE into the data canvas (spec
    /// `RB2_1_1/06_usage_service.md` v2, mock 04a_v6 approved 2026-08-09).
    ///
    /// Three lenses — Thermal Data (default) · Electrical Data · Software — as
    /// pills at the very TOP of the page: no title, no subtitle (general rule;
    /// the rail already names the tab). Lens pills re-tint the STAGE model via
    /// the model link; a pinch on the stage while this tab is active opens the
    /// part's USAGE RECORD (per-tab routing, wired here into the link).
    ///
    /// Demo content baked at build time; UsePhaseView.Populate overwrites from
    /// the payload's `unit_use_phase` block. Every baked figure is the real
    /// v0.18 value so an offline build still tells the true story.
    ///
    /// Safe to re-run: rebuilds UsagePage only, then MERGES it into tabPages
    /// (trap 4: an overwrite orphans sibling pages into permanently-active
    /// ghosts with invisible hit areas).
    /// </summary>
    public static partial class DPPUIBuilder
    {
        private const float UpMargin = 24f, UpW = 372f;   // content column on the 420-wide canvas

        [MenuItem("RBv2_1_1/3 — Usage & service into the data canvas", false, 3)]
        public static void Build_UsePhaseIntoRig()
        {
            DPPSpriteFactory.GenerateAll();
            ResolveFonts();

            var rig = SpFind("DppSuperPanel");
            if (rig == null) { Debug.LogError("[DPPUIBuilder] No DppSuperPanel — run RBv2_1_1/1 first."); return; }
            var data = rig.transform.Find("DataCanvas") as RectTransform;
            if (data == null) { Debug.LogError("[DPPUIBuilder] DppSuperPanel has no DataCanvas — re-run RBv2_1_1/1."); return; }
            var view = rig.GetComponent<SuperPanelView>();

            var old = data.Find("UsagePage");
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);

            var page = Stretch("UsagePage", data);
            Undo.RegisterCreatedObjectUndo(page.gameObject, "Build Usage & service page");
            AddImage(Stretch("PageBG", page), DPPSpriteFactory.RoundedR22, DPPTheme.NavyPanel, sliced: true);
            var ups = page.gameObject.AddComponent<UsePhaseView>();

            // ---------------- lens pills — the top of the page ----------------
            string[] lensNames = { "Thermal Data", "Electrical Data", "Software" };
            var lensFills = new Image[UsePhaseView.LensCount];
            var lensStrokes = new Image[UsePhaseView.LensCount];
            var lensLabels = new TMP_Text[UsePhaseView.LensCount];
            for (int i = 0; i < UsePhaseView.LensCount; i++)
            {
                var root = TL($"Lens{i}", page, UpMargin + i * 126f, 20f, 120f, 32f);
                // Full elevation kit (00 §4.1) — shadow + gloss + rise. The first
                // build shipped the pills flat (Thiago, round 2 of this page).
                AddShadow(root, 120f, 32f, DPPSpriteFactory.RoundedR13);
                var outline = AddImage(CenterIn("HoverOutline", root, 120f + HoverHalo, 32f + HoverHalo),
                    DPPSpriteFactory.RoundedR13, Color.white, sliced: true);
                outline.gameObject.SetActive(false);
                lensStrokes[i] = AddImage(CenterIn("Stroke", root, 120f, 32f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#21407a"), sliced: true);
                lensFills[i] = AddImage(CenterIn("Fill", root, 118f, 30f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#0E2950"), sliced: true, raycast: true);
                AddGloss(root, 120f, 32f, DPPSpriteFactory.RoundedR13);
                lensLabels[i] = AddText(Stretch("Label", root), lensNames[i], 10.5f,
                    DPPTheme.TextSecondary, bold: i == 0, align: TextAlignmentOptions.Center);

                var btn = root.gameObject.AddComponent<Button>();
                btn.transition = Selectable.Transition.None;
                btn.targetGraphic = lensFills[i];
                WireClick(btn, ups, "ShowLens" + i);
                var hover = root.gameObject.AddComponent<HoverHighlight>();
                SetRef(hover, "highlightOutline", outline.gameObject);
            }

            // ---------------- lens roots ----------------
            var thermal = Stretch("ThermalData", page);
            var electrical = Stretch("ElectricalData", page);
            var software = Stretch("Software", page);

            // ================= THERMAL DATA (default) =================
            AddText(TL("SohCap", thermal, UpMargin, 62f, 300f, 12f),
                "STATE OF HEALTH — MIN OF MECHANISMS", 8f, DPPTheme.TealAccent, bold: true);
            var sohValue = AddText(TL("SohValue", thermal, UpMargin, 76f, 110f, 34f), "48 %", 26f,
                DPPTheme.TextOnNavy, bold: true);
            var sohMech = AddText(TL("SohMech", thermal, UpMargin, 112f, 130f, 12f),
                "limiting: thermal fatigue", 8.5f, DPPTheme.TextSecondary, bold: false);

            AddText(TL("FlashLbl", thermal, 150f, 78f, 80f, 11f), "Flash endurance", 8.5f, DPPTheme.TextOnNavy, bold: false);
            AddImage(TL("FlashTrack", thermal, 232f, 80f, 120f, 7f), DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#0a1a38"), sliced: true);
            var flashBarImg = AddImage(TL("FlashBar", thermal, 232f, 80f, 66f, 7f), DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#f0c879"), sliced: true);
            var flashPct = AddText(TL("FlashPct", thermal, 356f, 78f, 40f, 11f), "55 %", 8.5f, DPPTheme.TextOnNavy, bold: false);

            AddText(TL("FatLbl", thermal, 150f, 98f, 80f, 11f), "Thermal fatigue", 8.5f, DPPTheme.TextOnNavy, bold: false);
            AddImage(TL("FatTrack", thermal, 232f, 100f, 120f, 7f), DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#0a1a38"), sliced: true);
            var fatBarImg = AddImage(TL("FatBar", thermal, 232f, 100f, 58f, 7f), DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#f0c879"), sliced: true);
            var fatPct = AddText(TL("FatPct", thermal, 356f, 98f, 40f, 11f), "48 %", 8.5f, DPPTheme.TextOnNavy, bold: false);

            AddImage(TL("ReuseStrip", thermal, UpMargin, 128f, UpW, 26f), DPPSpriteFactory.RoundedR13,
                DPPTheme.Hex("#0E2950"), sliced: true);
            var reuseFraction = AddText(TL("ReuseFrac", thermal, UpMargin + 12f, 133f, 240f, 16f),
                "0.767 of mass carries a reuse verdict", 9.5f, DPPTheme.TealAccent, bold: true);
            AddText(TL("ReuseBand", thermal, UpMargin + 254f, 134f, 116f, 14f),
                "→ inside Sc4's [A] band", 7.5f, DPPTheme.TextSecondary, bold: false);

            // verdict legend — colour chips with masses
            (string label, string hex)[] legend =
            {
                ("reuse 356.0 g", "#2eb086"), ("after test 150.1 g", "#f0c879"),
                ("recovery 146.1 g", "#21407a"), ("consumable 8.0 g", "#6f86a8"),
            };
            float lx = UpMargin;
            foreach (var (label, hex) in legend)
            {
                AddImage(TLCenter($"Chip{lx:0}", thermal, lx + 5f, 168f, 9f, 9f), DPPSpriteFactory.Circle64, DPPTheme.Hex(hex));
                AddText(TL($"ChipLbl{lx:0}", thermal, lx + 12f, 162f, 90f, 12f), label, 7.5f, DPPTheme.TextSecondary, bold: false);
                lx += 94f;
            }

            AddText(TL("DeltaCap", thermal, UpMargin, 182f, UpW, 11f),
                "ΔT BAND · CYCLES · DAMAGE — WHERE THE 48 % COMES FROM", 7.5f, DPPTheme.Hex("#5d7396"), bold: true);

            string[] demoBands = { "below 20 °C", "20–40 °C", "40–60 °C", "60–80 °C", "above 80 °C" };
            string[] demoCycles = { "4,200", "4,600", "2,000", "420", "30" };
            string[] demoDamage = { "0.039", "0.173", "0.208", "0.086", "0.010" };
            float[] demoBarW = { 27f, 116f, 140f, 58f, 7f };
            var bandLbls = new TMP_Text[5]; var cycleLbls = new TMP_Text[5];
            var dmgLbls = new TMP_Text[5]; var dmgBars = new RectTransform[5];
            for (int i = 0; i < 5; i++)
            {
                float y = 198f + i * 19f;
                bandLbls[i] = AddText(TL($"Band{i}", thermal, UpMargin, y, 86f, 12f), demoBands[i], 8.5f, DPPTheme.TextOnNavy, bold: false);
                cycleLbls[i] = AddText(TL($"Cyc{i}", thermal, 116f, y, 50f, 12f), demoCycles[i], 8.5f, DPPTheme.TextOnNavy, bold: false);
                var bar = TL($"DmgBar{i}", thermal, 172f, y + 2f, demoBarW[i], 7f);
                AddImage(bar, DPPSpriteFactory.RoundedR3, DPPTheme.Hex("#f08a3c"), sliced: true);   // chart colour, decision 2
                dmgBars[i] = bar;
                dmgLbls[i] = AddText(TL($"Dmg{i}", thermal, 330f, y, 66f, 12f), demoDamage[i], 8.5f,
                    DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
            }
            AddImage(TL("SumRule", thermal, UpMargin, 294f, UpW, 1f), null, DPPTheme.Hex("#1a335f"));
            var damageSum = AddText(TL("DmgSum", thermal, UpMargin, 300f, UpW, 12f),
                "Σ damage 0.516 → consumed 51.6 % · remaining 48 %", 8.5f, DPPTheme.TextSecondary, bold: false);

            var findLbls = new TMP_Text[2]; var findVals = new TMP_Text[2];
            string[] demoFindL = { "Over-temperature", "Processor failure 2025-12" };
            string[] demoFindV = { "41 h above 80 °C limit · peak 94 °C", "replaced at 220,260 km · heat implicated" };
            for (int i = 0; i < 2; i++)
            {
                float y = 318f + i * 26f;
                AddImage(TL($"FindBG{i}", thermal, UpMargin, y, UpW, 22f), DPPSpriteFactory.RoundedR13,
                    DPPTheme.Hex("#0a1a38"), sliced: true);
                findLbls[i] = AddText(TL($"FindL{i}", thermal, UpMargin + 10f, y + 4f, 150f, 14f), demoFindL[i], 8.5f, DPPTheme.TextOnNavy, bold: false);
                findVals[i] = AddText(TL($"FindV{i}", thermal, UpMargin + 162f, y + 4f, 204f, 14f), demoFindV[i], 8f, DPPTheme.TextSecondary, bold: false);
            }

            // ================= ELECTRICAL DATA =================
            AddText(TL("ElCap", electrical, UpMargin, 66f, 200f, 12f), "ELECTRICAL", 8f, DPPTheme.Hex("#5d7396"), bold: true);
            var elT = UpRow(electrical, "Transients", 84f, "Voltage transients", "2,180  (ISO 7637-2)");
            var elU = UpRow(electrical, "Undervolt", 108f, "Undervoltage events", "37");
            var elL = UpRow(electrical, "LoadDump", 132f, "Load dumps", "4");
            AddText(TL("ElNote", electrical, UpMargin, 154f, UpW, 24f),
                "Undervoltage events cluster at the 12 V battery failures (2012, 2020).",
                8f, DPPTheme.Hex("#5d7396"), bold: false);

            AddText(TL("DaCap", electrical, UpMargin, 190f, 200f, 12f), "DATA", 8f, DPPTheme.Hex("#5d7396"), bold: true);
            var daF = UpRow(electrical, "Flash", 208f, "Flash write cycles", "45,000 / 100,000 · 55 % left");
            var daC = UpRow(electrical, "Cpu", 232f, "CPU above 80 % load", "675 h");
            var daE = UpRow(electrical, "EccResets", 256f, "ECC corrected · resets", "118 · 9");
            var daN = UpRow(electrical, "Can", 280f, "CAN", "8,640 error frames · 6 bus-off");
            var daD = UpRow(electrical, "Dtc", 304f, "Diagnostic codes", "41 total · 1 active · 40 cleared");
            AddText(TL("DaNote", electrical, UpMargin, 330f, UpW, 12f),
                "flash is the COUNTED limit (hard); everything else is context", 8f, DPPTheme.Hex("#5d7396"), bold: false);
            electrical.gameObject.SetActive(false);

            // ================= SOFTWARE =================
            var swF = UpRow(software, "Firmware", 70f, "Firmware", "15 versions · v1.0 → v15.0");
            var swM = UpRow(software, "Maps", 94f, "Calibration map changes", "6");
            var swR = UpRow(software, "Recal", 118f, "Sensor recalibrations", "3");
            AddImage(TL("LinkBG", software, UpMargin, 146f, UpW, 64f), DPPSpriteFactory.RoundedR13,
                DPPTheme.Hex("#0a1a38"), sliced: true);
            var swLink = AddText(TL("LinkTxt", software, UpMargin + 12f, 154f, UpW - 24f, 50f),
                "13 diagnostic codes tie to logged service events — the unit raises codes for the " +
                "whole vehicle, which is why the service log carries vehicle systems as well as " +
                "its own faults.", 8.5f, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.TopLeft);
            swLink.textWrappingMode = TextWrappingModes.Normal;
            AddText(TL("Basis", software, UpMargin, 224f, UpW, 12f), "basis: simulated", 7.5f,
                DPPTheme.Hex("#5d7396"), bold: false);
            software.gameObject.SetActive(false);

            // ================= PART RECORD =================
            var record = Stretch("PartRecord", page);
            var partName = AddText(TL("PartName", record, UpMargin, 64f, UpW, 20f),
                "Connectors 3× AS018-35", 14f, DPPTheme.TextOnNavy, bold: true);
            var chip = AddImage(TLCenter("VerdictChip", record, UpMargin + 6f, 100f, 11f, 11f),
                DPPSpriteFactory.Circle64, DPPTheme.Hex("#f0c879"));
            var partVerdict = AddText(TL("PartVerdict", record, UpMargin + 18f, 93f, 150f, 14f),
                "reuse after test", 10f, DPPTheme.Hex("#f0c879"), bold: false);
            var partMass = AddText(TL("PartMass", record, UpMargin + 170f, 93f, 80f, 14f),
                "150.1 g", 10f, DPPTheme.TextSecondary, bold: false);
            AddImage(TL("ReasonBG", record, UpMargin, 116f, UpW, 64f), DPPSpriteFactory.RoundedR13,
                DPPTheme.Hex("#0a1a38"), sliced: true);
            var partReason = AddText(TL("ReasonTxt", record, UpMargin + 12f, 124f, UpW - 24f, 50f),
                "One SENS-B reseat logged 2022 — contact resistance to be verified before reuse.",
                8.5f, DPPTheme.TextSecondary, bold: false, align: TextAlignmentOptions.TopLeft);
            partReason.textWrappingMode = TextWrappingModes.Normal;
            // (Round 3, Thiago 2026-08-08: the record's own Back pill is GONE — it
            // duplicated the page's bottom Back, which already closes the record
            // first: UsePhaseView.OnBack → CloseRecord when a record is open.)
            record.gameObject.SetActive(false);

            // ================= bottom bar =================
            var backBtn = PsSmallPill(page, "BackButton", UpMargin + 45f, 90f, "Back",
                primary: false, out _, cy: 402f, fontSize: 11f);
            WireClick(backBtn, ups, nameof(UsePhaseView.OnBack));
            var primaryBtn = PsSmallPill(page, "PrimaryButton", 420f - UpMargin - 75f, 150f, "Next",
                primary: true, out var primaryLbl, cy: 402f, fontSize: 11f);
            WireClick(primaryBtn, ups, nameof(UsePhaseView.OnPrimary));

            // ================= wiring =================
            SetRef(ups, "owner", view);
            var stageLink = rig.GetComponentInChildren<ModelLinkController>(true);
            if (stageLink != null)
            {
                SetRef(ups, "modelLink", stageLink);
                SetRef(stageLink, "usePhase", ups);
                SetInt(stageLink, "usageTab", 1);
                Debug.Log("[DPPUIBuilder] Usage link closed: lens tints + per-tab pick routing.");
            }
            else Debug.LogWarning("[DPPUIBuilder] No ModelLinkController under the rig — lenses will not " +
                                  "tint the model and picks will not reach the usage record.");

            SetRefArray(ups, "lensFills", lensFills);
            SetRefArray(ups, "lensStrokes", lensStrokes);
            SetRefArray(ups, "lensLabels", lensLabels);
            SetRefArray(ups, "lensRoots", new GameObject[] { thermal.gameObject, electrical.gameObject, software.gameObject });

            SetRef(ups, "sohValue", sohValue);
            SetRef(ups, "sohMechanism", sohMech);
            SetRef(ups, "flashBar", flashBarImg.rectTransform);
            SetRef(ups, "flashPct", flashPct);
            SetRef(ups, "fatigueBar", fatBarImg.rectTransform);
            SetRef(ups, "fatiguePct", fatPct);
            SetRef(ups, "reuseFraction", reuseFraction);
            SetRefArray(ups, "deltaBandLabels", bandLbls);
            SetRefArray(ups, "deltaCycles", cycleLbls);
            SetRefArray(ups, "deltaDamage", dmgLbls);
            SetRefArray(ups, "deltaDamageBars", dmgBars);
            SetRef(ups, "damageSum", damageSum);
            SetRefArray(ups, "findingLabels", findLbls);
            SetRefArray(ups, "findingValues", findVals);
            SetFloat(ups, "barTrack", 120f);
            SetFloat(ups, "damageBarTrack", 140f);

            SetRef(ups, "elTransients", elT); SetRef(ups, "elUndervoltage", elU); SetRef(ups, "elLoadDumps", elL);
            SetRef(ups, "daFlash", daF); SetRef(ups, "daCpu", daC); SetRef(ups, "daEccResets", daE);
            SetRef(ups, "daCan", daN); SetRef(ups, "daDtc", daD);
            SetRef(ups, "swFirmware", swF); SetRef(ups, "swMaps", swM); SetRef(ups, "swRecal", swR);
            SetRef(ups, "swLinkage", swLink);

            SetRef(ups, "partRecordRoot", record.gameObject);
            SetRef(ups, "partName", partName);
            SetRef(ups, "partVerdictChip", chip);
            SetRef(ups, "partVerdict", partVerdict);
            SetRef(ups, "partMass", partMass);
            SetRef(ups, "partReason", partReason);
            SetRef(ups, "primaryLabel", primaryLbl);

            // ---- tabPages MERGE (trap 4 — never overwrite) ----
            if (view != null)
            {
                var pages = new GameObject[SuperPanelView.TabCount];
                var specs = SpFind("ProductSpecsPage");
                if (specs != null) pages[0] = specs;
                pages[1] = page.gameObject;
                var envPage = SpFind("EnvironmentalPage");   // 04e: Training is gone; env is tab 2
                if (envPage != null) pages[2] = envPage;
                // data.Find, NOT SpFind — RBv2_1/8's legacy flat-canvas screen
                // shares the "CertificatesPage" name (device bug, 2026-08-08).
                var certs = data.Find("CertificatesPage");
                if (certs != null) pages[3] = certs.gameObject;
                SetRefArray(view, "tabPages", pages);
            }

            page.gameObject.SetActive(false);   // SuperPanelView shows it when tab 1 opens

            Selection.activeGameObject = page.gameObject;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("[DPPUIBuilder] RBv2_1_1/3 — Usage & service built into the data canvas. " +
                      "Run RBv2_1/Tools/Verify wiring, then SAVE THE SCENE.");
        }

        /// <summary>One label/value row of the Usage page: grey label left, white value right.</summary>
        private static TMP_Text UpRow(RectTransform parent, string name, float y, string label, string demoValue)
        {
            AddText(TL($"{name}Lbl", parent, UpMargin, y, 170f, 14f), label, 9f, DPPTheme.TextSecondary, bold: false);
            return AddText(TL($"{name}Val", parent, UpMargin + 130f, y, UpW - 130f, 14f), demoValue, 9f,
                DPPTheme.TextOnNavy, bold: false, align: TextAlignmentOptions.MidlineRight);
        }
    }
}
