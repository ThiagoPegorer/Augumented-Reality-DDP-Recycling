using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 — 04 v2: THE SUPER PANEL (spec `04_DPP_page.md` v2, mocks
    /// `drafts/04b_v2_super_panel.svg` + `04b_v3_plan_view.svg`).
    ///
    /// Three world-space canvases toed in about a single rig:
    ///
    ///   RAIL   220 × 430, yawed to face the eye — navigation only
    ///   STAGE  340 × 430, yaw 0 — the model, permanently on screen
    ///   DATA   420 × 430, yawed to face the eye — the active tab's content
    ///
    /// WHY THIS EXISTS: P02 and P03 both reported not perceiving the 3D model.
    /// v1 answered by spawning the model first; it was still BESIDE the passport.
    /// v2 puts it between the navigation and the data, so there is no state in
    /// which the passport is visible and the model is not.
    ///
    /// THE RIG IS NOT THE PANEL CANVAS. Every other screen (Welcome, QR,
    /// stakeholder, disassembly, steps, summary) still lives on the flat
    /// 640 × 430 DPPPanelCanvas. Replacing that canvas would have put every
    /// screen in the blast radius for one screen's benefit. ScreenRouter shows
    /// the rig and this canvas as alternatives (see ShowSuperPanel).
    ///
    /// LOCKED vs UNLOCKED (spec §3.2): locked, the model is a child of the stage
    /// and yaws slowly — a living illustration, which is what stops it reading as
    /// decoration. Unlocked, it re-parents OUT to its own root with its own grab
    /// bar and the standard two-hand gestures. Re-lock SNAPS it home rather than
    /// carrying it, because once freed it no longer follows the rig and the user
    /// can leave it across the room.
    ///
    /// ⚠ THE LAYOUT NEVER REFLOWS when the model leaves (00 §5: hit targets do
    /// not move under the user). The stage keeps a ghost outline of its home.
    /// </summary>
    public class SuperPanelView : MonoBehaviour
    {
        public const int TabCount = 4;

        // =================================================================
        // Wiring (set by RBv2_1_1/1)
        // =================================================================
        [Header("Routing")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private WelcomeController welcome;
        [SerializeField] private QRScanController scanner;

        [Header("Rail — four tab buttons")]
        [SerializeField] private RectTransform[] tabRoots;
        [SerializeField] private Image[] tabFills;
        [SerializeField] private Image[] tabStrokes;
        [SerializeField] private Image[] tabAccents;      // the 4 × 40 left bar, active only
        [SerializeField] private GameObject[] tabTicks;   // visited marker
        [SerializeField] private TMP_Text[] tabLine1;
        [SerializeField] private TMP_Text[] tabLine2;
        [SerializeField] private Image[] tabIcons;
        [SerializeField] private Button[] tabButtons;

        [Header("Rail — certificates, a fifth entry above the four tabs")]
        [SerializeField] private Image certFill;
        [SerializeField] private Image certStroke;
        [SerializeField] private TMP_Text certLabel;
        [SerializeField] private Image certIcon;
        [SerializeField] private Button certButton;

        [Header("Data canvas — one page per tab, index-aligned with the rail")]
        [SerializeField] private GameObject[] tabPages;
        [SerializeField] private GameObject certPage;
        [SerializeField] private GameObject placeholderPage;
        [SerializeField] private TMP_Text placeholderLabel;

        [Header("Stage")]
        [SerializeField] private Transform stageModelHome;   // locked parent + snap target
        [SerializeField] private Transform freeModelRoot;    // unlocked parent, sibling of the rig
        [SerializeField] private Transform model;
        [SerializeField] private GameObject ghostOutline;
        [SerializeField] private GameObject freeModelGrabber;
        [SerializeField] private TMP_Text lockLabel;
        [SerializeField] private Image lockGlyph;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;

        [Header("Stage behaviour")]
        [Tooltip("Degrees per second of the locked idle yaw. Spec §3.2 asks for a ~12 s loop.")]
        [SerializeField] private float lockedYawSpeed = 30f;
        [Tooltip("Seconds for the snap home on re-lock. Spec §7 open item 7 — tune on device.")]
        [SerializeField] private float snapSeconds = 0.28f;

        [Header("Colours")]
        [SerializeField] private Color tabActiveFill    = new Color32(0x0D, 0x2A, 0x57, 0xFF);
        [SerializeField] private Color tabActiveStroke  = new Color32(0x2E, 0x5A, 0xA0, 0xFF);
        [SerializeField] private Color tabRestFill      = new Color32(0x0E, 0x29, 0x50, 0xFF);
        [SerializeField] private Color tabRestStroke    = new Color32(0x21, 0x40, 0x7A, 0xFF);
        [SerializeField] private Color textOnNavy       = Color.white;
        [SerializeField] private Color textSecondary    = new Color32(0x9F, 0xB3, 0xD1, 0xFF);

        [Tooltip("Opacity of a tab the Recycler has not reached yet (spec §6.2).")]
        [SerializeField] private float dimmedAlpha = 0.38f;

        private int _active;
        private bool _certOpen;
        private readonly bool[] _visited = new bool[TabCount];
        private bool _unlocked;
        private float _snapT = -1f;
        private Vector3 _snapFromPos;
        private Quaternion _snapFromRot;
        private Vector3 _snapFromScale;
        private Vector3 _homeScale = Vector3.one;

        /// <summary>True while the Recycler walkthrough is in force. The Product
        /// user is never walked: they have no disassembly to reach, so all four
        /// tabs are lit from the start (spec §6).</summary>
        private bool Walkthrough => router != null && router.Mode == StakeholderMode.Recycler;

        // =================================================================
        // Lifecycle
        // =================================================================

        private void OnEnable()
        {
            // A fresh visit is a fresh walkthrough. Clearing here rather than on
            // the stakeholder screen keeps the kiosk cycle safe by the same
            // argument ScreenRouter.ShowStakeholder makes about Mode: every route
            // in passes through OnEnable, so participant 2 can never inherit
            // participant 1's progress.
            for (int i = 0; i < TabCount; i++) _visited[i] = false;
            _certOpen = false;

            if (_unlocked) ReLock(instant: true);
            SelectTab(0);
        }

        private void OnDisable()
        {
            // Leaving the passport with the model floating in the room would
            // strand it — nothing else re-parents it back.
            if (_unlocked) ReLock(instant: true);
        }

        private void Update()
        {
            if (model == null) return;

            if (_snapT >= 0f)
            {
                _snapT += Time.deltaTime;
                float t = snapSeconds <= 0f ? 1f : Mathf.Clamp01(_snapT / snapSeconds);
                float e = 1f - (1f - t) * (1f - t);           // ease-out quad, no DOTween dependency
                model.localPosition = Vector3.Lerp(_snapFromPos, Vector3.zero, e);
                model.localRotation = Quaternion.Slerp(_snapFromRot, Quaternion.identity, e);
                model.localScale    = Vector3.Lerp(_snapFromScale, _homeScale, e);
                if (t >= 1f) _snapT = -1f;
                return;
            }

            // Locked idle: a slow yaw, no input accepted. Unlocked, the user owns
            // the transform and this must not fight them.
            if (!_unlocked) model.Rotate(Vector3.up, lockedYawSpeed * Time.deltaTime, Space.Self);
        }

        // =================================================================
        // Tabs
        // =================================================================

        public void SelectTab(int index)
        {
            if (index < 0 || index >= TabCount) return;
            if (Walkthrough && !IsReachable(index)) return;

            _active = index;
            _visited[index] = true;
            _certOpen = false;

            // Changing tab re-locks the model (spec §3.2). Each tab re-reads the
            // model differently, so a freed model would otherwise keep showing a
            // tint that belongs to the tab the user just left.
            if (_unlocked) ReLock(instant: false);

            for (int i = 0; i < TabCount; i++)
            {
                bool on = i == index;
                bool reachable = !Walkthrough || IsReachable(i);

                if (tabFills != null   && i < tabFills.Length   && tabFills[i] != null)
                    tabFills[i].color = Fade(on ? tabActiveFill : tabRestFill, reachable);
                if (tabStrokes != null && i < tabStrokes.Length && tabStrokes[i] != null)
                    tabStrokes[i].color = Fade(on ? tabActiveStroke : tabRestStroke, reachable);
                if (tabAccents != null && i < tabAccents.Length && tabAccents[i] != null)
                    tabAccents[i].gameObject.SetActive(on);
                // Recycler only (Thiago, 2026-08-06). The Product user has every tab
                // open from the start, so a "you have been here" mark would imply a
                // sequence that does not exist for them.
                if (tabTicks != null   && i < tabTicks.Length   && tabTicks[i] != null)
                    tabTicks[i].SetActive(Walkthrough && _visited[i] && !on);

                Color label = Fade(on ? textOnNavy : textSecondary, reachable);
                if (tabLine1 != null && i < tabLine1.Length && tabLine1[i] != null) tabLine1[i].color = label;
                if (tabLine2 != null && i < tabLine2.Length && tabLine2[i] != null) tabLine2[i].color = label;
                if (tabIcons != null && i < tabIcons.Length && tabIcons[i] != null)
                    tabIcons[i].color = Fade(on ? new Color32(0x5D, 0xCA, 0xA5, 0xFF) : textSecondary, reachable);

                // Not-yet-reached tabs are dimmed AND refuse the click. Dimming
                // alone would be a lie: the target would still fire.
                if (tabButtons != null && i < tabButtons.Length && tabButtons[i] != null)
                    tabButtons[i].interactable = reachable;
            }

            PaintCert();
            ShowPage(index);
        }

        // =================================================================
        // Certificates & safety — a rail entry, not a screen
        // =================================================================

        /// <summary>
        /// Thiago, 2026-08-06: *"add it on the top of the tabs and when it clicked,
        /// make it open the information on the data canva as well, no more a
        /// separate panel"* — so compliance stops being a detour and becomes the
        /// fifth thing the rail can put in the data canvas.
        ///
        /// It is NOT part of the Recycler walkthrough. It is a reference page about
        /// the product, always reachable, and inserting it into the Back/Next chain
        /// would make the number of Nexts depend on whether the user happened to
        /// read it (spec §6).
        /// </summary>
        public void ShowCertificates()
        {
            _certOpen = true;
            PaintCert();
            ShowPage(_active);
        }

        /// <summary>The certificates page's own button — back to whichever tab was
        /// active when it was opened, never to a fixed tab.</summary>
        public void CloseCertificates()
        {
            _certOpen = false;
            PaintCert();
            ShowPage(_active);
        }

        private void PaintCert()
        {
            if (certFill != null)   certFill.color   = _certOpen ? tabActiveFill : tabRestFill;
            if (certLabel != null)  certLabel.color  = _certOpen ? textOnNavy : textSecondary;
            if (certIcon != null)   certIcon.color   = _certOpen ? textOnNavy : textSecondary;
            // certStroke keeps its red: 00 §2.1 meaning 4 marks this as regulatory
            // whether or not it is the open page, and the label names what it marks.
        }

        /// <summary>Visited tabs stay selectable so the recycler can re-read at any
        /// point (spec §6.5); the next unvisited one is the only new door.</summary>
        private bool IsReachable(int index)
        {
            if (_visited[index]) return true;
            for (int i = 0; i < index; i++) if (!_visited[i]) return false;
            return true;
        }

        private Color Fade(Color c, bool reachable)
        {
            if (reachable) return c;
            c.a *= dimmedAlpha;
            return c;
        }

        private void ShowPage(int index)
        {
            if (certPage != null) certPage.SetActive(_certOpen);

            bool built = !_certOpen && tabPages != null && index < tabPages.Length && tabPages[index] != null;
            for (int i = 0; tabPages != null && i < tabPages.Length; i++)
                if (tabPages[i] != null) tabPages[i].SetActive(i == index && built);

            bool showPlaceholder = !_certOpen && !built;
            if (placeholderPage != null) placeholderPage.SetActive(showPlaceholder);
            if (showPlaceholder && placeholderLabel != null)
                placeholderLabel.text = $"{TabName(index)}\n\nnot built yet — RBv2.1.1 phase 2";
        }

        private static string TabName(int i)
        {
            switch (i)
            {
                case 0: return "Product specifications";
                case 1: return "Usage & service";
                case 2: return "Environmental impact";
                default: return "Training disassembly";
            }
        }

        // ---- targets the rail buttons are wired to ----
        public void SelectTab0() => SelectTab(0);
        public void SelectTab1() => SelectTab(1);
        public void SelectTab2() => SelectTab(2);
        public void SelectTab3() => SelectTab(3);

        /// <summary>Called by a tab page's primary CTA. Recycler: the next tab, or
        /// the teardown from tab 4. Product user: the next unit.</summary>
        public void NextTab()
        {
            if (!Walkthrough)
            {
                if (scanner != null) scanner.BeginNewScan();
                else Debug.LogWarning("[SuperPanel] No QRScanController — cannot start a new scan.");
                return;
            }
            if (_active >= TabCount - 1)
            {
                if (router != null) router.ShowDisassembly();
                return;
            }
            SelectTab(_active + 1);
        }

        /// <summary>Called by a tab page's left button once that page's own drill
        /// is exhausted. On tab 1 it leaves the passport (spec §6.6).</summary>
        public void PrevTab()
        {
            // BOTH roles step back to the stakeholder fork (Thiago, 2026-08-06).
            //
            // v1 sent the Product user to Welcome and called the button Quit. On
            // device that was wrong twice over: it skipped the role fork, and it
            // ended the session when the user only wanted to leave the passport.
            // "Quit" also over-promised — the label now reads Back for both roles,
            // and Welcome is reached the way it always was, by Close app.
            if (!Walkthrough || _active <= 0)
            {
                if (router != null) router.ShowStakeholder();
                else Debug.LogWarning("[SuperPanel] No router — cannot return to the stakeholder screen.");
                return;
            }
            SelectTab(_active - 1);
        }

        /// <summary>True on the last tab of the walkthrough, where the CTA becomes
        /// `Continue to disassembly`. Tab pages read this to label their button.</summary>
        public bool IsLastTab => _active >= TabCount - 1;

        public string PrimaryLabel =>
            !Walkthrough ? "Scan next product" : (IsLastTab ? "Continue to disassembly" : "Next");

        /// <summary>Both roles read "Back": it leaves the passport for the role
        /// fork, which is one step back, not an exit (00 §5).</summary>
        public string BackLabel => "Back";

        // =================================================================
        // Lock / unlock
        // =================================================================

        public void ToggleLock()
        {
            if (model == null)
            {
                Debug.LogWarning("[SuperPanel] No model wired — nothing to unlock. Re-run RBv2_1_1/1.");
                return;
            }
            if (_unlocked) ReLock(instant: false); else Unlock();
        }

        private void Unlock()
        {
            if (freeModelRoot == null)
            {
                Debug.LogWarning("[SuperPanel] No freeModelRoot — the model cannot leave the stage.");
                return;
            }
            _unlocked = true;
            _snapT = -1f;

            // Re-parent WORLD-POSE-PRESERVING so the release is invisible: the
            // model must not jump at the moment it is freed. The stage's yaw is
            // 0° precisely so this holds (spec §2.1).
            freeModelRoot.SetPositionAndRotation(model.position, model.rotation);
            model.SetParent(freeModelRoot, worldPositionStays: true);

            if (ghostOutline != null) ghostOutline.SetActive(true);
            if (freeModelGrabber != null) freeModelGrabber.SetActive(true);
            Paint(false);
        }

        private void ReLock(bool instant)
        {
            _unlocked = false;
            if (model == null) return;

            if (stageModelHome != null)
                model.SetParent(stageModelHome, worldPositionStays: !instant);

            if (instant)
            {
                model.localPosition = Vector3.zero;
                model.localRotation = Quaternion.identity;
                model.localScale = _homeScale;
                _snapT = -1f;
            }
            else
            {
                // Snap home from wherever it ended up, rather than leaving the
                // user to carry it back. Without this, "return here to re-lock"
                // is a chore and the whole feature becomes a trap (spec §3.2).
                _snapFromPos = model.localPosition;
                _snapFromRot = model.localRotation;
                _snapFromScale = model.localScale;
                _snapT = 0f;
            }

            if (ghostOutline != null) ghostOutline.SetActive(false);
            if (freeModelGrabber != null) freeModelGrabber.SetActive(false);
            Paint(true);
        }

        private void Paint(bool locked)
        {
            if (lockLabel != null) lockLabel.text = locked ? "LOCKED" : "UNLOCKED";
            if (lockGlyph != null)
            {
                var s = locked ? lockedSprite : unlockedSprite;
                if (s != null) lockGlyph.sprite = s;
            }
        }

        /// <summary>Captured once so a re-lock restores the fitted scale the
        /// builder computed, not whatever the user zoomed to.</summary>
        private void Awake()
        {
            if (model != null) _homeScale = model.localScale;
        }
    }
}
