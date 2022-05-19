using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace CHARK.ScriptableScenes.Tests
{
    internal class ScriptableSceneControllerTests
    {
        #region Private Fields

        private ScriptableSceneController controller;

        #endregion

        #region Public Methods

        [SetUp]
        public void SetUp()
        {
            controller = ScriptableSceneTestUtilities.CreateController();
        }

        [UnityTest]
        public IEnumerator ShouldLoadSceneCollection()
        {
            // Given: collection with two scenes.
            var collection = ScriptableSceneTestUtilities.CreateCollection(
                new ScriptableSceneTestUtilities.SceneDefinition { BuildIndex = 0 },
                new ScriptableSceneTestUtilities.SceneDefinition { BuildIndex = 1 }
            );

            // When: loading a collection.
            yield return controller.LoadRoutine(collection);

            // Then: collection should be loaded.
            if (controller.TryGetLoadedSceneCollection(out var loadedCollection))
            {
                Assert.AreEqual(collection, loadedCollection);
            }
            else
            {
                Assert.Fail($"Collection \"{collection.Name}\" was not loaded");
            }
        }

        [UnityTest]
        public IEnumerator ShouldReloadSceneCollection()
        {
            // Given: loaded collection with two scenes.
            var collection = ScriptableSceneTestUtilities.CreateCollection(
                new ScriptableSceneTestUtilities.SceneDefinition { BuildIndex = 0 },
                new ScriptableSceneTestUtilities.SceneDefinition { BuildIndex = 1 }
            );

            yield return controller.LoadRoutine(collection);

            // When: reloading loaded collection.
            yield return controller.ReloadRoutine();

            // Then: collection should be loaded.
            if (controller.TryGetLoadedSceneCollection(out var loadedCollection))
            {
                Assert.AreEqual(collection, loadedCollection);
            }
            else
            {
                Assert.Fail($"Collection \"{collection.Name}\" was not reloaded");
            }
        }

        #endregion
    }
}
