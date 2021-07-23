using UnityEngine;
using UnityEngine.InputSystem;

namespace WGame
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        private Player _character;
        private Controls _input;
        private bool _isAttacking;
        private Vector2 _attackDirection;

        private void Awake()
        {
            _character = GetComponent<Player>();
            _input = new Controls();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Move.performed += Move;
            _input.Player.Move.canceled += MoveCanceled;
            _input.Player.Attack.performed += Attack;
            _input.Player.Attack.canceled += AttackCanceled;
            _input.Player.Interact.performed += Interact;
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Move.performed -= Move;
            _input.Player.Move.canceled -= MoveCanceled;
            _input.Player.Attack.performed -= Attack;
            _input.Player.Attack.canceled -= AttackCanceled;
            _input.Player.Interact.performed -= Interact;
        }

        private void Update()
        {
            if (_isAttacking)
            {
                _character.Attack(_attackDirection);
            }
        }

        private void Move(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            _character.SetMove(input);
        }

        private void MoveCanceled(InputAction.CallbackContext context)
        {
            _character.SetMove(Vector2.zero);
        }

        private void Attack(InputAction.CallbackContext context)
        {
            _attackDirection = context.ReadValue<Vector2>();
            _isAttacking = true;
        }

        private void AttackCanceled(InputAction.CallbackContext context)
        {
            _isAttacking = false;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            _character.Interact();
        }
    }
}