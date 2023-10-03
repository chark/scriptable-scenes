using System;
using System.Collections.Generic;
using System.Linq;
using CHARK.ScriptableScenes.Utilities;
using UnityEditor;

namespace CHARK.ScriptableScenes.Editor
{
    internal static class MenuItemExtensions
    {
        // MenuItemConstants.BaseWindowItemName + "/Scriptable Scene Manager",
        // priority = MenuItemConstants.BaseWindowPriority
        [MenuItem(
            MenuItemConstants.BaseCreateItemName + "/Scriptable Scene From Selection",
            priority = MenuItemConstants.BaseCreateItemPriority
        )]
        private static void CreateScriptableScene()
        {
            var selectedSceneAssets = GetSelectedSceneAssets();
            var sceneAsset = selectedSceneAssets.FirstOrDefault();
            if (sceneAsset == default)
            {
                return;
            }

            var scenePath = AssetDatabase.GetAssetOrScenePath(sceneAsset);
            var scriptableScene = ScriptableScene.CreateEditor(sceneAsset, scenePath);
            var scriptableScenePath = GetScriptableScenePath(sceneAsset, scenePath);

            SaveScriptableScene(scriptableScene, scriptableScenePath);
            SelectScriptableScene(scriptableScene);
        }

        [MenuItem(
            MenuItemConstants.BaseCreateItemName + "/Scriptable Scene From Selection",
            priority = MenuItemConstants.BaseCreateItemPriority,
            validate = true
        )]
        private static bool IsValidCreateScriptableScene()
        {
            var selectedSceneAssets = GetSelectedSceneAssets();
            return selectedSceneAssets.Count == 1;
        }

        private static string GetScriptableScenePath(SceneAsset sceneAsset, string scenePath)
        {
            var sceneName = sceneAsset.name;

            var scriptableScenePath = scenePath.Replace(
                $"{sceneName}.unity",
                $"{sceneName}_ScriptableScene.asset"
            );

            return AssetDatabase.GenerateUniqueAssetPath(scriptableScenePath);
        }

        private static IReadOnlyCollection<SceneAsset> GetSelectedSceneAssets()
        {
            var selectedObjects = Selection.objects;
            if (selectedObjects.Length == 0)
            {
                return Array.Empty<SceneAsset>();
            }

            var sceneAssets = new List<SceneAsset>();
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is SceneAsset sceneAsset)
                {
                    sceneAssets.Add(sceneAsset);
                }
            }

            return sceneAssets;
        }

        private static void SaveScriptableScene(ScriptableScene scriptableScene, string path)
        {
            AssetDatabase.CreateAsset(scriptableScene, path);
            AssetDatabase.SaveAssets();
        }

        private static void SelectScriptableScene(ScriptableScene scriptableScene)
        {
            Selection.activeObject = scriptableScene;
            EditorGUIUtility.PingObject(scriptableScene);
        }
    }
}
