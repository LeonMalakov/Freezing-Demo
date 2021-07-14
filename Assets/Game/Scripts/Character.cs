using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(CharacterMovement))]
    public class Character : MonoBehaviour
    {
        private CharacterMovement _movement;

        public Transform Point => _movement.Helper;

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);
    }
}