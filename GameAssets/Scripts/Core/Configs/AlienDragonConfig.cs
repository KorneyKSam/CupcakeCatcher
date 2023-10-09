using NaughtyAttributes;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = nameof(AlienDragonConfig), menuName = "Core/Proglot config", order = 3)]
    public class AlienDragonConfig : ScriptableObject
    {
        private const string Coldowns = nameof(Coldowns);

        [SerializeField] private int m_MaxEnergyPoints = 100;
        [SerializeField] private int m_OneMassDebuff = 20;
        [SerializeField] private float m_EnergyWastingPerSecond = 2f;
        [SerializeField] private float m_DefaultSpeed = 1500f;
        [SerializeField] private float m_NeedleForce = 300f;

        [SerializeField, Foldout(Coldowns)] private float m_EatCooldown = 2f;
        [SerializeField, Foldout(Coldowns)] private float m_CatchCooldown = 0.5f;
        [SerializeField, Foldout(Coldowns)] private float m_NeedleCooldown = 1f;

        public int MaxEnergyPoints => m_MaxEnergyPoints;
        public int OneMassDebuff => m_OneMassDebuff;
        public float DefaultSpeed => m_DefaultSpeed;
        public float EnergyWastingPerSecond => m_EnergyWastingPerSecond;
        public float NeedleForce => m_NeedleForce;
        public float EatCooldown => m_EatCooldown;
        public float CatchCooldown => m_CatchCooldown;
        public float NeedleCooldown => m_NeedleCooldown;
    }
}