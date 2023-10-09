using UnityEngine;

namespace Common
{
    public class FollowingBehaviour : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float MoveStrength = 0.1f;
        public bool DisableVerticalParallax;
        public bool IsOppositeMovement = false;

        private Transform m_FollowingTarget;
        private Vector3 m_TargetPreviousPosition;
        private Vector3 m_InitialPosition;
        private Transform m_Transform;

        public void StartFollowing(Transform target)
        {
            m_FollowingTarget = target;
            m_TargetPreviousPosition = m_FollowingTarget.position;
            m_Transform ??= transform;
            m_InitialPosition = m_Transform.position;
        }

        public void StopFollowing()
        {
            m_FollowingTarget = null;
            m_Transform.position = m_InitialPosition;
        }

        private void Update()
        {
            if (m_FollowingTarget != null)
            {
                var delta = m_FollowingTarget.position - m_TargetPreviousPosition;
                if (DisableVerticalParallax)
                {
                    delta.y = 0;
                }

                m_TargetPreviousPosition = m_FollowingTarget.position;
                m_Transform.position = IsOppositeMovement ? m_Transform.position - delta * MoveStrength : m_Transform.position + delta * MoveStrength;
            }
        }
    }
}