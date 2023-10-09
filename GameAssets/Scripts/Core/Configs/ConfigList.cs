namespace Core.Configs
{
    public class ConfigList
    {
        public ConfigList(CoreGameplayConfig childMode, CoreGameplayConfig normalMode)
        {
            ChildMode = childMode;
            NormalMode = normalMode;
        }

        public CoreGameplayConfig ChildMode { get; private set; }
        public CoreGameplayConfig NormalMode { get; private set; }
    }
}