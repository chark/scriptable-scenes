using UnityEngine;
using UnityEngine.UI;

namespace CHARK.ScriptableScenes
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    internal sealed class PauseCanvasController : MonoBehaviour
    {
        #region Editor Fields

        [Header("Scenes")]
        [SerializeField]
        private BaseScriptableSceneCollection menuSceneCollection;

        [Header("Buttons")]
        [SerializeField]
        private Button restartButton;

        [SerializeField]
        private Button exitButton;

        [Header("Keys")]
        [SerializeField]
        private KeyCode pauseKey = KeyCode.Escape;

        #endregion

        #region Private Fields

        private GraphicRaycaster graphicRaycaster;
        private Canvas canvas;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            canvas = GetComponent<Canvas>();
        }

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

        #endregion

        #region Private Methods

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

        #endregion
    }
}
