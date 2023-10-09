using Animation;
using System;

namespace Animaiton.Dragon
{
    public class AlienDragonAnimator : AnimatedSprite
    {
        private AlienDragonAnimation m_CurrentAnimation;

        public void Play(AlienDragonAnimation alienDragonAnimation, Action onComplete = null)
        {
            if (m_CurrentAnimation != alienDragonAnimation)
            {
                m_CurrentAnimation = alienDragonAnimation;
                Play(m_CurrentAnimation.ToString(), onComplete);
            }
        }
    }
}