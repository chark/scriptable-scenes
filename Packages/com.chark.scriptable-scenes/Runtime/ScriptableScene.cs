using System.Collections;
using System.Linq;
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

#if UNITY_EDITOR
        [Header("Scene")]
        [SerializeField]
        private UnityEditor.SceneAsset sceneAsset;
#endif

        [ReadOnly]
        [Tooltip("Path to the scene asset")]
        [SerializeField]
        private string scenePath;

        [Header("Configuration")]
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

        public override string Name => name;

        public override string ScenePath => scenePath;

        public override bool IsActivate => isActivate;

        public override bool IsPersist => isPersist;

        public override bool IsLoaded => GetScene().isLoaded;

        public override bool IsValid => GetScene().IsValid();

        public override ISceneEventHandler SceneEvents => sceneEvents;

        #endregion

        #region Private Fields

        private const float MaxSceneLoadProgress = 0.9f;

        #endregion

        #region Unity Lifecycle

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            UpdateScenePath();
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

            if (IsLoaded == false)
            {
#if UNITY_EDITOR
                // When scenes are loaded in Editor, in some cases we might want to load a
                // scene which is not added to build settings (e.g., during testing). So all checks
                // are ignored in this case.
                yield return LoadInternalRoutine();
#else
                // In the player, checking (not IsValid) by build since the scene might not be
                // loaded yet.
                if (IsValidBuildIndex())
                {
                    yield return LoadInternalRoutine();
                }
                else
                {
                    Debug.LogWarning(
                        $"Cannot load scene - invalid Scene at path \"{scenePath}\"",
                        this
                    );
                }
#endif
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
            if (IsLoaded)
            {
#if UNITY_EDITOR
                // Same as with scene loading, in Editor we want to unload even invalid scenes.
                yield return UnloadInternalRoutine();
#else
                // IsValid can be called as the scene had to be loaded to get to this point.
                if (IsValid)
                {
                    yield return UnloadInternalRoutine();
                }
                else
                {
                    Debug.LogWarning(
                        $"Cannot unload scene - invalid Scene at path \"{scenePath}\"",
                        this
                    );
                }
#endif
            }

            sceneEvents.RaiseUnloadExited(this);
        }

        public override void SetActive()
        {
            sceneEvents.RaiseActivateEntered(this);
#if UNITY_EDITOR
            // Same as with scene loading, in Editor we want to activate invalid scenes.
            SetActiveInternal();
#else
            // In Player at this point the scene should have been loaded and we can call IsValid.
            if (IsValid)
            {
                SetActiveInternal();
            }
            else
            {
                Debug.LogWarning(
                    $"Cannot activate scene - invalid Scene at path \"{scenePath}\"",
                    this
                );
            }
#endif

            sceneEvents.RaiseActivateExited(this);
        }

        public override bool Equals(Scene otherScene)
        {
            var scene = GetScene();
            return scene == otherScene;
        }

        #endregion

        #region Private Methods

#if UNITY_EDITOR
        private void UpdateScenePath()
        {
            if (sceneAsset.TryGetSceneDetails(out var newScenePath, out _) == false)
            {
                return;
            }

            var oldScenePath = scenePath;
            if (string.Equals(oldScenePath, newScenePath))
            {
                return;
            }

            scenePath = newScenePath;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
        }
#endif

        private IEnumerator LoadInternalRoutine()
        {
            var operation = StartLoadSceneOperation();

            AsyncOperation StartLoadSceneOperation()
            {
                var parameters = new LoadSceneParameters(LoadSceneMode.Additive);

#if UNITY_EDITOR
                // Allow to load scenes which are not added to Build Settings (Editor only).
                return UnityEditor.SceneManagement.EditorSceneManager
                    .LoadSceneAsyncInPlayMode(scenePath, parameters);
#else
                return SceneManager.LoadSceneAsync(scenePath, parameters);
#endif
            }

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
            yield return SceneManager.UnloadSceneAsync(scenePath);
        }

        private void SetActiveInternal()
        {
            var sceneByPath = SceneManager.GetSceneByPath(scenePath);
            SceneManager.SetActiveScene(sceneByPath);
        }

#if UNITY_EDITOR == false
        private bool IsValidBuildIndex()
        {
            return SceneUtility.GetBuildIndexByScenePath(scenePath) >= 0;
        }
#endif

        private Scene GetScene()
        {
            return SceneManager.GetSceneByPath(scenePath);
        }

#if UNITY_EDITOR
        private class ScenePathProcessor : UnityEditor.AssetPostprocessor
        {
            private static void OnPostprocessAllAssets(
                string[] importedAssets,
                string[] deletedAssets,
                string[] movedAssets,
                string[] movedFromAssetPaths
            )
            {
                var scriptableScenes = UnityEditor.AssetDatabase
                    .FindAssets($"t:{typeof(ScriptableScene)}")
                    .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                    .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableScene>);

                foreach (var scriptableScene in scriptableScenes)
                {
                    scriptableScene.UpdateScenePath();
                }
            }
        }
#endif

        #endregion
    }
}
