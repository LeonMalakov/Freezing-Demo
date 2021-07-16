using UnityEngine;

namespace WGame
{
    public class CharacterFollower : MonoBehaviour
    {
        [SerializeField] private Player _target;

        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;

        private void Start()
        {
            _positionOffset = transform.position - _target.Point.position;
            _rotationOffset = Quaternion.Inverse(_target.Point.rotation) * transform.rotation;
        }

        private void Update()
        {
            transform.rotation = _target.Point.rotation * _rotationOffset;
            transform.position = _target.Point.position + _target.Point.TransformDirection(_positionOffset);
        }
    }
}