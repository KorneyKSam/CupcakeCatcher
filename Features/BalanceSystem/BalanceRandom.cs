namespace BalanceSystem
{
    public static class BalanceRandom
    {
        private static readonly System.Random m_Random = new();

        public static float Range(float min, float max) => GetRandomByUnityRandom(min, max);
        public static int Range(int min, int max) => GetRandomByUnityRandom(min, max);
        public static float GetRandomPercentage() => Range(BalanceConstants.MinPercentage, BalanceConstants.MaxPercentage);

        private static float GetRandomByUnityRandom(float min, float max) => UnityEngine.Random.Range(min, max);
        private static int GetRandomByUnityRandom(int min, int max) => UnityEngine.Random.Range(min, max + 1);
        private static int GetRandomBySystemRandom(int min, int max) => m_Random.Next(min, max);
    }
}