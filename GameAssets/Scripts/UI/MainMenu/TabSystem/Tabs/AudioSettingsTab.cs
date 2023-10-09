using Audio.Enums;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem.Settings
{
    public class AudioSettingsTab : MonoBehaviourTab
    {
        private const int PercentageFactor = 100;

        [Header("Audio settings content")]
        [SerializeField] private Slider m_MasterSlider;
        [SerializeField] private Slider m_MusicSlider;
        [SerializeField] private Slider m_AmbientSlider;
        [SerializeField] private Slider m_SoundSlider;
        [SerializeField] private TMP_Text m_MasterPercent;
        [SerializeField] private TMP_Text m_MusicMersent;
        [SerializeField] private TMP_Text m_AmbientPercent;
        [SerializeField] private TMP_Text m_SoundPercent;
        [SerializeField] private Button m_ApplyBtn;
        [SerializeField] private Button m_CancelBtn;

        [Inject] private readonly GameSettingsService m_SettingsService;

        private void SynchronizeUIWithSettings()
        {
            m_MasterSlider.value = m_SettingsService.GetChannelVolume(AudioGroup.Master);
            m_MusicSlider.value = m_SettingsService.GetChannelVolume(AudioGroup.Music);
            m_AmbientSlider.value = m_SettingsService.GetChannelVolume(AudioGroup.Ambient);
            m_SoundSlider.value = m_SettingsService.GetChannelVolume(AudioGroup.Sounds);

            UpdateMasterPersentage();
            UpdateMusicPercentage();
            UpdateAmbientPercentage();
            UpdateSoundPercentage();
        }

        private void AddListeners()
        {
            RemoveListeners();
            m_MasterSlider.onValueChanged.AddListener(SetMasterChannelVolume);
            m_MusicSlider.onValueChanged.AddListener(SetMusicChannelVolume);
            m_AmbientSlider.onValueChanged.AddListener(SetAmbientChannelVolume);
            m_SoundSlider.onValueChanged.AddListener(SetSoundChannelVolume);
            m_ApplyBtn.onClick.AddListener(ApplySettings);
            m_CancelBtn.onClick.AddListener(ResetSettings);
        }

        private void RemoveListeners()
        {
            m_MasterSlider.onValueChanged.RemoveListener(SetMasterChannelVolume);
            m_MusicSlider.onValueChanged.RemoveListener(SetMusicChannelVolume);
            m_AmbientSlider.onValueChanged.RemoveListener(SetAmbientChannelVolume);
            m_SoundSlider.onValueChanged.RemoveListener(SetSoundChannelVolume);
            m_ApplyBtn.onClick.RemoveListener(ApplySettings);
            m_CancelBtn.onClick.RemoveListener(ResetSettings);
        }

        private void SetMasterChannelVolume(float volume)
        {
            m_SettingsService.SetVolumeForChannel(AudioGroup.Master, volume);
            UpdateMasterPersentage();
        }

        private void SetMusicChannelVolume(float volume)
        {
            m_SettingsService.SetVolumeForChannel(AudioGroup.Music, volume);
            UpdateMusicPercentage();
        }

        private void SetAmbientChannelVolume(float volume)
        {
            m_SettingsService.SetVolumeForChannel(AudioGroup.Ambient, volume);
            UpdateAmbientPercentage();
        }

        private void SetSoundChannelVolume(float volume)
        {
            m_SettingsService.SetVolumeForChannel(AudioGroup.Sounds, volume);
            UpdateSoundPercentage();
        }

        private void OnEnable()
        {
            SynchronizeUIWithSettings();
            AddListeners();
        }

        private void OnDisable() => RemoveListeners();
        private void UpdateMasterPersentage() => m_MasterPercent.text = GetPercentage(m_MasterSlider.value);
        private void UpdateMusicPercentage() => m_MusicMersent.text = GetPercentage(m_MusicSlider.value);
        private void UpdateAmbientPercentage() => m_AmbientPercent.text = GetPercentage(m_AmbientSlider.value);
        private void UpdateSoundPercentage() => m_SoundPercent.text = GetPercentage(m_SoundSlider.value);
        private string GetPercentage(float value) => (int)(value * PercentageFactor) + "%";
        private void ApplySettings() => m_SettingsService.SaveCurrentSettings();
        private void ResetSettings() => m_SettingsService.ApplySavedSettings();
    }
}