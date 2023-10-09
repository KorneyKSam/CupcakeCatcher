using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BalanceSystem
{
    public class PercentageValueGetter<T>
    {
        private readonly Dictionary<T, float> m_PercentageMap = new();
        private readonly Dictionary<Func<T>, float> m_PercentageFuncMap = new();

        private Dictionary<T, PercentageRange> m_BuildedMap = new();
        private Dictionary<Func<T>, PercentageRange> m_BuildedFuncMap = new();

        public PercentageValueGetter<T> AddPercentages(IEnumerable<ConfigPercentageValue<T>> configPercentageValues)
        {
            foreach (var configValue in configPercentageValues)
            {
                AddPercentage(configValue.Value, configValue.Percentage);
            }
            return this;
        }

        public PercentageValueGetter<T> AddPercentage(T returnedValue, float percentage)
        {
            m_PercentageMap.Add(returnedValue, percentage);
            return this;
        }

        public PercentageValueGetter<T> AddPercentage(Func<T> funcWithReturnedValue, float percentage)
        {
            m_PercentageFuncMap.Add(funcWithReturnedValue, percentage);
            return this;
        }

        public void Build()
        {
            m_BuildedMap.Clear();
            float totalPercentage = BalanceConstants.MinPercentage;

            foreach (var percentageMap in m_PercentageMap)
            {
                m_BuildedMap.Add(percentageMap.Key, new PercentageRange(totalPercentage, totalPercentage + percentageMap.Value));
                totalPercentage += percentageMap.Value;
            }

            foreach (var percentageMap in m_PercentageFuncMap)
            {
                m_BuildedFuncMap.Add(percentageMap.Key, new PercentageRange(totalPercentage, totalPercentage + percentageMap.Value));
                totalPercentage += percentageMap.Value;
            }

            if (totalPercentage != BalanceConstants.MaxPercentage)
            {
                Debug.LogError($"Balance is not correct! The total percentage is {totalPercentage}. It has to equal to {BalanceConstants.MaxPercentage}");
            }

            m_PercentageMap.Clear();
        }

        public T GetRandomValue()
        {
            var percentage = BalanceRandom.GetRandomPercentage();
            for (int i = 0; i < m_BuildedMap.Count; i++)
            {
                if (m_BuildedMap.ElementAt(i).Value.Contains(percentage))
                {
                    return m_BuildedMap.ElementAt(i).Key;
                }
            }

            for (int i = 0; i < m_BuildedFuncMap.Count; i++)
            {
                if (m_BuildedFuncMap.ElementAt(i).Value.Contains(percentage))
                {
                    var value = m_BuildedFuncMap.ElementAt(i).Key.Invoke() ??
                        throw new NullReferenceException("Returned value from func is null");

                    return value;
                }
            }

            throw new Exception("Balance is not correct!");
        }
    }
}