using System;

namespace BalanceSystem
{
    public class PercentageRange : BalanceRange
    {
        public PercentageRange(float startValue, float endValue) => Set(startValue, endValue);
        public PercentageRange() { }

        public override void Set(float startValue, float endValue)
        {
            if (startValue < BalanceConstants.MinPercentage)
            {
                throw new IndexOutOfRangeException($"Start value ({startValue}) out of min value ({BalanceConstants.MinPercentage})!");
            }

            if (endValue > BalanceConstants.MaxPercentage)
            {
                throw new IndexOutOfRangeException($"End value ({endValue}) out of max value ({BalanceConstants.MinPercentage})!");
            }

            base.Set(startValue, endValue);
        }
    }
}