using Core.Configs;

namespace Core.Balance.Models
{
    public class AlienDragonModel : IAlienDragonModel
    {
        public AlienDragonConfig Config { get; set; }
        public bool IsNeedleTriggeredMultipleTimes { get; set; }
    }
}