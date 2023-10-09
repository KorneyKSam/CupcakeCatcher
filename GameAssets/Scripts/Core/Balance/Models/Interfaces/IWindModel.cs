using UnityEngine;

namespace Core.Balance.Models
{
    public interface IWindModel
    {
        float Interval { get; }
        float Duration { get; }
        Vector2 Direction { get; }
        float Force { get; }
    }
}