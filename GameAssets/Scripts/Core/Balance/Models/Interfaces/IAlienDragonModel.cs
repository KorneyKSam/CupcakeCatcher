using Core.Configs;

namespace Core.Balance.Models
{
    public interface IAlienDragonModel
    {
        AlienDragonConfig Config { get; }
        bool IsNeedleTriggeredMultipleTimes { get; }
    }
}