using System;
using System.Collections;
using UnityEngine;

namespace CHARK.ScriptableScenes.Transitions
{
    /// <summary>
    /// Transition used to transition between <see cref="ScriptableSceneCollection"/> when they are
    /// loaded and unloaded.
    /// </summary>
    public abstract class ScriptableSceneTransition : ScriptableObject
    {
        /// <summary>
        /// Invoked when <see cref="ShowRoutine"/> is starts.
        /// </summary>
        public event Action OnShowEntered;

        /// <summary>
        /// Invoked when <see cref="ShowRoutine"/> is finishes.
        /// </summary>
        public event Action OnShowExited;

        /// <summary>
        /// Invoked when <see cref="HideRoutine"/> is starts.
        /// </summary>
        public event Action OnHideEntered;

        /// <summary>
        /// Invoked when <see cref="HideRoutine"/> is finishes.
        /// </summary>
        public event Action OnHideExited;

        /// <returns>
        /// Enumerator which transitions into the <see cref="ScriptableSceneCollection"/>. Called
        /// before the collection is loaded.
        /// </returns>
        public IEnumerator ShowRoutine()
        {
            OnShowEntered?.Invoke();
            try
            {
                yield return OnShowRoutine();
            }
            finally
            {
                OnShowExited?.Invoke();
            }
        }

        /// <returns>
        /// Enumerator which delays the transition into the
        /// <see cref="ScriptableSceneCollection"/>.
        /// </returns>
        public IEnumerator DelayRoutine()
        {
            yield return OnDelayRoutine();
        }

        /// <returns>
        /// Enumerator which transitions out of the <see cref="ScriptableSceneCollection"/>. Called
        /// before the collection is unloaded.
        /// </returns>
        public IEnumerator HideRoutine()
        {
            OnHideEntered?.Invoke();
            try
            {
                yield return OnHideRoutine();
            }
            finally
            {
                OnHideExited?.Invoke();
            }
        }

        protected abstract IEnumerator OnShowRoutine();

        protected abstract IEnumerator OnDelayRoutine();

        protected abstract IEnumerator OnHideRoutine();
    }
}
