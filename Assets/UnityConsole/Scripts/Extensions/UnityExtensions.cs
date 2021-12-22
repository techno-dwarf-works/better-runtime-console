using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityConsole.Scripts.Extensions
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Scrolls ScrollRect to last item
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="reverseOrder"></param>
        public static void SnapToLatest(this ScrollRect scrollRect, bool reverseOrder)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = reverseOrder ? 1f : 0f;
        }
        
        /// <summary>
        /// Changing canvas visibility and interactivity
        /// </summary>
        /// <param name="group"></param>
        /// <param name="isVisible"></param>
        public static void ChangeGroupState(this CanvasGroup group, bool isVisible)
        {
            group.alpha = isVisible ? 1 : 0;
            group.interactable = isVisible;
            group.blocksRaycasts = isVisible;
        }
        
        /// <summary>
        /// Removing range of items from list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RemoveRange<T>(this List<T> list, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable) list.Remove(item);
            return list;
        }
    }
}