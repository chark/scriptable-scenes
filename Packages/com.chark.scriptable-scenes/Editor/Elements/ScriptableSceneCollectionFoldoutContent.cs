using UnityEditor.Search;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionFoldoutContent : VisualElement
    {
        private const string ReadonlyUssClassName = "readonly";

        private ObjectField objectField;

        internal ScriptableSceneCollectionFoldoutContent()
        {
            InitializeObjectField();
        }

        internal void Bind(ScriptableSceneCollection collection)
        {
            objectField.value = collection;
        }

        private void InitializeObjectField()
        {
            objectField = new ObjectField
            {
                label = "Scene Collection",
            };

            objectField.AddToClassList(ReadonlyUssClassName);

            Add(objectField);
        }
    }
}
