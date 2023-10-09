using Common;
using Core.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem
{
    public class LevelResultTab : MonoBehaviourTab
    {
        [Inject] private readonly LevelResultManager m_ResultManager;

        [Header("Level result content")]
        [SerializeField] private Button m_TryAgainButton;
        [SerializeField] private Button m_MainMenuButton;
        [SerializeField] private TabGroup m_MainMenuGroup;
        [SerializeField] private ChoosePlayModeTab m_ChoosePlayModeTab;

        [Header("Statistic texts")]
        [SerializeField] private TMP_Text m_TotalEnergyTMP;
        [SerializeField] private TMP_Text m_CountOfEatenCupcakesTMP;
        [SerializeField] private TMP_Text m_FlattenedCucpcakesTMP;
        [SerializeField] private TMP_Text m_MissingCupcakesTMP;
        [SerializeField] private TMP_Text m_TotalTimeSecondsTMP;
        [SerializeField] private TMP_Text m_RottenFoodCountTMP;

        private void OnEnable()
        {
            var statistic = m_ResultManager.LevelStatistic;
            m_TotalEnergyTMP.text = $"{statistic.TotalEnergy}%";
            m_CountOfEatenCupcakesTMP.text = statistic.CountOfEatenCupcakes.ToString();
            m_FlattenedCucpcakesTMP.text = statistic.FlattenedCupcakes.ToString();
            m_MissingCupcakesTMP.text = statistic.MissingCupcakes.ToString();
            m_TotalTimeSecondsTMP.text = statistic.TotalTimeSeconds.ConvertToTimeFormat();
            m_RottenFoodCountTMP.text = statistic.RootenFoodCount.ToString();

            m_MainMenuButton.onClick.AddListener(TransitToMainMenu);
            m_TryAgainButton.onClick.AddListener(m_ChoosePlayModeTab.StartLevel);
        }

        private void OnDisable()
        {
            m_MainMenuButton.onClick.RemoveListener(TransitToMainMenu);
            m_TryAgainButton.onClick.RemoveListener(m_ChoosePlayModeTab.StartLevel);
        }

        private void TransitToMainMenu() => TabController.SetGroup(m_MainMenuGroup);
    }
}