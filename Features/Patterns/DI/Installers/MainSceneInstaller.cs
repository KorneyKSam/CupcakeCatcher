using Assets;
using Audio;
using Balloons;
using Balloons.Content;
using Common;
using Common.Statistic;
using Core;
using Core.Balance;
using Core.Balance.Models;
using Core.Features;
using Core.Statistics;
using Dragon;
using Environment.Background;
using MyNamespace;
using Settings;
using UI;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DI
{
    public class MainSceneInstaller : MonoInstaller
    {
        [Inject] private readonly DiContainer m_DiContainer;

        [SerializeField] private PlayerInput m_PlayerInput;
        [SerializeField] private AlienDragon m_AlienDragon;
        [SerializeField] private MainMenuUI m_MainMenuUI;
        [SerializeField] private HUD m_Hud;
        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private BackgroundController m_BackgroundController;
        [SerializeField] private Wind m_Wind;
        [SerializeField] private GlobalMonoBehaviour m_Context;
        [SerializeField] private BoxColliderRandomPosition m_SpawnPosition;
        [SerializeField] private Transform m_SpawnContainer;
        [SerializeField] private Bootstrap m_Bootstrap;
        [SerializeField] private BalloonContent m_BalloonContent;
        [SerializeField] private BalloonBehaviour m_BalloonBehaviour;
        [SerializeField] private AssetLoader m_AssetLoader;
        [SerializeField] private DestroyArea m_GlobalDestroyArea;
        [SerializeField] private AchievementManager m_AchievementManager;

        public override void InstallBindings()
        {
            BindCoreBalance();
            m_DiContainer.Bind<AssetLoader>().FromInstance(m_AssetLoader).AsSingle();
            m_DiContainer.Bind<AssetAudioService>().FromNew().AsSingle();
            m_DiContainer.BindInterfacesAndSelfTo<AlienDragon>().FromInstance(m_AlienDragon).AsSingle();
            m_DiContainer.Bind<BalloonContent>().FromInstance(m_BalloonContent).AsSingle();
            m_DiContainer.Bind<BalloonBehaviour>().FromInstance(m_BalloonBehaviour).AsSingle();
            m_DiContainer.Bind<GlobalContainers>().FromNew().AsSingle().WithArguments(m_SpawnContainer);
            m_DiContainer.Bind<HUD>().FromInstance(m_Hud).AsSingle();
            m_DiContainer.Bind<GlobalMonoBehaviour>().FromInstance(m_Context).AsSingle();
            m_DiContainer.Bind<MainMenuUI>().FromInstance(m_MainMenuUI).AsSingle();
            m_DiContainer.Bind<CameraController>().FromInstance(m_CameraController).AsSingle();
            m_DiContainer.Bind<PlayerInput>().FromInstance(m_PlayerInput).AsSingle();
            m_DiContainer.Bind<BackgroundController>().FromInstance(m_BackgroundController).AsSingle();
            m_DiContainer.Bind<Wind>().FromInstance(m_Wind).AsSingle();
            m_DiContainer.Bind<PlatformSpecification>().FromNew().AsSingle();
            m_DiContainer.Bind<GameSettingsService>().FromNew().AsSingle();
            m_DiContainer.Bind<CoreGameplay>().FromNew().AsSingle();
            m_DiContainer.Bind<GameBehaviour>().FromNew().AsSingle();
            m_DiContainer.Bind<DestroyArea>().FromInstance(m_GlobalDestroyArea).AsSingle();
            m_DiContainer.Bind<AchievementManager>().FromInstance(m_AchievementManager).AsSingle();
            m_DiContainer.Bind<LevelResultManager>().FromNew().AsSingle();

            m_DiContainer.BindInterfacesAndSelfTo<TabController>().FromNew().AsSingle();
            m_DiContainer.BindInterfacesAndSelfTo<Bootstrap>().FromInstance(m_Bootstrap).AsSingle();
        }

        private void BindCoreBalance()
        {
            var coreBalance = new CoreBalance(m_SpawnPosition);

            m_DiContainer.Bind<IBalloonModel>().FromInstance(coreBalance.BalloonModel).AsSingle();
            m_DiContainer.Bind<ISpawnModel>().FromInstance(coreBalance.SpawnModel).AsSingle();
            m_DiContainer.Bind<IWindModel>().FromInstance(coreBalance.WindModel).AsSingle();
            m_DiContainer.Bind<IAlienDragonModel>().FromInstance(coreBalance.AlienDragonModel).AsSingle();
            m_DiContainer.Bind<ICoreGameplayModel>().FromInstance(coreBalance.GameplayModel).AsSingle();

            m_DiContainer.Bind<CoreBalance>().FromInstance(coreBalance).AsSingle();
        }
    }
}