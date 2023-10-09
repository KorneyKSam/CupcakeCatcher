using BalanceSystem;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = nameof(CoreGameplayConfig), menuName = "Core/Gameplay config", order = 6)]
    public class CoreGameplayConfig : ScriptableObject
    {
        public const int InitialBalloonCount = 10;

        private const string SpawnSettings = "SPAWN SETTINGS";
        private const string ContentSettings = "CONTENT SETTINGS";
        private const string BalloonSettings = "BALLOON SETTINGS";
        private const string ProglotSettings = "PROGLOT SETTINGS";
        private const string WindSettings = "WIND SETTINGS";
        private const string PreloadSettings = "PRELOAD SETTINGS";
        private const string Features = "FEATURES";

        [Header(BalloonSettings)]
        [Expandable]
        [SerializeField] private BalloonsConfig m_BalloonsConfig;

        [Space(20)]
        [Header(ProglotSettings)]
        [Expandable]
        [SerializeField] private AlienDragonConfig m_ProglotConfig;

        [Space(20)]
        [Header(Features)]
        [SerializeField] private bool m_EnableSplatSliding;
        [SerializeField] private bool m_IsNeedleTriggeredMultipleTimes;
        [SerializeField] private bool m_EnableWind;
        [SerializeField] private bool m_SpawnEmptyBalloons;
        [ShowIf(nameof(m_SpawnEmptyBalloons))]
        [SerializeField] private float m_EmptyBalloonPercentage;

        [Header(WindSettings)]
        [Expandable]
        [ShowIf(nameof(m_EnableWind))]
        [SerializeField] private WindConfig m_WindConfig;

        [Space(20)]
        [Header(SpawnSettings)]
        [SerializeField]
        [ValidateInput(nameof(IsValidConfigRange), ConfigValidator.NotValidRange)]
        private ConfigRangeValue m_SpawnInterval;

        [Space(20)]
        [Header(ContentSettings)]
        [SerializeField]
        [ValidateInput(nameof(IsValidBaloonTypeBalance), ConfigValidator.NotValidPercentage)]
        private List<ConfigPercentageValue<BalloonContentConfig>> m_ContentConfigs;

        public BalloonsConfig BalloonsConfig => m_BalloonsConfig;
        public AlienDragonConfig AlienDragonConfig => m_ProglotConfig;
        public WindConfig WindConfig => m_WindConfig;
        public ConfigRangeValue SpawnInterval => m_SpawnInterval;
        public IEnumerable<ConfigPercentageValue<BalloonContentConfig>> ContentConfigs => m_ContentConfigs;
        public bool IsWindEnabled => m_EnableWind;
        public bool IsEnableSliding => m_EnableSplatSliding;
        public bool IsNeedleTriggeredMultipleTimes => m_IsNeedleTriggeredMultipleTimes;
        public bool SpawnEmptyBalloon => m_SpawnEmptyBalloons;
        public float EmptyBalloonPercentage => m_EmptyBalloonPercentage;

        private bool IsValidConfigRange(ConfigRangeValue value) => ConfigValidator.IsValidConfigRange(value);
        private bool IsValidBaloonTypeBalance(List<ConfigPercentageValue<BalloonContentConfig>> values) => ConfigValidator.IsValidBalance(values);
    }
}