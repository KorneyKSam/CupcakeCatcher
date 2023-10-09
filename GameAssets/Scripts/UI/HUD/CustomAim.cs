using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class CustomAim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event Action<Vector2> FingerDown;
        public event Action<Vector2> FingerMove;
        public event Action<Vector2> FingerUp;

        private int? m_CurrentPointerId;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_CurrentPointerId == null)
            {
                m_CurrentPointerId = eventData.pointerId;
                FingerDown?.Invoke(eventData.position);
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (m_CurrentPointerId == eventData.pointerId)
            {
                FingerMove?.Invoke(eventData.position);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_CurrentPointerId == eventData.pointerId)
            {
                FingerUp?.Invoke(eventData.position);
                m_CurrentPointerId = null;
            }
        }
    }
}