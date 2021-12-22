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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityConsole.Scripts.ConsoleElements
{
    /// <summary>
    /// Log count displayer for <see cref="RuntimeConsole"/>
    /// </summary>
    public abstract class CountDisplayer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private protected LogType designatedType;
        
        [Header("References")]
        [SerializeField] private protected TMP_Text countText;
        [SerializeField] private protected Image icon;

        protected ConsoleIcons _icons;

        /// <summary>
        /// Initialize CountDisplayer with icons
        /// </summary>
        /// <param name="icons"></param>
        /// <returns></returns>
        public virtual CountDisplayer Initialize(ConsoleIcons icons)
        {
            _icons = icons;
            icon.sprite = icons.GetLogIconSprite(designatedType, true);
            return this;
        }

        /// <summary>
        /// Displaying new count
        /// </summary>
        /// <param name="types"></param>
        /// <param name="count"></param>
        public virtual void OnLogCountChanged(HashSet<LogType> types, int count)
        {
            if (types.Contains(designatedType)) countText.text = count <= 999 ? $"{count}" : "999+";
        }

        /// <summary>
        /// Setting action when interaction with CountDisplayer happens
        /// </summary>
        /// <param name="onInteractWithDisplayer"></param>
        /// <returns></returns>
        public abstract CountDisplayer SetInteractionAction(Action<LogType, bool> onInteractWithDisplayer);
    }
}
