using BalanceSystem;
using Core.Configs;

namespace Core.Balance.Models
{
    public class BalloonModel : IBalloonModel
    {
        public BalloonVisualConfig VisualConfig => BalloonTypes.GetRandomValue().VisualTypes.GetRandomValue();
        public BalloonContentConfig BalloonContentConfig => ContentGetter.GetRandomValue();
        public float Speed => SpeedRange.GetRandom();

        public bool UseEmptyBalloon { get; set; }
        public bool EnableFoodSliding { get; set; }

        public BalanceRange SpeedRange { get; } = new();
        public PercentageValueGetter<BalloonType> BalloonTypes { get; } = new();
        public PercentageValueGetter<BalloonContentConfig> ContentGetter { get; } = new();
    }
}