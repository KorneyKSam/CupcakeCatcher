using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public delegate T InstantiateFunction<T>(T prefab, Transform parent) where T : MonoBehaviour;
    public class MonoBehaviourPool<T> where T : MonoBehaviour
    {
        public T Prefab { get; }
        public bool AutoExpand { get; set; }
        public Transform Container { get; }

        private readonly List<T> m_Pool;
        private readonly InstantiateFunction<T> m_InstantiateFunction;

        public MonoBehaviourPool(T prefab, int count = 0, bool autoExpand = false, InstantiateFunction<T> instantiateFunction = null) : this(prefab, null, count, autoExpand, instantiateFunction) { }

        public MonoBehaviourPool(T prefab, Transform container, int count = 0, bool autoExpand = false, InstantiateFunction<T> instantiateFunction = null)
        {
            m_InstantiateFunction = instantiateFunction == null ? Object.Instantiate : instantiateFunction;
            m_Pool = new List<T>(count);
            Prefab = prefab;
            Container = container;
            AutoExpand = autoExpand;

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        public bool HasFreeElements(out T element)
        {
            foreach (var mono in m_Pool)
            {
                if (!mono.gameObject.activeInHierarchy)
                {
                    element = mono;
                    mono.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (HasFreeElements(out var element))
            {
                return element;
            }

            if (AutoExpand)
            {
                return CreateObject(true);
            }
            else
            {
                throw new System.Exception($"There is no free elements in pool of type {typeof(T)}, check your code, or set AutoExpand to true!");
            }
        }

        public List<T> GetFreeElements(uint count)
        {
            var result = new List<T>();
            for (int i = 0; i < count; i++)
            {
                result.Add(GetFreeElement());
            }

            return result;
        }

        public void AddElementToPool(T element)
        {
            element.gameObject.SetActive(false);
            m_Pool.Add(element);
        }

        public void DeactivateAllElements() => m_Pool.ForEach(o => o.gameObject.SetActive(false));

        private T CreateObject(bool isActivateByDefault = false)
        {
            var creratedObject = m_InstantiateFunction.Invoke(Prefab, Container);
            creratedObject.gameObject.SetActive(isActivateByDefault);
            m_Pool.Add(creratedObject);
            return creratedObject;
        }
    }
}