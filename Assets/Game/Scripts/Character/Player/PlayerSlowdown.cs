using System;
using UnityEngine;

namespace WGame
{
    public class PlayerSlowdown : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float _radius = 2;

        private Action<bool> _stateChanged;
        private bool _isEnemyInside;
        private Trigger<Enemy> _enemiesTrigger;

        public bool IsSlowdown => _isEnemyInside;

        public void Init(Action<bool> stateChanged)
        {
            _stateChanged = stateChanged;
        }

        private void Awake()
        {
            _enemiesTrigger = new Trigger<Enemy>(Constants.Layers.Character, null, null);
        }

        private void FixedUpdate()
        {
            bool isEnemyInside = _enemiesTrigger.CheckIfAnyInside(transform.position, _radius);

            if(_isEnemyInside != isEnemyInside)
            {
                _isEnemyInside = isEnemyInside;

                _stateChanged?.Invoke(_isEnemyInside);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}