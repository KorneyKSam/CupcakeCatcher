using Animation;
using System;
using UnityEngine;

namespace Balloons
{
    public class BalloonVisualisation : AnimatedSprite
    {
        [SerializeField] private SpriteRenderer m_SpriteRnd;

        public void Play(BalloonAnimation balloonAnimation, Action onComplete = null)
        {
            Play(balloonAnimation.ToString(), onComplete);
        }

        public void SetColor(Color color) => m_SpriteRnd.color = color;
        public void SetSprite(Sprite sprite) => m_SpriteRnd.sprite = sprite;
    }
}