using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// RBv2.1.1 — spec 10: THE GUIDED DISASSEMBLY MODE OF THE SUPER PANEL
    /// (mock `drafts/10_v1_disassembly_mode.svg`, approved 2026-08-09).
    ///
    /// One rig, two faces. The rail-gate CTA no longer routes to the RB2_0 flat
    /// screens — it swaps the SAME rig's rail and data content in place:
    ///
    ///   RAIL   7 entries — Intro · steps 1–5 · Summary (sequential unlock)
    ///   STAGE  the same clone, driven to the CURRENT PHYSICAL STATE of the run:
    ///          parts removed in completed steps are HIDDEN, the current step's
    ///          parts keep their true materials, everything else ghosts
    ///   DATA   briefing → per-step task pages → per-step summary table
    ///
    /// Approved rulings baked in here:
    ///   · Sequential unlock; Back surfs already-completed steps; on step 1 the
    ///     Back slot reads "Quit" → confirm modal → Intro, run + timer cancelled.
    ///   · Once the Summary is reached the run is FINAL — the rail locks backward.
    ///   · The timer runs from Start and is NEVER shown live (spec 09: time is an
    ///     achievement, not pressure; a live clock would contaminate the study).
    ///     Total + per-step splits appear only on the Summary.
    ///   · No idle spin during steps (the participant is matching the model to
    ///     the unit in their hands); the showcase spin bookends the flow on the
    ///     Intro and the Summary.
    ///   · LINKED only while a step is showing: the padlock column hides during
    ///     steps 1–5 (FREE would REASSEMBLE the model — the exact opposite of a
    ///     half-dismantled unit) and returns on Intro/Summary.
    ///   · Model picks are INERT for the whole mode (ModelLinkController.SetGuided).
    ///   · Completing a step plays that step's removal via DisassemblyAnimator —
    ///     never a bespoke animation — then hides the removed parts.
    ///
    /// Built and wired by RBv2_1_1/14. The RB2_0 flat screens (menu 05/06/07)
    /// stay in the scene as the rollback path until the retirement pass.
    /// </summary>
    public class DisassemblyModeController : MonoBehaviour
    {
        private const int StepCount = 5;
        private const int EntryCount = StepCount + 2;   // intro + 5 steps + summary

        private enum Phase { Off, Intro, Step, Summary }

        // =================================================================
        // Wiring (set by RBv2_1_1/14)
        // =================================================================
        [Header("Wiring")]
        [SerializeField] private SuperPanelView owner;
        [SerializeField] private ModelLinkController modelLink;
        [Tooltip("The STAGE CLONE's animator — never the original VCU_assembly (menu 05/06 own that one).")]
        [SerializeField] private DPP.DisassemblyAnimator stageAnimator;

        [Header("Rail — the 7 guided entries")]
        [SerializeField] private GameObject railGroup;
        [SerializeField] private Image[] entryFills;
        [SerializeField] private Image[] entryStrokes;
        [SerializeField] private Image[] entryAccents;
        [SerializeField] private GameObject[] entryTicks;
        [SerializeField] private TMP_Text[] entryLabels;
        [SerializeField] private Image[] entryDiscs;
        [SerializeField] private TMP_Text[] entryDiscLabels;
        [SerializeField] private Button[] entryButtons;

        [Header("Pages (data canvas)")]
        [SerializeField] private GameObject introPage;
        [SerializeField] private GameObject stepPage;
        [SerializeField] private GameObject summaryPage;

        [Header("Intro bindings")]
        [SerializeField] private TMP_Text introTools;
        [SerializeField] private TMP_Text introTime;
        [SerializeField] private TMP_Text introScope;
        [SerializeField] private GameObject[] introPartRows;
        [SerializeField] private TMP_Text[] introPartLabels;

        [Header("Step page bindings")]
        [SerializeField] private TMP_Text stepTitle;
        [SerializeField] private TMP_Text stepCaption;
        [SerializeField] private GameObject stepToolRow;
        [SerializeField] private TMP_Text stepTool;
        [SerializeField] private Image task1Fill;
        [SerializeField] private GameObject task1Cross;
        [SerializeField] private GameObject task1Check;
        [SerializeField] private TMP_Text task1Title;
        [SerializeField] private TMP_Text task1Subtitle;
        [SerializeField] private Button task1Button;
        [SerializeField] private Image task2Fill;
        [SerializeField] private GameObject task2Cross;
        [SerializeField] private GameObject task2Check;
        [SerializeField] private TMP_Text task2Title;
        [SerializeField] private TMP_Text task2Subtitle;
        [SerializeField] private Button task2Button;
        [SerializeField] private TMP_Text gateHint;
        [SerializeField] private Button backButton;
        [SerializeField] private TMP_Text backLabel;
        [SerializeField] private Button nextButton;
        [SerializeField] private Image nextFill;
        [SerializeField] private TMP_Text nextLabel;

        [Header("Quit modal (step 1's Back)")]
        [SerializeField] private GameObject quitModal;

        [Header("Summary")]
        [Tooltip("A SECOND CompletionSummaryView instance on the 420-wide summary page. All the " +
                 "table/report logic is reused; only the exits are re-wired through this controller " +
                 "so leaving the summary restores the passport chrome.")]
        [SerializeField] private CompletionSummaryView summaryView;

        // =================================================================
        // Colours — same status language as the RB2_0 step flow
        // =================================================================
        private static readonly Color PendingRed  = DPPTheme.Hex("#e24b4a");
        private static readonly Color DoneGreen   = DPPTheme.TealAccent;
        private static readonly Color LockedText  = DPPTheme.Hex("#5d7396");
        private static readonly Color ActiveFill  = DPPTheme.Hex("#0d2a57");
        private static readonly Color ActiveStroke= DPPTheme.Hex("#2e5aa0");
        private static readonly Color RestFill    = DPPTheme.Hex("#0e2950");
        private static readonly Color RestStroke  = DPPTheme.Hex("#21407a");
        private static readonly Color LockedFill  = DPPTheme.Hex("#0a1f44");
        private static readonly Color LockedStroke= DPPTheme.Hex("#14284a");
        private const float DimAlpha = 0.38f;      // same as the passport rail's dimmedAlpha

        // =================================================================
        // Run state
        // =================================================================
        private Phase _phase = Phase.Off;
        private int _view = 1;                                   // 1-based step being shown
        private readonly bool[] _stepDone = new bool[StepCount];
        private readonly bool[] _taskDone = new bool[2];         // frontier step's transient tasks
        private readonly List<int> _splits = new List<int>();
        private float _flowStart, _stepStart;
        private int _returnTab;
        private bool _transition;                                // Start's reassemble running
        private Coroutine _entryAnim;                            // step-entry explode (round 2)
        private DPPData _data;

        // Trap 1: every state colour goes through HoverHighlight.SetRestFillColor.
        // The TASK CIRCLES are exempt on purpose: the row is the button (chrome =
        // touchable — a boxed row must be pressable), its HoverHighlight resolves
        // the ROW's own "Fill" by name, and the circle is a plain status light
        // ("CircleFill") no hover ever repaints.
        private HoverHighlight[] _entryHovers;
        private HoverHighlight _nextHover;
        private bool _hoversResolved;

        /// <summary>First step (1-based) that is not completed yet — the only step
        /// whose tasks are live. 6 once all five are done.</summary>
        private int Frontier
        {
            get
            {
                for (int i = 0; i < StepCount; i++) if (!_stepDone[i]) return i + 1;
                return StepCount + 1;
            }
        }

        // =================================================================
        // Lifecycle
        // =================================================================

        /// <summary>The rig was hidden mid-run (scan, welcome, …). A guided run
        /// cannot survive that: hidden model parts and the suppressed link would
        /// leak into the next passport visit. Restore the model wholesale here;
        /// SuperPanelView.OnEnable restores its own chrome on the way back in.</summary>
        private void OnDisable()
        {
            if (_phase == Phase.Off) return;
            _phase = Phase.Off;
            if (railGroup != null) railGroup.SetActive(false);
            HideAllPages();
            if (stageAnimator != null) stageAnimator.ClearGuidedState();
            if (modelLink != null) modelLink.SetGuided(false);
        }

        // =================================================================
        // Mode entry / exit
        // =================================================================

        /// <summary>Called by SuperPanelView.OnRailCta when the gate is open.</summary>
        public void EnterMode()
        {
            if (owner == null || railGroup == null || introPage == null)
            {
                Debug.LogWarning("[DisassemblyMode] Not fully built — run RBv2_1_1/14. Falling back is the caller's job.");
                return;
            }
            _returnTab = owner.ActiveTab;
            _data = ResolveData();

            owner.ForceRelock();
            if (modelLink != null)
            {
                // Clear the passport's selection/lens state BEFORE standing the
                // link down, so no ghost or tint survives into the guided view.
                modelLink.ClearSelection();
                modelLink.ClearLensTint();
                modelLink.SetGuided(true);
            }
            owner.SetGuidedChrome(true);
            railGroup.SetActive(true);

            if (summaryView != null && _data != null) summaryView.Populate(_data);

            ResetRun();
            ShowIntro();
            Debug.Log("[DisassemblyMode] Entered guided disassembly (return tab " + _returnTab + ").");
        }

        private void ExitMode()
        {
            _phase = Phase.Off;
            HideAllPages();
            if (quitModal != null) quitModal.SetActive(false);
            if (railGroup != null) railGroup.SetActive(false);

            // Restate the LINKED showcase exactly as the passport left it: every
            // part visible, open pose, selection re-applied, picks live again.
            if (stageAnimator != null) stageAnimator.ClearGuidedState();
            if (modelLink != null)
            {
                modelLink.SetGuided(false);
                modelLink.SetLinked(true);
            }
            owner.ShowGestureColumn(true);
            owner.SetGuidedChrome(false);   // repaints the rail and reopens tab _returnTab's page
            Debug.Log("[DisassemblyMode] Exited to the passport.");
        }

        private void ResetRun()
        {
            for (int i = 0; i < StepCount; i++) _stepDone[i] = false;
            _taskDone[0] = _taskDone[1] = false;
            _splits.Clear();
            _flowStart = _stepStart = 0f;
            _transition = false;
        }

        // =================================================================
        // Phases
        // =================================================================

        private void ShowIntro()
        {
            _phase = Phase.Intro;
            ShowPage(introPage);

            // The showcase bookend: full model, open pose, idle spin, padlock back.
            if (stageAnimator != null) stageAnimator.ApplyOpenInstant();
            owner.ShowGestureColumn(true);
            owner.SetGuidedSpin(true);

            BindIntro();
            PaintRail();
        }

        private void GoToStep(int step)
        {
            if (_entryAnim != null) { StopCoroutine(_entryAnim); _entryAnim = null; }
            _phase = Phase.Step;
            _view = Mathf.Clamp(step, 1, StepCount);
            ShowPage(stepPage);
            if (quitModal != null) quitModal.SetActive(false);

            // LINKED only, no spin, no padlock while a step is showing.
            owner.ForceRelock();
            owner.SetGuidedSpin(false);
            owner.ShowGestureColumn(false);

            // Device rounds 2–3: the step's parts EXPLODE OUT as the page opens —
            // the guidance leads, the participant follows — and they stay out
            // through the whole step (round 3: hiding them at the second tick
            // yanked the reference away mid-work). They disappear HERE, on the
            // jump to the next step: SetGuidedStepState hides the removals of
            // every earlier step. A revisited (completed) step shows the state
            // AFTER it instead: its parts are already off the physical unit,
            // and no animation replays.
            bool completed = _stepDone[_view - 1];
            if (stageAnimator != null)
            {
                stageAnimator.SetGuidedStepState(_view, completed);
                if (!completed) _entryAnim = StartCoroutine(EntryAnimRoutine(_view));
            }

            BindStep(_view);
            PaintRail();
        }

        /// <summary>The step's removal plays as the page opens. Task ticking is
        /// deliberately NOT blocked while it runs — the participant works on the
        /// real unit; navigation is safe because GoToStep stops this coroutine
        /// and SetGuidedStepState kills the tweens.</summary>
        private IEnumerator EntryAnimRoutine(int step)
        {
            yield return stageAnimator.RunStep(step);
            _entryAnim = null;
        }

        private void ShowSummary()
        {
            _phase = Phase.Summary;
            ShowPage(summaryPage);

            // Bookend: everything visible again, exploded, spinning, padlock back.
            if (stageAnimator != null) stageAnimator.ApplyOpenInstant();
            owner.ShowGestureColumn(true);
            owner.SetGuidedSpin(true);

            int elapsed = Mathf.RoundToInt(Time.realtimeSinceStartup - _flowStart);
            if (summaryView != null)
            {
                if (_data != null) summaryView.Populate(_data);
                summaryView.SetSession(elapsed, StepCount, TotalSteps(), _splits.ToArray());
            }
            PaintRail();
        }

        private void ShowPage(GameObject page)
        {
            if (introPage != null) introPage.SetActive(page == introPage);
            if (stepPage != null) stepPage.SetActive(page == stepPage);
            if (summaryPage != null) summaryPage.SetActive(page == summaryPage);
        }

        private void HideAllPages() => ShowPage(null);

        // =================================================================
        // Button targets (wired by RBv2_1_1/14)
        // =================================================================

        /// <summary>Intro's Back — leave the mode, back to the passport tabs.</summary>
        public void OnIntroBack()
        {
            if (_phase != Phase.Intro) return;
            ExitMode();
        }

        /// <summary>Intro's primary — the run and its timer start HERE, not on
        /// mode entry: reading the briefing is not dismantling.</summary>
        public void OnIntroStart()
        {
            if (_phase != Phase.Intro || _transition) return;
            _flowStart = _stepStart = Time.realtimeSinceStartup;
            StartCoroutine(StartRunRoutine());
        }

        /// <summary>
        /// Device round 2, feedback 1: Start used to freeze the showcase at a
        /// random spin yaw and snap-cut to the assembled pose. The run now OPENS
        /// with a transition the participant can read: the pivot eases back to
        /// its home yaw while the parts REASSEMBLE from the exploded view — the
        /// model becomes the closed unit on the desk — and only then step 1
        /// plays its own entry explode (feedback 2).
        /// </summary>
        private IEnumerator StartRunRoutine()
        {
            _phase = Phase.Step;      // locks out a double Start and Intro's Back
            _view = 1;
            _transition = true;
            owner.ForceRelock();
            owner.SetGuidedSpin(false);
            owner.ShowGestureColumn(false);
            owner.SnapModelHome();    // pivot yaw home while the parts travel
            if (stageAnimator != null)
            {
                stageAnimator.StopAllCoroutines();
                stageAnimator.Reassemble();
                yield return new WaitForSeconds(stageAnimator.ReassembleDuration + 0.1f);
            }
            _transition = false;
            GoToStep(1);
        }

        public void ToggleTask1() => ToggleTask(0);
        public void ToggleTask2() => ToggleTask(1);

        private void ToggleTask(int i)
        {
            // Task circles are live on the FRONTIER step only — a completed
            // step's record is history, not an editable checklist.
            if (_phase != Phase.Step || _transition || _stepDone[_view - 1]) return;
            _taskDone[i] = !_taskDone[i];
            // Round 3 (Thiago): ticking does NOT touch the model any more. The
            // exploded parts stay out until the user advances — GoToStep(n+1)'s
            // SetGuidedStepState hides them as part of the next step's state
            // (round 2 hid them at the second tick, which yanked the reference
            // away while the participant was still mid-comparison).
            PaintTasks();
            PaintNext();
        }

        /// <summary>Back: step 1 → quit modal; otherwise one step back (revisit).</summary>
        public void OnBack()
        {
            if (_phase != Phase.Step || _transition) return;
            if (_view <= 1)
            {
                if (quitModal != null) quitModal.SetActive(true);
                else OnQuitYes();   // modal not built — degrade to the ruling's outcome
                return;
            }
            GoToStep(_view - 1);
        }

        /// <summary>Next: on the frontier it completes the step (both tasks
        /// required — a locked press hints); on a revisited step it just walks
        /// forward again.</summary>
        public void OnNext()
        {
            if (_phase != Phase.Step || _transition) return;

            if (_stepDone[_view - 1])
            {
                if (_view < StepCount) GoToStep(_view + 1);
                return;   // view==5 done can't happen: completing 5 goes to Summary
            }

            if (!(_taskDone[0] && _taskDone[1]))
            {
                if (gateHint != null) StartCoroutine(GateHintRoutine());
                return;
            }

            // Frontier completion — instant advance: the removal already played
            // on entry and the parts vanished at the second tick (round 2; the
            // round-1 flow animated HERE, which the participant read as the app
            // lagging behind work they had already finished).
            float now = Time.realtimeSinceStartup;
            _splits.Add(Mathf.RoundToInt(now - _stepStart));
            _stepStart = now;
            _stepDone[_view - 1] = true;
            if (_view >= StepCount) ShowSummary();
            else GoToStep(_view + 1);
        }

        private IEnumerator GateHintRoutine()
        {
            gateHint.text = "Tick both tasks first";
            gateHint.color = PendingRed;
            yield return new WaitForSeconds(1.8f);
            if (gateHint != null)
            {
                gateHint.text = GateHintRest;
                gateHint.color = DPPTheme.TextTip;
            }
        }

        private const string GateHintRest = "Next unlocks when both tasks are ticked";

        // (CompleteRoutine + SetNavInteractable died in round 2: completion no
        // longer animates, so there is no window to lock the buttons over.)

        public void OnQuitYes()
        {
            // The ruling: Quit returns to the INTRO, cancelling the timestamp and
            // the run. The mode itself stays — Intro's Back is the way out.
            if (quitModal != null) quitModal.SetActive(false);
            ResetRun();
            ShowIntro();
        }

        public void OnQuitNo()
        {
            if (quitModal != null) quitModal.SetActive(false);
        }

        // ---- rail entries ----
        public void OnEntry0() => OnEntry(0);
        public void OnEntry1() => OnEntry(1);
        public void OnEntry2() => OnEntry(2);
        public void OnEntry3() => OnEntry(3);
        public void OnEntry4() => OnEntry(4);
        public void OnEntry5() => OnEntry(5);
        public void OnEntry6() => OnEntry(6);

        private void OnEntry(int index)
        {
            if (_transition) return;
            // Mid-run the intro entry is LOCKED: leaving a timed run is an
            // explicit decision (step 1's Quit + modal), never a stray rail tap.
            if (index == 0 || index == EntryCount - 1) return;   // intro / summary never navigate directly
            if (_phase != Phase.Step) return;
            int step = index;                                    // entries 1..5 are steps 1..5
            if (step > Mathf.Min(Frontier, StepCount)) return;   // sequential unlock
            if (step == _view) return;
            GoToStep(step);
        }

        // ---- summary ----

        /// <summary>The summary's single action: Send report → Done. Send is the
        /// reused CompletionSummaryView logic; Done leaves the mode (the reused
        /// view's own Done would route the router at a screen that is already
        /// showing and strand the rig in guided chrome).</summary>
        public void OnSummaryAction()
        {
            if (_phase != Phase.Summary) return;
            if (summaryView == null) { ExitMode(); return; }
            if (summaryView.Sent) ExitMode();
            else summaryView.OnActionButton();
        }

        /// <summary>
        /// Post-report modal QUIT (grey) — device round 2, feedback 3: the run is
        /// over, so the session ends at the Welcome page, not back in the
        /// passport. The rig is hidden EXPLICITLY: WelcomeController.ShowWelcome
        /// only hides the flat 640 panel canvas — the rig is a scene root it has
        /// never heard of, and skipping this line leaves the whole Super Panel
        /// floating behind the Welcome screen.
        /// </summary>
        public void OnSummaryQuit()
        {
            if (_phase != Phase.Summary) return;
            var welcome = FindFirstObjectByType<WelcomeController>(FindObjectsInactive.Include);
            ExitMode();   // restore chrome + model first, so the next rig entry is clean
            if (owner != null) owner.gameObject.SetActive(false);          // the rig root
            var freeRoot = GameObject.Find("DppFreeModel");
            if (freeRoot != null) freeRoot.SetActive(false);
            if (welcome != null) welcome.ShowWelcome();
            else Debug.LogWarning("[DisassemblyMode] No WelcomeController — the rig is hidden but no screen took over.");
        }

        /// <summary>Post-report modal CLOSE APP (solid red — 00 §2.1 meaning 3:
        /// "this action ends the session", the one sanctioned red button use).
        /// Routed through the Welcome controller so the app has ONE quit path.</summary>
        public void OnSummaryCloseApp()
        {
            var welcome = FindFirstObjectByType<WelcomeController>(FindObjectsInactive.Include);
            if (welcome != null) { welcome.CloseApp(); return; }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>Round-1 modal targets, superseded 2026-08-10 by Quit/Close app.
        /// Kept so a scene whose /14 has not been re-run yet degrades to the old
        /// behaviour instead of logging missing-method errors (project lore).</summary>
        public void OnSummaryScanNew()
        {
            if (_phase != Phase.Summary) return;
            var scanner = FindFirstObjectByType<QRScanController>(FindObjectsInactive.Include);
            ExitMode();
            if (scanner != null) scanner.BeginNewScan();
            else Debug.LogWarning("[DisassemblyMode] No QRScanController — staying on the passport.");
        }

        public void OnSummaryDone()
        {
            if (_phase != Phase.Summary) return;
            ExitMode();
        }

        // =================================================================
        // Data binding
        // =================================================================

        private DPPData ResolveData()
        {
            var mgr = FindFirstObjectByType<DPP.DPPManager>(FindObjectsInactive.Include);
            var data = mgr != null ? mgr.Latest : null;
            if (data == null)
                Debug.LogWarning("[DisassemblyMode] No payload cached — pages show builder-baked demo content.");
            return data;
        }

        private int TotalSteps()
        {
            var steps = _data?.disassembly?.steps;
            int n = _data?.disassembly?.total_steps ?? 0;
            if (n <= 0) n = steps != null && steps.Count > 0 ? steps.Count : StepCount;
            if (n != StepCount)
                Debug.LogWarning($"[DisassemblyMode] Payload declares {n} steps but the rail is built for {StepCount}.");
            return Mathf.Min(n, StepCount);
        }

        private void BindIntro()
        {
            var d = _data?.disassembly;
            if (d == null) return;   // baked demo stays

            if (introTools != null && d.tools != null && d.tools.Count > 0)
                introTools.text = string.Join(" · ", d.tools);
            if (introTime != null && d.estimated_time_min > 0)
                introTime.text = $"~{d.estimated_time_min} min";
            if (introScope != null && d.total_steps > 0)
                introScope.text = $"{d.total_steps} steps";

            if (introPartLabels != null && d.parts != null)
                for (int i = 0; i < introPartLabels.Length; i++)
                {
                    bool has = i < d.parts.Count;
                    if (introPartRows != null && i < introPartRows.Length && introPartRows[i] != null)
                        introPartRows[i].SetActive(has);
                    if (has && introPartLabels[i] != null) introPartLabels[i].text = d.parts[i];
                }
        }

        private void BindStep(int step)
        {
            ResolveHovers();
            var steps = _data?.disassembly?.steps;
            Step s = steps != null && step - 1 < steps.Count ? steps[step - 1] : null;

            if (stepTitle != null && s != null) stepTitle.text = s.title;
            if (stepCaption != null) stepCaption.text = $"Step {step} of {TotalSteps()}";

            bool hasTool = s != null && !string.IsNullOrEmpty(s.tool);
            if (stepToolRow != null) stepToolRow.SetActive(hasTool || s == null);
            if (stepTool != null && hasTool) stepTool.text = s.tool;

            BindAction(s, 0, task1Title, task1Subtitle);
            BindAction(s, 1, task2Title, task2Subtitle);

            // Frontier: tasks reset and live. Completed: shown done, read-only.
            bool done = _stepDone[step - 1];
            _taskDone[0] = _taskDone[1] = done;
            if (task1Button != null) task1Button.interactable = !done;
            if (task2Button != null) task2Button.interactable = !done;

            if (backLabel != null) backLabel.text = step <= 1 ? "Quit" : "Back";
            if (gateHint != null)
            {
                gateHint.gameObject.SetActive(!done);
                gateHint.text = GateHintRest;
                gateHint.color = DPPTheme.TextTip;
            }

            PaintTasks();
            PaintNext();
        }

        private static void BindAction(Step s, int i, TMP_Text title, TMP_Text subtitle)
        {
            if (s?.actions == null || i >= s.actions.Count) return;
            StepAction a = s.actions[i];
            if (title != null) title.text = a.title;
            if (subtitle != null)
            {
                subtitle.text = a.subtitle ?? "";
                // Gold = value-bearing subtitle (00 §2), same rule as the RB2_0 flow.
                subtitle.color = a.value ? DPPTheme.Hex("#f0c879") : DPPTheme.TextSecondary;
            }
        }

        // =================================================================
        // Painting
        // =================================================================

        private void ResolveHovers()
        {
            if (_hoversResolved) return;
            _hoversResolved = true;
            if (nextFill != null) _nextHover = nextFill.GetComponentInParent<HoverHighlight>();
            _entryHovers = new HoverHighlight[EntryCount];
            for (int i = 0; i < EntryCount; i++)
                if (entryFills != null && i < entryFills.Length && entryFills[i] != null)
                    _entryHovers[i] = entryFills[i].GetComponentInParent<HoverHighlight>();
        }

        private void PaintTasks()
        {
            PaintTask(_taskDone[0], task1Fill, task1Cross, task1Check);
            PaintTask(_taskDone[1], task2Fill, task2Cross, task2Check);
        }

        /// <summary>The circle is named "CircleFill", not "Fill" — the row's
        /// HoverHighlight can therefore never recapture or repaint it (trap 1
        /// avoided structurally rather than worked around).</summary>
        private static void PaintTask(bool done, Image fill, GameObject cross, GameObject check)
        {
            Color c = done ? DoneGreen : PendingRed;
            if (fill != null) fill.color = c;
            if (cross != null) cross.SetActive(!done);
            if (check != null) check.SetActive(done);
        }

        private bool NextIsOpen() => _phase == Phase.Step &&
            (_stepDone[_view - 1] || (_taskDone[0] && _taskDone[1]));

        private void PaintNext()
        {
            bool open = NextIsOpen();
            Color fill = open ? DPPTheme.TealAccent : DPPTheme.SecondaryButtonFill;
            // The locked Next stays PRESSABLE (04e rule: a locked press is never
            // silent — OnNext answers with the gate hint). Only a running removal
            // animation actually refuses input.
            if (nextButton != null) nextButton.interactable = !_transition;
            if (nextFill != null) nextFill.color = fill;
            if (_nextHover != null)
            {
                _nextHover.SetRestFillColor(fill);
                _nextHover.enabled = open;
            }
            if (nextLabel != null)
            {
                nextLabel.text = "Next";
                nextLabel.color = open ? DPPTheme.TextOnNavy : LockedText;
            }
        }

        /// <summary>Entry states: active (accent bar + active fill), completed
        /// (tick, revisitable), locked (dimmed + refuses the click — dimming alone
        /// would be a lie). At the Summary everything locks backward: the run is
        /// final by ruling.</summary>
        private void PaintRail()
        {
            ResolveHovers();
            int frontier = Frontier;

            for (int i = 0; i < EntryCount; i++)
            {
                bool active =
                    (_phase == Phase.Intro   && i == 0) ||
                    (_phase == Phase.Step    && i == _view) ||
                    (_phase == Phase.Summary && i == EntryCount - 1);

                bool ticked =
                    (i >= 1 && i <= StepCount && _stepDone[i - 1] && !active) ||
                    (i == 0 && _phase != Phase.Intro);           // briefing read

                bool reachable;
                if (_phase == Phase.Summary) reachable = active; // locked backward
                else if (_phase == Phase.Intro) reachable = i == 0;
                else reachable = i >= 1 && i <= StepCount && i <= Mathf.Min(frontier, StepCount);

                Color fill   = active ? ActiveFill   : reachable || ticked ? RestFill   : LockedFill;
                Color stroke = active ? ActiveStroke : reachable || ticked ? RestStroke : LockedStroke;
                Color label  = active ? Color.white  : DPPTheme.TextSecondary;
                if (!reachable && !ticked) label.a *= DimAlpha;

                if (entryFills != null && i < entryFills.Length && entryFills[i] != null)
                    entryFills[i].color = fill;
                if (_entryHovers != null && i < _entryHovers.Length && _entryHovers[i] != null)
                    _entryHovers[i].SetRestFillColor(fill);
                if (entryStrokes != null && i < entryStrokes.Length && entryStrokes[i] != null)
                    entryStrokes[i].color = stroke;
                if (entryAccents != null && i < entryAccents.Length && entryAccents[i] != null)
                    entryAccents[i].gameObject.SetActive(active);
                if (entryTicks != null && i < entryTicks.Length && entryTicks[i] != null)
                    entryTicks[i].SetActive(ticked);
                if (entryLabels != null && i < entryLabels.Length && entryLabels[i] != null)
                {
                    entryLabels[i].color = label;
                    entryLabels[i].fontStyle = active ? FontStyles.Bold : FontStyles.Normal;
                }
                if (entryDiscs != null && i < entryDiscs.Length && entryDiscs[i] != null)
                {
                    Color disc = active ? DoneGreen : DPPTheme.CardBlue;
                    if (!reachable && !ticked && !active) disc.a *= DimAlpha;
                    entryDiscs[i].color = disc;
                }
                if (entryDiscLabels != null && i < entryDiscLabels.Length && entryDiscLabels[i] != null)
                {
                    Color d = active ? Color.white : DPPTheme.TextSecondary;
                    if (!reachable && !ticked) d.a *= DimAlpha;
                    entryDiscLabels[i].color = d;
                }
                if (entryButtons != null && i < entryButtons.Length && entryButtons[i] != null)
                    entryButtons[i].interactable = reachable && !active;
            }
        }
    }
}
