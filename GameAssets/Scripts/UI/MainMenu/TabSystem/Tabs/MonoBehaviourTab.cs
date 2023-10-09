using Assets;
using Audio;
using System;
using System.Collections.Generic;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.TabSystem
{
    [DisallowMultipleComponent]
    public class MonoBehaviourTab : MonoBehaviour, ITab
    {
        [Inject] private readonly ITabController m_TabController;
        [Inject] private readonly AssetAudioService m_AssetAudioService;

        [Header("Tab")]
        [SerializeField] private string m_TitleKey;
        [SerializeField] private List<TabNavigation> m_Navigations;
        [SerializeField] private List<Button> m_BackButtons;

        private List<TabActivator> m_TabActivators = new();
        private bool m_IsActive;

        public event Action<bool> Activated;

        public string TitleKey => m_TitleKey;
        protected ITabController TabController => m_TabController;

        public void SetActive(bool active)
        {
            if (m_IsActive == active)
            {
                return;
            }

            m_IsActive = active;

            gameObject.SetActive(active);
            if (active)
            {
                m_Navigations.ForEach(n => m_TabActivators.Add(new(m_TabController, n)));
                m_AssetAudioService.PlaySound(AudioAssetType.TabSwitch);
                m_BackButtons.ForEach(b => b.onClick.AddListener(TabController.GetBack));
            }
            else
            {
                m_TabActivators.ForEach(a => a.Dispose());
                m_BackButtons.ForEach(b => b.onClick.RemoveListener(TabController.GetBack));
                m_TabActivators.Clear();
            }

            Activated?.Invoke(active);
        }
    }
}