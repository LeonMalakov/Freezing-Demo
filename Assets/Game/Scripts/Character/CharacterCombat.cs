using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class CharacterCombat : MonoBehaviour
    {
        private const float AttackSectorAngle = 100f;

        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Vector3 _attackShape;
        [SerializeField] private LayerMask _layers;
        [SerializeField] [Range(0, 500)] private int _damage = 20;
        [SerializeField] [Range(0, 10)] private float _findingClosestTargetRadius = 2.5f;

        private bool _isEnabled;
        private bool _isAttacking;
        Func<IAttackable, bool> _targetsFilter;
        private Action<IAttackable, Vector3> _attacking;

        public void Init(Action<IAttackable, Vector3> attacking, Func<IAttackable, bool> targetsFilter)
        {
            _targetsFilter = targetsFilter;
            _attacking = attacking;
            SetIsEnabledState(true);
        }

        public void SetIsEnabledState(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void Attack(Vector3 attackDirection)
        {
            if (_isEnabled == false || _isAttacking) return;

            _isAttacking = true;
            var closestTarget = GetClosestTargetAtDirection(attackDirection);
            _attacking?.Invoke(closestTarget, attackDirection);
        }

        public void ApplyDamageToTargets()
        {
            var targets = GetTargets();

            foreach (var target in targets)
            {
                target.TakeDamage(_damage, transform.position);
            }
        }

        public void AttackEnded()
        {
            _isAttacking = false;
        }

        private IEnumerable<IAttackable> GetTargets()
        {
            var hits = Physics.OverlapBox(_attackPoint.position, _attackShape * 0.5f, _attackPoint.rotation, _layers);
            return FilterTargets(hits.Take<IAttackable>());
        }

        private IAttackable GetClosestTargetAtDirection(Vector3 attackDirection)
        {
            var hits = CollisionUtilities.GetComponentsViaOverlapSphere<IAttackable>(transform.position, _findingClosestTargetRadius, _layers);

            return FilterTargets(hits)
                .TakeAlive()
                .TakeAtSector(transform.position, attackDirection, AttackSectorAngle)
                .TakePrioritiesIfExist()
                .FindClosest(transform.position);
        }

        private IEnumerable<IAttackable> FilterTargets(IEnumerable<IAttackable> targets)
        {
            return targets
                .Where(x => x.Transform != transform)
                .Where(_targetsFilter);
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
