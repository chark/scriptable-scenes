namespace CHARK.ScriptableScenes.Events
{
    public interface ICollectionEventHandler
    {
        /// <summary>
        /// Called when loading of <see cref="ScriptableSceneCollection"/> begins.
        /// </summary>
        public event CollectionLoadEvent OnLoadEntered;

        /// <summary>
        /// Called when loading of <see cref="ScriptableSceneCollection"/> finishes.
        /// </summary>
        public event CollectionLoadEvent OnLoadExited;

        /// <summary>
        /// Called loading of <see cref="ScriptableSceneCollection"/> progresses.
        /// </summary>
        public event CollectionLoadProgressEvent OnLoadProgress;

        /// <summary>
        /// Called when unloading of <see cref="ScriptableSceneCollection"/> begins.
        /// </summary>
        public event CollectionUnloadEvent OnUnloadEntered;

        /// <summary>
        /// Called when unloading of <see cref="ScriptableSceneCollection"/> finishes.
        /// </summary>
        public event CollectionUnloadEvent OnUnloadExited;

        /// <summary>
        /// Called when transition of <see cref="ScriptableSceneCollection"/> starts to show.
        /// </summary>
        public event CollectionTransitionEvent OnShowTransitionEntered;

        /// <summary>
        /// Called when transition of <see cref="ScriptableSceneCollection"/> is shown.
        /// </summary>
        public event CollectionTransitionEvent OnShowTransitionExited;

        /// <summary>
        /// Called when transition of <see cref="ScriptableSceneCollection"/> starts to hide.
        /// </summary>
        public event CollectionTransitionEvent OnHideTransitionEntered;

        /// <summary>
        /// Called when transition of <see cref="ScriptableSceneCollection"/> is hidden.
        /// </summary>
        public event CollectionTransitionEvent OnHideTransitionExited;
    }
}
