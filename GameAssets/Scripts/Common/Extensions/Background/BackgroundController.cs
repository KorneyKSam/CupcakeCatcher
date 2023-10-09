using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Background
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private EndlessMover m_Clouds;
        [SerializeField] private List<FollowingBehaviour> m_ParallaxBehaviours;

        public void ActivateClouds(bool isActive) => m_Clouds.Activate(isActive);

        public void StartParallax(Transform target)
        {
            m_ParallaxBehaviours.ForEach(p => p.StartFollowing(target));
        }

        public void StopParallax()
        {
            m_ParallaxBehaviours.ForEach(p => p.StopFollowing());
        }
    }
}