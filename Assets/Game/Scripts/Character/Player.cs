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
        [SerializeField] private CharacterView _view;
        [SerializeField] private GameObject _weaponModel;

        [SerializeField] [Range(1, 10)] private float _normalMoveSpeed = 1;
        [SerializeField] [Range(1, 10)] private float _loadedMoveSpeed = 1;
        [SerializeField] [Range(0, 500)] private int _health = 100;

        private CharacterMovement _movement;
        private CharacterGrabbing _grabbing;
        private CharacterInteraction _interaction;
        private CharacterCombat _combat;

        public Transform Point => _movement.Helper;
        public Item Grabbed => _grabbing.Grabbed;
        public bool IsGrabbing => _grabbing.IsGrabbing;
        public bool IsAlive => _health > 0;

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
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void Interact() => _interaction.Interact();

        public void Drop() => _grabbing.Drop();

        public bool Grab(Item item) => _grabbing.Grab(item);

        public void Attack() => _combat.Attack();

        public void TakeDamage(int damage)
        {
            _health -= damage;
            CheckDie();
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

    }
}