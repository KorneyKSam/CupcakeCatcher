using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetLoading
{
    public static class AssetReferenceLoader
    {
        public static T Load<T>(AssetReference assetReference)
        {
            if (!assetReference.IsValid())
            {
                var handle = assetReference.LoadAssetAsync<T>();
                handle.WaitForCompletion();
                return handle.Result;
            }
            else
            {
                return (T)assetReference.OperationHandle.Result;
            }
        }

        public static async Task<T> LoadAsync<T>(AssetReference assetReference)
        {
            if (!assetReference.IsDone)
            {
                await assetReference.OperationHandle.Task;
                return (T)assetReference.OperationHandle.Result;
            }

            if (!assetReference.IsValid())
            {
                var handle = assetReference.LoadAssetAsync<T>();
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return handle.Result;
                }
                else
                {
                    Debug.LogError($"Asset ({assetReference.Asset.name}) was not loaded, result: {handle.Status}");
                    return default;
                }
            }

            return (T)assetReference.OperationHandle.Result;
        }

        public static void ReleaseAsset(AssetReference assetReference)
        {
            if (assetReference.IsValid())
            {
                assetReference.ReleaseAsset();
            }
        }

        public static void PreloadAssets<T>(params AssetReference[] assetReferences)
        {
            for (int i = 0; i < assetReferences.Length; i++)
            {
                _ = LoadAsync<T>(assetReferences[i]);
            }
        }

        public static void ReleaseAssets(params AssetReference[] assetReferences)
        {
            for (int i = 0; i < assetReferences.Length; i++)
            {
                ReleaseAsset(assetReferences[i]);
            }
        }

        public static void PreloadAssets<T>(List<AssetReference> assetReferences) => assetReferences.ForEach(a => _ = LoadAsync<T>(a));
        public static void ReleaseAssets(List<AssetReference> assetReferences) => assetReferences.ForEach(a => ReleaseAsset(a));
    }
}