using Core.Configs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Core
{
    public class TimeOfDayController : MonoBehaviour
    {
        [Inject] private readonly TimeOfDayModel m_Model;

        [Header("Day")]
        [SerializeField] private Sprite m_DaySky;
        [SerializeField] private Color m_DayLightColor = Color.white;
        [SerializeField] private float m_DayLightIntencity;
        [SerializeField] private VolumeProfile m_DayProfile;

        [Header("Night")]
        [SerializeField] private Sprite m_NightSky;
        [SerializeField] private Color m_NightLightColor = Color.black;
        [SerializeField] private float m_NightLightIntencity;
        [SerializeField] private VolumeProfile m_NightProfile;

        public void SetTimeOfDay(TimeOfDay timeOfDay)
        {
            bool isDay = timeOfDay == TimeOfDay.Day;

            m_Model.Sun.gameObject.SetActive(isDay);

            m_Model.Moon.gameObject.SetActive(!isDay);
            m_Model.Stars.gameObject.SetActive(!isDay);
            m_Model.SkySpriteRnd.sprite = isDay ? m_DaySky : m_NightSky;
            m_Model.GlobalLight.color = isDay ? m_DayLightColor : m_NightLightColor;
            m_Model.GlobalLight.intensity = isDay ? m_DayLightIntencity : m_NightLightIntencity;
            //m_Tonemapping.mode.value = isDay ? TonemappingMode.None : TonemappingMode.ACES;
            m_Model.GlobalVolume.profile = isDay ? m_DayProfile : m_NightProfile;
        }

        [ContextMenu("Set as day")]
        private void SetAsDay() => SetTimeOfDay(TimeOfDay.Day);

        [ContextMenu("Set as night")]
        private void SetAsNight() => SetTimeOfDay(TimeOfDay.Night);
    }
}