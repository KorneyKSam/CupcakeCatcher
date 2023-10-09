using BalanceSystem;
using Core.Configs;

namespace Core.Balance.Models
{
    public class GameplayModel : ICoreGameplayModel
    {
        public bool UseEmptyBalloon => SpawnEmptyBalloons ? EmptyBalloonPercentageRange.Contains(BalanceRandom.GetRandomPercentage()) : false;
        public bool UseWind { get; set; }
        public bool IsChildMode { get; set; }
        public TimeOfDay TimeOfDay { get; set; }

        public bool SpawnEmptyBalloons { get; set; }
        public PercentageRange EmptyBalloonPercentageRange { get; set; } = new();

    }
}