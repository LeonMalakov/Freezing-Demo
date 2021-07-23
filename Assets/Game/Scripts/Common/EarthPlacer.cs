using UnityEngine;

namespace WGame
{
    public class EarthPlacer : MonoBehaviour
    {
        private const int GroundLayerNumber = 8;
        public const int MaxRaycastDistance = 100;

        public static LayerMask GroundMask => 1 << GroundLayerNumber;
        public static Ray GroundRay(Transform t) => new Ray(t.position + t.up * 0.1f, -t.up);

        [ContextMenu("Place")]
        public void Place()
        {
            if (Physics.Raycast(GroundRay(transform), out var hit, MaxRaycastDistance, GroundMask))
            {
                RotateByGroundNormal(hit);
                PlaceAtGround(hit);
            }
        }

        private void PlaceAtGround(RaycastHit hit)
        {
            transform.position = hit.point;
        }

        private void RotateByGroundNormal(RaycastHit hit)
        {
            Vector3 forward = Vector3.Cross(transform.right, hit.normal);
            transform.rotation = Quaternion.LookRotation(forward, hit.normal);
        }
    }
}