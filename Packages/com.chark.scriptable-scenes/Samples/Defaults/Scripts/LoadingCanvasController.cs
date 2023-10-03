using CHARK.ScriptableScenes.Events;
using TMPro;
using UnityEngine;

namespace CHARK.ScriptableScenes.Samples.Defaults
{
    internal sealed class LoadingCanvasController : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField]
        private ScriptableSceneController sceneController;

        [Header("Text")]
        [SerializeField]
        private TMP_Text loadingStateText;

        [SerializeField]
        private TMP_Text loadingPercentageText;

        private void OnEnable()
        {
            var collectionEvents = sceneController.CollectionEvents;
            collectionEvents.OnLoadEntered += OnLoadEntered;
            collectionEvents.OnLoadProgress += OnLoadProgress;
        }

        private void OnDisable()
        {
            var collectionEvents = sceneController.CollectionEvents;
            collectionEvents.OnLoadEntered -= OnLoadEntered;
            collectionEvents.OnLoadProgress -= OnLoadProgress;
        }

        private void OnLoadEntered(CollectionLoadEventArgs args)
        {
            loadingStateText.text = "";
            loadingPercentageText.text = "0%";
        }

        private void OnLoadProgress(CollectionLoadProgressEventArgs args)
        {
            var collectionName = args.Collection.Name;
            var percentage = (int) (args.CollectionLoadProgress * 100);

            loadingStateText.text = $"Loading \"{collectionName}\"...";
            loadingPercentageText.text = $"{percentage}%";
        }
    }
}
