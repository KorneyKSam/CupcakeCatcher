using Audio;
using Localization;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem
{
    public class HistoryTab : MonoBehaviourTab
    {
        private readonly Dictionary<string, string> m_Texts = new()
        {
            { "History1", "History1Btn"},
            { "History2", "History2Btn"},
            { "History3", "History3Btn"},
            { "History4", "History4Btn"},
            { "History5", "History5Btn"},
            { "History6", "History6Btn"},
        };

        [Inject] private readonly AssetAudioService m_AssetAudioService;

        [Header("History content")]
        [SerializeField] private TMP_Text m_HistoryText;
        [SerializeField] private TMP_Text m_UniqueBtnText;
        [SerializeField] private Button m_UniqueBtn;

        private int m_HistoryIndex;

        private void OnEnable()
        {
            AddListeners();
            m_HistoryIndex = 0;
            ShowCurrentHistoryStep();
        }

        private void SwitchNextState()
        {
            m_HistoryIndex += 1;
            if (m_Texts.Count <= m_HistoryIndex)
            {
                TabController.GetBack();
                return;
            }

            if (m_HistoryIndex + 1 == m_Texts.Count)
            {
                m_AssetAudioService.PlaySound(Assets.AudioAssetType.Eralash);
            }

            EventSystem.current.SetSelectedGameObject(null);
            ShowCurrentHistoryStep();
        }

        private void ShowCurrentHistoryStep()
        {
            m_HistoryText.text = LocalizationSystem.GetLocalizedValue(m_Texts.ElementAt(m_HistoryIndex).Key);
            m_UniqueBtnText.text = LocalizationSystem.GetLocalizedValue(m_Texts.ElementAt(m_HistoryIndex).Value);
        }

        private void OnDisable() => RemoveListeners();
        private void AddListeners() => m_UniqueBtn.onClick.AddListener(SwitchNextState);
        private void RemoveListeners() => m_UniqueBtn.onClick.RemoveListener(SwitchNextState);
    }
}