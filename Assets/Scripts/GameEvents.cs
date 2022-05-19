using UnityEngine;

namespace CHARK.ScriptableScenes
{
    internal static class GameEvents
    {
        #region Private Fields

        private static ScriptableSceneController sceneController;

        #endregion

        #region Internal Methods

        internal static void RaiseReloadLoadedScene()
        {
            if (sceneController == false)
            {
                SetupSceneController();
            }

            sceneController.ReloadLoadedSceneCollection();
        }

        internal static void RaiseLoadScene(BaseScriptableSceneCollection collection)
        {
            if (sceneController == false)
            {
                SetupSceneController();
            }

            sceneController.LoadSceneCollection(collection);
        }

        #endregion

        #region Private Methods

        private static void SetupSceneController()
        {
            sceneController = Object.FindObjectOfType<ScriptableSceneController>();
        }

        #endregion
    }
}
