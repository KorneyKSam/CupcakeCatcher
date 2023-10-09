using Balloon.Content;
using EventManager;
using UnityEngine;

namespace Core.Features
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SmokeGrenade : ContentEffect
    {
        [SerializeField] private BoxCollider2D m_SmokeCollider;
        [SerializeField] private ParticleSystem m_ParticleSystem;

        private float m_LeftBound;
        private float m_RightBound;
        private float m_CenterPosition;

        private void OnEnable()
        {
            m_LeftBound = m_SmokeCollider.bounds.min.x;
            m_RightBound = m_SmokeCollider.bounds.max.x;
            m_CenterPosition = m_SmokeCollider.transform.position.x;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            EventHolder.SmokeImpact.Invoke(GetCenterApproximation(collision.transform.position.x,
                collision.transform.position.x <= m_SmokeCollider.transform.position.x
                ? m_LeftBound : m_RightBound));
        }

        private void OnTriggerExit2D(Collider2D collision) => EventHolder.SmokeImpact.Invoke(default);
        private void OnValidate() => m_SmokeCollider ??= GetComponent<BoxCollider2D>();

        public void Pause() => m_ParticleSystem.Pause();
        public void Play() => m_ParticleSystem.Play();

        private float GetCenterApproximation(float collisionPosition, float boundValue)
        {
            return (collisionPosition - boundValue) / (m_CenterPosition - boundValue);
        }
    }
}