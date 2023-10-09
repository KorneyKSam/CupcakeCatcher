using Localization;
using Settings;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem.Settings
{
    public class LanguageSettingTab : MonoBehaviourTab
    {
        [Header("Language settings content")]
        [SerializeField] private Button m_ApplyBtn;
        [SerializeField] private Button m_CancelBtn;
        [SerializeField] private TMP_Dropdown m_LanguageDropdown;

        [Inject] private readonly GameSettingsService m_SettingsService;

        private readonly List<string> m_Languages = new();

        private void Start()
        {
            m_LanguageDropdown.ClearOptions();
            foreach (var item in (Language[])Enum.GetValues(typeof(Language)))
            {
                m_Languages.Add(item.ToString());
            }
            m_LanguageDropdown.AddOptions(m_Languages);
            UpdateSelectedLanguage();
        }

        private void OnEnable()
        {
            UpdateSelectedLanguage();
            m_ApplyBtn.onClick.AddListener(ApplySettings);
            m_CancelBtn.onClick.AddListener(ResetSettings);
        }


        private void OnDisable()
        {
            m_ApplyBtn.onClick.RemoveListener(ApplySettings);
            m_CancelBtn.onClick.RemoveListener(ResetSettings);
        }

        private void ApplySettings()
        {
            m_SettingsService.SetLanguage(m_LanguageDropdown.options[m_LanguageDropdown.value].text);
            m_SettingsService.SaveCurrentSettings();
        }

        private void UpdateSelectedLanguage() => m_LanguageDropdown.value = m_Languages.IndexOf(m_SettingsService.CurrentLanugage);
        private void ResetSettings() => m_SettingsService.ApplySavedSettings();
    }
}