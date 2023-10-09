using NewInputSystem;
using System;
using System.Collections.Generic;

namespace Settings
{
    [Serializable]
    public class GameSettings
    {
        public Dictionary<string, float> ChannelVolumes = new()
        {
            { "Master", 1f },
            { "Music", 0.5f},
            { "Ambient", 0.5f},
            { "Sounds", 0.6f },
        };

        public bool IsFPSCounterEnabled;
        public bool IsVSynchronized;
        public bool IsLimited;
        public float TargetFrameRate;
        public string Language;
        public TouchScreenControlScheme TouchScreenControlScheme;
    }
}