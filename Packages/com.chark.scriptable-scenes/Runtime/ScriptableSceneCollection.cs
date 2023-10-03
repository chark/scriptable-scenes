using System.Collections;
using System.Collections.Generic;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.Transitions;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Collection of <see cref="ScriptableScene"/> which can be used to play, load or open a set
    /// of scenes at once.
    /// </summary>
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(ScriptableSceneCollection),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Scriptable Scene Collection",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    public sealed class ScriptableSceneCollection : ScriptableObject, ISerializationCallbackReceiver
    {

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
        [Sirenix.OdinInspector.ReadOnly]
#else
        [Header("General")]
        [PropertyAttributes.ReadOnly]
#endif
        [Tooltip("Unique collection asset GUID")]
        [SerializeField]
        private string guid;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#endif
        [Tooltip("User-friendly name for this collection")]
        [SerializeField]
        private string prettyName;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#else
        [Header("Features")]
#endif
        [Tooltip("Optional transition used to transition into and out of this collection")]
        [SerializeField]
        private ScriptableSceneTransition transition;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.PropertySpace]
        [Sirenix.OdinInspector.ListDrawerSettings(DefaultExpandedState = true)]
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#endif
        [Tooltip("List of Scriptable Scenes to be loaded with this collection")]
        [SerializeField]
        private List<ScriptableScene> scriptableScenes = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Collection Events")]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#else
        [Header("Events")]
#endif
        [SerializeField]
        private CollectionEventHandler collectionEvents = new();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Scene Events")]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        private SceneEventHandler sceneEvents = new();

        /// <summary>
        /// Unique collection id.
        /// </summary>
        public string Guid => guid;

        /// <summary>
        /// Name of this collection.
        /// </summary>
        public string Name
        {
            get
            {
                var trimmedPrettyName = prettyName?.Trim();
                if (string.IsNullOrEmpty(trimmedPrettyName))
                {
                    return name;
                }

                return trimmedPrettyName;
            }
        }

        /// <summary>
        /// Count of <see cref="ScriptableScene"/> in this collection.
        /// </summary>
        public int SceneCount => GetValidScriptableScenes().Count;

        /// <summary>
        /// Available <see cref="ScriptableScene"/> in this collection.
        /// </summary>
        public IEnumerable<ScriptableScene> Scenes => GetValidScriptableScenes();

        /// <summary>
        /// Collection events invoked on this collection.
        /// </summary>
        public ICollectionEventHandler CollectionEvents => collectionEvents;

        /// <summary>
        /// Scene events invoked on scenes of this collection.
        /// </summary>
        public ISceneEventHandler SceneEvents => sceneEvents;

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

        /// <returns>
        /// Routine which loads this collection.
        /// </returns>
        public IEnumerator LoadRoutine()
        {
            collectionEvents.RaiseLoadEntered(this);
            yield return LoadInternalRoutine();
            collectionEvents.RaiseLoadExited(this);
        }

        /// <returns>
        /// Routine which unloads this collection.
        /// </returns>
        public IEnumerator UnloadRoutine()
        {
            collectionEvents.RaiseUnloadEntered(this);
            yield return UnloadInternalRoutine();
            collectionEvents.RaiseUnloadExited(this);
        }

        /// <returns>
        /// Routine which shows the transition of this collection.
        /// </returns>
        public IEnumerator ShowTransitionRoutine()
        {
            if (transition)
            {
                collectionEvents.RaiseShowTransitionEntered();
                yield return transition.ShowRoutine();
                collectionEvents.RaiseShowTransitionExited();
            }
        }

        /// <returns>
        /// Routine which delays the transition of this collection.
        /// </returns>
        public IEnumerator DelayTransitionRoutine()
        {
            if (transition)
            {
                yield return transition.DelayRoutine();
            }
        }

        /// <returns>
        /// Routine which hides the transition of this collection.
        /// </returns>
        public IEnumerator HideTransitionRoutine()
        {
            if (transition)
            {
                collectionEvents.RaiseHideTransitionEntered();
                yield return transition.HideRoutine();
                collectionEvents.RaiseHideTransitionExited();
            }
        }

        private IEnumerator LoadInternalRoutine()
        {
            var validScriptableScenes = GetValidScriptableScenes();
            var sceneCount = validScriptableScenes.Count;

            for (var index = 0; index < validScriptableScenes.Count; index++)
            {
                var totalLoadProgress = Mathf.Clamp01((float)index / sceneCount);
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

                // ReSharper disable once InconsistentNaming
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

        private IReadOnlyList<ScriptableScene> GetValidScriptableScenes()
        {
            var scenes = new List<ScriptableScene>();
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
    }
}
