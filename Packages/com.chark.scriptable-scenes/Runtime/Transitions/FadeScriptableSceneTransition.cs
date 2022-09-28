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
    internal sealed class FadeScriptableSceneTransition : BaseScriptableSceneTransition
    {
        #region Editor Fields

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
        [Tooltip("Time taken to fade in the canvas")]
        [Min(0f)]
        [SerializeField]
        private float fadeInDurationSeconds = 0.5f;

        [Tooltip("Amount of seconds to wait before fading out the canvas")]
        [Min(0f)]
        [SerializeField]
        private float fadeOutWaitDurationSeconds = 2.0f;

        [Tooltip("Time taken to fade out the canvas")]
        [Min(0f)]
        [SerializeField]
        private float fadeOutDurationSeconds = 0.5f;

        #endregion

        #region Private Fields

        private readonly List<FadeCanvas> canvases = new List<FadeCanvas>();

        #endregion

        #region Protected Methods

        protected override IEnumerator OnShowRoutine()
        {
            // Fade in the curtains.
            ShowFadeCanvas();
            yield return FadeRoutine(fadeOutAlpha, fadeInAlpha, fadeInDurationSeconds);
        }

        protected override IEnumerator OnHideRoutine()
        {
            if (fadeOutWaitDurationSeconds > 0.0f)
            {
                yield return new WaitForSeconds(fadeOutWaitDurationSeconds);
            }

            // Fade out the curtains.
            yield return FadeRoutine(fadeInAlpha, fadeOutAlpha, fadeOutDurationSeconds);
            HideFadeCanvas();
        }

        #endregion

        #region Internal Methods

        internal void AddCanvas(FadeCanvas canvas)
        {
            canvases.Add(canvas);
        }

        internal void RemoveCanvas(FadeCanvas canvas)
        {
            canvases.Remove(canvas);
        }

        #endregion

        #region Private Methods

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

        #endregion
    }
}
