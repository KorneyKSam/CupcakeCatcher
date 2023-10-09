using Common;
using System;
using System.Collections.Generic;

namespace Profile
{
    public class ProfileManager
    {
        private const string DefaultName = "Player";

        private readonly ProfileManagerInfo m_ProfileManagerInfo;

        private List<string> m_ProfileNames;

        public ProfileManager()
        {
            m_ProfileManagerInfo = DataService.Load<ProfileManagerInfo>();

            if (m_ProfileManagerInfo.Profiles.Count == 0)
            {
                m_ProfileManagerInfo.Profiles.Add(new ProfileInfo() { Name = DefaultName });
                m_ProfileManagerInfo.CurrentProfile = DefaultName;
                SaveInfo();
            }
        }

        public event Action<string> ChangedCurrentProfile;

        public string CurrentProfileName => m_ProfileManagerInfo.CurrentProfile;
        public List<string> ProfileNames => m_ProfileNames ??= CreateProfileNames();

        public bool TryToAddProfileName(string profileName, out ProfileResult result)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                result = ProfileResult.EmptyOrWhiteSpace;
                return false;
            }
            else if (ProfileNames.Contains(profileName))
            {
                result = ProfileResult.AlreadyExist;
                return false;
            }
            else
            {
                result = ProfileResult.Success;
                ProfileNames.Add(profileName);
                m_ProfileManagerInfo.Profiles.Add(new ProfileInfo() { Name = profileName });
                SaveInfo();
                return true;
            }
        }

        public void ChooseProfile(string profileName)
        {
            if (ProfileNames.Contains(profileName))
            {
                m_ProfileManagerInfo.CurrentProfile = profileName;
                SaveInfo();
                ChangedCurrentProfile?.Invoke(m_ProfileManagerInfo.CurrentProfile);
            }
        }

        private List<string> CreateProfileNames()
        {
            if (m_ProfileManagerInfo.Profiles.Count == 0)
            {
                return new List<string>() { m_ProfileManagerInfo.CurrentProfile };
            }

            var result = new List<string>();
            m_ProfileManagerInfo.Profiles.ForEach(p => result.Add(p.Name));
            return result;
        }

        private void SaveInfo() => DataService.Save(m_ProfileManagerInfo);
    }
}