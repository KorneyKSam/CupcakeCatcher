using NewInputSystem;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Common
{
    public class PlatformSpecification
    {
        [Inject] private readonly PlayerInput m_PlayerInput;
        [Inject] private readonly HUD m_Hud;
        [Inject] private readonly GameSettingsService m_GameSettingsService;

        public IInputController GetPlatformInputController(IControllable controllable, Camera cameraToConvertScreenPosition)
        {
            if (Application.isMobilePlatform)
            {
                var inputController = new CustomDeviceInputController(controllable, m_Hud, cameraToConvertScreenPosition);
                m_GameSettingsService.SetTouchSchemeForController(inputController);
                return inputController;
            }
            return GetClassicInputController(controllable, cameraToConvertScreenPosition);
        }

        private InputController GetClassicInputController(IControllable controllable, Camera cameraToConvertScreenPosition)
        {
            var inputController = new InputController(m_PlayerInput, controllable, cameraToConvertScreenPosition);
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.LinuxPlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                inputController.UpdateControlScheme(ControlScheme.KeyboardAndMouse);
            }
            else if (Application.platform == RuntimePlatform.Android ||
                     Application.platform == RuntimePlatform.IPhonePlayer &&
                     Touchscreen.current != null)
            {
                inputController.UpdateControlScheme(ControlScheme.Touchscreen);
            }
            else if (Application.platform == RuntimePlatform.XboxOne ||
                     Application.platform == RuntimePlatform.PS5 &&
                     Gamepad.current != null)
            {
                inputController.UpdateControlScheme(ControlScheme.Gamepad);
            }

            return inputController;
        }
    }
}