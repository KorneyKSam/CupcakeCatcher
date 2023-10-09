using Audio;
using Core.Balance.Models;
using UnityEngine;
using Zenject;

namespace Core.Features
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wind : MonoBehaviour
    {
        [Inject] private readonly AssetAudioService m_AssetAudioService;
        [Inject] private readonly IWindModel m_WindModel;

        [SerializeField] private BoxCollider2D m_BoxCollider2D;

        private float m_Force;
        private Vector2 m_Direction;
        private GameplayStopwatch m_GameplayStopwatch;

        private void OnTriggerEnter2D(Collider2D collision) =>
            collision.gameObject.GetComponent<IExposedToTheWind>()?.Exposure(m_Direction * m_Force);

        private void OnTriggerExit2D(Collider2D collision) =>
            collision.gameObject.GetComponent<IExposedToTheWind>()?.Exposure(Vector2.zero);

        private void OnValidate() => m_BoxCollider2D = GetComponent<BoxCollider2D>();

        public void Enable(GameplayStopwatch stopwatch)
        {
            m_GameplayStopwatch = stopwatch;
            AddTriggers();
        }

        public void Disable() => StopWind();

        private void AddTriggers()
        {
            var interval = m_WindModel.Interval;
            m_GameplayStopwatch.AddTrigger(m_GameplayStopwatch.CurrentTime + interval, StartWind);
            m_GameplayStopwatch.AddTriggers(m_GameplayStopwatch.CurrentTime + interval + m_WindModel.Duration, StopWind, AddTriggers);
        }

        private void StartWind()
        {
            m_Direction = m_WindModel.Direction;
            m_Force = m_WindModel.Force;
            m_BoxCollider2D.enabled = true;
            m_AssetAudioService.PlayAmbient(Assets.AudioAssetType.WindLoop2);
        }

        private void StopWind()
        {
            m_BoxCollider2D.enabled = false;
            m_AssetAudioService.PlayAmbient(Assets.AudioAssetType.WindLoop1);
        }
    }
}