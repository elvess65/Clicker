using System.Collections;
using System.Collections.Generic;
using clicker.datatables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace clicker.general.ui
{
    public class UIElement_CraftItem_DraggableIcon : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public System.Action<PointerEventData> OnPoinerDownEvent;
        public System.Action<PointerEventData> OnDragEvent;
        public System.Action<PointerEventData> OnPointerUpEvent;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPoinerDownEvent?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke(eventData);
        }
    }
}
