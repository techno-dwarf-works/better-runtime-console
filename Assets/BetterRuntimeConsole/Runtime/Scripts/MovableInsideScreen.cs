using Better.RuntimeConsole.Runtime.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Better.RuntimeConsole.Runtime
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
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(drivenTransform, data.position,
                    data.pressEventCamera, out var globalMousePos))
            {
                drivenTransform.position = _dragOffset + globalMousePos;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(drivenTransform, eventData.position,
                    eventData.pressEventCamera,
                    out var globalMousePos))
            {
                return;
            }

            _dragOffset = (Vector2)drivenTransform.position - (Vector2)globalMousePos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (drivenTransform.IsFullyVisibleFrom(Camera.main))
            {
                return;
            }

            drivenTransform.KeepFullyOnScreen(container);
        }
    }
}