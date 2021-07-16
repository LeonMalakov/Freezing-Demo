using System;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] [Range(1, 10)] private int _turnSpeed = 5;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Vector2 _input;
        private float _speed;
        private bool _isEnabled;
        private Action<float> _velocityChanged;

        public Transform Helper { get; private set; }

        public void Init(Action<float> velocityChanged)
        {
            _velocityChanged = velocityChanged;

            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
            Helper = new GameObject("Helper").transform;

            SetIsEnabledState(true);
        }

        public void SetIsEnabledState(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void SetMove(Vector2 input)
        {
            _input = input;
        }

        private void FixedUpdate()
        {
            ResetRigidbodyVelocities();

            if (_isEnabled)
            {
                PerformMove();
            }
        }

        private void PerformMove()
        {
            if (Physics.Raycast(EarthPlacer.GroundRay(transform), out var hit, EarthPlacer.MaxRaycastDistance, EarthPlacer.GroundMask))
            {
                Vector3 helperForward = Vector3.Cross(Helper.right, hit.normal);
                UpdateHelper(hit, helperForward);

                Vector3 moveDirection = CalculateMoveDirection(hit, helperForward);

                RotateByGroundNormal(hit);
                RotateToMoveDirection(hit, moveDirection);

                Move(hit, moveDirection);
            }
        }

        private void ResetRigidbodyVelocities()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
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