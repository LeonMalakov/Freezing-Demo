using UnityEngine;
using WGame.UI;

namespace WGame
{
    public class CampfireLifeTimeView : MonoBehaviour
    {
        [SerializeField] private Campfire _campfire;
        [SerializeField] private ProgressBar _bar;

        private void OnEnable() => TrySubscribe();

        private void OnDisable() => TryUnsubscribe();

        private void LateUpdate() => LookAtCamera();

        private void TrySubscribe()
        {
            if (_campfire != null)
                Subscribe();
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

        private void LookAtCamera() 
            => transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, _campfire.Transform.up);
    }
}