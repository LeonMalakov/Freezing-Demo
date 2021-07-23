using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WGame
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float _range = 5;

        private bool _isEnabled;
        private Player _character;
        private IInteractivable _active;
        private Action<IInteractivable> _activeChanged;

        public void Init(Player character, Action<IInteractivable> activeChanged)
        {
            _character = character;
            _activeChanged = activeChanged;
            SetIsEnabledState(true);
        }

        public void SetIsEnabledState(bool isEnabled)
        {
            _isEnabled = isEnabled;

            if (_isEnabled == false)
                SetActive(null);
        }

        private void FixedUpdate()
        {
            if (_isEnabled)
            {
                UpdateActive();
            }
        }

        public void Interact()
        {
            if (!_isEnabled) return;

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

            SetActive(newActive);
        }

        private void SetActive(IInteractivable newActive)
        {
            if (_active != newActive)
            {
                if (_active != null)
                    _active.BecomeInactive();

                _active = newActive;

                if (_active != null)
                    _active.BecomeActive();

                _activeChanged?.Invoke(_active);
            }
        }

        private IInteractivable GetClosestInteractivable()
        {
            IEnumerable<IInteractivable> hits = GetInteractivables();
            IInteractivable closest = hits.FindClosest(transform.position);

            return closest;
        }

        private IEnumerable<IInteractivable> GetInteractivables()
        {
            var hits = Physics.OverlapSphere(transform.position, _range)
                .Take<IInteractivable>();
            
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
