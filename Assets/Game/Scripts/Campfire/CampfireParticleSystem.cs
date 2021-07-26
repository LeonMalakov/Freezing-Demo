using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(ParticleSystem))]
    public class CampfireParticleSystem : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] private float _minSpeed = 0.5f;
        [SerializeField] [Range(0, 10)] private float _maxSpeed = 3;
        [SerializeField] [Range(0, 10)] private float _minSize = 0.8f;
        [SerializeField] [Range(0, 10)] private float _maxSize = 3.5f;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0,0, 1, 1);
        [SerializeField] [Range(0, 1)] private float _power = 1;

        private ParticleSystem _system;

        private void OnValidate()
        {
            if (_system == null)
                Awake();

            UpdatePower(_power);
        }

        private void Awake()
        {
            _system = GetComponent<ParticleSystem>();
        }

        public void UpdatePower(float percents)
        {
            _power = percents;

            var module = _system.main;
            module.startSize = Mathf.Lerp(_minSize, _maxSize, _curve.Evaluate(_power));
            module.startSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, _curve.Evaluate(_power));
        }

        internal void Stop()
        {
            _system.Stop();
        }
    }
}
