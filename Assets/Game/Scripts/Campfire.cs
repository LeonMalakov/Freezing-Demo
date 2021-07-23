using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    public class Campfire : GameBehaviour, IInteractivable
    {
        private const float UpdateLifetimeInterval = 1f;
        private const int LifetimeToRemove = 1;
        [SerializeField] [Range(0, 300)] private int _maxLifeTime = 120;
        [SerializeField] [Range(0, 300)] private int _startLifeTime = 45;
        [SerializeField] [Range(0, 100)] private float _warmRadius = 10;

        private Stat _lifeTime;
        private Player _player;
        private PlayerTrigger _playerTrigger;

        public bool IsAlive => _lifeTime > 0;
        public Transform Transform => transform;
        public int MaxLifeTime => _maxLifeTime;
        public int LifeTime => _lifeTime;

        public event Action<int> LifetimeChanged;
        public event Action Died;

        public void Init()
        {
            _lifeTime = new Stat(_maxLifeTime, OnLifetimeChanged);
            _lifeTime.Set(_startLifeTime);

            Died += () =>
            {
                if (_player != null)
                    _player.ExitWarmArea();
            };

            _playerTrigger = new PlayerTrigger(
                (x) =>
                {
                    _player = x;
                    x.EnterWarmArea();
                },
                (x) => x.ExitWarmArea());

            StartCoroutine(UpdateLifetimeLoop());
        }

        private void FixedUpdate()
        {
            if (IsAlive)
                _playerTrigger.Check(transform.position, _warmRadius);
        }

        public bool Interact(Player character) => false;

        public bool InteractWithItem(Player character, Item item)
        {
            if (IsAlive == false) return false;

            if (_lifeTime >= _maxLifeTime - item.LifeTimeToAdd) return false;

            AddItem(item);
            return true;
        }

        public void Recycle()
        {
            Game.RemoveCampfire(this);
        }

        private void AddItem(Item item)
        {
            _lifeTime += item.LifeTimeToAdd;
            item.Recycle();
        }

        private void Die()
        {
            Died?.Invoke();
        }

        private void OnLifetimeChanged(int value)
        {
            LifetimeChanged?.Invoke(value);

            if (value <= 0)
                Die();
        }

        private IEnumerator UpdateLifetimeLoop()
        {
            var waitForSeconds = new WaitForSeconds(UpdateLifetimeInterval);

            while (IsAlive)
            {
                if (_lifeTime > 0)
                    _lifeTime -= LifetimeToRemove;

                yield return waitForSeconds;
            }
        }

        public void BecomeActive()
        {
        }

        public void BecomeInactive()
        {
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _warmRadius);
        }
    }
}