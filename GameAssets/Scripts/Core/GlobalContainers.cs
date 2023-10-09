using UnityEngine;

namespace Core
{
    public class GlobalContainers
    {
        public Transform SpawnParent { get; private set; }

        public GlobalContainers(Transform spawnParent)
        {
            SpawnParent = spawnParent;
        }
    } 
}
