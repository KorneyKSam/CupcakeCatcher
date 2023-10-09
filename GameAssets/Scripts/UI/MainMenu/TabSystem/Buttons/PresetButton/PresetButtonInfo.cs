using Core;
using System;
using UnityEngine;

namespace UI.TabSystem
{
    [Serializable]
    public class PresetButtonInfo
    {
        public GameMode Mode;
        public PresetType Type;
        public Sprite Sprite;
        public string ReturnedKey;
    }
}