﻿using CHARK.ScriptableScenes.Events;
using TMPro;
using UnityEngine;

namespace CHARK.ScriptableScenes
{
    internal sealed class LoadingCanvasController : MonoBehaviour
    {
        #region Editor Fields

        [Header("Scenes")]
        [SerializeField]
        private ScriptableSceneController sceneController;

        [Header("Text")]
        [SerializeField]
        private TMP_Text loadingStateText;

        [SerializeField]
        private TMP_Text loadingPercentageText;

        #endregion

        #region Unity Lifecycle

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

        #endregion

        #region Private Methods

        private void OnLoadEntered(CollectionLoadEventArgs args)
        {
            loadingStateText.text = "";
            loadingPercentageText.text = "0%";
        }

        private void OnLoadProgress(CollectionLoadProgressEventArgs args)
        {
            var sceneName = args.Scene.Name;
            var percentage = (int) (args.CollectionLoadProgress * 100);

            loadingStateText.text = $"Loading scene {sceneName}...";
            loadingPercentageText.text = $"{percentage}%";
        }

        #endregion
    }
}