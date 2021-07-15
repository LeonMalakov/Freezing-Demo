using UnityEngine;

namespace WGame
{
    [RequireComponent(
        typeof(CharacterMovement), 
        typeof(CharacterGrabbing),
        typeof(CharacterInteraction))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private CharacterView _view;

        private CharacterMovement _movement;
        private CharacterGrabbing _grabbing;
        private CharacterInteraction _interaction;

        public Transform Point => _movement.Helper;
        public Item Grabbed => _grabbing.Grabbed;
        public bool IsGrabbing => _grabbing.IsGrabbing;

        private void Awake()
        {
            _movement = GetComponent<CharacterMovement>();
            _grabbing = GetComponent<CharacterGrabbing>();
            _interaction = GetComponent<CharacterInteraction>();

            _view.Init();
            _movement.Init(_view.SetVelocity);
            _grabbing.Init(OnIsLoadedChanged);
            _interaction.Init(this);
        }

        public void SetMove(Vector2 input) => _movement.SetMove(input);

        public void Interact() => _interaction.Interact();

        public void Drop() => _grabbing.Drop();

        public bool Grab(Item item) => _grabbing.Grab(item);

        private void OnIsLoadedChanged(bool isLoaded)
        {
            _movement.ChangeSpeed(isLoaded);
            _view.SetIsLoaded(isLoaded);
        }
    }
}