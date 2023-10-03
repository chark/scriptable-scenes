using System;
using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneStatus : VisualElement
    {
        private HelpBox addToBuildSettingsHelpBox;
        private Button addToBuildSettingsButton;

        internal event Action OnAddToBuildSettingsButtonClicked;

        internal ScriptableSceneStatus()
        {
            InitializeAddToBuildSettingsHelpBox();
            InitializeAddToBuildSettingsButton();
        }

        internal void Bind(ScriptableScene scene)
        {
            if (scene.IsAddedToBuildSettings())
            {
                addToBuildSettingsHelpBox.style.display = DisplayStyle.None;
                addToBuildSettingsButton.style.display = DisplayStyle.None;
            }
            else
            {
                addToBuildSettingsHelpBox.style.display = DisplayStyle.Flex;
                addToBuildSettingsButton.style.display = DisplayStyle.Flex;
            }
        }

        private void InitializeAddToBuildSettingsHelpBox()
        {
            addToBuildSettingsHelpBox = new HelpBox
            {
                messageType = HelpBoxMessageType.Warning,
                text = "This Scriptable Scene is not added to Build Settings",
            };

            Add(addToBuildSettingsHelpBox);
        }

        private void InitializeAddToBuildSettingsButton()
        {
            addToBuildSettingsButton = new Button
            {
                tooltip = "Add this scene to Build Settings",
                text = "Fix",
            };

            addToBuildSettingsButton.clicked += OnInternalAddToBuildSettingsButtonClicked;

            addToBuildSettingsHelpBox.Add(addToBuildSettingsButton);
        }

        private void OnInternalAddToBuildSettingsButtonClicked()
        {
            OnAddToBuildSettingsButtonClicked?.Invoke();
        }
    }
}
