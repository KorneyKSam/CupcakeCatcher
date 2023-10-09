using System;
using UnityEngine;

namespace BalanceSystem
{
    [Serializable]
    public class ConfigRangeValue
    {
        [SerializeField] private float m_Min;
        [SerializeField] private float m_Max;

        public float Min => m_Min;
        public float Max => m_Max;
    }
}