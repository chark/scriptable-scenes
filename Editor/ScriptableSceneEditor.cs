using CHARK.ScriptableScenes.Editor.Utilities;
using CHARK.ScriptableScenes.Editor.Elements;
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
    [CustomEditor(typeof(ScriptableScene), true)]
    internal class ScriptableSceneEditor
#if ODIN_INSPECTOR
        : Sirenix.OdinInspector.Editor.OdinEditor
#else
        : UnityEditor.Editor
#endif
    {
        [SerializeField]
        private StyleSheet styleSheet;

        private ScriptableScene scriptableScene;

        private ScriptableSceneStatus statusElement;

#if ODIN_INSPECTOR
        protected override void OnEnable()
#else
        private void OnEnable()
#endif
        {
            scriptableScene = (ScriptableScene)target;
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

            statusElement = new ScriptableSceneStatus();
            statusElement.OnAddToBuildSettingsButtonClicked += OnAddToBuildSettingsButtonClicked;
            rootVisualElement.Add(statusElement);

            var defaultGui = new IMGUIContainer(OnInspectorGUI);
            rootVisualElement.Add(defaultGui);

            rootVisualElement.TrackSerializedObjectValue(
                serializedObject,
                _ => BindUIElements()
            );

            BindUIElements();

            return rootVisualElement;
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

        private void OnEditorStateChanged()
        {
            BindUIElements();
        }

        private void OnAddToBuildSettingsButtonClicked()
        {
            scriptableScene.AddToBuildSettings();
        }

        private void BindUIElements()
        {
            statusElement?.Bind(scriptableScene);
        }
    }
}
