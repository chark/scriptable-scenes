namespace CHARK.ScriptableScenes.Events
{
    public interface ISceneEventHandler
    {
        #region Public Events

        /// <summary>
        /// Called when loading of <see cref="BaseScriptableScene"/> begins.
        /// </summary>
        public event SceneLoadEvent OnLoadEntered;

        /// <summary>
        /// Called when loading of <see cref="BaseScriptableScene"/> finishes.
        /// </summary>
        public event SceneLoadEvent OnLoadExited;

        /// <summary>
        /// Called when loading of <see cref="BaseScriptableScene"/> updates.
        /// </summary>
        public event SceneLoadProgressEvent OnLoadProgress;

        /// <summary>
        /// Called when unloading of <see cref="BaseScriptableScene"/> begins.
        /// </summary>
        public event SceneUnloadEvent OnUnloadEntered;

        /// <summary>
        /// Called when unloading of <see cref="BaseScriptableScene"/> finishes.
        /// </summary>
        public event SceneUnloadEvent OnUnloadExited;

        /// <summary>
        /// Called when activation of <see cref="BaseScriptableScene"/> beings.
        /// </summary>
        public event SceneActivateEvent OnActivateEntered;

        /// <summary>
        /// Called when activation of <see cref="BaseScriptableScene"/> finishes.
        /// </summary>
        public event SceneActivateEvent OnActivateExited;

        #endregion
    }
}
