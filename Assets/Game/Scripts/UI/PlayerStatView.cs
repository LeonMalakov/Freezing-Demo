using UnityEngine;
using WGame.UI;

namespace WGame
{
    [RequireComponent(typeof(ProgressBar))]
    public abstract class PlayerStatView : MonoBehaviour, IPlayerTargeting
    {
        private Player _player;
        private ProgressBar _bar;

        public void SetTarget(Player player)
        {
            TryUnsubscribe();
            _player = player;

            if (isActiveAndEnabled)
                TrySubscribe();
        }

        private void Awake()
        {
            _bar = GetComponent<ProgressBar>();
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
                Subscribe(_player);
                OnValueChanged(GetCurrentValue(_player));
            }
        }

        private void TryUnsubscribe()
        {
            if (_player != null)
                Unsubscribe(_player);
        }


        protected void OnValueChanged(int value)
        {
            _bar.SetProgress(value / (float)GetMaxValue(_player));
        }

        protected abstract void Subscribe(Player player);

        protected abstract void Unsubscribe(Player player);

        protected abstract int GetMaxValue(Player player);

        protected abstract int GetCurrentValue(Player player);
    }
}