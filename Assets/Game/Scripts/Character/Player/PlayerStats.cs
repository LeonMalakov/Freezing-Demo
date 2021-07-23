using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    public class PlayerStats : MonoBehaviour
    {
        private const int StatsUpdateTime = 1;

        [SerializeField] [Range(0, 100)] private int _maxHealth = 100;
        [SerializeField] [Range(0, 100)] private int _maxWarm = 100;

        [SerializeField] [Range(0, 50)] private int _healthToAdd = 5;
        [SerializeField] [Range(0, 50)] private int _healthToRemove = 2;
        [SerializeField] [Range(0, 50)] private int _warmToAdd = 10;
        [SerializeField] [Range(0, 50)] private int _warmToRemove = 3;

        private Stat _health;
        private Stat _warm;
        private bool _isInWarmArea;
        private Action<bool> _isInWarmAreaStateChanged;

        public int MaxHealth => _maxHealth;
        public int MaxWarm => _maxWarm;
        public int Health => _health;
        public int Warm => _warm;
        public bool IsInWarmArea => _isInWarmArea;
        public bool IsAlive => _health > 0;

        public void Init(Action<int> healthChanged, Action<int> warmChanged, Action<bool> isInWarmAreaStateChanged)
        {
            _health = new Stat(_maxHealth, healthChanged);
            _warm = new Stat(_maxWarm, warmChanged);
            _isInWarmAreaStateChanged = isInWarmAreaStateChanged;

            StartCoroutine(StatsUpdateLoop());
        }

        public void TakeDamage(int damage) => _health -= damage;

        public void EnterWarmArea()
        {
            _isInWarmArea = true;
            _isInWarmAreaStateChanged?.Invoke(_isInWarmArea);
        }

        public void ExitWarmArea()
        {
            _isInWarmArea = false;
            _isInWarmAreaStateChanged?.Invoke(_isInWarmArea);
        }

        private IEnumerator StatsUpdateLoop()
        {
            var waitForSeconds = new WaitForSeconds(StatsUpdateTime);

            while (IsAlive)
            {
                UpdateWarmStat();
                UpdateHealthStat();
                yield return waitForSeconds;
            }
        }

        private void UpdateWarmStat()
        {
            if (_isInWarmArea)
            {
                if (_warm < _maxWarm)
                    _warm += _warmToAdd;
            }
            else
            {
                if (_warm > 0)
                    _warm -= _warmToRemove;
            }
        }

        private void UpdateHealthStat()
        {
            if (_isInWarmArea)
            {
                if (_health < _maxHealth)
                    _health += _healthToAdd;
            }
            else if (_warm <= 0)
            {
                _health -= _healthToRemove;
            }
        }
    }
}
