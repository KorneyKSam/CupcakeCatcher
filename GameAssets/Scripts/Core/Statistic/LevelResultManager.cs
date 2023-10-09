using Common;
using Profile;
using System.Collections.Generic;
using Zenject;

namespace Core.Statistics
{
    public class LevelResultManager
    {
        private const string ResultsFileName = "TableResults";

        [Inject] private readonly ProfileManager m_ProfileManager;

        private List<TableResultInfo> m_TableResultInfos;
        private LevelStatistic m_LastStatistic;

        private readonly Dictionary<LevelMedal, float> m_MedalTimes = new()
        {
            {LevelMedal.Bronze,  240f},
            {LevelMedal.Silver, 420f },
            {LevelMedal.Gold, 600f },
            {LevelMedal.Platinum, 900f }
        };

        public int CountOfRecords => m_TableResultInfos.Count;
        public LevelStatistic LevelStatistic => m_LastStatistic;

        public LevelResultManager()
        {
            m_TableResultInfos = DataService.Load<List<TableResultInfo>>(ResultsFileName);
        }

        public List<UITableResultInfo> GetUIInfos()
        {
            var result = new List<UITableResultInfo>();

            for (int i = 0; i < m_TableResultInfos.Count; i++)
            {
                result.Add(new()
                {
                    Number = i + 1,
                    CupcakeCount = m_TableResultInfos[i].LevelStatistic.CountOfEatenCupcakes,
                    ProfileName = m_TableResultInfos[i].ProfileName,
                    TotalTimeSeconds = m_TableResultInfos[i].LevelStatistic.TotalTimeSeconds,
                    IsCurrentProfile = m_TableResultInfos[i].ProfileName == m_ProfileManager.CurrentProfileName,
                    IsLastGame = m_LastStatistic == m_TableResultInfos[i].LevelStatistic
                });
            }

            return result;
        }

        public bool Update(LevelStatistic levelStatistic)
        {
            m_LastStatistic = levelStatistic;

            bool result = false;
            int numberInTable = 0;

            if (m_TableResultInfos.Count == 0)
            {
                InsertValue(numberInTable, m_LastStatistic);
                result = true;
            }
            else
            {
                for (int i = 0; i < m_TableResultInfos.Count; i++)
                {
                    if (m_TableResultInfos[i].LevelStatistic.TotalTimeSeconds >= m_LastStatistic.TotalTimeSeconds)
                    {
                        numberInTable++;
                    }
                    else
                    {
                        InsertValue(numberInTable, m_LastStatistic);
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }


        private void InsertValue(int index, LevelStatistic levelStatistic)
        {
            m_TableResultInfos.Insert(index, new()
            {
                ProfileName = m_ProfileManager.CurrentProfileName,
                LevelStatistic = levelStatistic,
            });
            DataService.Save(m_TableResultInfos, fileName: ResultsFileName);
        }
    }
}