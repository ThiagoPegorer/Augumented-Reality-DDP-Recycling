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
        // 04e v2 (2026-08-08): Training disassembly is GONE — repetitive with the
        // guided flow that follows. 04e round 2 (same day): Certificates & safety
        // is a REAL FOURTH TAB — same size, same sequential walkthrough rule
        // (reachable only after Environmental impact), red regulatory styling.
        // The teardown route is the gated rail-bottom CTA (OnRailCta).
        public const int TabCount = 4;
        public const int CertTab = 3;

        // =================================================================
        // Wiring (set by RBv2_1_1/1)
        // =================================================================
        [Header("Routing")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private WelcomeController welcome;
        [SerializeField] private QRScanController scanner;

        [Header("Rail — four tab buttons (tab 3 = Certificates & safety, red)")]
        [SerializeField] private RectTransform[] tabRoots;
        [SerializeField] private Image[] tabFills;
        [SerializeField] private Image[] tabStrokes;
        [SerializeField] private Image[] tabAccents;      // the 4 × 40 left bar, active only
        [SerializeField] private GameObject[] tabTicks;   // visited marker
        [SerializeField] private TMP_Text[] tabLine1;
        [SerializeField] private TMP_Text[] tabLine2;
        [SerializeField] private Image[] tabIcons;
        [SerializeField] private Button[] tabButtons;

        [Header("Rail — bottom CTA (04e v2): recycler gate / product-user Back")]
        [SerializeField] private Button railCtaButton;
        [SerializeField] private Image railCtaFill;
        [SerializeField] private TMP_Text railCtaLabel;

        [Header("Data canvas — one page per tab, index-aligned with the rail")]
        [SerializeField] private GameObject[] tabPages;   // [3] = CertificatesPage (04e round 2)
        [SerializeField] private GameObject placeholderPage;
        [SerializeField] private TMP_Text placeholderLabel;

        [Header("Stage")]
        [SerializeField] private Transform stageModelHome;   // locked parent + snap target
        [SerializeField] private Transform freeModelRoot;    // unlocked parent, sibling of the rig
        [SerializeField] private Transform model;
        [SerializeField] private GameObject ghostOutline;
        [SerializeField] private GameObject freeModelGrabber;
        [SerializeField] private TMP_Text lockLabel;

        [Tooltip("RBv2.1.1 — the model link. LINKED explodes and follows the data canvas; FREE cuts " +
                 "the connection both ways. Null is fine: the stage then behaves as it did before.")]
        [SerializeField] private ModelLinkController modelLink;

        [Tooltip("RBv2.1.1 stage gestures (twist yaw + dial zoom, live in BOTH states). Reset on " +
                 "tab change and on re-link so LINKED always means one pose family. Null is fine.")]
        [SerializeField] private TwoHandTwistRotate stageGestures;

        [Tooltip("Device round 4: the gesture column follows the FREED model (zone §3.2 behaviour) " +
                 "and parks back on the stage on re-link. Null is fine.")]
        [SerializeField] private StageGestureHudFollower hudFollower;

        [Header("Gesture column (round 5 — collapsed while LINKED)")]
        [Tooltip("The column's backplate, anchored TOP so it grows downward.")]
        [SerializeField] private RectTransform hudBackplate;
        [Tooltip("Everything below the lock: hand lights + YAW/DIST/ZOOM. Hidden while LINKED.")]
        [SerializeField] private GameObject hudExtras;
        [SerializeField] private float hudCollapsedHeight = 70f;
        [SerializeField] private float hudExpandedHeight = 180f;
        [SerializeField] private float hudExpandSeconds = 0.2f;

        [Header("FREE transition (round 5)")]
        [Tooltip("Seconds to ease the freed model upright — pitch 25° → 0°, yaw back to 205°.")]
        [SerializeField] private float uprightSeconds = 0.35f;
        [Tooltip("World rotation of the FREED model. Round 7: 0°/180° — upright, connector face " +
                 "square to the user, no iso offset.")]
        [SerializeField] private Vector3 freeEuler = new Vector3(0f, 180f, 0f);
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
        private Coroutine _ctaHint;
        private HoverHighlight _ctaHover;   // trap 1 — state colours go through SetRestFillColor
        private readonly bool[] _visited = new bool[TabCount];
        private bool _unlocked;
        private Coroutine _freeSeq;
        private float _snapT = -1f;
        private Vector3 _snapFromPos;
        private Quaternion _snapFromRot;
        private Vector3 _snapFromScale;
        private Vector3 _homeScale = Vector3.one;

        /// <summary>True while the Recycler walkthrough is in force. The Product
        /// user is never walked: they have no disassembly to reach, so all four
        /// tabs are lit from the start (spec §6).</summary>
        private bool Walkthrough => router != null && router.Mode == StakeholderMode.Recycler;

        /// <summary>Spec 06: the model link routes a pinch by the ACTIVE tab
        /// (Usage tab → usage record; anywhere else → Component ID).</summary>
        public int ActiveTab => _active;

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
            _ctaHint = null;   // coroutines died with the disable; drop the stale handle

            if (_unlocked) ReLock(instant: true);
            else if (model != null)
            {
                // Round 7 (Thiago, 2026-08-09): every DPP-page entry starts the
                // showcase at the HOME pose (25°/205°). The idle spin accumulates
                // on the pivot and used to survive leaving the passport, so a
                // re-entry started wherever the last visit happened to stop.
                model.localPosition = Vector3.zero;
                model.localRotation = Quaternion.identity;
                model.localScale = _homeScale;
            }
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

            // IDLE YAW IS BACK (round 5, Thiago 2026-08-08) — scenario 1: after the
            // entry teardown finishes, the exploded model turns slowly about its own
            // middle axis at the iso tilt, a showcase the user selects from. It was
            // removed in RBv2.1.1 because a drifting body is harder to pinch; Thiago
            // overruled with the routine design. At 30°/s (12 s loop) picking is
            // slow-moving, not static — if study pilots show missed pinches, the
            // fallback is pausing the spin while the ray hovers the model.
            // Gated on OpenDone: the spin starts AFTER the show, never during it.
            if (!_unlocked && lockedYawSpeed > 0f && (modelLink == null || modelLink.OpenDone))
                model.Rotate(Vector3.up, lockedYawSpeed * Time.deltaTime, Space.Self);

            // FREE STAYS UPRIGHT — ENFORCED, not just eased (device round 6: the
            // model sat back at 25° after the sequence; the free grab bar's
            // billboard re-orients the free ROOT toward the eye, which re-tilted
            // the pivot the moment it activated). Scenario 2's contract is "no
            // inclination", so every frame in FREE the pivot's pitch and roll are
            // flattened while its yaw — the user's twist — is kept.
            if (_unlocked && _freeSeq == null)
            {
                Vector3 fwd = model.forward;
                fwd.y = 0f;
                if (fwd.sqrMagnitude > 1e-6f)
                    model.rotation = Quaternion.LookRotation(fwd.normalized, Vector3.up);
            }
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

            // Changing tab re-locks the model (spec §3.2). Each tab re-reads the
            // model differently, so a freed model would otherwise keep showing a
            // tint that belongs to the tab the user just left. (Round 5: no
            // gesture reset in the LINKED branch — gestures are disabled while
            // LINKED, and a ResetPose here would snap the idle spin's yaw.)
            if (_unlocked) ReLock(instant: false);

            for (int i = 0; i < TabCount; i++)
            {
                bool on = i == index;
                bool reachable = !Walkthrough || IsReachable(i);

                // ⚠ The "stroke" is a FULL rounded rect UNDER the fill — the 1 px
                // border look exists only because the opaque fill covers the rest.
                // Alpha-fading the fill of an unreachable tab therefore lets the
                // rect bleed through, and with a RED rect the whole tile read
                // solid red (device, 2026-08-08). Cert tab: the fill stays OPAQUE;
                // its dim state is a dark red stroke + faded label/icon instead.
                Color fillC = on ? tabActiveFill : tabRestFill;
                if (tabFills != null   && i < tabFills.Length   && tabFills[i] != null)
                    tabFills[i].color = i == CertTab ? fillC : Fade(fillC, reachable);
                // Tab 3 keeps its regulatory red identity in EVERY state (00 §2.1
                // meaning 4): bright red when reachable, dark red when dimmed.
                Color strokeC = i == CertTab
                    ? (Color)(reachable ? new Color32(0xE2, 0x4B, 0x4A, 0xFF)
                                        : new Color32(0x5C, 0x16, 0x22, 0xFF))
                    : Fade(on ? tabActiveStroke : tabRestStroke, reachable);
                if (tabStrokes != null && i < tabStrokes.Length && tabStrokes[i] != null)
                    tabStrokes[i].color = strokeC;
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
                    tabIcons[i].color = Fade(on ? (i == CertTab ? Color.white
                                                                : (Color)new Color32(0x5D, 0xCA, 0xA5, 0xFF))
                                                : textSecondary, reachable);

                // Not-yet-reached tabs are dimmed AND refuse the click. Dimming
                // alone would be a lie: the target would still fire.
                if (tabButtons != null && i < tabButtons.Length && tabButtons[i] != null)
                    tabButtons[i].interactable = reachable;
            }

            PaintRailCta();
            ShowPage(index);
        }

        // =================================================================
        // Certificates & safety — tab 3 since 04e round 2 (2026-08-08)
        // =================================================================

        /// <summary>History: a fifth entry above the tabs (2026-08-06), then the
        /// fourth slot outside the walkthrough (04e v2), now a FULL TAB — same
        /// size, same sequential rule as the others, reachable only after
        /// Environmental impact. These two wrappers survive because serialized
        /// UnityEvents in older scenes may still point at them (the
        /// modelExploration lesson: deleting the method logs missing-method
        /// errors instead of being silently unused).</summary>
        public void ShowCertificates() => SelectTab(CertTab);

        /// <summary>The certificates page's Back — one step back, like every page.</summary>
        public void CloseCertificates() => PrevTab();

        // =================================================================
        // Rail-bottom CTA — the teardown gate (04e v2, approved 2026-08-08)
        // =================================================================

        /// <summary>The gate: all four tabs visited (certificates is tab 3 since
        /// 04e round 2). Visiting counts — anything stricter (scroll depth, dwell
        /// time) is unverifiable and punishes study participants.</summary>
        private bool GateOpen
        {
            get
            {
                for (int i = 0; i < TabCount; i++) if (!_visited[i]) return false;
                return true;
            }
        }

        /// <summary>One button, one slot, two roles: RECYCLER sees "Continue to
        /// disassembly" (grey + inert until <see cref="GateOpen"/>, then green);
        /// PRODUCT USER sees a plain "Back" to the stakeholder fork — they never
        /// enter the teardown, so there is nothing to gate.</summary>
        public void OnRailCta()
        {
            if (!Walkthrough)
            {
                if (router != null) router.ShowStakeholder();
                else Debug.LogWarning("[SuperPanel] No router — cannot return to the stakeholder screen.");
                return;
            }
            if (!GateOpen)
            {
                // A dead-feeling button is a bug report waiting to happen: the
                // locked press answers ON the pill, naming what is missing.
                if (_ctaHint != null) StopCoroutine(_ctaHint);
                _ctaHint = StartCoroutine(CtaHint());
                return;
            }
            if (router != null) router.ShowDisassembly();
            else Debug.LogWarning("[SuperPanel] No router — cannot start the disassembly.");
        }

        private System.Collections.IEnumerator CtaHint()
        {
            if (railCtaLabel != null)
                railCtaLabel.text = "Visit every tab first";
            yield return new WaitForSeconds(1.8f);
            _ctaHint = null;
            PaintRailCta();
        }

        private void PaintRailCta()
        {
            if (railCtaFill == null && railCtaLabel == null) return;
            if (_ctaHint != null) { StopCoroutine(_ctaHint); _ctaHint = null; }

            Color fill, label;
            string text;
            bool bold;
            if (!Walkthrough)
            {
                text = "Back"; fill = tabRestFill; label = textSecondary; bold = false;
            }
            else if (GateOpen)
            {
                text = "Continue to disassembly";
                fill = new Color32(0x27, 0xC4, 0x6C, 0xFF);    // green — same as FREE
                label = new Color32(0x08, 0x33, 0x1C, 0xFF);
                bold = true;
            }
            else
            {
                text = "Continue to disassembly";
                fill = new Color32(0x2B, 0x3A, 0x52, 0xFF);    // grey — visibly not ready
                label = new Color32(0x7C, 0x8B, 0xA1, 0xFF);
                bold = false;
            }

            if (railCtaFill != null) railCtaFill.color = fill;
            // Trap 1: the pill is hover-brightened, so the state colour must go
            // through SetRestFillColor or the next hover ease repaints the old one.
            if (_ctaHover == null && railCtaFill != null)
                _ctaHover = railCtaFill.GetComponentInParent<HoverHighlight>();
            if (_ctaHover != null) _ctaHover.SetRestFillColor(fill);
            if (railCtaLabel != null)
            {
                railCtaLabel.text = text;
                railCtaLabel.color = label;
                railCtaLabel.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
            }
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
            bool built = tabPages != null && index < tabPages.Length && tabPages[index] != null;
            for (int i = 0; tabPages != null && i < tabPages.Length; i++)
                if (tabPages[i] != null) tabPages[i].SetActive(i == index && built);

            bool showPlaceholder = !built;
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
                default: return "Certificates & safety";
            }
        }

        // ---- targets the rail buttons are wired to ----
        public void SelectTab0() => SelectTab(0);
        public void SelectTab1() => SelectTab(1);
        public void SelectTab2() => SelectTab(2);
        public void SelectTab3() => SelectTab(3);

        /// <summary>Called by a tab page's primary CTA. Recycler: the next tab —
        /// the certificates tab is the end of the chain, and the teardown is ONLY
        /// reachable through the rail gate. Product user: the next unit.</summary>
        public void NextTab()
        {
            if (!Walkthrough)
            {
                if (scanner != null) scanner.BeginNewScan();
                else Debug.LogWarning("[SuperPanel] No QRScanController — cannot start a new scan.");
                return;
            }
            if (_active >= TabCount - 1) return;   // last tab: the rail CTA is the only door on
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

        /// <summary>True on the last tab (certificates). Its page has no primary
        /// button — the rail gate is the only way forward.</summary>
        public bool IsLastTab => _active >= TabCount - 1;

        /// <summary>04e round 2 (Thiago): every page primary reads "Next" — the
        /// certificates tab is a normal step in the chain now, and the teardown
        /// CTA lives on the rail, not on a page.</summary>
        public string PrimaryLabel => !Walkthrough ? "Scan next product" : "Next";

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

        /// <summary>
        /// Round 5 — FREE is a SEQUENCE, not a switch (Thiago, 2026-08-08):
        /// the lock turns green at once, the spin stops, the model eases upright
        /// (25°/205° → 0°/205°) while the parts REASSEMBLE — the user then floats
        /// the closed, real-size unit, as if holding the physical one — the column
        /// extends to show the gesture readouts, and only THEN the grab bar
        /// appears. Gestures (twist/zoom) enable at the end, with the drag.
        /// </summary>
        private void Unlock()
        {
            if (freeModelRoot == null)
            {
                Debug.LogWarning("[SuperPanel] No freeModelRoot — the model cannot leave the stage.");
                return;
            }
            _unlocked = true;   // also stops the idle spin — Update checks it
            _snapT = -1f;

            // Re-parent WORLD-POSE-PRESERVING so the release is invisible: the
            // model must not jump at the moment it is freed. The stage's yaw is
            // 0° precisely so this holds (spec §2.1).
            freeModelRoot.SetPositionAndRotation(model.position, model.rotation);
            model.SetParent(freeModelRoot, worldPositionStays: true);

            if (ghostOutline != null) ghostOutline.SetActive(true);
            // FREE cuts the link BOTH ways — no highlight follows the data canvas and
            // no pinch on a body navigates (Thiago, 2026-08-07). Also undims.
            if (modelLink != null) modelLink.SetLinked(false);
            Paint(false);   // green immediately — the click must answer instantly

            if (_freeSeq != null) StopCoroutine(_freeSeq);
            _freeSeq = StartCoroutine(FreeSequence());
        }

        private System.Collections.IEnumerator FreeSequence()
        {
            // 1 + 2 in parallel: ease upright while the parts come home.
            if (modelLink != null) modelLink.PlayReassemble();
            float reassemble = modelLink != null ? modelLink.ReassembleSeconds : 0f;

            Quaternion from = model.rotation;
            Quaternion to = Quaternion.Euler(freeEuler);
            float t = 0f;
            while (t < uprightSeconds)
            {
                t += Time.deltaTime;
                float e = 1f - (1f - Mathf.Clamp01(t / uprightSeconds)) * (1f - Mathf.Clamp01(t / uprightSeconds));
                model.rotation = Quaternion.Slerp(from, to, e);
                yield return null;
            }
            model.rotation = to;

            // Wait out whatever the reassembly still owes.
            float remaining = reassemble - uprightSeconds;
            if (remaining > 0f) yield return new WaitForSeconds(remaining);

            // 3. The column extends: lock stays, the readout rows fade in below.
            if (hudExtras != null) hudExtras.SetActive(true);
            if (hudBackplate != null)
            {
                float h0 = hudBackplate.sizeDelta.y;
                t = 0f;
                while (t < hudExpandSeconds)
                {
                    t += Time.deltaTime;
                    float h = Mathf.Lerp(h0, hudExpandedHeight, Mathf.Clamp01(t / hudExpandSeconds));
                    hudBackplate.sizeDelta = new Vector2(hudBackplate.sizeDelta.x, h);
                    yield return null;
                }
                hudBackplate.sizeDelta = new Vector2(hudBackplate.sizeDelta.x, hudExpandedHeight);
            }

            // 4. Only now: the drag affordance, the follower, and the gestures.
            if (freeModelGrabber != null) freeModelGrabber.SetActive(true);
            if (hudFollower != null) hudFollower.SetFree(true);
            if (stageGestures != null) stageGestures.enabled = true;
            _freeSeq = null;
        }

        private void ReLock(bool instant)
        {
            _unlocked = false;
            if (model == null) return;

            // Round 5: tear the FREE state down in reverse — kill a half-finished
            // free sequence, silence the gestures, collapse the column, hide the
            // drag affordance. The snap home + instant re-explode below restate
            // scenario 1, and the idle spin resumes on its own (OpenDone).
            if (_freeSeq != null) { StopCoroutine(_freeSeq); _freeSeq = null; }
            if (stageGestures != null) stageGestures.enabled = false;
            if (hudFollower != null) hudFollower.SetFree(false);
            if (hudExtras != null) hudExtras.SetActive(false);
            if (hudBackplate != null)
                hudBackplate.sizeDelta = new Vector2(hudBackplate.sizeDelta.x, hudCollapsedHeight);

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
            // Re-linking RESTATES the model: snapped home above, re-exploded here, and
            // re-selected to whatever the data canvas is currently showing. LINKED then
            // always means the same thing, however the user left it.
            if (modelLink != null) modelLink.SetLinked(true);
            // Sync the gesture's internal zoom to the restored scale. The snap lerp
            // owns the visible pose; without this the NEXT zoom gesture started
            // from the stale zoom value and the model jumped a frame (see
            // TwoHandTwistRotate.ResetPose).
            if (stageGestures != null) stageGestures.ResetPose();
            Paint(true);
        }

        /// <summary>
        /// ⚠ THE WORDS ARE "LINKED" AND "FREE", not LOCKED/UNLOCKED (Thiago,
        /// 2026-08-07). Once LOCKED came to mean "exploded, tappable, driving the
        /// passport", the old label said the opposite of what the state does, and a
        /// participant who reads LOCK as "nothing works here" never tries the one
        /// interaction the state exists for. The field names keep the old spelling so
        /// the scene wiring survives; only the words the user reads changed.
        /// </summary>
        private void Paint(bool locked)
        {
            // Round 6 (Thiago, 2026-08-09): the padlock artwork itself carries the
            // state — orange closed lock for LINKED, green open lock for FREE
            // (Thiago's own icons, white keyhole). The glyph is NEVER tinted; only
            // the word underneath takes the state colour.
            Color state = locked ? new Color32(0xF2, 0x8C, 0x28, 0xFF)    // orange
                                 : new Color32(0x27, 0xC4, 0x6C, 0xFF);   // green
            if (lockLabel != null)
            {
                lockLabel.text = locked ? "LINKED" : "FREE";
                lockLabel.color = state;
            }
            if (lockGlyph != null)
            {
                var s = locked ? lockedSprite : unlockedSprite;
                if (s != null) lockGlyph.sprite = s;
                lockGlyph.color = Color.white;   // the sprite is pre-coloured
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
