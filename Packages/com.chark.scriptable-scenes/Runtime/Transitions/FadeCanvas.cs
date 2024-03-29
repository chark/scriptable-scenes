﻿using CHARK.ScriptableScenes.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CHARK.ScriptableScenes.Transitions
{
    [AddComponentMenu(
        AddComponentMenuConstants.BaseMenuName + "/Fade Canvas",
        AddComponentMenuConstants.TransitionOrder
    )]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    internal sealed class FadeCanvas : MonoBehaviour
    {
        [SerializeField]
        private FadeScriptableSceneTransition transition;

        // Optional raycaster.
        private GraphicRaycaster graphicRaycaster;

        private CanvasGroup canvasGroup;
        private Canvas canvas;

        private void Awake()
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            if (transition)
            {
                transition.AddCanvas(this);
            }
        }

        private void OnDisable()
        {
            if (transition)
            {
                transition.RemoveCanvas(this);
            }
        }

        internal void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }

        internal void ShowCanvas()
        {
            if (graphicRaycaster)
            {
                graphicRaycaster.enabled = true;
            }

            canvasGroup.enabled = true;
            canvas.enabled = true;
        }

        internal void HideCanvas()
        {
            if (graphicRaycaster)
            {
                graphicRaycaster.enabled = false;
            }

            canvasGroup.enabled = false;
            canvas.enabled = false;
        }
    }
}
