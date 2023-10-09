using Core;
using Pause;
using UnityEngine;
using Zenject;

namespace Dragon
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public class AlienDragonNeedle : MonoBehaviour, IDestroyable, IPauseHandler
    {
        private const float MagnitudeToDamage = 50f;

        [Inject] private PauseManager m_PauseManager;

        [SerializeField] private Rigidbody2D m_RigidBody;

        private bool m_IsRepetitivelyTriggered;
        private Transform m_Transform;

        private Vector2 m_TempVelocity;
        private float m_TempAngularVelocity;

        private Transform Transform => m_Transform ??= m_RigidBody.transform;

        private void OnValidate() => m_RigidBody = GetComponent<Rigidbody2D>();

        private void Start() => m_PauseManager.Register(this);

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_RigidBody.velocity.magnitude > MagnitudeToDamage)
            {
                if (collision.collider.gameObject.TryGetComponent<IDamagable>(out var damagable))
                {
                    damagable.Damage();
                    if (!m_IsRepetitivelyTriggered)
                    {
                        gameObject.layer = LayerMask.NameToLayer(LayerMasks.Junk);
                    }
                }
                m_RigidBody.velocity = Vector2.zero;
            }
        }

        public void Shoot(Vector3 position, Vector3 direction, Quaternion rotation, float force, bool isRepetitivelyTriggered)
        {
            m_IsRepetitivelyTriggered = isRepetitivelyTriggered;
            gameObject.SetActive(true);
            Transform.SetPositionAndRotation(position, rotation);
            m_RigidBody.velocity = direction * force;
        }

        public void Destroy()
        {
            if (!m_IsRepetitivelyTriggered)
            {
                gameObject.layer = LayerMask.NameToLayer(LayerMasks.Needles);
            }

            Transform.rotation = Quaternion.identity;
            m_RigidBody.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

        public void SetPaused(bool isPaused)
        {
            if (isPaused)
            {
                m_TempVelocity = m_RigidBody.velocity;
                m_TempAngularVelocity = m_RigidBody.angularVelocity;

                m_RigidBody.angularVelocity = 0f;
                m_RigidBody.velocity = Vector2.zero;
                m_RigidBody.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {
                m_RigidBody.velocity = m_TempVelocity;
                m_RigidBody.angularVelocity = m_TempAngularVelocity;
                m_RigidBody.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}