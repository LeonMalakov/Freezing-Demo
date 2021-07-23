using System;
using UnityEngine;

namespace WGame
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterGrabbing))]
    [RequireComponent(typeof(PlayerInteraction))]
    [RequireComponent(typeof(CharacterCombat))]
    [RequireComponent(typeof(PlayerSlowdown))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(Collider))]
    public class Player : GameBehaviour, IAttackable
    {
        [Header("References")]
        [SerializeField] private CharacterView _view;
        [SerializeField] private GameObject _weaponModel;

        [Header("Parameters")]
        [SerializeField] [Range(1, 10)] private float _normalMoveSpeed = 6;
        [SerializeField] [Range(1, 10)] private float _loadedMoveSpeed = 4;
        [SerializeField] [Range(0, 1)] private float _slowdownAreaSpeedMultipler = 0.6f;

        private CharacterMovement _movement;
        private CharacterGrabbing _grabbing;
        private PlayerInteraction _interaction;
        private CharacterCombat _combat;
        private PlayerSlowdown _slowdown;
        private PlayerStats _stats;

        public int MaxHealth => _stats.MaxHealth;
        public int MaxWarm => _stats.MaxWarm;
        public int Health => _stats.Health;
        public int Warm => _stats.Warm;
        public bool IsInWarmArea => _stats.IsInWarmArea;
        public Transform Point => _movement.Helper;
        public Item Grabbed => _grabbing.Grabbed;
        public bool IsGrabbing => _grabbing.IsGrabbing;
        public Transform Transform => transform;
        public bool IsAlive => _stats.IsAlive;
        public bool IsPriority => true;

        public event Action<int> HealthChanged;
        public event Action<int> WarmChanged;
        public event Action<bool> IsInWarmAreaStateChanged;
        public event Action<IAttackable> Attacking;
        public event Action<IInteractivable> InteractionActiveChanged;
        public event Action<bool> IsLoadedChanged;
        public event Action Died;

        public void Init()
        {
            _movement = GetComponent<CharacterMovement>();
            _grabbing = GetComponent<CharacterGrabbing>();
            _interaction = GetComponent<PlayerInteraction>();
            _combat = GetComponent<CharacterCombat>();
            _slowdown = GetComponent<PlayerSlowdown>();
            _stats = GetComponent<PlayerStats>();

            _view.Init(_combat.ApplyDamageToTargets, OnAttackEnded);
            _movement.Init(_view.SetVelocity);
            _movement.SetSpeed(_normalMoveSpeed);
            _grabbing.Init(OnIsLoadedChanged);
            _interaction.Init(this, OnInteractionActiveChanged);
            _combat.Init(OnAttacking, targetsFilter: x => true);
            _slowdown.Init(OnSlowdownStateChanged);
            _stats.Init(OnHealthChanged, OnWarmChanged, OnIsInWarmAreaChanged);
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void Interact() => _interaction.Interact();

        public void Drop() => _grabbing.Drop();

        public bool Grab(Item item) => _grabbing.Grab(item);

        public void Attack(Vector2 attackDirection) => _combat.Attack(_movement.CalculateDirectionVector(attackDirection));

        void IAttackable.TakeDamage(int damage) => _stats.TakeDamage(damage);

        public void EnterWarmArea() => _stats.EnterWarmArea();

        public void ExitWarmArea() => _stats.ExitWarmArea();

        public void Recycle() => Game.RemovePlayer(this);

        private void Die()
        {
            _grabbing.Drop();
            _movement.SetIsEnabledState(false);
            _combat.SetIsEnabledState(false);
            _interaction.SetIsEnabledState(false);
            _view.SetDie();
            DisableCollision();
            Died?.Invoke();
        }

        private void OnHealthChanged(int value)
        {
            if (IsAlive == false)
                Die();

            HealthChanged?.Invoke(value);
        }

        private void OnWarmChanged(int value) => WarmChanged?.Invoke(value);

        private void OnIsInWarmAreaChanged(bool state) => IsInWarmAreaStateChanged?.Invoke(state);

        private void OnIsLoadedChanged(bool isLoaded)
        {
            _view.SetIsLoaded(isLoaded);
            _combat.SetIsEnabledState(!isLoaded);
            _weaponModel.SetActive(!isLoaded);
            ChangeSpeed();

            IsLoadedChanged?.Invoke(isLoaded);
        }

        private void OnAttacking(IAttackable target, Vector3 attackDirection)
        {
            _movement.SetIsMovementEnabledState(false);
            var lookTarget = target != null ? target.Transform.position : transform.position + attackDirection;
            _movement.SetLookAtTarget(lookTarget);
            _movement.SetLookAtMode(CharacterMovement.LookAtMode.Target);

            _view.SetAttack();
            Attacking?.Invoke(target);
        }

        private void OnAttackEnded()
        {
            _combat.AttackEnded();
            _movement.SetLookAtMode(CharacterMovement.LookAtMode.MoveDirection);
            _movement.SetIsMovementEnabledState(true);
        }

        private void OnInteractionActiveChanged(IInteractivable target) => InteractionActiveChanged?.Invoke(target);

        private void OnSlowdownStateChanged(bool isSlowdown) => ChangeSpeed();

        private void ChangeSpeed()
        {
            float speed = _grabbing.IsGrabbing ? _loadedMoveSpeed : _normalMoveSpeed;
            if (_slowdown.IsSlowdown)
                speed *= _slowdownAreaSpeedMultipler;
            _movement.SetSpeed(speed);
        }

        private void DisableCollision() => GetComponent<Collider>().enabled = false;
    }
}