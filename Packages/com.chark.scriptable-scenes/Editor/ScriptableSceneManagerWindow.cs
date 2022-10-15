using System.Collections.Generic;
using System.Linq;
using CHARK.ScriptableScenes.Editor.Utilities;
using CHARK.ScriptableScenes.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CHARK.ScriptableScenes.Editor
{
    /// <summary>
    /// Window used to manage all available <see cref="ScriptableSceneCollection"/> assets.
    /// </summary>
    internal sealed class ScriptableSceneManagerWindow : EditorWindow
    {
        #region Editor Fields

        [SerializeField]
        private Vector2 scrollPosition;

        #endregion

        #region Private Fields

        private const float CollectionListFieldMargin = 2f;
        private const float CollectionListMargin = 8f;
        private const float CollectionListControlsExtraMargin = 2f;
        private const float CollectionListControlButtonMargin = 9f;

        private const int CollectionFieldCount = 4;

        private List<BaseScriptableSceneCollection> sceneCollections;
        private ReorderableList sceneCollectionsList;

        #endregion

        #region Unity Lifecycle

        [MenuItem(
            MenuItemConstants.BaseWindowItemName + "/Scriptable Scene Manager",
            priority = MenuItemConstants.BaseWindowPriority
        )]
        private static void ShowWindow()
        {
            var sceneManagerWindow = GetWindow<ScriptableSceneManagerWindow>();
            sceneManagerWindow.titleContent = new GUIContent("Scriptable Scene Manager");

            var minSize = sceneManagerWindow.minSize;
            minSize.x = 200f;
            minSize.y = 200f;
            sceneManagerWindow.minSize = minSize;

            sceneManagerWindow.Show();
        }

        private void OnEnable()
        {
            SetupWindow();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            DrawPlayModeControls();
            DrawSceneCollections();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Setup (reload) scene collections assigned to this window.
        /// </summary>
        internal void SetupWindow()
        {
            sceneCollections = ScriptableSceneEditorUtilities.GetScriptableSceneCollections();
            sceneCollectionsList = CreateSceneCollectionList(sceneCollections);
        }

        #endregion

        #region Private Play Mode Control Methods

        private static void DrawPlayModeControls()
        {
            var isEnabled = GUI.enabled;
            GUI.enabled = Application.isPlaying && isEnabled;

            // EditorGUILayout.BeginHorizontal(new GUIStyle
            // {
            //     alignment = TextAnchor.MiddleCenter
            // });

            DrawStopGameButton();
            DrawPauseGameButton();
            DrawStepGameButton();

            // EditorGUILayout.EndHorizontal();

            GUI.enabled = isEnabled;
        }

        private static void DrawStopGameButton()
        {
            var iconContent = EditorGUIUtility.IconContent(
                "PreMatQuad",
                "Stop game"
            );

            if (ScriptableSceneGUI.Button(iconContent))
            {
                ScriptableSceneEditorUtilities.StopGame();
            }
        }

        private static void DrawPauseGameButton()
        {
            var iconContent = EditorGUIUtility.IconContent(
                "PauseButton",
                "Pause game"
            );

            var isPausedOld = EditorApplication.isPaused;
            var isPausedNew = ScriptableSceneGUI.Toggle(isPausedOld, iconContent, "Button");
            if (isPausedOld != isPausedNew)
            {
                ScriptableSceneEditorUtilities.SetPausedGame(isPausedNew);
            }
        }

        private static void DrawStepGameButton()
        {
            var iconContent = EditorGUIUtility.IconContent(
                "StepButton",
                "Step game forward by one frame"
            );

            if (ScriptableSceneGUI.Button(iconContent))
            {
                ScriptableSceneEditorUtilities.StepGame();
            }
        }

        #endregion

        #region Private Scene Collection Methods

        private void DrawSceneCollections()
        {
            var margins = GetMarginStyle();
            EditorGUILayout.BeginHorizontal(margins);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            sceneCollectionsList.DoLayoutList();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndHorizontal();
        }

        private static GUIStyle GetMarginStyle()
        {
            var style = new GUIStyle(EditorStyles.inspectorDefaultMargins);
            var padding = style.padding;
            padding.left = 4;
            return style;
        }

        private static ReorderableList CreateSceneCollectionList(
            List<BaseScriptableSceneCollection> collections
        )
        {
            var list = new ReorderableList(
                collections,
                typeof(BaseScriptableSceneCollection),
                true,
                false,
                false,
                false
            )
            {
                elementHeightCallback = OnGetElementHeight,
                onReorderCallback = OnReorder,
                drawElementCallback = OnDrawElement
            };

            float OnGetElementHeight(int index)
            {
                var collection = collections[index];
                return GetElementHeight(collection);
            }

            void OnReorder(ReorderableList reorderableList)
            {
                for (var index = 0; index < collections.Count; index++)
                {
                    var collection = collections[index];
                    UpdateDisplayOrder(collection, index);
                }
            }

            void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                var collection = collections[index];
                DrawSceneCollection(rect, collection);
            }

            return list;
        }

        private static float GetElementHeight(BaseScriptableSceneCollection collection)
        {
            var isExpanded = collection.IsExpanded();
            if (isExpanded == false)
            {
                return EditorGUIUtility.singleLineHeight + CollectionListFieldMargin;
            }

            return (EditorGUIUtility.singleLineHeight + CollectionListFieldMargin)
                   * CollectionFieldCount
                   + CollectionListControlsExtraMargin
                   + CollectionListMargin;
        }

        private static void UpdateDisplayOrder(BaseScriptableSceneCollection collection, int index)
        {
            if (collection.GetDisplayOrder() == index)
            {
                return;
            }

            collection.SetDisplayOrder(index);
        }

        private static void DrawSceneCollection(Rect rect, BaseScriptableSceneCollection collection)
        {
            var fieldYOffset = EditorGUIUtility.singleLineHeight + CollectionListFieldMargin;
            var fieldRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

            var isExpanded = DrawTitle(fieldRect, collection);
            if (isExpanded == false)
            {
                return;
            }

            EditorGUI.indentLevel++;

            fieldRect.y += fieldYOffset;
            DrawAssetField(fieldRect, collection);

            fieldRect.y += fieldYOffset;
            DrawSceneCountField(fieldRect, collection);

            fieldRect.y += fieldYOffset + CollectionListControlsExtraMargin;
            DrawControls(EditorGUI.IndentedRect(fieldRect), collection);

            EditorGUI.indentLevel--;
        }

        private static void DrawControls(Rect rect, BaseScriptableSceneCollection collection)
        {
            var isAddedScenes = collection.Scenes.Any();
            var isEnabled = GUI.enabled;

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying == false;

            rect.width = rect.width / 3f - CollectionListControlButtonMargin / 3f;
            DrawOpenButton(rect, collection);

            rect.x += rect.width + CollectionListControlButtonMargin / 2f;
            DrawPlayButton(rect, collection);

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying;
            rect.x += rect.width + CollectionListControlButtonMargin / 2f;
            DrawLoadButton(rect, collection);

            GUI.enabled = isEnabled;
        }

        private static bool DrawTitle(Rect rect, BaseScriptableSceneCollection collection)
        {
            var name = collection.Name;
            var prettyName = ObjectNames.NicifyVariableName(name);

            var isExpanded = collection.IsExpanded();

            EditorGUI.BeginChangeCheck();

            var style = GetFoldoutTitleStyle();
            isExpanded = EditorGUI.Foldout(rect, isExpanded, prettyName, true, style);

            if (EditorGUI.EndChangeCheck())
            {
                collection.SetExpanded(isExpanded);
            }

            return isExpanded;
        }

        private static void DrawAssetField(Rect rect, BaseScriptableSceneCollection collection)
        {
            ScriptableSceneGUI.ObjectField(rect, "Scene Collection", collection, false);
        }

        private static void DrawSceneCountField(Rect rect, BaseScriptableSceneCollection collection)
        {
            var title = new GUIContent(
                "Scene Count",
                "Number of scenes added to this collection"
            );

            ScriptableSceneGUI.IntField(rect, title, collection.SceneCount);
        }

        private static void DrawOpenButton(Rect rect, BaseScriptableSceneCollection collection)
        {
            var iconContent = EditorGUIUtility.IconContent(
                "Folder Icon",
                "Open scene collection"
            );

            if (ScriptableSceneGUI.Button(rect, iconContent))
            {
                collection.Open();
            }
        }

        private static void DrawPlayButton(Rect rect, BaseScriptableSceneCollection collection)
        {
            var iconContent = EditorGUIUtility.IconContent(
                "PlayButton",
                "Run game in selected scene collection"
            );

            if (ScriptableSceneGUI.Button(rect, iconContent))
            {
                collection.Play();
            }
        }

        private static void DrawLoadButton(Rect rect, BaseScriptableSceneCollection collection)
        {
            var iconContent = EditorGUIUtility.IconContent(
                "SceneLoadIn",
                "Load scene collection (runtime)"
            );

            if (ScriptableSceneGUI.Button(rect, iconContent))
            {
                collection.Load();
            }
        }

        private static GUIStyle GetFoldoutTitleStyle()
        {
            return new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Clip
            };
        }

        #endregion
    }
}
