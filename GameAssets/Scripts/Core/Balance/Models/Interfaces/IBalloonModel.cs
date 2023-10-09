using Core.Configs;

namespace Core.Balance.Models
{
    public interface IBalloonModel
    {
        public BalloonVisualConfig VisualConfig { get; }
        public BalloonContentConfig BalloonContentConfig { get; }
        public bool UseEmptyBalloon { get; }
        public bool EnableFoodSliding { get; }
        public float Speed { get; }
    }
}