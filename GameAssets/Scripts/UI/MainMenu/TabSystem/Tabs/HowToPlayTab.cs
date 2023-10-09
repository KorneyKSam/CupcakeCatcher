namespace UI.TabSystem
{
    public class HowToPlayTab : MonoBehaviourTab
    {
        private void AddListeners()
        {
            RemoveListeners();

        }

        private void RemoveListeners()
        {

        }

        private void OnEnable() => AddListeners();
        private void OnDisable() => RemoveListeners();
    }
}