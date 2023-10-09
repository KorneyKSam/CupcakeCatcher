using System.Collections;
using UnityEngine;

namespace Common
{
    public class EndlessMover : MonoBehaviour
    {
        [SerializeField] private Transform m_Transform;
        [SerializeField] private Transform m_StartTransform;
        [SerializeField] private Transform m_TargetTransform;
        [SerializeField] private float m_MoveSpeed;

        private bool m_IsActive;
        private Coroutine m_Coroutine;

        public void Activate(bool isActive)
        {
            if (m_IsActive == isActive)
            {
                return;
            }

            if (m_IsActive = isActive)
            {
                m_Coroutine ??= StartCoroutine(Move());
            }
            else
            {
                m_Coroutine = null;
                StopAllCoroutines();
            }
        }

        private IEnumerator Move()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            while (true)
            {
                m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_TargetTransform.position, m_MoveSpeed * Time.deltaTime);
                if (m_Transform.position == m_TargetTransform.position)
                {
                    m_Transform.position = m_StartTransform.position;
                }
                yield return waitForEndOfFrame;
            }
        }
    }
}