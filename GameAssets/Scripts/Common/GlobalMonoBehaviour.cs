using System;
using System.Collections;
using UnityEngine;

namespace Common
{
    public class GlobalMonoBehaviour : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(this);

        public MonoBehaviour GetContext() => this;

        public void InvokeAfterDelay(Action action, float delay)
        {
            StartCoroutine(RoutineWithDelay(action, delay));
        }

        private IEnumerator RoutineWithDelay(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}
