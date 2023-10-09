using System;
using UnityEngine;

namespace Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedSprite : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;

        private Action m_CompletedAnimaitonCallback;

        public event Action CompletedAnimation;
        public float Speed { get => m_Animator.speed; set => m_Animator.speed = value; }
        public bool IsPlaying => m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        public string Clip => m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        public void Play(string animationName, Action onCompleteAnimation = null)
        {
            m_CompletedAnimaitonCallback = onCompleteAnimation;
            m_Animator?.Play(animationName);
        }

        /// <summary>
        /// Animator Event
        /// </summary>
        private void InvokeOnAnimationComplete()
        {
            CompletedAnimation?.Invoke();
            InvokeAndForgetCallback();
        }

        private void InvokeAndForgetCallback()
        {
            m_CompletedAnimaitonCallback?.Invoke();
            m_CompletedAnimaitonCallback = null;
        }

        private void OnValidate()
        {
            m_Animator ??= GetComponent<Animator>();
        }
    }
}