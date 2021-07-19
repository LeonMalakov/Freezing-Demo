using System.Collections;
using UnityEngine;

namespace WGame
{
    public class Tree : GameBehaviour, IAttackable
    {
        private const float DroppedItemsHeight = 1f;
        private const float DroppedItemsNoise = 0.8f;
        [SerializeField] private TreeView _view;
        [SerializeField] [Range(0, 1000)] private int _maxHealth = 100;

        [SerializeField] [Range(1, 10)] private int _dropItemsCount = 3;
        [SerializeField] [Range(0, 500)] private float _recoverTime = 180;

        private int _health;

        public bool IsAlive => _health > 0;

        public void Init()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (IsAlive)
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
            if (IsAlive == false)
                Die();
        }

        private void Die()
        {           
            _view.Die();
            SpawnItems();

            StartCoroutine(RecoverLoop());
        }

        private void Recover()
        {
            _view.Recover();
            _health = _maxHealth;
        }

        private void SpawnItems()
        {
            float piFactor = Mathf.PI / 3 * RandomSign();

            for (int i = 0; i < _dropItemsCount; i++)
            {
                var displacement =
                    new Vector3(Mathf.Sin(i * piFactor), 0, Mathf.Cos(i * piFactor)) * (i / 4 + 1)
                    + Vector3.up * DroppedItemsHeight
                    + Noise();

                var position = transform.position + transform.TransformDirection(displacement);
                Quaternion rotation = Quaternion.LookRotation(transform.position - position, transform.up);
                Game.CreateItem(position, rotation);
            }

            static Vector3 Noise() => new Vector3(Random.Range(-DroppedItemsNoise, DroppedItemsNoise), 0, Random.Range(-DroppedItemsNoise, DroppedItemsNoise));

            static int RandomSign() => (Random.Range(0, 2) == 0 ? 1 : -1);
        }

        private IEnumerator RecoverLoop()
        {
            yield return new WaitForSeconds(_recoverTime);
            Recover();
        }
    }
}
