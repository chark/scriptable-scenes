using System.Collections;
using System.Collections.Generic;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.PropertyAttributes;
using CHARK.ScriptableScenes.Transitions;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;

namespace CHARK.ScriptableScenes
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(ScriptableSceneCollection),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Scriptable Scene Collection",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class ScriptableSceneCollection :
        BaseScriptableSceneCollection,
        ISerializationCallbackReceiver
    {
        #region Editor Fields

        [Header("Internal")]
        [ReadOnly]
        [Tooltip("Unique collection asset GUID")]
        [SerializeField]
        private string guid;

        [Header("Configuration")]
        [Tooltip("Optional transition used to transition into and out of this collection")]
        [SerializeField]
        private BaseScriptableSceneTransition transition;

        [Tooltip("List of Scriptable Scenes to be loaded with this collection")]
        [SerializeField]
        private List<BaseScriptableScene> scriptableScenes = new List<BaseScriptableScene>();

        [Header("Events")]
        [SerializeField]
        private CollectionEventHandler collectionEvents = new CollectionEventHandler();

        [SerializeField]
        private SceneEventHandler sceneEvents = new SceneEventHandler();

        #endregion

        #region Public Properties

        public override IEnumerable<BaseScriptableScene> Scenes => GetValidScriptableScenes();

        public override ICollectionEventHandler CollectionEvents => collectionEvents;

        public override ISceneEventHandler SceneEvents => sceneEvents;

        public override string Guid => guid;

        public override int SceneCount => scriptableScenes.Count;

        public override string Name => name;

        #endregion

        #region Unity Lifecycle

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (this.TryGetAssetGuid(out var newGuid))
            {
                guid = newGuid;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }

        #endregion

        #region Public Methods

        public override IEnumerator LoadRoutine()
        {
            collectionEvents.RaiseLoadEntered(this);
            yield return LoadInternalRoutine();
            collectionEvents.RaiseLoadExited(this);
        }

        public override IEnumerator UnloadRoutine()
        {
            collectionEvents.RaiseUnloadEntered(this);
            yield return UnloadInternalRoutine();
            collectionEvents.RaiseUnloadExited(this);
        }

        public override IEnumerator ShowTransitionRoutine()
        {
            if (transition)
            {
                collectionEvents.RaiseShowTransitionEntered();
                yield return transition.ShowRoutine();
                collectionEvents.RaiseShowTransitionExited();
            }
        }

        public override IEnumerator HideTransitionRoutine()
        {
            if (transition)
            {
                collectionEvents.RaiseHideTransitionEntered();
                yield return transition.HideRoutine();
                collectionEvents.RaiseHideTransitionExited();
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator LoadInternalRoutine()
        {
            var validScriptableScenes = GetValidScriptableScenes();
            var sceneCount = validScriptableScenes.Count;

            for (var index = 0; index < validScriptableScenes.Count; index++)
            {
                var totalLoadProgress = Mathf.Clamp01((float) index / sceneCount);
                var scriptableScene = validScriptableScenes[index];

                try
                {
                    scriptableScene.SceneEvents.AddListeners(sceneEvents);
                    scriptableScene.SceneEvents.OnLoadProgress += OnLoadProgress;

                    yield return scriptableScene.LoadRoutine();

                    if (scriptableScene.IsActivate)
                    {
                        scriptableScene.SetActive();
                    }
                }
                finally
                {
                    scriptableScene.SceneEvents.RemoveListeners(sceneEvents);
                    scriptableScene.SceneEvents.OnLoadProgress -= OnLoadProgress;
                }

                void OnLoadProgress(SceneLoadProgressEventArgs args)
                {
                    var collectionProgress = Mathf.Clamp01(
                        totalLoadProgress + args.Progress / sceneCount
                    );

                    collectionEvents.RaiseLoadProgress(
                        this,
                        scriptableScene,
                        collectionProgress,
                        args.Progress
                    );
                }
            }
        }

        private IEnumerator UnloadInternalRoutine()
        {
            foreach (var scriptableScene in GetValidScriptableScenes())
            {
                if (scriptableScene.IsPersist)
                {
                    continue;
                }

                try
                {
                    scriptableScene.SceneEvents.AddListeners(sceneEvents);
                    yield return scriptableScene.UnloadRoutine();
                }
                finally
                {
                    scriptableScene.SceneEvents.RemoveListeners(sceneEvents);
                }
            }
        }

        private IReadOnlyList<BaseScriptableScene> GetValidScriptableScenes()
        {
            var scenes = new List<BaseScriptableScene>();
            foreach (var scriptableScene in scriptableScenes)
            {
                // Scenes might ne null if the user adds a new scene and forgets to select an asset.
                if (scriptableScene)
                {
                    scenes.Add(scriptableScene);
                }
            }

            return scenes;
        }

        #endregion
    }
}
