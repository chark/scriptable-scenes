namespace CHARK.ScriptableScenes.Events
{
    public interface ICollectionEventHandler
    {
        #region Public Events

        /// <summary>
        /// Called when loading of <see cref="BaseScriptableSceneCollection"/> begins.
        /// </summary>
        public event CollectionLoadEvent OnLoadEntered;

        /// <summary>
        /// Called when loading of <see cref="BaseScriptableSceneCollection"/> finishes.
        /// </summary>
        public event CollectionLoadEvent OnLoadExited;

        /// <summary>
        /// Called loading of <see cref="BaseScriptableSceneCollection"/> progresses.
        /// </summary>
        public event CollectionLoadProgressEvent OnLoadProgress;

        /// <summary>
        /// Called when unloading of <see cref="BaseScriptableSceneCollection"/> begins.
        /// </summary>
        public event CollectionUnloadEvent OnUnloadEntered;

        /// <summary>
        /// Called when unloading of <see cref="BaseScriptableSceneCollection"/> finishes.
        /// </summary>
        public event CollectionUnloadEvent OnUnloadExited;

        #endregion
    }
}
