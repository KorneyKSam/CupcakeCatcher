using UI;
using UnityEngine;
using EnchancedTouch = UnityEngine.InputSystem.EnhancedTouch;

namespace NewInputSystem
{
    public class CustomDeviceInputController : IInputController
    {
        private readonly HUD m_Hud;
        private readonly IControllable m_Controllable;
        private readonly Camera m_Camera;

        private const int MaxDirectionValue = 1;
        private const int MinDirectionValue = -1;
        private const int Sensetivity = 8;

        private bool m_IsEnabled;
        private EnchancedTouch.Finger m_MovementFinger;
        private EnchancedTouch.Finger m_AimFinger;
        private Vector2 m_LastPosition;
        private TouchScreenControlScheme m_ControlScheme;

        public CustomDeviceInputController(IControllable controllable, HUD hud, Camera cameraToConvertScreenPosition)
        {
            m_Controllable = controllable;
            m_Hud = hud;
            m_Camera = cameraToConvertScreenPosition;
        }

        public void SetTouchScreenControlScheme(TouchScreenControlScheme touchScreenControlScheme)
        {
            m_ControlScheme = touchScreenControlScheme;

            bool isTouchMovement = m_ControlScheme == TouchScreenControlScheme.TouchMovement;
            m_Hud.DeviceControls.AimShootingZone.gameObject.SetActive(isTouchMovement);
            m_Hud.DeviceControls.MovementZone.gameObject.SetActive(isTouchMovement);

            m_Hud.DeviceControls.CustomAim.gameObject.SetActive(!isTouchMovement);
            m_Hud.DeviceControls.ToLeftButton.gameObject.SetActive(!isTouchMovement);
            m_Hud.DeviceControls.ToRightButton.gameObject.SetActive(!isTouchMovement);

            if (m_IsEnabled)
            {
                DisableControl();
                EnableControl();
            }
        }

        public void EnableControl()
        {
            if (!m_IsEnabled)
            {
                m_IsEnabled = true;
                m_Hud.DeviceControls.gameObject.SetActive(true);

                if (m_ControlScheme == TouchScreenControlScheme.TouchMovement)
                {
                    EnchancedTouch.EnhancedTouchSupport.Enable();
                    AddEnhancedTouchListeners();
                }
                else
                {
                    AddArrowSchemeListeners();
                }
            }
        }

        public void DisableControl()
        {
            if (m_IsEnabled)
            {
                m_Hud.DeviceControls.gameObject.SetActive(false);
                if (m_ControlScheme == TouchScreenControlScheme.TouchMovement)
                {
                    EnchancedTouch.EnhancedTouchSupport.Disable();
                    RemoveEnhancedTouchListeners();
                }
                else
                {
                    RemoveArrowSchemeListeners();
                }

                m_IsEnabled = false;
            }
        }

        private void AddEnhancedTouchListeners()
        {
            m_Hud.DeviceControls.EatButton.Button.onClick.AddListener(Eat);
            EnchancedTouch.Touch.onFingerUp += OnFingerUp;
            EnchancedTouch.Touch.onFingerDown += OnFingerDown;
            EnchancedTouch.Touch.onFingerMove += OnFingerMove;
        }

        private void RemoveEnhancedTouchListeners()
        {
            m_Hud.DeviceControls.EatButton.Button.onClick.RemoveListener(Eat);
            EnchancedTouch.Touch.onFingerUp -= OnFingerUp;
            EnchancedTouch.Touch.onFingerDown -= OnFingerDown;
            EnchancedTouch.Touch.onFingerMove -= OnFingerMove;
        }

        private void AddArrowSchemeListeners()
        {
            m_Hud.DeviceControls.EatButton.Button.onClick.AddListener(Eat);
            m_Hud.DeviceControls.ToLeftButton.FingerDown += MoveToLeft;
            m_Hud.DeviceControls.ToLeftButton.FingerUp += RefreshMovement;
            m_Hud.DeviceControls.ToRightButton.FingerDown += MoveToRight;
            m_Hud.DeviceControls.ToRightButton.FingerUp += RefreshMovement;
            m_Hud.DeviceControls.CustomAim.FingerDown += SetAim;
            m_Hud.DeviceControls.CustomAim.FingerMove += SetAim;
            m_Hud.DeviceControls.CustomAim.FingerUp += OnFingerUp;
        }

        private void RemoveArrowSchemeListeners()
        {
            m_Hud.DeviceControls.EatButton.Button.onClick.RemoveListener(Eat);
            m_Hud.DeviceControls.ToLeftButton.FingerDown -= MoveToLeft;
            m_Hud.DeviceControls.ToLeftButton.FingerUp -= RefreshMovement;
            m_Hud.DeviceControls.ToRightButton.FingerDown -= MoveToRight;
            m_Hud.DeviceControls.ToRightButton.FingerUp -= RefreshMovement;
            m_Hud.DeviceControls.CustomAim.FingerDown -= SetAim;
            m_Hud.DeviceControls.CustomAim.FingerMove -= SetAim;
            m_Hud.DeviceControls.CustomAim.FingerUp -= OnFingerUp;
        }

        private void OnFingerDown(EnchancedTouch.Finger finger)
        {
            if (m_MovementFinger == null && RectangleContainsScreenPoint(m_Hud.DeviceControls.MovementZone, finger.currentTouch.startScreenPosition))
            {
                m_MovementFinger = finger;
            }

            if (m_AimFinger == null && RectangleContainsScreenPoint(m_Hud.DeviceControls.AimShootingZone, finger.currentTouch.startScreenPosition))
            {
                m_AimFinger = finger;
                SetAimPosition(finger);
            }
        }

        private void OnFingerMove(EnchancedTouch.Finger finger)
        {
            if (CheckIfMovementFinger(finger))
            {
                var direction = GetDerection(finger.currentTouch.screenPosition, m_LastPosition);

                if (direction != Vector2.zero)
                {
                    m_Controllable.SetMovementDirection(direction);
                }
                m_LastPosition = finger.currentTouch.screenPosition;
            }
            else if (CheckIfAimFinger(finger))
            {
                SetAimPosition(finger);
            }
        }

        private void OnFingerUp(EnchancedTouch.Finger finger)
        {
            if (CheckIfMovementFinger(finger))
            {
                RefreshMovement();
                m_MovementFinger = null;
            }
            else if (CheckIfAimFinger(finger))
            {
                m_Controllable.Shoot();
                m_AimFinger = null;
            }
        }

        private Vector2 GetDerection(Vector2 newPosition, Vector2 oldPosition)
        {
            return new Vector2(newPosition.x > oldPosition.x + Sensetivity ? MaxDirectionValue : newPosition.x < oldPosition.x - Sensetivity ? MinDirectionValue : 0,
                               newPosition.y > oldPosition.y + Sensetivity ? MaxDirectionValue : newPosition.y < oldPosition.y - Sensetivity ? MinDirectionValue : 0);
        }

        private void OnFingerUp(Vector2 screenPosition)
        {
            SetAim(screenPosition);
            m_Controllable.Shoot();
        }

        private void SetAim(Vector2 screenPosition)
        {
            m_Controllable.SetAimPosition(GetWorldPoint(screenPosition));
        }

        private void SetAimPosition(EnchancedTouch.Finger finger) => m_Controllable.SetAimPosition(GetWorldPoint(finger.currentTouch.screenPosition));
        private bool CheckIfMovementFinger(EnchancedTouch.Finger finger) => m_MovementFinger != null && m_MovementFinger == finger;
        private bool CheckIfAimFinger(EnchancedTouch.Finger finger) => m_AimFinger != null && m_AimFinger == finger;
        private bool RectangleContainsScreenPoint(RectTransform rect, Vector2 position) => RectTransformUtility.RectangleContainsScreenPoint(rect, position, m_Camera);
        private Vector2 GetWorldPoint(Vector2 screenPosition) => m_Camera.ScreenToWorldPoint(screenPosition);
        private void Eat() => m_Controllable.Eat();

        private void MoveToLeft() => m_Controllable.SetMovementDirection(Vector2.left);
        private void MoveToRight() => m_Controllable.SetMovementDirection(Vector2.right);
        private void RefreshMovement() => m_Controllable.SetMovementDirection(Vector2.zero);
    }
}