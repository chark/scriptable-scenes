using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionStatus : VisualElement
    {
        private HelpBox addToBuildSettingsHelpBoxElement;
        private HelpBox missingScenesHelpBoxElement;

        internal ScriptableSceneCollectionStatus()
        {
            InitializeAddToBuildSettingsHelpBox();
            InitializeMissingScenesHelpBox();
        }

        internal void Bind(ScriptableSceneCollection collection)
        {
            var status = collection.GetStatus();
            if (status == CollectionStatus.InvalidBuildSettings)
            {
                addToBuildSettingsHelpBoxElement.style.display = DisplayStyle.Flex;
            }
            else
            {
                addToBuildSettingsHelpBoxElement.style.display = DisplayStyle.None;
            }

            if (status == CollectionStatus.MissingScenes)
            {
                missingScenesHelpBoxElement.style.display = DisplayStyle.Flex;
            }
            else
            {
                missingScenesHelpBoxElement.style.display = DisplayStyle.None;
            }
        }

        private void InitializeAddToBuildSettingsHelpBox()
        {
            addToBuildSettingsHelpBoxElement = new HelpBox
            {
                messageType = HelpBoxMessageType.Warning,
                text = "Some Scriptable Scenes in this collection are not added to Build Settings",
            };

            Add(addToBuildSettingsHelpBoxElement);
        }

        private void InitializeMissingScenesHelpBox()
        {
            missingScenesHelpBoxElement = new HelpBox
            {
                messageType = HelpBoxMessageType.Error,
                text = "This collection has no scenes",
            };

            Add(missingScenesHelpBoxElement);
        }
    }
}
