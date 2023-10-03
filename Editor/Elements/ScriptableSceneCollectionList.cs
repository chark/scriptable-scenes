using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionList : ListView
    {
        private readonly List<ScriptableSceneCollection> collections;

        internal event Action<ScriptableSceneCollection> OnOpenButtonClicked;

        internal event Action<ScriptableSceneCollection> OnPlayButtonClicked;

        internal event Action<ScriptableSceneCollection> OnLoadButtonClicked;

        internal event Action OnSortOrderChanged;

        internal ScriptableSceneCollectionList(List<ScriptableSceneCollection> collections)
        {
            this.collections = collections;

            showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            selectionType = SelectionType.None;
            reorderMode = ListViewReorderMode.Animated;
            reorderable = true;

            itemsSource = collections;
            makeItem = MakeItem;
            bindItem = BindItem;

            itemIndexChanged += OnInternalSortOrderChanged;
        }

        private ScriptableSceneCollectionListItem MakeItem()
        {
            var item = new ScriptableSceneCollectionListItem();

            item.OnOpenButtonClicked += OnInternalOpenButtonClicked;
            item.OnPlayButtonClicked += OnInternalPlayButtonClicked;
            item.OnLoadButtonClicked += OnInternalLoadButtonClicked;

            return item;
        }

        private void BindItem(VisualElement element, int index)
        {
            var collection = collections[index];
            var item = (ScriptableSceneCollectionListItem)element;

            item.Bind(collection);
        }

        private void OnInternalSortOrderChanged(int srcIndex, int dstIndex)
        {
            OnSortOrderChanged?.Invoke();
        }

        private void OnInternalOpenButtonClicked(ScriptableSceneCollection collection)
        {
            OnOpenButtonClicked?.Invoke(collection);
        }

        private void OnInternalPlayButtonClicked(ScriptableSceneCollection collection)
        {
            OnPlayButtonClicked?.Invoke(collection);
        }

        private void OnInternalLoadButtonClicked(ScriptableSceneCollection collection)
        {
            OnLoadButtonClicked?.Invoke(collection);
        }
    }
}
