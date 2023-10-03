using System;
using System.Linq;
using CHARK.ScriptableScenes.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionActions : VisualElement
    {
        private Button openButton;
        private Button playButton;
        private Button loadButton;

        internal event Action OnOpenButtonClicked;

        internal event Action OnPlayButtonClicked;

        internal event Action OnLoadButtonClicked;

        internal ScriptableSceneCollectionActions()
        {
            InitializeOpenButton();
            InitializePlayButton();
            InitializeLoadButton();
        }

        internal void Bind(ScriptableSceneCollection collection)
        {
            var isCollectionOpen = collection.IsOpen();
            var isContainsScenes = collection.Scenes.Any();

            var isEditing = EditorApplication.isPlayingOrWillChangePlaymode == false;
            var isPlaying = Application.isPlaying;

            openButton.SetEnabled(isCollectionOpen == false && isContainsScenes && isEditing);
            playButton.SetEnabled(isContainsScenes && isEditing);
            loadButton.SetEnabled(isContainsScenes && isPlaying);
        }

        private void InitializeOpenButton()
        {
            openButton = new Button
            {
                text = "Open",
                tooltip = "Open all scenes in selected Scene Collection",
            };

            openButton.SetEnabled(false);
            openButton.clicked += OnInternalOpenButtonClicked;

            Add(openButton);
        }

        private void InitializePlayButton()
        {
            playButton = new Button
            {
                text = "Play",
                tooltip = "Play the game in selected Scene Collection",
            };

            playButton.SetEnabled(false);
            playButton.clicked += OnInternalPlayButtonClicked;

            Add(playButton);
        }

        private void InitializeLoadButton()
        {
            loadButton = new Button
            {
                text = "Load",
                tooltip = "Load scene collection through the Scene Controller (runtime)",
            };

            loadButton.SetEnabled(false);
            loadButton.clicked += OnInternalLoadButtonClicked;

            Add(loadButton);
        }

        private void OnInternalOpenButtonClicked()
        {
            OnOpenButtonClicked?.Invoke();
        }

        private void OnInternalPlayButtonClicked()
        {
            OnPlayButtonClicked?.Invoke();
        }

        private void OnInternalLoadButtonClicked()
        {
            OnLoadButtonClicked?.Invoke();
        }
    }
}
