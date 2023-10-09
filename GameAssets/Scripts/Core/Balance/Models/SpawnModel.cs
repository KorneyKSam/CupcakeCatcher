using BalanceSystem;
using Common;
using UnityEngine;

namespace Core.Balance.Models
{
    public class SpawnModel : ISpawnModel
    {
        public float Interval => IntervalRange.GetRandom();
        public Vector2 Position => BoxColliderRandomPosition.GetPosition();

        public BalanceRange IntervalRange { get; } = new();
        public BoxColliderRandomPosition BoxColliderRandomPosition { get; set; }
    }
}