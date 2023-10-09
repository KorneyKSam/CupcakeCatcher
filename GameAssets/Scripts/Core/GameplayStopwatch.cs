using Common;
using Pause;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class GameplayStopwatch
    {
        private readonly GlobalMonoBehaviour m_MonoBehaviour;
        private readonly PauseManager m_PauseManager;

        private Action<string> m_UpdateUI;
        private Coroutine m_Coroutine;
        private float m_CurentTime;
        private List<StopwatchTrigger> m_Triggers = new();

        public float CurrentTime => m_CurentTime;

        public GameplayStopwatch(GlobalMonoBehaviour monoBehaviour, PauseManager pauseManager)
        {
            m_MonoBehaviour = monoBehaviour;
            m_PauseManager = pauseManager;
        }

        public void AddTrigger(float triggerTime, Action triggerAction)
        {
            m_Triggers.Add(new StopwatchTrigger(triggerTime, triggerAction));
            OrderByTime();
        }

        public void AddTriggers(float triggerTime, params Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                m_Triggers.Add(new StopwatchTrigger(triggerTime, actions[i]));
            }
            OrderByTime();
        }

        public void Stop()
        {
            StopCoroutine();
            m_CurentTime = 0f;
            TryToUpdateUI();
        }

        public void Pause() => StopCoroutine();
        public void StartStopwatch() => m_Coroutine ??= m_MonoBehaviour.StartCoroutine(StartTick());
        public void SetUITimer(Action<string> uiAction) => m_UpdateUI = uiAction;
        public void ClearAllTriggers() => m_Triggers.Clear();

        private IEnumerator StartTick()
        {
            if (m_PauseManager.IsPaused)
            {
                yield return null;
            }

            var waitForEndOfFrame = new WaitForEndOfFrame();
            while (true)
            {
                m_CurentTime += Time.deltaTime;
                if (m_Triggers.Count > 0 && m_Triggers[0].TriggerTime <= m_CurentTime)
                {
                    var aciton = m_Triggers[0].TriggerAction;
                    m_Triggers.Remove(m_Triggers[0]);
                    aciton?.Invoke();
                }
                TryToUpdateUI();
                yield return waitForEndOfFrame;
            }
        }

        private void StopCoroutine()
        {
            if (m_Coroutine != null)
            {
                m_MonoBehaviour.StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        private void TryToUpdateUI()
        {
            if (m_UpdateUI != null)
            {
                m_UpdateUI.Invoke(m_CurentTime.ConvertToTimeFormat());
            }
        }

        private void OrderByTime()
        {
            if (m_Triggers.Count > 1)
            {
                m_Triggers = m_Triggers.OrderBy(t => t.TriggerTime).ToList();
            }
        }
    }
}