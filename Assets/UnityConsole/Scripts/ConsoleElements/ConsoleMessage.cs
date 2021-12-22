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
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityConsole.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UnityConsole.Scripts.ConsoleElements
{
    /// <summary>
    /// Log message for <see cref="RuntimeConsole"/>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup), typeof(LayoutElement))]
    public class ConsoleMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        private string _logText;
        private string _stackTrace;
        private LogType _logType;
        private CanvasGroup _canvasGroup;
        private LayoutElement _layoutElement;
        private ConsoleTextSettings _currentSettings;

        public LogType Type => _logType;

        public string LogText => _logText;

        public string StackTrace => _stackTrace;

        /// <summary>
        /// Clear highlight marks
        /// </summary>
        public void ClearHighlight()
        {
            textField.text = textField.text.Replace(MarkStart(), "").Replace(MarkEnd(), "");
        }

        private string CombinedStackTrace(string logText, string stackTrace, ConsoleTextSettings settings)
        {
            return $"<b><size={settings.LogTextSize}>{logText}</size></b>" +
                   $"\n\n<size={settings.StackTraceTextSize}>{stackTrace}</size>";
        }

        /// <summary>
        /// Sets highlight marks
        /// </summary>
        public ConsoleMessage HighlightText(string text)
        {
            var markStart = MarkStart();
            var markEnd = MarkEnd();
            textField.text = textField.text.Replace(markStart, "").Replace(markEnd, "");
            var buffer = textField.text;
            buffer = Regex.Replace(buffer, text, match => $"{markStart}{match.Value}{markEnd}", RegexOptions.IgnoreCase);
            textField.text = buffer;
            return this;
        }

        /// <summary>
        /// Initializing console message
        /// </summary>
        /// <param name="logText"></param>
        /// <param name="stackTrace"></param>
        /// <param name="logType"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ConsoleMessage Initialize(string logText, string stackTrace, LogType logType, ConsoleTextSettings settings)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _layoutElement = GetComponent<LayoutElement>();
            _logType = logType;
            _logText = logText;
            _stackTrace = stackTrace;
            _currentSettings = new ConsoleTextSettings(settings);
            icon.preserveAspect = true;
            var time = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
            textField.text = $"[{time}] {LogText}";
            textField.fontSize = settings.LogTextSize;

            textField.color = _logType switch
                              {
                                  LogType.Error => Color.red,
                                  LogType.Assert => Color.red,
                                  LogType.Warning => Color.yellow,
                                  LogType.Log => Color.white,
                                  LogType.Exception => Color.red,
                                  _ => throw new ArgumentOutOfRangeException()
                              };
            return this;
        }

        private string MarkEnd()
        {
            return "</mark>";
        }

        private string MarkStart()
        {
            var htmlStringRGBA = ColorUtility.ToHtmlStringRGBA(_currentSettings.HighlightColor);
            var markStart = $"<mark=#{htmlStringRGBA}>";
            return markStart;
        }

        /// <summary>
        /// Setting active message in console
        /// </summary>
        /// <param name="state"></param>
        public ConsoleMessage SetActive(bool state)
        {
            _canvasGroup.ChangeGroupState(state);
            _layoutElement.ignoreLayout = !state;
            return this;
        }

        public ConsoleMessage SetIcon(ConsoleIcons icons)
        {
            icon.sprite = icons.GetLogIconSprite(_logType, true);
            return this;
        }

        /// <summary>
        /// Subscribes action to message button
        /// </summary>
        /// <param name="onClickAction"></param>
        /// <returns></returns>
        public ConsoleMessage SubscribeOnButtonClick(Action<string> onClickAction)
        {
            button.onClick.AddListener(() => onClickAction?.Invoke(CombinedStackTrace(LogText, StackTrace, _currentSettings)));
            return this;
        }
    }
}
