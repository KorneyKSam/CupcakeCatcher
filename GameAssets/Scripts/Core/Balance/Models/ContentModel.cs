using Balloons.Content;
using UnityEngine;

namespace Core.Balance.Models
{
    public class ContentModel : IContentModel
    {
        public Sprite MainSprite { get; set; }
        public Sprite DestroySprite { get; set; }

        public ContentType ContentType { get; set; }
        public FoodEffect FoodEffect { get; set; }
        public GrenadeEffect GrenadeEffect { get; set; }

        public float Mass { get; set; }
        public float DissapearanceDelay { get; set; }
        public int HungerPoints { get; set; }
        public bool EnableSliding { get; set; }
    }
}