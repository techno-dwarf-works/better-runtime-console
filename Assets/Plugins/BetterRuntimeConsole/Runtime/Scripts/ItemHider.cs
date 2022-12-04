using System;
using System.Collections.Generic;
using System.Linq;
using Better.Extensions.Runtime;
using Better.RuntimeConsole.Runtime.Extensions;
using UnityEngine;

namespace Better.RuntimeConsole.Runtime
{
    /// <summary>
    /// UI element hider. Useful than needed to hide elements on UI change its' size
    /// </summary>
    [ExecuteAlways]
    public class ItemHider : MonoBehaviour
    {
        [SerializeField] private List<LODGroup> lods;

        private List<LODGroup> _lodToHide;
        private List<LODGroup> _lodToShow;
        private RectTransform _thisRectTransform;

        private void Awake()
        {
            _lodToHide = new List<LODGroup>(lods);
            _lodToShow = new List<LODGroup>();
            _thisRectTransform = (RectTransform)transform;
        }

        private List<LODGroup> CheckLODGroups(ref List<LODGroup> lodGroups, bool state)
        {
            var range = lodGroups.Where(x =>
            {
                var sizeDelta = _thisRectTransform.sizeDelta;

                var checkingValue = x.Dir switch
                {
                    LODGroup.Direction.Horizontal => sizeDelta.x,
                    LODGroup.Direction.Vertical => sizeDelta.y,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return state ? x.Size < checkingValue : x.Size > checkingValue;
            }).ToList();
            foreach (var item in range) item.Item.SetActive(state);
            lodGroups = lodGroups.RemoveRange(range);
            return range;
        }

        private void OnRectTransformDimensionsChange()
        {
            var listInitialized = _lodToHide != null && _lodToShow != null;
            if (listInitialized && _lodToHide.Count > 0)
            {
                _lodToShow.AddRange(CheckLODGroups(ref _lodToHide, false));
            }

            if (listInitialized && _lodToShow.Count > 0)
            {
                _lodToHide.AddRange(CheckLODGroups(ref _lodToShow, true));
            }
        }

        private void OnValidate()
        {
            _lodToHide = new List<LODGroup>(lods);
            _lodToShow = new List<LODGroup>();
            _thisRectTransform = (RectTransform)transform;
        }

        [Serializable]
        public class LODGroup
        {
            [SerializeField] private CanvasGroup item;
            [SerializeField] private float size;
            [SerializeField] private Direction direction;

            public enum Direction
            {
                Horizontal,
                Vertical
            }

            public CanvasGroup Item => item;

            public float Size => size;

            public Direction Dir => direction;
        }
    }
}