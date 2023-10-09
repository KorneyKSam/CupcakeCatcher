using Balloons.Content;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = nameof(BalloonContentConfig), menuName = "Core/Content/Config", order = 2)]
    public class BalloonContentConfig : ScriptableObject
    {
        [SerializeField] private ContentType m_ContentType;
        [ShowIf(nameof(m_ContentType), ContentType.Food)]
        [SerializeField] private int m_HungerPoints = 1;
        [ShowIf(nameof(m_ContentType), ContentType.Food)]
        [SerializeField] private FoodEffect m_FoodEffect;
        [ShowIf(nameof(m_ContentType), ContentType.Grenade)]
        [SerializeField] private GrenadeEffect m_DestroyEffect;
        [SerializeField] private float m_DissapearanceDelay = 2;
        [SerializeField] private float m_Mass = 4f;
        [SerializeField] private List<ContentSpritesConfig> m_ContentSprites;

        public float DissapearanceDelay => m_DissapearanceDelay;
        public ContentType ContentType => m_ContentType;
        public GrenadeEffect GrenadeEffect => m_DestroyEffect;
        public FoodEffect FoodEffect => m_FoodEffect;
        public int Energy => m_HungerPoints;
        public float Mass => m_Mass;
        public List<ContentSpritesConfig> ContentSprites => m_ContentSprites;
    }
}