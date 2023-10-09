using System;
using UnityEngine;

namespace Core.Configs
{
    [Serializable]
    public class ContentSpritesConfig
    {
        [SerializeField] private string m_MainSpriteName;
        [SerializeField] private string m_DestroySpriteName;

        public string MainSpriteName => m_MainSpriteName;
        public string DestroySpriteName => m_DestroySpriteName;
    }
}