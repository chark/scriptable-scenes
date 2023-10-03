using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionListItem : VisualElement
    {
        private ScriptableSceneCollection collection;

        private ScriptableSceneCollectionFoldout foldoutElement;
        private ScriptableSceneCollectionStatusIcon statusIconElement;
        private ScriptableSceneCollectionActions actionsElement;
        private ScriptableSceneCollectionFoldoutContent contentElement;

        internal event Action<ScriptableSceneCollection> OnOpenButtonClicked;

        internal event Action<ScriptableSceneCollection> OnPlayButtonClicked;

        internal event Action<ScriptableSceneCollection> OnLoadButtonClicked;

        internal ScriptableSceneCollectionListItem()
        {
            InitializeFoldout();
            InitializeActions();
            InitializeContent();
        }

        internal void Bind(ScriptableSceneCollection newCollection)
        {
            var oldCollection = collection;

            collection = newCollection;

            if (oldCollection != newCollection)
            {
                this.Unbind();
                this.TrackSerializedObjectValue(
                    new SerializedObject(collection),
                    _ => Bind()
                );
            }

            Bind();
        }

        private void Bind()
        {
            foldoutElement.Bind(collection);
            statusIconElement.Bind(collection);
            actionsElement.Bind(collection);
            contentElement.Bind(collection);
        }

        private void InitializeFoldout()
        {
            foldoutElement = new ScriptableSceneCollectionFoldout();
            Add(foldoutElement);
        }

        private void InitializeActions()
        {
            statusIconElement = new ScriptableSceneCollectionStatusIcon();
            actionsElement = new ScriptableSceneCollectionActions();

            actionsElement.OnOpenButtonClicked += OnInternalOpenButtonClicked;
            actionsElement.OnPlayButtonClicked += OnInternalPlayButtonClicked;
            actionsElement.OnLoadButtonClicked += OnInternalLoadButtonClicked;

            foldoutElement.AddHeader(statusIconElement);
            foldoutElement.AddHeader(actionsElement);
        }

        private void InitializeContent()
        {
            contentElement = new ScriptableSceneCollectionFoldoutContent();
            foldoutElement.AddContent(contentElement);
        }

        private void OnInternalOpenButtonClicked()
        {
            OnOpenButtonClicked?.Invoke(collection);
        }

        private void OnInternalPlayButtonClicked()
        {
            OnPlayButtonClicked?.Invoke(collection);
        }

        private void OnInternalLoadButtonClicked()
        {
            OnLoadButtonClicked?.Invoke(collection);
        }
    }
}
