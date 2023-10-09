using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnergyPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform m_PopupTransform;
        [SerializeField] private TMP_Text m_PopupText;
        [SerializeField] private Color m_PositiveColor = Color.white;
        [SerializeField] private Color m_NegativColor = Color.red;

        [Header("Settings")]
        [SerializeField] private float m_AnimaitonDuration = 2f;
        [SerializeField] private Vector2 m_Offset;
        [SerializeField] private Vector2 m_MovingOffset;

        private Vector2 m_TargetPosition;

        private void Awake() => m_PopupTransform.localScale = Vector3.zero;

        public void ShowText(Vector2 position, int energy)
        {
            m_PopupTransform.position = position + m_Offset;
            m_TargetPosition = position + m_MovingOffset;
            m_PopupText.text = energy < 0 ? $"{energy}%" : $"+{energy}%";
            m_PopupText.color = energy < 0 ? m_NegativColor : m_PositiveColor;

            ShowAnimation();
        }

        private void ShowAnimation()
        {
            var halfOfDuration = m_AnimaitonDuration / 2;
            DOTween.Sequence().Append(m_PopupTransform.DOScale(0f, 0f))
                              .Append(m_PopupTransform.DOScale(1f, halfOfDuration))
                              .Join(m_PopupTransform.DOMove(m_TargetPosition, halfOfDuration))
                              .Append(m_PopupTransform.DOScale(0f, halfOfDuration))
                              .Join(m_PopupText.DOFade(0f, halfOfDuration));
        }

    }
}