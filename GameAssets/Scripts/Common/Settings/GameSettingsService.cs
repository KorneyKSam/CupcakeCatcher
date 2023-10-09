using Audio.Enums;
using Common;
using Localization;
using NewInputSystem;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Settings
{
    public class GameSettingsService
    {
        public bool IsFPSEnabled => m_CurrentGameSettings.IsFPSCounterEnabled;
        public bool IsVSynchronized => m_CurrentGameSettings.IsVSynchronized;
        public bool IsLimitedFR => m_CurrentGameSettings.IsLimited;
        public float TargetFrameRateSlider => m_CurrentGameSettings.TargetFrameRate;
        public string CurrentLanugage => LocalizationSystem.CurrentLanguage.ToString();
        public TouchScreenControlScheme TouchScreenControlScheme => m_CurrentGameSettings.TouchScreenControlScheme;

        private const int MinVolumeValue = -80;
        private const int MaxVolumeValue = 20;

        private const int MinLimitedFrameRate = 30;
        private const int MaxLimitedFrameRate = 144;

        [Inject] private readonly FPSCounter m_FPSCounter;
        [Inject] private readonly AudioMixer m_AudioMixer;

        private readonly GameSettings m_CurrentGameSettings = new();

        private CustomDeviceInputController m_CustomDeviceInputController;

        public void ApplySavedSettings()
        {
            var loadedGameSettings = DataService.Load<GameSettings>();

            foreach (var channelVolume in loadedGameSettings.ChannelVolumes)
            {
                SetVolumeForChannel(channelVolume.Key, channelVolume.Value);
            }

            ActivateFPSCounter(loadedGameSettings.IsFPSCounterEnabled);
            SetVerticalSynchronization(loadedGameSettings.IsVSynchronized);
            SetLanguage(GetLanguage(loadedGameSettings));
            SetTargetFrameRate(loadedGameSettings.TargetFrameRate, loadedGameSettings.IsLimited);
            SetTouchSceheme(loadedGameSettings.TouchScreenControlScheme);
        }

        public void SetVolumeForChannel(string audioGroup, float volume)
        {
            var mixerVolume = Mathf.Lerp(MinVolumeValue, MaxVolumeValue, volume);
            m_AudioMixer.SetFloat(audioGroup.ToString(), mixerVolume);
            m_CurrentGameSettings.ChannelVolumes[audioGroup] = volume;
        }

        public void ActivateFPSCounter(bool isActive)
        {
            m_CurrentGameSettings.IsFPSCounterEnabled = isActive;
            m_FPSCounter.gameObject.SetActive(isActive);
        }

        public void SetVerticalSynchronization(bool isVsOn)
        {
            m_CurrentGameSettings.IsVSynchronized = isVsOn;
            QualitySettings.vSyncCount = isVsOn ? 1 : 0;
        }

        public void SetTargetFrameRate(float sliderValue, bool limited)
        {
            int targetFR = GetFrameRateFromSliderValue(sliderValue);
            m_CurrentGameSettings.TargetFrameRate = sliderValue;
            m_CurrentGameSettings.IsLimited = limited;
            Application.targetFrameRate = limited ? targetFR : -1;
        }

        public void SetLanguage(string language)
        {
            if (Enum.TryParse<Language>(language, out var result))
            {
                LocalizationSystem.CurrentLanguage = result;
                m_CurrentGameSettings.Language = language;
            }
        }

        public void SetTouchSceheme(TouchScreenControlScheme touchScreenControlScheme)
        {
            m_CurrentGameSettings.TouchScreenControlScheme = touchScreenControlScheme;
            if (m_CustomDeviceInputController != null)
            {
                m_CustomDeviceInputController.SetTouchScreenControlScheme(m_CurrentGameSettings.TouchScreenControlScheme);
            }
        }

        public void SetTouchSchemeForController(CustomDeviceInputController inputController)
        {
            m_CustomDeviceInputController = inputController;
            m_CustomDeviceInputController.SetTouchScreenControlScheme(m_CurrentGameSettings.TouchScreenControlScheme);
        }

        public int GetFrameRateFromSliderValue(float sliderValue) => (int)Mathf.Lerp(MinLimitedFrameRate, MaxLimitedFrameRate, sliderValue);
        public float GetChannelVolume(AudioGroup audioGroup) => m_CurrentGameSettings.ChannelVolumes[audioGroup.ToString()];
        public void SetVolumeForChannel(AudioGroup audioGroup, float volume) => SetVolumeForChannel(audioGroup.ToString(), volume);
        public void SaveCurrentSettings() => DataService.Save(m_CurrentGameSettings);

        private string GetLanguage(GameSettings loadedGameSettings) => string.IsNullOrWhiteSpace(loadedGameSettings.Language) ? Application.systemLanguage.ToString() : loadedGameSettings.Language;
    }
}
