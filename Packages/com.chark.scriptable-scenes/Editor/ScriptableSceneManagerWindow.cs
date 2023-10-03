using System.Collections.Generic;
using CHARK.ScriptableScenes.Editor.Elements;
using CHARK.ScriptableScenes.Editor.Utilities;
using CHARK.ScriptableScenes.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor
{
    /// <summary>
    /// Window used to manage all available <see cref="ScriptableSceneCollection"/> assets.
    /// </summary>
    internal sealed class ScriptableSceneManagerWindow : EditorWindow
    {
        [SerializeField]
        private StyleSheet styleSheet;

        private readonly List<ScriptableSceneCollection> sceneCollections = new();

        private ScriptableSceneCollectionGlobalActions globalActionsElement;
        private ScriptableSceneCollectionList collectionListElement;

        [MenuItem(
            MenuItemConstants.BaseWindowItemName + "/Scriptable Scenes",
            priority = MenuItemConstants.BaseWindowPriority
        )]
        private static void ShowWindow()
        {
            var sceneManagerWindow = GetWindow<ScriptableSceneManagerWindow>();
            sceneManagerWindow.titleContent = new GUIContent("Scriptable Scenes");

            var minSize = sceneManagerWindow.minSize;
            minSize.x = 250f;
            minSize.y = 100f;
            sceneManagerWindow.minSize = minSize;

            sceneManagerWindow.Show();
        }

        private void OnEnable()
        {
            ScriptableSceneEditorUtilities.OnEditorStateChanged += OnEditorStateChanged;

            InitializeUIElements();
            BindUIElements();
        }

        private void OnDisable()
        {
            ScriptableSceneEditorUtilities.OnEditorStateChanged -= OnEditorStateChanged;
        }

        private void InitializeUIElements()
        {
            globalActionsElement = new ScriptableSceneCollectionGlobalActions();
            globalActionsElement.OnStopButtonClicked += OnGlobalActionsStopButtonClicked;
            globalActionsElement.OnPauseButtonClicked += OnGlobalActionsPauseButtonClicked;
            globalActionsElement.OnStepButtonClicked += OnGlobalActionsStepButtonClicked;

            collectionListElement = new ScriptableSceneCollectionList(sceneCollections);
            collectionListElement.OnSortOrderChanged += CollectionListElementSortOrderChanged;
            collectionListElement.OnOpenButtonClicked += CollectionListElementOpenButtonClicked;
            collectionListElement.OnPlayButtonClicked += CollectionListElementPlayButtonClicked;
            collectionListElement.OnLoadButtonClicked += CollectionListElementLoadButtonClicked;

            rootVisualElement.styleSheets.Add(styleSheet);
            rootVisualElement.AddToClassList("window-content");
            rootVisualElement.Add(globalActionsElement);
            rootVisualElement.Add(collectionListElement);
        }

        private void OnEditorStateChanged()
        {
            BindUIElements();
        }

        private static void OnGlobalActionsStopButtonClicked()
        {
            ScriptableSceneEditorUtilities.StopGame();
        }

        private static void OnGlobalActionsPauseButtonClicked()
        {
            ScriptableSceneEditorUtilities.SetPausedGame(EditorApplication.isPaused == false);
        }

        private static void OnGlobalActionsStepButtonClicked()
        {
            ScriptableSceneEditorUtilities.StepGame();
        }

        private void CollectionListElementSortOrderChanged()
        {
            for (var index = 0; index < sceneCollections.Count; index++)
            {
                var collection = sceneCollections[index];
                if (collection.GetDisplayOrder() == index)
                {
                    continue;
                }

                collection.SetDisplayOrder(index);
            }
        }

        private static void CollectionListElementOpenButtonClicked(ScriptableSceneCollection collection)
        {
            collection.Open();
        }

        private static void CollectionListElementPlayButtonClicked(ScriptableSceneCollection collection)
        {
            collection.Play();
        }

        private static void CollectionListElementLoadButtonClicked(ScriptableSceneCollection collection)
        {
            collection.Load();
        }

        private void BindUIElements()
        {
            globalActionsElement?.Bind();

            sceneCollections.Clear();
            sceneCollections.AddRange(ScriptableSceneEditorUtilities.GetScriptableSceneCollections());
            collectionListElement?.RefreshItems();
        }
    }
}
