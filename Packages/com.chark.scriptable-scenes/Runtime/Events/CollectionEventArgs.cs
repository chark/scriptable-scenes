namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Event arguments used in <see cref="CollectionLoadEvent"/>.
    /// </summary>
    public readonly struct CollectionLoadEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Scene collection which is loading.
        /// </summary>
        public BaseScriptableSceneCollection Collection { get; }

        #endregion

        #region Internal Methods

        internal CollectionLoadEventArgs(BaseScriptableSceneCollection collection)
        {
            Collection = collection;
        }

        #endregion
    }

    /// <summary>
    /// Event arguments used in <see cref="CollectionLoadProgressEvent"/>.
    /// </summary>
    public readonly struct CollectionLoadProgressEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Scene Collection which is loading.
        /// </summary>
        public BaseScriptableSceneCollection Collection { get; }

        /// <summary>
        /// Scene which is loading.
        /// </summary>
        public BaseScriptableScene Scene { get; }

        /// <summary>
        /// Load progress for <see cref="Collection"/> (goes from 0 to 1).
        /// </summary>
        public float CollectionLoadProgress { get; }

        /// <summary>
        /// Load progress for <see cref="Scene"/> (goes from 0 to 1).
        /// </summary>
        public float SceneLoadProgress { get; }

        #endregion

        #region Internal Methods

        internal CollectionLoadProgressEventArgs(
            BaseScriptableSceneCollection collection,
            BaseScriptableScene scene,
            float collectionLoadProgress,
            float sceneLoadProgress
        )
        {
            Collection = collection;
            Scene = scene;
            CollectionLoadProgress = collectionLoadProgress;
            SceneLoadProgress = sceneLoadProgress;
        }

        #endregion
    }

    /// <summary>
    /// Event used in <see cref="CollectionUnloadEvent"/>.
    /// </summary>
    public readonly struct CollectionUnloadEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Collection which is unloading.
        /// </summary>
        public BaseScriptableSceneCollection Collection { get; }

        #endregion

        #region Internal Methods

        internal CollectionUnloadEventArgs(BaseScriptableSceneCollection collection)
        {
            Collection = collection;
        }

        #endregion
    }
}
