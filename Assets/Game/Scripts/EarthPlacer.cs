using UnityEngine;

namespace WGame
{
    public class EarthPlacer : MonoBehaviour
    {
        private Ray GroundRay => new Ray(transform.position + transform.up * 0.1f, -transform.up);

        [ContextMenu("Place")]
        public void Place()
        {
            if (Physics.Raycast(GroundRay, out var hit))
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