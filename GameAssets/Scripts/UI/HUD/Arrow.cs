using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Arrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action FingerDown;
        public event Action FingerUp;

        public void OnPointerDown(PointerEventData eventData) => FingerDown?.Invoke();
        public void OnPointerUp(PointerEventData eventData) => FingerUp?.Invoke();
    }
}