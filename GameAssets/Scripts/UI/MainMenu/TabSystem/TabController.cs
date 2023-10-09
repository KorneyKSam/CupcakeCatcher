using System;
using System.Collections.Generic;
using UI;
using UI.TabSystem;

namespace MyNamespace
{
    public class TabController : ITabController
    {
        private TabGroup m_CurrentTabGroup;

        private readonly Stack<ITab> m_TabHistory = new();

        public event Action<bool> ActivatedHistory;
        public event Action<string> TitleKeyChanged;

        public bool HasHistory => m_TabHistory.Count > 0;

        public void ActivateTab(ITab tab)
        {
            if (m_CurrentTabGroup != null)
            {
                if (HasHistory)
                {
                    var lastTab = m_TabHistory.Peek();

                    if (tab != lastTab)
                    {
                        lastTab.SetActive(false);
                        AddToHistory(tab);
                    }

                    if (m_CurrentTabGroup.CheckIfRootTab(tab))
                    {
                        ClearHistory();
                        m_TabHistory.Push(tab);
                    }
                }
                else
                {
                    m_TabHistory.Push(tab);
                }
            }

            tab.SetActive(true);
            TitleKeyChanged?.Invoke(tab.TitleKey);
        }

        public void GetBack()
        {
            if (HasHistory)
            {
                m_TabHistory.Pop().SetActive(false);
            }

            if (HasHistory)
            {
                var lastTab = m_TabHistory.Peek();
                lastTab.SetActive(true);
                TitleKeyChanged?.Invoke(lastTab.TitleKey);
                ActivatedHistory?.Invoke(m_TabHistory.Count > 1);
            }
            else
            {
                ActivateTab(m_CurrentTabGroup.RootTab);
            }
        }

        public void SetGroup(TabGroup tabGroup)
        {
            ResetCurrentGroup();
            m_CurrentTabGroup = tabGroup;
            m_CurrentTabGroup.Activate();
            ActivateTab(m_CurrentTabGroup.RootTab);
        }

        public void SetTab(ITab tab)
        {
            ResetCurrentGroup();
            ActivateTab(tab);
        }

        private void ResetCurrentGroup()
        {
            if (m_CurrentTabGroup != null)
            {
                CloseAllTabs();
                ClearHistory();
                m_CurrentTabGroup.Hide();
                m_CurrentTabGroup = null;
            }
        }

        private void AddToHistory(ITab tab)
        {
            m_TabHistory.Push(tab);
            ActivatedHistory?.Invoke(true);
        }

        private void ClearHistory()
        {
            m_TabHistory.Clear();
            ActivatedHistory?.Invoke(false);
        }

        private void CloseAllTabs()
        {
            while (m_TabHistory.Count > 0)
            {
                m_TabHistory.Pop().SetActive(false);
            }
            ActivatedHistory?.Invoke(false);
        }
    }
}