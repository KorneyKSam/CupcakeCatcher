using Common.Statistic;
using UnityEngine;
using Zenject;

namespace UI.TabSystem
{
    public class AchievementsTab : MonoBehaviourTab
    {
        [Inject] private readonly AchievementManager m_AchievementManager;

        [Header("Achievements content")]
        [SerializeField] private UIAchievement m_EasterEggAchevement;

        private void OnEnable()
        {
            m_EasterEggAchevement.Set(m_AchievementManager.EasterEggInfo, m_AchievementManager.IsEasterEggAchieved);
        }
    }
}