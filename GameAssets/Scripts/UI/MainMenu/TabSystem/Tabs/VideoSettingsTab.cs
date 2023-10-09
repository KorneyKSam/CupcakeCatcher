using Settings;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem.Settings
{
    public class VideoSettingsTab : MonoBehaviourTab
    {
        private const string LimitedFrameRate = nameof(LimitedFrameRate);

        [Header("Video settings content")]
        [SerializeField] private Slider m_FPSSlider;
        [SerializeField] private Toggle m_FPSToggle;
        [SerializeField] private Toggle m_SynchornizationToggle;
        [SerializeField] private Toggle m_LimitFPSToggle;
        [SerializeField] private Button m_ApplyBtn;
        [SerializeField] private Button m_CancelBtn;
        [SerializeField] private TMP_Text m_FPSText;

        [Inject] private readonly GameSettingsService m_SettingsService;

        private string m_LocalizedFrameRateText;

        private void OnEnable()
        {
            m_LocalizedFrameRateText = LocalizationSystem.GetLocalizedValue(LimitedFrameRate);
            m_FPSToggle.isOn = m_SettingsService.IsFPSEnabled;
            m_SynchornizationToggle.isOn = m_SettingsService.IsVSynchronized;
            m_LimitFPSToggle.isOn = m_SettingsService.IsLimitedFR;
            m_ApplyBtn.onClick.AddListener(ApplySettings);
            m_CancelBtn.onClick.AddListener(ResetSettings);
            m_FPSSlider.onValueChanged.AddListener(UpdateFPSText);
            m_FPSSlider.value = m_SettingsService.TargetFrameRateSlider;
            UpdateFPSText(m_SettingsService.TargetFrameRateSlider);
        }

        private void OnDisable()
        {
            m_ApplyBtn.onClick.RemoveListener(ApplySettings);
            m_CancelBtn.onClick.RemoveListener(ResetSettings);
            m_FPSSlider.onValueChanged.RemoveListener(UpdateFPSText);
        }

        private void UpdateFPSText(float sliderValue)
        {
            m_FPSText.text = $"{m_LocalizedFrameRateText} {m_SettingsService.GetFrameRateFromSliderValue(sliderValue)}";
        }

        private void ApplySettings()
        {
            m_SettingsService.ActivateFPSCounter(m_FPSToggle.isOn);
            m_SettingsService.SetVerticalSynchronization(m_SynchornizationToggle.isOn);
            m_SettingsService.SetTargetFrameRate(m_FPSSlider.value, m_LimitFPSToggle.isOn);
            m_SettingsService.SaveCurrentSettings();
        }

        private void ResetSettings() => m_SettingsService.ApplySavedSettings();
    }
}