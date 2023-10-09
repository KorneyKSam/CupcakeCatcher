using System;

namespace UI.MainMenu
{
    public class TabActivator : IDisposable
    {
        private readonly ITabController m_ITabController;
        private readonly TabNavigation m_TabNavigation;

        public TabActivator(ITabController iTabController, TabNavigation tabNavigation)
        {
            m_TabNavigation = tabNavigation;
            m_ITabController = iTabController;
            m_TabNavigation.Button.onClick.AddListener(TransitToTab);
        }

        public void Dispose() => m_TabNavigation.Button.onClick.RemoveListener(TransitToTab);

        private void TransitToTab() => m_ITabController.ActivateTab(m_TabNavigation.Tab);
    }
}
