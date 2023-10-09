using Animation;
using Assets;
using Audio;
using Core;
using Core.Balance.Models;
using Core.Features;
using EventManager;
using Pause;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Balloons.Content
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class BalloonContent : MonoBehaviour, IDestroyable, ISurfaceInteractable, IPauseHandler
    {
        [Inject] private readonly AssetLoader m_AssetLoader;
        [Inject] private readonly AssetAudioService m_AssetAudioService;
        [Inject] private readonly PauseManager m_PauseManager;

        [Header("References")]
        [SerializeField] private AnimatedSprite m_AnimatedSprite;
        [SerializeField] private SpriteRenderer m_MainSpriteRnd;
        [SerializeField] private SpriteRenderer m_DestroySpriteRnd;
        [SerializeField] private AudioSource m_AudioSource;

        [Header("Effects")]
        [SerializeField] private SplatSlide m_SplatSlide;
        [SerializeField] private SmokeGrenade m_SmokeGrenade;
        [SerializeField] private GameObject m_PoisonEffect;

        private IContentModel m_Model;
        private Transform m_Transform;
        private Rigidbody2D m_Rigidbody2D;
        private BoxCollider2D m_BoxCollider2D;
        private Vector2 m_TempVelocity;
        private bool m_Interacted;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
            m_Transform = m_Rigidbody2D.transform;
            m_PauseManager.Register(this);
        }

        public void Activate(IContentModel contentModel, Vector2 worldPosition)
        {
            m_Transform.position = worldPosition;
            TryToResetInteraction();
            SetSettings(contentModel);
            ResetRotation();
            ActivatePhysics();
            AddLisnteners();
        }

        public void Destroy()
        {
            TryToResetInteraction();
            StopAllCoroutines();
            RemoveLisnteners();
            gameObject.SetActive(false);
        }

        public void Interact()
        {
            if (!m_Interacted)
            {
                m_Interacted = true;
                ResetRotation();
                DeactivatePhysics();
                m_AnimatedSprite.Play(ContentAnimaitons.Destroy, onCompleteAnimation: () => StartCoroutine(ShowDisappearAnimationWithDelay()));

                if (m_Model.ContentType == ContentType.Food)
                {
                    m_AssetAudioService.PlaySound(AudioAssetType.FallenCupcake, true);
                    m_SplatSlide.gameObject.SetActive(m_Model.EnableSliding);
                }
                else if (m_Model.ContentType == ContentType.Grenade && m_Model.GrenadeEffect == GrenadeEffect.Smoke)
                {
                    m_AssetAudioService.PlaySound(AudioAssetType.GrenadeFall, true);
                    ShowSmokeEffect();
                }

                if (m_Model.HungerPoints > 0)
                {
                    EventHolder.CupcakeFlattened.Invoke();
                }
            }
        }

        public void SetPaused(bool isPaused)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            if (isPaused)
            {

                if (!m_Interacted)
                {
                    m_TempVelocity = m_Rigidbody2D.velocity;
                    DeactivatePhysics();
                }
                else if (m_Model.ContentType == ContentType.Grenade)
                {
                    m_SmokeGrenade.Pause();
                    m_AudioSource.Stop();
                }
            }
            else
            {
                if (!m_Interacted)
                {
                    m_Rigidbody2D.velocity = m_TempVelocity;
                    ActivatePhysics();
                }
                else if (m_Model.ContentType == ContentType.Grenade)
                {
                    m_SmokeGrenade.Play();
                    m_AudioSource.Play();
                }
            }
        }

        public CatchedObjectInfo GetInfo()
        {
            return new()
            {
                HungerPoints = m_Model.HungerPoints,
                Mass = m_Model.Mass,
                Sprite = m_MainSpriteRnd.sprite,
            };
        }

        private void AddLisnteners()
        {
            RemoveLisnteners();
            m_SplatSlide.Destryed += Destroy;
            m_SmokeGrenade.Destryed += Destroy;
        }

        private void RemoveLisnteners()
        {
            m_SplatSlide.Destryed -= Destroy;
            m_SmokeGrenade.Destryed -= Destroy;
        }

        private void SetSettings(IContentModel contentModel)
        {
            m_Model = contentModel;
            m_Rigidbody2D.mass = m_Model.Mass;
            m_MainSpriteRnd.sprite = m_Model.MainSprite;
            m_DestroySpriteRnd.sprite = m_Model.DestroySprite;
            gameObject.layer = LayerMask.NameToLayer(m_Model.ContentType == ContentType.Food ? LayerMasks.Catchable : LayerMasks.NotCatchable);
            m_PoisonEffect.SetActive(m_Model.ContentType == ContentType.Food && m_Model.FoodEffect == FoodEffect.Poison);
        }

        private void ShowSmokeEffect()
        {
            m_SmokeGrenade.gameObject.SetActive(true);
            m_AudioSource.loop = true;
            m_AudioSource.clip = m_AssetLoader.LoadAudioClip(AudioAssetType.SmokeLoop);
            m_AudioSource.Play();
        }

        private void TryToResetInteraction()
        {
            if (m_Interacted)
            {
                m_SplatSlide.Reset();
                m_SplatSlide.gameObject.SetActive(false);
                m_SmokeGrenade.gameObject.SetActive(false);
                m_AnimatedSprite.Play(ContentAnimaitons.Idle);
                m_AudioSource.Stop();
                m_Interacted = false;
            }
        }

        private void ActivatePhysics()
        {
            m_Rigidbody2D.freezeRotation = false;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_BoxCollider2D.enabled = true;
        }

        private void DeactivatePhysics()
        {
            m_Rigidbody2D.freezeRotation = true;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            m_BoxCollider2D.enabled = false;
            m_Rigidbody2D.velocity = Vector3.zero;
        }

        private IEnumerator ShowDisappearAnimationWithDelay()
        {
            yield return new WaitForSeconds(m_Model.DissapearanceDelay);
            m_AnimatedSprite.Play(ContentAnimaitons.Disappearance, Destroy);
        }

        private void ResetRotation() => m_Transform.rotation = Quaternion.identity;
    }
}