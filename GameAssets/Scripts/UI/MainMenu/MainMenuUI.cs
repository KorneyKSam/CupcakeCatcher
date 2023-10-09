using DG.Tweening;
using EventManager;
using Localization;
using MyNamespace;
using Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.TabSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class MainMenuUI : MonoBehaviour
    {
        [Inject] private readonly ProfileManager m_ProfileManager;
        [Inject] private readonly TabController m_TabController;

        [Header("Tab groups")]
        [SerializeField] private List<TabGroup> m_TabGroup;

        [Header("Tabs")]
        [SerializeField] private MonoBehaviourTab m_LuckTab;

        [Header("Common")]
        [SerializeField] private TMP_Text m_TitleText;
        [SerializeField] private TMP_Text m_ChoosenProfileText;
        [SerializeField] private Button m_QuitButton;
        [SerializeField] private Button m_BackButton;

        [Header("Settings")]
        [SerializeField] private Transform m_Airship;
        [SerializeField] private Vector2 m_HideOffset;
        [SerializeField] private float m_Duration;

        private Vector2 m_InitialPosition;

        private void Awake() => m_InitialPosition = m_Airship.localPosition;

        public void Show(TabGroupType tabGroupType, Action onCompleteCallback = null)
        {
            gameObject.SetActive(true);
            m_BackButton.gameObject.SetActive(m_TabController.HasHistory);
            AddListeners();
            m_ChoosenProfileText.text = m_ProfileManager.CurrentProfileName;
            var tabGroup = m_TabGroup.First(t => t.TabType == tabGroupType);
            m_TabController.SetGroup(tabGroup);
            m_Airship.DOLocalMove(m_InitialPosition, m_Duration).OnComplete(() => onCompleteCallback?.Invoke());
        }

        public void Hide(Action onCompleteCallback = null)
        {
            m_TabController.ActivateTab(m_LuckTab);
            m_BackButton.gameObject.SetActive(false);
            m_Airship.DOLocalMove(m_InitialPosition + m_HideOffset, m_Duration).OnComplete(() =>
            {
                gameObject.SetActive(false);
                onCompleteCallback?.Invoke();
            });
            RemoveListeners();
        }

        private void AddListeners()
        {
            RemoveListeners();
            m_TabController.ActivatedHistory += ActivateBackButton;
            m_TabController.TitleKeyChanged += LinkTitleLocalization;
            m_ProfileManager.ChangedCurrentProfile += ChangeProfileName;

            m_BackButton.onClick.AddListener(m_TabController.GetBack);
            m_QuitButton.onClick.AddListener(EventHolder.ExitGameClicked.Invoke);
        }

        private void RemoveListeners()
        {
            m_TabController.ActivatedHistory -= ActivateBackButton;
            m_TabController.TitleKeyChanged -= LinkTitleLocalization;
            m_ProfileManager.ChangedCurrentProfile -= ChangeProfileName;

            m_BackButton.onClick.RemoveListener(m_TabController.GetBack);
            m_QuitButton.onClick.RemoveListener(EventHolder.ExitGameClicked.Invoke);
        }

        private void LinkTitleLocalization(string key) => LocalizationSystem.LinkText(m_TitleText, key);
        private void ActivateBackButton(bool isActive) => m_BackButton.gameObject.SetActive(isActive);
        private void ChangeProfileName(string name) => m_ChoosenProfileText.text = name;
    }
}
