using UnityEngine;
using WGame.UI;

namespace WGame
{
    [RequireComponent(typeof(Checkbox))]
    public class PlayerIsInWarmAreaView : MonoBehaviour, IPlayerTargeting
    {
        private Player _player;
        private Checkbox _checkbox;

        public void SetTarget(Player player)
        {
            TryUnsubscribe();
            _player = player;

            if (isActiveAndEnabled)
                TrySubscribe();
        }

        private void Awake()
        {
            _checkbox = GetComponent<Checkbox>();
        }

        private void OnEnable()
        {
            TrySubscribe();
        }

        private void OnDisable()
        {
            TryUnsubscribe();
        }

        private void TrySubscribe()
        {
            if (_player != null)
            {
                Subscribe();
                OnValueChanged(GetCurrentValue());
            }
        }

        private void TryUnsubscribe()
        {
            if (_player != null)
                Unsubscribe();
        }

        private bool GetCurrentValue() => _player.IsInWarmArea;

        private void Subscribe()
        {
            _player.IsInWarmAreaStateChanged += OnValueChanged;
        }

        private void Unsubscribe()
        {
            _player.IsInWarmAreaStateChanged -= OnValueChanged;
        }

        private void OnValueChanged(bool value)
        {
            _checkbox.SetValue(value);
        }
    }
}