using CHARK.ScriptableScenes.Events;
using CHARK.ScriptableScenes.Utilities;
using UnityEngine;

namespace CHARK.ScriptableScenes
{
    /// <summary>
    /// Component which can be used to print debug logs for a
    /// <see cref="ScriptableSceneController"/>.
    /// </summary>
    [AddComponentMenu(
        AddComponentMenuConstants.BaseMenuName + "/Scriptable Scene Controller Debugger"
    )]
    [RequireComponent(typeof(ScriptableSceneController))]
    internal sealed class ScriptableSceneControllerDebugger : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#else
        [Header("General")]
#endif
        [SerializeField]
        private bool isDebugCollectionEvents = true;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("General", Expanded = true)]
#endif
        [SerializeField]
        private bool isDebugSceneEvents = true;

        private ScriptableSceneController controller;

        private void Awake()
        {
            controller = GetComponent<ScriptableSceneController>();
        }

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            if (isDebugCollectionEvents)
            {
                var collectionEvents = controller.CollectionEvents;
                collectionEvents.OnLoadEntered += DebugLoadEntered;
                collectionEvents.OnLoadExited += DebugLoadExited;
                collectionEvents.OnLoadProgress += DebugLoadProgress;
                collectionEvents.OnUnloadEntered += DebugUnloadEntered;
                collectionEvents.OnUnloadExited += DebugUnloadExited;
            }

            if (isDebugSceneEvents)
            {
                var sceneEvents = controller.SceneEvents;
                sceneEvents.OnLoadEntered += DebugLoadEntered;
                sceneEvents.OnLoadExited += DebugLoadExited;
                sceneEvents.OnLoadProgress += DebugLoadProgress;
                sceneEvents.OnUnloadEntered += DebugUnloadEntered;
                sceneEvents.OnUnloadExited += DebugUnloadExited;
                sceneEvents.OnActivateEntered += DebugActivateEntered;
                sceneEvents.OnActivateExited += DebugActivateExited;
            }
        }

        private void RemoveListeners()
        {
            var collectionEvents = controller.CollectionEvents;
            collectionEvents.OnLoadEntered -= DebugLoadEntered;
            collectionEvents.OnLoadExited -= DebugLoadExited;
            collectionEvents.OnLoadProgress -= DebugLoadProgress;
            collectionEvents.OnUnloadEntered -= DebugUnloadEntered;
            collectionEvents.OnUnloadExited -= DebugUnloadExited;

            var sceneEvents = controller.SceneEvents;
            sceneEvents.OnLoadEntered -= DebugLoadEntered;
            sceneEvents.OnLoadExited -= DebugLoadExited;
            sceneEvents.OnLoadProgress -= DebugLoadProgress;
            sceneEvents.OnUnloadEntered -= DebugUnloadEntered;
            sceneEvents.OnUnloadExited -= DebugUnloadExited;
            sceneEvents.OnActivateEntered -= DebugActivateEntered;
            sceneEvents.OnActivateExited -= DebugActivateExited;
        }

        private void DebugLoadEntered(CollectionLoadEventArgs args)
        {
            Log(
                "Collection Load Entered",
                $"Name: {args.Collection.Name}"
            );
        }

        private void DebugLoadExited(CollectionLoadEventArgs args)
        {
            Log(
                "Collection Load Exited",
                $"Name: {args.Collection.Name}"
            );
        }

        private void DebugLoadProgress(CollectionLoadProgressEventArgs args)
        {
            Log(
                "Collection Load Progress",
                $"Collection Name: {args.Collection.Name} ({args.CollectionLoadProgress * 100f}%)",
                $"Scene Name: {args.Scene.Name} ({args.SceneLoadProgress * 100f}%)"
            );
        }

        private void DebugUnloadEntered(CollectionUnloadEventArgs args)
        {
            Log(
                "Collection Unload Entered",
                $"Name: {args.Collection.Name}"
            );
        }

        private void DebugUnloadExited(CollectionUnloadEventArgs args)
        {
            Log(
                "Collection Unload Exited",
                $"Name: {args.Collection.Name}"
            );
        }

        private void DebugLoadEntered(SceneLoadEventArgs args)
        {
            Log(
                "Scene Load Entered",
                $"Name: {args.Scene.Name}"
            );
        }

        private void DebugLoadExited(SceneLoadEventArgs args)
        {
            Log(
                "Scene Load Exited",
                $"Name: {args.Scene.Name}"
            );
        }

        private void DebugLoadProgress(SceneLoadProgressEventArgs args)
        {
            Log(
                $"Scene Load Progress",
                $"Scene Name: {args.Scene.Name} ({args.Progress * 100f}%)"
            );
        }

        private void DebugUnloadEntered(SceneUnloadEventArgs args)
        {
            Log(
                "Scene Unload Entered",
                $"Name: {args.Scene.Name}"
            );
        }

        private void DebugUnloadExited(SceneUnloadEventArgs args)
        {
            Log(
                "Scene Unload Exited",
                $"Name: {args.Scene.Name}"
            );
        }

        private void DebugActivateEntered(SceneActivateEventArgs args)
        {
            Log(
                "Scene Activate Entered",
                $"Name: {args.Scene.Name}"
            );
        }

        private void DebugActivateExited(SceneActivateEventArgs args)
        {
            Log(
                "Scene Activate Exited",
                $"Name: {args.Scene.Name}"
            );
        }

        private void Log(string title, params string[] details)
        {
#if UNITY_EDITOR
            Debug.Log(
                $"<b><color=cyan>{title}</color></b>\n" +
                $"{string.Join(", ", details)}",
                this
            );
#else
            Debug.Log(
                $"{title}\n" +
                $"{string.Join(", ", details)}",
                this
            );
#endif
        }
    }
}
