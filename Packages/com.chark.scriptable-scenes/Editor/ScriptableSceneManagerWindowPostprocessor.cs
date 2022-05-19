using UnityEditor;

namespace CHARK.ScriptableScenes.Editor
{
    /// <summary>
    /// Reloads assigned <see cref="ScriptableSceneCollection"/> to
    /// <see cref="ScriptableSceneManagerWindow"/>.
    /// </summary>
    internal sealed class ScriptableSceneManagerWindowPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            if (EditorWindow.HasOpenInstances<ScriptableSceneManagerWindow>() == false)
            {
                return;
            }

            var window = EditorWindow.GetWindow<ScriptableSceneManagerWindow>();
            window.SetupWindow();
        }
    }
}
