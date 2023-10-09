using Assets;
using Audio;
using BalanceSystem;
using Core;
using Core.Balance.Models;
using Core.Configs;
using EventManager;
using Pause;
using UnityEngine;
using Zenject;

namespace Balloons
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BalloonBehaviour : MonoBehaviour, IDestroyable, IDamagable, IExposedToTheWind, IPauseHandler
    {
        [Inject] private readonly IBalloonModel m_Model;
        [Inject] private readonly AssetAudioService m_AssetAudioService;
        [Inject] private readonly PauseManager m_PauseManager;
        [Inject] private readonly AssetLoader m_AssetLoader;

        [Header("References")]
        [SerializeField] private BalloonVisualisation m_Visualisation;
        [SerializeField] private SpriteRenderer m_ContentSpriteRnd;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        [SerializeField] private BoxCollider2D m_BoxCollider2D;

        private BalloonContentConfig m_ContentConfig;
        private ContentSpritesConfig m_ContentSpritesConfig;
        private Vector2 m_Direction = Vector2.up;
        private Vector2 m_WindForce;
        private bool m_IsDamaged;
        private float m_Speed;
        private Transform m_ContentRndTransform;

        private void OnValidate()
        {
            m_BoxCollider2D ??= GetComponent<BoxCollider2D>();
            m_Rigidbody2D ??= GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            m_ContentRndTransform = m_ContentSpriteRnd.transform;
            m_PauseManager.Register(this);
        }

        private void FixedUpdate()
        {
            if (m_PauseManager.IsPaused)
            {
                return;
            }

            if (m_WindForce != Vector2.zero)
            {
                m_Rigidbody2D.AddForce(m_Direction + m_WindForce);
            }
            else
            {
                m_Rigidbody2D.velocity = m_Direction * m_Speed * Time.fixedDeltaTime;
            }
        }

        public void Initialize()
        {
            m_Speed = m_Model.Speed;
            var visualConfig = m_Model.VisualConfig;
            m_BoxCollider2D.size = visualConfig.BoxColiderSize;
            m_BoxCollider2D.offset = visualConfig.BoxColliderOffset;
            m_ContentRndTransform.localPosition = visualConfig.ContainerPosition;
            m_Visualisation.SetSprite(m_AssetLoader.LoadBalloonSprite(visualConfig.SpriteName));
            m_Visualisation.SetColor(visualConfig.ColorType == ColorType.RandomColor ? Random.ColorHSV()
                                   : visualConfig.SpecificColor);
            m_Visualisation.Play(BalloonAnimation.Idle);
            m_Direction = Vector2.up;

            m_ContentConfig = null;

            if (visualConfig.CanBeEmpty && m_Model.UseEmptyBalloon)
            {
                return;
            }

            m_ContentConfig = m_Model.BalloonContentConfig;
            m_ContentSpritesConfig = m_ContentConfig.ContentSprites.GetRandomValue();
            m_ContentSpriteRnd.sprite = m_AssetLoader.LoadContentSprite(m_ContentSpritesConfig.MainSpriteName);
        }

        public void Destroy()
        {
            if (!m_IsDamaged && m_ContentConfig.Energy > 0)
            {
                EventHolder.CupcakeMissed.Invoke();
            }

            StopFly();
            DisableBalloon();
        }

        public void Damage()
        {
            if (!m_IsDamaged)
            {
                m_IsDamaged = true;
                StopFly();
                SpawnContent();
                m_Visualisation.Play(BalloonAnimation.Blast, DisableBalloon);
                m_AssetAudioService.PlaySound(AudioAssetType.BalloonBlast);
            }
        }

        public void SetPaused(bool isPaused)
        {
            m_Direction = isPaused ? Vector2.zero : Vector2.up;
            m_Rigidbody2D.velocity = isPaused ? Vector2.zero : m_Rigidbody2D.velocity;
            m_Visualisation.Speed = isPaused ? 0f : 1f;
        }

        public void Exposure(Vector2 force) => m_WindForce = force;

        private void StopFly()
        {
            m_Direction = Vector2.zero;
            m_Rigidbody2D.velocity = Vector2.zero;
        }

        private void DisableBalloon()
        {
            m_IsDamaged = false;
            gameObject.SetActive(false);
        }

        public void SpawnContent()
        {
            if (m_ContentConfig != null)
            {
                var content = GlobalPools.GetBalloonContent();
                content.Activate(GetContentModel(), m_ContentRndTransform.position);
            }
        }

        private ContentModel GetContentModel()
        {
            return new ContentModel()
            {
                MainSprite = m_ContentSpriteRnd.sprite,
                DestroySprite = m_AssetLoader.LoadContentSprite(m_ContentSpritesConfig.DestroySpriteName),
                ContentType = m_ContentConfig.ContentType,
                FoodEffect = m_ContentConfig.FoodEffect,
                GrenadeEffect = m_ContentConfig.GrenadeEffect,
                Mass = m_ContentConfig.Mass,
                DissapearanceDelay = m_ContentConfig.DissapearanceDelay,
                HungerPoints = m_ContentConfig.Energy,
                EnableSliding = m_Model.EnableFoodSliding,
            };
        }
    }
}