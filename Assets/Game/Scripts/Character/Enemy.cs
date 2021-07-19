using System;
using System.Collections;
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
        [SerializeField] [Range(0, 30)] private float _disappearTime = 10;

        private CharacterMovement _movement;
        private CharacterCombat _combat;

        public Transform Point => _movement.Helper;
        Transform IGameObject.Transform => transform;
        bool IAttackable.IsAlive => _health > 0;
        bool IAttackable.IsPriority => true;


        internal void Init()
        {
            _movement = GetComponent<CharacterMovement>();
            _combat = GetComponent<CharacterCombat>();

            _view.Init(_combat.ApplyDamageToTargets, OnAttackEnded);
            _movement.Init(_view.SetVelocity);
            _movement.SetSpeed(_normalMoveSpeed);
            _combat.Init(OnAttacking, targetsFilter: x => x is Player);
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void SetNormalSpeed() => _movement.SetSpeed(_normalMoveSpeed);

        public void SetRunSpeed() => _movement.SetSpeed(_runMoveSpeed);

        public void Attack() => _combat.Attack();

        void IAttackable.TakeDamage(int damage)
        {
            _health -= damage;
            CheckDie();
        }

        public void Recycle()
        {
            Game.RemoveEnemy(this);
        }

        private void CheckDie()
        {
            if (((IAttackable)this).IsAlive == false)
                Die();
        }

        private void Die()
        {
            _movement.SetIsEnabledState(false);
            _combat.SetIsEnabledState(false);
            _view.SetIsDead();

            StartCoroutine(DisappearLoop());
        }

        private void OnAttacking(IAttackable target)
        {
            _movement.SetIsMovementEnabledState(false);
            _movement.SetLookAtTarget(target.Transform.position);
            _movement.SetLookAtMode(CharacterMovement.LookAtMode.Target);
            _view.SetAttack();
        }

        private void OnAttackEnded()
        {
            _combat.AttackEnded();
            _movement.SetLookAtMode(CharacterMovement.LookAtMode.MoveDirection);
            _movement.SetIsMovementEnabledState(true);
        }

        private IEnumerator DisappearLoop()
        {
            yield return new WaitForSeconds(_disappearTime);
            Recycle();
        }
    }
}