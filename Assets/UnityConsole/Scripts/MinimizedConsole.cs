#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using UnityConsole.Scripts.ConsoleElements;
using UnityConsole.Scripts.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityConsole.Scripts
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
        [Header("References")]
        [SerializeField] private List<CountDisplayer> countDisplayers;

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
            _consoleCanvasGroup.ChangeGroupState(state);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _previouslyDragged = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_previouslyDragged)
                Maximize();
            else
                _previouslyDragged = false;
        }
    }
}
