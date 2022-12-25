using System;
using UnityEngine;
using UnityEngine.UI;

namespace Better.RuntimeConsole.Runtime.ConsoleElements
{
    /// <summary>
    /// Log toggle for <see cref="RuntimeConsole"/>
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleCountDisplayer : CountDisplayer
    {
        private Toggle _toggle;

        public override CountDisplayer Initialize(ConsoleIcons icons)
        {
            base.Initialize(icons);
            _toggle = GetComponent<Toggle>();
            return this;
        }

        private void SetActiveIcon(bool state)
        {
            icon.sprite = _icons.GetLogIconSprite(designatedType, state);
        }

        public override CountDisplayer SetInteractionAction(Action<LogType, bool> onInteractWithDisplayer)
        {
            _toggle.onValueChanged.AddListener(state => onInteractWithDisplayer?.Invoke(designatedType, state));
            _toggle.onValueChanged.AddListener(SetActiveIcon);
            return this;
        }
    }
}
