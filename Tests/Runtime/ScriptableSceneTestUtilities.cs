using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARK.ScriptableScenes.Tests
{
    internal static class ScriptableSceneTestUtilities
    {
        #region Helper Classes

        /// <summary>
        /// Defines how a scene should be created.
        /// </summary>
        internal class SceneDefinition
        {
            internal int BuildIndex { get; set; }
        }

        #endregion

        #region Private Fields

        private const int MaxCollectionLoadWaitTicks = 120;

        #endregion

        #region Internal Methods

        /// <returns>
        /// New controller which can be used to load scenes.
        /// </returns>
        internal static ScriptableSceneController CreateController()
        {
            var gameObject = new GameObject(nameof(ScriptableSceneController));
            var controller = gameObject.AddComponent<ScriptableSceneController>();

            return controller;
        }

        /// <returns>
        /// New collection with a set of scenes specified in <paramref name="sceneDefinitions"/>.
        /// </returns>
        internal static BaseScriptableSceneCollection CreateCollection(
            params SceneDefinition[] sceneDefinitions
        )
        {
            var collection = ScriptableObject.CreateInstance<ScriptableSceneCollection>();
            collection.SetField("name", $"Test Collection ({Guid.NewGuid().ToString()})");

            var scriptableScenes = collection
                .GetValue<List<BaseScriptableScene>>("scriptableScenes");

            foreach (var sceneDefinition in sceneDefinitions)
            {
                var scene = ScriptableObject.CreateInstance<ScriptableScene>();
                scene.SetField("sceneBuildIndex", sceneDefinition.BuildIndex);

                scriptableScenes.Add(scene);
            }

            return collection;
        }

        /// <returns>
        /// Routine which loads provided <paramref name="collection"/> and waits for loading to
        /// finish.
        /// </returns>
        internal static IEnumerator LoadRoutine(
            this ScriptableSceneController controller,
            BaseScriptableSceneCollection collection
        )
        {
            controller.LoadSceneCollection(collection);
            yield return controller.WaitUntilLoadedRoutine();
        }


        /// <returns>
        /// Routine which reloads currently loaded scenes and waits for reload to finish.
        /// </returns>
        internal static IEnumerator ReloadRoutine(
            this ScriptableSceneController controller
        )
        {
            controller.ReloadLoadedSceneCollection();
            yield return controller.WaitUntilLoadedRoutine();
        }

        /// <returns>
        /// Routine which waits for scene loading to finish.
        /// </returns>
        internal static IEnumerator WaitUntilLoadedRoutine(
            this ScriptableSceneController controller
        )
        {
            var collectionLoadWaitTicks = 0;
            yield return new WaitUntil(() =>
            {
                if (controller.IsLoading == false)
                {
                    return true;
                }

                if (collectionLoadWaitTicks >= MaxCollectionLoadWaitTicks)
                {
                    return true;
                }

                collectionLoadWaitTicks++;
                return false;
            });
        }

        #endregion
    }
}
