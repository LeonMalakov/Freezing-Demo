using System;
using UnityEngine;

namespace WGame
{
    public class CharacterGrabbing : MonoBehaviour
    {
        [SerializeField] private Transform _anchor;

        private Transform _grabbedLastParent;
        private Action<bool> _isGrabbedChanged;

        public Item Grabbed { get; private set; }
        public bool IsGrabbing => Grabbed != null;

        public void Init(Action<bool> isGrabbedChanged)
        {
            _isGrabbedChanged = isGrabbedChanged;
        }

        public bool Grab(Item item)
        {
            if (Grabbed != null) return false;

            Grabbed = item;
            _grabbedLastParent = Grabbed.transform.parent;

            Grabbed.DisableCollision();
            Grabbed.transform.parent = _anchor;
            Grabbed.transform.localPosition = Vector3.zero;
            Grabbed.transform.localRotation = Quaternion.identity;

            _isGrabbedChanged?.Invoke(true);

            return true;
        }

        public void Drop()
        {
            if (Grabbed != null)
            {
                Grabbed.EnableCollision();
                Grabbed.transform.parent = _grabbedLastParent;
                Grabbed.transform.rotation = transform.rotation;
                Grabbed.EarthPlacer.Place();
                Grabbed = null;

                _isGrabbedChanged?.Invoke(false);
            }
        }
    }
}
