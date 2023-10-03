using System.Collections;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Central scene controller which handles loading and unloading of
    /// <see cref="ScriptableSceneCollection"/>.
    /// </summary>
    [AddComponentMenu(
        AddComponentMenuConstants.BaseMenuName + "/Scriptable Scene Controller"
    )]
    public sealed class ScriptableSceneController : MonoBehaviour
    {
        private enum CollectionLoadMode
        {
            [Tooltip("Collection will not be loaded automatically")]
            // ReSharper disable once UnusedMember.Local
            None,

            [Tooltip("Automatically load collection in Awake() method")]
            Awake,

            [Tooltip("Automatically load collection in Start() method")]
            Start,
        }

        // ReSharper disable once NotAccessedField.Local
        [Header("Configuration")]
        [Tooltip("Scene collection which is first to be loaded when the game runs in build mode")]
        [SerializeField]
        private ScriptableSceneCollection initialCollection;

        [Tooltip("Should and when " + nameof(initialCollection) + " be loaded?")]
        [FormerlySerializedAs("initialSceneLoadMode")]
        [SerializeField]
        private CollectionLoadMode initialCollectionLoadMode = CollectionLoadMode.Start;

        [Header("Events")]
        [Tooltip("handler for global (invoked for all collections) collection events")]
        [SerializeField]
        private CollectionEventHandler collectionEvents = new();

        [Tooltip("Handler for global (invoked for all scenes) scene events")]
        [SerializeField]
        private SceneEventHandler sceneEvents = new();

        private ScriptableSceneCollection loadedCollection;

        /// <summary>
        /// Global events invoked for all <see cref="ScriptableSceneCollection"/> assets.
        /// </summary>
        public ICollectionEventHandler CollectionEvents => collectionEvents;

        /// <summary>
        /// Global events invoked for all <see cref="ScriptableScene"/> assets.
        /// </summary>
        public ISceneEventHandler SceneEvents => sceneEvents;

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// <c>true</c> if there currently a collection being loaded in
        /// <see cref="LoadingCollection"/> or <c>false</c> otherwise.
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Currently loading collection.
        /// </summary>
        public ScriptableSceneCollection LoadingCollection { get; private set; }

        private void Awake()
        {
            if (initialCollectionLoadMode != CollectionLoadMode.Awake)
            {
                return;
            }

#if UNITY_EDITOR
            LoadSelectedOrOpenedCollection();
#else
            LoadInitialSceneCollection();
#endif
        }

        private void Start()
        {
            if (initialCollectionLoadMode != CollectionLoadMode.Start)
            {
                return;
            }

#if UNITY_EDITOR
            LoadSelectedOrOpenedCollection();
#else
            LoadInitialSceneCollection();
#endif
        }

        /// <summary>
        /// Reloads <see cref="loadedCollection"/>.
        /// </summary>
        public void ReloadLoadedSceneCollection()
        {
            if (loadedCollection == false)
            {
                Debug.LogWarning(
                    $"No {nameof(ScriptableSceneCollection)} is loaded, reload will be ignored",
                    this
                );

                return;
            }

            LoadSceneCollection(loadedCollection);
        }

        /// <summary>
        /// Load a set of scenes using the provided <paramref name="collection"/> and unload
        /// <see cref="loadedCollection"/>.
        /// </summary>
        public void LoadSceneCollection(ScriptableSceneCollection collection)
        {
            StartCoroutine(LoadSceneCollectionRoutine(collection));
        }

        /// <returns>
        /// <c>true</c> i`f a collection which is currently being loaded is retrieved or
        /// <c>false</c> otherwise.
        /// </returns>
        public bool TryGetLoadingSceneCollection(out ScriptableSceneCollection collection)
        {
            collection = LoadingCollection;
            return collection != false;
        }

        /// <returns>
        /// <c>true</c> if a collection which is currently loaded is retrieved or <c>false</c>
        /// otherwise.
        /// </returns>
        public bool TryGetLoadedSceneCollection(out ScriptableSceneCollection collection)
        {
            collection = loadedCollection;
            return collection != false;
        }

#if UNITY_EDITOR
        private void LoadSelectedOrOpenedCollection()
        {
            StartCoroutine(LoadSelectedOrOpenedCollectionRoutine());
        }

        private IEnumerator LoadSelectedOrOpenedCollectionRoutine()
        {
            if (ScriptableSceneUtilities.TryGetSelectedCollection(out var selected))
            {
                yield return LoadSceneCollectionRoutine(selected);
                yield break;
            }

            if (ScriptableSceneUtilities.TryGetOpenCollection(out var open))
            {
                yield return LoadSceneCollectionRoutine(open);
                yield break;
            }

            Debug.LogWarning(
                $"Cannot load initial {nameof(ScriptableSceneCollection)}, make sure a valid " +
                $"{nameof(ScriptableSceneCollection)} exists which matches currently loaded " +
                $"scenes, or use the Scene Manager Window",
                this
            );
        }
#else
        private void LoadInitialSceneCollection()
        {
            StartCoroutine(LoadInitialSceneCollectionRoutine());
        }

        private IEnumerator LoadInitialSceneCollectionRoutine()
        {
            if (initialCollection == false)
            {
                Debug.LogWarning(
                    $"{nameof(initialCollection)} is not set, initial scene setup will not be " +
                    $"loaded",
                    this
                );

                yield break;
            }

            yield return LoadSceneCollectionRoutine(initialCollection);
        }
#endif

        private IEnumerator LoadSceneCollectionRoutine(ScriptableSceneCollection collection)
        {
            if (collection.SceneCount == 0)
            {
                Debug.LogWarning(
                    $"Collection \"{collection.Name}\" does not contain any scenes! Load will be " +
                    $"ignored",
                    this
                );

                yield break;
            }

            if (IsLoading)
            {
                Debug.LogWarning(
                    $"Can't load two collections at the same time, collection " +
                    $"\"{LoadingCollection.Name}\" is currently being loaded",
                    this
                );

                yield break;
            }

            LoadingCollection = collection;
            IsLoading = true;

            try
            {
                yield return LoadSceneCollectionInternalRoutine(collection);
                loadedCollection = collection;
            }
            finally
            {
                LoadingCollection = null;
                IsLoading = false;
            }
        }

        private IEnumerator LoadSceneCollectionInternalRoutine(
            ScriptableSceneCollection collection
        )
        {
            // TODO: reduce nesting
            try
            {
                collection.CollectionEvents.AddTransitionListeners(collectionEvents);
                yield return collection.ShowTransitionRoutine();

                if (loadedCollection != false)
                {
                    try
                    {
                        loadedCollection.CollectionEvents.AddListeners(collectionEvents);
                        loadedCollection.SceneEvents.AddListeners(sceneEvents);
                        yield return loadedCollection.UnloadRoutine();
                    }
                    finally
                    {
                        loadedCollection.CollectionEvents.RemoveListeners(collectionEvents);
                        loadedCollection.SceneEvents.RemoveListeners(sceneEvents);
                    }
                }

                try
                {
                    collection.CollectionEvents.AddListeners(collectionEvents);
                    collection.SceneEvents.AddListeners(sceneEvents);
                    yield return collection.LoadRoutine();

                    // TODO: scuffed
                    LightProbes.TetrahedralizeAsync();
                }
                finally
                {
                    collection.CollectionEvents.RemoveListeners(collectionEvents);
                    collection.SceneEvents.RemoveListeners(sceneEvents);
                }

                yield return collection.DelayTransitionRoutine();
                yield return collection.HideTransitionRoutine();
            }
            finally
            {
                collection.CollectionEvents.RemoveTransitionListeners(collectionEvents);
            }
        }
    }
}
