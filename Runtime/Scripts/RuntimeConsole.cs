using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Better.DataStructures.Ranges;
using Better.Extensions.Runtime;
using Better.RuntimeConsole.Runtime.ConsoleElements;
using Better.RuntimeConsole.Runtime.Dispatcher;
using Better.RuntimeConsole.Runtime.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Better.RuntimeConsole.Runtime
{
    /// <summary>
    /// Main class for RuntimeConsole
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class RuntimeConsole : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private bool reverseOrder;
        [SerializeField] private bool disableAutoScroll;
        [SerializeField] private ConsoleTextSettings settings;
        [SerializeField] private Range<float> autoScrollSensitivity = new Range<float>(0.05f, 0.95f);

        [Header("References")] [SerializeField]
        private TMP_InputField searchInputField;

        [SerializeField] private TMP_Text stackTraceTextField;
        [SerializeField] private Button clearSearchButton;
        [SerializeField] private Button clearLogsButton;
        [SerializeField] private Button minimizedButton;
        [SerializeField] private Toggle reverseSortingToggle;
        [SerializeField] private ScrollRect logsScrollRect;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private List<CountDisplayer> logButtons;

        [Header("Prefabs")] [SerializeField] private ConsoleMessage consoleMessagePrefab;

        private readonly Dictionary<LogType, List<ConsoleMessage>> _logs =
            new Dictionary<LogType, List<ConsoleMessage>>();

        private readonly HashSet<LogType> _displayedLogTypes = new HashSet<LogType>
        {
            LogType.Error,
            LogType.Assert,
            LogType.Warning,
            LogType.Log,
            LogType.Exception
        };

        private Action<HashSet<LogType>, int> _onLogCountUpdated;
        private Action _onConsoleMinimized;
        private CanvasGroup _consoleCanvasGroup;
        private string _recentSearchText;

        public event Action<HashSet<LogType>, int> OnLogCountUpdated
        {
            add => _onLogCountUpdated += value;
            remove => _onLogCountUpdated -= value;
        }

        private bool AutoScrollThreshold()
        {
            return reverseOrder
                ? logsScrollRect.verticalNormalizedPosition >= autoScrollSensitivity.Max
                : logsScrollRect.verticalNormalizedPosition <= autoScrollSensitivity.Min;
        }

        private void ClearLogs()
        {
            foreach (var messages in _logs.Values)
            {
                foreach (var message in messages)
                {
                    Destroy(message.gameObject);
                }
            }

            _logs.Clear();
            _onLogCountUpdated?.Invoke(LogTypes(new HashSet<LogType> { LogType.Log, LogType.Warning, LogType.Error }),
                0);
        }

        private void ClearSearch()
        {
            searchInputField.text = string.Empty;
        }

        private void CreateMessage(string condition, string stacktrace, LogType type)
        {
            var needScroll = AutoScrollThreshold();

            var instance = Instantiate(consoleMessagePrefab, logsScrollRect.content)
                .Initialize(condition, stacktrace, type, settings).SubscribeOnButtonClick(DisplayStackTrace);

            if (_logs.TryGetValue(type, out var logs))
            {
                logs.Add(instance);
                _logs[type] = logs;
            }
            else
            {
                _logs.Add(type, new List<ConsoleMessage> { instance });
            }

            _onLogCountUpdated?.Invoke(LogTypes(type), _logs[type].Count);
            DisplayByLogType(instance);

            if (!string.IsNullOrEmpty(_recentSearchText) &&
                !string.IsNullOrWhiteSpace(_recentSearchText))
            {
                SearchLogs(_recentSearchText);
            }

            if (!disableAutoScroll && needScroll)
            {
                logsScrollRect.SnapToLatest(reverseOrder);
            }
        }

        private void DisplayByLogType(ConsoleMessage message)
        {
            message.SetActive(_displayedLogTypes.Contains(message.Type));
        }

        private void DisplayStackTrace(string stackTrace)
        {
            stackTraceTextField.text = stackTrace;
        }

        /// <summary>
        /// Initializing RuntimeConsole
        /// </summary>
        /// <param name="onMinimized">Action what will executed on console minimized</param>
        /// <param name="icons"></param>
        /// <returns></returns>
        public RuntimeConsole Initialize(Action onMinimized, ConsoleIcons icons)
        {
            _consoleCanvasGroup = GetComponent<CanvasGroup>();
            layoutGroup.reverseArrangement = reverseOrder;
            reverseSortingToggle.isOn = reverseOrder;
            Application.logMessageReceivedThreaded += MessageReceivedThreaded;
            foreach (var logButton in logButtons)
                _onLogCountUpdated +=
                    logButton.Initialize(icons).SetInteractionAction(OnStateChanged).OnLogCountChanged;
            _onConsoleMinimized += onMinimized;
            searchInputField.onValueChanged.AddListener(text => _recentSearchText = text);
            searchInputField.onValueChanged.AddListener(SearchLogs);
            clearSearchButton.onClick.AddListener(ClearSearch);
            clearLogsButton.onClick.AddListener(ClearLogs);
            reverseSortingToggle.onValueChanged.AddListener(ToggleSorting);
            minimizedButton.onClick.AddListener(Minimize);
            ClearLogs();
            return this;
        }

        private HashSet<LogType> LogTypes(LogType designatedType)
        {
            var states = designatedType switch
            {
                LogType.Warning => new HashSet<LogType> { LogType.Warning },
                LogType.Log => new HashSet<LogType> { LogType.Log },
                LogType.Error => new HashSet<LogType> { LogType.Error, LogType.Exception, LogType.Assert },
                LogType.Assert => new HashSet<LogType> { LogType.Error, LogType.Exception, LogType.Assert },
                LogType.Exception => new HashSet<LogType> { LogType.Error, LogType.Exception, LogType.Assert },
                _ => throw new ArgumentOutOfRangeException()
            };
            return states;
        }

        private HashSet<LogType> LogTypes(HashSet<LogType> designatedTypes)
        {
            var states = Enumerable.Empty<LogType>();
            var result = states;
            foreach (var type in designatedTypes)
            {
                result = result.Concat(LogTypes(type));
            }

            return new HashSet<LogType>(result);
        }

        private void MessageReceivedThreaded(string condition, string stacktrace, LogType type)
        {
            MainThreadDispatcher.Enqueue(() => CreateMessage(condition, stacktrace, type));
        }

        private void Minimize()
        {
            SetActive(false);
            _onConsoleMinimized?.Invoke();
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= MessageReceivedThreaded;
        }

        private void OnStateChanged(LogType designatedType, bool state)
        {
            var states = LogTypes(designatedType);
            SetActiveLogs(states, state);
        }

        private void SearchLogs(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) ||
                string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var message in _logs.Values.SelectMany(messages => messages))
                    message.SetActive(true).ClearHighlight();
                return;
            }

            var regex = new Regex(searchText, RegexOptions.IgnoreCase);

            foreach (var messages in _logs.Values)
            {
                foreach (var message in messages)
                {
                    var isMatch = regex.IsMatch(message.LogText);

                    if (isMatch)
                    {
                        message.SetActive(true).HighlightText(searchText);
                    }
                    else
                    {
                        message.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// Hides or Show console
        /// </summary>
        /// <param name="state"></param>
        public void SetActive(bool state)
        {
            _consoleCanvasGroup.SetActive(state);
        }

        private void SetActiveLogs(IEnumerable<LogType> logTypes, bool state)
        {
            if (state)
            {
                _displayedLogTypes.UnionWith(logTypes);
            }
            else
            {
                _displayedLogTypes.ExceptWith(logTypes);
            }

            foreach (var logType in _logs.Keys)
            {
                foreach (var message in _logs[logType]) message.SetActive(_displayedLogTypes.Contains(logType));
            }
        }

        private void ToggleSorting(bool state)
        {
            var needScroll = AutoScrollThreshold();
            reverseOrder = state;
            layoutGroup.reverseArrangement = reverseOrder;
            if (!disableAutoScroll && needScroll)
            {
                logsScrollRect.SnapToLatest(reverseOrder);
            }
        }
    }
}