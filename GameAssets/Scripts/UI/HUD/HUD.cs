using DG.Tweening;
using EventManager;
using NewInputSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class HUD : MonoBehaviour
    {
        private const float FadeDuration = 0.5f;
        private const float TextDuration = 4f;

        [SerializeField] private TMP_Text m_CupcakeCounter;
        [SerializeField] private TMP_Text m_Timer;
        [SerializeField] private TMP_Text m_EnergyPercentage;
        [SerializeField] private Button m_PauseButton;
        [SerializeField] private GameObject m_GamepadControls;
        [SerializeField] private DeviceControls m_DeviceControls;
        [SerializeField] private Slider m_EnergySlider;
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TMP_Text m_NoEnergyText1;
        [SerializeField] private TMP_Text m_NoEnergyText2;

        public DeviceControls DeviceControls => m_DeviceControls;

        private void Awake()
        {
            Hide();
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            m_PauseButton.onClick.AddListener(EventHolder.PauseClicked.Invoke);
            m_NoEnergyText1.DOFade(0f, 0f);
            m_NoEnergyText2.DOFade(0f, 0f);
        }

        private void OnDisable() => m_PauseButton.onClick.RemoveListener(EventHolder.PauseClicked.Invoke);

        public void SetEnergySlider(float value)
        {
            m_EnergySlider.value = value;
            m_EnergyPercentage.text = $"{(int)(value * 100f)}%";
        }

        public void SetTime(string time) => m_Timer.text = time;
        public void SetCupcakeCount(int points) => m_CupcakeCounter.text = points.ToString();
        public void ActivateGamepadControls(bool isActive) => m_GamepadControls.gameObject.SetActive(isActive);
        public void SetCountOfFreePlaces(int places) => m_DeviceControls.EatButton.SetCountOfFreePlaces(places);

        public void Show()
        {
            gameObject.SetActive(true);
            DOTween.Sequence().Append(m_CanvasGroup.DOFade(1f, FadeDuration));
        }

        public void Hide()
        {
            FadeHud(() => gameObject.SetActive(false));
        }

        public void ShowNoEnergy(Action onComplete)
        {
            FadeHud();
            float duration = TextDuration / 6;
            DOTween.Sequence().Append(m_NoEnergyText1.DOFade(1f, duration))
                              .AppendInterval(duration)
                              .Append(m_NoEnergyText1.DOFade(0f, duration))
                              .Append(m_NoEnergyText2.DOFade(1f, duration))
                              .AppendInterval(duration)
                              .Append(m_NoEnergyText2.DOFade(0f, duration))
                              .OnComplete(() => onComplete?.Invoke());
        }

        private void FadeHud(Action onComplete = null)
        {
            DOTween.Sequence().Append(m_CanvasGroup.DOFade(0f, FadeDuration)).OnComplete(() => onComplete?.Invoke());
        }
    }
}