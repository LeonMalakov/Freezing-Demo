using UnityEngine;

namespace WGame
{
    public class PlayerFollower : MonoBehaviour, IPlayerTargeting
    {
        private Player _target;
        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;

        public void SetTarget(Player target)
        {
            _target = target;
        }

        void Awake()
        {
            _positionOffset = transform.position;
            _rotationOffset = transform.rotation;
        }

        private void Update()
        {
            if (_target == null) return;

            transform.rotation = _target.Point.rotation * _rotationOffset;
            transform.position = _target.Point.position + _target.Point.TransformDirection(_positionOffset);
        }
    }
}