using System;

namespace Settings
{
    [Serializable]
    public class LevelSettingsFromUI
    {
        public bool IsChildMode { get; set; } = true;
        public bool IsNightMode { get; set; } = false;
    }
}