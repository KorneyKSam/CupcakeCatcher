namespace UI
{
    public interface ITab
    {
        public string TitleKey { get; }
        public void SetActive(bool active);
    }
}