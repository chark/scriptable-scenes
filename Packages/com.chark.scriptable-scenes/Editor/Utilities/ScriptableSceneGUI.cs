using UnityEditor;
using UnityEngine;

namespace CHARK.ScriptableScenes.Editor.Utilities
{
    /// <summary>
    /// Wrappers for Unity GUI field and property drawing methods.
    /// </summary>
    // TODO: add Odin support
    // TODO: comment methods in this class
    internal static class ScriptableSceneGUI
    {
        #region Internal Methods

        internal static bool Toggle(bool isToggled, string label, GUIStyle style)
        {
            return GUILayout.Toggle(isToggled, label, style);
        }

        internal static bool Toggle(bool isToggled, GUIContent content, GUIStyle style)
        {
            return GUILayout.Toggle(isToggled, content, style);
        }

        internal static T ObjectField<T>(
            Rect rect,
            string label,
            T @object,
            bool isAllowSceneObjects
        ) where T : Object
        {
            var result = EditorGUI.ObjectField(
                rect,
                label,
                @object,
                typeof(T),
                isAllowSceneObjects
            );

            return (T) result;
        }

        internal static void LabelField(string label, GUIStyle style)
        {
            EditorGUILayout.LabelField(label, style);
        }

        internal static int IntField(Rect rect, GUIContent label, int value)
        {
            return EditorGUI.IntField(rect, label, value);
        }

        internal static bool Button(Rect rect, GUIContent content)
        {
            return GUI.Button(rect, content);
        }

        internal static bool Button(Rect rect, string text)
        {
            return GUI.Button(rect, text);
        }

        internal static bool Button(GUIContent content)
        {
            return GUILayout.Button(content, GUILayout.Width(20f));
        }

        internal static bool Button(string text)
        {
            return GUILayout.Button(text);
        }

        #endregion
    }
}
