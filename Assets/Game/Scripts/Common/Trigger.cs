using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class Trigger<T> where T : MonoBehaviour
    {
        private LayerMask _mask;
        private Action<T> _playerEnter;
        private Action<T> _playerExit;

        private IEnumerable<T> _enemies = new HashSet<T>();

        public Trigger( LayerMask mask, Action<T> playerEnter, Action<T> playerExit)
        {
            _mask = mask;
            _playerEnter = playerEnter;
            _playerExit = playerExit;
        }

        public void Check(Vector3 position, float radius)
        {
            var enemiesInArea = CollisionUtilities.GetComponentsViaOverlapSphere<T>(position, radius, _mask);

            var entered = enemiesInArea.Where(x => _enemies.Contains(x) == false);
            var exited = _enemies.Where(x => enemiesInArea.Contains(x) == false);

            foreach (var x in exited)
                _playerExit?.Invoke(x);

            foreach (var x in entered)
                _playerEnter?.Invoke(x);

            _enemies = enemiesInArea;
        }

        public bool CheckIfAnyInside(Vector3 position, float radius) 
            => CollisionUtilities.GetComponentsViaOverlapSphere<T>(position, radius, _mask).FirstOrDefault() != null;
    }
}
