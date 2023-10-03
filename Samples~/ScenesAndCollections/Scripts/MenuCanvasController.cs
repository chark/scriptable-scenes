using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CHARK.ScriptableScenes
{
    internal sealed class MenuCanvasController : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField]
        private ScriptableSceneCollection playSceneCollection;

        [Header("Buttons")]
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button exitButton;

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
    }
}
