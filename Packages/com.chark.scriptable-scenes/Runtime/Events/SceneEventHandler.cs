using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace CHARK.ScriptableScenes.Events
{
    /// <summary>
    /// Event handler for <see cref="ScriptableScene"/> assets.
    /// </summary>
    [Serializable]
    internal sealed class SceneEventHandler : ISceneEventHandler
    {
        #region Editor Fields

        [SerializeField]
        private UnityEvent<SceneLoadEventArgs> onLoadEntered =
            new UnityEvent<SceneLoadEventArgs>();

        [SerializeField]
        private UnityEvent<SceneLoadEventArgs> onLoadExited =
            new UnityEvent<SceneLoadEventArgs>();

        [SerializeField]
        private UnityEvent<SceneLoadProgressEventArgs> onLoadProgress =
            new UnityEvent<SceneLoadProgressEventArgs>();

        [SerializeField]
        private UnityEvent<SceneUnloadEventArgs> onUnloadEntered =
            new UnityEvent<SceneUnloadEventArgs>();

        [SerializeField]
        private UnityEvent<SceneUnloadEventArgs> onUnloadExited =
            new UnityEvent<SceneUnloadEventArgs>();

        [SerializeField]
        private UnityEvent<SceneActivateEventArgs> onActivateEntered =
            new UnityEvent<SceneActivateEventArgs>();

        [SerializeField]
        private UnityEvent<SceneActivateEventArgs> onActivateExited =
            new UnityEvent<SceneActivateEventArgs>();

        #endregion

        #region Public Events

        public event SceneLoadEvent OnLoadEntered;

        public event SceneLoadEvent OnLoadExited;

        public event SceneLoadProgressEvent OnLoadProgress;

        public event SceneUnloadEvent OnUnloadEntered;

        public event SceneUnloadEvent OnUnloadExited;

        public event SceneActivateEvent OnActivateEntered;

        public event SceneActivateEvent OnActivateExited;

        #endregion

        #region Internal Methods

        internal void RaiseLoadEntered(BaseScriptableScene scene)
        {
            var args = new SceneLoadEventArgs(scene);
            RaiseLoadEntered(args);
        }

        internal void RaiseLoadEntered(SceneLoadEventArgs args)
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

        internal void RaiseLoadExited(BaseScriptableScene scene)
        {
            var args = new SceneLoadEventArgs(scene);
            RaiseLoadExited(args);
        }

        internal void RaiseLoadExited(SceneLoadEventArgs args)
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

        internal void RaiseLoadProgress(BaseScriptableScene scene, float progress)
        {
            var args = new SceneLoadProgressEventArgs(scene, progress);
            RaiseLoadProgress(args);
        }

        internal void RaiseLoadProgress(SceneLoadProgressEventArgs args)
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

        internal void RaiseUnloadEntered(BaseScriptableScene scene)
        {
            var args = new SceneUnloadEventArgs(scene);
            RaiseUnloadEntered(args);
        }

        internal void RaiseUnloadEntered(SceneUnloadEventArgs args)
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

        internal void RaiseUnloadExited(BaseScriptableScene scene)
        {
            var args = new SceneUnloadEventArgs(scene);
            RaiseUnloadExited(args);
        }

        internal void RaiseUnloadExited(SceneUnloadEventArgs args)
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

        internal void RaiseActivateEntered(BaseScriptableScene scene)
        {
            var args = new SceneActivateEventArgs(scene);
            RaiseActivateEntered(args);
        }

        internal void RaiseActivateEntered(SceneActivateEventArgs args)
        {
            try
            {
                onActivateEntered.Invoke(args);
                OnActivateEntered?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        internal void RaiseActivateExited(BaseScriptableScene scene)
        {
            var args = new SceneActivateEventArgs(scene);
            RaiseActivateExited(args);
        }

        internal void RaiseActivateExited(SceneActivateEventArgs args)
        {
            try
            {
                onActivateExited.Invoke(args);
                OnActivateExited?.Invoke(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }

        #endregion
    }
}
