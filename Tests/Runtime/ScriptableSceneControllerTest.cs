using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace CHARK.ScriptableScenes.Tests.Runtime
{
    internal class ScriptableSceneControllerTests
    {
        private ScriptableSceneController controller;

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
                new ScriptableSceneTestUtilities.SceneDefinition(TestSceneId.TestSceneA),
                new ScriptableSceneTestUtilities.SceneDefinition(TestSceneId.TestSceneB)
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

        // TODO: UnloadSceneAsync() is not working in Unity 2022 during playmode tests
        [UnityTest]
        public IEnumerator ShouldReloadSceneCollection()
        {
            // Given: loaded collection with two scenes.
            var collection = ScriptableSceneTestUtilities.CreateCollection(
                new ScriptableSceneTestUtilities.SceneDefinition(TestSceneId.TestSceneA),
                new ScriptableSceneTestUtilities.SceneDefinition(TestSceneId.TestSceneB)
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
