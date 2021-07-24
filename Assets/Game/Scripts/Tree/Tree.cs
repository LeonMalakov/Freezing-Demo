using System.Collections;
using UnityEngine;

namespace WGame
{
    public class Tree : GameBehaviour, IAttackable
    {
        private const float DroppedItemsHeight = 1f;
        private const float DroppedItemsNoise = 0.25f;
        private const float DroppedItemsStep = 0.8f;
        [SerializeField] private TreeView _view;
        [SerializeField] [Range(0, 1000)] private int _maxHealth = 100;

        [SerializeField] [Range(1, 10)] private int _dropItemsCount = 3;
        [SerializeField] [Range(0, 500)] private float _recoverTime = 180;

        private int _health;

        Transform IGameObject.Transform => transform;
        bool IAttackable.IsAlive => _health > 0;
        bool IAttackable.IsPriority => false;

        public void Init()
        {
            _health = _maxHealth;
        }

        void IAttackable.TakeDamage(int damage)
        {
            if (((IAttackable)this).IsAlive)
            {
                _health -= damage;
                CheckDie();
            }
        }

        public void Recycle()
        {
            Game.RemoveTree(this);
        }

        private void CheckDie()
        {
            if (((IAttackable)this).IsAlive == false)
                Die();
        }

        private void Die()
        {           
            _view.Die(callback: () => SpawnItems());
            StartCoroutine(RecoverLoop());
        }

        private void Recover()
        {
            _view.Recover(callback: () => _health = _maxHealth);
        }

        private void SpawnItems()
        {
            for (int i = 0; i < _dropItemsCount; i++)
            {
                var displacement =
                    Vector3.forward * DroppedItemsStep * (i + 1)
                    + Noise();

                var position = transform.position + transform.TransformDirection(displacement);
                Quaternion rotation = Quaternion.LookRotation(transform.position - position, transform.up);
                Game.CreateItem(position + transform.TransformDirection(Vector3.up * DroppedItemsHeight), rotation);
            }

            static Vector3 Noise() => new Vector3(Random.Range(-DroppedItemsNoise, DroppedItemsNoise), 0, Random.Range(-DroppedItemsNoise, DroppedItemsNoise));
        }

        private IEnumerator RecoverLoop()
        {
            yield return new WaitForSeconds(_recoverTime);
            Recover();
        }
    }
}
