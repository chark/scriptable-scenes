using System.Collections;
using System.Linq;
using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Reference to a <see cref="UnityEditor.SceneAsset"/>.
    /// </summary>
    [CreateAssetMenu(
        fileName = CreateAssetMenuConstants.BaseFileName + nameof(ScriptableScene),
        menuName = CreateAssetMenuConstants.BaseMenuName + "/Scriptable Scene",
        order = CreateAssetMenuConstants.BaseOrder
    )]
    public sealed class ScriptableScene : ScriptableObject, ISerializationCallbackReceiver
    {
#if UNITY_EDITOR

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#else
        [Header("General")]
#endif
        [SerializeField]
        private UnityEditor.SceneAsset sceneAsset;
#endif

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
        [Sirenix.OdinInspector.ReadOnly]
#else
        [CHARK.ScriptableScenes.PropertyAttributes.ReadOnly]
#endif

        [Tooltip("Path to the scene asset")]
        [SerializeField]
        private string scenePath;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#else
        [Header("Features")]
#endif
        [Tooltip("Should this scene be activated on load?")]
        [SerializeField]
        private bool isActivate;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Features", Expanded = true)]
#endif
        [Tooltip("Should this scene persist between scene changes (never unloaded)?")]
        [SerializeField]
        private bool isPersist;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Scene Events")]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#else
        [Header("Events")]
#endif
        [SerializeField]
        private SceneEventHandler sceneEvents = new();

        /// <summary>
        /// Name of this scene.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Path to the scene.
        /// </summary>
        public string ScenePath => scenePath;

        /// <summary>
        /// Should this scene be activated after loading.
        /// </summary>
        public bool IsActivate => isActivate;

        /// <summary>
        /// Should this scene persist between scene loads (never unloaded, useful for setup scenes).
        /// </summary>
        public bool IsPersist => isPersist;

        /// <summary>
        /// <c>true</c> if this scene is currently loaded or <c>false</c> otherwise.
        /// </summary>
        public bool IsLoaded => GetScene().isLoaded;

        /// <summary>
        /// <c>true</c> if this scene is valid or <c>false</c> otherwise.
        /// </summary>
        public bool IsValid => IsValidBuildIndex();

        /// <summary>
        /// Event handler assigned to this scene.
        /// </summary>
        public ISceneEventHandler SceneEvents => sceneEvents;

#if UNITY_EDITOR
        internal static ScriptableScene CreateEditor(
            UnityEditor.SceneAsset asset,
            string path
        )
        {
            var scriptableScene = CreateInstance<ScriptableScene>();
            scriptableScene.sceneAsset = asset;
            scriptableScene.scenePath = path;

            return scriptableScene;
        }
#endif

        private const float MaxSceneLoadProgress = 0.9f;

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            UpdateScenePath();
#endif
        }

        public void OnAfterDeserialize()
        {
        }

        /// <returns>
        /// Routine which loads this scene.
        /// </returns>
        public IEnumerator LoadRoutine()
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

        /// <returns>
        /// Routine which unloads this scene.
        /// </returns>
        public IEnumerator UnloadRoutine()
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

        /// <summary>
        /// Activate this scene via <see cref="SceneManager.SetActiveScene"/>.
        /// </summary>
        public void SetActive()
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

        /// <returns>
        /// <c>true</c> if this Scriptable Scene is pointing to <paramref name="otherScene"/>.
        /// </returns>
        public bool Equals(Scene otherScene)
        {
            var scene = GetScene();
            return scene == otherScene;
        }

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

        private bool IsValidBuildIndex()
        {
            if (string.IsNullOrWhiteSpace(scenePath))
            {
                return false;
            }

            return SceneUtility.GetBuildIndexByScenePath(scenePath) >= 0;
        }

        private Scene GetScene()
        {
            if (string.IsNullOrWhiteSpace(scenePath))
            {
                return new Scene
                {
                    name = "Invalid Scene",
                };
            }

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
    }
}
