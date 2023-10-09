using EventManager;
using GOOfTpeAttribute;
using UnityEngine;

namespace EasterEggs
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EasterEggTriggerArea : MonoBehaviour
    {
        [Tooltip("Game object has to implement " + nameof(IEasterEgg))]
        [GameObjectOfType(typeof(IEasterEgg))]
        [SerializeField]
        private GameObject m_EasterEggGO;

        [SerializeField]
        private BoxCollider2D m_BoxCollider;

        private IEasterEgg m_EasterEgg;

        private void OnValidate() => m_BoxCollider ??= GetComponent<BoxCollider2D>();
        private void Start() => m_EasterEgg = m_EasterEggGO.GetComponent<IEasterEgg>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_EasterEgg.Detonate += OnDetonate;
            m_EasterEgg.EnableEsterEgg = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            m_EasterEgg.Detonate -= OnDetonate;
            m_EasterEgg.EnableEsterEgg = false;
        }

        private void OnDetonate()
        {
            m_EasterEgg.Detonate -= OnDetonate;
            m_BoxCollider.enabled = false;
            EventHolder.EasterEggFinded.Invoke();
        }
    }
}