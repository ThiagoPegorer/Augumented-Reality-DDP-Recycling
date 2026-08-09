using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// State machine + view for the guided step flow (spec 04 v3, screens 04–08).
    /// One screen, content swapped per step from backend `disassembly.steps[]`.
    ///
    /// v3 (2026-07-10):
    ///   - Header eyebrow + "Step n of 5" REMOVED (progress rail carries n/5).
    ///   - Task rows are UNBOXED; each has a clickable status circle:
    ///     RED ✗ (pending) → tap → GREEN ✓ (done). Tapping again un-checks
    ///     (glove mis-taps happen).
    ///
    /// v3.2 (Thiago, 2026-08-01): the pending state shows a universal ✗ instead
    /// of the action's own glyph (screw, lever, board …). The circle is a status
    /// light, and a status light must have exactly two readings — a glyph on red
    /// invited "what is this icon telling me?" instead of "this is not done yet".
    /// The task's identity is carried by the title and subtitle beside it, so
    /// nothing is lost. The `iconKeys`/`iconSprites` lookup died with it, and
    /// `StepAction.icon` in the payload now has no reader.
    ///   - "Confirm & next" is LOCKED (grey, non-interactable) until BOTH
    ///     tasks are green. Task state resets on entering a step (incl. Back).
    ///   - The how-to slot plays the current step's motion on the real model
    ///     via StepHowToLoop (SetStep on every refresh).
    ///
    /// Navigation: Back on step 1 → intro; Confirm on last step → completion.
    /// Entering the flow always restarts at step 1 (OnEnable).
    /// </summary>
    public class StepFlowController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private CompletionSummaryView summary;
        [SerializeField] private StepHowToLoop howToLoop;

        [Header("Title")]
        [SerializeField] private TMP_Text titleText;

        [Header("Progress (track height 378)")]
        [SerializeField] private RectTransform progressFill;
        [SerializeField] private TMP_Text progressLabel;   // "n/5"
        private const float TrackHeight = 378f;

        [Header("Task row 1")]
        [SerializeField] private Image task1Fill;          // status circle fill (red/green)
        [SerializeField] private GameObject task1Cross;    // ✗ mark (pending state)
        [SerializeField] private GameObject task1Check;    // ✓ mark (done state)
        [SerializeField] private TMP_Text task1Title;
        [SerializeField] private TMP_Text task1Subtitle;

        [Header("Task row 2")]
        [SerializeField] private Image task2Fill;
        [SerializeField] private GameObject task2Cross;
        [SerializeField] private GameObject task2Check;
        [SerializeField] private TMP_Text task2Title;
        [SerializeField] private TMP_Text task2Subtitle;

        [Header("Cancel modal (Back opens it; Yes → main page, No → stay)")]
        [SerializeField] private GameObject cancelModal;

        [Header("Confirm button (lockable)")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Image confirmFill;
        [SerializeField] private TMP_Text confirmLabel;
        [SerializeField] private Image confirmChevron1;
        [SerializeField] private Image confirmChevron2;
        [SerializeField] private HoverHighlight confirmHover;

        private List<Step> _steps;
        private int _index;
        private Coroutine _progressAnim;
        private float _flowStartTime;                       // stopwatch: flow entry → finish (spec 09 §3)
        private float _stepStartTime;                       // split stopwatch: current step entry
        private readonly List<int> _stepSplits = new List<int>();  // per-step seconds (summary v3 + report)
        private readonly bool[] _done = new bool[2];

        // The HoverHighlight on each status button. Resolved from the fill at
        // RUNTIME, not serialized — the scene wiring is untouched, so this fix
        // needs no phase re-run (re-running RBv2_1_1/06 after RBv2_1_1/10 is the
        // forbidden order: its active-only animator Find can grab the stage clone).
        private HoverHighlight _task1Hover, _task2Hover;
        private bool _hoversResolved;

        // Status + accent colors (spec 04 v3).
        private static readonly Color PendingRed = DPPTheme.Hex("#e24b4a");
        private static readonly Color DoneGreen  = DPPTheme.TealAccent;
        private static readonly Color LockedText = DPPTheme.Hex("#5d7396");
        private static readonly Color GoldText   = DPPTheme.Hex("#f0c879");

        public void Populate(DPPData data)
        {
            _steps = data?.disassembly?.steps;
            if (gameObject.activeInHierarchy) Refresh(false);
        }

        private void OnEnable()
        {
            _index = 0;
            _flowStartTime = Time.realtimeSinceStartup; // timer starts with the flow
            _stepStartTime = _flowStartTime;
            _stepSplits.Clear();                        // cancelled runs discard their splits
            if (cancelModal != null) cancelModal.SetActive(false);
            Refresh(false);
        }

        // ---- Button targets (wired by builder) ----

        public void ToggleTask1() => ToggleTask(0);
        public void ToggleTask2() => ToggleTask(1);

        public void Confirm()
        {
            if (!(_done[0] && _done[1])) return;   // locked — belt & braces beside interactable=false

            // Record this step's split (entry → confirm).
            float now = Time.realtimeSinceStartup;
            _stepSplits.Add(Mathf.RoundToInt(now - _stepStartTime));
            _stepStartTime = now;

            int total = TotalSteps();
            if (_index >= total - 1)
            {
                // Finish: stop the stopwatch, hand the session to the summary.
                int elapsed = Mathf.RoundToInt(now - _flowStartTime);
                if (summary != null) summary.SetSession(elapsed, total, total, _stepSplits.ToArray());
                if (router != null) router.ShowCompletion();
                else Debug.LogWarning("[StepFlowController] No router — cannot show completion summary.");
                return;
            }
            _index++;
            Refresh(true);
        }

        /// <summary>Back = abort (v3.1): opens the cancel modal from ANY step.
        /// Per-step back-navigation was removed deliberately — the physical
        /// teardown is one-way, and task state resets per step anyway.</summary>
        public void BackStep()
        {
            if (cancelModal != null) { cancelModal.SetActive(true); return; }
            // Fallback if the modal isn't built: old behaviour (step 1 → intro).
            if (_index == 0)
            {
                if (router != null) router.ShowDisassembly();
                return;
            }
            _index--;
            Refresh(true);
        }

        /// <summary>
        /// Modal "Yes" — abandon the run.
        ///
        /// RBv1.0 returned to the Main Page. RBv2.0 (Miro journey v4) returns to
        /// the DISASSEMBLY INTRO instead: every Back edge moves exactly one level,
        /// and the participant who cancels almost always wants to restart the run,
        /// not leave the product. The cancel modal itself stays — it is what stops
        /// an accidental Back from silently discarding a timed run.
        ///
        /// The run's splits and stopwatch reset in ResetState() when the flow is
        /// re-entered, so a cancelled attempt contributes nothing to step_times_s.
        /// </summary>
        public void CancelYes()
        {
            if (cancelModal != null) cancelModal.SetActive(false);
            if (router != null) router.ShowDisassembly();
            else Debug.LogWarning("[StepFlowController] No router — cannot return to the disassembly intro.");
        }

        /// <summary>Modal "No" — dismiss, keep working.</summary>
        public void CancelNo()
        {
            if (cancelModal != null) cancelModal.SetActive(false);
        }

        // ---- Internals ----

        private int TotalSteps() => _steps != null && _steps.Count > 0 ? _steps.Count : 5;

        private void ToggleTask(int i)
        {
            _done[i] = !_done[i];
            ApplyTaskVisual(i);
            ApplyConfirmState();
        }

        private void Refresh(bool animateProgress)
        {
            int total = TotalSteps();
            int n = _index + 1;

            if (progressLabel != null) progressLabel.text = $"{n}/{total}";

            float targetH = TrackHeight * n / total;
            if (progressFill != null)
            {
                if (_progressAnim != null) StopCoroutine(_progressAnim);
                if (animateProgress && gameObject.activeInHierarchy)
                    _progressAnim = StartCoroutine(AnimateProgress(targetH));
                else
                    progressFill.sizeDelta = new Vector2(progressFill.sizeDelta.x, targetH);
            }

            // Task gating: every step entry starts with both tasks pending.
            _done[0] = _done[1] = false;
            ApplyTaskVisual(0);
            ApplyTaskVisual(1);
            ApplyConfirmState();

            // How-to loop follows the current step.
            if (howToLoop != null) howToLoop.SetStep(n);

            if (_steps == null || _index >= _steps.Count)
            {
                Debug.LogWarning("[StepFlowController] No step data — showing builder-baked demo content.");
                return;
            }

            Step step = _steps[_index];
            if (titleText != null) titleText.text = step.title;

            ApplyAction(step, 0, task1Title, task1Subtitle);
            ApplyAction(step, 1, task2Title, task2Subtitle);

            if (confirmLabel != null)
                confirmLabel.text = n >= total ? "Finish & see summary" : "Confirm & next";
        }

        private void ApplyAction(Step step, int i, TMP_Text title, TMP_Text subtitle)
        {
            bool has = step.actions != null && i < step.actions.Count;
            if (!has) return;

            StepAction a = step.actions[i];
            if (title != null) title.text = a.title;
            if (subtitle != null)
            {
                subtitle.text = a.subtitle ?? "";
                // Gold high-value accent lives on the subtitle (the status
                // circle is binary — red ✗ / green ✓ only).
                subtitle.color = a.value ? GoldText : DPPTheme.TextSecondary;
            }
            // a.icon is no longer read: the circle is a status light, not a glyph slot.
        }

        /// <summary>
        /// ⚠ THE COLOUR GOES THROUGH HoverHighlight, NOT ONLY THE Image (device
        /// round 1, 2026-08-07). The status button carries a HoverHighlight whose
        /// name-resolved `fill` IS this circle, and its Apply() repaints the rest
        /// colour it captured at Awake — red — on every ease frame and every
        /// enable/disable. Writing `fill.color = green` alone therefore survived
        /// only while the hand still hovered the button: the ✓ appeared, the
        /// circle stayed red. SetRestFillColor updates the colour the hover
        /// restores, which is the colour that actually persists.
        /// </summary>
        private void ApplyTaskVisual(int i)
        {
            if (!_hoversResolved)
            {
                _hoversResolved = true;
                if (task1Fill != null) _task1Hover = task1Fill.GetComponentInParent<HoverHighlight>();
                if (task2Fill != null) _task2Hover = task2Fill.GetComponentInParent<HoverHighlight>();
            }

            Image fill = i == 0 ? task1Fill : task2Fill;
            GameObject cross = i == 0 ? task1Cross : task2Cross;
            GameObject check = i == 0 ? task1Check : task2Check;
            HoverHighlight hover = i == 0 ? _task1Hover : _task2Hover;

            Color c = _done[i] ? DoneGreen : PendingRed;
            if (fill != null) fill.color = c;                 // kept for a row with no hover
            if (hover != null) hover.SetRestFillColor(c);     // the write that persists
            if (cross != null) cross.SetActive(!_done[i]);
            if (check != null) check.SetActive(_done[i]);
        }

        private void ApplyConfirmState()
        {
            bool unlocked = _done[0] && _done[1];
            Color fillColor = unlocked ? DPPTheme.TealAccent : DPPTheme.SecondaryButtonFill;

            if (confirmButton != null) confirmButton.interactable = unlocked;
            if (confirmFill != null) confirmFill.color = fillColor;
            if (confirmLabel != null) confirmLabel.color = unlocked ? DPPTheme.TextOnNavy : LockedText;
            if (confirmChevron1 != null) confirmChevron1.color = unlocked ? Color.white : LockedText;
            if (confirmChevron2 != null) confirmChevron2.color = unlocked ? Color.white : LockedText;

            // Same trap as the status circles: the hover's OnEnable/OnDisable
            // snap-repaints its captured rest colour over whatever this method
            // just wrote. Hand it the state colour FIRST, then toggle it.
            if (confirmHover != null)
            {
                confirmHover.SetRestFillColor(fillColor);
                confirmHover.enabled = unlocked;
            }
        }

        private IEnumerator AnimateProgress(float targetH)
        {
            float startH = progressFill.sizeDelta.y;
            float t = 0f;
            const float duration = 0.25f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float h = Mathf.Lerp(startH, targetH, Mathf.SmoothStep(0f, 1f, t / duration));
                progressFill.sizeDelta = new Vector2(progressFill.sizeDelta.x, h);
                yield return null;
            }
            progressFill.sizeDelta = new Vector2(progressFill.sizeDelta.x, targetH);
            _progressAnim = null;
        }
    }
}
