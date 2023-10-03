namespace CHARK.ScriptableScenes.Events
{
    public interface ISceneEventHandler
    {
        /// <summary>
        /// Called when loading of <see cref="ScriptableScene"/> begins.
        /// </summary>
        public event SceneLoadEvent OnLoadEntered;

        /// <summary>
        /// Called when loading of <see cref="ScriptableScene"/> finishes.
        /// </summary>
        public event SceneLoadEvent OnLoadExited;

        /// <summary>
        /// Called when loading of <see cref="ScriptableScene"/> updates.
        /// </summary>
        public event SceneLoadProgressEvent OnLoadProgress;

        /// <summary>
        /// Called when unloading of <see cref="ScriptableScene"/> begins.
        /// </summary>
        public event SceneUnloadEvent OnUnloadEntered;

        /// <summary>
        /// Called when unloading of <see cref="ScriptableScene"/> finishes.
        /// </summary>
        public event SceneUnloadEvent OnUnloadExited;

        /// <summary>
        /// Called when activation of <see cref="ScriptableScene"/> beings.
        /// </summary>
        public event SceneActivateEvent OnActivateEntered;

        /// <summary>
        /// Called when activation of <see cref="ScriptableScene"/> finishes.
        /// </summary>
        public event SceneActivateEvent OnActivateExited;
    }
}
