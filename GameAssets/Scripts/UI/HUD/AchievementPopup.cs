using Common.Statistic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class AchievementPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIAchievement m_UIAchievement;

        [Header("Settings")]
        [SerializeField] private float m_MoveDuration;
        [SerializeField] private float m_ShowDuration;
        [SerializeField] private Transform m_TargetTransform;

        private Vector2 m_StartPosition;
        private Transform m_Transform;

        private void Awake()
        {
            m_Transform = transform;
            m_StartPosition = m_Transform.localPosition;
        }

        public void ShowPopup(AchievementInfo achievementInfo)
        {
            m_UIAchievement.Set(achievementInfo, true);
            ShoPopupAnimaiton();
        }

        [ContextMenu(nameof(ShoPopupAnimaiton))]
        private void ShoPopupAnimaiton()
        {
            DOTween.Sequence().Append(m_Transform.DOLocalMove(m_TargetTransform.localPosition, m_MoveDuration))
                  .AppendInterval(m_ShowDuration)
                  .Append(m_Transform.DOLocalMove(m_StartPosition, m_MoveDuration));
        }
    }
}