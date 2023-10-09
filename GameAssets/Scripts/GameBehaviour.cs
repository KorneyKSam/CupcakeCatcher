using Assets;
using Audio;
using Common;
using Core.Balance;
using Core.Configs;
using Core.Statistics;
using Dragon;
using Environment.Background;
using EventManager;
using NewInputSystem;
using Pause;
using Settings;
using System;
using UI.MainMenu;
using UI.TabSystem;
using Zenject;

namespace Core
{
    public class GameBehaviour : IDisposable
    {
        [Inject] private readonly MainMenuUI m_MainMenuUI;
        [Inject] private readonly ConfigList m_ConfigList;
        [Inject] private readonly CoreBalance m_CoreBalance;
        [Inject] private readonly AlienDragon m_AlienDragon;
        [Inject] private readonly PauseManager m_PauseManager;
        [Inject] private readonly CoreGameplay m_CoreGameplay;
        [Inject] private readonly AssetAudioService m_AssetAudioService;

        [Inject] private readonly AssetLoader m_AssetLoader;
        [Inject] private readonly CameraController m_CameraController;
        [Inject] private readonly LevelResultManager m_BestResultManager;
        [Inject] private readonly BackgroundController m_BackgroundController;

        private LevelSettingsFromUI m_GameRoundSettingsFromUI;
        private CoreGameplayConfig m_CurrentConfig;
        private bool m_IsStarted;

        public void Start(IInputController inputController)
        {
            if (m_IsStarted)
            {
                return;
            }

            m_IsStarted = true;
            m_BackgroundController.ActivateClouds(true);
            ShowMainMenu();
            AddListeners();
            PlayFoamRubber();
            PlayAmbient();
            m_BackgroundController.StartParallax(m_CameraController.MainCamera.transform);
            m_CoreGameplay.SetInputController(inputController);
        }

        public void Dispose()
        {
            m_AssetLoader.Release(AssetPreloadGroup.MainMenu);
            RemoveListeners();
            m_CoreGameplay.Dispose();
        }

        private void PreloadGameplay(LevelSettingsFromUI settings)
        {
            if (!m_CoreGameplay.IsPlaying)
            {
                SetLevelSettings(settings);
                m_CoreGameplay.Preload();
                HideMainMenu(StartGameplay);
            }
        }

        private void SetLevelSettings(LevelSettingsFromUI settings)
        {
            m_GameRoundSettingsFromUI = settings;
            m_CurrentConfig = m_GameRoundSettingsFromUI.IsChildMode ? m_ConfigList.ChildMode : m_ConfigList.NormalMode;
            m_CoreBalance.BuildModels(m_CurrentConfig, m_GameRoundSettingsFromUI.IsNightMode ? TimeOfDay.Night : TimeOfDay.Day, settings.IsChildMode);
        }

        private void StartGameplay()
        {
            if (!m_CoreGameplay.IsPlaying)
            {
                m_AssetLoader.Release(AssetPreloadGroup.MainMenu);
                GC.Collect();
                m_CoreGameplay.StartLevel();
                m_CameraController.Follow(m_AlienDragon.transform);
                m_CoreGameplay.GameFinished += ShowLevelResult;
            }
        }

        private void FinishGameplay()
        {
            if (m_CoreGameplay.IsPlaying)
            {
                m_CoreGameplay.FinishLevel();
                PlayFoamRubber();
                m_CameraController.ResetPosition();
                m_CoreGameplay.GameFinished -= ShowLevelResult;
                GC.Collect();
            }
        }

        private void ShowLevelResult()
        {
            m_BestResultManager.Update(m_CoreGameplay.LevelStatistic);
            m_CameraController.ResetPosition();
            m_MainMenuUI.Show(TabGroupType.Result, onCompleteCallback: () =>
            {
                FinishGameplay();
            });
        }

        private void AddListeners()
        {
            EventHolder.StartedGame.AddListener(PreloadGameplay);
            EventHolder.PauseResult.AddListener(OnPauseViewResult);
            EventHolder.PauseClicked.AddListener(OpenPauseMenu);
        }

        private void RemoveListeners()
        {
            EventHolder.StartedGame.RemoveListener(PreloadGameplay);
            EventHolder.PauseResult.RemoveListener(OnPauseViewResult);
            EventHolder.PauseClicked.RemoveListener(OpenPauseMenu);
        }

        private void OpenPauseMenu()
        {
            m_CameraController.ResetPosition();
            m_MainMenuUI.Show(TabGroupType.Pause);
            SetPause(true);
            GC.Collect();
        }


        private void OnPauseViewResult(bool resume)
        {
            if (!m_PauseManager.IsPaused)
            {
                return;
            }

            if (resume)
            {
                m_CameraController.Follow(m_AlienDragon.transform);
                HideMainMenu(() => SetPause(false));
            }
            else
            {
                m_AssetLoader.Release(AssetPreloadGroup.MainMenu);
                SetPause(false);
                FinishGameplay();
                ShowMainMenu();
            }
        }

        private void ShowMainMenu() => m_MainMenuUI.Show(TabGroupType.MainMenu);
        private void PlayAmbient() => m_AssetAudioService.PlayAmbient(AudioAssetType.WindLoop1);
        private void PlayFoamRubber() => m_AssetAudioService.PlayMusic(Music.FoamRubber);
        private void HideMainMenu(Action onComplete) => m_MainMenuUI.Hide(onComplete);
        private void SetPause(bool isPaused) => m_PauseManager.SetPaused(isPaused);
    }
}