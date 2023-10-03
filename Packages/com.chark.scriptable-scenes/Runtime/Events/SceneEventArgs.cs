namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Event arguments used in <see cref="SceneLoadEvent"/>.
    /// </summary>
    public readonly struct SceneLoadEventArgs
    {
        /// <summary>
        /// Scene which is loading.
        /// </summary>
        public ScriptableScene Scene { get; }

        internal SceneLoadEventArgs(ScriptableScene scene)
        {
            Scene = scene;
        }
    }

    /// <summary>
    /// Event arguments used in <see cref="SceneLoadProgressEvent"/>.
    /// </summary>
    public readonly struct SceneLoadProgressEventArgs
    {
        /// <summary>
        /// Scene which is loading.
        /// </summary>
        public ScriptableScene Scene { get; }

        /// <summary>
        /// The progress <see cref="Scene"/> loading. Goes from 0 to 1 (inclusive).
        /// </summary>
        public float Progress { get; }

        internal SceneLoadProgressEventArgs(ScriptableScene scene, float progress)
        {
            Scene = scene;
            Progress = progress;
        }
    }

    /// <summary>
    /// Event arguments used in <see cref="SceneUnloadEvent"/>.
    /// </summary>
    public readonly struct SceneUnloadEventArgs
    {
        /// <summary>
        /// Scene which is unloading.
        /// </summary>
        public ScriptableScene Scene { get; }

        internal SceneUnloadEventArgs(ScriptableScene scene)
        {
            Scene = scene;
        }
    }

    /// <summary>
    /// Event arguments used in <see cref="SceneActivateEvent"/>.
    /// </summary>
    public readonly struct SceneActivateEventArgs
    {
        /// <summary>
        /// Scene which is being activated.
        /// </summary>
        public ScriptableScene Scene { get; }

        internal SceneActivateEventArgs(ScriptableScene scene)
        {
            Scene = scene;
        }
    }
}
