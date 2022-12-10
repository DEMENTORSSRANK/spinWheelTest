using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public class Bet
    {
        public Bet(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            Value = value;
        }

        public int Value { get; private set; }

        public event Action<int> ValueChanged;

        public int CalculateWin(IEnumerable<float> coefficients) => coefficients.Sum(coefficient => Mathf.RoundToInt(Value * coefficient));

        /// <summary>
        /// Trying to take value
        /// </summary>
        /// <param name="value">What value trying to take</param>
        /// <returns>Returns true on success, if false - not enough bet value</returns>
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
        /// Adding bet
        /// </summary>
        /// <param name="value">Value that adding</param>
        /// <exception cref="ArgumentOutOfRangeException">Adding value should be positive</exception>
        public void Add(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            Value += value;
            
            ValueChanged?.Invoke(Value);
        }

        public void Set(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            Value = value;

            ValueChanged?.Invoke(value);
        }

        /// <summary>
        /// Sets bet to zero
        /// </summary>
        public void Reset()
        {
            Value = 0;
            
            ValueChanged?.Invoke(Value);
        }
    }
}