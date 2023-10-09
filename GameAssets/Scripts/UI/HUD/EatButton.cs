using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EatButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private TMP_Text m_CountOfFreePlacesText;
        [SerializeField] private Color m_NormalColor = Color.white;
        [SerializeField] private Color m_NoPlaceColor = Color.red;

        public Button Button => m_Button;

        public void SetCountOfFreePlaces(int countOfFreePlaces)
        {
            m_CountOfFreePlacesText.color = countOfFreePlaces <= 0 ? m_NoPlaceColor : m_NormalColor;
            m_CountOfFreePlacesText.text = countOfFreePlaces.ToString();
        }
    }
}