namespace Core.Statistics
{
    public class LevelStatistic
    {
        public float TotalEnergy { get; set; }
        public int CountOfEatenCupcakes { get; set; }
        public int RootenFoodCount { get; set; }
        public int FlattenedCupcakes { get; set; }
        public int MissingCupcakes { get; set; }
        public LevelMedal LevelMedal { get; set; }
        public float TotalTimeSeconds { get; set; }
        public AchievementConditions AchevementConditions { get; set; } = new();
    }
}