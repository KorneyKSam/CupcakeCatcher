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
    public class It : MonoBehaviour, IEasterEgg, IPointerClickHandler
    {
        [Inject] private readonly AssetAudioService m_AssetAudioService;

        [SerializeField] private SpriteRenderer m_LeftEye;
        [SerializeField] private SpriteRenderer m_RightEye;

        public event Action Detonate;

        public bool EnableEsterEgg { get; set; }

        private void Start() => HideEyes();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (EnableEsterEgg)
            {
                DOTween.Sequence().Append(ShowEye(m_LeftEye)).SetDelay(0.3f).Join(ShowEye(m_RightEye)).SetDelay(1f).OnComplete(HideEyes);
                m_AssetAudioService.PlaySound(AudioAssetType.Penniwais);
                Detonate?.Invoke();
            }
        }

        private void HideEyes()
        {
            HideEye(m_LeftEye);
            HideEye(m_RightEye);
        }

        private void HideEye(SpriteRenderer srpiteRenderer) => srpiteRenderer.DOFade(0f, 1f);
        private Tween ShowEye(SpriteRenderer srpiteRenderer) => srpiteRenderer.DOFade(1f, 0.4f);
    }
}