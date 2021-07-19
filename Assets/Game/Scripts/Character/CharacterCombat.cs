using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Vector3 _attackShape;
        [SerializeField] private LayerMask _layers;
        [SerializeField] [Range(0, 500)] private int _damage = 20;
        [SerializeField] [Range(0, 10)] private float _findingClosestTargetRadius = 2.5f;

        private bool _isEnabled;
        private bool _isAttacking;
        private Action<IAttackable> _attacking;

        public void Init(Action<IAttackable> attacking)
        {
            _attacking = attacking;
            SetIsEnabledState(true);
        }

        public void SetIsEnabledState(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void Attack()
        {
            if (_isEnabled == false || _isAttacking) return;

            _isAttacking = true;
            var closestTarget = GetClosestTarget();
            _attacking?.Invoke(closestTarget);
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
            var hits = Physics.OverlapBox(_attackPoint.position, _attackShape * 0.5f, _attackPoint.rotation, _layers);
            var targets = TakeAttackables(hits);
            return targets;
        }

        private IAttackable GetClosestTarget()
        {
            var hits = Physics.OverlapSphere(transform.position, _findingClosestTargetRadius, _layers);
            var targets = TakeAttackables(hits);
            return targets.FindClosest(transform.position);
        }

        private IEnumerable<IAttackable> TakeAttackables(Collider[] hits)
        {
            return hits
                .Where(x => x.gameObject != gameObject)
                .Select(x => x.GetComponent<IAttackable>())
                .Where(x => x != null);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _findingClosestTargetRadius);

            if (_attackPoint == null)
                return;

            var lastMatrix = Gizmos.matrix;
            Gizmos.color = Color.red;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(_attackPoint.position, _attackPoint.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _attackShape);
            Gizmos.matrix = lastMatrix;
        }
    }
}
