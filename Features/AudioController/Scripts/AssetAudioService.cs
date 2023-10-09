using Assets;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AssetAudioService
    {
        [Inject] private readonly AssetLoader m_AssetLoader;
        [Inject] private readonly AudioService m_AudioService;

        public void PlayMusic(Music music)
        {
            m_AudioService.StopMusic();
            m_AssetLoader.ReleaseLastMusic();
            m_AudioService.PlayMusic(GetMusic(music));
        }

        public void PlayAmbient(AudioAssetType audioAssetType) => m_AudioService.PlayAmbient(GetAssetType(audioAssetType));
        public void PlaySound(AudioAssetType audioAssetType, bool useRandomSpitch = false) => m_AudioService.PlaySound(GetAssetType(audioAssetType), useRandomSpitch);

        public AudioClip GetAssetType(AudioAssetType assetType) => m_AssetLoader.LoadAudioClip(assetType);
        public AudioClip GetMusic(Music music) => m_AssetLoader.LoadMusic(music);
    }
}