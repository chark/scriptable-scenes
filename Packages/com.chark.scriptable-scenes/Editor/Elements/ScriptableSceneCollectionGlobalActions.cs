using System;
using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionGlobalActions : VisualElement
    {
        private const string PressedUssClassName = "pressed";

        private Button stopButton;
        private Button pauseButton;
        private Button stepButton;

        internal event Action OnStopButtonClicked;

        internal event Action OnPauseButtonClicked;

        internal event Action OnStepButtonClicked;

        internal ScriptableSceneCollectionGlobalActions()
        {
            InitializeStopButton();
            InitializePauseButton();
            InitializeStepButton();
        }

        internal void Bind()
        {
            var isPlaying = Application.isPlaying;
            var isPaused = EditorApplication.isPaused;

            stopButton.SetEnabled(isPlaying);
            pauseButton.SetEnabled(isPlaying);
            if (isPlaying && isPaused)
            {
                pauseButton.AddToClassList(PressedUssClassName);
            }
            else
            {
                pauseButton.RemoveFromClassList(PressedUssClassName);
            }

            stepButton.SetEnabled(isPlaying);
        }

        private void InitializeStopButton()
        {
            stopButton = new Button();
            var image = new Image
            {
                image = ScriptableSceneEditorStyles.StopButtonIcon,
            };

            stopButton.SetEnabled(false);
            stopButton.Add(image);
            stopButton.clicked += OnInternalStopButtonClicked;

            Add(stopButton);
        }

        private void InitializePauseButton()
        {
            pauseButton = new Button();
            var image = new Image
            {
                image = ScriptableSceneEditorStyles.PauseButtonIcon,
            };

            pauseButton.SetEnabled(false);
            pauseButton.Add(image);
            pauseButton.clicked += OnInternalPauseButtonClicked;

            Add(pauseButton);
        }

        private void InitializeStepButton()
        {
            stepButton = new Button();
            var image = new Image
            {
                image = ScriptableSceneEditorStyles.StepButtonIcon,
            };

            stepButton.SetEnabled(false);
            stepButton.Add(image);
            stepButton.clicked += OnInternalStepButtonClicked;

            Add(stepButton);
        }

        private void OnInternalStopButtonClicked()
        {
            OnStopButtonClicked?.Invoke();
        }

        private void OnInternalPauseButtonClicked()
        {
            OnPauseButtonClicked?.Invoke();
        }

        private void OnInternalStepButtonClicked()
        {
            OnStepButtonClicked?.Invoke();
        }
    }
}
