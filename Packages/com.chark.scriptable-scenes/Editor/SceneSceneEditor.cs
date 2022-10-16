using System.Linq;
using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

namespace CHARK.ScriptableScenes.Editor
{
    /// <summary>
    /// Custom inspector for <see cref="ScriptableSceneCollection"/>, used to draw debug buttons.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BaseScriptableScene), true)]
    internal class SceneSceneEditor : UnityEditor.Editor
    {
        #region Private Fields

        private BaseScriptableScene scriptableScene;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            scriptableScene = (BaseScriptableScene)target;
        }

        public override void OnInspectorGUI()
        {
            DrawBuildSettingsHelpBox();
            EditorGUILayout.Space();

            base.OnInspectorGUI();
            EditorGUILayout.Space();

            DrawControls();
        }

        #endregion

        #region Private Methods

        private void DrawBuildSettingsHelpBox()
        {
            if (scriptableScene.IsAddedToBuildSettings() == false)
            {
                ScriptableSceneGUI.WarningHelpBox(
                    "Scriptable Scene is not added to Build Settings"
                );
            }
        }

        private void DrawControls()
        {
            ScriptableSceneGUI.LabelField("Controls", EditorStyles.boldLabel);

            var isEnabled = GUI.enabled;

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = scriptableScene.IsAddedToBuildSettings() == false;
            DrawAddToBuildSettingsButton();
            GUI.enabled = isEnabled;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawAddToBuildSettingsButton()
        {
            if (ScriptableSceneGUI.Button("Add To Build Settings"))
            {
                var scriptableScenePath = scriptableScene.ScenePath;
                var scenes = EditorBuildSettings.scenes.ToList();

                for (var index = 0; index < scenes.Count; index++)
                {
                    var otherScene = scenes[index];
                    if (scriptableScenePath == otherScene.path)
                    {
                        otherScene.enabled = true;
                        scenes[index] = otherScene;
                        EditorBuildSettings.scenes = scenes.ToArray();
                        return;
                    }
                }

                var scenePath = scriptableScene.ScenePath;
                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                EditorBuildSettings.scenes = scenes.ToArray();
            }
        }

        private bool TryGetEditorBuildSettingsScene(out EditorBuildSettingsScene scene)
        {
            scene = null;

            var scriptableScenePath = scriptableScene.ScenePath;
            var scenes = EditorBuildSettings.scenes.ToList();

            foreach (var otherScene in scenes)
            {
                if (otherScene.path == scriptableScenePath)
                {
                    scene = otherScene;
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
