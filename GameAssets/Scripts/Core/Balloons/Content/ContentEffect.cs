using Core;
using System;
using UnityEngine;

namespace Balloon.Content
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ContentEffect : MonoBehaviour, IDestroyable
    {
        public event Action Destryed;

        public void Destroy() => Destryed?.Invoke();
    }
}