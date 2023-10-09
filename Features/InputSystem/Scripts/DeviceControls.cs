using UI;
using UnityEngine;

namespace NewInputSystem
{
    public class DeviceControls : MonoBehaviour
    {
        public RectTransform AimShootingZone => m_AimShootingZone;
        public RectTransform MovementZone => m_MovementZone;
        public EatButton EatButton => m_EatButton;
        public Arrow ToLeftButton => m_ToLeftBtn;
        public Arrow ToRightButton => m_ToRightBtn;
        public CustomAim CustomAim => m_CustomAim;

        [SerializeField] private RectTransform m_AimShootingZone;
        [SerializeField] private RectTransform m_MovementZone;
        [SerializeField] private EatButton m_EatButton;
        [SerializeField] private Arrow m_ToLeftBtn;
        [SerializeField] private Arrow m_ToRightBtn;
        [SerializeField] private CustomAim m_CustomAim;
    }
}