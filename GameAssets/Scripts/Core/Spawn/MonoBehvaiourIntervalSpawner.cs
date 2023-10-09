using System.Collections;
using UnityEngine;

namespace Core.Spawn
{
    public delegate Vector2 PositionDelegate();
    public delegate float IntervalDelegate();
    public delegate void InitializationDelegate<T>(T createdObject);

    public class MonoBehvaiourIntervalSpawner<T> where T : MonoBehaviour
    {
        private readonly MonoBehaviourSpawnerModel<T> m_Model;

        private Coroutine m_Coroutine;

        public MonoBehvaiourIntervalSpawner(MonoBehaviourSpawnerModel<T> model) => m_Model = model;

        public void Start() => m_Coroutine ??= m_Model.CoroutineMonoBehaviour.StartCoroutine(Spawn());

        public void Stop()
        {
            if (m_Coroutine != null)
            {
                m_Model.CoroutineMonoBehaviour.StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_Model.IntervalDelegate.Invoke());
                var freeElement = m_Model.Pool.GetFreeElement();
                freeElement.gameObject.transform.position = m_Model.PositionDelegate.Invoke();
                m_Model.InitializationDelegate?.Invoke(freeElement);
            }
        }
    }
}