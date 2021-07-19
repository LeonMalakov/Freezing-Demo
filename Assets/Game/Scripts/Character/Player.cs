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
        private bool _isInWarmArea;

        public int Health { get; private set; }
        public int Warm { get; private set; }

        public Transform Point => _movement.Helper;
        public Item Grabbed => _grabbing.Grabbed;
        public bool IsGrabbing => _grabbing.IsGrabbing;
        public bool IsAlive => Health > 0;
        public IPlayerOwner OriginFactory { get; set; }

        protected override void Awake()
        {
            base.Awake();

            _movement = GetComponent<CharacterMovement>();
            _grabbing = GetComponent<CharacterGrabbing>();
            _interaction = GetComponent<CharacterInteraction>();
            _combat = GetComponent<CharacterCombat>();

            _view.Init(_combat.ApplyDamageToTargets, OnAttackEnded);
            _movement.Init(_view.SetVelocity);
            _movement.SetSpeed(_normalMoveSpeed);
            _grabbing.Init(OnIsLoadedChanged);
            _interaction.Init(this);
            _combat.Init(OnAttacking);

            Health = _maxHealth;
            Warm = _maxWarm;

            StartCoroutine(StatsUpdateLoop());
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void Interact() => _interaction.Interact();

        public void Drop() => _grabbing.Drop();

        public bool Grab(Item item) => _grabbing.Grab(item);

        public void Attack() => _combat.Attack();

        public void TakeDamage(int damage)
        {
            Health -= damage;
            CheckDie();
        }

        public void EnterWarmArea() => _isInWarmArea = true;

        public void ExitWarmArea() => _isInWarmArea = false;

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
            _grabbing.Drop();
            _movement.SetIsEnabledState(false);
            _combat.SetIsEnabledState(false);
            _interaction.SetIsEnabledState(false);
            _view.SetIsDead();
        }

        private void OnIsLoadedChanged(bool isLoaded)
        {
            _movement.SetSpeed(isLoaded ? _loadedMoveSpeed : _normalMoveSpeed);
            _view.SetIsLoaded(isLoaded);
            _combat.SetIsEnabledState(!isLoaded);
            _weaponModel.SetActive(!isLoaded);
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

        private IEnumerator StatsUpdateLoop()
        {
            var waitForSeconds = new WaitForSeconds(StatsUpdateTime);

            while (IsAlive)
            {
                yield return waitForSeconds;

                UpdateWarmStat();

                UpdateHealthStat();
            }
        }

        private void UpdateWarmStat()
        {
            if (_isInWarmArea)
            {
                if (Warm < _maxWarm)
                    Warm = Mathf.Min(Warm + _warmToAdd, _maxWarm);
            }
            else
            {
                if (Warm > 0)
                    Warm = Mathf.Max(Warm - _warmToRemove, 0);
            }
        }

        private void UpdateHealthStat()
        {
            if (Warm > 0)
            {
                if (Health < _maxHealth)
                    Health = Mathf.Min(Health + _healthToAdd, _maxHealth);
            }
            else
            {
                Health = Mathf.Max(Health - _healthToRemove, 0);
                CheckDie();
            }
        }
    }
}