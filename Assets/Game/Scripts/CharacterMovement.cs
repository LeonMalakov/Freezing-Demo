using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] [Range(1, 10)] private float _speed = 1;

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Vector2 _input;
        private Transform _helper;

        public Transform Helper => _helper;
        private Ray GroundRay => new Ray(_transform.position + _transform.up * 0.1f, -_transform.up);

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;

            _helper = new GameObject("Helper").transform;        
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(GroundRay, out var hit))
            {
                Vector3 helperForward = Vector3.Cross(_helper.right, hit.normal);
                UpdateHelper(hit, helperForward);

                Vector3 moveDirection = CalculateMoveDirection(hit, helperForward);

                RotateByGroundNormal(hit);
                RotateToMoveDirection(hit, moveDirection);

                Move(hit, moveDirection);
            }
        }

        public void SetMove(Vector2 input)
        {
            _input = input;
        }

        private void UpdateHelper(RaycastHit hit, Vector3 helperForward)
        {
            _helper.rotation = Quaternion.LookRotation(helperForward, hit.normal);
            _helper.position = _transform.position;
        }

        private void Move(RaycastHit hit, Vector3 moveDirection)
        {
            Vector3 move = moveDirection * _speed;
            Vector3 nextPosition = hit.point + move * Time.fixedDeltaTime;
            _rigidbody.MovePosition(nextPosition);
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
                _transform.rotation = Quaternion.Lerp(_transform.rotation, rot, Time.fixedDeltaTime * 3);
            }
        }
    }
}