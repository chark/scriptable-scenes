namespace CHARK.ScriptableScenes.Editor.Utilities
{
    internal enum CollectionStatus
    {
        /// <summary>
        /// Collection has no scenes.
        /// </summary>
        MissingScenes,

        /// <summary>
        /// Build settings are invalid (e.g., one of the scenes is not enabled).
        /// </summary>
        InvalidBuildSettings,

        /// <summary>
        /// All ready to go.
        /// </summary>
        Ready,
    }
}
