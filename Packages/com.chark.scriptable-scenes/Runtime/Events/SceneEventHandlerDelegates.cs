namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Invoked when <see cref="ScriptableScene"/> starts to load or is loaded.
    /// </summary>
    public delegate void SceneLoadEvent(SceneLoadEventArgs args);

    /// <summary>
    /// Invoked when <see cref="ScriptableScene"/> loading progress updates.
    /// </summary>
    public delegate void SceneLoadProgressEvent(SceneLoadProgressEventArgs args);

    /// <summary>
    /// Invoked when <see cref="ScriptableScene"/> starts to unload or is unloaded.
    /// </summary>
    public delegate void SceneUnloadEvent(SceneUnloadEventArgs args);

    /// <summary>
    /// Invoked when <see cref="ScriptableScene"/> starts to activate or is activated.
    /// </summary>
    public delegate void SceneActivateEvent(SceneActivateEventArgs args);
}
