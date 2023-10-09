using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetLoading
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid) { }
    }
}