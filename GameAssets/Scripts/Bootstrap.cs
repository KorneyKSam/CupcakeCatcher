using Core;
using Dragon;
using EventManager;
using NewInputSystem;
using Settings;
using UI;
using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Common
{
    public class Bootstrap : MonoBehaviour, IInitializable
    {
        [Inject] private readonly AlienDragon m_AlienDragon;
        [Inject] private readonly CameraController m_CameraController;
        [Inject] private readonly GameSettingsService m_GameSettingsService;
        [Inject] private readonly PlatformSpecification m_PlatformSpecification;
        [Inject] private readonly GameBehaviour m_GameBehaviour;
        [Inject] private readonly HUD m_HUD;
        [Inject] private readonly MainMenuUI m_MainMenuUI;

        private IInputController m_InputController;

        private void OnDestroy()
        {
            m_GameBehaviour.Dispose();
            RemoveListeners();
        }

        public void Initialize()
        {
            var safeArea = new SafeArea();
            safeArea.ScaleRectTransform(m_HUD.GetComponent<RectTransform>());
            safeArea.ScaleRectTransform(m_MainMenuUI.GetComponent<RectTransform>());
            m_GameSettingsService.ApplySavedSettings();
            m_InputController = m_PlatformSpecification.GetPlatformInputController(m_AlienDragon, m_CameraController.MainCamera);
            m_GameBehaviour.Start(m_InputController);
            AddListeners();
        }

        private void Close()
        {
            m_GameBehaviour.Dispose();
            RemoveListeners();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#else
            Application.Quit();
#endif
        }

        private void AddListeners() => EventHolder.ExitGameClicked.AddListener(Close);
        private void RemoveListeners() => EventHolder.ExitGameClicked.RemoveListener(Close);
    }
}