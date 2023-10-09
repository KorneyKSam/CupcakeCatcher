using NaughtyAttributes;
using System;
using UnityEngine;

namespace Core.Configs
{
    [Serializable]
    public class BalloonVisualConfig
    {
        [SerializeField] private string m_SpriteName;
        [SerializeField] private Vector2 m_BoxColliderOffset;
        [SerializeField] private Vector2 m_BoxColiderSize;
        [SerializeField] private Vector2 m_ContainerPosition;
        [SerializeField] private bool m_CanBeEmpty = true;
        [SerializeField] private ColorType m_ColorType;
        [ShowIf(nameof(m_ColorType), ColorType.SpecificColor)]
        [AllowNesting]
        [SerializeField] private Color m_SpecificColor = Color.white;

        public string SpriteName => m_SpriteName;
        public Vector2 BoxColliderOffset => m_BoxColliderOffset;
        public Vector2 BoxColiderSize => m_BoxColiderSize;
        public Vector2 ContainerPosition => m_ContainerPosition;
        public Color SpecificColor => m_SpecificColor;
        public ColorType ColorType => m_ColorType;
        public bool CanBeEmpty => m_CanBeEmpty;
    }
}