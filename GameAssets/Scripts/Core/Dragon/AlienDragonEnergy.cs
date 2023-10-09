using System;
using System.Collections;
using UnityEngine;

namespace Dragon
{
    public class AlienDragonEnergy
    {
        private const float EnergyWaitTime = 1f;

        private readonly MonoBehaviour m_Context;

        private Coroutine m_Coroutine;

        private float m_WastePerSecond;
        private float m_CurrentEnergy;
        private float m_MaxEnergy;

        public AlienDragonEnergy(MonoBehaviour context) => m_Context = context;

        public event Action<float> ChangedEnergy;
        public event Action FinishedEnergy;

        public void Init(float maxEnergy, float wastePerSecond)
        {
            m_MaxEnergy = maxEnergy;
            m_WastePerSecond = wastePerSecond;
            m_CurrentEnergy = m_MaxEnergy;
        }

        public void StartWaste()
        {
            m_Coroutine ??= m_Context.StartCoroutine(EnergyRoutine());
            ChangedEnergy?.Invoke(m_CurrentEnergy);
        }

        public void StopWaste()
        {
            if (m_Coroutine != null)
            {
                m_Context.StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }

        public void AddEnergy(float energy)
        {
            m_CurrentEnergy = ClampEnergy(m_CurrentEnergy + energy);
            ChangedEnergy?.Invoke(m_CurrentEnergy);

            if (m_CurrentEnergy <= 0)
            {
                FinishedEnergy?.Invoke();
            }
        }

        private IEnumerator EnergyRoutine()
        {
            var waitForSecond = new WaitForSeconds(EnergyWaitTime);

            while (true)
            {
                yield return waitForSecond;
                AddEnergy(-m_WastePerSecond);
            }
        }

        private float ClampEnergy(float amount) => amount > m_MaxEnergy ? m_MaxEnergy : amount < 0 ? 0 : amount;
    }
}