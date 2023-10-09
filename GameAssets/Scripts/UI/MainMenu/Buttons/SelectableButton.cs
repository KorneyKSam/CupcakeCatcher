using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public event UnityAction Clicked
        {
            add { m_Button.onClick.AddListener(value); }
            remove { m_Button.onClick.RemoveListener(value); }
        }

        public bool IsSelected
        {
            get => m_IsSelected;
            set
            {
                m_IsSelected = value;
                UpdateColor(value);
            }
        }

        public bool Interacteble { get => m_Button.interactable; set => m_Button.interactable = value; }

        [Header("References")]
        [SerializeField]
        private Image m_ButtonImage;

        [SerializeField]
        private Button m_Button;

        [Header("Settings")]
        [SerializeField]
        private Color m_SelectColor = Color.white;

        [SerializeField]
        private Color m_NormalColor = Color.gray;

        [SerializeField]
        private Color m_HighlightedColor = Color.white;

        private bool m_IsSelected;

        public void OnPointerEnter(PointerEventData eventData) => SetColor(m_HighlightedColor);
        public void OnPointerExit(PointerEventData eventData) => UpdateColor(m_IsSelected);

        private void Init()
        {
            m_ButtonImage ??= GetComponent<Image>();
            m_Button ??= GetComponent<Button>();
            UpdateColor(m_IsSelected);
        }

        private void UpdateColor(bool value) => SetColor(value ? m_SelectColor : m_NormalColor);
        private void SetColor(Color color) => m_ButtonImage.color = color;
        private void Start() => Init();
        private void OnValidate() => Init();
    }
}