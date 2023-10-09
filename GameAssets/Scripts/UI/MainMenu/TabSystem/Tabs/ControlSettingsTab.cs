using NewInputSystem;
using Settings;
using Localization;
using ModestTree;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem.Settings
{
    public class ControlSettingsTab : MonoBehaviourTab
    {
        [Header("Control settings content")]
        [SerializeField] private TMP_Dropdown m_ControlDropdown;
        [SerializeField] private Button m_ApplyButton;

        [Inject] private GameSettingsService m_GameSettingsService;

        private readonly List<string> m_DropdownValues = new();
        private TouchScreenControlScheme[] m_TouchScreenControlSchemes;

        private void Awake()
        {
            m_TouchScreenControlSchemes = (TouchScreenControlScheme[])Enum.GetValues(typeof(TouchScreenControlScheme));
        }

        private void OnEnable()
        {
            m_DropdownValues.Clear();
            m_ControlDropdown.ClearOptions();

            foreach (var controlScheme in m_TouchScreenControlSchemes)
            {
                m_DropdownValues.Add(LocalizationSystem.GetLocalizedValue(controlScheme.ToString()));
            }

            m_ControlDropdown.AddOptions(m_DropdownValues);

            UpdateSelectedScheme();
            m_ApplyButton.onClick.AddListener(ApplySettings);
        }

        private void OnDisable()
        {
            m_ApplyButton.onClick.RemoveListener(ApplySettings);
        }

        private void ApplySettings()
        {
            m_GameSettingsService.SetTouchSceheme(m_TouchScreenControlSchemes[m_ControlDropdown.value]);
            m_GameSettingsService.SaveCurrentSettings();
        }

        private void UpdateSelectedScheme() => m_ControlDropdown.value = m_TouchScreenControlSchemes.IndexOf(m_GameSettingsService.TouchScreenControlScheme);
    }
}