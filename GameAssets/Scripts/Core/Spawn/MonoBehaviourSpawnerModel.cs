using Common;
using Patterns;
using UnityEngine;

namespace Core.Spawn
{
    public class MonoBehaviourSpawnerModel<T> where T : MonoBehaviour
    {
        public PositionDelegate PositionDelegate { get; set; }
        public IntervalDelegate IntervalDelegate { get; set; }
        public InitializationDelegate<T> InitializationDelegate { get; set; }
        public MonoBehaviourPool<T> Pool { get; set; }
        public GlobalMonoBehaviour CoroutineMonoBehaviour { get; set; }
    }
}