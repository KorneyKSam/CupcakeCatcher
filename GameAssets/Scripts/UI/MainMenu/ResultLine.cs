using Common;
using Core.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Results
{
    public class ResultLine : MonoBehaviour
    {
        [SerializeField] private Image m_CurrentProfileMark;
        [SerializeField] private Image m_CurrentGameMark;
        [SerializeField] private TMP_Text m_NumberText;
        [SerializeField] private TMP_Text m_ProfileText;
        [SerializeField] private TMP_Text m_CountOfCupcakesText;
        [SerializeField] private TMP_Text m_TimeText;

        public void Set(UITableResultInfo tableResultInfo)
        {
            m_NumberText.text = tableResultInfo.Number.ToString();
            m_ProfileText.text = tableResultInfo.ProfileName;
            m_CountOfCupcakesText.text = tableResultInfo.CupcakeCount.ToString();
            m_TimeText.text = tableResultInfo.TotalTimeSeconds.ConvertToTimeFormat();
            m_CurrentProfileMark.gameObject.SetActive(tableResultInfo.IsCurrentProfile);
            m_CurrentGameMark.gameObject.SetActive(tableResultInfo.IsLastGame);
        }
    }
}