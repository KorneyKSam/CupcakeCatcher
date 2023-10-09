using System;
using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxColliderClickHandler : MonoBehaviour
    {
        public event Action Down;

        private void OnMouseDown()
        {
            Down?.Invoke();
        }
    }
}