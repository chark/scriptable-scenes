using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CHARK.ScriptableScenes.Events;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace CHARK.ScriptableScenes.Tests
{
    internal class ScriptableSceneControllerEventTest
    {
        private ScriptableSceneController controller;
        private ICollectionEventHandler collectionEvents;
        private ISceneEventHandler sceneEvents;

        private ScriptableSceneCollection collection;
        private ScriptableScene scene;

        [SetUp]
        public void SetUp()
        {
            controller = ScriptableSceneTestUtilities.CreateController();
            collectionEvents = controller.CollectionEvents;
            sceneEvents = controller.SceneEvents;

            var testScene = ScriptableSceneTestUtilities.CreateTestScene("TestScene");
            collection = ScriptableSceneTestUtilities.CreateCollection(
                new ScriptableSceneTestUtilities.SceneDefinition(testScene)
            );

            scene = collection.Scenes.First();
        }

        [UnityTest]
        public IEnumerator ShouldInvokeCollectionOnLoadEntered()
        {
            // Given: event handler with one listener.
            CollectionLoadEventArgs args = default;
            collectionEvents.OnLoadEntered += invokedArgs => args = invokedArgs;

            // When: loading a scene collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(collection, args.Collection);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeCollectionOnLoadExited()
        {
            // Given: event handler with one listener.
            CollectionLoadEventArgs args = default;
            collectionEvents.OnLoadExited += invokedArgs => args = invokedArgs;

            // When: loading a scene collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(collection, args.Collection);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeCollectionOnLoadProgress()
        {
            // Given: event handler with one listener.
            var argsList = new List<CollectionLoadProgressEventArgs>();
            collectionEvents.OnLoadProgress += invokedArgs => argsList.Add(invokedArgs);

            // When: loading a scene collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured and load progress should be 100%.
            var collections = argsList.Select(arg => arg.Collection);
            Assert.That(collections, Is.All.EqualTo(collection));

            var scenes = argsList.SelectMany(arg => arg.Collection.Scenes);
            Assert.That(scenes, Is.All.EqualTo(scene));

            var collectionLoadProgress = argsList.Select(arg => arg.CollectionLoadProgress).Max();
            Assert.AreEqual(1f, collectionLoadProgress);

            var sceneLoadProgress = argsList.Select(arg => arg.SceneLoadProgress).Max();
            Assert.AreEqual(1f, sceneLoadProgress);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeCollectionOnUnloadEntered()
        {
            // Given: event handler with one listener.
            CollectionUnloadEventArgs args = default;
            collectionEvents.OnUnloadEntered += invokedArgs => args = invokedArgs;

            // When: loading a scene collection twice (to unload the 1st one).
            yield return controller.LoadRoutine(collection);
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(collection, args.Collection);
        }


        [UnityTest]
        public IEnumerator ShouldInvokeCollectionOnUnloadExited()
        {
            // Given: event handler with one listener.
            CollectionUnloadEventArgs args = default;
            collectionEvents.OnUnloadExited += invokedArgs => args = invokedArgs;

            // When: loading a scene collection twice (to unload the 1st one).
            yield return controller.LoadRoutine(collection);
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(collection, args.Collection);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnLoadEntered()
        {
            // Given: event handler with one listener.
            SceneLoadEventArgs args = default;
            sceneEvents.OnLoadEntered += invokedArgs => args = invokedArgs;

            // When: loading a collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(scene, args.Scene);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnLoadExited()
        {
            // Given: event handler with one listener.
            SceneLoadEventArgs args = default;
            sceneEvents.OnLoadExited += invokedArgs => args = invokedArgs;

            // When: loading a collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(scene, args.Scene);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnLoadProgress()
        {
            // Given: event handler with one listener.
            var argsList = new List<SceneLoadProgressEventArgs>();
            sceneEvents.OnLoadProgress += invokedArgs => argsList.Add(invokedArgs);

            // When: loading a scene collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured and load progress should be 100%.
            var scenes = argsList.Select(arg => arg.Scene);
            Assert.That(scenes, Is.All.EqualTo(scene));

            var sceneLoadProgress = argsList.Select(arg => arg.Progress).Max();
            Assert.AreEqual(1f, sceneLoadProgress);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnUnloadEntered()
        {
            // Given: event handler with one listener.
            SceneUnloadEventArgs args = default;
            sceneEvents.OnUnloadEntered += invokedArgs => args = invokedArgs;

            // When: loading a scene collection twice (to unload the 1st one).
            yield return controller.LoadRoutine(collection);
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(scene, args.Scene);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnUnloadExited()
        {
            // Given: event handler with one listener.
            SceneUnloadEventArgs args = default;
            sceneEvents.OnUnloadExited += invokedArgs => args = invokedArgs;

            // When: loading a scene collection twice (to unload the 1st one).
            yield return controller.LoadRoutine(collection);
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(scene, args.Scene);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnActivateEntered()
        {
            // Given: event handler with one listener.
            SceneActivateEventArgs args = default;
            sceneEvents.OnActivateEntered += invokedArgs => args = invokedArgs;
            scene.SetField("isActivate", true);

            // When: loading a collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(scene, args.Scene);
        }

        [UnityTest]
        public IEnumerator ShouldInvokeSceneOnActivateExited()
        {
            // Given: event handler with one listener.
            SceneActivateEventArgs args = default;
            sceneEvents.OnActivateExited += invokedArgs => args = invokedArgs;
            scene.SetField("isActivate", true);

            // When: loading a collection.
            yield return controller.LoadRoutine(collection);

            // Then: event arguments should be captured.
            Assert.AreEqual(scene, args.Scene);
        }
    }
}
