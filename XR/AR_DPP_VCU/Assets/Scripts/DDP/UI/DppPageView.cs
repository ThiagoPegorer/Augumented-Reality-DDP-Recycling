using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1 — 04: THE DPP PAGE. One panel, four tabs, two roles.
    ///
    /// Replaces RB2.0's split between the DPP Canva (spec 13) and Composition &amp;
    /// impact (spec 14). Thiago, 2026-08-04: "split it is being confuse and not a
    /// smart way for the user."
    ///
    /// The screen is built ONCE and runs in two modes, driven by
    /// <see cref="ScreenRouter.Mode"/>:
    ///
    ///   PRODUCT USER  header arrow -> stakeholder · Quit (red) -> welcome ·
    ///                 Scan next product
    ///   RECYCLER      no header arrow · Back -> stakeholder ·
    ///                 Continue to disassembly · all four tabs
    ///
    /// Building two canvases instead would double every future DPP edit and
    /// guarantee the two drift apart.
    ///
    /// Certificates &amp; safety is a SEPARATE SCREEN owned by ScreenRouter, not a
    /// modal on this page. It covered the whole panel anyway, and an overlay that
    /// shares this canvas plane with live controls lets a click resolve to the
    /// button underneath it - which is how the old modal's Close fired "Scan next
    /// product" (device test 2026-08-05).
    ///
    /// CHIP RULE (spec 00 section 5): a chip's width is its label's
    /// preferredWidth + 24, measured at runtime. Every value here is bound from
    /// the payload, so a hardcoded width clips the moment a number gains a digit.
    /// The builder bakes a starting width only so the editor preview reads right.
    /// </summary>
    public class DppPageView : MonoBehaviour
    {
        // =================================================================
        // Wiring (set by RBv2_1/8)
        // =================================================================
        [Header("Routing")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private WelcomeController welcome;
        [SerializeField] private QRScanController scanner;

        [Header("Header")]
        [SerializeField] private GameObject backButton;      // product user only
        [SerializeField] private RectTransform title;        // shifts right when the arrow shows

        [Header("Bottom bar")]
        [SerializeField] private TMP_Text leftLabel;
        [SerializeField] private TMP_Text primaryLabel;


        [Header("Tab 1 — product specifications")]
        [SerializeField] private RectTransform chipModel;    [SerializeField] private TMP_Text lblModel;
        [SerializeField] private RectTransform chipMaker;    [SerializeField] private TMP_Text lblMaker;
        [SerializeField] private RectTransform chipSerial;   [SerializeField] private TMP_Text lblSerial;

        [Header("Tab 2 — usage history")]
        [SerializeField] private RectTransform chipEnergy;   [SerializeField] private TMP_Text lblEnergy;
        [SerializeField] private RectTransform chipDistance; [SerializeField] private TMP_Text lblDistance;
        [SerializeField] private RectTransform chipHours;    [SerializeField] private TMP_Text lblHours;

        [Header("Tab 3 — environmental impact (equal-width set)")]
        [SerializeField] private RectTransform chipCo2;      [SerializeField] private TMP_Text lblCo2;
        [SerializeField] private RectTransform chipMinerals; [SerializeField] private TMP_Text lblMinerals;
        [SerializeField] private RectTransform chipEutro;    [SerializeField] private TMP_Text lblEutro;

        [Header("Tab 4 — training disassembly")]
        [SerializeField] private RectTransform chipSteps;    [SerializeField] private TMP_Text lblSteps;
        [SerializeField] private RectTransform chipActions;  [SerializeField] private TMP_Text lblActions;
        [SerializeField] private RectTransform chipMinutes;  [SerializeField] private TMP_Text lblMinutes;

        [Header("Left button colours")]
        [SerializeField] private Image leftFill;
        [SerializeField] private Image leftStroke;

        [Header("Behaviour")]
        [Tooltip("Panel-local x of the title with the back arrow present. Without the arrow the " +
                 "title sits at the 24 px margin (Thiago, 2026-08-04: 'keep in the extreme left').")]
        [SerializeField] private float titleXWithArrow = 76f;
        [SerializeField] private float titleXPlain = 24f;

        private const float ChipPad = 24f;   // 12 px each side (spec 00 section 5)

        private static readonly Color DestructiveRed = new Color32(0xE2, 0x4B, 0x4A, 0xFF);
        private static readonly Color SecondaryFill  = new Color32(0x1A, 0x27, 0x40, 0xFF);
        private static readonly Color SecondaryStroke= new Color32(0x32, 0x4A, 0x6D, 0xFF);
        private static readonly Color SecondaryText  = new Color32(0x9F, 0xB3, 0xD1, 0xFF);

        // The fetch lands while this screen is still INACTIVE (DPPManager populates
        // every view at once, long before the user reaches the passport). TMP
        // reports preferredWidth = 0 for text that has never been laid out, so
        // fitting chips inside Populate alone produced 24 px stubs and a collapsed
        // row - exactly what the 2026-08-05 device test showed. The labels keep
        // their text either way, so re-fitting in OnEnable is enough: no cache.

        // =================================================================
        // Lifecycle
        // =================================================================

        private void OnEnable()
        {
            ApplyMode();
            RefitChips();
        }

        /// <summary>Called by DPPManager on every successful fetch.</summary>
        public void Populate(DPPData data)
        {
            if (data == null) return;

            var id = data.identity;
            if (id != null)
            {
                Set(lblModel,  id.model);
                Set(lblMaker,  id.manufacturer);
                Set(lblSerial, id.serial_number);
            }

            var use = data.environmental != null ? data.environmental.usage_profile : null;
            if (use != null)
            {
                if (use.lifetime_energy_kwh.HasValue)
                    Set(lblEnergy, $"{use.lifetime_energy_kwh.Value:0.#} kWh");
                if (use.lifetime_distance_km.HasValue)
                    Set(lblDistance, $"{use.lifetime_distance_km.Value:N0} km");
                if (use.operating_hours.HasValue)
                    Set(lblHours, $"{use.operating_hours.Value:N0} h");
            }

            // Tab 3 keeps the strings Thiago dictated verbatim. The values are
            // bound; the category wording is not derived from the payload,
            // because the payload's category names are the raw EF 3.1 labels.
            var rec = data.environmental != null ? data.environmental.impact_recovery : null;
            if (rec != null)
            {
                foreach (var r in rec)
                {
                    if (r == null || string.IsNullOrEmpty(r.category)) continue;
                    string c = r.category.ToLowerInvariant();
                    if (c.Contains("climate"))
                        Set(lblCo2, $"CO2 Emissions {r.baseline:0.##} kg CO2 eq");
                    else if (c.Contains("mineral"))
                        Set(lblMinerals, $"Minerals & Metals {r.baseline:0.#####} kg Sb eq");
                    else if (c.Contains("eutroph"))
                        Set(lblEutro, $"Eutroph. Freshwater {r.baseline:0.#####} kg P eq");
                }
            }

            var steps = data.disassembly != null ? data.disassembly.steps : null;
            if (steps != null && steps.Count > 0)
            {
                int actions = 0;
                foreach (var s in steps) if (s != null && s.actions != null) actions += s.actions.Count;
                Set(lblSteps,   $"{steps.Count} steps");
                Set(lblActions, $"{actions} actions");
                int mins = data.disassembly.estimated_time_min;
                if (mins > 0) Set(lblMinutes, $"~{mins} min");
            }

            RefitChips();
        }

        // =================================================================
        // Role
        // =================================================================

        private void ApplyMode()
        {
            bool recycler = router == null || router.Mode != StakeholderMode.ProductUser;

            if (backButton != null) backButton.SetActive(!recycler);

            if (title != null)
            {
                var p = title.anchoredPosition;
                title.anchoredPosition = new Vector2(recycler ? titleXPlain : titleXWithArrow, p.y);
            }

            Set(leftLabel,    recycler ? "Back" : "Quit");
            Set(primaryLabel, recycler ? "Continue to disassembly" : "Scan next product");

            // Quit ends the participant's session, so it takes the destructive
            // treatment of 00 s.2.1 meaning 3: solid red, white bold label.
            // Back continues the journey and stays the secondary pill.
            if (leftFill != null)   leftFill.color   = recycler ? SecondaryFill : DestructiveRed;
            if (leftStroke != null) leftStroke.color = recycler ? SecondaryStroke : DestructiveRed;
            if (leftLabel != null)  leftLabel.color  = recycler ? SecondaryText : Color.white;
        }

        // =================================================================
        // Button targets (wired by the builder)
        // =================================================================

        /// <summary>Header arrow — product user only. One step back, to the role fork.</summary>
        public void OnBack()
        {
            if (router != null) router.ShowStakeholder();
            else Debug.LogWarning("[DppPage] No router — cannot return to the stakeholder screen.");
        }

        /// <summary>Bottom-left. Recycler: Back to the role fork. Product user: Quit
        /// to Welcome — "an edge that leaves the session says Quit, never Back" (00 §5).
        /// It leaves the SESSION, not the app; Close app still owns Application.Quit.</summary>
        public void OnLeftButton()
        {
            bool recycler = router == null || router.Mode != StakeholderMode.ProductUser;
            if (recycler) { OnBack(); return; }

            if (welcome != null) welcome.ShowWelcome();
            else Debug.LogWarning("[DppPage] No WelcomeController — cannot quit the session.");
        }

        /// <summary>Bottom-right primary. Recycler: the teardown. Product user: the next unit.</summary>
        public void OnPrimary()
        {
            bool recycler = router == null || router.Mode != StakeholderMode.ProductUser;
            if (recycler)
            {
                if (router != null) router.ShowDisassembly();
                return;
            }
            if (scanner != null) scanner.BeginNewScan();
            else Debug.LogWarning("[DppPage] No QRScanController — cannot start a new scan.");
        }

        // ---- the four seams the per-tab phases plug into (spec 04 section 10) ----
        public void OpenTab1() => Debug.Log("[DppPage] + Product specifications — phase 2, not built yet.");
        public void OpenTab2() => Debug.Log("[DppPage] + Usage history — phase 3, not built yet.");
        public void OpenTab3() => Debug.Log("[DppPage] + Environmental impact — phase 4, not built yet.");
        public void OpenTab4() => Debug.Log("[DppPage] + Training disassembly — phase 5, not built yet.");

        // =================================================================
        // Chips
        // =================================================================

        private void RefitChips()
        {
            Fit(chipModel, lblModel);
            Fit(chipMaker, lblMaker);
            Fit(chipSerial, lblSerial);

            Fit(chipEnergy, lblEnergy);
            Fit(chipDistance, lblDistance);
            Fit(chipHours, lblHours);

            Fit(chipSteps, lblSteps);
            Fit(chipActions, lblActions);
            Fit(chipMinutes, lblMinutes);

            Reflow(chipMaker, chipSerial);
            Reflow(chipEnergy, chipDistance, chipHours);
            Reflow(chipSteps, chipActions, chipMinutes);

            // Tab 3 is a SET: one shared width, so the three read as one column
            // rather than a ragged stack.
            float w = 0f;
            w = Mathf.Max(w, Width(lblCo2));
            w = Mathf.Max(w, Width(lblMinerals));
            w = Mathf.Max(w, Width(lblEutro));
            SetWidth(chipCo2, w);
            SetWidth(chipMinerals, w);
            SetWidth(chipEutro, w);
        }

        private static float Width(TMP_Text label)
        {
            if (label == null) return 0f;
            label.ForceMeshUpdate();          // preferredWidth is stale until the mesh is built
            return label.preferredWidth + ChipPad;
        }

        private static void Fit(RectTransform chip, TMP_Text label)
            => SetWidth(chip, Width(label));

        private static void SetWidth(RectTransform chip, float w)
        {
            if (chip == null || w <= 0f) return;
            chip.sizeDelta = new Vector2(w, chip.sizeDelta.y);
        }

        /// <summary>Re-lay a horizontal chip run left to right with a 6 px gutter,
        /// keeping the first chip where the builder put it.</summary>
        private static void Reflow(params RectTransform[] run)
        {
            if (run == null || run.Length == 0 || run[0] == null) return;
            float x = run[0].anchoredPosition.x;
            foreach (var c in run)
            {
                if (c == null) continue;
                var p = c.anchoredPosition;
                c.anchoredPosition = new Vector2(x, p.y);
                x += c.sizeDelta.x + 6f;
            }
        }

        private static void Set(TMP_Text t, string v)
        {
            if (t != null && !string.IsNullOrEmpty(v)) t.text = v;
        }
    }
}
