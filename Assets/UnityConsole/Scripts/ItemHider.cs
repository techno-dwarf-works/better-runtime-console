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
using System.Linq;
using UnityConsole.Scripts.Extensions;
using UnityEngine;

namespace UnityConsole.Scripts
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
            _thisRectTransform = (RectTransform) transform;
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
            foreach (var item in range) item.Item.ChangeGroupState(state);
            lodGroups = lodGroups.RemoveRange(range);
            return range;
        }

        private void OnRectTransformDimensionsChange()
        {
            var listInitialized = _lodToHide != null && _lodToShow != null;
            if (listInitialized && _lodToHide.Count > 0) _lodToShow.AddRange(CheckLODGroups(ref _lodToHide, false));
            if (listInitialized && _lodToShow.Count > 0) _lodToHide.AddRange(CheckLODGroups(ref _lodToShow, true));
        }

        private void OnValidate()
        {
            _lodToHide = new List<LODGroup>(lods);
            _lodToShow = new List<LODGroup>();
            _thisRectTransform = (RectTransform) transform;
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
