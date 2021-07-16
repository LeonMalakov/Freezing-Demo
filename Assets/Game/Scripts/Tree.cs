﻿using UnityEngine;

namespace WGame
{
    public class Tree : GameBehaviour, IAttackable
    {
        private const float DroppedItemsHeight = 1;

        [SerializeField] [Range(0, 1000)] private int _health = 100;
        [SerializeField] [Range(1, 10)] private int _dropItemsCount = 3;

        public ITreesOwner OriginFactory { get; set; }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            CheckDie();
        }

        private void CheckDie()
        {
            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            SpawnItems();
            Recycle();
        }

        public void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        private void SpawnItems()
        {
            for (int i = 0; i < _dropItemsCount; i++)
            {
                var displacement = 
                    new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)) * (i / 4 + 1)
                    + Vector3.up * DroppedItemsHeight;

                var position = transform.position + transform.TransformDirection(displacement);
                Quaternion rotation = Quaternion.LookRotation(transform.position - position, transform.up);
                Game.CreateItem(position, rotation);
            }
        }
    }
}
