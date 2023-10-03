using System.Collections;
using System.Collections.Generic;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;

namespace CHARK.ScriptableScenes.Transitions
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(FadeScriptableSceneTransition),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Fade Scriptable Scene Transition",
        order = CreateAssetMenuConstants.TransitionOrder
    )]
    internal sealed class FadeScriptableSceneTransition : ScriptableSceneTransition
    {
        [Header("Alpha")]
        [Tooltip("Canvas alpha which is used when the canvas is faded in")]
        [Range(0f, 1f)]
        [SerializeField]
        private float fadeInAlpha = 1.0f;

        [Tooltip("Canvas alpha which is used when the canvas is faded out")]
        [Range(0f, 1f)]
        [SerializeField]
        private float fadeOutAlpha;

        [Header("Timings")]
        [Tooltip("Amount of seconds to wait before fading out the canvas")]
        [Min(0f)]
        [SerializeField]
        private float transitionDelaySeconds = 2.0f;

        [Tooltip("Time taken to fade in the canvas")]
        [Min(0f)]
        [SerializeField]
        private float fadeInDurationSeconds = 0.5f;

        [Tooltip("Time taken to fade out the canvas")]
        [Min(0f)]
        [SerializeField]
        private float fadeOutDurationSeconds = 0.5f;

        private readonly List<FadeCanvas> canvases = new();

        protected override IEnumerator OnShowRoutine()
        {
            // Fade in the curtains.
            ShowFadeCanvas();
            yield return FadeRoutine(fadeOutAlpha, fadeInAlpha, fadeInDurationSeconds);
        }

        protected override IEnumerator OnDelayRoutine()
        {
            if (transitionDelaySeconds > 0.0f)
            {
                yield return new WaitForSeconds(transitionDelaySeconds);
            }
        }

        protected override IEnumerator OnHideRoutine()
        {
            // Fade out the curtains.
            yield return FadeRoutine(fadeInAlpha, fadeOutAlpha, fadeOutDurationSeconds);
            HideFadeCanvas();
        }

        internal void AddCanvas(FadeCanvas canvas)
        {
            canvases.Add(canvas);
        }

        internal void RemoveCanvas(FadeCanvas canvas)
        {
            canvases.Remove(canvas);
        }

        private IEnumerator FadeRoutine(
            float from,
            float to,
            float duration
        )
        {
            var progress = 0f;

            while (progress < 1f)
            {
                var value = Mathf.Lerp(from, to, progress);
                SetAlpha(value);

                progress += Time.unscaledDeltaTime / duration;

                yield return null;
            }

            SetAlpha(to);

            yield return null;
        }

        private void SetAlpha(float alpha)
        {
            foreach (var canvas in canvases)
            {
                canvas.SetAlpha(alpha);
            }
        }

        private void ShowFadeCanvas()
        {
            foreach (var canvas in canvases)
            {
                canvas.ShowCanvas();
            }
        }

        private void HideFadeCanvas()
        {
            foreach (var canvas in canvases)
            {
                canvas.HideCanvas();
            }
        }
    }
}
