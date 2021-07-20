using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterGrabbing))]
    [RequireComponent(typeof(CharacterInteraction))]
    [RequireComponent(typeof(CharacterCombat))]
    public class Player : GameBehaviour, IAttackable
    {
        private const int StatsUpdateTime = 1;

        [Header("References")]
        [SerializeField] private CharacterView _view;
        [SerializeField] private GameObject _weaponModel;

        [Header("Stats")]
        [SerializeField] [Range(1, 10)] private float _normalMoveSpeed = 1;
        [SerializeField] [Range(1, 10)] private float _loadedMoveSpeed = 1;
        [SerializeField] [Range(0, 100)] private int _maxHealth = 100;
        [SerializeField] [Range(0, 100)] private int _maxWarm = 100;

        [Header("Stats updates")]
        [SerializeField] [Range(0, 50)] private int _healthToAdd = 5;
        [SerializeField] [Range(0, 50)] private int _healthToRemove = 2;
        [SerializeField] [Range(0, 50)] private int _warmToAdd = 10;
        [SerializeField] [Range(0, 50)] private int _warmToRemove = 3;

        private CharacterMovement _movement;
        private CharacterGrabbing _grabbing;
        private CharacterInteraction _interaction;
        private CharacterCombat _combat;
        private Stat _health;
        private Stat _warm;
        private bool _isInWarmArea;

        public int MaxHealth => _maxHealth;
        public int MaxWarm => _maxWarm;
        public int Health => _health;
        public int Warm => _warm;
        public bool IsInWarmArea
        {
            get => _isInWarmArea;
            set
            {
                _isInWarmArea = value;
                IsInWarmAreaStateChanged?.Invoke(_isInWarmArea);
            }
        }

        public Transform Point => _movement.Helper;
        public Item Grabbed => _grabbing.Grabbed;
        public bool IsGrabbing => _grabbing.IsGrabbing;
        public Transform Transform => transform;
        public bool IsAlive => _health > 0;
        public bool IsPriority => true;


        public event Action<int> HealthChanged;
        public event Action<int> WarmChanged;
        public event Action<bool> IsInWarmAreaStateChanged;
        public event Action<IAttackable> Attacking;
        public event Action<IInteractivable> InteractionActiveChanged;
        public event Action<bool> IsLoadedChanged;

        public void Init()
        {
            _movement = GetComponent<CharacterMovement>();
            _grabbing = GetComponent<CharacterGrabbing>();
            _interaction = GetComponent<CharacterInteraction>();
            _combat = GetComponent<CharacterCombat>();

            _view.Init(_combat.ApplyDamageToTargets, OnAttackEnded);
            _movement.Init(_view.SetVelocity);
            _movement.SetSpeed(_normalMoveSpeed);
            _grabbing.Init(OnIsLoadedChanged);
            _interaction.Init(this, OnInteractionActiveChanged);
            _combat.Init(OnAttacking, targetsFilter: x => true);

            _health = new Stat(_maxHealth, OnHealthChanged);
            _warm = new Stat(_maxWarm, OnWarmChanged);

            StartCoroutine(StatsUpdateLoop());
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void Interact() => _interaction.Interact();

        public void Drop() => _grabbing.Drop();

        public bool Grab(Item item) => _grabbing.Grab(item);

        public void Attack(Vector2 attackDirection) => _combat.Attack(_movement.CalculateDirectionVector(attackDirection));

        void IAttackable.TakeDamage(int damage)
        {
            _health -= damage;
        }

        public void EnterWarmArea() => IsInWarmArea = true;

        public void ExitWarmArea() => IsInWarmArea = false;

        public void Recycle()
        {
            Game.RemovePlayer(this);
        }

        private void Die()
        {
            _grabbing.Drop();
            _movement.SetIsEnabledState(false);
            _combat.SetIsEnabledState(false);
            _interaction.SetIsEnabledState(false);
            _view.SetIsDead();
        }

        private void OnHealthChanged(int value)
        {
            if (((IAttackable)this).IsAlive == false)
                Die();

            HealthChanged?.Invoke(value);
        }

        private void OnWarmChanged(int value)
        {
            WarmChanged?.Invoke(value);
        }

        private void OnIsLoadedChanged(bool isLoaded)
        {
            _movement.SetSpeed(isLoaded ? _loadedMoveSpeed : _normalMoveSpeed);
            _view.SetIsLoaded(isLoaded);
            _combat.SetIsEnabledState(!isLoaded);
            _weaponModel.SetActive(!isLoaded);

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

        private void OnInteractionActiveChanged(IInteractivable target)
        {
            InteractionActiveChanged?.Invoke(target);
        }

        private IEnumerator StatsUpdateLoop()
        {
            var waitForSeconds = new WaitForSeconds(StatsUpdateTime);

            while (((IAttackable)this).IsAlive)
            {
                UpdateWarmStat();
                UpdateHealthStat();
                yield return waitForSeconds;
            }
        }

        private void UpdateWarmStat()
        {
            if (IsInWarmArea)
            {
                if (_warm < _maxWarm)
                    _warm += _warmToAdd;
            }
            else
            {
                if (_warm > 0)
                    _warm -= _warmToRemove;
            }
        }

        private void UpdateHealthStat()
        {
            if (_warm > 0)
            {
                if (_health < _maxHealth)
                    _health += _healthToAdd;
            }
            else
            {
                _health -= _healthToRemove;
            }
        }
    }
}