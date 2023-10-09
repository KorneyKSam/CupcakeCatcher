using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core
{
    public class TimeOfDayModel
    {
        public SpriteRenderer SkySpriteRnd { get; set; }
        public GameObject Sun { get; set; }
        public GameObject Moon { get; set; }
        public GameObject Stars { get; set; }
        public Volume GlobalVolume { get; set; }
        public Light2D GlobalLight { get; set; }
    }
}