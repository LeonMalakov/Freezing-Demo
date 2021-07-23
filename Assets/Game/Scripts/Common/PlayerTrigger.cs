using System;
using UnityEngine;

namespace WGame
{
    public class PlayerTrigger
    {
        private Transform _transform;
        private float _radius;
        private Action<Player> _playerEnter;
        private Action<Player> _playerExit;

        private Player _player;

        public PlayerTrigger(Transform transform, float radius, Action<Player> playerEnter, Action<Player> playerExit)
        {
            _transform = transform;
            _radius = radius;
            _playerEnter = playerEnter;
            _playerExit = playerExit;
        }

        public void Check()
        {
            Player newPlayer = CollisionUtilities.GetPlayerViaOverlapSphere(_transform.position, _radius);

            if (_player != newPlayer)
            {
                if (newPlayer == null && _player != null)
                    _playerExit?.Invoke(_player);
                else
                    _playerEnter?.Invoke(newPlayer);

                _player = newPlayer;
            }
        }
    }
}
