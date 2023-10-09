using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BalanceSystem
{
    public static class BalanceExtensions
    {
        public static T GetRandomValue<T>(this List<T> list) => list.Count > 1 ? list[BalanceRandom.Range(0, list.Count - 1)] : list[0];

        public static KeyValuePair<T, T> GetRandomPair<T>(this Dictionary<T, T> dict) => dict.ElementAt(BalanceRandom.Range(0, dict.Count - 1));
    }
}