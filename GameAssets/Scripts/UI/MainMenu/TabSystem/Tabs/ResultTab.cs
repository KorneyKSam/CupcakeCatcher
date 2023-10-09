using Core.Statistics;
using Patterns;
using UI.Results;
using UnityEngine;
using Zenject;

namespace UI.TabSystem
{
    public class ResultTab : MonoBehaviourTab
    {
        [Inject] private readonly LevelResultManager m_ResultManager;

        [Header("Results content")]
        [SerializeField] private ResultLine m_ResultLinePrefab;
        [SerializeField] private Transform m_ScrollViewContent;

        private MonoBehaviourPool<ResultLine> m_ResultLinePool;

        private void OnEnable()
        {
            m_ResultLinePool ??= new(m_ResultLinePrefab, m_ScrollViewContent, m_ResultManager.CountOfRecords, true);
            UpdateScrollView();
        }

        private void UpdateScrollView()
        {
            m_ResultLinePool.DeactivateAllElements();

            var resultInfos = m_ResultManager.GetUIInfos();

            for (int i = 0; i < resultInfos.Count; i++)
            {
                var line = m_ResultLinePool.GetFreeElement();
                line.Set(resultInfos[i]);
            }
        }
    }
}
