using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private GameObject _weaponModel;
        [SerializeField] private Vector3 _attackShape;
        [SerializeField] private LayerMask _layers;
        [SerializeField] [Range(0, 500)] private int _damage = 20;

        private bool _isEnabled;
        private bool _isAttacking;
        private Action _attacking;

        public void Init(Action attacking)
        {
            _attacking = attacking;
            SetIsEnabledState(true);
        }

        public void SetIsEnabledState(bool isEnabled)
        {
            _isEnabled = isEnabled;
            _weaponModel.SetActive(_isEnabled);
        }

        public void Attack()
        {
            if (_isEnabled == false || _isAttacking) return;

            _isAttacking = true;
            _attacking?.Invoke();
        }

        public void ApplyDamageToTargets()
        {
            var targets = GetTargets();

            foreach (var target in targets)
            {
                target.TakeDamage(_damage);
            }
        }

        public void AttackEnded()
        {
            _isAttacking = false;
        }

        private IEnumerable<IAttackable> GetTargets()
        {
            var hits = Physics.OverlapBox(_attackPoint.position, _attackShape, _attackPoint.rotation, _layers);
            var targets = hits.Select(x => x.GetComponent<IAttackable>())
                .Where(x => x != null);
            return targets;
        }

        private void OnDrawGizmosSelected()
        {
            var lastMatrix = Gizmos.matrix;
            Gizmos.color = Color.red;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(_attackPoint.position, _attackPoint.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _attackShape * 0.5f);
            Gizmos.matrix = lastMatrix;
        }
    }
}
