using Core.Configs;

namespace Core.Balance.Models
{
    public interface ICoreGameplayModel
    {
        bool UseWind { get; }
        bool IsChildMode { get; }
        public TimeOfDay TimeOfDay { get; }
    }
}