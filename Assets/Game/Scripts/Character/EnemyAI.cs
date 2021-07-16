using System.Linq;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAI : MonoBehaviour
    {
        private const int AttackAngle = 25;
        [SerializeField] [Range(0, 20)] private float _targetsDetectionRange = 10;
        [SerializeField] [Range(0, 5)] private float _attackRange = 1.5f;
        [SerializeField] private LayerMask _targetsMask;

        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void FixedUpdate()
        {
            if (_enemy.IsAlive == false) return;

            var target = GetTarget();

            if (target != null)
            {
                if (IsTargetInAttackRange(target))
                {
                    if (Vector3.Angle(transform.forward, target.transform.position - transform.position) < AttackAngle)
                        _enemy.Attack();
                    else
                        MoveToTarget(target);
                }
                else
                {
                    MoveToTarget(target);
                }
            }
        }

        private bool IsTargetInAttackRange(Player target) 
            => Vector3.Distance(target.transform.position, transform.position) < _attackRange;

        private void MoveToTarget(Player target)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            directionToTarget = _enemy.Point.InverseTransformDirection(directionToTarget);
            Vector2 moveInput = new Vector2(directionToTarget.x, directionToTarget.z);
            _enemy.SetMove(moveInput);
        }

        private Player GetTarget()
        {
            var hits = Physics.OverlapSphere(transform.position, _targetsDetectionRange, _targetsMask);
            var target = hits.Select(x => x.GetComponent<Player>()).Where(x => x != null && x.IsAlive).FirstOrDefault();
            return target;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _targetsDetectionRange);
        }
    }
}