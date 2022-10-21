using System.Collections;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Central scene controller which handles loading and unloading of
    /// <see cref="BaseScriptableSceneCollection"/>.
    /// </summary>
    [AddComponentMenu(
        AddComponentMenuConstants.BaseMenuName + "/Scriptable Scene Controller"
    )]
    public sealed class ScriptableSceneController : MonoBehaviour
    {
        #region Editor Fields

        private enum SceneLoadMode
        {
            [Tooltip("Scenes will not be loaded automatically")]
            // ReSharper disable once UnusedMember.Local
            None,

            [Tooltip("Automatically load scenes in Awake")]
            Awake,

            [Tooltip("Automatically load scenes in Start")]
            Start
        }

        // ReSharper disable once NotAccessedField.Local
        [Header("Configuration")]
        [Tooltip("Scene collection which is first to be loaded when the game runs in build mode")]
        [SerializeField]
        private BaseScriptableSceneCollection initialCollection;

        [Tooltip("Should and when " + nameof(initialCollection) + " be loaded?")]
        [SerializeField]
        private SceneLoadMode initialSceneLoadMode = SceneLoadMode.Start;

        [Header("Events")]
        [Tooltip("handler for global (invoked for all collections) collection events")]
        [SerializeField]
        private CollectionEventHandler collectionEvents = new CollectionEventHandler();

        [Tooltip("Handler for global (invoked for all scenes) scene events")]
        [SerializeField]
        private SceneEventHandler sceneEvents = new SceneEventHandler();

        #endregion

        #region Private Fields

        private BaseScriptableSceneCollection loadingCollection;
        private BaseScriptableSceneCollection loadedCollection;

        #endregion

        #region Public Properties

        /// <summary>
        /// Global events invoked for all <see cref="BaseScriptableSceneCollection"/> assets.
        /// </summary>
        public ICollectionEventHandler CollectionEvents => collectionEvents;

        /// <summary>
        /// Global events invoked for all <see cref="ScriptableScene"/> assets.
        /// </summary>
        public ISceneEventHandler SceneEvents => sceneEvents;

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// <c>true</c> if there currently a collection being loaded in
        /// <see cref="loadingCollection"/> or <c>false</c> otherwise.
        /// </summary>
        public bool IsLoading { get; private set; }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (initialSceneLoadMode != SceneLoadMode.Awake)
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
            if (initialSceneLoadMode != SceneLoadMode.Start)
            {
                return;
            }

#if UNITY_EDITOR
            LoadSelectedOrOpenedCollection();
#else
            LoadInitialSceneCollection();
#endif
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reloads <see cref="loadedCollection"/>.
        /// </summary>
        public void ReloadLoadedSceneCollection()
        {
            if (loadedCollection == false)
            {
                Debug.LogWarning(
                    $"No {nameof(BaseScriptableSceneCollection)} is loaded, reload will be ignored",
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
        public void LoadSceneCollection(BaseScriptableSceneCollection collection)
        {
            StartCoroutine(LoadSceneCollectionRoutine(collection));
        }

        /// <returns>
        /// <c>true</c> i`f a collection which is currently being loaded is retrieved or
        /// <c>false</c> otherwise.
        /// </returns>
        public bool TryGetLoadingSceneCollection(out BaseScriptableSceneCollection collection)
        {
            collection = loadingCollection;
            return collection != false;
        }

        /// <returns>
        /// <c>true</c> if a collection which is currently loaded is retrieved or <c>false</c>
        /// otherwise.
        /// </returns>
        public bool TryGetLoadedSceneCollection(out BaseScriptableSceneCollection collection)
        {
            collection = loadedCollection;
            return collection != false;
        }

        #endregion

        #region Private Methods

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
                $"Cannot load initial {nameof(BaseScriptableSceneCollection)}, make sure a valid " +
                $"{nameof(BaseScriptableSceneCollection)} exists which matches currently loaded " +
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

        private IEnumerator LoadSceneCollectionRoutine(BaseScriptableSceneCollection collection)
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
                    $"\"{loadingCollection.Name}\" is currently being loaded",
                    this
                );

                yield break;
            }

            loadingCollection = collection;
            IsLoading = true;

            try
            {
                yield return LoadSceneCollectionInternalRoutine(collection);
                loadedCollection = collection;
            }
            finally
            {
                loadingCollection = null;
                IsLoading = false;
            }
        }

        private IEnumerator LoadSceneCollectionInternalRoutine(
            BaseScriptableSceneCollection collection
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
                }
                finally
                {
                    collection.CollectionEvents.RemoveListeners(collectionEvents);
                    collection.SceneEvents.RemoveListeners(sceneEvents);
                }

                yield return collection.HideTransitionRoutine();
            }
            finally
            {
                collection.CollectionEvents.RemoveTransitionListeners(collectionEvents);
            }
        }

        #endregion
    }
}
