using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DPP.Models;

namespace DPP.UI
{
    /// <summary>
    /// State machine + view for the guided step flow (spec 04 v2, screens 04–08).
    /// One screen, content swapped per step from backend v0.4 `disassembly.steps[]`.
    ///
    /// Navigation: Back on step 1 → intro; Confirm on last step → completion
    /// (phase 5; logs a stub until built). Entering the flow always restarts at
    /// step 1 (OnEnable). The exploded-view canvas is shown/hidden by
    /// ScreenRouter together with this screen.
    /// </summary>
    public class StepFlowController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private ScreenRouter router;
        [SerializeField] private CompletionSummaryView summary;

        [Header("Header")]
        [SerializeField] private TMP_Text stepIndicator;   // "Step n of 5"
        [SerializeField] private TMP_Text titleText;

        [Header("Progress (track height 378)")]
        [SerializeField] private RectTransform progressFill;
        [SerializeField] private TMP_Text progressLabel;   // "n/5"
        private const float TrackHeight = 378f;

        [Header("Action card 1")]
        [SerializeField] private Image card1Ring;
        [SerializeField] private Image card1Icon;
        [SerializeField] private TMP_Text card1Title;
        [SerializeField] private TMP_Text card1Subtitle;

        [Header("Action card 2")]
        [SerializeField] private Image card2Ring;
        [SerializeField] private Image card2Icon;
        [SerializeField] private TMP_Text card2Title;
        [SerializeField] private TMP_Text card2Subtitle;

        [Header("Confirm button label")]
        [SerializeField] private TMP_Text confirmLabel;

        [Header("Icon lookup (wired by builder; keys per spec 04 §8)")]
        [SerializeField] private string[] iconKeys;
        [SerializeField] private Sprite[] iconSprites;

        private List<Step> _steps;
        private int _index;
        private Coroutine _progressAnim;
        private float _flowStartTime;   // stopwatch: flow entry → finish (spec 09 §3)

        // Accent colors (spec 04 §3).
        private static readonly Color TealRing = DPPTheme.TealLight;
        private static readonly Color TealIcon = DPPTheme.TealLight;
        private static readonly Color GoldRing = DPPTheme.GoldPartStroke;
        private static readonly Color GoldText = DPPTheme.Hex("#f0c879");

        public void Populate(DPPData data)
        {
            _steps = data?.disassembly?.steps;
            if (gameObject.activeInHierarchy) Refresh(false);
        }

        private void OnEnable()
        {
            _index = 0;
            _flowStartTime = Time.realtimeSinceStartup; // timer starts with the flow
            Refresh(false);
        }

        // ---- Button targets (wired by builder) ----

        public void Confirm()
        {
            int total = TotalSteps();
            if (_index >= total - 1)
            {
                // Finish: stop the stopwatch, hand the session to the summary.
                int elapsed = Mathf.RoundToInt(Time.realtimeSinceStartup - _flowStartTime);
                if (summary != null) summary.SetSession(elapsed, total, total);
                if (router != null) router.ShowCompletion();
                else Debug.LogWarning("[StepFlowController] No router — cannot show completion summary.");
                return;
            }
            _index++;
            Refresh(true);
        }

        public void BackStep()
        {
            if (_index == 0)
            {
                if (router != null) router.ShowDisassembly();
                return;
            }
            _index--;
            Refresh(true);
        }

        // ---- Internals ----

        private int TotalSteps() => _steps != null && _steps.Count > 0 ? _steps.Count : 5;

        private void Refresh(bool animateProgress)
        {
            int total = TotalSteps();
            int n = _index + 1;

            if (stepIndicator != null) stepIndicator.text = $"Step {n} of {total}";
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

            if (_steps == null || _index >= _steps.Count)
            {
                Debug.LogWarning("[StepFlowController] No step data — showing builder-baked demo content.");
                return;
            }

            Step step = _steps[_index];
            if (titleText != null) titleText.text = step.title;

            ApplyAction(step, 0, card1Ring, card1Icon, card1Title, card1Subtitle);
            ApplyAction(step, 1, card2Ring, card2Icon, card2Title, card2Subtitle);

            if (confirmLabel != null)
                confirmLabel.text = n >= total ? "Finish & see summary" : "Confirm & next";
        }

        private void ApplyAction(Step step, int i, Image ring, Image icon, TMP_Text title, TMP_Text subtitle)
        {
            bool has = step.actions != null && i < step.actions.Count;
            if (!has) return;

            StepAction a = step.actions[i];
            if (title != null) title.text = a.title;
            if (subtitle != null)
            {
                subtitle.text = a.subtitle ?? "";
                subtitle.color = a.value ? GoldText : DPPTheme.TextSecondary;
            }
            if (ring != null) ring.color = a.value ? GoldRing : TealRing;
            if (icon != null)
            {
                icon.color = a.value ? GoldText : TealIcon;
                Sprite s = LookupIcon(a.icon);
                if (s != null) icon.sprite = s;
            }
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
