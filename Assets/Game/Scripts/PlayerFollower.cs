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
            _positionOffset = transform.position - _target.Point.position;
            _rotationOffset = Quaternion.Inverse(_target.Point.rotation) * transform.rotation;
        }

        private void Update()
        {
            if (_target == null) return;

            transform.rotation = _target.Point.rotation * _rotationOffset;
            transform.position = _target.Point.position + _target.Point.TransformDirection(_positionOffset);
        }
    }
}