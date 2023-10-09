using UnityEngine;
using UnityEngine.InputSystem;

namespace NewInputSystem
{
    public class InputController : IInputController
    {
        private readonly PlayerInput m_PlayerInput;
        private readonly IControllable m_Controllable;
        private readonly InputAction m_AimAction;
        private readonly InputAction m_EatAction;
        private readonly InputAction m_ShootAction;
        private readonly InputAction m_MovementAction;
        private readonly Camera m_Camera;

        public InputController(PlayerInput playerInput, IControllable controllable, Camera cameraToConvertScreenPosition)
        {
            m_PlayerInput = playerInput;
            m_Controllable = controllable;
            m_PlayerInput.enabled = false;
            m_PlayerInput.neverAutoSwitchControlSchemes = true;
            m_Camera = cameraToConvertScreenPosition;

            m_AimAction = m_PlayerInput.actions.FindAction("Aim");
            m_EatAction = m_PlayerInput.actions.FindAction("Eat");
            m_ShootAction = m_PlayerInput.actions.FindAction("Shoot");
            m_MovementAction = m_PlayerInput.actions.FindAction("Movement");
        }

        public void EnableControl()
        {
            if (!m_PlayerInput.enabled)
            {
                m_PlayerInput.enabled = true;
                AddListeners();
            }
        }

        public void DisableControl()
        {
            if (m_PlayerInput.enabled)
            {
                m_PlayerInput.enabled = false;
                RemoveListeners();
            }
        }

        public void UpdateControlScheme(ControlScheme controlSchemes)
        {
            if (TryToUpdateControlScheme(controlSchemes))
            {
                Debug.Log($"Control scheme was updated, new scheme: {m_PlayerInput.currentControlScheme}");
            }
            else
            {
                Debug.Log($"Control scheme was not updated, default scheme: {m_PlayerInput.currentControlScheme}");
            }
        }

        private void AddListeners()
        {
            m_MovementAction.performed += ChangeMovement;
            m_MovementAction.canceled += ChangeMovement;

            m_AimAction.performed += PerformAim;
            m_ShootAction.performed += PerformShoot;
            m_EatAction.performed += PerformEat;
        }

        private void RemoveListeners()
        {
            m_MovementAction.performed -= ChangeMovement;
            m_MovementAction.canceled -= ChangeMovement;

            m_AimAction.performed -= PerformAim;
            m_ShootAction.performed -= PerformShoot;
            m_EatAction.performed -= PerformEat;
        }

        private bool TryToUpdateControlScheme(ControlScheme controlSchemes)
        {
            switch (controlSchemes)
            {
                case ControlScheme.KeyboardAndMouse:
                    if (Keyboard.current != null && Mouse.current != null)
                    {
                        m_PlayerInput.SwitchCurrentControlScheme(controlSchemes.ToString(), Keyboard.current, Mouse.current);
                        return true;
                    }
                    break;
                case ControlScheme.Gamepad:
                    if (Touchscreen.current != null)
                    {
                        m_PlayerInput.SwitchCurrentControlScheme(controlSchemes.ToString(), Touchscreen.current);
                        return true;
                    }
                    break;
                case ControlScheme.Touchscreen:
                    if (Gamepad.current != null)
                    {
                        m_PlayerInput.SwitchCurrentControlScheme(controlSchemes.ToString(), Gamepad.current);
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }

        private void PerformAim(InputAction.CallbackContext context)
        {
            m_Controllable.SetAimPosition(m_Camera.ScreenToWorldPoint(context.ReadValue<Vector2>()));
        }

        private void PerformShoot(InputAction.CallbackContext context)
        {
            m_Controllable.Shoot();
        }

        private void PerformEat(InputAction.CallbackContext context)
        {
            m_Controllable.Eat();
        }

        private void ChangeMovement(InputAction.CallbackContext context)
        {
            m_Controllable.SetMovementDirection(context.ReadValue<Vector2>());
        }
    }
}