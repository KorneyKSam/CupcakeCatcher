using System.Collections.Generic;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = nameof(BalloonType), menuName = "Core/Balloons/Balloon type", order = 2)]
    public class BalloonType : ScriptableObject
    {
        [field: SerializeField] public List<BalloonVisualConfig> VisualTypes { get; private set; }
    }
}