using System;

namespace Common
{
    public static class GameExtensions
    {
        private const string TimerFormat = @"mm\:ss\.ff";

        public static string ConvertToTimeFormat(this float time) => TimeSpan.FromSeconds(time).ToString(TimerFormat);
    }
}