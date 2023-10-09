using BalanceSystem;
using Core.Configs;
using UnityEngine;

namespace Core.Balance.Models
{
    public class WindModel : IWindModel
    {
        public float Interval => IntervalGetter.GetRandom();
        public float Duration => DurationGetter.GetRandom();

        public float Force { get; set; }
        public Vector2 Direction => WindConfig.Directions.GetRandomValue();

        public BalanceRange IntervalGetter { get; } = new();
        public BalanceRange DurationGetter { get; } = new();
    }
}