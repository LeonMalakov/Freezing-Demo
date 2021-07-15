using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            HandleAttack();
            HandleMovement();
            HandleInteraction();
        }

        private void HandleAttack()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
                _character.Attack();
        }

        private void HandleInteraction()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _character.Interact();
        }

        private void HandleMovement()
        {
            Vector2 input = new Vector2(
                x: Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0,
                y: Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0);

            _character.SetMove(input);
        }
    }
}