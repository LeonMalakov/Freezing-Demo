using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(CharacterMovement))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private CharacterView _view;

        private CharacterMovement _movement;      

        public Transform Point => _movement.Helper;

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();

            _view.Init();
            _movement.Init(_view.SetVelocity);
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);
    }
}