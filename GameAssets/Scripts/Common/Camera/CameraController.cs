using System.Collections;
using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform m_DefaultCameraTarget;
        [SerializeField] private Camera m_MainCamera;

        [Header("Settings")]
        [SerializeField] private Vector3 m_Offset;
        [SerializeField] private float m_SmoothTime;

        private Coroutine m_Coroutine;
        private Transform m_TargetTransform;
        private Transform m_CameraTransform;
        private Vector3 m_Velocity = Vector3.zero;

        public Camera MainCamera => m_MainCamera;

        private void Start() => m_CameraTransform = m_MainCamera.transform;
        private void OnValidate() => m_MainCamera ??= GetComponent<Camera>();

        public void Follow(Transform target)
        {
            m_TargetTransform = target;
            m_Coroutine ??= StartCoroutine(FollowCoroutine());
        }

        public void StopFollow()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        public void ResetPosition() => Follow(m_DefaultCameraTarget);

        private IEnumerator FollowCoroutine()
        {
            var newWaitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                m_CameraTransform.position = Vector3.SmoothDamp(m_CameraTransform.position, m_TargetTransform.position + m_Offset, ref m_Velocity, m_SmoothTime);
                yield return newWaitForFixedUpdate;
            }
        }

    }
}