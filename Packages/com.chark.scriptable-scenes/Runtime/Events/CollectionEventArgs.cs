namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Event arguments used in <see cref="CollectionLoadEvent"/>.
    /// </summary>
    public readonly struct CollectionLoadEventArgs
    {
        /// <summary>
        /// Scene collection which is loading.
        /// </summary>
        public ScriptableSceneCollection Collection { get; }

        internal CollectionLoadEventArgs(ScriptableSceneCollection collection)
        {
            Collection = collection;
        }
    }

    /// <summary>
    /// Event arguments used in <see cref="CollectionLoadProgressEvent"/>.
    /// </summary>
    public readonly struct CollectionLoadProgressEventArgs
    {
        /// <summary>
        /// Scene Collection which is loading.
        /// </summary>
        public ScriptableSceneCollection Collection { get; }

        /// <summary>
        /// Scene which is loading.
        /// </summary>
        public ScriptableScene Scene { get; }

        /// <summary>
        /// Load progress for <see cref="Collection"/> (goes from 0 to 1).
        /// </summary>
        public float CollectionLoadProgress { get; }

        /// <summary>
        /// Load progress for <see cref="Scene"/> (goes from 0 to 1).
        /// </summary>
        public float SceneLoadProgress { get; }

        internal CollectionLoadProgressEventArgs(
            ScriptableSceneCollection collection,
            ScriptableScene scene,
            float collectionLoadProgress,
            float sceneLoadProgress
        )
        {
            Collection = collection;
            Scene = scene;
            CollectionLoadProgress = collectionLoadProgress;
            SceneLoadProgress = sceneLoadProgress;
        }
    }

    /// <summary>
    /// Event used in <see cref="CollectionUnloadEvent"/>.
    /// </summary>
    public readonly struct CollectionUnloadEventArgs
    {
        /// <summary>
        /// Collection which is unloading.
        /// </summary>
        public ScriptableSceneCollection Collection { get; }

        internal CollectionUnloadEventArgs(ScriptableSceneCollection collection)
        {
            Collection = collection;
        }
    }
}
