using EventManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabSystem
{
    public class PauseTab : MonoBehaviourTab
    {
        [Header("Pause content")]
        [SerializeField]
        private Button m_LeaveBtn;

        [SerializeField]
        private Button m_ResumeBtn;

        private void AddListeners()
        {
            RemoveListeners();
            m_ResumeBtn.onClick.AddListener(() => EventHolder.PauseResult.Invoke(true));
            m_LeaveBtn.onClick.AddListener(() => EventHolder.PauseResult.Invoke(false));
        }

        private void RemoveListeners()
        {
            m_ResumeBtn.onClick.RemoveAllListeners();
            m_LeaveBtn.onClick.RemoveAllListeners();
        }

        private void OnEnable() => AddListeners();
        private void OnDisable() => RemoveListeners();
    }
}