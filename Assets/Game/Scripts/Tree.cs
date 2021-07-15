using System;
using UnityEngine;

namespace WGame
{
    public class Tree : MonoBehaviour, IAttackable
    {
        [SerializeField] [Range(0, 1000)] private int _health = 100;

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
            Recycle();
        }

        public void Recycle()
        {
            Destroy(gameObject);
        }
    }
}
