using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CHARK.ScriptableScenes
{
    internal sealed class MenuCanvasController : MonoBehaviour
    {
        #region Editor Fields

        [Header("Scenes")]
        [SerializeField]
        private BaseScriptableSceneCollection playSceneCollection;

        [Header("Buttons")]
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button exitButton;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            playButton.onClick.AddListener(HandlePlayButtonClick);
            exitButton.onClick.AddListener(HandleExitButtonClick);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(HandlePlayButtonClick);
            exitButton.onClick.RemoveListener(HandleExitButtonClick);
        }

        #endregion

        #region Private Methods

        private void HandlePlayButtonClick()
        {
            GameEvents.RaiseLoadScene(playSceneCollection);
        }

        private static void HandleExitButtonClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}
