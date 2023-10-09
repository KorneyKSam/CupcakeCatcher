using System;

namespace EventManager
{
    public class GlobalEvent
    {
        private Action m_Action = delegate { };

        public void Invoke() => m_Action.Invoke();
        public void RemoveListener(Action listener) => m_Action -= listener;
        public void AddListener(Action listener)
        {
            RemoveListener(listener);
            m_Action += listener;
        }
    }

    public class GlobalEvent<T>
    {
        private Action<T> m_Action = delegate { };
        private T m_CurrentState;

        public void Invoke(T param)
        {
            m_CurrentState = param;
            m_Action.Invoke(param);
        }

        public void RemoveListener(Action<T> listener) => m_Action -= listener;

        public void AddListener(Action<T> listener, bool instantNotify = false)
        {
            RemoveListener(listener);
            m_Action += listener;

            if (instantNotify && m_CurrentState != null)
            {
                listener.Invoke(m_CurrentState);
            }
        }
    }
}