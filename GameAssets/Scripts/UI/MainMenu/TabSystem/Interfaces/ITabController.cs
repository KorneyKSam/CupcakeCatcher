using UI.TabSystem;

namespace UI
{
    public interface ITabController
    {
        public void ActivateTab(ITab tab);
        public void SetGroup(TabGroup tabGroup);
        public void GetBack();
    }
}