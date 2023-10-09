using System.Threading.Tasks;
using UnityEngine;

namespace AssetLoading
{
    public class BaseAssetsProvider<T> : BaseMonoBehaviourAssetLoader where T : MonoBehaviour
    {
        private T m_MonoBehaviour;

        public Task<T> Load(string assetId)
        {
            if (m_MonoBehaviour == null)
            {
                return LoadInternal<T>(assetId);
            }
            else
            {
                return Task.Run(() => m_MonoBehaviour);
            }
        }

        public void Unload()
        {
            UnloadInternal();
            m_MonoBehaviour = null;
        }
    }
}