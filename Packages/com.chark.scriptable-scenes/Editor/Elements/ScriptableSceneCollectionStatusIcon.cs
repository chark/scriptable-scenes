using CHARK.ScriptableScenes.Editor.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHARK.ScriptableScenes.Editor.Elements
{
    internal sealed class ScriptableSceneCollectionStatusIcon : Image
    {
        internal void Bind(ScriptableSceneCollection collection)
        {
            var status = collection.GetStatus();
            tooltip = GetStatusTooltip(status);
            image = GetStatusIcon(status);
        }

        private static string GetStatusTooltip(CollectionStatus status)
        {
            return status switch
            {
                CollectionStatus.MissingScenes => "This collection contains no scenes",
                CollectionStatus.InvalidBuildSettings => "One of the scenes in this collection are missing from Build Settings",
                CollectionStatus.Ready => "All scenes in this collection are added to Build Settings",
                _ => GetDefaultStatusTooltip(),
            };
        }

        private static Texture GetStatusIcon(CollectionStatus status)
        {
            return status switch
            {
                CollectionStatus.MissingScenes => ScriptableSceneEditorStyles.ErrorIcon,
                CollectionStatus.InvalidBuildSettings => ScriptableSceneEditorStyles.WarningIcon,
                CollectionStatus.Ready => ScriptableSceneEditorStyles.SuccessIcon,
                _ => GetDefaultStatusIcon(),
            };
        }

        private static string GetDefaultStatusTooltip()
        {
            return "Unknown status";
        }

        private static Texture GetDefaultStatusIcon()
        {
            return ScriptableSceneEditorStyles.QuestionMarkIcon;
        }
    }
}
