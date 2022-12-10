using System;

namespace Model
{
    public class Balance
    {
        public Balance(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            Value = value;
        }

        public int Value { get; private set; }

        public event Action<int> ValueChanged;

        /// <summary>
        /// Trying to take value
        /// </summary>
        /// <param name="value">What value trying to take</param>
        /// <returns>Returns true on success, if false - not enough balance</returns>
        /// <exception cref="ArgumentOutOfRangeException">Value should be bigger or equals zero</exception>
        public bool TryTake(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (value > Value)
                return false;

            Value -= value;
            
            ValueChanged?.Invoke(Value);
            
            return true;
        }

        /// <summary>
        /// Adding balance
        /// </summary>
        /// <param name="value">What value need to add</param>
        /// <exception cref="ArgumentOutOfRangeException">Adding value should be more or equals zero</exception>
        public void Add(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            Value += value;
            
            ValueChanged?.Invoke(Value);
        }
    }
}