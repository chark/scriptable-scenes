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
    internal class SceneReferenceCollectionEditor : UnityEditor.Editor
    {
        #region Private Fields

        private BaseScriptableSceneCollection sceneCollection;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            sceneCollection = (BaseScriptableSceneCollection) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            ScriptableSceneGUI.LabelField("Controls", EditorStyles.boldLabel);
            DrawControls(sceneCollection);
        }

        #endregion

        #region Private Methods

        private static void DrawControls(BaseScriptableSceneCollection collection)
        {
            var isAddedScenes = collection.Scenes.Any();
            var isEnabled = GUI.enabled;

            EditorGUILayout.BeginHorizontal();

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying == false;
            DrawOpenButton(collection);
            DrawPlayButton(collection);

            GUI.enabled = isEnabled && isAddedScenes && Application.isPlaying;
            DrawLoadButton(collection);

            EditorGUILayout.EndHorizontal();

            GUI.enabled = isEnabled;
        }

        private static void DrawOpenButton(BaseScriptableSceneCollection collection)
        {
            if (ScriptableSceneGUI.Button("Open"))
            {
                collection.Open();
            }
        }

        private static void DrawPlayButton(BaseScriptableSceneCollection collection)
        {
            if (ScriptableSceneGUI.Button("Play"))
            {
                collection.Play();
            }
        }

        private static void DrawLoadButton(BaseScriptableSceneCollection collection)
        {
            if (ScriptableSceneGUI.Button("Load"))
            {
                collection.Load();
            }
        }

        #endregion
    }
}
