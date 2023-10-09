using Common;
using Core.Balance.Models;
using Core.Configs;

namespace Core.Balance
{
    public class CoreBalance
    {
        private readonly WindModel m_WindModel = new();
        private readonly SpawnModel m_SpawnModel = new();
        private readonly BalloonModel m_BalloonModel = new();
        private readonly GameplayModel m_GameplayModel = new();
        private readonly AlienDragonModel m_AlienDragonModel = new();

        private CoreGameplayConfig m_CoreGameplayConfig;
        private BoxColliderRandomPosition m_RandomPositionGetter;

        public WindModel WindModel => m_WindModel;
        public SpawnModel SpawnModel => m_SpawnModel;
        public BalloonModel BalloonModel => m_BalloonModel;
        public GameplayModel GameplayModel => m_GameplayModel;
        public AlienDragonModel AlienDragonModel => m_AlienDragonModel;

        public CoreBalance(BoxColliderRandomPosition boxColliderRandomPosition) => m_RandomPositionGetter = boxColliderRandomPosition;

        public void BuildModels(CoreGameplayConfig coreGameplayConfig, TimeOfDay timeOfDay, bool isChildMode)
        {
            m_CoreGameplayConfig = coreGameplayConfig;
            m_GameplayModel.TimeOfDay = timeOfDay;
            BuildSpawnModel();
            BuildGameplayModel(isChildMode);
            BuildBallonModel();
            BuildContentModel();
            BuildAlienDragonModel();

            if (m_GameplayModel.UseWind)
            {
                BuildWindModel();
            }
        }

        private void BuildSpawnModel()
        {
            m_SpawnModel.BoxColliderRandomPosition = m_RandomPositionGetter;
            m_SpawnModel.IntervalRange.Set(m_CoreGameplayConfig.SpawnInterval);
        }

        private void BuildGameplayModel(bool isChildMode)
        {
            m_GameplayModel.UseWind = m_CoreGameplayConfig.IsWindEnabled;
            m_GameplayModel.IsChildMode = isChildMode;
            m_GameplayModel.SpawnEmptyBalloons = m_CoreGameplayConfig.SpawnEmptyBalloon;
            if (m_CoreGameplayConfig.SpawnEmptyBalloon)
            {
                m_GameplayModel.EmptyBalloonPercentageRange.Set(0f, m_CoreGameplayConfig.EmptyBalloonPercentage);
            }
        }

        private void BuildBallonModel()
        {
            m_BalloonModel.SpeedRange.Set(m_CoreGameplayConfig.BalloonsConfig.Speed);
            m_BalloonModel.BalloonTypes.AddPercentages(m_CoreGameplayConfig.BalloonsConfig.BalloonTypePercentages).Build();
        }

        private void BuildContentModel()
        {
            m_BalloonModel.ContentGetter.AddPercentages(m_CoreGameplayConfig.ContentConfigs).Build();
            m_BalloonModel.EnableFoodSliding = m_CoreGameplayConfig.IsEnableSliding;
        }

        private void BuildAlienDragonModel()
        {
            m_AlienDragonModel.Config = m_CoreGameplayConfig.AlienDragonConfig;
            m_AlienDragonModel.IsNeedleTriggeredMultipleTimes = m_CoreGameplayConfig.IsNeedleTriggeredMultipleTimes;
        }

        private void BuildWindModel()
        {
            m_WindModel.Force = m_CoreGameplayConfig.WindConfig.Force;
            m_WindModel.IntervalGetter.Set(m_CoreGameplayConfig.WindConfig.Interval);
            m_WindModel.DurationGetter.Set(m_CoreGameplayConfig.WindConfig.Duration);
        }
    }
}