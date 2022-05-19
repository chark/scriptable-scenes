using System.Collections;
using CHARK.ScriptableScenes.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Wrapper for <see cref="Scene"/>.
    /// </summary>
    public abstract class BaseScriptableScene : ScriptableObject
    {
        #region Public Properties

        /// <summary>
        /// Name of this scene.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Path to the scene.
        /// </summary>
        public abstract string ScenePath { get; }

        /// <summary>
        /// Scene build index.
        /// </summary>
        public abstract int SceneBuildIndex { get; }

        /// <summary>
        /// Should this scene be activated after loading.
        /// </summary>
        public abstract bool IsActivate { get; }

        /// <summary>
        /// Should this scene persist between scene loads (never unloaded, useful for setup scenes).
        /// </summary>
        public abstract bool IsPersist { get; }

        /// <summary>
        /// Event handler assigned to this scene.
        /// </summary>
        public abstract ISceneEventHandler SceneEvents { get; }

        #endregion

        #region Public Methods

        /// <returns>
        /// Routine which loads this scene.
        /// </returns>
        public abstract IEnumerator LoadRoutine();

        /// <returns>
        /// Routine which unloads this scene.
        /// </returns>
        public abstract IEnumerator UnloadRoutine();

        /// <summary>
        /// Activate this scene via <see cref="SceneManager.SetActiveScene"/>.
        /// </summary>
        public abstract void SetActive();

        #endregion
    }
}
