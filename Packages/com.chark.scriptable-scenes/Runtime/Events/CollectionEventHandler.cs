using System;
using UnityEngine;
using UnityEngine.Events;

namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Event handler for <see cref="ScriptableSceneCollection"/> assets.
    /// </summary>
    [Serializable]
    internal sealed class CollectionEventHandler : ICollectionEventHandler
    {
        #region Editor Fields

        [SerializeField]
        private UnityEvent<CollectionLoadEventArgs> onLoadEntered =
            new UnityEvent<CollectionLoadEventArgs>();

        [SerializeField]
        private UnityEvent<CollectionLoadEventArgs> onLoadExited =
            new UnityEvent<CollectionLoadEventArgs>();

        [SerializeField]
        private UnityEvent<CollectionLoadProgressEventArgs> onLoadProgress =
            new UnityEvent<CollectionLoadProgressEventArgs>();

        [SerializeField]
        private UnityEvent<CollectionUnloadEventArgs> onUnloadEntered =
            new UnityEvent<CollectionUnloadEventArgs>();

        [SerializeField]
        private UnityEvent<CollectionUnloadEventArgs> onUnloadExited =
            new UnityEvent<CollectionUnloadEventArgs>();

        #endregion

        #region Public Events

        public event CollectionLoadEvent OnLoadEntered;

        public event CollectionLoadEvent OnLoadExited;

        public event CollectionLoadProgressEvent OnLoadProgress;

        public event CollectionUnloadEvent OnUnloadEntered;

        public event CollectionUnloadEvent OnUnloadExited;

        #endregion

        #region Internal Methods

        internal void RaiseLoadEntered(BaseScriptableSceneCollection collection)
        {
            var args = new CollectionLoadEventArgs(collection);
            RaiseLoadEntered(args);
        }

        internal void RaiseLoadEntered(CollectionLoadEventArgs args)
        {
            try
            {
                onLoadEntered.Invoke(args);
                OnLoadEntered?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseLoadExited(BaseScriptableSceneCollection collection)
        {
            var args = new CollectionLoadEventArgs(collection);
            RaiseLoadExited(args);
        }

        internal void RaiseLoadExited(CollectionLoadEventArgs args)
        {
            try
            {
                onLoadExited.Invoke(args);
                OnLoadExited?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseLoadProgress(
            BaseScriptableSceneCollection collection,
            BaseScriptableScene scene,
            float collectionProgress,
            float sceneProgress
        )
        {
            var args = new CollectionLoadProgressEventArgs(
                collection,
                scene,
                collectionProgress,
                sceneProgress
            );

            RaiseLoadProgress(args);
        }

        internal void RaiseLoadProgress(CollectionLoadProgressEventArgs args)
        {
            try
            {
                onLoadProgress.Invoke(args);
                OnLoadProgress?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseUnloadEntered(BaseScriptableSceneCollection collection)
        {
            var args = new CollectionUnloadEventArgs(collection);
            RaiseUnloadEntered(args);
        }

        internal void RaiseUnloadEntered(CollectionUnloadEventArgs args)
        {
            try
            {
                onUnloadEntered.Invoke(args);
                OnUnloadEntered?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseUnloadExited(BaseScriptableSceneCollection collection)
        {
            var args = new CollectionUnloadEventArgs(collection);
            RaiseUnloadExited(args);
        }

        internal void RaiseUnloadExited(CollectionUnloadEventArgs args)
        {
            try
            {
                onUnloadExited.Invoke(args);
                OnUnloadExited?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        #endregion
    }
}
