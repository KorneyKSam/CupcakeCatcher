using Audio;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        [Inject] private AssetAudioService m_AssetAudioService;

        [SerializeField] private Button m_Button;
        [SerializeField] private Vector3 m_SelectOffset;
        [SerializeField] private float m_Duration;

        private Transform m_ButtonTransform;
        private Vector3 m_InitialPosition;
        private Action m_Callback;
        private bool m_IsSelected;

        private void Awake()
        {
            m_ButtonTransform = transform;
            m_InitialPosition = m_ButtonTransform.localPosition;
        }

        private void OnValidate() => m_Button ??= GetComponent<Button>();
        private void OnEnable() => m_Button.onClick.AddListener(InvokeCallback);
        private void OnDisable() => m_Button.onClick.RemoveListener(InvokeCallback);

        public void SetSelected(bool isSelected)
        {
            if (m_IsSelected == isSelected)
            {
                return;
            }

            m_IsSelected = isSelected;
            m_ButtonTransform.DOLocalMove(isSelected ? m_InitialPosition + m_SelectOffset
                                                    : m_InitialPosition, m_Duration);
        }

        public void SetCallback(Action callback) => m_Callback = callback;

        private void InvokeCallback()
        {
            m_AssetAudioService.PlaySound(Assets.AudioAssetType.BalloonPressed);
            m_Callback?.Invoke();
        }
    }
}