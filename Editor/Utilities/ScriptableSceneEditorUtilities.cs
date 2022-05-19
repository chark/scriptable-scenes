using System.Collections.Generic;
using System.Linq;
using CHARK.ScriptableScenes.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHARK.ScriptableScenes.Editor.Utilities
{
    /// <summary>
    /// General utilities for interacting with <see cref="ScriptableSceneCollection"/> assets in
    /// Editor scripts.
    /// </summary>
    internal static class ScriptableSceneEditorUtilities
    {
        #region Unity Lifecycle

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
        }

        #endregion

        #region Internal Methods

        /// <returns>
        /// Collection of all <see cref="ScriptableSceneCollection"/> assets in the project.
        /// </returns>
        internal static List<BaseScriptableSceneCollection> GetScriptableSceneCollections()
        {
            return AssetDatabase
                .FindAssets($"t:{typeof(BaseScriptableSceneCollection)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<BaseScriptableSceneCollection>)
                .OrderBy(collection => collection.GetDisplayOrder())
                .ThenBy(collection => collection.Name)
                .ToList();
        }

        /// <summary>
        /// Start playing the game using the given <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection"></param>
        internal static void Play(this BaseScriptableSceneCollection collection)
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"Must be in edit mode to play {collection.Name}");
                return;
            }

            var scriptableScenes = collection.Scenes.ToList();
            var scriptableScene = scriptableScenes.FirstOrDefault();

            if (scriptableScene == default)
            {
                return;
            }

            EditorSceneManager.playModeStartScene =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(scriptableScene.ScenePath);

            ScriptableSceneUtilities.SetSelectedCollection(collection);
            EditorApplication.isPlaying = true;
        }

        /// <summary>
        /// Load the given <paramref name="collection"/> during play mode.
        /// </summary>
        internal static void Load(this BaseScriptableSceneCollection collection)
        {
            if (Application.isPlaying == false)
            {
                Debug.LogWarning($"Must be in play mode to load {collection.Name}");
                return;
            }

            var sceneController = Object.FindObjectOfType<ScriptableSceneController>();
            if (sceneController)
            {
                sceneController.LoadSceneCollection(collection);
            }
            else
            {
                Debug.LogWarning($"{nameof(ScriptableSceneController)} is missing");
            }
        }

        /// <summary>
        /// Open the given <paramref name="collection"/> during edit mode.
        /// </summary>
        internal static void Open(this BaseScriptableSceneCollection collection)
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"Must be in edit mode to open {collection.Name}");
                return;
            }

            var scriptableScenes = collection.Scenes.ToList();
            if (scriptableScenes.Count == 0)
            {
                return;
            }

            for (var index = 0; index < scriptableScenes.Count; index++)
            {
                var scriptableScene = scriptableScenes[index];
                var scenePath = scriptableScene.ScenePath;

                Scene scene;
                if (index == 0)
                {
                    scene = EditorSceneManager.OpenScene(scenePath);
                }
                else
                {
                    scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }

                if (scriptableScene.IsActivate)
                {
                    SceneManager.SetActiveScene(scene);
                }
            }
        }

        /// <summary>
        /// Stop the game inside the Editor.
        /// </summary>
        internal static void StopGame()
        {
            EditorApplication.isPlaying = false;
        }

        /// <summary>
        /// Pause or unpause the game inside the Editor.
        /// </summary>
        internal static void SetPausedGame(bool isPaused)
        {
            EditorApplication.isPaused = isPaused;
        }

        /// <summary>
        /// Progress the game by one step inside the Editor. Note that this will pause the game if
        /// its unpaused!
        /// </summary>
        internal static void StepGame()
        {
            EditorApplication.Step();
            GUIUtility.ExitGUI();
        }

        /// <returns>
        /// The sort order at which to display the given <paramref name="collection"/> in Editor
        /// lists.
        /// </returns>
        internal static int GetDisplayOrder(this BaseScriptableSceneCollection collection)
        {
            var key = GeDisplayOrderKey(collection);
            return EditorPrefs.GetInt(key, 0);
        }

        /// <summary>
        /// Set the sort order at which to display the given <paramref name="collection"/> in Editor
        /// lists.
        /// </summary>
        internal static void SetDisplayOrder(
            this BaseScriptableSceneCollection collection,
            int order
        )
        {
            var key = GeDisplayOrderKey(collection);
            EditorPrefs.SetInt(key, order);
        }

        /// <returns>
        /// <c>true</c> if the collection should be expanded in Editor foldouts or <c>false</c>
        /// otherwise.
        /// </returns>
        internal static bool IsExpanded(this BaseScriptableSceneCollection collection)
        {
            var key = GetIsExpandedKey(collection);
            return EditorPrefs.GetBool(key, false);
        }

        /// <summary>
        /// Set if the given <paramref name="collection"/> should be expanded in Editor foldouts.
        /// </summary>
        internal static void SetExpanded(
            this BaseScriptableSceneCollection collection,
            bool isExpanded
        )
        {
            var key = GetIsExpandedKey(collection);
            EditorPrefs.SetBool(key, isExpanded);
        }

        #endregion

        #region Private Methods

        private static void HandlePlayModeStateChanged(PlayModeStateChange change)
        {
            if (change != PlayModeStateChange.EnteredEditMode)
            {
                return;
            }

            EditorSceneManager.playModeStartScene = null;
            ScriptableSceneUtilities.ClearSelectedCollection();
        }

        private static string GeDisplayOrderKey(BaseScriptableSceneCollection collection)
        {
            var prefix = typeof(ScriptableSceneUtilities).FullName;
            const string function = nameof(GeDisplayOrderKey);
            var target = collection.Guid;

            return $"{prefix}_{function}_{target}";
        }

        private static string GetIsExpandedKey(BaseScriptableSceneCollection collection)
        {
            var prefix = typeof(ScriptableSceneUtilities).FullName;
            const string function = nameof(GetIsExpandedKey);
            var target = collection.Guid;

            return $"{prefix}_{function}_{target}";
        }

        #endregion
    }
}
