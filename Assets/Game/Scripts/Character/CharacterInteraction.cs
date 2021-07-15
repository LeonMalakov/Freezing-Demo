using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class CharacterInteraction : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float _range = 5;

        private Character _character;
        private Interactivable _active;

        public void Init(Character character)
        {
            _character = character;
        }

        private void FixedUpdate()
        {
            UpdateActive();
        }

        public void Interact()
        {
            if (_active != null)
            {
                if (_character.IsGrabbing)
                    InteractWithItem();
                else
                    _active.Interact(_character);
            }
            else if (_character.IsGrabbing)
            {
                _character.Drop();
            }
        }

        private void InteractWithItem()
        {
            var item = _character.Grabbed;
            _character.Drop();
            bool success = _active.InteractWithItem(_character, item);

            if (success == false)
                _character.Grab(item);
        }

        private void UpdateActive()
        {
            var newActive = GetClosestInteractivable();

            if (_active != newActive)
            {
                if (_active != null)
                    _active.BecomeInactive();

                _active = newActive;

                if (_active != null)
                    _active.BecomeActive();
            }
        }

        private Interactivable GetClosestInteractivable()
        {
            IEnumerable<Interactivable> hits = GetInteractivables();
            Interactivable closest = FindClosest(hits);

            return closest;
        }

        private Interactivable FindClosest(IEnumerable<Interactivable> hits)
        {
            float minDistance = float.MaxValue;
            Interactivable closest = null;
            foreach (var x in hits)
            {
                float dist = Vector3.Distance(x.transform.position, transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = x;
                }
            }

            return closest;
        }

        private IEnumerable<Interactivable> GetInteractivables()
        {
            var hits = Physics.OverlapSphere(transform.position, _range)
                           .Select(x => x.GetComponent<Interactivable>())
                           .Where(x => x != null);

            if (_character.IsGrabbing)
                hits = hits.Except(new Interactivable[] { _character.Grabbed });

            return hits;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}
