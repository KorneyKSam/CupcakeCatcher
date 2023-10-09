using Common;
using Core;
using EventManager;
using Patterns;
using Settings;
using System.Collections.Generic;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabSystem
{
    public class ChoosePlayModeTab : MonoBehaviourTab
    {
        [Header("Play content")]
        [SerializeField] private SelectableButton m_ChildModeBtn;
        [SerializeField] private SelectableButton m_NormalModeBtn;
        [SerializeField] private Button m_StartGameBtn;
        [SerializeField] private Button m_SaveModeBtn;
        [SerializeField] private Toggle m_NightWalkToggle;
        [SerializeField] private PresetInfoTab m_InfoTab;

        [Header("Preset content")]
        [SerializeField] private PresetButton m_PresetButtonPrefab;
        [SerializeField] private Transform m_ButtonContainer;
        [SerializeField] private PresetButtonInfo m_NightPresetInfo;
        [SerializeField] private List<PresetButtonInfo> m_PresetButtonInfos;

        private GameMode m_SelectedMode;
        private PresetButton m_NightPresetButton;
        private List<PresetButton> m_ActiveButtons;
        private MonoBehaviourPool<PresetButton> m_ButtonsPool;


        private void Awake() => m_ButtonsPool = new(m_PresetButtonPrefab, m_ButtonContainer, 6, true);

        private void OnEnable()
        {
            AddListeners();
            SetMode(m_SelectedMode);
        }

        private void OnDisable() => RemoveListeners();

        private void AddListeners()
        {
            RemoveListeners();
            m_ChildModeBtn.Clicked += SetChildMode;
            m_NormalModeBtn.Clicked += SetNormalMode;
            m_SaveModeBtn.onClick.AddListener(SaveMode);
            m_StartGameBtn.onClick.AddListener(StartLevel);
            m_NightWalkToggle.onValueChanged.AddListener(OnNightToogleChange);
        }

        private void RemoveListeners()
        {
            m_ChildModeBtn.Clicked -= SetChildMode;
            m_NormalModeBtn.Clicked -= SetNormalMode;
            m_SaveModeBtn.onClick.RemoveListener(SaveMode);
            m_StartGameBtn.onClick.RemoveListener(StartLevel);
            m_NightWalkToggle.onValueChanged.RemoveListener(OnNightToogleChange);
        }

        public void StartLevel() => EventHolder.StartedGame.Invoke(GetLevelSettingsFromUI());

        private void SetChildMode() => SetMode(GameMode.ChildMode);
        private void SetNormalMode() => SetMode(GameMode.NormalMode);
        private void SaveMode() => DataService.Save(GetLevelSettingsFromUI());

        private LevelSettingsFromUI GetLevelSettingsFromUI()
        {
            return new LevelSettingsFromUI()
            {
                IsChildMode = m_SelectedMode == GameMode.ChildMode,
                IsNightMode = m_NightWalkToggle.isOn,
            };
        }

        private void SetMode(GameMode gameMode)
        {
            m_SelectedMode = gameMode;
            DeactivateModePresets();
            ActivateModePresets(gameMode);

            switch (m_SelectedMode)
            {
                case GameMode.None:
                    m_NormalModeBtn.IsSelected = false;
                    m_ChildModeBtn.IsSelected = false;
                    m_StartGameBtn.interactable = false;
                    break;
                case GameMode.ChildMode:
                    m_NormalModeBtn.IsSelected = false;
                    m_ChildModeBtn.IsSelected = true;
                    m_StartGameBtn.interactable = true;
                    break;
                case GameMode.NormalMode:
                    m_ChildModeBtn.IsSelected = false;
                    m_NormalModeBtn.IsSelected = true;
                    m_StartGameBtn.interactable = true;
                    break;
                default:
                    break;
            }
        }

        private void OnNightToogleChange(bool isOn)
        {
            if (isOn && m_NightPresetButton == null)
            {
                m_NightPresetButton = GetPresetButton();
                m_NightPresetButton.Initialize(m_NightPresetInfo);
                ActivatePresetButton(m_NightPresetButton);
            }
            else if (!isOn && m_NightPresetButton != null)
            {
                DeactivatePresetButton(m_NightPresetButton);
                m_NightPresetButton.gameObject.SetActive(false);
                m_NightPresetButton = null;
            }
        }

        private void ActivateModePresets(GameMode mode)
        {
            m_ActiveButtons = new();
            var modePresets = m_PresetButtonInfos.FindAll(info => info.Mode == mode);
            foreach (var buttonInfo in m_PresetButtonInfos)
            {
                if (buttonInfo.Mode == mode || buttonInfo.Mode == GameMode.None)
                {
                    var button = m_ButtonsPool.GetFreeElement();
                    button.Initialize(buttonInfo);
                    ActivatePresetButton(button);
                    m_ActiveButtons.Add(button);
                }
            }
        }

        private void DeactivateModePresets()
        {
            m_ActiveButtons?.ForEach(b => DeactivatePresetButton(b));
            m_ActiveButtons = null;
        }

        private void OnPresetButtonClicked(PresetButtonInfo presetInfo)
        {
            m_InfoTab.SetPreset(presetInfo);
            TabController.ActivateTab(m_InfoTab);
        }

        private void ActivatePresetButton(PresetButton presetButton)
        {
            presetButton.Clicked -= OnPresetButtonClicked;
            presetButton.Clicked += OnPresetButtonClicked;
        }

        private void DeactivatePresetButton(PresetButton presetButton)
        {
            presetButton.Clicked -= OnPresetButtonClicked;
            presetButton.gameObject.SetActive(false);
        }

        private PresetButton GetPresetButton() => m_ButtonsPool.GetFreeElement();
    }
}
