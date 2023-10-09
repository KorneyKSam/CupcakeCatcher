using BalanceSystem;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = nameof(BalloonsConfig), menuName = "Core/Balloons/Balloons config", order = 1)]
    public class BalloonsConfig : ScriptableObject
    {
        [SerializeField]
        [ValidateInput(nameof(IsValidConfigRange), ConfigValidator.NotValidRange)]
        private ConfigRangeValue m_Speed;

        [SerializeField]
        [ValidateInput(nameof(IsValidBaloonTypeBalance), ConfigValidator.NotValidPercentage)]
        private List<ConfigPercentageValue<BalloonType>> m_BalloonTypePercentages;

        public ConfigRangeValue Speed => m_Speed;
        public IEnumerable<ConfigPercentageValue<BalloonType>> BalloonTypePercentages => m_BalloonTypePercentages;

        private bool IsValidConfigRange(ConfigRangeValue value) => ConfigValidator.IsValidConfigRange(value);
        private bool IsValidBaloonTypeBalance(List<ConfigPercentageValue<BalloonType>> values) => ConfigValidator.IsValidBalance(values);
    }
}