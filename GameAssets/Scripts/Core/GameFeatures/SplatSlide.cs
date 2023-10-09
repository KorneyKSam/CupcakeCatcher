using Balloon.Content;
using DG.Tweening;
using UnityEngine;

namespace Core.Features
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SplatSlide : ContentEffect
    {
        private const int DirectionOffsetFactor = 2;
        private const float SlideForce = 600f;
        private const float XScaleIfSplat = 2f;
        private const float AnimationDuration = 0.5f;

        [SerializeField] private Transform m_SmashingTransform;

        private Vector2 m_InitialPosition;
        private bool m_IsSmashed;

        private void Start() => m_InitialPosition = m_SmashingTransform.localPosition;

        public void Reset()
        {
            if (m_IsSmashed)
            {
                m_IsSmashed = false;
                m_SmashingTransform.localScale = Vector2.one;
                m_SmashingTransform.localPosition = m_InitialPosition;
                m_SmashingTransform.localRotation = Quaternion.identity;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_IsSmashed && collision.gameObject.TryGetComponent<ISlidable>(out var slidable))
            {
                m_IsSmashed = true;
                var direction = slidable.AddSlideForce(SlideForce);
                var sequnce = DOTween.Sequence();
                sequnce.Append(m_SmashingTransform.DOLocalMove(new Vector3(m_SmashingTransform.localPosition.x + direction.x * DirectionOffsetFactor, m_SmashingTransform.localPosition.y), AnimationDuration))
                       .Join(m_SmashingTransform.DOScale(new Vector2(XScaleIfSplat, m_SmashingTransform.localScale.y), AnimationDuration));
            }
        }
    }
}