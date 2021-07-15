using System;
using UnityEngine;

namespace WGame
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterGrabbing))]
    [RequireComponent(typeof(CharacterInteraction))]
    [RequireComponent(typeof(CharacterCombat))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private CharacterView _view;

        private CharacterMovement _movement;
        private CharacterGrabbing _grabbing;
        private CharacterInteraction _interaction;
        private CharacterCombat _combat;

        public Transform Point => _movement.Helper;
        public Item Grabbed => _grabbing.Grabbed;
        public bool IsGrabbing => _grabbing.IsGrabbing;

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
            _grabbing = GetComponent<CharacterGrabbing>();
            _interaction = GetComponent<CharacterInteraction>();
            _combat = GetComponent<CharacterCombat>();

            _view.Init(_combat.ApplyDamageToTargets, OnAttackEnded);
            _movement.Init(_view.SetVelocity);
            _grabbing.Init(OnIsLoadedChanged);
            _interaction.Init(this);
            _combat.Init(OnAttacking);
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void Interact() => _interaction.Interact();

        public void Drop() => _grabbing.Drop();

        public bool Grab(Item item) => _grabbing.Grab(item);

        public void Attack() => _combat.Attack();

        private void OnIsLoadedChanged(bool isLoaded)
        {
            _movement.ChangeSpeed(isLoaded);
            _view.SetIsLoaded(isLoaded);
            _combat.SetIsEnabledState(!isLoaded);
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