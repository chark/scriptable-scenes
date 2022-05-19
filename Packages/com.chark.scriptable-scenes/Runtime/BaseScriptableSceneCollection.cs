using System.Collections;
using System.Collections.Generic;
using CHARK.ScriptableScenes.Events;
using UnityEngine;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Collection of <see cref="BaseScriptableScene"/> which can be used to load a set of scenes
    /// at once.
    /// </summary>
    public abstract class BaseScriptableSceneCollection : ScriptableObject
    {
        #region Public Properties

        /// <summary>
        /// Unique collection id.
        /// </summary>
        public abstract string Guid { get; }

        /// <summary>
        /// Name of this collection.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Count of <see cref="BaseScriptableScene"/> in this collection.
        /// </summary>
        public abstract int SceneCount { get; }

        /// <summary>
        /// Available <see cref="BaseScriptableScene"/> in this collection.
        /// </summary>
        public abstract IEnumerable<BaseScriptableScene> Scenes { get; }

        /// <summary>
        /// Collection events invoked on this collection.
        /// </summary>
        public abstract ICollectionEventHandler CollectionEvents { get; }

        /// <summary>
        /// Scene events invoked on scenes of this collection.
        /// </summary>
        public abstract ISceneEventHandler SceneEvents { get; }

        #endregion

        #region Public Methods

        /// <returns>
        /// Routine which loads this collection.
        /// </returns>
        public abstract IEnumerator LoadRoutine();

        /// <returns>
        /// Routine which unloads this collection.
        /// </returns>
        public abstract IEnumerator UnloadRoutine();

        /// <returns>
        /// Routine which shows the transition of this collection.
        /// </returns>
        public abstract IEnumerator ShowTransitionRoutine();

        /// <returns>
        /// Routine which hides the transition of this collection.
        /// </returns>
        public abstract IEnumerator HideTransitionRoutine();

        #endregion
    }
}
