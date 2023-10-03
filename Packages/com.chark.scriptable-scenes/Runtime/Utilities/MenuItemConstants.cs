using UnityEditor;

namespace CHARK.ScriptableScenes.Utilities
{
    /// <summary>
    /// Constants for <see cref="MenuItem"/>.
    /// </summary>
    internal static class MenuItemConstants
    {
        /// <summary>
        /// Base menu item name for Window menu items.
        /// </summary>
        internal const string BaseWindowItemName = "Window/CHARK";

        /// <summary>
        /// Base menu item priority for Window menu items.
        /// </summary>
        internal const int BaseWindowPriority = -1000;

        /// <summary>
        /// Base menu item name for Create asset menu items.
        /// </summary>
        internal const string BaseCreateItemName = "Assets/Create/CHARK/Scriptable Scenes";

        /// <summary>
        /// Base menu item priority for Create asset menu items.
        /// </summary>
        internal const int BaseCreateItemPriority = -1000;
    }
}
