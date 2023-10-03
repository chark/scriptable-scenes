using UnityEngine;

namespace CHARK.ScriptableScenes.Utilities
{
    /// <summary>
    /// Constants for <see cref="CreateAssetMenuAttribute"/>.
    /// </summary>
    internal static class CreateAssetMenuConstants
    {
        /// <summary>
        /// Base file name (prefix).
        /// </summary>
        internal const string BaseFileName = "New ";

        /// <summary>
        /// Base menu name (sub-menu category).
        /// </summary>
        internal const string BaseMenuName = "CHARK/Scriptable Scenes";

        /// <summary>
        /// Base menu order.
        /// </summary>
        internal const int BaseOrder = -1000;

        /// <summary>
        /// Order of transition menu items.
        /// </summary>
        internal const int TransitionOrder = BaseOrder + 20;
    }
}
