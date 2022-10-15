﻿using System.Collections.Generic;
using System.Linq;
using CHARK.ScriptableScenes.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CHARK.ScriptableScenes.Utilities
{
    /// <summary>
    /// Utilities for interacting with <see cref="ScriptableSceneCollection"/> in Runtime.
    /// </summary>
    internal static class ScriptableSceneUtilities
    {
        #region Private Fields

        private static readonly string SelectedCollectionKey =
            typeof(ScriptableSceneUtilities).FullName + "_" + "Guids";

        #endregion

        #region Internal Methods

        /// <returns>
        /// <c>true</c> if scene information is retrieved for <paramref name="obj"/> or
        /// <c>false</c> otherwise.
        /// </returns>
        internal static bool TryGetSceneDetails(
            this Object obj,
            out string scenePath,
            out int sceneBuildIndex
        )
        {
            scenePath = string.Empty;
            sceneBuildIndex = -1;

#if UNITY_EDITOR
            if (Application.isPlaying || obj == false)
            {
                return false;
            }

            scenePath = UnityEditor.AssetDatabase.GetAssetPath(obj);
            sceneBuildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
            return true;
#else
            return false;
#endif
        }

        /// <returns>
        /// <c>true</c> if a GUID is retrieved for <paramref name="obj"/> or <c>false</c> otherwise.
        /// </returns>
        internal static bool TryGetAssetGuid(this Object obj, out string guid)
        {
            guid = string.Empty;

#if UNITY_EDITOR
            if (Application.isPlaying || obj == false)
            {
                return false;
            }

            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
            guid = UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);

            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Sets currently selected collection.
        /// </summary>
        internal static void SetSelectedCollection(BaseScriptableSceneCollection collection)
        {
#if UNITY_EDITOR
            if (collection)
            {
                UnityEditor.EditorPrefs.SetString(SelectedCollectionKey, collection.Guid);
            }
#endif
        }

        /// <summary>
        /// Clears currently selected collection.
        /// </summary>
        internal static void ClearSelectedCollection()
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.DeleteKey(SelectedCollectionKey);
#endif
        }

        /// <returns>
        /// <c>true</c> if collection which is selected in the Editor is retrieved or <c>false</c>
        /// otherwise.
        /// </returns>
        internal static bool TryGetSelectedCollection(
            out BaseScriptableSceneCollection collection
        )
        {
#if UNITY_EDITOR
            var guid = UnityEditor.EditorPrefs.GetString(SelectedCollectionKey);
            if (string.IsNullOrEmpty(guid))
            {
                collection = null;
                return false;
            }

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var guidCollection = UnityEditor.AssetDatabase
                .LoadAssetAtPath<BaseScriptableSceneCollection>(path);

            if (guidCollection == false)
            {
                collection = null;
                return false;
            }

            collection = guidCollection;
            return true;
#else
            collection = null;
            return false;
#endif
        }

        /// <returns>
        /// <c>true</c> if a collection which is currently loaded is retrieved or <c>false</c>
        /// otherwise.
        /// </returns>
        internal static bool TryGetLoadedCollection(out BaseScriptableSceneCollection collection)
        {
#if UNITY_EDITOR
            collection = UnityEditor.AssetDatabase
                .FindAssets($"t:{typeof(BaseScriptableSceneCollection)}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<BaseScriptableSceneCollection>)
                .FirstOrDefault(IsLoaded);

            return collection != false;
#else
            collection = null;
            return false;
#endif
        }

        /// <returns>
        /// <c>true</c> if a <paramref name="scene"/> is retrieved for given
        /// <paramref name="scriptableScene"/> or <c>false</c> otherwise.
        /// </returns>
        internal static bool TryGetLoadedScene(BaseScriptableScene scriptableScene, out Scene scene)
        {
            var targetSceneBuildIndex = scriptableScene.SceneBuildIndex;
            scene = default;

            foreach (var loadedScene in GetValidScenes())
            {
                var loadedSceneBuildIndex = loadedScene.buildIndex;
                if (targetSceneBuildIndex == loadedSceneBuildIndex)
                {
                    scene = loadedScene;
                    return true;
                }
            }

            return false;
        }

        // TODO: how can we avoid this?
        internal static void AddTransitionListeners(
            this ICollectionEventHandler handler,
            CollectionEventHandler otherHandler
        )
        {
            handler.OnShowTransitionEntered += otherHandler.RaiseShowTransitionEntered;
            handler.OnShowTransitionExited += otherHandler.RaiseShowTransitionExited;
            handler.OnHideTransitionEntered += otherHandler.RaiseHideTransitionEntered;
            handler.OnHideTransitionExited += otherHandler.RaiseHideTransitionExited;
        }

        // TODO: how can we avoid this?
        internal static void RemoveTransitionListeners(
            this ICollectionEventHandler handler,
            CollectionEventHandler otherHandler
        )
        {
            handler.OnShowTransitionEntered -= otherHandler.RaiseShowTransitionEntered;
            handler.OnShowTransitionExited -= otherHandler.RaiseShowTransitionExited;
            handler.OnHideTransitionEntered -= otherHandler.RaiseHideTransitionEntered;
            handler.OnHideTransitionExited -= otherHandler.RaiseHideTransitionExited;
        }

        // TODO: how can we avoid this?
        internal static void AddListeners(
            this ICollectionEventHandler handler,
            CollectionEventHandler otherHandler
        )
        {
            handler.OnLoadEntered += otherHandler.RaiseLoadEntered;
            handler.OnLoadExited += otherHandler.RaiseLoadExited;
            handler.OnLoadProgress += otherHandler.RaiseLoadProgress;
            handler.OnUnloadEntered += otherHandler.RaiseUnloadEntered;
            handler.OnUnloadExited += otherHandler.RaiseUnloadExited;
        }

        // TODO: how can we avoid this?
        internal static void RemoveListeners(
            this ICollectionEventHandler handler,
            CollectionEventHandler otherHandler
        )
        {
            handler.OnLoadEntered -= otherHandler.RaiseLoadEntered;
            handler.OnLoadExited -= otherHandler.RaiseLoadExited;
            handler.OnLoadProgress -= otherHandler.RaiseLoadProgress;
            handler.OnUnloadEntered -= otherHandler.RaiseUnloadEntered;
            handler.OnUnloadExited -= otherHandler.RaiseUnloadExited;
        }

        // TODO: how can we avoid this?
        internal static void AddListeners(
            this ISceneEventHandler handler,
            SceneEventHandler otherHandler
        )
        {
            handler.OnLoadEntered += otherHandler.RaiseLoadEntered;
            handler.OnLoadExited += otherHandler.RaiseLoadExited;
            handler.OnLoadProgress += otherHandler.RaiseLoadProgress;
            handler.OnUnloadEntered += otherHandler.RaiseUnloadEntered;
            handler.OnUnloadExited += otherHandler.RaiseUnloadExited;
            handler.OnActivateEntered += otherHandler.RaiseActivateEntered;
            handler.OnActivateExited += otherHandler.RaiseActivateExited;
        }

        // TODO: how can we avoid this?
        internal static void RemoveListeners(
            this ISceneEventHandler handler,
            SceneEventHandler otherHandler
        )
        {
            handler.OnLoadEntered -= otherHandler.RaiseLoadEntered;
            handler.OnLoadExited -= otherHandler.RaiseLoadExited;
            handler.OnLoadProgress -= otherHandler.RaiseLoadProgress;
            handler.OnUnloadEntered -= otherHandler.RaiseUnloadEntered;
            handler.OnUnloadExited -= otherHandler.RaiseUnloadExited;
            handler.OnActivateEntered -= otherHandler.RaiseActivateEntered;
            handler.OnActivateExited -= otherHandler.RaiseActivateExited;
        }

        #endregion

        #region Private Methods

        private static bool IsLoaded(BaseScriptableSceneCollection collection)
        {
            var scriptableSceneIndices = collection.Scenes
                .Select(scene => scene.SceneBuildIndex);

            var uniqueScriptableSceneIndices = new HashSet<int>(scriptableSceneIndices);

            var validSceneIndices = GetValidScenes().Select(scene => scene.buildIndex);
            var uniqueLoadedSceneIndices = new HashSet<int>(validSceneIndices);

            return uniqueScriptableSceneIndices.SetEquals(uniqueLoadedSceneIndices);
        }

        private static IEnumerable<Scene> GetValidScenes()
        {
            for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var scene = SceneManager.GetSceneAt(sceneIndex);
                if (scene.IsValid())
                {
                    yield return scene;
                }
            }
        }

        #endregion
    }
}
