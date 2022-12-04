using System;
using UnityEngine;

namespace Better.RuntimeConsole.Runtime.ConsoleElements
{
    /// <summary>
    /// Settings class for <see cref="RuntimeConsole"/>
    /// </summary>
    [Serializable]
    public class ConsoleTextSettings
    {
        [SerializeField] private float logTextSize = 22f;
        [SerializeField] private float stackTraceTextSize = 15f;
        [SerializeField] private Color highlightColor;

        public float LogTextSize => logTextSize;

        public float StackTraceTextSize => stackTraceTextSize;

        public Color HighlightColor => highlightColor;

        public ConsoleTextSettings()
        {
        }

        public ConsoleTextSettings(ConsoleTextSettings settings)
        {
            logTextSize = settings.LogTextSize;
            stackTraceTextSize = settings.StackTraceTextSize;
            highlightColor = settings.HighlightColor;
        }
    }
}
