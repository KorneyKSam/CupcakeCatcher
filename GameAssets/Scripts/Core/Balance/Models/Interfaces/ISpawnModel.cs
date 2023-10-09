using UnityEngine;

namespace Core.Balance.Models
{
    public interface ISpawnModel
    {
        float Interval { get; }
        Vector2 Position { get; }
    }
}