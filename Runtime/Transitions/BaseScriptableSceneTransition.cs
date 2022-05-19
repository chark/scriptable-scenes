using System.Collections;
using UnityEngine;

namespace CHARK.ScriptableScenes.Transitions
{
    /// <summary>
    /// Transition used to transition between <see cref="BaseScriptableSceneCollection"/> when they are
    /// loaded and unloaded.
    /// </summary>
    public abstract class BaseScriptableSceneTransition : ScriptableObject
    {
        #region Public Methods

        /// <returns>
        /// Enumerator which transitions into the <see cref="BaseScriptableSceneCollection"/>. Called
        /// before the collection is loaded.
        /// </returns>
        public abstract IEnumerator ShowRoutine();

        /// <returns>
        /// Enumerator which transitions out of the <see cref="BaseScriptableSceneCollection"/>. Called
        /// before the collection is unloaded.
        /// </returns>
        public abstract IEnumerator HideRoutine();

        #endregion
    }
}
