using Animaiton.Dragon;
using Audio;
using Common;
using Core;
using Core.Balance.Models;
using Core.Configs;
using NewInputSystem;
using Patterns;
using Pause;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Dragon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AlienDragon : MonoBehaviour, IInitializable, IControllable, ISlidable, IPauseHandler
    {
        [Inject] private readonly IAlienDragonModel m_Model;

        [Inject] private readonly DiContainer m_DiContainer;
        [Inject] private readonly PauseManager m_PauseManager;
        [Inject] private readonly AssetAudioService m_AssetAudioService;
        [Inject] private readonly GlobalContainers m_GameObjectContainers;
        [Inject] private readonly GlobalMonoBehaviour m_GlobalMonoBehaviour;

        [Header("Dragon references")]
        [SerializeField] private Transform m_AimTransform;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        [SerializeField] private AlienDragonAnimator m_Animator;
        [SerializeField] private AlienDragonTentacle m_Tentacle;

        [Header("Night mode")]
        [SerializeField] private Transform m_LampTransform;
        [SerializeField] private List<Light2D> m_Lights;
        [SerializeField] private GameObject m_NightLamp;

        [Header("Needle references")]
        [SerializeField] private AlienDragonNeedle m_Needle;
        [SerializeField] private Transform m_FirePoint;
        [SerializeField] private Transform m_BoneRotation;

        private MonoBehaviourPool<AlienDragonNeedle> m_NeedlePool;
        private AlienDragonConfig m_Config;
        private AlienDragonEnergy m_Energy;

        private Transform m_Transform;

        private MealInfo m_MealInfo = new();

        private Vector3 m_InitialPosition;
        private Vector2 m_InitialAimPosition;
        private Vector2 m_InitialLampPosition;
        private Vector2 m_MovingDirection;
        private Vector2 m_TempVelocity;

        private TimeOfDay? m_CurrentTimeOfDayMode;

        private float m_ShootCooldown;
        private float m_CurrentSpeed;
        private float m_SlideForce;

        private bool m_IsReady;

        public event Action<MealInfo> HadMeal;
        public event Action<int> TentacleFreePlacesChanged;

        public event Action<float> ChangedEnergy
        {
            add { m_Energy.ChangedEnergy += value; }
            remove { m_Energy.ChangedEnergy -= value; }
        }

        public event Action FinishedEnergy
        {
            add { m_Energy.FinishedEnergy += value; }
            remove { m_Energy.FinishedEnergy -= value; }
        }

        public Vector2 Position => m_Transform.position;

        private bool IsReady
        {
            get => m_IsReady;
            set
            {
                m_IsReady = value;
                m_Tentacle.Enable(m_IsReady);
            }
        }

        private void OnValidate() => m_Rigidbody2D ??= GetComponent<Rigidbody2D>();

        private void FixedUpdate()
        {
            if (!m_PauseManager.IsPaused)
            {
                UpdateMovement();
            }
        }

        public void Initialize()
        {
            m_Transform = transform;
            m_InitialPosition = m_Transform.position;
            m_InitialAimPosition = m_AimTransform.position;
            m_InitialLampPosition = m_LampTransform.position;
            m_PauseManager.Register(this);
            m_Energy = new(m_GlobalMonoBehaviour.GetContext());
        }

        public void Enable()
        {
            if (!gameObject.activeInHierarchy)
            {
                m_Config = m_Model.Config;
                gameObject.SetActive(true);

                m_NeedlePool ??= new MonoBehaviourPool<AlienDragonNeedle>(m_Needle, m_GameObjectContainers.SpawnParent, count: 5, autoExpand: true,
                                                                          m_DiContainer.InstantiatePrefabForComponent<AlienDragonNeedle>);
                m_Animator.Play(AlienDragonAnimation.ResetToDefault);

                m_Tentacle.ResetTentacle();
                m_Tentacle.Changed += OnCatch;
                TentacleFreePlacesChanged?.Invoke(m_Tentacle.FreePlaces);

                m_Energy.Init(m_Config.MaxEnergyPoints, m_Config.EnergyWastingPerSecond);
                m_Energy.StartWaste();
                m_Energy.FinishedEnergy += SetNoEnergy;

                SetSpeedToDefault();
                SetReady();
            }
        }

        public void Disable()
        {
            if (gameObject.activeInHierarchy)
            {
                StopBehaviour();
                gameObject.SetActive(false);
                transform.position = m_InitialPosition;
            }
        }

        public void SetTimeOfDayMode(TimeOfDay timeOfDay)
        {
            if (m_CurrentTimeOfDayMode == null || m_CurrentTimeOfDayMode != timeOfDay)
            {
                m_CurrentTimeOfDayMode = timeOfDay;
                bool isNight = m_CurrentTimeOfDayMode == TimeOfDay.Night;
                m_NightLamp.SetActive(isNight);
                m_Lights.ForEach(l => l.gameObject.SetActive(isNight));
            }
        }

        public void Shoot()
        {
            if (Time.time > m_ShootCooldown)
            {
                m_ShootCooldown = Time.time + m_Config.NeedleCooldown;
                var needle = m_NeedlePool.GetFreeElement();
                m_FirePoint.LookAt(m_AimTransform.position);
                needle.Shoot(m_FirePoint.position, m_FirePoint.forward, m_BoneRotation.rotation,
                             m_Config.NeedleForce, m_Model.IsNeedleTriggeredMultipleTimes);
                m_AssetAudioService.PlaySound(Assets.AudioAssetType.Shoot);

            }
        }

        public void Eat()
        {
            if (IsReady)
            {
                IsReady = false;
                ResetVelocity();

                if (m_Tentacle.Points != default)
                {
                    m_Animator.Play(AlienDragonAnimation.Eat);
                    InvokeActionAfterDelay(GetPercentageOfCooldown(35), EatFromTentacle);
                }
                else
                {
                    m_Animator.Play(AlienDragonAnimation.NoFood);
                }

                InvokeActionAfterDelay(m_Config.EatCooldown, () =>
                {
                    SetSpeedToDefault();
                    SetReady();
                    TentacleFreePlacesChanged?.Invoke(m_Tentacle.FreePlaces);
                });
            }
        }

        public Vector2 AddSlideForce(float force)
        {
            m_SlideForce += force;
            return m_MovingDirection;
        }

        public void SetAimPosition(Vector2 worldPosition)
        {
            m_AimTransform.position = worldPosition;

            if (m_CurrentTimeOfDayMode == TimeOfDay.Night)
            {
                m_LampTransform.position = worldPosition;
            }
        }

        public void SetPaused(bool isPaused)
        {
            if (isPaused)
            {
                m_TempVelocity = m_Rigidbody2D.velocity;
                ResetVelocity();
                m_Animator.Speed = 0f;
                m_Energy.StopWaste();
            }
            else
            {
                m_Animator.Play(AlienDragonAnimation.ResetToDefault);
                m_Animator.Speed = 1f;
                m_Rigidbody2D.velocity = m_TempVelocity;
                m_Energy.StartWaste();
            }
        }

        public void SetMovementDirection(Vector2 direction) => m_MovingDirection = direction;

        private void SetNoEnergy()
        {
            StopBehaviour();
            m_Animator.Play(AlienDragonAnimation.NoEnergy);
        }

        private void StopBehaviour()
        {
            IsReady = false;
            StopAllCoroutines();

            m_Tentacle.Changed -= OnCatch;
            m_Energy.FinishedEnergy -= SetNoEnergy;
            m_Energy.StopWaste();

            m_AimTransform.position = m_InitialAimPosition;
            m_LampTransform.position = m_InitialLampPosition;

            SetSpeedToDefault();
        }

        private void OnCatch()
        {
            IsReady = false;
            m_Animator.Play(AlienDragonAnimation.Catch);
            m_AssetAudioService.PlaySound(Assets.AudioAssetType.Catch);
            ResetVelocity();
            InvokeActionAfterDelay(m_Config.CatchCooldown, () =>
            {
                SetReady();
            });
            SetDecreasedSpeed(m_Tentacle.Mass);
            TentacleFreePlacesChanged?.Invoke(m_Tentacle.FreePlaces);
        }

        private void SetReady()
        {
            m_Animator.Play(AlienDragonAnimation.Idle);
            IsReady = true;
        }

        private void SetDecreasedSpeed(float mass)
        {
            var debuffSpeed = m_Config.DefaultSpeed - mass * m_Config.OneMassDebuff;
            m_CurrentSpeed = debuffSpeed <= 0 ? 0 : debuffSpeed;
        }

        private void EatFromTentacle()
        {
            m_AssetAudioService.PlaySound(Assets.AudioAssetType.Eat);
            m_MealInfo.CupcakeCount = m_Tentacle.CupcakeCount;
            m_MealInfo.RottenFoodCount = m_Tentacle.RottenFoodCount;
            m_MealInfo.Energy = m_Tentacle.Points;
            m_Energy.AddEnergy(m_Tentacle.Points);
            HadMeal?.Invoke(m_MealInfo);
            m_Tentacle.ResetTentacle();
        }

        private IEnumerator InvokeActionAfterDelayCoroutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private void UpdateMovement()
        {
            if (IsReady)
            {
                if (m_SlideForce > 0)
                {
                    m_Rigidbody2D.AddForce(m_MovingDirection * m_SlideForce);
                    m_SlideForce -= 19f / 20f * m_SlideForce;

                    if (IsLeftDirection())
                    {
                        m_Animator.Play(AlienDragonAnimation.SlidingToLeft);
                    }
                    else if (IsRightDirection())
                    {
                        m_Animator.Play(AlienDragonAnimation.SlidingToRight);
                    }
                }
                else
                {
                    m_Rigidbody2D.velocity = m_MovingDirection * m_CurrentSpeed * Time.fixedDeltaTime;

                    if (IsLeftDirection())
                    {
                        m_Animator.Play(AlienDragonAnimation.WalkToLeft);
                    }
                    else if (IsRightDirection())
                    {
                        m_Animator.Play(AlienDragonAnimation.WalkToRight);
                    }
                    else
                    {
                        m_Animator.Play(AlienDragonAnimation.Idle);
                    }
                }
            }
            else
            {
                m_Rigidbody2D.velocity = Vector2.zero;
            }
        }

        private bool IsRightDirection()
        {
            return m_MovingDirection.x > 0 || m_MovingDirection.y > 0;
        }

        private bool IsLeftDirection()
        {
            return m_MovingDirection.x < 0 || m_MovingDirection.y < 0;
        }

        /// <summary>
        /// Called from animator
        /// </summary>
        private void PlayStepSound() => m_AssetAudioService.PlaySound(Assets.AudioAssetType.Step);

        private void InvokeActionAfterDelay(float delay, Action action) => StartCoroutine(InvokeActionAfterDelayCoroutine(delay, action));
        private void SetSpeedToDefault() => m_CurrentSpeed = m_Config.DefaultSpeed;
        private void ResetVelocity() => m_Rigidbody2D.velocity = Vector3.zero;
        private float GetPercentageOfCooldown(float percentage) => (percentage / 100) * m_Config.EatCooldown;
    }
}