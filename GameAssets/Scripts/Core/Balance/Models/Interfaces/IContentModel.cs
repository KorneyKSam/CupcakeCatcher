using Balloons.Content;
using UnityEngine;

namespace Core.Balance.Models
{
    public interface IContentModel
    {
        public Sprite MainSprite { get; }
        public Sprite DestroySprite { get; }

        public ContentType ContentType { get; }
        public FoodEffect FoodEffect { get; }
        public GrenadeEffect GrenadeEffect { get; }

        public float Mass { get; }
        public float DissapearanceDelay { get; }
        public bool EnableSliding { get; }
        public int HungerPoints { get; }
    }
}