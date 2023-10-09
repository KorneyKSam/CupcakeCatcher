using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(RectTransform), typeof(BoxCollider2D))]
    public class BoxColliderRectSynchronizer : MonoBehaviour
    {
        public RectTransform RectTransform => m_RectTransform;
        public BoxCollider2D BoxCollider2D => m_BoxCollider2D;

        [Header("Box collider synchronization")]
        [SerializeField]
        private RectTransform m_RectTransform;

        [SerializeField]
        private BoxCollider2D m_BoxCollider2D;

        public void Synchronize()
        {
            m_BoxCollider2D.size = new Vector2(m_RectTransform.rect.width, m_RectTransform.rect.height);
        }

        private void Start()
        {
            Synchronize();
        }

        private void OnValidate()
        {
            m_RectTransform ??= GetComponent<RectTransform>();
            m_BoxCollider2D ??= GetComponent<BoxCollider2D>();
        }
    }
}
