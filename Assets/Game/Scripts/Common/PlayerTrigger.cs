using System;
using UnityEngine;

namespace WGame
{
    public class PlayerTrigger
    {
        private Action<Player> _playerEnter;
        private Action<Player> _playerExit;

        private Player _player;

        public PlayerTrigger( Action<Player> playerEnter, Action<Player> playerExit)
        {
            _playerEnter = playerEnter;
            _playerExit = playerExit;
        }

        public void Check(Vector3 position, float radius)
        {
            Player newPlayer = CollisionUtilities.GetPlayerViaOverlapSphere(position, radius);

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
