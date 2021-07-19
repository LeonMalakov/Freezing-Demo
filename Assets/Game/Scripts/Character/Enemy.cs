using UnityEngine;

namespace WGame
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterCombat))]
    public class Enemy : GameBehaviour, IAttackable
    {
        [SerializeField] private CharacterView _view;
        [SerializeField] [Range(1, 10)] private float _normalMoveSpeed = 3;
        [SerializeField] [Range(1, 10)] private float _runMoveSpeed = 5.5f;
        [SerializeField] [Range(0, 500)] private int _health = 80;

        private CharacterMovement _movement;
        private CharacterCombat _combat;

        public Transform Point => _movement.Helper;
        public bool IsAlive => _health > 0;
        public IEnemiesOwner OriginFactory { get; set; }

        protected override void Awake()
        {
            base.Awake();

            _movement = GetComponent<CharacterMovement>();
            _combat = GetComponent<CharacterCombat>();

            _view.Init(_combat.ApplyDamageToTargets, OnAttackEnded);
            _movement.Init(_view.SetVelocity);
            _movement.SetSpeed(_normalMoveSpeed);
            _combat.Init(OnAttacking);
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void SetNormalSpeed() => _movement.SetSpeed(_normalMoveSpeed);

        public void SetRunSpeed() => _movement.SetSpeed(_runMoveSpeed);

        public void Attack() => _combat.Attack();

        public void TakeDamage(int damage)
        {
            _health -= damage;
            CheckDie();
        }

        public void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        private void CheckDie()
        {
            if (IsAlive == false)
                Die();
        }

        private void Die()
        {
            _movement.SetIsEnabledState(false);
            _combat.SetIsEnabledState(false);
            _view.SetIsDead();
        }

        private void OnAttacking()
        {
            _movement.SetIsEnabledState(false);
            _view.SetAttack();
        }

        private void OnAttackEnded()
        {
            _combat.AttackEnded();
            _movement.SetIsEnabledState(true);
        }
    }
}