namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Invoked when <see cref="BaseScriptableSceneCollection"/> starts to load or is loaded.
    /// </summary>
    public delegate void CollectionLoadEvent(CollectionLoadEventArgs args);

    /// <summary>
    /// Invoked when <see cref="BaseScriptableSceneCollection"/> loading process updates.
    /// </summary>
    public delegate void CollectionLoadProgressEvent(CollectionLoadProgressEventArgs args);

    /// <summary>
    /// Invoked when <see cref="BaseScriptableSceneCollection"/> starts to unload or is unloaded.
    /// </summary>
    public delegate void CollectionUnloadEvent(CollectionUnloadEventArgs args);

    /// <summary>
    /// Invoked when <see cref="BaseScriptableSceneCollection"/> starts to show or hides the
    /// transition.
    /// </summary>
    public delegate void CollectionTransitionEvent(); // TODO: add args
}
