using CHARK.ScriptableScenes.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace CHARK.ScriptableScenes.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    internal class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = isEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
