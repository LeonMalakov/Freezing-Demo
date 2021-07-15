using System;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] [Range(1, 10)] private float _normalSpeed = 1;
        [SerializeField] [Range(1, 10)] private float _loadedSpeed = 1;
        [SerializeField] [Range(1, 10)] private int _turnSpeed = 5;
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Vector2 _input;
        private float _speed;
        private Action<float> _velocityChanged;

        public Transform Helper { get; private set; }
        private Ray GroundRay => new Ray(_transform.position + _transform.up * 0.1f, -_transform.up);

        public void Init(Action<float> velocityChanged)
        {
            _velocityChanged = velocityChanged;

            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
            Helper = new GameObject("Helper").transform;

            _speed = _normalSpeed;
        }

        private void FixedUpdate()
        {
            ResetRigidbodyVelocities();

            if (Physics.Raycast(GroundRay, out var hit, _groundLayer))
            {
                Vector3 helperForward = Vector3.Cross(Helper.right, hit.normal);
                UpdateHelper(hit, helperForward);

                Vector3 moveDirection = CalculateMoveDirection(hit, helperForward);

                RotateByGroundNormal(hit);
                RotateToMoveDirection(hit, moveDirection);

                Move(hit, moveDirection);
            }
        }

        public void ChangeSpeed(bool isLoaded)
        {
            _speed = isLoaded ? _loadedSpeed : _normalSpeed;
        }

        private void ResetRigidbodyVelocities()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void SetMove(Vector2 input)
        {
            _input = input;
        }

        private void UpdateHelper(RaycastHit hit, Vector3 helperForward)
        {
            Helper.rotation = Quaternion.LookRotation(helperForward, hit.normal);
            Helper.position = _transform.position;
        }

        private void Move(RaycastHit hit, Vector3 moveDirection)
        {
            Vector3 move = moveDirection * _speed;
            Vector3 nextPosition = hit.point + move * Time.fixedDeltaTime;
            _rigidbody.MovePosition(nextPosition);

            _velocityChanged?.Invoke(move.magnitude);
        }

        private Vector3 CalculateMoveDirection(RaycastHit hit, Vector3 helperForward)
        {
            Vector3 moveDirection = Vector3.ClampMagnitude(new Vector3(_input.x, 0, _input.y), 1);
            var rot1 = Quaternion.LookRotation(helperForward, hit.normal);
            moveDirection = rot1 * moveDirection;
            return moveDirection;
        }

        private void RotateByGroundNormal(RaycastHit hit)
        {
            Vector3 forward = Vector3.Cross(_transform.right, hit.normal);
            _transform.rotation = Quaternion.LookRotation(forward, hit.normal);
        }

        private void RotateToMoveDirection(RaycastHit hit, Vector3 moveDirection)
        {
            if (moveDirection != Vector3.zero)
            {
                var rot = Quaternion.LookRotation(moveDirection, hit.normal);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, rot, Time.fixedDeltaTime * _turnSpeed);
            }
        }
    }
}