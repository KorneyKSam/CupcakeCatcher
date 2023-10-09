using Balloons;

namespace BalanceSystem
{
    public class BalanceRange
    {
        public float StartValue { get; private set; }
        public float EndValue { get; private set; }

        public BalanceRange(float startValue, float endValue) => Set(startValue, endValue);
        public BalanceRange() { }

        public virtual void Set(float starValue, float endValue)
        {
            if (starValue > endValue)
            {
                throw new System.Exception("Start value greater than end value in BalanceRange!");
            }

            StartValue = starValue;
            EndValue = endValue;
        }

        public void Set(ConfigRangeValue configRangeValue) => Set(configRangeValue.Min, configRangeValue.Max);
        public bool Contains(float value) => StartValue <= value && value <= EndValue;
        public float GetRandom() => BalanceRandom.Range(StartValue, EndValue);
    }
}