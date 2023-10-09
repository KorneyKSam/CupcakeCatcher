using Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class TimeOfDayControllerInstaller : MonoInstaller
{
    [SerializeField] private TimeOfDayController m_TimeOfDayController;
    [SerializeField] private SpriteRenderer m_SkyRenderer;
    [SerializeField] private GameObject m_Sun;
    [SerializeField] private GameObject m_Moon;
    [SerializeField] private GameObject m_Stars;
    [SerializeField] private Volume m_GlobalVolume;
    [SerializeField] private Light2D m_GlobalLight;

    public override void InstallBindings()
    {
        var timeOfDayModel = new TimeOfDayModel()
        {
            SkySpriteRnd = m_SkyRenderer,
            Sun = m_Sun,
            Moon = m_Moon,
            Stars = m_Stars,
            GlobalVolume = m_GlobalVolume,
            GlobalLight = m_GlobalLight,
        };
        Container.Bind<Volume>().FromInstance(m_GlobalVolume).AsSingle();
        Container.Bind<TimeOfDayModel>().FromInstance(timeOfDayModel).AsSingle();
        Container.Bind<TimeOfDayController>().FromInstance(m_TimeOfDayController).AsSingle();
    }
}