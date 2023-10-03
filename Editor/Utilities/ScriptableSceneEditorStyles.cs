using UnityEditor;
using UnityEngine;

namespace CHARK.ScriptableScenes.Editor.Utilities
{
    internal static class ScriptableSceneEditorStyles
    {
        private static Texture stopButtonIcon;
        private static Texture pauseButtonIcon;
        private static Texture stepButtonIcon;
        private static Texture questionMarkIcon;
        private static Texture successIcon;
        private static Texture warningIcon;
        private static Texture errorIcon;

        internal static Texture StopButtonIcon
        {
            get
            {
                if (stopButtonIcon == false)
                {
                    stopButtonIcon = GetIcon("PreMatQuad");
                }

                return stopButtonIcon;
            }
        }

        internal static Texture PauseButtonIcon
        {
            get
            {
                if (pauseButtonIcon == false)
                {
                    pauseButtonIcon = GetIcon("PauseButton");
                }

                return pauseButtonIcon;
            }
        }

        internal static Texture StepButtonIcon
        {
            get
            {
                if (stepButtonIcon == false)
                {
                    stepButtonIcon = GetIcon("StepButton");
                }

                return stepButtonIcon;
            }
        }

        internal static Texture QuestionMarkIcon
        {
            get
            {
                if (questionMarkIcon == false)
                {
                    questionMarkIcon = GetIcon("P4_Conflicted@2x");
                }

                return questionMarkIcon;
            }
        }

        internal static Texture SuccessIcon
        {
            get
            {
                if (successIcon == false)
                {
                    successIcon = GetIcon("Installed");
                }

                return successIcon;
            }
        }

        internal static Texture WarningIcon
        {
            get
            {
                if (warningIcon == false)
                {
                    warningIcon = GetIcon("Warning");
                }

                return warningIcon;
            }
        }

        internal static Texture ErrorIcon
        {
            get
            {
                if (errorIcon == false)
                {
                    errorIcon = GetIcon("Error");
                }

                return errorIcon;
            }
        }

        private static Texture GetIcon(string iconName)
        {
            var iconContent = EditorGUIUtility.IconContent(iconName);
            var iconImage = iconContent.image;

            return iconImage;
        }
    }
}
