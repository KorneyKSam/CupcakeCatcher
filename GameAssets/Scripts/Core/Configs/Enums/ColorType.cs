using UnityEngine;

namespace Core.Configs
{
    public enum ColorType
    {
        [InspectorName("Specific color")]
        SpecificColor = 0,
        [InspectorName("Random color")]
        RandomColor = 1,
    }
}