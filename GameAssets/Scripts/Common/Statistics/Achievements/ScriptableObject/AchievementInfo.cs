using UnityEngine;

namespace Common.Statistic
{
    [CreateAssetMenu(fileName = nameof(AchievementInfo), menuName = "Core/AchievementInfo", order = 7)]
    public class AchievementInfo : ScriptableObject
    {
        [field: SerializeField] public Sprite ActiveIcon { get; private set; }
        [field: SerializeField] public Sprite InactiveIcon { get; private set; }
        [field: SerializeField] public string TitleKey { get; private set; }
        [field: SerializeField] public string DescriptionKey { get; private set; }
    }
}