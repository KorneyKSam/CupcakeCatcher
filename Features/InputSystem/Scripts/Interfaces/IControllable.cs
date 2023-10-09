using UnityEngine;

namespace NewInputSystem
{
    public interface IControllable
    {
        public void Shoot();
        public void Eat();
        public void SetAimPosition(Vector2 worldPosition);
        public void SetMovementDirection(Vector2 direction);
    }
}