using UnityEngine;
using UnityEngine.UI;

namespace CHARK.ScriptableScenes.Samples.MultipleScenes
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    internal sealed class PauseCanvasController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private GraphicRaycaster graphicRaycaster;

        [SerializeField]
        private Canvas canvas;

        [Header("Scenes")]
        [SerializeField]
        private ScriptableSceneCollection menuSceneCollection;

        [Header("Buttons")]
        [SerializeField]
        private Button restartButton;

        [SerializeField]
        private Button exitButton;

        [Header("Keys")]
        [SerializeField]
        private KeyCode pauseKey = KeyCode.Escape;

        private void Start()
        {
            Hide();
        }

        private void OnEnable()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void Update()
        {
            if (Input.GetKeyDown(pauseKey))
            {
                if (IsVisible())
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }

        private static void OnRestartButtonClicked()
        {
            GameEvents.RaiseReloadLoadedScene();
        }

        private void OnExitButtonClicked()
        {
            GameEvents.RaiseLoadScene(menuSceneCollection);
        }

        private bool IsVisible()
        {
            return canvas.enabled;
        }

        private void Show()
        {
            SetIsVisible(true);
        }

        private void Hide()
        {
            SetIsVisible(false);
        }

        private void SetIsVisible(bool isVisible)
        {
            graphicRaycaster.enabled = isVisible;
            canvas.enabled = isVisible;
        }
    }
}
