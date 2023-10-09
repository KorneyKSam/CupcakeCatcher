using BalanceSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = nameof(WindConfig), menuName = "Core/Features/Wind config", order = 1)]
    public class WindConfig : ScriptableObject
    {
        public static readonly RandomValueGetter<Vector2> Directions = new(Vector2.left, Vector2.right);

        [SerializeField]
        private float m_Force = 50f;

        [SerializeField]
        [ValidateInput(nameof(IsValidConfigRange), ConfigValidator.NotValidRange)]
        private ConfigRangeValue m_Interval;

        [SerializeField]
        [ValidateInput(nameof(IsValidConfigRange), ConfigValidator.NotValidRange)]
        private ConfigRangeValue m_Duration;

        public float Force => m_Force;
        public ConfigRangeValue Interval => m_Interval;
        public ConfigRangeValue Duration => m_Duration;

        private bool IsValidConfigRange(ConfigRangeValue value) => ConfigValidator.IsValidConfigRange(value);
    }
}