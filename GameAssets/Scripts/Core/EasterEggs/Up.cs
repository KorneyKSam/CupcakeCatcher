using Assets;
using Audio;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace EasterEggs
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Up : MonoBehaviour, IEasterEgg, IPointerClickHandler
    {
        private const float EndYPosition = 150f;
        private const float Duration = 10f;
        private const float DelayHouseFly = 1.85f;

        [Inject] private readonly AssetAudioService m_AssetAudioService;

        [SerializeField] private Transform m_Balloons;
        [SerializeField] private Transform m_House;

        public event Action Detonate;
        public bool EnableEsterEgg { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (EnableEsterEgg)
            {
                m_AssetAudioService.PlaySound(AudioAssetType.KnockKnock);
                DOTween.Sequence().AppendInterval(2f).Append(m_Balloons.DOMoveY(EndYPosition, Duration))
                                  .Join(m_House.DOMoveY(EndYPosition, Duration).SetDelay(DelayHouseFly));
                Detonate?.Invoke();
            }
        }
    }
}