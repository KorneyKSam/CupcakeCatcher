using Balloons.Content;
using Core.Configs;
using Patterns;
using UnityEngine;
using Zenject;

namespace Core
{
    public static class GlobalPools
    {
        private static MonoBehaviourPool<BalloonContent> m_BalloonContentPool;

        public static void InitializeContentPool(BalloonContent prefab, Transform spawnParent, DiContainer diContainer)
        {
            m_BalloonContentPool = new(prefab, spawnParent, CoreGameplayConfig.InitialBalloonCount,
                                       true, diContainer.InstantiatePrefabForComponent<BalloonContent>);
        }

        public static BalloonContent GetBalloonContent() => m_BalloonContentPool.GetFreeElement();
        public static void DeactivateAllContents() => m_BalloonContentPool.DeactivateAllElements();
    }
}