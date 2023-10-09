using System.Collections.Generic;

namespace BalanceSystem
{
    public class RandomValueGetter<T>
    {
        private readonly List<T> m_Values = new();

        public RandomValueGetter() { }
        public RandomValueGetter(List<T> values) => m_Values = values;
        public RandomValueGetter(params T[] values) => AddValues(values);

        public RandomValueGetter<T> AddValues(params T[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                AddValue(values[i]);
            }
            return this;
        }

        public RandomValueGetter<T> AddValue(T returnedValue)
        {
            if (!m_Values.Contains(returnedValue))
            {
                m_Values.Add(returnedValue);
            }
            return this;
        }

        public T GetRandomValue() => m_Values.GetRandomValue();
        public void RemoveValue(T returnedValue) => m_Values.Remove(returnedValue);
    }
}