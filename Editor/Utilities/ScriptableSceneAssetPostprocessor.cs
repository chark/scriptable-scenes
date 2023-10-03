using UnityEditor;

namespace CHARK.ScriptableScenes.Editor.Utilities
{
    internal sealed class ScriptableSceneAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            if (importedAssets.Length == 0 && deletedAssets.Length == 0)
            {
                return;
            }

            ScriptableSceneEditorUtilities.TriggerEditorStateChange();
        }
    }
}
