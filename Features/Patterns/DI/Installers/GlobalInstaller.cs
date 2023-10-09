using Assets;
using Audio;
using Core.Configs;
using Pause;
using Profile;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace DI
{
    public class GlobalInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private AudioMixer m_AudioMixer;
        [SerializeField] private FPSCounter m_FPSCounter;
        [SerializeField] private AudioService m_AudioService;
        [SerializeField] private EnergyPopup m_EnergyPopup;

        [Header("Configs")]
        [SerializeField] private CoreGameplayConfig m_ChildMode;
        [SerializeField] private CoreGameplayConfig m_NormalMode;

        public override void InstallBindings()
        {
            var fpsCounter = InstantiateWithDefaultValues(m_FPSCounter);
            var audioService = InstantiateWithDefaultValues(m_AudioService);
            var energyPopup = InstantiateWithDefaultValues(m_EnergyPopup);

            Container.Bind<FPSCounter>().FromInstance(fpsCounter).AsSingle();
            Container.Bind<AudioService>().FromInstance(audioService).AsSingle();
            Container.Bind<EnergyPopup>().FromInstance(energyPopup).AsSingle();
            Container.Bind<AudioMixer>().FromInstance(m_AudioMixer).AsSingle();
            Container.Bind<PauseManager>().FromNew().AsSingle();
            Container.Bind<ProfileManager>().FromNew().AsSingle();
            Container.Bind<ConfigList>().FromNew().AsSingle().WithArguments(m_ChildMode, m_NormalMode);
            Container.Bind<DiContainer>().FromInstance(Container);
        }

        private T InstantiateWithDefaultValues<T>(T prefab) where T : MonoBehaviour
        {
            return Container.InstantiatePrefabForComponent<T>(prefab, Vector2.zero, Quaternion.identity, null);
        }
    }
}