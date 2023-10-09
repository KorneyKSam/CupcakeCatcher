using AssetLoading;
using BalanceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace Assets
{
    public class AssetLoader : MonoBehaviour
    {
        [SerializeField] private AssetReferenceAtlasedSprite m_BalloonsAtlas;
        [SerializeField] private AssetReferenceAtlasedSprite m_ContentsAtlas;
        [SerializeField] private List<AssetClipInfo> m_AssetClipInfos;
        [SerializeField] private List<MusicClipInfo> m_MusicClipInfos;

        private AssetReferenceAudioClip m_LastLoadedMusic;

        public void Preload(AssetPreloadGroup assetPreloadGroup)
        {
            if (assetPreloadGroup == AssetPreloadGroup.CoreGameplay)
            {
                AssetReferenceLoader.PreloadAssets<SpriteAtlas>(m_BalloonsAtlas, m_ContentsAtlas);
            }

            foreach (var clipInfo in m_AssetClipInfos)
            {
                if (clipInfo.Group == assetPreloadGroup)
                {
                    AssetReferenceLoader.PreloadAssets<AudioClip>(clipInfo.References.ToArray());
                }
            }
        }

        public void Release(AssetPreloadGroup assetPreloadGroup)
        {
            if (assetPreloadGroup == AssetPreloadGroup.CoreGameplay)
            {
                AssetReferenceLoader.ReleaseAssets(m_BalloonsAtlas, m_ContentsAtlas);
            }

            foreach (var clipInfo in m_AssetClipInfos)
            {
                if (clipInfo.Group == assetPreloadGroup)
                {
                    AssetReferenceLoader.ReleaseAssets(clipInfo.References.ToArray());
                }
            }
        }

        public AudioClip LoadAudioClip(AudioAssetType audioAssetType)
        {
            try
            {
                return LoadClip(m_AssetClipInfos.First(a => a.Type == audioAssetType).References.GetRandomValue());
            }
            catch (Exception excepition)
            {

                throw new Exception($"{nameof(AudioAssetType)}:{audioAssetType} not loaded from list!\n {excepition.Message}");
            }
        }

        public AudioClip LoadMusic(Music music)
        {
            m_LastLoadedMusic = m_MusicClipInfos.First(c => c.Type == music).AssetReference;
            return LoadClip(m_LastLoadedMusic);
        }

        public void ReleaseLastMusic()
        {
            if (m_LastLoadedMusic != null)
            {
                AssetReferenceLoader.ReleaseAsset(m_LastLoadedMusic);
                m_LastLoadedMusic = null;
            }
        }

        public Sprite LoadBalloonSprite(string spriteName) => AssetLoading.AssetReferenceLoader.Load<SpriteAtlas>(m_BalloonsAtlas).GetSprite(spriteName);
        public Sprite LoadContentSprite(string spriteName) => AssetLoading.AssetReferenceLoader.Load<SpriteAtlas>(m_ContentsAtlas).GetSprite(spriteName);
        public AudioClip LoadClip(AssetReferenceAudioClip assetReferenceAudioClip) => AssetLoading.AssetReferenceLoader.Load<AudioClip>(assetReferenceAudioClip);
    }
}