using Common.Statistic;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIAchievement : MonoBehaviour
    {
        [SerializeField] private Image m_Icon;
        [SerializeField] private TMP_Text m_Title;
        [SerializeField] private TMP_Text m_Description;
        [SerializeField] private GameObject m_CheckMark;

        public void Set(AchievementInfo achievementInfo, bool isCheckMarkActive)
        {
            m_Icon.sprite = isCheckMarkActive ? achievementInfo.ActiveIcon : achievementInfo.InactiveIcon;
            m_Title.text = LocalizationSystem.GetLocalizedValue(achievementInfo.TitleKey);
            m_Description.text = LocalizationSystem.GetLocalizedValue(achievementInfo.DescriptionKey);
            m_CheckMark.SetActive(isCheckMarkActive);
        }
    }
}