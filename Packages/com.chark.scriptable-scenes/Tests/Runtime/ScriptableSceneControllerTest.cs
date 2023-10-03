using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CHARK.ScriptableScenes.Tests
{
    internal class ScriptableSceneControllerTests
    {
        private Scene testSetupScene;
        private Scene testMainScene;

        private ScriptableSceneController controller;

        [SetUp]
        public void SetUp()
        {
            testSetupScene = ScriptableSceneTestUtilities.CreateTestScene("TestScene_A");
            testMainScene = ScriptableSceneTestUtilities.CreateTestScene("TestScene_B");

            controller = ScriptableSceneTestUtilities.CreateController();
        }

        [UnityTest]
        public IEnumerator ShouldLoadSceneCollection()
        {
            // Given: collection with two scenes.
            var collection = ScriptableSceneTestUtilities.CreateCollection(
                new ScriptableSceneTestUtilities.SceneDefinition(testSetupScene),
                new ScriptableSceneTestUtilities.SceneDefinition(testMainScene)
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
                new ScriptableSceneTestUtilities.SceneDefinition(testSetupScene),
                new ScriptableSceneTestUtilities.SceneDefinition(testMainScene)
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
    }
}
