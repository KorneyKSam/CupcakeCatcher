using Core.Statistics;
using UI;
using UnityEngine;

namespace Common.Statistic
{
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private AchievementPopup m_AchievementPopup;
        [SerializeField] private AchievementInfo m_EasterEggInfo;

        private AchievementStatistics m_AchievementStatistics;

        public bool IsEasterEggAchieved => m_AchievementStatistics.IsEasterEggsAchieved;
        public AchievementInfo EasterEggInfo => m_EasterEggInfo;

        public void Awake() => m_AchievementStatistics = DataService.Load<AchievementStatistics>();

        public void Check(AchievementConditions conditions)
        {
            if (!m_AchievementStatistics.IsEasterEggsAchieved &&
                conditions.NumberOfFindedEggs >= 2)
            {
                m_AchievementPopup.ShowPopup(m_EasterEggInfo);
                m_AchievementStatistics.IsEasterEggsAchieved = true;
                DataService.Save(m_AchievementStatistics);
            }
        }
    }
}