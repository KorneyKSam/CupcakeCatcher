using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.TabSystem
{
    public class TabGroup : MonoBehaviour
    {
        [Inject] private readonly ITabController m_TabCotnroller;

        [SerializeField] private TabGroupType m_TabGroupType;
        [SerializeField] private MonoBehaviourTab m_RootTab;
        [SerializeField] private List<TabButtonInfo> m_ButtonInfos;

        private bool m_IsActive;

        public TabGroupType TabType => m_TabGroupType;
        public ITab RootTab => m_RootTab;

        public void Activate()
        {
            if (!m_IsActive)
            {
                foreach (var buttonInfo in m_ButtonInfos)
                {
                    buttonInfo.TabButton.gameObject.SetActive(true);
                    buttonInfo.TabButton.SetCallback(() => m_TabCotnroller.ActivateTab(buttonInfo.MonoBehaviourrTab));
                    buttonInfo.MonoBehaviourrTab.Activated += buttonInfo.TabButton.SetSelected;
                }
                m_IsActive = true;
            }
        }

        public void Hide()
        {
            if (m_IsActive)
            {
                foreach (var buttonInfo in m_ButtonInfos)
                {
                    buttonInfo.TabButton.gameObject.SetActive(false);
                    buttonInfo.MonoBehaviourrTab.Activated -= buttonInfo.TabButton.SetSelected;
                }
                m_IsActive = false;
            }
        }

        public bool CheckIfRootTab(ITab tab) => m_RootTab.Equals(tab);

    }
}