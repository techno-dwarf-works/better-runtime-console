using System;
using System.Collections.Generic;
using System.Linq;
using UnityConsole.Scripts.ConsoleElements;
using UnityEngine;

namespace UnityConsole.Scripts
{
    /// <summary>
    /// Initialize minimized and maximized console
    /// </summary>
    public class ConsoleInitializer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool initializeMinimized;

        [SerializeField] private ConsoleIcons icons;

        [Header("References")]
        [SerializeField] private RuntimeConsole maximizedConsole;

        [SerializeField] private MinimizedConsole minimizedConsole;

        private void Awake()
        {
            #if DEBUG
            Initialize();
            #else
            Destroy(gameObject);
            #endif
        }

        private void Initialize()
        {
            if (FindObjectsOfType<RuntimeConsole>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            minimizedConsole.Initialize(OnConsoleMaximized, icons).SetActive(initializeMinimized);

            maximizedConsole.OnLogCountUpdated +=
                minimizedConsole.CountDisplayers.Aggregate<CountDisplayer, Action<HashSet<LogType>, int>>(null,
                    (current, displayer) => current + displayer.OnLogCountChanged);
            maximizedConsole.Initialize(OnConsoleMinimized, icons).SetActive(!initializeMinimized);
        }

        private void OnConsoleMaximized()
        {
            maximizedConsole.SetActive(true);
        }

        private void OnConsoleMinimized()
        {
            minimizedConsole.SetActive(true);
        }
    }
}
