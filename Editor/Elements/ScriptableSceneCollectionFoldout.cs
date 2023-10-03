using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionFoldout : Foldout
    {
        internal ScriptableSceneCollectionFoldout()
        {
            value = false;
        }

        internal void AddHeader(VisualElement element)
        {
            var toggle = this.Q<Toggle>();
            toggle.Add(element);
        }

        internal void AddContent(VisualElement element)
        {
            Add(element);
        }

        internal void Bind(ScriptableSceneCollection collection)
        {
            tooltip = collection.Name;
            text = collection.Name;
        }
    }
}
