using System;
using System.Collections.Generic;
using System.Linq;
using CHARK.ScriptableScenes.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CHARK.ScriptableScenes.Editor.Utilities
{
    /// <summary>
    /// General utilities for interacting with <see cref="ScriptableSceneCollection"/> assets in
    /// Editor scripts.
    /// </summary>
    internal static class ScriptableSceneEditorUtilities
    {
        private static readonly ISet<string> CurrentEnabledScenePaths = new HashSet<string>();
        private static readonly ISet<string> EditorEnabledScenePaths = new HashSet<string>();

        /// <summary>
        /// Called when editor state changes and Scriptable Scene editors should reload. This event
        /// will be called when:
        /// <ul>
        ///   <li>Play mode state changes</li>
        ///   <li>Pause state changes</li>
        ///   <li>Scene gets opened</li>
        ///   <li>Build settings change</li>
        ///   <li>An asset gets deleted or created</li>
        /// </ul>
        /// </summary>
        internal static event Action OnEditorStateChanged;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            EditorApplication.pauseStateChanged -= OnPauseStateChanged;
            EditorApplication.pauseStateChanged += OnPauseStateChanged;

            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneOpened += OnSceneOpened;

            EditorApplication.update -= OnEditorUpdated;
            EditorApplication.update += OnEditorUpdated;
        }

        /// <returns>
        /// Collection of all <see cref="ScriptableSceneCollection"/> assets in the project.
        /// </returns>
        internal static IEnumerable<ScriptableSceneCollection> GetScriptableSceneCollections()
        {
            return AssetDatabase
                .FindAssets($"t:{typeof(ScriptableSceneCollection)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableSceneCollection>)
                .OrderBy(collection => collection.GetDisplayOrder())
                .ThenBy(collection => collection.Name);
        }

        /// <summary>
        /// Start playing the game using the given <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection"></param>
        internal static void Play(this ScriptableSceneCollection collection)
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"Must be in edit mode to play {collection.Name}");
                return;
            }

            var scriptableScenes = collection.Scenes;
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
        internal static void Load(this ScriptableSceneCollection collection)
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
        internal static void Open(this ScriptableSceneCollection collection)
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning($"Must be in edit mode to open {collection.Name}");
                return;
            }

            var scriptableScenes = collection.Scenes.ToList();
            if (scriptableScenes.Count == 0)
            {
                Debug.LogWarning($"Collection {collection.Name} does not contain any scenes", collection);
                return;
            }

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false)
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
        internal static int GetDisplayOrder(this ScriptableSceneCollection collection)
        {
            var key = GeDisplayOrderKey(collection);
            return EditorPrefs.GetInt(key, 0);
        }

        /// <summary>
        /// Set the sort order at which to display the given <paramref name="collection"/> in Editor
        /// lists.
        /// </summary>
        internal static void SetDisplayOrder(
            this ScriptableSceneCollection collection,
            int order
        )
        {
            var key = GeDisplayOrderKey(collection);
            EditorPrefs.SetInt(key, order);
        }

        /// <returns>
        /// Status of given <paramref name="collection"/>.
        /// </returns>
        internal static CollectionStatus GetStatus(this ScriptableSceneCollection collection)
        {
            if (collection.SceneCount == 0)
            {
                return CollectionStatus.MissingScenes;
            }

            var scenes = collection.Scenes;
            foreach (var scene in scenes)
            {
                if (IsAddedToBuildSettings(scene) == false)
                {
                    return CollectionStatus.InvalidBuildSettings;
                }
            }

            return CollectionStatus.Ready;
        }

        /// <returns>
        /// <c>true</c> if given <paramref name="scene"/> is added to
        /// <see cref="EditorBuildSettings"/> and is enabled or <c>false</c> otherwise.
        /// </returns>
        internal static bool IsAddedToBuildSettings(this ScriptableScene scene)
        {
            return EditorBuildSettings.scenes
                .Where(otherScene => otherScene.enabled)
                .Any(otherScene => otherScene.path == scene.ScenePath);
        }

        /// <summary>
        /// Add given <paramref name="scene"/> to build settings.
        /// </summary>
        internal static void AddToBuildSettings(this ScriptableScene scene)
        {
            var scriptableScenePath = scene.ScenePath;
            var scenes = EditorBuildSettings.scenes.ToList();

            for (var index = 0; index < scenes.Count; index++)
            {
                var otherScene = scenes[index];
                if (scriptableScenePath == otherScene.path)
                {
                    otherScene.enabled = true;
                    scenes[index] = otherScene;
                    EditorBuildSettings.scenes = scenes.ToArray();
                    return;
                }
            }

            var scenePath = scene.ScenePath;
            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        /// <summary>
        /// Invoke <see cref="OnEditorStateChanged"/>.
        /// </summary>
        internal static void TriggerEditorStateChange()
        {
            OnEditorStateChanged?.Invoke();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorSceneManager.playModeStartScene = null;
                ScriptableSceneUtilities.ClearSelectedCollection();
            }

            TriggerEditorStateChange();
        }

        private static void OnPauseStateChanged(PauseState pauseState)
        {
            TriggerEditorStateChange();
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            TriggerEditorStateChange();
        }

        private static void OnEditorUpdated()
        {
            // Trying to avoid garbage with this comparison. Not fully sure if this is worth the
            // effort tho..
            EditorEnabledScenePaths.Clear();

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    EditorEnabledScenePaths.Add(scene.path);
                }
            }

            if (EditorEnabledScenePaths.SetEquals(CurrentEnabledScenePaths))
            {
                return;
            }

            CurrentEnabledScenePaths.Clear();
            foreach (var path in EditorEnabledScenePaths)
            {
                CurrentEnabledScenePaths.Add(path);
            }

            TriggerEditorStateChange();
        }

        private static string GeDisplayOrderKey(ScriptableSceneCollection collection)
        {
            var prefix = typeof(ScriptableSceneUtilities).FullName;
            const string function = nameof(GeDisplayOrderKey);
            var target = collection.Guid;

            return $"{prefix}_{function}_{target}";
        }
    }
}
