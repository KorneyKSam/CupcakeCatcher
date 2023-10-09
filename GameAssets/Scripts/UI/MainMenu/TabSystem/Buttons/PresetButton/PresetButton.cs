using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabSystem
{
    [RequireComponent(typeof(Button))]
    public class PresetButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_Icon;
        [SerializeField] private List<PresetBackgroundInfo> m_PresetBackgroundInfos;

        private PresetButtonInfo m_PresetInfo;
        private GameObject m_CurrentBackground;

        public event Action<PresetButtonInfo> Clicked;

        private void OnEnable() => m_Button.onClick.AddListener(InvokeClicked);
        private void OnDisable() => m_Button.onClick.RemoveListener(InvokeClicked);
        private void OnValidate() => m_Button ??= GetComponent<Button>();

        public void Initialize(PresetButtonInfo preset)
        {
            m_PresetInfo = preset;
            m_Icon.sprite = preset.Sprite;

            if (m_CurrentBackground != null)
            {
                m_CurrentBackground.gameObject.SetActive(false);
            }

            m_CurrentBackground = m_PresetBackgroundInfos.First(pb => pb.Type == preset.Type).Background;

            m_CurrentBackground.gameObject.SetActive(true);
        }

        private void InvokeClicked()
        {
            if (m_PresetInfo == null)
            {
                return;
            }

            Clicked?.Invoke(m_PresetInfo);
        }
    }
}