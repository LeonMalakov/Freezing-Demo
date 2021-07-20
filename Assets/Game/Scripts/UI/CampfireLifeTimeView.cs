using UnityEngine;
using WGame.UI;

namespace WGame
{
    public class CampfireLifeTimeView : MonoBehaviour, ICampfireTargeting
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private ProgressBar _bar;
        private Campfire _campfire;

        public void SetTarget(Campfire campfire)
        {
            TryUnsubscribe();
            _campfire = campfire;

            if (isActiveAndEnabled)
                TrySubscribeAndPlace();
        }

        private void OnEnable() => TrySubscribeAndPlace();

        private void OnDisable() => TryUnsubscribe();

        private void LateUpdate() => RotateToCamera();

        private void TrySubscribeAndPlace()
        {
            if (_campfire != null)
            {
                Place();
                Subscribe();
                OnValueChanged(GetCurrentValue());
            }
        }

        private void TryUnsubscribe()
        {
            if (_campfire != null)
                Unsubscribe();
        }


        private void OnValueChanged(int value)
        {
            _bar.SetProgress(value / (float)GetMaxValue());
        }

        private void Subscribe() => _campfire.LifetimeChanged += OnValueChanged;

        private void Unsubscribe() => _campfire.LifetimeChanged -= OnValueChanged;

        private int GetMaxValue() => _campfire.MaxLifeTime;

        private int GetCurrentValue() => _campfire.LifeTime;

        private void Place()
        {
            transform.position = _campfire.Transform.position;     
        }

        private void RotateToCamera()
        {
            transform.LookAt(_camera.position);
        }
    }
}