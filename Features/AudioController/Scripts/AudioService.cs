using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioService : MonoBehaviour
    {
        private const float MinPitch = 0.75f;
        private const float MaxPitch = 1f;

        [Header("References")]
        [SerializeField] private AudioSource m_SoundSource;
        [SerializeField] private AudioSource m_MusicSource;
        [SerializeField] private AudioSource m_AmbientSource;

        public void PlaySound(AudioClip audioClip, bool useRandomPitch = false)
        {
            if (useRandomPitch)
            {
                m_SoundSource.pitch = Random.Range(MinPitch, MaxPitch);
            }
            else
            {
                m_SoundSource.pitch = 1f;
            }
            m_SoundSource.PlayOneShot(audioClip);
        }

        public void PlayAmbient(AudioClip audioClip)
        {
            m_AmbientSource.clip = audioClip;
            m_AmbientSource.loop = true;
            m_AmbientSource.Play();
        }

        public void PlayMusic(AudioClip audioClip, bool isLooped = true)
        {
            m_MusicSource.clip = audioClip;
            m_MusicSource.loop = isLooped;
            m_MusicSource.Play();
        }

        public void StopMusic() => m_MusicSource.Stop();
        public void StopAmbient() => m_AmbientSource.Stop();
    }
}