using UnityEngine;

namespace CHARK.ScriptableScenes.Samples.MultipleScenes
{
    internal static class GameEvents
    {
        private static ScriptableSceneController sceneController;

        internal static void RaiseReloadLoadedScene()
        {
            if (sceneController == false)
            {
                SetupSceneController();
            }

            sceneController.ReloadLoadedSceneCollection();
        }

        internal static void RaiseLoadScene(ScriptableSceneCollection collection)
        {
            if (sceneController == false)
            {
                SetupSceneController();
            }

            sceneController.LoadSceneCollection(collection);
        }

        private static void SetupSceneController()
        {
            sceneController = Object.FindObjectOfType<ScriptableSceneController>();
        }
    }
}
