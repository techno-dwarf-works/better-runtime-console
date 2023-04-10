using System;
using System.Collections.Generic;
using Better.Extensions.Runtime;
using Better.RuntimeConsole.Runtime.ConsoleElements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Better.RuntimeConsole.Runtime
{
    /// <summary>
    /// Minimized console
    /// <remarks>
    /// Works together with <seealso cref="RuntimeConsole"/>
    /// </remarks>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class MinimizedConsole : MonoBehaviour, IPointerClickHandler, IBeginDragHandler
    {
        [Header("References")] [SerializeField]
        private List<CountDisplayer> countDisplayers;

        private CanvasGroup _consoleCanvasGroup;
        private Action _onConsoleMaximized;
        private bool _previouslyDragged;

        public List<CountDisplayer> CountDisplayers => countDisplayers;

        /// <summary>
        /// Initializing MinimizedConsole
        /// </summary>
        /// <param name="onMaximized"></param>
        /// <param name="icons"></param>
        /// <returns></returns>
        public MinimizedConsole Initialize(Action onMaximized, ConsoleIcons icons)
        {
            _consoleCanvasGroup = GetComponent<CanvasGroup>();
            _onConsoleMaximized += onMaximized;
            foreach (var countDisplayer in countDisplayers) countDisplayer.Initialize(icons);
            return this;
        }

        private void Maximize()
        {
            SetActive(false);
            _onConsoleMaximized?.Invoke();
        }

        /// <summary>
        /// Hides or Show console
        /// </summary>
        /// <param name="state"></param>
        public void SetActive(bool state)
        {
            _consoleCanvasGroup.SetActive(state);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _previouslyDragged = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_previouslyDragged)
            {
                Maximize();
            }
            else
            {
                _previouslyDragged = false;
            }
        }
    }
}