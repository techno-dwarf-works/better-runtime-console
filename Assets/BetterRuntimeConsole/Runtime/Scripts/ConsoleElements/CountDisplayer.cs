using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Better.RuntimeConsole.Runtime.ConsoleElements
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
            if (types.Contains(designatedType))
            {
                countText.text = count <= 999 ? $"{count}" : "999+";
            }
        }

        /// <summary>
        /// Setting action when interaction with CountDisplayer happens
        /// </summary>
        /// <param name="onInteractWithDisplayer"></param>
        /// <returns></returns>
        public abstract CountDisplayer SetInteractionAction(Action<LogType, bool> onInteractWithDisplayer);
    }
}
