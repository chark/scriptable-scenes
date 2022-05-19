using System.Collections;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.PropertyAttributes;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHARK.ScriptableScenes
{
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(ScriptableScene),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Scriptable Scene",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    internal sealed class ScriptableScene :
        BaseScriptableScene,
        ISerializationCallbackReceiver
    {
        #region Editor Fields

        [Header("Internal")]
        [ReadOnly]
        [Tooltip("Build index of the scene")]
        [SerializeField]
        private int sceneBuildIndex;

        [ReadOnly]
        [Tooltip("Path to the scene asset")]
        [SerializeField]
        private string scenePath;

#if UNITY_EDITOR
        [Header("Configuration")]
        [SerializeField]
        private UnityEditor.SceneAsset sceneAsset;
#endif

        [Tooltip("Should this scene be activated on load?")]
        [SerializeField]
        private bool isActivate;

        [Tooltip("Should this scene persist between scene changes (never unloaded)?")]
        [SerializeField]
        private bool isPersist;

        [Header("Events")]
        [SerializeField]
        private SceneEventHandler sceneEvents = new SceneEventHandler();

        #endregion

        #region Public Properties

        public override ISceneEventHandler SceneEvents => sceneEvents;

        public override int SceneBuildIndex => sceneBuildIndex;

        public override string ScenePath => scenePath;

        public override bool IsActivate => isActivate;

        public override bool IsPersist => isPersist;

        public override string Name => name;

        #endregion

        #region Private Fields

        private const float MaxSceneLoadProgress = 0.9f;

        #endregion

        #region Unity Lifecycle

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (sceneAsset.TryGetSceneDetails(out var newScenePath, out var newSceneBuildIndex))
            {
                scenePath = newScenePath;
                sceneBuildIndex = newSceneBuildIndex;
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
            sceneEvents.RaiseLoadEntered(this);

            if (IsLoaded() == false)
            {
                yield return LoadInternalRoutine();
            }
            else
            {
                sceneEvents.RaiseLoadProgress(this, 1f);
            }

            sceneEvents.RaiseLoadExited(this);
        }

        public override IEnumerator UnloadRoutine()
        {
            sceneEvents.RaiseUnloadEntered(this);
            yield return UnloadInternalRoutine();
            sceneEvents.RaiseUnloadExited(this);
        }

        public override void SetActive()
        {
            sceneEvents.RaiseActivateEntered(this);
            SetActiveInternal();
            sceneEvents.RaiseActivateExited(this);
        }

        #endregion

        #region Private Methods

        private IEnumerator LoadInternalRoutine()
        {
            var operation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);

            while (operation.isDone == false)
            {
                // Scene load progress range is [0, 0.9], need to remap to [0, 1] range.
                var loadProgress = Mathf.Clamp01(operation.progress / MaxSceneLoadProgress);
                sceneEvents.RaiseLoadProgress(this, loadProgress);

                yield return null;
            }
        }

        private IEnumerator UnloadInternalRoutine()
        {
            yield return SceneManager.UnloadSceneAsync(sceneBuildIndex);
        }

        private void SetActiveInternal()
        {
            var scene = GetScene();
            SceneManager.SetActiveScene(scene);
        }

        private bool IsLoaded()
        {
            var scene = GetScene();
            return scene.isLoaded;
        }

        private Scene GetScene()
        {
            return SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
        }

        #endregion
    }
}
