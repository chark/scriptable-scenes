using System.Collections.Generic;
using System.IO;
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
        private static string SelectedCollectionKey
        {
            get
            {
                var projectAssetDirPath = Application.dataPath;
                var projectDirPath = Path.GetDirectoryName(projectAssetDirPath);

                // Project dir is used as id for editor keys, useful when using Parallel sync or similar.
                var projectId = string.IsNullOrWhiteSpace(projectAssetDirPath)
                    ? Application.productName
                    : Path.GetFileName(projectDirPath);

                return $"{nameof(ScriptableSceneUtilities)}.{projectId}.SelectedCollection";
            }
        }

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
        internal static void SetSelectedCollection(ScriptableSceneCollection collection)
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
            out ScriptableSceneCollection collection
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
                .LoadAssetAtPath<ScriptableSceneCollection>(path);

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
        /// <c>true</c> if a collection which is currently opened in the scene manager is retrieved
        /// or <c>false</c> otherwise.
        /// </returns>
        internal static bool TryGetOpenCollection(out ScriptableSceneCollection collection)
        {
#if UNITY_EDITOR
            collection = UnityEditor.AssetDatabase
                .FindAssets($"t:{typeof(ScriptableSceneCollection)}")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableSceneCollection>)
                .FirstOrDefault(IsOpen);

            return collection != false;
#else
            collection = null;
            return false;
#endif
        }

        /// <returns>
        /// <c>true</c> if <paramref name="collection"/> is currently open in the hierarchy or
        /// <c>false</c> otherwise.
        /// </returns>
        internal static bool IsOpen(this ScriptableSceneCollection collection)
        {
            var scriptableScenes = collection.Scenes.ToList();
            var loadedScenes = GetValidLoadedScenes();

            var matchingSceneCount = 0;
            foreach (var loadedScene in loadedScenes)
            {
                foreach (var scriptableScene in scriptableScenes)
                {
                    if (scriptableScene.Equals(loadedScene))
                    {
                        matchingSceneCount++;
                    }
                }
            }

            return matchingSceneCount == collection.SceneCount;
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

        private static IEnumerable<Scene> GetValidLoadedScenes()
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
    }
}
