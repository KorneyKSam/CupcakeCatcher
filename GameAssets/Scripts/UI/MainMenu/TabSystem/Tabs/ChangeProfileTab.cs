using DG.Tweening;
using Localization;
using Profile;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem
{
    public class ChangeProfileTab : MonoBehaviourTab
    {
        private readonly Dictionary<ProfileResult, string> m_ResultMessages = new()
        {
            {ProfileResult.Success, "ProfileSuccess" },
            {ProfileResult.EmptyOrWhiteSpace, "ProfileEmpty" },
            {ProfileResult.AlreadyExist,  "ProfileExist" },
        };

        [Inject] private readonly ProfileManager m_ProfileManager;

        [SerializeField] private Button m_AddProfileBtn;
        [SerializeField] private TMP_InputField m_ProfileNameInputField;
        [SerializeField] private TMP_Dropdown m_ProfilesDropDown;
        [SerializeField] private Button m_ChooseBtn;
        [SerializeField] private Button m_CancelBtn;
        [SerializeField] private TMP_Text m_ResultText;
        [SerializeField] private float m_ResultTextDuration;

        private void OnEnable()
        {
            m_ResultText.DOFade(0f, 0f);
            AddListeners();
            RefreshProfileList();
        }

        private void OnDisable() => RemoveListeners();

        private void AddListeners()
        {
            m_AddProfileBtn.onClick.AddListener(AddProfile);
            m_ChooseBtn.onClick.AddListener(ChooseProfile);
        }

        private void RemoveListeners()
        {
            m_AddProfileBtn.onClick.RemoveListener(AddProfile);
            m_ChooseBtn.onClick.RemoveListener(ChooseProfile);
        }

        private void RefreshProfileList()
        {
            m_ProfilesDropDown.ClearOptions();
            m_ProfilesDropDown.AddOptions(m_ProfileManager.ProfileNames);
            m_ProfilesDropDown.value = m_ProfileManager.ProfileNames.IndexOf(m_ProfileManager.CurrentProfileName);
        }

        private void AddProfile()
        {
            if (m_ProfileManager.TryToAddProfileName(m_ProfileNameInputField.text, out var result))
            {
                RefreshProfileList();
            }

            ShowResult(LocalizationSystem.GetLocalizedValue(m_ResultMessages[result]));
        }

        private void ShowResult(string message)
        {
            m_ResultText.text = message;
            var duration = m_ResultTextDuration / 3;
            DOTween.Sequence().Append(m_ResultText.DOFade(1f, duration))
                              .AppendInterval(duration)
                              .Append(m_ResultText.DOFade(0f, duration));
        }

        private void ChooseProfile() => m_ProfileManager.ChooseProfile(m_ProfilesDropDown.options[m_ProfilesDropDown.value].text);
    }
}