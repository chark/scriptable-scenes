using CHARK.ScriptableScenes.Editor.Elements;
using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor
{
    /// <summary>
    /// Custom inspector for <see cref="ScriptableSceneCollection"/>, used to draw debug buttons.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableSceneCollection), true)]
    internal class ScriptableSceneCollectionEditor
#if ODIN_INSPECTOR
        : Sirenix.OdinInspector.Editor.OdinEditor
#else
        : UnityEditor.Editor
#endif
    {
        [SerializeField]
        private StyleSheet styleSheet;

        private const string ActionsUssClassName = "actions-container";

        private ScriptableSceneCollection collection;

        private ScriptableSceneCollectionStatus statusElement;
        private ScriptableSceneCollectionActions actionsElement;

#if ODIN_INSPECTOR
        protected override void OnEnable()
#else
        private void OnEnable()
#endif
        {
            collection = (ScriptableSceneCollection)target;
            ScriptableSceneEditorUtilities.OnEditorStateChanged += OnEditorStateChanged;
        }

#if ODIN_INSPECTOR
        protected override void OnDisable()
#else
        private void OnDisable()
#endif
        {
            ScriptableSceneEditorUtilities.OnEditorStateChanged -= OnEditorStateChanged;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var rootVisualElement = new VisualElement();
            rootVisualElement.styleSheets.Add(styleSheet);

            statusElement = new ScriptableSceneCollectionStatus();
            rootVisualElement.Add(statusElement);

            var defaultGui = new IMGUIContainer(OnInspectorGUI);
            rootVisualElement.Add(defaultGui);

            actionsElement = new ScriptableSceneCollectionActions();

            var actionsContainer = new VisualElement();
            actionsContainer.AddToClassList(ActionsUssClassName);
            actionsContainer.Add(new Label("Actions"));
            actionsContainer.Add(actionsElement);
            rootVisualElement.Add(actionsContainer);

            rootVisualElement.TrackSerializedObjectValue(
                serializedObject,
                _ => BindUIElements()
            );

            BindUIElements();

            return rootVisualElement;
        }

        private void OnEditorStateChanged()
        {
            BindUIElements();
        }

        private void BindUIElements()
        {
            statusElement?.Bind(collection);
            actionsElement?.Bind(collection);
        }

        public override void OnInspectorGUI()
        {
#if ODIN_INSPECTOR
            Tree.BeginDraw(true);
            Tree.DrawProperties();
            Tree.EndDraw();
#else
            DrawDefaultInspector();
#endif
        }
    }
}
