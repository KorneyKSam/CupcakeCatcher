using Common;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetLoading
{
    public abstract class BaseMonoBehaviourAssetLoader
    {
        private const string ExceptionMessage = "Detected a second try to load GameObject before unload the old one!";
        private GameObject m_CahdedObject;

        protected async Task<T> LoadInternal<T>(string assetId) where T : MonoBehaviour
        {
            if (m_CahdedObject.IsNull())
            {
                m_CahdedObject = await Addressables.InstantiateAsync(assetId).Task;
                if (m_CahdedObject.TryGetComponent(out T loadedObject) == false)
                {
                    throw new NullReferenceException($"Object of type {typeof(T)} is null on attempt to load it from addressables");
                }

                return loadedObject;
            }
            throw new Exception(ExceptionMessage);
        }

        protected void UnloadInternal()
        {
            if (m_CahdedObject != null)
            {
                m_CahdedObject.SetActive(false);
                Addressables.ReleaseInstance(m_CahdedObject);
                m_CahdedObject = null;
            }
        }
    }
}