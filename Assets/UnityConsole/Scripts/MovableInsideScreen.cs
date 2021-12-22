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

using UnityConsole.Scripts.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityConsole.Scripts
{
    /// <summary>
    /// Class for moving UI objects inside screen
    /// </summary>
    public class MovableInsideScreen : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] private RectTransform drivenTransform;
        [SerializeField] private RectTransform container;
        private Vector3 _dragOffset;

        private void SetDraggedPosition(PointerEventData data)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(drivenTransform, data.position, data.pressEventCamera, out var globalMousePos))
                drivenTransform.position = _dragOffset + globalMousePos;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(drivenTransform, eventData.position, eventData.pressEventCamera,
                                                                         out var globalMousePos))
                return;
            _dragOffset = (Vector2) drivenTransform.position - (Vector2) globalMousePos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (drivenTransform.IsFullyVisibleFrom(Camera.main)) return;
            drivenTransform.KeepFullyOnScreen(container);
        }
    }
}
