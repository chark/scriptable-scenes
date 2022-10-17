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

        private static readonly GUILayoutOption PlayModeButtonWidth = GUILayout.Width(30f);

        private const float CollectionFieldMargin = 2f;
        private const float CollectionMargin = 8f;

        private const float CollectionTitleStatusWidth = 20f;
        private const float CollectionTitleButtonMargin = 2f;
        private const float CollectionTitleButtonWidth = 40f;

        private const float CollectionTitleActionsWidth =
            CollectionTitleStatusWidth +
            CollectionTitleButtonWidth * 3f +
            CollectionTitleButtonMargin * 2f;

        private const int CollectionFieldCount = 3;

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

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            DrawStopGameButton();
            DrawPauseGameButton();
            DrawStepGameButton();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUI.enabled = isEnabled;
        }

        private static void DrawStopGameButton()
        {
            var icon = ScriptableSceneEditorUtilities.GetStopButtonIcon();
            var iconContent = new GUIContent(icon, "Stop Game");

            if (ScriptableSceneGUI.Button(iconContent, PlayModeButtonWidth))
            {
                ScriptableSceneEditorUtilities.StopGame();
            }
        }

        private static void DrawPauseGameButton()
        {
            var icon = ScriptableSceneEditorUtilities.GetPauseButtonIcon();
            var iconContent = new GUIContent(icon, "Pause Game");

            EditorGUI.BeginChangeCheck();

            var isPaused = ScriptableSceneGUI.Toggle(
                EditorApplication.isPaused,
                iconContent,
                "Button",
                PlayModeButtonWidth
            );

            if (EditorGUI.EndChangeCheck())
            {
                ScriptableSceneEditorUtilities.SetPausedGame(isPaused);
            }
        }

        private static void DrawStepGameButton()
        {
            var icon = ScriptableSceneEditorUtilities.GetStepButtonIcon();
            var iconContent = new GUIContent(icon, "Step one frame");

            if (ScriptableSceneGUI.Button(iconContent, PlayModeButtonWidth))
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

            // ReSharper disable once InconsistentNaming
            float OnGetElementHeight(int index)
            {
                var collection = collections[index];
                return GetElementHeight(collection);
            }

            // ReSharper disable once InconsistentNaming
            void OnReorder(ReorderableList reorderableList)
            {
                for (var index = 0; index < collections.Count; index++)
                {
                    var collection = collections[index];
                    UpdateDisplayOrder(collection, index);
                }
            }

            // ReSharper disable once InconsistentNaming
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
                return EditorGUIUtility.singleLineHeight + CollectionFieldMargin;
            }

            return (EditorGUIUtility.singleLineHeight + CollectionFieldMargin)
                   * CollectionFieldCount
                   + CollectionTitleButtonMargin
                   + CollectionMargin;
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
            var fieldYOffset = EditorGUIUtility.singleLineHeight + CollectionFieldMargin;
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

            EditorGUI.indentLevel--;
        }

        private static bool DrawTitle(Rect rect, BaseScriptableSceneCollection collection)
        {
            EditorGUI.BeginChangeCheck();

            var titleTextWidth = rect.width - CollectionTitleActionsWidth;
            var titleText = ObjectNames.NicifyVariableName(collection.Name);
            var titleStyle = GetFoldoutTitleStyle(titleTextWidth);

            rect.width = titleTextWidth;
            var isExpanded = ScriptableSceneGUI.Foldout(
                rect,
                collection.IsExpanded(),
                titleText,
                titleStyle
            );

            rect.x += rect.width;
            DrawStatusIndicator(rect, collection);

            rect.x += CollectionTitleStatusWidth;
            DrawControls(rect, collection);

            if (EditorGUI.EndChangeCheck())
            {
                collection.SetExpanded(isExpanded);
            }

            return isExpanded;
        }

        private static void DrawStatusIndicator(Rect rect, BaseScriptableSceneCollection collection)
        {
            var icon = collection.GetBuildStatusIcon();
            var tooltip = collection.IsAddedToBuildSettings()
                ? "All scenes in this collection are added to Build Settings"
                : "One of the scenes in this collection are missing from Build Settings";

            var content = new GUIContent(icon, tooltip);

            rect.width = CollectionTitleStatusWidth;
            ScriptableSceneGUI.LabelField(rect, content);
        }

        private static void DrawControls(Rect rect, BaseScriptableSceneCollection collection)
        {
            var isAddedScenes = collection.Scenes.Any();
            var isEnabled = GUI.enabled;

            GUI.enabled = isEnabled
                          && isAddedScenes
                          && EditorApplication.isPlayingOrWillChangePlaymode == false;

            rect.width = CollectionTitleButtonWidth;
            DrawOpenButton(rect, collection);

            rect.x += CollectionTitleButtonWidth + CollectionTitleButtonMargin;
            DrawPlayButton(rect, collection);

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying;

            rect.x += CollectionTitleButtonWidth + CollectionTitleButtonMargin;
            DrawLoadButton(rect, collection);

            GUI.enabled = isEnabled;
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
            var content = new GUIContent(
                "Open",
                "Open all scenes in selected Scene Collection"
            );

            if (ScriptableSceneGUI.Button(rect, content))
            {
                collection.Open();
            }
        }

        private static void DrawPlayButton(Rect rect, BaseScriptableSceneCollection collection)
        {
            var content = new GUIContent(
                "Play",
                "Play the game in selected Scene Collection"
            );

            if (ScriptableSceneGUI.Button(rect, content))
            {
                collection.Play();
            }
        }

        private static void DrawLoadButton(Rect rect, BaseScriptableSceneCollection collection)
        {
            var content = new GUIContent(
                "Load",
                "Load scene collection through the Scene Controller (runtime)"
            );

            if (ScriptableSceneGUI.Button(rect, content))
            {
                collection.Load();
            }
        }

        private static GUIStyle GetFoldoutTitleStyle(float width)
        {
            return new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Clip,
                fixedWidth = width
            };
        }

        #endregion
    }
}
