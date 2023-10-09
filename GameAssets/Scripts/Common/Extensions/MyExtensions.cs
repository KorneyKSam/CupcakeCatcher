using UnityEngine;

namespace Common
{
    public static class MyExtensions
    {
        public static bool IsNull(this GameObject monoBehaviour)
        {
            return System.Object.ReferenceEquals(monoBehaviour, null);
        }
    }
}