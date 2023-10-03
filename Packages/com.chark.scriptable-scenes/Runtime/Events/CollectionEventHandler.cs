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
        [SerializeField]
        private UnityEvent<CollectionLoadEventArgs> onLoadEntered = new();

        [SerializeField]
        private UnityEvent<CollectionLoadEventArgs> onLoadExited = new();

        [SerializeField]
        private UnityEvent<CollectionLoadProgressEventArgs> onLoadProgress = new();

        [SerializeField]
        private UnityEvent<CollectionUnloadEventArgs> onUnloadEntered = new();

        [SerializeField]
        private UnityEvent<CollectionUnloadEventArgs> onUnloadExited = new();

        [SerializeField]
        private UnityEvent onShowTransitionEntered = new();

        [SerializeField]
        private UnityEvent onShowTransitionExited = new();

        [SerializeField]
        private UnityEvent onHideTransitionEntered = new();

        [SerializeField]
        private UnityEvent onHideTransitionExited = new();

        public event CollectionLoadEvent OnLoadEntered;

        public event CollectionLoadEvent OnLoadExited;

        public event CollectionLoadProgressEvent OnLoadProgress;

        public event CollectionUnloadEvent OnUnloadEntered;

        public event CollectionUnloadEvent OnUnloadExited;

        public event CollectionTransitionEvent OnShowTransitionEntered;

        public event CollectionTransitionEvent OnShowTransitionExited;

        public event CollectionTransitionEvent OnHideTransitionEntered;

        public event CollectionTransitionEvent OnHideTransitionExited;

        internal void RaiseLoadEntered(ScriptableSceneCollection collection)
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

        internal void RaiseLoadExited(ScriptableSceneCollection collection)
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
            ScriptableSceneCollection collection,
            ScriptableScene scene,
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

        internal void RaiseUnloadEntered(ScriptableSceneCollection collection)
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

        internal void RaiseUnloadExited(ScriptableSceneCollection collection)
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

        // TODO: these events below are not tested
        internal void RaiseShowTransitionEntered()
        {
            try
            {
                onShowTransitionEntered.Invoke();
                OnShowTransitionEntered?.Invoke();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseShowTransitionExited()
        {
            try
            {
                onShowTransitionExited.Invoke();
                OnShowTransitionExited?.Invoke();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseHideTransitionEntered()
        {
            try
            {
                onHideTransitionEntered.Invoke();
                OnHideTransitionEntered?.Invoke();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseHideTransitionExited()
        {
            try
            {
                onHideTransitionExited.Invoke();
                OnHideTransitionExited?.Invoke();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}
