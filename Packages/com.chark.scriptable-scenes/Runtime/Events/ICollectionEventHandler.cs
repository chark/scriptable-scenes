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

        /// <summary>
        /// Called when transition of <see cref="BaseScriptableSceneCollection"/> starts to show.
        /// </summary>
        public event CollectionTransitionEvent OnShowTransitionEntered;

        /// <summary>
        /// Called when transition of <see cref="BaseScriptableSceneCollection"/> is shown.
        /// </summary>
        public event CollectionTransitionEvent OnShowTransitionExited;

        /// <summary>
        /// Called when transition of <see cref="BaseScriptableSceneCollection"/> starts to hide.
        /// </summary>
        public event CollectionTransitionEvent OnHideTransitionEntered;

        /// <summary>
        /// Called when transition of <see cref="BaseScriptableSceneCollection"/> is hidden.
        /// </summary>
        public event CollectionTransitionEvent OnHideTransitionExited;

        #endregion
    }
}
