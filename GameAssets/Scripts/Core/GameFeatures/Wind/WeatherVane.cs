using Assets;
using UnityEngine;
using Zenject;

namespace Core.Features
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class WeatherVane : MonoBehaviour, IExposedToTheWind
    {
        private const string TurnToLeft = nameof(TurnToLeft);
        private const string TurnToRight = nameof(TurnToRight);
        private const string BackFromLeft = nameof(BackFromLeft);
        private const string BackFromRight = nameof(BackFromRight);

        [Inject] private readonly AssetLoader m_AssetLoader;

        [SerializeField] private Animator m_VaneAnimator;
        [SerializeField] private AudioSource m_AudioSource;

        private string m_CurrentState;

        public void Exposure(Vector2 force)
        {
            var newState = string.Empty;
            if (force.x == 0)
            {
                if (m_CurrentState == TurnToLeft) { newState = BackFromLeft; }
                else if (m_CurrentState == TurnToRight) { newState = BackFromRight; }
            }
            else if (IsLeftDirection(force))
            {
                newState = TurnToLeft;
            }
            else if (IsRightDirection(force))
            {
                newState = TurnToRight;
            }

            if (m_CurrentState != newState)
            {
                m_CurrentState = newState;
                m_AudioSource.clip = m_AssetLoader.LoadAudioClip(AudioAssetType.WeatherVane);
                m_AudioSource.Play();
                m_VaneAnimator.Play(m_CurrentState);
            }
        }

        private bool IsRightDirection(Vector2 force) => force.x >= 0;
        private bool IsLeftDirection(Vector2 force) => force.x <= 0;
    }
}