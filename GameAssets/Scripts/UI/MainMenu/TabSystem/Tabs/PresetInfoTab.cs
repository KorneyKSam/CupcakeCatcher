using Localization;
using TMPro;
using UnityEngine;

namespace UI.TabSystem
{
    public class PresetInfoTab : MonoBehaviourTab
    {
        [Header("Info content")]
        [SerializeField] private TMP_Text m_Text;
        [SerializeField] private Transform m_Container;
        [SerializeField] private PresetButton m_Button;

        public void SetPreset(PresetButtonInfo presetButtonInfo)
        {
            m_Button.Initialize(presetButtonInfo);
            m_Text.text = LocalizationSystem.GetLocalizedValue(presetButtonInfo.ReturnedKey);
        }
    }
}
