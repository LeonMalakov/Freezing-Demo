using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class CharacterInteraction : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float _range = 5;

        private Character _character;
        private IInteractivable _active;

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

        private IInteractivable GetClosestInteractivable()
        {
            IEnumerable<IInteractivable> hits = GetInteractivables();
            IInteractivable closest = FindClosest(hits);

            return closest;
        }

        private IInteractivable FindClosest(IEnumerable<IInteractivable> hits)
        {
            float minDistance = float.MaxValue;
            IInteractivable closest = null;
            foreach (var x in hits)
            {
                float dist = Vector3.Distance(x.Position, transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = x;
                }
            }

            return closest;
        }

        private IEnumerable<IInteractivable> GetInteractivables()
        {
            var hits = Physics.OverlapSphere(transform.position, _range)
                           .Select(x => x.GetComponent<IInteractivable>())
                           .Where(x => x != null);

            if (_character.IsGrabbing)
                hits = hits.Except(new IInteractivable[] { _character.Grabbed });

            return hits;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}
