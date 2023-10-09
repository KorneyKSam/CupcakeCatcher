using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxColliderRandomPosition : MonoBehaviour
    {
        private BoxCollider2D m_BoxCollider2D;

        public Vector2 GetPosition()
        {
            var bounds = m_BoxCollider2D.bounds;
            return new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        }

        private void Start() => m_BoxCollider2D = GetComponent<BoxCollider2D>();
    }
}