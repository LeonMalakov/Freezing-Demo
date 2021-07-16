using System;
using UnityEngine;

namespace WGame
{
    public class Campfire : GameBehaviour, IInteractivable
    {
        [SerializeField] [Range(0, 120)] private float _startLifeTime = 60;

        private float _lifeTime;

        public Vector3 Position => transform.position;

        public event Action Died;

        private void Start()
        {
            _lifeTime = _startLifeTime;
        }

        private void Update()
        {
            DecreaseLifeTime();
            CheckDie();
        }

        public bool Interact(Player character) => false;

        public bool InteractWithItem(Player character, Item item)
        {
            AddItem(item);
            return true;
        }

        public void AddItem(Item item)
        {
            _lifeTime += item.LifeTimeToAdd;
            item.Recycle();

            Debug.Log($"LifeTime = {_lifeTime}");
        }

        private void DecreaseLifeTime()
        {
            _lifeTime -= Time.deltaTime;
        }

        private void CheckDie()
        {
            if (_lifeTime <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log("Fire died.");
            Died?.Invoke();
        }

        public void BecomeActive()
        {
        }

        public void BecomeInactive()
        {
        }
    }
}