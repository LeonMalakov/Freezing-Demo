using System;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class Campfire : GameBehaviour, IInteractivable
    {
        [SerializeField] [Range(0, 120)] private float _startLifeTime = 60;
        [SerializeField] [Range(0, 100)] private float _warmRadius = 10;
        [SerializeField] private LayerMask _playerLayer;

        private float _lifeTime;
        private Player _player;

        public Vector3 Position => transform.position;
        public bool IsAlive => _lifeTime > 0;

        public event Action Died;

        private void Start()
        {
            _lifeTime = _startLifeTime;

            Died += () => 
            {
                if (_player != null)
                    _player.ExitWarmArea();
            };
        }

        private void Update()
        {
            if (IsAlive)
            {
                DecreaseLifeTime();
                CheckDie();
            }
        }

        private void FixedUpdate()
        {
            if(IsAlive)
                CheckPlayerInsideWarmArea();
        }

        private void CheckPlayerInsideWarmArea()
        {
            Player newPlayer = GetPlayer();

            if (_player != newPlayer)
            {
                if (newPlayer == null && _player != null)
                    _player.ExitWarmArea();
                else
                    newPlayer.EnterWarmArea();

                _player = newPlayer;
            }
        }

        private Player GetPlayer()
        {
            var hits = Physics.OverlapSphere(transform.position, _warmRadius, _playerLayer);
            var newPlayer = hits.Select(x => x.GetComponent<Player>()).FirstOrDefault(x => x != null);
            return newPlayer;
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
            if (IsAlive == false)
                Die();
        }

        private void Die()
        {
            Died?.Invoke();
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