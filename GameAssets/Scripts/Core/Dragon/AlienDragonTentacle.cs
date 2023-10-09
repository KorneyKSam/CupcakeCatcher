using Balloons.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dragon
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class AlienDragonTentacle : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BoxCollider2D m_BoxCollider2D;
        [SerializeField] private List<SpriteRenderer> m_TentacleSprites;

        public event Action Changed;

        public float Points { get; private set; }
        public float Mass { get; private set; }
        public int FreePlaces { get; private set; }
        public int CupcakeCount { get; private set; }
        public int RottenFoodCount { get; private set; }

        private void OnValidate() => m_BoxCollider2D ??= GetComponent<BoxCollider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<BalloonContent>(out var content))
            {
                var tentaclePlaceSpriteRnd = m_TentacleSprites.FirstOrDefault(t => t.sprite == null);
                if (tentaclePlaceSpriteRnd != null)
                {
                    var catchedInfo = content.GetInfo();
                    content.Destroy();
                    Points += catchedInfo.HungerPoints;
                    Mass += catchedInfo.Mass;
                    FreePlaces--;
                    CupcakeCount = catchedInfo.HungerPoints > 0 ? CupcakeCount + 1 : CupcakeCount;
                    RottenFoodCount = catchedInfo.HungerPoints < 0 ? RottenFoodCount + 1 : RottenFoodCount;
                    tentaclePlaceSpriteRnd.sprite = catchedInfo.Sprite;
                    Changed?.Invoke();
                }
            }
        }

        public void Enable(bool isEnabled) => m_BoxCollider2D.enabled = isEnabled;

        public void ResetTentacle()
        {
            m_TentacleSprites.ForEach(t => t.sprite = null);
            Points = default;
            Mass = default;
            CupcakeCount = default;
            RottenFoodCount = default;
            FreePlaces = m_TentacleSprites.Count;
        }
    }
}