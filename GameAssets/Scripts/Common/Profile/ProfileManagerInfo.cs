using System;
using System.Collections.Generic;

namespace Profile
{
    [Serializable]
    public class ProfileManagerInfo
    {
        public List<ProfileInfo> Profiles { get; set; } = new();
        public string CurrentProfile { get; set; }
    }
}