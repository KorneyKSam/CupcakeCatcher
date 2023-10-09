using UnityEngine;

namespace Core.Statistics
{
    [SerializeField]
    public class UITableResultInfo
    {
        public int Number { get; set; }
        public string ProfileName { get; set; }
        public int CupcakeCount { get; set; }
        public float TotalTimeSeconds { get; set; }
        public bool IsCurrentProfile { get; set; }
        public bool IsLastGame { get; set; }
    }
}