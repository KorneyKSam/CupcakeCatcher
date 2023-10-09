using System.Collections.Generic;

namespace BalanceSystem
{
    public static class ConfigValidator
    {
        public const string NotValidPercentage = "Config is not valid, total percentage must equal 100%";
        public const string NotValidRange = "Config is not valid, Min value can't be greater than Max";

        public static bool IsValidConfigRange(ConfigRangeValue configRangeValue) => configRangeValue.Min <= configRangeValue.Max;

        public static bool IsValidBalance<T>(List<ConfigPercentageValue<T>> values)
        {
            if (values == null)
            {
                return false;
            }

            float totalPercentage = BalanceConstants.MinPercentage;

            values.ForEach(v => totalPercentage += v.Percentage);

            return totalPercentage == BalanceConstants.MaxPercentage;
        }
    }
}