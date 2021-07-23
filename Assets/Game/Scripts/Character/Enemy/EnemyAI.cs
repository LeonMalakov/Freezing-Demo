using System.Linq;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAI : MonoBehaviour
    {
        private const int UpdateTargetFrames = 4;
        [SerializeField] [Range(0, 20)] private float _targetsDetectionRange = 10;
        [SerializeField] [Range(0, 5)] private float _attackRange = 1.5f;
        [SerializeField] [Range(0, 20)] private float _randomMovingTime = 4;

        private Enemy _enemy;
        private byte _fixedUpdates;
        private Player _target;
        private float _nextDirectionChangeTime;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void FixedUpdate()
        {
            if (((IAttackable)_enemy).IsAlive == false) return;

            _fixedUpdates++;

            if (_fixedUpdates % UpdateTargetFrames == 0)
                UpdateTarget();

            if (_target != null)
            {
                FollowTarget(_target);
            }
            else
            {
                WalkAround();
            }
        }

        private void UpdateTarget()
        {
            var newTarget = GetTarget();
            if (newTarget != _target)
            {
                _target = newTarget;

                if (_target != null)
                    _enemy.SetRunSpeed();
                else
                    _enemy.SetNormalSpeed();
            }
        }

        private void WalkAround()
        {
            if (Time.timeSinceLevelLoad >= _nextDirectionChangeTime)
            {
                _nextDirectionChangeTime = Time.timeSinceLevelLoad + _randomMovingTime;
                var direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
                _enemy.SetMove(direction);
            }
        }

        private void FollowTarget(Player target)
        {
            if (IsTargetInAttackRange(target))
                _enemy.Attack((target.Transform.position - transform.position).normalized);
            else
                MoveToTarget(target);
        }

        private bool IsTargetInAttackRange(Player target)
            => Vector3.Distance(target.transform.position, transform.position) < _attackRange;

        private void MoveToTarget(Player target)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            directionToTarget = _enemy.Point.InverseTransformDirection(directionToTarget);
            Vector2 moveInput = new Vector2(directionToTarget.x, directionToTarget.z);
            _enemy.SetMove(moveInput);
        }

        private Player GetTarget()
        {
            var player = CollisionUtilities.GetPlayerViaOverlapSphere(transform.position, _targetsDetectionRange);
            if (player != null && player.IsAlive == false)
                player = null;
            return player;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _targetsDetectionRange);
        }
    }
}