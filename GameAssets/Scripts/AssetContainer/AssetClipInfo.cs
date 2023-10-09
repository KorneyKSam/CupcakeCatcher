using AssetLoading;
using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class AssetClipInfo
    {
        public AudioAssetType Type;
        public AssetPreloadGroup Group;
        public List<AssetReferenceAudioClip> References;
    }
}