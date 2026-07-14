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
    ///     RED (pending, shows the action glyph) → tap → GREEN (done, shows a
    ///     check). Tapping again un-checks (glove mis-taps happen).
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
        [SerializeField] private Image task1Icon;          // action glyph (pending state)
        [SerializeField] private GameObject task1Check;    // check mark (done state)
        [SerializeField] private TMP_Text task1Title;
        [SerializeField] private TMP_Text task1Subtitle;

        [Header("Task row 2")]
        [SerializeField] private Image task2Fill;
        [SerializeField] private Image task2Icon;
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

        [Header("Icon lookup (wired by builder; keys per spec 04 §8)")]
        [SerializeField] private string[] iconKeys;
        [SerializeField] private Sprite[] iconSprites;

        private List<Step> _steps;
        private int _index;
        private Coroutine _progressAnim;
        private float _flowStartTime;                       // stopwatch: flow entry → finish (spec 09 §3)
        private float _stepStartTime;                       // split stopwatch: current step entry
        private readonly List<int> _stepSplits = new List<int>();  // per-step seconds (summary v3 + report)
        private readonly bool[] _done = new bool[2];

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

        /// <summary>Modal "Yes" — abandon the run, back to the main page.</summary>
        public void CancelYes()
        {
            if (cancelModal != null) cancelModal.SetActive(false);
            if (router != null) router.ShowMainPage();
            else Debug.LogWarning("[StepFlowController] No router — cannot return to the main page.");
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

            ApplyAction(step, 0, task1Icon, task1Title, task1Subtitle);
            ApplyAction(step, 1, task2Icon, task2Title, task2Subtitle);

            if (confirmLabel != null)
                confirmLabel.text = n >= total ? "Finish & see summary" : "Confirm & next";
        }

        private void ApplyAction(Step step, int i, Image icon, TMP_Text title, TMP_Text subtitle)
        {
            bool has = step.actions != null && i < step.actions.Count;
            if (!has) return;

            StepAction a = step.actions[i];
            if (title != null) title.text = a.title;
            if (subtitle != null)
            {
                subtitle.text = a.subtitle ?? "";
                // Gold high-value accent lives on the subtitle now (the icon
                // circle is a status button — red/green only).
                subtitle.color = a.value ? GoldText : DPPTheme.TextSecondary;
            }
            if (icon != null)
            {
                Sprite s = LookupIcon(a.icon);
                if (s != null) icon.sprite = s;
            }
        }

        private void ApplyTaskVisual(int i)
        {
            Image fill = i == 0 ? task1Fill : task2Fill;
            Image icon = i == 0 ? task1Icon : task2Icon;
            GameObject check = i == 0 ? task1Check : task2Check;

            if (fill != null) fill.color = _done[i] ? DoneGreen : PendingRed;
            if (icon != null) icon.gameObject.SetActive(!_done[i]);
            if (check != null) check.SetActive(_done[i]);
        }

        private void ApplyConfirmState()
        {
            bool unlocked = _done[0] && _done[1];
            if (confirmButton != null) confirmButton.interactable = unlocked;
            if (confirmFill != null) confirmFill.color = unlocked ? DPPTheme.TealAccent : DPPTheme.SecondaryButtonFill;
            if (confirmLabel != null) confirmLabel.color = unlocked ? DPPTheme.TextOnNavy : LockedText;
            if (confirmChevron1 != null) confirmChevron1.color = unlocked ? Color.white : LockedText;
            if (confirmChevron2 != null) confirmChevron2.color = unlocked ? Color.white : LockedText;
            if (confirmHover != null) confirmHover.enabled = unlocked;
        }

        private Sprite LookupIcon(string key)
        {
            if (iconKeys == null || iconSprites == null || string.IsNullOrEmpty(key)) return null;
            for (int i = 0; i < iconKeys.Length && i < iconSprites.Length; i++)
                if (iconKeys[i] == key) return iconSprites[i];
            return null;
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
