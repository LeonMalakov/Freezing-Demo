using System;
using UnityEngine;

namespace WGame
{
    public class Stat
    {
        private readonly int _maxValue;
        private int _value;
        private Action<int> _changed;

        public Stat(int maxValue, Action<int> changed)
        {
            _maxValue = maxValue;
            _changed = changed;

            _value = _maxValue;
        }

        public void Set(int newValue)
        {
            int valueToSet = Mathf.Clamp(newValue, 0, _maxValue);

            if (valueToSet != _value)
            {
                _value = valueToSet;
                _changed?.Invoke(_value);
            }
        }

        public static implicit operator int(Stat stat) => stat._value;

        public static Stat operator +(Stat stat, int value)
        {
            stat.Set(stat._value + value);
            return stat;
        }

        public static Stat operator -(Stat stat, int value)
        {
            stat.Set(stat._value - value);
            return stat;
        }
    }
}
