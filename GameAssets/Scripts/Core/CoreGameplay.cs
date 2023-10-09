using Assets;
using Audio;
using Balloons;
using Balloons.Content;
using Common;
using Common.Statistic;
using Core.Balance.Models;
using Core.Configs;
using Core.Features;
using Core.Spawn;
using Core.Statistics;
using Dragon;
using EventManager;
using NewInputSystem;
using Patterns;
using Pause;
using System;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Core
{
    public class CoreGameplay : IPauseHandler, IDisposable
    {
        [Inject] private readonly ISpawnModel m_SpawnModel;
        [Inject] private readonly ICoreGameplayModel m_CoreGameplayModel;

        [Inject] private readonly HUD m_HUD;
        [Inject] private readonly Wind m_Wind;
        [Inject] private readonly Volume m_GlobalVolume;
        [Inject] private readonly EnergyPopup m_EnergyPopup;
        [Inject] private readonly AlienDragon m_AlienDragon;
        [Inject] private readonly DiContainer m_DiContainer;
        [Inject] private readonly PauseManager m_PauseManager;
        [Inject] private readonly GlobalContainers m_Containers;
        [Inject] private readonly BalloonContent m_ContentPrefab;
        [Inject] private readonly DestroyArea m_GlobalDestroyArea;
        [Inject] private readonly BalloonBehaviour m_BalloonPrefab;
        [Inject] private readonly AssetLoader m_AssetLoader;
        [Inject] private readonly AssetAudioService m_AssetAudioService;
        [Inject] private readonly AchievementManager m_AchievementManager;
        [Inject] private readonly TimeOfDayController m_TimeOfDayController;
        [Inject] private readonly GlobalMonoBehaviour m_GlobalMonoBehaviour;

        private IInputController m_InputController;

        private MonoBehaviourPool<BalloonBehaviour> m_BalloonPool;
        private MonoBehvaiourIntervalSpawner<BalloonBehaviour> m_BalloonsSpawner;

        private GameplayStopwatch m_Stopwatch;
        private ChromaticAberration m_ChromaticAberration;

        private bool m_IsInited;

        public event Action GameFinished;

        public bool IsPlaying { get; private set; }
        public LevelStatistic LevelStatistic { get; private set; }

        public void SetInputController(IInputController controller) => m_InputController = controller;

        public void Preload()
        {
            if (!IsPlaying)
            {
                if (!m_IsInited)
                {
                    Init();
                }
                m_AssetLoader.Preload(AssetPreloadGroup.CoreGameplay);
                m_HUD.SetCupcakeCount(0);
                LevelStatistic = new();
                m_AlienDragon.SetTimeOfDayMode(m_CoreGameplayModel.TimeOfDay);
                m_GlobalDestroyArea.gameObject.SetActive(false);
                AddListeners();
            }
        }

        public void StartLevel()
        {
            if (!IsPlaying)
            {
                IsPlaying = true;
                SetActiveHUD(true);
                m_BalloonsSpawner.Start();
                m_Stopwatch.SetUITimer(m_HUD.SetTime);
                m_Stopwatch.StartStopwatch();

                m_InputController.EnableControl();

                if (m_CoreGameplayModel.UseWind)
                {
                    m_Wind.Enable(m_Stopwatch);
                }

                m_TimeOfDayController.SetTimeOfDay(m_CoreGameplayModel.TimeOfDay);

                if (m_CoreGameplayModel.TimeOfDay == TimeOfDay.Night)
                {
                    m_AssetAudioService.PlayMusic(Music.SciFi);
                }
                else if (m_CoreGameplayModel.IsChildMode)
                {
                    m_AssetAudioService.PlayMusic(Music.SmallGuitar);
                }
                else
                {
                    m_AssetAudioService.PlayMusic(Music.JazzyFrenchy);
                }


                m_GlobalVolume.profile.TryGet(out m_ChromaticAberration);

                m_AlienDragon.Enable();
            }
        }

        public void FinishLevel()
        {
            if (IsPlaying)
            {
                IsPlaying = false;
                SetActiveHUD(false);
                m_BalloonsSpawner.Stop();
                m_AlienDragon.Disable();
                RemoveListeners();
                m_Stopwatch.Stop();
                m_Stopwatch.ClearAllTriggers();
                m_Wind.Disable();

                m_InputController.DisableControl();

                if (m_CoreGameplayModel.TimeOfDay == TimeOfDay.Night)
                {
                    m_TimeOfDayController.SetTimeOfDay(TimeOfDay.Day);
                }

                m_AssetLoader.Release(AssetPreloadGroup.CoreGameplay);
                m_GlobalDestroyArea.gameObject.SetActive(true);
            }
        }

        public void Dispose()
        {
            m_ChromaticAberration?.intensity.Override(0f);
            RemoveListeners();
        }

        public void Init()
        {
            m_BalloonPool = new(m_BalloonPrefab, m_Containers.SpawnParent, CoreGameplayConfig.InitialBalloonCount,
                                true, m_DiContainer.InstantiatePrefabForComponent<BalloonBehaviour>);
            GlobalPools.InitializeContentPool(m_ContentPrefab, m_Containers.SpawnParent, m_DiContainer);

            m_BalloonsSpawner = new(new()
            {
                Pool = m_BalloonPool,
                CoroutineMonoBehaviour = m_GlobalMonoBehaviour,
                IntervalDelegate = () => m_SpawnModel.Interval,
                PositionDelegate = () => m_SpawnModel.Position,
                InitializationDelegate = (b) => b.Initialize(),
            });

            m_Stopwatch = new(m_GlobalMonoBehaviour, m_PauseManager);
            m_PauseManager.Register(this);
            m_IsInited = true;
        }

        public void SetPaused(bool isPaused)
        {
            if (IsPlaying)
            {
                if (isPaused)
                {
                    SetActiveHUD(false);
                    m_BalloonsSpawner.Stop();
                    m_Stopwatch.Pause();
                    m_InputController.DisableControl();
                }
                else
                {
                    m_BalloonsSpawner.Start();
                    m_Stopwatch.StartStopwatch();
                    SetActiveHUD(true);
                    m_InputController.EnableControl();
                }
            }
        }

        private void OnFinshedEnergy()
        {
            LevelStatistic.TotalTimeSeconds = m_Stopwatch.CurrentTime;
            m_InputController.DisableControl();
            m_HUD.ShowNoEnergy(() =>
            {
                GameFinished?.Invoke();
                SetActiveHUD(false);
            });
        }

        private void AddListeners()
        {
            m_AlienDragon.HadMeal += OnChangeEnergy;
            m_AlienDragon.ChangedEnergy += OnChangeEnergy;
            m_AlienDragon.FinishedEnergy += OnFinshedEnergy;
            m_AlienDragon.TentacleFreePlacesChanged += OnChangeFreePlaces;

            EventHolder.SmokeImpact.AddListener(OnSmokeImpact);
            EventHolder.CupcakeMissed.AddListener(OnMissedCupcake);
            EventHolder.CupcakeFlattened.AddListener(OnFlattenedCupcake);
            EventHolder.EasterEggFinded.AddListener(OnEasterEggFinded);
        }

        private void RemoveListeners()
        {
            m_AlienDragon.HadMeal -= OnChangeEnergy;
            m_AlienDragon.ChangedEnergy -= OnChangeEnergy;
            m_AlienDragon.FinishedEnergy -= OnFinshedEnergy;
            m_AlienDragon.TentacleFreePlacesChanged -= OnChangeFreePlaces;

            EventHolder.SmokeImpact.RemoveListener(OnSmokeImpact);
            EventHolder.CupcakeMissed.RemoveListener(OnMissedCupcake);
            EventHolder.CupcakeFlattened.RemoveListener(OnFlattenedCupcake);
            EventHolder.EasterEggFinded.RemoveListener(OnEasterEggFinded);
        }

        private void OnChangeEnergy(MealInfo mealInfo)
        {
            LevelStatistic.TotalEnergy += mealInfo.Energy;
            LevelStatistic.RootenFoodCount += mealInfo.RottenFoodCount;
            m_EnergyPopup.ShowText(m_AlienDragon.Position, (int)mealInfo.Energy);
            AddCupcakes(mealInfo.CupcakeCount);
        }

        private void AddCupcakes(int count)
        {
            LevelStatistic.CountOfEatenCupcakes += count;
            m_HUD.SetCupcakeCount(LevelStatistic.CountOfEatenCupcakes);
        }

        private void SetActiveHUD(bool isActive)
        {
            if (isActive)
            {
                m_HUD.Show();
            }
            else
            {
                m_HUD.Hide();
            }
        }

        private void OnEasterEggFinded()
        {
            LevelStatistic.AchevementConditions.NumberOfFindedEggs++;
            m_AchievementManager.Check(LevelStatistic.AchevementConditions);
        }

        private void OnMissedCupcake() => LevelStatistic.MissingCupcakes++;
        private void OnFlattenedCupcake() => LevelStatistic.FlattenedCupcakes++;
        private void OnChangeEnergy(float value) => m_HUD.SetEnergySlider(value / 100f);
        private void OnChangeFreePlaces(int freePlaces) => m_HUD.SetCountOfFreePlaces(freePlaces);
        private void OnSmokeImpact(float value) => m_ChromaticAberration.intensity.Override(Mathf.Clamp01(value));
    }
}