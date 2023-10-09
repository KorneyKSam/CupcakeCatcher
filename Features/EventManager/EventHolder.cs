using Settings;

namespace EventManager
{
    public static class EventHolder
    {
        public static GlobalEvent<LevelSettingsFromUI> StartedGame = new();
        public static GlobalEvent<bool> PauseResult = new();
        public static GlobalEvent<float> SmokeImpact = new();

        public static GlobalEvent PauseClicked = new();
        public static GlobalEvent ExitGameClicked = new();
        public static GlobalEvent CupcakeMissed = new();
        public static GlobalEvent CupcakeFlattened = new();
        public static GlobalEvent EasterEggFinded = new();
    }
}