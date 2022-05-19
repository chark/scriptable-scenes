namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Event arguments used in <see cref="SceneLoadEvent"/>.
    /// </summary>
    public readonly struct SceneLoadEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Scene which is loading.
        /// </summary>
        public BaseScriptableScene Scene { get; }

        #endregion

        #region Internal Methods

        internal SceneLoadEventArgs(BaseScriptableScene scene)
        {
            Scene = scene;
        }

        #endregion
    }

    /// <summary>
    /// Event arguments used in <see cref="SceneLoadProgressEvent"/>.
    /// </summary>
    public readonly struct SceneLoadProgressEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Scene which is loading.
        /// </summary>
        public BaseScriptableScene Scene { get; }

        /// <summary>
        /// The progress <see cref="Scene"/> loading. Goes from 0 to 1 (inclusive).
        /// </summary>
        public float Progress { get; }

        #endregion

        #region Internal Methods

        internal SceneLoadProgressEventArgs(BaseScriptableScene scene, float progress)
        {
            Scene = scene;
            Progress = progress;
        }

        #endregion
    }

    /// <summary>
    /// Event arguments used in <see cref="SceneUnloadEvent"/>.
    /// </summary>
    public readonly struct SceneUnloadEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Scene which is unloading.
        /// </summary>
        public BaseScriptableScene Scene { get; }

        #endregion

        #region Internal Methods

        internal SceneUnloadEventArgs(BaseScriptableScene scene)
        {
            Scene = scene;
        }

        #endregion
    }

    /// <summary>
    /// Event arguments used in <see cref="SceneActivateEvent"/>.
    /// </summary>
    public readonly struct SceneActivateEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Scene which is being activated.
        /// </summary>
        public BaseScriptableScene Scene { get; }

        #endregion

        #region Internal Methods

        internal SceneActivateEventArgs(BaseScriptableScene scene)
        {
            Scene = scene;
        }

        #endregion
    }
}
