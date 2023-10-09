using BalanceSystem;
using System;
using UnityEngine;

namespace BalanceSystem
{
    [Serializable]
    public class ConfigPercentageValue<T>
    {
        [Range(BalanceConstants.MinPercentage, BalanceConstants.MaxPercentage)]
        [SerializeField] private float m_Percentage;
        [SerializeField] private T m_Value;

        public float Percentage => m_Percentage;
        public T Value => m_Value;
    }
}