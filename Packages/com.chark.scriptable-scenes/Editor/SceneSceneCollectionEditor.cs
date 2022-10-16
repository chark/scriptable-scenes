using System.Linq;
using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace CHARK.ScriptableScenes.Editor
{
    /// <summary>
    /// Custom inspector for <see cref="ScriptableSceneCollection"/>, used to draw debug buttons.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BaseScriptableSceneCollection), true)]
    internal class SceneSceneCollectionEditor : UnityEditor.Editor
    {
        #region Private Fields

        private BaseScriptableSceneCollection collection;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            collection = (BaseScriptableSceneCollection)target;
        }

        public override void OnInspectorGUI()
        {
            DrawBuildSettingsHelpBox();
            EditorGUILayout.Space();

            base.OnInspectorGUI();
            EditorGUILayout.Space();

            DrawNotAddedScenes();
            EditorGUILayout.Space();

            DrawControls();
        }

        #endregion

        #region Private Methods

        private void DrawBuildSettingsHelpBox()
        {
            if (collection.IsAddedToBuildSettings())
            {
                return;
            }

            ScriptableSceneGUI.WarningHelpBox(
                "Some Scriptable Scenes in this collection are not added to Build Settings"
            );
        }

        private void DrawNotAddedScenes()
        {
            if (collection.IsAddedToBuildSettings())
            {
                return;
            }

            var icon = ScriptableSceneEditorUtilities.GetBuildStatusWarningIcon();
            var content = new GUIContent("Scenes Not In Build Settings", icon);

            ScriptableSceneGUI.LabelField(content, EditorStyles.boldLabel);

            var scriptableScenes = collection.Scenes;
            foreach (var scriptableScene in scriptableScenes)
            {
                if (scriptableScene.IsAddedToBuildSettings())
                {
                    continue;
                }

                ScriptableSceneGUI.ObjectField(string.Empty, scriptableScene, false);
            }
        }

        private void DrawControls()
        {
            ScriptableSceneGUI.LabelField("Controls", EditorStyles.boldLabel);

            var isAddedScenes = collection.Scenes.Any();
            var isEnabled = GUI.enabled;

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying == false;
            DrawOpenButton();
            DrawPlayButton();

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying;
            DrawLoadButton();

            GUI.enabled = isEnabled;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawOpenButton()
        {
            if (ScriptableSceneGUI.Button("Open"))
            {
                collection.Open();
            }
        }

        private void DrawPlayButton()
        {
            if (ScriptableSceneGUI.Button("Play"))
            {
                collection.Play();
            }
        }

        private void DrawLoadButton()
        {
            if (ScriptableSceneGUI.Button("Load"))
            {
                collection.Load();
            }
        }

        #endregion
    }
}
